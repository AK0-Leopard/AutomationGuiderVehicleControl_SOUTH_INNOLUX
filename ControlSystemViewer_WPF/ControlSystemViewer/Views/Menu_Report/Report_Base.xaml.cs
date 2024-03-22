using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
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
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_Report
{
    /// <summary>
    /// Log_Base.xaml 的互動邏輯
    /// </summary>
    public partial class Report_Base : UserControl
    {
        #region 公用參數設定
        public WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string CustomerName = null;
        bool ReShow = true;
 
        
        #endregion 公用參數設定

        public Report_Base()
        {
            InitializeComponent();
        }

        public void Show(WindownApplication _app, string sSelectTab = null)
        {
            if (!this.IsVisible)
            {
                this.Visibility = Visibility.Visible;
                ReportDataProcess.Initializing(_app);
                startupUI(_app);
              
            }

            if (TabControl.Items != null)
            {
                if (!string.IsNullOrEmpty(sSelectTab))
                {
                    for (int i = TabControl.Items.Count - 1; i >= 0; i--)
                    {
                        TabItem tab = TabControl.Items[i] as TabItem;
                        if ((i > 0 && sSelectTab == tab.Header?.ToString())
                            || i == 0) // set as default
                        {
                            TabControl.SelectedIndex = i;
                            //lbl_Title.Content = tab.Header?.ToString();
                            break;
                        }
                    }
                }
            }


            //利用實際寬高註冊報表被重新Show的時候的事件

            //this.LayoutUpdated += (o, e) =>
            //{
            //    if ( (this.ActualHeight > 0 || this.ActualWidth > 0))
            //    {
            //        if(ReShow==true)
            //        {
            //            UserControlReShow();
            //            ReShow = false;
            //        }                 
            //    }
            //    else
            //    {
            //        ReShow = true;
            //    }
            //};

        }


        private void UserControlReShow()
        {
            try
            {
                if(ReportDataProcess.RefreshLimitTime())
                {
                    RefreshAllReport_LimitTime();
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }    
        }

        private void RefreshAllReport_LimitTime()
        {
            try
            {
                AlarmRate.ReSetLimitTime(ReportDataProcess.LimitStartDate_Alarm, ReportDataProcess.LimitFinalDate_Alarm);
                AlarmDetail.ReSetLimitTime(ReportDataProcess.LimitStartDate_Alarm, ReportDataProcess.LimitFinalDate_Alarm);
                MTBF.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                MCBF.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                MTTR.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                StabilityByDuration.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                StabilityByStatus.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                StabilityByDate.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                UtilizationRateByVehicle.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                UtilizationRateByPort.ReSetLimitTime( ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                UtilizationRateByHour.ReSetLimitTime( ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                MCBFByCMD.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                ErrorCMD.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                OHTCErrorCMD.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                TimeOutCMD.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                //LongCharging.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                LongCharging.ReSetLimitTime(DateTime.MinValue, DateTime.MaxValue);
                UtilizationRateByStatus.ReSetLimitTime( ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
                RealExecuteTime.ReSetLimitTime(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            }
            catch (Exception ex)
            {
                //do nothing
            }
        }


        public void Close()
        {
            if (this.IsVisible) _CloseEvent(null, null);
        }

        private void startupUI(WindownApplication _app)
        {
            app = _app;
            CustomerName= WindownApplication.getInstance().ObjCacheManager.GetSelectedProject().Customer.ToString();
            registerEvents();
            //ReportDataProcess.RefreshLimitTime();


            AlarmRate.StartupUI(ReportDataProcess.LimitStartDate_Alarm, ReportDataProcess.LimitFinalDate_Alarm);
            AlarmDetail.StartupUI(ReportDataProcess.LimitStartDate_Alarm, ReportDataProcess.LimitFinalDate_Alarm);
            MTBF.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            MCBF.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            MTTR.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            StabilityByDuration.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            StabilityByStatus.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            StabilityByDate.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
           

            var me = this;
            UtilizationRateByVehicle.StartupUI(ref me, ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            UtilizationRateByPort.StartupUI(ref me, ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            UtilizationRateByHour.StartupUI(ref me, ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            MCBFByCMD.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            ErrorCMD.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            OHTCErrorCMD.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            TimeOutCMD.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            LongCharging.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            UtilizationRateByStatus.StartupUI(ref me,ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            RealExecuteTime.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);
            //IntervalMTBF.StartupUI();
            //IntervalMCBFByCMD.StartupUI();
            //VehicleCommandHistory.StartupUI();
            //TransferCommandHistory.StartupUI();
            HIDinfohistory.StartupUI(ReportDataProcess.LimitStartTime_MCS_OHTC, ReportDataProcess.LimitEndTime_MCS_OHTC);


            if (app?.ObjCacheManager.ViewerSettings?.menuItem_Log.Visible_STATISTICS ?? false)
            {
                //TabItem_STATISTICS.Visibility = Visibility.Visible;
                //Chart_RunDownIdle.StartupUI();
            }
            if (app?.ObjCacheManager.ViewerSettings?.InfluxDBsetting.Token_VehicleInfo == "")
            {
                TabUtilizationRateByStatus.Visibility = Visibility.Collapsed;
                //TabLongCharging.Visibility = Visibility.Collapsed;
                TabStabilityByStatus.Visibility = Visibility.Collapsed;
                //TabItem_STATISTICS.Visibility = Visibility.Visible;
                //Chart_RunDownIdle.StartupUI();
            }

            if(!CustomerName.Contains("M4"))
            {
                TabRealExecuteTime.Visibility = Visibility.Collapsed;
                TabHIDinfohistory.Visibility = Visibility.Collapsed;
            }
        }

        private void registerEvents()
        {
            if (app != null)
                app.LanguageChanged += _LanguageChanged;

            TabControl.SelectionChanged += _SelectionChanged;

            AlarmRate.CloseEvent += _CloseEvent;
            AlarmDetail.CloseEvent += _CloseEvent;
            MTBF.CloseEvent += _CloseEvent;
            MCBF.CloseEvent += _CloseEvent;
            MTTR.CloseEvent += _CloseEvent;
            StabilityByDuration.CloseEvent += _CloseEvent;
            StabilityByStatus.CloseEvent += _CloseEvent;
            UtilizationRateByVehicle.CloseEvent += _CloseEvent;
            UtilizationRateByPort.CloseEvent += _CloseEvent;
            UtilizationRateByHour.CloseEvent += _CloseEvent;
            MCBFByCMD.CloseEvent += _CloseEvent;
            ErrorCMD.CloseEvent += _CloseEvent;
            OHTCErrorCMD.CloseEvent += _CloseEvent;
            TimeOutCMD.CloseEvent += _CloseEvent;
            StabilityByDate.CloseEvent += _CloseEvent;
            LongCharging.CloseEvent += _CloseEvent;
            UtilizationRateByStatus.CloseEvent += _CloseEvent;
           
            //IntervalMTBF.CloseEvent += _CloseEvent;
            //IntervalMCBFByCMD.CloseEvent += _CloseEvent;
            //VehicleCommandHistory.CloseEvent += _CloseEvent;
            //TransferCommandHistory.CloseEvent += _CloseEvent;
            if (CustomerName.Contains("M4"))
            {
                RealExecuteTime.CloseEvent += _CloseEvent;
            }
        }

        private void unregisterEvents()
        {
            if (app != null)
                app.LanguageChanged -= _LanguageChanged;

            TabControl.SelectionChanged -= _SelectionChanged;

            AlarmRate.CloseEvent -= _CloseEvent;
            AlarmDetail.CloseEvent -= _CloseEvent;
            MTBF.CloseEvent -= _CloseEvent;
            MCBF.CloseEvent -= _CloseEvent;
            MTTR.CloseEvent -= _CloseEvent;
            StabilityByDuration.CloseEvent -= _CloseEvent;
            StabilityByStatus.CloseEvent -= _CloseEvent;
            UtilizationRateByVehicle.CloseEvent -= _CloseEvent;
            UtilizationRateByPort.CloseEvent -= _CloseEvent;
            UtilizationRateByHour.CloseEvent -= _CloseEvent;
            MCBFByCMD.CloseEvent -= _CloseEvent;
            ErrorCMD.CloseEvent -= _CloseEvent;
            OHTCErrorCMD.CloseEvent -= _CloseEvent;
            TimeOutCMD.CloseEvent -= _CloseEvent;
            StabilityByDate.CloseEvent -= _CloseEvent;
            LongCharging.CloseEvent -= _CloseEvent;
            UtilizationRateByStatus.CloseEvent -= _CloseEvent;
            
            //IntervalMTBF.CloseEvent -= _CloseEvent;
            //IntervalMCBFByCMD.CloseEvent -= _CloseEvent;
            //VehicleCommandHistory.CloseEvent -= _CloseEvent;
            //TransferCommandHistory.CloseEvent -= _CloseEvent;
            if (CustomerName.Contains("M4"))
            {
                RealExecuteTime.CloseEvent -= _CloseEvent;
            }
        }

        private void _CloseEvent(object sender, EventArgs e)
        {
            try
            {
                unregisterEvents();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            this.Visibility = Visibility.Collapsed;
        }

        private void _SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetSubTitle();
            var x= ((SelectionChangedEventArgs)e).AddedItems[0];
            if ( x.GetType() == typeof(TabItem) )
            {
                UserControlReShow();
            }
          
        }

        private void _LanguageChanged(object sender, EventArgs e)
        {
            resetSubTitle();
        }

        private void resetSubTitle()
        {
            if (TabControl.SelectedIndex < 0)
            {
                lbl_Title.Content = "";
            }
            else
            {
                lbl_Title.Content = ((TabItem)TabControl.SelectedItem).Header?.ToString() ?? "";
            }
        }




    }


    public static class ReportDataProcess
    {
        //MCS_OHTC 資料時間區間
        public static DateTime LimitStartTime_MCS_OHTC = DateTime.MinValue;
        public static DateTime LimitEndTime_MCS_OHTC = DateTime.MaxValue;

        //Alarm 資料時間區間
        public static DateTime LimitStartDate_Alarm = DateTime.MinValue;
        public static DateTime LimitFinalDate_Alarm = DateTime.MaxValue;

        private static  WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static  void Initializing(WindownApplication _app)
        {
            if (_app == null) return;
            if (app != null) return;
            app = _app;
        }

        public static bool RefreshLimitTime()
        {
            bool RefreshFlag = false;
            try
            {

                    app.CmdBLL.GetHCMD_MCSLimitDateTime(out LimitStartTime_MCS_OHTC, out LimitEndTime_MCS_OHTC);
                    app.AlarmBLL.GetAlarmLimitDateTime(out LimitStartDate_Alarm, out LimitFinalDate_Alarm);
                    RefreshFlag = true;

                                  
            }
            catch (System.Data.Entity.Core.EntityCommandExecutionException eceex)
            {
                logger.Error(eceex, "EntityCommandExecutionException");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return RefreshFlag;
        }



        //Last Refresh
        private static DateTime LastUpdateLimitTime = DateTime.MinValue;




    }
}
