using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UtilsAPI.Tool
{
    public static class ColorCode
    {
        #region 色碼轉換
        public static Color ConvertStringColorToColor(string strColor)
        {
            Color rtn;
            try
            {
                int a = 255;
                int r = 255;
                int g = 255;
                int b = 255;
                strColor = strColor.Trim();
                if (strColor.Length == 6)
                {
                    r = CovertStringByteToInt(strColor.Substring(0, 2));
                    g = CovertStringByteToInt(strColor.Substring(2, 2));
                    b = CovertStringByteToInt(strColor.Substring(4, 2));
                }
                else if (strColor.Length == 8)
                {
                    a = CovertStringByteToInt(strColor.Substring(0, 2));
                    r = CovertStringByteToInt(strColor.Substring(2, 2));
                    g = CovertStringByteToInt(strColor.Substring(4, 2));
                    b = CovertStringByteToInt(strColor.Substring(6, 2));
                }
                rtn = Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
            }
            catch
            {
                //rtn = Color.FromRgb(0x1E, 0x90, 0xFF);
                rtn = Color.FromRgb(0xFF, 0x00, 0x00);
            }
            return rtn;
        }
        public static int CovertStringByteToInt(string strByte)
        {
            int rtn = 0;
            try
            {
                rtn += CovertCharToInt(strByte[0]) * 16;
                rtn += CovertCharToInt(strByte[1]);
            }
            catch
            {
                rtn = 255;
            }
            return rtn;
        }
        public static int CovertCharToInt(char c)
        {
            int rtn = 0;
            try
            {
                if (c >= '0' && c <= '9')
                    rtn += c - '0';
                else if (c >= 'A' && c <= 'F')
                    rtn += c - 'A' + 10;
                else if (c >= 'a' && c <= 'f')
                    rtn += c - 'a' + 10;
            }
            catch
            {
                rtn = 15;
            }
            return rtn;
        }
        #endregion 色碼轉換
    }
}
