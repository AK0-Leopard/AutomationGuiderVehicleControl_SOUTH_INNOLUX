using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VOPERATION
    {
        public VOPERATION()
        {
        }

        public string SEQ_NO { get; set; } = "";
        public string T_STAMP { get; set; } = "";
        public string USER_ID { get; set; } = "";
        public string FORM_NAME { get; set; } = "";
        public string ACTION { get; set; } = "";
        public string BUTTON_NAME { get; set; } = "";
        public string BUTTON_CONTENT { get; set; } = "";
        public Dictionary<string, string> input = null;
        public DateTime INSERT_TIME => Convert.ToDateTime(T_STAMP);
    }
}
