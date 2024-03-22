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
    /// Shelf.xaml 的互動邏輯
    /// </summary>
    public partial class Shelf : UserControl
    {
        #region "Internal Variable"
        private ViewerObject.Shelf vo_shelf = null;
        private double dWidth;
        private Point mapOffset;
        private Rectangle rectangle;
        #endregion	/* Internal Variable */

        #region "Property"
        public string p_ID => vo_shelf?.SHELF_ID ?? "";

        public int p_X => Convert.ToInt32((vo_shelf?.X ?? 0) + mapOffset.X);

        public int p_Y => Convert.ToInt32((vo_shelf?.Y ?? 0) + mapOffset.Y);

        public double p_Width => dWidth;

        public Brush p_Brush_Fill
        {
            get
            {
                bool enable = vo_shelf?.ENABLE ?? false;
                switch (vo_shelf?.SHELF_STATUS)
                {
                    case ViewerObject.Definition.ShelfStatus.Alternate:
                        return enable ? Brushes.Yellow : Brushes.Red;
                    case ViewerObject.Definition.ShelfStatus.PreOut:
                        return Brushes.Purple;
                    case ViewerObject.Definition.ShelfStatus.PreIn:
                        return Brushes.Pink;
                    case ViewerObject.Definition.ShelfStatus.Stored:
                        return enable ? Brushes.Lime : Brushes.Red;
                    case ViewerObject.Definition.ShelfStatus.Empty:
                        return Brushes.Transparent;
                    case ViewerObject.Definition.ShelfStatus.Default:
                    default:
                        return Brushes.Gray;
                }
            }
        }
        public Brush p_Brush_Stroke => (vo_shelf?.ENABLE ?? false) ? Brushes.LightGray : Brushes.Red;
        #endregion	/* Property */

        #region "Constructor／Destructor"
        public Shelf(ViewerObject.Shelf shelf, double width, Point _mapOffset)
        {
            InitializeComponent();

            vo_shelf = shelf;
            vo_shelf.ShelfStatusChanged += _ShelfStatusChanged;
            dWidth = width;
            mapOffset = _mapOffset;

            initToolTip();
            drawShelf();
        }
        #endregion	/* Constructor／Destructor */

        #region "Function"
        private void drawShelf()
        {
            rectangle = new Rectangle();
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Center;
            rectangle.StrokeThickness = p_Width / 6;
            rectangle.Stroke = p_Brush_Stroke;
            rectangle.Fill = p_Brush_Fill;
            rectangle.Width = p_Width;
            rectangle.Height = p_Width;
            rectangle.Margin = new Thickness(p_X - (rectangle.Width / 2), p_Y - (rectangle.Height / 2), 0, 0);

            ShelfCanvas.Children.Add(rectangle);
        }

        private void _ShelfStatusChanged(object sender, EventArgs e)
        {
            Adapter.Invoke((obj) =>
            {
                RefreshShelf();
            }, null);
        }

        public void RefreshShelf()
        {
            if (rectangle != null)
            {
                rectangle.Stroke = p_Brush_Stroke;
                rectangle.Fill = p_Brush_Fill;
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
                listTP.Add($"ShelfID: {p_ID}");
                listTP.Add($"ZoneID: {vo_shelf?.ZONE_ID}");
                listTP.Add($"Enable: {vo_shelf?.ENABLE}");
                listTP.Add($"Status: {vo_shelf?.SHELF_STATUS}");
                if (!string.IsNullOrWhiteSpace(vo_shelf?.BOX_ID))
                    listTP.Add($"BoxID: {vo_shelf?.BOX_ID}");
                if (!string.IsNullOrWhiteSpace(vo_shelf?.CST_ID))
                    listTP.Add($"CstID: {vo_shelf?.CST_ID}");
                ToolTipService.SetInitialShowDelay(t, 0);
                t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
            }
        }
        #endregion	/* Function */
    }
}
