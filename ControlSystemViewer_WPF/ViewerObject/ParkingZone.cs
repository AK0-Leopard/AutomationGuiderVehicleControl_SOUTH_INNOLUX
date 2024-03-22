using com.mirle.ibg3k0.sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class ParkingZone
    {
		public string ParkingZoneID;                    //Parking Zone ID
		public List<string> ParkAddressIDs;             //這個Parking Zone內的停車位。List內的順序就是權重，第一位是入口，最後一位是出口
		public List<E_VH_TYPE> AllowedVehicleTypes;     //哪些Type的車可以停這裡?
		public int LowWaterlevel;                       //低水位
		public E_PARK_TYPE order;
		public int PullDest;                            //允許拉車最遠距離(0為不限制)

		//以下是可以由其他屬性推算的資料
		public int Capacity;// => ParkAddressIDs.Count();                  //停車區容量
		public string EntryAddress => ParkAddressIDs.FirstOrDefault();  //入口Address
		public string ExitAddress => ParkAddressIDs.LastOrDefault();    //出口Address
	}
}
