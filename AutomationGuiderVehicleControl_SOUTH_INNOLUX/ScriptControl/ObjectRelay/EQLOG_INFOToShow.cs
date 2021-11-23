using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.ObjectRelay
{
    public class EQLOG_INFOToShow
    {
        public static App.SCApplication app = App.SCApplication.getInstance();
        public EQLOG_INFO eqLogInfo = null;
        public EQLOG_INFOToShow(EQLOG_INFO _eqLogInfo)
        {
            eqLogInfo = _eqLogInfo;
        }
        public DateTime TIME
        {
            get
            {
                DateTime.TryParse(eqLogInfo.TIME, out DateTime dateTime);
                return dateTime;
            }
        }

        public string sTIME { get { return TIME.ToString(sc.App.SCAppConstants.DateTimeFormat_22); } }
        public string FUNNAME { get { return eqLogInfo.FUNNAME?.Trim(); } }
        public int SEQNO { get { return eqLogInfo.SEQNO; } }
        public string VHID { get { return eqLogInfo.VHID?.Trim(); } }
        public string OHTCCMDID { get { return eqLogInfo.OHTCCMDID?.Trim(); } }
        public string ACTTYPE { get { return eqLogInfo.ACTTYPE?.Trim(); } }
        public string MCSCMDID { get { return eqLogInfo.MCSCMDID?.Trim(); } }
        public string EVENTTYPE { get { return eqLogInfo.EVENTTYPE?.Trim(); } }
        public string VHSTATUS { get { return eqLogInfo.VHSTATUS?.Trim(); } }
        public string MESSAGE { get { return eqLogInfo.MESSAGE?.Trim(); } }
        public string SENDRECEIVE { get { return eqLogInfo.SENDRECEIVE?.Trim(); } }


    }
}
