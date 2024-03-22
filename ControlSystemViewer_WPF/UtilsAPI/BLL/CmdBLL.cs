using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ViewerObject;
using ViewerObject.REPORT;

namespace UtilsAPI.BLL
{
    public class CmdBLL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        SqlConnection conn = null;

        public CmdBLL(WindownApplication _app)
        {
            app = _app;
        }

        #region WebAPI
        public bool ForceUpdataCmdStatus2FnishByVhID(string vh_id)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "Engineer",
                "ForceCmdFinish",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
            return result == "OK";
        }

        public (bool success, string response) CreateTransferCmd(string box_id, string cst_id, string source, string dest, string lot_id)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "TransferCreate",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(box_id)}={box_id}").Append("&");
            sb.Append($"{nameof(cst_id)}={cst_id}").Append("&");
            sb.Append($"{nameof(source)}={source}").Append("&");
            sb.Append($"{nameof(dest)}={dest}").Append("&");
            sb.Append($"{nameof(lot_id)}={lot_id}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
            return (result == "OK", result);
        }

        public (bool success, string response) CreateInstallCmd(string has_carrier, string carrier_id, string box_id, string carrier_loc)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "CassetteData",
                "InstallCreate",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(has_carrier)}={has_carrier}").Append("&");
            sb.Append($"{nameof(carrier_id)}={carrier_id}").Append("&");
            sb.Append($"{nameof(box_id)}={box_id}").Append("&");
            sb.Append($"{nameof(carrier_loc)}={carrier_loc}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            return (result == "OK", result);
        }
        #endregion WebAPI

        #region CMD
        public List<VCMD> LoadUnfinishCmds()
        {
            var default_result = new List<VCMD>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadUnfinishCmds();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VCMD GetCmdByID(string id)
        {
            VCMD default_result = null;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetCmdByID(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VCMD GetCmdByTransferID(string transfer_id)
        {
            VCMD default_result = null;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetCmdByTransferID(transfer_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VCMD GetExecuteCmdByVhID(string vh_id)
        {
            VCMD default_result = null;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetExecuteCmdByVhID(vh_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public bool VhHasQueueCmd(string vh_id)
        {
            bool default_result = false;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.VhHasQueueCmd(vh_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VCMD> GetHCmdByConditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null)
        {
            var default_result = new List<VCMD>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHCmdByConditions(startDatetime, endDatetime, cst_id, vh_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VCMD_ExportDetail> GetVCMDDetail_Export(DateTime startDatetime, DateTime endDatetime)
        {
            var default_result = new List<VCMD_ExportDetail>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetVCMDDetail_Export(startDatetime, endDatetime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VCMD> GetHCmdByDate(DateTime date)
        {
            var default_result = new List<VCMD>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHCmdByDate(date);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
        #endregion CMD

        #region TRANSFER
        public List<VTRANSFER> LoadUnfinishedTransfers()
        {
            var default_result = new List<VTRANSFER>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadUnfinishedTransfers();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VTRANSFER GetTransferByID(string id)
        {
            VTRANSFER default_result = null;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetTransferByID(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public int GetTransferMinPrioritySum()
        {
            int default_result = 0;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetTransferMinPrioritySum();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
        public int GetTransferMaxPrioritySum()
        {
            int default_result = 0;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetTransferMaxPrioritySum();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VTRANSFER> GetHTransferByConditions(DateTime startDatetime, DateTime endDatetime)
        {
            var default_result = new List<VTRANSFER>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHTransferByConditions(startDatetime, endDatetime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
        public int GetHTransferHourlyCount()
        {
            int default_result = -1;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHTransferHourlyCount();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
        public int GetHTransferTodayCount()
        {
            int default_result = -1;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHTransferTodayCount();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
        #endregion TRANSFER


        #region Report

        public List<VMCBF> LoadMTTRHCmd(DateTime StartTime, DateTime FinishTime)
        {
            List<VMCBF> default_result = new List<VMCBF>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadMTTRHCmd(StartTime, FinishTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VCMD_OHTC> GetHCMDOHTCByConditions(DateTime StartTime, DateTime FinishTime)
        {
            List<VCMD_OHTC> default_result = new List<VCMD_OHTC>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHCMDOHTCByConditions(StartTime, FinishTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VTransExecRate> LoadTransExecRate(DateTime StartTime,DateTime EndTime,out int VHNumbers)
        {
            var default_result = new List<VTransExecRate>();
            VHNumbers = 0;
            try
            {
                   return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadTransExecRate(StartTime,EndTime,out VHNumbers);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VTransExecRateByVHID> LoadTransExecRateGroupByEQPT(DateTime StartTime, DateTime EndTime)
        {
            var default_result = new List<VTransExecRateByVHID>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadTransExecRateGroupByEQPT(StartTime, EndTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VTransExecRateDetail> LoadTransExecRateDetail(DateTime StartTime, DateTime EndTime, out int VHNumbers)
        {
            var default_result = new List<VTransExecRateDetail>();
            VHNumbers = 0;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadTransExecRateDetail(StartTime, EndTime,out VHNumbers);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                VHNumbers = -1;
                return default_result;
            }
        }

        public void GetHCMD_MCSLimitDateTime(out DateTime StartTime, out DateTime EndTime )
        {
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MaxValue;
            try
            {
                 app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHCMD_MCSLimitDateTime(out StartTime,out EndTime);
                StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, StartTime.Hour, 0, 0);
                EndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, EndTime.Hour, 0, 0).AddHours(1);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public List<VRealExcuteTime> LoadRealExecuteTime(DateTime StartTime, DateTime FinishTime)
        {
            List<VRealExcuteTime> default_result = new List<VRealExcuteTime>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadRealExecuteTime(StartTime, FinishTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VSysExcuteQuality> LoadSysExecutionQuality(DateTime StartTime, DateTime FinishTime)
        {
            List<VSysExcuteQuality> default_result = new List<VSysExcuteQuality>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.CmdBLL.LoadSysExecutionQuality(StartTime, FinishTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }


        public List<string> GetHCMD_MCSVH()
        {
            List<string> lsReturn = new List<string>();
            try
            {
                lsReturn = app.ObjCacheManager.ObjConverter.BLL.CmdBLL.GetHCMD_MCSVH();
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
}
