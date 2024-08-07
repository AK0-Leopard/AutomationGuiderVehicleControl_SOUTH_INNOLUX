using com.mirle.ibg3k0.bc.winform.App;
using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.sc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.mirle.ibg3k0.bc.winform.UI
{
    public partial class VehicleOperationForm : Form
    {
        BCMainForm mainForm;
        BCApplication bcApp;
        AVEHICLE targetVh = null;
        public VehicleOperationForm(BCMainForm _mainForm)
        {
            InitializeComponent();
            mainForm = _mainForm;
            bcApp = _mainForm.BCApp;

            initialVhCombobox();
        }

        private void initialVhCombobox()
        {
            List<string> lstVh = new List<string>();
            lstVh.Add(string.Empty);
            lstVh.AddRange(bcApp.SCApplication.VehicleBLL.loadAllVehicle().Select(vh => vh.VEHICLE_ID).ToList());
            string[] allVh = lstVh.ToArray();
            cmb_vhs.DataSource = allVh;
        }



        private void VehicleOperationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.removeForm(typeof(VehicleOperationForm).Name);
        }

        private void cmb_vhs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vh_id = cmb_vhs.Text.Trim();
            targetVh = bcApp.SCApplication.getEQObjCacheManager().getVehicletByVHID(vh_id);
            if (targetVh == null)
            {
                tlp_VehicleOperationBlock.Enabled = false;
            }
            else
            {
                tlp_VehicleOperationBlock.Enabled = true;
                lbl_id_cancel_cmdID_value.Text = sc.Common.SCUtility.Trim(targetVh.OHTC_CMD, true);
                lbl_isinstalled_value.Text = targetVh.IS_INSTALLED.ToString();
                lbl_isremote_value.Text = (!targetVh.IS_CYCLING).ToString();
            }

        }

        private async void btn_changeToRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (targetVh == null)
                {
                    MessageBox.Show($"Please select vh.");
                    return;
                }
                btn_changeToRemove.Enabled = false;
                (bool isSuccess, string result) check_result = default((bool isSuccess, string result));
                await Task.Run(() => check_result = bcApp.SCApplication.VehicleService.Remove(targetVh.VEHICLE_ID));
                if (check_result.isSuccess)
                {
                    MessageBox.Show($"{targetVh.VEHICLE_ID} remove ok");
                }
                else
                {
                    MessageBox.Show($"{targetVh.VEHICLE_ID} remove fail.{Environment.NewLine}" +
                                    $"result:{check_result.result}");
                }
                lbl_isinstalled_value.Text = targetVh?.IS_INSTALLED.ToString();
            }
            finally
            {
                btn_changeToRemove.Enabled = true;
            }
        }
        private async void btn_changeToInstall_Click(object sender, EventArgs e)
        {
            try
            {
                if (targetVh.IS_INSTALLED)
                {
                    MessageBox.Show($"{targetVh.VEHICLE_ID} is install ready!");
                    return;
                }

                btn_changeToInstall.Enabled = false;
                (bool isSuccess, string result) check_result = default((bool isSuccess, string result));
                await Task.Run(() => check_result = bcApp.SCApplication.VehicleService.Install(targetVh.VEHICLE_ID));
                if (check_result.isSuccess)
                {
                    MessageBox.Show($"{targetVh.VEHICLE_ID} install ok");
                }
                else
                {
                    MessageBox.Show($"{targetVh.VEHICLE_ID} install fail.{Environment.NewLine}" +
                                    $"result:{check_result.result}");
                }
                lbl_isinstalled_value.Text = targetVh?.IS_INSTALLED.ToString();
            }
            finally
            {
                btn_changeToInstall.Enabled = true;
            }
        }

        private async void btn_auto_remote_Click(object sender, EventArgs e)
        {
            var result = await asyExecuteAction(targetVh.VEHICLE_ID, bcApp.SCApplication.VehicleService.changeVhStatusToAutoRemote);
            if (!result.ok)
            {
                MessageBox.Show($"{targetVh.VEHICLE_ID} change to auto remote fail.{Environment.NewLine}" +
                                $"result:{result.reason}");
            }
            lbl_isremote_value.Text = (!targetVh.IS_CYCLING).ToString();

        }

        private async void btn_auto_local_Click(object sender, EventArgs e)
        {
            var result = await asyExecuteAction(targetVh.VEHICLE_ID, bcApp.SCApplication.VehicleService.changeVhStatusToAutoLocal);
            if (!result.ok)
            {
                MessageBox.Show($"{targetVh.VEHICLE_ID} change to auto local fail.{Environment.NewLine}" +
                                $"result:{result.reason}");
            }

            lbl_isremote_value.Text = (!targetVh.IS_CYCLING).ToString();

        }

        private async void btn_auto_charge_Click(object sender, EventArgs e)
        {

        }
        private Task<(bool ok, string reason)> asyExecuteAction(string vhID, Func<string, (bool ok, string reason)> act)
        {
            return Task.Run(() =>
               {
                   return act(vhID);
               });
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            string cancel_cmd_id = lbl_id_cancel_cmdID_value.Text;
            if (sc.Common.SCUtility.isEmpty(cancel_cmd_id))
            {
                MessageBox.Show($"{targetVh.VEHICLE_ID} no command can cancel.");
                return;
            }

            Task.Run(() =>
            {
                targetVh.sned_Str37(cancel_cmd_id, sc.ProtocolFormat.OHTMessage.CMDCancelType.CmdCancel);
            });
        }

        private void btn_abort_Click(object sender, EventArgs e)
        {
            string abort_cmd_id = lbl_id_cancel_cmdID_value.Text;
            if (sc.Common.SCUtility.isEmpty(abort_cmd_id))
            {
                MessageBox.Show($"{targetVh.VEHICLE_ID} no command can abort.");
                return;
            }
            Task.Run(() =>
            {
                targetVh.sned_Str37(abort_cmd_id, sc.ProtocolFormat.OHTMessage.CMDCancelType.CmdAbort);
            });
        }

        private void btn_refsh_vh_status_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                bcApp.SCApplication.VehicleService.VehicleStatusRequest(targetVh.VEHICLE_ID, true);
            });
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                bool isSuccess = bcApp.SCApplication.CMDBLL.forceUpdataCmdStatus2FnishByVhID(targetVh.VEHICLE_ID);
                if (isSuccess)
                {
                    var vh = bcApp.SCApplication.VehicleBLL.getVehicleByID(targetVh.VEHICLE_ID);
                    vh.VehicleUnassign();
                    vh.NotifyVhExcuteCMDStatusChange();
                }
            });
        }
    }
}
