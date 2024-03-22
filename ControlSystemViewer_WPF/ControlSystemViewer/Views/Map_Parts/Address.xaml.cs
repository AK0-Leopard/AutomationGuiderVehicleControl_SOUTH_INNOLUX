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

namespace ControlSystemViewer.Views.Map_Parts
{
    /// <summary>
    /// Address.xaml 的互動邏輯
    /// </summary>
    public partial class Address : UserControl
    {
        public enum TypePort
        {
            NotPort = 0,
            Port,
            MonitoringPort
        }

        #region "Internal Variable"
        private WindownApplication app = null;
        private string m_sID;
        private Point m_pOriginXY;
        private int m_iAddressX;
        private int m_iAddressY;
        private double m_dAddressWidth;
        private double m_dAddressHeight;
        private Color m_clrAddressColor;
        private bool m_isCharger;
        private bool m_isCoupler;
        private TypePort m_typePort;
        private bool m_hasCarrier;
        private Rectangle rectangle;
        private Ellipse ellipse;
        private Image img_Charger;
        public Charger p_Charger = null;
        public Coupler p_Coupler = null;
        public VPORTSTATION p_Port = null;
        public EventHandler<string> AddressBeChosen;
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

        public bool p_IsCharger
        {
            get { return m_isCharger; }
            set
            {
                if (m_isCharger != value)
                {
                    m_isCharger = value;
                }
                if (value) setImgCharger();
                if (img_Charger != null)
                {
                    img_Charger.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        public bool p_IsCoupler
        {
            get { return m_isCoupler; }
            set
            {
                if (m_isCoupler != value)
                {
                    m_isCoupler = value;
                }
                if (value) setImgCharger();
                if (img_Charger != null)
                {
                    img_Charger.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        public TypePort p_TypePort
        {
            get { return m_typePort; }
            set
            {
                if (m_typePort != value)
                {
                    m_typePort = value;
                }
            }
        }

        public bool p_HasCarrier
        {
            get { return m_hasCarrier; }
            set
            {
                if (m_hasCarrier != value)
                {
                    m_hasCarrier = value;

                    //Brush brush = p_IsPort ? value ? Brushes.LimeGreen : new SolidColorBrush(Color.FromRgb(0x1E, 0x90, 0xFF)) : null;
                    //RefreshAddress(brush);
                }
            }
        }
        #endregion	/* Property */

        #region "Constructor／Destructor"
        public Address(ViewerObject.Address addr, double width, Point pointOffset)
        {
            InitializeComponent();

            app = WindownApplication.getInstance();

            if (addr != null && app != null)
            {
                m_sID = addr.ID;
                p_Port = app.ObjCacheManager.GetPortStation(addr.ID);
                m_typePort = p_Port == null ? TypePort.NotPort : p_Port.IS_MONITORING ? TypePort.MonitoringPort : TypePort.Port;
                p_Charger = app.ObjCacheManager.GetChargers()?.Where(c => c.AddressID?.Trim() == addr.ID).FirstOrDefault();
                p_Coupler = app.ObjCacheManager.GetCouplers()?.Where(c => c.AddressID?.Trim() == addr.ID).FirstOrDefault();
                m_isCharger = p_Charger != null;
                m_isCoupler = p_Coupler != null;
                m_dAddressWidth = width;
                m_dAddressHeight = width;
                m_pOriginXY = new Point(addr.X, addr.Y);
                m_iAddressX = (int)(addr.X + pointOffset.X);
                m_iAddressY = (int)(addr.Y + pointOffset.Y);
                m_clrAddressColor = Color.FromRgb(0xE1, 0xE1, 0xE1);
                m_hasCarrier = false;
                this.Tag = addr;
                initToolTip();
                drawAddress();
                setImgCharger();
            }
        }
        #endregion	/* Constructor／Destructor */


        #region "Function"
        public void RefreshAddress(System.Windows.Media.Brush brush = null)
        {
            ellipse.Fill = (p_IsCharger|| p_IsCoupler) ? getChargerBrush() :
                           brush ?? new SolidColorBrush(p_Color);
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
                listTP.Add($"AddressID: {p_ID}");
                if (p_TypePort != TypePort.NotPort) listTP.Add($"PortID: {p_Port.PORT_ID}");
                listTP.Add($"Position: ({p_OriginXY.X}, {p_OriginXY.Y})");
                if (p_IsCharger)
                {
                    listTP.Add($"ChargerID: {p_Charger.ChargerID}");
                    listTP.Add($"IsAlive: {p_Charger.IsAlive}");
                    //listTP.Add($"Status: {p_Charger.Status}");
                }
                if(p_IsCoupler)
                {
                    listTP.Add($"CouplerID: {p_Coupler.Name}");
                    //listTP.Add($"Status: {p_Coupler.Status}");
                    listTP.Add($"Status: {p_Coupler.ShowStatus}");
                    listTP.Add($"HPSafety: {p_Coupler.HPSafety}");
                }
                ToolTipService.SetInitialShowDelay(t, 0);
                t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
            }
        }

        private void drawAddress(System.Windows.Media.Brush brush = null)
        {
            if (p_TypePort != TypePort.NotPort) // 獨立外框
            {
                rectangle = new Rectangle();
                rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                rectangle.VerticalAlignment = VerticalAlignment.Center;
                rectangle.StrokeThickness = p_Width / 4;
                rectangle.Stroke = p_TypePort == TypePort.MonitoringPort ? Brushes.Orange : Brushes.Red;
                rectangle.Fill = Brushes.Transparent;
                rectangle.Width = (p_Width * 1.5) + (rectangle.StrokeThickness * 2);
                rectangle.Height = (p_Height * 1.5) + (rectangle.StrokeThickness * 2);
                rectangle.Margin = new Thickness(p_X - (rectangle.Width / 2), p_Y - (rectangle.Height / 2), 0, 0);

                AddressCanvas.Children.Add(rectangle);
            }

            ellipse = new Ellipse();
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Center;
            ellipse.StrokeThickness = 0;
            ellipse.Stroke = Brushes.Transparent;
            if (p_IsCharger || p_IsCoupler)
            {
                ellipse.Fill = getChargerBrush();
                ellipse.Width = p_Width * 1.5;
                ellipse.Height = p_Height * 1.5;
            }
            else
            {
                ellipse.Fill = brush ?? new SolidColorBrush(p_Color);
                ellipse.Width = p_Width;
                ellipse.Height = p_Height;
            }
            ellipse.Margin = new Thickness(p_X - (ellipse.Width / 2), p_Y - (ellipse.Height / 2), 0, 0);
            ellipse.Cursor = Cursors.Hand;
            ellipse.MouseLeftButtonDown += _MouseLeftButtonDown;

            AddressCanvas.Children.Add(ellipse);

            //rectangle = new Rectangle();
            //rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            //rectangle.VerticalAlignment = VerticalAlignment.Center;
            //rectangle.StrokeThickness = p_Width / 6;
            //rectangle.Stroke = brush ?? (p_IsCharger ? Brushes.Gold : new SolidColorBrush(p_Color));
            //rectangle.Fill = p_IsCharger ? Brushes.Gold :
            //            p_IsPort ? p_HasCarrier ? Brushes.Lime : new SolidColorBrush(Color.FromRgb(0x1E, 0x90, 0xFF)) :
            //            brush ?? new SolidColorBrush(p_Color);
            //rectangle.Width = p_Width;
            //rectangle.Height = p_Height;
            //rectangle.Margin = new Thickness(p_X - (p_Width / 2), p_Y - (p_Height / 2), 0, 0);
            //rectangle.Cursor = Cursors.Hand;
            //rectangle.MouseLeftButtonDown += _MouseLeftButtonDown;

            //AddressCanvas.Children.Add(rectangle);
        }

        private void setImgCharger()
        {
            if (!(p_IsCharger || p_IsCoupler) || img_Charger != null) return;

            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            img_Charger = new Image();
            img_Charger.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/SystemIcon/charging.png"));
            img_Charger.Width = p_Width * 1.5;
            img_Charger.Height = p_Height * 1.5;
            img_Charger.Margin = new Thickness(p_X - (img_Charger.Width / 2), p_Y - (img_Charger.Height / 2), 0, 0);
            img_Charger.Cursor = Cursors.Hand;
            img_Charger.MouseLeftButtonDown += _MouseLeftButtonDown;
            img_Charger.Visibility = (p_IsCharger|| p_IsCoupler) ? Visibility.Visible : Visibility.Hidden;
            AddressCanvas.Children.Add(img_Charger);
        }
        private Brush getChargerBrush()
        {
            Brush brush = Brushes.Gray;
            Coupler coupler = app.ObjCacheManager.GetCouplers().Where(c => c.AddressID.Trim() == p_ID).FirstOrDefault();
            if(coupler != null)
            {
                brush = coupler.StatusBrush ?? Brushes.Gray;
                Charger charger = app.ObjCacheManager.GetChargers().Where(c => c.ChargerID == coupler.ChargerID && c.IsAlive==true).FirstOrDefault();
                if(charger ==null)//如果Coupler對應Charger是關閉的，以Charger顯示
                {
                    brush = Brushes.Gray;
                }
            }
            return brush;
        }

        public void _MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddressBeChosen?.Invoke(this, p_ID);
        }
        #endregion	/* Function */
    }
}
