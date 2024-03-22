using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.LOG
{
    public interface IHostLogInfo
    {
        bool SetMCS_COMMU_LOG(byte[] input, out ViewerObject.LOG.MCS_COMMU_LOG log, out string result);
    }
}
