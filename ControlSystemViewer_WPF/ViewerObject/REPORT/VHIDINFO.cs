using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ViewerObject.REPORT.Define;

namespace ViewerObject.REPORT
{
    public class VHIDINFO
    {

        public VHIDINFO()
        {
        }
        //public double WT_Converted { get; set; }
        //public double? WS_Converted { get; set; }
        //public double? WR_Converted { get; set; }
        //public double? Sigma_A_Converted { get; set; }
        //public double? AT_Converted { get; set; }
        //public double? AS_Converted { get; set; }
        //public double? AR_Converted { get; set; }
        //public double? Sigma_V_Converted { get; set; }
        //public double? VT_Converted { get; set; }
        //public double? VS_Converted { get; set; }
        //public double? VR_Converted { get; set; }   
        public double? V_Converted { get; set; }
        public double? A_Converted { get; set; }
        public double? W_Converted { get; set; }      
        public string HID_ID { get; set; }
        public DateTime UPD_TIME { get; set; }
        public double? Sigma_W_Converted { get; set; }
        public double? Hour_Negative_Converted { get; set; }
        public double? Hour_Positive_Converted { get; set; }
        public double? Hour_Sigma_Converted { get; set; }
    }
}
