using ObjectConverterInterface;
using ObjectConverterInterface.BLL;
using ObjectConverterInterface.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterFactory
{
    public class BLL
    {
        public IAlarmBLL AlarmBLL = null;
        public ICmdBLL CmdBLL = null;
        public IMapBLL MapBLL = null;
        public IUserBLL UserBLL = null;
        public IVehicleBLL VehicleBLL = null;
        public IPortBLL PortBLL = null;
        public IShelfBLL ShelfBLL = null;
        public ITrackSwitchBLL TrackSwitchBLL = null;
        public ISegmentBLL SegmentBLL = null;
        public ICustomerObjBLL CustomerObjBLL = null;
        public IConstant ConstantBLL = null;
        public IPortStationBLL PortStationBLL = null;
        //2022 09 26 for M4
        public IPLCBLL PLCBLL = null;
        public IParkZoneBLL ParkZoneBLL = null;
    }
    public class LOG
    {
        public IEqLogInfo EqLogInfo = null;
        public IHostLogInfo HostLogInfo = null;
        public ISystemProcInfo SystemProcInfo = null;
    }
    public class ObjectConverter
    {
        private readonly string ns = "ObjectConverterFactory" + ".ObjectConverter";

        public IAlarm Alarm = null;
        public IALine ALine = null;
        public IAPortStation APortStation = null;
        public IASegment ASegment = null;
        public IASection ASectionBLL = null;
        public IAVehicle AVehicle = null;
        public IChargerInfo ChargerInfo = null;
        public IDBTableWatcher DBTableWatcher = null;
        public IDefinition Definition = null;
        public ILineInfo LineInfo = null;
        public ITipMessageInfo TipMessageInfo = null;
        public IVehicleInfo VehicleInfo = null;
        public BLL BLL = new BLL();
        public LOG LOG = new LOG();
        public string LanguagePath = "Language\\";

        public ObjectConverter(string objCvt, string connectionString, bool IsControlSystemConnentable, out string result)
        {
            result = "";
            switch (objCvt)
            {
                #region AGVC
                case "AGVC_AT_S":
                    Alarm = new ObjectConverter_AGVC_AT_S.Alarm();
                    ALine = new ObjectConverter_AGVC_AT_S.ALine();
                    APortStation = new ObjectConverter_AGVC_AT_S.APortStation();
                    ASegment = new ObjectConverter_AGVC_AT_S.ASegment();
                    AVehicle = new ObjectConverter_AGVC_AT_S.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_AT_S.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_AT_S.Definition();
                    LineInfo = new ObjectConverter_AGVC_AT_S.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_AT_S.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_AT_S.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_AT_S.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_AT_S.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_AT_S.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_AT_S.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_AT_S.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_AT_S.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_AGVC_AT_S.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_AT_S.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_AT_S.LOG.SystemProcInfo();
                    break;
                case "AGVC_AUO_600":
                    Alarm = new ObjectConverter_AGVC_AUO_600.Alarm();
                    ALine = new ObjectConverter_AGVC_AUO_600.ALine();
                    APortStation = new ObjectConverter_AGVC_AUO_600.APortStation();
                    ASegment = new ObjectConverter_AGVC_AUO_600.ASegment();
                    AVehicle = new ObjectConverter_AGVC_AUO_600.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_AUO_600.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_AUO_600.Definition();
                    LineInfo = new ObjectConverter_AGVC_AUO_600.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_AUO_600.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_AUO_600.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_AUO_600.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_AUO_600.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_AUO_600.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_AUO_600.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_AUO_600.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_AUO_600.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_AGVC_AUO_600.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_AUO_600.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_AUO_600.LOG.SystemProcInfo();
                    break;
                case "AGVC_AUO_800":
                    Alarm = new ObjectConverter_AGVC_AUO_800.Alarm();
                    ALine = new ObjectConverter_AGVC_AUO_800.ALine();
                    APortStation = new ObjectConverter_AGVC_AUO_800.APortStation();
                    ASegment = new ObjectConverter_AGVC_AUO_800.ASegment();
                    AVehicle = new ObjectConverter_AGVC_AUO_800.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_AUO_800.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_AUO_800.Definition();
                    LineInfo = new ObjectConverter_AGVC_AUO_800.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_AUO_800.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_AUO_800.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_AUO_800.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_AUO_800.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_AUO_800.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_AUO_800.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_AUO_800.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_AUO_800.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_AGVC_AUO_800.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_AUO_800.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_AUO_800.LOG.SystemProcInfo();
                    break;
                case "AGVC_SOUTH_INNOLUX":
                    Alarm = new ObjectConverter_AGVC_SOUTH_INNOLUX.Alarm();
                    ALine = new ObjectConverter_AGVC_SOUTH_INNOLUX.ALine();
                    APortStation = new ObjectConverter_AGVC_SOUTH_INNOLUX.APortStation();
                    ASegment = new ObjectConverter_AGVC_SOUTH_INNOLUX.ASegment();
                    ASectionBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.ASection();
                    AVehicle = new ObjectConverter_AGVC_SOUTH_INNOLUX.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_SOUTH_INNOLUX.Definition();
                    LineInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_SOUTH_INNOLUX.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.UserBLL(connectionString);
                    BLL.SegmentBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.SegmentBLL();
                    BLL.ConstantBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.ConstantBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.PortStationBLL(connectionString);
                    BLL.CustomerObjBLL = new ObjectConverter_AGVC_SOUTH_INNOLUX.BLL.CustomerBLL();
                    LOG.EqLogInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_SOUTH_INNOLUX.LOG.SystemProcInfo();
                    LanguagePath = "Language\\SOUTH_INNOLUX\\";
                    break;

                case "AGVC_NORTH_INNOLUX":
                    Alarm = new ObjectConverter_AGVC_NORTH_INNOLUX.Alarm();
                    ALine = new ObjectConverter_AGVC_NORTH_INNOLUX.ALine();
                    APortStation = new ObjectConverter_AGVC_NORTH_INNOLUX.APortStation();
                    ASegment = new ObjectConverter_AGVC_NORTH_INNOLUX.ASegment();
                    ASectionBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.ASection();
                    AVehicle = new ObjectConverter_AGVC_NORTH_INNOLUX.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_NORTH_INNOLUX.Definition();
                    LineInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_NORTH_INNOLUX.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.UserBLL(connectionString);
                    BLL.SegmentBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.SegmentBLL();
                    BLL.ConstantBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.ConstantBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_NORTH_INNOLUX.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_NORTH_INNOLUX.LOG.SystemProcInfo();
                    LanguagePath = "Language\\SOUTH_INNOLUX\\";
                    break;

                case "AGVC_UMTC":
                    Alarm = new ObjectConverter_AGVC_UMTC.Alarm();
                    ALine = new ObjectConverter_AGVC_UMTC.ALine();
                    APortStation = new ObjectConverter_AGVC_UMTC.APortStation();
                    ASegment = new ObjectConverter_AGVC_UMTC.ASegment();
                    AVehicle = new ObjectConverter_AGVC_UMTC.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_UMTC.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_UMTC.Definition();
                    LineInfo = new ObjectConverter_AGVC_UMTC.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_UMTC.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_UMTC.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_UMTC.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_UMTC.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_UMTC.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_UMTC.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_UMTC.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_UMTC.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_AGVC_UMTC.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_UMTC.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_UMTC.LOG.SystemProcInfo();
                    break;
                case "AGVC_ASE_K21":
                    Alarm = new ObjectConverter_AGVC_ASE_K21.Alarm();
                    ALine = new ObjectConverter_AGVC_ASE_K21.ALine();
                    APortStation = new ObjectConverter_AGVC_ASE_K21.APortStation();
                    ASegment = new ObjectConverter_AGVC_ASE_K21.ASegment();
                    AVehicle = new ObjectConverter_AGVC_ASE_K21.AVehicle();
                    ChargerInfo = new ObjectConverter_AGVC_ASE_K21.ChargerInfo();
                    Definition = new ObjectConverter_AGVC_ASE_K21.Definition();
                    LineInfo = new ObjectConverter_AGVC_ASE_K21.LineInfo();
                    TipMessageInfo = new ObjectConverter_AGVC_ASE_K21.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_AGVC_ASE_K21.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_AGVC_ASE_K21.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_AGVC_ASE_K21.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_AGVC_ASE_K21.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_AGVC_ASE_K21.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_AGVC_ASE_K21.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_AGVC_ASE_K21.BLL.PortStationBLL(connectionString);
                    BLL.CustomerObjBLL = new ObjectConverter_AGVC_ASE_K21.BLL.CustomerBLL();
                    LOG.EqLogInfo = new ObjectConverter_AGVC_ASE_K21.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_AGVC_ASE_K21.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_AGVC_ASE_K21.LOG.SystemProcInfo();

                    break;
                #endregion AGVC

                #region OHBC
                case "OHBC_ASE_K21":
                    Alarm = new ObjectConverter_OHBC_ASE_K21.Alarm();
                    ALine = new ObjectConverter_OHBC_ASE_K21.ALine();
                    APortStation = new ObjectConverter_OHBC_ASE_K21.APortStation();
                    ASegment = new ObjectConverter_OHBC_ASE_K21.ASegment();
                    AVehicle = new ObjectConverter_OHBC_ASE_K21.AVehicle();
                    ChargerInfo = new ObjectConverter_OHBC_ASE_K21.ChargerInfo();
                    Definition = new ObjectConverter_OHBC_ASE_K21.Definition();
                    LineInfo = new ObjectConverter_OHBC_ASE_K21.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHBC_ASE_K21.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHBC_ASE_K21.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHBC_ASE_K21.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHBC_ASE_K21.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHBC_ASE_K21.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHBC_ASE_K21.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHBC_ASE_K21.BLL.UserBLL(connectionString);
                    BLL.PortBLL = new ObjectConverter_OHBC_ASE_K21.BLL.PortBLL(connectionString);
                    BLL.ShelfBLL = new ObjectConverter_OHBC_ASE_K21.BLL.ShelfBLL();
                    BLL.PortStationBLL = new ObjectConverter_OHBC_ASE_K21.BLL.PortStationBLL(connectionString);
                    BLL.CustomerObjBLL = new ObjectConverter_OHBC_ASE_K21.BLL.CustomerBLL();
                    LOG.EqLogInfo = new ObjectConverter_OHBC_ASE_K21.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHBC_ASE_K21.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHBC_ASE_K21.LOG.SystemProcInfo();
                    break;
                case "OHBC_ASE_K24":
                    Alarm = new ObjectConverter_OHBC_ASE_K24.Alarm();
                    ALine = new ObjectConverter_OHBC_ASE_K24.ALine();
                    APortStation = new ObjectConverter_OHBC_ASE_K24.APortStation();
                    ASegment = new ObjectConverter_OHBC_ASE_K24.ASegment();
                    AVehicle = new ObjectConverter_OHBC_ASE_K24.AVehicle();
                    ChargerInfo = new ObjectConverter_OHBC_ASE_K24.ChargerInfo();
                    Definition = new ObjectConverter_OHBC_ASE_K24.Definition();
                    LineInfo = new ObjectConverter_OHBC_ASE_K24.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHBC_ASE_K24.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHBC_ASE_K24.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHBC_ASE_K24.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHBC_ASE_K24.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHBC_ASE_K24.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHBC_ASE_K24.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHBC_ASE_K24.BLL.UserBLL(connectionString);
                    BLL.PortBLL = new ObjectConverter_OHBC_ASE_K24.BLL.PortBLL(connectionString);
                    BLL.VehicleBLL = new ObjectConverter_OHBC_ASE_K24.BLL.VehicleBLL();
                    BLL.ShelfBLL = new ObjectConverter_OHBC_ASE_K24.BLL.ShelfBLL();
                    BLL.TrackSwitchBLL = new ObjectConverter_OHBC_ASE_K24.BLL.TrackSwitchBLL();
                    BLL.SegmentBLL = new ObjectConverter_OHBC_ASE_K24.BLL.SegmentBLL();
                    BLL.CustomerObjBLL = new ObjectConverter_OHBC_ASE_K24.BLL.CustomerBLL();
                    BLL.PortStationBLL = new ObjectConverter_OHBC_ASE_K24.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_OHBC_ASE_K24.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHBC_ASE_K24.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHBC_ASE_K24.LOG.SystemProcInfo();
                    break;

                case "OHBC_PTI":
                    Alarm = new ObjectConverter_OHBC_PTI.Alarm();
                    ALine = new ObjectConverter_OHBC_PTI.ALine();
                    APortStation = new ObjectConverter_OHBC_PTI.APortStation();
                    ASegment = new ObjectConverter_OHBC_PTI.ASegment();
                    AVehicle = new ObjectConverter_OHBC_PTI.AVehicle();
                    ChargerInfo = new ObjectConverter_OHBC_PTI.ChargerInfo();
                    Definition = new ObjectConverter_OHBC_PTI.Definition();
                    LineInfo = new ObjectConverter_OHBC_PTI.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHBC_PTI.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHBC_PTI.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHBC_PTI.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHBC_PTI.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHBC_PTI.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHBC_PTI.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHBC_PTI.BLL.UserBLL(connectionString);
                    BLL.PortBLL = new ObjectConverter_OHBC_PTI.BLL.PortBLL(connectionString);
                    BLL.VehicleBLL = new ObjectConverter_OHBC_PTI.BLL.VehicleBLL();
                    BLL.ShelfBLL = new ObjectConverter_OHBC_PTI.BLL.ShelfBLL();
                    BLL.TrackSwitchBLL = new ObjectConverter_OHBC_PTI.BLL.TrackSwitchBLL();
                    BLL.SegmentBLL = new ObjectConverter_OHBC_PTI.BLL.SegmentBLL();
                    BLL.PortStationBLL = new ObjectConverter_OHBC_PTI.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_OHBC_PTI.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHBC_PTI.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHBC_PTI.LOG.SystemProcInfo();
                    break;
                #endregion OHBC

                #region OHTC
                case "OHTC_AT_S":
                    Alarm = new ObjectConverter_OHTC_AT_S.Alarm();
                    ALine = new ObjectConverter_OHTC_AT_S.ALine();
                    APortStation = new ObjectConverter_OHTC_AT_S.APortStation();
                    ASegment = new ObjectConverter_OHTC_AT_S.ASegment();
                    AVehicle = new ObjectConverter_OHTC_AT_S.AVehicle();
                    ChargerInfo = new ObjectConverter_OHTC_AT_S.ChargerInfo();
                    Definition = new ObjectConverter_OHTC_AT_S.Definition();
                    LineInfo = new ObjectConverter_OHTC_AT_S.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHTC_AT_S.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHTC_AT_S.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHTC_AT_S.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHTC_AT_S.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHTC_AT_S.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHTC_AT_S.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHTC_AT_S.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_OHTC_AT_S.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_OHTC_AT_S.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHTC_AT_S.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHTC_AT_S.LOG.SystemProcInfo();
                    break;
                #endregion OHTC
                #region OHTC
                case "OHTC_M4":
                    Alarm = new ObjectConverter_OHTC_M4.Alarm();
                    ALine = new ObjectConverter_OHTC_M4.ALine();
                    APortStation = new ObjectConverter_OHTC_M4.APortStation();
                    ASegment = new ObjectConverter_OHTC_M4.ASegment();
                    AVehicle = new ObjectConverter_OHTC_M4.AVehicle();
                    ChargerInfo = new ObjectConverter_OHTC_M4.ChargerInfo();
                    Definition = new ObjectConverter_OHTC_M4.Definition();
                    LineInfo = new ObjectConverter_OHTC_M4.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHTC_M4.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHTC_M4.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHTC_M4.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHTC_M4.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHTC_M4.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHTC_M4.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHTC_M4.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_OHTC_M4.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_OHTC_M4.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHTC_M4.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHTC_M4.LOG.SystemProcInfo();
                    //2022 09 26
                    BLL.PLCBLL = new ObjectConverter_OHTC_M4.BLL.PLCBLL(connectionString);
                    break;

                case "OHTC_2F":
                    Alarm = new ObjectConverter_OHTC_2F.Alarm();
                    ALine = new ObjectConverter_OHTC_2F.ALine();
                    APortStation = new ObjectConverter_OHTC_2F.APortStation();
                    ASegment = new ObjectConverter_OHTC_2F.ASegment();
                    AVehicle = new ObjectConverter_OHTC_2F.AVehicle();
                    ChargerInfo = new ObjectConverter_OHTC_2F.ChargerInfo();
                    Definition = new ObjectConverter_OHTC_2F.Definition();
                    LineInfo = new ObjectConverter_OHTC_2F.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHTC_2F.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHTC_2F.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHTC_2F.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHTC_2F.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHTC_2F.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHTC_2F.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHTC_2F.BLL.UserBLL(connectionString);
                    BLL.PortStationBLL = new ObjectConverter_OHTC_2F.BLL.PortStationBLL(connectionString);
                    LOG.EqLogInfo = new ObjectConverter_OHTC_2F.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHTC_2F.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHTC_2F.LOG.SystemProcInfo();
                    break;

                case "OHTC_AT_S_MALASYIA":
                    Alarm = new ObjectConverter_OHTC_AT_S_MALASYIA.Alarm();
                    ALine = new ObjectConverter_OHTC_AT_S_MALASYIA.ALine();
                    APortStation = new ObjectConverter_OHTC_AT_S_MALASYIA.APortStation();
                    ASegment = new ObjectConverter_OHTC_AT_S_MALASYIA.ASegment();
                    AVehicle = new ObjectConverter_OHTC_AT_S_MALASYIA.AVehicle();
                    ChargerInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.ChargerInfo();
                    Definition = new ObjectConverter_OHTC_AT_S_MALASYIA.Definition();
                    LineInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.LineInfo();
                    TipMessageInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.TipMessageInfo();
                    VehicleInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.VehicleInfo();
                    if (IsControlSystemConnentable) DBTableWatcher = new ObjectConverter_OHTC_AT_S_MALASYIA.DBTableWatcher(connectionString, out result);
                    BLL.AlarmBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.CmdBLL(connectionString);
                    BLL.MapBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.MapBLL(connectionString);
                    BLL.UserBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.UserBLL(connectionString);
                    BLL.SegmentBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.SegmentBLL();
                    BLL.PortStationBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.PortStationBLL(connectionString);
                    BLL.ParkZoneBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.ParkZoneBLL(connectionString);
                    BLL.ShelfBLL = new ObjectConverter_OHTC_AT_S_MALASYIA.BLL.ShelfBLL();
                    LOG.EqLogInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.LOG.EqLogInfo();
                    LOG.HostLogInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.LOG.HostLogInfo();
                    LOG.SystemProcInfo = new ObjectConverter_OHTC_AT_S_MALASYIA.LOG.SystemProcInfo();
                    break;
                #endregion OHTC

                #region STKC
                case "STKC_ASE_K21":
                    Alarm = new ObjectConverter_STKC_ASE_K21.Alarm();
                    BLL.AlarmBLL = new ObjectConverter_STKC_ASE_K21.BLL.AlarmBLL(connectionString);
                    BLL.CmdBLL = new ObjectConverter_STKC_ASE_K21.BLL.CmdBLL(connectionString);
                    BLL.CustomerObjBLL = new ObjectConverter_STKC_ASE_K21.BLL.CustomerBLL();
                    break;
                #endregion
                default:
                    result = $"{ns} - new ObjectConverter({objCvt}): Unknown Object Converter";
                    break;
            }
        }
    }
}
