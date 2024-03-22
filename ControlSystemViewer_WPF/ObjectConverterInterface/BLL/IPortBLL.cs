using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface IPortBLL
    {
        bool GetMonitoringPortsFromDB(out List<ViewerObject.VPORTSTATION> mPorts, out string result);
        bool GetMonitoringPorts(out List<ViewerObject.VPORTSTATION> mPorts, out string result);
        bool GetEqPorts(out List<ViewerObject.VPORTSTATION> eqPorts, out string result);
        bool UpdatePorts(ref List<ViewerObject.VPORTSTATION> ports, out string result);
        bool SetPortRun(string portID, out string result);
        bool SetPortStop(string portID, out string result);
        bool ResetPortAlarm(string portID, out string result);
        bool SetPortDir(string portID, string dir, out string result);
        bool SetPortWaitIn(string portID, out string result);
        bool SetAgvStationOpen(string portID, bool open, out string result);
    }
}
