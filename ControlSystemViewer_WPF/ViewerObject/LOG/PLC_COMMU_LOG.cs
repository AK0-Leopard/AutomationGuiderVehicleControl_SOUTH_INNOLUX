using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.LOG
{
    public class PLC_COMMU_LOG
    {
        public PLC_COMMU_LOG()
        {
        }

        public string LogLevel { get; set; } = "";
        public string Class { get; set; } = "";
        public string Method { get; set; } = "";
        public string Device { get; set; } = "";
        public string LogID { get; set; } = "";
        public string ThreadID { get; set; } = "";
        public string Data { get; set; } = "";
        public string VhID { get; set; } = "";
        public string CrrID { get; set; } = "";
        public string Type { get; set; } = "";
        public string Lot { get; set; } = "";
        public string Level { get; set; } = "";
        public string XID { get; set; } = "";
        public string Seq { get; set; } = "0";
        public string TrxID { get; set; } = "";
        public string Details { get; set; } = "";
        public string Time { get; set; } = "";
        public string TimeWithoutDate => Convert.ToDateTime(Time).ToString("HH:mm:ss.fff");
    }
}
