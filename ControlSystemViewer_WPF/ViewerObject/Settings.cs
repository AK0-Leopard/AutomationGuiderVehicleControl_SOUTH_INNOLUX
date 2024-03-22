using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class Settings
    {
        #region 連線設定
        public class DB
        {
            public string ConnectionString = "";
        }
        public DB db { get; private set; } = new DB();

        public class WebClientManager
        {
            public string ControlSystemPort = "";
        }
        public WebClientManager webClientManager { get; private set; } = new WebClientManager();

        public class InfluxDBSetting
        {
            public string ConnectString = "http://localhost:8086";//預設為local default port
            public string Token_VehicleInfo = "";//VehicleInfo資訊存的Bukket API Token
            public string Token_VehicleStatistics = "";//task 計算VehicleInfo資訊後 AlarmCount存的Bukket API Token
        }
        public InfluxDBSetting InfluxDBsetting { get; private set; } = new InfluxDBSetting();

        public class Nats
        {
            public string Port = "4222"; //Nats Service Port
            public string ChannelBay = "";//如果有一套C對多套Bay，為了區別Nats頻道，增加此參數
        }
        public Nats nats { get; private set; } = new Nats();
        public class VehicleStatusInfoPath
        {
            public string Path = "C:\\LogFiles\\AGVC\\";
            public string ArchivePath = "C:\\LogFiles\\AGVC\\Archive\\";
        }
        public VehicleStatusInfoPath vehicleStatusInfoPath { get; private set; } = new VehicleStatusInfoPath();

        #endregion

        #region UI設定
        public class MainView
        {
            public string CustomerLogo = "";
            public string SystemModeControl = "System";
        }
        public MainView mainView { get; private set; } = new MainView();

        public class AOVPopupWindow
        {
            public int Style = 0;
        }
        public AOVPopupWindow aovPopupWindow { get; private set; } = new AOVPopupWindow();

        public class MainLayout
        {
            public bool ShowSignalLight = false;
            public class SideArea
            {
                public int DefaultWidth = 300;
                public bool ShowPortStatus = false;
            }
            public SideArea sideArea = new SideArea();
            public class CurrentData
            {
                public int DefaultHeight = 216;
            }
            public CurrentData currentData = new CurrentData();
        }
        public MainLayout mainLayout { get; private set; } = new MainLayout();

        public class MapBase
        {
            public bool IsTypeVertical = false;
            public bool ShowScaleRuler = false;
            public string ScaleRulerIcon = "";
            public int AngleOfView = 0;
        }
        public MapBase mapBase { get; private set; } = new MapBase();

        public class Map
        {
            public int RailWidth = 0;
            public int AddressWidth = 0;
            public int VehicleWidth = 0;
            public int SurroundingWhiteSpaceWidth = 0;
            public int RulerTickFrequency = 0;
            public bool ShowShelf = false;
            public int ShelfWidth = 0;
            public int ShelfOffset = 0;
            public bool ShowTrackSwitch = false;
            public int TrackSwitchWidth = 0;
        }
        public Map map { get; private set; } = new Map();

        public class MenuItem_System
        {
            public bool Visible_SystemModeControl = true;
        }
        public MenuItem_System menuItem_System { get; private set; } = new MenuItem_System();

        public class MenuItem_Operation
        {
            public bool Visible_SystemModeControl = false;
            public bool Visible_PortManagement = false;
        }
        public MenuItem_Operation menuItem_Operation { get; private set; } = new MenuItem_Operation();

        public class MenuItem_Log
        {
            public bool Visible_STATISTICS = false;
        }
        public MenuItem_Log menuItem_Log { get; private set; } = new MenuItem_Log();

        public class MenuItem_Report
        {
            public bool Display = true;//決定是否開啟報表功能
        }
        public MenuItem_Report menuItem_Report { get; private set; } = new MenuItem_Report();
        public class MenuItem_Maintenance
        {
            public bool Visible_VehicleManagement = true;
            public bool Visible_EquipmentConstant = false;
            public bool Visible_ParkZoneManagement = false;
        }
        public MenuItem_Maintenance menuItem_Maintenance { get; private set; } = new MenuItem_Maintenance();

        #endregion

        #region 系統設定
        public enum SystemType
        {
            Unknown = -1,
            AGVC = 0,
            OHTC = 1,
            OHBC = 2
        }
        public class System
        {
            public SystemType SystemType { get { return systemtype; } }//
            private SystemType systemtype = SystemType.Unknown;
            public int DayShiftHour = 8;//預設早上八點
            public bool OfflineMode = false;//強制進入不與C連線的離線模式
            public bool HasCarrierType = false;//設定系統是否有Carrier_Type，有的話會啟動相關功能

            public void GetSystemType(string strSystemType)
            {
                try
                {
                    //因為Switch在這版C#還不支援變數形式，所以這邊用if
                    if (strSystemType == SystemType.AGVC.ToString()) { systemtype = SystemType.AGVC; }
                    else if (strSystemType == SystemType.OHTC.ToString()) { systemtype = SystemType.OHTC; }
                    else if (strSystemType == SystemType.OHBC.ToString()) { systemtype = SystemType.OHBC; }
                }
                catch (Exception ex)
                {

                }
            }
        }
        public System system { get; private set; } = new System();
        #endregion

        #region 報表設定
        public class Report
        {
            public int AlarmGroupMin = 30;//預設Alarm為30秒鐘Group
            public eChkAlarm_CodeMode CheckAlarmCode_Mode = eChkAlarm_CodeMode.Small_Than_10000;//預設Alarm_Code 10000以下
            public bool CheckAlarmCode(string AlarmCode)
            {
                bool rt = true;
                switch (CheckAlarmCode_Mode)
                {
                    case eChkAlarm_CodeMode.Small_Than_10000:
                        if (Convert.ToInt32(AlarmCode.Trim()) >= 10000) rt = false;
                        break;
                    case eChkAlarm_CodeMode.Outer_10000_And_100000:
                        if (Convert.ToInt32(AlarmCode.Trim()) <= 100000 && Convert.ToInt32(AlarmCode.Trim()) >= 10000) rt = false;
                        break;
                    case eChkAlarm_CodeMode.Outer_100000:
                        if (Convert.ToInt32(AlarmCode.Trim()) <= 100000) rt = false;
                        break;
                    case eChkAlarm_CodeMode.Outer_100000_And_Small_Than_100:
                        if (Convert.ToInt32(AlarmCode.Trim()) >= 100 && Convert.ToInt32(AlarmCode.Trim()) < 100000) rt = false;
                        break;

                    default:
                        break;
                }

                return rt;

            }
            public int MeanTimeOverMin = 5;//預設MeanTime Alarm只計算大於5分鐘的Alarm
            public int ExportRowLimit = 10000;//預設匯出為10000行

        }
        public Report report { get; private set; } = new Report();
        public enum eChkAlarm_CodeMode
        {
            Small_Than_10000 = 0,     //K24
            Outer_10000_And_100000 = 1, //南、北群 友達600、800
            Outer_100000 = 2,
            Outer_100000_And_Small_Than_100 = 3 //UMTC		
        }
        #endregion


        public Settings(SettingFormat format)
        {
            foreach (var sitem in format.SettingItems)
            {
                switch (sitem.Category)
                {
                    #region 連線設定
                    case "DB":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "ConnectionString":
                                    db.ConnectionString = item.Value?.Trim() ?? "";
                                    break;
                            }
                        }
                        break;
                    case "WebClientManager":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "ControlSystemPort":
                                    webClientManager.ControlSystemPort = item.Value?.Trim() ?? "";
                                    break;
                            }
                        }
                        break;
                    case "InfluxDBSetting":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case nameof(InfluxDBsetting.ConnectString):
                                    InfluxDBsetting.ConnectString = item.Value?.Trim() ?? "";
                                    break;
                                case nameof(InfluxDBsetting.Token_VehicleInfo):
                                    InfluxDBsetting.Token_VehicleInfo = item.Value?.Trim() ?? "";
                                    break;
                                case nameof(InfluxDBsetting.Token_VehicleStatistics):
                                    InfluxDBsetting.Token_VehicleStatistics = item.Value?.Trim() ?? "";
                                    break;
                            }
                        }
                        break;
                    case "Nats":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case nameof(Nats.Port):
                                    nats.Port = item.Value?.Trim() ?? "";
                                    break;
                                case nameof(Nats.ChannelBay):
                                    nats.ChannelBay = item.Value?.Trim() ?? "";
                                    break;
                            }
                        }
                        break;
                    case "VehicleStatusInfoPath":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case nameof(vehicleStatusInfoPath.Path):
                                    vehicleStatusInfoPath.Path = item.Value?.Trim() ?? "";
                                    break;
                                case nameof(vehicleStatusInfoPath.ArchivePath):
                                    vehicleStatusInfoPath.ArchivePath = item.Value?.Trim() ?? "";
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region UI設定
                    case "MainView":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "CustomerLogo":
                                    mainView.CustomerLogo = item.Value?.Trim() ?? "";
                                    break;
                            }
                        }
                        break;
                    case "AOVPopupWindow":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "Style":
                                    aovPopupWindow.Style = Convert.ToInt32(item.Value?.Trim());
                                    break;
                            }
                        }
                        break;
                    case "MainLayout":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "ShowSignalLight":
                                    string sShowSignalLight = item.Value?.Trim().ToUpper() ?? "";
                                    mainLayout.ShowSignalLight = sShowSignalLight.Contains("Y");
                                    break;
                                case "SideArea_ShowPortStatus":
                                    string sShowPortStatus = item.Value?.Trim().ToUpper() ?? "";
                                    mainLayout.sideArea.ShowPortStatus = sShowPortStatus.Contains("Y");
                                    break;
                                case "SideArea_DefaultWidth":
                                    int iDefaultWidth = Convert.ToInt32(item.Value?.Trim());
                                    iDefaultWidth = iDefaultWidth >= 0 ? iDefaultWidth : 0;
                                    mainLayout.sideArea.DefaultWidth = iDefaultWidth;
                                    break;
                                case "CurrentData_DefaultHeight":
                                    int iDefaultHeight = Convert.ToInt32(item.Value?.Trim());
                                    iDefaultHeight = iDefaultHeight >= 0 ? iDefaultHeight : 0;
                                    mainLayout.currentData.DefaultHeight = iDefaultHeight;
                                    break;
                            }
                        }
                        break;
                    case "MapBase":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "Type":
                                    string type = item.Value?.Trim().ToUpper() ?? "";
                                    mapBase.IsTypeVertical = type.Contains("V");
                                    break;
                                case "ShowScaleRuler":
                                    string showScaleRuler = item.Value?.Trim().ToUpper() ?? "";
                                    mapBase.ShowScaleRuler = showScaleRuler.Contains("Y");
                                    break;
                                case "ScaleRulerIcon":
                                    mapBase.ScaleRulerIcon = item.Value?.Trim() ?? "";
                                    break;
                                case "AngleOfView":
                                    int aov = Convert.ToInt32(item.Value?.Trim());
                                    mapBase.AngleOfView = aov == 270 ? 270 :
                                                          aov == 180 ? 180 :
                                                          aov == 90 ? 90 :
                                                          0;
                                    break;
                            }
                        }
                        break;
                    case "Map":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "RailWidth":
                                    map.RailWidth = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "AddressWidth":
                                    map.AddressWidth = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "VehicleWidth":
                                    map.VehicleWidth = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "SurroundingWhiteSpaceWidth":
                                    map.SurroundingWhiteSpaceWidth = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "RulerTickFrequency":
                                    map.RulerTickFrequency = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "ShowShelf":
                                    string showShelf = item.Value?.Trim().ToUpper() ?? "";
                                    map.ShowShelf = showShelf.Contains("Y");
                                    break;
                                case "ShelfWidth":
                                    map.ShelfWidth = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "ShelfOffset":
                                    map.ShelfOffset = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case "ShowTrackSwitch":
                                    string showTrackSwitch = item.Value?.Trim().ToUpper() ?? "";
                                    map.ShowTrackSwitch = showTrackSwitch.Contains("Y");
                                    break;
                                case "TrackSwitchWidth":
                                    map.TrackSwitchWidth = Convert.ToInt32(item.Value?.Trim());
                                    break;
                            }
                        }
                        break;
                    case "MenuItem_System":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "SystemModeControl":
                                    string sSystemModeControl = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_System.Visible_SystemModeControl = sSystemModeControl.Contains("Y");
                                    break;
                            }
                        }
                        break;
                    case "MenuItem_Operation":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "SystemModeControl":
                                    string sSystemModeControl = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_Operation.Visible_SystemModeControl = sSystemModeControl.Contains("Y");
                                    break;
                                case "PortManagement":
                                    string sPortManagement = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_Operation.Visible_PortManagement = sPortManagement.Contains("Y");
                                    break;
                            }
                        }
                        break;
                    case "MenuItem_Log":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "STATISTICS":
                                    string sSTATISTICS = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_Log.Visible_STATISTICS = sSTATISTICS.Contains("Y");
                                    break;
                            }
                        }
                        break;
                    case nameof(MenuItem_Report):
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case nameof(MenuItem_Report.Display):
                                    string sDisplay = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_Report.Display = sDisplay.Contains("Y");
                                    break;
                            }
                        }
                        break;
                    case nameof(MenuItem_Maintenance):
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "EquipmentConstant":
                                    string sEquimentConstant = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_Maintenance.Visible_EquipmentConstant = sEquimentConstant.Contains("Y");
                                    break;
                                case "ParkZoneManagement":
                                    string sParkZoneManagment = item.Value?.Trim().ToUpper() ?? "";
                                    menuItem_Maintenance.Visible_ParkZoneManagement = sParkZoneManagment.Contains("Y");
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region 系統設定
                    case "System":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case "SystemType":
                                    system.GetSystemType(item.Value?.Trim() ?? "");
                                    break;
                                case "DayShiftHour":
                                    system.DayShiftHour = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case nameof(system.OfflineMode):
                                    //這邊是看json設定，但是如果系統是MulitySystem模式開啟的ObjCacheManager.initialSettings()中會覆寫，強制進入OfflineMode
                                    system.OfflineMode = (item.Value?.Trim() ?? "").Contains("Y");
                                    break;
                                case nameof(system.HasCarrierType):
                                    string sHasCarrierType = item.Value?.Trim().ToUpper() ?? "";
                                    system.HasCarrierType = sHasCarrierType.Contains("Y");
                                    break;

                            }
                        }
                        break;
                    #endregion

                    #region 報表設定
                    case "Report":
                        foreach (var item in sitem.Items)
                        {
                            switch (item.Name)
                            {
                                case nameof(Report.AlarmGroupMin):
                                    report.AlarmGroupMin = Convert.ToInt32(item.Value?.Trim());
                                    break;

                                case nameof(Report.CheckAlarmCode_Mode):
                                    report.CheckAlarmCode_Mode = (eChkAlarm_CodeMode)Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case nameof(Report.MeanTimeOverMin):
                                    report.MeanTimeOverMin = Convert.ToInt32(item.Value?.Trim());
                                    break;
                                case nameof(Report.ExportRowLimit):
                                    report.ExportRowLimit = Convert.ToInt32(item.Value?.Trim());
                                    break;
                            }
                        }
                        break;
                        #endregion


                }
            }
        }
    }
}
