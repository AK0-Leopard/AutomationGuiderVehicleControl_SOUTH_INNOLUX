﻿using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.ObjectRelay
{
    public class CMD_MCSObjToShow
    {
        //public static App.SCApplication app = App.SCApplication.getInstance();
        public BLL.PortStationBLL PortStationBLL = null;
        public BLL.VehicleBLL VehicleBLL = null;
        public ACMD_MCS cmd_mcs = null;

        public CMD_MCSObjToShow()
        {
        }
        public CMD_MCSObjToShow(BLL.PortStationBLL portStationBLL, ACMD_MCS _cmd_mcs)
        {
            cmd_mcs = _cmd_mcs;
            PortStationBLL = portStationBLL;
        }
        public CMD_MCSObjToShow(BLL.VehicleBLL vehicleBLL, BLL.PortStationBLL portStationBLL, ACMD_MCS _cmd_mcs)
        {
            cmd_mcs = _cmd_mcs;
            VehicleBLL = vehicleBLL;
            PortStationBLL = portStationBLL;
        }
        public string CMD_ID { get { return sc.Common.SCUtility.Trim(cmd_mcs.CMD_ID, true); } }
        public string CARRIER_ID { get { return sc.Common.SCUtility.Trim(cmd_mcs.CARRIER_ID, true); } }
        public string VEHICLE_ID
        {
            get
            {
                if (VehicleBLL != null)
                {
                    var vh = VehicleBLL.cache.getVehicleByMCSCmdID(CMD_ID);
                    return vh == null ? "" : vh.VEHICLE_ID;
                }
                else
                    return "";
            }
        }
        const string CONVER_TO_DISPLAY_INITIAL = "From Move";
        const string CONVER_TO_DISPLAY_TRAN = "To Move";
        const string CONVER_TO_DISPLAY_LOADING_UNLOADING = "Working";
        public E_TRAN_STATUS TRANSFERSTATE { get { return cmd_mcs.TRANSFERSTATE; } }
        public string TRANSFERSTATEToShow
        {
            get
            {
                string display_name = cmd_mcs.TRANSFERSTATE.ToString();
                if (cmd_mcs.isLoading || cmd_mcs.isUnloading)
                {
                    return CONVER_TO_DISPLAY_LOADING_UNLOADING;
                }
                else
                {
                    switch (cmd_mcs.TRANSFERSTATE)
                    {
                        case E_TRAN_STATUS.Initial:
                            return CONVER_TO_DISPLAY_INITIAL;
                        case E_TRAN_STATUS.Transferring:
                            return CONVER_TO_DISPLAY_TRAN;
                        default:
                            return cmd_mcs.TRANSFERSTATE.ToString();
                    }
                }
                //return cmd_mcs.TRANSFERSTATE;
            }
        }
        public string HOSTSOURCE
        {
            get
            {
                var portstation = PortStationBLL.OperateCatch.getPortStation(cmd_mcs.HOSTSOURCE);
                return portstation == null ? sc.Common.SCUtility.Trim(cmd_mcs.HOSTSOURCE,true) : portstation.ToString();
            }
        }
        public string HOSTDESTINATION
        {
            get
            {
                var portstation = PortStationBLL.OperateCatch.getPortStation(cmd_mcs.HOSTDESTINATION);
                return portstation == null ? sc.Common.SCUtility.Trim(cmd_mcs.HOSTDESTINATION,true) : portstation.ToString();
            }
        }

        //public int PRIORITY { get { return cmd_mcs.PRIORITY; } }
        public int PRIORITY
        {
            get
            {
                int priority = cmd_mcs.PRIORITY_SUM > 99 ? 99 : cmd_mcs.PRIORITY_SUM;
                return priority;
            }
        }
        public System.DateTime CMD_INSER_TIME { get { return cmd_mcs.CMD_INSER_TIME; } }
        public Nullable<System.DateTime> CMD_START_TIME { get { return cmd_mcs.CMD_START_TIME; } }
        public Nullable<System.DateTime> CMD_FINISH_TIME { get { return cmd_mcs.CMD_FINISH_TIME; } }
        public int REPLACE { get { return cmd_mcs.REPLACE; } }
        public bool isLoading
        {
            get { return (cmd_mcs.isLoading); }
        }
        public bool isUnloading
        {
            get { return (cmd_mcs.isUnloading); }
        }

    }
}
