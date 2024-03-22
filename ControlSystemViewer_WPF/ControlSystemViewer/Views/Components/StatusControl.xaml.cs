using com.mirle.ibg3k0.Utility.uc;
using NLog;
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

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// StatusControl.xaml 的互動邏輯
    /// </summary>
    public partial class StatusControl : UserControl
    {
        //*******************公用參數設定*******************
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ColorMode _colormode = ColorMode.Default;
        public ColorMode ColorMode {
            get { return _colormode; }
            set
            {
                if (value == ColorMode.Dark)
                {
                    ColorModeChange();
                }
                _colormode = value;
            }
        }


        //*******************公用參數設定*******************

        public StatusControl()
        {
            InitializeComponent();
        }

        public void SetTitleName(string TitleValue, string btn1_Content = null, string btn2_Content = null, string btn3_Content = null, string btn4_Content = null)
        {
            try
            {
                lab_TitleValue.Text = TitleValue;

                if (btn1_Content == null) Button1.Visibility = Visibility.Collapsed;
                else Button1.Content = btn1_Content;

                if (btn2_Content == null) Button2.Visibility = Visibility.Collapsed;
                else Button2.Content = btn2_Content;

                if (btn3_Content == null) Button3.Visibility = Visibility.Collapsed;
                else Button3.Content = btn3_Content;

                if (btn4_Content == null) Button4.Visibility = Visibility.Collapsed;
                else Button4.Content = btn4_Content;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        //更新連線狀態(連線/斷線)
        public void SetConnectSignal(string SignalValue, StatusColorType ConnectionStatus)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    lab_SignalValue.Text = SignalValue;

                    if (ConnectionStatus == StatusColorType.Green)
                    {
                        lab_SignalValue.Background = Brushes.LimeGreen;
                        lab_SignalValue.Foreground = Brushes.Black;
                    }
                    else if (ConnectionStatus == StatusColorType.Red)
                    {
                        lab_SignalValue.Background = Brushes.Red;
                        lab_SignalValue.Foreground = Brushes.White;
                    }
                    else if (ConnectionStatus == StatusColorType.Yellow)
                    {
                        lab_SignalValue.Background = Brushes.Yellow;
                        lab_SignalValue.Foreground = Brushes.Black;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ColorModeChange()
        {
            var bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom("#003366");
            this.StackPanel1.Background = (Brush)bc.ConvertFrom("#003366");
            this.lab_TitleValue.SetResourceReference(Control.StyleProperty, "ContentTitle_18px_Dark");
        }
    }

    public enum StatusColorType
    {
        Red = 0,
        Yellow,
        Green
    }

    public enum ColorMode
    {
        Default =0,
        Dark =1
    }
}
