using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using NLog;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;

namespace ObjectConverter_OHBC_ASE_K21.BLL
{
    public class CmdBLL : ICmdBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private CmdConverter cmdConverter = new CmdConverter();
        SqlConnection conn = null;
        private string connectionString = "";
        public CmdBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        #region CMD
        public List<ViewerObject.VCMD> LoadUnfinishCmds() => cmdConverter.GetVCMDs(loadUnfinishCmds());
        private List<ACMD_OHTC> loadUnfinishCmds()
        {
            List<ACMD_OHTC> cmds = new List<ACMD_OHTC>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.ACMD_OHTC.AsNoTracking()
                            where cmd.CMD_STAUS < E_CMD_STATUS.NormalEnd
                            orderby cmd.CMD_START_TIME
                            select cmd;
                cmds.AddRange(query?.ToList() ?? new List<ACMD_OHTC>());
            }
            return cmds;
        }

        public ViewerObject.VCMD GetCmdByID(string id) => cmdConverter.GetVCMD(getCmdByID(id));
        private ACMD_OHTC getCmdByID(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            id = id.Trim();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.ACMD_OHTC.AsNoTracking()
                            where cmd.CMD_ID.Trim() == id
                            select cmd;
                return query?.SingleOrDefault();
            }
        }

        public ViewerObject.VCMD GetCmdByTransferID(string transfer_id) => cmdConverter.GetVCMD(getCmdByTransferID(transfer_id));
        private ACMD_OHTC getCmdByTransferID(string transfer_id)
        {
            if (string.IsNullOrWhiteSpace(transfer_id)) return null;
            transfer_id = transfer_id.Trim();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.ACMD_OHTC.AsNoTracking()
                            where cmd.CMD_ID_MCS.Trim() == transfer_id
                            select cmd;
                return query?.FirstOrDefault();
            }
        }

        public ViewerObject.VCMD GetExecuteCmdByVhID(string vh_id) => cmdConverter.GetVCMD(getExecuteCmdByVhID(vh_id));
        private ACMD_OHTC getExecuteCmdByVhID(string vh_id)
        {
            if (string.IsNullOrWhiteSpace(vh_id)) return null;
            vh_id = vh_id.Trim();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.ACMD_OHTC.AsNoTracking()
                            where cmd.VH_ID.Trim() == vh_id
                            && cmd.CMD_STAUS >= E_CMD_STATUS.Sending
                            && cmd.CMD_STAUS < E_CMD_STATUS.NormalEnd
                            select cmd;
                return query?.FirstOrDefault();
            }
        }

        public bool VhHasQueueCmd(string vh_id)
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.ACMD_OHTC.AsNoTracking()
                            where cmd.VH_ID == vh_id.Trim() &&
                            cmd.CMD_STAUS == E_CMD_STATUS.Queue
                            select cmd;
                int count = query?.Count() ?? 0;
                return count > 0;
            }
        }

        public List<ViewerObject.REPORT.VCMD_ExportDetail> GetVCMDDetail_Export(DateTime startDatetime, DateTime endDatetime)
                                                      => cmdConverter.GetVCMD_Details(getHCmdByConditions(startDatetime, endDatetime));

        public List<ViewerObject.VCMD> GetHCmdByConditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null)
                                              => cmdConverter.GetVCMDs(getHCmdByConditions(startDatetime, endDatetime, cst_id, vh_id));
        private List<HCMD_OHTC> getHCmdByConditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null)
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.HCMD_OHTC.AsNoTracking()
                            where (cmd.CMD_START_TIME > startDatetime && cmd.CMD_START_TIME < endDatetime)
                               || (cmd.CMD_START_TIME > endDatetime && cmd.CMD_START_TIME < startDatetime)
                            select cmd;
                return query?.ToList().Where(t =>
                {
                    bool b = true;
                    if (!string.IsNullOrEmpty(cst_id))
                    {
                        b = b & (t.CARRIER_ID.Contains(cst_id.Trim()));
                    }
                    if (!string.IsNullOrEmpty(vh_id))
                    {
                        b = b & (t.VH_ID.Contains(vh_id.Trim()));
                    }
                    return b;
                })?.ToList() ?? new List<HCMD_OHTC>();
            }
        }

        public List<ViewerObject.VCMD> GetHCmdByDate(DateTime date) => cmdConverter.GetVCMDs(getHCmdByDate(date));
        private List<HCMD_OHTC> getHCmdByDate(DateTime date)
        {
            DateTime date_NextDay = date.Date.AddDays(1);
            List<HCMD_OHTC> cmds = null;
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.HCMD_OHTC.AsNoTracking()
                            where cmd.CMD_START_TIME < date_NextDay &&
                                  cmd.CMD_END_TIME >= date.Date
                            select cmd;
                cmds = query?.OrderBy(c => c.CMD_START_TIME).ToList();
            }
            return cmds;
        }
        #endregion CMD

        #region TRANSFER
        public List<ViewerObject.VTRANSFER> LoadUnfinishedTransfers() => cmdConverter.GetVTRANSFERs(loadUnfinishedTransfers());
        private List<VACMD_MCS> loadUnfinishedTransfers()
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.VACMD_MCS.AsNoTracking()
                            where cmd.TRANSFERSTATE >= E_TRAN_STATUS.Queue && cmd.TRANSFERSTATE <= E_TRAN_STATUS.Aborting
                            //&& cmd.CHECKCODE.Trim() == SECSConst.HCACK_Confirm
                            select cmd;
                return query?.ToList() ?? new List<VACMD_MCS>();
            }
        }

        public ViewerObject.VTRANSFER GetTransferByID(string id) => cmdConverter.GetVTRANSFER(getTransferByID(id));
        private VACMD_MCS getTransferByID(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            id = id.Trim();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.VACMD_MCS.AsNoTracking()
                            where cmd.CMD_ID.Trim() == id
                            select cmd;
                return query?.SingleOrDefault();
            }
        }

        public int GetTransferMinPrioritySum()
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.VACMD_MCS.AsNoTracking()
                            where cmd.TRANSFERSTATE == E_TRAN_STATUS.Queue
                            orderby cmd.PRIORITY_SUM ascending
                            select cmd.PRIORITY_SUM;
                return query?.FirstOrDefault() ?? 0;
            }
        }
        public int GetTransferMaxPrioritySum()
        {
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.VACMD_MCS.AsNoTracking()
                            where cmd.TRANSFERSTATE == E_TRAN_STATUS.Queue
                            orderby cmd.PRIORITY_SUM descending
                            select cmd.PRIORITY_SUM;
                return query?.FirstOrDefault() ?? 0;
            }
        }

        public List<ViewerObject.VTRANSFER> GetHTransferByConditions(DateTime startDatetime, DateTime endDatetime)
        {
            List<ViewerObject.VTRANSFER> result = new List<ViewerObject.VTRANSFER>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from tsf in con.HCMD_MCS.AsNoTracking()
                            join cmd in con.HCMD_OHTC.AsNoTracking() on tsf.CMD_ID equals cmd.CMD_ID_MCS
                            where cmd.CMD_START_TIME > startDatetime
                               && ((tsf.CMD_INSER_TIME > startDatetime && tsf.CMD_INSER_TIME < endDatetime)
                               || (tsf.CMD_INSER_TIME > endDatetime && tsf.CMD_INSER_TIME < startDatetime))
                            orderby tsf.CMD_INSER_TIME
                            select new ViewerObject.VTRANSFER
                            {
                                TRANSFER_ID = tsf.CMD_ID.Trim(),
                                CARRIER_ID = tsf.CARRIER_ID.Trim(),
                                BOX_ID = tsf.BOX_ID.Trim(),
                                LOT_ID = tsf.LOT_ID.Trim(),
                                HOSTSOURCE = tsf.HOSTSOURCE.Trim(),
                                HOSTDESTINATION = tsf.HOSTDESTINATION.Trim(),
                                PRIORITY = tsf.PRIORITY,
                                PORT_PRIORITY = tsf.PORT_PRIORITY,
                                TIME_PRIORITY = tsf.TIME_PRIORITY,
                                INSERT_TIME = tsf.CMD_INSER_TIME,
                                ASSIGN_TIME = tsf.CMD_START_TIME,
                                FINISH_TIME = tsf.CMD_FINISH_TIME,
                                TRANSFER_STATUS = tsf.TRANSFERSTATE == E_TRAN_STATUS.Queue ? ViewerObject.VTRANSFER_Def.TransferStatus.Queue :
                                                  tsf.TRANSFERSTATE == E_TRAN_STATUS.Transferring ? ViewerObject.VTRANSFER_Def.TransferStatus.Transferring :
                                                  tsf.TRANSFERSTATE == E_TRAN_STATUS.Paused ? ViewerObject.VTRANSFER_Def.TransferStatus.Paused :
                                                  tsf.TRANSFERSTATE == E_TRAN_STATUS.Canceling ? ViewerObject.VTRANSFER_Def.TransferStatus.Canceling :
                                                  tsf.TRANSFERSTATE == E_TRAN_STATUS.Aborting ? ViewerObject.VTRANSFER_Def.TransferStatus.Aborting :
                                                  tsf.TRANSFERSTATE == E_TRAN_STATUS.TransferCompleted ? ViewerObject.VTRANSFER_Def.TransferStatus.Complete :
                                                                                                         ViewerObject.VTRANSFER_Def.TransferStatus.Undefined,
                                CMD_ID = cmd.CMD_ID.Trim(),
                                VH_ID = cmd.VH_ID.Trim(),
                            };
                result = query?.ToList() ?? new List<ViewerObject.VTRANSFER>();
            }
            return result;
        }
        public int GetHTransferHourlyCount()
        {
            int result = 0;
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                DateTime dtS = DateTime.Now.AddHours(-1);
                var query = from cmd in con.HCMD_MCS
                            where cmd.CMD_FINISH_TIME != null
                            && cmd.CMD_FINISH_TIME > dtS && cmd.CMD_FINISH_TIME <= DateTime.Now
                            select cmd;
                result = query?.Count() ?? 0;
            }
            return result;
        }
        public int GetHTransferTodayCount()
        {
            int result = 0;
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from cmd in con.HCMD_MCS
                            where cmd.CMD_FINISH_TIME != null
                            && cmd.CMD_FINISH_TIME >= DateTime.Today && cmd.CMD_FINISH_TIME < DateTime.Now
                            select cmd;
                result = query?.Count() ?? 0;
            }
            return result;
        }
        #endregion TRANSFER

        #region Report
        public List<ViewerObject.VCMD_OHTC> GetHCMDOHTCByConditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null)
                                             => gethcmdohtcbyconditions(startDatetime, endDatetime, cst_id, vh_id);
        private List<VCMD_OHTC> gethcmdohtcbyconditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null)
        {
            List<VCMD_OHTC> lsRT = new List<VCMD_OHTC>();
            try
            {
                Definition.Convert defCvt = new Definition.Convert();
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                where (sbTable.MCS_CMD_START_TIME >= startDatetime && sbTable.MCS_CMD_START_TIME < endDatetime)
                                &&
                                 (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                                   sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                                   sbTable.OHTC_COMPLETE_STATUS == 26 || //emptyretrival
                                                   sbTable.OHTC_COMPLETE_STATUS == 27 || //doublestorage
                                                   sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                                   sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                                   sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan    
                                                   sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                                   ) &&
                                ((sbTable.VH_ID != null) && (vh_id == null) || (vh_id != null && sbTable.VH_ID == vh_id))

                                select sbTable;

                    foreach (var item in query?.ToList())
                    {

                        VCMD_OHTC oVCMD_OHTC = new VCMD_OHTC()
                        {
                            CMD_ID_MCS = item.CMD_ID_MCS,
                            VH_ID = item.VH_ID,
                            CARRIER_ID = item.CARRIER_ID,
                            HOSTSOURCE = item.HOSTSOURCE,
                            HOSTDESTINATION = item.HOSTDESTINATION,
                            MCS_CMD_INSERTTIME = item.MCS_CMD_INSER_TIME.Value,
                            MCS_CMD_START_TIME = item.MCS_CMD_START_TIME,
                            MCS_CMD_FINISH_TIME = item.MCS_CMD_FINISH_TIME,
                            OHTC_CMD_TYPE = defCvt.GetCmdTypeInt(item.OHTC_CMD_TYPE),
                            OHTC_CMD_STATUS = defCvt.GetCmdStatusInt(item.OHTC_CMD_STATUS),
                            MCS_COMMANDSTATE = defCvt.GetCommandStateInt(item.MCS_COMMANDSTATE ?? -1),
                            OHTC_CMD_START_TIME = item.OHTC_CMD_START_TIME,
                            OHTC_CMD_END_TIME = item.OHTC_CMD_END_TIME,
                            OHTC_COMPLETE_STATUS = defCvt.GetOHTCCompleteStatusInt(item.OHTC_COMPLETE_STATUS ?? -1)
                        };

                        lsRT.Add(oVCMD_OHTC);

                    }
                    return lsRT;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return lsRT;
            }
            finally
            {

                if (conn != null) conn.Close();
            }

        }

        public List<ViewerObject.VMCBF> LoadMTTRHCmd(DateTime StartTime, DateTime FinishTime) => loadCountMTTRHCmd(StartTime, FinishTime);
        private List<ViewerObject.VMCBF> loadCountMTTRHCmd(DateTime StartTime, DateTime FinishTime)
        {
            List<ViewerObject.VMCBF> lsReturn = new List<ViewerObject.VMCBF>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var subquery = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                   where (sbTable.MCS_CMD_START_TIME < FinishTime && sbTable.MCS_CMD_START_TIME >= StartTime)
                                         &&
                                         (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                               sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                               sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan    
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                               ) &&
                                         sbTable.VH_ID != null
                                   select new
                                   {
                                       VH_ID = sbTable.VH_ID,
                                       COMMANDSTATE = sbTable.MCS_COMMANDSTATE,
                                       CMD_STATUS = sbTable.OHTC_CMD_STATUS,
                                       CMD_TYPE = sbTable.OHTC_CMD_TYPE,
                                       OHTC_CMPSTATUS = sbTable.OHTC_COMPLETE_STATUS
                                   };

                    var query = from cmd in subquery
                                group cmd by new { VH_ID = cmd.VH_ID } into g
                                select new
                                {
                                    VH_ID = g.Key.VH_ID,
                                    CMDCount = g.Count(),
                                    EQPTAbortErrorCount = g.Sum(i => (i.OHTC_CMPSTATUS == (int)CompleteStatus.CmpStatusVehicleAbort) ? (Int32)1 : 0),
                                };


                    foreach (var item in query?.ToList())
                    {

                        ViewerObject.VMCBF oVMCBF = new ViewerObject.VMCBF(item.VH_ID.ToString());
                        oVMCBF.CMDCount = item.CMDCount;
                        oVMCBF.AlarmCount = item.EQPTAbortErrorCount;
                        lsReturn.Add(oVMCBF);

                    }
                }


                return lsReturn;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return lsReturn;
            }
            finally
            {

                if (conn != null) conn.Close();
            }
        }

        public List<ViewerObject.REPORT.VTransExecRate> LoadTransExecRate(DateTime StartTime, DateTime FinishTime, out int VHNumbers) => loadtransexecrate(StartTime, FinishTime, out VHNumbers);
        private List<ViewerObject.REPORT.VTransExecRate> loadtransexecrate(DateTime StartTime, DateTime FinishTime, out int VHNumbers)
        {
            //如果執行效能不好，請確認是否建立Index
            // create INDEX IHCMD_MCS ON HCMD_MCS(HOSTSOURCE,HOSTDESTINATION,CMD_INSER_TIME,CMD_START_TIME,CMD_FINISH_TIME,COMMANDSTATE);

            /*
            DemoSQL: 
            select HOSTSOURCE,HOSTDESTINATION,
            sum(1)as EntryCount,
            SUM(case when  COMMANDSTATE=128 then 1 else 0 end) as Completed,
            SUM(case when  COMMANDSTATE=512 then 1 else 0 end) as LoadErrorCount,
            SUM(case when  COMMANDSTATE=256 then 1 else 0 end) as UnLoadErrorCount,
            SUM(case when  COMMANDSTATE in(1024,2048) then 1 else 0 end) as CancelAbortErrorCount,
            SUM(DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME))/60 as LeadTime,
            MAX(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME) else 0 end ) as MaxLeadTime,
            MIN(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME) else 99999 end ) as MinLeadTime,
            AVG(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME) else 0 end ) as AVGLeadTime,  --AVG因誤差問題在Code改用128的sum / Completed
            SUM(DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME))/60 as CycleTime,
            MAX(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME) else 0 end ) as MaxCycleTime,
            MIN(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME) else 99999 end ) as MinCycleTime,
            AVG(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME) else 0 end ) as AVGCycleTime  --AVG因誤差問題在Code改用128的sum / Completed
            from (Select HOSTSOURCE,HOSTDESTINATION,CMD_INSER_TIME,CMD_START_TIME,CMD_FINISH_TIME,COMMANDSTATE from HCMD_MCS Where COMMANDSTATE in(128,256,512,1024,2048)) As SubTable
            group by HOSTSOURCE,HOSTDESTINATION;
            */
            VHNumbers = 0;
            List<ViewerObject.REPORT.VTransExecRate> transExecRates = new List<ViewerObject.REPORT.VTransExecRate>();
            Dictionary<string, int> MSTTotalCount = new Dictionary<string, int>();
            try
            {


                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    //(from sbTable in con.VTransExcRate.AsNoTracking()
                    // where sbTable.CMD_FINISH_TIME < FinishTime &&
                    //       sbTable.CMD_START_TIME >= StartTime
                    // select sbTable
                    //                        )
                    var queryNumbers = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                       join sbShelfDefFrom in con.ShelfDef.AsNoTracking() on sbTable.HOSTSOURCE equals sbShelfDefFrom.ShelfID into ShelfDefFrom
                                       join sbShelfDefTo in con.ShelfDef.AsNoTracking() on sbTable.HOSTDESTINATION equals sbShelfDefTo.ShelfID into sbShelfDefTo
                                       from ShelfDefFromi in ShelfDefFrom.DefaultIfEmpty()
                                       from sbShelfDefToi in sbShelfDefTo.DefaultIfEmpty()
                                       where ((sbTable.MCS_CMD_INSER_TIME < FinishTime && sbTable.MCS_CMD_INSER_TIME >= StartTime) || (sbTable.MCS_CMD_START_TIME < FinishTime && sbTable.MCS_CMD_START_TIME >= StartTime))
                                               &&
                                         (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                               sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                               sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan    
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                               )
                                               && sbTable.VH_ID != null
                                       group sbTable by new { VH_ID = sbTable.VH_ID } into g
                                       select new
                                       {
                                           VHIDNum = g.Key
                                       };

                    VHNumbers = queryNumbers?.ToList().Count() ?? 0;


                    //計算MCS總共來了幾筆
                    var query1 = from cmd in (
                                      from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                      join sbShelfDefFrom in con.ShelfDef.AsNoTracking() on sbTable.HOSTSOURCE equals sbShelfDefFrom.ShelfID into ShelfDefFrom
                                      join sbShelfDefTo in con.ShelfDef.AsNoTracking() on sbTable.HOSTDESTINATION equals sbShelfDefTo.ShelfID into sbShelfDefTo
                                      from ShelfDefFromi in ShelfDefFrom.DefaultIfEmpty()
                                      from sbShelfDefToi in sbShelfDefTo.DefaultIfEmpty()
                                      where (sbTable.MCS_CMD_INSER_TIME < FinishTime && sbTable.MCS_CMD_INSER_TIME >= StartTime)
                                                     &&
                                                      (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                                      sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                                      sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                                      sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                                      sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                                      sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                                      sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan   
                                                      sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                                      )
                                                     && sbTable.VH_ID != null
                                      select new
                                      {
                                          HOSTSOURCE = ShelfDefFromi.ZoneID ?? (sbTable.HOSTSOURCE.Contains("_P") ? sbTable.HOSTSOURCE.Substring(0, sbTable.HOSTSOURCE.Length - 4) : sbTable.HOSTSOURCE),//如果ZoneID不是Null,代表這個Port是Shelf，改秀ZoneID
                                          HOSTDESTINATION = sbShelfDefToi.ZoneID ?? (sbTable.HOSTDESTINATION.Contains("_P") ? sbTable.HOSTDESTINATION.Substring(0, sbTable.HOSTDESTINATION.Length - 4) : sbTable.HOSTDESTINATION),//如果ZoneID不是Null,代表這個Port是Shelf，改秀ZoneID
                                          MCS_CMD_INSER_TIME = sbTable.MCS_CMD_INSER_TIME
                                      }
                                           )
                                 group cmd by new
                                 {
                                     HostSource = cmd.HOSTSOURCE,
                                     HostDestination = cmd.HOSTDESTINATION,

                                 } into g
                                 select new
                                 {
                                     HostSource = g.Key.HostSource.ToString(),
                                     HostDestination = g.Key.HostDestination.ToString(),
                                     MCSTotalCount = g.Sum(i => (i.MCS_CMD_INSER_TIME < FinishTime && i.MCS_CMD_INSER_TIME >= StartTime) ? (Int32)1 : 0),
                                 };

                    foreach (var i in query1?.ToList())
                    {
                        MSTTotalCount.Add(i.HostSource + "-" + i.HostDestination, i.MCSTotalCount);
                    }



                    var query = from cmd in (
                                                  from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                                  join sbShelfDefFrom in con.ShelfDef.AsNoTracking() on sbTable.HOSTSOURCE equals sbShelfDefFrom.ShelfID into ShelfDefFrom
                                                  join sbShelfDefTo in con.ShelfDef.AsNoTracking() on sbTable.HOSTDESTINATION equals sbShelfDefTo.ShelfID into sbShelfDefTo
                                                  from ShelfDefFromi in ShelfDefFrom.DefaultIfEmpty()
                                                  from sbShelfDefToi in sbShelfDefTo.DefaultIfEmpty()
                                                  where (sbTable.MCS_CMD_START_TIME < FinishTime && sbTable.MCS_CMD_START_TIME >= StartTime)
                                                  &&
                                         (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                               sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                               sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan    
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                               )
                                                   && sbTable.VH_ID != null
                                                  select new
                                                  {
                                                      HOSTSOURCE = ShelfDefFromi.ZoneID ?? (sbTable.HOSTSOURCE.Contains("_P") ? sbTable.HOSTSOURCE.Substring(0, sbTable.HOSTSOURCE.Length - 4) : sbTable.HOSTSOURCE),//如果ZoneID不是Null,代表這個Port是Shelf，改秀ZoneID
                                                      HOSTDESTINATION = sbShelfDefToi.ZoneID ?? (sbTable.HOSTDESTINATION.Contains("_P") ? sbTable.HOSTDESTINATION.Substring(0, sbTable.HOSTDESTINATION.Length - 4) : sbTable.HOSTDESTINATION),//如果ZoneID不是Null,代表這個Port是Shelf，改秀ZoneID
                                                      MCS_CMD_INSER_TIME = sbTable.MCS_CMD_INSER_TIME,
                                                      MCS_CMD_START_TIME = sbTable.MCS_CMD_START_TIME,
                                                      OHTC_COMPLETE_STATUS = sbTable.OHTC_COMPLETE_STATUS,
                                                      MCS_CMD_FINISH_TIME = sbTable.MCS_CMD_FINISH_TIME,
                                                      OHTC_CMD_TYPE = sbTable.OHTC_CMD_TYPE,
                                                      COMMANDSTATE=sbTable.MCS_COMMANDSTATE
                                                  }
                                            )
                                group cmd by new { HOSTSOURCE = cmd.HOSTSOURCE, HOSTDESTINATION = cmd.HOSTDESTINATION } into g
                                select new
                                {
                                    HostSource = g.Key.HOSTSOURCE,
                                    HostDestination = g.Key.HOSTDESTINATION,
                                    //MCSTotalCount = g.Sum(i => 1),
                                    EntryCount = g.Sum(i => (i.MCS_CMD_START_TIME < FinishTime && i.MCS_CMD_START_TIME >= StartTime) ? (Int32)1 : 0),
                                    CompletedCount = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (Int32)1 : 0),
                                    EmptyRetrieval = g.Sum(i => i.OHTC_COMPLETE_STATUS == 27 ? (Int32)1 : 0),
                                    DoubleStorage = g.Sum(i => i.OHTC_COMPLETE_STATUS == 26 ? (Int32)1 : 0),
                                    InterLockCount = g.Sum(i => i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ? (Int32)1 : 0),
                                    EQPTAbortErrorCount = g.Sum(i => i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ? (Int32)1 : 0),
                                    ScanCount = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ? (Int32)1 : 0)),
                                    BarcodeReadFail = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed) ? (Int32)1 : 0)),

                                    LeadTime128 = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_INSER_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    LeadTime128_5 = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_INSER_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,

                                    TransExecTime = g.Sum(i => (DbFunctions.DiffMinutes(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME))) ?? 0,
                                    TransExecTime128 = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    TransExecTime128_5 = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    MaxTransExecTime = (int)(g.Max(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0),
                                    MinTransExecTime = (int?)(g.Min(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : null) ?? null),

                                    LoadingInterLock = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError) && ((i.COMMANDSTATE & 4) == 4) && ((i.COMMANDSTATE & 32) != 32)) ? (int)1 : 0),
                                    UnLoadingInterLock = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError) && ((i.COMMANDSTATE & 32) == 32)) ? (int)1 : 0)
                                };

                    int iMSTTotalCount = 0;
                    foreach (var item in query?.ToList())
                    {
                        if (MSTTotalCount.Keys.Contains(item.HostSource + "-" + item.HostDestination))
                        {
                            iMSTTotalCount = MSTTotalCount[item.HostSource + "-" + item.HostDestination];
                        }
                        else
                        {
                            iMSTTotalCount = 0;
                        }
                        transExecRates.Add(new ViewerObject.REPORT.VTransExecRate(
                                            _HostSource: item.HostSource,
                                            _HostDestination: item.HostDestination,
                                            _MCSTotalCount: iMSTTotalCount, _EntryCount: item.EntryCount, _CompletedCount: item.CompletedCount, _EmptyRetrievalErrorCount: item.EmptyRetrieval, _DoubleStorageErrorCount: item.DoubleStorage, _InterLockCount: item.InterLockCount, _EQPTAbortErrorCount: item.EQPTAbortErrorCount,
                                            _BarcodeReadFailCount: item.BarcodeReadFail, _ScanCount: item.ScanCount,
                                             _LoadingInterLock: item.LoadingInterLock, _UnLoadingInterLock: item.UnLoadingInterLock,
                                            _AvgLeadTime: (item.CompletedCount != 0) ? Math.Round((double)(item.LeadTime128_5 / item.CompletedCount), 2) : 0,
                                            _CycleTime: item.TransExecTime, _AvgCycleTime: (item.CompletedCount != 0) ? Math.Round((double)(item.TransExecTime128_5 / item.CompletedCount), 2) : 0, _MaxCycleTime: item.MaxTransExecTime, _MinCycleTime: item.MinTransExecTime,
                                            StartTime: StartTime, EndTime: FinishTime, _TransTime128: item.LeadTime128_5, _TransExecTime128: item.TransExecTime128_5
                                            )
                                          );
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                VHNumbers = -1;
                return null;
            }

            return transExecRates;
        }

        public List<ViewerObject.REPORT.VTransExecRateByVHID> LoadTransExecRateGroupByEQPT(DateTime StartTime, DateTime FinishTime) => loadtransexecrategroupbyeqpt(StartTime, FinishTime);
        private List<ViewerObject.REPORT.VTransExecRateByVHID> loadtransexecrategroupbyeqpt(DateTime StartTime, DateTime FinishTime)
        {
            //如果執行效能不好，請確認是否建立Index
            // create INDEX IHCMD_MCS ON HCMD_MCS(HOSTSOURCE,HOSTDESTINATION,CMD_INSER_TIME,CMD_START_TIME,CMD_FINISH_TIME,COMMANDSTATE);

            /*
            DemoSQL:           
            select SubOHTC.VH_ID,
            sum(1)as EntryCount,
            SUM(case when  SubTable.COMMANDSTATE=128 then 1 else 0 end) as Completed,
            SUM(case when  SubTable.COMMANDSTATE=512 then 1 else 0 end) as LoadErrorCount,
            SUM(case when  SubTable.COMMANDSTATE=256 then 1 else 0 end) as UnLoadErrorCount,
            SUM(case when  SubTable.COMMANDSTATE in(1024,2048) then 1 else 0 end) as CancelAbortErrorCount,
            SUM(DATEDIFF(MINUTE,SubTable.CMD_INSER_TIME,SubTable.CMD_FINISH_TIME)) as LeadTime,
            MAX(case when  SubTable.COMMANDSTATE=128 then DATEDIFF(SECOND,SubTable.CMD_INSER_TIME,SubTable.CMD_FINISH_TIME) else 0 end ) as MaxLeadTime,
            MIN(case when  SubTable.COMMANDSTATE=128 then DATEDIFF(SECOND,SubTable.CMD_INSER_TIME,SubTable.CMD_FINISH_TIME) else null end ) as MinLeadTime,
            AVG(case when  SubTable.COMMANDSTATE=128 then DATEDIFF(SECOND,SubTable.CMD_INSER_TIME,SubTable.CMD_FINISH_TIME) else 0 end ) as AVGLeadTime,
            SUM(DATEDIFF(MINUTE,SubTable.CMD_START_TIME,SubTable.CMD_FINISH_TIME)) as CycleTime,
            MAX(case when  SubTable.COMMANDSTATE=128 then DATEDIFF(SECOND,SubTable.CMD_START_TIME,SubTable.CMD_FINISH_TIME) else 0 end ) as MaxCycleTime,
            MIN(case when  SubTable.COMMANDSTATE=128 then DATEDIFF(SECOND,SubTable.CMD_START_TIME,SubTable.CMD_FINISH_TIME) else null end ) as MinCycleTime,
            AVG(case when  SubTable.COMMANDSTATE=128 then DATEDIFF(SECOND,SubTable.CMD_START_TIME,SubTable.CMD_FINISH_TIME) else 0 end ) as AVGCycleTime
            from (Select CMD_ID,HOSTSOURCE,HOSTDESTINATION,CMD_INSER_TIME,CMD_START_TIME,CMD_FINISH_TIME,COMMANDSTATE from HCMD_MCS Where COMMANDSTATE in(128,256,512,1024,2048)) As SubTable
            left join HCMD_OHTC SubOHTC
            on   SubTable.CMD_ID =SubOHTC.CMD_ID_MCS
            group by SubOHTC.VH_ID;
            */
            List<ViewerObject.REPORT.VTransExecRateByVHID> transExecRates = new List<ViewerObject.REPORT.VTransExecRateByVHID>();
            Dictionary<string, int> MSTTotalCount = new Dictionary<string, int>();
            try
            {


                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;


                    //計算MCS總共來了幾筆
                    var query1 = from cmd in (from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                              where (sbTable.MCS_CMD_INSER_TIME < FinishTime && sbTable.MCS_CMD_INSER_TIME >= StartTime)
                                                     &&
                                                    (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                                     sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                                     sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                                     )
                                                     && sbTable.VH_ID != null
                                              select sbTable
                                           )
                                 group cmd by new
                                 {
                                     VH_ID = cmd.VH_ID

                                 } into g
                                 select new
                                 {
                                     VH_ID = g.Key.VH_ID.ToString(),
                                     MCSTotalCount = g.Sum(i => (i.MCS_CMD_INSER_TIME < FinishTime && i.MCS_CMD_INSER_TIME >= StartTime) ? (Int32)1 : 0),
                                 };

                    foreach (var i in query1?.ToList())
                    {
                        MSTTotalCount.Add(i.VH_ID, i.MCSTotalCount);
                    }


                    var subquery = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                   where (sbTable.MCS_CMD_START_TIME < FinishTime && sbTable.MCS_CMD_START_TIME >= StartTime)
                                         &&
                                         (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                          sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                          sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                          sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                          sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                          sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                          sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan
                                          sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed      //barcoderead
                                          )
                                         && sbTable.VH_ID != null
                                   select new
                                   {
                                       VH_ID = sbTable.VH_ID,
                                       COMMANDSTATE = sbTable.MCS_COMMANDSTATE,
                                       MCS_CMD_INSER_TIME = sbTable.MCS_CMD_INSER_TIME,
                                       MCS_CMD_START_TIME = sbTable.MCS_CMD_START_TIME,
                                       MCS_CMD_FINISH_TIME = sbTable.MCS_CMD_FINISH_TIME,
                                       OHTC_CMD_TYPE = sbTable.OHTC_CMD_TYPE,
                                       CMD_STATUS = sbTable.OHTC_CMD_STATUS,
                                       OHTC_COMPLETE_STATUS = sbTable.OHTC_COMPLETE_STATUS
                                   };

                    var query = from cmd in subquery
                                group cmd by new { VH_ID = cmd.VH_ID } into g
                                select new
                                {
                                    VH_ID = g.Key,
                                    //MCSTotalCount = g.Sum(i => 1),
                                    EntryCount = g.Sum(i => (i.MCS_CMD_START_TIME < FinishTime && i.MCS_CMD_START_TIME >= StartTime) ? (Int32)1 : 0),
                                    CompletedCount = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (Int32)1 : 0),
                                    EmptyRetrieval = g.Sum(i => i.OHTC_COMPLETE_STATUS == 27 ? (Int32)1 : 0),
                                    DoubleStorage = g.Sum(i => i.OHTC_COMPLETE_STATUS == 26 ? (Int32)1 : 0),
                                    InterLockCount = g.Sum(i => i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ? (Int32)1 : 0),
                                    EQPTAbortErrorCount = g.Sum(i => i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ? (Int32)1 : 0),
                                    ScanCount = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ? (Int32)1 : 0)),
                                    BarcodeReadFail = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed) ? (Int32)1 : 0)),

                                    LeadTime128 = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_INSER_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    LeadTime128_5 = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_INSER_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,

                                    TransExecTime = g.Sum(i => (DbFunctions.DiffMinutes(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME))) ?? 0,
                                    TransExecTime128 = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    TransExecTime128_5 = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    MaxTransExecTime = (int)(g.Max(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0),
                                    MinTransExecTime = (int?)(g.Min(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : null) ?? null),

                                    LoadingInterLock = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError) && ((i.COMMANDSTATE & 4) == 4) && ((i.COMMANDSTATE & 32) != 32)) ? (int)1 : 0),
                                    UnLoadingInterLock = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError) && ((i.COMMANDSTATE & 32) == 32)) ? (int)1 : 0)
                                };


                    foreach (var item in query?.ToList())
                    {
                        transExecRates.Add(new ViewerObject.REPORT.VTransExecRateByVHID(
                                            _VHID: item.VH_ID.VH_ID.ToString(),
                                            _MCSTotalCount: MSTTotalCount[item.VH_ID.VH_ID.ToString()], _EntryCount: item.EntryCount, _CompletedCount: item.CompletedCount, _EmptyRetrievalErrorCount: item.EmptyRetrieval, _DoubleStorageErrorCount: item.DoubleStorage, _InterLockCount: item.InterLockCount, _EQPTAbortErrorCount: item.EQPTAbortErrorCount,
                                              _BarcodeReadFailCount: item.BarcodeReadFail, _ScanCount: item.ScanCount,
                                               _LoadingInterLock: item.LoadingInterLock, _UnLoadingInterLock: item.UnLoadingInterLock,
                                            _AvgLeadTime: (item.CompletedCount != 0) ? Math.Round((double)(item.LeadTime128_5 / item.CompletedCount), 2) : 0,
                                            _CycleTime: item.TransExecTime, _AvgCycleTime: (item.CompletedCount != 0) ? Math.Round((double)(item.TransExecTime128_5 / item.CompletedCount), 2) : 0, _MaxCycleTime: item.MaxTransExecTime, _MinCycleTime: item.MinTransExecTime,
                                            StartTime: StartTime, EndTime: FinishTime, _TransTime128: item.LeadTime128_5, _TransExecTime128: item.TransExecTime128_5
                                            )
                                          );
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return transExecRates;
        }

        public List<ViewerObject.REPORT.VTransExecRateDetail> LoadTransExecRateDetail(DateTime StartTime, DateTime FinishTime, out int VHNumbers) => loadtransexecratedetail(StartTime, FinishTime, out VHNumbers);
        private List<ViewerObject.REPORT.VTransExecRateDetail> loadtransexecratedetail(DateTime StartTime, DateTime FinishTime, out int VHNumbers)
        {
            //如果執行效能不好，請確認是否建立Index
            // create INDEX IHCMD_MCS ON HCMD_MCS(HOSTSOURCE,HOSTDESTINATION,CMD_INSER_TIME,CMD_START_TIME,CMD_FINISH_TIME,COMMANDSTATE);

            /*
            DemoSQL: 
            SELECT 
            datepart(YEAR,CMD_FINISH_TIME) AS dyear,datepart(MONTH,CMD_FINISH_TIME) AS dmonth, datepart(day,CMD_FINISH_TIME) AS ddate,datepart(hour,CMD_FINISH_TIME) AS dhour, 
            Count(CMD_ID)as EntryCount,
            SUM(case when  COMMANDSTATE=128 then 1 else 0 end) as Completed,
            SUM(case when  COMMANDSTATE=512 then 1 else 0 end) as LoadErrorCount,
            SUM(case when  COMMANDSTATE=256 then 1 else 0 end) as UnLoadErrorCount,
            SUM(case when  COMMANDSTATE in(1024,2048) then 1 else 0 end) as CancelAbortErrorCount,
            SUM(DATEDIFF(MINUTE,CMD_INSER_TIME,CMD_FINISH_TIME)) as LeadTime,
            MAX(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME) else 0 end ) as MaxLeadTime,
            MIN(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME) else 99999 end ) as MinLeadTime,
            AVG(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_INSER_TIME,CMD_FINISH_TIME) else 0 end ) as AVGLeadTime,
            SUM(DATEDIFF(MINUTE,CMD_START_TIME,CMD_FINISH_TIME)) as CycleTime,
            MAX(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME) else 0 end ) as MaxCycleTime,
            MIN(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME) else 99999 end ) as MinCycleTime,
            AVG(case when  COMMANDSTATE=128 then DATEDIFF(SECOND,CMD_START_TIME,CMD_FINISH_TIME) else 0 end ) as AVGCycleTime
            FROM      HCMD_MCS
            Where COMMANDSTATE in ('128','256','512','1024','2048')
            GROUP BY datepart(hour,CMD_FINISH_TIME),datepart(day,CMD_FINISH_TIME),datepart(MONTH,CMD_FINISH_TIME),datepart(YEAR,CMD_FINISH_TIME)
            order by dyear,dmonth,ddate,dhour;
            */
            List<ViewerObject.REPORT.VTransExecRateDetail> transExecRateDetails = new List<ViewerObject.REPORT.VTransExecRateDetail>();
            VHNumbers = 0;
            Dictionary<string, int> MSTTotalCount = new Dictionary<string, int>();
            try
            {


                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var queryNumbers = from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                       where ((sbTable.MCS_CMD_INSER_TIME < FinishTime && sbTable.MCS_CMD_INSER_TIME >= StartTime) || (sbTable.MCS_CMD_START_TIME < FinishTime && sbTable.MCS_CMD_START_TIME >= StartTime))
                                              &&
                                              (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                               sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                               sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan
                                               sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                               )
                                              && sbTable.VH_ID != null
                                       group sbTable by new { VH_ID = sbTable.VH_ID } into g
                                       select new
                                       {
                                           VHIDNum = g.Key
                                       };

                    VHNumbers = queryNumbers?.ToList().Count() ?? 0;

                    //計算MCS總共來了幾筆
                    var query1 = from cmd in (from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                              where (sbTable.MCS_CMD_INSER_TIME < FinishTime && sbTable.MCS_CMD_INSER_TIME >= StartTime)
                                                     &&
                                                      (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                                       sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                                       sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                                       sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                                       sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                                       sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                                       sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan
                                                       sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                                       )
                                                     && sbTable.VH_ID != null
                                              select sbTable
                                           )
                                 group cmd by new
                                 {
                                     dyear = SqlFunctions.DatePart("year", cmd.MCS_CMD_INSER_TIME),
                                     dmonth = SqlFunctions.DatePart("month", cmd.MCS_CMD_INSER_TIME),
                                     dday = SqlFunctions.DatePart("day", cmd.MCS_CMD_INSER_TIME),
                                     dhour = SqlFunctions.DatePart("hour", cmd.MCS_CMD_INSER_TIME)

                                 } into g
                                 select new
                                 {
                                     Year = g.Key.dyear.ToString(),
                                     Month = g.Key.dmonth.ToString(),
                                     Day = g.Key.dday.ToString(),
                                     Hour = g.Key.dhour.ToString(),

                                     MCSTotalCount = g.Sum(i => (i.MCS_CMD_INSER_TIME < FinishTime && i.MCS_CMD_INSER_TIME >= StartTime) ? (Int32)1 : 0),
                                 };

                    foreach (var i in query1?.ToList())
                    {
                        MSTTotalCount.Add(i.Year + "-" + i.Month + "-" + i.Day + "-" + i.Hour, i.MCSTotalCount);
                    }



                    //計算總共開始執行命令有幾筆
                    var query = from cmd in (from sbTable in con.VHCMD_OHTC_MCS.AsNoTracking()
                                             where (sbTable.MCS_CMD_START_TIME < FinishTime && sbTable.MCS_CMD_START_TIME >= StartTime)
                                                    &&
                                                    (sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload ||         //normal end
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload ||
                                                     sbTable.OHTC_COMPLETE_STATUS == 27 || //emptyretrival
                                                     sbTable.OHTC_COMPLETE_STATUS == 26 || //doublestorage
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ||  //interlock
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ||    //vehicleabort
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ||            //scan
                                                     sbTable.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed       //barcoderead
                                                     )
                                                    && sbTable.VH_ID != null
                                             select sbTable
                                            )
                                group cmd by new
                                {
                                    dyear = SqlFunctions.DatePart("year", cmd.MCS_CMD_START_TIME),
                                    dmonth = SqlFunctions.DatePart("month", cmd.MCS_CMD_START_TIME),
                                    dday = SqlFunctions.DatePart("day", cmd.MCS_CMD_START_TIME),
                                    dhour = SqlFunctions.DatePart("hour", cmd.MCS_CMD_START_TIME)

                                } into g
                                select new
                                {
                                    Year = g.Key.dyear.ToString(),
                                    Month = g.Key.dmonth.ToString(),
                                    Day = g.Key.dday.ToString(),
                                    Hour = g.Key.dhour.ToString(),

                                    //MCSTotalCount = g.Sum(i => (i.MCS_CMD_INSER_TIME < FinishTime && i.MCS_CMD_INSER_TIME >= StartTime) ? (Int32)1 : 0),
                                    EntryCount = g.Sum(i => (i.MCS_CMD_START_TIME < FinishTime && i.MCS_CMD_START_TIME >= StartTime) ? (Int32)1 : 0),
                                    CompletedCount = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (Int32)1 : 0),
                                    EmptyRetrieval = g.Sum(i => i.OHTC_COMPLETE_STATUS == 27 ? (Int32)1 : 0),
                                    DoubleStorage = g.Sum(i => i.OHTC_COMPLETE_STATUS == 26 ? (Int32)1 : 0),
                                    InterLockCount = g.Sum(i => i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError ? (Int32)1 : 0),
                                    EQPTAbortErrorCount = g.Sum(i => i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusVehicleAbort ? (Int32)1 : 0),
                                    ScanCount = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusScan ? (Int32)1 : 0)),
                                    BarcodeReadFail = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusIdreadFailed) ? (Int32)1 : 0)),

                                    LeadTime128 = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_INSER_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    LeadTime128_5 = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_INSER_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,

                                    CycleTime = g.Sum(i => (DbFunctions.DiffMinutes(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME))) ?? 0,
                                    CycleTime128 = g.Sum(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    CycleTime128_5 = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0,
                                    MaxCycleTime = (int)(g.Max(i => (i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : 0) ?? 0),
                                    MinCycleTime = (int?)(g.Min(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusUnload || i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusLoadunload) && i.OHTC_CMD_TYPE == 5) ? (DbFunctions.DiffSeconds(i.MCS_CMD_START_TIME, i.MCS_CMD_FINISH_TIME)) : null) ?? null),

                                    LoadingInterLock = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError) && ((i.MCS_COMMANDSTATE & 4) == 4) && ((i.MCS_COMMANDSTATE & 32) != 32)) ? (int)1 : 0),
                                    UnLoadingInterLock = g.Sum(i => ((i.OHTC_COMPLETE_STATUS == (int)CompleteStatus.CmpStatusInterlockError) && ((i.MCS_COMMANDSTATE & 32) == 32)) ? (int)1 : 0)
                                };

                    foreach (var item in query?.ToList())
                    {
                        transExecRateDetails.Add(new ViewerObject.REPORT.VTransExecRateDetail(
                                            _Year: item.Year, _Month: item.Month, _Day: item.Day, _Hour: item.Hour,
                                            _MCSTotalCount: MSTTotalCount[item.Year + "-" + item.Month + "-" + item.Day + "-" + item.Hour], _EntryCount: item.EntryCount, _CompletedCount: item.CompletedCount, _EmptyRetrievalErrorCount: item.EmptyRetrieval, _DoubleStorageErrorCount: item.DoubleStorage, _InterLockCount: item.InterLockCount, _EQPTAbortErrorCount: item.EQPTAbortErrorCount,
                                              _BarcodeReadFailCount: item.BarcodeReadFail, _ScanCount: item.ScanCount,
                                               _LoadingInterLock: item.LoadingInterLock, _UnLoadingInterLock: item.UnLoadingInterLock,
                                            _AvgLeadTime: (item.CompletedCount != 0) ? Math.Round((double)(item.LeadTime128_5 / item.CompletedCount), 2) : 0,
                                            _CycleTime: item.CycleTime, _AvgCycleTime: (item.CompletedCount != 0) ? Math.Round((double)(item.CycleTime128_5 / item.CompletedCount), 2) : 0, _MaxCycleTime: item.MaxCycleTime, _MinCycleTime: item.MinCycleTime,
                                            StartTime: StartTime, EndTime: FinishTime, _TransTime128: item.LeadTime128_5, _TransExecTime128: item.CycleTime128_5, _VCount: VHNumbers
                                            )
                                          );
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return transExecRateDetails;
        }

        public List<ViewerObject.REPORT.VRealExcuteTime> LoadRealExecuteTime(DateTime StartTime, DateTime FinishTime) => loadrealexecutetime(StartTime, FinishTime);
        private List<ViewerObject.REPORT.VRealExcuteTime> loadrealexecutetime(DateTime StartTime, DateTime FinishTime)
        {
            List<ViewerObject.REPORT.VRealExcuteTime> lsReturn = new List<ViewerObject.REPORT.VRealExcuteTime>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;


                    var query = from cmd in (from sbTable in con.ASYSEXCUTEQUALITY.AsNoTracking()
                                             where sbTable.CMD_START_TIME >= StartTime && sbTable.CMD_FINISH_TIME <= FinishTime
                                             select sbTable
                                        )
                                group cmd by new
                                {
                                    cmd.SOURCE_ADR,
                                    cmd.DESTINATION_ADR

                                } into g
                                select new
                                {
                                    SOURCE_ADR = g.Key.SOURCE_ADR.ToString(),
                                    DESTINATION_ADR = g.Key.DESTINATION_ADR.ToString(),

                                    Mean_CMDQUEUE_TIME = g.Sum(i => i.CMDQUEUE_TIME) / g.Count(i => i.SOURCE_ADR == g.Key.SOURCE_ADR.ToString() && i.DESTINATION_ADR == g.Key.DESTINATION_ADR.ToString()),
                                    Mean_MOVE_TO_SOURCE_TIME = g.Sum(i => i.MOVE_TO_SOURCE_TIME) / g.Count(i => i.SOURCE_ADR == g.Key.SOURCE_ADR.ToString() && i.DESTINATION_ADR == g.Key.DESTINATION_ADR.ToString()),
                                    Mean_MOVE_TO_DESTN_TIME = g.Sum(i => i.MOVE_TO_DESTN_TIME) / g.Count(i => i.SOURCE_ADR == g.Key.SOURCE_ADR.ToString() && i.DESTINATION_ADR == g.Key.DESTINATION_ADR.ToString()),
                                    Mean_CMD_TOTAL_EXCUTION_TIME = g.Sum(i => i.CMD_TOTAL_EXCUTION_TIME) / g.Count(i => i.SOURCE_ADR == g.Key.SOURCE_ADR.ToString() && i.DESTINATION_ADR == g.Key.DESTINATION_ADR.ToString())
                                };

                    foreach (var i in query?.ToList())
                    {
                        ViewerObject.REPORT.VRealExcuteTime oVRealExcuteTime = new ViewerObject.REPORT.VRealExcuteTime()
                        {
                            SOURCE_ADR = i.SOURCE_ADR,
                            DESTINATION_ADR = i.DESTINATION_ADR,
                            Mean_CMDQUEUE_TIME = i.Mean_CMDQUEUE_TIME,
                            Mean_CMD_TOTAL_EXCUTION_TIME = i.Mean_CMD_TOTAL_EXCUTION_TIME,
                            Mean_MOVE_TO_SOURCE_TIME = i.Mean_MOVE_TO_SOURCE_TIME,
                            Mean_MOVE_TO_DESTN_TIME = i.Mean_MOVE_TO_DESTN_TIME
                        };
                        lsReturn.Add(oVRealExcuteTime);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return lsReturn;
        }

        public List<ViewerObject.REPORT.VSysExcuteQuality> LoadSysExecutionQuality(DateTime StartTime, DateTime FinishTime) => loadsysexecutionquality(StartTime, FinishTime);
        private List<ViewerObject.REPORT.VSysExcuteQuality> loadsysexecutionquality(DateTime StartTime, DateTime FinishTime)
        {
            List<ViewerObject.REPORT.VSysExcuteQuality> lsReturn = new List<ViewerObject.REPORT.VSysExcuteQuality>();
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;


                    var query = from cmd in con.ASYSEXCUTEQUALITY.AsNoTracking()
                                where cmd.CMD_START_TIME >= StartTime && cmd.CMD_FINISH_TIME <= FinishTime
                                select cmd;



                    foreach (var i in query?.ToList())
                    {
                        ViewerObject.REPORT.VSysExcuteQuality oVSysExcuteQuality = new ViewerObject.REPORT.VSysExcuteQuality(
                             _CMD_ID_MCS: i.CMD_ID_MCS,
                             _CMD_Insert_Time: i.CMD_INSERT_TIME,
                             _CMD_Finish_Status: i.CMD_FINISH_STATUS.ToString(),
                             VH_ID: i.VH_ID,
                             _VH_Start_Sec_ID: i.VH_START_SEC_ID,
                             _Source_ADR: i.SOURCE_ADR,
                             _Sec_CNt_to_Source: i.SEC_CNT_TO_SOURCE.ToString(),
                             _Sec_Dis_to_Source: i.SEC_DIS_TO_SOURCE.ToString(),
                             _Destination_ADR: i.DESTINATION_ADR,
                             _Sec_CNt_to_Destn: i.SEC_CNT_TO_DESTN.ToString(),
                             _Sec_Dis_to_Destn: i.SEC_DIS_TO_DESTN.ToString(),
                             _Cmdqueue_time: i.CMDQUEUE_TIME,
                             _Move_to_source_time: i.MOVE_TO_DESTN_TIME,
                             _Total_Block_Time_to_Source: i.TOTAL_BLOCK_TIME_TO_SOURCE,
                             _Total_OCS_Time_to_Source: i.TOTAL_OCS_TIME_TO_SOURCE,
                             _Total_OCS_Count_to_Source: i.TOTAL_OCS_COUNT_TO_SOURCE,
                             _Move_to_Destn_Time: i.MOVE_TO_DESTN_TIME,
                             _Total_Block_Time_to_Destn: i.TOTAL_BLOCK_TIME_TO_DESTN,
                             _Total_OCS_Time_to_Destn: i.TOTAL_OCS_TIME_TO_DESTN,
                             _Total_Block_Count_to_Destn: i.TOTAL_BLOCK_COUNT_TO_DESTN,
                             _Total_OCS_Count_to_Destn: i.TOTAL_OCS_COUNT_TO_DESTN,
                             _Total_Pause_Time: i.TOTALPAUSE_TIME,
                             _CMD_Total_Excution_Time: i.CMD_TOTAL_EXCUTION_TIME,
                             _Total_ACT_VH_Count: i.TOTAL_ACT_VH_COUNT,
                             _Paking_VH_Count: i.PARKING_VH_COUNT,
                             _CycleRun_VH_Count: i.CYCLERUN_VH_COUNT,
                             _Total_Idle_VH_Count: i.TOTAL_IDLE_VH_COUNT,
                             _CMD_Start_Time: i.CMD_START_TIME,
                             _CMD_Finish_Time: i.CMD_FINISH_TIME
                            );
                        lsReturn.Add(oVSysExcuteQuality);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return lsReturn;
        }

        public void GetHCMD_MCSLimitDateTime(out DateTime StartTime, out DateTime EndTime) => gethcmd_mcslimitdatetime(out StartTime, out EndTime);
        private void gethcmd_mcslimitdatetime(out DateTime StartTime, out DateTime EndTime)
        {
            //如果執行效能不好，請確認是否建立Index
            /*
            DemoSQL: 
            select TOP 1 * from HCMD_MCS order by CMD_INSER_TIME asc
            select TOP 1 * from HCMD_MCS order by CMD_INSER_TIME desc
            */
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
            try
            {

                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = (from sbTable in con.HCMD_MCS.AsNoTracking()
                                 select sbTable.CMD_INSER_TIME).Min();

                    StartTime = query;



                    var query1 = (from sbTable in con.HCMD_MCS.AsNoTracking()
                                  select sbTable.CMD_INSER_TIME).Max();
                    EndTime = query1;


                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public List<string> GetHCMD_MCSVH() => gethcmd_mcsvh();
        private List<string> gethcmd_mcsvh()
        {
            //如果執行效能不好，請確認是否建立Index
            /*
            DemoSQL: 
            select distinct VH_ID from HCMD_OHTC;
            */
            List<string> lsReturn = new List<string>();
            try
            {

                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = (from sbTable in con.HCMD_OHTC.AsNoTracking()
                                 select sbTable.VH_ID).Distinct();

                    lsReturn = query?.ToList();
                }
                return lsReturn;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return lsReturn;
            }
        }
        #endregion
    }

    public class CmdConverter
    {
        Definition.Convert defCvt = new Definition.Convert();

        #region CMD
        public ViewerObject.VCMD GetVCMD(ACMD_OHTC input)
        {
            ViewerObject.VCMD output = null;
            if (input != null)
            {
                output = new ViewerObject.VCMD()
                {
                    CMD_ID = input.CMD_ID?.Trim() ?? "",
                    CMD_TYPE = defCvt.GetCmdType(input.CMD_TPYE),
                    VH_ID = input.VH_ID?.Trim() ?? "",
                    CARRIER_ID = input.CARRIER_ID?.Trim() ?? "",
                    BOX_ID = input.BOX_ID?.Trim() ?? "",
                    LOT_ID = input.LOT_ID?.Trim() ?? "",
                    SOURCE = input.SOURCE?.Trim() ?? "",
                    DESTINATION = input.DESTINATION?.Trim() ?? "",
                    PRIORITY = input.PRIORITY,
                    START_TIME = input.CMD_START_TIME,
                    END_TIME = input.CMD_END_TIME,
                    CMD_STATUS = defCvt.GetCmdStatus(input.CMD_STAUS),
                    TRANSFER_ID = input.CMD_ID_MCS?.Trim() ?? ""
                };                
            }
            return output;
        }
        public List<ViewerObject.VCMD> GetVCMDs(List<ACMD_OHTC> input)
        {
            List<ViewerObject.VCMD> output = new List<ViewerObject.VCMD>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVCMD(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
        public ViewerObject.VCMD GetVCMD(HCMD_OHTC input)
        {
            ViewerObject.VCMD output = null;
            if (input != null)
            {
                output = new ViewerObject.VCMD()
                {
                    CMD_ID = input.CMD_ID?.Trim() ?? "",
                    CMD_TYPE = defCvt.GetCmdType(input.CMD_TPYE),
                    VH_ID = input.VH_ID?.Trim() ?? "",
                    CARRIER_ID = input.CARRIER_ID?.Trim() ?? "",
                    BOX_ID = input.BOX_ID?.Trim() ?? "",
                    LOT_ID = input.LOT_ID?.Trim() ?? "",
                    SOURCE = input.SOURCE?.Trim() ?? "",
                    DESTINATION = input.DESTINATION?.Trim() ?? "",
                    PRIORITY = input.PRIORITY,
                    START_TIME = input.CMD_START_TIME,
                    END_TIME = input.CMD_END_TIME,
                    CMD_STATUS = defCvt.GetCmdStatus(input.CMD_STAUS),
                    TRANSFER_ID = input.CMD_ID_MCS?.Trim() ?? "",
                    //CompeleteStatus = IntTOcmpStatusString(input.COMPLETE_STATUS)
                    CompeleteStatus = input.COMPLETE_STATUS.ToString() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.VCMD> GetVCMDs(List<HCMD_OHTC> input)
        {
            List<ViewerObject.VCMD> output = new List<ViewerObject.VCMD>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVCMD(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }

        public ViewerObject.REPORT.VCMD_ExportDetail GetVCMD_Detail(HCMD_OHTC input)
        {
            ViewerObject.REPORT.VCMD_ExportDetail output = null;
            if (input != null)
            {
                output = new ViewerObject.REPORT.VCMD_ExportDetail()
                {
                    CMD_ID = input.CMD_ID?.Trim() ?? "",
                    CMD_TYPE = defCvt.GetCmdType(input.CMD_TPYE),
                    VH_ID = input.VH_ID?.Trim() ?? "",
                    CARRIER_ID = input.CARRIER_ID?.Trim() ?? "",
                    BOX_ID = input.BOX_ID?.Trim() ?? "",
                    LOT_ID = input.LOT_ID?.Trim() ?? "",
                    SOURCE = input.SOURCE?.Trim() ?? "",
                    DESTINATION = input.DESTINATION?.Trim() ?? "",
                    PRIORITY = input.PRIORITY,
                    START_TIME = input.CMD_START_TIME,
                    END_TIME = input.CMD_END_TIME,
                    CMD_STATUS = defCvt.GetCmdStatus(input.CMD_STAUS),
                    TRANSFER_ID = input.CMD_ID_MCS?.Trim() ?? "",
                    //CompeleteStatus = IntTOcmpStatusString(input.COMPLETE_STATUS)
                    CompeleteStatus = input.COMPLETE_STATUS.ToString() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.REPORT.VCMD_ExportDetail> GetVCMD_Details(List<HCMD_OHTC> input)
        {
            List<ViewerObject.REPORT.VCMD_ExportDetail> output = new List<ViewerObject.REPORT.VCMD_ExportDetail>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVCMD_Detail(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
        public string IntTOcmpStatusString(int? input)
        {
            switch (input)
            {
                case 0: return CompleteStatus.CmpStatusMove.ToString();
                case 1: return CompleteStatus.CmpStatusLoad.ToString();
                case 2: return CompleteStatus.CmpStatusUnload.ToString();
                case 3: return CompleteStatus.CmpStatusLoadunload.ToString();
                case 4: return CompleteStatus.CmpStatusHome.ToString();
                case 5: return CompleteStatus.CmpStatusOverride.ToString();
                case 6: return CompleteStatus.CmpStatusCstIdrenmae.ToString();
                case 7: return CompleteStatus.CmpStatusMtlhome.ToString();
                case 8: return CompleteStatus.CmpStatusScan.ToString();
                case 10: return CompleteStatus.CmpStatusMoveToMtl.ToString();
                case 11: return CompleteStatus.CmpStatusSystemOut.ToString();
                case 12: return CompleteStatus.CmpStatusSystemIn.ToString();
                case 13: return CompleteStatus.CmpStatusTechingMove.ToString();
                case 20: return CompleteStatus.CmpStatusCancel.ToString();
                case 21: return CompleteStatus.CmpStatusAbort.ToString();
                case 22: return CompleteStatus.CmpStatusVehicleAbort.ToString();
                case 23: return CompleteStatus.CmpStatusIdmisMatch.ToString();
                case 24: return CompleteStatus.CmpStatusIdreadFailed.ToString();
                case 25: return CompleteStatus.CmpStatusIdreadDuplicate.ToString();
                case 64: return CompleteStatus.CmpStatusInterlockError.ToString();
                case 98: return CompleteStatus.CmpStatusLongTimeInaction.ToString();
                //case 99: return CompleteStatus.CmpStatusForceFinishByOp.ToString();
                default: return "";
            }
        }
        #endregion CMD

        #region TRANSFER
        public ViewerObject.VTRANSFER GetVTRANSFER(VACMD_MCS input)
        {
            ViewerObject.VTRANSFER output = null;
            if (input != null)
            {
                output = new ViewerObject.VTRANSFER()
                {
                    TRANSFER_ID = input.CMD_ID?.Trim() ?? "",
                    CARRIER_ID = input.CARRIER_ID?.Trim() ?? "",
                    BOX_ID = "", //input.BOX_ID?.Trim() ?? "",
                    LOT_ID = "", //input.LOT_ID?.Trim() ?? "",
                    HOSTSOURCE = input.HOSTSOURCE?.Trim() ?? "",
                    HOSTDESTINATION = input.HOSTDESTINATION?.Trim() ?? "",
                    PRIORITY = input.PRIORITY,
                    PORT_PRIORITY = input.PORT_PRIORITY,
                    TIME_PRIORITY = input.TIME_PRIORITY,
                    INSERT_TIME = input.CMD_INSER_TIME,
                    ASSIGN_TIME = input.CMD_START_TIME,
                    FINISH_TIME = input.CMD_FINISH_TIME,
                    TRANSFER_STATUS = GetTransferStatus((E_TRAN_STATUS)input.TRANSFERSTATE),
                    CMD_ID = input.OHTC_CMD?.Trim() ?? "",
                    VH_ID = input.VH_ID?.Trim() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.VTRANSFER> GetVTRANSFERs(List<VACMD_MCS> input)
        {
            List<ViewerObject.VTRANSFER> output = new List<ViewerObject.VTRANSFER>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVTRANSFER(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
        public ViewerObject.VTRANSFER_Def.TransferStatus GetTransferStatus(E_TRAN_STATUS transfer_status)
        {
            switch (transfer_status)
            {
                case E_TRAN_STATUS.TransferCompleted:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Complete;
                case E_TRAN_STATUS.Aborting:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Aborting;
                case E_TRAN_STATUS.Canceling:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Canceling;
                case E_TRAN_STATUS.Transferring:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Transferring;
                case E_TRAN_STATUS.Paused:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Paused;
                case E_TRAN_STATUS.Queue:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Queue;
                default:
                    return ViewerObject.VTRANSFER_Def.TransferStatus.Undefined;
            }
        }
        #endregion TRANSFER
    }
}
