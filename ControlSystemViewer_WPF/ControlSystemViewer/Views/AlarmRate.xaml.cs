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
using Xceed.Wpf.Toolkit;
using static ViewerObject.VALARM_Def;

namespace ControlSystemViewer.Views.Menu_Report
{
    /// <summary>
    /// AlarmHistory.xaml 的互動邏輯
    /// </summary>
    public partial class AlarmRate : UserControl
    {
        #region 公用參數設定

        private static Logger logger = LogManager.GetCurrentClassLogger();
		//private SysExcuteQualityQueryService sysExcuteQualityQueryService;
		private List<RATEALARM> RATEALARM_List = new List<RATEALARM>();
		private int AutoTime = 0;
		private RATEALARM TotalData = new RATEALARM();
		public event EventHandler CloseEvent;

        #endregion 公用參數設定

        public AlarmRate()
		{
			try
			{
				InitializeComponent();

				//設定日期的最大與最小值
				m_StartDTCbx.Minimum = DateTime.Now.AddYears(-3);
				m_StartDTCbx.Maximum = DateTime.Today.AddDays(1).AddMilliseconds(-1);
				m_EndDTCbx.Minimum = DateTime.Now.AddYears(-3);
				m_EndDTCbx.Maximum = DateTime.Today.AddDays(1).AddMilliseconds(-1);

				//預設起訖日期
				m_StartDTCbx.Value = DateTime.Now.AddDays(-1);
				m_EndDTCbx.Value = DateTime.Now.AddMilliseconds(-1);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		public void StartupUI()
		{
			try
			{
				//預設起訖日期
				m_StartDTCbx.Value = DateTime.Now.AddDays(-1);
				m_EndDTCbx.Value = DateTime.Now.AddMilliseconds(-1);

				//build EQID list
				var eqList = new List<string>();
				var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
				var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();
				text_EQID.Text="";


				//sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
				//cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
				m_StartDTCbx.ValueChanged += new RoutedPropertyChangedEventHandler<object>(dtp_ValueChanged);
				m_EndDTCbx.ValueChanged += new RoutedPropertyChangedEventHandler<object>(dtp_ValueChanged);
				//this.frmquery = _frmquery;
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

		private void dtp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			try
			{
				//若當前時間已超過最大值，重新設定最大值
				if (DateTime.Now > m_EndDTCbx.Maximum)
				{
					m_StartDTCbx.Maximum = DateTime.Today.AddDays(1).AddMilliseconds(-1);
					m_EndDTCbx.Maximum = DateTime.Today.AddDays(1).AddMilliseconds(-1);
				}
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
				if(DateTime.Compare(m_StartDTCbx.Value.Value, m_EndDTCbx.Value.Value) > 0)
                {
					TipMessage_Type_Light.Show("Failure", string.Format("EndTime must be later than startime."), BCAppConstants.WARN_MSG);
					return;
				}
				AutoTime =(int)m_EndDTCbx.Value.Value.Subtract(m_StartDTCbx.Value.Value).TotalMinutes +1;

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
				text_EQID.Text ="";
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
				if (string.IsNullOrWhiteSpace(m_StartDTCbx.Value.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select start time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (string.IsNullOrWhiteSpace(m_EndDTCbx.Value.ToString()))
				{
					TipMessage_Type_Light.Show("Failure", string.Format("Please select end time."), BCAppConstants.WARN_MSG);
					return;
				}
				if (m_StartDTCbx.Value > m_EndDTCbx.Value)
				{
					DateTime dtTmp = (DateTime)m_StartDTCbx.Value;
					m_StartDTCbx.Value = m_EndDTCbx.Value;
					m_EndDTCbx.Value = dtTmp;
				}

				btn_Search.IsEnabled = false;
				DateTime dateTimeFrom = System.Convert.ToDateTime(m_StartDTCbx.Value);
				DateTime dateTimeTo = System.Convert.ToDateTime(m_EndDTCbx.Value);
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

				dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, dateTimeFrom.Minute, dateTimeFrom.Second, DateTimeKind.Local);
				dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, dateTimeTo.Minute, dateTimeTo.Second, DateTimeKind.Local);
				List<VALARM> Alarm_List = null;
				
				//await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
				//await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
				((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
				isLoading = true;
				await Task.Run(() =>
				{
					Alarm_List = scApp.AlarmBLL.GetAlarmsByConditions(dateTimeFrom, dateTimeTo, eqpt_id)?.OrderBy(info => info.RPT_DATE_TIME).ToList();
				});

				RATEALARM_List = GetGridData(Alarm_List);
				if (Alarm_List == null || Alarm_List.Count <= 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
				}
				else
				{
						dgv_log_query.ItemsSource = RATEALARM_List?.OrderBy(info => info.ErrorRate).ToList();

					
					foreach(DataGridColumn dc in dgv_log_query.Columns)
                    {
						StringReader stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Right\" /></Style>");
						XmlReader xmlReader = XmlReader.Create(stringReader);
						Style style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
						switch (dc.Header.ToString())
                        {
							case "ErrorRate":
								dc.Header = dc.Header.ToString() + "(%)";				
								dc.CellStyle = style;
								break;
							case "AlarmCount":
								dc.CellStyle = style;
								break;
							case "ErrorTime":
								dc.Header = dc.Header.ToString() + "(m)";
								dc.CellStyle = style;
								break;
							case "dErrorTime":							
								dc.Visibility = Visibility.Hidden;
								break;
							case "dErrorRate":
								dc.Visibility = Visibility.Hidden;
								break;

								//case "EQPT_ID":
								//	 stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Center\" /></Style>");
								//	 xmlReader = XmlReader.Create(stringReader);
								//	 style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
								//	 dc.CellStyle = style;
								//	break;
						}
                    }
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
				var data_list = dgv_log_query.ItemsSource as List<RATEALARM>;
				if (data_list == null || data_list.Count == 0)
				{
					var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
					string sTipMsg = languageDictionary?["TIPMSG_EXPORT_NO_DATA"]?.ToString() ?? "There is no data to export, please search first.";
					TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
					return;
				}

				System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
				dlg.Filter = "Files (*.xlsx)|*.xlsx";
				if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dlg.FileName))
				{
					return;
				}
				string filename = dlg.FileName;
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper();
				//取得轉為 xlsx 的物件
				ClosedXML.Excel.XLWorkbook xlsx = null;
				await Task.Run(() => xlsx = helper.Export(data_list));
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
				m_EndDTCbx.Value = DateTime.Now;

				if (sender.Equals(HypL30mins))
				{
					m_StartDTCbx.Value = DateTime.Now.AddMinutes(-30);
					await search();
				}
				else if (sender.Equals(HypL1hours))
				{
					m_StartDTCbx.Value = DateTime.Now.AddHours(-1);
					await search();
				}
				else if (sender.Equals(HypL4hours))
				{
					m_StartDTCbx.Value = DateTime.Now.AddHours(-4);
					await search();
				}
				else if (sender.Equals(HypL12hours))
				{
					m_StartDTCbx.Value = DateTime.Now.AddHours(-12);
					await search();
				}
				else if (sender.Equals(HypL24hours))
				{
					m_StartDTCbx.Value = DateTime.Now.AddDays(-1);
					await search();
				}
				else if (sender.Equals(HypL2days))
				{
					m_StartDTCbx.Value = DateTime.Now.AddDays(-2);
					await search();
				}
				else if (sender.Equals(HypL3days))
				{
					m_StartDTCbx.Value = DateTime.Now.AddDays(-3);
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

		private List<RATEALARM> GetGridData(List<VALARM> AlarmList)
        {

			DateTime defaultTime = DateTime.Now;
			List<FirstReport> lsFirstReport = new List<FirstReport>();
			FirstReport oFirstReport = null;
			RATEALARM oRATEALARM = null;
			Dictionary<string,RATEALARM> dicAlarm = new Dictionary<string,RATEALARM>();
			DateTime oRPTTime = defaultTime;
			DateTime oCLRTime = defaultTime;
			DateTime RPTTime =  defaultTime;
			DateTime CLRTime = defaultTime;

			TotalData = new RATEALARM();
			TotalData.ALARM_CODE = "Total";
			TotalData.ALARM_DESC = "*****";
			TotalData.EQPT_ID = "*****";
			TotalData.dErrorRate = -1;
			TotalData.AlarmCount = 0;
			TotalData.dErrorTime = 0;
			//First Report Data
			for (int i=0;i<AlarmList.Count;i++)
            {
				if (AlarmList[i].RPT_DATE_TIME.Trim() == "") continue;
				if (AlarmList[i].CLEAR_DATE_TIME.Trim() == "") continue;
				if (RPTTime ==defaultTime) RPTTime = Convert.ToDateTime(AlarmList[i].RPT_DATE_TIME);
				if (CLRTime == defaultTime) CLRTime = Convert.ToDateTime(AlarmList[i].CLEAR_DATE_TIME);
				if(oFirstReport == null)
                {
					oFirstReport = new FirstReport();
					oFirstReport.ALARM_CODE = AlarmList[i].ALARM_CODE;
					oFirstReport.ALARM_DESC = AlarmList[i].ALARM_DESC;
					oFirstReport.EQPT_ID = AlarmList[i].EQPT_ID;
				}

				oRPTTime = Convert.ToDateTime(AlarmList[i].RPT_DATE_TIME);				 
				oCLRTime = Convert.ToDateTime(AlarmList[i].CLEAR_DATE_TIME);


				if (DateTime.Compare(oRPTTime, RPTTime.AddSeconds(5)) > 0)
				{
					//Add FirstReportData
					oFirstReport.First_RPT_DataTime = RPTTime;
					oFirstReport.Final_Clear_DateTime = CLRTime;
					//if (CLRTime.Date != RPTTime.Date || CLRTime.Year != RPTTime.Year || CLRTime.Month != RPTTime.Month)
					//{
					//	DateTime odateTime = new DateTime(CLRTime.Year, CLRTime.Month, CLRTime.Day, 0, 0, 0);
					//	oFirstReport.ErrorTime = CLRTime.Subtract(odateTime);
					//}
					//else
					//{
					//	oFirstReport.ErrorTime = CLRTime.Subtract(RPTTime);
					//}

					//  Start -> End
					//  |  |                |      |
					//	   ↑				↑
					//		取這兩段的值為區間
					//如果選取的Start Time比資料晚，StartTime以選取為主
					if (DateTime.Compare(m_StartDTCbx.Value.Value, RPTTime) > 0)
					{
						oFirstReport.First_RPT_DataTime = m_StartDTCbx.Value.Value;
					}
					//如果資料比選取的EndTime晚，EndTime以選取為主
					if (DateTime.Compare(CLRTime, m_EndDTCbx.Value.Value) > 0)
					{
						oFirstReport.Final_Clear_DateTime = m_EndDTCbx.Value.Value;
					}

					oFirstReport.ErrorTime = oFirstReport.Final_Clear_DateTime.Subtract(oFirstReport.First_RPT_DataTime);
					lsFirstReport.Add(oFirstReport);

					//Create New Data
					oFirstReport = new FirstReport();
					oFirstReport.ALARM_CODE = AlarmList[i].ALARM_CODE;
					oFirstReport.ALARM_DESC = AlarmList[i].ALARM_DESC;
					oFirstReport.EQPT_ID = AlarmList[i].EQPT_ID;
					RPTTime = oRPTTime;
					CLRTime = oCLRTime;
				}
				else
				{
					if(i == AlarmList.Count - 1)
                    {
						//Add FirstReportData
						oFirstReport.First_RPT_DataTime = RPTTime;
						oFirstReport.Final_Clear_DateTime = CLRTime;
						if (DateTime.Compare(m_StartDTCbx.Value.Value, RPTTime) > 0)
						{
							oFirstReport.First_RPT_DataTime = m_StartDTCbx.Value.Value;
						}

						if (DateTime.Compare(CLRTime,m_EndDTCbx.Value.Value) > 0)
						{
							oFirstReport.Final_Clear_DateTime = m_EndDTCbx.Value.Value;
						}

						oFirstReport.ErrorTime = oFirstReport.Final_Clear_DateTime.Subtract(oFirstReport.First_RPT_DataTime);
						lsFirstReport.Add(oFirstReport);
					}
					if (DateTime.Compare(oCLRTime, CLRTime) > 0)
					{
						CLRTime = oCLRTime;
					}
				}
							
            }

			foreach(FirstReport ofirst in lsFirstReport)
            {
				if(dicAlarm.ContainsKey(ofirst.ALARM_CODE + "," + ofirst.EQPT_ID))
                {
					dicAlarm[ofirst.ALARM_CODE + "," + ofirst.EQPT_ID].dErrorTime += ofirst.ErrorTime.TotalMinutes;
					dicAlarm[ofirst.ALARM_CODE + "," + ofirst.EQPT_ID].AlarmCount += 1;
					dicAlarm[ofirst.ALARM_CODE + "," + ofirst.EQPT_ID].dErrorRate = Math.Round((dicAlarm[ofirst.ALARM_CODE + "," + ofirst.EQPT_ID].dErrorTime) / AutoTime * 100,2);
					TotalData.dErrorTime += ofirst.ErrorTime.TotalMinutes;
					TotalData.AlarmCount += 1;
				}
                else
                {
					// Add Dictionary
					oRATEALARM = new RATEALARM();
					oRATEALARM.ALARM_CODE = ofirst.ALARM_CODE;
					oRATEALARM.ALARM_DESC = ofirst.ALARM_DESC;
					oRATEALARM.EQPT_ID = ofirst.EQPT_ID;
					oRATEALARM.AlarmCount = 1;
					oRATEALARM.dErrorTime = ofirst.ErrorTime.TotalMinutes;
					oRATEALARM.dErrorRate = Math.Round((oRATEALARM.dErrorTime) / AutoTime * 100,2);
					TotalData.dErrorTime += oRATEALARM.dErrorTime;
					TotalData.AlarmCount += 1;

					dicAlarm.Add(ofirst.ALARM_CODE + "," + ofirst.EQPT_ID, oRATEALARM);
				}
            }
			dicAlarm.Add("TotalData",TotalData);
			return dicAlarm.Values.ToList();

		}

        private void text_EQID_KeyDown(object sender, KeyEventArgs e)
        {
			if(e.Key ==Key.Enter)
            {
				if(text_EQID.Text=="")
                {
					dgv_log_query.ItemsSource = RATEALARM_List?.OrderBy(info => info.ErrorRate).ToList();
				}
				else
                {
					dgv_log_query.ItemsSource = RATEALARM_List?.Where(x => x.EQPT_ID.Contains(text_EQID.Text)).OrderBy(info => info.ErrorRate).ToList();
				}
				
			}
        }

        //      private void cb_AlarmCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //      {
        //	string text = (sender as ComboBox).SelectedItem as string;
        //	if (text.Trim() != "")
        //	{
        //		dgv_log_query.ItemsSource = RATEALARM_List?.OrderBy(info => info.ErrorRate).Where(info => (info.ALARM_CODE == text.Trim())).ToList();
        //	}
        //	else
        //	{
        //		dgv_log_query.ItemsSource = RATEALARM_List?.OrderBy(info => info.ErrorRate).ToList();
        //	}
        //}
    }
}

public class RATEALARM 
{

	public string ALARM_CODE { get; set; }
	public string ALARM_DESC { get; set; }
	public string EQPT_ID { get; set; }
	public double dErrorRate { get; set; }
	public string ErrorRate 
	{
        get
        {
			if (dErrorRate >= 0)
            {
				return dErrorRate.ToString();
			}
			else
            {
				return "*****";
            }			
		}
	}
	public int AlarmCount { get; set; }
	public double dErrorTime { get; set; }
	public string ErrorTime
    {
        get
        {
			return dErrorTime.ToString("N0");
		}
    }

}

public class FirstReport
{

	public string ALARM_CODE { get; set; }
	public string ALARM_DESC { get; set; }
	public string EQPT_ID { get; set; }
	public TimeSpan ErrorTime { get; set; }
	public DateTime First_RPT_DataTime { get; set; }
	public DateTime Final_Clear_DateTime { get; set; }


}