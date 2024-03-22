using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHTC_AT_S_MALASYIA.LOG
{
    public class EqLogInfo : IEqLogInfo
    {
        private readonly string ns = "ObjectConverter_OHTC_AT_S_MALASYIA.LOG" + "EqLogInfo";

        public bool Convert2Object_EQLOG_INFO(byte[] input, out EQLOG_INFO obj, out string result)
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
                obj = com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TcpInfo(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TcpInfo result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch
            {
                result = $"{ns}.{ms} - com.mirle.ibg3k0.sc.BLL.LineBLL.Convert2Object_TcpInfo failed, input: {input}";
                return false;
            }
        }

        private bool setVEHICLE_COMMU_LOG(EQLOG_INFO info, out ViewerObject.LOG.VEHICLE_COMMU_LOG log, out string result)
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
                _doing = $"new ViewerObject.LOG.VEHICLE_COMMU_LOG()";
                log = new ViewerObject.LOG.VEHICLE_COMMU_LOG();
                _doing = $"Set VhID";
                log.VhID = info?.VHID?.Trim() ?? "";
                _doing = $"Set SendRecive";
                log.SendRecive = info?.SENDRECEIVE?.Trim() ?? "";
                _doing = $"Set FunName";
                log.FunName = info?.FUNNAME?.Trim() ?? "";
                _doing = $"Set SeqNo";
                log.SeqNo = info?.SEQNO.ToString();
                _doing = $"Set CmdID";
                log.CmdID = info?.OHTCCMDID?.Trim() ?? "";
                _doing = $"Set McsCmdID";
                log.McsCmdID = info?.MCSCMDID?.Trim() ?? "";
                _doing = $"Set ActType";
                log.ActType = info?.ACTTYPE?.Trim() ?? "";
                _doing = $"Set EventType";
                log.EventType = info?.EVENTTYPE?.Trim() ?? "";
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
        public bool SetVEHICLE_COMMU_LOG(byte[] input, out ViewerObject.LOG.VEHICLE_COMMU_LOG log, out string result)
        {
            result = "";
            log = null;
            bool isSuccess = Convert2Object_EQLOG_INFO(input, out EQLOG_INFO info, out result);
            if (!isSuccess) return false;
            else
            {
                isSuccess = setVEHICLE_COMMU_LOG(info, out log, out result);
                return isSuccess;
            }
        }
    }
}
