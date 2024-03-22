﻿using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace ObjectConverter_AGVC_ASE_K21.BLL
{
    public class CustomerBLL : ICustomerObjBLL
    {
        public MaintenanceAlarm GetMaintenanceAlarm(VALARM VAlarm, int ColNumber, List<EQMap> eqMap)
        {
            return new K21MaintenanceAlarm(VAlarm, ColNumber, eqMap);
        }

        public VTRANSFER GetVTransfer(VTRANSFER vTran, VCMD vCmd)
        {
            vTran.setVCMD(vCmd);
            return vTran;
        }
    }
}