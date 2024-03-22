using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public static class Definition
    {
        public enum LogLevel
        {
            Debug = 0,
            Info,
            Warn,
            Error,
            Fatal
        }
        public static LogLevel GetLogLevel(string sLevel)
        {
            LogLevel logLevel = LogLevel.Debug;
            if (!string.IsNullOrWhiteSpace(sLevel))
            {
                switch (sLevel.Trim().ToUpper())
                {
                    case "INFO":
                        logLevel = LogLevel.Info;
                        break;
                    case "WARN":
                        logLevel = LogLevel.Warn;
                        break;
                    case "ERROR":
                        logLevel = LogLevel.Error;
                        break;
                    case "FATAL":
                        logLevel = LogLevel.Fatal;
                        break;
                }
            }
            return logLevel;
        }

        public enum MapInfoType
        {
            MapID,
            EFConnectionString,
            Rail,
            Point,
            GroupRails,
            Address,
            Section,
            Segment,
            Port,
            PortIcon,
            Charger,
            CouplerInfo,
            Vehicle,
            Line,
            BlockZoneDetail,
            Alarm
        }

        public enum LinkStatus
        {
            LinkFail,
            LinkOK
        }

        public enum AngleOfViewType
        {
            degree_0 = 0,
            degree_90 = 90,
            degree_180 = 180,
            degree_270 = 270
        }

        public enum ShelfStatus
        {
            Default = -1,
            Empty,
            Stored,
            PreIn,
            PreOut,
            Alternate
        }
    }
}
