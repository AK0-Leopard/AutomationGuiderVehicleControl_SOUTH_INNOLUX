using com.mirle.ibg3k0.ohxc.wpf.ObjectRelay;
using ControlSystemViewer.Views.Menu_Operation;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlSystemViewer.PopupWindows
{
    /// <summary>
    /// ChangePriorityPopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ChangePriorityPopupWindow : Window
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string cmdID = string.Empty;
        private object caller = null;
        #endregion 公用參數設定

        public ChangePriorityPopupWindow(string cmd_id, object _caller = null)
        {
            InitializeComponent();

            cmdID = cmd_id;
            caller = _caller;

            this.Loaded += _Load;
            ChangePriority.CloseEvent += _CloseEvent;
            this.Closed += _Closed;
        }

        private void _Load(object sender, EventArgs e)
        {
            try
            {
                ChangePriority.InitUI(cmdID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void BringToFront()
        {
            if (!this.IsVisible)
            {
                this.Show();
            }

            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }

            this.Activate();
            this.Focus();
        }

        public void SetCmdID(string cmd_id)
        {
            try
            {
                cmdID = cmd_id;
                ChangePriority.InitUI(cmdID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        #region RemoveIcon
        protected override void OnSourceInitialized(EventArgs e)
        {
            UtilsAPI.Tool.IconHelper.RemoveIcon(this);
        }
        #endregion RemoveIcon

        private void _CloseEvent(object sender, EventArgs e)
        {
            try
            {
                ChangePriority.CloseEvent -= _CloseEvent;
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _Closed(object sender, EventArgs e)
        {
            try
            {
                TransferManagement transferManagement = caller as TransferManagement;
                if (transferManagement != null) transferManagement.PopupWindowClosed(this);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ChangePriority_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
