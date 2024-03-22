using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.LOG
{
    public class MCS_COMMU_LOG
    {
        public MCS_COMMU_LOG()
        {
        }

        public string EqID { get; set; } = "";
        public string SendRecive { get; set; } = "";
        public string FunName { get; set; } = "";
        public string Sx { get; set; } = "";
        public string Fy { get; set; } = "";
        public string DeviceID { get; set; } = "";
        public string Message { get; set; } = "";
        public string Time { get; set; } = "";      
        public string TimeWithoutDate => Convert.ToDateTime(Time).ToString("HH:mm:ss.fff");

        private string tempCEID = "";
        public string CEID
        {
            get
            {
                return getCEID();
            }
        }
        private string getCEID()
        {
            if (tempCEID != "") return tempCEID;
            string ceid = "";
            string[] lssplit = null;
            string[] strline = null;
            if (Message.Contains("CEID"))
            {
                lssplit = Message.Split('<');
                foreach (string str in lssplit)
                {
                    if (str.Contains("CEID"))
                    {
                        strline = str.Split('"');
                        ceid = strline[1];
                        break;
                    }
                }

            }
            tempCEID = ceid;
            return ceid;
        }
    }
}
