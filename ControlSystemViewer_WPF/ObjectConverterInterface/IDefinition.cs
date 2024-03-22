using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IDefinition
    {
        string GetString(ViewerObject.VCMD_Def.CmdType cmdType);
        string GetString(ViewerObject.VVEHICLE_Def.VPauseType pauseType);
        string GetString(ViewerObject.VVEHICLE_Def.VPauseEvent pauseEvent);
        string GetString(ViewerObject.VVEHICLE_Def.ModeStatus modeStatus);
        string GetString(ViewerObject.VPORTSTATION_Def.PortStatus portStatus);
        string GetString(ViewerObject.VPORTSTATION_Def.PortServiceStatus portServiceStatus);
    }
}
