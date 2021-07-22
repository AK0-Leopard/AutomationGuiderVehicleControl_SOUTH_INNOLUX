using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.ObjectRelay;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.mirle.ibg3k0.bc.winform.UI
{
    public partial class HistoryTransferCommandForm : Form
    {
        BCMainForm mainform;
        BindingSource hcmdMCS_bindingSource = new BindingSource();
        List<HCMD_MCSObjToShow> hcmdMCSList = null;
#pragma warning disable CS0414 // 已指派欄位 'HistoryTransferCommandForm.selection_index'，但從未使用過其值。
        int selection_index = -1;
#pragma warning restore CS0414 // 已指派欄位 'HistoryTransferCommandForm.selection_index'，但從未使用過其值。
        public HistoryTransferCommandForm(BCMainForm _mainForm)
        {
            InitializeComponent();
            dgv_TransferCommand.AutoGenerateColumns = false;
            mainform = _mainForm;

            dgv_TransferCommand.DataSource = hcmdMCS_bindingSource;

            m_StartDTCbx.Value = DateTime.Today;
            m_EndDTCbx.Value = DateTime.Now;
            m_exportBtn.Enabled = hcmdMCSList != null && hcmdMCSList.Count > 0;
        }

        private async Task updateTransferCommand()
        {
            await Task.Run(()=> hcmdMCSList = mainform.BCApp.SCApplication.CMDBLL.loadHCMD_MCSs(m_StartDTCbx.Value, m_EndDTCbx.Value));
            m_exportBtn.Enabled = hcmdMCSList != null && hcmdMCSList.Count > 0;
            hcmdMCS_bindingSource.DataSource = hcmdMCSList;
            dgv_TransferCommand.Refresh();
        }

        private void TransferCommandQureyListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainform.removeForm(this.Name);
        }

        private async void btnlSearch_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ProgressBarDialog progress = new ProgressBarDialog(null);
            try
            {
                if (btn != null) btn.Enabled = false;

                progress.StartPosition = FormStartPosition.CenterScreen;
                progress.Show();
                progress.SetText("Searching...");

                selection_index = -1;
                await updateTransferCommand();
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
            finally
            {
                if (btn != null)
                    btn.Enabled = true;
                progress.Dispose();
            }
        }

        private async void m_exportBtn_Click(object sender, EventArgs e)
        {
            if (hcmdMCSList == null || hcmdMCSList.Count == 0) return;
            Button btn = sender as Button;
            ProgressBarDialog progress = new ProgressBarDialog(null);
            try
            {
                if (btn != null)
                    btn.Enabled = false;

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Transfer Command files (*.xlsx)|*.xlsx";
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || bcf.Common.BCFUtility.isEmpty(dlg.FileName))
                {
                    return;
                }

                progress.StartPosition = FormStartPosition.CenterScreen;
                progress.Show();
                progress.SetText("Exporting...");

                string filename = dlg.FileName;
                //建立 xlxs 轉換物件
                Common.XSLXHelper helper = new Common.XSLXHelper();
                //取得轉為 xlsx 的物件
                ClosedXML.Excel.XLWorkbook xlsx = null;
                await Task.Run(() => xlsx = helper.Export(hcmdMCSList));
                if (xlsx != null)
                    xlsx.SaveAs(filename);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
            finally
            {
                if (btn != null)
                    btn.Enabled = true;
                progress.Dispose();
            }
        }
    }
}
