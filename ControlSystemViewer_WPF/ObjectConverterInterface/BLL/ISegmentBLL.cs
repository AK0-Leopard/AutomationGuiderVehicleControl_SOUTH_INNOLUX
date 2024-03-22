using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface ISegmentBLL
    {
        bool segControl(string segID, bool enable, out string replyResult);
    }
}
