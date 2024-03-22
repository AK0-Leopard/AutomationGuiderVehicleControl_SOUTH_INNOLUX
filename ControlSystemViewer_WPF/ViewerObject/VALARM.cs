using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ViewerObject
{
    public class VALARM
    {
        public VALARM()
        {
        }
        public string CMD_ID { get; set; }
        public string ALARM_CODE { get; set; }
        public string EQPT_ID { get; set; }
        public VALARM_Def.AlarmLvl ALARM_LVL { get; set; }
        public string ALARM_DESC { get; set; }
        public string RPT_DATE_TIME { get; set; }
        public string CLEAR_DATE_TIME { get; set; }
        public bool IS_CLEARED { get; set; }

        public double? AlarmTime
        {
            get
            {
                if (CLEAR_DATE_TIME == "") return null;
                if (Math.Round(Convert.ToDateTime(CLEAR_DATE_TIME).Subtract(Convert.ToDateTime(RPT_DATE_TIME)).TotalMinutes, 1) < 0) return null;
                return Math.Round(Convert.ToDateTime(CLEAR_DATE_TIME).Subtract(Convert.ToDateTime(RPT_DATE_TIME)).TotalMinutes, 1);
            }
        }

        //public int ALARM_CLASSIFICATION { get; set; } //alarm的分類

        public int _ALARM_CLASSIFICATION { private get; set; }

        public string ALARM_CLASSIFICATION
        {
            get
            {
                switch (_ALARM_CLASSIFICATION)
                {
                    case 1:
                        return "1.維修保養";
                    case 2:
                        return "2.異常衍生";
                    case 3:
                        return "3.人因產生";
                    case 4:
                        return "4.EQ產生";
                    case 5:
                        return "5.其他";
                    case 6:
                        return "6.硬體";
                    case 7:
                        return "7.軟體";
                    case 8:
                        return "8.電氣";
                    default:
                        return "";
                }
            }
        }

        public string ALARM_REMARK { get; set; } //alarm的實際描述

        public DateTime _RPT_DATE_TIME { get; set; }

        public string ADDRESS_ID { get; set; }
        public string PORT_ID { get; set; }
        public string CARRIER_ID { get; set; }
        public int ALARM_MODULE { get; set; }
        public string sALARM_MODULE { get; set; }
        public VALARM_Def.AlarmImportanceLevel IMPORTANCE_LEVEL { get; set; }


        #region For M4
        public string VH_INSTALL_FLAG { get; set; } = null;
        #endregion
    }


    public static class VALARM_Def
    {
        public enum AlarmLvl
        {
            None = 0,
            Warn = 1,
            Error = 2
        }
        public enum AlarmImportanceLevel
        {
            Low,
            High
        }


        public enum VH_INSTALL_FLAG
        {
            NULL=-1,
            Remove=0,
            Install=1
        }
    }
}
