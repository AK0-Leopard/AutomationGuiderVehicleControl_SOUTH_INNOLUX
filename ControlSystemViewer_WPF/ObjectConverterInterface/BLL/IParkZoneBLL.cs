using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface IParkZoneBLL
    {
        List<ViewerObject.ParkingZone> GetAllParkingZoneData();

        List<ViewerObject.ParkZoneMaster> LoadParkingZoneMaster();

        List<ViewerObject.ParkZoneDetail> LoadParkingZoneDetail(string address);
    }
}
