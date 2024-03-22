using com.mirle.ibg3k0.bc.wpf.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Vehicle.Model
{
    public class VehicleModel
    {
        public string img_Introduction { get; private set; }
        public string img_Vehicle_AutoLocal { get; private set; }
        public string img_Vehicle_AutoRemote { get; private set; }
        public string img_Vehicle_Manual { get; private set; }
        public string img_Vehicle_PowerOn { get; private set; }
        public string img_Vehicle_AutoCharging { get; private set; }
        public string img_Vehicle_Disconnect { get; private set; }
        public string img_Action_Cassette { get; private set; }
        public string img_Action_Correcting { get; private set; }
        public string img_Action_Maintenance { get; private set; }
        public string img_Action_Moving { get; private set; }
        public string img_Action_Parked { get; private set; }
        public string img_Action_Parking { get; private set; }
        public string img_Action_ReceiveCommand { get; private set; }
        public string img_Action_Loading { get; private set; }
        public string img_Action_Unloading { get; private set; }
        public string img_Alarm_Alert { get; private set; }
        public string img_Alarm_Error { get; private set; }
        public string img_Alarm_Warning { get; private set; }
        public string img_Pause_Block { get; private set; }
        public string img_Pause_Earthquake { get; private set; }
        public string img_Pause_HID { get; private set; }
        public string img_Pause_Obstructed { get; private set; }
        public string img_Pause_Pause { get; private set; }
        public string img_Pause_Safety { get; private set; }
        public string img_Speed_Fast { get; private set; }
        public string img_Speed_Medium { get; private set; }
        public string img_Speed_Slow { get; private set; }
        public string img_Battery_Full { get; private set; }
        public string img_Battery_High { get; private set; }
        public string img_Battery_Middle { get; private set; }
        public string img_Battery_Low { get; private set; }
        public string img_Battery_None { get; private set; }
        public List<string> img_Battery_Charging { get; private set; }

        public string img_Vehicle { get; set; }
        public string img_Action { get; set; }
        public string img_Alarm { get; set; }
        public string img_Pause { get; set; }
        public string img_Speed { get; set; }
        public string img_Battery { get; set; }


        public VehicleModel()
        {
            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            img_Introduction = sPath + "\\Resources\\VH_Display\\Vehicle display_V2.3.png";
            img_Vehicle_AutoLocal = sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Local].png";
            img_Vehicle_AutoRemote = sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Remote].png";
            img_Vehicle_Manual = sPath + "\\Resources\\VH_Display\\Vehicle [Manual].png";
            img_Vehicle_PowerOn = sPath + "\\Resources\\VH_Display\\Vehicle [Power on].png";
            img_Vehicle_AutoCharging = sPath + "\\Resources\\VH_Display\\Vehicle [Auto-Charging].png";
            img_Vehicle_Disconnect = sPath + "\\Resources\\VH_Display\\Vehicle [Unconnected].png";
            img_Action_Cassette = sPath + "\\Resources\\VH_Display\\Action [Cassette].png";
            img_Action_Correcting = sPath + "\\Resources\\VH_Display\\Action [Correcting action].png";
            img_Action_Loading = sPath + "\\Resources\\VH_Display\\Action [Loading].png";
            img_Action_Unloading = sPath + "\\Resources\\VH_Display\\Action [Unloading].png";
            img_Action_Maintenance = sPath + "\\Resources\\VH_Display\\Action [Maintenance].png";
            img_Action_Moving = sPath + "\\Resources\\VH_Display\\Action [Moving].png";
            img_Action_Parked = sPath + "\\Resources\\VH_Display\\Action [Parked].png";
            img_Action_Parking = sPath + "\\Resources\\VH_Display\\Action [Parking].png";
            img_Action_ReceiveCommand = sPath + "\\Resources\\VH_Display\\Action [Receive command].png";
            img_Alarm_Alert = sPath + "\\Resources\\VH_Display\\Alarm [Alert].png";
            img_Alarm_Error = sPath + "\\Resources\\VH_Display\\Alarm [Error].png";
            img_Alarm_Warning = sPath + "\\Resources\\VH_Display\\Alarm [Warning].png";
            img_Pause_Block = sPath + "\\Resources\\VH_Display\\Pause [Block].png";
            img_Pause_Earthquake = sPath + "\\Resources\\VH_Display\\Pause [Earthquake].png";
            img_Pause_HID = sPath + "\\Resources\\VH_Display\\Pause [HID].png";
            img_Pause_Obstructed = sPath + "\\Resources\\VH_Display\\Pause [Obstructed].png";
            img_Pause_Pause = sPath + "\\Resources\\VH_Display\\Pause [Pause].png";
            img_Pause_Safety = sPath + "\\Resources\\VH_Display\\Pause [Safety].png";
            img_Speed_Fast = sPath + "\\Resources\\VH_Display\\Speed [Fast].png";
            img_Speed_Medium = sPath + "\\Resources\\VH_Display\\Speed [Medium].png";
            img_Speed_Slow = sPath + "\\Resources\\VH_Display\\Speed [Slow].png";
            img_Battery_Full = sPath + "\\Resources\\SystemIcon\\battery_100per.png";
            img_Battery_High = sPath + "\\Resources\\SystemIcon\\battery_75per.png";
            img_Battery_Middle = sPath + "\\Resources\\SystemIcon\\battery_50per.png";
            img_Battery_Low = sPath + "\\Resources\\SystemIcon\\battery_25per.png";
            img_Battery_None = sPath + "\\Resources\\SystemIcon\\battery_0per.png";
            img_Battery_Charging = new List<string>();
            img_Battery_Charging.Add(sPath + "\\Resources\\SystemIcon\\battery_charging_25per.png");
            img_Battery_Charging.Add(sPath + "\\Resources\\SystemIcon\\battery_charging_50per.png");
            img_Battery_Charging.Add(sPath + "\\Resources\\SystemIcon\\battery_charging_75per.png");
            img_Battery_Charging.Add(sPath + "\\Resources\\SystemIcon\\battery_charging_100per.png");

            img_Vehicle = img_Vehicle_Disconnect;
            img_Action = null;
            img_Alarm = null;
            img_Pause = null;
            img_Speed = img_Speed_Slow;
            img_Battery = img_Battery_None;
        }
    }
}
