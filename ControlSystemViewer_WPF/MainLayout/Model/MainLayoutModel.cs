using com.mirle.ibg3k0.bc.wpf.App;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace MainLayout.Model
{
    public class MainLayoutModel
    {
        public string str_ProductLine { get; set; }
        public string img_Side { get; private set; }
        public string img_Home { get; private set; }
        public string img_SignalOn { get; private set; }
        public string img_SignalOff { get; private set; }
        public string img_Signal_Control { get; set; }
        public string img_Signal_Host { get; set; }
        public string img_Signal_Alarm { get; set; }
        public string img_LogInOut { get; private set; }
        public int iWidth_Side { get; set; }
        public int iHeight_Data { get; set; }
        public string img_Data_Size_Up { get; private set; }
        public string img_Data_Size_Default { get; private set; }
        public string img_Data_Size_Dowm { get; private set; }


        public MainLayoutModel()
        {
            str_ProductLine = "Product Line";
            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            img_Side = sPath + "\\Resources\\icon_Hamburger.png";
            img_Home = sPath + "\\Resources\\icon_Home.png";
            img_SignalOn = sPath + "\\Resources\\icon_Link_ON.png";
            img_SignalOff = sPath + "\\Resources\\icon_Link_OFF.png";
            img_Signal_Control = img_SignalOff;
            img_Signal_Host = img_SignalOff;
            img_Signal_Alarm = img_SignalOff;
            img_LogInOut = sPath + "\\Resources\\icon_LoginAndout.png";
            iWidth_Side = 500;
            iHeight_Data = 216;
            img_Data_Size_Up = sPath + "\\Resources\\icon_zoom_out.png";
            img_Data_Size_Default = sPath + "\\Resources\\icon_zoom_default.png";
            img_Data_Size_Dowm = sPath + "\\Resources\\icon_zoom_in.png";
        }
    }
}
