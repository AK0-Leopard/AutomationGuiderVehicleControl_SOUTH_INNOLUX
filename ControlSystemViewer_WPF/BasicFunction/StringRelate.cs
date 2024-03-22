using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction
{
    public static class StringRelate
    {
        public static string ExtractConnectionString(string fullConnectionString)
        {
            if (string.IsNullOrWhiteSpace(fullConnectionString)) return "";
            int iStart = fullConnectionString.IndexOf("provider connection string=\"");
            if (iStart < 0) return fullConnectionString;
            else iStart += "provider connection string=\"".Length;
            int iEnd = fullConnectionString.Length - 1;
            for (int i = iStart; i < fullConnectionString.Length; i++)
            {
                if (fullConnectionString[i] == '\"')
                {
                    iEnd = i - 1;
                    break;
                }
            }
            return fullConnectionString.Substring(iStart, iEnd - iStart + 1);
        }

        public static string ConvertStringListToString(List<string> list)
        {
            string rtn = "";
            if (list == null || list.Count == 0) return rtn;

            bool isFirst = true;
            foreach (string str in list)
            {
                if (isFirst) isFirst = false;
                else rtn += "\n";

                rtn += str;
            }
            return rtn;
        }
    }
}
