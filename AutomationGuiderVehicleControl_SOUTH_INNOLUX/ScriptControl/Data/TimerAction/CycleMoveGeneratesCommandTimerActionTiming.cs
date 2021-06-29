// ***********************************************************************
// Assembly         : ScriptControl
// Author           : 
// Created          : 03-31-2016
//
// Last Modified By : 
// Last Modified On : 03-24-2016
// ***********************************************************************
// <copyright file="BCSystemStatusTimer.cs" company="">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.mirle.ibg3k0.bcf.Data.TimerAction;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.Data.DAO;
using com.mirle.ibg3k0.sc.Data.SECS;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using NLog;

namespace com.mirle.ibg3k0.sc.Data.TimerAction
{
    public class CycleMoveGeneratesCommandTimerActionTiming : ITimerAction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected SCApplication scApp = null;

        public Dictionary<int, List<TranTask>> dicTranTaskSchedule = null;
        public int MCS_TaskIndex = 0;//420


        public CycleMoveGeneratesCommandTimerActionTiming(string name, long intervalMilliSec)
            : base(name, intervalMilliSec)
        {

        }
        public override void initStart()
        {
            scApp = SCApplication.getInstance();

        }
        Random rnd_Index = new Random(Guid.NewGuid().GetHashCode());
        private long syncPoint = 0;
        public override void doProcess(object obj)
        {
            if (!DebugParameter.CanAutoRandomGeneratesCommand) return;
            if (System.Threading.Interlocked.Exchange(ref syncPoint, 1) == 0)
            {
                try
                {
                    if (SCUtility.isEmpty(DebugParameter.SelectedCycleRunVh)) return;
                    var selected_vh = scApp.VehicleBLL.cache.getVehicle(DebugParameter.SelectedCycleRunVh);

                    if (!selected_vh.isTcpIpConnect ||
                        selected_vh.MODE_STATUS == VHModeStatus.Manual ||
                        scApp.CMDBLL.isCMD_OHTCQueueByVh(selected_vh.VEHICLE_ID))
                    {
                        return;
                    }

                    var addresses = scApp.AddressesBLL.cache.GetAddresses();
                    var cycle_run_adr = addresses.Where(adr => adr.cycleRunIndex != 0);
                    var group_cyc_run_adr = from adr in cycle_run_adr
                                            group adr by adr.cycleRunCount into grp
                                            orderby grp.Key
                                            select grp;
                    var group_result = group_cyc_run_adr.ToDictionary(grp => grp.Key, grp => grp.Select(rptid => rptid).ToList());
                    var cycle_run_count_min = group_result.First();
                    var wait_cycle_address = cycle_run_count_min.Value.OrderBy(adr => adr.cycleRunCount).ToList();
                    AADDRESS selected_adr = null;
                    switch (DebugParameter.CycleRunType)
                    {
                        case DebugParameter.CycleRun.Order:
                            selected_adr = wait_cycle_address.OrderBy(adr => adr.cycleRunIndex).FirstOrDefault();
                            break;
                        case DebugParameter.CycleRun.Random:
                            int task_RandomIndex = rnd_Index.Next(wait_cycle_address.Count() - 1);
                            selected_adr = wait_cycle_address[task_RandomIndex];
                            break;
                        default:
                            selected_adr = wait_cycle_address.OrderBy(adr => adr.cycleRunIndex).FirstOrDefault();
                            break;
                    }
                    bool is_success = creatMoveCommand(selected_vh.VEHICLE_ID, selected_adr.ADR_ID);
                    if (is_success)
                    {
                        selected_adr.cycleRunCount++;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                }
                finally
                {
                    System.Threading.Interlocked.Exchange(ref syncPoint, 0);
                }
            }
        }

        public bool creatMoveCommand(string vhID, string targetAdr)
        {
            return scApp.CMDBLL.doCreatTransferCommand(vhID, string.Empty, string.Empty,
                                                  E_CMD_TYPE.Move,
                                                  string.Empty,
                                                  targetAdr, 0, 0);
        }
    }

}

