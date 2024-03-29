﻿using com.mirle.ibg3k0.bcf.App;
using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.bcf.Data.ValueDefMapAction;
using com.mirle.ibg3k0.bcf.Data.VO;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Data.SECS;
using com.mirle.ibg3k0.sc.Data.VO;
using com.mirle.ibg3k0.sc.Data.VO.Interface;
using com.mirle.ibg3k0.sc.ObjectRelay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc
{
    public partial class ACMD_MCS
    {
        public static ConcurrentDictionary<string, ACMD_MCS> MCS_CMD_InfoList { get; private set; } = new ConcurrentDictionary<string, ACMD_MCS>();
        public static List<string> loadCurrentTransferCarrierID()
        {
            var carriers_id = MCS_CMD_InfoList.ToArray().Select(kv => sc.Common.SCUtility.Trim(kv.Value.CARRIER_ID)).ToList();
            return carriers_id;
        }

        public int RetryTimes = 0;
        public string ManualSelectedFinishCarrierLoc = "";
        /// <summary>
        /// 1 2 4 8 16 32 64 128
        /// 1 1 1 1 1  1  1  1
        /// 1 0 0 0 ...
        /// 1 1 0 0 ....
        /// 1 1 1 0 ....
        /// </summary>
        public const int COMMAND_STATUS_BIT_INDEX_ENROUTE = 1;
        public const int COMMAND_STATUS_BIT_INDEX_LOAD_ARRIVE = 2;
        public const int COMMAND_STATUS_BIT_INDEX_LOADING = 4;
        public const int COMMAND_STATUS_BIT_INDEX_LOAD_COMPLETE = 8;
        public const int COMMAND_STATUS_BIT_INDEX_UNLOAD_ARRIVE = 16;
        public const int COMMAND_STATUS_BIT_INDEX_UNLOADING = 32;
        public const int COMMAND_STATUS_BIT_INDEX_UNLOAD_COMPLETE = 64;
        public const int COMMAND_STATUS_BIT_INDEX_COMMNAD_FINISH = 128;

        public HCMD_MCS ToHCMD_MCS()
        {
            return new HCMD_MCS()
            {
                CMD_ID = this.CMD_ID,
                CARRIER_ID = this.CARRIER_ID,
                TRANSFERSTATE = this.TRANSFERSTATE,
                COMMANDSTATE = this.COMMANDSTATE,
                HOSTSOURCE = this.HOSTSOURCE,
                HOSTDESTINATION = this.HOSTDESTINATION,
                PRIORITY = this.PRIORITY,
                CHECKCODE = this.CHECKCODE,
                PAUSEFLAG = this.PAUSEFLAG,
                CMD_INSER_TIME = this.CMD_INSER_TIME,
                CMD_START_TIME = this.CMD_START_TIME,
                CMD_FINISH_TIME = this.CMD_FINISH_TIME,
                TIME_PRIORITY = this.TIME_PRIORITY,
                PORT_PRIORITY = this.PORT_PRIORITY,
                PRIORITY_SUM = this.PRIORITY_SUM,
                REPLACE = this.REPLACE
            };
        }
        public string SOURCE_GROUP_NAME
        {
            get
            {
                string group_name = string.Empty;
                string[] stemp = HOSTSOURCE.Split(':');
                if (stemp != null && stemp.Count() > 0)
                {
                    group_name = stemp[0];
                }
                return group_name;
            }
        }
        public string DESTINATION_GROUP_NAME
        {
            get
            {
                string group_name = string.Empty;
                string[] stemp = HOSTDESTINATION.Split(':');
                if (stemp != null && stemp.Count() > 0)
                {
                    group_name = stemp[0];
                }
                return group_name;
            }
        }
        public bool isLoading
        {
            get
            {
                COMMANDSTATE = COMMANDSTATE & 252;
                return COMMANDSTATE == COMMAND_STATUS_BIT_INDEX_LOADING;
            }
        }
        public bool isUnloading
        {

            get
            {
                COMMANDSTATE = COMMANDSTATE & 224;
                return COMMANDSTATE == COMMAND_STATUS_BIT_INDEX_UNLOADING;
            }
        }

        public void put(ACMD_MCS ortherObj)
        {
            CMD_ID = ortherObj.CMD_ID;
            CARRIER_ID = ortherObj.CARRIER_ID;
            TRANSFERSTATE = ortherObj.TRANSFERSTATE;
            COMMANDSTATE = ortherObj.COMMANDSTATE;
            HOSTSOURCE = ortherObj.HOSTSOURCE;
            HOSTDESTINATION = ortherObj.HOSTDESTINATION;
            PRIORITY = ortherObj.PRIORITY;
            CHECKCODE = ortherObj.CHECKCODE;
            PAUSEFLAG = ortherObj.PAUSEFLAG;
            CMD_INSER_TIME = ortherObj.CMD_INSER_TIME;
            CMD_START_TIME = ortherObj.CMD_START_TIME;
            CMD_FINISH_TIME = ortherObj.CMD_FINISH_TIME;
            TIME_PRIORITY = ortherObj.TIME_PRIORITY;
            PORT_PRIORITY = ortherObj.PORT_PRIORITY;
            PRIORITY_SUM = ortherObj.PRIORITY_SUM;
            REPLACE = ortherObj.REPLACE;
        }
    }

}
