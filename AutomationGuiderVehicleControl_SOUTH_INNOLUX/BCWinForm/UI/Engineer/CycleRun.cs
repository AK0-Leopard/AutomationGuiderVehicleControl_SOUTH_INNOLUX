using com.mirle.ibg3k0.bc.winform.App;
using com.mirle.ibg3k0.sc;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

namespace com.mirle.ibg3k0.bc.winform.UI
{
    public partial class CycleRun : Form
    {
        BCMainForm mainForm;
        BCApplication bcApp;
        private Logger logger = LogManager.GetCurrentClassLogger();

        List<AADDRESS> sourceAddresses = new List<AADDRESS>();
        BindingList<AADDRESS> addressesList = new BindingList<AADDRESS>();
        BindingList<AADDRESS> selectedAddressesList = new BindingList<AADDRESS>();


        public CycleRun(BCMainForm _mainForm)
        {
            InitializeComponent();
            mainForm = _mainForm;
            bcApp = mainForm.BCApp;

            List<string> lstVh = new List<string>();
            lstVh.Add(string.Empty);
            lstVh.AddRange(bcApp.SCApplication.VehicleBLL.loadAllVehicle().Select(vh => vh.VEHICLE_ID).ToList());
            string[] allVh = lstVh.ToArray();
            Common.BCUtility.setComboboxDataSource(cmb_vhIDs, allVh);


            sourceAddresses = bcApp.SCApplication.AddressesBLL.cache.GetAddresses().ToList();
            addressesList = new BindingList<AADDRESS>(sourceAddresses);

            lstBox_address_list.DataSource = addressesList;
            lstBox_address_list.DisplayMember = "DisplayAdrID";

            lstBox_cycle_run_adr_list.DataSource = selectedAddressesList;
            lstBox_cycle_run_adr_list.DisplayMember = "DisplayAdrID";
        }

        private void CycleRun_FormClosed(object sender, FormClosedEventArgs e)
        {
            sc.App.DebugParameter.CanAutoRandomGeneratesCommand = false;
            mainForm.removeForm(this.Name);
        }

        private void btn_left_Click(object sender, EventArgs e)
        {
            if (selectedAddressesList.Count <= 0)
            {
                MessageBox.Show("No object");
                return;
            }
            int index = lstBox_cycle_run_adr_list.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            var remove_adr = Take(selectedAddressesList, index);
            addressesList.Add(remove_adr);
            SortBindingList();
            resetCycleRunIndex();
        }
        private void SortBindingList()
        {
            sourceAddresses.Sort((AADDRESS X, AADDRESS Y) => X.ADR_ID.CompareTo(Y.ADR_ID));
            addressesList.ResetBindings();
        }
        private void btn_right_Click(object sender, EventArgs e)
        {
            if (addressesList.Count <= 0)
            {
                MessageBox.Show("No object");
                return;
            }
            int index = lstBox_address_list.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            var selected_adr = Take(addressesList, index);
            selectedAddressesList.Add(selected_adr);
            setCycleRunIndex();
        }

        private void setCycleRunIndex()
        {
            for (int i = 0; i < selectedAddressesList.Count; i++)
            {
                selectedAddressesList[i].cycleRunIndex = i + 1;
            }
        }
        private void resetCycleRunIndex()
        {
            for (int i = 0; i < addressesList.Count; i++)
            {
                addressesList[i].cycleRunIndex = 0;
            }
        }

        private AADDRESS Take(BindingList<AADDRESS> addresses, int index)
        {
            var adr = addresses[index];
            addresses.RemoveAt(index);
            return adr;
        }

        private void rad_order_cycleRun_CheckedChanged(object sender, EventArgs e)
        {
            sc.App.DebugParameter.CycleRunType = sc.App.DebugParameter.CycleRun.Order;
        }

        private void rad_random_cycRun_CheckedChanged(object sender, EventArgs e)
        {
            sc.App.DebugParameter.CycleRunType = sc.App.DebugParameter.CycleRun.Random;
            resetCycleRunInfo();
        }

        private void cb_startCyc_CheckedChanged(object sender, EventArgs e)
        {
            if (sc.Common.SCUtility.isEmpty(cmb_vhIDs.Text))
            {
                MessageBox.Show("Plase select vh.");
                return;
            }
            tableLayoutPanel1.Enabled = !cb_startCyc.Checked;
            sc.App.DebugParameter.CanAutoRandomGeneratesCommand = cb_startCyc.Checked;
            resetCycleRunInfo();
        }
        private void resetCycleRunInfo()
        {
            foreach (var adr in addressesList)
                adr.cycleRunCount = 0;
            foreach (var adr in selectedAddressesList)
                adr.cycleRunCount = 0;
        }

        private void CycleRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cb_startCyc.Checked)
            {
                MessageBox.Show("Plase stop cycle run first.");
                e.Cancel = true;
            }
        }

        private void cmb_vhIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            sc.App.DebugParameter.SelectedCycleRunVh = cmb_vhIDs.Text;
        }
    }
}
