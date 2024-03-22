using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.Utility.uc;
using ControlSystemViewer.PopupWindows;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using ViewerObject;
using static ViewerObject.VTRANSFER_Def;

namespace ControlSystemViewer.Views.Menu_Operation
{
    /// <summary>
    /// TransferManagement.xaml 的互動邏輯
    /// </summary>
    public partial class TransferManagement : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private VLINE line = null;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool shouldStopTimer = false;
        private const int iCountdown_StartValue = 60;
        private const int iCountdown_AlarmValue = 20;
        private int iCountdown = iCountdown_StartValue;
        private AssignVehiclePopupWindow assignVehiclePopup = null;
        private ShiftCommandPopupWindow shiftCommandPopup = null;
        private ChangeStatusPopupWindow changeStatusPopup = null;
        private ChangePriorityPopupWindow changePriorityPopup = null;
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public TransferManagement()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }

        public void StartupUI()
        {
            try
            {
                app = WindownApplication.getInstance();
                line = app.ObjCacheManager.GetLine();
                registerEvent();
                init();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Close()
        {
            if (togBtn_AutoAssign.Toggled1)
                mcsCommandAutoAssignChange(true);

            unregisterEvent();

            closeSubPage();
            stopCountdown();
        }

        private void grid_MCS_Command_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetCountdown();
        }

        private void TransferManagement_MouseMove(object sender, MouseEventArgs e)
        {
            resetCountdown();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (shouldStopTimer) return;

                iCountdown--;
                tbk_CountdownValue.Text = iCountdown.ToString();

                if (iCountdown > 0)
                {
                    pnl_Countdown.Visibility = iCountdown > iCountdown_AlarmValue ?
                                               Visibility.Hidden : Visibility.Visible;
                }
                else
                {
                    btn_AssignVh.IsEnabled = false;
                    btn_ShiftCmd.IsEnabled = false;
                    btn_ChangeStatus.IsEnabled = false;
                    mcsCommandAutoAssignChange(true);
                    togBtn_AutoAssign.set_ToggleButton(true);
                    pnl_Countdown.Visibility = Visibility.Hidden;
                    closeSubPage();
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                if (shouldStopTimer) timer.Stop();
            }
        }

        private void startCountdown()
        {
            iCountdown = iCountdown_StartValue;
            tbk_CountdownValue.Text = iCountdown.ToString();
            shouldStopTimer = false;
            timer.Start();
        }
        private void stopCountdown()
        {
            timer.Stop();
            shouldStopTimer = true;
            pnl_Countdown.Visibility = Visibility.Hidden;
        }
        private void resetCountdown()
        {
            iCountdown = iCountdown_StartValue;
        }

        private void closeSubPage()
        {
            if (assignVehiclePopup != null) { assignVehiclePopup.Close(); assignVehiclePopup = null; }
            if (shiftCommandPopup != null) { shiftCommandPopup.Close(); shiftCommandPopup = null; }
            if (changeStatusPopup != null) { changeStatusPopup.Close(); changeStatusPopup = null; }
            if (changePriorityPopup != null) { changePriorityPopup.Close(); changePriorityPopup = null; }
        }

        private void registerEvent()
        {
            try
            {
                line.TransferInfoChange += _TransferInfoChange;
                app.ObjCacheManager.TransferChange += ObjCacheManager_TransferChange;

                string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
                if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
                app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_TRANSFER_INFO+ ChannelBay, app.LineBLL.TransferInfo);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void unregisterEvent()
        {
            try
            {
                line.TransferInfoChange -= _TransferInfoChange;
                app.ObjCacheManager.TransferChange -= ObjCacheManager_TransferChange;

                string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
                if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
                app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_TRANSFER_INFO+ ChannelBay);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void init()
        {
            try
            {
                if (!(app.ObjCacheManager.GetSelectedProject().ObjectConverter == "OHBC_ASE_K21"))
                {
                    int index = grid_MCS_Command.Columns.IndexOf(this.dgtc_BoxID);
                    grid_MCS_Command.Columns[index].Visibility = Visibility.Collapsed;
                }
                var all_cmd_view_obj = app?.ObjCacheManager.GetTRANSFERs()?.ToList();
                grid_MCS_Command.ItemsSource = all_cmd_view_obj;
                //MCSQueueCount.Text = app.CmdBLL.getCMD_MCSIsQueueCount().ToString();
                //TotalCommandCount.Text = app.CmdBLL.getCMD_MCSTotalCount().ToString();
                MCSQueueCount.Text = app.ObjCacheManager.GetTRANSFERs()?.Where(cmd => cmd.TRANSFER_STATUS == TransferStatus.Queue)?.ToList().Count.ToString() ?? "0";
                TotalCommandCount.Text = app.ObjCacheManager.GetTRANSFERs()?.Count.ToString() ?? "0";
                bool isAutoAssign = line.IS_MCS_CMD_AUTO_ASSIGN;
                togBtn_AutoAssign.set_ToggleButton(isAutoAssign);
                if (!togBtn_AutoAssign.Toggled1) startCountdown();
                btn_AssignVh.IsEnabled = !isAutoAssign;
                btn_ShiftCmd.IsEnabled = !isAutoAssign;
                btn_ChangeStatus.IsEnabled = !isAutoAssign;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void ObjCacheManager_TransferChange(object sender, EventArgs e)
        {
            try
            {
                await Task.Delay(100);

                Adapter.Invoke((obj) =>
                {
                    refresh_MCSCMDGrp();
                    refresh_CMDCommandCount();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void refresh_CMDCommandCount()
        {
            try
            {
                //MCSQueueCount.Text = app.CmdBLL.getCMD_MCSIsQueueCount().ToString();
                //TotalCommandCount.Text = app.CmdBLL.getCMD_MCSTotalCount().ToString();
                MCSQueueCount.Text = app.ObjCacheManager.GetTRANSFERs()?.Where(cmd => cmd.TRANSFER_STATUS == TransferStatus.Queue)?.ToList().Count.ToString() ?? "0";
                TotalCommandCount.Text = app.ObjCacheManager.GetTRANSFERs()?.Count.ToString() ?? "0";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void refresh_MCSCMDGrp()
        {
            try
            {
                Object current = grid_MCS_Command.SelectedItem;
                List<ListSortDirection?> cols_sortdir = getColumnInfo(grid_MCS_Command);
                List<SortDescription> sortDescriptionList = getSortInfo(grid_MCS_Command);
                var all_cmd_view_obj = app?.ObjCacheManager.GetTRANSFERs()?.ToList();
                grid_MCS_Command.ItemsSource = all_cmd_view_obj;
                grid_MCS_Command.Items.Refresh();
                if (current != null)
                {
                    VTRANSFER pre = (VTRANSFER)current;
                    foreach (var item in grid_MCS_Command.Items)
                    {
                        VTRANSFER now = (VTRANSFER)item;

                        if (now.CMD_ID == pre.CMD_ID)
                        {
                            grid_MCS_Command.SelectedItem = item;
                            break;
                        }
                    }
                }
                setColumnInfo(grid_MCS_Command, cols_sortdir);
                setSortInfo(grid_MCS_Command, sortDescriptionList);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private List<ListSortDirection?> getColumnInfo(System.Windows.Controls.DataGrid dg)
        {
            List<ListSortDirection?> columnInfos = new List<ListSortDirection?>();
            foreach (var column in dg.Columns)
            {
                columnInfos.Add(column.SortDirection);
            }
            return columnInfos;
        }

        private List<SortDescription> getSortInfo(System.Windows.Controls.DataGrid dg)
        {
            List<SortDescription> sortInfos = new List<SortDescription>();
            foreach (var sortDescription in dg.Items.SortDescriptions)
            {
                sortInfos.Add(sortDescription);
            }
            return sortInfos;
        }

        private void setColumnInfo(System.Windows.Controls.DataGrid dg, List<ListSortDirection?> columnInfos)
        {
            //columnInfos.Sort((c1, c2) => { return c1.DisplayIndex - c2.DisplayIndex; });
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                dg.Columns[i].SortDirection = columnInfos[i];
            }
            //foreach (var columnInfo in columnInfos)
            //{
            //    var column = dg.Columns.FirstOrDefault(col => col.Header == columnInfo.Header);
            //    if (column != null)
            //    {
            //        if (columnInfo.SortDirection != null)
            //        {
            //        }
            //        column.SortDirection = columnInfo.SortDirection;
            //        column.DisplayIndex = columnInfo.DisplayIndex;
            //        column.Visibility = columnInfo.Visibility;
            //    }
            //}
        }

        private void setSortInfo(System.Windows.Controls.DataGrid dg, List<SortDescription> sortInfos)
        {
            dg.Items.SortDescriptions.Clear();
            foreach (var sortInfo in sortInfos)
            {
                dg.Items.SortDescriptions.Add(sortInfo);
            }
        }

        private void _TransferInfoChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    bool isAutoAssign = line.IS_MCS_CMD_AUTO_ASSIGN;
                    togBtn_AutoAssign.set_ToggleButton(isAutoAssign);
                    if (!togBtn_AutoAssign.Toggled1) startCountdown();
                    btn_AssignVh.IsEnabled = !isAutoAssign;
                    btn_ShiftCmd.IsEnabled = !isAutoAssign;
                    btn_ChangeStatus.IsEnabled = !isAutoAssign;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandAutoAssignChange(bool AutoAssign)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandAutoAssignChange(AutoAssign.ToString(), out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    //TipMessage_Type_Light_woBtn.Show("", "Auto Assign Changed", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandCancelAbort(string mcs_cmd)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandCancelAbort(mcs_cmd, out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Cancel Abort Succeed", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void mcsCommandForceFinish(string mcs_cmd)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                await Task.Run(() => isSuccess = app.LineBLL.SendMCSCommandForceFinish(mcs_cmd, out result));
                if (!isSuccess)
                {
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Force Finish Succeed", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender.Equals(btn_AddCommand))
                {
                    new AddCommandPopupWindow().ShowDialog();
                }
                else if (sender.Equals(btn_CancelAbort))
                {
                    if (grid_MCS_Command.SelectedItem == null)
                    {
                        TipMessage_Type_Light.Show("", "There is no selected Transfer Command.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    var confirmResult = TipMessage_Request_Light.Show("Cancel/Abort the command ?");
                    if (confirmResult != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        VTRANSFER vmcs_cmd = (VTRANSFER)grid_MCS_Command.SelectedItem;

                        mcsCommandCancelAbort(vmcs_cmd.TRANSFER_ID);
                       
                        //TipMessage_Type_Light.Show("", "Successfully command.", BCAppConstants.INFO_MSG);
                    }
                }
                else if (sender.Equals(btn_Finish))
                {
                    if (grid_MCS_Command.SelectedItem == null)
                    {
                        TipMessage_Type_Light.Show("", "There is no selected Transfer Command.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    var confirmResult = TipMessage_Request_Light.Show("Force finish the command ?");
                    if (confirmResult != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        VTRANSFER vmcs_cmd = (VTRANSFER)grid_MCS_Command.SelectedItem;
                        mcsCommandForceFinish(vmcs_cmd.TRANSFER_ID);
                        //TipMessage_Type_Light.Show("", "Successfully command.", BCAppConstants.INFO_MSG);
                    }
                }
                else if (sender.Equals(btn_AssignVh))
                {
                    if (grid_MCS_Command.SelectedItem == null)
                    {
                        TipMessage_Type_Light.Show("", "There is no Transfer Command has been selected.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    if (((VTRANSFER)grid_MCS_Command.SelectedItem).TRANSFER_STATUS != TransferStatus.Queue)
                    {
                        TipMessage_Type_Light.Show("", "Assign vehicle only for the command which transfer status is queue.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    VTRANSFER mcs_cmd = (VTRANSFER)grid_MCS_Command.SelectedItem;
                    //new AssignVehiclePopupWindow(mcs_cmd).ShowDialog();
                    if (assignVehiclePopup == null)
                        assignVehiclePopup = new AssignVehiclePopupWindow(mcs_cmd, this);
                    else
                        assignVehiclePopup.SetCmd(mcs_cmd);
                    assignVehiclePopup.BringToFront();
                }
                else if (sender.Equals(btn_ShiftCmd))
                {
                    if (grid_MCS_Command.SelectedItem == null)
                    {
                        TipMessage_Type_Light.Show("", "There is no Transfer Command has been selected.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    //if (((TransferCMDViewObj)grid_MCS_Command.SelectedItem).TRANSFERSTATE != E_TRAN_STATUS.Initial)
                    //{
                    //    TipMessage_Type_Light.Show("", "Shift command only for the command which transfer status is initial.", BCAppConstants.WARN_MSG);
                    //    return;
                    //}
                    if (string.IsNullOrWhiteSpace(((VTRANSFER)grid_MCS_Command.SelectedItem).VH_ID))
                    {
                        TipMessage_Type_Light.Show("", "Shift command only for the command which already assign vehicle.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    VTRANSFER mcs_cmd = (VTRANSFER)grid_MCS_Command.SelectedItem;
                    //new ShiftCommandPopupWindow(mcs_cmd).ShowDialog();
                    if (shiftCommandPopup == null)
                        shiftCommandPopup = new ShiftCommandPopupWindow(mcs_cmd, this);
                    else
                        shiftCommandPopup.SetCmd(mcs_cmd);
                    shiftCommandPopup.BringToFront();
                }
                else if (sender.Equals(btn_ChangeStatus))
                {
                    if (grid_MCS_Command.SelectedItem == null)
                    {
                        TipMessage_Type_Light.Show("", "There is no Transfer Command has been selected.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    VTRANSFER mcs_cmd = (VTRANSFER)grid_MCS_Command.SelectedItem;
                    //new ChangeStatusPopupWindow(mcs_cmd).ShowDialog();
                    if (changeStatusPopup == null)
                        changeStatusPopup = new ChangeStatusPopupWindow(mcs_cmd, this);
                    else
                        changeStatusPopup.SetCmd(mcs_cmd);
                    changeStatusPopup.BringToFront();
                }
                else if (sender.Equals(btn_ChangePriorty))
                {
                    if (grid_MCS_Command.SelectedItem == null)
                    {
                        TipMessage_Type_Light.Show("", "There is no Transfer Command has been selected.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    if (((VTRANSFER)grid_MCS_Command.SelectedItem).TRANSFER_STATUS != TransferStatus.Queue)
                    {
                        TipMessage_Type_Light.Show("", "Priority Change only for the command which transfer status is queue.", BCAppConstants.WARN_MSG);
                        return;
                    }
                    //ChangePriorityPopupForm changePriority = new ChangePriorityPopupForm(((VTRANSFER)grid_MCS_Command.SelectedItem).CMD_ID.Trim());
                    //changePriority.ShowDialog();

                    string mcs_cmd = ((VTRANSFER)grid_MCS_Command.SelectedItem).TRANSFER_ID.Trim();
                    //new ChangePriorityPopupWindow(mcs_cmd).ShowDialog();
                    if (changePriorityPopup == null)
                        changePriorityPopup = new ChangePriorityPopupWindow(mcs_cmd, this);
                    else
                        changePriorityPopup.SetCmdID(mcs_cmd);
                    changePriorityPopup.BringToFront();
                }
                else if (sender.Equals(btn_Export))
                {
                    if (CsvUtility.exportLotDataToCSV(app.ObjCacheManager.GetTRANSFERs()))
                    {
                        TipMessage_Type_Light_woBtn.Show("", "Export data completed.", BCAppConstants.INFO_MSG);
                    }
                    else
                    {
                        TipMessage_Type_Light.Show("", "Export data failed.", BCAppConstants.WARN_MSG);
                    }
                }
                else if (sender.Equals(btn_Close))
                {
                    if (togBtn_AutoAssign.Toggled1 == false)
                        mcsCommandAutoAssignChange(true);
                    CloseEvent?.Invoke(this, e);
                }
            }
            catch (Exception ex)
            { logger.Error(ex, "Exception"); }
        }

        public void PopupWindowClosed(object popup)
        {
            try
            {
                AssignVehiclePopupWindow _assignVehiclePopup = popup as AssignVehiclePopupWindow;
                if (_assignVehiclePopup != null) { assignVehiclePopup = null; return; }

                ShiftCommandPopupWindow _shiftCommandPopup = popup as ShiftCommandPopupWindow;
                if (_shiftCommandPopup != null) { shiftCommandPopup = null; return; }

                ChangeStatusPopupWindow _changeStatusPopup = popup as ChangeStatusPopupWindow;
                if (_changeStatusPopup != null) { changeStatusPopup = null; return; }

                ChangePriorityPopupWindow _changePriorityPopup = popup as ChangePriorityPopupWindow;
                if (_changePriorityPopup != null) { changePriorityPopup = null; return; }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void togBtn_AutoAssign_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (togBtn_AutoAssign.Toggled1 == true)
                {
                    btn_AssignVh.IsEnabled = false;
                    btn_ShiftCmd.IsEnabled = false;
                    btn_ChangeStatus.IsEnabled = false;
                    mcsCommandAutoAssignChange(true);
                    closeSubPage();
                    stopCountdown();
                }
                else
                {
                    btn_AssignVh.IsEnabled = true;
                    btn_ShiftCmd.IsEnabled = true;
                    btn_ChangeStatus.IsEnabled = true;
                    mcsCommandAutoAssignChange(false);
                    startCountdown();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public class MCSCommandAutoAssignUpdateEventArgs : EventArgs
        {
            public MCSCommandAutoAssignUpdateEventArgs(string autoAssign)
            {
                try
                {
                    this.autoAssign = autoAssign;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                }
            }

            public string autoAssign { get; private set; }
        }

        public class MCSCommandCancelAbortEventArgs : EventArgs
        {
            public MCSCommandCancelAbortEventArgs(string mcs_cmd)
            {
                this.mcs_cmd = mcs_cmd;
            }

            public string mcs_cmd
            {
                get;
                private set;
            }
        }

        public class MCSCommandForceFinishEventArgs : EventArgs
        {
            public MCSCommandForceFinishEventArgs(string mcs_cmd)
            {
                this.mcs_cmd = mcs_cmd;
            }

            public string mcs_cmd
            {
                get;
                private set;
            }
        }
    }
}
