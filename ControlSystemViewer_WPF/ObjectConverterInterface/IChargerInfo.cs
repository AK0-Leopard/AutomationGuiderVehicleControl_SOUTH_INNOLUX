using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessage.ProtocolFormat.ControllerSettingFun;

namespace ObjectConverterInterface
{
    public interface IChargerInfo
    {
        bool SetChargers(ref List<ViewerObject.Charger> objs, string couplerData, string unitData, out string result);
        (bool, ChargeSettingReply) chargeControl(ChargeSettingRequest request);
    }
}
