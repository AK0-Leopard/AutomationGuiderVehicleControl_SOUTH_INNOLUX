using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using MirleGO_UIFrameWork.UI.uc_Button;
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

namespace ControlSystemViewer.Views.Menu_Operation.Sub
{
    /// <summary>
    /// ChangePriority.xaml 的互動邏輯
    /// </summary>
    public partial class ChangePriority : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private string mcs_cmd_id = string.Empty;
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public ChangePriority()
        {
            InitializeComponent();

            num_PriSum.Focus();
        }

        public void InitUI(string cmd_id)
        {
            try
            {
                app = WindownApplication.getInstance();
                mcs_cmd_id = cmd_id;

                SetIsInputMethodEnabled();
                if (app.ObjCacheManager.GetTRANSFERs().Count > 0)
                {
                   
                    txt_CurMaxPriSum.Text = app.CmdBLL.GetTransferMaxPrioritySum().ToString();
                    txt_CurMinPriSum.Text = app.CmdBLL.GetTransferMinPrioritySum().ToString();
                    var mcs_cmd = app.CmdBLL.GetTransferByID(mcs_cmd_id);
                    txt_McsCmdID.Text = mcs_cmd_id;
                    txt_McsPri.Text = mcs_cmd.PRIORITY.ToString();
                    txt_TimePri.Text = mcs_cmd.TIME_PRIORITY.ToString();
                    num_PriSum.Text = mcs_cmd.PRIORITY_SUM.ToString(); ;
                    txt_PortPri.Value = mcs_cmd.PORT_PRIORITY;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void SetIsInputMethodEnabled() //設置IME和輸入是否可以是中文
        {
            try
            {
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_CurMaxPriSum, false);
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_CurMinPriSum, false);
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_McsCmdID, false);
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_McsPri, false);
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_PortPri, false);
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_TimePri, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    mcsCommandPriorityChange(mcs_cmd_id.Trim(), txt_PortPri.Value.ToString());
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandPriorityChange(string mcs_cmd, string priority)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandChangePriority(mcs_cmd, priority, out result));
             if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("", "Priority Change Succeed", BCAppConstants.INFO_MSG);
                    CloseEvent?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
