using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// Map.xaml 的互動邏輯
    /// </summary>
    public partial class Map_PZ : UserControl
	{
        #region 公用參數設定
        public WindownApplication app = null;
        private Settings.Map mapSettings = null;
        public List<Map_Parts.Rail> ListRail = null;
        public List<Menu_Maintenance.Address_PZ> ListAddress = null;
        public List<Map_Parts.Label> ListLabel = null;
        private double Margin_Top = 100;
        private double Margin_Left = 100;
        private double Margin_Right = 100;
        private double Margin_Bottom = 100;
        public Point pOffset = new Point(0, 0);
        private double dWidth_Rail = 200;
        private double dWidth_Address = 200;
        private double dWidth_Shelf = 200;
        private double dWidth_ShelfOffset = 500;
        private double dWidth_TrackSwitch = 200;
        public double dWidth_Vehicle = 1200;
        public double dWidth_SurroundingWhiteSpace = 1000;
        private double dTick_MinX;
        private double dTick_MaxX;
        private double dTick_MinY;
        private double dTick_MaxY;
        private bool isReversed_RulerTick_X = false;
        private bool isReversed_RulerTick_Y = false;
        private double dWidth_RulerTick = 50;
        private double dFrequency_RulerTick = 2000;
        private double dFontSize_RulerTick;
        private Canvas cvRuler = null;
        private Canvas cvRail = null;
        private Canvas cvAddress = null;
        private Canvas cvLabel = null;
        private int iAngleOfView = 0;
        private bool bFlipH = false;
        private bool bFlipV = false;
        private Point pCenter = new Point();
        private readonly int timer_interval = 5;
        #endregion 公用參數設定

        public Map_PZ()
        {
            InitializeComponent();
        }

		public void Start(WindownApplication _app, Definition.AngleOfViewType _aov, bool showScaleRuler,bool FlipH=false,bool FlipV=false)
		{
			app = _app;
            iAngleOfView = (int)_aov;
            bFlipH = FlipH;
            bFlipV = FlipV;

            initialMapSettings();
            initialMapSpace();
            initialRuler(showScaleRuler);
            initialRail();
            initialAddress();

            initialLabel();
        }

        private void initialMapSettings()
        {
            mapSettings = app?.ObjCacheManager?.ViewerSettings?.map;
            if (mapSettings != null)
            {
                dWidth_Rail = mapSettings.RailWidth;
                dWidth_Address = mapSettings.AddressWidth;
                dWidth_Vehicle = mapSettings.VehicleWidth;
                dWidth_SurroundingWhiteSpace = mapSettings.SurroundingWhiteSpaceWidth;
                dFrequency_RulerTick = mapSettings.RulerTickFrequency;
                dWidth_Shelf = mapSettings.ShelfWidth;
                dWidth_ShelfOffset = mapSettings.ShelfOffset;
                dWidth_TrackSwitch = mapSettings.TrackSwitchWidth;
            }
        }

        private void initialMapSpace()
        {
            Map_Canvas.Children.Clear();

            var listVisibleLabel = app.ObjCacheManager.Labels?.Where(l => l.IsVisible)?.ToList();
            dTick_MinX = app.ObjCacheManager.Addresses.Min(a => a.X);
            dTick_MinX = listVisibleLabel?.Count > 0 ? Math.Min(dTick_MinX, listVisibleLabel.Min(l => l.X1)) : dTick_MinX;
            dTick_MaxX = app.ObjCacheManager.Addresses.Max(a => a.X);
            dTick_MaxX = listVisibleLabel?.Count > 0 ? Math.Max(dTick_MaxX, listVisibleLabel.Max(l => l.X2)) : dTick_MaxX;
            dTick_MinY = app.ObjCacheManager.Addresses.Min(a => a.Y);
            dTick_MinY = listVisibleLabel?.Count > 0 ? Math.Min(dTick_MinY, listVisibleLabel.Min(l => l.Y1)) : dTick_MinY;
            dTick_MaxY = app.ObjCacheManager.Addresses.Max(a => a.Y);
            dTick_MaxY = listVisibleLabel?.Count > 0 ? Math.Max(dTick_MaxY, listVisibleLabel.Max(l => l.Y2)) : dTick_MaxY;

            double tickTextWidth;
            double dBestFontSize_Top = CustomTickBar.GetBestFontSize(((int)(dTick_MaxX - dTick_MinX) * 10).ToString(), dFrequency_RulerTick * 0.9, dWidth_SurroundingWhiteSpace / 2 * 0.8, out tickTextWidth);
            double dBestFontSize_Left = CustomTickBar.GetBestFontSize(((int)(dTick_MaxY - dTick_MinY) * 10).ToString(), dWidth_SurroundingWhiteSpace / 2 * 0.8, dFrequency_RulerTick * 0.9, out tickTextWidth);
            dFontSize_RulerTick = dBestFontSize_Top <= dBestFontSize_Left ? dBestFontSize_Top : dBestFontSize_Left;
            Margin_Left = dWidth_SurroundingWhiteSpace;
            Margin_Right = Margin_Left;
            Margin_Top = Margin_Left;
            Margin_Bottom = Margin_Left;
            pOffset = new Point(Margin_Left - dTick_MinX, Margin_Top - dTick_MinY);

            this.Width = dTick_MaxX - dTick_MinX + (Margin_Left + Margin_Right);
            this.Height = dTick_MaxY - dTick_MinY + (Margin_Top + Margin_Bottom);

            pCenter = new Point(this.Width / 2, this.Height / 2);

            if (iAngleOfView == 90 || iAngleOfView == 270)
            {
                double dTmp;

                dTmp = this.Width;
                this.Width = this.Height;
                this.Height = dTmp;

                dTmp = dTick_MinX;
                dTick_MinX = dTick_MinY;
                dTick_MinY = dTmp;

                dTmp = dTick_MaxX;
                dTick_MaxX = dTick_MaxY;
                dTick_MaxY = dTmp;
            }
        }

        private void initialRail()
        {
            cvRail = new Canvas();
            int index = 0;
            ListRail = new List<Map_Parts.Rail>();
            foreach (var sec in app.ObjCacheManager.Sections)
            {
                if (sec.StartAddress == null || sec.EndAddress == null) continue;
                
                ListRail.Add(new Map_Parts.Rail(sec, dWidth_Rail, pOffset));
                cvRail.Children.Add(ListRail[index]);
                index++;
            }

            TransformGroup myTransformGroup = new TransformGroup();
            if (bFlipH==true && bFlipV==false)
            {
                myTransformGroup.Children.Add(new ScaleTransform(-1,1, pCenter.X, pCenter.Y));
            }
            else if (bFlipH == false && bFlipV == true)
            {
                myTransformGroup.Children.Add(new ScaleTransform(1, -1, pCenter.X, pCenter.Y));
            }
            else if (bFlipH == true && bFlipV == true)
            {
                myTransformGroup.Children.Add(new ScaleTransform(-1, -1, pCenter.X, pCenter.Y));
            }


            if (iAngleOfView == 180)
            {
                myTransformGroup.Children.Add(new RotateTransform(iAngleOfView, pCenter.X, pCenter.Y));
            }
            else if (iAngleOfView == 90)
            {
                myTransformGroup.Children.Add(new RotateTransform(iAngleOfView) );
                cvRail.Margin = new Thickness(this.Width, 0, 0, 0);
            }
            else if (iAngleOfView == 270)
            {
                myTransformGroup.Children.Add(new RotateTransform(iAngleOfView));
                cvRail.Margin = new Thickness(0, this.Height, 0, 0);
            }

            if(myTransformGroup.Children.Count!=0)
            {
                cvRail.RenderTransform = myTransformGroup;
            }
            Map_Canvas.Children.Add(cvRail);

            SetDisableRail();
        }

        public void SetDisableRail()
        {
            var listSeg = app.ObjCacheManager.GetSegments();
            if (listSeg?.Count > 0)
            {
                foreach (var seg in listSeg)
                {
                    if (ListRail?.Count > 0 && !string.IsNullOrWhiteSpace(seg.SEG_ID))
                    {
                        List<Map_Parts.Rail> listSegRail = ListRail.Where(r => r.p_ID.StartsWith(seg.SEG_ID?.Trim()))?.ToList();
                        if (listSegRail?.Count > 0)
                        {
                            foreach (var rail in listSegRail)
                            {
                                if (rail.p_IsDisabled != seg.IS_DISABLED)
                                {
                                    rail.p_IsDisabled = seg.IS_DISABLED;                              
                                }
                                rail.RefreshRail();
                            }
                        }
                    }
                }
            }
        }

        private void initialAddress()
        {
            cvAddress = new Canvas();
            int index = 0;
            ListAddress = new List<Menu_Maintenance.Address_PZ>();
            foreach (var addr in app.ObjCacheManager.Addresses)
            {
                ListAddress.Add(new Menu_Maintenance.Address_PZ(addr, dWidth_Address, pOffset));
                cvAddress.Children.Add(ListAddress[index]);
                index++;
            }

            TransformGroup myTransformGroup = new TransformGroup();
            if (bFlipH == true && bFlipV == false)
            {
                myTransformGroup.Children.Add(new ScaleTransform(-1, 1, pCenter.X, pCenter.Y));
            }
            else if (bFlipH == false && bFlipV == true)
            {
                myTransformGroup.Children.Add(new ScaleTransform(1, -1, pCenter.X, pCenter.Y));
            }
            else if (bFlipH == true && bFlipV == true)
            {
                myTransformGroup.Children.Add(new ScaleTransform(-1, -1, pCenter.X, pCenter.Y));
            }


            if (iAngleOfView == 180)
            {
                myTransformGroup.Children.Add(new RotateTransform(iAngleOfView, pCenter.X, pCenter.Y) ); 
            }
            else if (iAngleOfView == 90)
            {
                myTransformGroup.Children.Add(new RotateTransform(iAngleOfView));
                cvAddress.Margin = new Thickness(this.Width, 0, 0, 0);
            }
            else if (iAngleOfView == 270)
            {
                myTransformGroup.Children.Add(new RotateTransform(iAngleOfView));
                cvAddress.Margin = new Thickness(0, this.Height, 0, 0);
            }

            if (myTransformGroup.Children.Count != 0)
            {
                cvAddress.RenderTransform = myTransformGroup;
            }
            Map_Canvas.Children.Add(cvAddress);
        }

        private void _TrackSwitchStatusChanged(object sender, string id)
        {
            var trackSwitch = app?.ObjCacheManager.TrackSwitches?.Where(t => t.ID == id).FirstOrDefault();
            if (trackSwitch == null) return;

            bool hasAlarm = trackSwitch.Status == TrackSwitchStatus.Alarm;
            var track1 = ListRail.Where(r => r.p_ID == trackSwitch.Track1.ID).FirstOrDefault();
            if (track1 != null)
            {
                Adapter.Invoke((obj) =>
                {
                    track1.p_HasAlarm = hasAlarm;
                }, null);
            }
            var track2 = ListRail.Where(r => r.p_ID == trackSwitch.Track2.ID).FirstOrDefault();
            if (track2 != null)
            {
                Adapter.Invoke((obj) =>
                {
                    track2.p_HasAlarm = hasAlarm;
                }, null);
            }
        }

        private void initialLabel()
        {
            bool isVertical = false;
            if (iAngleOfView == 90 || iAngleOfView == 270)
                isVertical = true;
            cvLabel = new Canvas();
            Point rotateCenter = new Point();
            int index = 0;
            ListLabel = new List<Map_Parts.Label>();
            foreach (var lbl in app.ObjCacheManager.Labels)
            {
                if (!lbl.IsVisible) continue;

                ListLabel.Add(new Map_Parts.Label(lbl, pOffset, isVertical));
                if (lbl.Text != "arrow.png") // 顯示箭頭圖示
                {
                    TransformGroup myTransformGroup = new TransformGroup();
                    if (bFlipH == true && bFlipV == false)
                    {
                        myTransformGroup.Children.Add(new ScaleTransform(-1, 1, rotateCenter.X, rotateCenter.Y));
                    }
                    else if (bFlipH == false && bFlipV == true)
                    {
                        myTransformGroup.Children.Add(new ScaleTransform(1, -1, rotateCenter.X, rotateCenter.Y));
                    }
                    else if (bFlipH == true && bFlipV == true)
                    {
                        myTransformGroup.Children.Add(new ScaleTransform(-1, -1, rotateCenter.X, rotateCenter.Y));
                    }


                    if (iAngleOfView == 180)
                    {
                        rotateCenter = new Point(ListLabel[index].p_X + ListLabel[index].p_Width / 2, ListLabel[index].p_Y + ListLabel[index].p_Height / 2);
                        myTransformGroup.Children.Add(new RotateTransform(-iAngleOfView, rotateCenter.X, rotateCenter.Y));
                    }
                    else if (iAngleOfView == 90)
                    {
                        rotateCenter = new Point(ListLabel[index].p_X, ListLabel[index].p_Y);
                        myTransformGroup.Children.Add(new RotateTransform(-iAngleOfView, rotateCenter.X, rotateCenter.Y));
                        ListLabel[index].Margin = new Thickness(0, ListLabel[index].p_Width, 0, 0);
                    }
                    else if (iAngleOfView == 270)
                    {
                        rotateCenter = new Point(ListLabel[index].p_X, ListLabel[index].p_Y);
                        myTransformGroup.Children.Add(new RotateTransform(-iAngleOfView, rotateCenter.X, rotateCenter.Y));
                        ListLabel[index].Margin = new Thickness(ListLabel[index].p_Height, 0, 0, 0);
                    }

                    if (myTransformGroup.Children.Count != 0)
                    {
                        ListLabel[index].RenderTransform = myTransformGroup;
                    }
                }

                cvLabel.Children.Add(ListLabel[index]);
                index++;
            }

            TransformGroup LabelTransformGroup = new TransformGroup();
            if (bFlipH == true && bFlipV == false)
            {
                LabelTransformGroup.Children.Add(new ScaleTransform(-1, 1, pCenter.X, pCenter.Y));
            }
            else if (bFlipH == false && bFlipV == true)
            {
                LabelTransformGroup.Children.Add(new ScaleTransform(1, -1, pCenter.X, pCenter.Y));
            }
            else if (bFlipH == true && bFlipV == true)
            {
                LabelTransformGroup.Children.Add(new ScaleTransform(-1, -1, pCenter.X, pCenter.Y));
            }



            if (iAngleOfView == 180)
            {
                LabelTransformGroup.Children.Add(new RotateTransform(iAngleOfView, pCenter.X, pCenter.Y));
            }
            else if (iAngleOfView == 90)
            {
                LabelTransformGroup.Children.Add(new RotateTransform(iAngleOfView));
                cvLabel.Margin = new Thickness(this.Width, 0, 0, 0);
            }
            else if (iAngleOfView == 270)
            {
                LabelTransformGroup.Children.Add(new RotateTransform(iAngleOfView));
                cvLabel.Margin = new Thickness(0, this.Height, 0, 0);
            }

            if (LabelTransformGroup.Children.Count != 0)
            {
                cvLabel.RenderTransform = LabelTransformGroup;
            }

            Map_Canvas.Children.Add(cvLabel);
        }

        #region Scale Ruler 
        private void initialRuler(bool showScaleRuler)
        {
            switch (iAngleOfView)
            {
                default:
                case 0:
                    isReversed_RulerTick_X = false;
                    isReversed_RulerTick_Y = false;
                    break;
                case 90:
                    isReversed_RulerTick_X = true;
                    isReversed_RulerTick_Y = false;
                    break;
                case 180:
                    isReversed_RulerTick_X = true;
                    isReversed_RulerTick_Y = true;
                    break;
                case 270:
                    isReversed_RulerTick_X = false;
                    isReversed_RulerTick_Y = true;
                    break;
            }
            cvRuler = new Canvas();
            drawRuler_Top();
            drawRuler_Left();
            cvRuler.Visibility = showScaleRuler ? Visibility.Visible : Visibility.Hidden;
            Map_Canvas.Children.Add(cvRuler);
        }

        private void drawRuler_Top()
        {
            double tickFrequency = dFrequency_RulerTick;
            double tickThickness = dWidth_RulerTick;
            double textFontSize = dFontSize_RulerTick;

            int tickFrequencyOffset = isReversed_RulerTick_X ? 0 : 1;
            double tickMax = dTick_MaxX;
            double tickMaxOffset = tickMax % tickFrequency == 0 ? 0 :
                                   tickFrequency * tickFrequencyOffset - (tickMax % tickFrequency);
            tickMax += tickMaxOffset;
            double tickMin = dTick_MinX;
            double tickMinOffset = tickMin % tickFrequency == 0 ? 0 :
                                   tickMin >= 0 ? (tickFrequency * tickFrequencyOffset) - (tickMin % tickFrequency) :
                                   -(tickFrequency * (1 - tickFrequencyOffset)) - (tickMin % tickFrequency);
            tickMin += tickMinOffset;
            double tickOffset = isReversed_RulerTick_X ? -tickMaxOffset : tickMinOffset;
            CustomTickBar tickBar = new CustomTickBar()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FlowDirection = FlowDirection.LeftToRight,
                Fill = Brushes.White,
                Margin = new Thickness(Margin_Left + tickOffset, 0, 0, 0),
                Width = tickMax - tickMin,
                Height = Margin_Top / 2,
                Maximum = tickMax,
                Minimum = tickMin,
                TickFrequency = tickFrequency,
                TickThickness = tickThickness,
                TextFontSize = textFontSize,
                TickAlignment = CustomTickBar.TickAlignmentType.Bottom,
                IsReversed = isReversed_RulerTick_X
            };
            Line line = new Line()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = tickThickness,
                Stroke = Brushes.White,
                X1 = 0,
                X2 = this.Width,
                Y1 = Margin_Top / 2,
                Y2 = Margin_Top / 2,
                Margin = new Thickness(0)
            };
            cvRuler.Children.Add(line);
            cvRuler.Children.Add(tickBar);
        }

        private void drawRuler_Left()
        {
            double tickFrequency = dFrequency_RulerTick;
            double tickThickness = dWidth_RulerTick;
            double textFontSize = dFontSize_RulerTick;

            int tickFrequencyOffset = isReversed_RulerTick_Y ? 0 : 1;
            double tickMax = dTick_MaxY;
            double tickMaxOffset = tickMax % tickFrequency == 0 ? 0 :
                                   tickFrequency * tickFrequencyOffset - (tickMax % tickFrequency);
            tickMax += tickMaxOffset;
            double tickMin = dTick_MinY;
            double tickMinOffset = tickMin % tickFrequency == 0 ? 0 :
                                   tickMin >= 0 ? (tickFrequency * tickFrequencyOffset) - (tickMin % tickFrequency) :
                                   -(tickFrequency * (1 - tickFrequencyOffset)) - (tickMin % tickFrequency);
            tickMin += tickMinOffset;
            double tickOffset = isReversed_RulerTick_Y ? -tickMaxOffset : tickMinOffset;
            CustomTickBar tickBar = new CustomTickBar()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FlowDirection = FlowDirection.LeftToRight,
                Fill = Brushes.White,
                Margin = new Thickness(0, Margin_Top + tickOffset, 0, 0),
                Width = Margin_Left / 2,
                Height = tickMax - tickMin,
                Maximum = tickMax,
                Minimum = tickMin,
                TickFrequency = tickFrequency,
                TickThickness = tickThickness,
                TextFontSize = textFontSize,
                TickAlignment = CustomTickBar.TickAlignmentType.Right,
                IsReversed = isReversed_RulerTick_Y
            };
            Line line = new Line()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = tickThickness,
                Stroke = Brushes.White,
                X1 = Margin_Left / 2,
                X2 = Margin_Left / 2,
                Y1 = 0,
                Y2 = this.Height,
                Margin = new Thickness(0)
            };
            cvRuler.Children.Add(line);
            cvRuler.Children.Add(tickBar);
        }

        public void SetRulerVisible(bool showScaleRuler)
        {
            if (cvRuler == null)
                initialRuler(showScaleRuler);
            else
                cvRuler.Visibility = showScaleRuler ? Visibility.Visible : Visibility.Hidden;
        }
        #endregion Scale Ruler

        public void setAddress(List<string> addressLst)
        {
            if(addressLst != null && addressLst.Count > 0)
            {
                foreach(Menu_Maintenance.Address_PZ add in ListAddress)
                {
                    if(addressLst.Contains(add.p_ID))
                    {
                        add.SetSelect(true);
                    }
                    else
                    {
                        add.SetSelect(false);
                    }
                }
            }
        }

    }

}
