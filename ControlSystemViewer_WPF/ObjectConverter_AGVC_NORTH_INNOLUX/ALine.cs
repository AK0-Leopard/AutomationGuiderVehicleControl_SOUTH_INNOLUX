using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_NORTH_INNOLUX
{
    public class ALine : IALine
    {
        private readonly string ns = "ObjectConverter_AGVC_NORTH_INNOLUX" + ".ALine";

        public bool Convert2Object_ALINE(string input, out ALINE obj, out string result)
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
                obj = JsonConvert.DeserializeObject<ALINE>(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<ALINE> result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<ALINE> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetVLINE_byString(ref ViewerObject.VLINE line, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            string _doing = "";
            try
            {
                _doing = "Convert data to ALINE";
                bool isSuccess = Convert2Object_ALINE(data, out ALINE aline, out result);
                if (!isSuccess) return false;

                _doing = "Set VLINE by ALINE";
                return SetVLINE(ref line, aline, out result);
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVLINE(ref ViewerObject.VLINE line, ALINE aline, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (aline == null)
            {
                result = $"{ns}.{ms} - aline = null";
                return false;
            }

            if (line == null)
            {
                line = new ViewerObject.VLINE();
            }
            else
            {
                string id = aline.LINE_ID?.Trim() ?? "";
                if (line.LINE_ID != id)
                {
                    result = $"{ns}.{ms} - port_id not same ({line.LINE_ID} != {id})";
                    return false;
                }
            }

            string _doing = "";
            try
            {
                Definition.Convert defCvt = new Definition.Convert();

                _doing = "LINE_ID";
                line.LINE_ID = aline.LINE_ID;
                _doing = "VERSION";
                line.VERSION = aline.Version;
                _doing = "IS_ALARM_HAPPEN";
                line.IS_ALARM_HAPPEN = aline.IsAlarmHappened;
                _doing = "IS_LINK_HOST";
                line.IS_LINK_HOST = aline.Secs_Link_Stat == com.mirle.ibg3k0.sc.App.SCAppConstants.LinkStatus.LinkOK;
                _doing = "HOST_CONTROL_STATE";
                line.HOST_CONTROL_STATE = defCvt.GetHostControlState(aline.Host_Control_State);
                _doing = "TSC_STATE";
                line.TSC_STATE = defCvt.GetTSCState(aline.SCStats);
                _doing = "IS_MCS_CMD_AUTO_ASSIGN";
                line.IS_MCS_CMD_AUTO_ASSIGN = aline.MCSCommandAutoAssign;
                _doing = "COUNT_HOST_CMD_TRANSFER_STATE_WAITING";
                line.COUNT_HOST_CMD_TRANSFER_STATE_WAITING = aline.CurrntHostCommandTransferStatueWaitingCounr;
                _doing = "COUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED";
                line.COUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED = aline.CurrntHostCommandTransferStatueAssignedCount;
                _doing = "COUNT_CST_STATE_WAITING";
                line.COUNT_CST_STATE_WAITING = aline.CurrntCSTStatueWaitingCount;
                _doing = "COUNT_CST_STATE_TRANSFER";
                line.COUNT_CST_STATE_TRANSFER = aline.CurrntCSTStatueTransferCount;
                _doing = "COUNT_VEHICLE_MODE_AUTO_REMOTE";
                line.COUNT_VEHICLE_MODE_AUTO_REMOTE = aline.CurrntVehicleModeAutoRemoteCount;
                _doing = "COUNT_VEHICLE_MODE_AUTO_LOCAL";
                line.COUNT_VEHICLE_MODE_AUTO_LOCAL = aline.CurrntVehicleModeAutoLoaclCount;
                _doing = "COUNT_VEHICLE_MODE_IDLE";
                line.COUNT_VEHICLE_MODE_IDLE = aline.CurrntVehicleStatusIdelCount;
                _doing = "COUNT_VEHICLE_MODE_ERROR";
                line.COUNT_VEHICLE_MODE_ERROR = aline.CurrntVehicleStatusErrorCount;
                _doing = "IS_CHECKED_CURR_PORT_STATE";
                line.IS_CHECKED_CURR_PORT_STATE = aline.CurrentPortStateChecked;
                _doing = "IS_CHECKED_CURR_STATE";
                line.IS_CHECKED_CURR_STATE = aline.CurrentStateChecked;
                _doing = "IS_CHECKED_TSC_STATE";
                line.IS_CHECKED_TSC_STATE = aline.TSCStateChecked;
                _doing = "IS_CHECKED_ENHANCED_VEHICLES";
                line.IS_CHECKED_ENHANCED_VEHICLES = aline.EnhancedVehiclesChecked;
                _doing = "IS_CHECKED_ENHANCED_TRANSFERS";
                line.IS_CHECKED_ENHANCED_TRANSFERS = aline.EnhancedTransfersChecked;
                _doing = "IS_CHECKED_ENHANCED_CARRIERS";
                line.IS_CHECKED_ENHANCED_CARRIERS = aline.EnhancedCarriersChecked;
                _doing = "IS_CONNECT_MCS";
                line.IS_CONNECT_MCS = aline.MCSConnectionSuccess;
                _doing = "IS_CONNECT_ROUTER";
                line.IS_CONNECT_ROUTER = aline.RouterConnectionSuccess;
                _doing = "IS_CONNECT_CHARGER_PLC";
                line.IS_CONNECT_CHARGER_PLC = aline.ChargePLCConnectionSuccess;

                _doing = "IS_CONNECT_APs";
                line.InitAPsConnectStatus(10);
                List<bool> allAPsConnectStatus = new List<bool>();
                allAPsConnectStatus.Add(aline.AP1ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP2ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP3ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP4ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP5ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP6ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP7ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP8ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP9ConnectionSuccess);
                allAPsConnectStatus.Add(aline.AP10ConnectionSuccess);
                line.SetAPsConnectStatus(allAPsConnectStatus);

                _doing = "IS_CONNECT_ADAMs";
                line.InitADAMsConnectStatus(5);
                List<bool> allADAMsConnectStatus = new List<bool>();
                allADAMsConnectStatus.Add(aline.ADAM1ConnectionSuccess);
                allADAMsConnectStatus.Add(aline.ADAM2ConnectionSuccess);
                allADAMsConnectStatus.Add(aline.ADAM3ConnectionSuccess);
                allADAMsConnectStatus.Add(aline.ADAM4ConnectionSuccess);
                allADAMsConnectStatus.Add(aline.ADAM5ConnectionSuccess);
                line.SetADAMsConnectStatus(allADAMsConnectStatus);

                _doing = "IS_CONNECT_VEHICLEs";
                line.InitVehiclesConnectStatus(14);
                List<bool> allVehiclesConnectStatus = new List<bool>();
                allVehiclesConnectStatus.Add(aline.AGV1ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV2ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV3ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV4ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV5ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV6ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV7ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV8ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV9ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV10ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV11ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV12ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV13ConnectionSuccess);
                allVehiclesConnectStatus.Add(aline.AGV14ConnectionSuccess);
                line.SetVehiclesConnectStatus(allVehiclesConnectStatus);

                _doing = "onStatusChange";
                line.onStatusChange();

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
