//#define DISPLAYCONTROLSTATUSDETAIL // v0.1.1 -1
//#define DISPLAYDEVICESTATUSPAGE // v0.1.1 -2

using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.Utility.uc;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
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


namespace ControlSystemViewer.Views.Menu_System
{
    /// <summary>
    /// SystemModeControl.xaml 的互動邏輯
    /// </summary>
    public partial class SystemModeControl : UserControl
    {
        #region 公用參數設定

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
		#endregion 公用參數設定

		public SystemModeControl()
        {
            InitializeComponent();

			this.Loaded += _Loaded;
          
        }

		private void _Loaded(object sender, EventArgs e)
        {
            SystemModeControlC1.colorMode = Components.ColorMode.Dark;
            SystemModeControlC1.StartupUI();
            StartupUI();
		}

        public void StartupUI()
        {
            try
            {
                app = WindownApplication.getInstance();
                //init();
                //registerEvent();
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
				//unregisterEvent();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

        private StringBuilder badgeNo = new StringBuilder();
        private DateTime preInputTime = DateTime.Now;
        int BadgeDefaultLength = 10;

        private async void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
             {
                bool isSuccess = false;
                string result = string.Empty;
                if (e.Key == Key.Return)
                {
                    int timeDiff = (int)DateTime.Now.Subtract(preInputTime).TotalMilliseconds;
                    if (timeDiff > 100)
                        badgeNo.Clear();
                    //badgeNo = new StringBuilder("123123123123", 12);
                    string sBadgeCode = badgeNo.ToString();
                    if (!string.IsNullOrWhiteSpace(sBadgeCode))
                    {
                        sBadgeCode = sBadgeCode.Trim();
                        if (sBadgeCode.Length < BadgeDefaultLength)
                        {
                            //BCUtility.showMsgBox_Warn(this, BCApplication.getMessageString("NO_AUTHORITY"));
                            logger.Warn("BadgeCode:[{0}] less than BadgeDefaultLength:[{1}]", sBadgeCode, BadgeDefaultLength);
                            return;
                        }
                        await Task.Run(() => isSuccess = app.LineBLL.SendHostModeCheckAuthorityByBadge(sBadgeCode, out result));
                        if (isSuccess)
                        {
                            TipMessage_Type_Light_woBtn.Show("", "Check OK!", BCAppConstants.INFO_MSG);
                            SystemModeControlC1.ControlStatus.Button1.IsEnabled = true;
                            SystemModeControlC1.ControlStatus.Button2.IsEnabled = true;
                            SystemModeControlC1.ControlStatus.Button3.IsEnabled = true;
                        }
                        else
                        {
                            TipMessage_Type_Light.Show("", "No authority!", BCAppConstants.WARN_MSG);
                            SystemModeControlC1.ControlStatus.Button1.IsEnabled = false;
                            SystemModeControlC1.ControlStatus.Button2.IsEnabled = false;
                            SystemModeControlC1.ControlStatus.Button3.IsEnabled = false;
                        }
                        //BCUtility.doLoginByBadge(this, bcApp, sBadgeCode.Trim());
                    }
                    badgeNo.Clear();
                    this.Focus();
                }
                else
                {
                    if (e.Key >= Key.D0 && e.Key <= Key.D9)
                    {
                        int timeDiff = (int)DateTime.Now.Subtract(preInputTime).TotalMilliseconds;
                        if (timeDiff > 100)
                            badgeNo.Clear();
                        switch (e.Key)
                        {
                            case Key.D0:
                                badgeNo.Append(0);
                                break;
                            case Key.D1:
                                badgeNo.Append(1);
                                break;
                            case Key.D2:
                                badgeNo.Append(2);
                                break;
                            case Key.D3:
                                badgeNo.Append(3);
                                break;
                            case Key.D4:
                                badgeNo.Append(4);
                                break;
                            case Key.D5:
                                badgeNo.Append(5);
                                break;
                            case Key.D6:
                                badgeNo.Append(6);
                                break;
                            case Key.D7:
                                badgeNo.Append(7);
                                break;
                            case Key.D8:
                                badgeNo.Append(8);
                                break;
                            case Key.D9:
                                badgeNo.Append(9);
                                break;
                        }
                    }
                    else
                        badgeNo.Clear();
                }
                preInputTime = DateTime.Now;
            }
            catch (Exception ex)
            {
            }
        }

        //		private void init()
        //		{
        //			try
        //			{
        //				CommunicationStatus.SetTitleName("Communication Status", "Enable", "Disable");
        //				ControlStatus.SetTitleName("Control Status", "Online Remote", "Online Local", "Offline");
        //				TSCStatus.SetTitleName("TSC Status", "Auto", "Pause");

        //				ObjCacheManager_ConnectionInfo_update(null, null);
        //				//ObjCacheManager_OnlineCheckInfo_update(null, null);
        //				//ObjCacheManager_PingCheckInfo_update(null, null);
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}

        //		private void registerEvent()
        //		{
        //			try
        //			{
        //				CommunicationStatus.Button1.Click += Button_Click;
        //				CommunicationStatus.Button2.Click += Button_Click;
        //				ControlStatus.Button1.Click += Button_Click;
        //				ControlStatus.Button2.Click += Button_Click;
        //				ControlStatus.Button3.Click += Button_Click;
        //				TSCStatus.Button1.Click += Button_Click;
        //				TSCStatus.Button2.Click += Button_Click;

        //				app.ObjCacheManager.ConnectionInfoUpdate += ObjCacheManager_ConnectionInfo_update;
        //				//app.ObjCacheManager.OnlineCheckInfoUpdate += ObjCacheManager_OnlineCheckInfo_update;
        //				//app.ObjCacheManager.PingCheckInfoUpdate += ObjCacheManager_PingCheckInfo_update;

        //				//app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO, app.LineBLL.ConnectioneInfo);
        //				//app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_ONLINE_CHECK_INFO, app.LineBLL.OnlineCheckInfo);
        //				//app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO, app.LineBLL.PingCheckInfo);
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}

        //		private void unregisterEvent()
        //		{
        //			try
        //			{
        //				CommunicationStatus.Button1.Click -= Button_Click;
        //				CommunicationStatus.Button2.Click -= Button_Click;
        //				ControlStatus.Button1.Click -= Button_Click;
        //				ControlStatus.Button2.Click -= Button_Click;
        //				ControlStatus.Button3.Click -= Button_Click;
        //				TSCStatus.Button1.Click -= Button_Click;
        //				TSCStatus.Button2.Click -= Button_Click;

        //				app.ObjCacheManager.ConnectionInfoUpdate -= ObjCacheManager_ConnectionInfo_update;
        //				//app.ObjCacheManager.OnlineCheckInfoUpdate -= ObjCacheManager_OnlineCheckInfo_update;
        //				//app.ObjCacheManager.PingCheckInfoUpdate -= ObjCacheManager_PingCheckInfo_update;

        //				//app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO);
        //				//app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_ONLINE_CHECK_INFO);
        //				//app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO);
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}

        //		private async void Button_Click(object sender, RoutedEventArgs e)
        //		{
        //			try
        //			{
        //				//var confirmResult = TipMessage_Request_Light.Show("Are you sure to change status ? ");

        //				if (sender.Equals(CommunicationStatus.Button1))
        //				{
        //					linkStatusChange(SCAppConstants.LinkStatus.LinkOK.ToString());

        //					//if (confirmResult != System.Windows.Forms.DialogResult.Yes)
        //					//{
        //					//    return;
        //					//}
        //					//else
        //					//{
        //					//    TipMessage_Type_Light.Show("Successfully Command", "Successfully command to change communication status.", BCAppConstants.INFO_MSG);
        //					//    await Task.Run(() => linkStatusChange?.Invoke(this, new LinkStatusUpdateEventArgs(SCAppConstants.LinkStatus.LinkOK.ToString())));
        //					//}
        //				}
        //				else if (sender.Equals(CommunicationStatus.Button2))
        //				{
        //					linkStatusChange(SCAppConstants.LinkStatus.LinkFail.ToString());

        //					//if (confirmResult != System.Windows.Forms.DialogResult.Yes)
        //					//{
        //					//    return;
        //					//}
        //					//else
        //					//{
        //					//    TipMessage_Type_Light.Show("Successfully Command", "Successfully command to change communication status.", BCAppConstants.INFO_MSG);
        //					//    await Task.Run(() => linkStatusChange?.Invoke(this, new LinkStatusUpdateEventArgs(SCAppConstants.LinkStatus.LinkFail.ToString())));
        //					//}
        //				}
        //				else if (sender.Equals(ControlStatus.Button1))
        //				{
        //					hostModeChange(SCAppConstants.LineHostControlState.HostControlState.On_Line_Remote.ToString());
        //				}
        //				else if (sender.Equals(ControlStatus.Button2))
        //				{
        //					hostModeChange(SCAppConstants.LineHostControlState.HostControlState.On_Line_Local.ToString());
        //				}
        //				else if (sender.Equals(ControlStatus.Button3))
        //				{
        //					hostModeChange(SCAppConstants.LineHostControlState.HostControlState.EQ_Off_line.ToString());
        //				}
        //				else if (sender.Equals(TSCStatus.Button1))
        //				{
        //					tscStateChange(ALINE.TSCState.AUTO.ToString());
        //				}
        //				else if (sender.Equals(TSCStatus.Button2))
        //				{
        //					tscStateChange(ALINE.TSCState.PAUSED.ToString());
        //				}
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}

        //		private void ObjCacheManager_ConnectionInfo_update(object sender, EventArgs e)
        //		{
        //			//Status Tree
        //			/*
        //			 * [Communication Status]
        //			 *			└Enable 
        //			 *			 .	└	[ControlStatus]
        //			 *			 .				└	Online Remote
        //			 *			 .				 .			└	[TSC Status]
        //			 *			 .				 .					└ AUTO
        //			 *			 .				 .					└ NONE
        //			 *			 .				 .					└ PAUSED
        //			 *			 .				 .					└ PAUSING
        //			 *			 .				 .					└ TSC_INIT
        //			 *			 .				└	Online Local
        //			 *			 .				└  Offline
        //			 *			└Disable
        //			 */

        //			Components.StatusColorType SECSstat_Color = Components.StatusColorType.Red;
        //			string SECSstat_Display = string.Empty;
        //			Components.StatusColorType Controlstat_Color = Components.StatusColorType.Red;
        //			string Controlstat_Display = BCAppConstants.HostModeDisplay.Offline;
        //			Components.StatusColorType TSCstat_Color = Components.StatusColorType.Red;
        //			string TSCstat_Display = BCAppConstants.TSCStateDisplay.Init;

        //			ALINE aLINE = app.ObjCacheManager.GetLine();
        //			try
        //			{
        //				Adapter.Invoke((obj) =>
        //				{
        //					//init status
        //					CommunicationStatus.Button1.IsEnabled = false;
        //					CommunicationStatus.Button2.IsEnabled = false;
        //					ControlStatus.Button1.IsEnabled = false;
        //					ControlStatus.Button2.IsEnabled = false;
        //					ControlStatus.Button3.IsEnabled = false;
        //					TSCStatus.Button1.IsEnabled = false;
        //					TSCStatus.Button2.IsEnabled = false;
        //					switch (aLINE.Secs_Link_Stat)
        //					{
        //						case SCAppConstants.LinkStatus.LinkOK:
        //							SECSstat_Color = Components.StatusColorType.Green;
        //							//SECSstat_Display = BCAppConstants.SECSLinkDisplay.Link;
        //							SECSstat_Display = CommunicationStatus.Button1.Content?.ToString() ?? "";
        //							CommunicationStatus.Button2.IsEnabled = true;
        //							switch (aLINE.Host_Control_State)
        //							{
        //								case SCAppConstants.LineHostControlState.HostControlState.On_Line_Remote:
        //									Controlstat_Color = Components.StatusColorType.Green;
        //									Controlstat_Display = BCAppConstants.HostModeDisplay.OnlineRemote;
        //									ControlStatus.Button2.IsEnabled = true;
        //									ControlStatus.Button3.IsEnabled = true;
        //									switch (aLINE.SCStats)
        //									{
        //										case TSCState.AUTO:
        //											TSCstat_Color = Components.StatusColorType.Green;
        //											TSCstat_Display = BCAppConstants.TSCStateDisplay.Auto;
        //											TSCStatus.Button2.IsEnabled = true;
        //											break;

        //										case TSCState.NONE:
        //											TSCstat_Color = Components.StatusColorType.Red;
        //											TSCstat_Display = BCAppConstants.TSCStateDisplay.None;
        //											TSCStatus.Button2.IsEnabled = true;
        //											break;

        //										case TSCState.PAUSED:
        //											TSCstat_Color = Components.StatusColorType.Yellow;
        //											TSCstat_Display = BCAppConstants.TSCStateDisplay.Pause;
        //											TSCStatus.Button1.IsEnabled = true;
        //											break;

        //										case TSCState.PAUSING:
        //											TSCstat_Color = Components.StatusColorType.Yellow;
        //											TSCstat_Display = BCAppConstants.TSCStateDisplay.Pausing;
        //											break;

        //										case TSCState.TSC_INIT:
        //											TSCstat_Color = Components.StatusColorType.Red;
        //											TSCstat_Display = BCAppConstants.TSCStateDisplay.Init;
        //											break;
        //									}
        //									break;//Host_Control_State End

        //								case SCAppConstants.LineHostControlState.HostControlState.On_Line_Local:
        //									Controlstat_Color = Components.StatusColorType.Yellow;
        //									Controlstat_Display = BCAppConstants.HostModeDisplay.OnlineLocal;
        //									ControlStatus.Button1.IsEnabled = true;
        //									ControlStatus.Button3.IsEnabled = true;
        //									break;

        //								case SCAppConstants.LineHostControlState.HostControlState.EQ_Off_line:
        //									Controlstat_Color = Components.StatusColorType.Red;
        //									Controlstat_Display = BCAppConstants.HostModeDisplay.Offline;
        //									ControlStatus.Button1.IsEnabled = true;
        //									ControlStatus.Button2.IsEnabled = true;
        //									break;
        //							}
        //							break;//Secs_Link_Stat End

        //						case SCAppConstants.LinkStatus.LinkFail:
        //							SECSstat_Color = Components.StatusColorType.Red;
        //							//SECSstat_Display = BCAppConstants.SECSLinkDisplay.NotLink;
        //							SECSstat_Display = CommunicationStatus.Button2.Content?.ToString() ?? "";
        //							CommunicationStatus.Button1.IsEnabled = true;
        //							break;
        //					}				
        //				}, null);
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //			CommunicationStatus.SetConnectSignal(SECSstat_Display, SECSstat_Color);
        //#if DISPLAYCONTROLSTATUSDETAIL
        //			ControlStatus.SetConnSignal(Controlstat_Display, Controlstat_Color);
        //#else
        //			ControlStatus.SetConnectSignal(Controlstat_Display, Controlstat_Color);
        //#endif
        //			TSCStatus.SetConnectSignal(TSCstat_Display, TSCstat_Color);
        //		}

        //		private void ObjCacheManager_OnlineCheckInfo_update(object sender, EventArgs e)
        //		{
        //			ALINE aLINE = app.ObjCacheManager.GetLine();
        //			try
        //			{
        //#if DISPLAYCONTROLSTATUSDETAIL
        //				Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
        //				{
        //					ControlStatus.uc_ControlStatusSignal1.SetConnStatus("Current state", aLINE.CurrentStateChecked);
        //					ControlStatus.uc_ControlStatusSignal2.SetConnStatus("TSC state", aLINE.TSCStateChecked);
        //					ControlStatus.uc_ControlStatusSignal3.SetConnStatus("Enhanced carriers", aLINE.EnhancedCarriersChecked);
        //					ControlStatus.uc_ControlStatusSignal4.SetConnStatus("Enhanced transfers", aLINE.EnhancedTransfersChecked);
        //					ControlStatus.uc_ControlStatusSignal5.SetConnStatus("Enhanced vehicles", aLINE.EnhancedVehiclesChecked);
        //					ControlStatus.uc_ControlStatusSignal6.SetConnStatus("Current port states", aLINE.CurrentPortStateChecked);
        //					//ControlStatus.uc_ControlStatusSignal7.SetConnStatus("Current Eq port states", aLINE.CurrentEQPortStateChecked);
        //					//ControlStatus.uc_ControlStatusSignal8.SetConnStatus("Current port types", aLINE.CurrentPortTypesChecked);
        //					//ControlStatus.uc_ControlStatusSignal9.SetConnStatus("Alarm set", aLINE.AlarmSetChecked);
        //					//ControlStatus.uc_ControlStatusSignal10.SetConnStatus("Unit alarm state list", aLINE.UnitAlarmStateListChecked);
        //				}), null);
        //#endif
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}

        //		private void ObjCacheManager_PingCheckInfo_update(object sender, EventArgs e)
        //		{
        //			ALINE aLINE = app.ObjCacheManager.GetLine();
        //			List<AVEHICLE> vhs = app.ObjCacheManager.GetVEHICLEs().ToList();
        //			try
        //			{
        //#if DISPLAYDEVICESTATUSPAGE
        //				Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
        //				{
        //					/*MCS Status*/
        //					uc_MCS_Status.SetConnStatus("MCS", aLINE.MCSConnectionSuccess);
        //					uc_Router_Status.SetConnStatus("Router", aLINE.RouterConnectionSuccess);
        //					/*Vehicle Link Status*/
        //					var vhlk = new[] { uc_VhLk_Status_OHT1, uc_VhLk_Status_OHT2, uc_VhLk_Status_OHT3, uc_VhLk_Status_OHT4, uc_VhLk_Status_OHT5, uc_VhLk_Status_OHT6, uc_VhLk_Status_OHT7, uc_VhLk_Status_OHT8, uc_VhLk_Status_OHT9, uc_VhLk_Status_OHT10, uc_VhLk_Status_OHT11, uc_VhLk_Status_OHT12, uc_VhLk_Status_OHT13, uc_VhLk_Status_OHT14, };
        //					var ohct = new[] { aLINE.AGV1ConnectionSuccess, aLINE.AGV2ConnectionSuccess, aLINE.AGV3ConnectionSuccess, aLINE.AGV4ConnectionSuccess, aLINE.AGV5ConnectionSuccess, aLINE.AGV6ConnectionSuccess, aLINE.AGV7ConnectionSuccess, aLINE.AGV8ConnectionSuccess, aLINE.AGV9ConnectionSuccess, aLINE.AGV10ConnectionSuccess, aLINE.AGV11ConnectionSuccess, aLINE.AGV12ConnectionSuccess, aLINE.AGV13ConnectionSuccess, aLINE.AGV14ConnectionSuccess, };
        //					for (int i = 0; i < vhs.Count(); i++)
        //					{
        //						vhlk[i].SetConnStatus(vhs[i].VEHICLE_ID, ohct[i]);
        //					}
        //					for (int i = vhs.Count(); i < 14; i++)
        //					{
        //						vhlk[i].Visibility = Visibility.Collapsed;
        //					}
        //					//uc_VhLk_Status_OHT1.SetConnStatus("OHT1", aLINE.OHT1ConnectionSuccess);
        //					//uc_VhLk_Status_OHT2.SetConnStatus("OHT2", aLINE.OHT2ConnectionSuccess);
        //					//uc_VhLk_Status_OHT3.SetConnStatus("OHT3", aLINE.OHT3ConnectionSuccess);
        //					//uc_VhLk_Status_OHT4.SetConnStatus("OHT4", aLINE.OHT4ConnectionSuccess);
        //					//uc_VhLk_Status_OHT5.SetConnStatus("OHT5", aLINE.OHT5ConnectionSuccess);
        //					//uc_VhLk_Status_OHT6.SetConnStatus("OHT6", aLINE.OHT6ConnectionSuccess);
        //					//uc_VhLk_Status_OHT7.SetConnStatus("OHT7", aLINE.OHT7ConnectionSuccess);
        //					//uc_VhLk_Status_OHT8.SetConnStatus("OHT8", aLINE.OHT8ConnectionSuccess);
        //					//uc_VhLk_Status_OHT9.SetConnStatus("OHT9", aLINE.OHT9ConnectionSuccess);
        //					//uc_VhLk_Status_OHT10.SetConnStatus("OHT10", aLINE.OHT10ConnectionSuccess);
        //					//uc_VhLk_Status_OHT11.SetConnStatus("OHT11", aLINE.OHT11ConnectionSuccess);
        //					//uc_VhLk_Status_OHT12.SetConnStatus("OHT12", aLINE.OHT12ConnectionSuccess);
        //					//uc_VhLk_Status_OHT13.SetConnStatus("OHT13", aLINE.OHT13ConnectionSuccess);
        //					//uc_VhLk_Status_OHT14.SetConnStatus("OHT14", aLINE.OHT14ConnectionSuccess);
        //					/*PLC Status*/
        //					//uc_PLC_Status_MTL.SetConnStatus("Master PLC", aLINE.MTLConnectionSuccess);
        //					//uc_PLC_Status_MTS1.SetConnStatus("MTS", aLINE.MTSConnectionSuccess);
        //					//uc_PLC_Status_MTS2.SetConnStatus("MTS2", aLINE.MTS2ConnectionSuccess);
        //					uc_PLC_Status_MTS1.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_MTS2.Visibility = Visibility.Collapsed;
        //					//uc_PLC_Status_HID1.SetConnStatus("HID", aLINE.HID1ConnectionSuccess);
        //					//uc_PLC_Status_HID2.SetConnStatus("HID2", aLINE.HID2ConnectionSuccess);
        //					//uc_PLC_Status_HID3.SetConnStatus("HID3", aLINE.HID3ConnectionSuccess);
        //					//uc_PLC_Status_HID4.SetConnStatus("HID4", aLINE.HID4ConnectionSuccess);
        //					//uc_PLC_Status_ADAM6050_1.SetConnStatus("ADAM6050-1", aLINE.Adam1ConnectionSuccess);
        //					//uc_PLC_Status_ADAM6050_2.SetConnStatus("ADAM6050-2", aLINE.Adam2ConnectionSuccess);
        //					//uc_PLC_Status_ADAM6050_3.SetConnStatus("ADAM6050-3", aLINE.Adam3ConnectionSuccess);
        //					//uc_PLC_Status_ADAM6050_4.SetConnStatus("ADAM6050-4", aLINE.Adam4ConnectionSuccess);
        //					uc_PLC_Status_HID2.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_HID3.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_HID4.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_ADAM6050_1.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_ADAM6050_2.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_ADAM6050_3.Visibility = Visibility.Collapsed;
        //					uc_PLC_Status_ADAM6050_4.Visibility = Visibility.Collapsed;
        //					/*AP Status*/
        //					uc_AP_Status_1.SetConnStatus("AP-1", aLINE.AP1ConnectionSuccess);
        //					uc_AP_Status_2.SetConnStatus("AP-2", aLINE.AP2ConnectionSuccess);
        //					uc_AP_Status_3.SetConnStatus("AP-3", aLINE.AP3ConnectionSuccess);
        //					uc_AP_Status_4.SetConnStatus("AP-4", aLINE.AP4ConnectionSuccess);
        //					uc_AP_Status_5.SetConnStatus("AP-5", aLINE.AP5ConnectionSuccess);
        //					uc_AP_Status_6.SetConnStatus("AP-6", aLINE.AP6ConnectionSuccess);
        //					uc_AP_Status_7.SetConnStatus("AP-7", aLINE.AP7ConnectionSuccess);
        //					uc_AP_Status_8.SetConnStatus("AP-8", aLINE.AP8ConnectionSuccess);
        //					uc_AP_Status_9.SetConnStatus("AP-9", aLINE.AP9ConnectionSuccess);
        //					uc_AP_Status_10.SetConnStatus("AP-10", aLINE.AP10ConnectionSuccess);
        //				}), null);
        //#endif
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}

        //		private async void linkStatusChange(string linkstatus)
        //		{
        //			try
        //			{
        //				bool isSuccess = false;
        //				string result = string.Empty;
        //				await Task.Run(() => isSuccess = app.LineBLL.SendLinkStatusChange(linkstatus, out result));
        //				if (!isSuccess)
        //				{
        //					//TipMessage_Type_Light.Show("Change Failure", "Communicating status can't be off.", BCAppConstants.INFO_MSG);
        //					TipMessage_Type_Light.Show("Change Failure", result, BCAppConstants.INFO_MSG);
        //					return;
        //				}

        //				await waitingForExpectedResult(() => linkStatusChangeResult(linkstatus));
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}
        //		private bool linkStatusChangeResult(string linkstatus)
        //		{
        //			try
        //			{
        //				return app?.ObjCacheManager?.GetLine()?.Secs_Link_Stat.ToString() == linkstatus;
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //				return false;
        //			}
        //		}

        //		private async void hostModeChange(string host_mode)
        //		{
        //			try
        //			{
        //				bool isSuccess = false;
        //				string result = string.Empty;
        //				await Task.Run(() => isSuccess = app.LineBLL.SendHostModeChange(host_mode, out result));
        //				if (!isSuccess)
        //				{
        //					TipMessage_Type_Light.Show("Change Failure", result, BCAppConstants.INFO_MSG);
        //					return;
        //				}

        //				await waitingForExpectedResult(() => hostModeChangeResult(host_mode));
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}
        //		private bool hostModeChangeResult(string host_mode)
        //		{
        //			try
        //			{
        //				return app?.ObjCacheManager?.GetLine()?.Host_Control_State.ToString() == host_mode;
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //				return false;
        //			}
        //		}

        //		private async void tscStateChange(string tscstate)
        //		{
        //			try
        //			{
        //				bool isSuccess = false;
        //				string result = string.Empty;
        //				await Task.Run(() => isSuccess = app.LineBLL.SendTSCStateChange(tscstate, out result));
        //				if (!isSuccess)
        //				{
        //					TipMessage_Type_Light.Show("Change Failure", result, BCAppConstants.INFO_MSG);
        //					return;
        //				}

        //				await waitingForExpectedResult(() => tscStateChangeResult(tscstate));
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //		}
        //		private bool tscStateChangeResult(string tscstate)
        //		{
        //			try
        //			{
        //				return app?.ObjCacheManager?.GetLine()?.SCStats.ToString() == tscstate;
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //				return false;
        //			}
        //		}


        //		private async Task waitingForExpectedResult(Func<bool> condition)
        //		{
        //			bool isLoading = false;
        //			try
        //			{
        //				//((MainWindow)App.Current.MainWindow).Loading_Start("Waiting");
        //				isLoading = true;

        //				int waitSec = 15;
        //				bool gotExpectedResult = false;
        //				await Task.Run(() =>
        //				{
        //					gotExpectedResult = SpinWait.SpinUntil(condition, waitSec * 1000);
        //				});
        //				if (!gotExpectedResult)
        //				{
        //					TipMessage_Type_Light.Show("", $"Not getting expected result over {waitSec} seconds", BCAppConstants.INFO_MSG);
        //				}
        //			}
        //			catch (Exception ex)
        //			{
        //				logger.Error(ex, "Exception");
        //			}
        //			finally
        //			{
        //				//if (isLoading) ((MainWindow)App.Current.MainWindow).Loading_Stop();
        //			}
        //		}
    }
}
