using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.LOG
{
    public interface ISystemProcInfo
    {
        bool SetSYSTEM_PROCESS_LOG(byte[] input, out ViewerObject.LOG.SYSTEM_PROCESS_LOG log, out string result);
    }
}
