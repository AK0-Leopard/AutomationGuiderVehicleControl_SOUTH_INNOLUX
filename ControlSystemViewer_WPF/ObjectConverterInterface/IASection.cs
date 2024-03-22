using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IASection
    {
        bool SetSections(ref List<ViewerObject.Section> secs, string data, out string result);
    }
}
