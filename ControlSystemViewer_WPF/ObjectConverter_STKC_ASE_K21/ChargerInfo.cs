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

namespace ObjectConverter_STKC_ASE_K21
{
    public class ChargerInfo : IChargerInfo
    {
        private readonly string ns = "ObjectConverter_STKC_ASE_K21" + ".ChargerInfo";

        public bool SetChargers(ref List<ViewerObject.Charger> objs, string couplerData, string unitData, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = $"{ns}.{ms} - Unsupport";
            return false;
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
