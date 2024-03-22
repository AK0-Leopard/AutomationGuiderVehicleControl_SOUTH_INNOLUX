using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface ITrackSwitchBLL
    {
        bool UpdateTrackSwitches(ref List<ViewerObject.TrackSwitch> trackSwitches, out string result);
        bool ResetTrackSwitchByID(string id, out string result);
    }
}
