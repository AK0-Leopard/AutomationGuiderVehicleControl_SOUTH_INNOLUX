using com.mirle.ibg3k0.ohxc.wpf.App;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMessage.ProtocolFormat;
using MirleGO_UIFrameWork.UI.uc_Button;
using com.mirle.ibg3k0.bc.wpf.App;

namespace UtilsAPI.BLL
{
    public class SegmentBLL
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        WindownApplication app = null;

        public class segControlEventArgs : EventArgs
        {
            public enum ContolMode
            {
                enable = 0,
                disable = 1
            }
            public string segNumber;
            public ContolMode contolMode;
            public bool isSuccess { get; private set; }
            public string serverReturn { get; private set; }
            public string exceptionMessage { get; private set; }
            public segControlEventArgs(string _segNumber, ContolMode _contolMode)
            {
                segNumber = _segNumber;
                contolMode = _contolMode;
            }
            public bool setResultInfo(bool resultIsSuccess, string resultServerReturn, string resultExcrption)
            {
                isSuccess = resultIsSuccess;
                serverReturn = resultServerReturn;
                exceptionMessage = resultExcrption;
                return true;
            }
        }

        public EventHandler<segControlEventArgs> segControlEventHandler;

        public SegmentBLL(WindownApplication _app)
        {
            app = _app;
            segControlEventHandler += segControl; 
        }

        #region WebAPI
        //public (bool isSuccess, string result) SendSegmentStatusUpdate(string seg_id, sc.ASEGMENT.DisableType type, sc.E_SEG_STATUS satus)
        //{
        //    string result = string.Empty;
        //    string[] action_targets = new string[]
        //    {
        //        "Segment",
        //        "StatusUpdate",
        //    };
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append($"{nameof(seg_id)}={seg_id}").Append("&");
        //    sb.Append($"{nameof(type)}={type.ToString()}").Append("&");
        //    sb.Append($"{nameof(satus)}={satus.ToString()}");
        //    byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
        //    result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
        //    return (result == "", result);
        //}
        #endregion WebAPI

        #region 人為觸發LINE上的enable/disable seg
        private void segControl(object o, segControlEventArgs args)
        {
            bool isSuccess = false;
            string result = "";
            if (args.contolMode == segControlEventArgs.ContolMode.enable)
                isSuccess = app.ObjCacheManager.ObjConverter.BLL.SegmentBLL.segControl(args.segNumber, true, out result);
            else
                isSuccess = app.ObjCacheManager.ObjConverter.BLL.SegmentBLL.segControl(args.segNumber, false, out result);

            if (!isSuccess)
            {
                TipMessage_Type_Light.Show("Send command failed", result, BCAppConstants.INFO_MSG);
            }
            else
            {
                TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
            }
        }
        #endregion
    }
}
