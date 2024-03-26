using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessage.ProtocolFormat.AlarmFun;


namespace ObjectConverterInterface.BLL
{
    public interface IOperationHistoryBLL
    {
        List<ViewerObject.VOPERATION> LoadOperationsByConditions(DateTime startDatetime, DateTime endDatetime);
        void InsertOperation(ViewerObject.VOPERATION valarm);
    }
}
