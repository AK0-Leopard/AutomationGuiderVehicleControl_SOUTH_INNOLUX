using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K21
{
    public class APortStation : IAPortStation
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K21" + ".APortStation";

        public bool Convert2Object_APORTSTATIONs(string input, out List<APORTSTATION> obj, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            obj = null;
            result = "";

            if (string.IsNullOrWhiteSpace(input))
            {
                result = $"{ns}.{ms} - input = null or empty";
                return false;
            }

            try
            {
                obj = JsonConvert.DeserializeObject<List<APORTSTATION>>(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<APORTSTATION>> result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<APORTSTATION>> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetVPORTSTATIONs(ref List<ViewerObject.VPORTSTATION> ports, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            result = "Use gRPC to get/set ports";
            return true;

            //string _doing = "";
            //try
            //{
            //    _doing = "Convert data to List<APORTSTATION>";
            //    bool isSuccess = Convert2Object_APORTSTATIONs(data, out List<APORTSTATION> aports, out result);
            //    if (!isSuccess) return false;

            //    _doing = "Init List<VPORTSTATION>";
            //    if (ports == null) ports = new List<ViewerObject.VPORTSTATION>();
            //    else ports.Clear();

            //    _doing = "Set each VPORTSTATION by APORTSTATION";
            //    foreach (var aport in aports)
            //    {
            //        if (!SetVPORTSTATION(aport, out ViewerObject.VPORTSTATION port, out result)) return false;
            //        ports.Add(port);
            //    }

            //    return true;
            //}
            //catch
            //{
            //    result = $"{ns}.{ms} - Exception occurred while {_doing}";
            //    return false;
            //}
        }
        public bool SetVPORTSTATION(APORTSTATION aport, out ViewerObject.VPORTSTATION port, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            port = null;

            if (aport == null)
            {
                result = $"{ns}.{ms} - aport = null";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = $"new ViewerObject.VPORTSTATION({aport.PORT_ID}, {aport.ADR_ID}, {1})";
                port = new ViewerObject.VPORTSTATION(aport.PORT_ID, aport.ADR_ID, 1/*aport.stageCount*/);
                _doing = "IS_IN_SERVICE";
                port.IS_IN_SERVICE = aport.PORT_SERVICE_STATUS == (int)PortStationServiceStatus.InService;
                _doing = "IS_INPUT_MODE";
                port.IS_INPUT_MODE = aport.PORT_DIR == 0; // ?
                _doing = "port.SetLocPresenceCstID";
                port.SetLocPresenceCstID(aport.CST_ID);

                _doing = "onStatusChange";
                port.onStatusChange();

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
