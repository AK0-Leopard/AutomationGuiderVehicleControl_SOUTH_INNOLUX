using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ViewerObject.REPORT.Define;

namespace ViewerObject.REPORT
{
    public class VTimeOutCMD
    {
        public List<VALARM> AlarmsInCMD = new List<VALARM>();

        public string MCS_CMD_ID { get; set; } = "";
        public string VH_ID { get; set; } = "";
        public string CARRIER_ID { get; set; } = "";
        public string HOSTSOURCE { get; set; } = "";
        public string HOSTDESTINATION { get; set; } = "";
        public E_OHTC_CMD_STATUS OHTC_CMD_STATUS { get; set; }
        public DateTime MCS_INSERT_TIME { get; set; }
        public DateTime MCS_START_TIME { get; set; }
        public DateTime MCS_FINISH_TIME { get; set; }
        public int CycleTime
        {  //Unit : Minute
            get
            {
                return  Convert.ToInt32(MCS_FINISH_TIME.Subtract(MCS_START_TIME).TotalMinutes);
            }
        }
    }
}
