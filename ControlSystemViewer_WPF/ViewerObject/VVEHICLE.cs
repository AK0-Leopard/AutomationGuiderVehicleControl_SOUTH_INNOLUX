using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ViewerObject
{
    public class VVEHICLE : ViewerObjectBase
    {
        public VVEHICLE(string vhID)
        {
            VEHICLE_ID = vhID?.Trim() ?? "";
        }

        public string VEHICLE_ID { get; private set; } = "";

        public event EventHandler StatusChange;
        public void onStatusChange()
        {
            StatusChange?.Invoke(this, null);
        }

        private VVEHICLE_Def.InstallStatus iNSTALL_STATUS = VVEHICLE_Def.InstallStatus.Removed;
        public VVEHICLE_Def.InstallStatus INSTALL_STATUS
        {
            get { return iNSTALL_STATUS; }
            set
            {
                if (iNSTALL_STATUS != value)
                {
                    iNSTALL_STATUS = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Connection
        private bool iS_TCPIP_CONNECT = false;
        public bool IS_TCPIP_CONNECT
        {
            get { return iS_TCPIP_CONNECT; }
            set
            {
                if (iS_TCPIP_CONNECT != value)
                {
                    iS_TCPIP_CONNECT = value;
                    OnPropertyChanged();
                    onConnectionStatusChange();
                }
            }
        }
        public event EventHandler<bool> ConnectionStatusChange;
        public void onConnectionStatusChange()
        {
            ConnectionStatusChange?.Invoke(this, IS_TCPIP_CONNECT);
        }
        #endregion Connection

        #region Mode
        private VVEHICLE_Def.ModeStatus modeStatus = VVEHICLE_Def.ModeStatus.None;
        public VVEHICLE_Def.ModeStatus MODE_STATUS
        {
            get => modeStatus;
            set
            {
                if (modeStatus != value)
                {
                    modeStatus = value;
                    OnPropertyChanged();
                    onModeStatusChange();
                }
            }
        }
        public event EventHandler<VVEHICLE_Def.ModeStatus> ModeStatusChange;
        public void onModeStatusChange()
        {
            ModeStatusChange?.Invoke(this, MODE_STATUS);
        }
        public string STRING_MODE_STATUS
        {
            get
            {
                switch (MODE_STATUS)
                {
                    case VVEHICLE_Def.ModeStatus.AutoMtl:
                        return "Auto MTL";
                    case VVEHICLE_Def.ModeStatus.InitialPowerOff:
                        return "Power Off";
                    case VVEHICLE_Def.ModeStatus.InitialPowerOn:
                        return "Power On";
                    //case VVEHICLE_Def.ModeStatus.AutoCharging:
                    //    return "Auto Charging";
                    case VVEHICLE_Def.ModeStatus.AutoLocal:
                        return "Auto Local";
                    case VVEHICLE_Def.ModeStatus.AutoRemote:
                        return "Auto Remote";
                    case VVEHICLE_Def.ModeStatus.Manual:
                        return "Manual";
                    default:
                        return "None";
                }
            }
        }
        #endregion Mode

        #region Action
        private VVEHICLE_Def.ActionStatus actionStatus = VVEHICLE_Def.ActionStatus.NoCommand;
        public VVEHICLE_Def.ActionStatus ACT_STATUS
        {
            get => actionStatus;
            set
            {
                if (actionStatus != value)
                {
                    actionStatus = value;
                    OnPropertyChanged();
                    onActionChange();
                }
            }
        }
        private bool iS_LOADING = false;
        public bool IS_LOADING
        {
            get { return iS_LOADING; }
            set
            {
                if (iS_LOADING != value)
                {
                    iS_LOADING = value;
                    OnPropertyChanged();
                    onActionChange();
                }
            }
        }
        private bool iS_UNLOADING = false;
        public bool IS_UNLOADING
        {
            get { return iS_UNLOADING; }
            set
            {
                if (iS_UNLOADING != value)
                {
                    iS_UNLOADING = value;
                    OnPropertyChanged();
                    onActionChange();
                }
            }
        }
        public bool IS_PARKED { get; private set; } = false;
        public string PARK_ADR_ID { get; private set; } = "";
        public void SetParkStatus(bool isParked, string parkAdrID)
        {
            string _parkAdrID = parkAdrID?.Trim() ?? "";
            if (IS_PARKED == isParked &&
                PARK_ADR_ID == _parkAdrID) return;
            IS_PARKED = isParked;
            PARK_ADR_ID = _parkAdrID;
            OnPropertyChanged("IS_PARKED");
            OnPropertyChanged("PARK_ADR_ID");
            onActionChange();
        }
        public event EventHandler ActionChange;
        public void onActionChange()
        {
            ActionChange?.Invoke(this, null);
        }
        #endregion Action

        #region Error
        private bool hAS_ERROR = false;
        public bool HAS_ERROR
        {
            get { return hAS_ERROR; }
            set
            {
                if (hAS_ERROR != value)
                {
                    hAS_ERROR = value;
                    OnPropertyChanged();
                    onErrorStatusChange();
                }
            }
        }
        public event EventHandler<bool> ErrorStatusChange;
        public void onErrorStatusChange()
        {
            ErrorStatusChange?.Invoke(this, HAS_ERROR);
        }
        #endregion Error

        #region Pause
        private bool iS_PAUSE = false;
        public bool IS_PAUSE
        {
            get { return iS_PAUSE; }
            set
            {
                if (iS_PAUSE != value)
                {
                    iS_PAUSE = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        private bool iS_PAUSE_BLOCK = false;
        public bool IS_PAUSE_BLOCK
        {
            get { return iS_PAUSE_BLOCK; }
            set
            {
                if (iS_PAUSE_BLOCK != value)
                {
                    iS_PAUSE_BLOCK = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        private bool iS_PAUSE_OBS = false;
        public bool IS_PAUSE_OBS
        {
            get { return iS_PAUSE_OBS; }
            set
            {
                if (iS_PAUSE_OBS != value)
                {
                    iS_PAUSE_OBS = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        private bool iS_PAUSE_HID = false;
        public bool IS_PAUSE_HID
        {
            get { return iS_PAUSE_HID; }
            set
            {
                if (iS_PAUSE_HID != value)
                {
                    iS_PAUSE_HID = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        private bool iS_PAUSE_EARTHQUAKE = false;
        public bool IS_PAUSE_EARTHQUAKE
        {
            get { return iS_PAUSE_EARTHQUAKE; }
            set
            {
                if (iS_PAUSE_EARTHQUAKE != value)
                {
                    iS_PAUSE_EARTHQUAKE = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        private bool iS_PAUSE_RESERVE = false;
        public bool IS_PAUSE_RESERVE
        {
            get { return iS_PAUSE_RESERVE; }
            set
            {
                if (iS_PAUSE_RESERVE != value)
                {
                    iS_PAUSE_RESERVE = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        private bool iS_PAUSE_SAFETY_DOOR = false;
        public bool IS_PAUSE_SAFETY_DOOR
        {
            get { return iS_PAUSE_SAFETY_DOOR; }
            set
            {
                if (iS_PAUSE_SAFETY_DOOR != value)
                {
                    iS_PAUSE_SAFETY_DOOR = value;
                    OnPropertyChanged();
                    onPauseStatusChange();
                }
            }
        }
        public event EventHandler PauseStatusChange;
        public void onPauseStatusChange()
        {
            PauseStatusChange?.Invoke(this, null);
        }
        #endregion Pause

        #region Location & Position
        public string CUR_ADR_ID { get; private set; } = "";
        public string CUR_SEC_ID { get; private set; } = "";
        public double ACC_SEC_DIST { get; private set; } = 0;
        public void SetLocation(string adr, string sec, double dist)
        {
            string _adr = adr?.Trim() ?? "";
            string _sec = sec?.Trim() ?? "";
            if (CUR_ADR_ID == _adr &&
                CUR_SEC_ID == _sec &&
                ACC_SEC_DIST == dist) return;
            CUR_ADR_ID = _adr;
            CUR_SEC_ID = _sec;
            ACC_SEC_DIST = dist;
            OnPropertyChanged("CUR_ADR_ID");
            OnPropertyChanged("CUR_SEC_ID");
            OnPropertyChanged("ACC_SEC_DIST");
            onLocationChange();
        }
        public event EventHandler LocationChange;
        public void onLocationChange()
        {
            LocationChange?.Invoke(this, null);
        }

        public double X_AXIS { get; private set; } = 0;
        public double Y_AXIS { get; private set; } = 0;
        public void SetPosition(double x, double y)
        {
            if (X_AXIS == x &&
                Y_AXIS == y) return;
            X_AXIS = x;
            Y_AXIS = y;
            OnPropertyChanged("X_AXIS");
            OnPropertyChanged("Y_AXIS");
            onPositionChange();
        }
        public event EventHandler<VVEHICLE_Def.PositionChangeEventArgs> PositionChange;
        public void onPositionChange()
        {
            PositionChange?.Invoke(this, new VVEHICLE_Def.PositionChangeEventArgs(X_AXIS, Y_AXIS));
        }
        #endregion Location & Position

        #region Transfer & Command
        private string tRANSFER_ID_1 = "";
        public string TRANSFER_ID_1
        {
            get => tRANSFER_ID_1;
            set
            {
                string _value = value?.Trim() ?? "";
                if (tRANSFER_ID_1 != _value)
                {
                    tRANSFER_ID_1 = _value;
                    OnPropertyChanged();
                }
            }
        }
        private string tRANSFER_ID_2 = "";
        public string TRANSFER_ID_2
        {
            get => tRANSFER_ID_2;
            set
            {
                string _value = value?.Trim() ?? "";
                if (tRANSFER_ID_2 != _value)
                {
                    tRANSFER_ID_2 = _value;
                    OnPropertyChanged();
                }
            }
        }
        private string tRANSFER_ID_3 = "";
        public string TRANSFER_ID_3
        {
            get => tRANSFER_ID_3;
            set
            {
                string _value = value?.Trim() ?? "";
                if (tRANSFER_ID_3 != _value)
                {
                    tRANSFER_ID_3 = _value;
                    OnPropertyChanged();
                }
            }
        }
        private string tRANSFER_ID_4 = "";
        public string TRANSFER_ID_4
        {
            get => tRANSFER_ID_4;
            set
            {
                string _value = value?.Trim() ?? "";
                if (tRANSFER_ID_4 != _value)
                {
                    tRANSFER_ID_4 = _value;
                    OnPropertyChanged();
                }
            }
        }

        private string cMD_ID_1 = "";
        public string CMD_ID_1
        {
            get => cMD_ID_1;
            set
            {
                string _value = value?.Trim() ?? "";
                if (cMD_ID_1 != _value)
                {
                    cMD_ID_1 = _value;
                    OnPropertyChanged();
                }
            }
        }
        private string cMD_ID_2 = "";
        public string CMD_ID_2
        {
            get => cMD_ID_2;
            set
            {
                string _value = value?.Trim() ?? "";
                if (cMD_ID_2 != _value)
                {
                    cMD_ID_2 = _value;
                    OnPropertyChanged();
                }
            }
        }
        private string cMD_ID_3 = "";
        public string CMD_ID_3
        {
            get => cMD_ID_3;
            set
            {
                string _value = value?.Trim() ?? "";
                if (cMD_ID_3 != _value)
                {
                    cMD_ID_3 = _value;
                    OnPropertyChanged();
                }
            }
        }
        private string cMD_ID_4 = "";
        public string CMD_ID_4
        {
            get => cMD_ID_4;
            set
            {
                string _value = value?.Trim() ?? "";
                if (cMD_ID_4 != _value)
                {
                    cMD_ID_4 = _value;
                    OnPropertyChanged();
                }
            }
        }

        private string cURRENT_EXCUTE_CMD_ID = "";
        public string CURRENT_EXCUTE_CMD_ID
        {
            get => cURRENT_EXCUTE_CMD_ID;
            set
            {
                string _value = value?.Trim() ?? "";
                if (cURRENT_EXCUTE_CMD_ID != _value)
                {
                    cURRENT_EXCUTE_CMD_ID = _value;
                    OnPropertyChanged();
                }
            }
        }

        private VCMD_Def.CmdType cMD_TYPE = 0;
        public VCMD_Def.CmdType CMD_TYPE
        {
            get => cMD_TYPE;
            set
            {
                if (cMD_TYPE != value)
                {
                    cMD_TYPE = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion Transfer & Command

        #region CST
        public bool HAS_CST_L { get; private set; } = false;
        public bool HAS_CST_R { get; private set; } = false;
        public string CST_ID_L { get; private set; } = "";
        public string CST_ID_R { get; private set; } = "";
        public void SetCST(bool hasCST, string cstID)
        {
            string _cstID = cstID?.Trim() ?? "";
            if (HAS_CST_L == hasCST &&
                CST_ID_L == _cstID) return;
            HAS_CST_L = hasCST;
            CST_ID_L = _cstID;
            OnPropertyChanged("HAS_CST_L");
            OnPropertyChanged("CST_ID_L");
            onCassetteStatusChange();
        }
        public void SetCST(bool hasCST_L, string cstID_L, bool hasCST_R, string cstID_R)
        {
            string _cstID_L = cstID_L?.Trim() ?? "";
            string _cstID_R = cstID_R?.Trim() ?? "";
            if (HAS_CST_L == hasCST_L &&
                CST_ID_L == _cstID_L &&
                HAS_CST_R == hasCST_R &&
                CST_ID_R == _cstID_R) return;
            HAS_CST_L = hasCST_L;
            CST_ID_L = _cstID_L;
            HAS_CST_R = hasCST_R;
            CST_ID_R = _cstID_R;
            OnPropertyChanged("HAS_CST_L");
            OnPropertyChanged("CST_ID_L");
            OnPropertyChanged("HAS_CST_R");
            OnPropertyChanged("CST_ID_R");
            onCassetteStatusChange();
        }
        public event EventHandler CassetteStatusChange;
        public void onCassetteStatusChange()
        {
            CassetteStatusChange?.Invoke(this, null);
        }
        #endregion CST

        #region Path
        public string PATH_START_ADR { get; private set; } = "";
        public string PATH_FROM_ADR { get; private set; } = "";
        public string PATH_TO_ADR { get; private set; } = "";
        public List<string> PATH_PREDICT_SECTIONS { get; private set; } = new List<string>();
        public List<string> PATH_WILLPASS_SECTIONS { get; private set; } = new List<string>();
        public List<string> PATH_RESERVED_SECTIONS { get; private set; } = new List<string>();
        public void SetPath(string startAdr, string fromAdr, string toAdr, List<string> predictSections, List<string> willPassSections, List<string> reservedSections)
        {
            string _startAdr = startAdr?.Trim() ?? "";
            string _fromAdr = fromAdr?.Trim() ?? "";
            string _toAdr = toAdr?.Trim() ?? "";
            List<string> _predictSections = predictSections ?? new List<string>();
            List<string> _willPassSections = willPassSections ?? new List<string>();
            List<string> _reservedSections = reservedSections ?? new List<string>();
            if (BasicFunction.Compare.Same(PATH_RESERVED_SECTIONS, _reservedSections) &&
                BasicFunction.Compare.Same(PATH_WILLPASS_SECTIONS, _willPassSections) &&
                BasicFunction.Compare.Same(PATH_PREDICT_SECTIONS, _predictSections) &&
                PATH_START_ADR == _startAdr &&
                PATH_FROM_ADR == _fromAdr &&
                PATH_TO_ADR == _toAdr) return;
            PATH_START_ADR = _startAdr;
            PATH_FROM_ADR = _fromAdr;
            PATH_TO_ADR = _toAdr;
            PATH_PREDICT_SECTIONS = _predictSections;
            PATH_WILLPASS_SECTIONS = _willPassSections;
            PATH_RESERVED_SECTIONS = _reservedSections;
            OnPropertyChanged("PATH_START_ADR");
            OnPropertyChanged("PATH_FROM_ADR");
            OnPropertyChanged("PATH_TO_ADR");
            OnPropertyChanged("PATH_PREDICT_SECTIONS");
            OnPropertyChanged("PATH_WILLPASS_SECTIONS");
            OnPropertyChanged("PATH_RESERVED_SECTIONS");
            onPathChange();
        }
        public event EventHandler PathChange;
        public void onPathChange()
        {
            PathChange?.Invoke(this, null);
        }
        #endregion Path

        #region Speed
        private double speed;
        public double SPEED
        {
            get { return speed; }
            set
            {
                if (speed != value)
                {
                    speed = value;
                    OnPropertyChanged();
                    onSpeedChange();
                }
            }
        }
        public event EventHandler SpeedChange;
        public void onSpeedChange()
        {
            SpeedChange?.Invoke(this, null);
        }
        #endregion Speed

        #region Battery

        private int batteryCapacity = 0;
        public int BATTERY_CAPACITY
        {
            get => batteryCapacity;
            set
            {
                if (batteryCapacity != value)
                {
                    batteryCapacity = value;
                    OnPropertyChanged();
                    onBatteryCapacityChange();
                }
            }
        }
        public event EventHandler<int> BatteryCapacityChange;
        public void onBatteryCapacityChange()
        {
            BatteryCapacityChange?.Invoke(this, BATTERY_CAPACITY);
        }

        private VVEHICLE_Def.BatteryLevel batteryLevel = VVEHICLE_Def.BatteryLevel.None;
        public VVEHICLE_Def.BatteryLevel BATTERY_LEVEL
        {
            get => batteryLevel;
            set
            {
                if (batteryLevel != value)
                {
                    batteryLevel = value;
                    OnPropertyChanged();
                    onBatteryLevelChange();
                }
            }
        }
        public event EventHandler<VVEHICLE_Def.BatteryLevel> BatteryLevelChange;
        public void onBatteryLevelChange()
        {
            BatteryLevelChange?.Invoke(this, BATTERY_LEVEL);
        }
        private bool iS_CHARGING= false;
        public bool IS_CHARGING
        {
            get { return iS_CHARGING; }
            set
            {
                if (iS_CHARGING != value)
                {
                    iS_CHARGING = value;
                    OnPropertyChanged();
                    onActionChange();
                }
            }
        }
        public event EventHandler ChargeStatusChange;
        public void onChargeStatusChange()
        {
            ChargeStatusChange?.Invoke(this, null);
        }

        #endregion Battery
    }

    public static class VVEHICLE_Addition
    {
        private static readonly string ns = "ViewerObject" + ".VVEHICLE_Addition";

        public static bool SetVhAxis(ref VVEHICLE vh, List<ViewerObject.Address> addresses, List<ViewerObject.Section> sections, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            if (vh == null)
            {
                result = $"{ns}.{ms} - vh = null";
                return false;
            }
            if (addresses == null || addresses.Count == 0)
            {
                result = $"{ns}.{ms} - vh = null";
                return false;
            }
            if (sections == null || sections.Count == 0)
            {
                result = $"{ns}.{ms} - vh = null";
                return false;
            }
            string _doing = "";
            try
            {
                _doing = "GetVhAxis";
                (double x, double y, float angle) vh_axis = GetVhAxis(vh.CUR_ADR_ID, vh.CUR_SEC_ID, vh.ACC_SEC_DIST, addresses, sections);
                _doing = "SetPosition";
                vh.SetPosition(vh_axis.x, vh_axis.y);
                return true;
            }
            catch //(Exception ex)
            {
                result = $"{ns}.{ms} - Exception occurred while {_doing}";
                return false;
            }
        }
        public static (double x, double y, float angle) GetVhAxis(string currAddressID, string currSectionID, double currentDis, List<Address> addresses, List<Section> sections)
        {
            double sectionTotalDis = 0;
            ViewerObject.Address fromAddr;
            ViewerObject.Address toAddr;

            string currSecId = currSectionID?.Trim() ?? "";
            ViewerObject.Section section = sections.FirstOrDefault(s => s.ID == currSecId);
            string currAddrId = currAddressID?.Trim() ?? "";
            if (currAddrId == section.StartAddress.ID.Trim())
            {
                fromAddr = section.StartAddress;
                toAddr = section.EndAddress;
            }
            else if (currAddrId == section.EndAddress.ID.Trim())
            {
                fromAddr = section.EndAddress;
                toAddr = section.StartAddress;
            }
            else return (0, 0, 0);

            sectionTotalDis = Math.Sqrt(Math.Pow(fromAddr.X - toAddr.X, 2) + Math.Pow(fromAddr.Y - toAddr.Y, 2));
            return GetVhAxis(sectionTotalDis, currentDis, fromAddr.X, fromAddr.Y, toAddr.X, toAddr.Y);
        }
        public static (double x, double y, float angle) GetVhAxis(double sectionTotalDis, double currentDis, double from_x, double from_y, double to_x, double to_y)
        {
            double x = 0;
            double y = 0;
            float angle = 0;
            if (from_x == to_x)
            {
                x = from_x;
                if (from_y > to_y)
                    y = from_y - currentDis;
                else
                    y = from_y + currentDis;
                angle = 90;
            }
            else if (from_y == to_y)
            {
                if (from_x > to_x)
                    x = from_x - currentDis;
                else
                    x = from_x + currentDis;
                y = from_y;
                angle = 0;

            }
            else
            {
                if (currentDis > (sectionTotalDis / 2))
                {
                    x = to_x;
                    y = to_y;
                }
                else
                {
                    x = from_x;
                    y = from_y;
                }
            }
            return (x, y, angle);
        }
    }

    public static class VVEHICLE_Def
    {
        public enum InstallStatus
        {
            Removed = 0,
            Installed
        }

        public enum ModeStatus
        {
            None = 0,
            InitialPowerOff = 1,
            InitialPowerOn = 2,
            Manual = 3,
            AutoRemote = 4,
            AutoLocal = 5,
            AutoMts = 6,
            AutoMtl = 7,
            AutoZoneChange,
            AutoCharging
        }

        public enum ActionStatus
        {
            NoCommand = 0,
            Commanding = 1,
            Teaching = 5,
            GripperTeaching = 6,
            CycleRun = 7
        }

        public enum VPauseType
        {
            None = 0,
            OhxC = 1,
            Block = 2,
            Hid,
            EarthQuake = 4,
            Safety = 5,
            Reserve = 6,
            Normal,
            ManualBlock,
            ManualHid,
            All
        }

        public enum VPauseEvent
        {
            Continue = 0,
            Pause = 1
        }

        public enum BatteryLevel
        {
            None = 0,
            Low = 1,
            Middle = 2,
            High = 3,
            Full = 4
        }
        public static int BATTERYLEVELVALUE_FULL { get; private set; } = 99;
        public static int BATTERYLEVELVALUE_HIGH { get; private set; } = 80;
        public static int BATTERYLEVELVALUE_MIDDLE { get; private set; } = 50;
        public static int BATTERYLEVELVALUE_LOW { get; private set; } = 25;
        public static bool SetBATTERYLEVELVALUE(int full, int high, int middle, int low, out string result)
        {
            result = "";
            if (full > high || high > middle || middle > low)
            {
                result = "Values MUST be descending order.";
                return false;
            }
            if (low <= 0)
            {
                result = "Low value MUST > 0.";
                return false;
            }
            BATTERYLEVELVALUE_FULL = full;
            BATTERYLEVELVALUE_HIGH = high;
            BATTERYLEVELVALUE_MIDDLE = middle;
            BATTERYLEVELVALUE_LOW = low;
            return true;
        }
        public static BatteryLevel GetBatteryLevel(int batteryCapacity)
        {
            return batteryCapacity >= BATTERYLEVELVALUE_FULL ? BatteryLevel.Full :
                   batteryCapacity > BATTERYLEVELVALUE_HIGH ? BatteryLevel.High :
                   batteryCapacity >= BATTERYLEVELVALUE_LOW ? BatteryLevel.Middle :
                   batteryCapacity > 0 ? BatteryLevel.Low : BatteryLevel.None;
        }
        public static InstallStatus GetIntallStatus(bool isInstall)
        {
            return isInstall ? InstallStatus.Installed : InstallStatus.Removed;
        }

        public class PositionChangeEventArgs : EventArgs
        {
            public double Last_X_Axis = 0;
            public double Last_Y_Axis = 0;
            public double Current_X_Axis = 0;
            public double Current_Y_Axis = 0;
            public PositionChangeEventArgs(double last_X_Axis, double last_Y_Axis, double current_X_Axis, double current_Y_Axis)
            {
                Last_X_Axis = last_X_Axis;
                Last_Y_Axis = last_Y_Axis;
                Current_X_Axis = current_X_Axis;
                Current_Y_Axis = current_Y_Axis;
            }
            public PositionChangeEventArgs(double current_X_Axis, double current_Y_Axis)
            {
                Current_X_Axis = current_X_Axis;
                Current_Y_Axis = current_Y_Axis;
            }
        }
    }
}
