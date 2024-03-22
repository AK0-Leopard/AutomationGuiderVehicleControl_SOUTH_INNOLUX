using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using CommonMessage.ProtocolFormat.PortFun;
using Grpc.Core;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K21.BLL
{
    public class PortBLL : IPortBLL
    {
        private readonly string ns = "ObjectConverter_OHBC_ASE_K21.BLL" + ".PortBLL";

        private static readonly string control_web_address = "ohxcv.ha.ohxc.mirle.com.tw";
        private Channel channel = new Channel(control_web_address, 7003, ChannelCredentials.Insecure);

        private string connectionString = "";
        public PortBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public bool GetMonitoringPortsFromDB(out List<ViewerObject.VPORTSTATION> mPorts, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            mPorts = new List<ViewerObject.VPORTSTATION>();
            result = "";
            try
            {
                using (DBConnection_EF con = DBConnection_EF.GetUContext())
                {
                    if (!string.IsNullOrWhiteSpace(connectionString))
                        con.Database.Connection.ConnectionString = connectionString;

                    var query = from port in con.PortDef.AsNoTracking()
                                where port.UnitType.Trim().ToUpper() == "AGV" ||
                                      port.UnitType.Trim().ToUpper() == "NTB" ||
                                      port.UnitType.Trim().ToUpper() == "STK" ||
                                      port.UnitType.Trim().ToUpper() == "OHCV"
                                select port;
                    var portDefs = query?.ToList() ?? new List<PortDef>();
                    foreach (var portDef in portDefs)
                    {
                        var port = new ViewerObject.VPORTSTATION(portDef.PLCPortID, portDef.ADR_ID, (int)(portDef.Stage ?? 1));
                        port.IS_MONITORING = true;
                        mPorts.Add(port);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool GetMonitoringPorts(out List<ViewerObject.VPORTSTATION> mPorts, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            mPorts = new List<ViewerObject.VPORTSTATION>();
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料 - Empty
                //提出要求並回收server端給予的回應
                var reply = client.getAllPortInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.PortInfoList == null)
                {
                    result = $"{ns}.{ms} - reply.PortInfoList = null";
                    return false;
                }
                foreach (var info in reply.PortInfoList)
                {
                    var port = new ViewerObject.VPORTSTATION(info.PortID, Convert.ToString(info.ADRID), info.Stage, info.UnitType, info.ZoneName);

                    port.IS_MONITORING = true;

                    mPorts.Add(port);
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool GetEqPorts(out List<ViewerObject.VPORTSTATION> eqPorts, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            eqPorts = new List<ViewerObject.VPORTSTATION>();
            result = "";
            return true; // K21 OHT 放到 station port 再由 AGV 搬到 EQ，所以 OHBC 不管 EQ
        }

        public bool UpdatePorts(ref List<ViewerObject.VPORTSTATION> ports, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料 - Empty
                //提出要求並回收server端給予的回應
                var reply = client.getAllPortInfo(new Empty());
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                if (reply.PortInfoList == null)
                {
                    result = $"{ns}.{ms} - reply.PortInfoList = null";
                    return false;
                }
                foreach (var info in reply.PortInfoList)
                {
                    var port = ports.Where(s => s.PORT_ID == info.PortID.Trim()).FirstOrDefault();
                    if (port == null) continue;

                    port.IS_IN_SERVICE = !info.IsInService;
                    port.IS_INPUT_MODE = info.IsInputMode;

                    port.SetLocPresenceCstIDBoxID(info.LoadPosition1, info.LoadPositionCST1?.Trim() ?? "", info.LoadPositionBOX1?.Trim() ?? "",
                                                  info.LoadPosition2, info.LoadPositionCST2?.Trim() ?? "", info.LoadPositionBOX2?.Trim() ?? "",
                                                  info.LoadPosition3, info.LoadPositionCST3?.Trim() ?? "", info.LoadPositionBOX3?.Trim() ?? "",
                                                  info.LoadPosition4, info.LoadPositionCST4?.Trim() ?? "", info.LoadPositionBOX4?.Trim() ?? "",
                                                  info.LoadPosition5, info.LoadPositionCST5?.Trim() ?? "", info.LoadPositionBOX5?.Trim() ?? "",
                                                  info.LoadPosition6, /*info.LoadPositionCST6?.Trim() ?? */"", /*info.LoadPositionBOX6?.Trim() ?? */"",
                                                  info.LoadPosition7, /*info.LoadPositionCST7?.Trim() ?? */"", /*info.LoadPositionBOX7?.Trim() ?? */"",
                                                  info.CassetteID?.Trim() ?? "", info.BoxID?.Trim() ?? "");

                    #region PortData
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.RUN] = info.OpAutoMode.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.IsAutoMode] = info.IsAutoMode.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.ErrorBit] = info.OpError.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.ErrorCode] = info.ErrorCode.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.IsModeChangable] = info.IsModeChangable.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.IsInputMode] = info.IsInputMode.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.IsOutputMode] = info.IsOutputMode.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.IsReadyToLoad] = info.IsReadyToLoad.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.IsReadyToUnload] = info.IsReadyToUnload.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.PortWaitIn] = info.PortWaitIn.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.PortWaitOut] = info.PortWaitOut.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.CIM_ON] = info.CimOn.ToString();
                    port.PORT_DATA.ListPlcPortStatus[(int)ViewerObject.PortData.PlcPortStatus.PreLoadOK] = info.PreLoadOK.ToString();

                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.Remove] = info.CstRemoveCheck.ToString();
                    //port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.Remove] = "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.BCRReadDone] = info.BCRReadDone.ToString();
                    //port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.BCRReadDone] = "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.BoxID] = info.BoxID.ToString();
                    //port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.BoxID] = "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition1] = info.LoadPosition1.ToString();
                    port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition1] = info.LoadPositionBOX1 ?? "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition2] = info.LoadPosition2.ToString();
                    port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition2] = info.LoadPositionBOX2 ?? "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition3] = info.LoadPosition3.ToString();
                    port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition3] = info.LoadPositionBOX3 ?? "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition4] = info.LoadPosition4.ToString();
                    port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition4] = info.LoadPositionBOX4 ?? "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition5] = info.LoadPosition5.ToString();
                    port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition5] = info.LoadPositionBOX5 ?? "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition6] = info.LoadPosition6.ToString();
                    //port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition6] = info.LoadPositionBOX6 ?? "";
                    port.PORT_DATA.ListStageInfo[(int)ViewerObject.PortData.StageInfo.LoadPosition7] = info.LoadPosition7.ToString();
                    //port.PORT_DATA.ListStageBoxID[(int)ViewerObject.PortData.StageInfo.LoadPosition7] = info.LoadPositionBOX7 ?? "";

                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.openAGV_Station] = info.AGVStationStatus.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.openAGV_AutoPortType] = info.AGVAutoPortType.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.IsAGVMode] = info.IsAGVMode.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.IsMGVMode] = info.IsMGVMode.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.AGVPortReady] = info.AGVPortReady.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.AGVPortMismatch] = info.CSTPresenceMismatch.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.CanOpenBox] = info.CanOpenBox.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.IsBoxOpen] = info.IsBoxOpen.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.CassetteID] = info.CassetteID.ToString();
                    port.PORT_DATA.ListAgvPortSignal[(int)ViewerObject.PortData.AgvPortSignal.IsCSTPresence] = info.IsCSTPresence.ToString();
                    #endregion PortData
                }
                return true;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetPortRun(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料
                requestPortMng request = new requestPortMng();
                request.PortID = portID;
                //提出要求並回收server端給予的回應
                var reply = client.setPortRun(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                result = reply.Result ?? "";
                return reply.IsSuccess;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetPortStop(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料
                requestPortMng request = new requestPortMng();
                request.PortID = portID;
                //提出要求並回收server端給予的回應
                var reply = client.setPortStop(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                result = reply.Result ?? "";
                return reply.IsSuccess;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool ResetPortAlarm(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料
                requestPortMng request = new requestPortMng();
                request.PortID = portID;
                //提出要求並回收server端給予的回應
                var reply = client.resetPortAlarm(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                result = reply.Result ?? "";
                return reply.IsSuccess;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetPortDir(string portID, string dir, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料
                requestPortDir request = new requestPortDir();
                request.PortID = portID;
                request.PortDir = (dir ?? "").Trim().ToUpper().Contains("IN") ? portDir.In : portDir.Out;
                //提出要求並回收server端給予的回應
                var reply = client.setPortDir(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                result = reply.Result ?? "";
                return reply.IsSuccess;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetPortWaitIn(string portID, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料
                requestPortMng request = new requestPortMng();
                request.PortID = portID;
                //提出要求並回收server端給予的回應
                var reply = client.setPortWaitIn(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                result = reply.Result ?? "";
                return reply.IsSuccess;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }

        public bool SetAgvStationOpen(string portID, bool open, out string result)
        {
            string ms = System.Reflection.MethodBase.GetCurrentMethod().Name;
            result = "";
            try
            {
                //使用Proto檔並傳入連線通道來建立client端
                var client = new PortGreeter.PortGreeterClient(channel);
                //建立提出要求的資料
                requestAgvStationOpen request = new requestAgvStationOpen();
                request.PortID = portID;
                request.Open = open;
                //提出要求並回收server端給予的回應
                var reply = client.openAgvStation(request);
                if (reply == null)
                {
                    result = $"{ns}.{ms} - reply = null";
                    return false;
                }
                result = reply.Result ?? "";
                return reply.IsSuccess;
            }
            catch (Exception ex)
            {
                result = $"{ns}.{ms} - Failed, Exception: {ex.Message}";
                return false;
            }
        }
    }
}
