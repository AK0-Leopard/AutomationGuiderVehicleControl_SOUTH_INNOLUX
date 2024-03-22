using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.LOG
{
    public interface IEqLogInfo
    {
        bool SetVEHICLE_COMMU_LOG(byte[] input, out ViewerObject.LOG.VEHICLE_COMMU_LOG log, out string result);
    }
}
