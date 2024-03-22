using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K24
{
    public class Alarm : IAlarm
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K24" + ".Alarm";

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
            if (valarms == null) valarms = new List<ViewerObject.VALARM>();
            return true;

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
                valarm.CLEAR_DATE_TIME = null;
                if (alarm.END_TIME != null)
                    valarm.CLEAR_DATE_TIME = alarm.END_TIME?.ToString("yyyy/MM/dd HH:mm:ss");

                _doing = $"Set ALARM_CLASSIFICATION";
                //valarm.ALARM_CLASSIFICATION = alarm.CLASS.ToString() ?? "0";
                //valarm._ALARM_CLASSIFICATION = alarm.CLASS ?? 0;
                valarm._ALARM_CLASSIFICATION = alarm.CLASS.HasValue ? (int)alarm.CLASS : 0;
                _doing = $"Set ALARM_REMARK";
                valarm.ALARM_REMARK = alarm.REMARK?.Trim() ?? "";
                _doing = $"Set _RPT_DATE_TIME";
                valarm._RPT_DATE_TIME = alarm.RPT_DATE_TIME;
                _doing = $"Set ADDRESS_ID";
                valarm.ADDRESS_ID = alarm.ADDRESS_ID;
                _doing = $"Set PORT_ID";
                valarm.PORT_ID = alarm.PORT_ID;
                _doing = $"Set CARRIER_ID";
                valarm.CARRIER_ID = alarm.CARRIER_ID;
                _doing = $"Set IMPORTANCE_LEVEL";
                valarm.IMPORTANCE_LEVEL = alarm.IMPORTANCE_LVL.HasValue ?
                    (ViewerObject.VALARM_Def.AlarmImportanceLevel)alarm.IMPORTANCE_LVL.Value : ViewerObject.VALARM_Def.AlarmImportanceLevel.Low;
                _doing = $"Set ALARM_MODULE";
                valarm.ALARM_MODULE = alarm.ALARM_MODULE.HasValue ? alarm.ALARM_MODULE.Value : 0;
                _doing = $"Set CMD_ID";
                valarm.CMD_ID = alarm.CMD_ID?.Trim() ?? "";
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
