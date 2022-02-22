using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace com.mirle.ibg3k0.sc.Common
{
    public static class LogStatus
    {
        //因邏輯簡化，將複數人邏輯註解處理

        //public static int Debug = 0;
        //public static int Info = 0;
        //public static int Warn = 0;
        //public static int Error = 0;
        //public static int Fatal = 0;
        public static string sLogLevel = "Debug";

        public static bool AddLevel(string newLevel)
        {
            bool result = false;
            //switch(newLevel)
            //{
            //    case "Debug":
            //        Debug += 1;
            //        break;
            //    case "Info":
            //        Info += 1;
            //        break;
            //    case "Warn":
            //        Warn += 1;
            //        break;
            //    case "Error":
            //        Error += 1;
            //        break;
            //    case "Fatal":
            //        Fatal += 1;
            //        break;
            //}
            sLogLevel = newLevel;
            result = true;
            return result;
        }

        //public static bool RemoveLevel(string oldLevel)
        //{
        //    bool result = false;
        //    switch (oldLevel)
        //    {
        //        case "Debug":
        //            Debug -= 1;
        //            break;
        //        case "Info":
        //            Info -= 1;
        //            break;
        //        case "Warn":
        //            Warn -= 1;
        //            break;
        //        case "Error":
        //            Error -= 1;
        //            break;
        //        case "Fatal":
        //            Fatal -= 1;
        //            break;
        //    }
        //    result = true;
        //    return result;
        //}

        public static bool LevelChange(string oldLevel, string newLevel)
        {
            bool result = false;
            //RemoveLevel(oldLevel);
            //AddLevel(newLevel);
            sLogLevel = newLevel;
            result = true;
            return result;
        }

        private static List<string> getLevel()
        {
            List<string> lsLevel = new List<string>() { "Debug", "Info", "Warn", "Error", "Fatal" };
            //if (Debug > 0) return lsLevel;
            //lsLevel.RemoveAt(0);
            //if (Info > 0) return lsLevel;
            //lsLevel.RemoveAt(0);
            //if (Warn > 0) return lsLevel;
            //lsLevel.RemoveAt(0);
            //if (Error > 0) return lsLevel;
            //lsLevel.RemoveAt(0);
            //if (Fatal > 0) return lsLevel;
            //lsLevel.RemoveAt(0);
            int iLogLevel = lsLevel.IndexOf(sLogLevel);
            int i = 0;
            while (i < iLogLevel)
            {
                lsLevel.RemoveAt(0);
                i++;
            }
            return lsLevel;
        }

        public static bool CheckLevel(string level)
        {
            return getLevel().Contains(level);
        }


    }
}
