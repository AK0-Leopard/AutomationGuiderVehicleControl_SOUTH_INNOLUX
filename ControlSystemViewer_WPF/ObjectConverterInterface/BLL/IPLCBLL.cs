using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMessage.ProtocolFormat.AlarmFun;


namespace ObjectConverterInterface.BLL
{
    public interface IPLCBLL
    {
        List<ViewerObject.REPORT.VHIDINFO> GetHidinfoByDate(DateTime startDatetime, DateTime endDatetime, string eqptID = null);
        
    }
}
