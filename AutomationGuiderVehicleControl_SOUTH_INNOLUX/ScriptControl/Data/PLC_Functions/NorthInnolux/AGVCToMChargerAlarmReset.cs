﻿using com.mirle.ibg3k0.sc.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.Data.PLC_Functions.NorthInnolux
{
    class AGVCToMChargerAlarmReset: PLC_FunBase
    {
        [PLCElement(ValueName = "AGVC_TO_MCHARGER_ALARM_RESET_INDEX")]
        public UInt16 Index;
    }


}
