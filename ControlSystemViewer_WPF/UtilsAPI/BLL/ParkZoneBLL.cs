using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ViewerObject;

namespace UtilsAPI.BLL
{
    public class ParkZoneBLL
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public ParkZoneBLL(WindownApplication _app)
        {
            app = _app;
        }

        public List<ParkingZone> GetAllParkingZoneData()
        {
            List<ParkingZone> default_result = new List<ParkingZone>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.ParkZoneBLL.GetAllParkingZoneData();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<ViewerObject.ParkZoneMaster> LoadParkingZoneMaster()
        {
            List<ParkZoneMaster> default_result = new List<ParkZoneMaster>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.ParkZoneBLL.LoadParkingZoneMaster();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<ViewerObject.ParkZoneDetail> LoadParkingZoneDetail(string address)
        {
            List<ParkZoneDetail> default_result = new List<ParkZoneDetail>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.ParkZoneBLL.LoadParkingZoneDetail(address);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public bool SendModifyParkZone(string act_type, ParkingZoneData pz, out string result)
        {
            result = string.Empty;

            try
            {
                string data = JsonConvert.SerializeObject(pz);

                app.SystemOperationLogBLL.addData_KeyValue(nameof(act_type), act_type);
                app.SystemOperationLogBLL.addData_KeyValue(nameof(data), data);

                result = string.Empty;
                string[] action_targets = new string[]
                {
                    "ParkZone",
                    "ModifyParkZone",
                };

                StringBuilder sb = new StringBuilder();
                sb.Append($"{nameof(act_type)}={act_type}").Append("&");
                sb.Append($"{nameof(data)}={data}");
                byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
                result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

                app.SystemOperationLogBLL.addSystemOperationHis(result);

                return result == "OK";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return false;
        }

    }

}
