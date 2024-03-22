using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectConverterInterface.BLL;
using CommonMessage.ProtocolFormat;
using System.Net;
using Grpc.Core;
using System.Collections.Specialized;

namespace ObjectConverter_OHTC_AT_S_MALASYIA.BLL
{
    public class SegmentBLL : ISegmentBLL
    {
        private readonly string ns = "ObjectConverter_OHTC_AT_S_MALASYIA" + ".SegmentBLL";

        private static readonly string trackSwitch_web_address = "ohxcv.ha.ohxc.mirle.com.tw";
        private Channel channel = new Channel(trackSwitch_web_address, 7004, ChannelCredentials.Insecure);
        public bool segControl(string segID, bool enable, out string result)
        {
            bool isSuccess = true;
            try
            {
                //发送Post请求
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["SegNum"] = segID;
                    values["Controll"] = enable ? "1" : "0";
                    var response = client.UploadValues("http://" + trackSwitch_web_address + ":3280/RoadMaint/UxSegmentControl", values);
                    var responseString = Encoding.Default.GetString(response);
                }

                result = "1";
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
