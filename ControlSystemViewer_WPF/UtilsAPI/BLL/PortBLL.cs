using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsAPI.BLL
{
    public class PortBLL
	{
		private string ns = "UtilsAPI.BLL" + ".PortBLL";

		private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public PortBLL(WindownApplication _app)
        {
            app = _app;
        }

		public bool SetPortRun(string port_id, out string result)
		{
			string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
			if (app.ObjCacheManager.GetPortStations() == null || !app.ObjCacheManager.GetPortStations().Any(p => p.PORT_ID == port_id))
			{
				result = $"Can't find {port_id} in PORTSTATIONs";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter == null)
			{
				result = $"ObjConverter = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter.BLL.PortBLL == null)
			{
				result = $"ObjConverter.BLL.PortBLL = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			try
			{
				bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.PortBLL.SetPortRun(port_id, out result);
				if (!isSuccess) logger.Warn($"{ns}.{mn}: {result}");
				return isSuccess;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				result = $"Exception happend, {ex.Message}";
				return false;
			}
		}
		public bool SetPortStop(string port_id, out string result)
		{
			string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
			if (app.ObjCacheManager.GetPortStations() == null || !app.ObjCacheManager.GetPortStations().Any(p => p.PORT_ID == port_id))
			{
				result = $"Can't find {port_id} in PORTSTATIONs";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter == null)
			{
				result = $"ObjConverter = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter.BLL.PortBLL == null)
			{
				result = $"ObjConverter.BLL.PortBLL = null";
				logger.Warn($"{ns}.{mn}: {result}");
			}
			try
			{
				bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.PortBLL.SetPortStop(port_id, out result);
				if (!isSuccess) logger.Warn($"{ns}.{mn}: {result}"); ;
				return isSuccess;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				result = $"Exception happend, {ex.Message}";
				return false;
			}
		}
		public bool ResetPortAlarm(string port_id, out string result)
		{
			string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
			if (app.ObjCacheManager.GetPortStations() == null || !app.ObjCacheManager.GetPortStations().Any(p => p.PORT_ID == port_id))
			{
				result = $"Can't find {port_id} in PORTSTATIONs";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter == null)
			{
				result = $"ObjConverter = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter.BLL.PortBLL == null)
			{
				result = $"ObjConverter.BLL.PortBLL = null";
				logger.Warn($"{ns}.{mn}: {result}");
			}
			try
			{
				bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.PortBLL.ResetPortAlarm(port_id, out result);
				if (!isSuccess) logger.Warn($"{ns}.{mn}: {result}");
				return isSuccess;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				result = $"Exception happend, {ex.Message}";
				return false;
			}
		}
		public bool SetPortDir(string port_id, string dir, out string result)
		{
			string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
			if (app.ObjCacheManager.GetPortStations() == null || !app.ObjCacheManager.GetPortStations().Any(p => p.PORT_ID == port_id))
			{
				result = $"Can't find {port_id} in PORTSTATIONs";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter == null)
			{
				result = $"ObjConverter = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter.BLL.PortBLL == null)
			{
				result = $"ObjConverter.BLL.PortBLL = null";
				logger.Warn($"{ns}.{mn}: {result}");
			}
			try
			{
				bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.PortBLL.SetPortDir(port_id, dir, out result);
				if (!isSuccess) logger.Warn($"{ns}.{mn}: {result}");
				return isSuccess;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				result = $"Exception happend, {ex.Message}";
				return false;
			}
		}
		public bool SetPortWaitIn(string port_id, out string result)
		{
			string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
			if (app.ObjCacheManager.GetPortStations() == null || !app.ObjCacheManager.GetPortStations().Any(p => p.PORT_ID == port_id))
			{
				result = $"Can't find {port_id} in PORTSTATIONs";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter == null)
			{
				result = $"ObjConverter = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter.BLL.PortBLL == null)
			{
				result = $"ObjConverter.BLL.PortBLL = null";
				logger.Warn($"{ns}.{mn}: {result}");
			}
			try
			{
				bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.PortBLL.SetPortWaitIn(port_id, out result);
				if (!isSuccess) logger.Warn($"{ns}.{mn}: {result}");
				return isSuccess;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				result = $"Exception happend, {ex.Message}";
				return false;
			}
		}
		public bool SetAgvStationOpen(string port_id, bool open, out string result)
		{
			string mn = System.Reflection.MethodBase.GetCurrentMethod().Name;
			if (app.ObjCacheManager.GetPortStations() == null || !app.ObjCacheManager.GetPortStations().Any(p => p.PORT_ID == port_id))
			{
				result = $"Can't find {port_id} in PORTSTATIONs";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter == null)
			{
				result = $"ObjConverter = null";
				logger.Warn($"{ns}.{mn}: {result}");
				return false;
			}
			if (app.ObjCacheManager.ObjConverter.BLL.PortBLL == null)
			{
				result = $"ObjConverter.BLL.PortBLL = null";
				logger.Warn($"{ns}.{mn}: {result}");
			}
			try
			{
				bool isSuccess = app.ObjCacheManager.ObjConverter.BLL.PortBLL.SetAgvStationOpen(port_id, open, out result);
				if (!isSuccess) logger.Warn($"{ns}.{mn}: {result}");
				return isSuccess;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				result = $"Exception happend, {ex.Message}";
				return false;
			}
		}
	}
}
