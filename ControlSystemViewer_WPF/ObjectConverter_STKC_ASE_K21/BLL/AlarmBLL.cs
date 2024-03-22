using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject.REPORT;
using CommonMessage.ProtocolFormat.AlarmFun;

namespace ObjectConverter_STKC_ASE_K21.BLL
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
        private List<AlarmViewExtension> loadSetAlarms()
        {
            List<AlarmViewExtension> alarms = new List<AlarmViewExtension>();
            //using (DBConnection_EF con = DBConnection_EF.GetUContext())
            //{
            //    if (!string.IsNullOrWhiteSpace(connectionString))
            //        con.Database.Connection.ConnectionString = connectionString;

            //    var query = from a in con.ALARM.AsNoTracking()
            //                where a.ALAM_STAT == ErrorStatus.ErrSet
            //                select a;
            //    alarms.AddRange(query?.ToList() ?? new List<ALARM>());
            //}
            return alarms;
        }

        public List<ViewerObject.VALARM> GetAlarmsByDate(DateTime date) => alarmConverter.GetVALARMs(getAlarmsByDate(date));
        private List<AlarmViewExtension> getAlarmsByDate(DateTime date)
        {
            int iDate = Convert.ToInt32(date.Date.ToString("yyyyMMddHHmmssfffff"));
            int iDate_NextDay = Convert.ToInt32(date.Date.AddDays(1).ToString("yyyyMMddHHmmssfffff"));
            List<AlarmViewExtension> alarms = new List<AlarmViewExtension>();
            //using (DBConnection_EF con = DBConnection_EF.GetUContext())
            //{
            //    if (!string.IsNullOrWhiteSpace(connectionString))
            //        con.Database.Connection.ConnectionString = connectionString;

            //    var query = from cmd in con.ALARM.AsNoTracking()
            //                where cmd.ALAM_LVL == E_ALARM_LVL.Error && Convert.ToInt32(cmd.RPT_DATE_TIME) < iDate_NextDay
            //                   && (cmd.END_TIME == null || Convert.ToInt32(cmd.END_TIME) >= iDate)
            //                select cmd;
            //    alarms.AddRange(query?.OrderBy(c => c.RPT_DATE_TIME).ToList() ?? new List<ALARM>());
            //}
            return alarms;
        }
        public static readonly string DateTimeFormat_23 = "yyyy-MM-dd HH:mm:ss.fff";

        public List<ViewerObject.VALARM> GetAlarmsByConditions(DateTime startDatetime, DateTime endDatetime, string eqptID = null, string alarmCode = null, string Alarm_Level = null, bool cleartimenotnull = false, List<ViewerObject.AlarmMap> alarmMap = null, List<ViewerObject.AlarmModule> alarmModule = null)
                                         => alarmConverter.GetVALARMs(getAlarmsByConditions(startDatetime, endDatetime, eqptID, alarmCode, Alarm_Level), alarmModule, alarmMap);
        private List<AlarmViewExtension> getAlarmsByConditions(DateTime startDatetime, DateTime endDatetime, string eqptID = null, string alarmCode = null, string Alarm_Level = null, bool cleartimenotnull = false)
        {
            //            E_ALARM_LVL AlarmLVL = CheckAlarmLevel(Alarm_Level);
            //List<ALARM> alarms = new List<ALARM>();
            //using (DBConnection_EF con = DBConnection_EF.GetUContext())
            //{
            //    if (!string.IsNullOrWhiteSpace(connectionString))
            //        con.Database.Connection.ConnectionString = connectionString;

            //    var query = from a in con.ALARM.AsNoTracking()
            //                where a.RPT_DATE_TIME >= startDatetime && a.RPT_DATE_TIME < endDatetime && (Alarm_Level == null || (a.ALAM_LVL == AlarmLVL && Alarm_Level != null)) && ((cleartimenotnull == false) || (cleartimenotnull == true && a.END_TIME != null)) && ((eqptID == null) || (a.EQPT_ID.Trim() == eqptID.Trim() && eqptID != null))
            //                select a;
            //    var list = query?.ToList() ?? new List<ALARM>();
            //    alarms.AddRange(list);
            //}
            //return alarms;
            int? ialarm_lvl = null;
            if (Alarm_Level != null && Alarm_Level == "2")
                ialarm_lvl = 0;

            List<AlarmViewExtension> result = new List<AlarmViewExtension>();
            using (var dbCon = new B7_STK01Entities())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    dbCon.Database.Connection.ConnectionString = connectionString;
                string start_date_time = startDatetime.ToString(DateTimeFormat_23);
                string end_date_time = endDatetime.ToString(DateTimeFormat_23);
                if(ialarm_lvl.HasValue)
                {
                    var temp = from a in dbCon.AlarmViewExtension
                               where a.RPT_DATE_TIME.CompareTo(start_date_time) > 0 &&
                                     a.RPT_DATE_TIME.CompareTo(end_date_time) < 0 &&
                                     a.ALAM_LVL == ialarm_lvl
                               select a;
                    var list = temp.ToList();
                    result.AddRange(list);
                }
                else
                {
                    var temp = from a in dbCon.AlarmViewExtension
                               where a.RPT_DATE_TIME.CompareTo(start_date_time) > 0 &&
                                     a.RPT_DATE_TIME.CompareTo(end_date_time) < 0
                               select a;
                    var list = temp.ToList();
                    result.AddRange(list);
                }
                //foreach (var alarm in list)
                //{
                //    var alarmConvert = new Alarm();
                //    ViewerObject.VALARM valarm = alarmConvert.SetVALARM(alarm);
                //    result.Add(valarm);
                //}
            }
            return result;

        }

        private bool CheckAlarmLevel(E_ALARM_LVL aLARM_LVL, string Alarm_Level)
        {
            if (Alarm_Level != null)
            {
                switch (Alarm_Level)
                {
                    case "0":
                        return aLARM_LVL == E_ALARM_LVL.None;
                    case "1":
                        return aLARM_LVL == E_ALARM_LVL.Warn;
                    case "2":
                        return aLARM_LVL == E_ALARM_LVL.Error;
                }
            }
            return true;
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
                                 where sbTable.END_TIME != null
                                 select sbTable.END_TIME).Min();

                    StartTime = query ?? DateTime.MinValue;



                    var query1 = (from sbTable in con.ALARM.AsNoTracking()
                                  where sbTable.END_TIME != null
                                  select sbTable.END_TIME).Max();
                    EndTime = query1 ?? DateTime.MaxValue;


                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
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
                                     cmd.END_TIME != null
                                     select cmd;
                    lsAlarm = Alarmquery?.ToList();


                    var CMDquery = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                   where (sbTable.MCS_CMD_FINISH_TIME < endDatetime && sbTable.MCS_CMD_START_TIME >= startDatetime)
                                         &&
                                         (sbTable.OHTC_COMPLETE_STATUS == 22) &&
                                         sbTable.VH_ID != null &&
                                         sbTable.MCS_CMD_FINISH_TIME != null
                                   select sbTable;

                    //foreach (var item in CMDquery?.ToList())
                    //{

                    //    ViewerObject.REPORT.VErrorCMD oVErrorCMD = new ViewerObject.REPORT.VErrorCMD();
                    //    oVErrorCMD.MCS_CMD_ID = item.CMD_ID_MCS.Trim();
                    //    oVErrorCMD.VH_ID = item.VH_ID.Trim();
                    //    oVErrorCMD.CARRIER_ID = item.CARRIER_ID.Trim();
                    //    oVErrorCMD.HOSTSOURCE = item.HOSTSOURCE.Trim();
                    //    oVErrorCMD.HOSTDESTINATION = item.HOSTDESTINATION.Trim();
                    //    oVErrorCMD.OHTC_CMD_STATUS = Define.DefineInt_To_EOHTC_CMD_STATUS(item.OHTC_CMD_STATUS);
                    //    oVErrorCMD.MCS_INSERT_TIME = item.MCS_CMD_INSER_TIME ?? DateTime.MinValue;
                    //    oVErrorCMD.MCS_START_TIME = Convert.ToDateTime(item.MCS_CMD_START_TIME);
                    //    oVErrorCMD.MCS_FINISH_TIME = Convert.ToDateTime(item.MCS_CMD_FINISH_TIME);
                    //    oVErrorCMD.AlarmsInCMD = alarmConverter.GetVALARMs(lsAlarm.Where(info => info.EQPT_ID.Trim() == item.VH_ID.Trim() && (
                    //    !(((oVErrorCMD.MCS_START_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_START_TIME < info.END_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.MCS_FINISH_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_FINISH_TIME < info.END_TIME.Value.AddMinutes(TimeIntervalAdd))) ||
                    //    ((oVErrorCMD.MCS_START_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_START_TIME > info.END_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.MCS_FINISH_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.MCS_FINISH_TIME > info.END_TIME.Value.AddMinutes(TimeIntervalAdd))))
                    //    )
                    //    ).ToList());
                    //    lsReturn.Add(oVErrorCMD);
                    //}
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
                                     cmd.END_TIME != null
                                     select cmd;
                    lsAlarm = Alarmquery?.ToList();


                    var CMDquery = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                   where (sbTable.MCS_CMD_FINISH_TIME < endDatetime && sbTable.MCS_CMD_START_TIME >= startDatetime)
                                         &&
                                         (DbFunctions.DiffMinutes(sbTable.MCS_CMD_START_TIME, sbTable.MCS_CMD_FINISH_TIME) >= OverTime_Min) &&
                                         sbTable.VH_ID != null &&
                                         sbTable.MCS_CMD_FINISH_TIME != null
                                   select sbTable;

                    //foreach (var item in CMDquery?.ToList())
                    //{

                    //    ViewerObject.REPORT.VTimeOutCMD oVTimeOutCMD = new ViewerObject.REPORT.VTimeOutCMD();
                    //    oVTimeOutCMD.MCS_CMD_ID = item.CMD_ID_MCS.Trim();
                    //    oVTimeOutCMD.VH_ID = item.VH_ID.Trim();
                    //    oVTimeOutCMD.CARRIER_ID = item.CARRIER_ID.Trim();
                    //    oVTimeOutCMD.HOSTSOURCE = item.HOSTSOURCE.Trim();
                    //    oVTimeOutCMD.HOSTDESTINATION = item.HOSTDESTINATION.Trim();
                    //    oVTimeOutCMD.OHTC_CMD_STATUS = Define.DefineInt_To_EOHTC_CMD_STATUS(item.OHTC_CMD_STATUS);
                    //    oVTimeOutCMD.MCS_INSERT_TIME = item.MCS_CMD_INSER_TIME ?? DateTime.MinValue;
                    //    oVTimeOutCMD.MCS_START_TIME = Convert.ToDateTime(item.MCS_CMD_START_TIME);
                    //    oVTimeOutCMD.MCS_FINISH_TIME = Convert.ToDateTime(item.MCS_CMD_FINISH_TIME);
                    //    oVTimeOutCMD.AlarmsInCMD = alarmConverter.GetVALARMs(lsAlarm.Where(info =>
                    //   !(((oVTimeOutCMD.MCS_START_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_START_TIME < info.END_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVTimeOutCMD.MCS_FINISH_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_FINISH_TIME < info.END_TIME.Value.AddMinutes(TimeIntervalAdd))) ||
                    //    ((oVTimeOutCMD.MCS_START_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_START_TIME > info.END_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVTimeOutCMD.MCS_FINISH_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVTimeOutCMD.MCS_FINISH_TIME > info.END_TIME.Value.AddMinutes(TimeIntervalAdd))))
                    //    ).ToList());
                    //    lsReturn.Add(oVTimeOutCMD);
                    //}
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
            //Type不是一般Alarm 先隱藏
            List<ViewerObject.REPORT.VErrorCMD_OHTC> lsReturn = new List<ViewerObject.REPORT.VErrorCMD_OHTC>();
            //List<ALARM> lsAlarm = new List<ALARM>();
            //try
            //{
            //    using (DBConnection_EF con = DBConnection_EF.GetUContext())
            //    {
            //        if (!string.IsNullOrWhiteSpace(connectionString))
            //            con.Database.Connection.ConnectionString = connectionString;



            //        var Alarmquery = from cmd in con.ALARM.AsNoTracking()
            //                         where (Alarm_Level == E_ALARM_LVL.None || cmd.ALAM_LVL == Alarm_Level) &&
            //                         (cmd.RPT_DATE_TIME < endDatetime && cmd.RPT_DATE_TIME >= startDatetime) &&
            //                         cmd.END_TIME != null
            //                         select cmd;
            //        lsAlarm = Alarmquery?.ToList();


            //        var CMDquery = from sbTable in con.HCMD_OHTC.AsNoTracking()
            //                       where (sbTable.CMD_END_TIME < endDatetime && sbTable.CMD_START_TIME >= startDatetime)
            //                             &&
            //                             (sbTable.COMPLETE_STATUS == CompleteStatus.CmpStatusVehicleAbort) &&
            //                             sbTable.VH_ID != null &&
            //                             sbTable.CMD_END_TIME != null
            //                       select sbTable;

            //        foreach (var item in CMDquery?.ToList())
            //        {

            //            ViewerObject.REPORT.VErrorCMD_OHTC oVErrorCMD = new ViewerObject.REPORT.VErrorCMD_OHTC();
            //            oVErrorCMD.MCS_CMD_ID = item.CMD_ID_MCS.Trim();
            //            oVErrorCMD.VH_ID = item.VH_ID.Trim();
            //            oVErrorCMD.CARRIER_ID = item.CARRIER_ID.Trim();
            //            oVErrorCMD.HOSTSOURCE = item.SOURCE.Trim();
            //            oVErrorCMD.HOSTDESTINATION = item.DESTINATION.Trim();
            //            oVErrorCMD.OHTC_CMD_STATUS = Define.DefineInt_To_EOHTC_CMD_STATUS((int)item.COMPLETE_STATUS);
            //            oVErrorCMD.START_TIME = Convert.ToDateTime(item.CMD_START_TIME);
            //            oVErrorCMD.FINISH_TIME = Convert.ToDateTime(item.CMD_END_TIME);
            //            oVErrorCMD.CARRIER_TYPE = "";
            //            oVErrorCMD.AlarmsInCMD = alarmConverter.GetVALARMs(lsAlarm.Where(info => info.EQPT_ID.Trim() == item.VH_ID.Trim() && (
            //            !(((oVErrorCMD.START_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.START_TIME < info.END_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.FINISH_TIME < info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.FINISH_TIME < info.END_TIME.Value.AddMinutes(TimeIntervalAdd))) ||
            //            ((oVErrorCMD.START_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.START_TIME > info.END_TIME.Value.AddMinutes(TimeIntervalAdd)) && (oVErrorCMD.FINISH_TIME > info.RPT_DATE_TIME.AddMinutes(0 - TimeIntervalAdd) && oVErrorCMD.FINISH_TIME > info.END_TIME.Value.AddMinutes(TimeIntervalAdd))))
            //            )
            //            ).ToList());
            //            lsReturn.Add(oVErrorCMD);
            //        }
            //    }
            //    return lsReturn;

            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Exception");
            //    return lsReturn;
            //}
            return lsReturn;

        }


        public bool AlarmUpdate(ControlRequest request, out string result)
        {
            try
            {

                string eq_id = request.EQPTID;
                DateTime userLastUpdateTime = DateTime.FromBinary(request.RPTDATETIME);//這邊是client告訴我們他最後更新的時間
                string s_time = userLastUpdateTime.ToString(DateTimeFormat_23);
                string error_code = request.ALARMCODE;
                string update_user = request.USERID;
                var update_classification = request.ALARMCLASSIFICATION;
                var alarm_module = request.ALARMMODULE;
                var alarm_importance_level = request.IMPORTANCELEVEL;
                string remark = request.ALARMREMARK;

                using (var dbCon = new B7_STK01Entities())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        dbCon.Database.Connection.ConnectionString = connectionString;

                    AlarmDataExtension alarm = getAlarm(dbCon, eq_id, error_code, s_time);
                    if (alarm == null)
                    {

                        alarm = new AlarmDataExtension();
                        alarm.STOCKERID = "";
                        alarm.EQID = eq_id;
                        alarm.ALARMSTS = 0;
                        alarm.ALARMCODE = error_code;
                        alarm.STRDT = s_time;

                        alarm.ALARM_MODULE = alarm_module;
                        alarm.CLASS = (decimal?)update_classification;
                        alarm.IMPORTANCE_LVL = alarm_importance_level;
                        alarm.REMARK = remark;
                        //dbCon.Entry(alarm);
                        dbCon.AlarmDataExtension.Add(alarm);
                    }
                    else
                    {
                        alarm.ALARM_MODULE = alarm_module;
                        alarm.CLASS = (decimal?)update_classification;
                        alarm.IMPORTANCE_LVL = alarm_importance_level;
                        alarm.REMARK = remark;
                    }
                    dbCon.SaveChanges();

                    result = "OK";
                    return true;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                result = "NG";
                return false;
            }
        }



        private AlarmDataExtension getAlarm(B7_STK01Entities conn, string eqID, string code, string rptDateTime)
        {
            var alarm = from b in conn.AlarmDataExtension
                        where b.ALARMCODE.Trim() == code.Trim() &&
                              b.EQID.Trim() == eqID.Trim() &&
                              b.STRDT.Trim() == rptDateTime.Trim()
                        select b;

            return alarm.FirstOrDefault();
        }

        #endregion
    }

    public class AlarmConverter
    {
        public ViewerObject.VALARM GetVALARM(AlarmViewExtension input)
        {
            if (new Alarm().SetVALARM(input, out ViewerObject.VALARM output, out string result))
                return output;
            else
                return null;
        }
        public List<ViewerObject.VALARM> GetVALARMs(List<AlarmViewExtension> input)
        {
            return GetVALARMs(input, null);
        }
        public List<ViewerObject.VALARM> GetVALARMs(List<AlarmViewExtension> input, List<ViewerObject.AlarmModule> alarmModule, List<ViewerObject.AlarmMap> alarmMap = null)
        {
            List<ViewerObject.VALARM> output = new List<ViewerObject.VALARM>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVALARM(i);

                    if (o != null)
                    {
                        trySetAlarmModuleInfo(o, alarmModule);
                        output.Add(o);
                    }
                }
            }
            return output;
        }
        private bool isNTB(string eqID)
        {
            switch (eqID)
            {
                case "B7_OHBLINE1_T01":
                case "B7_OHBLINE2_T01":
                case "B7_OHBLINE3_T01":
                case "B7_OHBLINE3_T02":
                    return true;
                default:
                    return false;
            }
        }
        private bool isOHCV_7(string eqID)
        {
            switch (eqID)
            {
                case "B7_OHBLOOP_T0A":
                    return true;
                default:
                    return false;
            }
        }
        private bool isOHCV_5(string eqID)
        {
            switch (eqID)
            {
                case "B7_OHBLINE2_T02":
                case "B7_OHBLINE2_T03":
                case "B7_OHBLOOP_T01":
                case "B7_OHBLOOP_T02":
                case "B7_OHBLOOP_T03":
                case "B7_OHBLOOP_T04":
                case "B7_OHBLOOP_T05":
                case "B7_OHBLOOP_T06":
                case "B7_OHBLOOP_T07":
                case "B7_OHBLOOP_T08":
                case "B7_OHBLOOP_T09":
                case "B7_OHBLOOP_T0B":
                case "B7_OHBLOOP_T0C":
                    return true;
                default:
                    return false;
            }
        }

        private void trySetAlarmModuleInfo(ViewerObject.VALARM o, List<ViewerObject.AlarmModule> alarmModule)
        {
            if (alarmModule == null) return;
            int i_alarm_module = o.ALARM_MODULE;
            var alarm_module = alarmModule.Where(m => m.Number == i_alarm_module).FirstOrDefault();
            if (alarm_module != null)
            {
                o.sALARM_MODULE = alarm_module.Module_TW;
            }
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
                    END_TIME = null
                };
                if (!string.IsNullOrWhiteSpace(input.CLEAR_DATE_TIME))
                    output.END_TIME = Convert.ToDateTime(input.CLEAR_DATE_TIME);
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
    }
}
