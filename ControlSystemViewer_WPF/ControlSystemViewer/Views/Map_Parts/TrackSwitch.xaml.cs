using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using System;
using System.Collections.Generic;
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

namespace ControlSystemViewer.Views.Map_Parts
{
    /// <summary>
    /// TrackSwitch.xaml 的互動邏輯
    /// </summary>
    public partial class TrackSwitch : UserControl
    {
        #region "Internal Variable"
        private WindownApplication app = null;
        private ViewerObject.TrackSwitch vo_trackSwitch = null;
        private double dWidth;
        private Point mapOffset;
        private Polygon polygon;
        #endregion	/* Internal Variable */

        #region "Property"
        public string p_ID => vo_trackSwitch?.ID ?? "";

        public int p_X => Convert.ToInt32((vo_trackSwitch?.X ?? 0) + mapOffset.X);

        public int p_Y => Convert.ToInt32((vo_trackSwitch?.Y ?? 0) + mapOffset.Y);

        public double p_Width => dWidth;

        public Brush p_Brush
        {
            get
            {
                switch (vo_trackSwitch?.Status)
                {
                    case ViewerObject.TrackSwitchStatus.Alarm:
                        return Brushes.Red;
                    default:
                        return Brushes.GreenYellow;
                }
            }
        }

        public EventHandler<string> StatusChanged;
        #endregion	/* Property */

        #region "Constructor／Destructor"
        public TrackSwitch(ViewerObject.TrackSwitch trackSwitch, double width, Point _mapOffset)
        {
            InitializeComponent();

            app = WindownApplication.getInstance();

            vo_trackSwitch = trackSwitch;
            vo_trackSwitch.StatusChanged += _StatusChanged;
            dWidth = width;
            mapOffset = _mapOffset;

            initToolTip();
            drawTrackSwitch();

            if (vo_trackSwitch.Status == ViewerObject.TrackSwitchStatus.Alarm)
                StatusChanged?.Invoke(this, p_ID);
        }
        #endregion	/* Constructor／Destructor */

        #region "Function"
        private void drawTrackSwitch()
        {
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(new Point(0, -Math.Sqrt(3) / 2));
            myPointCollection.Add(new Point(-1, Math.Sqrt(3) / 2));
            myPointCollection.Add(new Point(1, Math.Sqrt(3) / 2));

            polygon = new Polygon();
            polygon.Points = myPointCollection;
            //polygon.RenderTransform = new RotateTransform(-iAngleOfView, 0, 0);
            polygon.Width = dWidth;
            polygon.Height = dWidth;
            polygon.Stretch = Stretch.Fill;
            polygon.Stroke = Brushes.Black;
            polygon.StrokeThickness = polygon.Width / 10;
            polygon.Fill = p_Brush;
            polygon.Margin = new Thickness(vo_trackSwitch.X + mapOffset.X - (polygon.Width / 2), vo_trackSwitch.Y + mapOffset.Y - (polygon.Height / 2), 0, 0);

            var mi_Reset = new MenuItem();
            mi_Reset.Header = $"Reset {p_ID}";
            mi_Reset.Click += _Reset_Click;
            var contextMenu = new ContextMenu();
            contextMenu.Items.Add(mi_Reset);
            polygon.ContextMenu = contextMenu;

            TrackSwitchCanvas.Children.Add(polygon);
        }

        private void _StatusChanged(object sender, EventArgs e)
        {
            StatusChanged?.Invoke(this, p_ID);

            Adapter.Invoke((obj) =>
            {
                RefreshTrackSwitch();
            }, null);
        }

        public void RefreshTrackSwitch()
        {
            if (polygon != null)
            {
                polygon.Fill = p_Brush;
            }
        }

        private void initToolTip()
        {
            var t = new ToolTip();
            ToolTipService.SetInitialShowDelay(t, 0);
            t.Opened += ToolTip_Opened;
            this.ToolTip = t;
        }
        private void ToolTip_Opened(object sender, RoutedEventArgs e)
        {
            setToolTip();
        }
        private void setToolTip()
        {
            ToolTip t = this.ToolTip as ToolTip;
            if (t != null)
            {
                List<string> listTP = new List<string>();
                listTP.Add($"ID: {p_ID}");
                listTP.Add($"IsAlive: {vo_trackSwitch?.IsAlive}");
                listTP.Add($"Status: {vo_trackSwitch?.Status}");
                listTP.Add($"Alarm Code: {vo_trackSwitch?.AlarmCode}");
                listTP.Add($"Dir: {vo_trackSwitch?.Dir}");
                listTP.Add($"Track1: {vo_trackSwitch?.Track1.ID}");
                listTP.Add($"Track2: {vo_trackSwitch?.Track2.ID}");
                listTP.Add($"AutoChangeToDefaultDir: {vo_trackSwitch?.AutoChangeToDefaultDir}");
                listTP.Add($"DefaultDir: {vo_trackSwitch?.DefaultDir}");
                ToolTipService.SetInitialShowDelay(t, 0);
                t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
            }
        }
        #endregion	/* Function */

        private async void _Reset_Click(object sender, RoutedEventArgs e)
        {
            app.OperationHistoryBLL.addOperationHis(app.LoginUserID, "TrackSwitch", "", ((MenuItem)sender).Header.ToString());

            string id = p_ID.StartsWith("R") ? p_ID.Substring(1) : p_ID;
            await Task.Run(() => app.ObjCacheManager.resetTrackSwitch_grpc(id));
        }
    }
}
