using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_AUO_800
{
    public class LineInfo : ILineInfo
    {
        private readonly string ns = "ObjectConverter_AGVC_AUO_800" + ".LineInfo";
        Definition.Convert defCvt = new Definition.Convert();

        public string GetLineID(object obj) => ((LINE_INFO)obj)?.LineID ?? "";

        public bool Convert2Object_LineInfo(byte[] bytes, out object info, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            info = null;
            result = "";
            string _doing = "";
            try
            {
                _doing = "com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_LineInfo";
                info = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_LineInfo(bytes);

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool Convert2Object_LineInfo(string input, out object info, out string result)
        {
            info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_LineInfo(bytes, out info, out result);
                return isSuccess;
            }
        }
        public bool Convert2Object_TransferInfo(byte[] bytes, out object info, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            info = null;
            result = "";
            string _doing = "";
            try
            {
                _doing = "com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TransferInfo";
                info = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TransferInfo(bytes);

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool Convert2Object_TransferInfo(string input, out object info, out string result)
        {
            info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_TransferInfo(bytes, out info, out result);
                return isSuccess;
            }
        }
        public bool Convert2Object_OnlineCheckInfo(byte[] bytes, out object info, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            info = null;
            result = "";
            string _doing = "";
            try
            {
                _doing = "com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_OnlineCheckInfo";
                info = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_OnlineCheckInfo(bytes);

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool Convert2Object_OnlineCheckInfo(string input, out object info, out string result)
        {
            info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_OnlineCheckInfo(bytes, out info, out result);
                return isSuccess;
            }
        }
        public bool Convert2Object_PingCheckInfo(byte[] bytes, out object info, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            info = null;
            result = "";
            string _doing = "";
            try
            {
                _doing = "com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_PingCheckInfo";
                info = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_PingCheckInfo(bytes);

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool Convert2Object_PingCheckInfo(string input, out object info, out string result)
        {
            info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_PingCheckInfo(bytes, out info, out result);
                return isSuccess;
            }
        }

        public bool SetVLINE_by_LINE_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            if (line == null) line = new ViewerObject.VLINE();

            if (bytes == null)
            {
                result = $"{ns}.{ms} - bytes = null";
                return false;
            }
            if (!Convert2Object_LineInfo(bytes, out object obj, out result))
                return false;

            LINE_INFO info = (LINE_INFO)obj;
            if (info == null)
            {
                result = $"{ns}.{ms} - info = null or not typeof LINE_INFO";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "LINE_ID";
                line.LINE_ID = info.LineID;
                _doing = "IS_LINK_HOST";
                line.IS_LINK_HOST = info.Host == LinkStatus.LinkOk;
                _doing = "IS_LINK_PLC";
                line.IS_LINK_PLC = info.PLC == LinkStatus.LinkOk;
                _doing = "HOST_CONTROL_STATE";
                line.HOST_CONTROL_STATE = defCvt.GetHostControlState(info.HostMode);
                _doing = "TSC_STATE";
                line.TSC_STATE = defCvt.GetTSCState(info.TSCState);
                _doing = "IS_ALARM_HAPPEN";
                line.IS_ALARM_HAPPEN = info.AlarmHappen;
                _doing = "COUNT_HOST_CMD_TRANSFER_STATE_WAITING";
                line.COUNT_HOST_CMD_TRANSFER_STATE_WAITING = info.CurrntHostCommandTransferStatueWaitingCounr;
                _doing = "COUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED";
                line.COUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED = info.CurrntHostCommandTransferStatueAssignedCount;
                _doing = "COUNT_CST_STATE_WAITING";
                line.COUNT_CST_STATE_WAITING = info.CurrntCSTStatueWaitingCount;
                _doing = "COUNT_CST_STATE_TRANSFER";
                line.COUNT_CST_STATE_TRANSFER = info.CurrntCSTStatueTransferCount;
                _doing = "COUNT_VEHICLE_MODE_AUTO_REMOTE";
                line.COUNT_VEHICLE_MODE_AUTO_REMOTE = info.CurrntVehicleModeAutoRemoteCount;
                _doing = "COUNT_VEHICLE_MODE_AUTO_LOCAL";
                line.COUNT_VEHICLE_MODE_AUTO_LOCAL = info.CurrntVehicleModeAutoLoaclCount;
                _doing = "COUNT_VEHICLE_MODE_IDLE";
                line.COUNT_VEHICLE_MODE_IDLE = info.CurrntVehicleStatusIdelCount;
                _doing = "COUNT_VEHICLE_MODE_ERROR";
                line.COUNT_VEHICLE_MODE_ERROR = info.CurrntVehicleStatusErrorCount;

                _doing = "onLineInfoChange";
                line.onLineInfoChange();

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVLINE_by_CONNECTION_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (bytes == null)
            {
                result = $"{ns}.{ms} - bytes = null";
                return false;
            }
            if (!Convert2Object_LineInfo(bytes, out object obj, out result))
                return false;

            LINE_INFO info = (LINE_INFO)obj;
            if (info == null)
            {
                result = $"{ns}.{ms} - info = null or not typeof LINE_INFO";
                return false;
            }

            string _doing = "";
            try
            {
                bool changed = false;

                _doing = "IS_LINK_HOST";
                var is_link_host = info.Host == LinkStatus.LinkOk;
                if (line.IS_LINK_HOST != is_link_host)
                {
                    line.IS_LINK_HOST = is_link_host;
                    changed = true;
                }

                _doing = "HOST_CONTROL_STATE";
                var host_control_state = defCvt.GetHostControlState(info.HostMode);
                if (line.HOST_CONTROL_STATE != host_control_state)
                {
                    line.HOST_CONTROL_STATE = host_control_state;
                    changed = true;
                }

                _doing = "TSC_STATE";
                var tsc_state = defCvt.GetTSCState(info.TSCState);
                if (line.TSC_STATE != tsc_state)
                {
                    line.TSC_STATE = tsc_state;
                    changed = true;
                }

                if (changed)
                {
                    _doing = "onConnectionInfoChange";
                    line.onConnectionInfoChange();
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVLINE_by_TRANSFER_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (bytes == null)
            {
                result = $"{ns}.{ms} - bytes = null";
                return false;
            }
            if (!Convert2Object_TransferInfo(bytes, out object obj, out result))
                return false;

            TRANSFER_INFO info = (TRANSFER_INFO)obj;
            if (info == null)
            {
                result = $"{ns}.{ms} - info = null or not typeof TRANSFER_INFO";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "IS_MCS_CMD_AUTO_ASSIGN";
                line.IS_MCS_CMD_AUTO_ASSIGN = info.MCSCommandAutoAssign;

                _doing = "onTransferInfoChange";
                line.onTransferInfoChange();

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVLINE_by_ONLINE_CHECK_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (bytes == null)
            {
                result = $"{ns}.{ms} - bytes = null";
                return false;
            }
            if (!Convert2Object_OnlineCheckInfo(bytes, out object obj, out result))
                return false;

            ONLINE_CHECK_INFO info = (ONLINE_CHECK_INFO)obj;
            if (info == null)
            {
                result = $"{ns}.{ms} - info = null or not typeof ONLINE_CHECK_INFO";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "IS_CHECKED_CURR_PORT_STATE";
                line.IS_CHECKED_CURR_PORT_STATE = info.CurrentPortStateChecked;
                _doing = "IS_CHECKED_CURR_STATE";
                line.IS_CHECKED_CURR_STATE = info.CurrentStateChecked;
                _doing = "IS_CHECKED_TSC_STATE";
                line.IS_CHECKED_TSC_STATE = info.TSCStateChecked;
                _doing = "IS_CHECKED_ENHANCED_VEHICLES";
                line.IS_CHECKED_ENHANCED_VEHICLES = info.EnhancedVehiclesChecked;
                _doing = "IS_CHECKED_ENHANCED_TRANSFERS";
                line.IS_CHECKED_ENHANCED_TRANSFERS = info.EnhancedTransfersChecked;
                _doing = "IS_CHECKED_ENHANCED_CARRIERS";
                line.IS_CHECKED_ENHANCED_CARRIERS = info.EnhancedCarriersChecked;

                _doing = "onOnlineCheckInfoChange";
                line.onOnlineCheckInfoChange();

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVLINE_by_PING_CHECK_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (bytes == null)
            {
                result = $"{ns}.{ms} - bytes = null";
                return false;
            }
            if (!Convert2Object_PingCheckInfo(bytes, out object obj, out result))
                return false;

            PING_CHECK_INFO info = (PING_CHECK_INFO)obj;
            if (info == null)
            {
                result = $"{ns}.{ms} - info = null or not typeof PING_CHECK_INFO";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "IS_CONNECT_MCS";
                line.IS_CONNECT_MCS = info.MCSConnectionSuccess;
                _doing = "IS_CONNECT_ROUTER";
                line.IS_CONNECT_ROUTER = info.RouterConnectionSuccess;
                _doing = "IS_CONNECT_CHARGER_PLC";
                line.IS_CONNECT_CHARGER_PLC = info.ChargePLCConnectionSuccess;

                _doing = "IS_CONNECT_APs";
                line.InitAPsConnectStatus(10);
                List<bool> allAPsConnectStatus = new List<bool>();
                allAPsConnectStatus.Add(info.AP1ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP2ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP3ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP4ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP5ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP6ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP7ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP8ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP9ConnectionSuccess);
                allAPsConnectStatus.Add(info.AP10ConnectionSuccess);
                line.SetAPsConnectStatus(allAPsConnectStatus);

                _doing = "IS_CONNECT_ADAMs";
                line.InitADAMsConnectStatus(5);
                List<bool> allADAMsConnectStatus = new List<bool>();
                allADAMsConnectStatus.Add(info.ADAM1ConnectionSuccess);
                allADAMsConnectStatus.Add(info.ADAM2ConnectionSuccess);
                allADAMsConnectStatus.Add(info.ADAM3ConnectionSuccess);
                allADAMsConnectStatus.Add(info.ADAM4ConnectionSuccess);
                allADAMsConnectStatus.Add(info.ADAM5ConnectionSuccess);
                line.SetADAMsConnectStatus(allADAMsConnectStatus);

                _doing = "IS_CONNECT_VEHICLEs";
                line.InitVehiclesConnectStatus(14);
                List<bool> allVehiclesConnectStatus = new List<bool>();
                allVehiclesConnectStatus.Add(info.AGV1ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV2ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV3ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV4ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV5ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV6ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV7ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV8ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV9ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV10ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV11ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV12ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV13ConnectionSuccess);
                allVehiclesConnectStatus.Add(info.AGV14ConnectionSuccess);
                line.SetVehiclesConnectStatus(allVehiclesConnectStatus);

                _doing = "onPingCheckInfoChange";
                line.onPingCheckInfoChange();

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
