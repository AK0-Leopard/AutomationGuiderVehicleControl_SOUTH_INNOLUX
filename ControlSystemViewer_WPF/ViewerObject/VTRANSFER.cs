using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VTRANSFER
    {
        public VTRANSFER()
        {
        }
        public VTRANSFER(VTRANSFER transfer, VCMD cmd)
        {
            TRANSFER_ID = transfer?.TRANSFER_ID ?? "";
            CARRIER_ID = transfer?.CARRIER_ID ?? "";
            LOT_ID = transfer?.LOT_ID ?? "";
            HOSTSOURCE = transfer?.HOSTSOURCE ?? "";
            HOSTDESTINATION = transfer?.HOSTDESTINATION ?? "";
            PRIORITY = transfer?.PRIORITY ?? 0;
            PORT_PRIORITY = transfer?.PORT_PRIORITY ?? 0;
            TIME_PRIORITY = transfer?.TIME_PRIORITY ?? 0;
            INSERT_TIME = transfer?.INSERT_TIME ?? DateTime.MinValue;
            ASSIGN_TIME = transfer?.ASSIGN_TIME;
            FINISH_TIME = transfer?.FINISH_TIME;
            TRANSFER_STATUS = transfer?.TRANSFER_STATUS ?? VTRANSFER_Def.TransferStatus.Undefined;
            COMMANDSTATE = transfer?.COMMANDSTATE ?? VTRANSFER_Def.CommandState.Undefined;
            CMD_ID = cmd?.CMD_ID ?? "";
            VH_ID = cmd?.VH_ID ?? "";
        }
        public void setVCMD(VCMD cmd)
        {
            CMD_ID = cmd?.CMD_ID ?? "";
            VH_ID = cmd?.VH_ID ?? "";
        }

        public string TRANSFER_ID { get; set; } = "";
        public string CARRIER_ID { get; set; } = "";
        public string BOX_ID { get; set; } = "";
        public string LOT_ID { get; set; } = "";
        public string HOSTSOURCE { get; set; } = "";
        public string HOSTDESTINATION { get; set; } = "";
        public int PRIORITY { get; set; } = 0;
        public int PORT_PRIORITY { get; set; } = 0;
        public int TIME_PRIORITY { get; set; } = 0;
        public DateTime INSERT_TIME { get; set; } = DateTime.MinValue;
        public DateTime? ASSIGN_TIME { get; set; } = null;
        public DateTime? FINISH_TIME { get; set; } = null;
        public VTRANSFER_Def.TransferStatus TRANSFER_STATUS { get; set; } = VTRANSFER_Def.TransferStatus.Undefined;
        public VTRANSFER_Def.CommandState COMMANDSTATE { get; set; } = VTRANSFER_Def.CommandState.Undefined;
        public string CMD_ID { get; set; } = "";
        public string VH_ID { get; set; } = "";

        public int PRIORITY_SUM => PRIORITY + PORT_PRIORITY + TIME_PRIORITY;
        public string STR_PRIORITY => $"{PRIORITY}+{PORT_PRIORITY}+{TIME_PRIORITY}";
        public string STR_INSERT_TIME => INSERT_TIME.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";
        public string STR_ASSIGN_TIME => ASSIGN_TIME?.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";
        public string STR_FINISH_TIME => FINISH_TIME?.ToString("yyyy/MM/dd HH:mm:ss.fff") ?? "";
        public string REJECT_REASON { get; set; } = "";
        public string COMPLETE_STATUS { get; set; } = "";
        public virtual string CUSTOMER_TRANSFER_STATUS { get { return TRANSFER_STATUS.ToString(); } }
    }

    public static class VTRANSFER_Def
    {
        public enum TransferStatus
        {
            Undefined = -1,
            Queue = 0,
            PreInitial,
            Initial,
            Transferring,
            Paused,
            Canceling,
            Aborting,
            Canceled,
            Aborted,
            Complete,
            RouteChanging,
            Reject
        }

        public enum CommandState
        {
            #region Standard
            Undefined = -1,
            EnRoute = 1,
            LoadArrive = 2,
            Loading = 4,
            LoadComplete = 8,
            UnloadArrive = 16,
            Unloading = 32,
            UnloadComplete = 64,
            CommandFinish = 128,

            //Error
            Error_DoubleStorage = 256,      //二重格異常，要Unload時發現儲位內有其他Cassete
            Error_EmptyRetrieval = 512,     //空取異常，要Load時發現儲位是空的
            Error_InterlockError = 1024,    //交握異常
            Error_VehicleAbort = 2048,      //車子異常結束
            #endregion

            #region Customer
            //Customer K24
            k24_CST_TypeMismatch = 4096,
            k24_Inter_Error_When_Load = 8192,
            k24_Inter_Error_When_UnLoad = 16384,

            #endregion

        }
    }
}
