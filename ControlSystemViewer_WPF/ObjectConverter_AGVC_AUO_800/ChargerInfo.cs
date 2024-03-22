using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data.VO;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessage.ProtocolFormat.ControllerSettingFun;

namespace ObjectConverter_AGVC_AUO_800
{
    public class ChargerInfo : IChargerInfo
    {
        private readonly string ns = "ObjectConverter_AGVC_AUO_800" + ".ChargerInfo";

        public bool GetCouplerInfos(string input, out List<CouplerInfo> output, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            output = new List<CouplerInfo>();
            result = "";
            if (string.IsNullOrWhiteSpace(input))
            {
                result = $"{ns}.{ms} - input = null or empty";
                return false;
            }

            try
            {
                output.AddRange(JsonConvert.DeserializeObject<List<CouplerInfo>>(input) ?? new List<CouplerInfo>());

                if (output?.Count > 0) return true;
                else
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<CouplerInfo>> result = null, input: {input}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<CouplerInfo>> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool GetAUNITs(string input, out List<AUNIT> output, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            output = new List<AUNIT>();
            result = "";
            if (string.IsNullOrWhiteSpace(input))
            {
                result = $"{ns}.{ms} - input = null or empty";
                return false;
            }

            try
            {
                output.AddRange(JsonConvert.DeserializeObject<List<AUNIT>>(input) ?? new List<AUNIT>());

                if (output?.Count > 0) return true;
                else
                {
                    result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<AUNIT>> result = null, input: {input}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - JsonConvert.DeserializeObject<List<AUNIT>> failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetChargers(ref List<ViewerObject.Charger> objs, string couplerData, string unitData, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            List<CouplerInfo> couplerInfos;
            List<AUNIT> aUnits;
            if (!GetCouplerInfos(couplerData, out couplerInfos, out result)) return false;
            if (!GetAUNITs(unitData, out aUnits, out result)) return false;

            string _doing = "";
            try
            {
                if (objs == null) objs = new List<ViewerObject.Charger>();
                _doing = "Set Chargers by CouplerInfos and AUNITs";
                for (int i = 0; i < couplerInfos.Count; i++)
                {
                    if (!objs.Any(o => o.ChargerID == couplerInfos[i].ChargerID.Trim()))
                        objs.Add(new ViewerObject.Charger(couplerInfos[i].ChargerID.Trim()));
                    ViewerObject.Charger charger = objs?.Where(o => o.ChargerID == couplerInfos[i].ChargerID.Trim()).FirstOrDefault();
                    if (!SetCharger(ref charger, couplerInfos[i], aUnits.FirstOrDefault(u => u.UNIT_ID?.Trim() == couplerInfos[i].ChargerID?.Trim()), out result))
                        return false;
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public bool SetCharger(ref ViewerObject.Charger obj, CouplerInfo coupler, AUNIT unit, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";

            if (coupler == null)
            {
                result = $"{ns}.{ms} - coupler = null";
                return false;
            }
            if (unit == null)
            {
                result = $"{ns}.{ms} - unit = null";
                return false;
            }

            string charger_id = coupler.ChargerID?.Trim();
            if (obj == null) obj = new ViewerObject.Charger(charger_id);
            else
            {
                if (obj.ChargerID != charger_id)
                {
                    result = $"{ns}.{ms} - id not same ({obj.ChargerID} != {charger_id})";
                    return false;
                }
            }

            string _doing = "";
            try
            {
                _doing = "Set AddressID";
                obj.AddressID = coupler?.AddressID?.Trim() ?? "";
                _doing = "Set AliveHeartbeat";
                obj.AliveHeartbeat = unit?.ChargerAlive ?? 0;
                _doing = "Set Coupler";
                ViewerObject.Coupler oCoupler = null;

                int? id = coupler?.CouplerNum;

                if (id != null)
                {


                    oCoupler = new ViewerObject.Coupler(id.Value, "Coupler" + (id).ToString(), obj.ChargerID);
                    oCoupler.AddressID = coupler.AddressID;
                    _doing = "Set Coupler Status";
                    switch (id)
                    {
                        case 1:
                            oCoupler.Status = unit?.coupler1Status == null ? ViewerObject.Coupler_Def.CouplerStatus.None :
                             new Definition.Convert().GetCouplerStatus(unit.coupler1Status);
                            break;

                        case 2:
                            oCoupler.Status = unit?.coupler2Status == null ? ViewerObject.Coupler_Def.CouplerStatus.None :
                             new Definition.Convert().GetCouplerStatus(unit.coupler2Status);
                            break;

                        case 3:
                            oCoupler.Status = unit?.coupler3Status == null ? ViewerObject.Coupler_Def.CouplerStatus.None :
                             new Definition.Convert().GetCouplerStatus(unit.coupler3Status);
                            break;
                        default:
                            break;


                    }
                    obj.Couplers.AddOrUpdate(oCoupler.Name, oCoupler, (key, oldValue) => oldValue = oCoupler);
                }

                return true;
            }
            catch
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }

        public (bool, ChargeSettingReply) chargeControl(CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingRequest request)
        {
            bool isSuccess = false;
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingReply result = new CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingReply();
            result.Result = $"{ns}.{ms} - Unsupport";
            return (isSuccess, result);
        }

    }
}
