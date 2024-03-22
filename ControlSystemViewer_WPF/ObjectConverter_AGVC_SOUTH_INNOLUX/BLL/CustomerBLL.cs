using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace ObjectConverter_AGVC_SOUTH_INNOLUX.BLL
{
    public class CustomerBLL : ICustomerObjBLL
    {
        public MaintenanceAlarm GetMaintenanceAlarm(VALARM VAlarm, int ColNumber, List<EQMap> eqMap)
        {
            return new MaintenanceAlarm(VAlarm, ColNumber);
        }
        public VTRANSFER GetVTransfer(VTRANSFER vTran, VCMD vCmd)
        {
            var customer_tran = new AGVC_SOUTH_INNOLUX_VTRANSFER(vTran);
            customer_tran.setVCMD(vCmd);
            return customer_tran;
        }
    }
}
