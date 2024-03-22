using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IAPortStation
    {
        bool SetVPORTSTATIONs(ref List<ViewerObject.VPORTSTATION> ports, string data, out string result);
    }
}
