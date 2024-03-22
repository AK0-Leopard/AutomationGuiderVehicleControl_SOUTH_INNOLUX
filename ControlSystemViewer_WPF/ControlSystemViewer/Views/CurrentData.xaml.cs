using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using NLog;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using ViewerObject.LOG;

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// CurrentData.xaml 的互動邏輯
    /// </summary>
    public partial class CurrentData : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        public EventHandler<string> SelectedVhChanged;
        string SelectedVhID = null;
        #endregion 公用參數設定

        #region Commu Log
        private List<VEHICLE_COMMU_LOG> vhCommuLogs = new List<VEHICLE_COMMU_LOG>();
        private List<MCS_COMMU_LOG> mcsCommuLogs = new List<MCS_COMMU_LOG>();
        private List<SYSTEM_PROCESS_LOG> plcCommuLogs = new List<SYSTEM_PROCESS_LOG>();
        private List<SYSTEM_PROCESS_LOG> systemProcLogs = new List<SYSTEM_PROCESS_LOG>();
        private readonly int MAX_NUMBER_OF_COMMU_LOGS = 1000; // max of list count
        private DispatcherTimer timer_RefreshCommuLogs = new DispatcherTimer();
        private readonly int MILLISECONDS_REFRESH_COMMU_LOG = 1000; // timer interval
        private bool doFilter_VhCommuLogs = false;
        private bool doFilter_SystemProcLogs = false;

        private List<VEHICLE_COMMU_LOG> getVhCommuLogs()
        {
            return vhCommuLogs;
        }
        private bool addVhCommuLog(List<VEHICLE_COMMU_LOG> logs)
        {
            if (doFilter_VhCommuLogs)
            {
                if (!string.IsNullOrWhiteSpace(grid_VehicleCommu.cbx_VhID.Text))
                {
                    vhCommuLogs = vhCommuLogs.Where(l => l.VhID.Contains(grid_VehicleCommu.cbx_VhID.Text)).ToList();
                }
            }

            if (logs == null || logs.Count == 0) return false;
            var filtered_logs = logs.Where(l =>
            {
                bool b = true;
                if (!(bool)grid_VehicleCommu.ckb_Load134.IsChecked &&
                    (l.FunName.Contains("ID_34") || l.FunName.Contains("ID_134")))
                {
                    b = false;
                }
                else if (!(bool)grid_VehicleCommu.ckb_Load143.IsChecked &&
                    (l.FunName.Contains("ID_43") || l.FunName.Contains("ID_143")))
                {
                    b = false;
                }
                else if (!(bool)grid_VehicleCommu.ckb_Load144.IsChecked &&
                    (l.FunName.Contains("ID_44") || l.FunName.Contains("ID_144")))
                {
                    b = false;
                }
                return b && (string.IsNullOrWhiteSpace(grid_VehicleCommu.cbx_VhID.Text) || l.VhID.Contains(grid_VehicleCommu.cbx_VhID.Text));
            })?.ToList();
            if (filtered_logs == null || filtered_logs.Count == 0) return false;
            vhCommuLogs.InsertRange(0, filtered_logs);
            if (vhCommuLogs.Count > MAX_NUMBER_OF_COMMU_LOGS)
                vhCommuLogs.RemoveRange(MAX_NUMBER_OF_COMMU_LOGS, vhCommuLogs.Count - MAX_NUMBER_OF_COMMU_LOGS);
            return true;
        }

        private List<MCS_COMMU_LOG> getMcsCommuLogs()
        {
            return mcsCommuLogs;
        }
        private bool addMcsCommuLog(List<MCS_COMMU_LOG> logs)
        {
            if (logs == null || logs.Count == 0) return false;
            mcsCommuLogs.InsertRange(0, logs);
            if (mcsCommuLogs.Count > MAX_NUMBER_OF_COMMU_LOGS)
                mcsCommuLogs.RemoveRange(MAX_NUMBER_OF_COMMU_LOGS, mcsCommuLogs.Count - MAX_NUMBER_OF_COMMU_LOGS);
            return true;
        }

        private List<SYSTEM_PROCESS_LOG> getPlcCommuLogs()
        {
            return plcCommuLogs;
        }
        private bool addPlcCommuLog(List<SYSTEM_PROCESS_LOG> logs)
        {
            if (logs == null || logs.Count == 0) return false;
            var filtered_logs = logs.Where(l =>
            {
                bool b = false;
                if (l.Device?.ToUpper().Contains("CHARGER") ?? false)
                {
                    b = true;
                }
                return b;
            })?.ToList();
            if (filtered_logs == null || filtered_logs.Count == 0) return false;
            plcCommuLogs.InsertRange(0, filtered_logs);
            if (plcCommuLogs.Count > MAX_NUMBER_OF_COMMU_LOGS)
                plcCommuLogs.RemoveRange(MAX_NUMBER_OF_COMMU_LOGS, plcCommuLogs.Count - MAX_NUMBER_OF_COMMU_LOGS);
            return true;
        }

        private List<SYSTEM_PROCESS_LOG> getSystemProcLogs()
        {
            return systemProcLogs;
        }
        private bool addSystemProcLog(List<SYSTEM_PROCESS_LOG> logs)
        {
            //if (doFilter_SystemProcLogs)
            //{
            //    systemProcLogs = systemProcLogs.Where(l => l.LogLevel >= (Definition.LogLevel)grid_SysProcLog.cbx_LogLevel.SelectedItem).ToList();
            //    if (!string.IsNullOrWhiteSpace(grid_SysProcLog.cbx_VhID.Text))
            //    {
            //        systemProcLogs = systemProcLogs.Where(l => l.VhID.Contains(grid_SysProcLog.cbx_VhID.Text)).ToList();
            //    }
            //}

            if (logs == null || logs.Count == 0) return false;
            var filtered_logs = logs.Where(l =>
            {
                return l.LogLevel >= (Definition.LogLevel)grid_SysProcLog.cbx_LogLevel.SelectedItem &&
                       (string.IsNullOrWhiteSpace(grid_SysProcLog.cbx_VhID.Text) || l.VhID.Contains(grid_SysProcLog.cbx_VhID.Text));
            })?.ToList();
            if (filtered_logs == null || filtered_logs.Count == 0) return false;
            systemProcLogs.InsertRange(0, filtered_logs);
            if (systemProcLogs.Count > MAX_NUMBER_OF_COMMU_LOGS)
                systemProcLogs.RemoveRange(MAX_NUMBER_OF_COMMU_LOGS, systemProcLogs.Count - MAX_NUMBER_OF_COMMU_LOGS);
            return true;
        }

        private void initRefreshCommuLogs()
        {
            timer_RefreshCommuLogs.Interval = TimeSpan.FromMilliseconds(MILLISECONDS_REFRESH_COMMU_LOG);
            timer_RefreshCommuLogs.Tick += tick_RefreshCommuLogs;
        }

        private void tick_RefreshCommuLogs(object sender, EventArgs e)
        {
            if (!timer_RefreshCommuLogs.IsEnabled) return;
            timer_RefreshCommuLogs.IsEnabled = false;
            try
            {
                if (addVhCommuLog(app?.ObjCacheManager?.popAll_VhCommuLOGs()) || doFilter_VhCommuLogs)
                    refresh_VehicleCommu();

                doFilter_VhCommuLogs = false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            try
            {
                if (addMcsCommuLog(app?.ObjCacheManager?.popAll_McsCommuLOGs()))
                    refresh_McsCommu();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            //try
            //{
            //    if (addPlcCommuLog(app?.ObjCacheManager?.popAll_SystemProcLOGs()))
            //        refresh_PlcCommu();
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Exception");
            //}
            try
            {
                if (grid_SysProcLog?.ckb_LogEnable?.IsChecked ?? false)
                    if (addSystemProcLog(app?.ObjCacheManager?.popAll_SystemProcLOGs()) || doFilter_SystemProcLogs)
                        refresh_SystemProc(grid_SysProcLog.cbx_VhID.Text, grid_SysProcLog.cbx_LogLevel.Text);

                doFilter_SystemProcLogs = false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            timer_RefreshCommuLogs.IsEnabled = true;
        }
        #endregion Commu Log

        public CurrentData()
        {
            InitializeComponent();

            initRefreshCommuLogs();
        }

        public void Start(WindownApplication _app)
        {
            app = _app;
            if (app == null) return;

            int index;
            if (!app.ObjCacheManager.IsForAGVC)
            {
                index = grid_VHStatus.grid_VH_Status.Columns.IndexOf(grid_VHStatus.dgtc_BatteryCap);
                grid_VHStatus.grid_VH_Status.Columns[index].Visibility = Visibility.Collapsed; 
            }
            if (!(app.ObjCacheManager.GetSelectedProject().ObjectConverter == "OHBC_ASE_K24"))
            {
                index = grid_VHStatus.grid_VH_Status.Columns.IndexOf(grid_VHStatus.dgtc_Install);
                grid_VHStatus.grid_VH_Status.Columns[index].Visibility = Visibility.Collapsed;
            }
            if (!(app.ObjCacheManager.GetSelectedProject().ObjectConverter == "OHBC_ASE_K21"))
            {
                index = grid_TransCMD.grid_MCS_Command.Columns.IndexOf(grid_TransCMD.dgtc_BoxID);
                grid_TransCMD.grid_MCS_Command.Columns[index].Visibility = Visibility.Collapsed;
                index = grid_VehicleCMD.grid_VHCommand.Columns.IndexOf(grid_VehicleCMD.dgtc_BoxID);
                grid_VehicleCMD.grid_VHCommand.Columns[index].Visibility = Visibility.Collapsed;
            }

            grid_VHStatus.grid_VH_Status.ItemsSource = app.ObjCacheManager.GetVEHICLEs()?.ToList();
            //grid_TransCMD.grid_MCS_Command.ItemsSource = app.ObjCacheManager.GetTRANSFERs()?.Select(t => new VTRANSFER(t, app.CmdBLL.GetCmdByTransferID(t.TRANSFER_ID)))?.ToList();
            grid_TransCMD.grid_MCS_Command.ItemsSource = app.ObjCacheManager.GetTRANSFERs();
            grid_VehicleCMD.grid_VHCommand.ItemsSource = app.ObjCacheManager.GetCOMMANDs()?.ToList();
            grid_CurAlarm.grid_Cur_Alarm.ItemsSource = app.ObjCacheManager.GetAlarms()?.ToList();
            //grid_PortStatus.grid_PortDef.ItemsSource = app.ObjCacheManager.GetPortStation().Where(p => p.PORT_ID.StartsWith("OHT")).Select(t => new PortStatusViewObj(t)).ToList();
            //grid_CassetteStatus.grid_CassetteData.ItemsSource = app.ObjCacheManager.GetCassetteDatasForDisplay();
            grid_VehicleCommu.grid_CommuLog.ItemsSource = getVhCommuLogs()?.ToList();
            grid_McsCommu.grid_CommuLog.ItemsSource = getMcsCommuLogs()?.ToList();
            grid_PlcCommu.grid_CommuLog.ItemsSource = getPlcCommuLogs()?.ToList(); ;
            grid_SysProcLog.grid_CommuLog.ItemsSource = getSystemProcLogs()?.ToList();

            List<string> vhList = new List<string>();
            vhList.Add("");
            vhList.AddRange(app?.ObjCacheManager?.GetVEHICLEs()?.Select(v => v.VEHICLE_ID).ToList() ?? new List<string>());
            if (vhList.Count <= 2)
            {
                grid_VehicleCommu.lbl_VhID.Visibility = Visibility.Collapsed;
                grid_VehicleCommu.cbx_VhID.Visibility = Visibility.Collapsed;

                grid_SysProcLog.lbl_VhID.Visibility = Visibility.Collapsed;
                grid_SysProcLog.cbx_VhID.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_VehicleCommu.cbx_VhID.ItemsSource = vhList;
                grid_VehicleCommu.cbx_VhID.SelectedIndex = 0;

                grid_SysProcLog.cbx_VhID.ItemsSource = vhList;
                grid_SysProcLog.cbx_VhID.SelectedIndex = 0;
            }
            grid_SysProcLog.cbx_LogLevel.ItemsSource = Enum.GetValues(typeof(Definition.LogLevel)).Cast<Definition.LogLevel>();
            grid_SysProcLog.cbx_LogLevel.SelectedIndex = grid_SysProcLog.cbx_LogLevel.Items.IndexOf(Definition.LogLevel.Info);

            registerEvents();
            timer_RefreshCommuLogs.Start();
            grid_SysProcLog.Start(app);
        }

        public void SelectVh_VHStatus(string vh_id)
        {
            if (SelectedVhID == vh_id) return;
            if (string.IsNullOrWhiteSpace(vh_id))
            {
                grid_VHStatus.grid_VH_Status.SelectedIndex = -1;
                return;
            }

            try
            {
                if (grid_VHStatus.grid_VH_Status.Items?.Count > 0)
                {
                    foreach (var item in grid_VHStatus.grid_VH_Status.Items)
                    {
                        if (((VVEHICLE)item)?.VEHICLE_ID == vh_id?.Trim())
                        {
                            grid_VHStatus.grid_VH_Status.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void registerEvents()
        {
            //app.ObjCacheManager.VehicleUpdateComplete += ObjCacheManager_VehicleUpdateComplete; // 車輛參數改變會自動更新
            app.ObjCacheManager.CommandChange += ObjCacheManager_CommandChange;
            app.ObjCacheManager.TransferChange += ObjCacheManager_TransferChange;
            app.ObjCacheManager.AlarmChange += ObjCacheManager_AlarmChange;
            //app.ObjCacheManager.PortStationUpdateComplete += ObjCacheManager_PortDefUpdateComplete;
            app.ObjCacheManager.CarrierChange += ObjCacheManager_CarrierChange;

            grid_VHStatus.grid_VH_Status.SelectionChanged += VHStatus_SelectionChanged;

            grid_VehicleCommu.grid_CommuLog.SelectionChanged += VehicleCommu_SelectionChanged;
            grid_VehicleCommu.ckb_KeepSelection.Click += VehicleCommu_ResetSelection;
            grid_VehicleCommu.cbx_VhID.SelectionChanged += VehicleCommu_CbxVhID_SelectionChanged;

            grid_McsCommu.grid_CommuLog.SelectionChanged += McsCommu_SelectionChanged;
            grid_McsCommu.ckb_KeepSelection.Click += McsCommu_ResetSelection;

            grid_PlcCommu.grid_CommuLog.SelectionChanged += PlcCommu_SelectionChanged;
            grid_PlcCommu.ckb_KeepSelection.Click += PlcCommu_ResetSelection;

            grid_SysProcLog.grid_CommuLog.SelectionChanged += SysProcLog_SelectionChanged;
            grid_SysProcLog.ckb_LogEnable.Click += SysProcLog_Subcribe;
            grid_SysProcLog.ckb_KeepSelection.Click += SysProcLog_ResetSelection;
            grid_SysProcLog.cbx_VhID.SelectionChanged += SysProcLog_Cbx_SelectionChanged;
            grid_SysProcLog.cbx_LogLevel.SelectionChanged += SysProcLog_Cbx_SelectionChanged;
        }

        private void unregisterEvents()
        {
            //app.ObjCacheManager.VehicleUpdateComplete -= ObjCacheManager_VehicleUpdateComplete;
            app.ObjCacheManager.CommandChange -= ObjCacheManager_CommandChange;
            app.ObjCacheManager.TransferChange -= ObjCacheManager_TransferChange;
            app.ObjCacheManager.AlarmChange -= ObjCacheManager_AlarmChange;
            //app.ObjCacheManager.PortStationUpdateComplete -= ObjCacheManager_PortDefUpdateComplete;
            app.ObjCacheManager.CarrierChange -= ObjCacheManager_CarrierChange;

            grid_VHStatus.grid_VH_Status.SelectionChanged -= VHStatus_SelectionChanged;

            grid_VehicleCommu.grid_CommuLog.SelectionChanged -= VehicleCommu_SelectionChanged;
            grid_VehicleCommu.ckb_KeepSelection.Click -= VehicleCommu_ResetSelection;
            grid_VehicleCommu.cbx_VhID.SelectionChanged -= VehicleCommu_CbxVhID_SelectionChanged;

            grid_McsCommu.grid_CommuLog.SelectionChanged -= McsCommu_SelectionChanged;
            grid_McsCommu.ckb_KeepSelection.Click -= McsCommu_ResetSelection;

            grid_SysProcLog.grid_CommuLog.SelectionChanged -= SysProcLog_SelectionChanged;
            grid_SysProcLog.ckb_LogEnable.Click -= SysProcLog_Subcribe;
            grid_SysProcLog.ckb_KeepSelection.Click -= SysProcLog_ResetSelection;
            grid_SysProcLog.cbx_VhID.SelectionChanged -= SysProcLog_Cbx_SelectionChanged;
            grid_SysProcLog.cbx_LogLevel.SelectionChanged -= SysProcLog_Cbx_SelectionChanged;
        }

        private void SysProcLog_Subcribe(object sender, EventArgs e)
        {
            try
            {
                if (app == null) return;
                string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
                if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
                Adapter.Invoke((obj) =>
                {
                    if ((bool)grid_SysProcLog.ckb_LogEnable.IsChecked)
                        app?.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_SYSTEM_LOG+ ChannelBay, app.LineBLL.ProcSystemInfo);
                    else
                        app?.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_SYSTEM_LOG+ ChannelBay);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void SysProcLog_Cbx_SelectionChanged(object sender, EventArgs e)
        {
            doFilter_SystemProcLogs = true;
        }

        private void VehicleCommu_CbxVhID_SelectionChanged(object sender, EventArgs e)
        {
            doFilter_VhCommuLogs = true;
        }

        private void SysProcLog_ResetSelection(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (!(bool)grid_SysProcLog.ckb_KeepSelection.IsChecked)
                    {
                        if (grid_SysProcLog.grid_CommuLog.Items?.Count > 0)
                            grid_SysProcLog.grid_CommuLog.ScrollIntoView(grid_SysProcLog.grid_CommuLog.Items[0]);
                        grid_SysProcLog.grid_CommuLog.SelectedIndex = -1;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void PlcCommu_ResetSelection(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (!(bool)grid_PlcCommu.ckb_KeepSelection.IsChecked)
                    {
                        if (grid_PlcCommu.grid_CommuLog.Items?.Count > 0)
                            grid_PlcCommu.grid_CommuLog.ScrollIntoView(grid_PlcCommu.grid_CommuLog.Items[0]);
                        grid_PlcCommu.grid_CommuLog.SelectedIndex = -1;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void McsCommu_ResetSelection(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (!(bool)grid_McsCommu.ckb_KeepSelection.IsChecked)
                    {
                        if (grid_McsCommu.grid_CommuLog.Items?.Count > 0)
                            grid_McsCommu.grid_CommuLog.ScrollIntoView(grid_McsCommu.grid_CommuLog.Items[0]);
                        grid_McsCommu.grid_CommuLog.SelectedIndex = -1;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void VehicleCommu_ResetSelection(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (!(bool)grid_VehicleCommu.ckb_KeepSelection.IsChecked)
                    {
                        if (grid_VehicleCommu.grid_CommuLog.Items?.Count > 0)
                            grid_VehicleCommu.grid_CommuLog.ScrollIntoView(grid_VehicleCommu.grid_CommuLog.Items[0]);
                        grid_VehicleCommu.grid_CommuLog.SelectedIndex = -1;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void SysProcLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    SYSTEM_PROCESS_LOG selection = (SYSTEM_PROCESS_LOG)grid_SysProcLog.grid_CommuLog.SelectedItem;
                    grid_SysProcLog.txb_Message.Text = selection == null ? "" :
                                                       $"Time:[{selection.Time}]\n" +
                                                       $"Data:[{selection.Data}]\n" +
                                                       $"{selection.Details}";
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void PlcCommu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    PLC_COMMU_LOG selection = (PLC_COMMU_LOG)grid_PlcCommu.grid_CommuLog.SelectedItem;
                    grid_PlcCommu.txb_Message.Text = selection == null ? "" :
                                                     $"Time:[{selection.Time}]\n" +
                                                     $"Data:[{selection.Data}]\n" +
                                                     $"{selection.Details}";
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void McsCommu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    MCS_COMMU_LOG selection = (MCS_COMMU_LOG)grid_McsCommu.grid_CommuLog.SelectedItem;
                    grid_McsCommu.txb_Message.Text = selection == null ? "" :
                                                     $"Time:[{selection.Time}]\n" +
                                                     $"Direction:[{selection.SendRecive}]\n" +
                                                     $"{selection.Message}";
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void VehicleCommu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    VEHICLE_COMMU_LOG selection = (VEHICLE_COMMU_LOG)grid_VehicleCommu.grid_CommuLog.SelectedItem;
                    grid_VehicleCommu.txb_Message.Text = selection == null ? "" :
                                                         $"Time:[{selection.Time}]\n" +
                                                         $"Direction:[{selection.SendRecive}]\n" +
                                                         $"{selection.Message}";
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void VHStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (e.AddedItems?.Count > 0)
                    {
                        string vh_id = ((VVEHICLE)e.AddedItems[0])?.VEHICLE_ID?.Trim();
                        if (SelectedVhID != vh_id)
                        {
                            SelectedVhID = vh_id;
                            SelectedVhChanged?.Invoke(this, vh_id);
                        }
                    }
                }, null);
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
                    refresh_VHStatus();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_CommandChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refresh_VehicleCMDGrp();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_TransferChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refresh_TransCMDGrp();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_AlarmChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refresh_AlarmGrp();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_PortDefUpdateComplete(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refresh_PortGrp();
                    //uctl_Map?.refreshPortData();
                    //uctl_Map_Hor?.refreshPortData();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_CarrierChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    //if (InObservationVh == null)
                    //{
                    //    uctl_Map?.IniCstData();
                    //    uctl_Map_Hor?.IniCstData();
                    //}
                    refresh_CassetteGrp();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void refresh_SystemProc(String VhID = "All", String LogLevel = "Debug")
        {
            if (!(bool)grid_SysProcLog.ckb_KeepSelection.IsChecked)
            {
                if (grid_SysProcLog.grid_CommuLog.Items?.Count > 0)
                    grid_SysProcLog.grid_CommuLog.ScrollIntoView(grid_SysProcLog.grid_CommuLog.Items[0]);
                grid_SysProcLog.grid_CommuLog.SelectedIndex = -1;
            }
            object current = grid_SysProcLog.grid_CommuLog.SelectedItem;
            List<SYSTEM_PROCESS_LOG> _SysCommuLogs = new List<SYSTEM_PROCESS_LOG>();
            List<String> lsLogLevel = getLogLevel(LogLevel);
            if (VhID != "All" && VhID != "")
            {
                _SysCommuLogs = getSystemProcLogs().Where(l => l.VhID == VhID && lsLogLevel.Contains(l.LogLevel.ToString()))?.ToList();
            }
            else
            {
                _SysCommuLogs = getSystemProcLogs().Where(l => lsLogLevel.Contains(l.LogLevel.ToString()))?.ToList();
            }
            grid_SysProcLog.grid_CommuLog.ItemsSource = _SysCommuLogs;
            //grid_SysProcLog.grid_CommuLog.ItemsSource = getSystemProcLogs()?.ToList();
            if (current != null)
            {
                SYSTEM_PROCESS_LOG pre = (SYSTEM_PROCESS_LOG)current;
                foreach (var item in grid_SysProcLog.grid_CommuLog.Items)
                {
                    SYSTEM_PROCESS_LOG now = (SYSTEM_PROCESS_LOG)item;

                    if (now?.Time == pre?.Time)
                    {
                        grid_SysProcLog.grid_CommuLog.SelectedItem = item;
                        grid_SysProcLog.grid_CommuLog.ScrollIntoView(grid_SysProcLog.grid_CommuLog.SelectedItem);
                        break;
                    }
                }
            }
        }
        private List<String> getLogLevel(String LogLevel)
        {
            //LogLevel: "Debug", "Info", "Warn", "Error", "Fatal"
            //input: LogLevel
            //output:List: Level >= LogLevel
            //ex: LogLevel="Info", reutrn is {"Info", "Warn", "Error", "Fatal"}
            List<String> lsLogLevel = new List<string>();
            lsLogLevel.Add("Debug");
            lsLogLevel.Add("Info");
            lsLogLevel.Add("Warn");
            lsLogLevel.Add("Error");
            lsLogLevel.Add("Fatal");
            int iLogLevel = lsLogLevel.IndexOf(LogLevel);
            int i = 0;
            while (i < iLogLevel)
            {
                lsLogLevel.RemoveAt(0);
                i++;
            }

            return lsLogLevel;
        }

        public void refresh_PlcCommu()
        {
            if (!(bool)grid_PlcCommu.ckb_KeepSelection.IsChecked)
            {
                if (grid_PlcCommu.grid_CommuLog.Items?.Count > 0)
                    grid_PlcCommu.grid_CommuLog.ScrollIntoView(grid_PlcCommu.grid_CommuLog.Items[0]);
                grid_PlcCommu.grid_CommuLog.SelectedIndex = -1;
            }
            object current = grid_PlcCommu.grid_CommuLog.SelectedItem;
            grid_PlcCommu.grid_CommuLog.ItemsSource = getPlcCommuLogs()?.ToList();
            if (current != null)
            {
                SYSTEM_PROCESS_LOG pre = (SYSTEM_PROCESS_LOG)current;
                foreach (var item in grid_PlcCommu.grid_CommuLog.Items)
                {
                    SYSTEM_PROCESS_LOG now = (SYSTEM_PROCESS_LOG)item;

                    if (now?.Time == pre?.Time)
                    {
                        grid_PlcCommu.grid_CommuLog.SelectedItem = item;
                        grid_PlcCommu.grid_CommuLog.ScrollIntoView(grid_PlcCommu.grid_CommuLog.SelectedItem);
                        break;
                    }
                }
            }
        }

        public void refresh_McsCommu()
        {
            if (!(bool)grid_McsCommu.ckb_KeepSelection.IsChecked)
            {
                if (grid_McsCommu.grid_CommuLog.Items?.Count > 0)
                    grid_McsCommu.grid_CommuLog.ScrollIntoView(grid_McsCommu.grid_CommuLog.Items[0]);
                grid_McsCommu.grid_CommuLog.SelectedIndex = -1;
            }
            object current = grid_McsCommu.grid_CommuLog.SelectedItem;
            grid_McsCommu.grid_CommuLog.ItemsSource = getMcsCommuLogs()?.ToList();
            if (current != null)
            {
                MCS_COMMU_LOG pre = (MCS_COMMU_LOG)current;
                foreach (var item in grid_McsCommu.grid_CommuLog.Items)
                {
                    MCS_COMMU_LOG now = (MCS_COMMU_LOG)item;

                    if (now?.Time == pre?.Time)
                    {
                        grid_McsCommu.grid_CommuLog.SelectedItem = item;
                        grid_McsCommu.grid_CommuLog.ScrollIntoView(grid_McsCommu.grid_CommuLog.SelectedItem);
                        break;
                    }
                }
            }
        }

        public void refresh_VehicleCommu()
        {
            if (!(bool)grid_VehicleCommu.ckb_KeepSelection.IsChecked)
            {
                if (grid_VehicleCommu.grid_CommuLog.Items?.Count > 0)
                    grid_VehicleCommu.grid_CommuLog.ScrollIntoView(grid_VehicleCommu.grid_CommuLog.Items[0]);
                grid_VehicleCommu.grid_CommuLog.SelectedIndex = -1;
            }
            object current = grid_VehicleCommu.grid_CommuLog.SelectedItem;
            grid_VehicleCommu.grid_CommuLog.ItemsSource = getVhCommuLogs()?.ToList();
            if (current != null)
            {
                VEHICLE_COMMU_LOG pre = (VEHICLE_COMMU_LOG)current;
                foreach (var item in grid_VehicleCommu.grid_CommuLog.Items)
                {
                    VEHICLE_COMMU_LOG now = (VEHICLE_COMMU_LOG)item;

                    if (now?.Time == pre?.Time && now?.VhID == pre?.VhID)
                    {
                        grid_VehicleCommu.grid_CommuLog.SelectedItem = item;
                        grid_VehicleCommu.grid_CommuLog.ScrollIntoView(grid_VehicleCommu.grid_CommuLog.SelectedItem);
                        break;
                    }
                }
            }
        }

        public void refresh_VHStatus()
        {
            object current = grid_VHStatus.grid_VH_Status.SelectedItem;
            grid_VHStatus.grid_VH_Status.ItemsSource = app.ObjCacheManager.GetVEHICLEs()?.ToList();
            if (current != null)
            {
                VVEHICLE pre = (VVEHICLE)current;
                foreach (var item in grid_VHStatus.grid_VH_Status.Items)
                {
                    VVEHICLE now = (VVEHICLE)item;

                    if (now?.VEHICLE_ID?.Trim() == pre?.VEHICLE_ID?.Trim())
                    {
                        grid_VHStatus.grid_VH_Status.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public void refresh_VehicleCMDGrp()
        {
            object current = grid_VehicleCMD.grid_VHCommand.SelectedItem;
            grid_VehicleCMD.grid_VHCommand.ItemsSource = app.ObjCacheManager.GetCOMMANDs()?.ToList();
            if (current != null)
            {
                VCMD pre = (VCMD)current;
                foreach (var item in grid_VehicleCMD.grid_VHCommand.Items)
                {
                    VCMD now = (VCMD)item;

                    if (now?.CMD_ID?.Trim() == pre?.CMD_ID?.Trim())
                    {
                        grid_VehicleCMD.grid_VHCommand.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public void refresh_TransCMDGrp()
        {
            object current = grid_TransCMD.grid_MCS_Command.SelectedItem;
            //grid_TransCMD.grid_MCS_Command.ItemsSource = app.ObjCacheManager.GetTRANSFERs()?.Select(t => new VTRANSFER(t, app.CmdBLL.GetCmdByTransferID(t.TRANSFER_ID)))?.ToList();
            grid_TransCMD.grid_MCS_Command.ItemsSource = app.ObjCacheManager.GetTRANSFERs();
            if (current != null)
            {
                VTRANSFER pre = (VTRANSFER)current;
                foreach (var item in grid_TransCMD.grid_MCS_Command.Items)
                {
                    VTRANSFER now = (VTRANSFER)item;

                    if (now?.CMD_ID?.Trim() == pre?.CMD_ID?.Trim())
                    {
                        grid_TransCMD.grid_MCS_Command.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public void refresh_AlarmGrp()
        {
            object current = grid_CurAlarm.grid_Cur_Alarm.SelectedItem;
            grid_CurAlarm.grid_Cur_Alarm.ItemsSource = app.ObjCacheManager.GetAlarms()?.ToList();
            if (current != null)
            {
                VALARM pre = (VALARM)current;
                foreach (var item in grid_CurAlarm.grid_Cur_Alarm.Items)
                {
                    VALARM now = (VALARM)item;

                    if (now?.RPT_DATE_TIME == pre?.RPT_DATE_TIME)
                    {
                        grid_CurAlarm.grid_Cur_Alarm.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public void refresh_PortGrp()
        {
            //object current = grid_PortStatus.grid_PortDef.SelectedItem;
            //grid_PortStatus.grid_PortDef.ItemsSource = app.ObjCacheManager.GetPortStation().Where(p => p.PORT_ID.StartsWith("OHT")).Select(t => new PortStatusViewObj(t)).ToList();
            //if (current != null)
            //{
            //    PortStatusViewObj pre = (PortStatusViewObj)current;
            //    foreach (var item in grid_PortStatus.grid_PortDef.Items)
            //    {
            //        PortStatusViewObj now = (PortStatusViewObj)item;

            //        if (now?.PORT_ID?.Trim() == pre?.PORT_ID?.Trim())
            //        {
            //            grid_PortStatus.grid_PortDef.SelectedItem = item;
            //            break;
            //        }
            //    }
            //}
        }

        public void refresh_CassetteGrp()
        {
            //object current = grid_CassetteStatus.grid_CassetteData.SelectedItem;
            //grid_CassetteStatus.grid_CassetteData.ItemsSource = app.ObjCacheManager.GetCassetteDatasForDisplay();
            //if (current != null)
            //{
            //    ACARRIER pre = (ACARRIER)current;
            //    foreach (var item in grid_CassetteStatus.grid_CassetteData.Items)
            //    {
            //        ACARRIER now = (ACARRIER)item;

            //        if (now?.ID?.Trim() == pre?.ID?.Trim())
            //        {
            //            grid_CassetteStatus.grid_CassetteData.SelectedItem = item;
            //            break;
            //        }
            //    }
            //}
        }
    }
}
