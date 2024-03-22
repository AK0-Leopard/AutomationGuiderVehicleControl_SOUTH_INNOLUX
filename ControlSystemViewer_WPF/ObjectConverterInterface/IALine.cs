using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IALine
    {
        bool SetVLINE_byString(ref ViewerObject.VLINE line, string data, out string result);
    }
}
