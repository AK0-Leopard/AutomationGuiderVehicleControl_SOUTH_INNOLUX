using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class SYSTEMVOPERATION
    {
        public SYSTEMVOPERATION()
        {
        }

        public string TIME { get; set; } = "";
        public string USER_ID { get; set; } = "";
        public string FUNCTION { get; set; } = "";
        public string RESULT { get; set; } = "";
        public Dictionary<string, string> input = null;
        public Dictionary<string, string> input2 = null;
        public DateTime INSERT_TIME => Convert.ToDateTime(TIME);
    }
}
