﻿using ChartConverter.ChartDataClass;
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
using ViewerObject.REPORT;
using static ViewerObject.VALARM_Def;

namespace ControlSystemViewer.Views.Menu_Report
{
	/// <summary>
	/// AlarmHistory.xaml 的互動邏輯
	/// </summary>
	public partial class IntervalMCBFByCMD : UserControl
	{
		#region 公用參數設定

		private static Logger logger = LogManager.GetCurrentClassLogger();
		//private SysExcuteQualityQueryService sysExcuteQualityQueryService;
		private List<VMCBF> VMCBF_List = new List<VMCBF>();

		private int AutoTime = 0;
		public event EventHandler CloseEvent;

		char chPLC = 'N';
		ModeTimeInterval modeTimeInterval = ModeTimeInterval.Off;

		DateTime LimitStartTime = DateTime.Now;
		DateTime LimitEndTime = DateTime.Now;

		int iIntervalDay = 0;
		int iBlankDay = 0;
		int iIntervalNum = 0;
		DataTable dtView = new DataTable();
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


		public IntervalMCBFByCMD()
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

		public  void StartupUI(DateTime _LimitStartTime, DateTime _LimitEndTime)
		{
			try
			{

				ReSetLimitTime(_LimitStartTime, _LimitEndTime);
				//build EQID list
				var eqList = new List<string>();
				var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
				var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();
				text_EQID.Text = "";

				for (int i = 0; i <= 23; i++)
				{
					cbo_EndH.Items.Add(i.ToString("00"));
				}

				//sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
				//cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
				//m_StartDTCbx.ValueChanged += new RoutedPropertyChangedEventHandler<object>(dtp_ValueChanged);
				//m_EndDTCbx.ValueChanged += new RoutedPropertyChangedEventHandler<object>(dtp_ValueChanged);
				rdo_A.Click += RadioButton_Click;
				rdo_T.Click += RadioButton_Click;
				rdo_M.Click += RadioButton_Click;
				rdo_None.Click += RadioButton_Click;
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
			ProjectInfo oProjectInfo = WindownApplication.getInstance().ObjCacheManager.GetSelectedProject();
			string CustomerName = oProjectInfo.Customer.ToString();
			#region Hidden Columns
			oXSLXFormat.HiddenColumns.Add(nameof(VMTBF.TimeInterval));
			oXSLXFormat.HiddenColumns.Add(nameof(VALARM._RPT_DATE_TIME));

			if (CustomerName != "M4")
			{
				oXSLXFormat.HiddenColumns.Add(nameof(VALARM.VH_INSTALL_FLAG));
			}
			#endregion

			#region Columns ChangeHeader

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

		private void RadioButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				switch (((RadioButton)sender).Content.ToString())
				{
					case "None":
						chPLC = 'N';
						break;
					case "A":
						chPLC = 'A';
						break;
					case "T":
						chPLC = 'T';
						break;
					case "M":
						chPLC = 'M';
						break;
					case "On":
						modeTimeInterval = ModeTimeInterval.On;
						break;
					case "Off":
						modeTimeInterval = ModeTimeInterval.Off;
						break;
				}

			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
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

		//private void dtp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		//{
		//	try
		//	{
		//		//若當前時間已超過最大值，重新設定最大值
		//		if (DateTime.Now > m_EndDTCbx.Maximum)
		//		{
		//			m_StartDTCbx.Maximum = DateTime.Today.AddDays(1).AddMilliseconds(-1);
		//			m_EndDTCbx.Maximum = DateTime.Today.AddDays(1).AddMilliseconds(-1);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		logger.Error(ex, "Exception");
		//	}
		//}

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
				text_EQID.Text = "";
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
				if (text_BlankDays.Text.Trim() == "")
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please input BlankDays."), BCAppConstants.WARN_MSG);
					return;
				}
				if (text_IntervalNums.Text.Trim() == "")
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please input IntervalNums."), BCAppConstants.WARN_MSG);
					return;
				}
				if (Convert.ToInt32(text_IntervalDays.Text) < 1)
				{
					TipMessage_Type_Light.Show("Failure", string.Format("IntervalDays must be larger than 1."), BCAppConstants.WARN_MSG);
					return;
				}
				if (Convert.ToInt32(text_BlankDays.Text) < 1)
				{
					TipMessage_Type_Light.Show("Failure", string.Format("BlankDays must be larger than 1."), BCAppConstants.WARN_MSG);
					return;
				}
				if (Convert.ToInt32(text_IntervalNums.Text) < 1)
				{
					TipMessage_Type_Light.Show("Failure", string.Format("IntervalNums must be larger than 1."), BCAppConstants.WARN_MSG);
					return;
				}

				AutoTime = Convert.ToInt32(text_IntervalDays.Text) * 24;

				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_EndH.Text), 0, 0);
				btn_Search.IsEnabled = false;
				SelectEndTime = SearchTimeTo;
				
				string eqpt_id = string.Empty;
				string alarm_id = string.Empty;
				//if (SCUtility.isMatche(txtEQPT_ID.Text, ""))
				//{
				//	eqpt_id = null;
				//}
				//else
				//{
				//	eqpt_id = txtEQPT_ID.Text.Trim();
				//}
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

				iIntervalDay = Convert.ToInt32(text_IntervalDays.Text);
				iBlankDay = Convert.ToInt32(text_BlankDays.Text);
				iIntervalNum = Convert.ToInt32(text_IntervalNums.Text);
				ExportHeader = "EndTime : " + SelectEndTime.ToString("yyyy/MM/dd HH") + ", IntervalDay : " + iIntervalDay.ToString() + ", BlankDay : " + iBlankDay.ToString() + ", IntervalNum : " + iIntervalNum.ToString();


				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
				//await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;
				string sAlarmLevel = "2";
				
				dtView = new DataTable();
				dtValues = new Dictionary<string, Dictionary<string, double>>();
				dtColumnKey = new List<string>();
				dtView.Columns.Add("EQPT_ID", typeof(string));
				DateTime dateTimeFrom = SearchTimeTo;
				DateTime dateTimeTo = SearchTimeTo;
				await Task.Run(() =>
				{
					PrintAlarm_List = scApp.AlarmBLL.GetAlarmsByConditions(dateTimeTo.AddDays(0 - ((iIntervalNum - 1) * iBlankDay) - iIntervalDay), dateTimeTo, cleartimenotnull: true)?.OrderBy(info => info.RPT_DATE_TIME).ToList();
					VMCBF_List = new List<VMCBF>();
				});
				for (int i = iIntervalNum-1; i>=0;i--)
                {
					
						dateTimeFrom = SearchTimeTo.AddDays(0 - (i * iBlankDay) - iIntervalDay);
						dateTimeTo = SearchTimeTo.AddDays(0 - (i * iBlankDay));

						dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, 0, 0, DateTimeKind.Local);
						dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, 0, 0, DateTimeKind.Local);

						VMCBF_List = scApp.CmdBLL.LoadMTTRHCmd(dateTimeFrom, dateTimeTo).OrderBy(info => info.VH_ID).ToList();
						AddDatatableValue(ref VMCBF_List, dateTimeFrom, dateTimeTo);					
				}
				AddRows();


				if (dtView.Rows.Count  <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
					EnumerableRowCollection<DataRow> query = from order in dtView.AsEnumerable()
															 orderby order.Field<string>("EQPT_ID")
															 select order;
					dvExport = query.AsDataView();
					dgv_log_query.ItemsSource = query.AsDataView();
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
		private void AddDatatableValue(ref List<VMCBF> VMTBF_List,DateTime StartTime,DateTime EndTime)
        {
			try
            {
				#region Column Check
				if (!dtView.Columns.Contains(StartTime.ToString("MM-dd") + " to " + EndTime.ToString("MM-dd")))
                {
					dtView.Columns.Add(StartTime.ToString("MM-dd") + " to " + EndTime.ToString("MM-dd"),typeof(double));
					dtColumnKey.Add(StartTime.ToString("MM-dd") + " to " + EndTime.ToString("MM-dd"));
				}
                #endregion

                #region 將資料放進集合
				foreach(var oVMTBF in VMTBF_List)
                {
					string Key = StartTime.ToString("MM-dd") + " to " + EndTime.ToString("MM-dd");
					Dictionary<string, double> dic = new Dictionary<string, double>();
					if (!dtValues.ContainsKey(oVMTBF.VH_ID)) dtValues.Add(oVMTBF.VH_ID, new Dictionary<string, double>());
					if (!dtValues[oVMTBF.VH_ID].ContainsKey(Key)) dtValues[oVMTBF.VH_ID].Add(Key, oVMTBF.MCBF);
				}
                #endregion
            }
            catch (Exception ex)
            {
				logger.Error(ex, "Exception");
			}
        }

		private void AddRows()
		{
			try
			{
				#region 將資料放進集合
				int i = 0;
				foreach (var EQPT in dtValues.Keys)
				{
					DataRow row = dtView.NewRow();

					row["EQPT_ID"] = EQPT;
					foreach (string columnName in dtColumnKey)
                    {
						if (!dtValues.ContainsKey(EQPT))
                        {
							row[columnName] = 0;
							continue;
						}
						if (!dtValues[EQPT].ContainsKey(columnName))
						{
							row[columnName] = 0;
							continue;
						}
						row[columnName] = dtValues[EQPT][columnName];
					}
					dtView.Rows.Add(row);
				}

				#endregion
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}


		private async Task export()
		{
			try
			{
				DataTable dtExport = dvExport.ToTable();
				if (dtExport == null || dtExport.Rows.Count == 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_EXPORT_NO_DATA"]?.ToString() ?? "There is no data to export, please search first.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
					return;
				}

				var scApp = WindownApplication.getInstance();
				ProjectInfo oProjectInfo = scApp.ObjCacheManager.GetSelectedProject();
				string CustomerName = oProjectInfo.Customer.ToString();
				string ProductLine = oProjectInfo.ProductLine.ToString();

				System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
				dlg.Filter = "Files (*.xlsx)|*.xlsx";
				dlg.FileName = "[" + CustomerName + "_" + ProductLine + "] " + this.Name + "_" + SelectEndTime.ToString("yyyyMMddHH") ;
				if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dlg.FileName))
				{
					return;
				}
				string filename = dlg.FileName;
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper(WindownApplication.getInstance().ObjCacheManager.ViewerSettings.report.ExportRowLimit);
				//取得轉為 xlsx 的物件
				ClosedXML.Excel.XLWorkbook xlsx = null;
				await Task.Run(() =>
				{
					oXSLXFormat.WKSheetHeader.Clear();
					oXSLXFormat.WKSheetHeader.Add("Report", ExportHeader);
					xlsx = helper.Export_DataTable(dtExport, oXSLXFormat);
					//PrintAlarm_List = scApp.AlarmBLL.GetAlarmsByConditions(SelectEndTime.AddDays(0 - ((iIntervalNum - 1) * iBlankDay) - iIntervalDay), SelectEndTime)?.OrderBy(info => info.RPT_DATE_TIME).ToList();
					helper.AddSheet(ref xlsx, "AlarmDetail", PrintAlarm_List.ToList(), oXSLXFormat);
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

				//if (sender.Equals(HypLPreviousQuarter))//上季初月1號00:00:00 - 本季初月1號00:00:00
				//{
				//	st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
				//	m_EndDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);

				//	st = st.AddDays(-1);
				//	st = st.AddMonths(0 - (st.Month - 1) % 3).AddDays(1 - st.Day); //回到上季初月1號
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
				//	SearchFlag = true;
				//}
				//else if (sender.Equals(HypLThisQuarter))//本季初月1號00:00:00 - Now
				//{
				//	st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
				//	SearchFlag = true;
				//}
				//else if (sender.Equals(HypLPreviousMonth))//上月1號00:00:00 - 本月1號00:00:00
				//{
				//	st = DateTime.Now.AddMonths(-1);
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: 1, hour: 0, minute: 0, second: 0);

				//	m_EndDTCbx.Value = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: 1, hour: 0, minute: 0, second: 0);
				//	SearchFlag = true;
				//}
				//else if (sender.Equals(HypLThisMonth))//本月1號00:00:00 - Now
				//{
				//	st = DateTime.Now.AddDays(1 - DateTime.Now.Day);
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
				//	SearchFlag = true;
				//}
				//else if (sender.Equals(HypLLast7days))//前7天  - 今天00:00:00
				//{
				//	st = DateTime.Now.AddDays(-7);
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
				//	m_EndDTCbx.Value = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
					
				//}
				//else if (sender.Equals(HypLYesterday))//昨天00:00:00 - 今天00:00:00
				//{
				//	st = DateTime.Now.AddDays(-1);
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
				//	m_EndDTCbx.Value = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: 0, minute: 0, second: 0);
				//	SearchFlag = true;
				//}
				//else if (sender.Equals(HypLToday))// 今天00:00:00 - Now
				//{
				//	st = DateTime.Now;
				//	m_StartDTCbx.Value = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
				//	SearchFlag = true;
				//}

				//if(SearchFlag == true)
    //            {
				//	if (m_StartDTCbx.Value.Value.CompareTo(LimitEndTime) > 0) m_StartDTCbx.Value = LimitEndTime;
				//	if (m_EndDTCbx.Value.Value.CompareTo(LimitEndTime) > 0) m_EndDTCbx.Value = LimitEndTime;

				//	if (m_StartDTCbx.Value.Value.CompareTo(LimitStartTime) < 0) m_StartDTCbx.Value = LimitStartTime;
				//	if (m_EndDTCbx.Value.Value.CompareTo(LimitStartTime) < 0) m_EndDTCbx.Value = LimitStartTime;

				//	await search();
				//}
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

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			VALARM aLARM = new VALARM()
			{
				RPT_DATE_TIME = DateTime.Now.ToString("yyyyMMddhhmmss"),
				//RPT_DATE_TIME = DateTime.Now,
				ALARM_CODE = " 1",
				ALARM_DESC = "1",
				IS_CLEARED = true,
				ALARM_LVL = AlarmLvl.Error,
				EQPT_ID = "1",
				//ERROR_ID = "1",
				//UNIT_NUM = 1
			};
			var app = WindownApplication.getInstance();
			app.AlarmBLL.InsertAlarm(aLARM);
			//sysExcuteQualityQueryService.indexData(aLARM);

			//client.Index(aLARM, index => index.Index($"{Common.ElasticSearchManager.ELASTIC_TABLE_INDEX_AlarmHistory}"));
			//client.IndexDocument(aLARM);
		}


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

		private void btn_Filter_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				EnumerableRowCollection<DataRow> query = from order in dtView.AsEnumerable()
														 orderby order.Field<string>("EQPT_ID")
														 select order;
				DataView dvFilter = query.AsDataView();
				DataTable dtFilter = null;

				if (text_EQID.Text == "")
				{				
				}
				else
				{

					 query = from order in dtView.AsEnumerable()
							 where order.Field<string>("EQPT_ID").Contains(text_EQID.Text)
							 orderby order.Field<string>("EQPT_ID")
							 select order;

					dvFilter = query.AsDataView();
				}

				if (chPLC == 'N')
				{
					dvExport = dvFilter;
					dgv_log_query.ItemsSource = dvFilter;
					return;
				}
				string[] sp;
				dtFilter = dvFilter.ToTable();

				query = from order in dtFilter.AsEnumerable()
						where (order.Field<string>("EQPT_ID").Split('_')[order.Field<string>("EQPT_ID").Split('_').Count() - 1][0] == chPLC && !order.Field<string>("EQPT_ID").Split('_')[order.Field<string>("EQPT_ID").Split('_').Count() - 1].Contains("AGV"))
						select order;

				dvFilter = query.AsDataView();

				dvExport = dvFilter;
				dgv_log_query.ItemsSource = dvFilter;

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

		private void btn_ShowChart_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var dtEx = dvExport.ToTable();
				if (SelectEndTime == DateTime.MinValue) return;
				if (dtEx.Rows.Count == 0) return;

				Dictionary<double, string> XAxisDataStrings = new Dictionary<double, string>();
				List<double> XAxisDatas = new List<double>();
				Dictionary<string, List<double>> dicScatter = new Dictionary<string, List<double>>();
				double i = 1;
				foreach (DataColumn dc in dtEx.Columns)
				{
					if (dc.Caption.Contains("to"))
					{
						XAxisDataStrings.Add(i, dc.Caption);
						XAxisDatas.Add(i++);
					}

				}


				foreach (DataRow dr in dtEx.Rows)
				{
					string VHKey = "";
					List<object> drValue = new List<object>();
					VHKey = dr["EQPT_ID"].ToString().Trim();
					drValue = dr.ItemArray.ToList();
					drValue.RemoveAt(0);
					dicScatter.Add(VHKey, drValue.Select(s => (double)s).ToList());
				}


				Scatter oScatterChart = new Scatter(_XAxisData: XAxisDatas, _dicScatter: dicScatter, _XAxisDataString: XAxisDataStrings);
				oScatterChart.ShowValue = true;

				new Chart_BasePopupWindow(WindownApplication.getInstance().oChartConverter.GetScatterChart(oScatterChart, Title: this.Name),_filename: this.Name + "_" + SelectEndTime.ToString("yyyyMMddHH")).ShowDialog();
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
