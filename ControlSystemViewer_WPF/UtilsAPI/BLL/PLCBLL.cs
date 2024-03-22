using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject.REPORT;

namespace UtilsAPI.BLL
{
    public class PLCBLL
	{
		private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public PLCBLL(WindownApplication _app)
        {
            app = _app;
        }

        public List<VHIDINFO> GetHIDinfoByDate(DateTime startDatetime, DateTime endDatetime, string eqptID = null)
        {
            List<VHIDINFO> default_result = new List<VHIDINFO>();
            List<VHIDINFO> result;
            try
            {
                result = app.ObjCacheManager.ObjConverter.BLL.PLCBLL.GetHidinfoByDate(startDatetime, endDatetime, eqptID) ;
                
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
    }
}
