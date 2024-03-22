using com.mirle.ibg3k0.bc.winform.Common;
using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using om.mirle.ibg3k0.bc.winform.Common;
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
using UtilsAPI.Common;
using ViewerObject;
using static ViewerObject.VALARM_Def;

namespace ControlSystemViewer.Views.Menu_Report
{
	/// <summary>
	/// AlarmHistory.xaml 的互動邏輯
	/// </summary>
	public partial class RealExecuteTime : UserControl
	{
		#region 公用參數設定

		private static Logger logger = LogManager.GetCurrentClassLogger();
		//private SysExcuteQualityQueryService sysExcuteQualityQueryService;
		private List<ViewerObject.REPORT.VRealExcuteTime> VRealExcuteTimeList = new List<ViewerObject.REPORT.VRealExcuteTime>();
		private int AutoTime = 0;
		public event EventHandler CloseEvent;
		int AlarmGroupTime = 30;

		char chPLC = 'N';

		DateTime LimitStartTime = DateTime.Now;
		DateTime LimitEndTime = DateTime.Now;

		private DateTime SearchTimeFrom = DateTime.MinValue;
		private DateTime SearchTimeTo = DateTime.MinValue;

		private DateTime SelectStartTime = DateTime.MinValue;
		private DateTime SelectEndTime = DateTime.MinValue;

		XSLXFormat oXSLXFormat = new XSLXFormat();//同時把Excel匯出設定與View Ui顯示Header Unit以這個Class實現
		List<ViewerObject.REPORT.VSysExcuteQuality> Print_Detail = new List<ViewerObject.REPORT.VSysExcuteQuality>();

		#endregion 公用參數設定

		public RealExecuteTime()
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

		public  void StartupUI(DateTime _LimitStartTime, DateTime _LimitEndTime)
		{
			try
			{

				ReSetLimitTime(_LimitStartTime, _LimitEndTime);
				//build EQID list
				var eqList = new List<string>();
				var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
				var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();
				AlarmGroupTime = WindownApplication.getInstance().ObjCacheManager.ViewerSettings.report.AlarmGroupMin;
				text_SourceADR.Text = "";
				text_DestADR.Text = "";

				for (int i = 0; i <= 23; i++)
				{
					cbo_StartH.Items.Add(i.ToString("00"));
					cbo_EndH.Items.Add(i.ToString("00"));
				}
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
			//oXSLXFormat.HiddenColumns.Add(nameof(VMTTR.TimeInterval));
			//oXSLXFormat.HiddenColumns.Add(nameof(VALARM._RPT_DATE_TIME));
			#endregion

			#region Columns ChangeHeader
			//oXSLXFormat.ChangeHeaders.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_CMDQUEUE_TIME), "CMDQUEUE_TIME");
			//oXSLXFormat.ChangeHeaders.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_MOVE_TO_SOURCE_TIME), "MOVE_TO_SOURCE_TIME");
			//oXSLXFormat.ChangeHeaders.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_MOVE_TO_DESTN_TIME), "MOVE_TO_DESTN_TIME");
			//oXSLXFormat.ChangeHeaders.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_CMD_TOTAL_EXCUTION_TIME), "CMD_TOTAL_EXCUTION_TIME");
			#endregion

			#region Columns Unit
			oXSLXFormat.ColumnUnit.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_CMDQUEUE_TIME), "Avg/s");
			oXSLXFormat.ColumnUnit.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_MOVE_TO_SOURCE_TIME), "Avg/s");
			oXSLXFormat.ColumnUnit.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_MOVE_TO_DESTN_TIME), "Avg/s");
			oXSLXFormat.ColumnUnit.Add(nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_CMD_TOTAL_EXCUTION_TIME), "Avg/s");
			//oXSLXFormat.ColumnUnit.Add(nameof(VMTTR.AlarmTime), "min");

			#endregion

			#region Column Color

			#endregion
			XSLXStyle_Com oXSLXStyleStringDate = new XSLXStyle_Com();
			oXSLXStyleStringDate.NumberFormat = "yyyy/mm/dd HH:MM:ss";
			oXSLXFormat.ColumnFormat.Add(nameof(ViewerObject.REPORT.VSysExcuteQuality.CMD_Insert_Time), oXSLXStyleStringDate);
			oXSLXFormat.ColumnFormat.Add(nameof(ViewerObject.REPORT.VSysExcuteQuality.CMD_Start_Time), oXSLXStyleStringDate);
			oXSLXFormat.ColumnFormat.Add(nameof(ViewerObject.REPORT.VSysExcuteQuality.CMD_Finish_Time), oXSLXStyleStringDate);
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
				AutoTime = (int)SearchTimeTo.Subtract(SearchTimeFrom).TotalMinutes + 1;

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
				text_SourceADR.Text = "";
				text_DestADR.Text = "";
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

		private async Task search()
		{
			bool isLoading = false;
			try
			{
				SearchTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text), 0, 0);
				SearchTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_EndH.Text), 0, 0);

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

				dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, 0, 0, DateTimeKind.Local);
				dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, 0, 0, DateTimeKind.Local);

				SelectStartTime = dateTimeFrom;
				SelectEndTime = dateTimeTo;
				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
				//await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;

				WindownApplication app = WindownApplication.getInstance();
				await Task.Run(() =>
				{
					VRealExcuteTimeList = scApp.CmdBLL.LoadRealExecuteTime(dateTimeFrom, dateTimeTo)?.OrderBy(info => info.SOURCE_ADR).ToList();
				});


				if (VRealExcuteTimeList == null || VRealExcuteTimeList.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
					dgv_log_query.ItemsSource = VRealExcuteTimeList;
					GridRefresh();
					//告知使用者如果選的資料區間太短會造成資料失真


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

		private async Task export()
		{
			try
			{
				Dictionary<string, List<ViewerObject.REPORT.VRealExcuteTime>> dicDetail = new Dictionary<string, List<ViewerObject.REPORT.VRealExcuteTime>>();
				
				var data_list = dgv_log_query.ItemsSource.OfType<ViewerObject.REPORT.VRealExcuteTime>();
				if (data_list == null || data_list.Count() == 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_EXPORT_NO_DATA"]?.ToString() ?? "There is no data to export, please search first.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
					return;
				}

				foreach (var dataiem in data_list)
				{
					if(! dicDetail.Keys.Contains(dataiem.SOURCE_ADR.Trim()))
                    {
						List<ViewerObject.REPORT.VRealExcuteTime> list = new List<ViewerObject.REPORT.VRealExcuteTime>();
						list.Add(dataiem);
						dicDetail.Add(dataiem.SOURCE_ADR.Trim(),list);

					}
					else
                    {
						dicDetail[dataiem.SOURCE_ADR.Trim()].Add(dataiem);
					}						
					
				}
				var scApp = WindownApplication.getInstance();
				ProjectInfo oProjectInfo = scApp.ObjCacheManager.GetSelectedProject();
				string CustomerName = oProjectInfo.Customer.ToString();
				string ProductLine = oProjectInfo.ProductLine.ToString();

				System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
				dlg.Filter = "Files (*.xlsx)|*.xlsx";
				dlg.FileName = "[" + CustomerName + "_" + ProductLine + "] " + this.Name + "_" + SelectStartTime.ToString("yyyyMMddHH") + "-" + SelectEndTime.ToString("yyyyMMddHH");
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
					oXSLXFormat.WKSheetHeader.Add("Report", "StartTime : " + SelectStartTime.ToString("yyyy/MM/dd HH:mm:ss") + ", " + "EndTime : " + SelectEndTime.ToString("yyyy/MM/dd HH:mm:ss"));
					xlsx = helper.Export_Detail(dicDetail.Keys.ToList(), dicDetail, oXSLXFormat);
                    Print_Detail = scApp.CmdBLL.LoadSysExecutionQuality(SelectStartTime, SelectEndTime)?.OrderBy(info => info.CMD_Start_Time).ToList();
                    helper.AddSheet(ref xlsx, "SysExecutionDetail", Print_Detail.ToList(), oXSLXFormat);
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

				if (sender.Equals(HypLPreviousQuarter))//上季初月1號00:00:00 - 本季初月1號00:00:00
				{
					st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
					m_EndDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					cbo_EndH.Text = "00";
					st = st.AddDays(-1);
					st = st.AddMonths(0 - (st.Month - 1) % 3).AddDays(1 - st.Day); //回到上季初月1號
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLThisQuarter))//本季初月1號00:00:00 - Now
				{
					st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLPreviousMonth))//上月1號00:00:00 - 本月1號00:00:00
				{
					st = DateTime.Now.AddMonths(-1);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: 1, hour: 0, minute: 0, second: 0);

					m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: 1, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					cbo_EndH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLThisMonth))//本月1號00:00:00 - Now
				{
					st = DateTime.Now.AddDays(1 - DateTime.Now.Day);
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
					await search();
				}
				else if (sender.Equals(HypLLast7days))//前7天  - 今天00:00:00
				{
					st = DateTime.Now.AddDays(-7);
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
				else if (sender.Equals(HypLToday))// 今天00:00:00 - Now
				{
					st = DateTime.Now;
					m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
					cbo_StartH.Text = "00";
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
			if (col != null && e.PropertyType == typeof(decimal) || e.PropertyType == typeof(Int32) || e.PropertyType == typeof(double))
			{
				col.Binding = new Binding(e.PropertyName) { StringFormat = "{0:N0}" };
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
						case nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_CMDQUEUE_TIME):
						case nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_MOVE_TO_DESTN_TIME):
						case nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_MOVE_TO_SOURCE_TIME):
						case nameof(ViewerObject.REPORT.VRealExcuteTime.Mean_CMD_TOTAL_EXCUTION_TIME):
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
				List<ViewerObject.REPORT.VRealExcuteTime> list;
				if (text_SourceADR.Text == "" && text_DestADR.Text=="")
				{
					list = VRealExcuteTimeList?.OrderBy(info => info.SOURCE_ADR).ToList();
				}
				else if(text_SourceADR.Text != "" && text_DestADR.Text == "")
				{

					list = VRealExcuteTimeList?.Where(x =>  x.SOURCE_ADR.Contains(text_SourceADR.Text)).ToList();
				}
				else if (text_SourceADR.Text == "" && text_DestADR.Text != "")
				{

					list = VRealExcuteTimeList?.Where(x => x.DESTINATION_ADR.Contains(text_DestADR.Text)).ToList();
				}
				else
                {
					list = VRealExcuteTimeList?.Where(x => x.SOURCE_ADR.Contains(text_SourceADR.Text) && x.DESTINATION_ADR.Contains(text_DestADR.Text)).ToList();
				}
				dgv_log_query.ItemsSource = list;
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

		private void btn_CalTime_Click(object sender, RoutedEventArgs e)
		{
			if (text_AutoTime.Text.Trim() == "") return;
			try
			{
				DateTime? NowCal = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text), 0, 0);
				DateTime? AfterCal = null;
				switch (cbo_MagTime.Text)
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

