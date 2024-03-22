using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace ObjectConverter_STKC_ASE_K21
{

    public class K21MaintenanceAlarm : MaintenanceAlarm
    {

        public K21MaintenanceAlarm(VALARM VAlarm, int ColNumber, List<EQMap> eqMaps) : base(VAlarm, ColNumber)
        {
            colNumber = ColNumber;
            pDate = Convert.ToDateTime(VAlarm.RPT_DATE_TIME);
            palarmHappend = Convert.ToDateTime(VAlarm.RPT_DATE_TIME);
            palarmClear = (VAlarm.CLEAR_DATE_TIME == null) ? DateTime.MaxValue : Convert.ToDateTime(VAlarm.CLEAR_DATE_TIME);
            pworkTime = VAlarm.AlarmTime.ToString();
            pschedule = "";
            var try_get_eq_map_result = tryGetEqMap(eqMaps, VAlarm.EQPT_ID);
            if(try_get_eq_map_result.isExist)
            {
                pEQ_Name = try_get_eq_map_result.eqName;
                pEQ_Number = try_get_eq_map_result.eqNumber;
            }
            else
            {
                pEQ_Name = "Other";
                pEQ_Number = VAlarm.EQPT_ID;
            }

            pmoduleClassification = VAlarm.sALARM_MODULE;
            palarmClassification = VAlarm.ALARM_CLASSIFICATION;
            switch(VAlarm.IMPORTANCE_LEVEL)
            {
                case VALARM_Def.AlarmImportanceLevel.Low:
                    pimportance = "低";
                    break;
                case VALARM_Def.AlarmImportanceLevel.High:
                    pimportance = "高";
                    break;
            }
            palarmCode = VAlarm.ALARM_CODE;
            palarmDesc = VAlarm.ALARM_DESC;
            palarmRemark = VAlarm.ALARM_REMARK;
            plocation = VAlarm.ADDRESS_ID;
            pportID = VAlarm.PORT_ID;
            pboxNumber = VAlarm.CARRIER_ID;
            pworkingNumber = "U332MTB0";
            pworkingName = "日月光 K24 BE2 OHT";
            abnormalOrResidual = AbnormalOrResidual.isAbnormal;
        }

        private (bool isExist, string eqName, string eqNumber) tryGetEqMap(List<EQMap> eqMaps, string id)
        {
            var eq_map = eqMaps.Where(map => map.id?.Trim() == id?.Trim()).FirstOrDefault();
            if (eq_map == null)
            {
                return (false, "", "");
            }
            else
            {
                return (true, eq_map.eqName, eq_map.eqNumber);
            }
        }
    }
}
