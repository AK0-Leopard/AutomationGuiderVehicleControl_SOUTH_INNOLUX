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
using ViewerObject;
using static ViewerObject.VLINE_Def;
using TSCState = ViewerObject.VLINE_Def.TSCState;


namespace ControlSystemViewer.Views.Components
{
    /// <summary>
    /// SystemModeControl.xaml 的互動邏輯
    /// </summary>
    public partial class SystemModeControl : UserControl
    {
        #region 公用參數設定

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
		public VLINE line = null;
		private ColorMode _colormode = ColorMode.Default;
		private bool doneStartUp = false;
		string strLogContent = "";
		string OldStatus = "";
		public ColorMode colorMode
		{
			get { return _colormode; }
			set
			{
				if (value == ColorMode.Dark)
				{
					CommunicationStatus.ColorMode = value;
					ControlStatus.ColorMode = value;
					TSCStatus.ColorMode = value;
				}
				_colormode = value;
			}
		}
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
					unregisterEvent();
					doneStartUp = false;
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public void init()
		{
			try
			{
				CommunicationStatus.SetTitleName("Communication Status", "Enable", "Disable");
				ControlStatus.SetTitleName("Control Status", "Online Remote", "Online Local", "Offline", "Check Authority");
				TSCStatus.SetTitleName("TSC Status", "Auto", "Pause");

				_ConnectionInfoChange(null, null);

			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public void registerEvent()
		{
			try
			{
				CommunicationStatus.Button1.Click += Button_Click;
				CommunicationStatus.Button2.Click += Button_Click;
				ControlStatus.Button1.Click += Button_Click;
				ControlStatus.Button2.Click += Button_Click;
				ControlStatus.Button3.Click += Button_Click;
				TSCStatus.Button1.Click += Button_Click;
				TSCStatus.Button2.Click += Button_Click;

				line.LineInfoChange += _ConnectionInfoChange;
				line.ConnectionInfoChange += _ConnectionInfoChange;


				string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
				if(ChannelBay !="") ChannelBay="_" + ChannelBay;
				//app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO, app.LineBLL.ConnectioneInfo);
				app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_ONLINE_CHECK_INFO+ ChannelBay, app.LineBLL.OnlineCheckInfo);
				app.LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO+ ChannelBay, app.LineBLL.PingCheckInfo);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public void unregisterEvent()
		{
			try
			{
				CommunicationStatus.Button1.Click -= Button_Click;
				CommunicationStatus.Button2.Click -= Button_Click;
				ControlStatus.Button1.Click -= Button_Click;
				ControlStatus.Button2.Click -= Button_Click;
				ControlStatus.Button3.Click -= Button_Click;
				TSCStatus.Button1.Click -= Button_Click;
				TSCStatus.Button2.Click -= Button_Click;

				line.LineInfoChange -= _ConnectionInfoChange;
				line.ConnectionInfoChange -= _ConnectionInfoChange;


				string ChannelBay = app.ObjCacheManager.ViewerSettings.nats.ChannelBay;
				if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
				//app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO);
				app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_ONLINE_CHECK_INFO+ ChannelBay);
				app.LineBLL.UnsubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_PING_CHECK_INFO+ ChannelBay);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}



		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//var confirmResult = TipMessage_Request_Light.Show("Are you sure to change status ? ");

				if (sender.Equals(CommunicationStatus.Button1))
				{
					strLogContent = CommunicationStatus.Button1.Content.ToString();
					linkStatusChange(true);

					//if (confirmResult != System.Windows.Forms.DialogResult.Yes)
					//{
					//    return;
					//}
					//else
					//{
					//    TipMessage_Type_Light.Show("Successfully Command", "Successfully command to change communication status.", BCAppConstants.INFO_MSG);
					//    await Task.Run(() => linkStatusChange?.Invoke(this, new LinkStatusUpdateEventArgs(SCAppConstants.LinkStatus.LinkOK.ToString())));
					//}
				}
				else if (sender.Equals(CommunicationStatus.Button2))
				{
					strLogContent = CommunicationStatus.Button2.Content.ToString();
					linkStatusChange(false);

					//if (confirmResult != System.Windows.Forms.DialogResult.Yes)
					//{
					//    return;
					//}
					//else
					//{
					//    TipMessage_Type_Light.Show("Successfully Command", "Successfully command to change communication status.", BCAppConstants.INFO_MSG);
					//    await Task.Run(() => linkStatusChange?.Invoke(this, new LinkStatusUpdateEventArgs(SCAppConstants.LinkStatus.LinkFail.ToString())));
					//}
				}
				else if (sender.Equals(ControlStatus.Button1))
				{
					OldStatus = ControlStatus.lab_TitleValue.Text;
					strLogContent = HostControlState.On_Line_Remote.ToString();
					hostModeChange(HostControlState.On_Line_Remote.ToString());
				}
				else if (sender.Equals(ControlStatus.Button2))
				{
					OldStatus = ControlStatus.lab_TitleValue.Text;
					strLogContent = HostControlState.On_Line_Local.ToString();
					hostModeChange(HostControlState.On_Line_Local.ToString());
				}
				else if (sender.Equals(ControlStatus.Button3))
				{
					OldStatus = ControlStatus.lab_TitleValue.Text;
					strLogContent = HostControlState.EQ_Off_line.ToString();
					hostModeChange(HostControlState.EQ_Off_line.ToString());
				}
				else if (sender.Equals(TSCStatus.Button1))
				{
					OldStatus = TSCStatus.lab_TitleValue.Text;
					strLogContent = TSCState.AUTO.ToString();
					tscStateChange(TSCState.AUTO.ToString());
				}
				else if (sender.Equals(TSCStatus.Button2))
				{
					OldStatus = TSCStatus.lab_TitleValue.Text;
					strLogContent = TSCState.PAUSED.ToString();
					tscStateChange(TSCState.PAUSED.ToString());
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void _ConnectionInfoChange(object sender, EventArgs e)
		{
			//Status Tree
			/*
			 * [Communication Status]
			 *			└Enable 
			 *			 .	└	[ControlStatus]
			 *			 .				└	Online Remote
			 *			 .				 .			└	[TSC Status]
			 *			 .				 .					└ AUTO
			 *			 .				 .					└ NONE
			 *			 .				 .					└ PAUSED
			 *			 .				 .					└ PAUSING
			 *			 .				 .					└ TSC_INIT
			 *			 .				└	Online Local
			 *			 .				└  Offline
			 *			└Disable
			 */
			Components.StatusColorType SECSstat_Color = Components.StatusColorType.Red;
			string SECSstat_Display = string.Empty;
			Components.StatusColorType Controlstat_Color = Components.StatusColorType.Red;
			string Controlstat_Display = BCAppConstants.HostModeDisplay.Offline;
			Components.StatusColorType TSCstat_Color = Components.StatusColorType.Red;
			string TSCstat_Display = BCAppConstants.TSCStateDisplay.Init;


			try
			{
				Adapter.Invoke((obj) =>
				{
					//init status
					CommunicationStatus.Button1.IsEnabled = false;
					CommunicationStatus.Button2.IsEnabled = false;
					ControlStatus.Button1.IsEnabled = false;
					ControlStatus.Button2.IsEnabled = false;
					ControlStatus.Button3.IsEnabled = false;
					TSCStatus.Button1.IsEnabled = false;
					TSCStatus.Button2.IsEnabled = false;
					if (line.IS_LINK_HOST)
					{
						SECSstat_Color = Components.StatusColorType.Green;
						//SECSstat_Display = BCAppConstants.SECSLinkDisplay.Link;
						SECSstat_Display = CommunicationStatus.Button1.Content?.ToString() ?? "";
						CommunicationStatus.Button2.IsEnabled = true;
						switch (line.HOST_CONTROL_STATE)
						{
							case HostControlState.On_Line_Remote:
								Controlstat_Color = Components.StatusColorType.Green;
								Controlstat_Display = BCAppConstants.HostModeDisplay.OnlineRemote;
								ControlStatus.Button2.IsEnabled = true;
								ControlStatus.Button3.IsEnabled = true;
								//switch (line.TSC_STATE)
								//{
								//	case TSCState.AUTO:
								//		TSCstat_Color = Components.StatusColorType.Green;
								//		TSCstat_Display = BCAppConstants.TSCStateDisplay.Auto;
								//		TSCStatus.Button2.IsEnabled = true;
								//		break;

								//	case TSCState.NONE:
								//		TSCstat_Color = Components.StatusColorType.Red;
								//		TSCstat_Display = BCAppConstants.TSCStateDisplay.None;
								//		TSCStatus.Button2.IsEnabled = true;
								//		break;

								//	case TSCState.PAUSED:
								//		TSCstat_Color = Components.StatusColorType.Yellow;
								//		TSCstat_Display = BCAppConstants.TSCStateDisplay.Pause;
								//		//TSCStatus.Button1.IsEnabled = true;
								//		TSCStatus.Button1.IsEnabled = true;
								//		break;

								//	case TSCState.PAUSING:
								//		TSCstat_Color = Components.StatusColorType.Yellow;
								//		TSCstat_Display = BCAppConstants.TSCStateDisplay.Pausing;
								//		break;

								//	case TSCState.TSC_INIT:
								//		TSCstat_Color = Components.StatusColorType.Red;
								//		TSCstat_Display = BCAppConstants.TSCStateDisplay.Init;
								//		break;
								//}
								break;

							case HostControlState.On_Line_Local:
								Controlstat_Color = Components.StatusColorType.Yellow;
								Controlstat_Display = BCAppConstants.HostModeDisplay.OnlineLocal;
								ControlStatus.Button1.IsEnabled = true;
								ControlStatus.Button3.IsEnabled = true;
								break;

							case HostControlState.EQ_Off_line:
								Controlstat_Color = Components.StatusColorType.Red;
								Controlstat_Display = BCAppConstants.HostModeDisplay.Offline;
								ControlStatus.Button1.IsEnabled = true;
								ControlStatus.Button2.IsEnabled = true;
								break;
						}
						switch (line.TSC_STATE)
						{
							case TSCState.AUTO:
								TSCstat_Color = Components.StatusColorType.Green;
								TSCstat_Display = BCAppConstants.TSCStateDisplay.Auto;
								TSCStatus.Button2.IsEnabled = true;
								break;

							case TSCState.NONE:
								TSCstat_Color = Components.StatusColorType.Red;
								TSCstat_Display = BCAppConstants.TSCStateDisplay.None;
								TSCStatus.Button2.IsEnabled = true;
								break;

							case TSCState.PAUSED:
								TSCstat_Color = Components.StatusColorType.Yellow;
								TSCstat_Display = BCAppConstants.TSCStateDisplay.Pause;
								//TSCStatus.Button1.IsEnabled = true;
								TSCStatus.Button1.IsEnabled = true;
								break;

							case TSCState.PAUSING:
								TSCstat_Color = Components.StatusColorType.Yellow;
								TSCstat_Display = BCAppConstants.TSCStateDisplay.Pausing;
								break;

							case TSCState.TSC_INIT:
								TSCstat_Color = Components.StatusColorType.Red;
								TSCstat_Display = BCAppConstants.TSCStateDisplay.Init;
								break;
						}
					}
					else
					{
						SECSstat_Color = Components.StatusColorType.Red;
						//SECSstat_Display = BCAppConstants.SECSLinkDisplay.NotLink;
						SECSstat_Display = CommunicationStatus.Button2.Content?.ToString() ?? "";
						CommunicationStatus.Button1.IsEnabled = true;
					}




				}, null);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
			CommunicationStatus.SetConnectSignal(SECSstat_Display, SECSstat_Color);
#if DISPLAYCONTROLSTATUSDETAIL
			ControlStatus.SetConnSignal(Controlstat_Display, Controlstat_Color);
#else
			ControlStatus.SetConnectSignal(Controlstat_Display, Controlstat_Color);
#endif
			TSCStatus.SetConnectSignal(TSCstat_Display, TSCstat_Color);
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
