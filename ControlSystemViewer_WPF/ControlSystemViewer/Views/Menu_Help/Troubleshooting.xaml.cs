using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.Utility.uc;
using NLog;
using NLog.Time;
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
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_Help
{
    /// <summary>
    /// About.xaml 的互動邏輯
    /// </summary>
    public partial class Troubleshooting : UserControl
    {
        #region 公用參數設定
        private WindownApplication app = null;
        private string sLineID = string.Empty;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定

        public Troubleshooting()
        {
            try
            {
                InitializeComponent();
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception ex) { logger.Error(ex, "Exception"); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                app = WindownApplication.getInstance();

                initialDataGridView();
                //System Version
                //lbl_SofwVsion_Val.Text = "Version " + BCAppConstants.getMainFormVersion("");

                //System Build Date
                string dtBuildDate = File.GetLastWriteTimeUtc(Assembly.GetEntryAssembly().Location).ToLocalTime().ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                //lbl_SofwBuildDate_Val.Text = " Build " + dtBuildDate;

                //System Line ID
                //lbl_LineID.Text = app?.ObjCacheManager.GetLine()?.LINE_ID?.Trim() ?? "";
                //lbl_LineID.Text = app?.ObjCacheManager.MapId?.Trim() ?? "";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void initialDataGridView()
        {
            var itemSource = app.ObjCacheManager.GetAlarmMaps();
            dgv_alarmTable.ItemsSource = itemSource;
            /*
            dgvCol_alarmCode.Binding = new Binding("alarmID");
            dgvCol_alarmDesc.Binding = new Binding("alarmDesc");
            dgvCol_happendReason.Binding = new Binding("happendReason");
            dgvCol_solution.Binding = new Binding("solution");
            */
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var itemSource = app.ObjCacheManager.GetAlarmMaps();
            dgv_alarmTable.ItemsSource = itemSource.Where(item => item.alarmID.Contains(TB_AlarmCode.Text)).ToList();
        }

        private void dgv_alarmTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var item = dgv_alarmTable.SelectedItem as AlarmMap;
            //Task.Run(() => { MessageBox.Show(item.solution, "solution"); }) ;
        }

        private void dgv_alarmTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var item = dgv_alarmTable.SelectedItem as AlarmMap;
            TB_Solution.Text = item.solution;
        }
    }
}
