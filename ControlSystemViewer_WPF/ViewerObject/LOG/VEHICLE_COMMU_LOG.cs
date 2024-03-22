using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.LOG
{
    public class VEHICLE_COMMU_LOG
    {
        public VEHICLE_COMMU_LOG()
        {
        }

        public string VhID { get; set; } = "";
        public string SendRecive { get; set; } = "";
        public string FunName { get; set; } = "";
        public string SeqNo { get; set; } = "";
        public string CmdID { get; set; } = "";
        public string McsCmdID { get; set; } = "";
        public string ActType { get; set; } = "";
        public string EventType { get; set; } = "";
        public string Message { get; set; } = "";
        public string Time { get; set; } = "";
        public string TimeWithoutDate => Convert.ToDateTime(Time).ToString("HH:mm:ss.fff");
    }
}
