//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace com.mirle.ibg3k0.ohxc.wpf.Common
{
    public class WinFromUtility
    {
        #region Pixels & RealLength 的轉換
        private static int scale = 0;
        public static void setScale(int _scale, int zoon_Factor)
        {
            //scale = _scale * 100;
            scale = _scale * zoon_Factor;
        }
        public static double RealLengthToPixelsWidthByScale(double length)
        {
            double length_cm = lengthTransferByScale(length, scale);//1cm:10m
            double length_mm = length_cm * Math.Pow(10, -2) * Math.Pow(10, 3);
            return MillimetersToPixelsWidth(length_mm);
        }
        public static double PixelsWidthToRealLengthByScale(double pixel)
        {
            double length_mm = PixelsWidthToMillimeters(pixel);//1cm:10m
            double length_cm = length_mm * Math.Pow(10, 2) * Math.Pow(10, -3);
            return lengthTransfer2RealLengthByScale(length_cm, scale);
        }

        public static double lengthTransferByScale(double length, double scale)
        {
            return length / scale;
        }

        public static double lengthTransfer2RealLengthByScale(double length, double scale)
        {
            return length * scale;
        }


        public static double MillimetersToPixelsWidth(double length) //length是mm，1厘米=10毫米
        {
            //System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(p.Handle);
            //IntPtr hdc = g.GetHdc();
            //int width = GetDeviceCaps(hdc, 4);     // HORZRES  物理的寬度
            //int pixels = GetDeviceCaps(hdc, 8);     // BITSPIXEL
            int width = 508;                        // HORZRES  物理的寬度
            int pixels = 1920;                      // BITSPIXEL
            //g.ReleaseHdc(hdc);
            return (((double)pixels / (double)width) * (double)length);
        }

        public static double PixelsWidthToMillimeters(double PixelsWidth) //length是毫米，1厘米=10毫米
        {
            //System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(p.Handle);
            //IntPtr hdc = g.GetHdc();
            //int width = GetDeviceCaps(hdc, 4);     // HORZRES  物理的寬度
            //int pixels = GetDeviceCaps(hdc, 8);     // BITSPIXEL  解析度
            int width = 508;                        // HORZRES  物理的寬度
            int pixels = 1920;                      // BITSPIXEL   解析度
            //g.ReleaseHdc(hdc);

            return (((double)width / (double)pixels) * (double)PixelsWidth);
        }
        #endregion Pixels & RealLength 的轉換
        public static Color ConvStr2Color(string sText)
        {
            Color clrData;
            sText = (sText != null) ? sText.Trim() : sText;
            clrData = Color.FromName(sText);
            if (!clrData.IsKnownColor)
            {
                clrData = Color.FromArgb(int.Parse(sText, NumberStyles.AllowHexSpecifier));
            }

            return (clrData);
        }

        //public static void setComboboxDataSource(ComboBox crl_comboBox, string[] data_Source)
        //{
        //    crl_comboBox.DataSource = data_Source;
        //    if (crl_comboBox.AutoCompleteCustomSource.Count != 0)
        //    {
        //        crl_comboBox.AutoCompleteCustomSource.Clear();
        //    }
        //    crl_comboBox.AutoCompleteCustomSource.AddRange(data_Source);
        //    crl_comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
        //    crl_comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
        //}
        /// <summary>
        /// 解壓縮BytyArray資料
        /// </summary>
        /// <param name="compressString">The compress string.</param>
        /// <returns>System.String.</returns>
        public static byte[] unCompressString(string compressString)
        {
            byte[] zippedData = Convert.FromBase64String(compressString.ToString());
            System.IO.MemoryStream ms = new System.IO.MemoryStream(zippedData);
            System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);
            System.IO.MemoryStream outBuffer = new System.IO.MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }

        /// <summary>
        /// Converts an Olson time zone ID to a Windows time zone ID.
        /// </summary>
        /// <param name="olsonTimeZoneId">An Olson time zone ID. See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html. </param>
        /// <returns>
        /// The TimeZoneInfo corresponding to the Olson time zone ID, 
        /// or null if you passed in an invalid Olson time zone ID.
        /// </returns>
        /// <remarks>
        /// See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html
        /// </remarks>
        public static TimeZoneInfo OlsonTimeZoneToTimeZoneInfo(string olsonTimeZoneId)
        {
            var olsonWindowsTimes = new Dictionary<string, string>()
    {
        { "Africa/Bangui", "W. Central Africa Standard Time" },
        { "Africa/Cairo", "Egypt Standard Time" },
        { "Africa/Casablanca", "Morocco Standard Time" },
        { "Africa/Harare", "South Africa Standard Time" },
        { "Africa/Johannesburg", "South Africa Standard Time" },
        { "Africa/Lagos", "W. Central Africa Standard Time" },
        { "Africa/Monrovia", "Greenwich Standard Time" },
        { "Africa/Nairobi", "E. Africa Standard Time" },
        { "Africa/Windhoek", "Namibia Standard Time" },
        { "America/Anchorage", "Alaskan Standard Time" },
        { "America/Argentina/San_Juan", "Argentina Standard Time" },
        { "America/Asuncion", "Paraguay Standard Time" },
        { "America/Bahia", "Bahia Standard Time" },
        { "America/Bogota", "SA Pacific Standard Time" },
        { "America/Buenos_Aires", "Argentina Standard Time" },
        { "America/Caracas", "Venezuela Standard Time" },
        { "America/Cayenne", "SA Eastern Standard Time" },
        { "America/Chicago", "Central Standard Time" },
        { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
        { "America/Cuiaba", "Central Brazilian Standard Time" },
        { "America/Denver", "Mountain Standard Time" },
        { "America/Fortaleza", "SA Eastern Standard Time" },
        { "America/Godthab", "Greenland Standard Time" },
        { "America/Guatemala", "Central America Standard Time" },
        { "America/Halifax", "Atlantic Standard Time" },
        { "America/Indianapolis", "US Eastern Standard Time" },
        { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
        { "America/La_Paz", "SA Western Standard Time" },
        { "America/Los_Angeles", "Pacific Standard Time" },
        { "America/Mexico_City", "Mexico Standard Time" },
        { "America/Montevideo", "Montevideo Standard Time" },
        { "America/New_York", "Eastern Standard Time" },
        { "America/Noronha", "UTC-02" },
        { "America/Phoenix", "US Mountain Standard Time" },
        { "America/Regina", "Canada Central Standard Time" },
        { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
        { "America/Santiago", "Pacific SA Standard Time" },
        { "America/Sao_Paulo", "E. South America Standard Time" },
        { "America/St_Johns", "Newfoundland Standard Time" },
        { "America/Tijuana", "Pacific Standard Time" },
        { "Antarctica/McMurdo", "New Zealand Standard Time" },
        { "Atlantic/South_Georgia", "UTC-02" },
        { "Asia/Almaty", "Central Asia Standard Time" },
        { "Asia/Amman", "Jordan Standard Time" },
        { "Asia/Baghdad", "Arabic Standard Time" },
        { "Asia/Baku", "Azerbaijan Standard Time" },
        { "Asia/Bangkok", "SE Asia Standard Time" },
        { "Asia/Beirut", "Middle East Standard Time" },
        { "Asia/Calcutta", "India Standard Time" },
        { "Asia/Colombo", "Sri Lanka Standard Time" },
        { "Asia/Damascus", "Syria Standard Time" },
        { "Asia/Dhaka", "Bangladesh Standard Time" },
        { "Asia/Dubai", "Arabian Standard Time" },
        { "Asia/Irkutsk", "North Asia East Standard Time" },
        { "Asia/Jerusalem", "Israel Standard Time" },
        { "Asia/Kabul", "Afghanistan Standard Time" },
        { "Asia/Kamchatka", "Kamchatka Standard Time" },
        { "Asia/Karachi", "Pakistan Standard Time" },
        { "Asia/Katmandu", "Nepal Standard Time" },
        { "Asia/Kolkata", "India Standard Time" },
        { "Asia/Krasnoyarsk", "North Asia Standard Time" },
        { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
        { "Asia/Kuwait", "Arab Standard Time" },
        { "Asia/Magadan", "Magadan Standard Time" },
        { "Asia/Muscat", "Arabian Standard Time" },
        { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
        { "Asia/Oral", "West Asia Standard Time" },
        { "Asia/Rangoon", "Myanmar Standard Time" },
        { "Asia/Riyadh", "Arab Standard Time" },
        { "Asia/Seoul", "Korea Standard Time" },
        { "Asia/Shanghai", "China Standard Time" },
        { "Asia/Singapore", "Singapore Standard Time" },
        { "Asia/Taipei", "Taipei Standard Time" },
        { "Asia/Tashkent", "West Asia Standard Time" },
        { "Asia/Tbilisi", "Georgian Standard Time" },
        { "Asia/Tehran", "Iran Standard Time" },
        { "Asia/Tokyo", "Tokyo Standard Time" },
        { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
        { "Asia/Vladivostok", "Vladivostok Standard Time" },
        { "Asia/Yakutsk", "Yakutsk Standard Time" },
        { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
        { "Asia/Yerevan", "Armenian Standard Time" },
        { "Atlantic/Azores", "Azores Standard Time" },
        { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
        { "Atlantic/Reykjavik", "Greenwich Standard Time" },
        { "Australia/Adelaide", "Cen. Australia Standard Time" },
        { "Australia/Brisbane", "E. Australia Standard Time" },
        { "Australia/Darwin", "AUS Central Standard Time" },
        { "Australia/Hobart", "Tasmania Standard Time" },
        { "Australia/Perth", "W. Australia Standard Time" },
        { "Australia/Sydney", "AUS Eastern Standard Time" },
        { "Etc/GMT", "UTC" },
        { "Etc/GMT+11", "UTC-11" },
        { "Etc/GMT+12", "Dateline Standard Time" },
        { "Etc/GMT+2", "UTC-02" },
        { "Etc/GMT-12", "UTC+12" },
        { "Europe/Amsterdam", "W. Europe Standard Time" },
        { "Europe/Athens", "GTB Standard Time" },
        { "Europe/Belgrade", "Central Europe Standard Time" },
        { "Europe/Berlin", "W. Europe Standard Time" },
        { "Europe/Brussels", "Romance Standard Time" },
        { "Europe/Budapest", "Central Europe Standard Time" },
        { "Europe/Dublin", "GMT Standard Time" },
        { "Europe/Helsinki", "FLE Standard Time" },
        { "Europe/Istanbul", "GTB Standard Time" },
        { "Europe/Kiev", "FLE Standard Time" },
        { "Europe/London", "GMT Standard Time" },
        { "Europe/Minsk", "E. Europe Standard Time" },
        { "Europe/Moscow", "Russian Standard Time" },
        { "Europe/Paris", "Romance Standard Time" },
        { "Europe/Sarajevo", "Central European Standard Time" },
        { "Europe/Warsaw", "Central European Standard Time" },
        { "Indian/Mauritius", "Mauritius Standard Time" },
        { "Pacific/Apia", "Samoa Standard Time" },
        { "Pacific/Auckland", "New Zealand Standard Time" },
        { "Pacific/Fiji", "Fiji Standard Time" },
        { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
        { "Pacific/Guam", "West Pacific Standard Time" },
        { "Pacific/Honolulu", "Hawaiian Standard Time" },
        { "Pacific/Pago_Pago", "UTC-11" },
        { "Pacific/Port_Moresby", "West Pacific Standard Time" },
        { "Pacific/Tongatapu", "Tonga Standard Time" }
    };

            var windowsTimeZoneId = default(string);
            var windowsTimeZone = default(TimeZoneInfo);
            if (olsonWindowsTimes.TryGetValue(olsonTimeZoneId, out windowsTimeZoneId))
            {
                try { windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId); }
                catch (TimeZoneNotFoundException) { }
                catch (InvalidTimeZoneException) { }
            }
            return windowsTimeZone;
        }

        /// <summary>
        /// Converts a Windows time zone ID to an Olson time zone ID .
        /// </summary>
        /// <param name="olsonTimeZoneId">An Olson time zone ID. See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html. </param>
        /// <returns>
        /// The TimeZoneInfo corresponding to the Olson time zone ID, 
        /// or null if you passed in an invalid Olson time zone ID.
        /// </returns>
        /// <remarks>
        /// See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html
        /// </remarks>
        public static string TimeZoneInfoIDToOlsonTimeZoneID(TimeZoneInfo timeZoneInfo)
        {
            var olsonWindowsTimes = new Dictionary<string, string>()
    {
        { "Africa/Bangui", "W. Central Africa Standard Time" },
        { "Africa/Cairo", "Egypt Standard Time" },
        { "Africa/Casablanca", "Morocco Standard Time" },
        { "Africa/Harare", "South Africa Standard Time" },
        { "Africa/Johannesburg", "South Africa Standard Time" },
        { "Africa/Lagos", "W. Central Africa Standard Time" },
        { "Africa/Monrovia", "Greenwich Standard Time" },
        { "Africa/Nairobi", "E. Africa Standard Time" },
        { "Africa/Windhoek", "Namibia Standard Time" },
        { "America/Anchorage", "Alaskan Standard Time" },
        { "America/Argentina/San_Juan", "Argentina Standard Time" },
        { "America/Asuncion", "Paraguay Standard Time" },
        { "America/Bahia", "Bahia Standard Time" },
        { "America/Bogota", "SA Pacific Standard Time" },
        { "America/Buenos_Aires", "Argentina Standard Time" },
        { "America/Caracas", "Venezuela Standard Time" },
        { "America/Cayenne", "SA Eastern Standard Time" },
        { "America/Chicago", "Central Standard Time" },
        { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
        { "America/Cuiaba", "Central Brazilian Standard Time" },
        { "America/Denver", "Mountain Standard Time" },
        { "America/Fortaleza", "SA Eastern Standard Time" },
        { "America/Godthab", "Greenland Standard Time" },
        { "America/Guatemala", "Central America Standard Time" },
        { "America/Halifax", "Atlantic Standard Time" },
        { "America/Indianapolis", "US Eastern Standard Time" },
        { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
        { "America/La_Paz", "SA Western Standard Time" },
        { "America/Los_Angeles", "Pacific Standard Time" },
        { "America/Mexico_City", "Mexico Standard Time" },
        { "America/Montevideo", "Montevideo Standard Time" },
        { "America/New_York", "Eastern Standard Time" },
        { "America/Noronha", "UTC-02" },
        { "America/Phoenix", "US Mountain Standard Time" },
        { "America/Regina", "Canada Central Standard Time" },
        { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
        { "America/Santiago", "Pacific SA Standard Time" },
        { "America/Sao_Paulo", "E. South America Standard Time" },
        { "America/St_Johns", "Newfoundland Standard Time" },
        { "America/Tijuana", "Pacific Standard Time" },
        { "Antarctica/McMurdo", "New Zealand Standard Time" },
        { "Atlantic/South_Georgia", "UTC-02" },
        { "Asia/Almaty", "Central Asia Standard Time" },
        { "Asia/Amman", "Jordan Standard Time" },
        { "Asia/Baghdad", "Arabic Standard Time" },
        { "Asia/Baku", "Azerbaijan Standard Time" },
        { "Asia/Bangkok", "SE Asia Standard Time" },
        { "Asia/Beirut", "Middle East Standard Time" },
        { "Asia/Calcutta", "India Standard Time" },
        { "Asia/Colombo", "Sri Lanka Standard Time" },
        { "Asia/Damascus", "Syria Standard Time" },
        { "Asia/Dhaka", "Bangladesh Standard Time" },
        { "Asia/Dubai", "Arabian Standard Time" },
        { "Asia/Irkutsk", "North Asia East Standard Time" },
        { "Asia/Jerusalem", "Israel Standard Time" },
        { "Asia/Kabul", "Afghanistan Standard Time" },
        { "Asia/Kamchatka", "Kamchatka Standard Time" },
        { "Asia/Karachi", "Pakistan Standard Time" },
        { "Asia/Katmandu", "Nepal Standard Time" },
        { "Asia/Kolkata", "India Standard Time" },
        { "Asia/Krasnoyarsk", "North Asia Standard Time" },
        { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
        { "Asia/Kuwait", "Arab Standard Time" },
        { "Asia/Magadan", "Magadan Standard Time" },
        { "Asia/Muscat", "Arabian Standard Time" },
        { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
        { "Asia/Oral", "West Asia Standard Time" },
        { "Asia/Rangoon", "Myanmar Standard Time" },
        { "Asia/Riyadh", "Arab Standard Time" },
        { "Asia/Seoul", "Korea Standard Time" },
        { "Asia/Shanghai", "China Standard Time" },
        { "Asia/Singapore", "Singapore Standard Time" },
        { "Asia/Taipei", "Taipei Standard Time" },
        { "Asia/Tashkent", "West Asia Standard Time" },
        { "Asia/Tbilisi", "Georgian Standard Time" },
        { "Asia/Tehran", "Iran Standard Time" },
        { "Asia/Tokyo", "Tokyo Standard Time" },
        { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
        { "Asia/Vladivostok", "Vladivostok Standard Time" },
        { "Asia/Yakutsk", "Yakutsk Standard Time" },
        { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
        { "Asia/Yerevan", "Armenian Standard Time" },
        { "Atlantic/Azores", "Azores Standard Time" },
        { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
        { "Atlantic/Reykjavik", "Greenwich Standard Time" },
        { "Australia/Adelaide", "Cen. Australia Standard Time" },
        { "Australia/Brisbane", "E. Australia Standard Time" },
        { "Australia/Darwin", "AUS Central Standard Time" },
        { "Australia/Hobart", "Tasmania Standard Time" },
        { "Australia/Perth", "W. Australia Standard Time" },
        { "Australia/Sydney", "AUS Eastern Standard Time" },
        { "Etc/GMT", "UTC" },
        { "Etc/GMT+11", "UTC-11" },
        { "Etc/GMT+12", "Dateline Standard Time" },
        { "Etc/GMT+2", "UTC-02" },
        { "Etc/GMT-12", "UTC+12" },
        { "Europe/Amsterdam", "W. Europe Standard Time" },
        { "Europe/Athens", "GTB Standard Time" },
        { "Europe/Belgrade", "Central Europe Standard Time" },
        { "Europe/Berlin", "W. Europe Standard Time" },
        { "Europe/Brussels", "Romance Standard Time" },
        { "Europe/Budapest", "Central Europe Standard Time" },
        { "Europe/Dublin", "GMT Standard Time" },
        { "Europe/Helsinki", "FLE Standard Time" },
        { "Europe/Istanbul", "GTB Standard Time" },
        { "Europe/Kiev", "FLE Standard Time" },
        { "Europe/London", "GMT Standard Time" },
        { "Europe/Minsk", "E. Europe Standard Time" },
        { "Europe/Moscow", "Russian Standard Time" },
        { "Europe/Paris", "Romance Standard Time" },
        { "Europe/Sarajevo", "Central European Standard Time" },
        { "Europe/Warsaw", "Central European Standard Time" },
        { "Indian/Mauritius", "Mauritius Standard Time" },
        { "Pacific/Apia", "Samoa Standard Time" },
        { "Pacific/Auckland", "New Zealand Standard Time" },
        { "Pacific/Fiji", "Fiji Standard Time" },
        { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
        { "Pacific/Guam", "West Pacific Standard Time" },
        { "Pacific/Honolulu", "Hawaiian Standard Time" },
        { "Pacific/Pago_Pago", "UTC-11" },
        { "Pacific/Port_Moresby", "West Pacific Standard Time" },
        { "Pacific/Tongatapu", "Tonga Standard Time" }
    };

            var olsonTimeZoneId = olsonWindowsTimes.FirstOrDefault(q => q.Value == timeZoneInfo.Id).Key;
            return olsonTimeZoneId;
        }
        public static bool isMatch(string str1, string str2)
        {
            if (str1 == null || str2 == null) return false;
            return str1.Trim() == str2.Trim();
        }

    }

    public static class ObjectPutExtension
    {
        //public static void set(this sc.Data.VO.MaintainSpace mts, sc.ProtocolFormat.OHTMessage.MTL_MTS_INFO new_mtl_mts)
        //{
        //    mts.Plc_Link_Stat = new_mtl_mts.NetworkLink ? sc.App.SCAppConstants.LinkStatus.LinkOK : sc.App.SCAppConstants.LinkStatus.LinkFail;
        //    mts.Is_Eq_Alive = new_mtl_mts.Alive;
        //    mts.MTxMode = new_mtl_mts.Mode ? sc.ProtocolFormat.OHTMessage.MTxMode.Auto : sc.ProtocolFormat.OHTMessage.MTxMode.Manual;
        //    mts.Interlock = new_mtl_mts.Interlock;
        //    mts.CurrentCarID = new_mtl_mts.CarID;
        //    mts.SynchronizeTime = Convert.ToDateTime(new_mtl_mts.SynchronizeTime);
        //}

        //public static void set(this sc.Data.VO.MaintainLift mtl, sc.ProtocolFormat.OHTMessage.MTL_MTS_INFO new_mtl_mts)
        //{
        //    mtl.Plc_Link_Stat = new_mtl_mts.NetworkLink ? sc.App.SCAppConstants.LinkStatus.LinkOK : sc.App.SCAppConstants.LinkStatus.LinkFail;
        //    mtl.Is_Eq_Alive = new_mtl_mts.Alive;
        //    mtl.MTxMode = new_mtl_mts.Mode ? sc.ProtocolFormat.OHTMessage.MTxMode.Auto : sc.ProtocolFormat.OHTMessage.MTxMode.Manual;
        //    mtl.Interlock = new_mtl_mts.Interlock;
        //    mtl.CurrentCarID = new_mtl_mts.CarID;
        //    mtl.MTLLocation = new_mtl_mts.MTLLocation.Trim()== MTLLocation.Upper.ToString()? MTLLocation.Upper:
        //        new_mtl_mts.MTLLocation.Trim() == MTLLocation.Bottorn.ToString()? MTLLocation.Bottorn:
        //        MTLLocation.None;
        //    mtl.SynchronizeTime = Convert.ToDateTime(new_mtl_mts.SynchronizeTime);
        //}

        //public static void set(this sc.PortDef port, PORT_INFO new_port)
        //{
        //    port.IsReadyToUnload = new_port.IsReadyToUnload; ;
        //    port.LoadPosition2 = new_port.LoadPosition2;
        //    port.LoadPosition3 = new_port.LoadPosition3;
        //    port.LoadPosition4 = new_port.LoadPosition4;
        //    port.LoadPosition5 = new_port.LoadPosition5;
        //    port.LoadPosition7 = new_port.LoadPosition7;
        //    port.LoadPosition6 = new_port.LoadPosition6;
        //    port.IsCSTPresence = new_port.IsCSTPresence;
        //    port.LoadPosition1 = new_port.LoadPosition1;
        //    port.ErrorCode = new_port.ErrorCode;
        //    port.CanOpenBox = new_port.CanOpenBox;
        //    port.IsBoxOpen = new_port.IsBoxOpen;
        //    port.BCRReadDone = new_port.BCRReadDone;
        //    port.CSTPresenceMismatch = new_port.CSTPresenceMismatch;
        //    port.IsTransferComplete = new_port.IsTransferComplete;
        //    port.CstRemoveCheck = new_port.CstRemoveCheck;
        //    port.BoxID = new_port.BoxID;
        //    port.IsReadyToLoad = new_port.IsReadyToLoad;
        //    port.AGVPortReady = new_port.AGVPortReady;
        //    port.IsAutoMode = new_port.IsAutoMode;
        //    port.PortWaitIn = new_port.PortWaitIn;
        //    port.CassetteID = new_port.CassetteID;
        //    port.PortWaitOut = new_port.PortWaitOut;
        //    port.Timestamp = new_port.Timestamp;
        //    port.OpManualMode = new_port.OpManualMode;
        //    port.OpError = new_port.OpError;
        //    port.OpAutoMode = new_port.OpAutoMode;
        //    port.IsOutputMode = new_port.IsOutputMode;
        //    port.IsModeChangable = new_port.IsModeChangable;
        //    port.IsAGVMode = new_port.IsAGVMode;
        //    port.IsMGVMode = new_port.IsMGVMode;
        //    port.IsInputMode = new_port.IsInputMode;

        //    port.LoadPositionBOX1 = new_port.LoadPositionBOX1;
        //    port.LoadPositionBOX2 = new_port.LoadPositionBOX2;
        //    port.LoadPositionBOX3 = new_port.LoadPositionBOX3;
        //    port.LoadPositionBOX4 = new_port.LoadPositionBOX4;
        //    port.LoadPositionBOX5 = new_port.LoadPositionBOX5;
        //    port.FireAlarm = new_port.FireAlarm;
        //}
    }
}

