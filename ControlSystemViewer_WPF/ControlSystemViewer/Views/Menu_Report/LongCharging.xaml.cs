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
using ViewerObject.REPORT;
using DocumentFormat.OpenXml.Drawing;
using CsvHelper;

namespace ControlSystemViewer.Views.Menu_Report
{
    /// <summary>
    /// AlarmHistory.xaml 的互動邏輯
    /// </summary>
    public partial class LongCharging : UserControl
    {
        #region 公用參數設定

        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private SysExcuteQualityQueryService sysExcuteQualityQueryService;
        List<VehicleChargeResult> VehicleChargeList = null;

        private int AutoTime = 0;

        public event EventHandler CloseEvent;

        XSLXFormat oXSLXFormat = new XSLXFormat();//同時把Excel匯出設定與View Ui顯示Header Unit以這個Class實現
        char chPLC = 'N';
        private DateTime SelectStartTime = DateTime.MinValue;
        private DateTime SelectEndTime = DateTime.MinValue;

        private DateTime SearchTimeFrom = DateTime.MinValue;
        private DateTime SearchTimeTo = DateTime.MinValue;

        DateTime LimitStartTime = DateTime.Now;
        DateTime LimitEndTime = DateTime.Now;

        #endregion 公用參數設定

        public LongCharging()
        {
            try
            {
                InitializeComponent();

                //設定日期的最大與最小值
                m_StartDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
                m_StartDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);
                m_EndDTCbx.DisplayDateStart = DateTime.Now.AddYears(-3);
                m_EndDTCbx.DisplayDateEnd = DateTime.Today.AddDays(1).AddMilliseconds(-1);

                //預設起訖日期
                m_StartDTCbx.SelectedDate = DateTime.Now.AddDays(-1);
                m_EndDTCbx.SelectedDate = DateTime.Now.AddMilliseconds(-1);
                dgv_log_query.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;

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

        public void StartupUI(DateTime _LimitStartTime, DateTime _LimitEndTime)
        {
            try
            {
                ReSetLimitTime(_LimitStartTime, _LimitEndTime);

                //build EQID list
                var eqList = new List<string>();
                var portList = WindownApplication.getInstance().ObjCacheManager.GetPortStations();
                var vehicleList = WindownApplication.getInstance().ObjCacheManager.GetVEHICLEs();
                text_VHID.Text = "";

                //sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
                //cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
                for (int i = 0; i <= 23; i++)
                {
                    cbo_StartH.Items.Add(i.ToString("00"));
                    cbo_EndH.Items.Add(i.ToString("00"));
                }

                //this.frmquery = _frmquery;
                dgv_log_query.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
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
            //oXSLXFormat.HiddenColumns.Add(nameof(VALARM.IS_CLEARED));

            #endregion

            #region Columns ChangeHeader

            #endregion

            #region Columns Unit
            oXSLXFormat.ColumnUnit.Add(nameof(VLongCharging.ChargingTime), "min");
            #endregion

            #region Column Color

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
                text_VHID.Text = "";
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
                var scApp = WindownApplication.getInstance();

                dateTimeFrom = new DateTime(dateTimeFrom.Year, dateTimeFrom.Month, dateTimeFrom.Day, dateTimeFrom.Hour, 0, 0, DateTimeKind.Local);
                dateTimeTo = new DateTime(dateTimeTo.Year, dateTimeTo.Month, dateTimeTo.Day, dateTimeTo.Hour, 0, 0, DateTimeKind.Local);
                VehicleChargeList = null;
                SelectStartTime = dateTimeFrom;
                SelectEndTime = dateTimeTo;
                //await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
                //await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
                ((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
                isLoading = true;

                Dictionary<string, long> dicChargingCount = new Dictionary<string, long>();
                Dictionary<string, long> dicChargingTime = new Dictionary<string, long>();
                await Task.Run(() =>
                {
                    VehicleChargeList = GetVehicleChargeResults(dateTimeFrom, dateTimeTo);
                    //               dicChargingCount = await scApp.StatusInfoBLL.GetLongChargeCount(dateTimeFrom, dateTimeTo);
                    //dicChargingTime = await scApp.StatusInfoBLL.GetLongChargeTime(dateTimeFrom, dateTimeTo);
                });


                if (VehicleChargeList == null || VehicleChargeList.Count <= 0)
                {
                    var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
                    string sTipMsg = languageDictionary?["TIPMSG_QUERY_NO_DATA"]?.ToString() ?? "There is no matching data for your query.";
                    TipMessage_Type_Light.Show("", sTipMsg, BCAppConstants.WARN_MSG);
                }
                else
                {
                    dgv_log_query.ItemsSource = VehicleChargeList;
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
        public List<VehicleChargeResult> GetVehicleChargeResults(DateTime start, DateTime end)
        {
            var logs = GetLogsBetweenDates(start, end);
            var vehicleIDs = logs.Select(l => l.VehicleID).OrderBy(s => s).Distinct();

            var results = new List<VehicleChargeResult>();

            foreach (var vehicleID in vehicleIDs)
            {
                var vehicleLogs = logs.Where(l => l.VehicleID == vehicleID).OrderBy(l => l.Time).ToList();

                int chargeCount = 0;
                List<ChargeSession> chargeSessions = new List<ChargeSession>();

                for (int i = 1; i < vehicleLogs.Count; i++)
                {
                    if (vehicleLogs[i - 1].chargeStatus == "NoCharge" && vehicleLogs[i].chargeStatus == "Charging")
                    {
                        chargeCount++;
                        DateTime chargeStart = vehicleLogs[i].Time;

                        int j = i + 1;
                        while (j < vehicleLogs.Count && vehicleLogs[j].chargeStatus == "Charging")
                        {
                            j++;
                        }

                        DateTime chargeEnd = vehicleLogs[j - 1].Time;
                        chargeSessions.Add(new ChargeSession { Start = chargeStart, End = chargeEnd });
                    }
                }

                TimeSpan averageDuration = TimeSpan.FromTicks((long)chargeSessions.Average(cs => cs.Duration.Ticks));

                results.Add(new VehicleChargeResult
                {
                    VehicleID = vehicleID,
                    TotalChargeSessions = chargeCount,
                    AverageChargeDuration = averageDuration,
                    ChargeSessions = chargeSessions
                });
            }

            return results;
        }

        string[] defaultHeaders = new[] { "Time", "VehicleID", "IsConnected", "controlStatus", "vehicleStatus", "commandStatus", "vehicleState", "repairStatus", "errorStatus", "chargeStatus", "IsLongCharging", "IsCSTInstall", "opreationsTime" };
        public List<VehicleStatus> GetLogsBetweenDates(DateTime start, DateTime end)
        {
            var logs = new List<VehicleStatus>();
            var scApp = WindownApplication.getInstance();
            DateTime now_time = DateTime.Now;
            DateTime now_time_hour = new DateTime(now_time.Year, now_time.Month, now_time.Day, now_time.Hour, 0, 0, DateTimeKind.Local);
            string cur_path = scApp.ObjCacheManager.ViewerSettings.vehicleStatusInfoPath.Path;
            string old_path = scApp.ObjCacheManager.ViewerSettings.vehicleStatusInfoPath.ArchivePath;

            for (DateTime date = start; date <= end; date = date.AddHours(1))
            {
                string filePath = date == now_time_hour ? $"{cur_path}\\VehicleStatusInfo.csv" : $"{old_path}\\VehicleStatusInfo_{date.ToString("yyyyMMddHH")}.csv";

                if (File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            bool hasHeader = csv.Parser.Read() && csv.Parser.Record.Contains("VehicleID"); // Adjust this check as necessary
                            if (hasHeader)
                            {
                                csv.Read();
                            }
                            while (csv.Read())
                            {
                                var record = new VehicleStatus
                                {
                                    Time = csv.GetField<DateTime>(0),
                                    VehicleID = csv.GetField<string>(1),
                                    chargeStatus = csv.GetField<string>(9)
                                };
                                logs.Add(record);
                            }
                            //logs.AddRange(csv.GetRecords<VehicleStatus>());
                        }
                    }
                }
            }
            return logs;
        }
        private List<VLongCharging> GetLongCharging(ref Dictionary<string, long> dicChargingCount, ref Dictionary<string, long> dicChargingTime)
        {
            List<VLongCharging> list = new List<VLongCharging>();
            try
            {
                foreach (var VHID in dicChargingCount.Keys)
                {
                    VLongCharging oVLongCharging = new VLongCharging(VHID);
                    oVLongCharging.ChargingTime = Math.Round((double)dicChargingTime[VHID] / 60, 2);
                    oVLongCharging.ChargingCount = dicChargingCount[VHID];
                    list.Add(oVLongCharging);
                }


                return list;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return list;
            }

        }

        private async Task export()
        {
            try
            {
                var data_list = dgv_log_query.ItemsSource.OfType<VLongCharging>();
                if (data_list == null || data_list.Count() == 0)
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
                await Task.Run(() => xlsx = helper.Export(data_list.ToList(), oXSLXFormat));
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
                    await search();
                }
                else if (sender.Equals(HypLPrevious3Hour))// 前3小時 - Now
                {
                    m_StartDTCbx.SelectedDate = DateTime.Now.AddHours(-3);
                    cbo_StartH.Text = DateTime.Now.AddHours(-3).Hour.ToString("00");
                    await search();
                }
                else if (sender.Equals(HypLPreviousHour))// 前一個整點  - 目前整點 ex:  Now => 2022/03/14 8:32，則起始為2022/03/14 7:00 結束為 2022/03/14 8:00
                {
                    st = DateTime.Now.AddHours(-1);
                    m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: st.Hour, minute: 0, second: 0);
                    m_EndDTCbx.SelectedDate = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: DateTime.Now.Day, hour: DateTime.Now.Hour, minute: 0, second: 0);
                    cbo_StartH.Text = st.Hour.ToString("00");
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

                        case nameof(VLongCharging.ChargingCount):
                        case nameof(VLongCharging.ChargingTime):
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
                List<VehicleChargeResult> list2 = new List<VehicleChargeResult>();
                List<VehicleChargeResult> list = new List<VehicleChargeResult>();
                //Filter by EQPT_ID and Alarm_Code


                list = VehicleChargeList?.Where(x =>
                ((x.VehicleID.Contains(text_VHID.Text) && text_VHID.Text != "") || (text_VHID.Text == ""))
                ).OrderBy(info => info.VehicleID).ToList();
                list2 = list;
                dgv_log_query.ItemsSource = list2;

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
                System.Windows.Point relativePoint = senderItem.TransformToAncestor(MainGrid)
                                       .Transform(new System.Windows.Point(0, 0));

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
    public class VehicleStatus
    {
        public DateTime Time { get; set; }
        public string VehicleID { get; set; }
        public string chargeStatus { get; set; }
    }

    public class ChargeSession
    {
        public DateTime Start { get; set; }
        public string S_Start { get { return Start.ToString("yyyy/MM/dd HH:mm:ss"); } }
        public DateTime End { get; set; }
        public string S_End { get { return End.ToString("yyyy/MM/dd HH:mm:ss"); } }
        public TimeSpan Duration => End - Start;
    }

    public class VehicleChargeResult
    {
        public string VehicleID { get; set; }
        public int TotalChargeSessions { get; set; }
        public TimeSpan AverageChargeDuration { get; set; }
        public List<ChargeSession> ChargeSessions { get; set; }
    }

}

