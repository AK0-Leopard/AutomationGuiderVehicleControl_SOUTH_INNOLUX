using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_UMTC.LOG
{
    public class HostLogInfo : IHostLogInfo
    {
        private readonly string ns = "ObjectConverter_AGVC_UMTC.LOG" + "HostLogInfo";

        public bool Convert2Object_HOSTLOG_INFO(byte[] input, out HOSTLOG_INFO obj, out string result)
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
                obj = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_SECSInfo(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_SECSInfo result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch
            {
                result = $"{ns}.{ms} - com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_SECSInfo failed, input: {input}";
                return false;
            }
        }

        private bool setMCS_COMMU_LOG(HOSTLOG_INFO info, out ViewerObject.LOG.MCS_COMMU_LOG log, out string result)
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
                _doing = $"new ViewerObject.LOG.MCS_COMMU_LOG()";
                log = new ViewerObject.LOG.MCS_COMMU_LOG();
                _doing = $"Set EqID";
                log.EqID = info?.EQID?.Trim() ?? "";
                _doing = $"Set SendRecive";
                log.SendRecive = info?.SENDRECEIVE?.Trim() ?? "";
                _doing = $"Set FunName";
                log.FunName = info?.FUNNAME?.Trim() ?? "";
                _doing = $"Set Sx";
                log.Sx = info?.SX.ToString();
                _doing = $"Set Fy";
                log.Fy = info?.FY.ToString();
                _doing = $"Set DeviceID";
                log.DeviceID = info?.DEVICE?.Trim() ?? "";
                _doing = $"Set Message";
                log.Message = info?.MESSAGE?.Trim() ?? "";
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
        public bool SetMCS_COMMU_LOG(byte[] input, out ViewerObject.LOG.MCS_COMMU_LOG log, out string result)
        {
            result = "";
            log = null;
            bool isSuccess = Convert2Object_HOSTLOG_INFO(input, out HOSTLOG_INFO info, out result);
            if (!isSuccess) return false;
            else
            {
                isSuccess = setMCS_COMMU_LOG(info, out log, out result);
                return isSuccess;
            }
        }
    }
}
