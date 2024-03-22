using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mirle.UtilsAPI.Commands;
using Vehicle.Model;
using Mirle.UtilsAPI;
using com.mirle.ibg3k0.bc.wpf.App;
using System.Windows.Threading;
using System.Windows;
using System.Reflection;
using System.IO;
using com.mirle.ibg3k0.ohxc.wpf.App;

namespace Vehicle.ViewModel
{
    public class VehicleViewModel : ViewModelBase
    {
        #region Properties : 用到哪一些類別
        public WindownApplication app = null;
        private VehicleModel model = new VehicleModel();
        private DispatcherTimer dispatcherTimer = null;
        private string strVhNo = "";
        private string strValue = "000";
        private VEHICLE_TYPE typeVehicle = VEHICLE_TYPE.Disconnect;
        private VEHICLE_ACTION_TYPE typeAction = VEHICLE_ACTION_TYPE.Idle;
        private VEHICLE_ALARM_TYPE typeAlarm = VEHICLE_ALARM_TYPE.None;
        private VEHICLE_PAUSE_TYPE typePause = VEHICLE_PAUSE_TYPE.Safety;
        private VEHICLE_SPEED_TYPE typeSpeed = VEHICLE_SPEED_TYPE.Slow;
        private VEHICLE_BATTERY_LEVEL levelBattery = VEHICLE_BATTERY_LEVEL.None;
        #endregion

        #region Command : Button 要 Ref 的命令 (ICommand : 適用Windows 的 WPF 組件)
        public ICommand SideCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SetLanguageCommand { get; }
        #endregion

        #region  Constructors 
        public VehicleViewModel()
        {
            ////指定功能function
            //SideCommand = new AnotherCommandImplementation(_ => Side());    //AnotherCommandImplementation :  UtilsAPI 的類別
            //HomeCommand = new AnotherCommandImplementation(_ => Home());
            ////DeleteCommand = new AnotherCommandImplementation(_ => Delete());
            ////SaveCommand = new AnotherCommandImplementation(_ => Save());
            //SetLanguageCommand = new AnotherCommandImplementation(o => SetLanguage(o?.ToString()));

            //dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Interval = TimeSpan.FromSeconds(6);
            //dispatcherTimer.Tick += new EventHandler(AskControlExist);
            //dispatcherTimer.Start();
        }
        #endregion Constructors

        #region Display

        public string Vehicle_No
        {
            get { return strVhNo; }
            set
            {
                if (strVhNo != value)
                {
                    strVhNo = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Display_Value
        {
            get { return strValue; }
            set
            {
                if (strValue != value)
                {
                    strValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Img_Vehicle
        {
            get { return model.img_Vehicle; }
            set
            {
                if (model.img_Vehicle != value)
                {
                    model.img_Vehicle = value;
                    OnPropertyChanged();
                }
            }
        }
        public VEHICLE_TYPE Type_Vehicle
        {
            get { return typeVehicle; }
            set
            {
                if (typeVehicle != value)
                {
                    typeVehicle = value;
                    switch (typeVehicle)
                    {
                        case VEHICLE_TYPE.AutoMTL:
                        case VEHICLE_TYPE.AutoLocal:
                            Img_Vehicle = model.img_Vehicle_AutoLocal;
                            break;
                        case VEHICLE_TYPE.AutoRemote:
                            Img_Vehicle = model.img_Vehicle_AutoRemote;
                            break;
                        case VEHICLE_TYPE.Manual:
                            Img_Vehicle = model.img_Vehicle_Manual;
                            break;
                        case VEHICLE_TYPE.PowerOn:
                            Img_Vehicle = model.img_Vehicle_PowerOn;
                            break;
                        case VEHICLE_TYPE.AutoCharging:
                            Img_Vehicle = model.img_Vehicle_AutoCharging;
                            break;
                        case VEHICLE_TYPE.Disconnect:
                        default:
                            Img_Vehicle = model.img_Vehicle_Disconnect;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Img_Action
        {
            get { return model.img_Action; }
            set
            {
                if (model.img_Action != value)
                {
                    model.img_Action = value;
                    OnPropertyChanged();
                }
            }
        }
        public VEHICLE_ACTION_TYPE Type_Action
        {
            get { return typeAction; }
            set
            {
                if (typeAction != value)
                {
                    typeAction = value;
                    switch (typeAction)
                    {
                        case VEHICLE_ACTION_TYPE.Cassette:
                            Img_Action = model.img_Action_Cassette;
                            break;
                        case VEHICLE_ACTION_TYPE.Correcting:
                            Img_Action = model.img_Action_Correcting;
                            break;
                        case VEHICLE_ACTION_TYPE.Loading:
                            Img_Action = model.img_Action_Loading;
                            break;
                        case VEHICLE_ACTION_TYPE.Unloading:
                            Img_Action = model.img_Action_Unloading;
                            break;
                        case VEHICLE_ACTION_TYPE.Maintenance:
                            Img_Action = model.img_Action_Maintenance;
                            break;
                        case VEHICLE_ACTION_TYPE.Moving:
                            Img_Action = model.img_Action_Moving;
                            break;
                        case VEHICLE_ACTION_TYPE.Parked:
                            Img_Action = model.img_Action_Parked;
                            break;
                        case VEHICLE_ACTION_TYPE.Parking:
                            Img_Action = model.img_Action_Parking;
                            break;
                        case VEHICLE_ACTION_TYPE.ReceiveCommand:
                            Img_Action = model.img_Action_ReceiveCommand;
                            break;
                        case VEHICLE_ACTION_TYPE.Idle:
                        default:
                            Img_Action = null;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Img_Alarm
        {
            get { return model.img_Alarm; }
            set
            {
                if (model.img_Alarm != value)
                {
                    model.img_Alarm = value;
                    OnPropertyChanged();
                }
            }
        }
        public VEHICLE_ALARM_TYPE Type_Alarm
        {
            get { return typeAlarm; }
            set
            {
                if (typeAlarm != value)
                {
                    typeAlarm = value;
                    switch (typeAlarm)
                    {
                        case VEHICLE_ALARM_TYPE.Alert:
                            Img_Alarm = model.img_Alarm_Alert;
                            break;
                        case VEHICLE_ALARM_TYPE.Error:
                            Img_Alarm = model.img_Alarm_Error;
                            break;
                        case VEHICLE_ALARM_TYPE.Warning:
                            Img_Alarm = model.img_Alarm_Warning;
                            break;
                        case VEHICLE_ALARM_TYPE.None:
                        default:
                            Img_Alarm = null;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Img_Pause
        {
            get { return model.img_Pause; }
            set
            {
                if (model.img_Pause != value)
                {
                    model.img_Pause = value;
                    OnPropertyChanged();
                }
            }
        }
        public VEHICLE_PAUSE_TYPE Type_Pause
        {
            get { return typePause; }
            set
            {
                if (typePause != value)
                {
                    typePause = value;
                    switch (typePause)
                    {
                        case VEHICLE_PAUSE_TYPE.Block:
                            Img_Pause = model.img_Pause_Block;
                            break;
                        case VEHICLE_PAUSE_TYPE.Earthquake:
                            Img_Pause = model.img_Pause_Earthquake;
                            break;
                        case VEHICLE_PAUSE_TYPE.HID:
                            Img_Pause = model.img_Pause_HID;
                            break;
                        case VEHICLE_PAUSE_TYPE.Obstructed:
                            Img_Pause = model.img_Pause_Obstructed;
                            break;
                        case VEHICLE_PAUSE_TYPE.Safety:
                            Img_Pause = model.img_Pause_Safety;
                            break;
                        case VEHICLE_PAUSE_TYPE.Pause:
                            Img_Pause = model.img_Pause_Pause;
                            break;
                        case VEHICLE_PAUSE_TYPE.None:
                        default:
                            Img_Pause = null;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Img_Speed
        {
            get { return model.img_Speed; }
            set
            {
                if (model.img_Speed != value)
                {
                    model.img_Speed = value;
                    OnPropertyChanged();
                }
            }
        }

        public VEHICLE_SPEED_TYPE Type_Speed
        {
            get { return typeSpeed; }
            set
            {
                if (typeSpeed != value)
                {
                    typeSpeed = value;
                    switch (typeSpeed)
                    {
                        case VEHICLE_SPEED_TYPE.Fast:
                            Img_Speed = model.img_Speed_Fast;
                            break;
                        case VEHICLE_SPEED_TYPE.Medium:
                            Img_Speed = model.img_Speed_Medium;
                            break;
                        case VEHICLE_SPEED_TYPE.Slow:
                        default:
                            Img_Speed = model.img_Speed_Slow;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Img_Battery
        {
            get { return model.img_Battery; }
            set
            {
                if (model.img_Battery != value)
                {
                    model.img_Battery = value;
                    OnPropertyChanged();
                }
            }
        }

        public VEHICLE_BATTERY_LEVEL Level_Battery
        {
            get { return levelBattery; }
            set
            {
                if (levelBattery != value)
                {
                    levelBattery = value;
                    switch (levelBattery)
                    {
                        case VEHICLE_BATTERY_LEVEL.Full:
                            Img_Battery = model.img_Battery_Full;
                            break;
                        case VEHICLE_BATTERY_LEVEL.High:
                            Img_Battery = model.img_Battery_High;
                            break;
                        case VEHICLE_BATTERY_LEVEL.Middle:
                            Img_Battery = model.img_Battery_Middle;
                            break;
                        case VEHICLE_BATTERY_LEVEL.Low:
                            Img_Battery = model.img_Battery_Low;
                            break;
                        case VEHICLE_BATTERY_LEVEL.None:
                        default:
                            Img_Battery = model.img_Battery_None;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }
        #endregion Display

        #region Funtion
        private void AskControlExist(Object myObject, EventArgs myEventArgs)
        {
            //if (app != null)
            //{
            //    string result = string.Empty;
            //    SetSignal("Control", app.LineBLL.SendControlIsExist("", out result));
            //}
        }
#endregion Funtion

#region Vehicle Type
        public enum VEHICLE_TYPE
        {
            Disconnect = 0,
            PowerOn,
            Manual,
            AutoRemote,
            AutoLocal,
            AutoCharging,
            AutoMTL
        }
        public enum VEHICLE_ALARM_TYPE
        {
            None = 0,
            Alert,
            Error,
            Warning
        }
        public enum VEHICLE_ACTION_TYPE
        {
            Idle = 0,
            Cassette,
            Correcting,
            Loading,
            Unloading,
            Maintenance,
            Moving,
            Parked,
            Parking,
            ReceiveCommand,
            Charging
        }
        public enum VEHICLE_PAUSE_TYPE
        {
            None = 0,
            Block,
            Earthquake,
            HID,
            Obstructed,
            Safety,
            Pause
        }
        public enum VEHICLE_SPEED_TYPE
        {
            Slow = 0,
            Medium,
            Fast
        }
        public enum VEHICLE_BATTERY_LEVEL
        {
            None = 0,
            Low,
            Middle,
            High,
            Full,
            Charging_Low,
            Charging_Middle,
            Charging_High,
            Charging_Full
        }
        #endregion Rail Type
    }
}
