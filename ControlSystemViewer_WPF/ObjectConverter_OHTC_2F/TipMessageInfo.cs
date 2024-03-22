using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHTC_2F
{
    public class TipMessageInfo : ITipMessageInfo
    {
        private readonly string ns = "ObjectConverter_OHTC_2F" + ".TipMessageInfo";

        public bool Convert2Object_TipMsgInfoCollection(byte[] bytes, out object info, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            info = null;
            result = "";
            string _doing = "";
            try
            {
                _doing = "com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TipMsgInfoCollection";
                info = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TipMsgInfoCollection(bytes);

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool Convert2Object_TipMsgInfoCollection(string input, out object info, out string result)
        {
            info = null;
            result = "";
            byte[] bytes = BasicFunction.Compress.Uncompress_String2ArrayByte(input);
            bool isSuccess = bytes != null;
            if (!isSuccess) return false;
            else
            {
                isSuccess = Convert2Object_TipMsgInfoCollection(bytes, out info, out result);
                return isSuccess;
            }
        }

        public bool SetVTIPMESSAGEs(ref List<ViewerObject.VTIPMESSAGE> tipMsgs, byte[] bytes, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (bytes == null)
            {
                result = $"{ns}.{ms} - bytes = null";
                return false;
            }
            if (!Convert2Object_TipMsgInfoCollection(bytes, out object obj, out result))
                return false;

            TIP_MESSAGE_COLLECTION infos = (TIP_MESSAGE_COLLECTION)obj;
            if (infos == null || infos.TIPMESSAGEINFOS == null)
            {
                result = $"{ns}.{ms} - infos/infos.TIPMESSAGEINFOS = null or not typeof TIP_MESSAGE_COLLECTION";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = "Init List<ViewerObject.VTIPMESSAGE>";
                if (tipMsgs == null) tipMsgs = new List<ViewerObject.VTIPMESSAGE>();
                else tipMsgs.Clear();
                _doing = "Add each infos.TIPMESSAGEINFOS to List<ViewerObject.VTIPMESSAGE>";
                foreach (var info in infos.TIPMESSAGEINFOS)
                {
                    tipMsgs.Add(new ViewerObject.VTIPMESSAGE()
                    {
                        XID = info.XID?.Trim() ?? "",
                        Time = info.Time?.Trim() ?? "",
                        MsgLevel = new Definition.Convert().GetMsgLevel(info.MsgLevel),
                        Msg = info.Message?.Trim() ?? ""
                    });
                }

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
