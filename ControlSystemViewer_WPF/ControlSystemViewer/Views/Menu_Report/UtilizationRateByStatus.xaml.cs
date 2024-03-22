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
	public partial class UtilizationRateByStatus : UserControl
	{
		#region 公用參數設定

		private static Logger logger = LogManager.GetCurrentClassLogger();
		//private SysExcuteQualityQueryService sysExcuteQualityQueryService;
		private List<VUtilizationRateByStatus> VUtilizationRateByStatusList = new List<VUtilizationRateByStatus>();
		private List<VTransExecRateByVHID> VTransExecRateByVHIDList = new List<VTransExecRateByVHID>();
		private int AutoTime = 0;
		private VUtilizationRateByStatus TotalData = null;
		public event EventHandler CloseEvent;
		private Report_Base mybase  = null;
		public WindownApplication app = null;
		private int VHNumbers = 0;

		DateTime LimitStartTime = DateTime.Now;
		DateTime LimitEndTime = DateTime.Now;

		//紀錄查詢當下的選取時間
		private DateTime SelectStartTime = DateTime.MinValue;
		private DateTime SelectEndTime = DateTime.MinValue;

		private DateTime SearchTimeFrom = DateTime.MinValue;
		private DateTime SearchTimeTo = DateTime.MinValue;


		//Excel Export使用
		XSLXFormat oXSLXFormat = new XSLXFormat();//同時把Excel匯出設定與View Ui顯示Header Unit以這個Class實現
		List<ViewerObject.REPORT.VCMD_ExportDetail> PrintCMDList = new List<ViewerObject.REPORT.VCMD_ExportDetail>();

		#endregion 公用參數設定

		public UtilizationRateByStatus()
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

				app = WindownApplication.getInstance();

				//build EQID list
				var eqList = new List<string>();
				var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
				var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();



				//sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
				//cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);


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

			#region Hidden Columns

			#endregion

			#region Columns ChangeHeader
			#endregion

			#region Columns Unit

			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.MCSCycleTime), "Sum/h");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.OHTCCycleTime), "Sum/h");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.IdleTime), "h");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.IntervalTime), "h");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.LongChargeTime), "h");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.UnScheduleDownTime), "min");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.ScheduleDownTime), "min");
			oXSLXFormat.ColumnUnit.Add(nameof(VUtilizationRateByStatus.UtilizationRate), "%");
			#endregion

			#region Column Color

			XSLXStyle_Com oXSLXStylePercent = new XSLXStyle_Com();
			oXSLXStylePercent.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
			oXSLXStylePercent.NumberFormat = "0.00%";
			oXSLXFormat.ColumnFormat.Add(nameof(VUtilizationRateByStatus.UtilizationRate), oXSLXStylePercent);

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
				SearchTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, 0, 0, 0);
				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, 0, 0, 0);

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

				text_VHID.Text = "";
				SearchTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, 0, 0, 0);
				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day,0, 0, 0);

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
					m_EndDTCbx.SelectedDate = SearchTimeFrom;
				}

				btn_Search.IsEnabled = false;
				DateTime dateTimeFrom = System.Convert.ToDateTime(m_StartDTCbx.SelectedDate);
				DateTime dateTimeTo = System.Convert.ToDateTime(m_EndDTCbx.SelectedDate);
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

				Dictionary<string, long> dicMCSRunTime = new Dictionary<string, long>();
				Dictionary<string, long> dicOHTCRunTime = new Dictionary<string, long>();
				Dictionary<string, long> dicChargingTime = new Dictionary<string, long>();
				Dictionary<string, long> dicAlarmTime = new Dictionary<string, long>();
				Dictionary<string, long> dicScheduleDownTime = new Dictionary<string, long>();


				await Task.Run(async () =>
				{
					dicMCSRunTime = await scApp.StatusInfoBLL.GetRuneTime(dateTimeFrom, dateTimeTo);
					dicOHTCRunTime = await scApp.StatusInfoBLL.GetOHTCRuneTime(dateTimeFrom, dateTimeTo);
					dicChargingTime = await scApp.StatusInfoBLL.GetLongChargeTime(dateTimeFrom, dateTimeTo);

					dicAlarmTime = await scApp.StatusInfoBLL.GetStatusAlarmTime(dateTimeFrom, dateTimeTo);
					dicScheduleDownTime = await scApp.StatusInfoBLL.GetScheduleDownTime(dateTimeFrom, dateTimeTo);

					VTransExecRateByVHIDList = scApp.CmdBLL.LoadTransExecRateGroupByEQPT(dateTimeFrom, dateTimeTo)?.Where(x => x.MCSTotalCount != 0).OrderBy(info => info.VHID).ToList();
				});

				VUtilizationRateByStatusList = GetUtilizationRateByStatusList(ref dicMCSRunTime, ref dicOHTCRunTime, ref dicChargingTime, ref dicAlarmTime, ref dicScheduleDownTime);
				//RATEALARM_List = GetGridData(Alarm_List);
				if (VUtilizationRateByStatusList == null || VUtilizationRateByStatusList.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
					dgv_log_query.ItemsSource = VUtilizationRateByStatusList?.OrderBy(info => info.VHID).ToList();
					//uctl_ElasticQuery_CMDExcute_1.setDataItemsSource(system_qualitys);
				}
				GetTotalInstance( ref VUtilizationRateByStatusList);

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

		private List<VUtilizationRateByStatus> GetUtilizationRateByStatusList(ref Dictionary<string,long> dicMCSRunTime,
																			  ref Dictionary<string, long> dicOHTCRunTime,
																			  ref Dictionary<string, long> dicChargingTime,
																			  ref Dictionary<string, long> dicAlarmTime,
																			  ref Dictionary<string, long> dicScheduleDownTime)
        {
			List < VUtilizationRateByStatus > lsReturn = new List<VUtilizationRateByStatus >();
			try
            {
				long ChargingTime = 0;
				long OHTCRunTime = 0;
				long AlarmTime = 0;
				long ScheduleDownTime = 0;
				foreach (string vhid in dicMCSRunTime.Keys)
                {
					ChargingTime = 0;
					OHTCRunTime = 0;
					AlarmTime = 0;
					ScheduleDownTime = 0;

					if (dicChargingTime.ContainsKey(vhid)) ChargingTime = dicChargingTime[vhid];
					if (dicOHTCRunTime.ContainsKey(vhid)) OHTCRunTime = dicOHTCRunTime[vhid];
					if (dicAlarmTime.ContainsKey(vhid)) AlarmTime = dicAlarmTime[vhid];
					if (dicScheduleDownTime.ContainsKey(vhid)) ScheduleDownTime = dicScheduleDownTime[vhid];

					VUtilizationRateByStatus oVUtilizationRateByStatus = new VUtilizationRateByStatus(vhid, dicMCSRunTime[vhid], OHTCRunTime, ChargingTime, AlarmTime, ScheduleDownTime,
																									  SearchTimeFrom, SearchTimeTo, VTransExecRateByVHIDList.Where(info => info.VHID.Contains(vhid)).Select(info => info.EntryCount).FirstOrDefault());
					lsReturn.Add(oVUtilizationRateByStatus);
				}

				return lsReturn;
			}
			catch(Exception ex)
            {
				logger.Error(ex, "Exception");
				return lsReturn;
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

				var data_listVH = dgv_log_query.ItemsSource.OfType<VUtilizationRateByStatus>();
				if (data_listVH == null || data_listVH.Count() == 0)
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
				//建立 xlxs 轉換物件
				//取得轉為 xlsx 的物件

				await Task.Run(() =>
				{
					Dictionary<string, string> dicADR2PORT = new Dictionary<string, string>();
					List<VPORTSTATION> vPortstations = scApp.PortStationBLL.GetVPORTSTATION();
					foreach (VPORTSTATION node in vPortstations)
					{
						dicADR2PORT.Add(node.ADR_ID, node.PORT_ID);
					}

					oXSLXFormat.WKSheetHeader.Clear();
					oXSLXFormat.WKSheetHeader.Add("Report", "StartTime : " + SelectStartTime.ToString("yyyy/MM/dd HH:mm:ss") + ", " + "EndTime : " + SelectEndTime.ToString("yyyy/MM/dd HH:mm:ss"));
					xlsx = helper.Export(data_listVH.ToList(), oXSLXFormat);
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

		private void GetTotalInstance(ref List<VUtilizationRateByStatus> vTransExecRateByVHIDs )
        {
			try
			{
				VUtilizationRateByStatus oTransExecRateByVHID = null;
				List<VUtilizationRateByStatus> SourceTransExecRateByVHID = new List<VUtilizationRateByStatus>();

				dgv_total_query.ItemsSource = null;
				if (vTransExecRateByVHIDs == null) return;
				if (vTransExecRateByVHIDs.Count == 0) return;
				oTransExecRateByVHID = new VUtilizationRateByStatus("*****",
																	vTransExecRateByVHIDs.Sum(info=> info.MCSCycleTime*60*60), vTransExecRateByVHIDs.Sum(info => info.OHTCCycleTime * 60 * 60), vTransExecRateByVHIDs.Sum(info => info.LongChargeTime * 60 * 60), vTransExecRateByVHIDs.Sum(info => info.UnScheduleDownTime * 60 ), vTransExecRateByVHIDs.Sum(info => info.ScheduleDownTime  * 60),
																	SearchTimeFrom,SearchTimeTo, vTransExecRateByVHIDs.Sum(info => info.MCSCMDCount), Math.Round(vTransExecRateByVHIDs.Sum(info => info.UtilizationRate)/ vTransExecRateByVHIDs.Count(),2), vTransExecRateByVHIDs.Sum(info=>info.IdleTime *60*60));
											
				SourceTransExecRateByVHID.Add(oTransExecRateByVHID);
				dgv_total_query.ItemsSource = SourceTransExecRateByVHID;
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
				DateTime ed = new DateTime();
				m_EndDTCbx.SelectedDate = LimitEndTime;
				var app = WindownApplication.getInstance();
				if (sender.Equals(HypLPreviousQuarter))//上季初月1號00:00:00 - 本季初月1號00:00:00
				{
					st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
					m_EndDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					st = st.AddDays(-1);
					st = st.AddMonths(0 - (st.Month - 1) % 3).AddDays(1 - st.Day); //回到上季初月1號
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					await search();
				}
				else if (sender.Equals(HypLThisQuarter))//本季初月1號00:00:00 - Now
				{
					st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					await search();
				}
				else if (sender.Equals(HypLPreviousMonth))//上月1號00:00:00 - 本月1號00:00:00
				{
					st = DateTime.Now.AddMonths(-1);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: 1, hour: 0, minute: 0, second: 0);

					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: 1, hour: 0, minute: 0, second: 0);
					await search();
				}
				else if (sender.Equals(HypLThisMonth))//本月1號00:00:00 - Now
				{
					st = DateTime.Now.AddDays(1 - DateTime.Now.Day);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);

					await search();
				}
				else if (sender.Equals(HypLLast7days))//前7天  - 今天00:00:00
				{
					st = DateTime.Now.AddDays(-7);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
					await search();
				}
				else if (sender.Equals(HypLYesterday))//昨天00:00:00 - 今天00:00:00
				{
					st = DateTime.Now.AddDays(-1);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
					await search();
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
						
						case nameof(VUtilizationRateByStatus.UtilizationRate):
						case nameof(VUtilizationRateByStatus.MCSCycleTime):
						case nameof(VUtilizationRateByStatus.OHTCCycleTime):
						case nameof(VUtilizationRateByStatus.LongChargeTime):
						case nameof(VUtilizationRateByStatus.UnScheduleDownTime):
						case nameof(VUtilizationRateByStatus.ScheduleDownTime):
						case nameof(VUtilizationRateByStatus.IdleTime):
						case nameof(VUtilizationRateByStatus.MCSCMDCount):
						case nameof(VUtilizationRateByStatus.IntervalTime):
							dc.CellStyle = style;
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
						dc.Header = oXSLXFormat.ChangeHeaders[dc.Header.ToString()];
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
						case nameof(VUtilizationRateByStatus.UtilizationRate):
						case nameof(VUtilizationRateByStatus.MCSCycleTime):
						case nameof(VUtilizationRateByStatus.OHTCCycleTime):
						case nameof(VUtilizationRateByStatus.LongChargeTime):
						case nameof(VUtilizationRateByStatus.UnScheduleDownTime):
						case nameof(VUtilizationRateByStatus.ScheduleDownTime):
						case nameof(VUtilizationRateByStatus.IdleTime):
						case nameof(VUtilizationRateByStatus.MCSCMDCount):
						case nameof(VUtilizationRateByStatus.IntervalTime):
							dc.CellStyle = style;
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
				List<VUtilizationRateByStatus> list = new List<VUtilizationRateByStatus>();
				List<VUtilizationRateByStatus> listVHID = new List<VUtilizationRateByStatus> ();
				if (text_VHID.Text == "")
				{
					listVHID = VUtilizationRateByStatusList?.OrderBy(info => info.VHID).ToList();
				}
				else
				{

					listVHID = VUtilizationRateByStatusList?.Where(x => x.VHID.Contains(text_VHID.Text)).OrderBy(info => info.VHID).ToList();
				}
				dgv_log_query.ItemsSource = listVHID;
				GetTotalInstance( ref listVHID);


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

  //      private async void btn_Detail_Click(object sender, RoutedEventArgs e)
  //      {
		//	mybase.UtilizationRateByHour.m_StartDTCbx.SelectedDate = this.m_StartDTCbx.SelectedDate;
		//	mybase.UtilizationRateByHour.cbo_StartH.Text = this.cbo_StartH.Text	;
		//	mybase.UtilizationRateByHour.m_EndDTCbx.SelectedDate = this.m_EndDTCbx.SelectedDate;
		//	mybase.UtilizationRateByHour.cbo_EndH.Text = this.cbo_EndH.Text;
		//	mybase.Show(app, mybase.TabUtilizationRateByHour.Header?.ToString());
		//	await mybase.UtilizationRateByHour.search();
		//}

  //      private async void btn_AlarmRate_Click(object sender, RoutedEventArgs e)
  //      {
		//	if (this.dgv_log_query.SelectedItem == null) return;
		//	mybase.AlarmRate.m_StartDTCbx.SelectedDate = this.m_StartDTCbx.SelectedDate;
		//	mybase.AlarmRate.cbo_StartH.Text = this.cbo_StartH.Text;
		//	mybase.AlarmRate.m_EndDTCbx.SelectedDate = this.m_EndDTCbx.SelectedDate;
		//	mybase.AlarmRate.cbo_EndH.Text = this.cbo_EndH.Text;
		//	mybase.AlarmRate.text_EQID.Text = ((VTransExecRateByVHID)this.dgv_log_query.SelectedItem).VHID.ToString().Trim();
		//	mybase.AlarmRate.rdo_Error.IsChecked = true;
		//	mybase.Show(app, mybase.TabAlarmRate.Header?.ToString());
		//	await mybase.AlarmRate.search();
		//	mybase.AlarmRate.btn_Filter.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

		//}

		private void btn_CalTime_Click(object sender, RoutedEventArgs e)
		{
			if (text_AutoTime.Text.Trim() == "") return;
			try
			{
				DateTime? NowCal = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, 0, 0, 0);
				DateTime? AfterCal = null;
				switch (cbo_Mag.Text)
				{
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

				if(senderItem.Content.ToString().Contains("("))
                {
					Keysender = "DESC_" + senderItem.Content.ToString().Split('(')[0].ToUpper();
				}
				else
                {
					Keysender = "DESC_" + senderItem.Content.ToString().ToUpper();
				}



				if (! dic.Contains(Keysender)) return;

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
			catch(Exception ex)
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
			catch(Exception ex)
            {
				logger.Error(ex, "Exception");
			}
			
        }

        #endregion
    }
}

