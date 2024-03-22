using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ViewerObject
{
    public static class LogStatus
    {
        public static bool openlog = false;
        public static string oldLevel = "Debug";
        public static string nowLevel = "Debug";
    }
}
