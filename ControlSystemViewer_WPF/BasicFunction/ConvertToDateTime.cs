using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction
{
    public static class ConvertToDateTime
    {
        public static DateTime FromString(string sDateTime)
        {
            try
            {
                return Convert.ToDateTime(sDateTime);
            }
            catch
            {
                int length = sDateTime.Length;
                if (length >= 8) // yyyyMMdd to yyyy-MM-dd
                {
                    sDateTime = sDateTime.Insert(4, "-");
                    sDateTime = sDateTime.Insert(7, "-");
                }
                if (length >= 12) // yyyyMMddhhmm to yyyy-MM-dd hh:mm
                {
                    sDateTime = sDateTime.Insert(10, " ");
                    sDateTime = sDateTime.Insert(13, ":");
                }
                if (length >= 14) // yyyyMMddhhmmss to yyyy-MM-dd hh:mm:ss
                {
                    sDateTime = sDateTime.Insert(16, ":");
                }
                if (length > 14) // yyyyMMddhhmmssffffff to yyyy-MM-dd hh:mm:ss.ffffff
                {
                    sDateTime = sDateTime.Insert(19, ".");
                }
                return Convert.ToDateTime(sDateTime);
            }
        }
    }
}
