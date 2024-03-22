using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace ObjectConverterInterface.BLL
{
    public interface ICustomerObjBLL
    {
        MaintenanceAlarm GetMaintenanceAlarm(VALARM VAlarm, int ColNumber, List<EQMap> eqMap);
        VTRANSFER GetVTransfer(VTRANSFER vTran, VCMD vCmd);
    }
}
