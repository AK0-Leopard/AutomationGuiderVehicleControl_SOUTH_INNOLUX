using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilsAPI.BLL;

namespace com.mirle.ibg3k0.ohxc.wpf.Service
{
  //  public class SysExcuteQualityQueryService
  //  {

  //      public static string SYSEXCUTEQUALITY_CMD_QUEUE_TIME = "CMDQueueTime.png";
  //      public static string SYSEXCUTEQUALITY_CMD_COUNT = "CMDCount.png";
  //      SysExcuteQualityBLL sysExcuteQualityBLL = null;
  //      WindownApplication app;

  //      bool isComparisonBaseTableReady = false;
  //      public List<Port2Port_TransferInfo> port2Port_ComparisonBaseTable = null;

  //      public Image Image_Cmd_Count
  //      {
  //          get; private set;
  //      }
  //      public Image Image_Cmd_Queue_Time
  //      {
  //          get; private set;
  //      }
  //      public double OhxCExcuteEffectiveness_Percentage
  //      {
  //          get; private set;
  //      }
  //      public int OhxCExcuteEffectiveness_TotalCompleteCmd
  //      {
  //          get; private set;
  //      }
  //      public int CurrnetMCSCommandCount_All
  //      {
  //          get; private set;
  //      }
  //      public int CurrnetMCSCommandCount_Queue
  //      {
  //          get; private set;
  //      }

  //      public event EventHandler SysExcuteQualityImageChanged;
  //      public event EventHandler<int> VehicleIdleStatusChanged;
  //      /// <summary>
  //      /// int[0] = Total complete command,
  //      /// int[1] = Excute effectiveness percentage
  //      /// </summary>
  //      public event EventHandler<int[]> OhxCExcuteEffectivenessChanged;
  //      /// <summary>
  //      /// int[0] = current all command count,
  //      /// int[1] = current Queue command count
  //      /// </summary>
  //      public event EventHandler<int[]> CurrnetMCSCommandCountChanged;

  //      public SysExcuteQualityQueryService(WindownApplication _app)
  //      {
  //          sysExcuteQualityBLL = _app.SysExcuteQualityBLL;
  //          app = _app;
  //      }

  //      public void start()
  //      {
  //          if (WindownApplication.OHxCFormMode == OHxCFormMode.CurrentPlayer)
  //              Task.Run(() => RegularUpdateSysQualityStatus());
  //      }

  //      #region Current OHxC Status
  //      private void RegularUpdateSysQualityStatus()
  //      {
  //          while (true)
  //          {
  //              UpdateSysExcuteQuality();

  //              UpdateVehicleIdleStatus();

  //              CalculationSysEfficiency_60min_ago();

  //              updateCurrentMCSCommandExcuteStatus();

  //              SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 10));
  //          }
  //      }

  //      private void UpdateVehicleIdleStatus()
  //      {
  //          var vhs = app.ObjCacheManager.GetVEHICLEs();
  //          int idle_vh_count = vhs.
  //              Where(vh => vh.ACT_STATUS == ViewerObject.VVEHICLE_Def.ActionStatus.NoCommand ||
  //                          (vh.ACT_STATUS == ViewerObject.VVEHICLE_Def.ActionStatus.Commanding)).Count();
  //          VehicleIdleStatusChanged?.Invoke(this, idle_vh_count);
  //      }

  //      private void UpdateSysExcuteQuality()
  //      {
  //          DateTime now_date_time = DateTime.Now;

  //          now_date_time = now_date_time.ToUniversalTime();
  //          string start_time = now_date_time.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ssZ");
  //          string end_time = now_date_time.ToString("yyyy-MM-ddTHH:mm:ssZ");
  //          Image_Cmd_Count = sysExcuteQualityBLL.getSysExcuteQualityImaget(SYSEXCUTEQUALITY_CMD_QUEUE_TIME, start_time, end_time);
  //          Image_Cmd_Queue_Time = sysExcuteQualityBLL.getSysExcuteQualityImaget(SYSEXCUTEQUALITY_CMD_COUNT, start_time, end_time);
  //          SysExcuteQualityImageChanged?.Invoke(this, null);
  //      }


  //      public void UpdateSysEfficiencyComparisonBaseTable()
  //      {
  //          List<Port2Port_TransferInfo> port2Ports = new List<Port2Port_TransferInfo>();
  //          var port_station_list = app.ObjCacheManager.GetPortStations();
  //          List<string> from_ports = port_station_list.Select(port_st => port_st.PORT_ID).ToList();
  //          List<string> to_ports = port_station_list.Select(port_st => port_st.PORT_ID).ToList();
  //          DateTime query_dateTime = DateTime.Now;

  //          foreach (string from_port in from_ports)
  //          {
  //              foreach (string to_port in to_ports)
  //              {
  //                  if (sc.Common.SCUtility.isMatche(from_port, to_port))
  //                      continue;

  //                  Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //                  {
  //                      Field = "@timestamp",
  //                      GreaterThan = query_dateTime.AddDays(-7),
  //                      LessThan = query_dateTime
  //                  };
  //                  Nest.TermsQuery tsq = new Nest.TermsQuery()
  //                  {
  //                      Field = new Nest.Field("SOURCE_ADR.keyword"),
  //                      Terms = new List<string> { from_port.Trim() }
  //                  };
  //                  Nest.TermsQuery tsq1 = new Nest.TermsQuery()
  //                  {
  //                      Field = new Nest.Field("DESTINATION_ADR.keyword"),
  //                      Terms = new List<string> { to_port.Trim() }
  //                  };
  //                  string[] include_column = new string[]
  //                  {
  //                     "@timestamp", "SOURCE_ADR","DESTINATION_ADR","CMD_TOTAL_EXCUTION_TIME"
  //                  };

  //                  var queryList = app.GetElasticSearchManager().Search<sysexcutequality>
  //                      (
  //                      dq,
  //                      new[] { tsq, tsq1 },
  //                      include_column,
  //                      0,
  //                      10000);
  //                  if (queryList.Count == 0)
  //                      continue;
  //                  double min_excute_time_sec = queryList.Min(p => p.CMD_TOTAL_EXCUTION_TIME);

  //                  port2Ports.Add(new Port2Port_TransferInfo()
  //                  {
  //                      SOURCE_ADR = from_port,
  //                      DESTINATION_ADR = to_port,
  //                      CMD_TOTAL_EXCUTION_TIME_MinTime = min_excute_time_sec
  //                  });
  //              }
  //          }

  //      }

  //      public void UpdateSysEfficiencyComparisonBaseTable_parallel()
  //      {
  //          var port_station_list = app.ObjCacheManager.GetPortStations();
  //          List<string> from_ports = port_station_list.Select(port_st => port_st.PORT_ID).ToList();
  //          List<string> to_ports = port_station_list.Select(port_st => port_st.PORT_ID).ToList();
  //          DateTime query_dateTime = DateTime.Now;
  //          BlockingCollection<Port2Port_TransferInfo> listTemp = new BlockingCollection<Port2Port_TransferInfo>();

  //          Parallel.For(0, from_ports.Count(), from_port_index =>
  //          {
  //              Parallel.For(0, to_ports.Count(), to_port_index =>
  //              {
  //                  string from_port = from_ports[from_port_index];
  //                  string to_port = from_ports[to_port_index];
  //                  if (sc.Common.SCUtility.isMatche(from_port, to_port))
  //                      return;

  //                  Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //                  {
  //                      Field = "@timestamp",
  //                      GreaterThan = query_dateTime.AddDays(-7),
  //                      LessThan = query_dateTime
  //                  };
  //                  Nest.TermsQuery tsq = new Nest.TermsQuery()
  //                  {
  //                      Field = new Nest.Field("SOURCE_ADR.keyword"),
  //                      Terms = new List<string> { from_port.Trim() }
  //                  };
  //                  Nest.TermsQuery tsq1 = new Nest.TermsQuery()
  //                  {
  //                      Field = new Nest.Field("DESTINATION_ADR.keyword"),
  //                      Terms = new List<string> { to_port.Trim() }
  //                  };
  //                  string[] include_column = new string[]
  //                  {
  //                     "@timestamp", "SOURCE_ADR","DESTINATION_ADR","CMD_TOTAL_EXCUTION_TIME"
  //                  };

  //                  var queryList = app.GetElasticSearchManager().Search<sysexcutequality>
  //                      (
  //                      dq,
  //                      new[] { tsq, tsq1 },
  //                      include_column,
  //                      0,
  //                      500);
  //                  if (queryList.Count == 0)
  //                      return;
  //                  queryList = queryList.OrderBy(sys_excute => sys_excute.CMD_TOTAL_EXCUTION_TIME).ToList();
  //                  int total_count = queryList.Count;
  //                  double middle_num = total_count / 2;
  //                  int middle_index = Convert.ToInt16(Math.Ceiling(middle_num)) - 1;

  //                  //double min_excute_time_sec = queryList.Min(p => p.CMD_TOTAL_EXCUTION_TIME);
  //                  double middle_excute_time_sec = queryList[middle_index].CMD_TOTAL_EXCUTION_TIME;

  //                  listTemp.Add(new Port2Port_TransferInfo()
  //                  {
  //                      SOURCE_ADR = from_port,
  //                      DESTINATION_ADR = to_port,
  //                      //CMD_TOTAL_EXCUTION_TIME_MinTime = min_excute_time_sec
  //                      CMD_TOTAL_EXCUTION_TIME_MinTime = middle_excute_time_sec
  //                  });
  //              });
  //          });
  //          port2Port_ComparisonBaseTable = listTemp.ToList();
  //          isComparisonBaseTableReady = true;

  //      }

  //      public void CalculationSysEfficiency_60min_ago()
  //      {
  //          if (!isComparisonBaseTableReady)
  //              return;
  //          DateTime query_dateTime = DateTime.Now;
  //          string[] include_column = new string[]
  //                 {
  //                     "@timestamp", "SOURCE_ADR","DESTINATION_ADR","CMD_TOTAL_EXCUTION_TIME"
  //                 };
  //          Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //          {
  //              Field = "@timestamp",
  //              GreaterThan = query_dateTime.AddHours(-1),
  //              LessThan = query_dateTime
  //          };
  //          var queryList = app.GetElasticSearchManager().Search<sysexcutequality>
  //                      (
  //                      dq,
  //                      null,
  //                      include_column,
  //                      0,
  //                      10000);

  //          int total_cmp_cmd = 0;
  //          double excute_efficiency = 0;
  //          foreach (var sys_excute_info in queryList)
  //          {
  //              Port2Port_TransferInfo transferInfo_Base = port2Port_ComparisonBaseTable.
  //                  Where(info => info.SOURCE_ADR.Trim() == sys_excute_info.SOURCE_ADR.Trim() &&
  //                              info.DESTINATION_ADR.Trim() == sys_excute_info.DESTINATION_ADR.Trim()).SingleOrDefault();

  //              excute_efficiency += transferInfo_Base.CMD_TOTAL_EXCUTION_TIME_MinTime / sys_excute_info.CMD_TOTAL_EXCUTION_TIME;
  //          }
  //          total_cmp_cmd = queryList.Count;


  //          OhxCExcuteEffectiveness_TotalCompleteCmd = total_cmp_cmd;
  //          OhxCExcuteEffectiveness_Percentage = (excute_efficiency / total_cmp_cmd) * 100;
  //          OhxCExcuteEffectivenessChanged?.Invoke(this, new[] { OhxCExcuteEffectiveness_TotalCompleteCmd, (int)OhxCExcuteEffectiveness_Percentage });
  //      }

  //      public void updateCurrentMCSCommandExcuteStatus()
  //      {
  //          int cmd_queue = sysExcuteQualityBLL.getMapInfoFromHttp("CommandInQueueCount");
  //          int cmd_excute = sysExcuteQualityBLL.getMapInfoFromHttp("CommandInExcuteCount");
  //          CurrnetMCSCommandCount_All = cmd_queue + cmd_excute;
  //          CurrnetMCSCommandCount_Queue = cmd_queue;

  //          CurrnetMCSCommandCountChanged?.Invoke(this, new int[] { CurrnetMCSCommandCount_All, CurrnetMCSCommandCount_Queue });
  //      }
  //      #endregion Current OHxC Status

  //      #region History OHxC Status
  //      public void indexData<T>(T t) where T : class
  //      {
  //          app.GetElasticSearchManager().insertData(t);
  //      }
  //      public List<ASYSEXCUTEQUALITY> loadSysexcutequalities(DateTime start_time, DateTime end_time, string cstID = null)
  //      {

  //          Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //          {
  //              Field = "@timestamp",
  //              GreaterThan = start_time,
  //              LessThan = end_time
  //          };

  //          List<Nest.TermsQuery> tsqs = new List<Nest.TermsQuery>();
  //          if (cstID != null)
  //          {
  //              tsqs.Add(new Nest.TermsQuery()
  //              {
  //                  Field = new Nest.Field("CST_ID.keyword"),
  //                  Terms = new List<string> { cstID.Trim() }
  //              });
  //          }

  //          var queryList = app.GetElasticSearchManager().Search<ASYSEXCUTEQUALITY>
  //              (
  //              dq,
  //              tsqs.Count > 0 ? tsqs.ToArray() : null,
  //              0,
  //              10000);
  //          return queryList;
  //      }

  //      public List<RecordReportInfo> loadRecodeReportInfo(DateTime start_time, DateTime end_time, string vh_id, string cmd_id)
  //      {

  //          Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //          {
  //              Field = "@timestamp",
  //              GreaterThan = start_time,
  //              LessThan = end_time
  //          };
  //          Nest.TermsQuery tsq = null;
  //          Nest.TermsQuery tsq1 = null;
  //          if (vh_id != null)
  //          {
  //              tsq = new Nest.TermsQuery()
  //              {
  //                  Field = new Nest.Field("VH_ID.keyword"),
  //                  Terms = new List<string> { vh_id.Trim() }
  //              };
  //          }
  //          if (cmd_id != null)
  //          {
  //              tsq1 = new Nest.TermsQuery()
  //              {
  //                  Field = new Nest.Field("MCS_CMD_ID.keyword"),
  //                  Terms = new List<string> { cmd_id.Trim() }
  //              };
  //          }

  //          var queryList = app.GetElasticSearchManager().Search<RecordReportInfo>
  //              (
  //              dq,
  //              new[] { tsq, tsq1 },
  //              0,
  //              9999);
  //          return queryList;
  //      }

  //      public List<ALARM> loadALARMHistory(DateTime start_time, DateTime end_time)
  //      {
  //          //Nest.DateRangeQuery dq = null;
  //          Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //          {
  //              Field = "@timestamp",
  //              GreaterThan = start_time,
  //              LessThan = end_time
  //          };
  //          Nest.TermsQuery tsq = null;
  //          Nest.TermsQuery tsq1 = null;
  //          //if (vh_id != null)
  //          //{
  //          //    tsq = new Nest.TermsQuery()
  //          //    {
  //          //        Field = new Nest.Field("VH_ID.keyword"),
  //          //        Terms = new List<string> { vh_id.Trim() }
  //          //    };
  //          //}
  //          //if (cmd_id != null)
  //          //{
  //          //    tsq1 = new Nest.TermsQuery()
  //          //    {
  //          //        Field = new Nest.Field("MCS_CMD_ID.keyword"),
  //          //        Terms = new List<string> { cmd_id.Trim() }
  //          //    };
  //          //}

  //          var queryList = app.GetElasticSearchManager().Search<com.mirle.ibg3k0.sc.ALARM>
  //              (dq,
  //               null,
  //               0,
  //               9999);
  //          return queryList;
  //      }

  //      public List<RecordReportInfo> loadOHTHHistory(DateTime start_time, DateTime end_time, string funcID = null)
  //      {
  //          //Nest.DateRangeQuery dq = null;
  //          Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //          {
  //              Field = "@timestamp",
  //              GreaterThan = start_time,
  //              LessThan = end_time
  //          };
  //          //Nest.TermsQuery tsq = null;
  //          //Nest.TermsQuery tsq1 = null;
  //          //if (vh_id != null)
  //          //{
  //          //    tsq = new Nest.TermsQuery()
  //          //    {
  //          //        Field = new Nest.Field("VH_ID.keyword"),
  //          //        Terms = new List<string> { vh_id.Trim() }
  //          //    };
  //          //}
  //          //if (cmd_id != null)
  //          //{
  //          //    tsq1 = new Nest.TermsQuery()
  //          //    {
  //          //        Field = new Nest.Field("MCS_CMD_ID.keyword"),
  //          //        Terms = new List<string> { cmd_id.Trim() }
  //          //    };
  //          //}
  //          List<Nest.TermsQuery> tsqs = new List<Nest.TermsQuery>();
  //          if (funcID != null)
  //          {
  //              tsqs.Add(new Nest.TermsQuery()
  //              {
  //                  Field = new Nest.Field("FUN_NAME.keyword"),
  //                  Terms = new List<string> { funcID.Trim() }
  //              });
  //          }

  //          var queryList = app.GetElasticSearchManager().Search<RecordReportInfo>
  //              (
  //              dq,
  //              tsqs.Count > 0 ? tsqs.ToArray() : null,
  //              0,
  //              9999);
  //          return queryList;
  //      }

  // //     public List<com.mirle.ibg3k0.sc.ACMD> loadTransferCommandHistory(DateTime start_time, DateTime end_time)
  // //     {
		//	//Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
		//	//{
		//	//	Field = "@timestamp",
		//	//	GreaterThan = start_time,
		//	//	LessThan = end_time
		//	//};
  // //         //Nest.TermsQuery tsq = null;
  // //         //Nest.TermsQuery tsq1 = null;
  // //         //if (vh_id != null)
  // //         //{
  // //         //    tsq = new Nest.TermsQuery()
  // //         //    {
  // //         //        Field = new Nest.Field("VH_ID.keyword"),
  // //         //        Terms = new List<string> { vh_id.Trim() }
  // //         //    };
  // //         //}
  // //         //if (cmd_id != null)
  // //         //{
  // //         //    tsq1 = new Nest.TermsQuery()
  // //         //    {
  // //         //        Field = new Nest.Field("MCS_CMD_ID.keyword"),
  // //         //        Terms = new List<string> { cmd_id.Trim() }
  // //         //    };
  // //         //}

  // //         var queryList = app.GetElasticSearchManager().Search<com.mirle.ibg3k0.sc.ACMD>
  // //             (
  // //             dq,
  // //             null,
  // //             0,
  // //             9999);
  // //         return queryList;
  // //     }
  //      public List<RecordReportInfo> loadPLCHistory(DateTime start_time, DateTime end_time)
  //      {
  //          //Nest.DateRangeQuery dq = null;
  //          Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
  //          {
  //              Field = "@timestamp",
  //              GreaterThan = start_time,
  //              LessThan = end_time
  //          };
  //          Nest.TermsQuery tsq = null;
  //          Nest.TermsQuery tsq1 = null;

  //          //tsq = new Nest.TermsQuery()
  //          //{
  //          //    Field = new Nest.Field("VH_ID.keyword"),
  //          //    Terms = new List<string> { vh_id.Trim() }
  //          //};


  //          //tsq1 = new Nest.TermsQuery()
  //          //{
  //          //    Field = new Nest.Field("MCS_CMD_ID.keyword"),
  //          //    Terms = new List<string> { cmd_id.Trim() }
  //          //};


  //          var queryList = app.GetElasticSearchManager().Search<RecordReportInfo>
  //              (
  //              dq,
  //              null,
  //              0,
  //              9999);
  //          return queryList;
  //      }
		////public List<com.mirle.ibg3k0.sc.Data.VO.UserOperationLog> loadUserOperationLogHistory(DateTime start_time, DateTime end_time)
		////{
		////	Nest.DateRangeQuery dq = new Nest.DateRangeQuery()
		////	{
		////		Field = "@timestamp",
		////		GreaterThan = start_time,
		////		LessThan = end_time
		////	};
		////	//Nest.TermsQuery tsq = null;
		////	//Nest.TermsQuery tsq1 = null;
		////	//if (vh_id != null)
		////	//{
		////	//    tsq = new Nest.TermsQuery()
		////	//    {
		////	//        Field = new Nest.Field("VH_ID.keyword"),
		////	//        Terms = new List<string> { vh_id.Trim() }
		////	//    };
		////	//}
		////	//if (cmd_id != null)
		////	//{
		////	//    tsq1 = new Nest.TermsQuery()
		////	//    {
		////	//        Field = new Nest.Field("MCS_CMD_ID.keyword"),
		////	//        Terms = new List<string> { cmd_id.Trim() }
		////	//    };
		////	//}

		////	var queryList = app.GetElasticSearchManager().Search<com.mirle.ibg3k0.sc.Data.VO.UserOperationLog>
		////		(
		////		dq,
		////		null,
		////		0,
		////		9999);
		////	return queryList;
		////}
  //      #endregion History OHxC Status
  //  }

  //  public class sysexcutequality
  //  {
  //      private DateTime timestamp;
  //      [Nest.Date(Name = "@timestamp")]
  //      public DateTime TimeStamp
  //      {
  //          get
  //          {
  //              return timestamp.ToLocalTime();
  //          }
  //          set
  //          {
  //              timestamp = value;
  //          }
  //      }

  //      public string SOURCE_ADR;
  //      public string DESTINATION_ADR;
  //      public double CMD_TOTAL_EXCUTION_TIME;
  //  }

  //  public class SystemProcess
  //  {
  //      private DateTime cmd_inster_time;
  //      [Nest.Date(Name = "@timestamp")]
  //      public System.DateTime INSERT_TIME
  //      {
  //          get
  //          {
  //              return cmd_inster_time.ToLocalTime();
  //          }
  //          set
  //          {
  //              cmd_inster_time = value;
  //          }
  //      }
  //  }
  //  public partial class ASYSEXCUTEQUALITY
  //  {
  //      [Nest.Date(Name = "MCS_CMD_ID")]
  //      public string MCS_CMD_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "CST_ID")]
  //      public string CST_ID
  //      {
  //          get; set;
  //      }
  //      private DateTime cmd_inster_time;
  //      [Nest.Date(Name = "@timestamp")]
  //      public System.DateTime CMD_INSERT_TIME
  //      {
  //          get
  //          {
  //              return cmd_inster_time.ToLocalTime();
  //          }
  //          set
  //          {
  //              cmd_inster_time = value;
  //          }
  //      }
  //      [Nest.Date(Name = "CMD_START_TIME")]
  //      public Nullable<System.DateTime> CMD_START_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "CMD_FINISH_TIME")]
  //      public Nullable<System.DateTime> CMD_FINISH_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "CMD_FINISH_STATUS")]
  //      public Nullable<E_CMD_STATUS> CMD_FINISH_STATUS
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "VH_ID")]
  //      public string VH_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "VH_START_SEC_ID")]
  //      public string VH_START_SEC_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SOURCE_ADR")]
  //      public string SOURCE_ADR
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEC_CNT_TO_SOURCE")]
  //      public int SEC_CNT_TO_SOURCE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEC_DIS_TO_SOURCE")]
  //      public int SEC_DIS_TO_SOURCE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "DESTINATION_ADR")]
  //      public string DESTINATION_ADR
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEC_CNT_TO_DESTN")]
  //      public int SEC_CNT_TO_DESTN
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEC_DIS_TO_DESTN")]
  //      public int SEC_DIS_TO_DESTN
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "CMDQUEUE_TIME")]
  //      public double CMDQUEUE_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "MOVE_TO_SOURCE_TIME")]
  //      public double MOVE_TO_SOURCE_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_BLOCK_TIME_TO_SOURCE")]
  //      public double TOTAL_BLOCK_TIME_TO_SOURCE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_OCS_TIME_TO_SOURCE")]
  //      public double TOTAL_OCS_TIME_TO_SOURCE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_BLOCK_COUNT_TO_SOURCE")]
  //      public int TOTAL_BLOCK_COUNT_TO_SOURCE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_OCS_COUNT_TO_SOURCE")]
  //      public int TOTAL_OCS_COUNT_TO_SOURCE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "MOVE_TO_DESTN_TIME")]
  //      public double MOVE_TO_DESTN_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_BLOCK_TIME_TO_DESTN")]
  //      public double TOTAL_BLOCK_TIME_TO_DESTN
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_OCS_TIME_TO_DESTN")]
  //      public double TOTAL_OCS_TIME_TO_DESTN
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_BLOCK_COUNT_TO_DESTN")]
  //      public int TOTAL_BLOCK_COUNT_TO_DESTN
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_OCS_COUNT_TO_DESTN")]
  //      public int TOTAL_OCS_COUNT_TO_DESTN
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTALPAUSE_TIME")]
  //      public double TOTALPAUSE_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "CMD_TOTAL_EXCUTION_TIME")]
  //      public double CMD_TOTAL_EXCUTION_TIME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_ACT_VH_COUNT")]
  //      public int TOTAL_ACT_VH_COUNT
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "PARKING_VH_COUNT")]
  //      public int PARKING_VH_COUNT
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "CYCLERUN_VH_COUNT")]
  //      public int CYCLERUN_VH_COUNT
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "TOTAL_IDLE_VH_COUNT")]
  //      public int TOTAL_IDLE_VH_COUNT
  //      {
  //          get; set;
  //      }

  //  }

  //  public partial class RecordReportInfo
  //  {

  //      private DateTime timestamp;
  //      [Nest.Date(Name = "@timestamp")]
  //      public DateTime Timestamp
  //      {
  //          get
  //          {
  //              return timestamp.ToLocalTime();
  //          }
  //          set
  //          {
  //              timestamp = value;
  //          }
  //      }
  //      [Nest.Date(Name = "MSG_FROM")]
  //      public string MSG_FROM
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "MSG_TO")]
  //      public string MSG_TO
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "FUN_NAME")]
  //      public string FUN_NAME
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEQ_NUM")]
  //      public int SEQ_NUM
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "VH_ID")]
  //      public string VH_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "OHTC_CMD_ID")]
  //      public string OHTC_CMD_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "ACT_TYPE")]
  //      public string ACT_TYPE
  //      {
  //          get; set;
  //      }

  //      //S2F49
  //      [Nest.Date(Name = "MCS_CMD_ID")]
  //      public string MCS_CMD_ID
  //      {
  //          get; set;
  //      }
  //      //132
  //      [Nest.Date(Name = "TRAVEL_DIS")]
  //      public int TRAVEL_DIS
  //      {
  //          get; set;
  //      }
  //      //134 Rep
  //      [Nest.Date(Name = "ADR_ID")]
  //      public string ADR_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEC_ID")]
  //      public string SEC_ID
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "EVENT_TYPE")]
  //      public string EVENT_TYPE
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "SEC_DIS")]
  //      public uint SEC_DIS
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "BLOCK_SEC_ID")]
  //      public string BLOCK_SEC_ID
  //      {
  //          get; set;
  //      }
  //      //134 Reply
  //      [Nest.Date(Name = "IS_BLOCK_PASS")]
  //      public int IS_BLOCK_PASS
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "IS_HID_PASS")]
  //      public int IS_HID_PASS
  //      {
  //          get; set;
  //      }
  //      //144
  //      [Nest.Date(Name = "VH_STATUS")]
  //      public string VH_STATUS
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "MSG_BODY")]
  //      public string MSG_BODY
  //      {
  //          get; set;
  //      }
  //      [Nest.Date(Name = "RESULT")]
  //      public string RESULT
  //      {
  //          get; set;
  //      }
  //  }
  //  public class Port2Port_TransferInfo
  //  {
  //      public string SOURCE_ADR;
  //      public string DESTINATION_ADR;
  //      public double CMD_TOTAL_EXCUTION_TIME_MinTime;
  //  }
}
