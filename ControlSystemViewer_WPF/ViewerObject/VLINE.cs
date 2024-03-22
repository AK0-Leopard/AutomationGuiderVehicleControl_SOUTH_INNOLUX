using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VLINE : ViewerObjectBase
    {
        public VLINE()
        {
        }

        private string line_id = "";
        public string LINE_ID
        {
            get { return line_id; }
            set
            {
                string newLineID = value?.Trim() ?? "";
                if (line_id != newLineID)
                {
                    line_id = newLineID;
                    OnPropertyChanged();
                }
            }
        }

        private string version = "";
        public string VERSION
        {
            get { return version; }
            set
            {
                string newVersion = value?.Trim() ?? "";
                if (version != newVersion)
                {
                    version = newVersion;
                    OnPropertyChanged();
                }
            }
        }

        public event EventHandler StatusChange;
        public void onStatusChange()
        {
            StatusChange?.Invoke(this, null);
        }

        public event EventHandler LineInfoChange;
        public void onLineInfoChange()
        {
            LineInfoChange?.Invoke(this, null);
        }

        public event EventHandler ConnectionInfoChange;
        public void onConnectionInfoChange()
        {
            ConnectionInfoChange?.Invoke(this, null);
        }

        public event EventHandler TransferInfoChange;
        public void onTransferInfoChange()
        {
            TransferInfoChange?.Invoke(this, null);
        }

        public event EventHandler OnlineCheckInfoChange;
        public void onOnlineCheckInfoChange()
        {
            OnlineCheckInfoChange?.Invoke(this, null);
        }

        public event EventHandler PingCheckInfoChange;
        public void onPingCheckInfoChange()
        {
            PingCheckInfoChange?.Invoke(this, null);
        }

        private bool iS_ALARM_HAPPEN = false;
        public bool IS_ALARM_HAPPEN
        {
            get { return iS_ALARM_HAPPEN; }
            set
            {
                if (iS_ALARM_HAPPEN != value)
                {
                    iS_ALARM_HAPPEN = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_LINK_HOST = false;
        public bool IS_LINK_HOST
        {
            get { return iS_LINK_HOST; }
            set
            {
                if (iS_LINK_HOST != value)
                {
                    iS_LINK_HOST = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_LINK_PLC = false;
        public bool IS_LINK_PLC
        {
            get { return iS_LINK_PLC; }
            set
            {
                if (iS_LINK_PLC != value)
                {
                    iS_LINK_PLC = value;
                    OnPropertyChanged();
                }
            }
        }

        private VLINE_Def.HostControlState hOST_CONTROL_STATE = VLINE_Def.HostControlState.EQ_Off_line;
        public VLINE_Def.HostControlState HOST_CONTROL_STATE
        {
            get { return hOST_CONTROL_STATE; }
            set
            {
                if (hOST_CONTROL_STATE != value)
                {
                    hOST_CONTROL_STATE = value;
                    OnPropertyChanged();
                }
            }
        }

        private VLINE_Def.TSCState tSC_STATE = VLINE_Def.TSCState.NONE;
        public VLINE_Def.TSCState TSC_STATE
        {
            get { return tSC_STATE; }
            set
            {
                if (tSC_STATE != value)
                {
                    tSC_STATE = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_MCS_CMD_AUTO_ASSIGN = false;
        public bool IS_MCS_CMD_AUTO_ASSIGN
        {
            get { return iS_MCS_CMD_AUTO_ASSIGN; }
            set
            {
                if (iS_MCS_CMD_AUTO_ASSIGN != value)
                {
                    iS_MCS_CMD_AUTO_ASSIGN = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool iS_CHECKED_CURR_PORT_STATE = false;
        public bool IS_CHECKED_CURR_PORT_STATE
        {
            get { return iS_CHECKED_CURR_PORT_STATE; }
            set
            {
                if (iS_CHECKED_CURR_PORT_STATE != value)
                {
                    iS_CHECKED_CURR_PORT_STATE = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CHECKED_CURR_STATE = false;
        public bool IS_CHECKED_CURR_STATE
        {
            get { return iS_CHECKED_CURR_STATE; }
            set
            {
                if (iS_CHECKED_CURR_STATE != value)
                {
                    iS_CHECKED_CURR_STATE = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CHECKED_TSC_STATE = false;
        public bool IS_CHECKED_TSC_STATE
        {
            get { return iS_CHECKED_TSC_STATE; }
            set
            {
                if (iS_CHECKED_TSC_STATE != value)
                {
                    iS_CHECKED_TSC_STATE = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CHECKED_ENHANCED_VEHICLES = false;
        public bool IS_CHECKED_ENHANCED_VEHICLES
        {
            get { return iS_CHECKED_ENHANCED_VEHICLES; }
            set
            {
                if (iS_CHECKED_ENHANCED_VEHICLES != value)
                {
                    iS_CHECKED_ENHANCED_VEHICLES = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CHECKED_ENHANCED_TRANSFERS = false;
        public bool IS_CHECKED_ENHANCED_TRANSFERS
        {
            get { return iS_CHECKED_ENHANCED_TRANSFERS; }
            set
            {
                if (iS_CHECKED_ENHANCED_TRANSFERS != value)
                {
                    iS_CHECKED_ENHANCED_TRANSFERS = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CHECKED_ENHANCED_CARRIERS = false;
        public bool IS_CHECKED_ENHANCED_CARRIERS
        {
            get { return iS_CHECKED_ENHANCED_CARRIERS; }
            set
            {
                if (iS_CHECKED_ENHANCED_CARRIERS != value)
                {
                    iS_CHECKED_ENHANCED_CARRIERS = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool iS_CONNECT_MCS = false;
        public bool IS_CONNECT_MCS
        {
            get { return iS_CONNECT_MCS; }
            set
            {
                if (iS_CONNECT_MCS != value)
                {
                    iS_CONNECT_MCS = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CONNECT_ROUTER = false;
        public bool IS_CONNECT_ROUTER
        {
            get { return iS_CONNECT_ROUTER; }
            set
            {
                if (iS_CONNECT_ROUTER != value)
                {
                    iS_CONNECT_ROUTER = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_CONNECT_CHARGER_PLC = false;
        public bool IS_CONNECT_CHARGER_PLC
        {
            get { return iS_CONNECT_CHARGER_PLC; }
            set
            {
                if (iS_CONNECT_CHARGER_PLC != value)
                {
                    iS_CONNECT_CHARGER_PLC = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<bool> IS_CONNECT_APs { get; private set; } = new List<bool>();
        public void InitAPsConnectStatus(int count)
        {
            IS_CONNECT_APs.Clear();
            for (int i = 0; i < count; i++)
            {
                IS_CONNECT_APs.Add(false);
            }
        }
        public void SetAPsConnectStatus(List<bool> connectStatus)
        {
            if (connectStatus == null || connectStatus.Count < IS_CONNECT_APs.Count) return;

            for (int i = 0; i < IS_CONNECT_APs.Count; i++)
            {
                IS_CONNECT_APs[i] = connectStatus[i];
            }
            OnPropertyChanged("IS_CONNECT_APs");
        }

        public List<bool> IS_CONNECT_ADAMs { get; private set; } = new List<bool>();
        public void InitADAMsConnectStatus(int count)
        {
            IS_CONNECT_ADAMs.Clear();
            for (int i = 0; i < count; i++)
            {
                IS_CONNECT_ADAMs.Add(false);
            }
        }
        public void SetADAMsConnectStatus(List<bool> connectStatus)
        {
            if (connectStatus == null || connectStatus.Count < IS_CONNECT_ADAMs.Count) return;

            for (int i = 0; i < IS_CONNECT_ADAMs.Count; i++)
            {
                IS_CONNECT_ADAMs[i] = connectStatus[i];
            }
            OnPropertyChanged("IS_CONNECT_ADAMs");
        }

        public List<bool> IS_CONNECT_VEHICLEs { get; private set; } = new List<bool>();
        public void InitVehiclesConnectStatus(int count)
        {
            IS_CONNECT_VEHICLEs.Clear();
            for (int i = 0; i < count; i++)
            {
                IS_CONNECT_VEHICLEs.Add(false);
            }
        }
        public void SetVehiclesConnectStatus(List<bool> connectStatus)
        {
            if (connectStatus == null || connectStatus.Count < IS_CONNECT_VEHICLEs.Count) return;

            for (int i = 0; i < IS_CONNECT_VEHICLEs.Count; i++)
            {
                IS_CONNECT_VEHICLEs[i] = connectStatus[i];
            }
            OnPropertyChanged("IS_CONNECT_VEHICLEs");
        }


        private int cOUNT_HOST_CMD_TRANSFER_STATE_WAITING = 0;
        public int COUNT_HOST_CMD_TRANSFER_STATE_WAITING
        {
            get { return cOUNT_HOST_CMD_TRANSFER_STATE_WAITING; }
            set
            {
                if (cOUNT_HOST_CMD_TRANSFER_STATE_WAITING != value)
                {
                    cOUNT_HOST_CMD_TRANSFER_STATE_WAITING = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED = 0;
        public int COUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED
        {
            get { return cOUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED; }
            set
            {
                if (cOUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED != value)
                {
                    cOUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_CST_STATE_WAITING = 0;
        public int COUNT_CST_STATE_WAITING
        {
            get { return cOUNT_CST_STATE_WAITING; }
            set
            {
                if (cOUNT_CST_STATE_WAITING != value)
                {
                    cOUNT_CST_STATE_WAITING = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_CST_STATE_TRANSFER = 0;
        public int COUNT_CST_STATE_TRANSFER
        {
            get { return cOUNT_CST_STATE_TRANSFER; }
            set
            {
                if (cOUNT_CST_STATE_TRANSFER != value)
                {
                    cOUNT_CST_STATE_TRANSFER = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_VEHICLE_MODE_AUTO_REMOTE = 0;
        public int COUNT_VEHICLE_MODE_AUTO_REMOTE
        {
            get { return cOUNT_VEHICLE_MODE_AUTO_REMOTE; }
            set
            {
                if (cOUNT_VEHICLE_MODE_AUTO_REMOTE != value)
                {
                    cOUNT_VEHICLE_MODE_AUTO_REMOTE = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_VEHICLE_MODE_AUTO_LOCAL = 0;
        public int COUNT_VEHICLE_MODE_AUTO_LOCAL
        {
            get { return cOUNT_VEHICLE_MODE_AUTO_LOCAL; }
            set
            {
                if (cOUNT_VEHICLE_MODE_AUTO_LOCAL != value)
                {
                    cOUNT_VEHICLE_MODE_AUTO_LOCAL = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_VEHICLE_MODE_IDLE = 0;
        public int COUNT_VEHICLE_MODE_IDLE
        {
            get { return cOUNT_VEHICLE_MODE_IDLE; }
            set
            {
                if (cOUNT_VEHICLE_MODE_IDLE != value)
                {
                    cOUNT_VEHICLE_MODE_IDLE = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_VEHICLE_MODE_ERROR = 0;
        public int COUNT_VEHICLE_MODE_ERROR
        {
            get { return cOUNT_VEHICLE_MODE_ERROR; }
            set
            {
                if (cOUNT_VEHICLE_MODE_ERROR != value)
                {
                    cOUNT_VEHICLE_MODE_ERROR = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public static class VLINE_Def
    {
        public enum HostControlState
        {
            EQ_Off_line = 0,
            Going_Online = 1,
            Host_Offline = 2,
            On_Line_Local = 3,
            On_Line_Remote = 4
        }

        public enum TSCState
        {
            NONE = 0,
            TSC_INIT = 1,
            PAUSED = 2,
            AUTO = 3,
            PAUSING = 4,
            IN_STATUS = 99
        }
    }
}
