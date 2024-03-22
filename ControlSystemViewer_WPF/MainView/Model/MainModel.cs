using com.mirle.ibg3k0.bc.wpf.App;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace MainView.Model
{
    public class MainModel
    {
        public string hourlyProcess { get; set; }
        public string todayProcess { get; set; }
        public string runTime { get; set; }
        public string buildDate { get; set; }
        public string version { get; private set; }
        public string logo_Customer { get; set; }
        public string logo_Mirle { get; private set; }


        public MainModel()
        {
            runTime = "00d 00h 00m";
            buildDate = File.GetLastWriteTimeUtc(Assembly.GetEntryAssembly().Location).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            version = BCAppConstants.getMainFormVersion("");

            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            //logo_Customer = sPath + "\\Resources\\CustomerLogo.png";
            logo_Mirle = sPath + "\\Resources\\icon_MirleLogo.png";
        }
    }
}
