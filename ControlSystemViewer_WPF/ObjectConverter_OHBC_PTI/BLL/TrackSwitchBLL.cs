using RailChangerProtocol;
using Grpc.Core;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ObjectConverter_OHBC_PTI.BLL
{
    public class TrackSwitchBLL : ITrackSwitchBLL
    {
        private readonly string ns = "ObjectConverter_OHBC_PTI.BLL" + ".TrackSwitchBLL";

        private static readonly string trackSwitch_web_address = "trackswitch.ohxc.mirle.com.tw";
        private Channel channel = new Channel(trackSwitch_web_address, 6060, ChannelCredentials.Insecure);

        public TrackSwitchBLL()
        {
        }

        public bool UpdateTrackSwitches(ref List<ViewerObject.TrackSwitch> trackSwitches, out string result)
        {   
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new Greeter.GreeterClient(channel);
                //建立提出要求的資料 - Empty
                //提出要求並回收server端給予的回應
                var reply = client.RequestTracksInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.TracksInfo == null)
                {
                    result = $"{ns}.{ms} - reply.TracksInfo = null";
                    return false;
                }
                foreach (var info in reply.TracksInfo)
                {
                    var trackSwitch = trackSwitches.Where(s => s.ID == info.TrackId.Trim()).FirstOrDefault();
                    if (trackSwitch == null) continue;

                    trackSwitch.IsAlive = info.Alive;
                    trackSwitch.AlarmCode = info.AlarmCode;
                    trackSwitch.Status = GetTrackSwitchStatus(info.Status);
                    trackSwitch.Dir = GetTrackSwitchDir(info.Dir);
                    trackSwitch.AutoChangeToDefaultDir = info.AutoChangeTrack;
                    trackSwitch.DefaultDir = GetTrackSwitchDir(info.AutoChangeTrackDir);
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool ResetTrackSwitchByID(string id, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new Greeter.GreeterClient(channel);

                //建立提出要求的資料
                var alarmResetReq = new alarmRstRequest { RailChangerNumber = id };
                //提出要求並回收server端給予的回應
                var alarmResetReply = client.alarmRst(alarmResetReq);

                //建立提出要求的資料
                var blockResetReq = new blockRstRequest { RailChangerNumber = id };
                //提出要求並回收server端給予的回應
                var blockResetReply = client.blockRst(blockResetReq);
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public ViewerObject.TrackSwitchStatus GetTrackSwitchStatus(TrackStatus status)
        {
            switch (status)
            {
                case TrackStatus.Manaul:
                    return ViewerObject.TrackSwitchStatus.Manaul;
                case TrackStatus.Auto:
                    return ViewerObject.TrackSwitchStatus.Auto;
                case TrackStatus.Alarm:
                    return ViewerObject.TrackSwitchStatus.Alarm;
                case TrackStatus.NotDefine:
                default:
                    return ViewerObject.TrackSwitchStatus.Unknown;
            }
        }

        public ViewerObject.TrackSwitchDir GetTrackSwitchDir(TrackDir dir)
        {
            switch (dir)
            {
                case TrackDir.Straight:
                    return ViewerObject.TrackSwitchDir.Go_Track1;
                case TrackDir.Curve:
                    return ViewerObject.TrackSwitchDir.Go_Track2;
                default:
                    return ViewerObject.TrackSwitchDir.Unknown;
            }
        }
    }
}
