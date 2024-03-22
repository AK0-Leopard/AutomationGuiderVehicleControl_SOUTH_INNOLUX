using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using NLog;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Text;
using ViewerObject;
using static ViewerObject.VPORTSTATION_Def;

namespace UtilsAPI.BLL
{
    public class PortStationBLL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public PortStationBLL(WindownApplication _app)
        {
            app = _app;
        }

        #region WebAPI
        public bool SendPortPriorityUpdate(string port_id, int priority)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "PortStation",
                "PriorityUpdate",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(port_id)}={port_id}").Append("&");
            sb.Append($"{nameof(priority)}={priority.ToString()}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_id), port_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(priority), priority.ToString());
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPortTypeUpdate(string port_id, int port_type)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "PortStation",
                "PortTypeUpdate",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(port_id)}={port_id}").Append("&");
            sb.Append($"{nameof(port_type)}={port_type.ToString()}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_id), port_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_type), port_type.ToString());
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPortBcrEnable(string port_id)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "PortStation",
                "BcrEnable",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(port_id)}={port_id}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_id), port_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPortBcrDisable(string port_id)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "PortStation",
                "BcrDisable",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(port_id)}={port_id}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_id), port_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPortClearAlarm(string port_id)
        {
            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "PortStation",
                "ClearAlarm",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(port_id)}={port_id}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_id), port_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPortStatusChange(string port_id, PortServiceStatus portServiceStatus)
        {
            string status = "";
            bool default_result = false;
            try
            {
                status = app.ObjCacheManager.ObjConverter.Definition.GetString(portServiceStatus);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }

            string result = string.Empty;
            string[] action_targets = new string[]
            {
                "PortStation",
                "StatusChange",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(port_id)}={port_id}").Append("&");
            sb.Append($"{nameof(status)}={status}").Append("&");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(port_id), port_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(status), status);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }
        #endregion WebAPI

        #region GetDB Data
        public List<VPORTSTATION> GetVPORTSTATION()
        {
            List<VPORTSTATION> default_result = new List<VPORTSTATION>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.PortStationBLL.GetVPORTSTATION();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
        #endregion
    }
}
