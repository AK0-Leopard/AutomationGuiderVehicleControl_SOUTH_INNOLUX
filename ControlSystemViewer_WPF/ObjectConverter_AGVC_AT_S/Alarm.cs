using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_AT_S
{
    public class Alarm : IAlarm
    {
        private readonly string ns = "ObjectConverter_AGVC_AT_S" + ".Alarm";

        public bool Convert2Object_ALARMs(string input, out List<ALARM> obj, out string result)
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
                obj = JsonConvert.DeserializeObject<List<ALARM>>(input);
                if (obj == null)
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<ALARM>> result = null, input: {input}";
                    return false;
                }
                else return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<ALARM>> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetVALARMs(ref List<ViewerObject.VALARM> valarms, string data, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            string _doing = "";
            try
            {
                _doing = "Convert data to List<ALARM>";
                bool isSuccess = Convert2Object_ALARMs(data, out List<ALARM> alarms, out result);
                if (!isSuccess) return false;

                _doing = "Init List<VALARM>";
                if (valarms == null) valarms = new List<ViewerObject.VALARM>();
                else valarms.Clear();

                _doing = "Set each VALARM by ALARM";
                foreach (var alarm in alarms)
                {
                    if (!SetVALARM(alarm, out ViewerObject.VALARM valarm, out result)) return false;
                    valarms.Add(valarm);
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetVALARM(ALARM alarm, out ViewerObject.VALARM valarm, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            valarm = null;

            if (alarm == null)
            {
                result = $"{ns}.{ms} - alarm = null";
                return false;
            }

            string _doing = "";
            try
            {
                _doing = $"new ViewerObject.VALARM()";
                valarm = new ViewerObject.VALARM();
                _doing = $"Set RPT_DATE_TIME";
                valarm.RPT_DATE_TIME = alarm.RPT_DATE_TIME.ToString("yyyy/MM/dd HH:mm:ss");
                _doing = $"Set EQPT_ID";
                valarm.EQPT_ID = alarm.EQPT_ID?.Trim() ?? "";
                _doing = $"Set ALARM_LVL";
                valarm.ALARM_LVL = new Definition.Convert().GetAlarmLevel(alarm.ALAM_LVL);
                _doing = $"Set ALARM_CODE";
                valarm.ALARM_CODE = alarm.ALAM_CODE?.Trim() ?? "";
                _doing = $"Set ALARM_DESC";
                valarm.ALARM_DESC = alarm.ALAM_DESC?.Trim() ?? "";
                _doing = $"Set IS_CLEARED";
                valarm.IS_CLEARED = alarm.ALAM_STAT == ErrorStatus.ErrReset;
                _doing = $"Set CLEAR_DATE_TIME";
                valarm.CLEAR_DATE_TIME = alarm.CLEAR_DATE_TIME?.ToString("yyyy/MM/dd HH:mm:ss") ?? "";

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
