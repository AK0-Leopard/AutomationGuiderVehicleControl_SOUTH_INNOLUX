using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using CommonMessage.ProtocolFormat.AlarmFun;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject.REPORT;

namespace ObjectConverter_AGVC_AUO_600.BLL
{
    public class AlarmBLL : IAlarmBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private AlarmConverter alarmConverter = new AlarmConverter();

        private string connectionString = "";
        public AlarmBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public List<ViewerObject.VALARM> LoadSetAlarms() => alarmConverter.GetVALARMs(loadSetAlarms());
        private List<ALARM> loadSetAlarms()
        {
            List<ALARM> alarms = new List<ALARM>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from a in con.ALARM.AsNoTracking()
                            where a.ALAM_STAT == ErrorStatus.ErrSet
                            select a;
                alarms.AddRange(query?.ToList() ?? new List<ALARM>());
            }
            return alarms;
        }

        public List<ViewerObject.VALARM> GetAlarmsByDate(DateTime date) => alarmConverter.GetVALARMs(getAlarmsByDate(date));
        private List<ALARM> getAlarmsByDate(DateTime date)
        {
            DateTime date_NextDay = date.Date.AddDays(1);
            List<ALARM> alarms = new List<ALARM>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.ALARM.AsNoTracking()
                            where cmd.ALAM_LVL == E_ALARM_LVL.Error && cmd.RPT_DATE_TIME < date_NextDay
                               && (cmd.CLEAR_DATE_TIME == null || cmd.CLEAR_DATE_TIME >= date.Date)
                            select cmd;
                alarms.AddRange(query?.OrderBy(c => c.RPT_DATE_TIME).ToList() ?? new List<ALARM>());
            }
            return alarms;
        }

        public List<ViewerObject.VALARM> GetAlarmsByConditions(DateTime startDatetime, DateTime endDatetime, string eqptID = null, string alarmCode = null, string Alarm_Level = null, bool cleartimenotnull = false, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null) 
                                         => alarmConverter.GetVALARMs(getAlarmsByConditions(startDatetime, endDatetime, eqptID, alarmCode, CheckAlarmLevel(Alarm_Level)));
        private List<ALARM> getAlarmsByConditions(DateTime startDatetime, DateTime endDatetime, string eqptID = null, string alarmCode = null, E_ALARM_LVL Alarm_Level = E_ALARM_LVL.None, bool cleartimenotnull = false)
        {
            List<ALARM> alarms = new List<ALARM>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from a in con.ALARM.AsNoTracking()
                            where a.RPT_DATE_TIME >= startDatetime && a.RPT_DATE_TIME <= endDatetime && (Alarm_Level == E_ALARM_LVL.None || a.ALAM_LVL == Alarm_Level) && ((cleartimenotnull == false) || (cleartimenotnull == true && a.CLEAR_DATE_TIME != null))
                            select a;
                var list = query?.ToList().Where(t =>
                {
                    bool b = true;
                    if (eqptID != null)
                    {
                        b = b & (t.EQPT_ID?.Trim() == eqptID.Trim());
                    }
                    return b;
                })?.ToList() ?? new List<ALARM>();
                alarms.AddRange(list);
            }
            return alarms;
        }

        private E_ALARM_LVL CheckAlarmLevel(string Alarm_Level)
        {
            if (Alarm_Level != null)
            {
                switch (Alarm_Level)
                {
                    case "1":
                        return E_ALARM_LVL.Warn;
                    case "2":
                        return E_ALARM_LVL.Error;
                }
            }
            return E_ALARM_LVL.None;
        }

        public void InsertAlarm(ViewerObject.VALARM valarm)
        {
            insertAlarm(alarmConverter.GetALARM(valarm));
        }
        private void insertAlarm(ALARM alarm)
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                con.ALARM.Add(alarm);
                con.SaveChanges();
            }
        }

        #region Report

        public List<ViewerObject.REPORT.VErrorCMD> LoadAlarmsByErrorMCSCmdTime(DateTime startDatetime, DateTime endDatetime, string Alarm_Level = null, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null)
                                        => loadalarmsbyerrormcscmdtime(startDatetime, endDatetime, CheckAlarmLevel(Alarm_Level), TimeIntervalAdd);
        private List<ViewerObject.REPORT.VErrorCMD> loadalarmsbyerrormcscmdtime(DateTime startDatetime, DateTime endDatetime, E_ALARM_LVL Alarm_Level = E_ALARM_LVL.None, int TimeIntervalAdd = 0)
        {
            List<ViewerObject.REPORT.VErrorCMD> lsReturn = new List<ViewerObject.REPORT.VErrorCMD>();
            List<ALARM> lsAlarm = new List<ALARM>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;



                    var Alarmquery = from cmd in con.ALARM.AsNoTracking()
                                     where (Alarm_Level == E_ALARM_LVL.None || cmd.ALAM_LVL == Alarm_Level) &&
                                     (cmd.RPT_DATE_TIME < endDatetime && cmd.RPT_DATE_TIME >= startDatetime) &&
                                     cmd.CLEAR_DATE_TIME != null
                                     select cmd;
                    lsAlarm = Alarmquery?.ToList();


                    var CMDquery = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                   where (sbTable.MCS_CMD_FINISH_TIME < endDatetime && sbTable.MCS_CMD_START_TIME >= startDatetime)
                                         &&
                                         (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort) &&
                                         sbTable.VH_ID != null &&
                                         sbTable.MCS_CMD_FINISH_TIME != null
                                   select sbTable;

                    foreach (var item in CMDquery?.ToList())
                    {

                        ViewerObject.REPORT.VErrorCMD oVErrorCMD = new ViewerObject.REPORT.VErrorCMD();
                        oVErrorCMD.MCS_CMD_ID = item.CMD_ID_MCS.Trim();
                        oVErrorCMD.VH_ID = item.VH_ID.Trim();
                        oVErrorCMD.CARRIER_ID = item.CARRIER_ID.Trim();
                        oVErrorCMD.HOSTSOURCE = item.HOSTSOURCE.Trim();
                        oVErrorCMD.HOSTDESTINATION = item.HOSTDESTINATION.Trim();
                        oVErrorCMD.OHTC_CMD_STATUS = Define.DefineInt_To_EOHTC_CMD_STATUS(item.OHTC_CMD_STATUS);
                        oVErrorCMD.MCS_INSERT_TIME = Convert.ToDateTime(item.MCS_CMD_INSER_TIME);
                        oVErrorCMD.MCS_START_TIME = Convert.ToDateTime(item.MCS_CMD_START_TIME);
                        oVErrorCMD.MCS_FINISH_TIME = Convert.ToDateTime(item.MCS_CMD_FINISH_TIME);
                        oVErrorCMD.AlarmsInCMD = alarmConverter.GetVALARMs(lsAlarm.Where(info => info.EQPT_ID.Trim() == item.VH_ID.Trim() && (
                        !(((oVErrorCMD.MCS_START_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_START_TIME < info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.MCS_FINISH_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_FINISH_TIME < info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd))) ||
                        ((oVErrorCMD.MCS_START_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_START_TIME > info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.MCS_FINISH_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_FINISH_TIME > info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd))))
                        )
                        ).ToList());
                        lsReturn.Add(oVErrorCMD);
                    }
                }
                return lsReturn;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return lsReturn;
            }

        }

        public List<ViewerObject.REPORT.VTimeOutCMD> LoadAlarmsByTimeOutCycleTime_MCSCmd(DateTime startDatetime, DateTime endDatetime, int OverTime_Min, string Alarm_Level = null, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null)
                                          => loadalarmsbytimeoutcycletime_mcscmd(startDatetime, endDatetime, OverTime_Min, CheckAlarmLevel(Alarm_Level), TimeIntervalAdd);
        private List<ViewerObject.REPORT.VTimeOutCMD> loadalarmsbytimeoutcycletime_mcscmd(DateTime startDatetime, DateTime endDatetime, int OverTime_Min, E_ALARM_LVL Alarm_Level = E_ALARM_LVL.None, int TimeIntervalAdd = 0)
        {
            List<ViewerObject.REPORT.VTimeOutCMD> lsReturn = new List<ViewerObject.REPORT.VTimeOutCMD>();
            List<ALARM> lsAlarm = new List<ALARM>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;



                    var Alarmquery = from cmd in con.ALARM.AsNoTracking()
                                     where (Alarm_Level == E_ALARM_LVL.None || cmd.ALAM_LVL == Alarm_Level) &&
                                     (cmd.RPT_DATE_TIME < endDatetime && cmd.RPT_DATE_TIME >= startDatetime) &&
                                     cmd.CLEAR_DATE_TIME != null
                                     select cmd;
                    lsAlarm = Alarmquery?.ToList();


                    var CMDquery = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                   where (sbTable.MCS_CMD_FINISH_TIME < endDatetime && sbTable.MCS_CMD_START_TIME >= startDatetime)
                                         &&
                                         (DbFunctions.DiffMinutes(sbTable.MCS_CMD_START_TIME, sbTable.MCS_CMD_FINISH_TIME) >= OverTime_Min) &&
                                         sbTable.VH_ID != null &&
                                         sbTable.MCS_CMD_FINISH_TIME != null
                                   select sbTable;

                    foreach (var item in CMDquery?.ToList())
                    {

                        ViewerObject.REPORT.VTimeOutCMD oVTimeOutCMD = new ViewerObject.REPORT.VTimeOutCMD();
                        oVTimeOutCMD.MCS_CMD_ID = item.CMD_ID_MCS.Trim();
                        oVTimeOutCMD.VH_ID = item.VH_ID.Trim();
                        oVTimeOutCMD.CARRIER_ID = item.CARRIER_ID.Trim();
                        oVTimeOutCMD.HOSTSOURCE = item.HOSTSOURCE.Trim();
                        oVTimeOutCMD.HOSTDESTINATION = item.HOSTDESTINATION.Trim();
                        oVTimeOutCMD.OHTC_CMD_STATUS = Define.DefineInt_To_EOHTC_CMD_STATUS(item.OHTC_CMD_STATUS);
                        oVTimeOutCMD.MCS_INSERT_TIME = Convert.ToDateTime(item.MCS_CMD_INSER_TIME);
                        oVTimeOutCMD.MCS_START_TIME = Convert.ToDateTime(item.MCS_CMD_START_TIME);
                        oVTimeOutCMD.MCS_FINISH_TIME = Convert.ToDateTime(item.MCS_CMD_FINISH_TIME);
                        oVTimeOutCMD.AlarmsInCMD = alarmConverter.GetVALARMs(lsAlarm.Where(info =>
                       !(((oVTimeOutCMD.MCS_START_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_START_TIME < info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVTimeOutCMD.MCS_FINISH_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_FINISH_TIME < info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd))) ||
                        ((oVTimeOutCMD.MCS_START_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_START_TIME > info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVTimeOutCMD.MCS_FINISH_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_FINISH_TIME > info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd))))
                        ).ToList());
                        lsReturn.Add(oVTimeOutCMD);
                    }
                }
                return lsReturn;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return lsReturn;
            }

        }

        public List<ViewerObject.REPORT.VErrorCMD_OHTC> LoadAlarmsByErrorOHTCCmdTime(DateTime startDatetime, DateTime endDatetime, string Alarm_Level = null, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null)
                             => loadalarmsbyerrorohtccmdtime(startDatetime, endDatetime, CheckAlarmLevel(Alarm_Level), TimeIntervalAdd, alarmMap, alarmModule);
        private List<ViewerObject.REPORT.VErrorCMD_OHTC> loadalarmsbyerrorohtccmdtime(DateTime startDatetime, DateTime endDatetime, E_ALARM_LVL Alarm_Level = E_ALARM_LVL.None, int TimeIntervalAdd = 0, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null)
        {
            List<ViewerObject.REPORT.VErrorCMD_OHTC> lsReturn = new List<ViewerObject.REPORT.VErrorCMD_OHTC>();
            List<ALARM> lsAlarm = new List<ALARM>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;



                    var Alarmquery = from cmd in con.ALARM.AsNoTracking()
                                     where (Alarm_Level == E_ALARM_LVL.None || cmd.ALAM_LVL == Alarm_Level) &&
                                     (cmd.RPT_DATE_TIME < endDatetime && cmd.RPT_DATE_TIME >= startDatetime) &&
                                     cmd.CLEAR_DATE_TIME != null
                                     select cmd;
                    lsAlarm = Alarmquery?.ToList();


                    var CMDquery = from sbTable in con.HCMD_OHTC.AsNoTracking()
                                   where (sbTable.CMD_END_TIME < endDatetime && sbTable.CMD_START_TIME >= startDatetime)
                                         &&
                                         (sbTable.COMPLETE_STATUS == CompleteStatus.CmpStatusVehicleAbort) &&
                                         sbTable.VH_ID != null &&
                                         sbTable.CMD_END_TIME != null
                                   select sbTable;

                    foreach (var item in CMDquery?.ToList())
                    {

                        ViewerObject.REPORT.VErrorCMD_OHTC oVErrorCMD = new ViewerObject.REPORT.VErrorCMD_OHTC();
                        oVErrorCMD.ID = item.CMD_ID.Trim();
                        oVErrorCMD.MCS_CMD_ID = item.CMD_ID_MCS.Trim();
                        oVErrorCMD.VH_ID = item.VH_ID.Trim();
                        oVErrorCMD.CARRIER_ID = item.CARRIER_ID.Trim();
                        oVErrorCMD.HOSTSOURCE = item.SOURCE.Trim();
                        oVErrorCMD.HOSTDESTINATION = item.DESTINATION.Trim();
                        oVErrorCMD.OHTC_CMD_STATUS = Define.DefineInt_To_EOHTC_CMD_STATUS((int)item.COMPLETE_STATUS);
                        oVErrorCMD.START_TIME = Convert.ToDateTime(item.CMD_START_TIME);
                        oVErrorCMD.FINISH_TIME = Convert.ToDateTime(item.CMD_END_TIME);
                        oVErrorCMD.CARRIER_TYPE = "";
                        oVErrorCMD.AlarmsInCMD = alarmConverter.GetVALARMs(lsAlarm.Where(info => info.EQPT_ID.Trim() == item.VH_ID.Trim() && (
                        !(((oVErrorCMD.START_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.START_TIME < info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.FINISH_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.FINISH_TIME < info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd))) ||
                        ((oVErrorCMD.START_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.START_TIME > info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.FINISH_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.FINISH_TIME > info.CLEAR_DATE_TIME.Value.AddMinutes(TimeIntervalAdd))))
                        )
                        ).ToList());
                        lsReturn.Add(oVErrorCMD);
                    }
                }
                return lsReturn;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return lsReturn;
            }

        }



        public void GetAlarmLimitDateTime(out DateTime StartTime, out DateTime EndTime) => getalarmlimitdatetime(out StartTime, out EndTime);
        private void getalarmlimitdatetime(out DateTime StartTime, out DateTime EndTime)
        {
            //如果執行效能不好，請確認是否建立Index
            /*
            DemoSQL: 
            select TOP 1 * from ALARM order by END_TIME asc
            select TOP 1 * from ALARM order by END_TIME desc
            */
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
            try
            {

                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = (from sbTable in con.ALARM.AsNoTracking()
                                 where sbTable.CLEAR_DATE_TIME != null
                                 select sbTable.CLEAR_DATE_TIME).Min();

                    StartTime = query ?? DateTime.MinValue;



                    var query1 = (from sbTable in con.ALARM.AsNoTracking()
                                  where sbTable.CLEAR_DATE_TIME != null
                                  select sbTable.CLEAR_DATE_TIME).Max();
                    EndTime = query1 ?? DateTime.MaxValue;


                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public bool AlarmUpdate(ControlRequest request, out string result)
        {
            result = "sorry, we don't have this job";
            return false;
        }
        #endregion
    }

    public class AlarmConverter
    {
        public ViewerObject.VALARM GetVALARM(ALARM input)
        {
            if (new Alarm().SetVALARM(input, out ViewerObject.VALARM output, out string result))
                return output;
            else
                return null;
        }
        public List<ViewerObject.VALARM> GetVALARMs(List<ALARM> input)
        {
            List<ViewerObject.VALARM> output = new List<ViewerObject.VALARM>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVALARM(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }

        public ALARM GetALARM(ViewerObject.VALARM input)
        {
            ALARM output = null;
            if (input != null)
            {
                output = new ALARM()
                {
                    RPT_DATE_TIME = Convert.ToDateTime(input.RPT_DATE_TIME),
                    EQPT_ID = input.EQPT_ID,
                    ALAM_LVL = GetE_ALARM_LVL(input.ALARM_LVL),
                    ALAM_CODE = input.ALARM_CODE,
                    ALAM_STAT = input.IS_CLEARED ? ErrorStatus.ErrSet : ErrorStatus.ErrReset,
                    CLEAR_DATE_TIME =  null
                };
                if (!string.IsNullOrWhiteSpace(input.CLEAR_DATE_TIME))
                    output.CLEAR_DATE_TIME = Convert.ToDateTime(input.CLEAR_DATE_TIME);
            }
            return output;
        }
        public E_ALARM_LVL GetE_ALARM_LVL(ViewerObject.VALARM_Def.AlarmLvl alarmLVL)
        {
            switch (alarmLVL)
            {
                case ViewerObject.VALARM_Def.AlarmLvl.Error:
                    return E_ALARM_LVL.Error;
                case ViewerObject.VALARM_Def.AlarmLvl.Warn:
                    return E_ALARM_LVL.Warn;
                case ViewerObject.VALARM_Def.AlarmLvl.None:
                default:
                    return E_ALARM_LVL.None;
            }
        }
        bool AlarmUpdate(ControlRequest request, out string result)
        {
            result = "Sorry, We Don't do anthing";
            return false;
        }
    }
}
