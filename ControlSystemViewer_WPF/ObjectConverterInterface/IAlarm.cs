using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IAlarm
    {
        bool SetVALARMs(ref List<ViewerObject.VALARM> valarms, string data, out string result);
    }
}
