using com.mirle.ibg3k0.ohxc.wpf.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using ViewerObject;
using CommonMessage.ProtocolFormat.AlarmFun;
using MirleGO_UIFrameWork.UI.uc_Button;
using com.mirle.ibg3k0.bc.wpf.App;

namespace UtilsAPI.BLL
{
    public class AlarmBLL
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        WindownApplication app = null;
        //public EventHandler<CommonMessage.ProtocolFormat.AlarmFun.ControlRequest> alarmUpdateEventHandler;

        private bool mulitLanguage = false; //若該專案config底下有多語言系的Alarm map，則再取VAlarm時會將裡面的alarm_desc替換成csv檔的資料
        List<AlarmMap> alarmMap = null;
        List<AlarmModule> alarmModule = null;

        public AlarmBLL(WindownApplication _app)
        {
            app = _app;
            //alarmUpdateEventHandler += alarmUpdate;
        }

        public void setMulitLanguage(bool MulitLanguage, List<AlarmMap> AlarmMap)
        {
            mulitLanguage = MulitLanguage;
            alarmMap = AlarmMap;
        }
        public void setAlarmModuleLsit(List<AlarmModule> AlarmModule)
        {
            alarmModule = AlarmModule;
        }

        public List<VALARM> LoadSetAlarms()
        {
            List<VALARM> default_result = new List<VALARM>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadSetAlarms();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VALARM> GetAlarmsByDate(DateTime date)
        {
            List<VALARM> default_result = new List<VALARM>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.GetAlarmsByDate(date);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VALARM> GetAlarmsByConditions(DateTime startDatetime, DateTime endDatetime, string eqptID = null, string alarmCode = null, string Alarm_Level = null, bool cleartimenotnull = false)
        {
            List<VALARM> default_result = new List<VALARM>();
            List<VALARM> result;
            try
            {
                if (!mulitLanguage)
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.GetAlarmsByConditions(startDatetime,
                        endDatetime, eqptID, alarmCode, Alarm_Level, cleartimenotnull, alarmModule: alarmModule);
                else
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.GetAlarmsByConditions(startDatetime,
                        endDatetime, eqptID, alarmCode, Alarm_Level, cleartimenotnull, alarmMap, alarmModule: alarmModule); //用有無傳遞alarmMap來表示要不要做多國翻譯
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        /// <summary>
        /// 藉由錯誤的MCS命令找尋DB內相關時間的Alarm
        /// </summary>
        /// <param name="startDatetime">開始的CMD Start時間</param>
        /// <param name="endDatetime">結束的CMD Start時間</param>
        /// <param name="Alarm_Level">尋找的Alarm Level，預設null為全找</param>
        /// <param name="TimeIntervalAdd">開始/結束時間範圍區間擴大(單位:分鐘)</param>
        /// <returns> VErrorCMD List，包含Alarm List的CMD陣列</returns>
        public List<ViewerObject.REPORT.VErrorCMD> LoadAlarmsByErrorMCSCmdTime(DateTime startDatetime, DateTime endDatetime, string Alarm_Level = null, int TimeIntervalAdd = 0)
        {
            List<ViewerObject.REPORT.VErrorCMD> default_result = new List<ViewerObject.REPORT.VErrorCMD>();
            List<ViewerObject.REPORT.VErrorCMD> result;
            try
            {
                //return app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByErrorMCSCmdTime(startDatetime.AddMinutes(0 - TimeIntervalAdd), endDatetime.AddMinutes(TimeIntervalAdd), Alarm_Level);
                if (!mulitLanguage)
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByErrorMCSCmdTime(startDatetime.AddMinutes(0 - TimeIntervalAdd), endDatetime.AddMinutes(TimeIntervalAdd), Alarm_Level, alarmModule: alarmModule);
                else
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByErrorMCSCmdTime(startDatetime.AddMinutes(0 - TimeIntervalAdd), endDatetime.AddMinutes(TimeIntervalAdd), Alarm_Level, alarmMap: alarmMap, alarmModule: alarmModule); //用有無傳遞alarmMap來表示要不要做多國翻譯
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        /// <summary>
        /// 藉由錯誤的OHTC命令找尋DB內相關時間的Alarm
        /// </summary>
        /// <param name="startDatetime">開始的CMD Start時間</param>
        /// <param name="endDatetime">結束的CMD Start時間</param>
        /// <param name="Alarm_Level">尋找的Alarm Level，預設null為全找</param>
        /// <param name="TimeIntervalAdd">開始/結束時間範圍區間擴大(單位:分鐘)</param>
        /// <returns> VErrorCMD List，包含Alarm List的CMD陣列</returns>
        public List<ViewerObject.REPORT.VErrorCMD_OHTC> LoadAlarmsByErrorOHTCCmdTime(DateTime startDatetime, DateTime endDatetime, string Alarm_Level = null, int TimeIntervalAdd = 0)
        {
            List<ViewerObject.REPORT.VErrorCMD_OHTC> default_result = new List<ViewerObject.REPORT.VErrorCMD_OHTC>();
            List<ViewerObject.REPORT.VErrorCMD_OHTC> result;
            try
            {
                //return app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByErrorMCSCmdTime(startDatetime.AddMinutes(0 - TimeIntervalAdd), endDatetime.AddMinutes(TimeIntervalAdd), Alarm_Level);
                if (!mulitLanguage)
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByErrorOHTCCmdTime(startDatetime.AddMinutes(0 - TimeIntervalAdd), endDatetime.AddMinutes(TimeIntervalAdd), Alarm_Level, alarmModule: alarmModule);
                else
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByErrorOHTCCmdTime(startDatetime.AddMinutes(0 - TimeIntervalAdd), endDatetime.AddMinutes(TimeIntervalAdd), Alarm_Level, alarmMap: alarmMap, alarmModule: alarmModule); //用有無傳遞alarmMap來表示要不要做多國翻譯
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        /// <summary>
        /// 藉由超時MCS命令找尋DB內相關時間的Alarm
        /// </summary>
        /// <param name="startDatetime">開始的CMD Start時間</param>
        /// <param name="endDatetime">結束的CMD Start時間</param>
        /// <param name="Alarm_Level">尋找的Alarm Level，預設null為全找</param>
        /// <param name="TimeIntervalAdd">開始/結束時間範圍區間擴大(單位:分鐘)</param>
        /// <returns> VTimeOutCMD List，包含Alarm List的CMD陣列</returns>
        public List<ViewerObject.REPORT.VTimeOutCMD> LoadAlarmsByTimeOutCycleTime_MCSCmd(DateTime startDatetime, DateTime endDatetime, int OverTime_Min, string Alarm_Level = null, int TimeIntervalAdd = 0)
        {
            List<ViewerObject.REPORT.VTimeOutCMD> default_result = new List<ViewerObject.REPORT.VTimeOutCMD>();
            List<ViewerObject.REPORT.VTimeOutCMD> result;
            try
            {
                //return app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByTimeOutCycleTime_MCSCmd(startDatetime, endDatetime, OverTime_Min, Alarm_Level, TimeIntervalAdd);
                if (!mulitLanguage)
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByTimeOutCycleTime_MCSCmd(startDatetime, endDatetime, OverTime_Min, Alarm_Level, TimeIntervalAdd, alarmModule: alarmModule);
                else
                    result = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.LoadAlarmsByTimeOutCycleTime_MCSCmd(startDatetime, endDatetime, OverTime_Min, Alarm_Level, TimeIntervalAdd, alarmMap: alarmMap, alarmModule: alarmModule); //用有無傳遞alarmMap來表示要不要做多國翻譯
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }



        
        public void InsertAlarm(VALARM alarm)
        {
            try
            {
                app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.InsertAlarm(alarm);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void GetAlarmLimitDateTime(out DateTime StartTime, out DateTime EndTime)
        {
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
            try
            {
                app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.GetAlarmLimitDateTime(out StartTime, out EndTime);
                StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, StartTime.Hour, 0, 0);
                EndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, EndTime.Hour, 0, 0).AddHours(1);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }


        public bool alarmUpdate(object obj, ControlRequest request)
        {
            string result = "";
            bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.AlarmBLL.AlarmUpdate(request, out result);
            if (!isSuccess)
            {
                TipMessage_Type_Light.Show("Send command failed", result, BCAppConstants.INFO_MSG);
            }
            else
            {
                TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
            }
            return isSuccess;
        }

    }
}