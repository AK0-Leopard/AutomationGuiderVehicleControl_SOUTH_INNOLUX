using com.mirle.ibg3k0.ohxc.wpf.App;
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
using ViewerObject;

namespace ControlSystemViewer.Views.CurrentDataGrid
{
    /// <summary>
    /// CommuLog_SystemProc.xaml 的互動邏輯
    /// </summary>
    public partial class CommuLog_SystemProc : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public WindownApplication app = null;
        public CommuLog_SystemProc()
        {
            InitializeComponent();
        }
        public void Start(WindownApplication _app)
        {
            app = _app;
        }

        private void ckb_LogEnable_Click(object sender, RoutedEventArgs e)
        {
            var v = ((CheckBox)sender).IsChecked;
           
            if (v ==true)
            {
                LogStatus.openlog =(bool)v;
                LogStatus.nowLevel = cbx_LogLevel.Text;
                AddLogLevel();
            }
            else if(v==false)
            {
                LogStatus.openlog = (bool)v;
                LogStatus.nowLevel = cbx_LogLevel.Text;
                //RemoveLogLevel();
            }
            else
            {

            }
            
        }

        private void cbx_LogLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogStatus.openlog == false) return;
            LogStatus.oldLevel = cbx_LogLevel.Text;
            LogStatus.nowLevel = e.AddedItems[0].ToString();
            ChangeLogLevel();
        }


        private async void AddLogLevel()
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendLogLevelAdd(out result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public async void RemoveLogLevel()
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendLogLevelRemove(out result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void ChangeLogLevel()
        {
            try
            {
                if (LogStatus.oldLevel.Trim() == "" && LogStatus.nowLevel.Trim() == "") return;
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendLogLevelChange(out result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

    }
}
