﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc
{
    public partial class ACMD_OHTC
    {


        public HCMD_OHTC ToHCMD_OHTC()
        {
            return new HCMD_OHTC()
            {
                CMD_ID = this.CMD_ID,
                VH_ID = sc.Common.SCUtility.Trim(this.VH_ID),
                CARRIER_ID = this.CARRIER_ID,
                CMD_ID_MCS = this.CMD_ID_MCS,
                CMD_TPYE = this.CMD_TPYE,
                SOURCE = this.SOURCE,
                DESTINATION = this.DESTINATION,
                PRIORITY = this.PRIORITY,
                CMD_START_TIME = this.CMD_START_TIME,
                CMD_END_TIME = this.CMD_END_TIME.HasValue ? this.CMD_END_TIME.Value : DateTime.MinValue,
                CMD_STAUS = this.CMD_STAUS,
                CMD_PROGRESS = this.CMD_PROGRESS,
                INTERRUPTED_REASON = this.INTERRUPTED_REASON,
                ESTIMATED_TIME = this.ESTIMATED_TIME,
                ESTIMATED_EXCESS_TIME = this.ESTIMATED_EXCESS_TIME,
                REAL_CMP_TIME = this.REAL_CMP_TIME,
                COMPLETE_STATUS = this.COMPLETE_STATUS
            };
        }

        public override string ToString()
        {
            return $"command id:{Common.SCUtility.Trim(CMD_ID, true)}, mcs cmd id:{Common.SCUtility.Trim(CMD_ID_MCS, true)}, cmd type:{CMD_TPYE}, source:{Common.SCUtility.Trim(SOURCE, true)}, dest:{Common.SCUtility.Trim(DESTINATION, true)}, status:{CMD_STAUS}," +
                   $" start time:{CMD_START_TIME?.ToString(App.SCAppConstants.DateTimeFormat_19)}, end time:{CMD_END_TIME?.ToString(App.SCAppConstants.DateTimeFormat_19)}";
        }
    }
}
