using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessage.ProtocolFormat.ControllerSettingFun;
using ViewerObject;

namespace ObjectConverterInterface
{
    public interface IConstant
    {
        List<VCONSTANT> getAllVConstant();
        (bool, ControllerParameterSettingReply) constantControl(ControllerParameterSettingRequest request);
        void resetBuzzer();
    }
}
