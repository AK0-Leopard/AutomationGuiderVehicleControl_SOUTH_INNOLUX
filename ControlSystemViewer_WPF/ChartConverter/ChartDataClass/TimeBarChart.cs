using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace ChartConverter.ChartDataClass
{
    public class TimeBarChart
    {
        //重疊Bar型的資料維度會再多一維，因為單筆Bar會重疊多筆資料

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
        public Dictionary<string, Dictionary<string, List<double>>> dicBar
        {
            get
            {
                return dicbar;
            }
        }
        Dictionary<string, Dictionary<string, List<double>>>  dicbar = new Dictionary<string, Dictionary<string, List<double>>>();
        //Key值: 要顯示的資料筆數(ex: MCS命令總數、完成命令總數，這樣就是兩筆)
        //Value: 因為有重疊Bar的設計，在單一X座標軸上可能會有多筆資料重疊，所以第一維度的List是X座標維度
        //       第二維度的List則是要顯示在同一個Bar上的資料集合

        public TimeBarChart(DateTime _StartTime, DateTime _EndTime, Dictionary<string, Dictionary<string, List<double>>> _dicBar, string _Unit = "count")
        {
            starttime = _StartTime;
            endtime = _EndTime;
            dicbar = _dicBar;
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
