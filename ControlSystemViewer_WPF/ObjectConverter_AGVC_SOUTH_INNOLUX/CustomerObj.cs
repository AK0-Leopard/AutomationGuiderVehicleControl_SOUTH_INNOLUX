using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace ObjectConverter_AGVC_SOUTH_INNOLUX
{

    public class AGVC_SOUTH_INNOLUX_VTRANSFER : VTRANSFER
    {
        public AGVC_SOUTH_INNOLUX_VTRANSFER(VTRANSFER transfer)
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
        }
        const string CONVER_TO_DISPLAY_INITIAL = "From Move";
        const string CONVER_TO_DISPLAY_TRAN = "To Move";
        const string CONVER_TO_DISPLAY_LOADING_UNLOADING = "Working";
        //d
        public override string CUSTOMER_TRANSFER_STATUS
        {
            get
            {
                string display_name = TRANSFER_STATUS.ToString();
                if (isLoading || isUnloading)
                {
                    return CONVER_TO_DISPLAY_LOADING_UNLOADING;
                }
                else
                {
                    switch (TRANSFER_STATUS)
                    {
                        case VTRANSFER_Def.TransferStatus.Initial:
                            return CONVER_TO_DISPLAY_INITIAL;
                        case VTRANSFER_Def.TransferStatus.Transferring:
                            return CONVER_TO_DISPLAY_TRAN;
                        default:
                            return TRANSFER_STATUS.ToString();
                    }
                }
            }
        }
        public const int COMMAND_STATUS_BIT_INDEX_LOADING = 4;
        public const int COMMAND_STATUS_BIT_INDEX_UNLOADING = 32;

        public bool isLoading
        {
            get
            {
                var command_state = (int)COMMANDSTATE & 252;
                return command_state == COMMAND_STATUS_BIT_INDEX_LOADING;
            }
        }
        public bool isUnloading
        {

            get
            {
                var command_state = (int)COMMANDSTATE & 224;
                return command_state == COMMAND_STATUS_BIT_INDEX_UNLOADING;
            }
        }


    }

    public class AGVC_SOUTH_INNOLUX_Coupler : Coupler
    {
        public AGVC_SOUTH_INNOLUX_Coupler(int _id, string _Name, string _ChargerID) : base(_id, _Name, _ChargerID)
        {

        }
        public override string ShowStatus { get; set; }
    }

}
