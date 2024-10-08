﻿//*********************************************************************************
//      EQType2SecsMapAction.cs
//*********************************************************************************
// File Name: EQType2SecsMapAction.cs
// Description: Type2 EQ Map Action
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
//**********************************************************************************
using com.mirle.ibg3k0.bcf.App;
using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.bcf.Controller;
using com.mirle.ibg3k0.bcf.Data.VO;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.Data.PLC_Functions;
using com.mirle.ibg3k0.sc.Data.VO;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using com.mirle.iibg3k0.ttc;
using com.mirle.iibg3k0.ttc.Common;
using com.mirle.iibg3k0.ttc.Common.TCPIP;
using KingAOP;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static com.mirle.ibg3k0.sc.Data.PLC_Functions.ChargerInterface;

namespace com.mirle.ibg3k0.sc.Data.ValueDefMapAction
{
    public class EQTcpIpMapAction : ValueDefMapActionBase, IDynamicMetaObjectProvider
    {

        string tcpipAgentName = string.Empty;
        protected Logger logger_PLCConverLog;

        public EQTcpIpMapAction()
            : base()
        {

        }
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }

        public override string getIdentityKey()
        {
            return this.GetType().Name;
        }
        //protected AVEHICLE eqpt = null;

        public override void setContext(BaseEQObject baseEQ)
        {
            this.eqpt = baseEQ as AVEHICLE;

        }
        public override void unRegisterEvent()
        {
            //not implement
        }
        public override void doShareMemoryInit(BCFAppConstants.RUN_LEVEL runLevel)
        {
            try
            {
                switch (runLevel)
                {
                    case BCFAppConstants.RUN_LEVEL.ZERO:
                        //scApp.MapBLL.addVehicle(eqpt.VEHICLE_ID);
                        //eqpt.VID_Collection.VID_70_VehicleID.VEHILCE_ID = eqpt.VEHICLE_ID;
                        if (eqpt != null)
                        {
                            if (!SCUtility.isEmpty(eqpt.OHTC_CMD))
                            {
                                ACMD_OHTC aCMD_OHTC = scApp.CMDBLL.getExcuteCMD_OHTCByCmdID(eqpt.OHTC_CMD);
                                string[] PredictPath = scApp.CMDBLL.loadPassSectionByCMDID(eqpt.OHTC_CMD);
                                List<string> segments = new List<string>();
                                string pre_segment_id = null;
                                foreach (string section_id in PredictPath)
                                {
                                    ASECTION section = scApp.SectionBLL.cache.GetSection(section_id);
                                    if (!SCUtility.isMatche(pre_segment_id, section.SEG_NUM))
                                    {
                                        segments.Add(section.SEG_NUM.Trim());
                                        pre_segment_id = section.SEG_NUM;
                                    }
                                }
                                scApp.CMDBLL.setVhExcuteCmdToShow(aCMD_OHTC, this.eqpt, segments, PredictPath, null,
                                                                  new List<string>(), new List<string>());
                            }
                        }

                        //先讓車子一開始都當作是"VehicleInstall"的狀態
                        //之後要從DB得知上次的狀態，是否為Remove
                        eqpt.VehicleInstall();

                        if (eqpt.IS_INSTALLED)
                        {
                            eqpt.TcpAgentStart(bcfApp);
                        }
                        else
                        {
                            eqpt.TcpAgentStop(bcfApp);
                        }


                        break;
                    case BCFAppConstants.RUN_LEVEL.ONE:
                        break;
                    case BCFAppConstants.RUN_LEVEL.TWO:
                        break;
                    case BCFAppConstants.RUN_LEVEL.NINE:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exection:");
            }
        }

        protected void onStateChange_Initial()
        {

        }

        protected void str102_Receive(object sender, TcpIpEventArgs e)
        {
            //Boolean resp_cmp = false;
            //STR_VHMSG_VHCL_KISO_VERSION_REP recive_str = TCPUtility._Packet2Str<STR_VHMSG_VHCL_KISO_VERSION_REP>((byte[])e.objPacket, eqpt.TcpIpAgentName);

            //STR_VHMSG_VHCL_KISO_VERSION_RESP reply_str = new STR_VHMSG_VHCL_KISO_VERSION_RESP()
            //{
            //    SeqNum = recive_str.SeqNum,
            //    PacketID = VHMSGIF.ID_VHCL_KISO_VERSION_RESPONSE,
            //    RespCode = 0
            //};

            //string vhVerionTime = new string(recive_str.VerionStr);
            //DateTime.TryParse(vhVerionTime, out eqpt.VhBasisDataVersionTime);

            //resp_cmp = ITcpIpControl.sendSecondary(bcfApp, tcpipAgentName, reply_str);
            //Console.WriteLine("Recive");

            //if (resp_cmp
            //    && eqpt.VHStateMach.CanFire(SCAppConstants.E_VH_EVENT.doDataSync))
            //{
            //    eqpt.VHStateMach.Fire(SCAppConstants.E_VH_EVENT.doDataSync);
            //    doDataSysc();
            //}
        }
        object str132_lockObj = new object();
        protected void str132_Receive(object sender, TcpIpEventArgs e)
        {
            if (scApp.getEQObjCacheManager().getLine().ServerPreStop)
                return;

            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Stopwatch sw = scApp.StopwatchPool.GetObject();
            string lockTraceInfo = string.Format("VH ID:{0},Pack ID:{1},Seq Num:{2},ThreadID:{3},"
                , eqpt.VEHICLE_ID
                , e.iPacketID
                , e.iSeqNum.ToString()
                , threadID.ToString());
            try
            {
                sw.Start();
                LogManager.GetLogger("LockInfo").Debug(string.Concat(lockTraceInfo, "Wait Lock In"));
                //TODO Wait Lock In
                SCUtility.LockWithTimeout(str132_lockObj, SCAppConstants.LOCK_TIMEOUT_MS, str132_Process, sender, e);
                //Lock Out
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("LockInfo").Error(string.Concat(lockTraceInfo, "Lock Exception"));
                logger.Error(ex, "(str132_Receive) Exception");
            }
            finally
            {
                long ElapsedMilliseconds = sw.ElapsedMilliseconds;
                LogManager.GetLogger("LockInfo").Debug(string.Concat(lockTraceInfo, "Lock Out. ElapsedMilliseconds:", ElapsedMilliseconds.ToString()));
                scApp.StopwatchPool.PutObject(sw);
            }
        }
        protected void str132_Process(object sender, TcpIpEventArgs e)
        {
            ID_132_TRANS_COMPLETE_REPORT recive_str = (ID_132_TRANS_COMPLETE_REPORT)e.objPacket;
            scApp.VehicleBLL.setAndPublishPositionReportInfo2Redis(eqpt.VEHICLE_ID, recive_str);
            dynamic service = scApp.VehicleService;
            service.CommandCompleteReport(tcpipAgentName, bcfApp, eqpt, recive_str, e.iSeqNum);
        }
        protected void str134_Receive(object sender, TcpIpEventArgs e)
        {
            if (scApp.getEQObjCacheManager().getLine().ServerPreStop)
                return;
            try
            {
                ID_134_TRANS_EVENT_REP recive_str = (ID_134_TRANS_EVENT_REP)e.objPacket;
                SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, recive_str);
                int current_seq_num = e.iSeqNum;
                int pre_position_seq_num = eqpt.PrePositionSeqNum;
                bool need_process_position = true;
                lock (eqpt.PositionRefresh_Sync)
                {
                    need_process_position = checkPositionSeqNum(current_seq_num, pre_position_seq_num);
                    eqpt.PrePositionSeqNum = current_seq_num;
                }
                if (!need_process_position)
                {
                    LogHelper.Log(logger: logger, LogLevel: LogLevel.Warn, Class: nameof(EQTcpIpMapAction), Device: Service.VehicleService.DEVICE_NAME_AGV,
                       Data: $"The vehicles updata position report of seq num is old,by pass this one.old seq num;{pre_position_seq_num},current seq num:{current_seq_num}",
                       VehicleID: eqpt.VEHICLE_ID,
                       CarrierID: eqpt.CST_ID);
                    return;
                }
                scApp.VehicleBLL.setAndPublishPositionReportInfo2Redis(eqpt.VEHICLE_ID, recive_str);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "(str134_Receive) Exception");
            }
        }
        const int TOLERANCE_SCOPE = 50;
        private const ushort SEQNUM_MAX = 999;
        private bool checkPositionSeqNum(int currnetNum, int preNum)
        {
            try
            {
                int lower_limit = preNum - TOLERANCE_SCOPE;
                if (lower_limit >= 0)
                {
                    //如果該次的Num介於上次的值減去容錯值(TOLERANCE_SCOPE = 50) 至 上次的值
                    //就代表是舊的資料
                    if (currnetNum > (lower_limit) && currnetNum < preNum)
                    {
                        return false;
                    }
                }
                else
                {
                    //如果上次的值減去容錯值變成負的，代表要再由SENDSEQNUM_MAX往回推
                    lower_limit = SEQNUM_MAX + lower_limit;
                    if (currnetNum > (lower_limit) && currnetNum < preNum)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "(checkPositionSeqNum) Exception");
                return true;
            }
        }
        object str136_lockObj = new object();
        protected void str136_Receive(object sender, TcpIpEventArgs e)
        {

            if (scApp.getEQObjCacheManager().getLine().ServerPreStop)
                return;

            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Stopwatch sw = scApp.StopwatchPool.GetObject();
            string lockTraceInfo = string.Format("VH ID:{0},Pack ID:{1},Seq Num:{2},ThreadID:{3},"
                , eqpt.VEHICLE_ID
                , e.iPacketID
                , e.iSeqNum.ToString()
                , threadID.ToString());
            try
            {
                sw.Start();
                LogManager.GetLogger("LockInfo").Debug(string.Concat(lockTraceInfo, "Wait Lock In"));
                //TODO Wait Lock In
                SCUtility.LockWithTimeout(str136_lockObj, SCAppConstants.LOCK_TIMEOUT_MS, str136_Process, sender, e);
                //Lock Out
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("LockInfo").Error(string.Concat(lockTraceInfo, "Lock Exception"));
                logger.Error(ex, "(str136_Receive) Exception");
            }
            finally
            {
                long ElapsedMilliseconds = sw.ElapsedMilliseconds;
                LogManager.GetLogger("LockInfo").Debug(string.Concat(lockTraceInfo, "Lock Out. ElapsedMilliseconds:", ElapsedMilliseconds.ToString()));
                scApp.StopwatchPool.PutObject(sw);
            }
        }
        protected void str136_Process(object sender, TcpIpEventArgs e)
        {
            //dynamic service = scApp.BlockControlServer;
            dynamic service = scApp.VehicleService;
            ID_136_TRANS_EVENT_REP recive_str = (ID_136_TRANS_EVENT_REP)e.objPacket;
            //switch (recive_str.EventType)
            //{
            //    default:
            //        scApp.VehicleBLL.setAndPublishPositionReportInfo2Redis(eqpt.VEHICLE_ID, recive_str);
            //        break;
            //}
            scApp.VehicleBLL.setAndPublishPositionReportInfo2Redis(eqpt.VEHICLE_ID, recive_str);
            service.TranEventReport(bcfApp, eqpt, recive_str, e.iSeqNum);
        }
        object str144_lockObj = new object();
        protected void str144_Receive(object sender, TcpIpEventArgs e)
        {

            if (scApp.getEQObjCacheManager().getLine().ServerPreStop)
                return;

            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Stopwatch sw = scApp.StopwatchPool.GetObject();
            string lockTraceInfo = string.Format("VH ID:{0},Pack ID:{1},Seq Num:{2},ThreadID:{3},"
                , eqpt.VEHICLE_ID
                , e.iPacketID
                , e.iSeqNum.ToString()
                , threadID.ToString());
            try
            {
                sw.Start();
                LogManager.GetLogger("LockInfo").Debug(string.Concat(lockTraceInfo, "Wait Lock In"));
                //TODO Wait Lock In
                SCUtility.LockWithTimeout(str144_lockObj, SCAppConstants.LOCK_TIMEOUT_MS, str144_Process, sender, e);
                //Lock Out
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("LockInfo").Error(string.Concat(lockTraceInfo, "Lock Exception"));
                logger.Error(ex, "(str144_Receive) Exception");
            }
            finally
            {
                long ElapsedMilliseconds = sw.ElapsedMilliseconds;
                LogManager.GetLogger("LockInfo").Debug(string.Concat(lockTraceInfo, "Lock Out. ElapsedMilliseconds:", ElapsedMilliseconds.ToString()));
                scApp.StopwatchPool.PutObject(sw);
            }
        }
        protected void str144_Process(object sender, TcpIpEventArgs e)
        {
            bool need_process_status = true;
            int pre_status_seq_num = eqpt.PreStatusSeqNum;
            int current_seq_num = e.iSeqNum;
            need_process_status = checkPositionSeqNum(current_seq_num, pre_status_seq_num);
            eqpt.PreStatusSeqNum = current_seq_num;
            if (!need_process_status)
            {
                LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(EQTcpIpMapAction), Device: Service.VehicleService.DEVICE_NAME_AGV,
                   Data: $"The vehicles updata status report of seq num is old,by pass this one.old seq num;{pre_status_seq_num},current seq num:{current_seq_num}",
                   VehicleID: eqpt.VEHICLE_ID,
                   CarrierID: eqpt.CST_ID);
                return;
            }

            dynamic service = scApp.VehicleService;
            ID_144_STATUS_CHANGE_REP recive_str = (ID_144_STATUS_CHANGE_REP)e.objPacket;
            scApp.VehicleBLL.setAndPublishPositionReportInfo2Redis(eqpt.VEHICLE_ID, recive_str);
            service.StatusReport(bcfApp, eqpt, recive_str, e.iSeqNum);
        }

        protected void str152_Receive(object sender, TcpIpEventArgs e)
        {
            ID_152_AVOID_COMPLETE_REPORT recive_str = (ID_152_AVOID_COMPLETE_REPORT)e.objPacket;
            if (checkAvoidCompletePositionInfoIsCorrect(recive_str))
            {
                scApp.VehicleBLL.setAndPublishPositionReportInfo2Redis(eqpt.VEHICLE_ID, recive_str);
            }
            else
            {
                LogHelper.Log(logger: logger, LogLevel: LogLevel.Debug, Class: nameof(EQTcpIpMapAction), Device: "AGVC",
                   Data: $"Avoid complete info not correct,by pass it",
                   VehicleID: eqpt.VEHICLE_ID,
                   CarrierID: eqpt.CST_ID);
            }
            dynamic service = scApp.VehicleService;
            service.AvoidCompleteReport(bcfApp, eqpt, recive_str, e.iSeqNum);
        }

        private bool checkAvoidCompletePositionInfoIsCorrect(ID_152_AVOID_COMPLETE_REPORT recive_str)
        {
            string current_adr = recive_str.CurrentAdrID;
            string current_sec = recive_str.CurrentSecID;
            uint sec_distance = recive_str.SecDistance;
            double x_axis = recive_str.XAxis;
            double y_axis = recive_str.YAxis;
            double direction_angle = recive_str.DirectionAngle;
            double vehicle_angle = recive_str.VehicleAngle;

            if (!SCUtility.isEmpty(current_adr) &&
               !SCUtility.isEmpty(current_sec) &&
                x_axis != 0 &&
                y_axis != 0)
            {
                return true;
            }
            return false;
        }
        protected void str162_Receive(object sender, TcpIpEventArgs e)
        {
            //Boolean resp_cmp = false;
            //STR_VHMSG_INDIVIDUAL_DOWNLOAD_REQ recive_str = TCPUtility._Packet2Str<STR_VHMSG_INDIVIDUAL_DOWNLOAD_REQ>((byte[])e.objPacket, eqpt.TcpIpAgentName);

            ////todo 修改成正確的內容
            //STR_VHMSG_INDIVIDUAL_DOWNLOAD_REP reply_str = new STR_VHMSG_INDIVIDUAL_DOWNLOAD_REP()
            //{
            //    SeqNum = recive_str.SeqNum,
            //    PacketID = VHMSGIF.ID_TRANS_EVENT_RESPONSE,
            //    OffsetAddrPos = 10000,
            //    OffsetGuideFL = 20000,
            //    OffsetGuideFR = 30000,
            //    OffsetGuideRL = 40000,
            //    OffsetGuideRR = 50000
            //};
            //resp_cmp = ITcpIpControl.sendSecondary(bcfApp, tcpipAgentName, reply_str);
            //Console.WriteLine("Recive");

            //if (resp_cmp)
            //    eqpt.VHStateMach.Fire(SCAppConstants.E_VH_EVENT.CompensationDataRep);
        }
        protected void str172_Receive(object sender, TcpIpEventArgs e)
        {
            ID_172_RANGE_TEACHING_COMPLETE_REPORT recive_gpp = (ID_172_RANGE_TEACHING_COMPLETE_REPORT)e.objPacket;
            dynamic service = scApp.VehicleService;
            service.RangeTeachingCompleteReport("", bcfApp, eqpt, recive_gpp, e.iSeqNum);
        }
        protected void str174_Receive(object sender, TcpIpEventArgs e)
        {
            ID_174_ADDRESS_TEACH_REPORT recive_gpp = (ID_174_ADDRESS_TEACH_REPORT)e.objPacket;
            dynamic service = scApp.VehicleService;
            service.AddressTeachReport(bcfApp, eqpt, recive_gpp, e.iSeqNum);
        }
        protected void str194_Receive(object sender, TcpIpEventArgs e)
        {
            ID_194_ALARM_REPORT recive_gpp = (ID_194_ALARM_REPORT)e.objPacket;

            dynamic service = scApp.VehicleService;

            service.AlarmReport(bcfApp, eqpt, recive_gpp, e.iSeqNum);

            //STR_VHMSG_ALARM_REP recive_str = TCPUtility._Packet2Str<STR_VHMSG_ALARM_REP>((byte[])e.objPacket, eqpt.TcpIpAgentName);



            //STR_VHMSG_ALARM_RESP reply_str = new STR_VHMSG_ALARM_RESP()
            //{
            //    SeqNum = recive_str.SeqNum,
            //    PacketID = VHMSGIF.ID_TRANS_EVENT_RESPONSE,
            //    RespCode = 1
            //};
            //ITcpIpControl.sendSecondary(bcfApp, tcpipAgentName, reply_str);
            //Console.WriteLine("Recive");

            //eqpt.VHStateMach.Fire(SCAppConstants.E_VH_EVENT.CompensationDataError);
        }
        public override bool sned_Str21(ID_21_CONTROL_DATA_REP send_gpp, out ID_121_CONTROL_DATA_RESPONSE receive_gpp)
        {
            bool isSuccess = false;
            try
            {

                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.ControlDataRepFieldNumber,
                    ControlDataRep = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out string reason);
                if (result == TrxTcpIp.ReturnCode.Normal)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                receive_gpp = null;
            }
            return isSuccess;
        }
        public override bool sned_Str27(ID_27_PARAMETERS_REQUEST send_gpp, out ID_127_PARAMETERS_RESPONSE receive_gpp)
        {
            bool isSuccess = false;
            try
            {

                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.ControlDataRepFieldNumber,
                    ParametersReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out string reason);
                if (result == TrxTcpIp.ReturnCode.Normal)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                receive_gpp = null;
            }
            return isSuccess;
        }
        public override bool send_Str31(ID_31_TRANS_REQUEST send_gpp, out ID_131_TRANS_RESPONSE receive_gpp, out string reason)
        {
            bool isSuccess = false;
            try
            {

                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.TransReqFieldNumber,
                    TransReq = send_gpp
                };
                //SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, send_gpp);
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out reason);
                //SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, receive_gpp, result.ToString());
                if (result == TrxTcpIp.ReturnCode.Normal)
                {
                    isSuccess = true;
                    reason = receive_gpp.NgReason;
                }
                else
                {
                    isSuccess = false;
                    reason = result.ToString();
                }
                //isSuccess = result == TrxTcpIp.ReturnCode.Normal;
                //reason = receive_gpp.NgReason;
                if (isSuccess)
                    isSuccess = receive_gpp.ReplyCode == 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                receive_gpp = null;
                reason = "命令下達時發生錯誤!";
            }
            return isSuccess;
        }
        public override (bool isSendOK, int replyCode) send_Str37(string cmd_id, CMDCancelType actType)
        {
            bool is_scuess = false;
            int reply_code = 0;
            try
            {
                //由於A00的要求，希望可以在收到命令後 走完一個Section後再進行Cancel，因此先加入此Function
                //WaitPassOneSection();

                string rtnMsg = string.Empty;
                ID_37_TRANS_CANCEL_REQUEST stSend;
                ID_137_TRANS_CANCEL_RESPONSE stRecv;
                stSend = new ID_37_TRANS_CANCEL_REQUEST()
                {
                    CmdID = cmd_id,
                    ActType = actType
                };

                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.TransCancelReqFieldNumber,
                    TransCancelReq = stSend
                };

                SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, stSend);
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out stRecv, out rtnMsg);
                SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, stRecv, result.ToString());
                is_scuess = result == TrxTcpIp.ReturnCode.Normal;
                reply_code = stRecv.ReplyCode;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return (is_scuess, reply_code);
        }
        public override bool send_Str39(ID_39_PAUSE_REQUEST send_gpp, out ID_139_PAUSE_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.PauseReqFieldNumber,
                    PauseReq = send_gpp
                };
                // SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, send_gpp);
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                //SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, receive_gpp, result.ToString());
                isScuess = result == TrxTcpIp.ReturnCode.Normal;

                //ID_39_PAUSE_REQUEST stSend;
                //ID_139_PAUSE_RESPONSE stRecv;
                //stSend = new ID_39_PAUSE_REQUEST()
                //{
                //    EventType = eventType,
                //    PauseType = pauseType
                //};


                //SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, stSend);
                //com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out stRecv, out rtnMsg);
                //SCUtility.RecodeReportInfo(eqpt.VEHICLE_ID, 0, stRecv, result.ToString());
                //isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;

        }
        public override bool send_Str41(ID_41_MODE_CHANGE_REQ send_gpp, out ID_141_MODE_CHANGE_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.ModeChangeReqFieldNumber,
                    ModeChangeReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;

        }
        public override bool send_Str43(ID_43_STATUS_REQUEST send_gpp, out ID_143_STATUS_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.StatusReqFieldNumber,
                    StatusReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;

        }

        public override bool send_Str35(ID_35_CST_ID_RENAME_REQUEST send_gpp, out ID_135_CST_ID_RENAME_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.CSTIDRenameReqFieldNumber,
                    CSTIDRenameReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;

        }
        public override bool send_Str51(ID_51_AVOID_REQUEST send_gpp, out ID_151_AVOID_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.AvoidReqFieldNumber,
                    AvoidReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;

        }
        public override bool send_Str91(ID_91_ALARM_RESET_REQUEST send_gpp, out ID_191_ALARM_RESET_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.AlarmResetReqFieldNumber,
                    AlarmResetReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;
        }
        public override bool send_Str71(ID_71_RANGE_TEACHING_REQUEST send_gpp, out ID_171_RANGE_TEACHING_RESPONSE receive_gpp)
        {
            bool isScuess = false;
            try
            {
                string rtnMsg = string.Empty;
                WrapperMessage wrapper = new WrapperMessage
                {
                    ID = WrapperMessage.RangeTeachingReqFieldNumber,
                    RangeTeachingReq = send_gpp
                };
                com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode result = snedRecv(wrapper, out receive_gpp, out rtnMsg);
                isScuess = result == TrxTcpIp.ReturnCode.Normal;
            }
            catch (Exception ex)
            {
                receive_gpp = null;
                logger.Error(ex, "Exception");
            }
            return isScuess;

        }
        public override bool sendMessage(WrapperMessage wrapper, bool isReply = false)
        {
            Boolean resp_cmp = ITcpIpControl.sendGoogleMsg(bcfApp, tcpipAgentName, wrapper, true);
            return resp_cmp;
        }

        public async Task<(TrxTcpIp.ReturnCode returnCode, TSource2 stRecv, string rtnMsg)> snedRecvAsy<TSource2>(WrapperMessage wrapper)
        {
            TSource2 st = default(TSource2);
            string msg = null;
            TrxTcpIp.ReturnCode result = await Task.Run(() => snedRecv(wrapper, out st, out msg));
            return (result, st, msg);
        }
        object sendRecv_LockObj = new object();
        public override com.mirle.iibg3k0.ttc.Common.TrxTcpIp.ReturnCode snedRecv<TSource2>(WrapperMessage wrapper, out TSource2 stRecv, out string rtnMsg)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(sendRecv_LockObj, SCAppConstants.LOCK_TIMEOUT_MS, ref lockTaken);
                if (!lockTaken)
                    throw new TimeoutException("snedRecv time out lock happen");
                return ITcpIpControl.sendRecv_Google(bcfApp, tcpipAgentName, wrapper, out stRecv, out rtnMsg);
            }
            finally
            {
                if (lockTaken) Monitor.Exit(sendRecv_LockObj);
            }
        }
        protected void ReplyTimeOutHandler(object sender, TcpIpEventArgs e)
        {
            TcpIpExceptionEventArgs excptionArg = e as TcpIpExceptionEventArgs;
            if (e == null) return;
            scApp.AlarmBLL.onMainAlarm(SCAppConstants.MainAlarmCode.VH_WAIT_REPLY_TIME_OUT_0_1_2
                                       , eqpt.VEHICLE_ID
                                       , e.iPacketID
                                       , e.iSeqNum);
        }
        protected void SendErrorHandler(object sender, TcpIpEventArgs e)
        {
            TcpIpExceptionEventArgs excptionArg = e as TcpIpExceptionEventArgs;
            if (e == null) return;

            scApp.AlarmBLL.onMainAlarm(SCAppConstants.MainAlarmCode.VH_SEND_MSG_ERROR_0_1_2
                           , eqpt.VEHICLE_ID
                           , e.iPacketID
                           , e.iSeqNum);
        }
        protected void SendRecvStateChangeHandler(object sender, TcpIpAgent.E_Msg_STS msg_satae)
        {
            eqpt.TcpIp_Msg_State = msg_satae.ToString();
        }
        public static Google.Protobuf.IMessage unPackWrapperMsg(byte[] raw_data)
        {
            WrapperMessage WarpperMsg = ToObject<WrapperMessage>(raw_data);
            return WarpperMsg;
        }
        public static T ToObject<T>(byte[] buf) where T : Google.Protobuf.IMessage<T>, new()
        {
            if (buf == null)
                return default(T);
            Google.Protobuf.MessageParser<T> parser = new Google.Protobuf.MessageParser<T>(() => new T());
            return parser.ParseFrom(buf);
        }
        private void whenObstacleFinish()
        {
            AVEHICLE vh = scApp.VehicleBLL.getVehicleByID(eqpt.VEHICLE_ID);
            if (eqpt.ObstacleStatus == VhStopSingle.StopSingleOff &&
                !SCUtility.isEmpty(vh.MCS_CMD))
            {
                double OCSTime_ms = eqpt.watchObstacleTime.ElapsedMilliseconds;
                double OCSTime_s = OCSTime_ms / 1000;
                OCSTime_s = Math.Round(OCSTime_s, 1);
                if (eqpt.HAS_CST == 0)
                {
                    scApp.SysExcuteQualityBLL.updateSysExecQity_OCSTime2SurceOnTheWay(vh.MCS_CMD, OCSTime_s);
                }
                else
                {
                    scApp.SysExcuteQualityBLL.updateSysExecQity_OCSTime2DestnOnTheWay(vh.MCS_CMD, OCSTime_s);
                }
            }
        }
        private void whenBlockFinish()
        {
            AVEHICLE vh = scApp.VehicleBLL.getVehicleByID(eqpt.VEHICLE_ID);
            if (eqpt.BlockingStatus == VhStopSingle.StopSingleOff &&
                !SCUtility.isEmpty(vh.MCS_CMD))
            {
                double BlockTime_ms = eqpt.watchBlockTime.ElapsedMilliseconds;
                double BlockTime_s = BlockTime_ms / 1000;
                BlockTime_s = Math.Round(BlockTime_s, 1);
                if (eqpt.HAS_CST == 0)
                {
                    scApp.SysExcuteQualityBLL.
                        updateSysExecQity_BlockTime2SurceOnTheWay(vh.MCS_CMD, BlockTime_s);
                }
                else
                {
                    scApp.SysExcuteQualityBLL.
                        updateSysExecQity_BlockTime2DestnOnTheWay(vh.MCS_CMD, BlockTime_s);
                }
            }
        }
        private void whenPauseFinish()
        {
            AVEHICLE vh = scApp.VehicleBLL.getVehicleByID(eqpt.VEHICLE_ID);
            if (eqpt.PauseStatus == VhStopSingle.StopSingleOff &&
                !SCUtility.isEmpty(vh.MCS_CMD))
            {
                double PauseTime_ms = eqpt.watchPauseTime.ElapsedMilliseconds;
                double PauseTime_s = PauseTime_ms / 1000;
                PauseTime_s = Math.Round(PauseTime_s, 1);
                scApp.SysExcuteQualityBLL.updateSysExecQity_PauseTime(vh.MCS_CMD, PauseTime_s);
            }
        }

        string event_id = string.Empty;
        /// <summary>
        /// Does the initialize.
        /// </summary>
        public override void doInit()
        {
            try
            {
                if (eqpt == null)
                    return;
                event_id = "EQTcpIpMapAction_" + eqpt.VEHICLE_ID;
                tcpipAgentName = eqpt.TcpIpAgentName;
                //======================================連線狀態=====================================================
                RegisteredTcpIpProcEvent();
                ITcpIpControl.addTcpIpConnectedHandler(bcfApp, tcpipAgentName, Connection);
                ITcpIpControl.addTcpIpDisconnectedHandler(bcfApp, tcpipAgentName, Disconnection);

                ITcpIpControl.addTcpIpReplyTimeOutHandler(bcfApp, tcpipAgentName, ReplyTimeOutHandler);
                ITcpIpControl.addTcpIpSendErrorHandler(bcfApp, tcpipAgentName, SendErrorHandler);
                ITcpIpControl.addSendRecvStateChangeHandler(bcfApp, tcpipAgentName, SendRecvStateChangeHandler);
                eqpt.addEventHandler(event_id
                    , BCFUtility.getPropertyName(() => eqpt.ObstacleStatus)
                    , (s1, e1) => { whenObstacleFinish(); });
                eqpt.addEventHandler(event_id
                    , BCFUtility.getPropertyName(() => eqpt.BlockingStatus)
                    , (s1, e1) => { whenBlockFinish(); });
                eqpt.addEventHandler(event_id
                    , BCFUtility.getPropertyName(() => eqpt.PauseStatus)
                    , (s1, e1) => { whenPauseFinish(); });
            }
            catch (Exception ex)
            {
                scApp.getBCFApplication().onSMAppError(0, "EQTcpIpMapAction doInit");
                logger.Error(ex, "Exection:");
            }
        }
        public override void RegisteredTcpIpProcEvent()
        {
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.BasicInfoVersionRepFieldNumber.ToString(), str102_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.AvoidCompleteRepFieldNumber.ToString(), str152_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.IndividualDownloadReqFieldNumber.ToString(), str162_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.AddressTeachRepFieldNumber.ToString(), str174_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.RangeTeachingCmpRepFieldNumber.ToString(), str172_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.AlarmRepFieldNumber.ToString(), str194_Receive);

            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.TranCmpRepFieldNumber.ToString(), str132_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.TransEventRepFieldNumber.ToString(), str134_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.ImpTransEventRepFieldNumber.ToString(), str136_Receive);
            ITcpIpControl.addTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.StatueChangeRepFieldNumber.ToString(), str144_Receive);
        }
        public override void UnRgisteredProcEvent()
        {
            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.BasicInfoVersionRepFieldNumber.ToString(), str102_Receive);
            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.IndividualDownloadReqFieldNumber.ToString(), str162_Receive);
            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.AlarmRepFieldNumber.ToString(), str194_Receive);

            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.TranCmpRepFieldNumber.ToString(), str132_Receive);
            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.TransEventRepFieldNumber.ToString(), str134_Receive);
            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.ImpTransEventRepFieldNumber.ToString(), str136_Receive);
            ITcpIpControl.removeTcpIpReceivedHandler(bcfApp, tcpipAgentName, WrapperMessage.StatueChangeRepFieldNumber.ToString(), str144_Receive);
        }
    }
}
