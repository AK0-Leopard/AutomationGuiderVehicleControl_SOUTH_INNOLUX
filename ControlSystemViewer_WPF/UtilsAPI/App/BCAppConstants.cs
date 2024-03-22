using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.bc.wpf.App
{
    public class BCAppConstants
    {
        public static Color CLR_MAP_ADDRESS_DUFAULT = Color.Gainsboro;
        public static Color CLR_MAP_ADDRESS_START = Color.Violet;
        public static Color CLR_MAP_ADDRESS_FROM = Color.Lime;
        public static Color CLR_MAP_ADDRESS_TO = Color.Red;

        //Action Flag
        public const string ACT_FLAG_ADD = "Add";
        public const string ACT_FLAG_UPDATE = "Update";
        public const string ACT_FLAG_DELETE = "Delete";

        public const string SIGNAL_ON = "ON";
        public const string SIGNAL_OFF = "OFF";

        public const string LOGIN_USER_DEFAULT = "ADMIN";

        public const string TIP_MESSAGE_LEVEL_INFO = "Info";
        public const string TIP_MESSAGE_LEVEL_WARN = "Warn";
        public const string TIP_MESSAGE_LEVEL_ERROR = "Error";

        public const int INFO_MSG = 1;
        public const int WARN_MSG = 2;
        public const int ERROR_MSG = 3;

        public class TSCStateDisplay
        {
            public readonly static string Auto = "Auto";
            public readonly static string None = "None";
            public readonly static string Pause = "Pause";
            public readonly static string Pausing = "Pausing";
            public readonly static string Init = "Init";
        }
        public class SECSLinkDisplay
        {
            public readonly static string Link = "Selected";
            public readonly static string NotLink = "Not Selected";
        }
        public class HostModeDisplay
        {
            public readonly static string OnlineRemote = "Online Remote";
            public readonly static string OnlineLocal = "Online Local";
            public readonly static string Offline = "Offline";
        }

        public class NATSTopics
        {
            public readonly static string NATS_SUBJECT_VH_INFO_0 = "NATS_SUBJECT_KEY_VH_INFO_{0}_TEST";
            public readonly static string NATS_SUBJECT_VH_COMMAND_COMPLETE_0 = "NATS_SUBJECT_KEY_VH_COMMAND_COMPLETE_{0}";
            public readonly static string NATS_SUBJECT_LINE_INFO = "NATS_SUBJECT_KEY_LINE_INFO";
            public readonly static string NATS_SUBJECT_TIP_MESSAGE_INFO = "NATS_SUBJECT_KEY_TIP_MESSAGE_INFO";
            public readonly static string NATS_SUBJECT_CURRENT_ALARM = "NATS_SUBJECT_KEY_CURRENT_ALARMS";
            public readonly static string NATS_SUBJECT_CONNECTION_INFO = "NATS_SUBJECT_KEY_CONNECTION_INFO_INFO";
            public readonly static string NATS_SUBJECT_ONLINE_CHECK_INFO = "NATS_SUBJECT_KEY_ONLINE_CHECK_INFO";
            public readonly static string NATS_SUBJECT_PING_CHECK_INFO = "NATS_SUBJECT_KEY_PING_CHECK_INFO";
            public readonly static string NATS_SUBJECT_TRANSFER_INFO = "NATS_SUBJECT_KEY_TRANSFER_INFO";
            public readonly static string NATS_SUBJECT_MTLMTS_INFO = "NATS_SUBJECT_KEY_MTLMTS_INFO";

            public readonly static string NATS_SUBJECT_SYSTEM_LOG = "NATS_SUBJECT_KEY_SYSTEM_LOG";
            public readonly static string NATS_SUBJECT_TCPIP_LOG = "NATS_SUBJECT_KEY_TCPIP_LOG";
            public readonly static string NATS_SUBJECT_SECS_LOG = "NATS_SUBJECT_KEY_SECS_LOG";

            public const string NATS_SUBJECT_Port_INFO_0 = "NATS_SUBJECT_KEY_Port_INFO_{0}";
        }
        public class Filck_Action_Path
        {
            public static readonly string FROM = "From";
            public static readonly string TO = "To";
        }

        public const string CST_TYPE_CST = "CST";
        public const string CST_TYPE_TRAY = "TRAY";

        public class SubPageIdentifier
        {
            public readonly static string TRANSFER_CHANGE_STATUS = "TRANSFER_CHANGE_STATUS";
            public readonly static string TRANSFER_ASSIGN_VEHICLE = "TRANSFER_ASSIGN_VEHICLE";
            public readonly static string TRANSFER_SHIFT_COMMAND = "TRANSFER_SHIFT_COMMAND";
        }

        public enum LogType
        {
            PLC_ForEQ,
            SECS_ForHost,
            MQ_ForHost,
            TCP_ForEQ
        }

        public class RGBColor
        {
            //------------------------------------------------------系統菜單配色區------------------------------------------------------
            //系統菜單_背景色
            public readonly static Color Menu_BackColor = Color.FromArgb(0, 91, 168);       //深藍

            public readonly static Color SubMenu_BackColor = Color.White;

            //系統菜單_字體色
            public readonly static Color Menu_FontColor = Color.FromArgb(255, 255, 255);
            public readonly static Color SubMenu_FontColor = Color.FromArgb(74, 74, 74);       //黑
            //系統菜單_選擇時顏色
            //public readonly static Color Menu_SelectedColor = Color.FromArgb(103, 173, 249);
            public readonly static Color Menu_SelectedColor = Color.FromArgb(0, 138, 255);      //天空藍_深
            public readonly static Color SubMenu_SelectedColor = Color.FromArgb(0, 138, 255);    //天空藍_深
            //---------------------------------------------------------------------------------------------------------------------------------



            //------------------------------------------------------系統主訊號配色區---------------------------------------------------
            //系統主訊號_Title背景色
            //public readonly static Color MainSignal_TitleBackColor = Color.FromArgb(230, 230, 230);       //淺灰
            public readonly static Color MainSignal_TitleBackColor = Color.FromArgb(194, 229, 253);     //天空藍

            //系統主訊號_Title字體色
            public readonly static Color MainSignal_TitleFontColor = Color.FromArgb(74, 74, 74);    //灰黑

            //系統主訊號_資料背景色
            //public readonly static Color MainSignal_ValueBackColor = Color.FromArgb(204, 204, 204);       //淺灰
            public readonly static Color MainSignal_ValueBackColor = Color.White;

            //系統主訊號_公司與客戶Logo背景色
            public readonly static Color MainSignal_LogoBackColor = Color.FromArgb(200, 227, 253);     //天空藍

            //系統主訊號_資料字體色
            public readonly static Color MainSignal_ValueFontColor = Color.FromArgb(74, 74, 74);        //黑灰

            //系統主訊號_框架背景色
            public readonly static Color MainSignal_BackColor = Color.FromArgb(100, 189, 242);      //天空藍
            //---------------------------------------------------------------------------------------------------------------------------------



            //----------------------------Line狀態變化後，主訊號下方顯示的Line狀態顏色-----------------------------------
            public readonly static Color LineStatus_IDLE = Color.FromArgb(74, 74, 74);      //黑灰
            public readonly static Color LineStatus_RUN = Color.FromArgb(1, 255, 0);        //綠
            public readonly static Color LineStatus_DOWN = Color.FromArgb(255, 14, 14);     //紅
            public readonly static Color LineStatus_MAINT = Color.FromArgb(255, 14, 14);     //紅
            //---------------------------------------------------------------------------------------------------------------------------------



            //---------------------------------------系統啟動後，顯示Initial的介面配色區-----------------------------------------
            public readonly static Color ProcessBarDialog_BackColor = Color.FromArgb(0, 51, 102);        //天空藍_淺
            //---------------------------------------------------------------------------------------------------------------------------------



            //---------------------------------------使用者登入後，顯示登入帳號字體顏色---------------------------------------
            public readonly static Color UserLoginColor = Color.White;
            public readonly static Color UserLogoutColor = Color.White;
            public readonly static String UserLogin_Value = "Login";
            //---------------------------------------------------------------------------------------------------------------------------------







            //------------------------------------------------------設備組件配色區------------------------------------------------------
            //設備組件_標題背景色
            //public readonly static Color EQComp_TitleBackColor = Color.FromArgb(10, 81, 160);       //深藍
            public readonly static Color EQComp_TitleBackColor = Color.FromArgb(11, 143, 224);       //淺藍

            //設備組件_標題字體色
            public readonly static Color EQComp_TitleForeColor = Color.White;

            //設備組件_內容背景色
            //public readonly static Color EQComp_Content = Color.FromArgb(230, 230, 230);        //淺灰
            public readonly static Color EQComp_Content = Color.FromArgb(196, 229, 253);        //天空藍

            //設備組件_內容字體色
            public readonly static Color EQComp_EQSignalColor_Name = Color.FromArgb(74, 74, 74);        //灰黑

            //設備組件_內容背景色
            //public readonly static Color EQComp_EQSignalColor_Value = Color.FromArgb(145, 145, 145);      //灰色
            public readonly static Color EQComp_EQSignalColor_Value = Color.White;
            //---------------------------------------------------------------------------------------------------------------------------------



            //------------------------------------------------------Port組件配色區------------------------------------------------------
            //設備組件_標題背景色
            //public readonly static Color PortComp_TitleBackColor = Color.FromArgb(10, 81, 160);       //深藍
            public readonly static Color PortComp_TitleBackColor = Color.FromArgb(11, 143, 224);       //淺藍

            //設備組件_標題字體色
            public readonly static Color PortComp_TitleForeColor = Color.White;

            //設備組件_內容背景色
            //public readonly static Color EQComp_Content = Color.FromArgb(230, 230, 230);        //淺灰
            public readonly static Color PortComp_Content = Color.FromArgb(196, 229, 253);        //天空藍

            //設備組件_內容字體色
            public readonly static Color PortComp_PortSignalColor_Name = Color.FromArgb(74, 74, 74);        //灰黑

            //設備組件_內容背景色
            //public readonly static Color EQComp_EQSignalColor_Value = Color.FromArgb(145, 145, 145);      //灰色
            public readonly static Color PortComp_PortSignalColor_Value = Color.White;
            public readonly static Color PortComp_PortSignalColor_UnuseValue = Color.FromArgb(208, 205, 199);

            //設備組件_Abort或Cancel按鈕顏色
            public readonly static Color PortComp_uiButtonEnable = Color.FromArgb(11, 143, 224);
            public readonly static Color PortComp_uiButtonDisable = Color.Gray;

            //設備組件_Abort或Cancel按鈕陰影顏色
            public readonly static Color PortComp_uiButtonEnable_Background = Color.FromArgb(15, 72, 104);
            public readonly static Color PortComp_uiButtonDisable_Background = Color.FromArgb(64, 64, 64);
            //---------------------------------------------------------------------------------------------------------------------------------


            //------------------------------------------------------Buffer組件配色區------------------------------------------------------
            //標題背景色
            public readonly static Color BufferComp_TitleBackColor = Color.FromArgb(11, 143, 224);       //淺藍

            //標題字體色
            public readonly static Color BufferComp_TitleForeColor = Color.White;

            //內容背景色
            public readonly static Color BufferComp_Content = Color.FromArgb(196, 229, 253);        //天空藍
            //---------------------------------------------------------------------------------------------------------------------------------


            //----------------------------------------------BC與Host連線狀態配色區------------------------------------------------
            public readonly static Color OnlineRemoteFontColor = Color.FromArgb(1, 255, 0);     //綠色
            public readonly static Color OnlineLocalFontColor = Color.FromArgb(248, 231, 14);   //黃色
            public readonly static Color OfflineFontColor = Color.FromArgb(255, 14, 14);             //紅色
            //---------------------------------------------------------------------------------------------------------------------------------



            //----------------------------------------------快捷鍵配色區------------------------------------------------
            public readonly static Color ShortCut_BackGroundColor = Color.FromArgb(196, 229, 253);        //天空藍
            public readonly static Color ShortCut_ButtonEnableColor = Color.FromArgb(11, 143, 224);
            public readonly static Color ShortCut_ButtonShadowColor = Color.FromArgb(14, 85, 126);
            public readonly static Color ShortCut_ButtonEnableFontColor = Color.White;
            public readonly static Color ShortCut_ButtonDisableFontColor = Color.FromArgb(3, 49, 100);
            //---------------------------------------------------------------------------------------------------------------------------------


            //public readonly static Color uiButtonEnable = Color.FromArgb(200, 200, 200);
            public readonly static Color uiButtonEnable = Color.White;
            //public readonly static Color uiButtonDisable = Color.FromArgb(74, 74, 74);
            public readonly static Color uiButtonDisable = Color.FromArgb(204, 204, 204);

            public readonly static Color EQSignal_Abnormal = Color.FromArgb(255, 168, 168);
            public readonly static Color EQSignal_Normal = Color.FromArgb(74, 74, 74);
            public readonly static Color EQSignal_Running = Color.FromArgb(1, 255, 0);

            public readonly static Color EQSignal_Alarm = Color.FromArgb(255, 168, 168);
            public readonly static Color EQSignal_Warning = Color.FromArgb(255, 168, 168);

            public readonly static Color PortSignal_Abnormal = Color.FromArgb(255, 168, 168);
            public readonly static Color PortSignal_Normal = Color.FromArgb(74, 74, 74);

            public readonly static Color PortBackColor_Abnormal = Color.FromArgb(255, 14, 14);
            public readonly static Color PortBackColor_Normal = Color.FromArgb(155, 155, 155);

            public readonly static Color CSTSignal_Normal = Color.FromArgb(74, 74, 74);
            public readonly static Color CSTSignal_Processing = Color.FromArgb(1, 255, 0);




            public readonly static Color CIMMode_FontColor_Green = Color.FromArgb(1, 255, 0);
            public readonly static Color CIMMode_FontColor_Gray = Color.FromArgb(156, 152, 147);
            public readonly static Color CIMMode_FontColor_Red = Color.Red;

            public readonly static Color HostMode_FontColor = Color.FromArgb(74, 74, 74);
            public readonly static Color HostMode_OnlineRemote_FontColor = Color.FromArgb(1, 255, 0);
            public readonly static Color HostMode_OnlineLocal_FontColor = Color.FromArgb(1, 255, 0);
            public readonly static Color HostMode_Offline_FontColor = Color.FromArgb(255, 14, 14);
            public readonly static Color HostMode_ButtonColor = Color.White;

            public readonly static Color MouseEnterButtonColorChange = Color.FromArgb(0, 91, 168);
            public readonly static Color MouseLeaveButtonColorChange = Color.FromArgb(0, 91, 168);
            public readonly static Color FocusButtonColorChange = Color.FromArgb(0, 91, 168);
            public readonly static Color UnfocusButtonColorChange = Color.FromArgb(0, 91, 168);
            //------------------------------主頁面 配色區-------------------------------
            public readonly static Color VehicleStatus_Manual = Color.FromArgb(255, 128, 0); //Color.Blue;
            public readonly static Color VehicleStatus_Auto = Color.Lime;
            public readonly static Color VehicleStatus_Moving = Color.Cyan;
            public readonly static Color VehicleStatus_Blocking = Color.Yellow;
            public readonly static Color VehicleStatus_Pause = Color.Violet;
            public readonly static Color VehicleStatus_Obstacle = Color.FromArgb(255, 128, 0);
            public readonly static Color VehicleStatus_Error = Color.Red;

            public readonly static Color VehiclePresence_NoStock = Color.White;
            public readonly static Color VehiclePresence_Stock = Color.Lime;
            public readonly static Color PortStatus_NoLink = Color.Silver;
            public readonly static Color PortStatus_NoStock = Color.White;
            public readonly static Color PortStatus_Stock = Color.Lime;

            public readonly static Color AddressInstructions_Default = Color.Gray;
            public readonly static Color AddressInstructions_Start = Color.Violet;
            public readonly static Color AddressInstructions_From = Color.Lime;
            public readonly static Color AddressInstructions_To = Color.Red;
            public readonly static Color AddressInstructions_HasBox = Color.Yellow;
            //----------------------------------------------------------------------------
        }

        public class RBGColor_BlueStyle
        {
            //------------------------------------------------------系統主訊號配色區---------------------------------------------------
            //系統主訊號_Title背景色
            public readonly static Color MainSignal_TitleBackColor = Color.FromArgb(200, 227, 253);
            //系統主訊號_Title字體色
            public readonly static Color MainSignal_TitleFontColor = Color.FromArgb(74, 74, 74);
            //系統主訊號_資料背景色
            public readonly static Color MainSignal_ValueBackColor = Color.White;
            //系統主訊號_資料字體色
            public readonly static Color MainSignal_ValueFontColor = Color.FromArgb(74, 74, 74);
            //---------------------------------------------------------------------------------------------------------------------------------

            //----------------------------------------------快捷鍵配色區------------------------------------------------
            public readonly static Color ShortCut_BackGroundColor = Color.FromArgb(255, 242, 204);        //天空藍
            public readonly static Color ShortCut_ButtonEnableColor = Color.FromArgb(255, 192, 0);
            public readonly static Color ShortCut_ButtonShadowColor = Color.FromArgb(191, 144, 0);
            public readonly static Color ShortCut_ButtonEnableFontColor = Color.White;
            public readonly static Color ShortCut_ButtonDisableFontColor = Color.FromArgb(3, 49, 100);
            //---------------------------------------------------------------------------------------------------------------------------------

        }

        

        public class MTLMTS_Address
        {
            public readonly static string MTS_address = "20292";
            public readonly static string MTL_address = "20293";
            public readonly static string MTS2_address = "20296";
        }

        //取得主頁面 版本號碼
        public static String getMainFormVersion(String appendStr)
        {
            return FileVersionInfo.GetVersionInfo(
                Assembly.GetEntryAssembly().Location).FileVersion.ToString() + appendStr;
        }

        public static DateTime GetBuildDateTime()
        {
            string filePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];

            try
            {
                using (var s = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    s.Read(b, 0, 2048);
                }
            }
            catch
            {

            }
            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.ToLocalTime();
            return dt;
        }

    }
}
