using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
    //繼承VCMD，用於顯示報表匯出使用，會增加一些額外欄位的資訊
    public class VCMD_ExportDetail : VCMD
    {
        public VCMD_ExportDetail()
        {
        }

        public double? CMDCycleTime
        {
            get
            {
                if (START_TIME == null || END_TIME == null) return null;

                return Math.Round((double)(END_TIME?.Subtract(START_TIME.Value).TotalMinutes), 2);
            }
        }

        public string CARRIER_TYPE { get; set; } = "";//M4用
        public string CARRIER_ID_ON_CRANE { get; set; } = "";//M4用


    }

}
