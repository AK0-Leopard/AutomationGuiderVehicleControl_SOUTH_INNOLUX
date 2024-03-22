using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlSystemViewer.Views.CurrentDataGrid
{
    /// <summary>
    /// uc_grid_TransCMD.xaml 的互動邏輯
    /// </summary>
    public partial class uc_grid_TransCMD : UserControl
    {
        MenuItem mi_cmdCancelAbort;
        MenuItem mi_cmdAddPriority;
        WindownApplication app;
        public uc_grid_TransCMD()
        {
            InitializeComponent();

            app = WindownApplication.getInstance();
            var contextMenu = new ContextMenu();

            mi_cmdCancelAbort = new MenuItem();
            mi_cmdCancelAbort.Header = "Cancel/abort this command!";
            mi_cmdCancelAbort.Click += Mi_cmdCancelAbort_ClickAsync;
            contextMenu.Items.Add(mi_cmdCancelAbort);

            mi_cmdAddPriority = new MenuItem();
            mi_cmdAddPriority.Header = "Add 5 Priorties to this command!";
            mi_cmdAddPriority.Click += Mi_cmdAddPriority_ClickAsync;
            contextMenu.Items.Add(mi_cmdAddPriority);

            this.ContextMenu = contextMenu;

        }

        
        private async void Mi_cmdCancelAbort_ClickAsync(object sender, RoutedEventArgs e)
        {
            bool isSuccess = true;
            string result="";
            string questString = "";
            ViewerObject.VTRANSFER selectItem = (ViewerObject.VTRANSFER)this.grid_MCS_Command.SelectedItem;
            if (selectItem is null) return;
            questString = "Do you really want to Cance/lAbort this Command?\r\n" + "\r\n" +
                                "Command ID: " + selectItem.TRANSFER_ID + "\r\n" +
                                //"Carrier ID: " + (selectItem.CARRIER_ID=="" ? selectItem.BOX_ID : selectItem.CARRIER_ID) + "\r\n" +
                                "Carrier ID: " + selectItem.CARRIER_ID + "\r\n" +
                                "Source : " + selectItem.HOSTSOURCE + "\r\n" +
                                "Dest : " + selectItem.HOSTDESTINATION;
            
            //var Result = MessageBox.Show( questString, "CancelAbort,", MessageBoxButton.OKCancel);

            var Result = TipMessage_Request_Light.Show(questString);

            if (Result ==  System.Windows.Forms.DialogResult.Yes)
            {
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandCancelAbort(selectItem.TRANSFER_ID, out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("Send command failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
                }
            }
        }
        private async void Mi_cmdAddPriority_ClickAsync(object sender, RoutedEventArgs e)
        {
            bool isSuccess = true;
            string result = "";
            string questString = "";
            ViewerObject.VTRANSFER selectItem = (ViewerObject.VTRANSFER)this.grid_MCS_Command.SelectedItem;
            if (selectItem is null) return;
            questString = "Do you really want to Add Priority to this Command?\r\n" + "\r\n" +
                                "Priority: " + selectItem.PRIORITY_SUM + "\r\n" +
                                "Command ID: " + selectItem.TRANSFER_ID + "\r\n" +
                                //"Carrier ID: " + (selectItem.CARRIER_ID=="" ? selectItem.BOX_ID : selectItem.CARRIER_ID) + "\r\n" +
                                "Carrier ID: " + selectItem.CARRIER_ID + "\r\n" +
                                "Source : " + selectItem.HOSTSOURCE + "\r\n" +
                                "Dest : " + selectItem.HOSTDESTINATION;

            //var Result = MessageBox.Show( questString, "CancelAbort,", MessageBoxButton.OKCancel);

            var Result = TipMessage_Request_Light.Show(questString);

            if (Result == System.Windows.Forms.DialogResult.Yes)
            {
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandChangePriority(selectItem.TRANSFER_ID, (selectItem.PRIORITY+5)+"" ,out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("Add Prority failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Added Prority successfully", BCAppConstants.INFO_MSG);
                }
            }
        }

    }
}
