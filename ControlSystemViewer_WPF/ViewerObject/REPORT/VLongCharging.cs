using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
    public  class VLongCharging
    {
        public VLongCharging(string _vhid)
        {
            vhid = _vhid;
        }

        public string VHID { get { return vhid; } }
        private string vhid;

        public long ChargingCount { get; set; } = 0;
        public double ChargingTime { get; set; } = 0;//Unit:Minute
    }
}
