using com.mirle.ibg3k0.ohxc.wpf.App;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewerObject;
using ViewerObject.Customer;

namespace UtilsAPI.BLL
{
    public class CustomerObjBLL
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        WindownApplication app = null;

        public CustomerObjBLL(WindownApplication _app)
        {
            app = _app;
            //alarmUpdateEventHandler += alarmUpdate;
        }

        public MaintenanceAlarm GetMaintenanceAlarm(VALARM VAlarm, int ColNumber)
        {
            MaintenanceAlarm default_result = null;
            try
            {

                if (app.ObjCacheManager.ObjConverter.BLL.CustomerObjBLL == null) return new MaintenanceAlarm(VAlarm, ColNumber); //如果Factory沒有建立客製化的BLL,用預設的CommonBLL處理

                return app.ObjCacheManager.ObjConverter.BLL.CustomerObjBLL.GetMaintenanceAlarm(VAlarm, ColNumber, app.ObjCacheManager.GetEQMap()); //如果有建立客製化，直接走客製化路線處理
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VTRANSFER GetVTRANSFER(VTRANSFER vTran, VCMD vCmd)
        {
            try
            {
                if (app.ObjCacheManager.ObjConverter.BLL.CustomerObjBLL == null)
                {
                    vTran.setVCMD(vCmd);
                    return vTran;
                }
                return app.ObjCacheManager.ObjConverter.BLL.CustomerObjBLL.GetVTransfer(vTran, vCmd); //如果有建立客製化，直接走客製化路線處理
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return vTran;
            }
        }
    }
}
