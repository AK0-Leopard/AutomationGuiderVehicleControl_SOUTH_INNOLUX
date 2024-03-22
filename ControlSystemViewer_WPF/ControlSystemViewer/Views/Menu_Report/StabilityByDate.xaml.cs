using ChartConverter.ChartDataClass;
using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using ControlSystemViewer.PopupWindows;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using om.mirle.ibg3k0.bc.winform.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
using UtilsAPI.Common;
using ViewerObject;
using static ViewerObject.VALARM_Def;
using ViewerObject.REPORT;

namespace ControlSystemViewer.Views.Menu_Report
{
	/// <summary>
	/// AlarmHistory.xaml 的互動邏輯
	/// </summary>
	public partial class StabilityByDate : UserControl
	{
		#region 公用參數設定

		private static Logger logger = LogManager.GetCurrentClassLogger();
		//private SysExcuteQualityQueryService sysExcuteQualityQueryService;
		private Dictionary<string,VMCBF> VMCBF_List = new Dictionary<string, VMCBF>();
		private Dictionary<string,VMCBF> VMCBFCMD_List = new Dictionary<string, VMCBF>();
		private Dictionary<string,VMTBF> VMTBF_List = new Dictionary<string, VMTBF>();

		private List<VMeanTimeByVH> MeanTimeByVH_List = new List<VMeanTimeByVH>();
		List<VCMD_OHTC> VCMD_List = new List<VCMD_OHTC>();
		private int AutoTime = 0;
		public event EventHandler CloseEvent;
		int AlarmGroupTime = 30;


		DateTime LimitStartTime = DateTime.Now;
		DateTime LimitEndTime = DateTime.Now;
		int iIntervalDay = 0;
		List<string> dtColumnKey = new List<string>();
		Dictionary<string, Dictionary<string,double>> dtValues =new Dictionary<string, Dictionary<string, double>>();


		XSLXFormat oXSLXFormat = new XSLXFormat();//同時把Excel匯出設定與View Ui顯示Header Unit以這個Class實現
		DataView dvExport = new DataView();
		DateTime SelectEndTime = DateTime.MinValue;
		private string ExportHeader ="";
		List<VALARM> PrintAlarm_List = null;
		private DateTime SearchTimeTo = DateTime.MinValue;
		#endregion 公用參數設定

		enum ModeTimeInterval
        {
			On =0,
			Off =1
        }


		public StabilityByDate()
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

				m_EndDTCbx.BlackoutDates.Clear();
				if (LimitEndTime == DateTime.MaxValue || LimitStartTime == DateTime.MinValue)
				{
					//設定日期的最大與最小值
					m_EndDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
					m_EndDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);

					//預設起訖日期
					m_EndDTCbx.SelectedDate = DateTime.Now.AddMilliseconds(-1);

					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.Today.AddDays(1), DateTime.MaxValue));
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddYears(-3).AddDays(-1)));
				}
				else
				{
					m_EndDTCbx.DisplayDateStart = LimitStartTime;
					m_EndDTCbx.DisplayDateEnd = LimitEndTime;
					m_EndDTCbx.SelectedDate = LimitEndTime;

					
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(LimitEndTime.AddDays(1), DateTime.MaxValue));
					m_EndDTCbx.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, LimitStartTime.AddDays(-1)));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void StartupUI(DateTime _LimitStartTime, DateTime _LimitEndTime)
		{
			try
			{
				ReSetLimitTime(_LimitStartTime, _LimitEndTime);

				//build EQID list
				var eqList = new List<string>();
				var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
				var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();
				AlarmGroupTime = WindownApplication.getInstance().ObjCacheManager.ViewerSettings.report.AlarmGroupMin;

				//sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
				//cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
				//m_StartDTCbx.ValueChanged += new RoutedPropertyChangedEventHandler<object>(dtp_ValueChanged);
				//m_EndDTCbx.ValueChanged += new RoutedPropertyChangedEventHandler<object>(dtp_ValueChanged);
				dgv_log_query.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
				//this.frmquery = _frmquery;
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
			oXSLXFormat.HiddenColumns.Add(nameof(VMTBF.TimeInterval));
			#endregion

			#region Columns ChangeHeader
			//oXSLXFormat.ChangeHeaders.Add(nameof(VTransExecRateByVHID.MCSTotalCount), "MCS_CMD");
			oXSLXFormat.ChangeHeaders.Add(nameof(VMeanTimeByVH.MCBF_CMD), "MCBF(CMD)");
			oXSLXFormat.ChangeHeaders.Add(nameof(VMeanTimeByVH.MCBF_Alarn), "MCBF(Alarm)");
			#endregion

			#region Columns Unit
			oXSLXFormat.ColumnUnit.Add(nameof(VMTBF.MTBF), "h");
			oXSLXFormat.ColumnUnit.Add(nameof(VMTBF.ActiveTime), "h");

			#endregion

			#region Column Color

			#endregion


			XSLXStyle_Com oXSLXStyleStringDate = new XSLXStyle_Com();
			oXSLXStyleStringDate.NumberFormat = "yyyy/mm/dd HH:MM:ss";
			oXSLXFormat.ColumnFormat.Add(nameof(VALARM.RPT_DATE_TIME), oXSLXStyleStringDate);
			oXSLXFormat.ColumnFormat.Add(nameof(VALARM.CLEAR_DATE_TIME), oXSLXStyleStringDate);
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
				dgv_log_query.ItemsSource = null;
				dvExport = null;
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

		private async Task search()
		{
			bool isLoading = false;
			try
			{

				dvExport = null;
				DateTime datetemp = m_EndDTCbx.SelectedDate.Value;
				SearchTimeTo = new DateTime(datetemp.Year, datetemp.Month, datetemp.Day,0 , 0, 0);


				if (string.IsNullOrWhiteSpace(m_EndDTCbx.SelectedDate.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select end time."), BCAppConstants.WARN_MSG);
					return;
				}

				if (text_IntervalDays.Text.Trim()=="")
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please input IntervalDays."), BCAppConstants.WARN_MSG);
					return;
				}
				if (Convert.ToInt32(text_IntervalDays.Text) < 1)
				{
					TipMessage_Type_Light.Show("Failure", string.Format("IntervalDays must be larger than 1."), BCAppConstants.WARN_MSG);
					return;
				}

				AutoTime = Convert.ToInt32(text_IntervalDays.Text) * 24;
				iIntervalDay = Convert.ToInt32(text_IntervalDays.Text);


				btn_Search.IsEnabled = false;
				DateTime dateTimeFrom = SearchTimeTo.AddDays(0- iIntervalDay);
				DateTime dateTimeTo = SearchTimeTo;
				SelectEndTime = dateTimeTo;
				dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, 0, 0, DateTimeKind.Local);
				dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, 0, 0, DateTimeKind.Local);
				string alarm_id = string.Empty;



				if (string.IsNullOrWhiteSpace(txtAlarmID.Text))
				{
					alarm_id = string.Empty;
				}
				else
				{
					alarm_id = txtAlarmID.Text;
				}
				var scApp = WindownApplication.getInstance();


				ExportHeader = "EndTime : " + dateTimeTo.ToString("yyyy/MM/dd HH") + ", IntervalDay : " + iIntervalDay.ToString();
				List<VALARM> Alarm_List = null;

				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
				//await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;
				string sAlarmLevel = "2";
				WindownApplication app = WindownApplication.getInstance();
				await Task.Run(() =>
				{					
					Alarm_List = scApp.AlarmBLL.GetAlarmsByConditions(dateTimeFrom, dateTimeTo, Alarm_Level: sAlarmLevel, cleartimenotnull: true)?.Where(info => app.ObjCacheManager.ViewerSettings.report.CheckAlarmCode(info.ALARM_CODE) && Convert.ToDateTime(info.CLEAR_DATE_TIME).Subtract(Convert.ToDateTime(info.RPT_DATE_TIME)).TotalMinutes > WindownApplication.getInstance().ObjCacheManager.ViewerSettings.report.MeanTimeOverMin).OrderBy(info => info.RPT_DATE_TIME).ToList();
					VCMD_List = scApp.CmdBLL.GetHCMDOHTCByConditions(dateTimeFrom, dateTimeTo).OrderBy(info => info.VH_ID).ToList();
				});
				VMCBF_List.Clear();
				VMTBF_List.Clear();
				VMCBFCMD_List.Clear();
				MeanTimeByVH_List.Clear();

				VMCBF_List = GetGridData(Alarm_List.Where(info => Convert.ToDateTime(info.RPT_DATE_TIME) > dateTimeTo.AddDays(0 -iIntervalDay)  && Convert.ToDateTime(info.RPT_DATE_TIME) <= dateTimeTo ).ToList(), VCMD_List.Where(info => Convert.ToDateTime(info.MCS_CMD_START_TIME) >= dateTimeTo.AddDays(0  - iIntervalDay) && Convert.ToDateTime(info.MCS_CMD_START_TIME) < dateTimeTo).ToList(), dateTimeTo.AddDays(0 - iIntervalDay), dateTimeTo);
					//AddDatatableValue(ref VMCBF_List, dateTimeTo.AddDays(0 - i * iBlankDay - iIntervalDay) , dateTimeTo.AddDays(0 - i * iBlankDay) );

				VMTBF_List = GetGridData(Alarm_List.Where(info => Convert.ToDateTime(info.RPT_DATE_TIME) > dateTimeTo.AddDays(0  - iIntervalDay) && Convert.ToDateTime(info.RPT_DATE_TIME) <= dateTimeTo).ToList(), dateTimeTo.AddDays(0 - iIntervalDay), dateTimeTo);
					//AddDatatableValue(ref VMTBF_List, dateTimeTo.AddDays(0 - i * iBlankDay - iIntervalDay), dateTimeTo.AddDays(0 - i * iBlankDay));
				List<VMCBF> temp=new List<VMCBF>();
				temp = scApp.CmdBLL.LoadMTTRHCmd(dateTimeTo.AddDays(0 - iIntervalDay), dateTimeTo).OrderBy(info => info.VH_ID).ToList();
				foreach(VMCBF tp in temp)
                {
					VMCBFCMD_List.Add(tp.VH_ID, tp);
				}
				
				//AddDatatableValueCMD(ref VMCBFCMD_List, dateTimeTo.AddDays(0 - i * iBlankDay - iIntervalDay), dateTimeTo.AddDays(0 - i * iBlankDay));
				MeanTimeByVH_List = GetMeanTimeByVHList(ref VMCBF_List,ref VMTBF_List,ref VMCBFCMD_List);

				if (MeanTimeByVH_List == null || MeanTimeByVH_List.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{

					dgv_log_query.ItemsSource = MeanTimeByVH_List;
					GridRefresh();				
					//uctl_ElasticQuery_CMDExcute_1.setDataItemsSource(system_qualitys);
				}
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
		private List<VMeanTimeByVH> GetMeanTimeByVHList(ref Dictionary<string,VMCBF> vmcbf_list,ref Dictionary<string,VMTBF> vmtbf_list, ref Dictionary<string,VMCBF> vmcbfcmd_list)
        {
			
			List <VMeanTimeByVH> lsReturn = new List<VMeanTimeByVH>();
			try
            {
				double tpMTBF = 0;
				foreach (string vhid in vmcbfcmd_list.Keys)
                {
					if (!vmtbf_list.ContainsKey(vhid.Trim())) { tpMTBF = iIntervalDay * 24; }
					else { tpMTBF= vmtbf_list[vhid.Trim()].MTBF; }

					VMeanTimeByVH oVMeanTimeByVH = new VMeanTimeByVH(vhid, tpMTBF, vmcbf_list[vhid.Trim()].MCBF, vmcbfcmd_list[vhid].MCBF, vmcbfcmd_list[vhid].CMDCount);
					lsReturn.Add(oVMeanTimeByVH);
				}
				return lsReturn;
			}
			catch (Exception ex)
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

				var data_listVH = dgv_log_query.ItemsSource.OfType<VMeanTimeByVH>();
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
				dlg.FileName = "[" + CustomerName + "_" + ProductLine + "] " + this.Name + "_"  + SelectEndTime.ToString("yyyy-MM-dd");
				if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dlg.FileName))
				{
					return;
				}
				filename = dlg.FileName;
				//建立 xlxs 轉換物件
				//取得轉為 xlsx 的物件

				await Task.Run(() =>
				{
					oXSLXFormat.WKSheetHeader.Clear();
					oXSLXFormat.WKSheetHeader.Add("Report", ExportHeader);
					xlsx = helper.Export(data_listVH.ToList(), oXSLXFormat);
				});
				if (xlsx != null)
					xlsx.SaveAs(filename);


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
				/*因為DB內有到毫秒，所以終點不應該設前日23時59分59秒，這樣會少濾了23:59:59.xxxxx的資料*/

				DateTime st = new DateTime();
				m_EndDTCbx.SelectedDate = DateTime.Now;
				bool SearchFlag = false;

			
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


		#region MCBF(Alarm)
		private Dictionary<string,VMCBF> GetGridData(List<VALARM> AlarmList, List<VCMD_OHTC> VCMD_List,DateTime StartTime,DateTime EndTime)
		{
			try
			{
				//if (AlarmList.Count == 0) return new List<VMCBF>();
				DateTime defaultTime = DateTime.Now;

				#region First Report
				VMCBF_FirstReport oFirstReport = new VMCBF_FirstReport();
				Dictionary<string, List<VMCBF_FirstReport>> dicFirstReport = new Dictionary<string, List<VMCBF_FirstReport>>();
				#endregion

				#region Final Report

				Dictionary<string, VMCBF> dicAlarm = new Dictionary<string, VMCBF>();
				#endregion

				#region DateTime for EachItem
				DateTime oRPTTime = defaultTime;
				DateTime oCLRTime = defaultTime;
				#endregion

				#region DateTmie: 區間累計
				Dictionary<string, DateTime> RPTTimeByVHID = new Dictionary<string, DateTime>();
				Dictionary<string, DateTime> CLRTimeByVHID = new Dictionary<string, DateTime>();
				#endregion

				//First Report Data
				if(AlarmList.Count >0) oFirstReport.VH_ID = AlarmList[0].EQPT_ID;
				bool fnFlag = false;//Final Add Flag

				#region GetFirstReport
				for (int i = 0; i < AlarmList.Count; i++)
				{
					if (i == AlarmList.Count - 1)
					{
						fnFlag = true;
					}
					if (AlarmList[i].EQPT_ID == null) continue;
					if (AlarmList[i].RPT_DATE_TIME == null) continue;
					if (AlarmList[i].CLEAR_DATE_TIME == null) continue;
					if (AlarmList[i].RPT_DATE_TIME.Trim() == "") continue;
					if (AlarmList[i].CLEAR_DATE_TIME.Trim() == "") continue;
					if (AlarmList[i].EQPT_ID.Trim() == "") continue;

					//Get EachItem Data
					oRPTTime = Convert.ToDateTime(AlarmList[i].RPT_DATE_TIME);
					oCLRTime = Convert.ToDateTime(AlarmList[i].CLEAR_DATE_TIME);
					if (oCLRTime.Subtract(oRPTTime).TotalMinutes > 1440) continue;
					if (oRPTTime.CompareTo(StartTime) < 0) continue; //如果Start切割點是Alarm狀態，自動略過，會產生資料內縮的作用
					if (oCLRTime.CompareTo(EndTime) > 0) continue;//如果End切割點是Alarm狀態，自動略過，會產生資料內縮的作用
																				 //區間累計初始化，因為第0筆資料有可能是null資料，所以這段初始化擺在for迴圈內
					if (!RPTTimeByVHID.ContainsKey(AlarmList[i].EQPT_ID))
					{
						RPTTimeByVHID.Add(AlarmList[i].EQPT_ID, oRPTTime);
					}
					if (!CLRTimeByVHID.ContainsKey(AlarmList[i].EQPT_ID))
					{
						CLRTimeByVHID.Add(AlarmList[i].EQPT_ID, oCLRTime);
					}

					if (DateTime.Compare(oRPTTime, RPTTimeByVHID[AlarmList[i].EQPT_ID].AddSeconds(AlarmGroupTime)) > 0)
					{
						//超出5秒Alarm區間，新增一次表內容
						//Add FirstReportData
						oFirstReport = new VMCBF_FirstReport();
						oFirstReport.VH_ID = AlarmList[i].EQPT_ID;
						oFirstReport.First_RPT_DataTime = RPTTimeByVHID[AlarmList[i].EQPT_ID];
						oFirstReport.Final_Clear_DateTime = CLRTimeByVHID[AlarmList[i].EQPT_ID];
						AddFirstReport(ref dicFirstReport, oFirstReport);
						// New Data
						RPTTimeByVHID[AlarmList[i].EQPT_ID] = oRPTTime;
						CLRTimeByVHID[AlarmList[i].EQPT_ID] = oCLRTime;
					}
					else
					{
						//Clear_Date_Time以較晚的為基準，展開Alarm時間
						if (DateTime.Compare(oCLRTime, CLRTimeByVHID[AlarmList[i].EQPT_ID]) > 0)
						{
							CLRTimeByVHID[AlarmList[i].EQPT_ID] = oCLRTime;
						}

					}

				}

				foreach (var item in RPTTimeByVHID)
				{
					oFirstReport = new VMCBF_FirstReport();
					oFirstReport.VH_ID = item.Key;
					oFirstReport.First_RPT_DataTime = item.Value;
					oFirstReport.Final_Clear_DateTime = CLRTimeByVHID[item.Key];
					AddFirstReport(ref dicFirstReport, oFirstReport);
				}
				#endregion




				return AddFinalReport(ref dicFirstReport, ref VCMD_List);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return new Dictionary<string, VMCBF>();
			}

		}
		private void AddFirstReport(ref Dictionary<string, List<VMCBF_FirstReport>> dicFirstReport, VMCBF_FirstReport oFirstReport)
		{

			if (dicFirstReport.ContainsKey(oFirstReport.VH_ID))
			{
				dicFirstReport[oFirstReport.VH_ID].Add(oFirstReport);
			}
			else
			{
				List<VMCBF_FirstReport> lsFirstReport_MTBF = new List<VMCBF_FirstReport>();
				lsFirstReport_MTBF.Add(oFirstReport);
				dicFirstReport.Add(oFirstReport.VH_ID, lsFirstReport_MTBF);
			}
		}
		private Dictionary<string,VMCBF> AddFinalReport(ref Dictionary<string, List<VMCBF_FirstReport>> dicFirstReport, ref List<VCMD_OHTC> VCMD_List)
		{
			Dictionary<string,VMCBF> VMCBFs = new Dictionary<string,VMCBF>();
			try
            {
				int i = 0;
				
				List<string> AddedList = new List<string>();

				//從CMD撈所有VH，然後跟Alarm整理比對，將Count通通+1 以區隔空間，如果沒在Alarm裡面就是沒Alarm，Count以1計算
				foreach (var VH in VCMD_List.GroupBy(info => info.VH_ID))
				{
					VMCBF oVMCBF = new VMCBF(VH.Key.ToString().Trim());
					if (dicFirstReport.Keys.Contains(VH.Key.ToString().Trim()))
					{
						oVMCBF.AlarmCount = dicFirstReport[VH.Key.ToString().Trim()].Count();
					}
					else
					{
						oVMCBF.AlarmCount = 0;
					}

					oVMCBF.CMDCount = VCMD_List.Where(info => info.VH_ID == VH.Key.ToString()).ToList().Count();

					VMCBFs.Add(VH.Key.ToString().Trim(), oVMCBF);
					AddedList.Add(VH.Key.ToString().Trim());

				}

				return VMCBFs;
			}
			catch(Exception ex)
            {
				logger.Error(ex, "Exception");
				return VMCBFs;
			}
			
		}
		#endregion

		#region MTBF
		private Dictionary<string,VMTBF> GetGridData(List<VALARM> AlarmList, DateTime StartTime, DateTime EndTime)
		{
			try
			{
				//if (AlarmList.Count == 0) return new List<VMTBF>();
				DateTime defaultTime = DateTime.Now;

				#region First Report
				FirstReport_MTBF oFirstReport = new FirstReport_MTBF();
				Dictionary<string, List<FirstReport_MTBF>> dicFirstReport = new Dictionary<string, List<FirstReport_MTBF>>();
				#endregion

				#region Final Report

				Dictionary<string, VMTBF> dicAlarm = new Dictionary<string, VMTBF>();
				#endregion

				#region DateTime for EachItem
				DateTime oRPTTime = defaultTime;
				DateTime oCLRTime = defaultTime;
				#endregion

				#region DateTmie: 區間累計
				Dictionary<string, DateTime> RPTTimeByVHID = new Dictionary<string, DateTime>();
				Dictionary<string, DateTime> CLRTimeByVHID = new Dictionary<string, DateTime>();
				#endregion

				//First Report Data
				if (AlarmList.Count > 0) oFirstReport.EQPT_ID = AlarmList[0].EQPT_ID;
				bool fnFlag = false;//Final Add Flag

				#region GetFirstReport
				for (int i = 0; i < AlarmList.Count; i++)
				{
					if (i == AlarmList.Count - 1)
					{
						fnFlag = true;
					}
					if (AlarmList[i].EQPT_ID == null) continue;
					if (AlarmList[i].RPT_DATE_TIME == null) continue;
					if (AlarmList[i].CLEAR_DATE_TIME == null) continue;
					if (AlarmList[i].RPT_DATE_TIME.Trim() == "") continue;
					if (AlarmList[i].CLEAR_DATE_TIME.Trim() == "") continue;
					if (AlarmList[i].EQPT_ID.Trim() == "") continue;

					//Get EachItem Data
					oRPTTime = Convert.ToDateTime(AlarmList[i].RPT_DATE_TIME);
					oCLRTime = Convert.ToDateTime(AlarmList[i].CLEAR_DATE_TIME);
					if (oCLRTime.Subtract(oRPTTime).TotalMinutes > 1440) continue;
					if (oRPTTime.CompareTo(StartTime) < 0) continue; //如果Start切割點是Alarm狀態，自動略過，會產生資料內縮的作用
					if (oCLRTime.CompareTo(EndTime) > 0) continue;//如果End切割點是Alarm狀態，自動略過，會產生資料內縮的作用
																  //區間累計初始化，因為第0筆資料有可能是null資料，所以這段初始化擺在for迴圈內
					if (!RPTTimeByVHID.ContainsKey(AlarmList[i].EQPT_ID))
					{
						RPTTimeByVHID.Add(AlarmList[i].EQPT_ID, oRPTTime);
					}
					if (!CLRTimeByVHID.ContainsKey(AlarmList[i].EQPT_ID))
					{
						CLRTimeByVHID.Add(AlarmList[i].EQPT_ID, oCLRTime);
					}

					if (DateTime.Compare(oRPTTime, RPTTimeByVHID[AlarmList[i].EQPT_ID].AddSeconds(AlarmGroupTime)) > 0)
					{
						//超出5秒Alarm區間，新增一次表內容
						//Add FirstReportData
						oFirstReport = new FirstReport_MTBF();
						oFirstReport.EQPT_ID = AlarmList[i].EQPT_ID;
						oFirstReport.First_RPT_DataTime = RPTTimeByVHID[AlarmList[i].EQPT_ID];
						oFirstReport.Final_Clear_DateTime = CLRTimeByVHID[AlarmList[i].EQPT_ID];
						AddFirstReport(ref dicFirstReport, oFirstReport);
						// New Data
						RPTTimeByVHID[AlarmList[i].EQPT_ID] = oRPTTime;
						CLRTimeByVHID[AlarmList[i].EQPT_ID] = oCLRTime;
					}
					else
					{
						//Clear_Date_Time以較晚的為基準，展開Alarm時間
						if (DateTime.Compare(oCLRTime, CLRTimeByVHID[AlarmList[i].EQPT_ID]) > 0)
						{
							CLRTimeByVHID[AlarmList[i].EQPT_ID] = oCLRTime;
						}

					}

				}

				foreach (var item in RPTTimeByVHID)
				{
					oFirstReport = new FirstReport_MTBF();
					oFirstReport.EQPT_ID = item.Key;
					oFirstReport.First_RPT_DataTime = item.Value;
					oFirstReport.Final_Clear_DateTime = CLRTimeByVHID[item.Key];
					AddFirstReport(ref dicFirstReport, oFirstReport);
				}
				#endregion


				AddFinalReport(ref dicAlarm, ref dicFirstReport, StartTime, EndTime);




				return dicAlarm;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return new Dictionary<string, VMTBF>();
			}

		}
		private void AddFirstReport(ref Dictionary<string, List<FirstReport_MTBF>> dicFirstReport, FirstReport_MTBF oFirstReport)
		{

			if (dicFirstReport.ContainsKey(oFirstReport.EQPT_ID))
			{
				dicFirstReport[oFirstReport.EQPT_ID].Add(oFirstReport);
			}
			else
			{
				List<FirstReport_MTBF> lsFirstReport_MTBF = new List<FirstReport_MTBF>();
				lsFirstReport_MTBF.Add(oFirstReport);
				dicFirstReport.Add(oFirstReport.EQPT_ID, lsFirstReport_MTBF);
			}
		}
		private void AddFinalReport(ref Dictionary<string, VMTBF> dicAlarm, ref Dictionary<string, List<FirstReport_MTBF>> dicFirstReport, DateTime StartTime, DateTime EndTime)
		{
			//沒TimeInterval的版本，使用者選取區間，有Alarm就算Alarm時間
			//AlarmCount是Alarm時間次數
			try
			{
				var lsVH = VCMD_List.Select(info => info.VH_ID.Trim()).Distinct();
				int i = 0;
				//從CMD撈所有VH，然後跟Alarm整理比對，將Count通通+1 以區隔空間，如果沒在Alarm裡面就是沒Alarm，Count以1計算
				foreach (string EQPT_ID in dicFirstReport.Keys)
				{
					if (!lsVH.Contains(EQPT_ID)) continue;
					VMTBF oVMTBF = new VMTBF();
					TimeSpan subTime = new TimeSpan();
					TimeSpan AlarmTime = new TimeSpan();
					oVMTBF.EQPT_ID = EQPT_ID;
					//if (dicFirstReport[EQPT_ID].Count < 2) continue;
					for (i = 0; i <= dicFirstReport[EQPT_ID].Count - 1; i++)
					{
						if (dicFirstReport[EQPT_ID][i].Final_Clear_DateTime.CompareTo(dicFirstReport[EQPT_ID][i].First_RPT_DataTime) > 0)
						{
							AlarmTime += dicFirstReport[EQPT_ID][i].Final_Clear_DateTime.Subtract(dicFirstReport[EQPT_ID][i].First_RPT_DataTime); //計算所有Alarm時間
						}
					}
					oVMTBF.AlarmCount = dicFirstReport[EQPT_ID].Count;
					subTime = (EndTime.Subtract(StartTime)) - AlarmTime; //總正常時間  = 使用者選取區間 - Alarm總時間
																		 //oVMTBF.AddTimeInterval(dicFirstReport[EQPT_ID][0].Final_Clear_DateTime, dicFirstReport[EQPT_ID][dicFirstReport[EQPT_ID].Count - 1].First_RPT_DataTime);
					oVMTBF.AddActiveTime(Math.Round(subTime.TotalHours, 2));

					

					dicAlarm.Add(EQPT_ID, oVMTBF);
				}
				foreach (var VH in lsVH)
				{
					if (dicAlarm.ContainsKey(VH.ToString())) continue;
					VMTBF oVMTBF = new VMTBF();
					oVMTBF.EQPT_ID = VH.ToString();
					oVMTBF.AlarmCount = 0;
					oVMTBF.AddActiveTime(Math.Round((EndTime.Subtract(StartTime)).TotalHours, 2));
					dicAlarm.Add(VH.ToString(), oVMTBF);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}


		}
		#endregion

		private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			StringReader stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Right\" /></Style>");
			XmlReader xmlReader = XmlReader.Create(stringReader);
			Style style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
			DataGridTextColumn col = e.Column as DataGridTextColumn;
			if (col != null && e.PropertyType == typeof(decimal) || e.PropertyType == typeof(Int32) || e.PropertyType == typeof(double))
			{
				col.Binding = new Binding(e.PropertyName) { StringFormat = "{0:N0}" };
				col.CellStyle = style;
			}
		}

		private void GridRefresh()
		{
			try
			{
				foreach (DataGridColumn dc in dgv_log_query.Columns)
				{
					StringReader stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Right\" /></Style>");
					XmlReader xmlReader = XmlReader.Create(stringReader);
					Style style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
					switch (dc.Header.ToString())
					{
						case nameof(VMTBF.MTBF):
						case nameof(VMTBF.ActiveTime):
						case nameof(VMTBF.AlarmCount):
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

		//private void btn_ShowChart_Click(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		var dtEx = dvExport.ToTable();
		//		if ( SelectEndTime == DateTime.MinValue) return;
		//		if (dtEx.Rows.Count == 0) return;

		//		Dictionary<double,string> XAxisDataStrings = new Dictionary<double, string>();
		//		List<double> XAxisDatas = new List<double>();
		//		Dictionary<string, List<double>> dicScatter = new Dictionary<string, List<double>>();
		//		Dictionary<string, List<double>> dicScatterY2 = new Dictionary<string, List<double>>();
		//		double i = 1;
		//		foreach (DataColumn dc in dtEx.Columns)
		//              {
		//			if(dc.Caption.Contains("to"))
		//                  {
		//				XAxisDataStrings.Add(i,dc.Caption);
		//				XAxisDatas.Add(i++);
		//			}

		//              }


		//		foreach(DataRow dr in dtEx.Rows)
		//              {
		//			string VHKey = "";
		//			List<object> drValue = new List<object>();
		//			VHKey = dr["MeanTime"].ToString();
		//			drValue = dr.ItemArray.ToList();
		//			drValue.RemoveAt(0);
		//			if(!VHKey.Contains("MTBF"))
		//                  {
		//				dicScatter.Add(VHKey, drValue.Select(s => (double)s).ToList());
		//			}
		//			else
		//                  {
		//				dicScatterY2.Add(VHKey, drValue.Select(s => (double)s).ToList());
		//			}

		//		}


		//		Scatter oScatterChart = new Scatter(_XAxisData: XAxisDatas, _dicScatter: dicScatter, _XAxisDataString: XAxisDataStrings);
		//		oScatterChart.YAxisTitle = "AlarmCount";
		//		oScatterChart.ShowValue = true;
		//		Scatter oScatterChartY2 = new Scatter(_XAxisData: XAxisDatas, _dicScatter: dicScatterY2, _XAxisDataString: XAxisDataStrings, _Unit: "h");
		//		oScatterChartY2.YAxisTitle = "AlarmTime";
		//		oScatterChartY2.ShowValue = true;


		//		new Chart_BasePopupWindow(WindownApplication.getInstance().oChartConverter.GetScatterChart(oScatterChart, oScatterChartY2, Title: this.Name ),_filename: this.Name  + "_" + SelectEndTime.ToString("yyyyMMddHH")).ShowDialog();
		//	}
		//	catch (Exception ex)
		//	{
		//		logger.Error(ex, "Exception");
		//	}

		//}

	}
}

public class VMeanTimeByVH
{
	public VMeanTimeByVH(string _vhid,double _mtbf,double _mcbfAlarm,double _mcbfCMD,double _cmdcount)
    {
		vhid = _vhid;
		mtbf = _mtbf;
		mcbf_alarm = _mcbfAlarm;
		mcbf_cmd = _mcbfCMD;
		cmdcount = _cmdcount;

	}
	public string VHID { get { return vhid; } }
	private string vhid { get; set; }

	public double CMDCount { get { return cmdcount; } }
	private double cmdcount { get; set; }

	public double MTBF { get { return mtbf; } }
	private double mtbf { get; set; }

	public double MCBF_Alarn { get { return mcbf_alarm; } }
	private double mcbf_alarm { get; set; }

	public double MCBF_CMD { get { return mcbf_cmd; } }
	private double mcbf_cmd { get; set; }

}
