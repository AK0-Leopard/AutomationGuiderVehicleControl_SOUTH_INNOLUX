using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectConverterInterface.BLL;
using CommonMessage.ProtocolFormat;
using Grpc.Core;

namespace ObjectConverter_AGVC_NORTH_INNOLUX.BLL
{
    public class SegmentBLL : ISegmentBLL
    {
        private readonly string ns = "ObjectConverter_AGVC_NORTH_INNOLUX" + ".SegmentBLL";

        private static readonly string segControlServer = "ohxcv.ha.ohxc.mirle.com.tw";
        //private static readonly string segControlServer = "127.0.0.1";
        private Channel channel = new Channel(segControlServer, 7004, ChannelCredentials.Insecure);
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
                if (reply.Result != "Success")
                    isSuccess = false;
                result = reply.Result;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                result=ex.Message;
            }
            return isSuccess;
        }
    }
}
