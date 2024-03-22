using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IAVehicle
    {
        bool SetVVEHICLEs(ref List<ViewerObject.VVEHICLE> vhs, string data, out string result);
    }
}
