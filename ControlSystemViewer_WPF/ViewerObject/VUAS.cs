using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public static class UAS_Def
    {
        public class System_Function
        {
            public const string FUNC_LOGIN = "FUNC_LOGIN";
            public const string FUNC_CLOSE_SYSTEM = "FUNC_CLOSE_SYSTEM";
            public const string FUNC_CLOSE_MASTER_PC = "FUNC_CLOSE_MASTER_PC";
            public const string FUNC_USER_MANAGEMENT = "FUNC_USER_MANAGEMENT";
        }

        public class Operation_Function
        {
            public const string FUNC_OPERATION_FUN = "FUNC_OPERATION_FUN"; // MenuBar第一層 => 若DB有，子功能全開(.Enabled = true) / DB沒有，子功能獨立判斷，若有任一子功能有，第一層也要開

            public const string FUNC_SYSTEM_CONCROL_MODE = "FUNC_SYSTEM_CONCROL_MODE";
            public const string FUNC_TRANSFER_MANAGEMENT = "FUNC_TRANSFER_MANAGEMENT";
            public const string FUNC_ROAD_CONTROL = "FUNC_ROAD_CONTROL";
            public const string FUNC_OHT_CONTROL = "FUNC_OHT_CONTROL";
            public const string FUNC_PORT_CONTROL = "FUNC_PORT_CONTROL";
            public const string FUNC_MULTI_TRANFER_COMMAND = "FUNC_MULTI_TRANFER_COMMAND";
            public const string FUNC_TEST_RUN = "FUNC_TEST_RUN";
        }

        public class Maintenance_Function
        {
            public const string FUNC_MAINTENANCE_FUN = "FUNC_MAINTENANCE_FUN"; // MenuBar第一層 => 若DB有，子功能全開(.Enabled = true) / DB沒有，子功能獨立判斷，若有任一子功能有，第一層也要開

            public const string FUNC_VEHICLE_MANAGEMENT = "FUNC_VEHICLE_MANAGEMENT";
            public const string FUNC_MTS_MTL_MAINTENANCE = "FUNC_MTS_MTL_MAINTENANCE";
            public const string FUNC_PORT_MAINTENANCE = "FUNC_PORT_MAINTENANCE";
            //public const string FUNC_ADVANCED_SETTINGS = "FUNC_ADVANCED_SETTINGS";
            //public const string FUNC_ALARM_MAINTENANCE = "FUNC_ALARM_MAINTENANCE";
            public const string FUNC_ZONE_SHELF_MAINTENANCE = "FUNC_ZONE_SHELF_MAINTENANCE";
            public const string FUNC_CASSETTE_MAINTENANCE = "FUNC_CASSETTE_MAINTENANCE";
            public const string FUNC_COMMUNICATION_MAINTENANCE = "FUNC_COMMUNICATION_MAINTENANCE";

        }
    }

    public class VUASUSR
    {
        public VUASUSR()
        {
        }

        public string USER_ID { get; set; } = "";
        public string PASSWD { get; set; } = "";
        public string BADGE_NUMBER { get; set; } = "";
        public string USER_NAME { get; set; } = "";
        public string DISABLE_FLG { get; set; } = "";
        public string POWER_USER_FLG { get; set; } = "";
        public string ADMIN_FLG { get; set; } = "";
        public string USER_GRP { get; set; } = "";
        public string DEPARTMENT { get; set; } = "";

        public bool IS_ACTIVE => DISABLE_FLG != "Y";
        public bool IS_POWER_USER => POWER_USER_FLG == "Y";
        public bool IS_ADMIN => ADMIN_FLG == "Y";
    }

    public class VUASUSRGRP
    {
        public VUASUSRGRP()
        {
        }

        public string USER_GRP { get; set; } = "";
    }

    public class VUASUFNC
    {
        public VUASUFNC()
        {
        }

        public string USER_GRP { get; set; } = "";
        public string FUNC_CODE { get; set; } = "";
    }

    public class VUASFNC
    {
        public VUASFNC()
        {
        }

        public string FUNC_CODE { get; set; } = "";
        public string FUNC_NAME { get; set; } = "";
    }
}
