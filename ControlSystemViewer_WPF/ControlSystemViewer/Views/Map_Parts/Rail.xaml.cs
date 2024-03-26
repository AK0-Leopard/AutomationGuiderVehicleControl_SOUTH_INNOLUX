using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
using static UtilsAPI.Tool.ColorCode;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Section = ViewerObject.Section;
using Brushes = System.Windows.Media.Brushes;
using ViewerObject;
using com.mirle.ibg3k0.ohxc.wpf.App;

namespace ControlSystemViewer.Views.Map_Parts
{
    /// <summary>
    /// Rail.xaml 的互動邏輯
    /// </summary>
    public partial class Rail : UserControl
    {
        #region "Internal Variable"
        private string m_sID;
        private SecType m_eRailType;
        private int m_iRailStartX;
        private int m_iRailStartY;
        private int m_iRailEndX;
        private int m_iRailEndY;
        private double m_dRailWidth;
        private double m_dRailLength;
        private double m_dRailLength_Real;
        private Color m_clrRailColor;
        private bool m_isDisabled;
        private bool m_isReserved;
        private bool m_hasAlarm;
        private WindownApplication app = null;
        #endregion	/* Internal Variable */

        #region "Property"

        /// <summary>
        /// Object Name
        /// </summary>
        public string p_ID
        {
            get { return m_sID; }
            set
            {
                if (m_sID != value)
                {
                    m_sID = value;
                }
            }
        }

        public SecType p_RailType
        {
            get { return m_eRailType; }
            set
            {
                if (m_eRailType != value)
                {
                    m_eRailType = value;
                }
            }
        }

        public int p_RailStartX
        {
            get { return m_iRailStartX; }
            set
            {
                if (m_iRailStartX != value)
                {
                    m_iRailStartX = value;
                }
            }
        }

        public int p_RailStartY
        {
            get { return m_iRailStartY; }
            set
            {
                if (m_iRailStartY != value)
                {
                    m_iRailStartY = value;
                }
            }
        }

        public int p_RailEndX
        {
            get { return m_iRailEndX; }
            set
            {
                if (m_iRailEndX != value)
                {
                    m_iRailEndX = value;
                }
            }
        }

        public int p_RailEndY
        {
            get { return m_iRailEndY; }
            set
            {
                if (m_iRailEndY != value)
                {
                    m_iRailEndY = value;
                }
            }
        }

        public double p_RailWidth
        {
            get { return m_dRailWidth; }
            set
            {
                if (m_dRailWidth != value)
                {
                    m_dRailWidth = value;
                }
            }
        }

        public double p_RailLength
        {
            get { return m_dRailLength; }
            set
            {
                if (m_dRailLength != value)
                {
                    m_dRailLength = value;
                }
            }
        }

        public double p_RailLength_Real
        {
            get { return m_dRailLength_Real; }
            set
            {
                if (m_dRailLength_Real != value)
                {
                    m_dRailLength_Real = value;
                }
            }
        }

        public Color p_RailColor
        {
            get { return m_clrRailColor; }
            set
            {
                if (m_clrRailColor != value)
                {
                    m_clrRailColor = value;
                }
            }
        }

        public bool p_IsDisabled
        {
            get { return m_isDisabled; }
            set
            {
                if (m_isDisabled != value)
                {
                    m_isDisabled = value;
                }
            }
        }

        public bool p_IsReserved
        {
            get { return m_isReserved; }
            set
            {
                if (m_isReserved != value)
                {
                    m_isReserved = value;
                    setReserve();
                }
            }
        }

        public bool p_HasAlarm
        {
            get { return m_hasAlarm; }
            set
            {
                if (m_hasAlarm != value)
                {
                    m_hasAlarm = value;
                    setAlarm();
                }
            }
        }
        
        MenuItem mi_sectionEnable, mi_sectionDisable;

        #endregion	/* Property */

        #region "Constructor／Destructor"
        public Rail(Section sec, double width, Point pointOffset, bool isDisabled = false)
        {
            InitializeComponent();

          
            if (sec != null)
            {
                m_sID = sec.ID?.Trim() ?? "";
                m_eRailType = sec.Type;
                m_dRailWidth = width > 0 ? width : 0.25;
                m_iRailStartX = (int)(sec.StartAddress.X + pointOffset.X);
                m_iRailStartY = (int)(sec.StartAddress.Y + pointOffset.Y);
                m_iRailEndX = (int)(sec.EndAddress.X + pointOffset.X);
                m_iRailEndY = (int)(sec.EndAddress.Y + pointOffset.Y);
                m_dRailLength_Real = -1; // this length is the length in real environment, not length in map
                double railLength = getRailLenght();
                m_dRailLength = railLength > 0 ? railLength : 0.25;
                m_clrRailColor = Color.FromRgb(0x1E, 0x90, 0xFF);
                m_isDisabled = isDisabled;
                m_isReserved = false;
                m_hasAlarm = false;
                this.Tag = sec;
                this.p_IsDisabled = sec.enable ? false : true;

                setToolTip();
                drawRail();
                initReserve();
                initAlarm();

                if (p_IsDisabled)
                {
                    mi_sectionEnable.IsEnabled = true;
                    mi_sectionDisable.IsEnabled = false;
                }
                else
                {
                    mi_sectionEnable.IsEnabled = false;
                    mi_sectionDisable.IsEnabled = true;
                }
            }
            app = WindownApplication.getInstance();
        }
        #endregion	/* Constructor／Destructor */

        #region "Function"
        public void RefreshRail(System.Windows.Media.Brush brush = null)
        {
            //RailCanvas.Children.Clear();
            //drawRail(brush);
            // 重新繪製太慢，僅變更顏色
            try
            {
                if (RailCanvas.Children?.Count > 0)
                {
                    // 若未指定 且 禁用   => 灰色
                    // 若未指定 且 非禁用 => 預設顏色
                    // 若有指定           => 指定顏色
                    brush = brush == null ? p_IsDisabled ? Brushes.DimGray : new SolidColorBrush(p_RailColor) : brush;

                    if(p_IsDisabled)
                    {
                        mi_sectionEnable.IsEnabled = true;
                        mi_sectionDisable.IsEnabled = false;
                    }
                    else
                    {
                        mi_sectionEnable.IsEnabled = false;
                        mi_sectionDisable.IsEnabled = true;
                    }

                    Line line = RailCanvas.Children[0] as Line;
                    if (line != null)
                    {
                        line.Stroke = brush;
                        return;
                    }
                    Path path = RailCanvas.Children[0] as Path; // Curve
                    if (path != null)
                    {
                        path.Stroke = brush;
                        return;
                    }
                }
            }
            catch 
            { }
        }

        #region Reserve
        private void initReserve()
        {
            drawRail(Brushes.Lime); // For Reserve Display
            setReserve();
        }
        private void setReserve()
        {
            try
            {
                if (RailCanvas.Children?.Count > 1)
                {
                    Line line = RailCanvas.Children[1] as Line;
                    if (line != null)
                    {
                        line.Visibility = p_IsReserved ? Visibility.Visible : Visibility.Hidden;
                        return;
                    }
                    Path path = RailCanvas.Children[1] as Path; // Curve
                    if (path != null)
                    {
                        path.Visibility = p_IsReserved ? Visibility.Visible : Visibility.Hidden;
                        return;
                    }
                }
            }
            catch 
            { }
        }
        #endregion Reserve

        #region Alarm
        private void initAlarm()
        {
            drawRail(Brushes.Red); // For Alarm Display
            setAlarm();
        }
        private void setAlarm()
        {
            try
            {
                if (RailCanvas.Children?.Count > 2)
                {
                    Line line = RailCanvas.Children[2] as Line;
                    if (line != null)
                    {
                        line.Visibility = p_HasAlarm ? Visibility.Visible : Visibility.Hidden;
                        return;
                    }
                    Path path = RailCanvas.Children[2] as Path; // Curve
                    if (path != null)
                    {
                        path.Visibility = p_HasAlarm ? Visibility.Visible : Visibility.Hidden;
                        return;
                    }
                }
            }
            catch
            { }
        }
        #endregion Alarm

        private double getRailLenght()
        {
            return Math.Sqrt(Math.Pow(m_iRailStartX - m_iRailEndX, 2) + Math.Pow(m_iRailStartY - m_iRailEndY, 2));
        }

        private void setToolTip()
        {
            List<string> listTP = new List<string>();
            //listTP.Add($"Rail No: {p_ID}");
            //listTP.Add($"Size: {p_RailLength_Real}");
            listTP.Add($"Section No: {p_ID}");
             listTP.Add($"Size: {p_RailLength}");
             var t = new ToolTip();
            ToolTipService.SetInitialShowDelay(t, 0);
            t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
            RailCanvas.ToolTip = t;
        }

        private void drawRail(System.Windows.Media.Brush brush = null)
        {
            switch (p_RailType)
            {
                case SecType.StraightLine:
                    drawLine(brush);
                    break;
                case SecType.Curve_0to90:
                case SecType.Curve_90to180:
                case SecType.Curve_180to270:
                case SecType.Curve_270to360:
                case SecType.Curve_90to0:
                case SecType.Curve_180to90:
                case SecType.Curve_270to180:
                case SecType.Curve_360to270:
                    drawCurve(brush);
                    break;
                case SecType.Curve_0to180:
                case SecType.Curve_90to270:
                case SecType.Curve_180to0:
                case SecType.Curve_270to90:
                    drawUTurn(brush);
                    break;
            }
            mi_sectionEnable = new MenuItem();
            mi_sectionEnable.Header = $"Enable this section \"" + p_ID + "\" ?";
            mi_sectionEnable.Click += segControl_enable;

            mi_sectionDisable = new MenuItem();
            mi_sectionDisable.Header = $"Disable this section \"" + p_ID + "\" ?";
            mi_sectionDisable.Click += segControl_disable;


            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(mi_sectionEnable);
            contextMenu.Items.Add(mi_sectionDisable);
            RailCanvas.ContextMenu = contextMenu;
        }
        private void drawLine(System.Windows.Media.Brush brush)
        {
            Line line = new Line();
            line.StrokeThickness = p_RailWidth;
            line.Stroke = brush ?? new SolidColorBrush(p_RailColor);
            if (p_RailType == SecType.StraightLine)
            {
                line.HorizontalAlignment = HorizontalAlignment.Left;
                line.VerticalAlignment = VerticalAlignment.Center;
                line.X1 = p_RailStartX;
                line.X2 = p_RailEndX;
                line.Y1 = p_RailStartY;
                line.Y2 = p_RailEndY;
            }
            RailCanvas.Children.Add(line);
        }
        private void drawCurve(System.Windows.Media.Brush brush)
        {
            adjustStartEnd();

            Path path = new Path();
            path.StrokeThickness = p_RailWidth;
            double margin_X = 0;
            double margin_Y = 0;
            path.Stroke = brush ?? new SolidColorBrush(p_RailColor);
            path.Fill = System.Windows.Media.Brushes.Transparent;
            PathGeometry pg = new PathGeometry();
            PathFigureCollection pfc = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            ArcSegment arcs = new ArcSegment();
            double cordLengthX = Math.Abs(p_RailEndX - p_RailStartX);
            double cordLengthY = Math.Abs(p_RailEndY - p_RailStartY);
            if (p_RailType == SecType.Curve_0to90)
            {
                pf.StartPoint = new System.Windows.Point(0, 0);// starting cordinates of arcs
                arcs.Point = new System.Windows.Point(cordLengthX, cordLengthY);   // ending cordinates of arcs
            }
            else if (p_RailType == SecType.Curve_90to180)
            {
                pf.StartPoint = new System.Windows.Point(0, cordLengthY);// starting cordinates of arcs
                arcs.Point = new System.Windows.Point(cordLengthX, 0);   // ending cordinates of arcs
                margin_Y = -cordLengthY; // 左下移至左上
            }
            else if (p_RailType == SecType.Curve_180to270)
            {
                pf.StartPoint = new System.Windows.Point(cordLengthX, cordLengthY);// starting cordinates of arcs
                arcs.Point = new System.Windows.Point(0, 0);   // ending cordinates of arcs
            }
            else if (p_RailType == SecType.Curve_270to360)
            {
                pf.StartPoint = new System.Windows.Point(cordLengthX, 0);// starting cordinates of arcs
                arcs.Point = new System.Windows.Point(0, cordLengthY);   // ending cordinates of arcs
                margin_Y = -cordLengthY; // 左下移至左上
            }
            else if(p_RailType == SecType.Curve_90to0)
            {
                arcs.Point = new System.Windows.Point(0, 0);// starting cordinates of arcs
                pf.StartPoint = new System.Windows.Point(-cordLengthX, cordLengthY);   // ending cordinates of arcs
            }
            else if (p_RailType == SecType.Curve_180to90)
            {
                arcs.Point = new System.Windows.Point(0, cordLengthY);// starting cordinates of arcs
                pf.StartPoint = new System.Windows.Point(cordLengthX, cordLengthY * 2);   // ending cordinates of arcs
                margin_Y = -cordLengthY;
            }
            else if (p_RailType == SecType.Curve_270to180)
            {
                arcs.Point = new System.Windows.Point(0, cordLengthY);// starting cordinates of arcs
                pf.StartPoint = new System.Windows.Point(cordLengthX, 0);   // ending cordinates of arcs
                margin_Y = -cordLengthY;
            }
            else if (p_RailType == SecType.Curve_360to270)
            {
                arcs.Point = new System.Windows.Point(0, cordLengthY);// starting cordinates of arcs
                pf.StartPoint = new System.Windows.Point(-cordLengthX, 0);   // ending cordinates of arcs
                margin_Y = -cordLengthY;
            }
            arcs.Size = new System.Windows.Size(cordLengthX, cordLengthY);
            arcs.IsLargeArc = false;
            arcs.SweepDirection = SweepDirection.Clockwise;
            pf.Segments.Add(arcs);
            pfc.Add(pf);
            pg.Figures = pfc;
            path.Data = pg;
            path.Margin = new Thickness(p_RailStartX + margin_X, p_RailStartY + margin_Y, 0, 0);
            RailCanvas.Children.Add(path);
        }
        private void drawUTurn(System.Windows.Media.Brush brush)
        {
            Point start, control, end;
            Path path = new Path();
            PathGeometry pg = new PathGeometry();
            PathFigureCollection pfc = new PathFigureCollection();
            PathFigure pf = new PathFigure();
            PathSegmentCollection pc = new PathSegmentCollection();
            ArcSegment arcs = new ArcSegment();
            int commonX, commonY; //這個參數是預防兩個address同時不同X也不同Y，畫起來歪七扭八

            path.StrokeThickness = p_RailWidth;
            path.Stroke = brush ?? new SolidColorBrush(p_RailColor);
            //path.Stroke = Color.;
            path.Fill = System.Windows.Media.Brushes.Transparent;

            if (p_RailType == SecType.Curve_0to180)
            {
                commonY = (m_iRailStartY > m_iRailEndY) ? m_iRailEndY : m_iRailStartY;
                int height = Math.Abs(m_iRailStartX - m_iRailEndX);
                start = new Point(m_iRailStartX, commonY);
                control = new Point((m_iRailStartX+ m_iRailEndX)/2, commonY - height);
                end = new Point(m_iRailEndX, commonY);

                pf.StartPoint = start;
                pc.Add(new QuadraticBezierSegment(control, end, true));
                pf.Segments = pc;
                pfc.Add(pf);
                pg.Figures = pfc;
                path.Data = pg;
                path.Margin = new Thickness(0, 0, 0, 0);
                RailCanvas.Children.Add(path);
            }
            else if(p_RailType == SecType.Curve_180to0)
            {
                commonY = (m_iRailStartY < m_iRailEndY) ? m_iRailEndY : m_iRailStartY;
                int height = Math.Abs(m_iRailStartX - m_iRailEndX);
                start = new Point(m_iRailStartX, commonY);
                control = new Point((m_iRailStartX + m_iRailEndX) / 2, commonY + height);
                end = new Point(m_iRailEndX, commonY);

                pf.StartPoint = start;
                pc.Add(new QuadraticBezierSegment(control, end, true));
                pf.Segments = pc;
                pfc.Add(pf);
                pg.Figures = pfc;
                path.Data = pg;
                path.Margin = new Thickness(0, 0, 0, 0);
                RailCanvas.Children.Add(path);
            }
            else if(p_RailType == SecType.Curve_90to270)
            {
                commonX = (m_iRailStartX < m_iRailEndX) ? m_iRailEndX : m_iRailStartX;
                int width = Math.Abs(m_iRailStartY - m_iRailEndY);
                start = new Point(commonX, m_iRailStartY);
                control = new Point(commonX - width, (m_iRailStartY + m_iRailEndY)/2);
                end = new Point(commonX, m_iRailEndY);

                pf.StartPoint = start;
                pc.Add(new QuadraticBezierSegment(control, end, true));
                pf.Segments = pc;
                pfc.Add(pf);
                pg.Figures = pfc;
                path.Data = pg;
                path.Margin = new Thickness(0, 0, 0, 0);
                RailCanvas.Children.Add(path);
            }
            else if (p_RailType == SecType.Curve_270to90)
            {
                commonX = (m_iRailStartX > m_iRailEndX) ? m_iRailEndX : m_iRailStartX;
                int width = Math.Abs(m_iRailStartY - m_iRailEndY);
                start = new Point(commonX, m_iRailStartY);
                control = new Point(commonX + width, (m_iRailStartY + m_iRailEndY) / 2);
                end = new Point(commonX, m_iRailEndY);

                pf.StartPoint = start;
                pc.Add(new QuadraticBezierSegment(control, end, true));
                pf.Segments = pc;
                pfc.Add(pf);
                pg.Figures = pfc;
                path.Data = pg;
                path.Margin = new Thickness(0, 0, 0, 0);
                RailCanvas.Children.Add(path);
            }
        }
        private void adjustStartEnd()
        {
            switch (p_RailType)
            {
                case SecType.Curve_0to90:
                case SecType.Curve_180to270:
                    // 左上為起點
                    if (p_RailStartX > p_RailEndX && p_RailStartY > p_RailEndY)
                        swapStartEnd();
                    break;
                case SecType.Curve_90to180:
                case SecType.Curve_270to360:
                    // 左下為起點
                    if (p_RailStartX > p_RailEndX && p_RailStartY < p_RailEndY)
                        swapStartEnd();
                    break;
            }
        }
        private void swapStartEnd()
        {
            int tmp = p_RailStartX;
            p_RailStartX = p_RailEndX;
            p_RailEndX = tmp;
            tmp = p_RailStartY;
            p_RailStartY = p_RailEndY;
            p_RailEndY = tmp;
        }
        #endregion	/* Function */

        private async void _Reset_Click(object sender, RoutedEventArgs e)
        {
            //app.OperationHistoryBLL.addOperationHis(app.LoginUserID, "TrackSwitch", "", ((MenuItem)sender).Header.ToString());

            //string id = p_ID.StartsWith("R") ? p_ID.Substring(1) : p_ID;
            //await Task.Run(() => app.ObjCacheManager.resetTrackSwitch_grpc(id));
        }

        private async void segControl_enable(object sender, RoutedEventArgs e)
        {
            bool is_success = app.ObjCacheManager.segControl(p_ID, true);
            app.OperationHistoryBLL.
                addOperationHis(app.LoginUserID,
                                this.GetType().Name,
                                $"Excute section id:{p_ID} enable, is success:{is_success}");
        }
        private async void segControl_disable(object sender, RoutedEventArgs e)
        {
            bool is_success = app.ObjCacheManager.segControl(p_ID, false);
            app.OperationHistoryBLL.
                addOperationHis(app.LoginUserID,
                                this.GetType().Name,
                                $"Excute section id:{p_ID} disable, is success:{is_success}");
        }
    }
}
