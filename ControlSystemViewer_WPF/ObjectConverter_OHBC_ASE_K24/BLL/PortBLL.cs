using CommonMessage.ProtocolFormat.ManualPortFun;
using Grpc.Core;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ObjectConverter_OHBC_ASE_K24.BLL
{
    public class PortBLL : IPortBLL
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K24.BLL" + ".PortBLL";

        private static readonly string control_web_address = "ohxcv.ha.ohxc.mirle.com.tw";
        private Channel channel = new Channel(control_web_address, 7003, ChannelCredentials.Insecure);

        private string connectionString = "";
        public PortBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public bool GetMonitoringPortsFromDB(out List<ViewerObject.VPORTSTATION> mPorts, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            mPorts = new List<ViewerObject.VPORTSTATION>();
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }

        public bool GetMonitoringPorts(out List<ViewerObject.VPORTSTATION> mPorts, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            mPorts = new List<ViewerObject.VPORTSTATION>();
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new manualPortGreeter.manualPortGreeterClient(channel);
                //建立提出要求的資料 - Empty
                //提出要求並回收server端給予的回應
                var reply = client.getAllManualPortInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.ManualPortInfo == null)
                {
                    result = $"{ns}.{ms} - reply.ManualPortInfo = null";
                    return false;
                }
                foreach (var info in reply.ManualPortInfo)
                {
                    var port = new ViewerObject.VPORTSTATION(info.ManualPortId, info.AddressID, 1);

                    port.IS_MONITORING = true;
                    port.IS_IN_SERVICE = !info.IsDown;
                    port.IS_INPUT_MODE = info.IsInMode;

                    // 只有一個 stage
                    port.SetLocPresenceCstID(info.LoadPosition1, info.CarrierIdOfStage1?.Trim() ?? "");

                    mPorts.Add(port);
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }
        
        public bool GetEqPorts(out List<ViewerObject.VPORTSTATION> eqPorts, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            eqPorts = new List<ViewerObject.VPORTSTATION>();
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new manualPortGreeter.manualPortGreeterClient(channel);
                //建立提出要求的資料 - Empty
                //提出要求並回收server端給予的回應
                var reply = client.getAllEQPortInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.EQPortInfo == null)
                {
                    result = $"{ns}.{ms} - reply.EQPortInfo = null";
                    return false;
                }
                foreach (var info in reply.EQPortInfo)
                {
                    eqPorts.Add(new ViewerObject.VPORTSTATION(info.EQPortID, info.EQPortAddress, 1));
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool UpdatePorts(ref List<ViewerObject.VPORTSTATION> ports, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new manualPortGreeter.manualPortGreeterClient(channel);
                //建立提出要求的資料 - Empty
                //提出要求並回收server端給予的回應
                var reply = client.getAllManualPortInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.ManualPortInfo == null)
                {
                    result = $"{ns}.{ms} - reply.ManualPortInfo = null";
                    return false;
                }
                foreach (var info in reply.ManualPortInfo)
                {
                    var port = ports.Where(s => s.PORT_ID == info.ManualPortId.Trim()).FirstOrDefault();
                    if (port == null) continue;

                    port.IS_IN_SERVICE = !info.IsDown;
                    port.IS_INPUT_MODE = info.IsInMode;

                    // 只有一個 stage
                    port.IS_PRESENCE_LOC_1 = info.LoadPosition1;
                    port.CST_ID_LOC_1 = info.CarrierIdOfStage1?.Trim() ?? "";
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetPortRun(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }

        public bool SetPortStop(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }

        public bool ResetPortAlarm(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }

        public bool SetPortDir(string portID, string dir, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }

        public bool SetPortWaitIn(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }

        public bool SetAgvStationOpen(string portID, bool open, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
        }
    }
}
