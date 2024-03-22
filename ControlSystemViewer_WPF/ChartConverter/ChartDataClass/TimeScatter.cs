using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConverter.ChartDataClass
{
    public class TimeScatter
    {
        public bool ShowValue { get; set; } = false;
        DateTime starttime = DateTime.MinValue;
        public DateTime StartTime
        {
            get
            {
                return starttime;
            }
        }

        DateTime endtime = DateTime.MinValue;
        public DateTime EndTime
        {
            get
            {
                return endtime;
            }
        }

        public int TimeTick = 1; //Default 1 hr
        public Dictionary<string, List<double>> dicScatter
        {
            get
            {
                return dicscatter;
            }
        }
        Dictionary<string, List<double>> dicscatter = new Dictionary<string, List<double>>();

        public TimeScatter(DateTime _StartTime, DateTime _EndTime, Dictionary<string, List<double>> _dicScatter,string _Unit = "count")
        {
            starttime = _StartTime;
            endtime = _EndTime;
            dicscatter = _dicScatter;
            Unit = _Unit;
        }

        public int HourCount
        {
            get
            {
                return Convert.ToInt16(Math.Floor(endtime.Subtract(starttime).TotalHours) + 1);
            }
        }

        public string Unit { get; set; } = "count";
        public string YAxisTitle { get; set; } = "";
    }
}
