using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface ILineInfo
    {
        bool SetVLINE_by_LINE_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result);
        bool SetVLINE_by_CONNECTION_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result);
        bool SetVLINE_by_TRANSFER_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result);
        bool SetVLINE_by_ONLINE_CHECK_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result);
        bool SetVLINE_by_PING_CHECK_INFO(ref ViewerObject.VLINE line, byte[] bytes, out string result);
    }
}
