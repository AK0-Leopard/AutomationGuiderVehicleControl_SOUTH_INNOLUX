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
    public partial class About : UserControl
    {
        #region 公用參數設定
        private WindownApplication app = null;
        private string sLineID = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定

        public About()
        {
            try
            {
                InitializeComponent();

                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                img_MirleLogo.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + "\\Resources\\SystemIcon\\Mirle_logo.png"));
            }
            catch (Exception ex) { logger.Error(ex, "Exception"); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                app = WindownApplication.getInstance();

                //System Version
                lbl_SofwVsion_Val.Text = "Version " + BCAppConstants.getMainFormVersion("");

                //System Build Date
                string dtBuildDate = File.GetLastWriteTimeUtc(Assembly.GetEntryAssembly().Location).ToLocalTime().ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                lbl_SofwBuildDate_Val.Text = " Build " + dtBuildDate;

                //System Line ID
                lbl_LineID.Text = app?.ObjCacheManager.GetLine()?.LINE_ID?.Trim() ?? "";
                //lbl_LineID.Text = app?.ObjCacheManager.MapId?.Trim() ?? "";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
