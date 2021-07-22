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
    public class HCMD_MCSObjToShow
    {
        public static App.SCApplication app = App.SCApplication.getInstance();
        public HCMD_MCS cmd_mcs = null;
        public HCMD_MCSObjToShow()
        {
        }
        public string CMD_ID { get { return cmd_mcs.CMD_ID?.Trim(); } }
        public string CARRIER_ID { get { return cmd_mcs.CARRIER_ID?.Trim(); } }
        public E_TRAN_STATUS TRANSFERSTATE { get { return cmd_mcs.TRANSFERSTATE; } }
        public string HOSTSOURCE
        {
            get
            {
                var portstation = app.PortStationBLL.OperateCatch.getPortStation(cmd_mcs.HOSTSOURCE);
                return portstation == null ? cmd_mcs.HOSTSOURCE?.Trim() : portstation.ToString()?.Trim();
            }
        }
        public string HOSTDESTINATION
        {
            get
            {
                var portstation = app.PortStationBLL.OperateCatch.getPortStation(cmd_mcs.HOSTDESTINATION);
                return portstation == null ? cmd_mcs.HOSTDESTINATION?.Trim() : portstation.ToString()?.Trim();
            }
        }

        //public int PRIORITY { get { return cmd_mcs.PRIORITY; } }
        public int PRIORITY
        {
            get
            {
                int priority = cmd_mcs.PRIORITY_SUM > 99 ? 99 : cmd_mcs.PRIORITY_SUM;
                return priority;
            }
        }
        //public System.DateTime CMD_INSER_TIME { get { return cmd_mcs.CMD_INSER_TIME; } }
        //public Nullable<System.DateTime> CMD_START_TIME { get { return cmd_mcs.CMD_START_TIME; } }
        //public Nullable<System.DateTime> CMD_FINISH_TIME { get { return cmd_mcs.CMD_FINISH_TIME; } }
        public string CMD_INSER_TIME { get { return cmd_mcs.CMD_INSER_TIME.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public string CMD_START_TIME { get { return cmd_mcs.CMD_START_TIME?.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public string CMD_FINISH_TIME { get { return cmd_mcs.CMD_FINISH_TIME?.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public int REPLACE { get { return cmd_mcs.REPLACE; } }


    }
}
