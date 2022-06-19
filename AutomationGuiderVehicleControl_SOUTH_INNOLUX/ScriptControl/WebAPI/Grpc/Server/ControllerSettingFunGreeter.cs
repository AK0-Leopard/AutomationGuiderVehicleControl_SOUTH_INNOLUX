using CommonMessage.ProtocolFormat.ControllerSettingFun;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.WebAPI.Grpc.Server
{
    public class ControllerSettingFun : ControllerSettingFunGreeter.ControllerSettingFunGreeterBase
    {
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
            var mtl_mapaction = scApp.getEQObjCacheManager().getUnit("MCharger", charger_id).
            getMapActionByIdentityKey(nameof(com.mirle.ibg3k0.sc.Data.ValueDefMapAction.NorthInnolux.SubChargerValueDefMapAction)) as
            com.mirle.ibg3k0.sc.Data.ValueDefMapAction.NorthInnolux.SubChargerValueDefMapAction;
            bool is_success = mtl_mapaction.AGVCToChargerCouplerEnable(i_coupler_id, is_enable);
            ChargeSettingReply reply = new ChargeSettingReply();
            reply.Result = is_success ? "Success" : "Fail";
            return Task.FromResult(reply);
        }
        public override Task<ControllerParameterSettingReply> ControllerParameterSetting(ControllerParameterSettingRequest request, ServerCallContext context)
        {
            string ecid = request.ParameterID;
            string valus = request.ParameterValue;
            var result = scApp.LineService.doECDataUpdate(ecid, valus);
            ControllerParameterSettingReply reply = new ControllerParameterSettingReply();
            reply.Result = result.isSuccess ? "Success" : result.result;
            return Task.FromResult(reply);
        }
        public override Task<ControlReply> sectionControl(ControlRequest request, ServerCallContext context)
        {
            ControlReply reply = new ControlReply();
            var service_status = request.Enable ? E_PORT_STATUS.InService : E_PORT_STATUS.OutOfService;
            var result = scApp.VehicleService.doEnableDisableSection
            (sc.Common.SCUtility.Trim(request.Id, true), service_status);
            reply.Result = result.isSuccess ? "Success" : "Fail";
            return Task.FromResult(reply);
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
