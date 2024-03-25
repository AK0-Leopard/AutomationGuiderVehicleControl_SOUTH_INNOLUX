using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using Grpc.Core;
using MirleGO_UIFrameWork.UI.uc_Button;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using ViewerObject;
using ViewerObject.LOG;

namespace com.mirle.ibg3k0.ohxc.wpf.Common
{
    public class ObjCacheManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger loggerGrpc = LogManager.GetLogger("GrpcLogger");
        private static Logger loggerDebug = LogManager.GetLogger("DebugLogger");
        private WindownApplication app = null;

        public ObjectConverterFactory.ObjectConverter ObjConverter { get; private set; } = null;
        public bool IsForAGVC { get; private set; } = false;

        private List<VVEHICLE> Vehicles = null;
        //private List<AEQPT> Eqpts = null;
        private List<Charger> Chargers = null;
        private List<Coupler> Couplers = null;
        //private List<MaintainLift> MTLs = null;
        private VLINE Line = null;
        private List<VUASUSR> USERs = null;
        private List<VUASUSRGRP> USERGROUPs = null;
        private List<VUASUFNC> USERFUNCs = null;
        private List<VUASFNC> FUNCs = null;
        private List<VCMD> COMMANDs = null;
        private List<VTRANSFER> TRANSFERs = null;
        private List<VALARM> ALARMs = null;
        public List<VCONSTANT> CONSTANTs { get; private set; }
        private List<AlarmMap> alarmMap = null;
        private List<EQMap> eqMap = null;
        private List<AlarmModule> alarmModules = null;

        private string LanguagePath = "";
        //private List<ObjectRelay.CarInOutViewObj> HCarInOuts = null;

        private ConcurrentStack<VEHICLE_COMMU_LOG> vhCommuLOGs = new ConcurrentStack<VEHICLE_COMMU_LOG>();
        private ConcurrentStack<MCS_COMMU_LOG> mcsCommuLOGs = new ConcurrentStack<MCS_COMMU_LOG>();
        private ConcurrentStack<SYSTEM_PROCESS_LOG> systemProcLOGs = new ConcurrentStack<SYSTEM_PROCESS_LOG>();

        public List<VTIPMESSAGE> TipMessages = new List<VTIPMESSAGE>();

        public event EventHandler<List<VTIPMESSAGE>> TipMessagesChange;

        public event EventHandler ChargerUpdateComplete;

        public event EventHandler MTLMTSInfoUpdate;

        public event EventHandler HCarInOutUpdateComplete;

        public event EventHandler<string> VehicleUpdateComplete;

        public event EventHandler ReserveInfoUpdate;

        public event EventHandler LogInUserChanged;

        //public event EventHandler<CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingRequest> ChargeControl;

        public List<ProjectInfo> ProjectInfos { get; private set; } = null;
        public Settings ViewerSettings { get; private set; } = null;

        #region DBTableWatcher Events
        public event EventHandler UasChange;
        public event EventHandler PortStationChange;
        public event EventHandler SegmentChange;
        public event EventHandler SectionChange;
        public event EventHandler CommandChange;
        public event EventHandler TransferChange;
        public event EventHandler CarrierChange;
        public event EventHandler AlarmChange;
        public event EventHandler ConstantChange;
        public void onAlarmChange(object sender = null)
        {
            AlarmChange?.Invoke(sender, null);
        }
        #endregion DBTableWatcher Events

        #region MapInfo

        public List<Address> Addresses { get; private set; } = null;
        public List<Section> Sections { get; private set; } = null;
        private List<TrackSwitch> trackSwitches = null;
        public List<TrackSwitch> TrackSwitches => trackSwitches;
        private List<Shelf> shelves = null;
        public List<Shelf> Shelves => shelves;
        public List<Label> Labels { get; private set; } = null;

        private List<VPORTSTATION> PORTSTATIONs = null;
        private List<VSEGMENT> SEGMENTs = null;
        //private List<PortDef> PortDefs = null;
        //private List<ACARRIER> CassetteDatas = null;
        //private List<ZoneDef> ZoneDefs = null;
        //private List<ShelfDef> ShelfDefs = null;
        //private List<BLOCKZONEQUEUE> BlockZoneQueue = null;
        public string MapId { get; private set; } = null;
        public string EFConnectionString { get; private set; } = null;

        #endregion MapInfo

        #region Alarm Map

        //private List<AlarmMap> AlarmMaps = null;

        #endregion Alarm Map

        public ObjCacheManager(WindownApplication _app)
        {
            app = _app;
            //Vehicles = app.VehicleBLL.loadAllVehicle();
            //Line = new ALINE();
        }

        public bool start()
        {
            bool isSuccess;
            initialProjectInfo();
            IsForAGVC = GetSelectedProject()?.ObjectConverter?.ToUpper().Contains("AGVC") ?? false;
            initialSettings();
            isSuccess = initialMapInfo();
            initialLanguage();
            alarmMap = loadAlarmMap();
            eqMap = loadEQMap();
            alarmModules = loadAlarmModule();
            return isSuccess;
        }


        private void initialProjectInfo()
        {
            ProjectInfos = loadProjectInfo();
            string folder = GetSelectedProject()?.MapInfoFolder;
            //app.InitSettings(folder);
            app.InitMapInfo(folder);
            replaceDLL();
        }

        private void initialLanguage()
        {
            try
            {
                LoadLanguageFile("en-US.xaml");
                app.LanguageCode = "en-US";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public void LoadLanguageFile(string languagefileName)
        {
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
            {
                Source = new Uri(LanguagePath + languagefileName, UriKind.RelativeOrAbsolute)
            };
        }

        private bool replaceDLL()
        {
            //try // 共通dll
            //{
            //	string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            //	string sourcePath = $"{sPath}\\DLL";
            //	if (!System.IO.Directory.Exists(sourcePath)) return false;
            //	string[] files = System.IO.Directory.GetFiles(sourcePath);
            //	foreach (string s in files)
            //	{
            //		if (!s.ToLower().EndsWith(".dll")) continue;
            //		string fileName = System.IO.Path.GetFileName(s);
            //		string destFile = System.IO.Path.Combine(sPath, fileName);
            //		Adapter.Invoke((obj) =>
            //		{
            //			System.IO.File.Copy(s, destFile, true);
            //		}, null);
            //	}
            //}
            //catch (Exception ex)
            //{
            //	logger.Error(ex, "Exception");
            //	return false;
            //}

            try // 各專案的dll
            {
                string sourceFolder = GetSelectedProject()?.ObjectConverter;
                if (string.IsNullOrWhiteSpace(sourceFolder)) return false;

                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                string sourcePath = $"{sPath}\\DLL\\{sourceFolder}";
                if (!System.IO.Directory.Exists(sourcePath)) return false;
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                foreach (string s in files)
                {
                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(sPath, fileName);
                    if (Adapter.Dispacher == null) Adapter.Initialize();

                    Adapter.Invoke((obj) =>
                    {
                        try
                        {
                            System.IO.File.Copy(s, destFile, true);
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex, "Exception");//因為別的錯誤造成這邊有可能遇到重複搬運，如果是這種情況記Log然後吃案就好，第一次般正常會成功，如果真的有被卡到也會記Log，打不開會變成顯示真正的問題而不會直接Crash
                        }
                    }, null);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }

        private void initialSettings()
        {
            ViewerSettings = loadSettings();
            if (ConfigurationManager.AppSettings["MulitySystem"].ToString().Contains("Y"))
            {
                ViewerSettings.system.OfflineMode = true;

            }
        }

        private bool initialMapInfo()
        {
            try
            {
                app.GetWebClientManager().SetControlPort(ViewerSettings?.webClientManager.ControlSystemPort);
                if (!ViewerSettings?.system.OfflineMode ?? false)
                {
                    MapId = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.MapID);
                    EFConnectionString = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.EFConnectionString);
                }
                if (string.IsNullOrWhiteSpace(EFConnectionString))
                {
                    EFConnectionString = ViewerSettings?.db.ConnectionString ?? "";
                    app.IsControlSystemConnentable = false;
                }
                string result = "";
                string objCvt = GetSelectedProject()?.ObjectConverter;
                ObjConverter = new ObjectConverterFactory.ObjectConverter(objCvt, BasicFunction.StringRelate.ExtractConnectionString(EFConnectionString), app.IsControlSystemConnentable, out result);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    logger.Warn(result);
                    return false;
                }
                LanguagePath = ObjConverter.LanguagePath;
                if (!app.IsControlSystemConnentable)
                {
                    logger.Warn("ControlSystem Unconnentable!");
                    return true;  //調整: 當開啟失敗原因為無法連上C，仍可開啟Viewer使用報表功能
                }
                registerEvent_DBTableWatcher();
                string mapInfoData_Port = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Port);
                string mapInfoData_Segment = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Segment);
                string mapInfoData_Vehicle = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Vehicle);
                string mapInfoData_Line = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Line);
                string mapInfoData_Alarm = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Alarm);
                bool isSuccess = true;
                if (isSuccess) isSuccess = ObjConverter.APortStation.SetVPORTSTATIONs(ref PORTSTATIONs, mapInfoData_Port, out result);
                if (isSuccess) isSuccess = ObjConverter.ASegment.SetVSEGMENTs(ref SEGMENTs, mapInfoData_Segment, out result);
                if (isSuccess) isSuccess = ObjConverter.AVehicle.SetVVEHICLEs(ref Vehicles, mapInfoData_Vehicle, out result);
                if (isSuccess) isSuccess = ObjConverter.ALine.SetVLINE_byString(ref Line, mapInfoData_Line, out result);
                if (isSuccess) isSuccess = ObjConverter.Alarm.SetVALARMs(ref ALARMs, mapInfoData_Alarm, out result);
                if (!isSuccess)
                {
                    logger.Warn(result);
                    return false;
                }
                updateUser();
                updateUserGroup();
                updateUserGroupFunc();
                updateCarrier();
                updateFuncCode();
                updateTransfer();
                updateCommand();
                updateConstant();
                //HCarInOuts = app.CmdBLL.GetHCarInOutByConditions(50); // Top N data
                Addresses = loadAddress();
                Sections = loadSection();
                shelves = loadShelf();
                trackSwitches = loadTrackSwitch();
                Labels = loadLabel();

                initPutVehicleLocks();
                updateChargers();

                initPorts();
                initShelves();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }

        #region DBTableWatcher
        private void registerEvent_DBTableWatcher()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return;
            }
            ObjConverter.DBTableWatcher.UasUserChange += _UasUserChange;
            ObjConverter.DBTableWatcher.UasUserGroupChange += _UasUserGroupChange;
            ObjConverter.DBTableWatcher.UasGroupFuncChange += _UasGroupFuncChange;
            ObjConverter.DBTableWatcher.UasFuncCodeChange += _UasFuncCodeChange;
            ObjConverter.DBTableWatcher.PortStationChange += _PortStationChange;
            ObjConverter.DBTableWatcher.SegmentChange += _SegmentChange;
            ObjConverter.DBTableWatcher.CommandChange += _CommandChange;
            ObjConverter.DBTableWatcher.TransferChange += _TransferChange;
            ObjConverter.DBTableWatcher.CarrierChange += _CarrierChange;
            ObjConverter.DBTableWatcher.AlarmChange += _AlarmChange;
            ObjConverter.DBTableWatcher.SectionChange += _SectionChange;
            ObjConverter.DBTableWatcher.ConstantChange += _ConstantChange;
        }
        private void _UasUserChange(object sender, EventArgs e)
        {
            if (updateUser())
                UasChange?.Invoke(sender, e);
        }
        private void _UasUserGroupChange(object sender, EventArgs e)
        {
            if (updateUserGroup())
                UasChange?.Invoke(sender, e);
        }
        private void _UasGroupFuncChange(object sender, EventArgs e)
        {
            if (updateUserGroupFunc())
                UasChange?.Invoke(sender, e);
        }
        private void _UasFuncCodeChange(object sender, EventArgs e)
        {
            if (updateFuncCode())
                UasChange?.Invoke(sender, e);
        }
        private void _PortStationChange(object sender, EventArgs e)
        {
            if (updatePortStation())
                PortStationChange?.Invoke(sender, e);
        }
        private void _SegmentChange(object sender, EventArgs e)
        {
            if (updateSegments())
                SegmentChange?.Invoke(sender, e);
        }
        public void _SectionChange(object sender, EventArgs e)
        {
            if (updateSections())
                SectionChange?.Invoke(sender, e);
        }
        private void _CommandChange(object sender, EventArgs e)
        {
            if (updateCommand())
                CommandChange?.Invoke(sender, e);
        }
        private void _TransferChange(object sender, EventArgs e)
        {
            if (updateTransfer())
                TransferChange?.Invoke(sender, e);
        }
        private void _CarrierChange(object sender, EventArgs e)
        {
            if (updateCarrier())
                CarrierChange?.Invoke(sender, e);
        }
        private void _AlarmChange(object sender, EventArgs e)
        {
            if (updateAlarm())
                AlarmChange?.Invoke(sender, e);
        }
        private void _ConstantChange(object sender, EventArgs e)
        {
            if (updateConstant())
                ConstantChange?.Invoke(sender, e);
        }

        #endregion DBTableWatcher

        #region load config
        private List<ProjectInfo> loadProjectInfo()
        {
            try
            {
                DataTable dt = app.ViewerConfig.Tables["PROJECTINFO"];
                var query = from c in dt.AsEnumerable()
                            select new ProjectInfo(c.Field<string>("Customer"),
                                                   c.Field<string>("ProductLine"),
                                                   c.Field<string>("MapInfoFolder"),
                                                   c.Field<string>("ObjectConverter"),
                                                   c.Field<string>("IsSelected"));
                var projects = query?.ToList() ?? new List<ProjectInfo>();
                if (projects.Where(p => p.IsSelected).Count() > 1)
                {
                    bool isFirst = true;
                    foreach (var project in projects)
                    {
                        if (project.IsSelected)
                        {
                            if (isFirst) isFirst = false;
                            else project.IsSelected = false;
                        }
                    }
                }
                return projects;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                throw;
            }
        }
        private Settings loadSettings()
        {
            try
            {
                //DataTable dt = app.ViewerConfig.Tables["SETTINGS"];
                //var query = from c in dt.AsEnumerable()
                //			select new Settings(c.Field<string>("WebClientManager_ControlSystemPort"),
                //								   c.Field<string>("MainView_CustomerLogo"),
                //								   c.Field<string>("MapBase_Type"),
                //								   c.Field<string>("MapBase_ShowScaleRuler"),
                //								   c.Field<string>("MapBase_AngleOfView"),
                //								   c.Field<string>("Map_RailWidth"),
                //								   c.Field<string>("Map_AddressWidth"),
                //								   c.Field<string>("Map_VehicleWidth"),
                //								   c.Field<string>("Map_SurroundingWhiteSpaceWidth"),
                //								   c.Field<string>("Map_RulerTickFrequency"));
                //var projects = query.First();
                //return projects;

                string folderName = GetSelectedProject()?.MapInfoFolder;
                string fileName = $"{Environment.CurrentDirectory}\\Config\\{folderName}\\SETTINGS.json";
                StreamReader sr = new StreamReader(fileName);
                string jsonString = sr.ReadToEnd();
                sr.Close();
                SettingFormat format = JsonConvert.DeserializeObject<SettingFormat>(jsonString);
                return new Settings(format);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                throw;
            }
        }
        public List<Address> loadAddress()
        {
            try
            {
                DataTable dt = app.ViewerConfig.Tables["MapInfo\\AADDRESS"];
                var query = from c in dt.AsEnumerable()
                            select new Address(c.Field<string>("Id"), startToDouble(c.Field<string>("PositionX")), startToDouble(c.Field<string>("PositionY")));
                return query?.ToList() ?? new List<Address>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                throw;
            }
        }
        public List<Section> loadSection()
        {
            try
            {
                DataTable dt = app.ViewerConfig.Tables["MapInfo\\ASECTION"];
                var query = from c in dt.AsEnumerable()
                            select new Section(Addresses, c.Field<string>("Id"), c.Field<string>("FromAddress"), c.Field<string>("ToAddress"), c.Field<string>("Type"));
                return query?.ToList() ?? new List<Section>();
            }
            catch
            {
                try
                {
                    DataTable dt = app.ViewerConfig.Tables["MapInfo\\ASECTION"];
                    var query = from c in dt.AsEnumerable()
                                select new Section(Addresses, c.Field<string>("Id"), c.Field<string>("FromAddress"), c.Field<string>("ToAddress"));
                    return query?.ToList() ?? new List<Section>();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                    throw;
                }
            }
        }
        public List<Label> loadLabel()
        {
            try
            {
                DataTable dt = app.ViewerConfig.Tables["MapInfo\\ALABEL"];
                var query = from c in dt.AsEnumerable()
                            select new Label(c.Field<string>("Id"), c.Field<string>("Text"), c.Field<string>("TextColor"),
                            startToDouble(c.Field<string>("X1")), startToDouble(c.Field<string>("X2")),
                            startToDouble(c.Field<string>("Y1")), startToDouble(c.Field<string>("Y2")),
                            c.Field<string>("Visible"));
                return query?.ToList() ?? new List<Label>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                //throw;
                return new List<Label>();
            }
        }
        public List<Shelf> loadShelf()
        {
            if (!(ViewerSettings?.map.ShowShelf ?? false))
                return new List<Shelf>();
            try
            {
                DataTable dt = app.ViewerConfig.Tables["MapInfo\\SHELF"];
                if (dt == null) return new List<Shelf>();

                List<Shelf> result = new List<Shelf>();

                // 建立 ViewerObject.Shelf 基本資料
                foreach (DataRow row in dt.Rows)
                {
                    var address = Addresses.Where(a => a.ID == row.Field<string>("ADR_ID")).FirstOrDefault();
                    if (string.IsNullOrEmpty(row.Field<string>("ShelfID"))) continue;
                    if (address == null)
                    {
                        result.Add(new Shelf(row.Field<string>("ShelfID"),
                                         row.Field<string>("POS_X"),
                                         row.Field<string>("POS_Y")));
                    }
                    else
                    {
                        result.Add(new Shelf(row.Field<string>("ShelfID"),
                                         row.Field<string>("ZoneID"),
                                         address));
                    }

                }

                // 計算 Direction Vector
                if (GetSelectedProject().ObjectConverter == "OHBC_ASE_K21")
                {
                    // By Section 
                    if (Sections?.Count > 0)
                    {
                        foreach (var shelf in result)
                        {
                            Section section = Sections.Where(s => s.EndAdr == shelf.ADDRESS.ID).FirstOrDefault();
                            if (section == null) section = Sections.Where(s => s.StartAdr == shelf.ADDRESS.ID).FirstOrDefault();
                            if (section == null) continue;

                            Point dir_vec = new Point(section.EndAddress.X - section.StartAddress.X,
                                                      section.EndAddress.Y - section.StartAddress.Y);

                            shelf.ZONE_DIR_VEC = dir_vec;
                        }
                    }
                }
                else
                {
                    // Default: By Zone
                    List<string> zoneIDs = result.Select(s => s.ZONE_ID).Distinct().OrderBy(s => s).ToList();
                    foreach (var zoneID in zoneIDs)
                    {
                        var shelvesInZone = result.Where(s => s.ZONE_ID == zoneID).OrderBy(s => s.SNO).ToList();
                        if (shelvesInZone.Count < 2) continue; // 需要 2 個點才能求方向
                        Point zone_dir_vec = new Point(0, 0);
                        if (shelvesInZone[shelvesInZone.Count - 1].ADDRESS == null || shelvesInZone[0].ADDRESS == null)
                        {
                            zone_dir_vec = new Point(shelvesInZone[shelvesInZone.Count - 1].POS_X - shelvesInZone[0].POS_X,
                                                       shelvesInZone[shelvesInZone.Count - 1].POS_Y - shelvesInZone[0].POS_Y);
                        }
                        else
                            zone_dir_vec = new Point(shelvesInZone[shelvesInZone.Count - 1].ADDRESS.X - shelvesInZone[0].ADDRESS.X,
                                                       shelvesInZone[shelvesInZone.Count - 1].ADDRESS.Y - shelvesInZone[0].ADDRESS.Y);

                        foreach (var shelf in shelvesInZone)
                        {
                            shelf.ZONE_DIR_VEC = zone_dir_vec;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                //throw;
                return new List<Shelf>();
            }
        }
        public List<TrackSwitch> loadTrackSwitch()
        {
            if (!(ViewerSettings?.map.ShowTrackSwitch ?? false))
                return new List<TrackSwitch>();
            try
            {
                DataTable dt = app.ViewerConfig.Tables["MapInfo\\TRACKSWITCH"];
                if (dt == null) return new List<TrackSwitch>();

                var trackSwitches = new List<TrackSwitch>();
                foreach (DataRow row in dt.Rows)
                {
                    bool enable = row.Field<string>("Enable")?.ToUpper().Contains("Y") ?? false;
                    if (!enable) continue;
                    var track1 = Sections.Where(s => s.ID == row.Field<string>("Track1")).FirstOrDefault();
                    if (track1 == null) continue;
                    var track2 = Sections.Where(s => s.ID == row.Field<string>("Track2")).FirstOrDefault();
                    if (track2 == null) continue;
                    trackSwitches.Add(new TrackSwitch(row.Field<string>("ID"), track1, track2));
                }
                return trackSwitches;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return new List<TrackSwitch>();
            }
        }
        public List<AlarmMap> loadAlarmMap()
        {
            List<AlarmMap> result = new List<AlarmMap>();
            try
            {
                DataTable dt = app.ViewerConfig.Tables["ALARMMAP"];
                if (dt == null) return null;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new AlarmMap(
                            row.Field<string>("OBJECT_ID"),
                            row.Field<string>("ALARM_ID"),
                            row.Field<string>("ALARM_LVL"),
                            row.Field<string>("ALARM_DESC"),
                            row.Field<string>("ALARM_DESC_TW"),
                            row.Field<string>("HAPPENED_REASON"),
                            row.Field<string>("SOLUSTION")
                            ));
                }
                if (result != null) app.AlarmBLL.setMulitLanguage(true, result); // 如果AlarmMap被正式讀到且組物件完成，告訴AlarmBLL開啟多語系功能 
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                result = null;
            }
            return result;
        }
        private List<EQMap> loadEQMap()
        {
            List<EQMap> result = new List<EQMap>();
            try
            {
                DataTable dt = app.ViewerConfig.Tables["EQMAP"];
                if (dt == null) return null;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new EQMap(
                            row.Field<string>("ID"),
                            row.Field<string>("EQ_NAME"),
                            row.Field<string>("EQ_NUMBER")
                            ));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                result = null;
            }
            return result;
        }
        private List<AlarmModule> loadAlarmModule()
        {
            List<AlarmModule> result = new List<AlarmModule>();
            try
            {
                DataTable dt = app.ViewerConfig.Tables["ALARMMODULE"];
                if (dt == null) return null;
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new AlarmModule(
                            startToInt(row.Field<string>("NUMBER")),
                            row.Field<string>("MODULE_EN"),
                            row.Field<string>("MODULE_TW")
                            ));
                }
                if (result != null) app.AlarmBLL.setAlarmModuleLsit(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                result = null;
            }
            return result;
        }

        private double startToDouble(string value)
        {
            double.TryParse(value, out double result);
            return result;
        }
        private int startToInt(string value)
        {
            int.TryParse(value, out int result);
            return result;
        }

        #endregion load

        public List<VPORTSTATION> GetMonitoringPorts() => PORTSTATIONs?.Where(p => p.IS_MONITORING).ToList() ?? new List<VPORTSTATION>();

        public bool updateChargers()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!GetSelectedProject()?.ObjectConverter?.ToUpper().Contains("AGVC") ?? false)
            {
                return false;
            }
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            try
            {
                string couplerInfos = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.CouplerInfo);
                string unitsInfos = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Charger);
                string result = "";
                bool isSuccess = ObjConverter.ChargerInfo.SetChargers(ref Chargers, couplerInfos, unitsInfos, out result);
                if (!isSuccess) logger.Warn(result);
                else ChargerUpdateComplete?.Invoke(this, null);
                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }

        #region Get
        public ProjectInfo GetSelectedProject() => ProjectInfos?.Where(p => p.IsSelected).SingleOrDefault();
        public VVEHICLE GetVEHICLE(string vh_id) => Vehicles?.Where(vh => vh.VEHICLE_ID == vh_id.Trim()).SingleOrDefault();
        public List<VVEHICLE> GetVEHICLEs() => Vehicles ?? new List<VVEHICLE>();
        public List<string> GetAllReserveInfo()
        {
            List<string> reserveInfos = new List<string>();
            if (Vehicles?.Count > 0)
            {
                foreach (var vh in Vehicles)
                {
                    if (vh.PATH_RESERVED_SECTIONS.Count > 0)
                        reserveInfos.AddRange(vh.PATH_RESERVED_SECTIONS);
                }
            }
            return reserveInfos;
        }
        public List<string> GetAllReserveInfo(VVEHICLE MonitoringVh)
        {
            List<string> reserveInfos = new List<string>();
            if (Vehicles?.Count > 0)
            {
                foreach (var vh in Vehicles)
                {
                    if (vh != MonitoringVh) continue;
                    else if (vh.PATH_RESERVED_SECTIONS.Count > 0)
                        reserveInfos.AddRange(vh.PATH_RESERVED_SECTIONS);
                }
            }
            return reserveInfos;
        }
        public VLINE GetLine() => Line;
        public List<VSEGMENT> GetSegments() => SEGMENTs;
        public List<VPORTSTATION> GetPortStations() => PORTSTATIONs;
        public VPORTSTATION GetPortStation(string addressID) => PORTSTATIONs.Where(station => station.ADR_ID == addressID.Trim()).FirstOrDefault();
        public VPORTSTATION GetPortStationByPortID(string portID) => PORTSTATIONs.Where(p => p.PORT_ID == portID.Trim()).FirstOrDefault();
        public List<Charger> GetChargers() => Chargers;
        private static object _couplerlock = new object();
        public List<Coupler> GetCouplers()
        {
            if (Chargers == null)
            {
                Couplers = new List<Coupler>();
                return Couplers;
            }

            lock (_couplerlock)
            {
                if (Couplers == null) Couplers = new List<Coupler>();
                Couplers.Clear();
                foreach (var cg in Chargers)
                {
                    foreach (var cp in cg.Couplers)
                    {
                        Couplers.Add(cp.Value);
                    }
                }
            }
            return Couplers;
        }
        public List<VALARM> GetAlarms() => ALARMs;
        public int getAlarmCountByVehicleID(string vh_id) => ALARMs?.Where(a => a.EQPT_ID == vh_id?.Trim()).Count() ?? 0;
        public List<VCMD> GetCOMMANDs() => COMMANDs;
        public List<VTRANSFER> GetTRANSFERs() => TRANSFERs;
        //public List<ObjectRelay.CarInOutViewObj> GetHCarInOut() => HCarInOuts;
        public List<AlarmMap> GetAlarmMaps() => alarmMap;
        //public List<AEQPT> GetMTL1MTS1() => Eqpts.Where(c => c.EQPT_ID.Trim() == "MTS" || c.EQPT_ID.Trim() == "MTL").ToList();
        //public AEQPT GetMTLMTSByID(string station_id) => Eqpts.Where(c => c.EQPT_ID.Trim() == station_id.Trim()).FirstOrDefault();
        //public List<AEQPT> GetMTS2() => Eqpts.Where(c => c.EQPT_ID.Trim() == "MTS2").ToList();
        //public List<AEQPT> GetMaintainDevice()
        //{
        //	return Eqpts.Where(c => c is MaintainLift || c is MaintainSpace).ToList();
        //}

        //public OHCV GetOHCV(string segmentID)
        //{
        //	return Eqpts.Where(c => c is OHCV && (c as OHCV).SegmentLocation == segmentID).
        //				 FirstOrDefault() as OHCV;
        //}
        public List<VUASUSR> GetUsers() => USERs;
        public List<VUASUSRGRP> GetUserGroups() => USERGROUPs;
        public List<VUASUFNC> GetUserFuncs(string user_grp_id) => USERFUNCs.Where(f => f.USER_GRP == user_grp_id.Trim()).ToList();
        public List<VUASFNC> GetFunctionCodes() => FUNCs;
        public List<EQMap> GetEQMap() => eqMap;
        public List<AlarmModule> GetAlarmModule() => alarmModules;

        #endregion Get

        #region Put
        private List<object> PutVehicleLocks = new List<object>();
        private void initPutVehicleLocks()
        {
            PutVehicleLocks.Clear();
            for (int i = 0; i < Vehicles.Count; i++)
            {
                PutVehicleLocks.Add(new object());
            }
        }
        public void PutVehicle(byte[] new_vh_bytes)
        {
            try
            {
                if (new_vh_bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                object new_vh = null;
                string vh_id = "";
                bool isSuccess = true;
                string result = "";
                if (isSuccess) isSuccess = ObjConverter.VehicleInfo.Convert2Object_VehicleInfo(new_vh_bytes, out new_vh, out result);
                if (isSuccess) vh_id = ObjConverter.VehicleInfo.GetVhID(new_vh);
                if (!isSuccess)
                {
                    logger.Warn(result);
                    return;
                }
                var vh = Vehicles?.Where(v => v.VEHICLE_ID == vh_id).SingleOrDefault();
                if (vh == null) return;
                lock (PutVehicleLocks[Vehicles.IndexOf(vh)])
                {
                    var oldReservedSections = vh.PATH_RESERVED_SECTIONS;
                    isSuccess = ObjConverter.VehicleInfo.SetVVEHICLE(ref vh, new_vh, out result);
                    switch (GetSelectedProject()?.ObjectConverter)
                    {
                        case "AGVC_NORTH_INNOLUX":
                        case "AGVC_UMTC":
                        case "OHBC_ASE_K21":
                        case "OHTC_AT_S":
                            if (isSuccess) isSuccess = VVEHICLE_Addition.SetVhAxis(ref vh, Addresses, Sections, out result); // VehicleInfo沒有XY座標 另外設定
                            break;
                    }
                    if (!isSuccess) logger.Warn(result);
                    else
                    {
                        VehicleUpdateComplete?.Invoke(this, vh.VEHICLE_ID);
                        if (BasicFunction.Compare.Different(oldReservedSections, vh.PATH_RESERVED_SECTIONS))
                            ReserveInfoUpdate?.Invoke(this, null);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }

        //public void PutPortInfo(PORT_INFO new_port)
        //{
        //	if (new_port == null) return;
        //	APORTSTATION port = PORTSTATIONs.Where(x => x.PORT_ID.Trim() == new_port.PortID.Trim()).SingleOrDefault();
        //	//port.set(new_port);
        //	//PortDefUpdateComplete?.Invoke(this, EventArgs.Empty);
        //	//AVEHICLE vh = Vehicles.Where(v => v.VEHICLE_ID == new_vh.VEHICLEID.Trim()).SingleOrDefault();
        //	//vh.set(new_vh);
        //	//vh.NotifyVhPositionChange();
        //	//vh.NotifyVhStatusChange();
        //	//VehicleUpdateComplete?.Invoke(this, EventArgs.Empty);
        //}

        //public void PutVehicle(AVEHICLE new_vh)
        //{
        //    if (new_vh == null) return;
        //    AVEHICLE vh = Vehicles.Where(v => v.VEHICLE_ID == new_vh.VEHICLE_ID.Trim()).SingleOrDefault();
        //    vh.set(new_vh);
        //    vh.NotifyVhPositionChange();
        //    vh.NotifyVhStatusChange();
        //}

        public void putVhCommuLOG(byte[] data)
        {
            try
            {
                if (data == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                VEHICLE_COMMU_LOG log = null;
                bool isSuccess = ObjConverter.LOG.EqLogInfo.SetVEHICLE_COMMU_LOG(data, out log, out result);
                if (!isSuccess) logger.Warn(result);
                else vhCommuLOGs.Push(log);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public List<VEHICLE_COMMU_LOG> popAll_VhCommuLOGs()
        {
            List<VEHICLE_COMMU_LOG> logs = vhCommuLOGs.Count() > 0 ? vhCommuLOGs.OrderByDescending(l => l.Time).ToList() : new List<VEHICLE_COMMU_LOG>();
            vhCommuLOGs.Clear();
            return logs;
        }

        public void putMcsCommuLOG(byte[] data)
        {
            try
            {
                if (data == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                MCS_COMMU_LOG log = null;
                bool isSuccess = ObjConverter.LOG.HostLogInfo.SetMCS_COMMU_LOG(data, out log, out result);
                if (!isSuccess) logger.Warn(result);
                else mcsCommuLOGs.Push(log);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public List<MCS_COMMU_LOG> popAll_McsCommuLOGs()
        {
            List<MCS_COMMU_LOG> logs = mcsCommuLOGs.Count() > 0 ? mcsCommuLOGs.OrderByDescending(l => l.Time).ToList() : new List<MCS_COMMU_LOG>();
            mcsCommuLOGs.Clear();
            return logs;
        }

        public void putSystemProcLOGs(byte[] data)
        {
            try
            {
                if (data == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                SYSTEM_PROCESS_LOG log = null;
                bool isSuccess = ObjConverter.LOG.SystemProcInfo.SetSYSTEM_PROCESS_LOG(data, out log, out result);
                if (!isSuccess) logger.Warn(result);
                else systemProcLOGs.Push(log);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public List<SYSTEM_PROCESS_LOG> popAll_SystemProcLOGs()
        {
            List<SYSTEM_PROCESS_LOG> logs = systemProcLOGs.Count() > 0 ? systemProcLOGs.OrderByDescending(l => l.Time).ToList() : new List<SYSTEM_PROCESS_LOG>();
            systemProcLOGs.Clear();
            return logs;
        }

        public void putLine(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                bool isSuccess = ObjConverter.LineInfo.SetVLINE_by_LINE_INFO(ref Line, bytes, out result);
                if (!isSuccess) logger.Warn(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public void putConnectionInfo(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                bool isSuccess = ObjConverter.LineInfo.SetVLINE_by_CONNECTION_INFO(ref Line, bytes, out result);
                if (!isSuccess) logger.Warn(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public void putTransferInfo(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                bool isSuccess = ObjConverter.LineInfo.SetVLINE_by_TRANSFER_INFO(ref Line, bytes, out result);
                if (!isSuccess) logger.Warn(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public void putOnlineCheckInfo(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                bool isSuccess = ObjConverter.LineInfo.SetVLINE_by_ONLINE_CHECK_INFO(ref Line, bytes, out result);
                if (!isSuccess) logger.Warn(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        public void putPingCheckInfo(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }
                string result = "";
                bool isSuccess = ObjConverter.LineInfo.SetVLINE_by_PING_CHECK_INFO(ref Line, bytes, out result);
                if (!isSuccess) logger.Warn(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }

        //public void putMTL_MTSCheckInfo(sc.ProtocolFormat.OHTMessage.MTL_MTS_INFO newMTLMTSInfo)
        //{
        //	if (newMTLMTSInfo == null) return;
        //	AEQPT eqpt = Eqpts.Where(e => e.EQPT_ID == newMTLMTSInfo.StationID.Trim()).SingleOrDefault();
        //	if (eqpt is MaintainSpace)
        //	{
        //		MaintainSpace MTS = eqpt as MaintainSpace;
        //		MTS.Plc_Link_Stat = newMTLMTSInfo.NetworkLink ? sc.App.SCAppConstants.LinkStatus.LinkOK : sc.App.SCAppConstants.LinkStatus.LinkFail;
        //		MTS.Is_Eq_Alive = newMTLMTSInfo.Alive;
        //		MTS.MTxMode = newMTLMTSInfo.Mode ? sc.ProtocolFormat.OHTMessage.MTxMode.Auto : sc.ProtocolFormat.OHTMessage.MTxMode.Manual;
        //		MTS.Interlock = newMTLMTSInfo.Interlock;
        //		MTS.CurrentCarID = newMTLMTSInfo.CarID;
        //		MTS.CurrentPreCarOurDistance = Convert.ToUInt32(newMTLMTSInfo.Distance);
        //		MTS.SynchronizeTime = Convert.ToDateTime(newMTLMTSInfo.SynchronizeTime);
        //	}
        //	else if (eqpt is MaintainLift)
        //	{
        //		MaintainLift MTL = eqpt as MaintainLift;
        //		MTL.Plc_Link_Stat = newMTLMTSInfo.NetworkLink ? sc.App.SCAppConstants.LinkStatus.LinkOK : sc.App.SCAppConstants.LinkStatus.LinkFail;
        //		MTL.Is_Eq_Alive = newMTLMTSInfo.Alive;
        //		MTL.MTxMode = newMTLMTSInfo.Mode ? sc.ProtocolFormat.OHTMessage.MTxMode.Auto : sc.ProtocolFormat.OHTMessage.MTxMode.Manual;
        //		MTL.Interlock = newMTLMTSInfo.Interlock;
        //		MTL.CurrentCarID = newMTLMTSInfo.CarID;
        //		MTL.MTLLocation = newMTLMTSInfo.MTLLocation == MTLLocation.Bottorn.ToString() ? MTLLocation.Bottorn : newMTLMTSInfo.MTLLocation == MTLLocation.Upper.ToString() ? MTLLocation.Upper : MTLLocation.None;
        //		MTL.CurrentPreCarOurDistance = Convert.ToUInt32(newMTLMTSInfo.Distance);
        //		MTL.SynchronizeTime = Convert.ToDateTime(newMTLMTSInfo.SynchronizeTime);
        //	}

        //	MTLMTSInfoUpdate?.Invoke(this, EventArgs.Empty);
        //}

        public void putTipMessageInfos(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;
                string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
                if (ObjConverter == null)
                {
                    logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                    return;
                }

                int recTipMsgCount = TipMessages?.Count ?? 0;
                VTIPMESSAGE recFirstTipMsg = recTipMsgCount == 0 ? null : TipMessages[0];
                string result = "";
                bool isSuccess = ObjConverter.TipMessageInfo.SetVTIPMESSAGEs(ref TipMessages, bytes, out result);
                if (!isSuccess) logger.Warn(result);
                else
                {
                    int iTipMsgCount = TipMessages?.Count ?? 0;
                    if (iTipMsgCount == 0) return;

                    bool isSame = false;
                    if (recTipMsgCount == iTipMsgCount && recFirstTipMsg != null)
                    {
                        isSame = recFirstTipMsg.Msg == TipMessages[0].Msg &&
                                 recFirstTipMsg.MsgLevel == TipMessages[0].MsgLevel &&
                                 recFirstTipMsg.Time == TipMessages[0].Time &&
                                 recFirstTipMsg.XID == TipMessages[0].XID;
                    }
                    if (!isSame) TipMessagesChange?.Invoke(this, TipMessages);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

        }
        #endregion Put

        public void onLogInUserChanged()
        {
            LogInUserChanged?.Invoke(this, EventArgs.Empty);
        }
        public bool updateUser()
        {
            try
            {
                var users = app.UserBLL.LoadAllUser();
                USERs = users;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateUserGroup()
        {
            try
            {
                var userGroups = app.UserBLL.LoadAllUserGroup();
                USERGROUPs = userGroups;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateUserGroupFunc()
        {
            try
            {
                var userGroupFuncs = app.UserBLL.LoadAllUserGroupFunc();
                USERFUNCs = userGroupFuncs;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateFuncCode()
        {
            try
            {
                var FuncCodes = app.UserBLL.LoadAllFunctionCode();
                FUNCs = FuncCodes;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updatePortStation()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            string result = "";
            bool isSuccess = true;
            try
            {
                string mapInfoData_Port = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Port);
                isSuccess = ObjConverter.APortStation.SetVPORTSTATIONs(ref PORTSTATIONs, mapInfoData_Port, out result);
                if (!isSuccess) logger.Warn(result);

                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateSegments()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            string result = "";
            bool isSuccess = true;
            try
            {
                string mapInfoData_Segment = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Segment);
                isSuccess = ObjConverter.ASegment.SetVSEGMENTs(ref SEGMENTs, mapInfoData_Segment, out result);
                if (!isSuccess) logger.Warn(result);

                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateSections()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null || ObjConverter.ASectionBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            string result = "";
            bool isSuccess = true;
            List<ViewerObject.Section> secs = new List<Section>();
            try
            {
                string mapInfoData_Segment = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Section);
                isSuccess = ObjConverter.ASectionBLL.SetSections(ref secs, mapInfoData_Segment, out result);
                //再來把secs更新回主資料，也就是cache的sections
                foreach (var sec in secs)
                {
                    Section Osec = Sections.FirstOrDefault(x => x.ID == sec.ID);
                    if (Osec != null) { Osec.enable = sec.enable; }
                }
                if (!isSuccess) logger.Warn(result);

                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateCommand()
        {
            try
            {
                var cmd = app.CmdBLL.LoadUnfinishCmds();
                COMMANDs = cmd;
                updateAndCombinevTransfervCmd(TRANSFERs, cmd);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateTransfer()
        {
            try
            {
                var tsf = app.CmdBLL.LoadUnfinishedTransfers();
                //TRANSFERs = tsf;
                updateAndCombinevTransfervCmd(tsf, COMMANDs);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        private void updateAndCombinevTransfervCmd(List<VTRANSFER> vTrans, List<VCMD> vCmds)
        {
            List<VTRANSFER> vtran_temp = new List<VTRANSFER>();
            foreach (var tran in vTrans)
            {
                VCMD cmd = vCmds == null ?
                           null :
                           vCmds.Where(c => WinFromUtility.isMatch(c.TRANSFER_ID, tran.TRANSFER_ID)).FirstOrDefault();
                vtran_temp.Add(app.CustomerObjBLL.GetVTRANSFER(tran, cmd));
            }
            TRANSFERs = vtran_temp;
        }
        public bool updateCarrier()
        {
            try
            {
                //var zone_shelf = app.CassetteDataBLL.loadCassetteData();
                //CassetteDatas = zone_shelf;
                //CassetteDefUpdateComplete?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateAlarm()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            string result = "";
            bool isSuccess = true;
            try
            {
                string mapInfoData_Alarm = app.MapBLL.GetMapInfoFromHttp(Definition.MapInfoType.Alarm);
                isSuccess = ObjConverter.Alarm.SetVALARMs(ref ALARMs, mapInfoData_Alarm, out result);
                if (!isSuccess) logger.Warn(result);

                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateConstant()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.ConstantBLL == null)
            {
                return false; //目前只有南群有開ConstantBLL功能，其餘專案應保持null
            }
            string result = "";
            bool isSuccess = true;
            try
            {
                CONSTANTs = app.ConstantBLL.getAllContants();
                if (CONSTANTs.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public void updateAlarm(List<VALARM> alarms)
        {
            ALARMs = alarms ?? new List<VALARM>();
        }

        public void updateHCarInOut()
        {
            //HCarInOuts = app.CmdBLL.GetHCarInOutByConditions(50); // Top N data
            HCarInOutUpdateComplete?.Invoke(this, EventArgs.Empty);
        }



        //public void updatePortDef()
        //{
        //	var portdef = app.PortDefBLL.GetOHB_PortData(Line.LINE_ID);
        //	PortDefs = portdef;
        //	PortDefUpdateComplete?.Invoke(this, EventArgs.Empty);
        //}

        //public void updateShelfDef()
        //{
        //	var shelf = app.ShelfDefBLL.loadShelfData();
        //	ShelfDefs = shelf;
        //	ShelDeffUpdateComplete?.Invoke(this, EventArgs.Empty);
        //}

        //public void updateZoneDef()
        //{
        //	var zone = app.ZoneDefBLL.loadZoneData();
        //	ZoneDefs = zone;
        //	ZoneDefUpdateComplete?.Invoke(this, EventArgs.Empty);
        //}

        //public void updateMTL()
        //{
        //	MTLs = app.MapBLL.GetMapInfosFromHttp<MaintainLift>(Definition.MapInfoType.MTL);
        //	MTLMTSInfoUpdate?.Invoke(this, EventArgs.Empty);
        //}

        #region gRPC function
        #region Port
        public void initPorts()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;

            if (PORTSTATIONs != null)
            {
                // Already initialed by other method.
                return;
            }
            PORTSTATIONs = new List<VPORTSTATION>();

            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return;
            }
            if (ObjConverter.BLL.PortBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.PortBLL = null");
                return;
            }

            string result = "";
            bool isSuccess = false;
            //try
            //{
            //    isSuccess = ObjConverter.BLL.PortBLL.GetMonitoringPortsFromDB(out List<VPORTSTATION> mPorts, out result);
            //    if (!isSuccess) logger.Warn(result);
            //    else
            //    {
            //        PORTSTATIONs.AddRange(mPorts);
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Exception");
            //}
            try
            {
                //loggerGrpc.Info($"ObjCacheManager.{mn}: Start gRPC function GetMonitoringPorts()");
                isSuccess = ObjConverter.BLL.PortBLL.GetMonitoringPorts(out List<VPORTSTATION> mPorts, out result);
                //loggerGrpc.Info($"ObjCacheManager.{mn}: End gRPC function GetMonitoringPorts()");
                if (!isSuccess) logger.Warn(result);
                else PORTSTATIONs.AddRange(mPorts);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            try
            {
                isSuccess = ObjConverter.BLL.PortBLL.GetEqPorts(out List<VPORTSTATION> eqPorts, out result);
                if (!isSuccess) logger.Warn(result);
                else PORTSTATIONs.AddRange(eqPorts);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        public bool updatePorts_grpc()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (PORTSTATIONs == null || PORTSTATIONs.Count == 0)
            {
                return false;
            }
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.PortBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.PortBLL = null");
                return false;
            }
            try
            {
                //loggerGrpc.Info($"ObjCacheManager.{mn}: Start gRPC function UpdatePorts()");
                bool isSuccess = ObjConverter.BLL.PortBLL.UpdatePorts(ref PORTSTATIONs, out string result);
                //loggerGrpc.Info($"ObjCacheManager.{mn}: End gRPC function UpdatePorts()");
                if (!isSuccess) logger.Warn(result);
                else PortStationChange?.Invoke(this, null);
                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        #endregion Port
        #region Shelf
        public bool initShelves()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (Shelves == null || Shelves.Count == 0)
            {
                return false;
            }
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.ShelfBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.ShelfBLL = null");
                return false;
            }
            try
            {
                bool isSuccess = ObjConverter.BLL.ShelfBLL.GetAllShelves(ref shelves, out string result);
                if (!isSuccess) logger.Warn(result);
                //else ShelfStatusChanged?.Invoke(this, null);
                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool updateShelves_grpc()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (Shelves == null || Shelves.Count == 0)
            {
                return false;
            }
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.ShelfBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.ShelfBLL = null");
                return false;
            }
            try
            {
                //loggerGrpc.Info($"ObjCacheManager.{mn}: Start gRPC function UpdateShelves()");
                bool isSuccess = ObjConverter.BLL.ShelfBLL.UpdateShelves(ref shelves, out string result);
                //loggerGrpc.Info($"ObjCacheManager.{mn}: End gRPC function UpdateShelves()");
                if (!isSuccess) logger.Warn(result);
                //else ShelfStatusChanged?.Invoke(this, null);
                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        #endregion Shelf
        #region TrackSwitch
        public bool updateTrackSwitches_grpc()
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (TrackSwitches == null || TrackSwitches.Count == 0)
            {
                return false;
            }
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.TrackSwitchBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.TrackSwitchBLL = null");
                return false;
            }
            try
            {
                //loggerGrpc.Info($"ObjCacheManager.{mn}: Start gRPC function UpdateTrackSwitches()");
                bool isSuccess = ObjConverter.BLL.TrackSwitchBLL.UpdateTrackSwitches(ref trackSwitches, out string result);
                //loggerGrpc.Info($"ObjCacheManager.{mn}: End gRPC function UpdateTrackSwitches()");
                if (!isSuccess) logger.Warn(result);
                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        public bool resetTrackSwitch_grpc(string id)
        {
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (TrackSwitches == null || TrackSwitches.Count == 0)
            {
                return false;
            }
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.TrackSwitchBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.TrackSwitchBLL = null");
                return false;
            }
            try
            {
                //loggerGrpc.Info($"ObjCacheManager.{mn}: Start gRPC function ResetTrackSwitchByID()");
                bool isSuccess = ObjConverter.BLL.TrackSwitchBLL.ResetTrackSwitchByID(id, out string result);
                //loggerGrpc.Info($"ObjCacheManager.{mn}: End gRPC function ResetTrackSwitchByID()");
                if (!isSuccess) logger.Warn(result);
                return isSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return false;
            }
        }
        #endregion TrackSwitch
        #region segControl
        public bool segControl(string seg, bool enable)
        {
            bool isSuccess = true;

            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.SegmentBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.SegmentBLL = null");
                return false;
            }


            try
            {
                UtilsAPI.BLL.SegmentBLL.segControlEventArgs args;
                if (enable)
                    args = new UtilsAPI.BLL.SegmentBLL.segControlEventArgs(seg, UtilsAPI.BLL.SegmentBLL.segControlEventArgs.ContolMode.enable);
                else
                    args = new UtilsAPI.BLL.SegmentBLL.segControlEventArgs(seg, UtilsAPI.BLL.SegmentBLL.segControlEventArgs.ContolMode.disable);
                app.SegmentBLL.segControlEventHandler?.Invoke(this, args); //觸發BLL裡面的segControl事件
            }
            catch (Exception ex)
            {
                isSuccess = false;
                logger.Error(ex, "Exception");
            }
            return isSuccess;
        }
        #endregion
        #region constantControl
        public void constantControl(string ECID, string value)
        {
            bool isSuccess = false;
            CommonMessage.ProtocolFormat.ControllerSettingFun.ControllerParameterSettingRequest request =
                new CommonMessage.ProtocolFormat.ControllerSettingFun.ControllerParameterSettingRequest();
            CommonMessage.ProtocolFormat.ControllerSettingFun.ControllerParameterSettingReply reply;

            if (app.ConstantBLL == null || ObjConverter.BLL.ConstantBLL == null)
            {
                TipMessage_Type_Light.Show("No support", "Sorry, this method doesnt' support your project", BCAppConstants.INFO_MSG);
                return;
            }
            if (string.IsNullOrEmpty(ECID) || string.IsNullOrEmpty(value))
            {
                TipMessage_Type_Light.Show("Warring", "ECID or value in null!", BCAppConstants.INFO_MSG);
                return;
            }

            request.ParameterID = ECID;
            request.ParameterValue = value;

            try
            {
                (isSuccess, reply) = app.ObjCacheManager.ObjConverter.BLL.ConstantBLL.constantControl(request);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                reply = new CommonMessage.ProtocolFormat.ControllerSettingFun.ControllerParameterSettingReply();
                reply.Result = ex.Message;
                logger.Error(ex, "Expection");
            }
            if (!isSuccess)
            {
                TipMessage_Type_Light.Show("Send command failed", reply.Result, BCAppConstants.INFO_MSG);
            }
            else
            {
                TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
            }
        }
        #endregion
        #region Reset Buzzer
        public void resetBuzzer()
        {
            bool isSuccess = false;
            if (app.ConstantBLL == null || ObjConverter.BLL.ConstantBLL == null)
            {
                TipMessage_Type_Light.Show("No support", "Sorry, this method doesnt' support your project", BCAppConstants.INFO_MSG);
                return;
            }
            try
            {
                app.ObjCacheManager.ObjConverter.BLL.ConstantBLL.resetBuzzer();
                isSuccess = true;

            }
            catch (Exception ex)
            {
                isSuccess = false;
                logger.Error(ex, "Expection");
            }
            if (!isSuccess)
            {
                TipMessage_Type_Light.Show("Send command failed", "", BCAppConstants.INFO_MSG);
            }
            else
            {
                TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
            }
        }
        #endregion
        public (bool isSuccess, string result) chargeControl(string chargeID, int couplerID, bool enable)
        {
            bool isSuccess = false;
            CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingReply reply;
            CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingRequest request =
                new CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingRequest();
            request.ChargeID = chargeID;
            request.CouplerID = (uint)couplerID;
            request.Enable = enable;
            try
            {
                (isSuccess, reply) = ObjConverter.ChargerInfo.chargeControl(request);
                return (isSuccess, reply.Result);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                reply = new CommonMessage.ProtocolFormat.ControllerSettingFun.ChargeSettingReply();
                reply.Result = ex.Message;
                logger.Error(ex, "Expection");
                return (isSuccess, "Expection happend");
            }
        }
        #region alarmControl
        public bool alarmUpdate(string EQPT_ID, DateTime RPT_DATE_TIME, string ALARM_CODE, string UserID, int CLASSFICATION, string REMARK,
                                uint importance_lvl, uint alarmModule)
        {
            bool isSuccess = true;
            string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (ObjConverter == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter = null");
                return false;
            }
            if (ObjConverter.BLL.AlarmBLL == null)
            {
                logger.Warn($"ObjCacheManager.{mn}: ObjConverter.BLL.SegmentBLL = null");
                return false;
            }
            try
            {
                CommonMessage.ProtocolFormat.AlarmFun.ControlRequest args = new CommonMessage.ProtocolFormat.AlarmFun.ControlRequest();
                args.EQPTID = EQPT_ID;
                args.RPTDATETIME = RPT_DATE_TIME.ToBinary();
                args.ALARMCODE = ALARM_CODE;
                args.USERID = UserID;
                args.ALARMCLASSIFICATION = (CommonMessage.ProtocolFormat.AlarmFun.alarmClassification)CLASSFICATION;
                args.ALARMREMARK = REMARK;
                args.IMPORTANCELEVEL = importance_lvl;
                args.ALARMMODULE = alarmModule;
                //呼叫BLL Method
                // app.AlarmBLL.alarmUpdateEventHandler(this, args);
                isSuccess = app.AlarmBLL.alarmUpdate(this, args);

            }
            catch (Exception ex)
            {
                isSuccess = false;

                logger.Error(ex, "Exception");
            }
            return isSuccess;
        }
        #endregion
        #endregion gRPC function
    }
}