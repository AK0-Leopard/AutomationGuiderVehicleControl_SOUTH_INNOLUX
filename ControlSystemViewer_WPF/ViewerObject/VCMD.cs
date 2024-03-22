using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VCMD
    {
        public VCMD()
        {
        }

        public string CMD_ID { get; set; } = "";
        public VCMD_Def.CmdType CMD_TYPE { get; set; } = VCMD_Def.CmdType.Undefined;
        public string VH_ID { get; set; } = "";
        public string CARRIER_ID { get; set; } = "";
        public string BOX_ID { get; set; } = "";
        public string LOT_ID { get; set; } = "";
        public string SOURCE { get; set; } = "";
        public string DESTINATION { get; set; } = "";
        public int PRIORITY { get; set; }
        public DateTime? START_TIME { get; set; } = null;
        public DateTime? END_TIME { get; set; } = null;
        public VCMD_Def.CmdStatus CMD_STATUS { get; set; } = VCMD_Def.CmdStatus.Undefined;
        public string TRANSFER_ID { get; set; } = "";
        public string CompeleteStatus { get; set; } = "";

        public string STR_START_TIME => START_TIME?.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";
        public string STR_END_TIME => END_TIME?.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";


    }

    public class VCMD_OHTC
    {
        public VCMD_OHTC()
        {
        }

        public string CMD_ID_MCS { get; set; } = "";
        public string VH_ID { get; set; } = "";
        public string CARRIER_ID { get; set; } = "";
        public string HOSTSOURCE { get; set; } = "";
        public string HOSTDESTINATION { get; set; } = "";
        public DateTime MCS_CMD_INSERTTIME { get; set; }
        public DateTime? MCS_CMD_START_TIME { get; set; } = null;
        public DateTime? MCS_CMD_FINISH_TIME { get; set; } = null;
        public VCMD_Def.CmdType OHTC_CMD_TYPE { get; set; } = VCMD_Def.CmdType.Undefined;
        public VCMD_Def.CmdStatus OHTC_CMD_STATUS { get; set; } = VCMD_Def.CmdStatus.Undefined;
        public VTRANSFER_Def.CommandState MCS_COMMANDSTATE { get; set; } = VTRANSFER_Def.CommandState.Undefined;
        public DateTime? OHTC_CMD_START_TIME { get; set; } = null;
        public DateTime? OHTC_CMD_END_TIME { get; set; } = null;
        public VCMD_Def.OHTCCompleteStatus? OHTC_COMPLETE_STATUS { get; set; }=null;
        public double? CMDCycleTime
        {
            get
            {
                if (OHTC_CMD_START_TIME == null || OHTC_CMD_END_TIME == null) return null;

                return Math.Round((double)(OHTC_CMD_END_TIME?.Subtract(OHTC_CMD_START_TIME.Value).TotalMinutes), 2);
            }
        }

        public string STR_START_TIME => OHTC_CMD_START_TIME?.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";
        public string STR_END_TIME => OHTC_CMD_END_TIME?.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";


    }


    public static class VCMD_Def
    {
        public enum CmdType
        {
            Undefined = -1,
            Move = 0,
            MovePark,
            MoveMtport,
            MoveTeaching,
            MoveCharger,
            Load,
            Unload,
            LoadUnload,
            Teaching,
            Continue,
            Round,
            Home,
            MTLHome,
            Override,
            Scan = 15,
            Move_MTL = 16,
            SystemIn =17,
            SystemOut=18
        }

        public enum CmdStatus
        {
            Undefined = -1,
            Queue = 0,
            Sending,
            Execution,
            Aborting,
            Canceling,
            NormalEnd,
            AbnormalEndByVehicle,
            AbnormalEndByMCS,
            AbnormalEndByControlSystem,
            CancelEndByControlSystem
        }

        public enum OHTCCompleteStatus
        {
            CmpStatusMove = 0,
            CmpStatusLoad = 1,
            CmpStatusUnload = 2,
            CmpStatusLoadunload = 3,
            CmpStatusHome = 4,
            CmpStatusOverride = 5,
            CmpStatusCstIdrenmae = 6,
            CmpStatusMtlhome = 7,
            CmpStatusScan = 8,
            CmpStatusMoveToMtl = 10,
            CmpStatusSystemOut = 11,
            CmpStatusSystemIn = 12,
            CmpStatusTechingMove = 13,
            CmpStatusCancel = 20,
            CmpStatusAbort = 21,
            CmpStatusVehicleAbort = 22,
            CmpStatusIdmisMatch = 23,
            CmpStatusIdreadFailed = 24,
            CmpStatusIdreadDuplicate = 25,
            CmpStatusIddoubleStorage = 26,
            CmpStatusIdemptyRetrival = 27,
            CmpStatusIdcsttypeMismatch = 28,
            CmpStatusInterlockError = 64,
            CmpStatusCommandInitailFail = 96,
            CmpStatusLongTimeInaction = 97,
            CmpStatusForceAbnormalFinishByOp = 98,
            CmpStatusForceNormalFinishByOp = 99
        }

        public enum CarrierType
        {
            Type1 =1,//L516_W326_H350
            Type2 = 2,//L495_W300_H350
            Type3 = 3,//L495_W330_H350
            Type4 = 4,//L520_W420_H350
        }
    }
}
