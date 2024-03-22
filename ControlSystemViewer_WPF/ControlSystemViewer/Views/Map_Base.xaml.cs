using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using ControlSystemViewer.PopupWindows;
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

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// Map_Base.xaml 的互動邏輯
    /// </summary>
    public partial class Map_Base : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public WindownApplication app = null;
        private Settings.MapBase mapBaseSettings = null;
        private MainLayout mainLayout = null;
        private MapViewModel vm = null;
        //public List<Map_Parts.Vehicle> ListVehicle_R = null;
        //public List<Map_Parts.Vehicle> ListVehicle_T = null;
        // 車輛展示區塊 與 StatusTree-Vehicle 合併
        private List<Map_Parts.Vehicle> listVehicle_StatusTree = null;
        private bool isChecked_ShowScaleRuler = false;
        private Definition.AngleOfViewType currAOV = Definition.AngleOfViewType.degree_0;
        private bool currFlipH = false;
        private bool currFlipV = false;
        //private QuickVehicleCommandPopupWindow qkVhCmdPopupWindow = null;
        #endregion 公用參數設定

        public Map_Base()
        {
            InitializeComponent();

            vm = DataContext as MapViewModel;
        }

        public void Start(WindownApplication _app, MainLayout _mainLayout, List<Map_Parts.Vehicle> _listVehicle_StatusTree = null)
        {
            app = _app;
            mapBaseSettings = app?.ObjCacheManager?.ViewerSettings?.mapBase;

            mainLayout = _mainLayout;

            listVehicle_StatusTree = _listVehicle_StatusTree;

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
                    //double width = (grid_Right1.ActualWidth - ckb_ShowScaleRuler_R.Margin.Left - ckb_ShowScaleRuler_R.Margin.Right) * 0.6;
                    //ckb_ShowScaleRuler_B.Width = width;
                    //ckb_ShowScaleRuler_R.Width = width;
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

            //initialVehicle();

            registerEvents();

            app.ObjCacheManager._SectionChange(this, null);
        }

        //private void initialVehicle()
        //{
        //    int index = 0;
        //    ListVehicle_R = new List<Map_Parts.Vehicle>();
        //    ListVehicle_T = new List<Map_Parts.Vehicle>();
        //    var vhs = app.ObjCacheManager.GetVEHICLEs();
        //    VehicleDafualtLocation_R.Children.Add(new Border() { Height = 20 });
        //    VehicleDafualtLocation_T.Children.Add(new Border() { Width = 20 });
        //    foreach (var vh in vhs)
        //    {
        //        ListVehicle_R.Add(new Map_Parts.Vehicle(app, vh, new Point(0, 0), VehicleDafualtLocation_R.Width, _isShowcase: true));
        //        VehicleDafualtLocation_R.Children.Add(new Border() { Height = 10});
        //        VehicleDafualtLocation_R.Children.Add(ListVehicle_R[index].vhPresenter);

        //        ListVehicle_T.Add(new Map_Parts.Vehicle(app, vh, new Point(0, 0), VehicleDafualtLocation_T.Height, _isShowcase: true));
        //        VehicleDafualtLocation_T.Children.Add(new Border() { Width = 10});
        //        VehicleDafualtLocation_T.Children.Add(ListVehicle_T[index].vhPresenter);

        //        index++;
        //    }
        //}

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

        public void TransferObjectMouseLeftButtonDown(string objID, object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(objID)) return;
            var vh = Map.ListVehicle.FirstOrDefault(v => v.p_ID == objID);
            if (vh != null)
            {
                vh._MouseLeftButtonDown(sender, e);
                return;
            }
            var addr = Map.ListAddress.FirstOrDefault(a => a.p_Charger?.ChargerID == objID);
            if (addr != null)
            {
                //addr._MouseLeftButtonDown(sender, e);
                SetFocusPoint(sender, new Point(addr.p_X + (addr.p_Width / 2), addr.p_Y + (addr.p_Height / 2)));
                return;
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
                foreach (Map_Parts.Address addr in Map.ListAddress)
                {
                    addr.AddressBeChosen += SetQuickVehicleCommand_AdrID;
                }
            }
            #endregion Address Event

            #region Vehicle Event
            //if (ListVehicle_R?.Count > 0)
            //{
            //    foreach (Map_Parts.Vehicle vh in ListVehicle_R)
            //    {
            //        vh.OpenQuickVehicleCommand += OpenQuickVehicleCommand;
            //        vh.VehicleBeChosen += SetQuickVehicleCommand_VhID;
            //        vh.VehicleBeChosen += SetMonitorVehicle;
            //    }
            //}
            //if (ListVehicle_T?.Count > 0)
            //{
            //    foreach (Map_Parts.Vehicle vh in ListVehicle_T)
            //    {
            //        vh.OpenQuickVehicleCommand += OpenQuickVehicleCommand;
            //        vh.VehicleBeChosen += SetQuickVehicleCommand_VhID;
            //        vh.VehicleBeChosen += SetMonitorVehicle;
            //    }
            //}
            if (listVehicle_StatusTree?.Count > 0)
            {
                foreach (Map_Parts.Vehicle vh in listVehicle_StatusTree)
                {
                    vh.OpenQuickVehicleCommand += OpenQuickVehicleCommand;
                    vh.VehicleBeChosen += SetQuickVehicleCommand_VhID;
                    vh.VehicleBeChosen += SetMonitorVehicle;
                }
            }
            if (Map.ListVehicle?.Count > 0)
            {
                foreach (Map_Parts.Vehicle vh in Map.ListVehicle)
                {
                    vh.OpenQuickVehicleCommand += OpenQuickVehicleCommand;
                    vh.VehicleBeChosen += SetQuickVehicleCommand_VhID;
                    vh.VehicleBeChosen += SetMonitorVehicle;
                    vh.ZoomIn += SetFocusPoint;
                }
            }
            #endregion Vehicle Event

            app.ObjCacheManager.SegmentChange += ObjCacheManager_SegmentChange;
            app.ObjCacheManager.SectionChange += ObjCacheManager_SectionChange;
            app.ObjCacheManager.ReserveInfoUpdate += ObjCacheManager_ReserveInfoUpdate;
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

            #region Vehicle Event
            //if (ListVehicle_R?.Count > 0)
            //{
            //    foreach (Map_Parts.Vehicle vh in ListVehicle_R)
            //    {
            //        vh.OpenQuickVehicleCommand -= OpenQuickVehicleCommand;
            //        vh.VehicleBeChosen -= SetQuickVehicleCommand_VhID;
            //        vh.VehicleBeChosen -= SetMonitorVehicle;
            //    }
            //}
            //if (ListVehicle_T?.Count > 0)
            //{
            //    foreach (Map_Parts.Vehicle vh in ListVehicle_T)
            //    {
            //        vh.OpenQuickVehicleCommand -= OpenQuickVehicleCommand;
            //        vh.VehicleBeChosen -= SetQuickVehicleCommand_VhID;
            //        vh.VehicleBeChosen -= SetMonitorVehicle;
            //    }
            //}
            if (listVehicle_StatusTree?.Count > 0)
            {
                foreach (Map_Parts.Vehicle vh in listVehicle_StatusTree)
                {
                    vh.OpenQuickVehicleCommand -= OpenQuickVehicleCommand;
                    vh.VehicleBeChosen -= SetQuickVehicleCommand_VhID;
                    vh.VehicleBeChosen -= SetMonitorVehicle;
                }
            }
            if (Map.ListVehicle?.Count > 0)
            {
                foreach (Map_Parts.Vehicle vh in Map.ListVehicle)
                {
                    vh.OpenQuickVehicleCommand -= OpenQuickVehicleCommand;
                    vh.VehicleBeChosen -= SetQuickVehicleCommand_VhID;
                    vh.VehicleBeChosen -= SetMonitorVehicle;
                }
            }
            #endregion Vehicle Event

            app.ObjCacheManager.SegmentChange -= ObjCacheManager_SegmentChange;
            app.ObjCacheManager.ReserveInfoUpdate -= ObjCacheManager_ReserveInfoUpdate;
        }

        private void ObjCacheManager_ReserveInfoUpdate(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    setReservedRail(app?.ObjCacheManager?.GetAllReserveInfo() ?? new List<string>());

                    if(MonitoringVh != null)
                        setReservedRail(app?.ObjCacheManager?.GetAllReserveInfo(MonitoringVh) ?? new List<string>());
                    else
                        setReservedRail(app?.ObjCacheManager?.GetAllReserveInfo() ?? new List<string>());
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_SegmentChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    Map?.SetDisableRail();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void ObjCacheManager_SectionChange(object sender, EventArgs e)
        {
            //map 收到section狀態被變更
            foreach(var sec in app.ObjCacheManager.Sections)
            {
                var r = Map.ListRail.Find(x => x.p_ID == sec.ID);
                if (r != null)
                {
                    r.p_IsDisabled = !sec.enable;
                    Adapter.Invoke((obj) =>
                    {
                        r.RefreshRail();
                    }, null);
                }
            }
        }
        public EventHandler<string> MonitoringVhChanged;
        VVEHICLE MonitoringVh = null;
        string predictPathHandler = "predictPathHandler";
        public void InitMonitorVehicle()
        {
            try
            {
                if (MonitoringVh != null)
                {
                    MonitoringVh.PathChange -= changeMonitoringVhPath;
                }

                MonitoringVh = null;

                initRec();

                Adapter.Invoke((o) =>
                {
                    resetAllRail();
                    ResetAllAdr();
                }, null);

                MonitoringVhChanged?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        public void SetMonitorVehicle(object obj, string vh_id)
        {
            if (string.IsNullOrWhiteSpace(vh_id)) return;
            if (MonitoringVh?.VEHICLE_ID?.Trim() == vh_id) return;

            resetMonitorVehicle(vh_id);
        }
        private void resetMonitorVehicle(string vh_id)
        {
            if (string.IsNullOrWhiteSpace(vh_id)) return;

            try
            {
                lock (predictPathHandler)
                {
                    if (MonitoringVh != null)
                        MonitoringVh.PathChange -= changeMonitoringVhPath;

                    MonitoringVh = app.ObjCacheManager.GetVEHICLE(vh_id);
                    if (MonitoringVh != null)
                    {
                        initRec();
                        changeMonitoringVhPath(null, null);
                        MonitoringVh.PathChange += changeMonitoringVhPath;
                        MonitoringVhChanged?.Invoke(this, vh_id);
                        //選取完車子之後刷新reserve資訊\
                        setReservedRail(app?.ObjCacheManager?.GetAllReserveInfo(MonitoringVh) ?? new List<string>());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void changeMonitoringVhPath(object obj, EventArgs e)
        {
            try
            {
                Adapter.Invoke((o) =>
                {
                    setSpecifyRail(MonitoringVh?.PATH_PREDICT_SECTIONS, MonitoringVh?.PATH_WILLPASS_SECTIONS);
                    setSpecifyAdr(MonitoringVh?.PATH_START_ADR, MonitoringVh?.PATH_FROM_ADR, MonitoringVh?.PATH_TO_ADR);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        #region MonitoringVhPath 相關功能
        private List<string> rec_listPathSecs = null;
        private string rec_startAdr = null;
        private string rec_fromAdr = null;
        private string rec_toAdr = null;
        private void initRec()
        {
            rec_listPathSecs = null;
            rec_startAdr = null;
            rec_fromAdr = null;
            rec_toAdr = null;
        }
        public void setReservedRail(List<string> listReservedSecs)
        {
            if (Map?.ListRail?.Count > 0)
            {
                foreach (var rail in Map.ListRail)
                {
                    bool isReserved = false;
                    if (listReservedSecs?.Count > 0)
                    {
                        isReserved = listReservedSecs.Any(rs => rs == rail.p_ID);
                    }
                    rail.p_IsReserved = isReserved;
                }
            }
        }
        private void setSpecifyRail(List<string> listFullPathSecs, List<string> listWillPassSecs)
        {
            if (isSame(rec_listPathSecs, listWillPassSecs)) return;

            resetAllRail();
            if (listFullPathSecs?.Count > 0 && listWillPassSecs?.Count > 0)
            {
                foreach (string sSec in listFullPathSecs)
                {
                    //Brush brush = listWillPassSecs.Any(wp => wp == sSec) ? Brushes.Yellow : Brushes.Gold;
                    Map?.ListRail?.Where(r => r.p_ID == sSec)?.FirstOrDefault()?.RefreshRail(Brushes.Yellow);
                }
            }
            rec_listPathSecs = listWillPassSecs;
        }
        private void setSpecifyAdr(string startAdr, string fromAdr, string toAdr)
        {
            if (rec_startAdr == startAdr && rec_fromAdr == fromAdr && rec_toAdr == toAdr) return;

            ResetAllAdr();
            if (!string.IsNullOrWhiteSpace(startAdr))
            {
                Map?.ListAddress?.Where(a => a.p_ID == startAdr)?.FirstOrDefault()?.RefreshAddress(Brushes.Violet);
            }
            if (!string.IsNullOrWhiteSpace(fromAdr))
            {
                Map?.ListAddress?.Where(a => a.p_ID == fromAdr)?.FirstOrDefault()?.RefreshAddress(Brushes.Lime);
            }
            if (!string.IsNullOrWhiteSpace(toAdr))
            {
                Map?.ListAddress?.Where(a => a.p_ID == toAdr)?.FirstOrDefault()?.RefreshAddress(Brushes.Red);
            }
            rec_startAdr = startAdr;
            rec_fromAdr = fromAdr;
            rec_toAdr = toAdr;
        }
        private void resetAllRail()
        {
            if (Map?.ListRail?.Count > 0)
            {
                foreach (var rail in Map.ListRail)
                {
                    rail.RefreshRail();
                }
            }
        }
        public void ResetAllAdr()
        {
            if (Map?.ListAddress?.Count > 0)
            {
                foreach (var adr in Map.ListAddress)
                {
                    adr.RefreshAddress();
                }
            }
        }
        public void ResetAdr_Charger()
        {
            if (Map?.ListAddress?.Count > 0)
            {
                foreach (var adr in Map.ListAddress)
                {
                    if (adr.p_IsCharger|| adr.p_IsCoupler)
                        adr.RefreshAddress();
                }
            }
        }
        private bool isSame(List<string> a, List<string> b)
        {
            if (a == null && b == null) return true;
            if (a == null && b != null) return false;
            if (a != null && b == null) return false;
            if (a.Count != b.Count) return false;
            for (int i=0;i<a.Count;i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        #endregion MonitoringVhPath 相關功能

        #region QuickVehicleCommandForm 相關
        public void OpenQuickVehicleCommand(object obj, string vh_id)
        {
            try
            {
                //Adapter.Invoke((o) =>
                //{
                //    if (qkVhCmdPopupWindow == null)
                //    {
                //        qkVhCmdPopupWindow = new QuickVehicleCommandPopupWindow(this, obj as Map_Parts.Vehicle);
                //        qkVhCmdPopupWindow.Show();
                //        app.ObjCacheManager.LogInUserChanged += objCacheManager_LoginChanged;
                //    }
                //}, null);
                SetQuickVehicleCommand_VhID(obj, vh_id);
                SetMonitorVehicle(obj, vh_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void objCacheManager_LoginChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    Adapter.Invoke((o) =>
            //    {
            //        if (string.IsNullOrEmpty(app.LoginUserID))
            //        {
            //            app.ObjCacheManager.LogInUserChanged -= objCacheManager_LoginChanged;
            //            qkVhCmdPopupWindow?.Close();
            //        }
            //    }, null);
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Exception");
            //}
        }

        public void Closed_QuickVehicleCommandForm()
        {
            //try
            //{
            //    qkVhCmdPopupWindow = null;
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Exception");
            //}
        }

        public void SetQuickVehicleCommand_VhID(object obj, string vh_id)
        {
            try
            {
                Adapter.Invoke((o) =>
                {
                    //if (qkVhCmdPopupWindow != null)
                    //{
                    //    qkVhCmdPopupWindow.BringToFront();
                    //    qkVhCmdPopupWindow.SelectVehicle(vh_id);
                    //}
                    mainLayout.QuickVehicleCommand_SelectVh(vh_id);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private int iSelectType_Switch = 1; // 0:From, 1:To
        public void SetQuickVehicleCommand_AdrID(object obj, string sAddr)
        {
            try
            {
                Adapter.Invoke((o) =>
                {
                    //if (qkVhCmdPopupWindow != null)
                    //{
                    //    qkVhCmdPopupWindow.BringToFront();
                    //    if (iSelectType_Switch == 0)
                    //    {
                    //        qkVhCmdPopupWindow.SelectFromAdr(sAddr);
                    //    }
                    //    else //if (iSelectType_Switch == 1)
                    //    {
                    //        qkVhCmdPopupWindow.SelectToAdr(sAddr);
                    //    }
                    //    iSelectType_Switch = iSelectType_Switch == 0 ? 1 : 0;
                    //}
                    mainLayout.QuickVehicleCommand_SelectAdr(sAddr);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        #endregion QuickVehicleCommandForm 相關

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


        public Definition.AngleOfViewType GetCurrAOV() => currAOV;
        public bool GetcurrFlipH() => currFlipH;
        public bool GetcurrFlipV() => currFlipV;

        public void ChangeAOV(object sender, Definition.AngleOfViewType aov)
        {
            aov_Changed(sender, aov);
        }

        #region Flip Version
        public void ChangeAOVWithFlip(object sender, Definition.AngleOfViewType aov)
        {
            aov_ChangedWithFlip(sender, aov,currFlipH,currFlipV);
        }

        public void ChangeFlipH(object sender, bool FlipH=false)
        {
            aov_ChangedWithFlip(sender, currAOV, FlipH, currFlipV);
        }

        public void ChangeFlipV(object sender,bool FlipV = false)
        {
            aov_ChangedWithFlip(sender, currAOV, currFlipH, FlipV);
        }

        private void aov_ChangedWithFlip(object sender, Definition.AngleOfViewType aov, bool FlipH = false, bool FlipV = false)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (currAOV == aov && currFlipH == FlipH && currFlipV == FlipV) return;

                    currAOV = aov;
                    currFlipH = FlipH;
                    currFlipV = FlipV;

                    unregisterEvents();

                    Map.Start(app, currAOV, isChecked_ShowScaleRuler, currFlipH, currFlipV);

                    setContextMenu_ShowFullMap();

                    ShowFullMap();

                    registerEvents();

                    resetMonitorVehicle(MonitoringVh?.VEHICLE_ID?.Trim());
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        #endregion

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

                    resetMonitorVehicle(MonitoringVh?.VEHICLE_ID?.Trim());
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

    public class Map_Type
    {
        public const int Horizontal = 0;
        public const int Vertical = 1;
    }
}
