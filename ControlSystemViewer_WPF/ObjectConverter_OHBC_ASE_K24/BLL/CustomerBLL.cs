using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace ObjectConverter_OHBC_ASE_K24.BLL
{
    public  class CustomerBLL : ICustomerObjBLL
    {
        public  MaintenanceAlarm GetMaintenanceAlarm(VALARM VAlarm, int ColNumber, List<EQMap> eqMap) 
        {
            return  new K24MaintenanceAlarm(VAlarm, ColNumber, eqMap);
        }
        public VTRANSFER GetVTransfer(VTRANSFER vTran, VCMD vCmd)
        {
            vTran.setVCMD(vCmd);
            return vTran;
        }

    }
}
