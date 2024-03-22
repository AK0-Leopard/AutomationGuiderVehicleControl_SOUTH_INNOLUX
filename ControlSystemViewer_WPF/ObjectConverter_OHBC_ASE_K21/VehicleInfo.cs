using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K21
{
    public class VehicleInfo : IVehicleInfo
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K21" + ".VehicleInfo";
        Definition.Convert defCvt = new Definition.Convert();

        public string GetVhID(object vh_info_obj) => ((VEHICLE_INFO)vh_info_obj)?.VEHICLEID ?? "";
        public string GetCmdID(object vh_info_obj) => ((VEHICLE_INFO)vh_info_obj)?.OHTCCMD ?? "";
        public string GetCstID(object vh_info_obj) => ((VEHICLE_INFO)vh_info_obj)?.CSTID ?? "";

        public bool Convert2Object_VehicleInfo(byte[] bytes, out object vh_info, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
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
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool Convert2Object_VehicleInfo(string input, out object vh_info, out string result)
        {
            vh_info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_VehicleInfo(bytes, out vh_info, out result);
                return isSuccess;
            }
        }

        public bool SetVVEHICLEs(ref List<ViewerObject.VVEHICLE> vhs, List<byte[]> vh_info_data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (vh_info_data == null || vh_info_data.Count == 0)
            {
                result = $"{ns}.{ms} - vh_info_data = null or empty";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "Convert List<byte[]> to List<VEHICLE_INFO>";
                List<VEHICLE_INFO> vehicle_infos = new List<VEHICLE_INFO>();
                foreach (var data in vh_info_data)
                {
                    if (Convert2Object_VehicleInfo(data, out object vehicle_info_obj, out result))
                    {
                        vehicle_infos.Add((VEHICLE_INFO)vehicle_info_obj);
                    }
                    else return false;
                }

                _doing = "Init List<ViewerObject.VVEHICLE> by Distinct VEHICLEIDs in List<VEHICLE_INFO>";
                vhs = new List<ViewerObject.VVEHICLE>();
                var vhIDs = vehicle_infos.Select(i => i.VEHICLEID.Trim()).Distinct().OrderBy(vhid => vhid).ToList();
                foreach (string vhid in vhIDs)
                {
                    vhs.Add(new ViewerObject.VVEHICLE(vhid));
                }

                _doing = "Set each VVEHICLE by VEHICLE_INFO";
                foreach (var vehicle_info in vehicle_infos)
                {
                    ViewerObject.VVEHICLE vh = vhs.Where(v => v.VEHICLE_ID == vehicle_info.VEHICLEID.Trim()).Single();
                    if (!SetVVEHICLE(ref vh, vehicle_info, out result))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVVEHICLE(ref ViewerObject.VVEHICLE vh, object vh_info_obj, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            VEHICLE_INFO vh_info = vh_info_obj as VEHICLE_INFO;
            if (vh_info == null)
            {
                result = $"{ns}.{ms} - vh_info = null or not typeof VEHICLE_INFO";
                return false;
            }

            string vh_id = vh_info.VEHICLEID?.Trim();
            if (vh.VEHICLE_ID != vh_id)
            {
                result = $"{ns}.{ms} - vh_id not same ({vh.VEHICLE_ID} != {vh_id})";
                return false;
            }

            string _doing = "";
            try
            {
                //_doing = "INSTALL_STATUS"; // todo C提供INSTALL_STATUS 或 Viewer隱藏該欄位
                //vh.INSTALL_STATUS = vh_info.ISINSTALLED ? ViewerObject.VVEHICLE_Def.InstallStatus.Installed :
                //                                       ViewerObject.VVEHICLE_Def.InstallStatus.Removed;

                _doing = "IS_TCPIP_CONNECT";
                vh.IS_TCPIP_CONNECT = vh_info.IsTcpIpConnect;

                _doing = "MODE_STATUS";
                vh.MODE_STATUS = defCvt.GetModeStatus(vh_info.MODESTATUS);

                _doing = "ACT_STATUS";
                vh.ACT_STATUS = defCvt.GetActionStatus(vh_info.ACTSTATUS);
                _doing = "IS_LOADING";
                vh.IS_LOADING = vh_info.VhRecentTranEvent == EventType.Vhloading;
                _doing = "IS_UNLOADING";
                vh.IS_UNLOADING = vh_info.VhRecentTranEvent == EventType.Vhunloading;
                _doing = "SetParkStatus";
                vh.SetParkStatus(vh_info.ISPARKING, vh_info.PARKADRID);

                _doing = "HAS_ERROR";
                vh.HAS_ERROR = vh_info.ERROR == VhStopSingle.StopSingleOn;


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
                //_doing = "SetPosition";
                //vh.SetPosition(vh_info.XAxis, vh_info.YAxis); // todo C提供XY 或 Viewer用Location資料換算

                _doing = "TRANSFER_ID_1";
                vh.TRANSFER_ID_1 = vh_info.MCSCMD;
                _doing = "CMD_ID_1";
                vh.CMD_ID_1 = vh_info.OHTCCMD;
                _doing = "CURRENT_EXCUTE_CMD_ID";
                vh.CURRENT_EXCUTE_CMD_ID = vh_info.OHTCCMD;
                _doing = "CMD_TYPE";
                vh.CMD_TYPE = defCvt.GetCmdType(vh_info.CmdType);

                _doing = "SetCST";
                vh.SetCST(vh_info.HASCST == 1, vh_info.CSTID);

                _doing = "SetPath";
                vh.SetPath(vh_info.StartAdr, vh_info.FromAdr, vh_info.ToAdr,
                            vh_info.PredictPath?.ToList(), vh_info.WillPassSectionID?.ToList(), null);// vh_info.ReservedSectionID?.ToList());

                _doing = "SPEED";
                vh.SPEED = vh_info.Speed;

                _doing = "onStatusChange";
                vh.onStatusChange();

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
    }
}
