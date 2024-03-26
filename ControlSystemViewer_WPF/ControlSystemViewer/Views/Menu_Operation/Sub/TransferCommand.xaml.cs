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
using ViewerObject;
using static ViewerObject.VTRANSFER_Def;

namespace ControlSystemViewer.Views.Menu_Operation.Sub
{
    /// <summary>
    /// TransferCommand.xaml 的互動邏輯
    /// </summary>
    public partial class TransferCommand : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private string identity = string.Empty;
        private string mcs_cmd_id;
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public TransferCommand()
        {
            InitializeComponent();

            this.Loaded += _Load;
        }

        public void InitUI(VTRANSFER mcs_cmd, string identifier)
        {
            try
            {
                app = WindownApplication.getInstance();
                identity = identifier;
                mcs_cmd_id = mcs_cmd.CMD_ID;
                if (identifier == BCAppConstants.SubPageIdentifier.TRANSFER_CHANGE_STATUS)
                {
                    txt_McsCmdID.Text = mcs_cmd.CMD_ID;
                    ComboBox.Items.Clear();
                    foreach (TransferStatus item in Enum.GetValues(typeof(TransferStatus)))
                    {
                        ComboBox.Items.Add(item);
                    }
                    ComboBox.SelectedItem = mcs_cmd.TRANSFER_STATUS;
                }
                else if (identifier == BCAppConstants.SubPageIdentifier.TRANSFER_ASSIGN_VEHICLE)
                {
                    txt_McsCmdID.Text = mcs_cmd.CMD_ID;
                    ComboBox.Items.Clear();
                    var vhs = app.ObjCacheManager.GetVEHICLEs();
                    app.VehicleBLL.filterVh(ref vhs);
                    foreach (var vh in vhs)
                    {
                        ComboBox.Items.Add(vh.VEHICLE_ID);
                    }
                }
                else if (identifier == BCAppConstants.SubPageIdentifier.TRANSFER_SHIFT_COMMAND)
                {
                    txt_McsCmdID.Text = mcs_cmd.CMD_ID;
                    ComboBox.Items.Clear();
                    var vhs = app.ObjCacheManager.GetVEHICLEs();
                    app.VehicleBLL.filterVh(ref vhs);
                    foreach (var vh in vhs)
                    {
                        if (mcs_cmd.VH_ID.Trim() == vh.VEHICLE_ID)
                        {
                            vhs.Remove(vh);
                        }
                    }
                    foreach (var vh in vhs)
                    {
                        ComboBox.Items.Add(vh.VEHICLE_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _Load(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBox.Focus(); //將游標放置指定位置
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_McsCmdID, false); //設置IME和輸入是否可以是中文
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetTitleName(string title, string contentTitle)
        {
            try
            {
                Title.Text = title;
                ContentTitle.Text = contentTitle;
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
                if (identity == BCAppConstants.SubPageIdentifier.TRANSFER_CHANGE_STATUS)
                {
                    string status = string.Empty;
                    Adapter.Invoke(async (obj) =>
                    {
                        TransferStatus ee = ((TransferStatus)ComboBox.SelectedItem);
                        status = ee.ToString();
                        mcsCommandStatusChange(mcs_cmd_id, status);
                    }, null);
              
                }
                else if (identity == BCAppConstants.SubPageIdentifier.TRANSFER_ASSIGN_VEHICLE)
                {
                    string vhid = string.Empty;
                    Adapter.Invoke(async (obj) =>
                    {
                        if (ComboBox.SelectedItem == null)
                        {
                            TipMessage_Type_Light.Show("", "Please select vehicle.", BCAppConstants.INFO_MSG);
                            return;
                        }
                        vhid = ComboBox.SelectedItem.ToString();
                        mcsCommandVehicleAssign(mcs_cmd_id, vhid);
                    }, null);
                }
                else if (identity == BCAppConstants.SubPageIdentifier.TRANSFER_SHIFT_COMMAND)
                {
                    string vhid = string.Empty;
                    Adapter.Invoke(async (obj) =>
                    {
                        if (ComboBox.SelectedItem == null)
                        {
                            TipMessage_Type_Light.Show("", "Please select vehicle.", BCAppConstants.INFO_MSG);
                            return;
                        }
                        vhid = ComboBox.SelectedItem.ToString();
                        mcsCommandShift(mcs_cmd_id, vhid);
                    }, null);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandStatusChange(string mcs_cmd, string status)
        {
            try
            {
                TransferStatus cur_status = app.ObjCacheManager.GetTRANSFERs().Where(cmd => cmd.CMD_ID.StartsWith(mcs_cmd)).First().TRANSFER_STATUS;
                TransferStatus new_status = TransferStatus.Reject;
                foreach (TransferStatus item in Enum.GetValues(typeof(TransferStatus)))
                {
                    if (item.ToString().Equals(status))
                    {
                        new_status = item;
                        break;
                    }
                }
                if (cur_status >= new_status)
                {
                    TipMessage_Type_Light_woBtn.Show("", "Status Change Failed,\nShould NOT Change Status Backward", BCAppConstants.WARN_MSG);
                    return;
                }

                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandChangeStatus(mcs_cmd, status, out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light_woBtn.Show("", "Status Change Failed,\n" + result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Status Change Succeed", BCAppConstants.INFO_MSG);
                    CloseEvent?.Invoke(this, null);
                }
                app.OperationHistoryBLL.
                    addOperationHis(app.LoginUserID,
                                    this.GetType().Name,
                                    $"Excute trnasfer cmd id:{mcs_cmd} status to:{status}, is success:{isSuccess}");

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandVehicleAssign(string mcs_cmd, string vh_id)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandAssignVehicle(mcs_cmd, vh_id, out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Assign Vehicle Succeed", BCAppConstants.INFO_MSG);
                    CloseEvent?.Invoke(this, null);
                }
                app.OperationHistoryBLL.
                    addOperationHis(app.LoginUserID,
                                    this.GetType().Name,
                                    $"Excute trnasfer cmd id:{mcs_cmd} force assign to vh:{vh_id}, is success:{isSuccess}");

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandShift(string mcs_cmd, string vh_id)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandShift(mcs_cmd, vh_id, out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Shift Command Succeed", BCAppConstants.INFO_MSG);
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
