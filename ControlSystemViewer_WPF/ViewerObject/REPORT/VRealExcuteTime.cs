using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
    public class VRealExcuteTime
    {
        public string SOURCE_ADR { get; set; } = "";
        public string DESTINATION_ADR { get; set; } = "";
        public double? Mean_CMDQUEUE_TIME { get; set; } = null;
        public double? Mean_MOVE_TO_SOURCE_TIME { get; set; } = null;
        public double? Mean_MOVE_TO_DESTN_TIME { get; set; } = null;
        public double? Mean_CMD_TOTAL_EXCUTION_TIME { get; set; } = null;
    }
}
