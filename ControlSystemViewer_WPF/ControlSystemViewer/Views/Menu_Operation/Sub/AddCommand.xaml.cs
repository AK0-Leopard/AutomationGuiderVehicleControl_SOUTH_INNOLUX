using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
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

namespace ControlSystemViewer.Views.Menu_Operation.Sub
{
    /// <summary>
    /// AddCommand.xaml 的互動邏輯
    /// </summary>
    public partial class AddCommand : UserControl
    {
        WindownApplication app = null;
        // List<ACARRIER> cassetteDatas = null;
        List<VPORTSTATION> portDefs = null;
        public event EventHandler CloseEvent;

        public AddCommand()
        {
            InitializeComponent();

            this.Loaded += _Load;
        }

        private void _Load(object sender, EventArgs e)
        {
            app = WindownApplication.getInstance();
            //cassetteDatas = app.ObjCacheManager.GetCassetteDatas();
            portDefs = app.ObjCacheManager.GetPortStations();

            //cmb_CassetteID.ItemsSource = cassetteDatas.Select(x => x.ID).ToList();
            //for (int i = 0; i < cmb_CassetteID.Items.Count; i++)
            //{
            //    cmb_CassetteID.Items[i] = Convert.ToString(cmb_CassetteID.Items[i])?.TrimEnd();
            //}

            cmb_Destination.ItemsSource = portDefs.Where(x => x.IS_IN_SERVICE).Select(x => x.PORT_ID.ToString()).ToList();
            for (int i = 0; i < cmb_Destination.Items.Count; i++)
            {
                cmb_Destination.Items[i] = Convert.ToString(cmb_Destination.Items[i])?.TrimEnd();
            }

            if (cmb_CassetteID.Items?.Count > 0)
                cmb_CassetteID.SelectedIndex = 0;
            if (cmb_Destination.Items?.Count > 0)
                cmb_Destination.SelectedIndex = 0;
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Source.Text) || string.IsNullOrEmpty(cmb_Destination.Text) || string.IsNullOrEmpty(cmb_CassetteID.Text))
            {
                MessageBox.Show("不能有欄位為空");
                return;
            }
            if (txt_Source.Text.Equals(cmb_Destination.Text?.TrimEnd()))
            {
                MessageBox.Show("來源跟目的不能相同");
                return;
            }
            if (TipMessage_Request_Light.Show("Create the command ?") == System.Windows.Forms.DialogResult.Yes)
            {
                var result = app.CmdBLL.CreateTransferCmd(cmb_CassetteID.Text, cmb_CassetteID.Text, txt_Source.Text, cmb_Destination.Text, txt_LotID.Text); // v0.1.1 -1
                if (result.success)
                {
                    TipMessage_Type_Light.Show("Succeed", "Create success", BCAppConstants.INFO_MSG);
                    CloseEvent?.Invoke(this, null);
                }
                else
                {
                    TipMessage_Type_Light.Show("Succeed", $"Create fail\r\n{result.response}", BCAppConstants.INFO_MSG);
                }
            }
        }

        private void cmb_CassetteID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (sender is ComboBox cb)
            //{
            //    txt_Source.Text = cassetteDatas.Where(x => x.ID.StartsWith(cb.Text)).Select(x => x.LOCATION).FirstOrDefault()?.TrimEnd();
            //    txt_LotID.Text = cassetteDatas.Where(x => x.ID.StartsWith(cb.Text)).Select(x => x.LOT_ID).FirstOrDefault()?.TrimEnd();
            //}
        }
    }
}
