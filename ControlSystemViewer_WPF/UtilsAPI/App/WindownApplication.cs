using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.BackgroundWork;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using com.mirle.ibg3k0.ohxc.wpf.Schedule;
using com.mirle.ibg3k0.Utility.uc;
using GenericParsing;
using Newtonsoft.Json;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilsAPI.BLL;
using ChartConverter.Converter;

namespace com.mirle.ibg3k0.ohxc.wpf.App
{
	public enum OHxCFormMode
	{
		CurrentPlayer,
		HistoricalPlayer
	}

	public class WindownApplication
	{
		private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		public static OHxCFormMode OHxCFormMode = OHxCFormMode.CurrentPlayer;
		public static string ID { get; private set; } = "";
		public string ViewerID { get; private set; } = "";

		public bool IsControlSystemConnentable { get; set; } = true; // 初始化階段 若開啟只是因C無法連上 跳過相關功能 不強制關閉Viewer

		#region Language
		public EventHandler LanguageChanged;
		private string languageCode = "en-US";
		public string LanguageCode 
		{
			get { return languageCode; }
			set
            {
				if (languageCode != value)
                {
					languageCode = value;
					LanguageChanged?.Invoke(this, null);
				}
            }
		}
		#endregion Language

		public BackgroundWorkDriver BackgroundWork_ProcessVhPositionUpdate { get; private set; }

		#region UAS

		private string loginUserID = null;
		public string LoginUserID { get { return loginUserID; } }

		//private Dictionary<string, Forms.ToolStripStatusLabel> statusUserIDLabelDic =
		//	new Dictionary<string, System.Windows.Forms.ToolStripStatusLabel>();

		private Dictionary<string, Action<object>> refresh_UIDisplay_FunDic =
			new Dictionary<string, Action<object>>(); //A0.01

		#endregion UAS

		//config
		private DataSet viewerConfig = null;
		public DataSet ViewerConfig { get { return viewerConfig; } }

		private NatsManager natsManager = null;
		private RedisCacheManager redisCacheManager = null;
		private WebClientManager webClientManager = null;
		private ElasticSearchManager elasticSearchManager = null;

		//private HistoricalReplyService HistoricalReplyService = null;
		//private SysExcuteQualityQueryService SysExcuteQualityQueryService = null;

		public ObjCacheManager ObjCacheManager { get; private set; }
		public ObjChartConverter oChartConverter { get; private set; }

		public VehicleBLL VehicleBLL { get; private set; }
		public MapBLL MapBLL { get; private set; }
		public LineBLL LineBLL { get; private set; }

		public CmdBLL CmdBLL { get; private set; }
		public AlarmBLL AlarmBLL { get; private set; }
		public PortStationBLL PortStationBLL { get; private set; }
		public PortBLL PortBLL { get; private set; }
		public SegmentBLL SegmentBLL { get; private set; }
		public StatusInfoBLL StatusInfoBLL { get; private set; }
		public CustomerObjBLL CustomerObjBLL { get; private set; }
		public PLCBLL PLCBLL { get; private set; }

		public SysExcuteQualityBLL SysExcuteQualityBLL { get; private set; }
		public UserBLL UserBLL { get; private set; }
		public OperationHistoryBLL OperationHistoryBLL { get; private set; }
		public SystemOperationLogBLL SystemOperationLogBLL { get;  set; }
		public ConstantBLL ConstantBLL { get; private set; }

		public ParkZoneBLL ParkZoneBLL { get; private set; }
		//public PortDefBLL PortDefBLL { get; private set; }
		//public ShelfDefBLL ShelfDefBLL { get; private set; }
		//public ZoneDefBLL ZoneDefBLL { get; private set; }
		//public CassetteDataBLL CassetteDataBLL { get; private set; }
		//public ControlerBLL ControlerBLL { get; private set; }
		//public EqptBLL EqptBLL { get; private set; }

		//取得主頁面 版本號碼
		public static string getMainFormVersion(string appendStr)
		{
			return FileVersionInfo.GetVersionInfo(
				Assembly.GetExecutingAssembly().Location).FileVersion.ToString() + appendStr;
		}

		//取得主頁面 建置時間
		public static DateTime GetBuildDateTime()
		{
			string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;
			byte[] b = new byte[2048];
			System.IO.Stream s = null;

			try
			{
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
				{
					s.Close();
				}
			}

			int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.ToLocalTime();
			return dt;
		}

		//public void addOperationHis(string user_id, string formName, string action)
		//{
		//    DBConnection conn = null;
		//    try
		//    {
		//        conn = scApp.getDBConnection();
		//        conn.BeginTransaction();
		//        string timeStamp = BCFUtility.formatDateTime(DateTime.Now, SCAppConstants.TimestampFormat_19);
		//        OperationHis his = new OperationHis()
		//        {
		//            T_Stamp = timeStamp,
		//            User_ID = user_id,
		//            Form_Name = formName,
		//            Action = action
		//        };
		//        SCUtility.PrintOperationLog(his);
		//        deleteOperationHis(conn);        //A0.06
		//        operationHisDao.insertOperationHis(conn, his);
		//        conn.Commit();
		//    }
		//    catch (Exception)
		//    {
		//        if (conn != null) { try { conn.Rollback(); } catch { } }
		//    }
		//    finally
		//    {
		//        if (conn != null) { try { conn.Close(); } catch { } }
		//    }
		//}

		private static object _lock = new object();
		private static WindownApplication application;

		public static WindownApplication getInstance()
		{
			if (application == null)
			{
				lock (_lock)
				{
					if (application == null)
					{
						application = new WindownApplication(out bool isSuccess);
						if (!isSuccess) application = null;
					}
				}
			}
			return application;
		}

		#region Redis

		public const string REDIS_LIST_KEY_VEHICLES = "Redis_List_Vehicles";
		public const string REDIS_KEY_CURRENT_ALARM = "Current Alarm";

		#endregion Redis

		public WindownApplication(out bool isSuccess)
		{
			isSuccess = true;
			string _doing = "";
			try
			{
				_doing = "init ViewerID";
				ViewerID = getString("Viewer_ID", "") + "_" + Environment.MachineName;

				_doing = "init webClientManager";
				webClientManager = WebClientManager.getInstance();

				_doing = "init ObjCacheManager";
				ObjCacheManager = new ObjCacheManager(this);

				_doing = "init ObjChartConverter";
				oChartConverter = ObjChartConverter.GetInstance();

				_doing = "init elasticSearchManager";
				elasticSearchManager = new ElasticSearchManager();

				_doing = "init BLLs";
				MapBLL = new MapBLL(this);
				UserBLL = new UserBLL(this);
				OperationHistoryBLL = new OperationHistoryBLL(this);
				SystemOperationLogBLL = new SystemOperationLogBLL(this);
				CmdBLL = new CmdBLL(this);
				AlarmBLL = new AlarmBLL(this);
				PortStationBLL = new PortStationBLL(this);
				PortBLL = new PortBLL(this);
				SegmentBLL = new SegmentBLL(this);
				StatusInfoBLL = new StatusInfoBLL(this);
				CustomerObjBLL = new CustomerObjBLL(this);
				ConstantBLL = new ConstantBLL(this);
				//PortDefBLL = new PortDefBLL(this);
				//ShelfDefBLL = new ShelfDefBLL(this);
				//ZoneDefBLL = new ZoneDefBLL(this);
				//CassetteDataBLL = new CassetteDataBLL(this);
				//ControlerBLL = new ControlerBLL(this);
				//EqptBLL = new EqptBLL(this);
				LineBLL = new LineBLL(this);
				VehicleBLL = new VehicleBLL(this);
				SysExcuteQualityBLL = new SysExcuteQualityBLL(this);
				//2022 0926 for M4
				PLCBLL = new PLCBLL(this);
				ParkZoneBLL = new ParkZoneBLL(this);

				_doing = "init Config (CSV)";
				initConfig();
				_doing = "ObjCacheManager.start()";
				isSuccess = ObjCacheManager.start();
				if (!isSuccess || !IsControlSystemConnentable) return;
				ID = ObjCacheManager.MapId;

				//_doing = "init SysExcuteQualityQueryService";
				//SysExcuteQualityQueryService = new SysExcuteQualityQueryService(this);
				//initBackgroundWork();
				//  SysExcuteQualityQueryService = new SysExcuteQualityQueryService();
				switch (WindownApplication.OHxCFormMode)
				{
					case OHxCFormMode.CurrentPlayer:
						_doing = "init redisCacheManager";
						redisCacheManager = new RedisCacheManager(ID);
						_doing = "init natsManager";
						natsManager = new NatsManager(ID, "nats-cluster", ViewerID , ObjCacheManager.ViewerSettings.nats.Port);
						_doing = "SubscriberNatsEvent";
						string ChannelBay = ObjCacheManager.ViewerSettings.nats.ChannelBay;
						SubscriberNatsEvent(ChannelBay);
						//SysExcuteQualityQueryService.start();
						break;

						//case OHxCFormMode.HistoricalPlayer:
						//	HistoricalReplyService = new HistoricalReplyService(this);
						//	//HistoricalReplyService.loadVhHistoricalInfo();
						//	break;
				}
				//VehicleBLL.ReguestViewerUpdate();
			}
			catch (Exception ex)
			{
				isSuccess = false;
				logger.Error($"Exception occurred while {_doing}.");
				logger.Error(ex, "Exception");
			}
		}

		private void initBackgroundWork()
		{
			BackgroundWork_ProcessVhPositionUpdate = new BackgroundWorkDriver(new BackgroundWork_ProcessVhPositionUpdate());
		}

		object lineStatLock = new object();
		private void SubscriberNatsEvent(string ChannelBay)
		{
			try
			{
				if (ChannelBay != "") ChannelBay = "_" + ChannelBay;
				foreach (var vh in ObjCacheManager.GetVEHICLEs())
				{
					string subject_id = string.Format(BCAppConstants.NATSTopics.NATS_SUBJECT_VH_INFO_0, vh.VEHICLE_ID);
					VehicleBLL.SubscriberVehicleInfo(subject_id+ ChannelBay, VehicleBLL.ProcVehicleInfo);
					System.Threading.Thread.Sleep(20);
				}
				//foreach (var port in ObjCacheManager.GetPortStation())
				//{
				//	string subject_id = string.Format(BCAppConstants.NATSTopics.NATS_SUBJECT_POSTATION_CHANGE, port.PORT_ID.Trim());
				//	PortDefBLL.SubscriberPortInfo(subject_id, PortDefBLL.ProcPortInfo);
				//	System.Threading.Thread.Sleep(20);
				//}

				//EqptBLL.SubscriberMTLInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_MTLMTS, EqptBLL.ProcMTLInfo);
				//System.Threading.Thread.Sleep(20);


				LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_LINE_INFO+ ChannelBay, LineBLL.ProcLineInfo);
				LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_CONNECTION_INFO+ ChannelBay, LineBLL.ConnectioneInfo);
				LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_TCPIP_LOG+ ChannelBay, LineBLL.ProcTcpInfo);
				LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_SECS_LOG+ ChannelBay, LineBLL.ProcSecsInfo);
				//LineBLL.SubscriberLineInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_SYSTEM_LOG, LineBLL.ProcSystemInfo);
				LineBLL.SubscriberTipMessageInfo(BCAppConstants.NATSTopics.NATS_SUBJECT_TIP_MESSAGE_INFO+ ChannelBay, LineBLL.ProcTipMessageInfo);
				GetNatsManager().Subscriber(BCAppConstants.NATSTopics.NATS_SUBJECT_CURRENT_ALARM+ ChannelBay, ProcCurrentAlarm);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Exception");
			}
		}

		private void ProcCurrentAlarm(object sender, StanMsgHandlerArgs handler)
		{
			//List<sc.ALARM> alarms = getCurrentAlarmFromRedis();
			//List<sc.ALARM> alarms = AlarmBLL.loadSetAlarm();
			//ObjCacheManager.updateAlarm(alarms);
			//CurrentAlarmChange?.Invoke(this, alarms);
			if (ObjCacheManager.updateAlarm())
				ObjCacheManager.onAlarmChange(sender);
		}

		private void setEFConnectionString(string connectionstring)
		{
			string connectionName = "OHTC_DevEntities";
			// Get the configuration file.
			System.Configuration.Configuration config =
				ConfigurationManager.OpenExeConfiguration(
				ConfigurationUserLevel.None);
			// Add the connection string.
			ConnectionStringsSection csSection =
				config.ConnectionStrings;
			csSection.ConnectionStrings.Add(
			new ConnectionStringSettings(connectionName, connectionstring));
		}

		public NatsManager GetNatsManager()
		{
			return natsManager;
		}

		public RedisCacheManager GetRedisCacheManager()
		{
			return redisCacheManager;
		}

		public WebClientManager GetWebClientManager()
		{
			return webClientManager;
		}

		public ElasticSearchManager GetElasticSearchManager()
		{
			return elasticSearchManager;
		}

		//public HistoricalReplyService GetHistoricalReplyService()
		//{
		//	return HistoricalReplyService;
		//}

		//public SysExcuteQualityQueryService GetSysExcuteQualityQueryService()
		//{
		//	return SysExcuteQualityQueryService;
		//}

		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>System.String.</returns>
		private string getString(string key, string defaultValue)
		{
			string rtn = defaultValue;
			try
			{
				rtn = ConfigurationManager.AppSettings.Get(key);
			}
			catch (Exception e)
			{
				logger.Warn("Get Config error[key:{0}][Exception:{1}]", key, e);
			}
			return rtn;
		}

#region UAS
		public void login(string user_id)
		{
			loginUserID = user_id;
			ObjCacheManager.onLogInUserChanged();
			//refreshLoginUserInfo();
			refresh_UIDisplayFun();//A0.01
		}

		//private void refreshLoginUserInfo()
		//{
		//    foreach (System.Windows.Forms.ToolStripStatusLabel label in statusUserIDLabelDic.Values)
		//    {
		//        try
		//        {
		//            if (label == null || label.IsDisposed)
		//            {
		//                continue;
		//            }
		//            label.Text = loginUserID;
		//        }
		//        catch (Exception ex)
		//        {
		//            logger.Error(ex, "Exception");
		//        }
		//    }
		//}

		/// <summary>
		/// A0.01
		/// </summary>
		private void refresh_UIDisplayFun()
		{
			foreach (Action<object> action in refresh_UIDisplay_FunDic.Values)
			{
				try
				{
					if (action == null)
					{
						continue;
					}
					action(new object());
				}
				catch (Exception ex)
				{
					logger.Error(ex, "Exception");
				}
			}
		}

		public void logout()
		{
			login("");
		}

		/// <summary>
		/// A0.01
		/// </summary>
		/// <param name="refreshFun"></param>
		public void addRefreshUIDisplayFun(Action<object> refreshFun)
		{
			//statusUserIDLabelDic
			if (refresh_UIDisplay_FunDic.ContainsKey(refreshFun.Method.Name))
			{
				refresh_UIDisplay_FunDic[refreshFun.Method.Name] = refreshFun;
			}
			else
			{
				refresh_UIDisplay_FunDic.Add(refreshFun.Method.Name, refreshFun);
			}
		}

		public void addRefreshUIDisplayFun(string formName, Action<object> refreshFun)
		{
			//statusUserIDLabelDic
			if (refresh_UIDisplay_FunDic.ContainsKey(formName))
			{
				refresh_UIDisplay_FunDic[formName] = refreshFun;
			}
			else
			{
				refresh_UIDisplay_FunDic.Add(formName, refreshFun);
			}
		}

		//public void addRe

#endregion UAS

#region Config
		private void initConfig()
		{
			if (viewerConfig == null) viewerConfig = new DataSet();
			else viewerConfig.Clear();

			loadCSVToDataset(viewerConfig, "PROJECTINFO");
		}
		private void initConfig_Settings(string folder)
		{
			loadCSVToDataset(viewerConfig, "SETTINGS", folder);
		}
		private void initConfig_MapInfo(string folder)
		{
			loadCSVToDataset(viewerConfig, "MapInfo\\AADDRESS", folder);
			loadCSVToDataset(viewerConfig, "MapInfo\\ASECTION", folder);
			try { loadCSVToDataset(viewerConfig, "MapInfo\\ALABEL", folder); } catch { }
			try { loadCSVToDataset(viewerConfig, "MapInfo\\SHELF", folder); } catch { }
			try { loadCSVToDataset(viewerConfig, "MapInfo\\TRACKSWITCH", folder); } catch { }
			try { loadCSVToDataset(viewerConfig, "ALARMMAP", folder); } catch { }
			try { loadCSVToDataset(viewerConfig, "ALARMMODULE", folder); } catch { }
			try { loadCSVToDataset(viewerConfig, "EQMAP", folder); } catch { }
		}
		public bool InitSettings(string folder)
		{
			if (string.IsNullOrWhiteSpace(folder))
			{
				logger.Warn($"Folder({folder}) MUST NOT be empty.");
			}

			string _doing = "";
			try
			{
				_doing = "init Config Settings";
				initConfig_Settings(folder);
				return true;
			}
			catch (Exception ex)
			{
				logger.Error($"Exception occurred while {_doing}.");
				logger.Error(ex, "Exception");
				return false;
			}
		}
		public bool InitMapInfo(string folder)
		{
			if (string.IsNullOrWhiteSpace(folder))
			{
				logger.Warn($"Folder({folder}) MUST NOT be empty.");
			}

			string _doing = "";
			try
			{
				_doing = "init Config MapInfo";
				initConfig_MapInfo(folder);
				ID = ObjCacheManager.MapId;
				return true;
			}
			catch (Exception ex)
			{
				logger.Error($"Exception occurred while {_doing}.");
				logger.Error(ex, "Exception");
				return false;
			}
		}
		private void loadCSVToDataset(DataSet ds, string tableName, string folderName = "")
		{
			using (GenericParser parser = new GenericParser())
			{
				string fileName = $"{Environment.CurrentDirectory}\\Config\\";
				if (!string.IsNullOrWhiteSpace(folderName)) fileName += $"{folderName}\\";
				fileName += $"{tableName}.csv";
				parser.SetDataSource(fileName, System.Text.Encoding.Default);
				parser.ColumnDelimiter = ',';
				parser.FirstRowHasHeader = true;
				//parser.SkipStartingDataRows = 1;
				parser.MaxBufferSize = 1024;
				//parser.MaxRows = 500;
				//parser.TextQualifier = '\"';


				DataTable dt = new System.Data.DataTable(tableName);

				bool isfirst = true;
				while (parser.Read())
				{

					int cs = parser.ColumnCount;
					if (isfirst)
					{

						for (int i = 0; i < cs; i++)
						{
							dt.Columns.Add(parser.GetColumnName(i), typeof(string));
						}
						isfirst = false;
					}


					DataRow dr = dt.NewRow();

					for (int i = 0; i < cs; i++)
					{
						string val = parser[i];
						//ALARM 要可以接受 16進制的 2015.02.23 by Kevin Wei
						//if (dt.Columns[i] != null && BCFUtility.isMatche(dt.Columns[i].ColumnName, "ALARM_ID"))
						//{
						//    int valInt = Convert.ToInt32(val);
						//    val = val;
						//}
						dr[i] = val;
						//                        dr[i] = parser[i];
					}
					dt.Rows.Add(dr);
				}
				ds.Tables.Add(dt);
			}
		}
#endregion Config
	}
}