using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.ObjectRelay;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.mirle.ibg3k0.sc.Data.VO;

namespace com.mirle.ibg3k0.bc.winform.UI
{
    public partial class AlarmListForm : Form
    {
        BCMainForm mainform;
        List<AlarmMap> alarmMapList = null;
        public AlarmListForm(BCMainForm _mainForm)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            mainform = _mainForm;
            alarmMapList = mainform.BCApp.SCApplication.AlarmBLL.loadAlarmMaps();
            dataGridView1.DataSource = alarmMapList;
        }

        private void updateAlarms()
        {

        }

        private void TransferCommandQureyListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainform.removeForm(this.Name);
        }

        private void btnlSearch_Click(object sender, EventArgs e)
        {
            updateAlarms();
        }

        private async void m_exportBtn_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Warn(ex, "Exception");
            }
        }

        private void m_AlarmCodeTbl_TextChanged(object sender, EventArgs e)
        {
            string code = m_AlarmCodeTbl.Text;

            var new_list = alarmMapList.Where(alarm => alarm.ALARM_ID.Contains(code)).ToList();
            dataGridView1.DataSource = new_list;

            dataGridView1.Refresh();
        }
    }
}
