using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class AlarmMap
    {
        public string objectID;
        public string alarmID { get; set; } = "";
        public string alarmLVL;
        public string alarmDesc { get; set; } = "";
        public string alarmDesc_TW;
        public string happendReason { get; set; } = "";
        public string solution { get; set; } = "";

        public AlarmMap(string ObjectID, string AlarmID, string Alarm_LVL, string AlarmDesc, string AlarmDesc_TW, string HAPPENED_REASON, string SOLUSTION)
        {
            objectID = ObjectID;
            alarmID = AlarmID;
            alarmLVL = Alarm_LVL;
            alarmDesc = AlarmDesc;
            alarmDesc_TW = AlarmDesc_TW;
            happendReason = HAPPENED_REASON;
            solution = SOLUSTION;
        }

    }
}
