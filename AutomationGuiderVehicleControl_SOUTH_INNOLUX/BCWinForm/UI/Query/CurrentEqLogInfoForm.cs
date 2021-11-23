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
    public partial class CurrentEqLogInfoForm : Form
    {
        BCMainForm mainform;
        BindingSource eqLogInfo_bindingSource = new BindingSource();
        List<EQLOG_INFOToShow> eqLogInfoShow = null;
        int selection_index = -1;
        public CurrentEqLogInfoForm(BCMainForm _mainForm)
        {
            InitializeComponent();
            dgv_eqLogInfos.AutoGenerateColumns = false;
            mainform = _mainForm;

            dgv_eqLogInfos.DataSource = eqLogInfo_bindingSource;

            m_StartDTCbx.Value = DateTime.Today;
            m_EndDTCbx.Value = DateTime.Now;
        }

        private void updateEqLogInfo()
        {
            string command_id = txt_commandID.Text;
            string mcs_cmd_id = txt_mcsCmdID.Text;

            DateTime start_time = m_StartDTCbx.Value;
            DateTime end_time = m_EndDTCbx.Value;

            var eqLogInfo = mainform.BCApp.SCApplication.getEQObjCacheManager().CommonInfo.EQLogInfos.ToList();
            eqLogInfoShow = eqLogInfo.Select(e => new EQLOG_INFOToShow(e)).ToList();
            eqLogInfoShow = eqLogInfoShow.Where(e => e.TIME >= start_time && e.TIME <= end_time).ToList();
            if (!SCUtility.isEmpty(command_id))
            {
                eqLogInfoShow = eqLogInfoShow.Where(e => SCUtility.isMatche(e.OHTCCMDID, command_id)).ToList();
            }
            if (!SCUtility.isEmpty(mcs_cmd_id))
            {
                eqLogInfoShow = eqLogInfoShow.Where(e => SCUtility.isMatche(e.MCSCMDID, mcs_cmd_id)).ToList();
            }
            eqLogInfoShow = eqLogInfoShow.OrderByDescending(e => e.TIME).ToList();
            eqLogInfo_bindingSource.DataSource = eqLogInfoShow;
            dgv_eqLogInfos.Refresh();
        }

        private void TransferCommandQureyListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainform.removeForm(this.Name);
        }

        private void btnlSearch_Click(object sender, EventArgs e)
        {
            selection_index = -1;
            updateEqLogInfo();
        }

        private async void m_exportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //SaveFileDialog dlg = new SaveFileDialog();
                //dlg.Filter = "Alarm files (*.xlsx)|*.xlsx";
                //if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || bcf.Common.BCFUtility.isEmpty(dlg.FileName))
                //{
                //    return;
                //}
                //string filename = dlg.FileName;
                ////建立 xlxs 轉換物件
                //Common.XSLXHelper helper = new Common.XSLXHelper();
                ////取得轉為 xlsx 的物件
                //ClosedXML.Excel.XLWorkbook xlsx = null;
                //await Task.Run(() => xlsx = helper.Export(alarmShowList));
                //if (xlsx != null)
                //    xlsx.SaveAs(filename);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
        }

        private void dgv_eqLogInfos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgv_eqLogInfos.SelectedRows.Count > 0)
                    selection_index = dgv_eqLogInfos.SelectedRows[0].Index;
                if (selection_index < 0) return;
                txt_message.Text = eqLogInfoShow[selection_index].MESSAGE;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
        }
    }
}
