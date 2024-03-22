using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessage.ProtocolFormat.AlarmFun;


namespace ObjectConverterInterface.BLL
{
    public interface IAlarmBLL
    {
        List<ViewerObject.VALARM> LoadSetAlarms();
        List<ViewerObject.VALARM> GetAlarmsByDate(DateTime date);
        List<ViewerObject.VALARM> GetAlarmsByConditions(DateTime startDatetime, DateTime endDatetime, string eqptID = null, string alarmCode = null, string Alarm_Level = null, bool cleartimenotnull = false, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null);
        void InsertAlarm(ViewerObject.VALARM valarm);
        List<ViewerObject.REPORT.VErrorCMD> LoadAlarmsByErrorMCSCmdTime(DateTime startDatetime, DateTime endDatetime, string Alarm_Level = null, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null);
        List<ViewerObject.REPORT.VTimeOutCMD> LoadAlarmsByTimeOutCycleTime_MCSCmd(DateTime startDatetime, DateTime endDatetime, int OverTime_Min, string Alarm_Level = null, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null);
        void GetAlarmLimitDateTime(out DateTime StartTime, out DateTime EndTime);
        List<ViewerObject.REPORT.VErrorCMD_OHTC> LoadAlarmsByErrorOHTCCmdTime(DateTime startDatetime, DateTime endDatetime, string Alarm_Level = null, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null);

        bool AlarmUpdate(ControlRequest request, out string result);
    }
}
