using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface IVehicleBLL
    {
        bool SendCmdToControl(string vh_id, string cmd_type, string carrier_id, string from_port_id, string to_port_id, out string result);
    }
}
