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
        public string alarmID;
        public string alarmLVL;
        public string alarmDesc;
        public string alarmDesc_TW;

        public AlarmMap(string ObjectID, string AlarmID, string Alarm_LVL, string AlarmDesc, string AlarmDesc_TW)
        {
            objectID = ObjectID;
            alarmID = AlarmID;
            alarmLVL = Alarm_LVL;
            alarmDesc = AlarmDesc;
            alarmDesc_TW = AlarmDesc_TW;
        }

    }
}
