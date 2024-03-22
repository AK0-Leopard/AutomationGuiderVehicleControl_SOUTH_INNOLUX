using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VMCBF
    {
        private string vhid;
        public string VH_ID 
        {
            get { return vhid; }
        }

        public double MCBF
        {
            get { return Math.Ceiling(CMDCount / (AlarmCount+1)); }
        }

        public double AlarmCount { get; set; }
        public double CMDCount { get; set; }
        //public string TimeInterval
        //{
        //    get
        //    {
        //        return dTimeInterval.ToString("N0");
        //    }
        //}
        //private double dTimeInterval { get; set; }

      


        public VMCBF(string _vhid)
        {
            vhid = _vhid;
            CMDCount = 1;
        }

        //public void AddTimeInterval(DateTime Start_Time, DateTime End_Time)
        //{
        //    dTimeInterval = End_Time.Subtract(Start_Time).TotalHours;

        //}
    }

    public class VMCBF_FirstReport
    {

        public string VH_ID { get; set; }
        public DateTime First_RPT_DataTime { get; set; }
        public DateTime Final_Clear_DateTime { get; set; }
        public int AlarmCount { get; set; } = 1;

    }

}
