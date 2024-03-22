using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHTC_CSOT
{
    public static class VehicleInfo
    {
        public static string GetVhID(object vh_info_obj) => ((VEHICLE_INFO)vh_info_obj)?.VEHICLEID ?? "";
        public static string GetCmdID(object vh_info_obj) => ((VEHICLE_INFO)vh_info_obj)?.OHTCCMD ?? "";
        public static string GetCstID(object vh_info_obj) => ((VEHICLE_INFO)vh_info_obj)?.CSTID ?? "";

        public static bool Convert2Object_VehicleInfo(byte[] bytes, out object vh_info, out string result)
        {
            vh_info = null;
            result = "";
            string _doing = "";
            try
            {
                _doing = "com.mirle.ibg3k0.sc.BLL.VehicleBLL.Convert2Object_VehicleInfo";
                vh_info = com.mirle.ibg3k0.sc.BLL.VehicleBLL.Convert2Object_VehicleInfo(bytes);

                return true;
            }
            catch
            {
                result = $"Exception occurred while {_doing}";
                return false;
            }
        }
        public static bool Convert2Object_VehicleInfo(string input, out object vh_info, out string result)
        {
            vh_info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_VehicleInfo(bytes, out vh_info, out result);
                if (!isSuccess) return false;
            }

            return true;
        }

        public static bool SetVVEHICLE(ref ViewerObject.VVEHICLE vh, object vh_info_obj, out string result)
        {
            result = "";

            VEHICLE_INFO vh_info = vh_info_obj as VEHICLE_INFO;
            if (vh_info == null)
            {
                result = "vh_info = null or not typeof VEHICLE_INFO";
                return false;
            }

            string vh_id = vh_info.VEHICLEID?.Trim();
            if (vh.VEHICLE_ID != vh_id)
            {
                result = $"vh_id not same ({vh.VEHICLE_ID} != {vh_id})";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "INSTALL_STATUS";
                vh.INSTALL_STATUS = vh_info.ISINSTALLED ? ViewerObject.VVEHICLE_Def.InstallStatus.Installed :
                                                       ViewerObject.VVEHICLE_Def.InstallStatus.Removed;

                _doing = "IS_TCPIP_CONNECT";
                vh.IS_TCPIP_CONNECT = vh_info.IsTcpIpConnect;

                _doing = "MODE_STATUS";
                vh.MODE_STATUS = getModeStatus(vh_info.MODESTATUS);

                _doing = "ACT_STATUS";
                vh.ACT_STATUS = getActionStatus(vh_info.ACTSTATUS);
                _doing = "IS_LOADING";
                vh.IS_LOADING = vh_info.VhRecentTranEvent == EventType.Vhloading;
                _doing = "IS_UNLOADING";
                vh.IS_UNLOADING = vh_info.VhRecentTranEvent == EventType.Vhunloading;
                _doing = "SetParkStatus";
                vh.SetParkStatus(vh_info.ISPARKING, vh_info.PARKADRID);

                _doing = "HAS_ERROR";
                vh.HAS_ERROR = vh_info.ERROR == VhStopSingle.StopSingleOn;

                _doing = "IS_PAUSE";
                vh.IS_PAUSE = vh_info.PauseStatus == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_BLOCK";
                vh.IS_PAUSE_BLOCK = vh_info.BLOCKPAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_HID";
                vh.IS_PAUSE_HID = vh_info.HIDPAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_OBS";
                vh.IS_PAUSE_OBS = vh_info.OBSPAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_EARTHQUAKE";
                vh.IS_PAUSE_EARTHQUAKE = vh_info.EARTHQUAKEPAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_SAFETY_DOOR";
                vh.IS_PAUSE_SAFETY_DOOR = vh_info.SAFETYDOORPAUSE == VhStopSingle.StopSingleOn;

                _doing = "SetLocation";
                vh.SetLocation(vh_info.CURADRID, vh_info.CURSECID, vh_info.ACCSECDIST);
                _doing = "SetPosition";
                vh.SetPosition(vh_info.XAxis, vh_info.YAxis);

                _doing = "TRANSFER_ID_1";
                vh.TRANSFER_ID_1 = vh_info.MCSCMD;
                _doing = "CMD_ID_1";
                vh.CMD_ID_1 = vh_info.OHTCCMD;
                _doing = "CURRENT_EXCUTE_CMD_ID";
                vh.CURRENT_EXCUTE_CMD_ID = vh_info.OHTCCMD;
                _doing = "CMD_TYPE";
                vh.CMD_TYPE = getCmdType(vh_info.CmdType);

                _doing = "SetCST";
                vh.SetCST(vh_info.HASCST == 1, vh_info.CSTID);

                _doing = "SetPath";
                vh.SetPath(vh_info.StartAdr, vh_info.FromAdr, vh_info.ToAdr,
                            vh_info.PredictPath?.ToList(), vh_info.WillPassSectionID?.ToList(), null);

                _doing = "SPEED";
                vh.SPEED = vh_info.Speed;

                //_doing = "BATTERY_CAPACITY";
                //vh.BATTERY_CAPACITY = vh_info.BatteryCapacity
                //_doing = "BATTERY_LEVEL";
                //vh.BATTERY_LEVEL = vh_info.BatteryLevel

                _doing = "onStatusChange";
                vh.onStatusChange();
                
                return true;
            }
            catch
            {
                result = $"Exception occurred while {_doing}";
                return false;
            }
        }

        private static ViewerObject.VVEHICLE_Def.ModeStatus getModeStatus(VHModeStatus vHModeStatus)
        {
            switch (vHModeStatus)
            {
                case VHModeStatus.AutoMtl:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.AutoMtl;
                case VHModeStatus.AutoMts:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.AutoMts;
                case VHModeStatus.AutoRemote:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.AutoRemote;
                case VHModeStatus.AutoLocal:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.AutoLocal;
                case VHModeStatus.Manual:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.Manual;
                case VHModeStatus.InitialPowerOn:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.InitialPowerOn;
                case VHModeStatus.InitialPowerOff:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.InitialPowerOff;
                case VHModeStatus.None:
                default:
                    return ViewerObject.VVEHICLE_Def.ModeStatus.None;
            }
        }

        private static ViewerObject.VVEHICLE_Def.ActionStatus getActionStatus(VHActionStatus vHActionStatus)
        {
            switch (vHActionStatus)
            {
                case VHActionStatus.CycleRun:
                    return ViewerObject.VVEHICLE_Def.ActionStatus.CycleRun;
                case VHActionStatus.GripperTeaching:
                    return ViewerObject.VVEHICLE_Def.ActionStatus.GripperTeaching;
                case VHActionStatus.Teaching:
                    return ViewerObject.VVEHICLE_Def.ActionStatus.Teaching;
                case VHActionStatus.Commanding:
                    return ViewerObject.VVEHICLE_Def.ActionStatus.Commanding;
                case VHActionStatus.NoCommand:
                default:
                    return ViewerObject.VVEHICLE_Def.ActionStatus.NoCommand;
            }
        }

        private static ViewerObject.VCMD_Def.CmdType getCmdType(CommandType commandType)
        {
            switch (commandType)
            {
                case CommandType.CmdOverride:
                    return ViewerObject.VCMD_Def.CmdType.Override;
                case CommandType.CmdHome:
                    return ViewerObject.VCMD_Def.CmdType.Home;
                case CommandType.CmdRound:
                    return ViewerObject.VCMD_Def.CmdType.Round;
                case CommandType.CmdContinue:
                    return ViewerObject.VCMD_Def.CmdType.Continue;
                case CommandType.CmdTeaching:
                    return ViewerObject.VCMD_Def.CmdType.Teaching;
                case CommandType.CmdLoadUnload:
                    return ViewerObject.VCMD_Def.CmdType.LoadUnload;
                case CommandType.CmdUnload:
                    return ViewerObject.VCMD_Def.CmdType.Unload;
                case CommandType.CmdLoad:
                    return ViewerObject.VCMD_Def.CmdType.Load;
                case CommandType.CmdMoveMtport:
                    return ViewerObject.VCMD_Def.CmdType.MoveMtport;
                case CommandType.CmdMovePark:
                    return ViewerObject.VCMD_Def.CmdType.MovePark;
                case CommandType.CmdMove:
                default:
                    return ViewerObject.VCMD_Def.CmdType.Move;
            }
        }
    }

    public static class Addition
    {
        public static bool AddXY_VEHICLE_INFO(ref object vh_info_obj, List<ViewerObject.Address> addresses, List<ViewerObject.Section> sections)
        {
            VEHICLE_INFO vh = vh_info_obj as VEHICLE_INFO;
            if (vh == null) return false;
            if (vh.XAxis != 0 || vh.YAxis != 0) return false;
            if (addresses == null || addresses.Count == 0) return false;
            if (sections == null || sections.Count == 0) return false;
            try
            {
                (double x, double y, float angle) vh_axis = ViewerObject.VVEHICLE_Addition.GetVhAxis(vh.CURADRID, vh.CURSECID, vh.ACCSECDIST, addresses, sections);
                vh.XAxis = vh_axis.x;
                vh.YAxis = vh_axis.y;
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }
    }
}
