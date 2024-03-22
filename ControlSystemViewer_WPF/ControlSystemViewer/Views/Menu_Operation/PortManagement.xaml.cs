using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ControlSystemViewer.Views.Menu_Operation
{
    /// <summary>
    /// PortManagement.xaml 的互動邏輯
    /// </summary>
    public partial class PortManagement : UserControl
    {
        private readonly string ns = "PortManagement";

        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        private ObservableCollection<PORT_DETAIL> listPortStatus = null;
        private ObservableCollection<PORT_DETAIL> listStageInfo = null;
        private ObservableCollection<PORT_DETAIL> listAgvPortSignal = null;
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public PortManagement()
        {
            InitializeComponent();
        }

        public void StartupUI()
        {
            try
            {
                app = WindownApplication.getInstance();

                cb_PortID.Items.Clear();
                foreach (var port in app.ObjCacheManager.GetPortStations())
                {
                    if (!port.IS_MONITORING) continue;
                    cb_PortID.Items.Add(port.PORT_ID);
                }
                if (cb_PortID.Items.Count == 0)
                {
                    logger.Warn("PortManagement - StartupUI - Port Count: 0.");
                    return;
                }

                initDataGrid();

                cb_PortID.SelectionChanged += cb_PortID_SelectionChanged;
                cb_PortID.SelectedIndex = 0;

                app.ObjCacheManager.PortStationChange += ObjCacheManager_PortStationChange;
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
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_PortStationChange(object sender, EventArgs e)
        {
            Adapter.Invoke((obj) =>
            {
                updatePortData();
            }, null);
        }

        private void cb_PortID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Adapter.Invoke((obj) =>
            {
                updatePortData();
            }, null);
        }

        private void updatePortData()
        {
            try
            {
                string portID = Convert.ToString(cb_PortID.SelectedItem);
                ViewerObject.PortData portData = app.ObjCacheManager.GetPortStationByPortID(portID)?.PORT_DATA ?? new ViewerObject.PortData();
                lbl_CurrDir.Content = (app.ObjCacheManager.GetPortStationByPortID(portID)?.IS_INPUT_MODE ?? false) ? "In" : "Out";

                for (int i = 0; i < listPortStatus.Count; i++)
                {
                    listPortStatus[i].STATUS = portData.ListPlcPortStatus[i];
                }
                grid_PortStatus.Items.Refresh();

                for (int i = 0; i < listStageInfo.Count; i++)
                {
                    listStageInfo[i].STATUS = portData.ListStageInfo[i];
                    listStageInfo[i].BOX_ID = portData.ListStageBoxID[i];
                }
                grid_StageInfo.Items.Refresh();

                for (int i = 0; i < listAgvPortSignal.Count; i++)
                {
                    listAgvPortSignal[i].STATUS = portData.ListAgvPortSignal[i];
                }
                grid_AgvPortSignal.Items.Refresh();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        #region btn_Click
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

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_PortRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                if (!app.PortBLL.SetPortRun(portID, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }

        private void btn_PortStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                if (!app.PortBLL.SetPortStop(portID, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }

        private void btn_AlarmReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                if (!app.PortBLL.ResetPortAlarm(portID, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }
        #region Bcr
        private void btn_BcrEnable_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_BcrDisable_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_BcrRead_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_BcrStop_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        #endregion Bcr
        #region 流向
        private void btn_DirReserve_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_DirReserveCancel_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        private void btn_DirChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                string dir = (app.ObjCacheManager.GetPortStationByPortID(portID)?.IS_INPUT_MODE ?? false) ?
                             "OUT" : "IN";
                if (!app.PortBLL.SetPortDir(portID, dir, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addData_KeyValue(nameof(dir), dir);
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }

        private void btn_AutoChangeDirEnable_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_AutoChangeDirDisable_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_AutoChangeDirEnableAll_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_AutoChangeDirDisableAll_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        #endregion 流向
        private void btn_SetAutoTransferTime_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        #region 自動補退盒
        private void btn_AutoAdjustBoxEnable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                bool open = true;
                if (!app.PortBLL.SetAgvStationOpen(portID, open, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addData_KeyValue(nameof(open), open.ToString());
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }

        private void btn_AutoAdjustBoxDisable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                bool open = false;
                if (!app.PortBLL.SetAgvStationOpen(portID, open, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addData_KeyValue(nameof(open), open.ToString());
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }

        private void btn_AutoAdjustBoxEnableAll_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_AutoAdjustBoxDisableAll_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        #endregion 自動補退盒
        private void btn_WaitIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portID = cb_PortID.Text;
                if (!app.PortBLL.SetPortWaitIn(portID, out string result))
                    TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(portID), portID);
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                TipMessage_Type_Light.Show("", $"Exception happend, {ex.Message}", BCAppConstants.WARN_MSG);
            }
        }
        #region AGV Port
        private void btn_SetAGV_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_SetMGV_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_OpenBox_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        #endregion AGV Port
        #region Port Info
        private void btn_PortInfoRefresh_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_PortSetEnable_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_PortSetDisable_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_PortSetInService_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }

        private void btn_PortSetOutOfService_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        private void btn_ReportAllPortStatus_Click(object sender, RoutedEventArgs e)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.Warn($"{ns}.{ms} - Unsupport");
            TipMessage_Type_Light.Show("", "Unsupport", BCAppConstants.WARN_MSG);
        }
        #endregion Port Info
        #endregion btn_Click

        private void initDataGrid()
        {
            #region grid_PortStatus
            listPortStatus = new ObservableCollection<PORT_DETAIL>();
            listPortStatus.Add(new PORT_DETAIL("運轉狀態", "RUN", ""));                         //0
            listPortStatus.Add(new PORT_DETAIL("自動模式", "IsAutoMode", ""));                  //1
            listPortStatus.Add(new PORT_DETAIL("異常狀態", "ErrorBit", ""));                    //2
            listPortStatus.Add(new PORT_DETAIL("異常代碼", "ErrorCode", ""));                   //3
            listPortStatus.Add(new PORT_DETAIL("流向", "", ""));                                //4
            listPortStatus.Add(new PORT_DETAIL("是否能切換流向", "IsModeChangable", ""));       //5
            listPortStatus.Add(new PORT_DETAIL("流向:Port 往 OHT", "IsInputMode", ""));         //6
            listPortStatus.Add(new PORT_DETAIL("流向:OHT 往 Port", "IsOutputMode", ""));        //7
            listPortStatus.Add(new PORT_DETAIL("投出入說明", "", ""));                          //8
            listPortStatus.Add(new PORT_DETAIL("Port 是否能搬入 BOX ", "IsReadyToLoad", ""));   //9
            listPortStatus.Add(new PORT_DETAIL("Port 是否能搬出 BOX ", "IsReadyToUnload", "")); //10
            listPortStatus.Add(new PORT_DETAIL("等待說明", "", ""));                            //11
            listPortStatus.Add(new PORT_DETAIL("等待 OHT 搬走", "PortWaitIn", ""));             //12
            listPortStatus.Add(new PORT_DETAIL("等待從 Port 搬走", "PortWaitOut", ""));         //13
            listPortStatus.Add(new PORT_DETAIL("狀態說明", "", ""));                            //14
            listPortStatus.Add(new PORT_DETAIL("PLC 離線狀態", "CIM_ON", ""));                  //15
            listPortStatus.Add(new PORT_DETAIL("PLC 預先入料完成", "PreLoadOK", ""));           //16
            grid_PortStatus.ItemsSource = listPortStatus;
            #endregion grid_PortStatus

            #region grid_StageInfo
            listStageInfo = new ObservableCollection<PORT_DETAIL>();
            listStageInfo.Add(new PORT_DETAIL("帳移除", "Remove", ""));                    //0
            listStageInfo.Add(new PORT_DETAIL("", "", ""));                                //1
            listStageInfo.Add(new PORT_DETAIL("盒子 BCR 讀取狀態", "BCRReadDone", ""));    //2
            listStageInfo.Add(new PORT_DETAIL("盒子ID", "BoxID", ""));                     //3
            listStageInfo.Add(new PORT_DETAIL("", "", ""));                                //4
            listStageInfo.Add(new PORT_DETAIL("節數 1 是否有盒子", "LoadPosition1", ""));  //5
            listStageInfo.Add(new PORT_DETAIL("節數 2 是否有盒子", "LoadPosition2", ""));  //6
            listStageInfo.Add(new PORT_DETAIL("節數 3 是否有盒子", "LoadPosition3", ""));  //7
            listStageInfo.Add(new PORT_DETAIL("節數 4 是否有盒子", "LoadPosition4", ""));  //8
            listStageInfo.Add(new PORT_DETAIL("節數 5 是否有盒子", "LoadPosition5", ""));  //9
            listStageInfo.Add(new PORT_DETAIL("節數 6 是否有盒子", "LoadPosition6", ""));  //10
            listStageInfo.Add(new PORT_DETAIL("節數 7 是否有盒子", "LoadPosition7", ""));  //11
            grid_StageInfo.ItemsSource = listStageInfo;
            #endregion grid_StageInfo

            #region grid_AgvPortSignal
            listAgvPortSignal = new ObservableCollection<PORT_DETAIL>();
            listAgvPortSignal.Add(new PORT_DETAIL("開啟自動補退盒子功能", "openAGV_Station"));       //0
            listAgvPortSignal.Add(new PORT_DETAIL("開啟自動切換流向功能", "openAGV_AutoPortType"));  //1
            listAgvPortSignal.Add(new PORT_DETAIL("AGV 模式", "IsAGVMode"));                         //2
            listAgvPortSignal.Add(new PORT_DETAIL("MGV 模式", "IsMGVMode"));                         //3
            listAgvPortSignal.Add(new PORT_DETAIL("", "", ""));                                      //4
            listAgvPortSignal.Add(new PORT_DETAIL("AGV 能投放", "AGVPortReady"));                    //5
            listAgvPortSignal.Add(new PORT_DETAIL("AGV 不能投放", "AGVPortMismatch"));               //6
            listAgvPortSignal.Add(new PORT_DETAIL("", "", ""));                                      //7
            listAgvPortSignal.Add(new PORT_DETAIL("是否能開蓋", "CanOpenBox"));                      //8
            listAgvPortSignal.Add(new PORT_DETAIL("開蓋狀態", "IsBoxOpen"));                         //9
            listAgvPortSignal.Add(new PORT_DETAIL("", "", ""));                                      //10
            listAgvPortSignal.Add(new PORT_DETAIL("卡匣ID", "CassetteID"));                          //11
            listAgvPortSignal.Add(new PORT_DETAIL("是否有卡匣", "IsCSTPresence"));                   //12
            grid_AgvPortSignal.ItemsSource = listAgvPortSignal;
            #endregion grid_AgvPortSignal

            grid_GeneralPortStatus.ItemsSource = app?.ObjCacheManager.GetPortStations();
        }

        public class PORT_DETAIL
        {
            public string DESC { get; set; } = "";
            public string SIGNAL_NAME { get; set; } = "";
            public string STATUS { get; set; } = "";
            public string BOX_ID { get; set; } = "";

            public PORT_DETAIL(string desc, string signal_name, string status = "", string boxid = "")
            {
                DESC = desc?.Trim() ?? "";
                SIGNAL_NAME = signal_name?.Trim() ?? "";
                STATUS = status?.Trim() ?? "";
                BOX_ID = boxid?.Trim() ?? "";
            }
        }
    }
}
