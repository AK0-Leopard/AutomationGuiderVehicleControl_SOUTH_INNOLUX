using com.mirle.ibg3k0.sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectConverterInterface;
using ViewerObject;
using Newtonsoft.Json;
namespace ObjectConverter_AGVC_SOUTH_INNOLUX
{
    public class ASection : IASection
    {
        private readonly string ns = "ObjectConverter_AGVC_SOUTH_INNOLUX" + ".ASection";
        public bool Convert2Object_SECTIONs(string input, out List<ASECTION> obj, out string result)
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
                obj = JsonConvert.DeserializeObject<List<ASECTION>>(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<ASEGMENT>> result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<ASEGMENT>> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetSections(ref List<ViewerObject.Section> secs, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            bool isSuccess;
            string _doing = "";
            try
            {
                _doing = "Convert data to List<ASECTION>";
                isSuccess = Convert2Object_SECTIONs(data, out List<ASECTION> asecs, out result);
                if (!isSuccess) return false;

                _doing = "Init List<VSEGMENT>";
                if (secs == null) secs = new List<ViewerObject.Section>();
                else secs.Clear();

                _doing = "Set each Section by ASECTION";
                foreach (var asec in asecs)
                {
                    if (!SetVSEGMENT(asec, out ViewerObject.Section sec, out result)) return false;
                    secs.Add(sec);
                }

                return isSuccess;
            }
            catch
            {
                isSuccess = false;
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return isSuccess;
            }
        }
        public bool SetVSEGMENT(ASECTION asec, out ViewerObject.Section sec, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            sec = null;

            if (asec == null)
            {
                result = $"{ns}.{ms} - aseg = null";
                return false;
            }

            string _doing = "";
            try
            {
                //建構
                sec = new Section(asec.SEC_ID, (asec.DISABLE_TIME == null) ? true : false);
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
