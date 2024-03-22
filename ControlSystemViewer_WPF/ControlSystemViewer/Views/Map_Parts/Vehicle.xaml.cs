//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UtilsAPI.Common;
using Vehicle.ViewModel;
using ViewerObject;
using static Vehicle.ViewModel.VehicleViewModel;
using static ViewerObject.VCMD_Def;
using static ViewerObject.VVEHICLE_Def;
using Adapter = com.mirle.ibg3k0.Utility.uc.Adapter;

namespace ControlSystemViewer.Views.Map_Parts
{
    /// <summary>
    /// Vehicle.xaml 的互動邏輯
    /// </summary>
    public partial class Vehicle : UserControl
    {
        #region "Internal Variable"
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private VehicleViewModel vm = null;
        private WindownApplication app = null;
        private VVEHICLE vh = null;
        private string m_sID;
        private string m_sStatus = null;
        private Point m_pPosition;
        private Point m_pOffset = new Point(0, 0);
        private bool isShowcase = false;
        private bool isCharging = false;
        private bool stopAllTimer = false;
        private DispatcherTimer dispatcherTimer_Charging = new DispatcherTimer();
        private DispatcherTimer dispatcherTimer_Alarm = new DispatcherTimer();
        public EventHandler<string> VehicleBeChosen;
        public EventHandler<string> OpenQuickVehicleCommand;
        public EventHandler VehicleStatusChanged;
        public EventHandler<Point> ZoomIn;
        public EventHandler<(string, Point)> PositionChanged;
        #endregion	/* Internal Variable */

        #region "Property"

        /// <summary>
        /// Object Name
        /// </summary>

        public string p_Status
        {
            get { return m_sStatus ?? "Disconnect"; }
            set
            {
                if (m_sStatus != value)
                {
                    m_sStatus = value;
                    VehicleStatusChanged?.Invoke(this, null);
                }
            }
        }

        public string p_ID
        {
            get { return m_sID; }
            set
            {
                if (m_sID != value)
                {
                    m_sID = value;
                    p_No = getVehicleNo();
                }
            }
        }

        public string p_No
        {
            get { return vm?.Vehicle_No ?? ""; }
            set
            {
                if (vm != null)
                {
                    vm.Vehicle_No = value;
                }
            }
        }

        public string p_Display_Value
        {
            get { return vm?.Display_Value ?? ""; }
            set
            {
                if (vm != null)
                {
                    vm.Display_Value = value;
                }
            }
        }

        public Point p_Position
        {
            get { return m_pPosition; }
            set
            {
                m_pPosition = value;
                //this.Margin = new Thickness(value.X + p_Offset.X - (VehicleViewBox.Width / 2), value.Y + p_Offset.Y - VehicleViewBox.Height, 0, 0);
                PositionChanged?.Invoke(this, (p_ID, new Point(p_Position.X + p_Offset.X + (Width / 2),
                                                               p_Position.Y + p_Offset.Y - (Height / 2))));
            }
        }

        public Point p_Offset
        {
            get { return m_pOffset; }
            set
            {
                m_pOffset = value;
            }
        }

        public VEHICLE_TYPE p_VEHICLE_TYPE
        {
            get { return vm?.Type_Vehicle ?? VEHICLE_TYPE.Disconnect; }
            set
            {
                if (vm != null)
                {
                    if (vm.Type_Vehicle != value)
                    {
                        vm.Type_Vehicle = value;
                        lbl_VehicleNo.Foreground = vm.Type_Vehicle == VEHICLE_TYPE.PowerOn ? Brushes.Black : Brushes.WhiteSmoke;
                    }
                }
            }
        }

        public VEHICLE_ACTION_TYPE p_VEHICLE_ACTION_TYPE
        {
            get { return vm?.Type_Action ?? VEHICLE_ACTION_TYPE.Idle; }
            set
            {
                if (vm != null)
                {
                    if (vm.Type_Action != value)
                    {
                        vm.Type_Action = value;
                    }
                }
            }
        }

        public VEHICLE_ALARM_TYPE p_VEHICLE_ALARM_TYPE
        {
            get { return vm?.Type_Alarm ?? VEHICLE_ALARM_TYPE.None; }
            set
            {
                if (vm != null)
                {
                    vm.Type_Alarm = value;

                    if (vm.Type_Alarm == VEHICLE_ALARM_TYPE.None)
                        dispatcherTimer_Alarm?.Stop();
                    else
                        dispatcherTimer_Alarm?.Start();
                }
            }
        }

        public VEHICLE_PAUSE_TYPE p_VEHICLE_PAUSE_TYPE
        {
            get { return vm?.Type_Pause ?? VEHICLE_PAUSE_TYPE.None; }
            set
            {
                if (vm != null)
                {
                    vm.Type_Pause = value;
                }
            }
        }

        public VEHICLE_SPEED_TYPE p_VEHICLE_SPEED_TYPE
        {
            get { return vm?.Type_Speed ?? VEHICLE_SPEED_TYPE.Slow; }
            set
            {
                if (vm != null)
                {
                    vm.Type_Speed = value;
                }
            }
        }

        public VEHICLE_BATTERY_LEVEL p_VEHICLE_BATTERY_LEVEL
        {
            get { return vm?.Level_Battery ?? VEHICLE_BATTERY_LEVEL.None; }
            set
            {
                if (vm != null)
                {
                    vm.Level_Battery = value;
                }
            }
        }

        public bool p_IsCharging
        {
            get { return isCharging; }
            set
            {
                if (isCharging != value)
                {
                    isCharging = value;

                    if (isCharging)
                        dispatcherTimer_Alarm?.Start();
                    else
                    {
                        dispatcherTimer_Alarm?.Stop();
                        updateBatteryLevel();
                    }
                }
            }
        }
        #endregion	/* Property */

        #region	Animation
        public DoubleAnimation doubleAnimation_x { get; private set; } = new DoubleAnimation();
        public DoubleAnimation doubleAnimation_y { get; private set; } = new DoubleAnimation();
        Storyboard moveStoryboard;
        public ContentControl vhPresenter { get; private set; } = new ContentControl();
        private static DispatcherTimer positionUpdat3Timer = new DispatcherTimer();
        #endregion	/* Animation */

        #region "Constructor／Destructor"
        public Vehicle(WindownApplication _app, VVEHICLE _vh, Point pointOffset, double width = 150, bool _isShowcase = false)
        {
            InitializeComponent();

            app = _app;
            vh = _vh;

            if (app?.ObjCacheManager.IsForAGVC ?? false)
            {
                Image_Battery.Visibility = Visibility.Visible;
            }

            isShowcase = _isShowcase;
            vm = DataContext as VehicleViewModel;

            VehicleViewBox.Width = width;
            VehicleViewBox.Height = width;
            this.Width = VehicleViewBox.Width;
            this.Height = VehicleViewBox.Height;

#if IS_FOR_OHTC_NOT_AGVC
#else
            dispatcherTimer_Charging.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer_Charging.Tick += displayChargingGIF;
#endif

            dispatcherTimer_Alarm.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer_Alarm.Tick += changeAlarmVisibility;

            if (vm != null && vh != null)
            {
                p_ID = vh.VEHICLE_ID?.Trim() ?? "";

                initToolTip();

                if (isShowcase)
                {
                    //m_pOffset = new Point(0, VehicleViewBox.Height / 2);
                    p_Position = new Point(0, 0);
                    this.Margin = new Thickness(-this.Width / 2, -this.Height / 2, 0, 0);
                }
                else //if (!isShowcase)
                {
                    m_pOffset = pointOffset;
                    p_Position = new Point(vh.X_AXIS, vh.Y_AXIS);
                    updatePosition(new PositionChangeEventArgs(0, 0, vh.X_AXIS, vh.Y_AXIS));
                }
                updateModeStatus();
                updateActionStatus();
                updatePauseStatus();
                //updateSpeedStatus();
#if IS_FOR_OHTC_NOT_AGVC
#else
                //updateBatteryCapacity();
                updateBatteryLevel();
                updateChargeStatus();
#endif
                //updateBatteryLevel();
                updateAlarmStatus();
                registerEvents();

                updateChargeStatus();
                updateStatus();
            }

            initialAnimation();
        }
        #endregion	/* Constructor／Destructor */

        public void Close()
        {
            if (vh != null)
            {
                try
                {
                    unregisterEvents();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                }
            }
#if IS_FOR_OHTC_NOT_AGVC
#else
            try
            {
                dispatcherTimer_Charging.Stop();
                dispatcherTimer_Charging.Tick -= displayChargingGIF;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
#endif
            try
            {
                dispatcherTimer_Alarm.Stop();
                dispatcherTimer_Alarm.Tick -= changeAlarmVisibility;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            try
            {
                positionUpdat3Timer.Stop();
                positionUpdat3Timer.Tick -= positionUpdat3;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            stopAllTimer = true;
        }

        #region Event
        private void registerEvents()
        {
            unregisterEvents();
            vh.ConnectionStatusChange += (s1, e1) => updateModeStatus();
            vh.ModeStatusChange += (s1, e1) => updateModeStatus();
            vh.ErrorStatusChange += (s1, e1) => updateAlarmStatus();
            vh.ActionChange += (s1, e1) => updateActionStatus();
            vh.CassetteStatusChange += (s1, e1) => updateActionStatus();
            vh.PauseStatusChange += (s1, e1) => updatePauseStatus();
            if (!isShowcase) vh.PositionChange += (s1, e1) => updatePosition(e1);
            vh.SpeedChange += (s1, e1) => updateSpeedStatus();
            vh.BatteryCapacityChange += (s1, e1) => updateBatteryCapacity();
            vh.BatteryLevelChange += (s1, e1) => updateBatteryLevel();
            vh.StatusChange += (s1, e1) => updateStatus();
        }

        private void unregisterEvents()
        {
            vh.ConnectionStatusChange -= (s1, e1) => updateModeStatus();
            vh.ModeStatusChange -= (s1, e1) => updateModeStatus();
            vh.ErrorStatusChange -= (s1, e1) => updateAlarmStatus();
            vh.ActionChange -= (s1, e1) => updateActionStatus();
            vh.CassetteStatusChange -= (s1, e1) => updateActionStatus();
            vh.PauseStatusChange -= (s1, e1) => updatePauseStatus();
            if (!isShowcase) vh.PositionChange -= (s1, e1) => updatePosition(e1);
            vh.SpeedChange -= (s1, e1) => updateSpeedStatus();
            vh.BatteryCapacityChange -= (s1, e1) => updateBatteryCapacity();
            vh.BatteryLevelChange -= (s1, e1) => updateBatteryLevel();
            vh.StatusChange -= (s1, e1) => updateStatus();
        }
        #endregion Event

        #region Function

        private void initialAnimation()
        {
            vhPresenter = new ContentControl();
            vhPresenter.Name = p_ID;
            vhPresenter.Content = this;
            vhPresenter.RenderTransform = new TranslateTransform();

            if (isShowcase) return;

            TimeSpan animationDuration = TimeSpan.FromSeconds(1.2);

            doubleAnimation_x.Duration = animationDuration;
            doubleAnimation_y.Duration = animationDuration;
            moveStoryboard = new Storyboard();
            moveStoryboard.Children.Add(doubleAnimation_x);
            moveStoryboard.Children.Add(doubleAnimation_y);

            Storyboard.SetTarget(doubleAnimation_x, vhPresenter);
            Storyboard.SetTarget(doubleAnimation_y, vhPresenter);
            Storyboard.SetTargetProperty(doubleAnimation_x, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
            Storyboard.SetTargetProperty(doubleAnimation_y, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));

            positionUpdat3Timer.Tick += positionUpdat3;
            positionUpdat3Timer.Interval = animationDuration;
            positionUpdat3Timer.Start();
        }

        private void positionUpdat3(object sender, EventArgs e)
        {
            // test
            //if (p_ID == "AGV01")
            //{
            //    if (vh.BatteryLevel == BatteryLevel.None)
            //        vh.BatteryLevel = BatteryLevel.Full;
            //    else
            //        vh.BatteryLevel--;

            //    foreach (var cg in app.ObjCacheManager.GetChargers())
            //    {
            //        cg.IsAlive = true;
            //        if (cg.Status == com.mirle.ibg3k0.sc.App.SCAppConstants.CouplerStatus.Error)
            //            cg.Status = 0;
            //        else
            //            cg.Status++;
            //        if (cg.Status == com.mirle.ibg3k0.sc.App.SCAppConstants.CouplerStatus.Manual)
            //            cg.IsAlive = false;
            //    }
            //    app.ObjCacheManager.onChargerUpdateComplete();
            //}

            try
            {
                double display_x = p_Position.X + p_Offset.X - (this.Width / 2);
                double display_y = p_Position.Y + p_Offset.Y - this.Height;
                if (display_x == doubleAnimation_x.To && display_y == doubleAnimation_y.To) return;
                if (display_x <= 0 || display_y <= 0) return;
                Adapter.Invoke((obj) =>
                {
                    // 未設定過位置
                    if (doubleAnimation_x.To == null || doubleAnimation_y.To == null)
                    {
                        TimeSpan animationDuration = doubleAnimation_x.Duration.TimeSpan;
                        doubleAnimation_x.Duration = TimeSpan.FromSeconds(0);
                        doubleAnimation_y.Duration = TimeSpan.FromSeconds(0);

                        doubleAnimation_x.To = display_x;
                        doubleAnimation_y.To = display_y;
                        moveStoryboard.Begin(this);

                        doubleAnimation_x.Duration = animationDuration;
                        doubleAnimation_y.Duration = animationDuration;
                    }
                    else
                    {
                        doubleAnimation_x.To = display_x;
                        doubleAnimation_y.To = display_y;
                        moveStoryboard.Begin(this);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                if (stopAllTimer)
                {
                    try
                    {
                        positionUpdat3Timer.Stop();
                        positionUpdat3Timer.Tick -= positionUpdat3;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Exception");
                    }
                }
            }
        }

        private void initToolTip()
        {
            var t = new ToolTip();
            ToolTipService.SetInitialShowDelay(t, 0);
            t.Opened += ToolTip_Opened;
            VehicleViewBox.ToolTip = t;
        }
        private void ToolTip_Opened(object sender, RoutedEventArgs e)
        {
            setToolTip();
        }
        private void setToolTip()
        {
            ToolTip t = VehicleViewBox.ToolTip as ToolTip;
            if (t != null)
            {
                List<string> listTP = new List<string>();
                listTP.Add($"Vehicle ID: {p_ID}");
                if (app?.ObjCacheManager.IsForAGVC ?? false)
                    listTP.Add($"Battery Cap: {vh.BATTERY_CAPACITY}");
                listTP.Add($"Action: {vh.ACT_STATUS}");
                listTP.Add($"Current ADR: {vh.CUR_ADR_ID}");
                listTP.Add($"Current SEC: {vh.CUR_SEC_ID}");
                listTP.Add($"Current Position: ({vh.X_AXIS}, {vh.Y_AXIS})");
                if (p_VEHICLE_ACTION_TYPE == VEHICLE_ACTION_TYPE.Cassette)
                    listTP.Add($"CST ID: {vh.CST_ID_L?.Trim() ?? ""}");
                t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
            }
        }

#if IS_FOR_OHTC_NOT_AGVC
#else
        private void displayChargingGIF(object sender, EventArgs e)
        {
            try
            {
                // 切換 VEHICLE_BATTERY_LEVEL 以呈現 Charging GIF
                // Charging_Low >> Charging_Middle >> Charging_High >> Charging_Full >> Charging_Low

                if (p_VEHICLE_BATTERY_LEVEL < VEHICLE_BATTERY_LEVEL.Charging_Low) // 還未進入 Charging GIF 流程，跳到起點
                {
                    p_VEHICLE_BATTERY_LEVEL = getBatteryLevel_ChargingStart();
                }
                else if (p_VEHICLE_BATTERY_LEVEL < VEHICLE_BATTERY_LEVEL.Charging_Full) // 已進入 Charging GIF 流程，往下一步
                {
                    p_VEHICLE_BATTERY_LEVEL++;
                }
                else if (p_VEHICLE_BATTERY_LEVEL >= VEHICLE_BATTERY_LEVEL.Charging_Full) // Charging GIF 流程終點，跳回起點
                {
                    p_VEHICLE_BATTERY_LEVEL = getBatteryLevel_ChargingStart();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                if (stopAllTimer)
                {
                    try
                    {
                        dispatcherTimer_Charging.Stop();
                        dispatcherTimer_Charging.Tick -= displayChargingGIF;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Exception");
                    }
                }
            }
        }
        private VEHICLE_BATTERY_LEVEL getBatteryLevel_ChargingStart()
        {
            try
            {
                VEHICLE_BATTERY_LEVEL batteryLevel_ChargingStart = VEHICLE_BATTERY_LEVEL.Charging_Low;
                switch (vh.BATTERY_LEVEL)
                {
                    case BatteryLevel.None:
                    case BatteryLevel.Low:
                    default:
                        batteryLevel_ChargingStart = VEHICLE_BATTERY_LEVEL.Charging_Low;
                        break;
                    case BatteryLevel.Middle:
                        batteryLevel_ChargingStart = VEHICLE_BATTERY_LEVEL.Charging_Middle;
                        break;
                    case BatteryLevel.High:
                        batteryLevel_ChargingStart = VEHICLE_BATTERY_LEVEL.Charging_High;
                        break;
                    case BatteryLevel.Full:
                        batteryLevel_ChargingStart = VEHICLE_BATTERY_LEVEL.Charging_Full;
                        break;
                }
                return batteryLevel_ChargingStart;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return VEHICLE_BATTERY_LEVEL.Charging_Low;
            }
        }
#endif

        private void changeAlarmVisibility(object sender, EventArgs e)
        {
            try
            {
                switch (Image_Alarm.Visibility)
                {
                    case Visibility.Visible:
                        Image_Alarm.Visibility = Visibility.Hidden;
                        break;
                    case Visibility.Hidden:
                        Image_Alarm.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                if (stopAllTimer)
                {
                    try
                    {
                        dispatcherTimer_Alarm.Stop();
                        dispatcherTimer_Alarm.Tick -= changeAlarmVisibility;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Exception");
                    }
                }
            }
        }

        private void updateModeStatus()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (!vh.IS_TCPIP_CONNECT)
                    {
                        p_VEHICLE_TYPE = VEHICLE_TYPE.Disconnect;
                    }
                    else
                    {
                        switch (vh.MODE_STATUS)
                        {
                            case ModeStatus.AutoMtl:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.AutoMTL;
                                break;
                            case ModeStatus.AutoLocal:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.AutoLocal;
                                break;
                            case ModeStatus.AutoRemote:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.AutoRemote;
                                break;
                            case ModeStatus.InitialPowerOff:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.Disconnect;
                                break;
                            case ModeStatus.InitialPowerOn:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.PowerOn;
                                break;
                            case ModeStatus.AutoCharging:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.AutoCharging;
                                break;
                            case ModeStatus.AutoZoneChange:
                            case ModeStatus.Manual:
                            case ModeStatus.None:
                            default:
                                p_VEHICLE_TYPE = VEHICLE_TYPE.Manual;
                                break;
                        }
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateActionStatus()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    switch (vh.ACT_STATUS)
                    {
                        case ActionStatus.NoCommand:
                            if (vh.IS_CHARGING)
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Charging;
                            else if (vh.IS_PARKED)
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Parked;
                            else if (vh.HAS_CST_L || vh.HAS_CST_R)
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Cassette;
                            else
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Idle;
                            break;
                        //    case ActionStatus.Home:
                        case ActionStatus.Teaching:
                            p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Correcting;
                            break;
                        case ActionStatus.Commanding:
                            if (vh.MODE_STATUS == ModeStatus.AutoLocal ||
                                vh.MODE_STATUS == ModeStatus.AutoMtl ||
                                vh.MODE_STATUS == ModeStatus.AutoMts)
                            //vh.MODE_STATUS == ModeStatus.AutoCharging)
                            {
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Maintenance;
                            }
                            else if (vh.IS_PARKED)
                            {
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Parked;
                            }
                            else if (vh.IS_LOADING)
                            {
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Loading;
                            }
                            else if (vh.IS_UNLOADING)
                            {
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Unloading;
                            }
                            else if (vh.HAS_CST_L || vh.HAS_CST_R)
                            {
                                p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Cassette;
                            }
                            else
                            {
                                switch (vh.CMD_TYPE)
                                {
                                    case CmdType.Load:
                                    case CmdType.LoadUnload:
                                        p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.ReceiveCommand;
                                        break;
                                    case CmdType.Move:
                                    case CmdType.MoveMtport:
                                    case CmdType.MoveCharger:
                                        p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Moving;
                                        break;
                                    case CmdType.MovePark:
                                        p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Parking;
                                        break;
                                    default:
                                        p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Idle;
                                        break;

                                }
                            }
                            break;
                        default:
                            p_VEHICLE_ACTION_TYPE = VEHICLE_ACTION_TYPE.Idle;
                            break;
                    }
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateAlarmStatus()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    p_VEHICLE_ALARM_TYPE = vh.HAS_ERROR ? VEHICLE_ALARM_TYPE.Error : VEHICLE_ALARM_TYPE.None;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updatePauseStatus()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    p_VEHICLE_PAUSE_TYPE = vh.IS_PAUSE_BLOCK ? VEHICLE_PAUSE_TYPE.Block :
                                           vh.IS_PAUSE_RESERVE ? VEHICLE_PAUSE_TYPE.Block :
                                           vh.IS_PAUSE_HID ? VEHICLE_PAUSE_TYPE.HID :
                                           vh.IS_PAUSE_OBS ? VEHICLE_PAUSE_TYPE.Obstructed :
                                           vh.IS_PAUSE_EARTHQUAKE ? VEHICLE_PAUSE_TYPE.Earthquake :
                                           vh.IS_PAUSE_SAFETY_DOOR ? VEHICLE_PAUSE_TYPE.Safety :
                                           vh.IS_PAUSE ? VEHICLE_PAUSE_TYPE.Pause :
                                           VEHICLE_PAUSE_TYPE.None;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateSpeedStatus()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    int iSpeed = Convert.ToInt32(vh.SPEED);
                    p_Display_Value = Convert.ToString(iSpeed);
                    p_VEHICLE_SPEED_TYPE = iSpeed >= 50 ? VEHICLE_SPEED_TYPE.Fast :
                                           iSpeed >= 30 ? VEHICLE_SPEED_TYPE.Medium :
                                                          VEHICLE_SPEED_TYPE.Slow;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateBatteryCapacity()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    p_Display_Value = Convert.ToString(vh.BATTERY_CAPACITY);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateBatteryLevel()
        {
            try
            {
                if (p_IsCharging) return; // will use dispatcherTimer_Charging to display as GIF

                Adapter.Invoke((obj) =>
                {
                    // additional condition for BatteryLevel.None
                    p_VEHICLE_BATTERY_LEVEL = vh.BATTERY_CAPACITY <= 0 ? VEHICLE_BATTERY_LEVEL.None :
                                                                         (VEHICLE_BATTERY_LEVEL)vh.BATTERY_LEVEL;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateChargeStatus()
        {
            try
            {
                //Adapter.Invoke((obj) =>
                //{
                //    p_IsCharging = vh.ChargeStatus == VhChargeStatus.ChargeStatusCharging;
                //}, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateStatus()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    p_Status = p_VEHICLE_TYPE == VEHICLE_TYPE.Disconnect ? "Disconnect" :
                               p_VEHICLE_TYPE == VEHICLE_TYPE.Manual ? "Manual" :
                               p_VEHICLE_TYPE == VEHICLE_TYPE.AutoLocal ? "AutoLocal" :
                               p_VEHICLE_TYPE == VEHICLE_TYPE.AutoMTL ? "AutoMTL" :
                               p_VEHICLE_ALARM_TYPE != VEHICLE_ALARM_TYPE.None ? "Alarm" :
                               p_VEHICLE_PAUSE_TYPE != VEHICLE_PAUSE_TYPE.None ? "Pause" :
                               //p_IsCharging ? "Charging" :
                               p_VEHICLE_ACTION_TYPE == VEHICLE_ACTION_TYPE.Charging ? "Charging" :
                               p_VEHICLE_ACTION_TYPE == VEHICLE_ACTION_TYPE.Correcting ? "Correcting" :
                               p_VEHICLE_ACTION_TYPE == VEHICLE_ACTION_TYPE.Maintenance ? "Maintenance" :
                               p_VEHICLE_ACTION_TYPE == VEHICLE_ACTION_TYPE.Loading ? "Loading" :
                               p_VEHICLE_ACTION_TYPE == VEHICLE_ACTION_TYPE.Unloading ? "Unloading" :
                               !string.IsNullOrWhiteSpace(vh.PATH_TO_ADR) ? "Moving" :
                               "Idle";
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updatePosition(PositionChangeEventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    p_Position = new Point(e.Current_X_Axis, e.Current_Y_Axis);
                }, null);

                updateVisibility();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        private void updateVisibility()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    //this.Visibility = !isShowcase && (p_Position.X <= 0 || p_Position.Y <= 0) ?
                    //                  Visibility.Hidden : Visibility.Visible;
                    // 有負座標
                    this.Visibility = !isShowcase &&
                                      (p_Position.X > -1 && p_Position.X < 1) &&
                                      (p_Position.X > -1 && p_Position.Y < 1) ?
                                      Visibility.Hidden : Visibility.Visible;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void updateCurrentExcuteCmd(string cmd_id)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private string getVehicleNo()
        {
            string result = "";

            for (int i = p_ID.Length - 1; i >= 0; i--)
            {
                if (p_ID[i] >= '0' && p_ID[i] <= '9')
                {
                    result = p_ID[i] + result;
                }
                else break;
            }

            return result;
        }
        #endregion Function

        public void SetFocus()
        {
            ZoomIn?.Invoke(this, new Point(p_Position.X + p_Offset.X + (Width / 2),
                                           p_Position.Y + p_Offset.Y - (Height / 2)));
        }

        public void _MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VehicleBeChosen?.Invoke(this, p_ID);

            if (e.ClickCount >= 2) // Double Click
            {
                //ZoomIn?.Invoke(this, !this.IsVisible ? new Point(0, 0) :
                //new Point(p_Position.X + p_Offset.X + (Width / 2), p_Position.Y + p_Offset.Y - (Height / 2)));
            }
        }

        private void _ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            mi_QUICK_VEHICLE_COMMAND.IsEnabled = string.IsNullOrEmpty(app.LoginUserID) ? false :
                app.UserBLL.CheckUserAuthority(app.LoginUserID, UAS_Def.Maintenance_Function.FUNC_VEHICLE_MANAGEMENT);
        }

        private void _QUICK_VEHICLE_COMMAND_Click(object sender, RoutedEventArgs e)
        {
            OpenQuickVehicleCommand?.Invoke(this, p_ID);
        }
    }
}
