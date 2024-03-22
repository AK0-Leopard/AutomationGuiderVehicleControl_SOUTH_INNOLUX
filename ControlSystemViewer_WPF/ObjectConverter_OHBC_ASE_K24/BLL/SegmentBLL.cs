using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectConverterInterface.BLL;
using CommonMessage.ProtocolFormat;
using System.Net;
using Grpc.Core;

namespace ObjectConverter_OHBC_ASE_K24.BLL
{
    public class SegmentBLL : ISegmentBLL
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K24" + ".SegmentBLL";

        private static readonly string trackSwitch_web_address = "ohxcv.ha.ohxc.mirle.com.tw";
        private Channel channel = new Channel(trackSwitch_web_address, 7004, ChannelCredentials.Insecure);
        public bool segControl(string segID, bool enable, out string result)
        {
            bool isSuccess = true;
            try
            {
                CommonMessage.ProtocolFormat.SegFun.ControlRequest request = new CommonMessage.ProtocolFormat.SegFun.ControlRequest();
                CommonMessage.ProtocolFormat.SegFun.ControlReply reply;
                CommonMessage.ProtocolFormat.SegFun.segmentGreeter.segmentGreeterClient client;
                client = new CommonMessage.ProtocolFormat.SegFun.segmentGreeter.segmentGreeterClient(channel);
                request.Id = segID;
                request.Enable = enable;
                reply = client.sectionControl(request); //提出要求
                result = reply.Result;

            }
            catch (Exception ex)
            {
                result = ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}
