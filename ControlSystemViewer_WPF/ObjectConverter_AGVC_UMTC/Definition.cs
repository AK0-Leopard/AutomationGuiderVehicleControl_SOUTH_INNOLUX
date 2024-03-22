﻿using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_AGVC_UMTC
{
    public class Definition : IDefinition
    {
        public class Convert
        {
            public ViewerObject.VALARM_Def.AlarmLvl GetAlarmLevel(E_ALARM_LVL alarmLVL)
            {
                switch (alarmLVL)
                {
                    case E_ALARM_LVL.Error:
                        return ViewerObject.VALARM_Def.AlarmLvl.Error;
                    case E_ALARM_LVL.Warn:
                        return ViewerObject.VALARM_Def.AlarmLvl.Warn;
                    case E_ALARM_LVL.None:
                    default:
                        return ViewerObject.VALARM_Def.AlarmLvl.None;
                }
            }

            public ViewerObject.VLINE_Def.HostControlState GetHostControlState(com.mirle.ibg3k0.sc.App.SCAppConstants.LineHostControlState.HostControlState hostControlState)
            {
                switch (hostControlState)
                {
                    case com.mirle.ibg3k0.sc.App.SCAppConstants.LineHostControlState.HostControlState.On_Line_Remote:
                        return ViewerObject.VLINE_Def.HostControlState.On_Line_Remote;
                    case com.mirle.ibg3k0.sc.App.SCAppConstants.LineHostControlState.HostControlState.On_Line_Local:
                        return ViewerObject.VLINE_Def.HostControlState.On_Line_Local;
                    case com.mirle.ibg3k0.sc.App.SCAppConstants.LineHostControlState.HostControlState.EQ_Off_line:
                    default:
                        return ViewerObject.VLINE_Def.HostControlState.EQ_Off_line;
                }
            }

            public ViewerObject.VLINE_Def.HostControlState GetHostControlState(HostMode hostMode)
            {
                switch (hostMode)
                {
                    case HostMode.OnlineRemote:
                        return ViewerObject.VLINE_Def.HostControlState.On_Line_Remote;
                    case HostMode.OnlineLocal:
                        return ViewerObject.VLINE_Def.HostControlState.On_Line_Local;
                    case HostMode.Offline:
                    default:
                        return ViewerObject.VLINE_Def.HostControlState.EQ_Off_line;
                }
            }

            public ViewerObject.VLINE_Def.TSCState GetTSCState(ALINE.TSCState tscMode)
            {
                switch (tscMode)
                {
                    case ALINE.TSCState.AUTO:
                        return ViewerObject.VLINE_Def.TSCState.AUTO;
                    case ALINE.TSCState.PAUSING:
                        return ViewerObject.VLINE_Def.TSCState.PAUSING;
                    case ALINE.TSCState.PAUSED:
                        return ViewerObject.VLINE_Def.TSCState.PAUSED;
                    case ALINE.TSCState.TSC_INIT:
                        return ViewerObject.VLINE_Def.TSCState.TSC_INIT;
                    case ALINE.TSCState.NONE:
                    default:
                        return ViewerObject.VLINE_Def.TSCState.NONE;
                }
            }

            public ViewerObject.VLINE_Def.TSCState GetTSCState(TSCState tscMode)
            {
                switch (tscMode)
                {
                    case TSCState.Auto:
                        return ViewerObject.VLINE_Def.TSCState.AUTO;
                    case TSCState.Pausing:
                        return ViewerObject.VLINE_Def.TSCState.PAUSING;
                    case TSCState.Paused:
                        return ViewerObject.VLINE_Def.TSCState.PAUSED;
                    case TSCState.Tscint:
                        return ViewerObject.VLINE_Def.TSCState.TSC_INIT;
                    case TSCState.Tscnone:
                    default:
                        return ViewerObject.VLINE_Def.TSCState.NONE;
                }
            }

            public ViewerObject.VPORTSTATION_Def.PortStatus GetPortStatus(PortStationStatus portStationStatus)
            {
                switch (portStationStatus)
                {
                    case PortStationStatus.LoadRequest:
                        return ViewerObject.VPORTSTATION_Def.PortStatus.LoadRequest;
                    case PortStationStatus.UnloadRequest:
                        return ViewerObject.VPORTSTATION_Def.PortStatus.UnloadRequest;
                    case PortStationStatus.Wait:
                        return ViewerObject.VPORTSTATION_Def.PortStatus.Wait;
                    case PortStationStatus.Disabled:
                        return ViewerObject.VPORTSTATION_Def.PortStatus.Disabled;
                    case PortStationStatus.Down:
                    default:
                        return ViewerObject.VPORTSTATION_Def.PortStatus.Down;
                }
            }
            public PortStationStatus GetPortStationStatus(ViewerObject.VPORTSTATION_Def.PortStatus portStatus)
            {
                switch (portStatus)
                {
                    case ViewerObject.VPORTSTATION_Def.PortStatus.LoadRequest:
                        return PortStationStatus.LoadRequest;
                    case ViewerObject.VPORTSTATION_Def.PortStatus.UnloadRequest:
                        return PortStationStatus.UnloadRequest;
                    case ViewerObject.VPORTSTATION_Def.PortStatus.Wait:
                        return PortStationStatus.Wait;
                    case ViewerObject.VPORTSTATION_Def.PortStatus.Disabled:
                        return PortStationStatus.Disabled;
                    default:
                        return PortStationStatus.Down;
                }
            }
            public PortStationServiceStatus GetPortStationServiceStatus(ViewerObject.VPORTSTATION_Def.PortServiceStatus portServiceStatus)
            {
                switch (portServiceStatus)
                {
                    case ViewerObject.VPORTSTATION_Def.PortServiceStatus.InService:
                        return PortStationServiceStatus.InService;
                    case ViewerObject.VPORTSTATION_Def.PortServiceStatus.OutOfService:
                        return PortStationServiceStatus.OutOfService;
                    default:
                        return PortStationServiceStatus.NoDefinition;
                }
            }

            public ViewerObject.VTIPMESSAGE_Def.MsgLevel GetMsgLevel(MsgLevel msgLevel)
            {
                switch (msgLevel)
                {
                    case MsgLevel.Error:
                        return ViewerObject.VTIPMESSAGE_Def.MsgLevel.Error;
                    case MsgLevel.Warn:
                        return ViewerObject.VTIPMESSAGE_Def.MsgLevel.Warn;
                    case MsgLevel.Info:
                    default:
                        return ViewerObject.VTIPMESSAGE_Def.MsgLevel.Info;
                }
            }

            public ViewerObject.VVEHICLE_Def.ModeStatus GetModeStatus(VHModeStatus vHModeStatus)
            {
                switch (vHModeStatus)
                {
                    case VHModeStatus.AutoCharging:
                        return ViewerObject.VVEHICLE_Def.ModeStatus.AutoCharging;
                    case VHModeStatus.AutoZoneChange:
                        return ViewerObject.VVEHICLE_Def.ModeStatus.AutoZoneChange;
                    case VHModeStatus.AutoRemote:
                        return ViewerObject.VVEHICLE_Def.ModeStatus.AutoRemote;
                    case VHModeStatus.AutoLocal:
                        return ViewerObject.VVEHICLE_Def.ModeStatus.AutoLocal;
                    case VHModeStatus.Manual:
                        return ViewerObject.VVEHICLE_Def.ModeStatus.Manual;
                    case VHModeStatus.None:
                    default:
                        return ViewerObject.VVEHICLE_Def.ModeStatus.None;
                }
            }
            public VHModeStatus GetVHModeStatus(ViewerObject.VVEHICLE_Def.ModeStatus modeStatus)
            {
                switch (modeStatus)
                {
                    case ViewerObject.VVEHICLE_Def.ModeStatus.AutoCharging:
                        return VHModeStatus.AutoCharging;
                    case ViewerObject.VVEHICLE_Def.ModeStatus.AutoZoneChange:
                        return VHModeStatus.AutoZoneChange;
                    case ViewerObject.VVEHICLE_Def.ModeStatus.AutoRemote:
                        return VHModeStatus.AutoRemote;
                    case ViewerObject.VVEHICLE_Def.ModeStatus.AutoLocal:
                        return VHModeStatus.AutoLocal;
                    case ViewerObject.VVEHICLE_Def.ModeStatus.Manual:
                        return VHModeStatus.Manual;
                    default:
                        return VHModeStatus.None;
                }
            }

            public ViewerObject.VVEHICLE_Def.ActionStatus GetActionStatus(VHActionStatus vHActionStatus)
            {
                switch (vHActionStatus)
                {
                    case VHActionStatus.CycleRun:
                        return ViewerObject.VVEHICLE_Def.ActionStatus.CycleRun;
                    case VHActionStatus.GripperTeaching:
                        return ViewerObject.VVEHICLE_Def.ActionStatus.GripperTeaching;
                    case VHActionStatus.Teaching:
                        return ViewerObject.VVEHICLE_Def.ActionStatus.Teaching;
                    case VHActionStatus.Commanding:
                        return ViewerObject.VVEHICLE_Def.ActionStatus.Commanding;
                    case VHActionStatus.NoCommand:
                    default:
                        return ViewerObject.VVEHICLE_Def.ActionStatus.NoCommand;
                }
            }

            public ViewerObject.VCMD_Def.CmdType GetCmdType(E_CMD_TYPE? cmd_type)
            {
                switch (cmd_type)
                {
                    case E_CMD_TYPE.Override:
                        return ViewerObject.VCMD_Def.CmdType.Override;
                    case E_CMD_TYPE.MTLHome:
                        return ViewerObject.VCMD_Def.CmdType.MTLHome;
                    case E_CMD_TYPE.Home:
                        return ViewerObject.VCMD_Def.CmdType.Home;
                    case E_CMD_TYPE.Round:
                        return ViewerObject.VCMD_Def.CmdType.Round;
                    case E_CMD_TYPE.Continue:
                        return ViewerObject.VCMD_Def.CmdType.Continue;
                    case E_CMD_TYPE.Teaching:
                        return ViewerObject.VCMD_Def.CmdType.Teaching;
                    case E_CMD_TYPE.LoadUnload:
                        return ViewerObject.VCMD_Def.CmdType.LoadUnload;
                    case E_CMD_TYPE.Unload:
                        return ViewerObject.VCMD_Def.CmdType.Unload;
                    case E_CMD_TYPE.Load:
                        return ViewerObject.VCMD_Def.CmdType.Load;
                    case E_CMD_TYPE.Move_Teaching:
                        return ViewerObject.VCMD_Def.CmdType.MoveTeaching;
                    //case E_CMD_TYPE.MoveMtport:
                    //    return ViewerObject.VCMD_Def.CmdType.MoveMtport;
                    case E_CMD_TYPE.Move_Park:
                        return ViewerObject.VCMD_Def.CmdType.MovePark;
                    case E_CMD_TYPE.Move_Charger:
                    case E_CMD_TYPE.Move:
                        return ViewerObject.VCMD_Def.CmdType.Move;
                    default:
                        return ViewerObject.VCMD_Def.CmdType.Undefined;
                }
            }
            public ViewerObject.VCMD_Def.CmdType GetCmdTypeInt(int? cmd_type)
            {
                switch (cmd_type)
                {
                    case (int)E_CMD_TYPE.Override:
                        return ViewerObject.VCMD_Def.CmdType.Override;
                    case (int)E_CMD_TYPE.MTLHome:
                        return ViewerObject.VCMD_Def.CmdType.MTLHome;
                    case (int)E_CMD_TYPE.Home:
                        return ViewerObject.VCMD_Def.CmdType.Home;
                    case (int)E_CMD_TYPE.Round:
                        return ViewerObject.VCMD_Def.CmdType.Round;
                    case (int)E_CMD_TYPE.Continue:
                        return ViewerObject.VCMD_Def.CmdType.Continue;
                    case (int)E_CMD_TYPE.Teaching:
                        return ViewerObject.VCMD_Def.CmdType.Teaching;
                    case (int)E_CMD_TYPE.LoadUnload:
                        return ViewerObject.VCMD_Def.CmdType.LoadUnload;
                    case (int)E_CMD_TYPE.Unload:
                        return ViewerObject.VCMD_Def.CmdType.Unload;
                    case (int)E_CMD_TYPE.Load:
                        return ViewerObject.VCMD_Def.CmdType.Load;
                    case (int)E_CMD_TYPE.Move_Teaching:
                        return ViewerObject.VCMD_Def.CmdType.MoveTeaching;
                    //case E_CMD_TYPE.MoveMtport:
                    //    return ViewerObject.VCMD_Def.CmdType.MoveMtport;
                    case (int)E_CMD_TYPE.Move_Park:
                        return ViewerObject.VCMD_Def.CmdType.MovePark;
                    case (int)E_CMD_TYPE.Move_Charger:
                    case (int)E_CMD_TYPE.Move:
                        return ViewerObject.VCMD_Def.CmdType.Move;
                    default:
                        return ViewerObject.VCMD_Def.CmdType.Undefined;
                }
            }
            public E_CMD_TYPE GetE_CMD_TYPE(ViewerObject.VCMD_Def.CmdType cmdType)
            {
                switch (cmdType)
                {
                    case ViewerObject.VCMD_Def.CmdType.Override:
                        return E_CMD_TYPE.Override;
                    case ViewerObject.VCMD_Def.CmdType.MTLHome:
                        return E_CMD_TYPE.MTLHome;
                    case ViewerObject.VCMD_Def.CmdType.Home:
                        return E_CMD_TYPE.Home;
                    case ViewerObject.VCMD_Def.CmdType.Round:
                        return E_CMD_TYPE.Round;
                    case ViewerObject.VCMD_Def.CmdType.Continue:
                        return E_CMD_TYPE.Continue;
                    case ViewerObject.VCMD_Def.CmdType.Teaching:
                        return E_CMD_TYPE.Teaching;
                    case ViewerObject.VCMD_Def.CmdType.LoadUnload:
                        return E_CMD_TYPE.LoadUnload;
                    case ViewerObject.VCMD_Def.CmdType.Unload:
                        return E_CMD_TYPE.Unload;
                    case ViewerObject.VCMD_Def.CmdType.Load:
                        return E_CMD_TYPE.Load;
                    case ViewerObject.VCMD_Def.CmdType.MoveTeaching:
                        return E_CMD_TYPE.Move_Teaching;
                    case ViewerObject.VCMD_Def.CmdType.MoveCharger:
                        return E_CMD_TYPE.Move_Charger;
                    case ViewerObject.VCMD_Def.CmdType.MovePark:
                        return E_CMD_TYPE.Move_Park;
                    case ViewerObject.VCMD_Def.CmdType.Move:
                    default:
                        return E_CMD_TYPE.Move;
                }
            }

            public ViewerObject.VCMD_Def.CmdType GetCmdType(CommandType commandType)
            {
                switch (commandType)
                {
                    case CommandType.CmdOverride:
                        return ViewerObject.VCMD_Def.CmdType.Override;
                    case CommandType.CmdHome:
                        return ViewerObject.VCMD_Def.CmdType.Home;
                    case CommandType.CmdRound:
                        return ViewerObject.VCMD_Def.CmdType.Round;
                    case CommandType.CmdContinue:
                        return ViewerObject.VCMD_Def.CmdType.Continue;
                    case CommandType.CmdTeaching:
                        return ViewerObject.VCMD_Def.CmdType.Teaching;
                    case CommandType.CmdLoadUnload:
                        return ViewerObject.VCMD_Def.CmdType.LoadUnload;
                    case CommandType.CmdUnload:
                        return ViewerObject.VCMD_Def.CmdType.Unload;
                    case CommandType.CmdLoad:
                        return ViewerObject.VCMD_Def.CmdType.Load;
                    case CommandType.CmdMoveMtport:
                        return ViewerObject.VCMD_Def.CmdType.MoveMtport;
                    case CommandType.CmdMovePark:
                        return ViewerObject.VCMD_Def.CmdType.MovePark;
                    case CommandType.CmdMove:
                        return ViewerObject.VCMD_Def.CmdType.Move;
                    default:
                        return ViewerObject.VCMD_Def.CmdType.Undefined;
                }
            }

            public ViewerObject.VCMD_Def.CmdStatus GetCmdStatus(E_CMD_STATUS? cmd_status)
            {
                switch (cmd_status)
                {
                    case E_CMD_STATUS.CancelEndByOHTC:
                        return ViewerObject.VCMD_Def.CmdStatus.CancelEndByControlSystem;
                    case E_CMD_STATUS.AbnormalEndByOHTC:
                        return ViewerObject.VCMD_Def.CmdStatus.AbnormalEndByControlSystem;
                    case E_CMD_STATUS.AbnormalEndByMCS:
                        return ViewerObject.VCMD_Def.CmdStatus.AbnormalEndByMCS;
                    case E_CMD_STATUS.AbnormalEndByOHT:
                        return ViewerObject.VCMD_Def.CmdStatus.AbnormalEndByVehicle;
                    case E_CMD_STATUS.NormalEnd:
                        return ViewerObject.VCMD_Def.CmdStatus.NormalEnd;
                    case E_CMD_STATUS.Canceling:
                        return ViewerObject.VCMD_Def.CmdStatus.Canceling;
                    case E_CMD_STATUS.Aborting:
                        return ViewerObject.VCMD_Def.CmdStatus.Aborting;
                    case E_CMD_STATUS.Execution:
                        return ViewerObject.VCMD_Def.CmdStatus.Execution;
                    case E_CMD_STATUS.Sending:
                        return ViewerObject.VCMD_Def.CmdStatus.Sending;
                    case E_CMD_STATUS.Queue:
                        return ViewerObject.VCMD_Def.CmdStatus.Queue;
                    default:
                        return ViewerObject.VCMD_Def.CmdStatus.Undefined;
                }
            }
            public ViewerObject.VCMD_Def.CmdStatus GetCmdStatusInt(int? cmd_status)
            {
                switch (cmd_status)
                {
                    case (int)E_CMD_STATUS.CancelEndByOHTC:
                        return ViewerObject.VCMD_Def.CmdStatus.CancelEndByControlSystem;
                    case (int)E_CMD_STATUS.AbnormalEndByOHTC:
                        return ViewerObject.VCMD_Def.CmdStatus.AbnormalEndByControlSystem;
                    case (int)E_CMD_STATUS.AbnormalEndByMCS:
                        return ViewerObject.VCMD_Def.CmdStatus.AbnormalEndByMCS;
                    case (int)E_CMD_STATUS.AbnormalEndByOHT:
                        return ViewerObject.VCMD_Def.CmdStatus.AbnormalEndByVehicle;
                    case (int)E_CMD_STATUS.NormalEnd:
                        return ViewerObject.VCMD_Def.CmdStatus.NormalEnd;
                    case (int)E_CMD_STATUS.Canceling:
                        return ViewerObject.VCMD_Def.CmdStatus.Canceling;
                    case (int)E_CMD_STATUS.Aborting:
                        return ViewerObject.VCMD_Def.CmdStatus.Aborting;
                    case (int)E_CMD_STATUS.Execution:
                        return ViewerObject.VCMD_Def.CmdStatus.Execution;
                    case (int)E_CMD_STATUS.Sending:
                        return ViewerObject.VCMD_Def.CmdStatus.Sending;
                    case (int)E_CMD_STATUS.Queue:
                        return ViewerObject.VCMD_Def.CmdStatus.Queue;
                    default:
                        return ViewerObject.VCMD_Def.CmdStatus.Undefined;
                }
            }

            public PauseType GetPauseType(ViewerObject.VVEHICLE_Def.VPauseType pauseType)
            {
                switch (pauseType)
                {
                    case ViewerObject.VVEHICLE_Def.VPauseType.All:
                        return PauseType.All;
                    case ViewerObject.VVEHICLE_Def.VPauseType.Safety:
                        return PauseType.Safety;
                    case ViewerObject.VVEHICLE_Def.VPauseType.EarthQuake:
                        return PauseType.EarthQuake;
                    case ViewerObject.VVEHICLE_Def.VPauseType.Block:
                        return PauseType.Block;
                    case ViewerObject.VVEHICLE_Def.VPauseType.Normal:
                        return PauseType.Normal;
                    default:
                        return PauseType.None;
                }
            }

            public PauseEvent GetPauseEvent(ViewerObject.VVEHICLE_Def.VPauseEvent pauseEvent)
            {
                switch (pauseEvent)
                {
                    case ViewerObject.VVEHICLE_Def.VPauseEvent.Pause:
                        return PauseEvent.Pause;
                    default:
                        return PauseEvent.Continue;
                }
            }

            public ViewerObject.Coupler_Def.CouplerStatus GetCouplerStatus(SCAppConstants.CouplerStatus couplerStatus)
            {
                switch (couplerStatus)
                {
                    case SCAppConstants.CouplerStatus.Error:
                        return ViewerObject.Coupler_Def.CouplerStatus.Error;
                    case SCAppConstants.CouplerStatus.Charging:
                        return ViewerObject.Coupler_Def.CouplerStatus.Charging;
                    case SCAppConstants.CouplerStatus.Auto:
                        return ViewerObject.Coupler_Def.CouplerStatus.Auto;
                    case SCAppConstants.CouplerStatus.Manual:
                        return ViewerObject.Coupler_Def.CouplerStatus.Manual;
                    default:
                        return ViewerObject.Coupler_Def.CouplerStatus.None;
                }
            }

            public ViewerObject.Coupler_Def.CouplerHPSafety GetCouplerHPSafety(SCAppConstants.CouplerHPSafety couplerHPSafety)
            {
                switch (couplerHPSafety)
                {
                    case SCAppConstants.CouplerHPSafety.Safyte:
                        return ViewerObject.Coupler_Def.CouplerHPSafety.Safyte;
                    default:
                        return ViewerObject.Coupler_Def.CouplerHPSafety.NonSafety;
                }
            }

            public ViewerObject.VTRANSFER_Def.CommandState GetCommandStateInt(int input)
            {
                switch (input)
                {
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.EnRoute:
                        return ViewerObject.VTRANSFER_Def.CommandState.EnRoute;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.LoadArrive:
                        return ViewerObject.VTRANSFER_Def.CommandState.LoadArrive;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.Loading:
                        return ViewerObject.VTRANSFER_Def.CommandState.Loading;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.LoadComplete:
                        return ViewerObject.VTRANSFER_Def.CommandState.LoadComplete;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.UnloadArrive:
                        return ViewerObject.VTRANSFER_Def.CommandState.UnloadArrive;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.Unloading:
                        return ViewerObject.VTRANSFER_Def.CommandState.Unloading;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.UnloadComplete:
                        return ViewerObject.VTRANSFER_Def.CommandState.UnloadComplete;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.CommandFinish:
                        return ViewerObject.VTRANSFER_Def.CommandState.CommandFinish;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.Error_DoubleStorage:
                        return ViewerObject.VTRANSFER_Def.CommandState.Error_DoubleStorage;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.Error_EmptyRetrieval:
                        return ViewerObject.VTRANSFER_Def.CommandState.Error_EmptyRetrieval;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.Error_InterlockError:
                        return ViewerObject.VTRANSFER_Def.CommandState.Error_InterlockError;
                    case (int)ViewerObject.VTRANSFER_Def.CommandState.Error_VehicleAbort:
                        return ViewerObject.VTRANSFER_Def.CommandState.Error_VehicleAbort;
                    default:
                        return ViewerObject.VTRANSFER_Def.CommandState.Undefined;
                }
            }

            public ViewerObject.VCMD_Def.OHTCCompleteStatus? GetOHTCCompleteStatus(CompleteStatus? input)
            {
                switch (input)
                {
                    case CompleteStatus.Move:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusMove;
                    case CompleteStatus.Load:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusLoad;
                    case CompleteStatus.Unload:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusUnload;
                    case CompleteStatus.Loadunload:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusLoadunload;
                    case CompleteStatus.Cancel:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusCancel;
                    case CompleteStatus.Abort:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusAbort;
                    case CompleteStatus.VehicleAbort:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusVehicleAbort;
                    case CompleteStatus.IdmisMatch:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusIdmisMatch;
                    case CompleteStatus.IdreadFailed:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusIdreadFailed;
                    case CompleteStatus.InterlockError:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusInterlockError;
                    case CompleteStatus.CommandInitailFail:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusCommandInitailFail;
                    case CompleteStatus.LongTimeInaction:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusLongTimeInaction;
                    case CompleteStatus.ForceAbnormalFinishByOp:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusForceAbnormalFinishByOp;
                    case CompleteStatus.ForceNormalFinishByOp:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusForceNormalFinishByOp;
                    default:
                        return null;
                }
            }

            public ViewerObject.VCMD_Def.OHTCCompleteStatus? GetOHTCCompleteStatusInt(int? input)
            {
                switch (input)
                {
                    case (int)CompleteStatus.Move:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusMove;
                    case (int)CompleteStatus.Load:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusLoad;
                    case (int)CompleteStatus.Unload:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusUnload;
                    case (int)CompleteStatus.Loadunload:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusLoadunload;
                    case (int)CompleteStatus.Cancel:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusCancel;
                    case (int)CompleteStatus.Abort:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusAbort;
                    case (int)CompleteStatus.VehicleAbort:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusVehicleAbort;
                    case (int)CompleteStatus.IdmisMatch:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusIdmisMatch;
                    case (int)CompleteStatus.IdreadFailed:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusIdreadFailed;
                    case (int)CompleteStatus.InterlockError:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusInterlockError;
                    case (int)CompleteStatus.CommandInitailFail:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusCommandInitailFail;
                    case (int)CompleteStatus.LongTimeInaction:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusLongTimeInaction;
                    case (int)CompleteStatus.ForceAbnormalFinishByOp:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusForceAbnormalFinishByOp;
                    case (int)CompleteStatus.ForceNormalFinishByOp:
                        return ViewerObject.VCMD_Def.OHTCCompleteStatus.CmpStatusForceNormalFinishByOp;
                    default:
                        return null;
                }
            }
        }

        Convert convert = new Convert();
        #region GetString
        public string GetString(ViewerObject.VCMD_Def.CmdType cmdType) => convert.GetE_CMD_TYPE(cmdType).ToString();
        public string GetString(ViewerObject.VVEHICLE_Def.VPauseType pauseType) => convert.GetPauseType(pauseType).ToString();
        public string GetString(ViewerObject.VVEHICLE_Def.VPauseEvent pauseEvent) => convert.GetPauseEvent(pauseEvent).ToString();
        public string GetString(ViewerObject.VVEHICLE_Def.ModeStatus modeStatus) => convert.GetVHModeStatus(modeStatus).ToString();
        public string GetString(ViewerObject.VPORTSTATION_Def.PortStatus portStatus) => convert.GetPortStationStatus(portStatus).ToString();
        public string GetString(ViewerObject.VPORTSTATION_Def.PortServiceStatus portServiceStatus) => convert.GetPortStationServiceStatus(portServiceStatus).ToString();
        #endregion GetString
    }
}
