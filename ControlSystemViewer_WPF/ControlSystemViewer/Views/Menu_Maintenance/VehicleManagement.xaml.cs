//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

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
using UtilsAPI.Common;
using ViewerObject;
using static ViewerObject.VVEHICLE_Def;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    /// <summary>
    /// VehicleManagement.xaml 的互動邏輯
    /// </summary>
    public partial class VehicleManagement : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private VLINE line = null;
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public VehicleManagement()
        {
            InitializeComponent();
        }

        public void StartupUI()
        {
            try
            {
                app = WindownApplication.getInstance();
                line = app.ObjCacheManager.GetLine();
                initTitle();
                init();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Close()
        {
            try
            {
                unregisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void initTitle()
        {
            try
            {
                VehicleStatus.SetTXBTitleName("Vehicle ID", "Mode", "Current Address", "Current Section", "Section Distance", "Alarm Count", "Instal/Remove");
                CommandStatus.SetTXBTitleName("Action Status", "MCS Command ID", "Vehicle Command ID", "Command Type", "Carrier ID", "Source", "Destination");
                //VehicleStatus.SetBTNTitleName("Auto Remote", "Auto Local", "Auto MTL", "Manual", "Alarm Reset"); // oht
                VehicleStatus.SetBTNTitleName("Auto Remote", "Auto Local", "Manual", "Alarm Reset", "Install", "Remove");
                CommandStatus.SetBTNTitleName("Cancel/Abort Cmd", "Force finish Cmd");
                PauseType.SetTXBTitleName("Error Status", "Normal Pause", "Block Pause", "Obstacle Pause", "HID Pause", "Safety Pause", "Earthquake Pause", "Pause Type");
                PauseType.SetBTNTitleName("Pause", "Continue");
                VehicleCommand.SetTXBTitleName("Vehicle ID", "Command Type", "Carrier ID", "Source", "Destination");
                VehicleCommand.SetBTNTitleName("Command");

                var vhs = app.ObjCacheManager.GetVEHICLEs();
                var vhlk = new[] { uc_VhLk_Status_1, uc_VhLk_Status_2, uc_VhLk_Status_3, uc_VhLk_Status_4, uc_VhLk_Status_5, uc_VhLk_Status_6, uc_VhLk_Status_7, uc_VhLk_Status_8, uc_VhLk_Status_9, uc_VhLk_Status_10, uc_VhLk_Status_11, uc_VhLk_Status_12, uc_VhLk_Status_13, uc_VhLk_Status_14, };
                var ohct = line.IS_CONNECT_VEHICLEs;
                for (int i = 0; i < vhs.Count(); i++)
                {
                    vhlk[i].SetConnStatus(vhs[i].VEHICLE_ID, ohct[i]);
                }
                for (int i = vhs.Count(); i < 14; i++)
                {
                    vhlk[i].Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void init()
        {
            registerEvent();

            combo_VehicleID.Items.Clear();
            foreach (var vh in app.ObjCacheManager.GetVEHICLEs())
            {
                combo_VehicleID.Items.Add(vh.VEHICLE_ID);
            }

            PauseType.combo_Content.ItemsSource = Enum.GetValues(typeof(VPauseType)).Cast<VPauseType>();

            VehicleCommand.combo_Content1.ItemsSource = Enum.GetValues(typeof(E_UICMD_TYPE)).Cast<E_UICMD_TYPE>();
            foreach (ViewerObject.Address adr in app.ObjCacheManager.Addresses)
            {
                string s_address = adr.ID;
                try
                {
                    VPORTSTATION port = app.ObjCacheManager.GetPortStation(s_address);
                    if (port != null)
                    {
                        s_address = s_address + $" ({port.PORT_ID.Trim()})";
                    }
                    VehicleCommand.combo_Content2.Items.Add(s_address);
                    VehicleCommand.combo_Content3.Items.Add(s_address);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                }
            }
            combo_VehicleID.SelectedIndex = 0;
            PauseType.combo_Content.SelectedIndex = 0;
            VehicleCommand.combo_Content1.SelectedIndex = 0;
            //VehicleCommand.combo_Content1.IsEnabled = false; // AGV Carrier 有分左右，暫時關閉 Move 以外的命令類型
            VehicleCommand.combo_Content2.SelectedIndex = 0;
            VehicleCommand.combo_Content3.SelectedIndex = 0;

            var vh_alarms = app.ObjCacheManager.GetAlarms().Where(a => a.EQPT_ID == combo_VehicleID.Text.Trim()).ToList();
            txb_AlarmCnt.Text = vh_alarms.Count.ToString();
            alarmlist.ItemsSource = vh_alarms;


        }

        private void registerEvent()
        {
            try
            {
                PauseType.btn_Title1.Click += btn_Click;
                PauseType.btn_Title2.Click += btn_Click;
                VehicleStatus.btn_Title1.Click += btn_Click;
                VehicleStatus.btn_Title2.Click += btn_Click;
                VehicleStatus.btn_Title3.Click += btn_Click;
                VehicleStatus.btn_Title4.Click += btn_Click;
                VehicleStatus.btn_Title5.Click += btn_Click;
                VehicleStatus.btn_Title6.Click += btn_Click;

                VehicleCommand.btn_Title1.Click += btn_Click;
                CommandStatus.btn_Title1.Click += btn_Click;
                CommandStatus.btn_Title2.Click += btn_Click;

                VehicleCommand.combo_Content1.SelectionChanged += commandTypeSelecttionChanged;
                PauseType.combo_Content.SelectionChanged += pauseSelecttionChanged;
                combo_VehicleID.SelectionChanged += vehicleSelecttionChanged;

                app.ObjCacheManager.VehicleUpdateComplete += ObjCacheManager_VehicleUpdateComplete;
                app.ObjCacheManager.AlarmChange += ObjCacheManager_AlarmChange;

                line.PingCheckInfoChange += _PingCheckInfoChange;

                string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
                if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
                app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO + ChannelBay, app.LineBLL.PingCheckInfo);


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
                PauseType.btn_Title1.Click -= btn_Click;
                PauseType.btn_Title2.Click -= btn_Click;
                VehicleStatus.btn_Title1.Click -= btn_Click;
                VehicleStatus.btn_Title2.Click -= btn_Click;
                VehicleStatus.btn_Title3.Click -= btn_Click;
                VehicleStatus.btn_Title4.Click -= btn_Click;
                VehicleStatus.btn_Title5.Click -= btn_Click;
                VehicleStatus.btn_Title6.Click -= btn_Click;

                VehicleCommand.btn_Title1.Click -= btn_Click;
                CommandStatus.btn_Title1.Click -= btn_Click;

                VehicleCommand.combo_Content1.SelectionChanged -= commandTypeSelecttionChanged;
                PauseType.combo_Content.SelectionChanged -= pauseSelecttionChanged;
                combo_VehicleID.SelectionChanged -= vehicleSelecttionChanged;

                app.ObjCacheManager.VehicleUpdateComplete -= ObjCacheManager_VehicleUpdateComplete;
                app.ObjCacheManager.AlarmChange -= ObjCacheManager_AlarmChange;

                line.PingCheckInfoChange -= _PingCheckInfoChange;
                string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
                if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
                app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO + ChannelBay);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void _PingCheckInfoChange(object sender, EventArgs e)
        {
            var vhs = app.ObjCacheManager.GetVEHICLEs();
            try
            {
                Adapter.Invoke((obj) =>
                {
                    var vhlk = new[] { uc_VhLk_Status_1, uc_VhLk_Status_2, uc_VhLk_Status_3, uc_VhLk_Status_4, uc_VhLk_Status_5, uc_VhLk_Status_6, uc_VhLk_Status_7, uc_VhLk_Status_8, uc_VhLk_Status_9, uc_VhLk_Status_10, uc_VhLk_Status_11, uc_VhLk_Status_12, uc_VhLk_Status_13, uc_VhLk_Status_14, };
                    var ohct = line.IS_CONNECT_VEHICLEs;
                    for (int i = 0; i < vhs.Count(); i++)
                    {
                        vhlk[i].SetConnStatus(vhs[i].VEHICLE_ID, ohct[i]);
                    }
                    for (int i = vhs.Count(); i < 14; i++)
                    {
                        vhlk[i].Visibility = Visibility.Collapsed;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void refreshUI()
        {
            try
            {
                string select_vh_id = (string)combo_VehicleID.SelectedItem;

                if (!string.IsNullOrEmpty(select_vh_id))
                {
                    var select_vh = app.ObjCacheManager.GetVEHICLE(select_vh_id);
                    //ACMD_MCS mcs_cmd = app.CmdBLL.GetCmd_MCSByID(select_vh.MCS_CMD);
                    //ACMD ohxc_cmd = app.CmdBLL.GetExecuteCmd_OhtcByID(select_vh.VEHICLE_ID);
                    var ctl_cmd = app.CmdBLL.GetExecuteCmdByVhID(select_vh.VEHICLE_ID);
                    VehicleStatus.SetTXBVehicleInfo(select_vh.VEHICLE_ID,
                                                    select_vh.MODE_STATUS.ToString(),
                                                    select_vh.CUR_ADR_ID,
                                                    select_vh.CUR_SEC_ID,
                                                    select_vh.ACC_SEC_DIST.ToString(),
                    app.ObjCacheManager.getAlarmCountByVehicleID(select_vh_id).ToString(),
                                                    select_vh.INSTALL_STATUS.ToString());
                    PauseType.SetTXBPauseInfo(select_vh.HAS_ERROR ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_BLOCK ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_OBS ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_HID ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_SAFETY_DOOR ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_EARTHQUAKE ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF);
                    CommandStatus.SetVehicleCmdInfo(select_vh.ACT_STATUS.ToString(),
                                                    //select_vh.MCS_CMD,
                                                    //select_vh.OHTC_CMD,
                                                    ctl_cmd != null ? ctl_cmd.TRANSFER_ID.ToString() : string.Empty,
                                                    ctl_cmd != null ? ctl_cmd.CMD_ID.ToString() : string.Empty,
                                                    ctl_cmd != null ? ctl_cmd.CMD_TYPE.ToString() : string.Empty,
                                                    ctl_cmd != null ? ctl_cmd.CARRIER_ID : string.Empty,
                                                    ctl_cmd != null ? ctl_cmd.SOURCE : string.Empty,
                                                    ctl_cmd != null ? ctl_cmd.DESTINATION : string.Empty);
                    VPORTSTATION port = app.ObjCacheManager.GetPortStation(select_vh.CUR_ADR_ID);
                    string addr = null;
                    if (port == null) //20210609 markchou 避免該Address沒有對應Port導致Exception發生
                    {
                        addr = $"{select_vh.CUR_ADR_ID?.Trim()}";
                    }
                    else
                    {
                        addr = $"{select_vh.CUR_ADR_ID?.Trim()} ({port.PORT_ID.Trim()})";
                    }
                    VehicleCommand.SetVehicleCommandInfo(select_vh_id, select_vh.CST_ID_L?.Trim(), addr);
                    VPauseType pause_type = (VPauseType)PauseType.combo_Content.SelectedItem;
                    AnaPauseType(pause_type);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void vehicleSelecttionChanged(object sender, EventArgs e)
        {
            try
            {
                string select_vh_id = (string)combo_VehicleID.SelectedItem;

                if (!string.IsNullOrEmpty(select_vh_id))
                {
                    var vh_alarms = app.ObjCacheManager.GetAlarms().Where(a => a.EQPT_ID == select_vh_id.Trim()).ToList();
                    txb_AlarmCnt.Text = vh_alarms.Count.ToString();
                    alarmlist.ItemsSource = vh_alarms;
#if DISPLAYCOMMUNICATIONLOGPAGE
					CommunLog_VhID.Text = select_vh_id;
#endif

                    var select_vh = app.ObjCacheManager.GetVEHICLE(select_vh_id);
                    //ACMD_MCS mcs_cmd = app.CmdBLL.GetCmd_MCSByID(select_vh.MCS_CMD);
                    //ACMD ohxc_cmd = app.CmdBLL.GetCmd_OhtcByID(select_vh.CMD_ID);
                    var ctl_cmd = app.CmdBLL.GetExecuteCmdByVhID(select_vh.VEHICLE_ID);
                    VehicleStatus.SetTXBVehicleInfo(select_vh.VEHICLE_ID, select_vh.MODE_STATUS.ToString(), select_vh.CUR_ADR_ID
                        , select_vh.CUR_SEC_ID, select_vh.ACC_SEC_DIST.ToString(), app.ObjCacheManager.getAlarmCountByVehicleID(select_vh_id).ToString(), select_vh.INSTALL_STATUS.ToString());
                    PauseType.SetTXBPauseInfo(select_vh.HAS_ERROR ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_BLOCK ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_OBS ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_HID ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_SAFETY_DOOR ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF,
                                              select_vh.IS_PAUSE_EARTHQUAKE ? BCAppConstants.SIGNAL_ON : BCAppConstants.SIGNAL_OFF);
                    CommandStatus.SetVehicleCmdInfo(select_vh.ACT_STATUS.ToString(),
                    $"{select_vh.TRANSFER_ID_1}",
                    $"{select_vh.CMD_ID_1}",
                    ctl_cmd != null ? ctl_cmd.CMD_TYPE.ToString() : string.Empty, ctl_cmd != null ? ctl_cmd.CARRIER_ID : string.Empty, ctl_cmd != null ? ctl_cmd.SOURCE : string.Empty, ctl_cmd != null ? ctl_cmd.DESTINATION : string.Empty);
                    VPORTSTATION port = app.ObjCacheManager.GetPortStation(select_vh.CUR_ADR_ID);

                    string addr = $"{select_vh.CUR_ADR_ID?.Trim()} ({port?.PORT_ID ?? ""})";
                    VehicleCommand.SetVehicleCommandInfo(select_vh_id, select_vh.CST_ID_L?.Trim(), addr);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void commandTypeSelecttionChanged(object sender, EventArgs e)
        {
            try
            {
                E_UICMD_TYPE select_cmd_type = (E_UICMD_TYPE)VehicleCommand.combo_Content1.SelectedItem;
                switch (select_cmd_type)
                {
                    case E_UICMD_TYPE.Move:
                        VehicleCommand.combo_Content2.IsEnabled = false;
                        VehicleCommand.combo_Content3.IsEnabled = true;
                        break;

                    case E_UICMD_TYPE.Load:
                        VehicleCommand.combo_Content2.IsEnabled = true;
                        VehicleCommand.combo_Content3.IsEnabled = false;
                        break;

                    case E_UICMD_TYPE.Unload:
                        VehicleCommand.combo_Content2.IsEnabled = false;
                        VehicleCommand.combo_Content3.IsEnabled = true;
                        break;

                    case E_UICMD_TYPE.LoadUnload:
                        VehicleCommand.combo_Content2.IsEnabled = true;
                        VehicleCommand.combo_Content3.IsEnabled = true;
                        break;

                    //case E_UICMD_TYPE.Scan:
                    //	VehicleCommand.combo_Content2.IsEnabled = false;
                    //	VehicleCommand.combo_Content3.IsEnabled = true;
                    //	break;

                    default:
                        VehicleCommand.combo_Content2.IsEnabled = true;
                        VehicleCommand.combo_Content3.IsEnabled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void pauseSelecttionChanged(object sender, EventArgs e)
        {
            try
            {
                VPauseType pause_type = (VPauseType)PauseType.combo_Content.SelectedItem;
                AnaPauseType(pause_type);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void AnaPauseType(VPauseType _pauseType)
        {
            try
            {
                if (_pauseType is VPauseType pause_type)
                {
                    switch (pause_type)
                    {
                        //case VPauseType.OhxC:
                        //	if (PauseType.txb_Value2.Text == BCAppConstants.SIGNAL_OFF)
                        //	{
                        //		PauseType.btn_Title1.IsEnabled = true;
                        //		PauseType.btn_Title2.IsEnabled = false;
                        //	}
                        //	else
                        //	{
                        //		PauseType.btn_Title1.IsEnabled = false;
                        //		PauseType.btn_Title2.IsEnabled = true;
                        //	}
                        //	break;

                        case VPauseType.Block:
                            if (PauseType.txb_Value3.Text == BCAppConstants.SIGNAL_OFF)
                            {
                                PauseType.btn_Title1.IsEnabled = true;
                                PauseType.btn_Title2.IsEnabled = false;
                            }
                            else
                            {
                                PauseType.btn_Title1.IsEnabled = false;
                                PauseType.btn_Title2.IsEnabled = true;
                            }
                            break;

                            //case sc.App.SCAppConstants.OHxCPauseType.Hid:
                            //	if (PauseType.txb_Value5.Text == BCAppConstants.SIGNAL_OFF)
                            //	{
                            //		PauseType.btn_Title1.IsEnabled = true;
                            //		PauseType.btn_Title2.IsEnabled = false;
                            //	}
                            //	else
                            //	{
                            //		PauseType.btn_Title1.IsEnabled = false;
                            //		PauseType.btn_Title2.IsEnabled = true;
                            //	}
                            //	break;

                            //case sc.App.SCAppConstants.OHxCPauseType.Safty:
                            //	if (PauseType.txb_Value6.Text == BCAppConstants.SIGNAL_OFF)
                            //	{
                            //		PauseType.btn_Title1.IsEnabled = true;
                            //		PauseType.btn_Title2.IsEnabled = false;
                            //	}
                            //	else
                            //	{
                            //		PauseType.btn_Title1.IsEnabled = false;
                            //		PauseType.btn_Title2.IsEnabled = true;
                            //	}
                            //	break;

                            //case sc.App.SCAppConstants.OHxCPauseType.Earthquake:
                            //	if (PauseType.txb_Value7.Text == BCAppConstants.SIGNAL_OFF)
                            //	{
                            //		PauseType.btn_Title1.IsEnabled = true;
                            //		PauseType.btn_Title2.IsEnabled = false;
                            //	}
                            //	else
                            //	{
                            //		PauseType.btn_Title1.IsEnabled = false;
                            //		PauseType.btn_Title2.IsEnabled = true;
                            //	}
                            //	break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void pauseStatusChange(string vh_id, VPauseType pauseType, VPauseEvent event_type)
        {
            try
            {
                if (!app.VehicleBLL.SendPauseStatusChange(vh_id, pauseType, event_type, out string result))
                {
                    TipMessage_Type_Light.Show("Pause failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Pause succeeded", "", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void modeStatusChange(string vh_id, ModeStatus modeStatus)
        {
            try
            {
                if (!app.VehicleBLL.SendModeStatusChange(vh_id, modeStatus, out string result))
                {
                    TipMessage_Type_Light.Show("Change status failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Change status succeed", "", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void installStatusChange(string vh_id, bool isInstall)
        {
            try
            {
                if (!app.VehicleBLL.SendInstallStatusChange(vh_id, isInstall, out string result))
                {
                    TipMessage_Type_Light.Show("Change status failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Change status succeed", "", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void vehicleCommandSend(string vh_id, string cmd_type, string carrier_id, string source, string destination)
        {
            try
            {
                bool isSuccess = false;
                string result = "";
                await Task.Run(() => isSuccess = app.VehicleBLL.SendCmdToControl(vh_id, cmd_type, carrier_id, source, destination, ref result));
                if (!isSuccess)
                {
                    //MessageBox.Show(result);
                    TipMessage_Type_Light.Show("Send command failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Send command succeeded", "", BCAppConstants.INFO_MSG);
                }
                //app.LineBLL.SendHostModeChange(e.host_mode);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void vehicleResetSend(string vh_id)
        {
            try
            {
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, nameof(VehicleManagement), $"Excute vh:{vh_id} reset ...");
                string result = "";
                if (!app.VehicleBLL.SendVehicleResetToControl(vh_id, out result))
                {
                    TipMessage_Type_Light.Show("Send reset failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Send reset succeeded", "", BCAppConstants.INFO_MSG);
                }
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, nameof(VehicleManagement), $"Excute vh:{vh_id} reset, action result:{result}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void vehicleCommandCancelAbortSend(string vh_id)
        {
            try
            {
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, nameof(VehicleManagement), $"Excute vh:{vh_id} cancel/abort command...");
                if (!app.VehicleBLL.SendVehicleCMDCancelAbortToControl(vh_id, out string result))
                {
                    TipMessage_Type_Light.Show("Send Cancel/Abort failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Send Cancel/Abort succeed", "", BCAppConstants.INFO_MSG);
                }
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, nameof(VehicleManagement), $"Excute vh:{vh_id} cancel/abort command,action resule:{result}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void vehicleCommandForceFinishSend(string vh_id)
        {
            try
            {
                string result = "";
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, nameof(VehicleManagement), $"Excute vh:{vh_id} force finish command...");
                if (!app.CmdBLL.ForceUpdataCmdStatus2FnishByVhID(vh_id))
                {
                    TipMessage_Type_Light.Show("Send force finish failed", "", BCAppConstants.INFO_MSG);
                    result = "NG";
                }
                else
                {
                    TipMessage_Type_Light.Show("Send force finish succeed", "", BCAppConstants.INFO_MSG);
                    result = "OK";
                }
                app.OperationHistoryBLL.addOperationHis(app.LoginUserID, nameof(VehicleManagement), $"Excute vh:{vh_id} force finish command,action resule:{result}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void vehicleAlarmResetSend(string vh_id)
        {
            try
            {
                if (!app.VehicleBLL.SendVehicleAlarmResetRequest(vh_id, out string result))
                {
                    TipMessage_Type_Light.Show("Send vehicle alarm reset failed", result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show("Send vehicle alarm reset succeed", "", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_VehicleUpdateComplete(object sender, string vh_id)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refreshUI();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_AlarmChange(object obj, EventArgs e)
        {
            try
            {
                Adapter.Invoke((o) =>
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    var vh_alarms = app.ObjCacheManager.GetAlarms()?.Where(a => a.EQPT_ID == select_vh_id.Trim()).ToList();
                    txb_AlarmCnt.Text = (vh_alarms?.Count ?? 0).ToString();
                    alarmlist.ItemsSource = vh_alarms;
                    refreshUI();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult = TipMessage_Request_Light.Show("Are you sure?");
            if (confirmResult != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (sender.Equals(btn_reset))
                {
                    //foreach (string vh_id in app.ObjCacheManager.hasAlarmVehicleList())
                    //{
                    //    vehicleAlarmResetSend(vh_id);
                    //}
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    vehicleAlarmResetSend(select_vh_id);
                }
                else if (sender.Equals(VehicleStatus.btn_Title1))//AutoRemote
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    modeStatusChange(select_vh_id, ModeStatus.AutoRemote);
                }
                else if (sender.Equals(VehicleStatus.btn_Title2))//AutoLocal
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    modeStatusChange(select_vh_id, ModeStatus.AutoLocal);
                }
                //else if (sender.Equals(VehicleStatus.btn_Title3))//AutoMTL
                //{
                //	string select_vh_id = (string)combo_VehicleID.SelectedItem;
                //	modeStatusChange(select_vh_id, VHModeStatus.AutoMtl);
                //}
                //else if (sender.Equals(VehicleStatus.btn_Title4))//AutoMTS
                //{
                //    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                //    modeStatusChange(select_vh_id, VHModeStatus.AutoMts);
                //}
                //else if (sender.Equals(VehicleStatus.btn_Title4))//Manual
                //{
                //	string select_vh_id = (string)combo_VehicleID.SelectedItem;
                //	modeStatusChange(select_vh_id, VHModeStatus.Manual);
                //}
                //else if (sender.Equals(VehicleStatus.btn_Title5))//alarm reset
                //{
                //	string select_vh_id = (string)combo_VehicleID.SelectedItem;
                //	vehicleAlarmResetSend(select_vh_id);
                //}
                else if (sender.Equals(VehicleStatus.btn_Title3))//Manual
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    modeStatusChange(select_vh_id, ModeStatus.Manual);
                }
                else if (sender.Equals(VehicleStatus.btn_Title4))//alarm reset
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    vehicleAlarmResetSend(select_vh_id);
                }
                else if (sender.Equals(VehicleStatus.btn_Title5))//Vehicle install
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    installStatusChange(select_vh_id, true);
                }
                else if (sender.Equals(VehicleStatus.btn_Title6))//vehicle remove
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    installStatusChange(select_vh_id, false);
                }
                else if (sender.Equals(PauseType.btn_Title1))
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    VPauseType pause_type = (VPauseType)PauseType.combo_Content.SelectedItem;
                    pauseStatusChange(select_vh_id, pause_type, VPauseEvent.Pause);
                }
                else if (sender.Equals(PauseType.btn_Title2))
                {
                    string select_vh_id = (string)combo_VehicleID.SelectedItem;
                    VPauseType pause_type = (VPauseType)PauseType.combo_Content.SelectedItem;
                    pauseStatusChange(select_vh_id, pause_type, VPauseEvent.Continue);
                }
                else if (sender.Equals(VehicleCommand.btn_Title1))
                {
                    string select_vh_id = VehicleCommand.txt_Content1.Text;
                    string cmd_type = VehicleCommand.combo_Content1.Text;
                    string carrier = VehicleCommand.txt_Content2.Text;
                    string source = ((string)VehicleCommand.combo_Content2.SelectedItem).Split(' ')[0];
                    string destination = ((string)VehicleCommand.combo_Content3.SelectedItem).Split(' ')[0];

                    vehicleCommandSend(select_vh_id, cmd_type, carrier, source, destination);
                }
                else if (sender.Equals(btn_VhReset))
                {
                    string select_vh_id = VehicleCommand.txt_Content1.Text;
                    vehicleResetSend(select_vh_id);
                }
                else if (sender.Equals(CommandStatus.btn_Title1))
                {
                    string select_vh_id = VehicleCommand.txt_Content1.Text;
                    vehicleCommandCancelAbortSend(select_vh_id);
                }
                else if (sender.Equals(CommandStatus.btn_Title2))
                {
                    string select_vh_id = VehicleCommand.txt_Content1.Text;
                    vehicleCommandForceFinishSend(select_vh_id);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseEvent?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public enum E_UICMD_TYPE
        {
            Move = 0,
            Load = 3,
            Unload = 4,
            LoadUnload = 5
            //Scan = 15
        }
    }
}
