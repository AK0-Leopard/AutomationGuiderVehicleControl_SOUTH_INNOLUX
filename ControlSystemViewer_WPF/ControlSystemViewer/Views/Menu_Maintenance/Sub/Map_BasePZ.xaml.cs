using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using ControlSystemViewer.PopupWindows;
using ControlSystemViewer.Views.Menu_Maintenance;
using Map.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// Map_Base.xaml 的互動邏輯
    /// </summary>
    public partial class Map_BasePZ : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public WindownApplication app = null;
        private Settings.MapBase mapBaseSettings = null;
        private ParkZoneManagement pzManagement = null;
        private MapViewModel vm = null;
        // 車輛展示區塊 與 StatusTree-Vehicle 合併
        private bool isChecked_ShowScaleRuler = false;
        private Definition.AngleOfViewType currAOV = Definition.AngleOfViewType.degree_0;
        private bool currFlipH = false;
        private bool currFlipV = false;
        #endregion 公用參數設定

        public Map_BasePZ()
        {
            InitializeComponent();

            vm = DataContext as MapViewModel;
        }

        public void Start(WindownApplication _app, ParkZoneManagement _pzManagement)
        {
            app = _app;
            mapBaseSettings = app?.ObjCacheManager?.ViewerSettings?.mapBase;

            pzManagement = _pzManagement;

            isChecked_ShowScaleRuler = mapBaseSettings?.ShowScaleRuler ?? false;
            if (ckb_ShowScaleRuler_B != null)
                ckb_ShowScaleRuler_B.IsChecked = isChecked_ShowScaleRuler;
            if (ckb_ShowScaleRuler_R != null)
                ckb_ShowScaleRuler_R.IsChecked = isChecked_ShowScaleRuler;

            currAOV = (Definition.AngleOfViewType)(mapBaseSettings?.AngleOfView ?? 0);
            if (aov_ControlPanel_B != null && aov_ControlPanel_B.IsVisible)
                aov_ControlPanel_B.SelectAOV(currAOV);
            if (aov_ControlPanel_R != null && aov_ControlPanel_R.IsVisible)
                aov_ControlPanel_R.SelectAOV(currAOV);

            if (vm != null && !string.IsNullOrWhiteSpace(mapBaseSettings?.ScaleRulerIcon))
            {
                BitmapImage img = null;
                bool isSuccess = false;
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                string sFile = $"{sPath}\\Resources\\{mapBaseSettings.ScaleRulerIcon}.png";
                try
                {
                    img = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new System.Drawing.Bitmap(sFile));
                    isSuccess = img != null;
                }
                catch
                {
                    logger.Warn("Load ScaleRulerIcon Failed, File: " + sFile);
                    isSuccess = false;
                }
                if (isSuccess)
                {
                    double height = (grid_Bottom1.Height - ckb_ShowScaleRuler_B.Margin.Top - ckb_ShowScaleRuler_B.Margin.Bottom) * 0.5;
                    ckb_ShowScaleRuler_B.Height = height;
                    ckb_ShowScaleRuler_R.Height = height;
                    ckb_ShowScaleRuler_B.Content = new Image() { Source = img };
                    ckb_ShowScaleRuler_R.Content = new Image() { Source = img };
                }
            }

            bool isTypeVertical = mapBaseSettings?.IsTypeVertical ?? false;
            if (isTypeVertical)
                SetMapType(Map_Type.Vertical);
            else
                SetMapType(Map_Type.Horizontal);

            SetMaxScale();

            Map.Start(app, currAOV, isChecked_ShowScaleRuler);

            setContextMenu_ShowFullMap();

            ShowFullMap();

            registerEvents();

            app.ObjCacheManager._SectionChange(this, null);
        }

        public void SetMaxScale()
        {
            if (vm != null)
            {
                vm.MaximumScale = 75 / Map.dWidth_Vehicle; // Zoom In 後, 車子佔 N pixel
            }
        }

        public void ShowFullMap()
        {
            if (vm != null)
            {
                vm.ShowFullMap(grid_Map.ActualWidth, grid_Map.ActualHeight, Map.Width, Map.Height);
            }
        }

        public async void SetFocusPoint(object sender, Point focus)
        {
            // 若在預設位置 則回到全地圖模式
            if (focus.X == 0 && focus.Y == 0)
            {
                ShowFullMap();
                return;
            }

            if (vm != null)
            {
                if (vm.Scale != vm.MaximumScale)
                {
                    vm.Scale = vm.MaximumScale;
                    await Task.Delay(250);
                }
                focus = rotatePoint(focus);
                double horizontalOffset = focus.X * vm.MaximumScale - (sv_Map.ViewportWidth / 2);
                horizontalOffset = horizontalOffset < 0 ? 0 :
                                   horizontalOffset > sv_Map.ScrollableWidth ? sv_Map.ScrollableWidth :
                                   horizontalOffset;
                double verticalOffset = focus.Y * vm.MaximumScale - (sv_Map.ViewportHeight / 2);
                verticalOffset = verticalOffset < 0 ? 0 :
                                 verticalOffset > sv_Map.ScrollableHeight ? sv_Map.ScrollableHeight :
                                 verticalOffset;
                sv_Map.ScrollToHorizontalOffset(horizontalOffset);
                sv_Map.ScrollToVerticalOffset(verticalOffset);
            }
        }

        private Point rotatePoint(Point point)
        {
            const double pi = 3.14159;
            double arc = Convert.ToDouble(currAOV) / 180 * pi;
            double sinA = Math.Sin(arc);
            double cosA = Math.Cos(arc);
            Point rotateCenter = new Point();
            Point offset = new Point();

            switch (currAOV)
            {
                case Definition.AngleOfViewType.degree_0:
                    break;
                case Definition.AngleOfViewType.degree_180:
                    rotateCenter = new Point(Map.Width / 2, Map.Height / 2);
                    break;
                case Definition.AngleOfViewType.degree_90:
                    offset = new Point(Map.Width, 0);
                    break;
                case Definition.AngleOfViewType.degree_270:
                    offset = new Point(0, Map.Height);
                    break;
            }

            // translate point back to origin:
            point.X -= rotateCenter.X;
            point.Y -= rotateCenter.Y;

            // rotate point
            double newX = point.X * cosA - point.Y * sinA;
            double newY = point.X * sinA + point.Y * cosA;

            // translate point back:
            point.X = newX + rotateCenter.X;
            point.Y = newY + rotateCenter.Y;

            // translate point back to origin:
            point.X += offset.X;
            point.Y += offset.Y;

            return point;
        }

        public void SetMapType(int _type)
        {
            switch (_type)
            {
                case Map_Type.Horizontal:
                    grid_Left1.Visibility = Visibility.Collapsed;
                    grid_Right1.Visibility = Visibility.Visible;
                    grid_Right2.Visibility = Visibility.Visible;
                    grid_Top1.Visibility = Visibility.Collapsed;
                    grid_Bottom1.Visibility = Visibility.Collapsed;
                    grid_Map.Visibility = Visibility.Visible;
                    break;
                case Map_Type.Vertical:
                    grid_Left1.Visibility = Visibility.Collapsed;
                    grid_Right1.Visibility = Visibility.Collapsed;
                    grid_Right2.Visibility = Visibility.Collapsed;
                    grid_Top1.Visibility = Visibility.Visible;
                    grid_Bottom1.Visibility = Visibility.Visible;
                    grid_Map.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

            ckb_ShowScaleRuler_B.IsChecked = isChecked_ShowScaleRuler;
            ckb_ShowScaleRuler_R.IsChecked = isChecked_ShowScaleRuler;

            aov_ControlPanel_B.SelectAOV(currAOV);
            aov_ControlPanel_R.SelectAOV(currAOV);
        }

        #region Event
        private void registerEvents()
        {
            grid_Map.SizeChanged += grid_Map_SizeChanged;

            ckb_ShowScaleRuler_B.Checked += ckb_ShowScaleRuler_Checked;
            ckb_ShowScaleRuler_B.Unchecked += ckb_ShowScaleRuler_Unchecked;
            ckb_ShowScaleRuler_R.Checked += ckb_ShowScaleRuler_Checked;
            ckb_ShowScaleRuler_R.Unchecked += ckb_ShowScaleRuler_Unchecked;

            if (aov_ControlPanel_B.IsVisible)
                aov_ControlPanel_B.AngleOfViewChanged += aov_Changed;
            if (aov_ControlPanel_R.IsVisible)
                aov_ControlPanel_R.AngleOfViewChanged += aov_Changed;

            #region Address Event
            if (Map.ListAddress?.Count > 0)
            {
                foreach (Menu_Maintenance.Address_PZ addr in Map.ListAddress)
                {
                    addr.AddressBeChosen += SetParkingZone_AdrID;
                }
            }
            #endregion Address Event
        }

        private void unregisterEvents()
        {
            grid_Map.SizeChanged -= grid_Map_SizeChanged;

            ckb_ShowScaleRuler_B.Checked -= ckb_ShowScaleRuler_Checked;
            ckb_ShowScaleRuler_B.Unchecked -= ckb_ShowScaleRuler_Unchecked;
            ckb_ShowScaleRuler_R.Checked -= ckb_ShowScaleRuler_Checked;
            ckb_ShowScaleRuler_R.Unchecked -= ckb_ShowScaleRuler_Unchecked;

            aov_ControlPanel_B.AngleOfViewChanged -= aov_Changed;
            aov_ControlPanel_R.AngleOfViewChanged -= aov_Changed;
        }

        public void SetParkingZone_AdrID(object obj, string sAddr)
        {
            try
            {
                Adapter.Invoke((o) =>
                {
                    if (Map.ListAddress?.Count > 0)
                    {
                        Menu_Maintenance.Address_PZ addr = Map.ListAddress.Where(a => a.p_ID.Trim() == sAddr.Trim()).FirstOrDefault();

                        pzManagement.SetSelectAdr(sAddr, addr.p_IsSelect);
                    }

                    
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void grid_Map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (vm != null)
            {
                if (vm.Scale == vm.MinimumScale)
                {
                    ShowFullMap();
                }
            }
        }
        #endregion Event

        private void SetMapType_Hor_Click(object sender, RoutedEventArgs e)
        {
            SetMapType(Map_Type.Horizontal);
        }

        private void SetMapType_Ver_Click(object sender, RoutedEventArgs e)
        {
            SetMapType(Map_Type.Vertical);
        }

        private void ckb_ShowScaleRuler_Checked(object sender, RoutedEventArgs e)
        {
            isChecked_ShowScaleRuler = true;
            Map.SetRulerVisible(isChecked_ShowScaleRuler);
        }

        private void ckb_ShowScaleRuler_Unchecked(object sender, RoutedEventArgs e)
        {
            isChecked_ShowScaleRuler = false;
            Map.SetRulerVisible(isChecked_ShowScaleRuler);
        }

        private void aov_Changed(object sender, Definition.AngleOfViewType aov)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (currAOV == aov) return;

                    currAOV = aov;


                    unregisterEvents();

                    Map.Start(app, currAOV, isChecked_ShowScaleRuler);

                    setContextMenu_ShowFullMap();

                    ShowFullMap();

                    registerEvents();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
 
        private void ScaleSlider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider == null || !slider.IsEnabled || vm == null) return;
            slider.IsEnabled = false;
            try
            {
                if (e.Delta > 0)
                {
                    // Zoom Out
                    vm.Scale -= vm.ScaleTickFrequency;
                    vm.Scale = vm.Scale < vm.MinimumScale ? vm.MinimumScale : vm.Scale;
                }
                else if (e.Delta < 0)
                {
                    // Zoom In
                    vm.Scale += vm.ScaleTickFrequency;
                    vm.Scale = vm.Scale > vm.MaximumScale ? vm.MaximumScale : vm.Scale;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                slider.IsEnabled = true;
            }
        }

        private void setContextMenu_ShowFullMap()
        {
            MenuItem mi = new MenuItem();
            mi.Header = "Show Full Map";
            mi.Click += ShowFullMap_Click;
            mi.FontSize = 16;

            ContextMenu cm = new ContextMenu();
            cm.Items.Add(mi);

            Grid gridSpace = new Grid()
            {
                Width = Map.Width,
                Height = Map.Height,
                Background = Brushes.Transparent,
                ContextMenu = cm
            };
            gridSpace.ContextMenuOpening += _ContextMenuOpening;
            Map.Map_Canvas.Children.Insert(0, gridSpace);
        }
        private void ShowFullMap_Click(object sender, EventArgs e)
        {
            ShowFullMap();
        }
        private void _ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            try
            {
                if (vm != null)
                {
                    e.Handled = vm.Scale == vm.MinimumScale;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                e.Handled = false;
            }
        }

        private void Map_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (!Keyboard.IsKeyDown(Key.LeftCtrl)) return;
            
            Slider slider = null;
            if(ScaleSlider_B.Visibility==Visibility.Visible)
            {
                slider = ScaleSlider_B;
            }
            else if (ScaleSlider_R.Visibility == Visibility.Visible)
            {
                slider = ScaleSlider_R;
            }

            if (slider == null || !slider.IsEnabled || vm == null) return;
            slider.IsEnabled = false;
            try
            {
                if (e.Delta < 0)
                {
                    // Zoom Out
                    vm.Scale -= vm.ScaleTickFrequency;
                    vm.Scale = vm.Scale < vm.MinimumScale ? vm.MinimumScale : vm.Scale;
                }
                else if (e.Delta > 0)
                {
                    // Zoom In
                    vm.Scale += vm.ScaleTickFrequency;
                    vm.Scale = vm.Scale > vm.MaximumScale ? vm.MaximumScale : vm.Scale;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                slider.IsEnabled = true;
            }
        }
    }

}
