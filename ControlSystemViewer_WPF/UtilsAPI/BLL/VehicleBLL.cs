//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

using com.mirle.ibg3k0.ohxc.wpf.App;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Text;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using System.IO;
using System.IO.Compression;
using NLog;
using System.Linq;
using Newtonsoft.Json;
using ViewerObject;
using static ViewerObject.VVEHICLE_Def;

namespace UtilsAPI.BLL
{
    public class VehicleBLL
    {
        const string WEB_API_ACTION_VIEWERUPDATE = "ViewerUpdate";

        private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public VehicleBLL(WindownApplication _app)
        {
            app = _app;
        }

        #region Nats
        public void SubscriberVehicleInfo(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {
            app.GetNatsManager().Subscriber(subject, handler, is_last: true);
            //app.GetNatsManager().Subscriber(subject, handler);
        }

        public void ProcVehicleInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.PutVehicle(handler.Message.Data);
        }
        #endregion Nats

        public void filterVh(ref List<VVEHICLE> vhs)
        {
            foreach (var vh in vhs)
            {
                if (vh.HAS_ERROR ||
                    !string.IsNullOrWhiteSpace(vh.CMD_ID_1) ||
                    vh.HAS_CST_L ||
                    vh.MODE_STATUS != ModeStatus.AutoRemote ||
                    string.IsNullOrWhiteSpace(vh.CUR_ADR_ID) ||
                    app.CmdBLL.VhHasQueueCmd(vh.VEHICLE_ID))
                {
                    vhs.Remove(vh);
                }
            }
        }

        #region WebAPI
        public void ReguestViewerUpdate()
        {
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "ViewerUpdate"
            };
            string post_data = "";
            byte[] byteArray = Encoding.UTF8.GetBytes(post_data);
            app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
        }

        public bool SendVehicleResetToControl(string vh_id, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "SendReset",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendVehicleCMDCancelAbortToControl(string vh_id, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "SendCancelAbort",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendCmdToControl(string vh_id, string cmd_type, string carrier_id, string from_port_id, string to_port_id, ref string result, string carrier_type = "", string from_port = "", string to_port = "")
        {
            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(cmd_type), cmd_type);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(carrier_id), carrier_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(carrier_type), carrier_type);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(from_port_id), from_port_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(to_port_id), to_port_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(from_port), from_port);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(to_port), to_port);

            // try gRPC
            string objcvt = app.ObjCacheManager.GetSelectedProject()?.ObjectConverter;
            if (objcvt == "OHBC_ASE_K24")
            {
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                result = "";
                if (app.ObjCacheManager.ObjConverter == null)
                {
                    result = $"UtilsAPI.BLL.VehicleBLL.{mn}: ObjConverter = null";
                    logger.Warn(result);
                    return false;
                }
                if (app.ObjCacheManager.ObjConverter.BLL.VehicleBLL == null)
                {
                    result = $"UtilsAPI.BLL.VehicleBLL.{mn}: ObjConverter.BLL.VehicleBLL = null";
                    logger.Warn(result);
                    return false;
                }
                try
                {
                    bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.VehicleBLL.SendCmdToControl(vh_id, cmd_type, carrier_id, from_port_id, to_port_id, out result);
                    if (!isSuccess) logger.Warn(result);
                    app.SystemOperationLogBLL.addSystemOperationHis(result);
                    return isSuccess;
                }
                catch (Exception ex)
                {
                    result = $"UtilsAPI.BLL.VehicleBLL.{mn}: Exception: {ex.Message}";
                    logger.Error(ex, "Exception");
                    return false;
                }
            }

            result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "SendCommand",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            sb.Append($"{nameof(cmd_type)}={cmd_type}").Append("&");
            sb.Append($"{nameof(carrier_id)}={carrier_id}").Append("&");
            if (app.ObjCacheManager.ViewerSettings.system.HasCarrierType) sb.Append($"{nameof(carrier_type)}={carrier_type}").Append("&");
            sb.Append($"{nameof(from_port_id)}={from_port_id}").Append("&");
            sb.Append($"{nameof(to_port_id)}={to_port_id}").Append("&");
            sb.Append($"{nameof(from_port)}={from_port}").Append("&");
            sb.Append($"{nameof(to_port)}={to_port}");

            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendCmdToControl(string vh_id, string cmd_type, string from_port_id = "", string to_port_id = "")
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "SendCommand",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            sb.Append($"{nameof(cmd_type)}={cmd_type}").Append("&");
            sb.Append($"{nameof(from_port_id)}={from_port_id}").Append("&");
            sb.Append($"{nameof(to_port_id)}={to_port_id}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(cmd_type), cmd_type);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(from_port_id), from_port_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(to_port_id), to_port_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPaserToControl(string vh_id, VPauseEvent vpauseEvent)
        {
            string event_type = "";
            bool default_result = false;
            try
            {
                event_type = app.ObjCacheManager.ObjConverter.Definition.GetString(vpauseEvent);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }

            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "PauseEvent",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            sb.Append($"{nameof(event_type)}={event_type}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(event_type), event_type);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPauseStatusChange(string vh_id, VPauseType vpauseType, VPauseEvent vpauseEvent, out string result)
        {
            string pauseType = "";
            string event_type = "";
            bool default_result = false;
            result = string.Empty;
            try
            {
                pauseType = app.ObjCacheManager.ObjConverter.Definition.GetString(vpauseType);
                event_type = app.ObjCacheManager.ObjConverter.Definition.GetString(vpauseEvent);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }

            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "PauseStatusChange",
            };
            //var old_vh = app.ObjCacheManager.GetVEHICLE(vh_id);

            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            sb.Append($"{nameof(pauseType)}={pauseType}").Append("&");
            sb.Append($"{nameof(event_type)}={event_type}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(pauseType), pauseType);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(event_type), event_type);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendModeStatusChange(string vh_id, ModeStatus _modeStatus, out string result)
        {
            string modeStatus = "";
            bool default_result = false;
            result = string.Empty;
            try
            {
                modeStatus = app.ObjCacheManager.ObjConverter.Definition.GetString(_modeStatus);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }

            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "ModeStatusChange",
            };
            var old_vh = app.ObjCacheManager.GetVEHICLE(vh_id);

            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            sb.Append($"{nameof(modeStatus)}={modeStatus}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id, vh_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(modeStatus), old_vh.STRING_MODE_STATUS, modeStatus);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }
        public bool SendInstallStatusChange(string vh_id, bool isInstall, out string result)
        {
            bool default_result = false;
            result = string.Empty;

            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "InstallStatusChange",
            };
            var old_vh = app.ObjCacheManager.GetVEHICLE(vh_id);

            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}").Append("&");
            sb.Append($"{nameof(isInstall)}={isInstall}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id, vh_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(isInstall), old_vh.INSTALL_STATUS.ToString(), isInstall.ToString());
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendVehicleAlarmResetRequest(string vh_id, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "ResetAlarm",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(vh_id)}={vh_id}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendOHTFunc(int Func_Num, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "Debug_Function",
            };
            StringBuilder sb = new StringBuilder();
            //var a = JsonConvert.SerializeObject(obj);
            sb.Append($"{nameof(Func_Num)}={Func_Num}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
            return result == "OK";
        }

        public bool SendOHTFunc(object obj, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "AVEHICLES",
                "Debug_Function",
            };
            StringBuilder sb = new StringBuilder();
            var json = JsonConvert.SerializeObject(obj);
            //sb.Append($"{nameof(Func_Num)}={Func_Num}");
            byte[] byteArray = Encoding.UTF8.GetBytes(json.ToString());

            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
            return result == "OK";
        }
        #endregion WebAPI
    }
}
