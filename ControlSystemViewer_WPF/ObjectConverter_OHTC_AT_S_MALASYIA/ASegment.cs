using com.mirle.ibg3k0.sc;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHTC_AT_S_MALASYIA
{
    public class ASegment : IASegment
    {
        private readonly string ns = "ObjectConverter_OHTC_AT_S_MALASYIA" + ".ASegment";

        public bool Convert2Object_ASEGMENTs(string input, out List<ASEGMENT> obj, out string result)
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
                obj = JsonConvert.DeserializeObject<List<ASEGMENT>>(input);
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

        public bool SetVSEGMENTs(ref List<ViewerObject.VSEGMENT> segs, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            string _doing = "";
            try
            {
                _doing = "Convert data to List<ASEGMENT>";
                bool isSuccess = Convert2Object_ASEGMENTs(data, out List<ASEGMENT> asegs, out result);
                if (!isSuccess) return false;

                _doing = "Init List<VSEGMENT>";
                if (segs == null) segs = new List<ViewerObject.VSEGMENT>();
                else segs.Clear();

                _doing = "Set each VSEGMENT by ASEGMENT";
                foreach (var aseg in asegs)
                {
                    if (!SetVSEGMENT(aseg, out ViewerObject.VSEGMENT seg, out result)) return false;
                    segs.Add(seg);
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVSEGMENT(ASEGMENT aseg, out ViewerObject.VSEGMENT seg, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            seg = null;

            if (aseg == null)
            {
                result = $"{ns}.{ms} - aseg = null";
                return false;
            }

            string _doing = "";
            string SEGID = "";
            try
            {
                SEGID = "3" + aseg.SEG_ID.Remove(0, 1).Trim();//因為C那邊傳過來是001，但實際使用是301，要把第一碼轉換為3
                _doing = $"new ViewerObject.VSEGMENT({SEGID}, {aseg.STATUS == E_SEG_STATUS.Closed})";
                seg = new ViewerObject.VSEGMENT(SEGID, aseg.STATUS == E_SEG_STATUS.Closed);

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
