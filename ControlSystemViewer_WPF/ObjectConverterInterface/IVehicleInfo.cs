using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IVehicleInfo
    {
        string GetVhID(object vh_info_obj);
        string GetCmdID(object vh_info_obj);
        string GetCstID(object vh_info_obj);
        bool Convert2Object_VehicleInfo(byte[] bytes, out object vh_info, out string result);
        bool SetVVEHICLE(ref ViewerObject.VVEHICLE vh, object vh_info_obj, out string result);
    }
}
