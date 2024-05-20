using com.mirle.ibg3k0.sc.Common;
using CommonMessage.ProtocolFormat.ControllerSettingFun;
using DocumentFormat.OpenXml.Wordprocessing;
using Grpc.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.WebAPI.Grpc.Server
{
    public class ControllerSettingFun : ControllerSettingFunGreeter.ControllerSettingFunGreeterBase
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private App.SCApplication scApp = null;
        public ControllerSettingFun(App.SCApplication _scApp)
        {
            scApp = _scApp;
        }

        public override Task<ChargeSettingReply> ChargeSetting(ChargeSettingRequest request, ServerCallContext context)
        {
            string charger_id = request.ChargeID;
            uint i_coupler_id = request.CouplerID;
            bool is_enable = request.Enable;
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(ChargeSetting)}] requset, charget ID:{charger_id} coupler ID:{i_coupler_id} is enable:{is_enable}...");
            var mtl_mapaction = scApp.getEQObjCacheManager().getUnit("MCharger", charger_id).
            getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.NorthInnolux.SubChargerValueDefMapAction)) as
            com.mirle.ibg3k0.sc.Data.ValueDefMapAction.NorthInnolux.SubChargerValueDefMapAction;
            bool is_success = mtl_mapaction.AGVCToChargerCouplerEnable(i_coupler_id, is_enable);
            ChargeSettingReply reply = new ChargeSettingReply();
            reply.Result = is_success ? "Success" : "Fail";
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(ChargeSetting)}] requset, charget ID:{charger_id} coupler ID:{i_coupler_id} is enable:{is_enable},excute result:{is_success}");
            return Task.FromResult(reply);
        }
        public override Task<ControllerParameterSettingReply> ControllerParameterSetting(ControllerParameterSettingRequest request, ServerCallContext context)
        {
            string ecid = request.ParameterID;
            string valus = request.ParameterValue;
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(ControllerParameterSetting)}] requset, ecid:{ecid} vaule:{valus} ...");
            var result = scApp.LineService.doECDataUpdate(ecid, valus);
            ControllerParameterSettingReply reply = new ControllerParameterSettingReply();
            reply.Result = result.isSuccess ? "Success" : result.result;
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(ControllerParameterSetting)}] requset, ecid:{ecid} vaule:{valus},excute result:{result.isSuccess}");
            return Task.FromResult(reply);
        }
        public override Task<ControlReply> sectionControl(ControlRequest request, ServerCallContext context)
        {
            ControlReply reply = new ControlReply();
            var service_status = request.Enable ? E_PORT_STATUS.InService : E_PORT_STATUS.OutOfService;
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(sectionControl)}] requset, section id:{request.Id} is enable:{request.Enable} ...");
            var result = scApp.VehicleService.doEnableDisableSection
            (sc.Common.SCUtility.Trim(request.Id, true), service_status);
            reply.Result = result.isSuccess ? "Success" : $"Fail,reason:{result.reason}";
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(sectionControl)}] requset, section id:{request.Id} is enable:{request.Enable},excute result:{result.isSuccess} ,reason:{result.reason}");

            return Task.FromResult(reply);
        }

        public override Task<Empty> resetBuzzer(Empty request, ServerCallContext context)
        {
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(resetBuzzer)}] requset ...");
            var Lighthouse = scApp.getEQObjCacheManager().getEquipmentByEQPTID("ColorLight");
            Lighthouse.setColorLightBuzzer(false);
            LogHelper.Log(logger: logger, LogLevel: LogLevel.Info, Class: nameof(ControllerSettingFun), Device: "AGVC",
               Data: $"Revice gRPC [{nameof(resetBuzzer)}] requset,process end");
            return Task.FromResult(new Empty());
        }

        //public override Task<ControlReply> sectionControl(ControlRequest request, ServerCallContext context)
        //{
        //    ControlReply reply = new ControlReply();
        //    var service_status = request.Enable ? E_PORT_STATUS.InService : E_PORT_STATUS.OutOfService;
        //    var result = scApp.VehicleService.doEnableDisableSection
        //    (sc.Common.SCUtility.Trim(request.Id, true), service_status);
        //    reply.Result = result.isSuccess ? "Success" : "Fail";
        //    return Task.FromResult(reply);
        //}

    }
}
