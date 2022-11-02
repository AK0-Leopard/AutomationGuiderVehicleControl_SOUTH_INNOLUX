//*********************************************************************************
//      TrafficLightValueDefMapAction.cs
//*********************************************************************************
// File Name: TrafficLightValueDefMapAction.cs
// Description: 
//
//(c) Copyright 2018, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
//**********************************************************************************
using com.mirle.ibg3k0.bcf.App;
using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.bcf.Common.MPLC;
using com.mirle.ibg3k0.bcf.Controller;
using com.mirle.ibg3k0.bcf.Data.ValueDefMapAction;
using com.mirle.ibg3k0.bcf.Data.VO;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.Data.PLC_Functions;
using com.mirle.ibg3k0.sc.Data.PLC_Functions.SouthInnolux;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using KingAOP;
using NLog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace com.mirle.ibg3k0.sc.Data.ValueDefMapAction
{
    public class QuakeSensorValueDefMapAction : IValueDefMapAction
    {
        public string DEVICE_NAME_QUAKE_SENSOR = "QuakeSensor";
        Logger logger = NLog.LogManager.GetCurrentClassLogger();
        AEQPT eqpt = null;
        protected SCApplication scApp = null;
        protected BCFApplication bcfApp = null;
        public QuakeSensorValueDefMapAction()
            : base()
        {
            scApp = SCApplication.getInstance();
            bcfApp = scApp.getBCFApplication();
        }
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }

        public virtual string getIdentityKey()
        {
            return this.GetType().Name;
        }
        public virtual void setContext(BaseEQObject baseEQ)
        {
            this.eqpt = baseEQ as AEQPT;

        }
        public virtual void unRegisterEvent()
        {
            //not implement
        }
        public virtual void doShareMemoryInit(BCFAppConstants.RUN_LEVEL runLevel)
        {
            try
            {
                switch (runLevel)
                {
                    case BCFAppConstants.RUN_LEVEL.ZERO:
                        QuakeSignalChange(null, null);
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

        object quake_signal_change_loc = new object();
        private void QuakeSignalChange(object sender, ValueChangedEventArgs e)
        {
            var function = scApp.getFunBaseObj<QuakeSensorSignal>(eqpt.EQPT_ID) as QuakeSensorSignal;
            try
            {
                //1.建立各個Function物件
                function.Read(bcfApp, eqpt.EqptObjectCate, eqpt.EQPT_ID);
                //2.read log
                function.Timestamp = DateTime.Now;
                LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(QuakeSensorValueDefMapAction), Device: DEVICE_NAME_QUAKE_SENSOR,
                         Data: function.ToString());
                var line = scApp.getEQObjCacheManager().getLine();
                lock (quake_signal_change_loc)
                {
                    if (function.QuakeSensorSingnal != line.IsEarthquakeHappend)
                    {
                        line.IsEarthquakeHappend = function.QuakeSensorSingnal;
                        var vhs = scApp.VehicleBLL.cache.loadAllVh();
                        if (function.QuakeSensorSingnal)
                        {
                            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(QuakeSensorValueDefMapAction), Device: DEVICE_NAME_QUAKE_SENSOR,
                                     Data: "Earthquake happend,start pause vh and force finish charger charging...");

                            line.IsEarthquakeHappend = function.QuakeSensorSingnal;
                            //呼叫車子暫停
                            foreach (var vh in vhs)
                            {
                                //scApp.VehicleService.PauseRequest(v.VEHICLE_ID, PauseEvent.Pause, SCAppConstants.OHxCPauseType.Normal);
                                if (!SCUtility.isEmpty(vh))
                                {
                                    LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(QuakeSensorValueDefMapAction), Device: DEVICE_NAME_QUAKE_SENSOR,
                                             Data: $"send cancel cmd, vh:{vh.VEHICLE_ID} cmd id:{vh.OHTC_CMD} cancel type:{CMDCancelType.CmdEms}...");
                                    bool result = scApp.VehicleService.doAbortCommand(vh, vh.OHTC_CMD, CMDCancelType.CmdEms);
                                    LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(QuakeSensorValueDefMapAction), Device: DEVICE_NAME_QUAKE_SENSOR,
                                             Data: $"send cancel cmd, vh:{vh.VEHICLE_ID} cmd id:{vh.OHTC_CMD} cancel type:{CMDCancelType.CmdEms},result:{result}");
                                }
                            }
                            //呼叫充電站斷充電
                            var mtl_mapaction = scApp.getEQObjCacheManager().getEquipmentByEQPTID("MCharger").
                                getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction)) as
                                com.mirle.ibg3k0.sc.Data.ValueDefMapAction.ChargerValueDefMapAction;
                            mtl_mapaction.AGVCToMChargerAllChargerChargingFinish();
                            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(QuakeSensorValueDefMapAction), Device: DEVICE_NAME_QUAKE_SENSOR,
                                     Data: "Earthquake happend,end pause vh and force finish charger charging ");
                            
                            scApp.LineService.ProcessAlarmReport(eqpt.EQPT_ID, BLL.AlarmBLL.EarthquakeIsHappening, ErrorStatus.ErrSet, $"Earthquake is happening");
                            BCFApplication.onWarningMsg($"Earthquake is happening");

                        }
                        else
                        {
                            scApp.LineService.ProcessAlarmReport(eqpt.EQPT_ID, BLL.AlarmBLL.EarthquakeIsHappening, ErrorStatus.ErrReset, $"Earthquake is happening");

                            //foreach (var v in vhs)
                            //{
                            //    scApp.VehicleService.PauseRequest(v.VEHICLE_ID, PauseEvent.Continue, SCAppConstants.OHxCPauseType.Normal);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                scApp.putFunBaseObj<QuakeSensorSignal>(function);
            }
        }
        string event_id = string.Empty;
        /// <summary>
        /// Does the initialize.
        /// </summary>
        public virtual void doInit()
        {
            try
            {
                ValueRead vr = null;
                if (bcfApp.tryGetReadValueEventstring(eqpt.EqptObjectCate, eqpt.EQPT_ID, "QUAKE_SENSOR_SIGNAL", out vr))
                {
                    vr.afterValueChange += (_sender, _e) => QuakeSignalChange(_sender, _e);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exection:");
            }

        }

    }
}
