using com.mirle.ibg3k0.sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public partial class ParkZoneMaster
    {
        public string PARK_TYPE_ID { get; set; }
        public string PARK_ZONE_ID { get; set; }
        public int VEHICLE_TYPE { get; set; }
        public string ENTRY_ADR_ID { get; set; }
        public int TOTAL_BORDER { get; set; }
        public int LOWER_BORDER { get; set; }
        public int PARK_TYPE { get; set; }
        public bool IS_ACTIVE { get; set; }
        public int PULL_DEST { get; set; }
    }
}
