using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;

namespace ObjectConverterInterface.BLL
{
    public interface ICmdBLL
    {
        List<ViewerObject.VCMD> LoadUnfinishCmds();
        ViewerObject.VCMD GetCmdByID(string id);
        ViewerObject.VCMD GetCmdByTransferID(string transfer_id);
        ViewerObject.VCMD GetExecuteCmdByVhID(string vh_id);
        bool VhHasQueueCmd(string vh_id);
        List<ViewerObject.VCMD> GetHCmdByConditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null);
        List<ViewerObject.VCMD> GetHCmdByDate(DateTime date);
        List<ViewerObject.VTRANSFER> LoadUnfinishedTransfers();
        ViewerObject.VTRANSFER GetTransferByID(string id);
        int GetTransferMinPrioritySum();
        int GetTransferMaxPrioritySum();
        List<ViewerObject.VTRANSFER> GetHTransferByConditions(DateTime startDatetime, DateTime endDatetime);
        int GetHTransferHourlyCount();
        int GetHTransferTodayCount();

        //Method for Report
        List<ViewerObject.VMCBF> LoadMTTRHCmd(DateTime StartTime, DateTime FinishTime);
        List<ViewerObject.REPORT.VTransExecRate> LoadTransExecRate(DateTime StartTime, DateTime FinishTime,out int VHNumbers);
        List<ViewerObject.REPORT.VTransExecRateByVHID> LoadTransExecRateGroupByEQPT(DateTime StartTime, DateTime FinishTime);
        List<ViewerObject.REPORT.VTransExecRateDetail> LoadTransExecRateDetail(DateTime StartTime, DateTime FinishTime, out int VHNumbers);
        void GetHCMD_MCSLimitDateTime(out DateTime StartTime, out DateTime EndTime);
        List<string> GetHCMD_MCSVH();
        List<ViewerObject.VCMD_OHTC> GetHCMDOHTCByConditions(DateTime startDatetime, DateTime endDatetime, string cst_id = null, string vh_id = null);
        List<ViewerObject.REPORT.VRealExcuteTime> LoadRealExecuteTime(DateTime StartTime, DateTime FinishTime);
        List<ViewerObject.REPORT.VSysExcuteQuality> LoadSysExecutionQuality(DateTime StartTime, DateTime FinishTime);
        List<ViewerObject.REPORT.VCMD_ExportDetail> GetVCMDDetail_Export(DateTime startDatetime, DateTime endDatetime);
    }
}
