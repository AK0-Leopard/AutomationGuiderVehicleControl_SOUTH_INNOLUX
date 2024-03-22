using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_AUO_600.LOG
{
    public class SystemProcInfo : ISystemProcInfo
    {
        private readonly string ns = "ObjectConverter_AGVC_AUO_600.LOG" + "SystemProcInfo";

        public bool Convert2Object_SYSTEMPROCESS_INFO(byte[] input, out SYSTEMPROCESS_INFO obj, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            obj = null;
            result = "";

            if (input == null || input.Length == 0)
            {
                result = $"{ns}.{ms} - input = null or empty";
                return false;
            }

            try
            {
                obj = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_SystemInfo(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_SystemInfo result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch
            {
                result = $"{ns}.{ms} - com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_SystemInfo failed, input: {input}";
                return false;
            }
        }

        private bool setSYSTEM_PROCESS_LOG(SYSTEMPROCESS_INFO info, out ViewerObject.LOG.SYSTEM_PROCESS_LOG log, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            log = null;

            if (info == null)
            {
                result = $"{ns}.{ms} - info = null";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = $"new ViewerObject.LOG.SYSTEM_PROCESS_LOG()";
                log = new ViewerObject.LOG.SYSTEM_PROCESS_LOG();
                _doing = $"Set LogLevel";
                log.LogLevel = ViewerObject.Definition.GetLogLevel(info?.LOGLEVEL);
                _doing = $"Set Class";
                log.Class = info?.CLASS?.Trim() ?? "";
                _doing = $"Set Method";
                log.Method = info?.METHOD.ToString();
                _doing = $"Set Device";
                log.Device = info?.DEVICE?.Trim() ?? "";
                _doing = $"Set LogID";
                log.LogID = info?.LOGID.Trim() ?? "";
                _doing = $"Set ThreadID";
                log.ThreadID = info?.THREADID.Trim() ?? "";
                _doing = $"Set Data";
                log.Data = info?.DATA.Trim() ?? "";
                _doing = $"Set VhID";
                log.VhID = info?.VHID.Trim() ?? "";
                _doing = $"Set CrrID";
                log.CrrID = info?.CRRID.Trim() ?? "";
                _doing = $"Set Type";
                log.Type = info?.TYPE.Trim() ?? "";
                _doing = $"Set Lot";
                log.Lot = info?.LOT?.Trim() ?? "";
                _doing = $"Set Level";
                log.Level = info?.LEVEL?.Trim() ?? "";
                _doing = $"Set XID";
                log.XID = info?.XID?.Trim() ?? "";
                _doing = $"Set Seq";
                log.Seq = info != null ? Convert.ToString(info?.SEQ) : "0";
                _doing = $"Set TrxID";
                log.TrxID = info?.TRXID?.Trim() ?? "";
                _doing = $"Set Details";
                log.Details = info?.DETAILS?.Trim() ?? "";
                _doing = $"Set Time";
                log.Time = info?.TIME?.Trim() ?? "";

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetSYSTEM_PROCESS_LOG(byte[] input, out ViewerObject.LOG.SYSTEM_PROCESS_LOG log, out string result)
        {
            result = "";
            log = null;
            bool isSuccess = Convert2Object_SYSTEMPROCESS_INFO(input, out SYSTEMPROCESS_INFO info, out result);
            if (!isSuccess) return false;
            else
            {
                isSuccess = setSYSTEM_PROCESS_LOG(info, out log, out result);
                return isSuccess;
            }
        }
    }
}
