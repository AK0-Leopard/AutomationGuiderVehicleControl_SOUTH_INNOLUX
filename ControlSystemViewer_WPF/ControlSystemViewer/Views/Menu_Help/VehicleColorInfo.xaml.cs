using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.Utility.uc;
using NLog;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace ControlSystemViewer.Views.Menu_Help
{
    /// <summary>
    /// About.xaml 的互動邏輯
    /// </summary>
    public partial class VehicleColorInfo : UserControl
    {
        #region 公用參數設定
        private WindownApplication app = null;
        private string sLineID = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定
        public string Img_Vehicle_DIS { get; set; }
        public VehicleColorInfo()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex) { logger.Error(ex, "Exception"); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                app = WindownApplication.getInstance();
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                string img_paht_vehicle_disconnection = sPath + "\\Resources\\VH_Display\\Vehicle [Unconnected].png";
                string img_paht_vehicle_manual= sPath + "\\Resources\\VH_Display\\Vehicle [Manual].png";
                string img_paht_vehicle_auto_remote= sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Remote].png";
                string img_paht_vehicle_auto_local= sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Local].png";
                string img_paht_vehicle_auto_charging= sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Charging].png";
                img_disconnection.Source = new BitmapImage(new Uri(img_paht_vehicle_disconnection, UriKind.Absolute)); ;
                img_manual.Source = new BitmapImage(new Uri(img_paht_vehicle_manual, UriKind.Absolute)); ;
                img_auto_remove.Source = new BitmapImage(new Uri(img_paht_vehicle_auto_remote, UriKind.Absolute)); ;
                img_auto_local.Source = new BitmapImage(new Uri(img_paht_vehicle_auto_local, UriKind.Absolute)); ;
                img_auto_charging.Source = new BitmapImage(new Uri(img_paht_vehicle_auto_charging, UriKind.Absolute)); ;
                //img_Vehicle_AutoRemote = sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Remote].png";
                //img_Vehicle_Manual = sPath + "\\Resources\\VH_Display\\Vehicle [Manual].png";
                //img_Vehicle_PowerOn = sPath + "\\Resources\\VH_Display\\Vehicle [Power on].png";
                //img_Vehicle_AutoCharging = sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Charging].png";
                //img_Vehicle_Disconnect = sPath + "\\Resources\\VH_Display\\Vehicle [Unconnected].png";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
