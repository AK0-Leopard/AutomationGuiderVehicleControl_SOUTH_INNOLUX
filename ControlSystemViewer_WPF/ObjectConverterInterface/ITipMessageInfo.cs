using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface ITipMessageInfo
    {
        bool SetVTIPMESSAGEs(ref List<ViewerObject.VTIPMESSAGE> tipMsgs, byte[] bytes, out string result);
    }
}
