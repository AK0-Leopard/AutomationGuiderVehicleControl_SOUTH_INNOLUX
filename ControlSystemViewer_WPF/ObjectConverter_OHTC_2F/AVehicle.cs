using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHTC_2F
{
    public class AVehicle : IAVehicle
    {
        private readonly string ns = "ObjectConverter_OHTC_2F" + ".AVehicle";
        Definition.Convert defCvt = new Definition.Convert();

        public bool Convert2Object_AVEHICLEs(string input, out List<AVEHICLE> obj, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            obj = null;
            result = "";

            if (string.IsNullOrWhiteSpace(input))
            {
                result = $"{ns}.{ms} - input = null or empty";
                return false;
            }

            try
            {
                obj = JsonConvert.DeserializeObject<List<AVEHICLE>>(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<AVEHICLE>> result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<AVEHICLE>> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetVVEHICLEs(ref List<ViewerObject.VVEHICLE> vhs, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            string _doing = "";
            try
            {
                _doing = "Convert data to List<AVEHICLE>";
                bool isSuccess = Convert2Object_AVEHICLEs(data, out List<AVEHICLE> avhs, out result);
                if (!isSuccess) return false;

                _doing = "Init List<ViewerObject.VVEHICLE> by Distinct VEHICLE_IDs in List<AVEHICLE>";
                if (vhs == null) vhs = new List<ViewerObject.VVEHICLE>();
                else vhs.Clear();
                var vhIDs = avhs.Select(i => i.VEHICLE_ID.Trim()).Distinct().OrderBy(vhid => vhid).ToList();
                foreach (string vhid in vhIDs)
                {
                    vhs.Add(new ViewerObject.VVEHICLE(vhid));
                }

                _doing = "Set each VVEHICLE by VEHICLE_INFO";
                foreach (var avh in avhs)
                {
                    ViewerObject.VVEHICLE vh = vhs.Where(v => v.VEHICLE_ID == avh.VEHICLE_ID.Trim()).Single();
                    if (!SetVVEHICLE(ref vh, avh, out result))
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
        public bool SetVVEHICLE(ref ViewerObject.VVEHICLE vh, AVEHICLE avh, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (avh == null)
            {
                result = $"{ns}.{ms} - avh = null";
                return false;
            }

            string id = avh.VEHICLE_ID?.Trim() ?? "";
            if (vh.VEHICLE_ID != id)
            {
                result = $"{ns}.{ms} - id not same ({vh.VEHICLE_ID} != {id})";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "INSTALL_STATUS";
                vh.INSTALL_STATUS = avh.IS_INSTALLED ? ViewerObject.VVEHICLE_Def.InstallStatus.Installed :
                                                       ViewerObject.VVEHICLE_Def.InstallStatus.Removed;

                _doing = "IS_TCPIP_CONNECT";
                vh.IS_TCPIP_CONNECT = avh.isTcpIpConnect;

                _doing = "MODE_STATUS";
                vh.MODE_STATUS = defCvt.GetModeStatus(avh.MODE_STATUS);

                _doing = "ACT_STATUS";
                vh.ACT_STATUS = defCvt.GetActionStatus(avh.ACT_STATUS);
                //_doing = "IS_LOADING";
                //vh.IS_LOADING = avh.VhRecentTranEvent == EventType.Vhloading;
                //_doing = "IS_UNLOADING";
                //vh.IS_UNLOADING = avh.VhRecentTranEvent == EventType.Vhunloading;
                //_doing = "SetParkStatus";
                //vh.SetParkStatus(avh.IS_PARKING, avh.PARK_ADR_ID);

                _doing = "HAS_ERROR";
                vh.HAS_ERROR = avh.ERROR == VhStopSingle.StopSingleOn;

                _doing = "IS_PAUSE_BLOCK";
                vh.IS_PAUSE_BLOCK = avh.BLOCK_PAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_HID";
                vh.IS_PAUSE_HID = avh.HID_PAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_OBS";
                vh.IS_PAUSE_OBS = avh.OBS_PAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_EARTHQUAKE";
                vh.IS_PAUSE_EARTHQUAKE = avh.EARTHQUAKE_PAUSE == VhStopSingle.StopSingleOn;
                _doing = "IS_PAUSE_SAFETY_DOOR";
                vh.IS_PAUSE_SAFETY_DOOR = avh.SAFETY_PAUSE == VhStopSingle.StopSingleOn;

                _doing = "SetLocation";
                vh.SetLocation(avh.CUR_ADR_ID, avh.CUR_SEC_ID, avh.ACC_SEC_DIST);
                _doing = "SetPosition";
                vh.SetPosition(avh.X_Axis, avh.Y_Axis);

                _doing = "TRANSFER_ID_1";
                vh.TRANSFER_ID_1 = avh.TRANSFER_ID;
                _doing = "CMD_ID_1";
                vh.CMD_ID_1 = avh.CMD_ID;
                _doing = "CURRENT_EXCUTE_CMD_ID";
                vh.CURRENT_EXCUTE_CMD_ID = avh.CurrentExcuteCmdID;
                _doing = "CMD_TYPE";
                vh.CMD_TYPE = defCvt.GetCmdType(avh.CmdType);

                _doing = "SetCST";
                vh.SetCST(avh.HAS_CST, avh.CST_ID);

                _doing = "SetPath";
                vh.SetPath(avh.StartAdr, avh.FromAdr, avh.ToAdr,
                            avh.PredictSections?.ToList(), avh.WillPassSectionID?.ToList(), null);// avh.ReservedSectionID?.ToList());

                _doing = "SPEED";
                vh.SPEED = avh.Speed;

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
