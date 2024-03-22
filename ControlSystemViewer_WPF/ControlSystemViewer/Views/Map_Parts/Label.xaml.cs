using System;
using System.Collections.Generic;
using System.Globalization;
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
using static UtilsAPI.Tool.ColorCode;

namespace ControlSystemViewer.Views.Map_Parts
{
    /// <summary>
    /// Label.xaml 的互動邏輯
    /// </summary>
    public partial class Label : UserControl
    {
        #region "Internal Variable"
        private string m_sID;
        private string m_sText;
        private Color m_cTextColor;
        private double m_dX;
        private double m_dY;
        #endregion	/* Internal Variable */

        #region "Property"

        public EventHandler AddressSelected;

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
        public string p_Text
        {
            get { return m_sText; }
            set
            {
                if (m_sText != value)
                {
                    m_sText = value;
                }
            }
        }

        public Color p_TextColor
        {
            get { return m_cTextColor; }
            set
            {
                if (m_cTextColor != value)
                {
                    m_cTextColor = value;
                }
            }
        }

        public double p_X
        {
            get { return m_dX; }
            set
            {
                if (m_dX != value)
                {
                    m_dX = value;
                }
            }
        }

        public double p_Y
        {
            get { return m_dY; }
            set
            {
                if (m_dY != value)
                {
                    m_dY = value;
                }
            }
        }

        public double p_Width
        {
            get { return this.Width; }
            set
            {
                if (this.Width != value)
                {
                    this.Width = value;
                }
            }
        }

        public double p_Height
        {
            get { return this.Height; }
            set
            {
                if (this.Height != value)
                {
                    this.Height = value;
                }
            }
        }
        #endregion	/* Property */

        #region "Constructor／Destructor"
        public Label(ViewerObject.Label lbl, Point pointOffset, bool isVertical)
        {
            InitializeComponent();

            p_ID = lbl.ID;
            p_Text = getText(lbl.Text, isVertical);
            p_TextColor = ConvertStringColorToColor(lbl.TextColor);
            double x = lbl.X2 > lbl.X1 ? lbl.X1 : lbl.X2;
            double y = lbl.Y2 > lbl.Y1 ? lbl.Y1 : lbl.Y2;
            p_X = x + pointOffset.X;
            p_Y = y + pointOffset.Y;
            p_Width = Math.Abs(lbl.X2 - lbl.X1);
            p_Height = Math.Abs(lbl.Y2 - lbl.Y1);

            //this.Background = Brushes.Purple;
            //this.Margin = new Thickness(p_X, p_Y, 0, 0);

            Image img = null;
            if (lbl.Text == "arrow.png") // 顯示箭頭圖示
            {
                img = new Image();
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                img.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new System.Drawing.Bitmap(sPath + "\\Resources\\arrow.png"));
                //img.HorizontalAlignment = HorizontalAlignment.Center;
                //img.VerticalAlignment = VerticalAlignment.Center;
                img.Stretch = Stretch.Fill;
                img.Width = p_Width;
                img.Height = p_Height;
                // 原圖向左 <=
                // 根據lbl範圍調整方向
                double angle = p_Width >= p_Height ?
                               lbl.X2 > lbl.X1 ? 180 : 0 :
                               lbl.Y2 > lbl.Y1 ? 270 : 90;
                img.RenderTransform = new RotateTransform(angle, 0, 0);

                double rotateOffsetX = 0;
                double rotateOffsetY = 0;
                switch (angle)
                {
                    case 90:
                        img.Width = p_Height;
                        img.Height = p_Width;
                        rotateOffsetX = img.Height;
                        break;
                    case 180:
                        rotateOffsetX = img.Width;
                        rotateOffsetY = img.Height;
                        break;
                    case 270:
                        img.Width = p_Height;
                        img.Height = p_Width;
                        rotateOffsetY = img.Width;
                        break;
                }
                img.Margin = new Thickness(p_X + rotateOffsetX, p_Y + rotateOffsetY, 0, 0);
            }

            drawLabel(isVertical, img);
        }
        #endregion /* "Constructor／Destructor" */

        private string getText(string text, bool isVertical)
        {
            string result = text;
            if (isVertical)
            {
                result = "";
                for (int i = 0; i < text.Length; i++)
                {
                    result += text[i];
                    if (i < text.Length - 1)
                        result += '\n';
                }
            }
            return result;
        }

        private void drawLabel(bool isVertical, Image img = null)
        {
            if (img != null)
            {
                Canvas_Label.Children.Add(img);
                return;
            }

            if (isVertical)
            {
                double dTmp = p_Width;
                p_Width = p_Height;
                p_Height = dTmp;
            }

            double font_size = getBestFontSize(p_Text);

            System.Windows.Controls.Label label = new System.Windows.Controls.Label()
            {
                Margin = new Thickness(p_X, p_Y, 0, 0),
                Width = this.Width,
                Height = this.Height,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Content = p_Text,
                FontFamily = new FontFamily("Consolas"),
                FontSize = font_size,
                Foreground = new SolidColorBrush(p_TextColor)
            };
            Canvas_Label.Children.Add(label);
        }

        private double getBestFontSize(string text)
        {
            double best_size = 0.25;
            text = text ?? " ";
            FormattedText ft = new FormattedText(text,
                                                 CultureInfo.GetCultureInfo("en-us"),
                                                 FlowDirection.LeftToRight,
                                                 new Typeface("Consolas"),
                                                 best_size,
                                                 Brushes.White);

            double magn = 128; //倍率參數 用以調整best_size
            int i = 1;
            while (magn > 0.24) //0.25以上
            {
                ft.SetFontSize(best_size + (magn * i));
                if (ft.Width > this.Width || ft.Height > this.Height)
                {
                    best_size += magn * (i - 1);
                    magn /= 2;
                    i = 1;
                    continue;
                }
                i++;
            }
            return best_size;
        }
    }
}
