using com.mirle.ibg3k0.ohxc.wpf.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using ViewerObject;
using InfluxDB.Client;
using Nest;
using System.Threading.Tasks;

namespace UtilsAPI.BLL
{
	public class StatusInfoBLL
	{
		NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		WindownApplication app = null;

		public StatusInfoBLL(WindownApplication _app)
		{
			app = _app;
		}

        #region Alarm
        public async Task<Dictionary<string,long>> GetStatusAlarmCount(DateTime? StartTime=null , DateTime? EndTime=null)
        {
			//_measurement:vehiclealarmcount task有經過UTF處理，所以直接用進來的時間查詢即可
			//但是初始檢查的BigData Bucket沒有，所以需要將時間轉成UTF8
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			Dictionary<DateTime, List<string>> dicAlarmTime = new Dictionary<DateTime, List<string>>();
			try
			{
				string RngStartTime = "start: 0";
				string RngEndTime = "";
				string nowvh = "";
				long nowvalue = 0;

				#region 第一筆Alarm因沒有與前面的相減值，所以無法計算，所以撈AlarmTime出來，如果有就代表他有Alarm，先幫Count+1處理
				dicAlarmTime = await CheckAlarmTimeByTime(StartTime, EndTime);
				foreach (var dateTime in dicAlarmTime.Keys)
				{
					foreach (var vh in dicAlarmTime[dateTime])
					{
						if (dicReturn.ContainsKey(vh))
						{
							dicReturn[vh] += 1;
						}
						else
						{
							dicReturn.Add(vh, 1);
						}
					}

				}

				#endregion

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);


				 RngStartTime = "start: 0";
				 RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range("+ RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclealarmcount\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();


					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue;
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}

				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		public async Task<Dictionary<string, long>> GetStatusAlarmTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclealarmtime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				long nowvalue = 0;
				foreach (var record in tables.SelectMany(table => table.Records))
				{
					
						nowvh = (string)record.GetValueByKey("vehicleid");
						nowvalue = (long)record.GetValue();
						if (dicReturn.ContainsKey(nowvh))
						{
							dicReturn[nowvh] += nowvalue;
						}
						else
						{
							dicReturn.Add(nowvh, nowvalue);
						}				
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		public async Task<Dictionary<DateTime, List<string>>> CheckAlarmTimeByTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<DateTime, List<string>> dicReturn = new Dictionary<DateTime, List<string>>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclealarmtime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				DateTime nowdate;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowdate =Convert.ToDateTime(record.GetValueByKey("_time").ToString());
					if (dicReturn.ContainsKey(nowdate))
					{
						dicReturn[nowdate].Add(nowvh);
					}
					else
					{
						dicReturn[nowdate] = new List<string> { nowvh };
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}
		#endregion

		#region Long Charging
		public async Task<Dictionary<string, long>> GetLongChargeCount(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//_measurement:vehiclealarmcount task有經過UTF處理，所以直接用進來的時間查詢即可
			//但是初始檢查的BigData Bucket沒有，所以需要將時間轉成UTF8
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			Dictionary<DateTime, List<string>>  dicChargeTimeByTime = new Dictionary<DateTime, List<string>>();
			try
			{
				string RngStartTime = "start: 0";
				string RngEndTime = "";
				string nowvh = "";
				long nowvalue = 0;

				#region 由於每天的第一筆Charge因沒有與前面的相減值，所以無法計算，所以撈ChargeTime出來，如果有就代表他有Charg，先幫Count+1處理
				dicChargeTimeByTime = await CheckLongChargeTimeByTime(StartTime, EndTime);
				foreach (var dateTime in dicChargeTimeByTime.Keys)
				{
					foreach(var vh in dicChargeTimeByTime[dateTime])
                    {
						if (dicReturn.ContainsKey(vh))
						{
							dicReturn[vh] += 1;
						}
						else
						{
							dicReturn.Add(vh, 1);
						}
					}
					
				}

				#endregion

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);


				RngStartTime = "start: 0";
				RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclealongchargecount\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");


				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();


					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue; //這邊不再+1，因為次數會先被前面的ChargeTime處理加上1了
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}

				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		public async Task<Dictionary<string, long>> GetLongChargeTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclealongchargectime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				long nowvalue = 0;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();
					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue;
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		public async Task<Dictionary<DateTime, List<string>>> CheckLongChargeTimeByTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<DateTime, List<string>> dicReturn = new Dictionary<DateTime, List<string>>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclealongchargectime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				DateTime nowdate;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowdate = Convert.ToDateTime(record.GetValueByKey("_time").ToString());
					if (dicReturn.ContainsKey(nowdate))
					{
						dicReturn[nowdate].Add(nowvh);
					}
					else
					{
						dicReturn[nowdate]=new List<string> { nowvh };
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		#endregion

		#region RunTime
		public async Task<Dictionary<string, long>> GetRuneTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehicleruntime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				long nowvalue = 0;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();
					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue;
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		public async Task<Dictionary<string, long>> GetMCSRuneTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclemcsruntime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				long nowvalue = 0;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();
					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue;
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}

		public async Task<Dictionary<string, long>> GetOHTCRuneTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehicleohtcruntime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				long nowvalue = 0;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();
					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue;
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}
		#endregion

		#region Schedule Down Time
		public async Task<Dictionary<string, long>> GetScheduleDownTime(DateTime? StartTime = null, DateTime? EndTime = null)
		{
			//這支Function 不加入VH_ID的Filter，因InfluxDB處理會變成Scan，執行效能會直接炸開，真的要單獨VH_ID的資料，因已Group完，資料筆數很低，前端執行就好
			//抓回來的資料單位為秒
			Dictionary<string, long> dicReturn = new Dictionary<string, long>();
			try
			{

				var options = new InfluxDBClientOptions.Builder()
									.Url(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.ConnectString)
									.AuthenticateToken(app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleStatistics)
									.TimeOut(TimeSpan.FromSeconds(60))
									.Build();
				var influxDBClient = InfluxDBClientFactory.Create(options);

				string RngStartTime = "start: 0";
				string RngEndTime = "";
				//因InfluxDB為UTC存取，時間要轉UTC，並且報表會因為Offset延遲所以搜索範圍要晚一天
				if (StartTime != null) RngStartTime = "start: time(v : \"" + StartTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";
				if (EndTime != null) RngEndTime = ",stop: time(v : \"" + EndTime.Value.ToUniversalTime().AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\")";

				var query =
				 "from(bucket: \"VehicleStatistics\")" +
				  "|> range(" + RngStartTime + RngEndTime + ")" +
				  "|> filter(fn: (r) => r[\"_measurement\"] == \"vehiclescheduledowntime\")";

				var tables = await influxDBClient.GetQueryApi().QueryAsync(query, "Mirle");

				string nowvh = "";
				long nowvalue = 0;
				foreach (var record in tables.SelectMany(table => table.Records))
				{

					nowvh = (string)record.GetValueByKey("vehicleid");
					nowvalue = (long)record.GetValue();
					if (dicReturn.ContainsKey(nowvh))
					{
						dicReturn[nowvh] += nowvalue;
					}
					else
					{
						dicReturn.Add(nowvh, nowvalue);
					}
				}
				influxDBClient.Dispose();
				return dicReturn;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
				return dicReturn;
			}
		}
		#endregion
	}
}