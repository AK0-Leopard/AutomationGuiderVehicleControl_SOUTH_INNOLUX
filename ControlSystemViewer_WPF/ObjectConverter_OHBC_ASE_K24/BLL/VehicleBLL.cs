using com.mirle.AK0.ProtocolFormat;
using Grpc.Core;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K24.BLL
{
    public class VehicleBLL : IVehicleBLL
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K24.BLL" + ".TrackSwitchBLL";

        private static readonly string control_web_address = "ohxcv.ha.ohxc.mirle.com.tw";
        private Channel channel = new Channel(control_web_address, 7001, ChannelCredentials.Insecure);

        public bool SendCmdToControl(string vh_id, string cmd_type, string carrier_id, string from_port_id, string to_port_id, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new VehicleControlFun.VehicleControlFunClient(channel);
                //建立提出要求的資料
                VehicleCommandInfo request = new VehicleCommandInfo
                {
                    VhId = vh_id ?? "",
                    Type = cmd_type == null ? CommandEventType.Move :
                           cmd_type.ToUpper().Contains("MOVE") ? CommandEventType.Move :
                           cmd_type.ToUpper().Contains("LOAD") ? CommandEventType.Load :
                           cmd_type.ToUpper().Contains("UNLOAD") ? CommandEventType.Unload :
                           cmd_type.ToUpper().Contains("LOADUNLOAD") ? CommandEventType.LoadUnload :
                           CommandEventType.Move,
                    CarrierId = carrier_id ?? "",
                    FromPortId = from_port_id ?? "",
                    ToPortId = to_port_id ?? ""
                };
                //提出要求並回收server端給予的回應
                var reply = client.RequestTrnsfer(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.Result == Result.Ng)
                {
                    result = $"{ns}.{ms} - reply.Result = NG, {reply.Reason}";
                    return false;
                }
                result = $"{ns}.{ms} - reply.Result = OK, {reply.Reason}";
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }
    }
}
