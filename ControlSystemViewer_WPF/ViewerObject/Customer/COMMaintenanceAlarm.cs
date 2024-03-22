using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.Customer
{
    // 售後服務用的特殊VALARM，共用父級Class，Default Class
    public  class MaintenanceAlarm
    {
        public string workingNumber 
        { 
            get 
            {
                if (string.IsNullOrEmpty(pworkingNumber))
                    return "";
                else 
                    return pworkingNumber;
            }
        } //工令號
        public string workingName
        {
            get
            {
                if (string.IsNullOrEmpty(pworkingName))
                    return "";
                else
                    return pworkingName;
            }
        }//工令名稱
        public string abnormalOrResidualStirng
        {
            get
            {
                if (abnormalOrResidual == AbnormalOrResidual.isAbnormal)
                    return "異常";
                else if (abnormalOrResidual == AbnormalOrResidual.isResidual)
                    return "殘項";
                else
                    return "例外";
            }
        } //異常 or 殘項
        
        public int colNumber { get; set; }
        public string OneDayCount { get; set; }
        public DateTime Date { get {return pDate; } }
        public DateTime alarmHappend { get {return palarmHappend; } }
        public DateTime alarmClear { get { return palarmClear; } }
        public string workTime { get { return pworkTime; } } //排除時間
        public string schedule { get { return pschedule; } } //排班，班別
        public string EQ_Name { get { return pEQ_Name; } } //設備名稱
        public string EQ_Number { get { return pEQ_Number; } } //設備編號
        public string moduleClassification { get { return pmoduleClassification; } } //模組分類
        public string alarmClassification { get { return palarmClassification; } } //異常分類
                                                   //public Importance importance { get; } //重要度
        public string importance { get { return pimportance; } } //重要度
        public string alarmCode { get { return palarmCode; } } //異常碼
        public string alarmDesc { get { return palarmDesc; } } //異常說明

        public string location { get { return plocation; } } //位置
        public string portID { get { return pportID; } } //位置
        public string CurrentHigh { get; set; } //卷陽當前高度
        public string TeachModifyData { get; set; } //TEACH修改數據
        public string boxNumber { get { return pboxNumber; } } //盒號
        public string Version { get; set; } //版本號碼

        public string alarmRemark { get { return palarmRemark; } } //異常碼註解
        public enum Importance
        {
            high = 3,
            normal = 2,
            low = 1
        }

        public enum AbnormalOrResidual
        {
            isAbnormal = 0,
            isResidual = 1,
        }


        #region proteced

        protected DateTime pDate { get; set; }
        protected DateTime palarmHappend { get; set; }
        protected DateTime palarmClear { get; set; }
        protected string pworkTime { get; set; } //排除時間
        protected string pschedule { get; set; } //排班，班別
        protected string pEQ_Name { get; set; } //設備名稱
        protected string pEQ_Number { get; set; } //設備編號
        protected string pmoduleClassification { get; set; }//模組分類
        protected string palarmClassification { get; set; } //異常分類
                                       //public Importance importance { get; } //重要度
        protected string pimportance { get; set; } //重要度
        protected string palarmCode { get; set; } //異常碼
        protected string palarmDesc { get; set; } //異常說明
        protected string plocation { get; set; } //位置
        protected string pportID { get; set; }//位置
        protected string pboxNumber { get; set; }//盒號
        protected string palarmRemark { get; set; }//異常碼註解
        protected string pworkingNumber { get; set; } //工令號
        protected string pworkingName { get; set; } //工令名稱
        protected AbnormalOrResidual abnormalOrResidual; //殘項or異常
        #endregion
        public MaintenanceAlarm(VALARM VAlarm, int ColNumber)
        {
            colNumber = ColNumber;
            pDate = Convert.ToDateTime(VAlarm.RPT_DATE_TIME);
            palarmHappend = Convert.ToDateTime(VAlarm.RPT_DATE_TIME);
            palarmClear = (VAlarm.CLEAR_DATE_TIME == null) ? DateTime.MaxValue : Convert.ToDateTime(VAlarm.CLEAR_DATE_TIME);
            pworkTime = VAlarm.AlarmTime.ToString();
            pschedule = "";
            if(VAlarm.EQPT_ID.Contains("OHB"))
            {
                pEQ_Name = "OHBC System";
                pEQ_Number = VAlarm.EQPT_ID.Substring(VAlarm.EQPT_ID.Length-2); //預設抓最後兩位
            }
            else
            {
                pEQ_Name = "Other";
                pEQ_Number = VAlarm.EQPT_ID;
            }

            pmoduleClassification = "";
            palarmClassification = VAlarm.ALARM_CLASSIFICATION;
            pimportance = ""; //預設
            palarmCode = VAlarm.ALARM_CODE;
            palarmDesc = VAlarm.ALARM_DESC;
            palarmRemark = VAlarm.ALARM_REMARK;
            plocation = VAlarm.ADDRESS_ID;
            pportID = VAlarm.PORT_ID;
            pboxNumber = VAlarm.CARRIER_ID;
        }
    }

}


