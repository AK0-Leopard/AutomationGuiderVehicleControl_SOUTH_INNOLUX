using CommonMessage.ProtocolFormat.SegFun;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.WebAPI.Grpc.Server
{
    public class RoadControl : segmentGreeter.segmentGreeterBase
    {
        private App.SCApplication scApp = null;
        public RoadControl(App.SCApplication _scApp)
        {
            scApp = _scApp;
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
        //public override Task<segControlReply>  (segControlRequest request, ServerCallContext context)
        //{
        //    var result = scApp.VehicleService.doEnableDisableSection
        //    (sc.Common.SCUtility.Trim(request.SegID, true), portStatus);
        //}
    }
}
