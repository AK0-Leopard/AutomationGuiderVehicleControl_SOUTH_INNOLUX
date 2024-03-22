//#define DISPLAYCONTROLSTATUSDETAIL // v0.1.1 -1
//#define DISPLAYDEVICESTATUSPAGE // v0.1.1 -2

using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.Utility.uc;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ControlSystemViewer.Views.Chart
{
    /// <summary>
    /// SystemModeControl.xaml 的互動邏輯
    /// </summary>
    public partial class Chart_Base : UserControl
    {
        #region 公用參數設定
        public event EventHandler CloseEvent;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        WpfPlot MainPlot = new WpfPlot();
        private string DefaultFileName = "";
        #endregion 公用參數設定

        public Chart_Base()
        {
            InitializeComponent();
            this.Loaded += _Loaded;

        }

        private void _Loaded(object sender, EventArgs e)
        {
            StartupUI();
        }

        public void StartupUI()
        {
            try
            {
                app = WindownApplication.getInstance();
                //init();
                //registerEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Show(ref ScottPlot.WpfPlot inplot, string _filename = "")
        {
            try
            {
                if (inplot == null) return;
                MainPlot = inplot;
                PlotGrid.Children.Add((UserControl)MainPlot);
                Grid.SetRow((UserControl)MainPlot, 0);
                Grid.SetColumn((UserControl)MainPlot, 0);
                DefaultFileName = _filename;
                MainPlot.Refresh();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Close()
        {
            try
            {
                //unregisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseEvent?.Invoke(this, e);
                //unregisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_AutoAxis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainPlot.Plot.AxisAuto();
                MainPlot.Refresh();
                //unregisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SaveImg();
                //unregisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
           
        }

        private async  Task SaveImg()
        {
            try
            {
                ProjectInfo oProjectInfo = WindownApplication.getInstance().ObjCacheManager.GetSelectedProject();
                string CustomerName = oProjectInfo.Customer.ToString();
                string ProductLine = oProjectInfo.ProductLine.ToString();

              
                System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
                dlg.FileName = "[" + CustomerName + "_" + ProductLine + "] " + DefaultFileName;
                dlg.Filter = "Files (*.png)|*.png";
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dlg.FileName))
                {
                    return;
                }
                string filename = dlg.FileName;
                await Task.Run( () => MainPlot.Plot.SaveFig(filename) );
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
         
        }
    }
}
