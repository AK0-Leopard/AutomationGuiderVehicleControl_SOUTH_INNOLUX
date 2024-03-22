using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{

    public class VUtilizationRateByStatus
    {
        public VUtilizationRateByStatus(string _vhid,
                                        double _mcscmdtime, double _ohtccmdtime, double _longchargetime, double _unscheduledowntime, double _scheduledowntime,
                                        DateTime _starttime, DateTime _endtime, int _mcscmdcount, double _utilizationrate = double.MinValue, double _totalIdleTime=double.MinValue)
        {
            vhid = _vhid;
            starttime = _starttime;
            endtime= _endtime;
            mcscycletime = _mcscmdtime;
            ohtccycletime = _ohtccmdtime;
            longchargetime = _longchargetime;
            unscheduledowntime = _unscheduledowntime;
            scheduledowntime = _scheduledowntime;

            mcscmdcount = _mcscmdcount;

            if (_utilizationrate != double.MinValue)//因為Total的有倍率問題，所以開放自己填
            {
                utilizationrate = _utilizationrate;
            }
            else
            {
                utilizationrate = Math.Round( (mcscycletime + ohtccycletime + longchargetime)  / (endtime.Subtract(starttime).TotalSeconds - scheduledowntime) * 100, 2);
            }


            if (_totalIdleTime != double.MinValue)//因為Total的有倍率問題，所以開放自己填
            {
                idletime = _totalIdleTime;
            }
            else
            {
                idletime = endtime.Subtract(starttime).TotalSeconds - scheduledowntime - unscheduledowntime - longchargetime - ohtccycletime - mcscycletime;
            }

        }

        private string vhid { get; set; }
        public string VHID { get { return vhid; } }

        private double utilizationrate { get; set; }
        public double UtilizationRate
        {
            get { return utilizationrate; }
        }

        private int mcscmdcount { get; set; }
        public int MCSCMDCount
        {
            get { return mcscmdcount; }
        }

        public double IntervalTime { get { return endtime.Subtract(starttime).TotalHours; } }//Unit: Hour

        public double MCSCycleTime { get { return Math.Round(mcscycletime / 60 /60,2); } }//Unit: Hour
        private double mcscycletime { get; set; }//Unit : s

        public double OHTCCycleTime { get { return Math.Round(ohtccycletime / 60 / 60, 2); } }//Unit: Hour
        private double ohtccycletime { get; set; }//Unit : s

        public double LongChargeTime { get { return Math.Round(longchargetime / 60 / 60 , 2); } }//Unit: Hour
        private double longchargetime { get; set; }//unit : s

        public double UnScheduleDownTime { get { return Math.Round(unscheduledowntime / 60 , 2); } }//Unit: Min
        private double unscheduledowntime { get; set; }//unit : s

        public double ScheduleDownTime { get { return Math.Round(scheduledowntime / 60 , 2); } }//Unit: Min
        private double scheduledowntime { get; set; }//unit : s


        public double IdleTime { get { return Math.Round(idletime / 60/60, 2); } }//Unit: Hour
        private double idletime { get; set; }//unit : s

        //public DateTime StartTime { get { return starttime; } }
        private DateTime starttime { get; set; }

        //public DateTime EndTime { get { return endtime; } }
        private DateTime endtime { get; set; }

       
    }
}
