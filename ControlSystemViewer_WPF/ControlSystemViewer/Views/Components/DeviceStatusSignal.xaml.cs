using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// DeviceStatusSignal.xaml 的互動邏輯
    /// </summary>
    public partial class DeviceStatusSignal : UserControl
    {
        //*******************公用參數設定*******************
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private BitmapImage img_ON = null;
        private BitmapImage img_OFF = null;
        //*******************公用參數設定*******************

        public DeviceStatusSignal()
        {
            InitializeComponent();

            try
            {
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                img_ON = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/icon_Link_ON.png"));
                img_OFF = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/icon_Link_ON.png"));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetConnStatus(string title, bool isConnected)
        {
            try
            {
                TitleName.Content = title;
                LinkSignal.Source = isConnected ? img_ON : img_OFF;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
