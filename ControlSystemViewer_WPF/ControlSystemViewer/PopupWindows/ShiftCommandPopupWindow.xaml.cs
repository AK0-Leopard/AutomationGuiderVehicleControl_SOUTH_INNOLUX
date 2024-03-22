using com.mirle.ibg3k0.bc.wpf.App;
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
using ViewerObject;

namespace ControlSystemViewer.PopupWindows
{
    /// <summary>
    /// ShiftCommandPopupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ShiftCommandPopupWindow : Window
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private VTRANSFER cmd = null;
        private object caller = null;
        #endregion 公用參數設定

        public ShiftCommandPopupWindow(VTRANSFER _cmd, object _caller = null)
        {
            InitializeComponent();

            cmd = _cmd;
            caller = _caller;

            this.Loaded += _Load;
            TransferCommand.CloseEvent += _CloseEvent;
            this.Closed += _Closed;
        }

        private void _Load(object sender, EventArgs e)
        {
            try
            {
                TransferCommand.SetTitleName("Shift Command", "Shift Vehicle ID");
                TransferCommand.InitUI(cmd, BCAppConstants.SubPageIdentifier.TRANSFER_SHIFT_COMMAND);
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

        public void SetCmd(VTRANSFER _cmd)
        {
            try
            {
                cmd = _cmd;
                TransferCommand.InitUI(cmd, BCAppConstants.SubPageIdentifier.TRANSFER_SHIFT_COMMAND);
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
                TransferCommand.CloseEvent -= _CloseEvent;
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
    }
}
