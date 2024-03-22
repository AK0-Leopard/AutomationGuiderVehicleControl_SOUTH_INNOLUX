using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ViewerObject;
using ViewerObject.REPORT;

namespace UtilsAPI.BLL
{
    public class ConstantBLL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        WindownApplication app;
        public ConstantBLL(WindownApplication _app)
        {
            app = _app;
        }

        public List<VCONSTANT> getAllContants()
        {
            List<VCONSTANT> contants = new List<VCONSTANT>();

            try
            {
                if (app.ObjCacheManager.ObjConverter.BLL.ConstantBLL == null)
                    return null;
                else
                    return app.ObjCacheManager.ObjConverter.BLL.ConstantBLL.getAllVConstant();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return contants;
            }

        }

    }
}
