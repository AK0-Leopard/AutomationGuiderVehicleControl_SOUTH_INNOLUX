using com.mirle.ibg3k0.ohxc.wpf.App;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// Address.xaml 的互動邏輯
    /// </summary>
    public partial class Address_PZ : UserControl
    {
        #region "Internal Variable"
        private WindownApplication app = null;
        private string m_sID;
        private Point m_pOriginXY;
        private int m_iAddressX;
        private int m_iAddressY;
        private double m_dAddressWidth;
        private double m_dAddressHeight;
        private Color m_clrAddressColor;
        private Ellipse ellipse;
        public EventHandler<string> AddressBeChosen;
        private bool m_isSelect = false;
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

        public Point p_OriginXY
        {
            get { return m_pOriginXY; }
            set
            {
                if (m_pOriginXY != value)
                {
                    m_pOriginXY = value;
                }
            }
        }

        public int p_X
        {
            get { return m_iAddressX; }
            set
            {
                if (m_iAddressX != value)
                {
                    m_iAddressX = value;
                }
            }
        }

        public int p_Y
        {
            get { return m_iAddressY; }
            set
            {
                if (m_iAddressY != value)
                {
                    m_iAddressY = value;
                }
            }
        }

        public double p_Width
        {
            get { return m_dAddressWidth; }
            set
            {
                if (m_dAddressWidth != value)
                {
                    m_dAddressWidth = value;
                }
            }
        }

        public double p_Height
        {
            get { return m_dAddressHeight; }
            set
            {
                if (m_dAddressHeight != value)
                {
                    m_dAddressHeight = value;
                }
            }
        }

        public Color p_Color
        {
            get { return m_clrAddressColor; }
            set
            {
                if (m_clrAddressColor != value)
                {
                    m_clrAddressColor = value;
                    Background = new SolidColorBrush(m_clrAddressColor);
                }
            }
        }

        public bool p_IsSelect
        {
            get { return m_isSelect; }
            set
            {
                if (m_isSelect != value)
                {
                    m_isSelect = value;
                }
            }
        }
        #endregion	/* Property */

        #region "Constructor／Destructor"
        public Address_PZ(ViewerObject.Address addr, double width, Point pointOffset)
        {
            InitializeComponent();

            app = WindownApplication.getInstance();

            if (addr != null && app != null)
            {
                m_sID = addr.ID;
                m_dAddressWidth = width;
                m_dAddressHeight = width;
                m_pOriginXY = new Point(addr.X, addr.Y);
                m_iAddressX = (int)(addr.X + pointOffset.X);
                m_iAddressY = (int)(addr.Y + pointOffset.Y);
                m_clrAddressColor = Color.FromRgb(0xE1, 0xE1, 0xE1);
                this.Tag = addr;
                initToolTip();
                drawAddress();
            }
        }
        #endregion	/* Constructor／Destructor */


        #region "Function"
        public void RefreshAddress(System.Windows.Media.Brush brush = null)
        {
            ellipse.Fill = brush ?? new SolidColorBrush(p_Color);
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

        public void SetSelect(bool select)
        {
            if(select)
            {
                m_isSelect = select;
                RefreshAddress(Brushes.Red);
            }
            else
            {
                m_isSelect = false;
                RefreshAddress(null);
            }

            setToolTip();
        }

        private void setToolTip()
        {
            ToolTip t = this.ToolTip as ToolTip;
            if (t != null)
            {
                List<string> listTP = new List<string>();
                listTP.Add($"AddressID: {p_ID}");
                listTP.Add($"Position: ({p_OriginXY.X}, {p_OriginXY.Y})");
                ToolTipService.SetInitialShowDelay(t, 0);
                t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
            }
        }

        private void drawAddress(System.Windows.Media.Brush brush = null)
        {
            ellipse = new Ellipse();
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Center;
            ellipse.StrokeThickness = 0;
            ellipse.Stroke = Brushes.Transparent;
            ellipse.Fill = brush ?? new SolidColorBrush(p_Color);
            ellipse.Width = p_Width;
            ellipse.Height = p_Height;
            ellipse.Margin = new Thickness(p_X - (ellipse.Width / 2), p_Y - (ellipse.Height / 2), 0, 0);
            ellipse.Cursor = Cursors.Hand;
            ellipse.MouseLeftButtonDown += _MouseLeftButtonDown;

            AddressCanvas.Children.Add(ellipse);
        }

        public void _MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!m_isSelect)
            {
                m_isSelect = true;
                RefreshAddress(Brushes.Red);
            }
            else
            {
                m_isSelect = false;
                RefreshAddress(null);
            }

            AddressBeChosen?.Invoke(this, p_ID);
        }
        #endregion	/* Function */
    }
}
