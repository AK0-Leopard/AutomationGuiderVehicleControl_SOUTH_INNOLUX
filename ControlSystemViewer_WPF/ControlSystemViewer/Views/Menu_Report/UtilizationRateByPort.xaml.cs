using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xaml;
using System.Xml;
using ViewerObject;
using static ViewerObject.VALARM_Def;
using  ViewerObject.REPORT;
using om.mirle.ibg3k0.bc.winform.Common;
using UtilsAPI.Common;

namespace ControlSystemViewer.Views.Menu_Report
{
	/// <summary>
	/// AlarmHistory.xaml 的互動邏輯
	/// </summary>
	public partial class UtilizationRateByPort : UserControl
	{
		#region 公用參數設定

		private static Logger logger = LogManager.GetCurrentClassLogger();
		//private SysExcuteQualityQueryService sysExcuteQualityQueryService;
		private List<VTransExecRate> VTransExecRateList = new List<VTransExecRate>();

		private int AutoTime = 0;
		private VTransExecRate TotalData = null;
		public event EventHandler CloseEvent;
		private Report_Base mybase  = null;
		public WindownApplication app = null;
		private int VHNumbers = 0;

		DateTime LimitStartTime = DateTime.Now;
		DateTime LimitEndTime = DateTime.Now;

		//紀錄查詢當下的選取時間
		private DateTime SelectStartTime = DateTime.MinValue;
		private DateTime SelectEndTime = DateTime.MinValue;

		//Excel Export使用
		XSLXFormat oXSLXFormat = new XSLXFormat();//同時把Excel匯出設定與View Ui顯示Header Unit以這個Class實現
		List<ViewerObject.REPORT.VCMD_ExportDetail> PrintCMDList = new List<ViewerObject.REPORT.VCMD_ExportDetail>();

		private DateTime SearchTimeFrom = DateTime.MinValue;
		private DateTime SearchTimeTo = DateTime.MinValue;

		#endregion 公用參數設定

		public UtilizationRateByPort()
		{
			try
			{
				InitializeComponent();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public void ReSetLimitTime(DateTime _LimitStartTime, DateTime _LimitEndTime)
		{
			try
			{
				LimitStartTime = _LimitStartTime;
				LimitEndTime = _LimitEndTime;

				m_StartDTCbx.BlackoutDates.Clear();
				m_EndDTCbx.BlackoutDates.Clear();

				if (LimitEndTime == DateTime.MaxValue || LimitStartTime == DateTime.MinValue)
				{
					//設定日期的最大與最小值
					m_StartDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
					m_StartDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
					m_EndDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
					m_EndDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);

					//預設起訖日期
					m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-1);
					m_EndDTCbx.SelectedDate = DateTime.Now.AddMilliseconds(-1);

					m_StartDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
					m_StartDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddYears(-3).AddDays(-1)));
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddYears(-3).AddDays(-1)));
				}
				else
				{
					m_StartDTCbx.DisplayDateStart = LimitStartTime;
					m_EndDTCbx.DisplayDateStart = LimitStartTime;
					m_StartDTCbx.DisplayDateEnd = LimitEndTime;
					m_EndDTCbx.DisplayDateEnd = LimitEndTime;
					m_StartDTCbx.SelectedDate = LimitEndTime.AddDays(-1);
					m_EndDTCbx.SelectedDate = LimitEndTime;

					
					m_StartDTCbx.BlackoutDates.Add(new CalendarDateRange(LimitEndTime.AddDays(1), DateTime.MaxValue));
					m_StartDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, LimitStartTime.AddDays(-1)));
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(LimitEndTime.AddDays(1), DateTime.MaxValue));
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, LimitStartTime.AddDays(-1)));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void StartupUI(ref Report_Base report_base, DateTime _LimitStartTime, DateTime _LimitEndTime)
		{
			try
			{

				ReSetLimitTime(_LimitStartTime, _LimitEndTime);

				//build EQID list
				var eqList = new List<string>();
				var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
				var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();
				text_PortID.Text = "";


				//sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
				//cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
				//this.frmquery = _frmquery;

				for (int i = 0; i <= 23; i++)
				{
					cbo_StartH.Items.Add(i.ToString("00"));
					cbo_EndH.Items.Add(i.ToString("00"));
				}
				dgv_log_query.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
				dgv_total_query.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
				
				mybase = report_base;
				ViewSetting();

			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}
		private void ViewSetting()
		{
			ProjectInfo oProjectInfo = WindownApplication.getInstance().ObjCacheManager.GetSelectedProject();
			string CustomerName = oProjectInfo.Customer.ToString();


			#region Hidden Columns
			oXSLXFormat.HiddenColumns.Add(nameof(VTransExecRate.TransTime128));
			oXSLXFormat.HiddenColumns.Add(nameof(VTransExecRate.TransExecTime128));
			oXSLXFormat.HiddenColumns.Add(nameof(VTransExecRate.CycleTime));
			oXSLXFormat.HiddenColumns.Add(nameof(VTransExecRate.MinCycleTime));

			oXSLXFormat.HiddenColumns.Add(nameof(VCMD.STR_START_TIME));
			oXSLXFormat.HiddenColumns.Add(nameof(VCMD.STR_END_TIME));

			if (CustomerName != "M4")
			{
				oXSLXFormat.HiddenColumns.Add(nameof(VCMD_ExportDetail.CARRIER_TYPE));
				oXSLXFormat.HiddenColumns.Add(nameof(VTransExecRateByVHID.LoadPIOError));
				oXSLXFormat.HiddenColumns.Add(nameof(VTransExecRateByVHID.UnLoadPIOError));
			}

			#endregion

			#region Columns ChangeHeader
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.MCSTotalCount), "MCS_CMD");
			if (WindownApplication.getInstance().ObjCacheManager.ViewerSettings.system.SystemType == Settings.SystemType.OHBC)//AGVC沒有這兩個機制，所以AGVC系統中不顯示
			{
				oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.EntryCount), "OHBC_CMD");
			}
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.CMDSuccessRate), "Success");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.VehicleAbort), "CMD_Fail");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.EmptyRetrieval), "Empty");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.DoubleStorage), "Double");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.BarcodeRead), "BCR_Fail");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.UtilizationRate), "Utilization");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.AvgCycleTime), "CycleTime");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.MaxCycleTime), "CycleTime");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.AvgLeadTime), "LeadTime");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.LoadingInterLock), "InterLock(L)");
			oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRate.UnLoadingInterLock), "InterLock(UL)");
			#endregion

			#region Columns Unit
			oXSLXFormat.ColumnUnit.Add(nameof(VTransExecRate.CMDSuccessRate), "%");
			oXSLXFormat.ColumnUnit.Add(nameof(VTransExecRate.UtilizationRate), "%");
			oXSLXFormat.ColumnUnit.Add(nameof(VTransExecRate.AvgCycleTime), "Avg/s");
			oXSLXFormat.ColumnUnit.Add(nameof(VTransExecRate.MaxCycleTime), "Max/s");
			oXSLXFormat.ColumnUnit.Add(nameof(VTransExecRate.AvgLeadTime), "Avg/s");

			oXSLXFormat.ColumnUnit.Add(nameof(VCMD_ExportDetail.CMDCycleTime), "min");
			#endregion

			#region Column Color
			XSLXStyle_Com oXSLXStyleCMDFail = new XSLXStyle_Com();
			oXSLXStyleCMDFail.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
			oXSLXStyleCMDFail.BackgroundColor = System.Drawing.Color.PaleVioletRed;
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.VehicleAbort), oXSLXStyleCMDFail);

			XSLXStyle_Com oXSLXStyleCMDOther = new XSLXStyle_Com();
			oXSLXStyleCMDOther.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
			oXSLXStyleCMDOther.BackgroundColor = System.Drawing.Color.DarkSeaGreen;
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.InterLock), oXSLXStyleCMDOther);
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.EmptyRetrieval), oXSLXStyleCMDOther);
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.DoubleStorage), oXSLXStyleCMDOther);
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.BarcodeRead), oXSLXStyleCMDOther);
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.Scan), oXSLXStyleCMDOther);


			XSLXStyle_Com oXSLXStylePercent = new XSLXStyle_Com();
			oXSLXStylePercent.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
			oXSLXStylePercent.NumberFormat = "0.00%";
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.UtilizationRate), oXSLXStylePercent);
			oXSLXFormat.ColumnFormat.Add(nameof(VTransExecRate.CMDSuccessRate), oXSLXStylePercent);

			#endregion

		}
		private void cb_HrsInterval_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			try
			{
				e.Handled = true;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}


		private async void btn_Search_Click(object sender, EventArgs e)
		{
			try
			{
				SearchTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text), 0, 0);
				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_EndH.Text), 0, 0);

				if (DateTime.Compare(SearchTimeFrom, SearchTimeTo) > 0)
				{
					TipMessage_Type_Light.Show("Failure", string.Format("EndTime must be later than startime."), BCAppConstants.WARN_MSG);
					return;
				}

				dgv_total_query.ItemsSource = null;
				dgv_total_query.Items.Refresh();

				dgv_log_query.ItemsSource = null;
				dgv_log_query.Items.Refresh();
				await search();

			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void btn_Clear_Click(object sender, EventArgs e)
		{
			try
			{
				//cb_HrsInterval.SelectedIndex = -1;
				text_PortID.Text = "";
				dgv_log_query.ItemsSource = null;
				dgv_log_query.Items.Refresh();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private async void btn_Export_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				await export();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public async Task search()
		{
			bool isLoading = false;
			try
			{
				SearchTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text), 0, 0);
				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_EndH.Text), 0, 0);
				text_PortID.Text = "";
				AutoTime = (int)SearchTimeTo.Subtract(SearchTimeFrom).TotalMinutes + 1;
				if (string.IsNullOrWhiteSpace(m_StartDTCbx.SelectedDate.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select start time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (string.IsNullOrWhiteSpace(m_EndDTCbx.SelectedDate.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select end time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (SearchTimeFrom > SearchTimeTo)
				{
					DateTime dtTmp = SearchTimeFrom;
					m_StartDTCbx.SelectedDate = m_EndDTCbx.SelectedDate;
					cbo_StartH.Text = cbo_EndH.Text;
					m_EndDTCbx.SelectedDate = SearchTimeFrom;
					cbo_EndH.Text = SearchTimeFrom.Hour.ToString("00");
				}

				btn_Search.IsEnabled = false;
				DateTime dateTimeFrom = SearchTimeFrom;
				DateTime dateTimeTo = SearchTimeTo;
				string eqpt_id = string.Empty;
				string alarm_id = string.Empty;
				eqpt_id = null;


				if (string.IsNullOrWhiteSpace(txtAlarmID.Text))
				{
					alarm_id = string.Empty;
				}
				else
				{
					alarm_id = txtAlarmID.Text;
				}
				var scApp = WindownApplication.getInstance();

				dateTimeFrom = SearchTimeFrom;
				dateTimeTo = SearchTimeTo;
				SelectStartTime = dateTimeFrom;
				SelectEndTime = dateTimeTo;


				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
				//await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;
				//if (rdo_Error.IsChecked == true) sAlarmLevel = "2";
				await Task.Run(() =>
				{
					VTransExecRateList = scApp.CmdBLL.LoadTransExecRate(dateTimeFrom, dateTimeTo, out VHNumbers)?.OrderBy(info => info.HostSource).ThenBy(info => info.HostDestination).ToList();
				});
				//RATEALARM_List = GetGridData(Alarm_List);
				if (VTransExecRateList == null || VTransExecRateList.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
					dgv_log_query.ItemsSource = VTransExecRateList?.Where(x => x.EntryCount != 0).OrderBy(info => info.HostSource).ThenBy(info => info.HostDestination).ToList();
					//uctl_ElasticQuery_CMDExcute_1.setDataItemsSource(system_qualitys);
				}
				GetTotalInstance(ref VTransExecRateList);

			}
			catch (System.Data.Entity.Core.EntityCommandExecutionException eceex)
			{
				logger.Error(eceex, "EntityCommandExecutionException");
				TipMessage_Type_Light.Show("", eceex.Message, BCAppConstants.WARN_MSG);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
			finally
			{
				btn_Search.IsEnabled = true;
				if (isLoading) ((MainWindow)App.Current.MainWindow).Loading_Stop();
			}
		}

		private async Task export()
		{
			try
			{
				System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
				dlg.Filter = "Files (*.xlsx)|*.xlsx";
				string filename;
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper(WindownApplication.getInstance().ObjCacheManager.ViewerSettings.report.ExportRowLimit);
				//取得轉為 xlsx 的物件
				ClosedXML.Excel.XLWorkbook xlsx = null;
				var scApp = WindownApplication.getInstance();

				var data_list = dgv_log_query.ItemsSource.OfType<VTransExecRate>();
				if (data_list == null || data_list.Count() == 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_EXPORT_NO_DATA"]?.ToString() ?? "There is no data to export, please search first.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
					return;
				}
				ProjectInfo oProjectInfo = scApp.ObjCacheManager.GetSelectedProject();
				string CustomerName = oProjectInfo.Customer.ToString();
				string ProductLine = oProjectInfo.ProductLine.ToString();
				dlg.FileName = "[" + CustomerName + "_" + ProductLine + "] " + this.Name + "_" + SelectStartTime.ToString("yyyyMMddHH") + "-" + SelectEndTime.ToString("yyyyMMddHH");
				if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dlg.FileName))
				{
					return;
				}
				filename = dlg.FileName;



				await Task.Run(() =>
				{
					Dictionary<string, string> dicADR2PORT = new Dictionary<string, string>();
					List<VPORTSTATION> vPortstations = scApp.PortStationBLL.GetVPORTSTATION();
					foreach (VPORTSTATION node in vPortstations)
					{
						if (!dicADR2PORT.ContainsKey(node.ADR_ID))
						{
							dicADR2PORT.Add(node.ADR_ID, node.PORT_ID);
						}
					}

					oXSLXFormat.WKSheetHeader.Clear();
					oXSLXFormat.WKSheetHeader.Add("Report", "StartTime : " + SelectStartTime.ToString("yyyy/MM/dd HH:mm:ss") + ", " + "EndTime : " + SelectEndTime.ToString("yyyy/MM/dd HH:mm:ss"));
					List<VTransExecRate> lsExport = dgv_total_query.ItemsSource.OfType<VTransExecRate>().ToList();
					lsExport.AddRange(data_list.ToList());
					xlsx = helper.Export(lsExport, oXSLXFormat);
					PrintCMDList = scApp.CmdBLL.GetVCMDDetail_Export(SelectStartTime, SelectEndTime)?.OrderBy(info => info.START_TIME).ToList();
					foreach (VCMD node in PrintCMDList)
					{
						if (dicADR2PORT.ContainsKey(node.SOURCE)) node.SOURCE = node.SOURCE + "(" + dicADR2PORT[node.SOURCE] + ")";
						if (dicADR2PORT.ContainsKey(node.DESTINATION)) node.DESTINATION = node.DESTINATION + "(" + dicADR2PORT[node.DESTINATION] + ")";
					}

					helper.AddSheet(ref xlsx, "CMDDetail", PrintCMDList.ToList(), oXSLXFormat);
				});
				if (xlsx != null)
					xlsx.SaveAs(filename);


			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void GetTotalInstance(ref List<VTransExecRate> vTransExecRates )
        {
			try
			{
				VTransExecRate oTransExecRate = null;
				VTransExecRateByVHID oTransExecRateByVHID = null;

				List<VTransExecRate> SourceTransExecRate = new List<VTransExecRate>();
				List<VTransExecRateByVHID> SourceTransExecRateByVHID = new List<VTransExecRateByVHID>();

				dgv_total_query.ItemsSource = null;
				if (vTransExecRates == null) return;
				if (vTransExecRates.Count == 0) return;
				double TotalTransExecRate = 0.0;
				if (VHNumbers == 0 || VHNumbers == -1)
				{

				}
				else
				{
					TotalTransExecRate = Math.Round((double)(vTransExecRates.Sum(i => i.CycleTime) / SearchTimeTo.Subtract(SearchTimeFrom).TotalMinutes / VHNumbers) * 100, 2);

				}
				oTransExecRate = new ViewerObject.REPORT.VTransExecRate(
									_HostSource: "Total",
									_HostDestination: "Total",
									_MCSTotalCount: vTransExecRates.Sum(i => i.MCSTotalCount), _EntryCount: vTransExecRates.Sum(i => i.EntryCount), _CompletedCount: vTransExecRates.Sum(i => i.Completed), _EmptyRetrievalErrorCount: vTransExecRates.Sum(i => i.EmptyRetrieval), _DoubleStorageErrorCount: vTransExecRates.Sum(i => i.DoubleStorage), _InterLockCount: vTransExecRates.Sum(i => i.InterLock), _EQPTAbortErrorCount: vTransExecRates.Sum(i => i.VehicleAbort),
									_BarcodeReadFailCount: vTransExecRates.Sum(i => i.BarcodeRead), _ScanCount: vTransExecRates.Sum(i => i.Scan),
									_LoadingInterLock: vTransExecRates.Sum(i => i.LoadingInterLock), _UnLoadingInterLock: vTransExecRates.Sum(i => i.UnLoadingInterLock),
									_AvgLeadTime: Math.Round((double)(vTransExecRates.Sum(i => i.TransTime128) / vTransExecRates.Sum(i => i.Completed)), 2),
									_CycleTime: vTransExecRates.Sum(i => i.CycleTime), _AvgCycleTime: Math.Round((double)(vTransExecRates.Sum(i => i.TransExecTime128) / vTransExecRates.Sum(i => i.Completed)), 2), _MaxCycleTime: vTransExecRates.Max(i => i.MaxCycleTime), _MinCycleTime: vTransExecRates.Min(i => i.MinCycleTime),
									StartTime: System.Convert.ToDateTime(SearchTimeFrom), EndTime: System.Convert.ToDateTime(SearchTimeTo), _TransTime128: vTransExecRates.Sum(i => i.TransTime128), _TransExecTime128: vTransExecRates.Sum(i => i.TransExecTime128), _TotalTransExecRate: TotalTransExecRate
									)
                {
					LoadPIOError = vTransExecRates.Sum(i => i.LoadPIOError),
					UnLoadPIOError = vTransExecRates.Sum(i => i.UnLoadPIOError)
				};
				SourceTransExecRate.Add(oTransExecRate);
				dgv_total_query.ItemsSource = SourceTransExecRate;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
			finally
            {
				GridRefresh();
            }
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				/*因為DB內有到毫秒，所以終點不應該設前日23時59分59秒，這樣會少濾了23:59:59.xxxxx的資料*/

				DateTime st = new DateTime();
				m_EndDTCbx.SelectedDate = DateTime.Now;
				cbo_EndH.Text = DateTime.Now.Hour.ToString("00");
				if (sender.Equals(HypLLast7days))//前7天  - 今天00:00:00
				{
					st = DateTime.Now.AddDays(-7);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					cbo_EndH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLLast3days))//前3天  - 今天00:00:00
				{
					st = DateTime.Now.AddDays(-3);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					cbo_EndH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLYesterday))//昨天00:00:00 - 今天00:00:00
				{
					st = DateTime.Now.AddDays(-1);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					cbo_EndH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLYesterdayOfDayShift))//昨天日班 - 今天日班
				{
					DateTime ed = new DateTime();
					if (DateTime.Now.Hour >= app.ObjCacheManager.ViewerSettings.system.DayShiftHour)
					{
						st = DateTime.Now.AddDays(-1);
						ed = DateTime.Now;
					}
					else
					{
						st = DateTime.Now.AddDays(-2);
						ed = DateTime.Now.AddDays(-1);
					}


					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: app.ObjCacheManager.ViewerSettings.system.DayShiftHour, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: ed.Year, month: ed.Month, day: ed.Day, hour: app.ObjCacheManager.ViewerSettings.system.DayShiftHour, minute: 0, second: 0);
					cbo_StartH.Text = app.ObjCacheManager.ViewerSettings.system.DayShiftHour.ToString("00");
					cbo_EndH.Text = app.ObjCacheManager.ViewerSettings.system.DayShiftHour.ToString("00");
					await search();
				}
				else if (sender.Equals(HypLToday))// 今天00:00:00 - Now
				{
					st = DateTime.Now;
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLPrevious12Hour))// 前12小時 = Now
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-12);
					cbo_StartH.Text = DateTime.Now.AddHours(-12).Hour.ToString("00");
					m_EndDTCbx.SelectedDate = DateTime.Now;
					cbo_EndH.Text = DateTime.Now.Hour.ToString("00");
					await search();
				}
				else if (sender.Equals(HypLPrevious3Hour))// 前3小時 - Now
				{
					m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-3);
                    cbo_StartH.Text = DateTime.Now.AddHours(-3).Hour.ToString("00");

					m_EndDTCbx.SelectedDate = DateTime.Now;
					cbo_EndH.Text = DateTime.Now.Hour.ToString("00");
					await search();
				}
				else if (sender.Equals(HypLPreviousHour))// 前一個整點  - 目前整點 ex:  Now => 2022/03/14 8:32，則起始為2022/03/14 7:00 結束為 2022/03/14 8:00
				{
					st = DateTime.Now.AddHours(-1);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: st.Hour, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour:DateTime.Now.Hour, minute: 0, second: 0);
					cbo_StartH.Text = DateTime.Now.Hour.ToString("00");
					cbo_EndH.Text = DateTime.Now.Hour.ToString("00");
					await search();
				}
				else if (sender.Equals(HypLThisHour))// 目前整點 - Now
				{
					m_StartDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: DateTime.Now.Hour, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = DateTime.Now;
					cbo_StartH.Text = DateTime.Now.Hour.ToString("00");
					cbo_EndH.Text = DateTime.Now.Hour.ToString("00");
					await search();
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("SelectedDate"))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("There is no data in interval time."), BCAppConstants.WARN_MSG);
				}
				else
				{
					logger.Error(ex, "Exception");
				}
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

		private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			DataGridTextColumn col = e.Column as DataGridTextColumn;
			if (col != null && e.PropertyType == typeof(decimal) || e.PropertyType ==typeof(Int32))
			{
				col.Binding = new Binding(e.PropertyName) { StringFormat = "{0:N0}" };
			}
		}

		private void GridRefresh()
		{
			try
			{
				foreach (DataGridColumn dc in dgv_total_query.Columns)
				{
					StringReader stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Right\" /></Style>");
					XmlReader xmlReader = XmlReader.Create(stringReader);
					Style style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
					switch (dc.Header.ToString())
					{
						case nameof(VTransExecRate.MCSTotalCount):
						case nameof(VTransExecRate.EntryCount):
						case nameof(VTransExecRate.Completed):
						case nameof(VTransExecRate.CMDSuccessRate):
						case nameof(VTransExecRate.VehicleAbort):
						case nameof(VTransExecRate.InterLock):
						case nameof(VTransExecRate.BarcodeRead):
						case nameof(VTransExecRate.Scan):
						case nameof(VTransExecRate.UtilizationRate):
						case nameof(VTransExecRate.AvgCycleTime):
						case nameof(VTransExecRate.MaxCycleTime):
						case nameof(VTransExecRate.AvgLeadTime):
						case nameof(VTransExecRate.LoadingInterLock):
						case nameof(VTransExecRate.UnLoadingInterLock):
						case nameof(VTransExecRate.LoadPIOError):
						case nameof(VTransExecRate.UnLoadPIOError):
							dc.CellStyle = style;
							break;
						case nameof(VTransExecRate.EmptyRetrieval):
						case nameof(VTransExecRate.DoubleStorage):
							dc.CellStyle = style;
							if (WindownApplication.getInstance().ObjCacheManager.ViewerSettings.system.SystemType == Settings.SystemType.AGVC)//AGVC沒有這兩個機制，所以AGVC系統中不顯示
							{
								dc.Visibility = Visibility.Hidden;
							}
							break;
					}
					if (oXSLXFormat.HiddenColumns.Contains(dc.Header.ToString()))
					{
						dc.Visibility = Visibility.Hidden;
					}
					string unit = "";
					if (oXSLXFormat.ColumnUnit.ContainsKey(dc.Header.ToString())) unit = "(" + oXSLXFormat.ColumnUnit[dc.Header.ToString()] + ")";
					if (oXSLXFormat.ChangeHeaders.ContainsKey(dc.Header.ToString()))
					{
						dc.Header = oXSLXFormat.ChangeHeaders[dc.Header.ToString()] ;
					}
					dc.Header = dc.Header + unit;
				}
				foreach (DataGridColumn dc in dgv_log_query.Columns)
				{
					StringReader stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Right\" /></Style>");
					XmlReader xmlReader = XmlReader.Create(stringReader);
					Style style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
					switch (dc.Header.ToString())
					{
						case nameof(VTransExecRate.MCSTotalCount):
						case nameof(VTransExecRate.EntryCount):
						case nameof(VTransExecRate.Completed):
						case nameof(VTransExecRate.CMDSuccessRate):
						case nameof(VTransExecRate.VehicleAbort):
						case nameof(VTransExecRate.InterLock):
						case nameof(VTransExecRate.BarcodeRead):
						case nameof(VTransExecRate.Scan):
						case nameof(VTransExecRate.UtilizationRate):
						case nameof(VTransExecRate.AvgCycleTime):
						case nameof(VTransExecRate.MaxCycleTime):
						case nameof(VTransExecRate.AvgLeadTime):
						case nameof(VTransExecRate.LoadingInterLock):
						case nameof(VTransExecRate.UnLoadingInterLock):
						case nameof(VTransExecRate.LoadPIOError):
						case nameof(VTransExecRate.UnLoadPIOError):
							dc.CellStyle = style;
							break;

						case nameof(VTransExecRate.EmptyRetrieval):
						case nameof(VTransExecRate.DoubleStorage):
							dc.CellStyle = style;
							if (WindownApplication.getInstance().ObjCacheManager.ViewerSettings.system.SystemType == Settings.SystemType.AGVC)//AGVC沒有這兩個機制，所以AGVC系統中不顯示
							{
								dc.Visibility = Visibility.Hidden;
							}
							break;
					}
					if (oXSLXFormat.HiddenColumns.Contains(dc.Header.ToString()))
					{
						dc.Visibility = Visibility.Hidden;
					}
					string unit = "";
					if (oXSLXFormat.ColumnUnit.ContainsKey(dc.Header.ToString())) unit = "(" + oXSLXFormat.ColumnUnit[dc.Header.ToString()] + ")";
					if (oXSLXFormat.ChangeHeaders.ContainsKey(dc.Header.ToString()))
					{
						dc.Header = oXSLXFormat.ChangeHeaders[dc.Header.ToString()] ;
					}
					dc.Header = dc.Header + unit;
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void btn_Filter_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				List<VTransExecRate> list = new List<VTransExecRate>();
				List<VTransExecRateByVHID> listVHID = new List<VTransExecRateByVHID> ();
				if (text_PortID.Text == "")
				{
					list = VTransExecRateList?.Where(x => x.EntryCount != 0).OrderBy(info => info.HostSource).ThenBy(info => info.HostDestination).ToList();
				}
				else
				{

					list = VTransExecRateList?.Where(x => (x.HostDestination.Contains(text_PortID.Text) || x.HostSource.Contains(text_PortID.Text)) && x.EntryCount != 0).OrderBy(info => info.HostSource).ThenBy(info => info.HostDestination).ToList();
				}
				dgv_log_query.ItemsSource = list;
				GetTotalInstance(ref list);


			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
			finally
			{
				GridRefresh();
			}
		}

        private async void btn_Detail_Click(object sender, RoutedEventArgs e)
        {
			mybase.UtilizationRateByHour.m_StartDTCbx.SelectedDate = this.m_StartDTCbx.SelectedDate;
			mybase.UtilizationRateByHour.cbo_StartH.Text = this.cbo_StartH.Text;
			mybase.UtilizationRateByHour.m_EndDTCbx.SelectedDate = this.m_EndDTCbx.SelectedDate;
			mybase.UtilizationRateByHour.cbo_EndH.Text = this.cbo_EndH.Text;
			mybase.Show(app, mybase.TabUtilizationRateByHour.Header?.ToString());
			mybase.UtilizationRateByHour.m_StartDTCbx.SelectedDate = this.m_StartDTCbx.SelectedDate;
			await mybase.UtilizationRateByHour.search();
		}

        private async void btn_AlarmRate_Click(object sender, RoutedEventArgs e)
        {
			if (this.dgv_log_query.SelectedItem == null) return;
			mybase.AlarmRate.m_StartDTCbx.SelectedDate = this.m_StartDTCbx.SelectedDate;
			mybase.AlarmRate.cbo_StartH.Text = this.cbo_StartH.Text;
			mybase.AlarmRate.m_EndDTCbx.SelectedDate =  this.m_EndDTCbx.SelectedDate;
			mybase.AlarmRate.cbo_EndH.Text = this.cbo_EndH.Text;
			mybase.AlarmRate.text_EQID.Text = ((VTransExecRateByVHID)this.dgv_log_query.SelectedItem).VHID.ToString().Trim();
			mybase.AlarmRate.rdo_Error.IsChecked = true;
			mybase.Show(app, mybase.TabAlarmRate.Header?.ToString());
			await mybase.AlarmRate.search();
			mybase.AlarmRate.btn_Filter.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

		}

		private void btn_CalTime_Click(object sender, RoutedEventArgs e)
		{
			if (text_AutoTime.Text.Trim() == "") return;
			try
			{
				DateTime? NowCal = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text), 0, 0);
				DateTime? AfterCal = null;
				switch (cbo_Mag.Text)
				{
					case "Hour":
						AfterCal = NowCal.Value.AddHours(Convert.ToInt32(text_AutoTime.Text));
						break;
					case "Day":
						AfterCal = NowCal.Value.AddDays(Convert.ToInt32(text_AutoTime.Text));
						break;
					case "Week":
						AfterCal = NowCal.Value.AddDays(Convert.ToInt32(text_AutoTime.Text) * 7);
						break;
					case "Month":
						AfterCal = NowCal.Value.AddMonths(Convert.ToInt32(text_AutoTime.Text));
						break;
					default:
						AfterCal = null;
						break;
				}
				m_EndDTCbx.SelectedDate = AfterCal;
				cbo_EndH.Text = AfterCal.Value.Hour.ToString("00");

			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}



        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
			try
			{
				//若當前時間已超過最大值，重新設定最大值
				if (DateTime.Now > m_EndDTCbx.DisplayDateEnd)
				{
					m_StartDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
					m_EndDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		#region 游標事件
		System.Windows.Controls.Label label = null;
		private void MyMouseEnterHandler(object sender, MouseEventArgs e)
		{
			try
			{
				string Keysender = "";
				System.Windows.Controls.Primitives.DataGridColumnHeader senderItem = (System.Windows.Controls.Primitives.DataGridColumnHeader)sender;

				//取得絕對位置
				Point relativePoint = senderItem.TransformToAncestor(MainGrid)
									   .Transform(new Point(0, 0));

				ResourceDictionary dic = Application.Current.Resources.MergedDictionaries[0];//取得現在的語系內容

				if (senderItem.Content.ToString().Contains("("))
				{
					Keysender = "DESC_" + senderItem.Content.ToString().Split('(')[0].ToUpper();
				}
				else
				{
					Keysender = "DESC_" + senderItem.Content.ToString().ToUpper();
				}



				if (!dic.Contains(Keysender)) return;

				//新增Label
				label = new System.Windows.Controls.Label();
				label.Background = System.Windows.Media.Brushes.LightYellow;
				label.FontSize = 16;
				label.Content = dic[Keysender];

				label.Width = Double.NaN;//自動適應內容調整長度
				label.Height = 30;

				//設定Label位置
				Canvas.SetLeft(label, relativePoint.X);
				Canvas.SetTop(label, relativePoint.Y - label.Height);
				canvas.Children.Add(label);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}


		}

		private void MyMouseLeaveHandler(object sender, MouseEventArgs e)
		{
			try
			{
				if (label != null)
				{
					canvas.Children.Remove(label);
					label = null;
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}

		}

		#endregion
	}
}

