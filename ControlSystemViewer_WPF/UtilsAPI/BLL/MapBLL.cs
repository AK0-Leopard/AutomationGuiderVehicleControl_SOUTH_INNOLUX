using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewerObject;

namespace UtilsAPI.BLL
{
    public class MapBLL
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        WindownApplication app = null;

        public MapBLL(WindownApplication _app)
        {
            app = _app;
        }

        #region WebAPI
        public string GetMapInfoFromHttp(Definition.MapInfoType dataType)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                    "MapInfo"
            };
            StringBuilder sb = new StringBuilder();
            //sb.Append($"{nameof(SCAppConstants.MapInfoDataType)}={dataType}");
            sb.Append(dataType);
            result = app.GetWebClientManager().GetInfoFromServer(WebClientManager.OHxC_CONTROL_URI, action_targets, sb.ToString());
            return result;
        }
        public List<T> GetMapInfosFromHttp<T>(Definition.MapInfoType dataType)
        {
            List<T> objs = null;
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                    "MapInfo"
            };
            StringBuilder sb = new StringBuilder();
            //sb.Append($"{nameof(SCAppConstants.MapInfoDataType)}={dataType}");
            sb.Append(dataType);
            result = app.GetWebClientManager().GetInfoFromServer(WebClientManager.OHxC_CONTROL_URI, action_targets, sb.ToString());
            //logger.Info($"GetInfoFromServer<{dataType}> result: {result}");
            try
            {
                objs = JsonConvert.DeserializeObject<List<T>>(result);
            }
            catch //(Exception ex)
            {
                //logger.Info(ex, $"JsonConvert.DeserializeObject<List<{dataType}>> failed, Exception");
                logger.Info($"JsonConvert.DeserializeObject<List<{dataType}>> failed, GetInfoFromServer<{dataType}> result: {result}");
                objs = null;
            }
            return objs;
        }
        public T GetMapInfoFromHttp<T>(Definition.MapInfoType dataType)
        {
            T obj;
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "MapInfo"
            };
            StringBuilder sb = new StringBuilder();
            //sb.Append($"{nameof(SCAppConstants.MapInfoDataType)}={dataType}");
            sb.Append(dataType);
            result = app.GetWebClientManager().GetInfoFromServer(WebClientManager.OHxC_CONTROL_URI, action_targets, sb.ToString());
            obj = JsonConvert.DeserializeObject<T>(result);
            return obj;
        }
        #endregion WebAPI
    }
}