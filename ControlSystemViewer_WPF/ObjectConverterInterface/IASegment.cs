using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IASegment
    {
        bool SetVSEGMENTs(ref List<ViewerObject.VSEGMENT> segs, string data, out string result);
    }
}
