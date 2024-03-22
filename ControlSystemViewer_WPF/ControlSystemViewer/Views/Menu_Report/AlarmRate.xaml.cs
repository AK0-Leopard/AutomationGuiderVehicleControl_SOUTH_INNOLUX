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
using System.Globalization;
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
    public partial class AlarmRate : UserControl
    {
        #region 公用參數設定

        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private SysExcuteQualityQueryService sysExcuteQualityQueryService;
        private List<RATEALARM> RATEALARM_List = new List<RATEALARM>();
        private List<RATEALARM> TotalList = new List<RATEALARM>();
        private int AutoTime = 0;
        private RATEALARM TotalData = new RATEALARM();
        public event EventHandler CloseEvent;
        private HashSet<string> EQPTNum = new HashSet<string>();

        private DateTime SelectStartTime = DateTime.MinValue;
        private DateTime SelectEndTime = DateTime.MinValue;

        private DateTime SearchTimeFrom = DateTime.MinValue;
        private DateTime SearchTimeTo = DateTime.MinValue;

        DateTime LimitStartTime = DateTime.Now;
        DateTime LimitEndTime = DateTime.Now;

        int AlarmGroupTime = 30;

        XSLXFormat oXSLXFormat = new XSLXFormat();//同時把Excel匯出設定與View Ui顯示Header Unit以這個Class實現
        char chPLC = 'N';
        List<VALARM> PrintAlarm_List = null;
        #endregion 公用參數設定

        public AlarmRate()
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
            catch(Exception ex)
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
                text_EQID.Text = "";
                for (int i = 0; i <= 23; i++)
                {
                    cbo_StartH.Items.Add(i.ToString("00"));
                    cbo_EndH.Items.Add(i.ToString("00"));
                }

                //sysExcuteQualityQueryService = WindownApplication.getInstance().GetSysExcuteQualityQueryService();
                //cb_HrsInterval.MouseWheel += new MouseWheelEventHandler(cb_HrsInterval_MouseWheel);
                rdo_A.Click += RadioButton_Click;
                rdo_T.Click += RadioButton_Click;
                rdo_M.Click += RadioButton_Click;
                rdo_None.Click += RadioButton_Click;
                //this.frmquery = _frmquery;
                dgv_log_query.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
                dgv_total_query.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
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
            oXSLXFormat.HiddenColumns.Add(nameof(VALARM.IS_CLEARED));
            oXSLXFormat.HiddenColumns.Add(nameof(VALARM._RPT_DATE_TIME));

            if (CustomerName != "M4")
            {
                oXSLXFormat.HiddenColumns.Add(nameof(VALARM.VH_INSTALL_FLAG));
            }

            #endregion

            #region Columns ChangeHeader

            #endregion

            #region Columns Unit
            oXSLXFormat.ColumnUnit.Add(nameof(RATEALARM.ErrorRate), "%");
            oXSLXFormat.ColumnUnit.Add(nameof(RATEALARM.ErrorTime), "min");

            #endregion

            #region Column Color


            XSLXStyle_Com oXSLXStylePercent = new XSLXStyle_Com();
            oXSLXStylePercent.FontAlignment = XSLXStyle_Com.eFontAlignment.Right;
            oXSLXStylePercent.NumberFormat = "0.00%";
            oXSLXFormat.ColumnFormat.Add(nameof(RATEALARM.ErrorRate), oXSLXStylePercent);

            XSLXStyle_Com oXSLXStyleStringDate = new XSLXStyle_Com();
            oXSLXStyleStringDate.NumberFormat = "yyyy/mm/dd HH:MM:ss";
            oXSLXFormat.ColumnFormat.Add(nameof(VALARM.RPT_DATE_TIME), oXSLXStyleStringDate);
            oXSLXFormat.ColumnFormat.Add(nameof(VALARM.CLEAR_DATE_TIME), oXSLXStyleStringDate);

            #endregion

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

        private async void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTimeFrom = new DateTime(m_StartDTCbx.SelectedDate.Value.Year, m_StartDTCbx.SelectedDate.Value.Month, m_StartDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_StartH.Text), 0, 0);
                DateTime dateTimeTo = new DateTime(m_EndDTCbx.SelectedDate.Value.Year, m_EndDTCbx.SelectedDate.Value.Month, m_EndDTCbx.SelectedDate.Value.Day, Convert.ToInt32(cbo_EndH.Text), 0, 0);

                if (DateTime.Compare(dateTimeFrom, dateTimeTo) > 0)
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
                text_EQID.Text = "";
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

                EQPTNum.Clear();
                AutoTime = (int)SearchTimeTo.Subtract(SearchTimeFrom).TotalMinutes + 1;
                if (string.IsNullOrWhiteSpace(SearchTimeFrom.ToString()))
                {
                    TipMessage_Type_Light.Show("Failure", string.Format("Please select start time."), BCAppConstants.WARN_MSG);
                    return;
                }
                if (string.IsNullOrWhiteSpace(m_EndDTCbx.SelectedDate.Value.ToString()))
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
                List<VALARM> Alarm_List = null;
                SelectStartTime = dateTimeFrom;
                SelectEndTime = dateTimeTo;
                //await Task.Run(() => system_qualitys = sysExcuteQualityQueryService.loadALARMHistory(dateTimeFrom, dateTimeTo));
                //await Task.Run(() => Alarm_List = scApp.AlarmBLL.loadAlarmByConditions(dateTimeFrom, dateTimeTo, true, eqpt_id));
                ((MainWindow)App.Current.MainWindow).Loading_Start("Searching");
                isLoading = true;
                string sAlarmLevel = null;
                if (rdo_Error.IsChecked == true) sAlarmLevel = "2";
                WindownApplication app = WindownApplication.getInstance();
                await Task.Run(() =>
                {
                    Alarm_List = scApp.AlarmBLL.GetAlarmsByConditions(dateTimeFrom, dateTimeTo, eqpt_id, Alarm_Level: sAlarmLevel, cleartimenotnull: true)?.Where(info => (app.ObjCacheManager.ViewerSettings.report.CheckAlarmCode(info.ALARM_CODE) && sAlarmLevel == "2") || sAlarmLevel == null).OrderBy(info => info.RPT_DATE_TIME).ToList();
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
                    dgv_log_query.ItemsSource = RATEALARM_List?.OrderByDescending(info => info.ErrorTime).ToList();
                    dgv_total_query.ItemsSource = TotalList?.OrderByDescending(info => info.ErrorTime).ToList();
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

        private async Task export()
        {
            try
            {
                var data_list = dgv_log_query.ItemsSource.OfType<RATEALARM>();
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
                await Task.Run(() =>
                {
                    oXSLXFormat.WKSheetHeader.Clear();
                    oXSLXFormat.WKSheetHeader.Add("Report", "StartTime : " + SelectStartTime.ToString("yyyy/MM/dd hh:mm:ss") + ", " + "EndTime : " + SelectEndTime.ToString("yyyy/MM/dd hh:mm:ss"));
                    xlsx = helper.Export(data_list.ToList(), oXSLXFormat);
                    PrintAlarm_List = scApp.AlarmBLL.GetAlarmsByConditions(SelectStartTime, SelectEndTime, cleartimenotnull: true)?.OrderBy(info => info.RPT_DATE_TIME).ToList();
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
                DateTime ed = new DateTime();
                m_EndDTCbx.SelectedDate = LimitEndTime;
                cbo_EndH.Text = LimitEndTime.Hour.ToString("00");
                var app = WindownApplication.getInstance();
                if (sender.Equals(HypLPreviousQuarter))//上季初月1號00:00:00 - 本季初月1號00:00:00
                {
                    st = DateTime.Now.AddMonths(0 - (DateTime.Now.Month - 1) % 3).AddDays(1 - DateTime.Now.Day);//回到本季初月1號
                    m_EndDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
                    cbo_EndH.Text = "00";
                    st = st.AddDays(-1);
                    st = st.AddMonths(0 - (st.Month - 1) % 3).AddDays(1 - st.Day); //回到上季初月1號
                    m_StartDTCbx.SelectedDate = new DateTime(year: st.Year, month: st.Month, day: st.Day, hour: 0, minute: 0, second: 0);
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
                else if (sender.Equals(HypLYesterdayOfDayShift))//昨天日班 - 今天日班
                {
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

                    m_EndDTCbx.SelectedDate = DateTime.Now;
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

        private List<RATEALARM> GetGridData(List<VALARM> AlarmList)
        {
            try
            {
                ProjectInfo oProjectInfo = WindownApplication.getInstance().ObjCacheManager.GetSelectedProject();
                string CustomerName = oProjectInfo.Customer.ToString();

                if (AlarmList.Count == 0) return new List<RATEALARM>();
                DateTime defaultTime = DateTime.Now;

                #region First Report
                #endregion

                #region Final Report

                Dictionary<string, RATEALARM> dicAlarm = new Dictionary<string, RATEALARM>();
                #endregion

                #region DateTime for EachItem
                DateTime oRPTTime = defaultTime;
                DateTime oCLRTime = defaultTime;
                #endregion

                #region DateTmie: 區間累計
                Dictionary<string, FirstReport> FirstReportByVHID = new Dictionary<string, FirstReport>();
                #endregion

                TotalData = new RATEALARM();
                TotalData.ALARM_CODE = "Total";
                TotalData.ALARM_DESC = "*****";
                TotalData.EQPT_ID = "*****";
                TotalData.AlarmCount = 0;
                TotalData.ErrorTime = 0;

                //First Report Data

                bool fnFlag = false;//Final Add Flag

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
                    if(CustomerName=="PTI")
                    {
                        
                        oRPTTime =DateTime.ParseExact(AlarmList[i].RPT_DATE_TIME.Substring(0, 17), "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                        oCLRTime =DateTime.ParseExact(AlarmList[i].CLEAR_DATE_TIME.Substring(0, 17), "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        oRPTTime = Convert.ToDateTime(AlarmList[i].RPT_DATE_TIME);
                        oCLRTime = Convert.ToDateTime(AlarmList[i].CLEAR_DATE_TIME);
                    }
                   
                    if (oCLRTime.Subtract(oRPTTime).TotalMinutes > 1440) continue;
                    //區間累計初始化，因為第0筆資料有可能是null資料，所以這段初始化擺在for迴圈內
                    if (!FirstReportByVHID.ContainsKey(AlarmList[i].EQPT_ID))
                    {
                        FirstReport oFirstReport = new FirstReport();
                        oFirstReport.ALARM_CODE = AlarmList[i].ALARM_CODE;
                        oFirstReport.ALARM_DESC = AlarmList[i].ALARM_DESC;
                        oFirstReport.EQPT_ID = AlarmList[i].EQPT_ID;
                        oFirstReport.First_RPT_DataTime = oRPTTime;
                        oFirstReport.Final_Clear_DateTime = oCLRTime;
                        oFirstReport.SelectStartTime = SelectStartTime;
                        oFirstReport.SelectEndTime = SelectEndTime;


                        FirstReportByVHID.Add(AlarmList[i].EQPT_ID, oFirstReport);
                    }

                    if (DateTime.Compare(oRPTTime, FirstReportByVHID[AlarmList[i].EQPT_ID].First_RPT_DataTime.AddSeconds(AlarmGroupTime)) > 0)
                    {
                        //Add FirstReportData
                        FirstReportByVHID[AlarmList[i].EQPT_ID].TimeCheck(FirstReportByVHID[AlarmList[i].EQPT_ID].First_RPT_DataTime, FirstReportByVHID[AlarmList[i].EQPT_ID].Final_Clear_DateTime);
                        AddFinalReport(ref dicAlarm, FirstReportByVHID[AlarmList[i].EQPT_ID]);

                        // New Data
                        FirstReportByVHID[AlarmList[i].EQPT_ID].ALARM_CODE = AlarmList[i].ALARM_CODE;
                        FirstReportByVHID[AlarmList[i].EQPT_ID].ALARM_DESC = AlarmList[i].ALARM_DESC;
                        FirstReportByVHID[AlarmList[i].EQPT_ID].EQPT_ID = AlarmList[i].EQPT_ID;
                        FirstReportByVHID[AlarmList[i].EQPT_ID].First_RPT_DataTime = oRPTTime;
                        FirstReportByVHID[AlarmList[i].EQPT_ID].Final_Clear_DateTime = oCLRTime;
                        FirstReportByVHID[AlarmList[i].EQPT_ID].SelectStartTime = SelectStartTime;
                        FirstReportByVHID[AlarmList[i].EQPT_ID].SelectEndTime = SelectEndTime;

                    }
                    else
                    {
                        if (DateTime.Compare(oCLRTime, FirstReportByVHID[AlarmList[i].EQPT_ID].Final_Clear_DateTime) > 0)
                        {
                            FirstReportByVHID[AlarmList[i].EQPT_ID].Final_Clear_DateTime = oCLRTime;
                        }

                    }

                }

                foreach (var item in FirstReportByVHID)
                {
                    item.Value.TimeCheck(item.Value.First_RPT_DataTime, item.Value.Final_Clear_DateTime);
                    AddFinalReport(ref dicAlarm, item.Value);
                }


                TotalList.Clear();
                TotalData.ErrorRate = Math.Round(TotalData.ErrorTime / AutoTime / EQPTNum.Count() * 100, 2);
                TotalList.Add(TotalData);

                return dicAlarm.Values.ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return new List<RATEALARM>();
            }

        }

        private void AddFinalReport(ref Dictionary<string, RATEALARM> dicAlarm, FirstReport oFirstReport)
        {
            if (DateTime.Compare(oFirstReport.Final_Clear_DateTime, oFirstReport.First_RPT_DataTime) < 0) return;

            if (dicAlarm.ContainsKey(oFirstReport.ALARM_CODE + "," + oFirstReport.EQPT_ID))
            {
                dicAlarm[oFirstReport.ALARM_CODE + "," + oFirstReport.EQPT_ID].ErrorTime += oFirstReport.Final_Clear_DateTime.Subtract(oFirstReport.First_RPT_DataTime).TotalMinutes;
                dicAlarm[oFirstReport.ALARM_CODE + "," + oFirstReport.EQPT_ID].AlarmCount += 1;
                dicAlarm[oFirstReport.ALARM_CODE + "," + oFirstReport.EQPT_ID].ErrorRate = Math.Round((dicAlarm[oFirstReport.ALARM_CODE + "," + oFirstReport.EQPT_ID].ErrorTime) / AutoTime * 100, 2);
                TotalData.ErrorTime += oFirstReport.Final_Clear_DateTime.Subtract(oFirstReport.First_RPT_DataTime).TotalMinutes;
                TotalData.AlarmCount += 1;
            }
            else
            {
                // Add Dictionary
                RATEALARM oRATEALARM = new RATEALARM();
                oRATEALARM.ALARM_CODE = oFirstReport.ALARM_CODE;
                oRATEALARM.ALARM_DESC = oFirstReport.ALARM_DESC;
                oRATEALARM.EQPT_ID = oFirstReport.EQPT_ID;
                oRATEALARM.AlarmCount = 1;
                oRATEALARM.ErrorTime = oFirstReport.Final_Clear_DateTime.Subtract(oFirstReport.First_RPT_DataTime).TotalMinutes;
                oRATEALARM.ErrorRate = Math.Round((oRATEALARM.ErrorTime) / AutoTime * 100, 2);
                TotalData.ErrorTime += oRATEALARM.ErrorTime;
                TotalData.AlarmCount += 1;

                dicAlarm.Add(oFirstReport.ALARM_CODE + "," + oFirstReport.EQPT_ID, oRATEALARM);
                if (!EQPTNum.Contains(oFirstReport.EQPT_ID))
                {
                    EQPTNum.Add(oFirstReport.EQPT_ID);
                }
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
                foreach (DataGridColumn dc in dgv_total_query.Columns)
                {
                    StringReader stringReader = new StringReader("<Style xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" TargetType=\"{x:Type DataGridCell}\"> <Setter Property=\"Control.HorizontalAlignment\" Value = \"Right\" /></Style>");
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    Style style = (Style)System.Windows.Markup.XamlReader.Load(xmlReader);
                    switch (dc.Header.ToString())
                    {
                        case nameof(RATEALARM.ErrorRate):
                        case nameof(RATEALARM.AlarmCount):
                        case nameof(RATEALARM.ErrorTime):
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
                        case nameof(RATEALARM.ErrorRate):
                        case nameof(RATEALARM.AlarmCount):
                        case nameof(RATEALARM.ErrorTime):
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
                dgv_total_query.ItemsSource = null;
                TotalData = new RATEALARM();
                TotalData.ALARM_CODE = "Total";
                TotalData.ALARM_DESC = "*****";
                TotalData.EQPT_ID = "*****";
                TotalData.AlarmCount = 0;
                TotalData.ErrorTime = 0;

                List<RATEALARM> list;
                List<RATEALARM> list2;

                //Filter by EQPT_ID and Alarm_Code
                list = RATEALARM_List?.Where(x =>
                ((x.EQPT_ID.Contains(text_EQID.Text) && text_EQID.Text != "") || (text_EQID.Text == ""))
                &&
                ((x.ALARM_CODE.Contains(text_Alarm_Code.Text) && text_Alarm_Code.Text != "") || (text_Alarm_Code.Text == ""))
                ).OrderByDescending(info => info.ErrorRate).ToList();

                list2 = list;
                TotalList.Clear();

                if (chPLC == 'N')
                {

                }
                else
                {
                    //split PLC Type
                    string[] sp;
                    list2 = list?.Where(x =>
                    {
                        sp = x.EQPT_ID.Split('_');
                        return (sp[sp.Count() - 1][0] == chPLC && !sp[sp.Count() - 1].Contains("AGV"));
                    }).ToList();
                }


                dgv_log_query.ItemsSource = list2;

                #region Total Grid Data
                TotalData.AlarmCount = list2.Sum(i => i.AlarmCount);
                TotalData.ErrorTime = list2.Sum(i => i.ErrorTime);
                EQPTNum.Clear();

                foreach (RATEALARM o in list2)
                {
                    if (!EQPTNum.Contains(o.EQPT_ID))
                    {
                        EQPTNum.Add(o.EQPT_ID);

                    }
                }

                TotalData.ErrorRate = Math.Round(TotalData.ErrorTime / AutoTime / EQPTNum.Count() * 100, 2);
                TotalList.Add(TotalData);
                dgv_total_query.ItemsSource = TotalList;
                #endregion

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
        private void btn_Chart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_StartDTCbx.SelectedDate.Value == null && m_EndDTCbx.SelectedDate.Value == null) return;
                if (dgv_log_query.ItemsSource.OfType<RATEALARM>().Count() == 0) return;
                List<RATEALARM> oRATEALARM = dgv_log_query.ItemsSource.OfType<RATEALARM>().ToList();
                var groups = oRATEALARM.GroupBy(o => o.ALARM_CODE).Select(o => new GroupRATEALARM() { alarmCode = o.Key, RATEALARMs = o.ToList() }).OrderByDescending(g => g.count);
                Dictionary<string, double> dicValues = new Dictionary<string, double>();
                Dictionary<string, string> dicLabel = new Dictionary<string, string>();
                //foreach (RATEALARM item in oRATEALARM)
                foreach (var item in groups)
                {
                    dicValues.Add(item.alarmCode, item.count);
                    dicLabel.Add(item.alarmCode, "Code:" + item.alarmCode + $"-Desc:{item.alarmDesc}" + "(" + item.sum + " min)");

                    //if (!dicValues.ContainsKey(item.ALARM_CODE))
                    //{
                    //    dicValues.Add(item.ALARM_CODE, oRATEALARM.Where(info => info.ALARM_CODE == item.ALARM_CODE).Sum(info => info.AlarmCount));
                    //    dicLabel.Add(item.ALARM_CODE, "Code:" + item.ALARM_CODE + $"Desc:{item.ALARM_DESC}" + "(" + oRATEALARM.Where(info => info.ALARM_CODE == item.ALARM_CODE).Sum(info => info.ErrorTime) + " min)");
                    //}
                }

                PieChart oPieChart = new PieChart(dicValues, dicLabel);
                oPieChart.ShowLabelInChart = false;
                new Chart_BasePopupWindow(WindownApplication.getInstance().oChartConverter.GetPieChart(oPieChart, Title: this.Name), _filename: this.Name + "_" + SelectStartTime.ToString("yyyyMMddHH") + "-" + SelectEndTime.ToString("yyyyMMddHH")).ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        public class GroupRATEALARM
        {
            public string alarmCode = "";
            public string alarmDesc
            {
                get
                {
                    if (RATEALARMs == null || RATEALARMs.Count == 0) return "";
                    return RATEALARMs[0].ALARM_DESC;
                }
            }
            public List<RATEALARM> RATEALARMs = null;
            public int count
            {
                get
                {
                    if (RATEALARMs == null) return 0;

                    return RATEALARMs.Sum(info => info.AlarmCount);
                }
            }
            public double sum
            {
                get
                {
                    if (RATEALARMs == null || RATEALARMs.Count == 0) return 0;
                    return RATEALARMs.Sum(info => info.ErrorTime);
                }
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

public class RATEALARM
{

    public string ALARM_CODE { get; set; }
    public string ALARM_DESC { get; set; }
    public string EQPT_ID { get; set; }
    public double ErrorRate { get; set; }

    public int AlarmCount { get; set; }
    public double ErrorTime { get { return Math.Round(errorRate)==0? 1:Math.Round(errorRate) ; } set { errorRate = value; } }
    private double errorRate { get; set; } = 0.0;


}

public class FirstReport
{

    public string ALARM_CODE { get; set; }
    public string ALARM_DESC { get; set; }
    public string EQPT_ID { get; set; }
    public DateTime First_RPT_DataTime { get; set; }
    public DateTime Final_Clear_DateTime { get; set; }
    public DateTime SelectStartTime { get; set; }
    public DateTime SelectEndTime { get; set; }


    public void TimeCheck(DateTime RPTTime, DateTime CLRTime)
    {
        //  Start -> End
        //  |  |                |      |
        //	   ↑				↑
        //		取這兩段的值為區間
        //如果選取的Start Time比資料晚，StartTime以選取為主
        if (DateTime.Compare(SelectStartTime, RPTTime) > 0)
        {
            First_RPT_DataTime = SelectStartTime;
        }
        else
        {
            First_RPT_DataTime = RPTTime;
        }

        if (DateTime.Compare(CLRTime, SelectEndTime) > 0)
        {
            Final_Clear_DateTime = SelectEndTime;
        }
        else
        {
            Final_Clear_DateTime = CLRTime;
        }
    }
}