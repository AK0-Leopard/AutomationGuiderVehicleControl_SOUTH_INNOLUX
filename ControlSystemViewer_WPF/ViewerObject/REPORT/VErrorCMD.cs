using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ViewerObject.REPORT.Define;

namespace ViewerObject.REPORT
{
    public class VErrorCMD
    {
        //由於ErrorCMD與超時CMD顯示內容相同，僅條件差異，故使用同樣元件，若有需要分離要再創一個超時CMD的Class
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

        public string CARRIER_TYPE { get; set; } = "";


    }

    public class VErrorCMD_OHTC
    {

        public List<VALARM> AlarmsInCMD = new List<VALARM>();

        public string ID { get; set; } = "";
        public string MCS_CMD_ID { get; set; } = "";
        public string VH_ID { get; set; } = "";
        public string CARRIER_ID { get; set; } = "";
        public string HOSTSOURCE { get; set; } = "";
        public string HOSTDESTINATION { get; set; } = "";
        public E_OHTC_CMD_STATUS OHTC_CMD_STATUS { get; set; }
        //public DateTime INSERT_TIME { get; set; }
        public DateTime START_TIME { get; set; }
        public DateTime FINISH_TIME { get; set; }

        public string CARRIER_TYPE { get; set; } = "";


    }
}
