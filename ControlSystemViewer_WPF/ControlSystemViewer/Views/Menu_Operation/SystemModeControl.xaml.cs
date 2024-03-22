//#define DISPLAYCONTROLSTATUSDETAIL // v0.1.1 -1
#define DISPLAYDEVICESTATUSPAGE // v0.1.1 -2

using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
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
using ViewerObject;
using static ViewerObject.VLINE_Def;
using TSCState = ViewerObject.VLINE_Def.TSCState;

namespace ControlSystemViewer.Views.Menu_Operation
{
    /// <summary>
    /// SystemModeControl.xaml 的互動邏輯
    /// </summary>
    public partial class SystemModeControl : UserControl
    {
        #region 公用參數設定

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
		public event EventHandler CloseEvent;
		private  VLINE line = null;
		private bool doneStartUp = false;
		#endregion 公用參數設定

		public SystemModeControl()
        {
            InitializeComponent();
	
		}

        public void StartupUI()
        {
            try
            {
                app = WindownApplication.getInstance();
				line = app.ObjCacheManager.GetLine();
				SystemModeControlC1.StartupUI();
				init();			
				registerEvent();
				doneStartUp = true;
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
				if (doneStartUp)
				{
					SystemModeControlC1.Close();
					unregisterEvent();
					doneStartUp = false;
				}
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
                //CommunicationStatus.SetTitleName("Communication Status", "Enable", "Disable");
                //ControlStatus.SetTitleName("Control Status", "Online Remote", "Online Local", "Offline");
                //TSCStatus.SetTitleName("TSC Status", "Auto", "Pause");

                ////_ConnectionInfoChange(null, null);
                _OnlineCheckInfoChange(null, null);
                _PingCheckInfoChange(null, null);
            }
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void registerEvent()
		{
			try
			{
                //CommunicationStatus.Button1.Click += Button_Click;
                //CommunicationStatus.Button2.Click += Button_Click;
                //ControlStatus.Button1.Click += Button_Click;
                //ControlStatus.Button2.Click += Button_Click;
                //ControlStatus.Button3.Click += Button_Click;
                //TSCStatus.Button1.Click += Button_Click;
                //TSCStatus.Button2.Click += Button_Click;

                //line.ConnectionInfoChange += _ConnectionInfoChange;
                line.OnlineCheckInfoChange += _OnlineCheckInfoChange;
                line.PingCheckInfoChange += _PingCheckInfoChange;

                //app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO, app.LineBLL.ConnectioneInfo);
                //app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_ONLINE_CHECK_INFO, app.LineBLL.OnlineCheckInfo);
                //app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO, app.LineBLL.PingCheckInfo);
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
                //CommunicationStatus.Button1.Click -= Button_Click;
                //CommunicationStatus.Button2.Click -= Button_Click;
                //ControlStatus.Button1.Click -= Button_Click;
                //ControlStatus.Button2.Click -= Button_Click;
                //ControlStatus.Button3.Click -= Button_Click;
                //TSCStatus.Button1.Click -= Button_Click;
                //TSCStatus.Button2.Click -= Button_Click;

                //line.ConnectionInfoChange -= _ConnectionInfoChange;
                line.OnlineCheckInfoChange -= _OnlineCheckInfoChange;
                line.PingCheckInfoChange -= _PingCheckInfoChange;

                //app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO);
                //app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_ONLINE_CHECK_INFO);
                //app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO);
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

		//private async void Button_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		//var confirmResult = TipMessage_Request_Light.Show("Are you sure to change status ? ");

		//		if (sender.Equals(CommunicationStatus.Button1))
		//		{
		//			linkStatusChange(true);

		//			//if (confirmResult != System.Windows.Forms.DialogResult.Yes)
		//			//{
		//			//    return;
		//			//}
		//			//else
		//			//{
		//			//    TipMessage_Type_Light.Show("Successfully Command", "Successfully command to change communication status.", BCAppConstants.INFO_MSG);
		//			//    await Task.Run(() => linkStatusChange?.Invoke(this, new LinkStatusUpdateEventArgs(SCAppConstants.LinkStatus.LinkOK.ToString())));
		//			//}
		//		}
		//		else if (sender.Equals(CommunicationStatus.Button2))
		//		{
		//			linkStatusChange(false);

		//			//if (confirmResult != System.Windows.Forms.DialogResult.Yes)
		//			//{
		//			//    return;
		//			//}
		//			//else
		//			//{
		//			//    TipMessage_Type_Light.Show("Successfully Command", "Successfully command to change communication status.", BCAppConstants.INFO_MSG);
		//			//    await Task.Run(() => linkStatusChange?.Invoke(this, new LinkStatusUpdateEventArgs(SCAppConstants.LinkStatus.LinkFail.ToString())));
		//			//}
		//		}
		//		else if (sender.Equals(ControlStatus.Button1))
		//		{
		//			hostModeChange(HostControlState.On_Line_Remote.ToString());
		//		}
		//		else if (sender.Equals(ControlStatus.Button2))
		//		{
		//			hostModeChange(HostControlState.On_Line_Local.ToString());
		//		}
		//		else if (sender.Equals(ControlStatus.Button3))
		//		{
		//			hostModeChange(HostControlState.EQ_Off_line.ToString());
		//		}
		//		else if (sender.Equals(TSCStatus.Button1))
		//		{
		//			tscStateChange(TSCState.AUTO.ToString());
		//		}
		//		else if (sender.Equals(TSCStatus.Button2))
		//		{
		//			tscStateChange(TSCState.PAUSED.ToString());
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		logger.Error(ex, "Exception");
		//	}
		//}

//		private void _ConnectionInfoChange(object sender, EventArgs e)
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
//					if (line.IS_LINK_HOST)
//					{
//						SECSstat_Color = Components.StatusColorType.Green;
//						//SECSstat_Display = BCAppConstants.SECSLinkDisplay.Link;
//						SECSstat_Display = CommunicationStatus.Button1.Content?.ToString() ?? "";
//						CommunicationStatus.Button2.IsEnabled = true;
//						switch (line.HOST_CONTROL_STATE)
//						{
//							case HostControlState.On_Line_Remote:
//								Controlstat_Color = Components.StatusColorType.Green;
//								Controlstat_Display = BCAppConstants.HostModeDisplay.OnlineRemote;
//								ControlStatus.Button2.IsEnabled = true;
//								ControlStatus.Button3.IsEnabled = true;
//								switch (line.TSC_STATE)
//								{
//									case TSCState.AUTO:
//										TSCstat_Color = Components.StatusColorType.Green;
//										TSCstat_Display = BCAppConstants.TSCStateDisplay.Auto;
//										TSCStatus.Button2.IsEnabled = true;
//										break;

//									case TSCState.NONE:
//										TSCstat_Color = Components.StatusColorType.Red;
//										TSCstat_Display = BCAppConstants.TSCStateDisplay.None;
//										TSCStatus.Button2.IsEnabled = true;
//										break;

//									case TSCState.PAUSED:
//										TSCstat_Color = Components.StatusColorType.Yellow;
//										TSCstat_Display = BCAppConstants.TSCStateDisplay.Pause;
//										//TSCStatus.Button1.IsEnabled = true;
//										TSCStatus.Button1.IsEnabled = true;
//										break;

//									case TSCState.PAUSING:
//										TSCstat_Color = Components.StatusColorType.Yellow;
//										TSCstat_Display = BCAppConstants.TSCStateDisplay.Pausing;
//										break;

//									case TSCState.TSC_INIT:
//										TSCstat_Color = Components.StatusColorType.Red;
//										TSCstat_Display = BCAppConstants.TSCStateDisplay.Init;
//										break;
//								}
//								break;

//							case HostControlState.On_Line_Local:
//								Controlstat_Color = Components.StatusColorType.Yellow;
//								Controlstat_Display = BCAppConstants.HostModeDisplay.OnlineLocal;
//								ControlStatus.Button1.IsEnabled = true;
//								ControlStatus.Button3.IsEnabled = true;
//								break;

//							case HostControlState.EQ_Off_line:
//								Controlstat_Color = Components.StatusColorType.Red;
//								Controlstat_Display = BCAppConstants.HostModeDisplay.Offline;
//								ControlStatus.Button1.IsEnabled = true;
//								ControlStatus.Button2.IsEnabled = true;
//								break;
//						}
//					}
//					else
//					{
//						SECSstat_Color = Components.StatusColorType.Red;
//						//SECSstat_Display = BCAppConstants.SECSLinkDisplay.NotLink;
//						SECSstat_Display = CommunicationStatus.Button2.Content?.ToString() ?? "";
//						CommunicationStatus.Button1.IsEnabled = true;
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

		private void _OnlineCheckInfoChange(object sender, EventArgs e)
		{
			try
			{
#if DISPLAYCONTROLSTATUSDETAIL
				Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
				{
					ControlStatus.uc_ControlStatusSignal1.SetConnStatus("Current state", aLINE.CurrentStateChecked);
					ControlStatus.uc_ControlStatusSignal2.SetConnStatus("TSC state", aLINE.TSCStateChecked);
					ControlStatus.uc_ControlStatusSignal3.SetConnStatus("Enhanced carriers", aLINE.EnhancedCarriersChecked);
					ControlStatus.uc_ControlStatusSignal4.SetConnStatus("Enhanced transfers", aLINE.EnhancedTransfersChecked);
					ControlStatus.uc_ControlStatusSignal5.SetConnStatus("Enhanced vehicles", aLINE.EnhancedVehiclesChecked);
					ControlStatus.uc_ControlStatusSignal6.SetConnStatus("Current port states", aLINE.CurrentPortStateChecked);
					//ControlStatus.uc_ControlStatusSignal7.SetConnStatus("Current Eq port states", aLINE.CurrentEQPortStateChecked);
					//ControlStatus.uc_ControlStatusSignal8.SetConnStatus("Current port types", aLINE.CurrentPortTypesChecked);
					//ControlStatus.uc_ControlStatusSignal9.SetConnStatus("Alarm set", aLINE.AlarmSetChecked);
					//ControlStatus.uc_ControlStatusSignal10.SetConnStatus("Unit alarm state list", aLINE.UnitAlarmStateListChecked);
				}), null);
#endif
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
#if DISPLAYDEVICESTATUSPAGE
				//Adapter.BeginInvoke(new SendOrPostCallback((o1) =>
				Adapter.Invoke((obj) =>
				{
					/*MCS Status*/
			uc_MCS_Status.SetConnStatus("MCS", line.IS_CONNECT_MCS);
					uc_Router_Status.SetConnStatus("Router", line.IS_CONNECT_ROUTER);

					/*Vehicle Link Status*/
					var vhlk = new[] { uc_VhLk_Status_AGV1, uc_VhLk_Status_AGV2, uc_VhLk_Status_AGV3, uc_VhLk_Status_AGV4, uc_VhLk_Status_AGV5, uc_VhLk_Status_AGV6, uc_VhLk_Status_AGV7, uc_VhLk_Status_AGV8, uc_VhLk_Status_AGV9, uc_VhLk_Status_AGV10, uc_VhLk_Status_AGV11, uc_VhLk_Status_AGV12, uc_VhLk_Status_AGV13, uc_VhLk_Status_AGV14, };
					var ohct = line.IS_CONNECT_VEHICLEs;
					for (int i = 0; i < vhs.Count(); i++)
					{
						vhlk[i].SetConnStatus(vhs[i].VEHICLE_ID, ohct[i]);
					}
					for (int i = vhs.Count(); i < 14; i++)
					{
						vhlk[i].Visibility = Visibility.Collapsed;
					}

					/*PLC Status*/
					if (app?.ObjCacheManager.GetSelectedProject()?.ObjectConverter.Contains("AGVC") ?? false)
						uc_PLC_Status_Charger.SetConnStatus("Charger PLC", line.IS_CONNECT_CHARGER_PLC);
					else uc_PLC_Status_Charger.Visibility = Visibility.Collapsed;

					int index = 0;
					if (line?.IS_CONNECT_ADAMs?.Count > index)
						uc_PLC_Status_Adam6050_1.SetConnStatus("Adam 6050-1", line.IS_CONNECT_ADAMs[index]);
					else uc_PLC_Status_Adam6050_1.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_ADAMs?.Count > index)
						uc_PLC_Status_Adam6050_2.SetConnStatus("Adam 6050-2", line.IS_CONNECT_ADAMs[index]);
					else uc_PLC_Status_Adam6050_2.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_ADAMs?.Count > index)
						uc_PLC_Status_Adam6050_3.SetConnStatus("Adam 6050-3", line.IS_CONNECT_ADAMs[index]);
					else uc_PLC_Status_Adam6050_3.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_ADAMs?.Count > index)
						uc_PLC_Status_Adam6050_4.SetConnStatus("Adam 6050-4", line.IS_CONNECT_ADAMs[index]);
					else uc_PLC_Status_Adam6050_4.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_ADAMs?.Count > index)
						uc_PLC_Status_Adam6050_5.SetConnStatus("Adam 6050-5", line.IS_CONNECT_ADAMs[index]);
					else uc_PLC_Status_Adam6050_5.Visibility = Visibility.Collapsed;

					/*AP Status*/
					index = 0;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_1.SetConnStatus("AP01", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_1.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_2.SetConnStatus("AP02", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_2.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_3.SetConnStatus("AP03", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_3.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_4.SetConnStatus("AP04", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_4.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_5.SetConnStatus("AP05", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_5.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_6.SetConnStatus("AP06", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_6.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_7.SetConnStatus("AP07", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_7.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_8.SetConnStatus("AP08", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_8.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_9.SetConnStatus("AP09", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_9.Visibility = Visibility.Collapsed;
					index++;
					if (line?.IS_CONNECT_APs?.Count > index)
						uc_AP_Status_AP_10.SetConnStatus("AP10", line.IS_CONNECT_APs[index]);
					else uc_AP_Status_AP_10.Visibility = Visibility.Collapsed;
				}, null);
				//}), null);
#endif
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async void linkStatusChange(bool isLinkOK)
		{
			try
			{
				bool isSuccess = false;
				string result = string.Empty;
				await Task.Run(() => isSuccess = app.LineBLL.SendLinkStatusChange(isLinkOK, out result));
				if (!isSuccess)
				{
					//TipMessage_Type_Light.Show("Change Failure", "Communicating status can't be off.", BCAppConstants.INFO_MSG);
					TipMessage_Type_Light.Show("Change Failure", result, BCAppConstants.INFO_MSG);
					return;
				}

				await waitingForExpectedResult(() => linkStatusChangeResult(isLinkOK));
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}
		private bool linkStatusChangeResult(bool isLinkOK)
		{
			try
			{
				return isLinkOK ? line.IS_LINK_HOST : !line.IS_LINK_HOST;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return false;
			}
		}

		private async void hostModeChange(string host_mode)
		{
			try
			{
				bool isSuccess = false;
				string result = string.Empty;
				await Task.Run(() => isSuccess = app.LineBLL.SendHostModeChange(host_mode, out result));
				if (!isSuccess)
				{
					TipMessage_Type_Light.Show("Change Failure", result, BCAppConstants.INFO_MSG);
					return;
				}

				await waitingForExpectedResult(() => hostModeChangeResult(host_mode));
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}
		private bool hostModeChangeResult(string host_mode)
		{
			try
			{
				return line.HOST_CONTROL_STATE.ToString() == host_mode;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return false;
			}
		}

		private async void tscStateChange(string tscstate)
		{
			try
			{
				bool isSuccess = false;
				string result = string.Empty;
				await Task.Run(() => isSuccess = app.LineBLL.SendTSCStateChange(tscstate, out result));
				if (!isSuccess)
				{
					TipMessage_Type_Light.Show("Change Failure", result, BCAppConstants.INFO_MSG);
					return;
				}

				await waitingForExpectedResult(() => tscStateChangeResult(tscstate));
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}
		private bool tscStateChangeResult(string tscstate)
		{
			try
			{
				return line.TSC_STATE.ToString() == tscstate;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return false;
			}
		}


		private async Task waitingForExpectedResult(Func<bool> condition)
		{
			bool isLoading = false;
			try
			{
				((MainWindow)App.Current.MainWindow).Loading_Start("Waiting");
				isLoading = true;

				int waitSec = 15;
				bool gotExpectedResult = false;
				await Task.Run(() =>
				{
					gotExpectedResult = SpinWait.SpinUntil(condition, waitSec * 1000);
				});
				if (!gotExpectedResult)
				{
					TipMessage_Type_Light.Show("", $"Not getting expected result over {waitSec} seconds", BCAppConstants.INFO_MSG);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
			finally
			{
				if (isLoading) ((MainWindow)App.Current.MainWindow).Loading_Stop();
			}
		}
        
    }
}
