using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface
{
    public interface IDBTableWatcher
    {
        event EventHandler UasUserChange;
        event EventHandler UasUserGroupChange;
        event EventHandler UasGroupFuncChange;
        event EventHandler UasFuncCodeChange;
        event EventHandler PortStationChange;
        event EventHandler SegmentChange;
        event EventHandler CommandChange;
        event EventHandler TransferChange;
        event EventHandler CarrierChange;
        event EventHandler AlarmChange;
        event EventHandler SectionChange;
        event EventHandler ConstantChange;
    }
}
