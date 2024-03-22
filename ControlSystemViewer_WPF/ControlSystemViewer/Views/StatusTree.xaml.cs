using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UtilsAPI.Tool;
using ViewerObject;
using static ViewerObject.VLINE_Def;
using static Vehicle.ViewModel.VehicleViewModel;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using HostControlState = ViewerObject.VLINE_Def.HostControlState;
using static ViewerObject.Coupler_Def;
using MirleGO_UIFrameWork.UI.uc_Button;
using com.mirle.ibg3k0.bc.wpf.App;

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// StatusTree.xaml 的互動邏輯
    /// </summary>
    public partial class StatusTree : UserControl
    {
        #region 公用參數設定
        private WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public List<Map_Parts.Vehicle> ListVehicle = null;
        public List<ChargerIcon> ListChargerIcon = null;
        private bool isConnected_ControlSystem = false;
        private bool timerLock = false; //紀錄timer所觸發的執行序是否有被做完
        public bool IsConnected_ControlSystem
        {
            get { return isConnected_ControlSystem; }
            set
            {
                if (isConnected_ControlSystem != value)
                {
                    isConnected_ControlSystem = value;

                    if (isConnected_ControlSystem)
                        dispatcherTimer.Start();
                    else
                        dispatcherTimer.Stop();
                }
            }
        }


        #endregion 公用參數設定

        public StatusTree()
        {
            InitializeComponent();

            dispatcherTimer.Interval = TimeSpan.FromSeconds(5);
            dispatcherTimer.Tick += RefreshChargerData;
        }

        public async void RefreshChargerData(object sender, EventArgs e)
        {
            if (timerLock) return;
            timerLock = true;
            //if (!dispatcherTimer.IsEnabled) return;
            //dispatcherTimer.IsEnabled = false;
            bool isSuccess = false;
            try
            {
                app = app ?? WindownApplication.getInstance();

                await Task.Run(() => isSuccess = app.ObjCacheManager.updateChargers());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                timerLock = false;
                if (!isSuccess)
                {
                    //dispatcherTimer.Tick -= RefreshChargerData;
                    //dispatcherTimer.Stop();
                }
                //else dispatcherTimer.IsEnabled = true;
            }
        }

        public void Start(int width)
        {
            try
            {
                tv_Status.Width = width;

                app = WindownApplication.getInstance();

                initialVehicle();

                initialCharger();

                setStatusTree();

                registerEvents();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Close()
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

        private void initialVehicle()
        {
            app = app ?? WindownApplication.getInstance();
            ListVehicle = new List<Map_Parts.Vehicle>();
            var vhs = app.ObjCacheManager.GetVEHICLEs();
            if (vhs?.Count > 0)
            {
                foreach (var vh in vhs)
                {
                    ListVehicle.Add(new Map_Parts.Vehicle(app, vh, new Point(0, 0), 30, _isShowcase: true));
                }
            }
        }

        private void initialCharger()
        {
            app = app ?? WindownApplication.getInstance();
            ListChargerIcon = new List<ChargerIcon>();
            List<Charger> cgs = app.ObjCacheManager.GetChargers();

            if (cgs?.Count > 0)
            {
                foreach (var cg in cgs)
                {
                    ListChargerIcon.Add(new ChargerIcon(app, cg, 20));
                }
            }
        }

        public void ControlSystemConnectionChange(bool isConnected)
        {
            try
            {
                IsConnected_ControlSystem = isConnected;
                refreshBranch_ControlSystem();
                refreshBranch_Vehicle();
                refreshBranch_Charger();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void ControlSystemInfoUpdate()
        {
            try
            {
                refreshBranch_ControlSystem();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void ChargerUpdate()
        {
            try
            {
                refreshBranch_Charger();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void registerEvents()
        {
            if (ListVehicle?.Count > 0)
            {
                foreach (Map_Parts.Vehicle vehicle in ListVehicle)
                {
                    vehicle.VehicleStatusChanged += vehicleStatusChanged;
                }
            }
        }

        private void unregisterEvents()
        {
            if (ListVehicle?.Count > 0)
            {
                foreach (Map_Parts.Vehicle vehicle in ListVehicle)
                {
                    vehicle.VehicleStatusChanged -= vehicleStatusChanged;
                }
            }
        }

        private void vehicleStatusChanged(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refreshBranch_Vehicle();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void setStatusTree()
        {
            try
            {
                List<TreeViewModel> treeView = new List<TreeViewModel>();
                treeView.Add(getBranch_Vehicle());
                if (app?.ObjCacheManager.GetSelectedProject()?.ObjectConverter?.ToUpper().Contains("AGVC") ?? false)
                    treeView.Add(getBranch_Charger());
                treeView.Add(getBranch_ControlSystem());
                tv_Status.ItemsSource = treeView;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void SetSelectedVehicle(string vh_id)
        {
            try
            {
                List<TreeViewModel> treeView = tv_Status.ItemsSource as List<TreeViewModel>;
                if (treeView?.Count > 0)
                {
                    foreach (TreeViewModel branch in treeView)
                    {
                        if (branch.Name == "Vehicle")
                        {
                            if (branch?.Children?.Count > 0)
                            {
                                foreach (TreeViewModel tv in branch.Children)
                                {
                                    tv.IsInitiallySelected = tv.Name?.Trim() == vh_id?.Trim();
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void setSelectedItem(TreeViewModel select, TreeViewModel branch)
        {
            if (select != null && branch?.Children?.Count > 0)
            {
                foreach (TreeViewModel tv in branch.Children)
                {
                    tv.IsInitiallySelected = tv.Name?.Trim() == select.Name?.Trim();
                }
            }
        }

        private void refreshBranch_Vehicle()
        {
            try
            {
                TreeViewModel select = tv_Status.SelectedItem as TreeViewModel;
                List<TreeViewModel> treeView = tv_Status.ItemsSource as List<TreeViewModel>;
                if (treeView?.Count > 0)
                {
                    foreach (TreeViewModel branch in treeView)
                    {
                        if (branch.Name == "Vehicle")
                        {
                            branch.SetTreeViewModel(IsConnected_ControlSystem ? getBranch_Vehicle() : getBranch_Vehicle_Empty());
                            setSelectedItem(select, branch);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void refreshBranch_Charger()
        {
            try
            {
                TreeViewModel select = tv_Status.SelectedItem as TreeViewModel;
                List<TreeViewModel> treeView = tv_Status.ItemsSource as List<TreeViewModel>;
                if (treeView?.Count > 0)
                {
                    foreach (TreeViewModel branch in treeView)
                    {
                        if (branch.Name == "Charger")
                        {
                            branch.SetTreeViewModel(IsConnected_ControlSystem ? getBranch_Charger() : getBranch_Charger_Empty());
                            setSelectedItem(select, branch);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void refreshBranch_ControlSystem()
        {
            try
            {
                TreeViewModel select = tv_Status.SelectedItem as TreeViewModel;
                List<TreeViewModel> treeView = tv_Status.ItemsSource as List<TreeViewModel>;
                if (treeView?.Count > 0)
                {
                    foreach (TreeViewModel branch in treeView)
                    {
                        if (branch.Name == "ControlSystem")
                        {
                            branch.SetTreeViewModel(getBranch_ControlSystem());
                            setSelectedItem(select, branch);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private TreeViewModel getBranch_Vehicle_Empty()
        {
            string sTitle = "Vehicle";
            try
            {
                return new TreeViewModel(sTitle, "");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return null;
            }
        }
        private TreeViewModel getBranch_Vehicle()
        {
            string sTitle = "Vehicle";
            try
            {
                List<TreeViewModel> ltv_Vehicle = new List<TreeViewModel>();
                int countDisconnect = 0;
                foreach (var vehicle in ListVehicle)
                {
                    string sStatus = vehicle.p_Status;
                    Brush brush = null;
                    if (vehicle.p_VEHICLE_TYPE == VEHICLE_TYPE.Disconnect)
                    {
                        countDisconnect++;
                        brush = Brushes.DarkGray;
                    }
                    ltv_Vehicle.Add(new TreeViewModel(vehicle.p_ID.Trim(), $"{sStatus}", brush, brush, objectIcon: vehicle));
                }
                TreeViewModel branchVehicle = new TreeViewModel(sTitle, $"{ListVehicle.Count - countDisconnect} / {ListVehicle.Count}", children: ltv_Vehicle);
                return branchVehicle;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return new TreeViewModel(sTitle, "");
            }
        }

        private TreeViewModel getBranch_Charger_Empty()
        {
            string sTitle = "Charger";
            try
            {
                return new TreeViewModel(sTitle, "");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return null;
            }
        }
        private TreeViewModel getBranch_Charger()
        {
            string sTitle = "Charger";
            try
            {
                List<TreeViewModel> ltv_Charger = new List<TreeViewModel>();
               
                CouplerIcon oCouplerIcon = null;
                int countDisconnect = 0;
                foreach (var chargerIcon in ListChargerIcon)
                {
                    chargerIcon.Update();
                    Charger charger = chargerIcon.ChargerData;
                    //string sStatus = charger.Status == ChargerStatus.Disable ?
                    //                 "Unexecute" : charger.Status.ToString();
                    List<TreeViewModel> ltv_Coupler = new List<TreeViewModel>();
                    string sStatus = "Online";
                    Brush brush = null;
                    Brush brush_coupler = null;
                    if (!charger.IsAlive)
                    {
                        countDisconnect++;
                        sStatus = "Offline";
                        brush = Brushes.DarkGray;
                    }
                    foreach (var cp in charger.Couplers.Values.OrderBy(c => c.Name).ToList())
                    {
                        oCouplerIcon = new CouplerIcon(app, cp, 20);
                        brush_coupler = null;
                        if (cp.Status == CouplerStatus.Disable || cp.Status == CouplerStatus.None || (!charger.IsAlive)) brush_coupler = Brushes.DarkGray;
                        //ltv_Coupler.Add(new TreeViewModel($"{cp.Name}", $"{cp.Status.ToString()}", brush_coupler, brush_coupler, objectIcon: oCouplerIcon));
                        ltv_Coupler.Add(new TreeViewModel($"{cp.Name}", $"{cp.ShowStatus}", brush_coupler, brush_coupler, objectIcon: oCouplerIcon));
                    }
                    ltv_Charger.Add(new TreeViewModel($"{charger.ChargerID}", $"{sStatus}", brush, brush, children: ltv_Coupler, objectIcon: chargerIcon));
                }
                TreeViewModel branchVehicle = new TreeViewModel(sTitle, $"{ListChargerIcon.Count - countDisconnect} / {ListChargerIcon.Count}", children: ltv_Charger);
                return branchVehicle;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return new TreeViewModel(sTitle, "");
            }
        }

        private TreeViewModel getBranch_ControlSystem()
        {
            string sTitle = "ControlSystem";
            try
            {
                string sSystemConnection = IsConnected_ControlSystem ? "Connect" : "Disconnect";
                Brush brush = IsConnected_ControlSystem ? Brushes.LimeGreen : Brushes.Red;
                List<TreeViewModel> ltv_ControlSystem = null;
                if (IsConnected_ControlSystem)
                {
                    app = app ?? WindownApplication.getInstance();
                    VLINE line = app.ObjCacheManager.GetLine();
                    ltv_ControlSystem = new List<TreeViewModel>();
                    string sCommuState = line.IS_LINK_HOST ? "ENABLED" : "DISABLED";
                    ltv_ControlSystem.Add(new TreeViewModel("Commu State", sCommuState));
                    string sControlState = line.HOST_CONTROL_STATE == HostControlState.EQ_Off_line ? "EQ OFFLINE" :
                                           line.HOST_CONTROL_STATE == HostControlState.Host_Offline ? "Host OFFLINE" :
                                           line.HOST_CONTROL_STATE == HostControlState.Going_Online ? "Attempt ONLINE" :
                                           line.HOST_CONTROL_STATE == HostControlState.On_Line_Local ? "ONLINE LOCAL" :
                                           line.HOST_CONTROL_STATE == HostControlState.On_Line_Remote ? "ONLINE REMOTE" :
                                           "OFFLINE";
                    ltv_ControlSystem.Add(new TreeViewModel("Control State", sControlState));
                    ltv_ControlSystem.Add(new TreeViewModel("TSC State", $"{line.TSC_STATE}"));
                    //string sAlarmState = aline.IsAlarmHappened ? "ALARMS" : "NO ALARMS";
                    string sAlarmState = app.ObjCacheManager.GetAlarms().Count > 0 ? "ALARMS" : "NO ALARMS";
                    ltv_ControlSystem.Add(new TreeViewModel("TSC Alarm State", sAlarmState));
                    //string sVehicleComm = aline.Redis_Link_Stat == com.mirle.ibg3k0.sc.App.SCAppConstants.LinkStatus.LinkOK ? "ON" : "OFF";
                    //ltv_ControlSystem.Add(new TreeViewModel("Vehicle Comm", sVehicleComm));
                    //string sChargerComm = aline.Redis_Link_Stat == com.mirle.ibg3k0.sc.App.SCAppConstants.LinkStatus.LinkOK ? "ON" : "OFF";
                    //ltv_ControlSystem.Add(new TreeViewModel("Charger Comm", sChargerComm));
                    //string sIOComm = aline.Redis_Link_Stat == com.mirle.ibg3k0.sc.App.SCAppConstants.LinkStatus.LinkOK ? "ON" : "OFF";
                    //ltv_ControlSystem.Add(new TreeViewModel("I/O Comm", sIOComm));
                }
                TreeViewModel branchControlSystem = new TreeViewModel(sTitle, sSystemConnection, textBrush: brush, children: ltv_ControlSystem);
                return branchControlSystem;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return new TreeViewModel(sTitle, "");
            }
        }

        public class ChargerIcon : UserControl
        {
            private WindownApplication app = null;
            public Charger ChargerData;
            private Canvas canvas;
            private Ellipse ellipse;
            private Image image;
            //public List<CouplerIcon> CouplerIcons= new List<CouplerIcon>();

            public ChargerIcon(WindownApplication _app, Charger charger, double width)
            {
                app = _app;
                ChargerData = charger;

                canvas = new Canvas();
                canvas.Width = width;
                canvas.Height = width;
                this.Content = canvas;
                this.Width = width;
                this.Height = width;
                this.Margin = new Thickness(-15, 0, 0, 0); // to match Vehicle Icon

                initEllipse();
                initImage();

                initToolTip();


            }

            public void Update()
            {
                ellipse.Fill = ChargerData?.StatusBrush ?? Brushes.Gray;
            }

            private void initEllipse()
            {
                ellipse = new Ellipse();
                ellipse.StrokeThickness = 0;
                ellipse.Stroke = Brushes.Transparent;
                ellipse.Fill = ChargerData?.StatusBrush ?? Brushes.Gray;
                ellipse.Width = this.Width;
                ellipse.Height = this.Height;
                canvas.Children.Add(ellipse);
            }
            private void initImage()
            {
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                image = new Image();
                image.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/SystemIcon/charging.png"));
                image.Width = this.Width;
                image.Height = this.Height;
                canvas.Children.Add(image);
            }

            private void initToolTip()
            {
                var t = new ToolTip();
                ToolTipService.SetInitialShowDelay(t, 0);
                t.Opened += ToolTip_Opened;
                this.ToolTip = t;
            }
            private void ToolTip_Opened(object sender, RoutedEventArgs e)
            {
                setToolTip();
            }
            private void setToolTip()
            {
                ToolTip t = this.ToolTip as ToolTip;
                if (t != null)
                {
                    List<string> listTP = new List<string>();
                    listTP.Add($"ChargerID: {ChargerData.ChargerID}");
                    listTP.Add($"AddressID: {ChargerData.AddressID}");
                    listTP.Add($"IsAlive: {ChargerData.IsAlive}");
                    //listTP.Add($"Status: {ChargerData.Status}");//TODO:這個應該可以刪掉
                    ToolTipService.SetInitialShowDelay(t, 0);
                    t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
                }
            }
        }

        public class CouplerIcon : UserControl
        {
            private WindownApplication app = null;
            public Coupler CouplerData;
            private Canvas canvas;
            private Ellipse ellipse;
            private Image image;
            private MenuItem menuItem_chargeToEnable, menuItem_chargeToDisable;

            public CouplerIcon(WindownApplication _app, Coupler coupler, double width)
            {
                app = _app;
                CouplerData = coupler;

                canvas = new Canvas();
                canvas.Width = width;
                canvas.Height = width;
                this.Content = canvas;
                this.Width = width;
                this.Height = width;
                this.Margin = new Thickness(-15, 0, 0, 0); // to match Vehicle Icon

                initEllipse();
                initImage();

                initToolTip();

                menuItem_chargeToEnable = new MenuItem();
                menuItem_chargeToEnable.Click += chargeToEnable;
                menuItem_chargeToDisable = new MenuItem();
                menuItem_chargeToDisable.Click += chargeToDisable;

                menuItem_chargeToEnable.Header = "Enable";
                menuItem_chargeToDisable.Header = "Disable";

                var contextMenu = new ContextMenu();
                //if (coupler.Status == CouplerStatus.Disable)
                //{
                //    menuItem_chargeToEnable.IsEnabled = true;
                //    menuItem_chargeToDisable.IsEnabled = false;
                //}
                //else
                //{
                //    menuItem_chargeToEnable.IsEnabled = false;
                //    menuItem_chargeToDisable.IsEnabled = true;
                //}
                contextMenu.Items.Add(menuItem_chargeToEnable);
                contextMenu.Items.Add(menuItem_chargeToDisable);
                canvas.ContextMenu = contextMenu;
            }

            public void Update()
            {
                ellipse.Fill = CouplerData?.StatusBrush ?? Brushes.Gray;
            }

            private void initEllipse()
            {
                ellipse = new Ellipse();
                ellipse.StrokeThickness = 0;
                ellipse.Stroke = Brushes.Transparent;
                ellipse.Fill = CouplerData?.StatusBrush ?? Brushes.Gray;
                ellipse.Width = this.Width;
                ellipse.Height = this.Height;
                canvas.Children.Add(ellipse);
            }
            private void initImage()
            {
                string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
                image = new Image();
                image.Source = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + $"/Resources/SystemIcon/charging.png"));
                image.Width = this.Width;
                image.Height = this.Height;
                canvas.Children.Add(image);
            }

            private void initToolTip()
            {
                var t = new ToolTip();
                ToolTipService.SetInitialShowDelay(t, 0);
                t.Opened += ToolTip_Opened;
                this.ToolTip = t;
            }
            private void ToolTip_Opened(object sender, RoutedEventArgs e)
            {
                setToolTip();
            }
            private void setToolTip()
            {
                ToolTip t = this.ToolTip as ToolTip;
                if (t != null)
                {
                    List<string> listTP = new List<string>();
                    listTP.Add($"CouplerID: {CouplerData.Name}");
                    listTP.Add($"AddressID: {CouplerData.AddressID}");
                    //listTP.Add($"IsAlive: {ChargerData.IsAlive}");
                    //listTP.Add($"Status: {CouplerData.Status}");
                    listTP.Add($"Status: {CouplerData.ShowStatus}");
                    ToolTipService.SetInitialShowDelay(t, 0);
                    t.Content = BasicFunction.StringRelate.ConvertStringListToString(listTP);
                }
            }

            private void chargeToEnable(object sender, RoutedEventArgs args)
            {
                askChargeEnableOrDisabel(true);
            }

            private void askChargeEnableOrDisabel(bool isEnalbe)
            {
                var ask_result = app.ObjCacheManager.chargeControl(CouplerData.ChargerID, CouplerData.ID, isEnalbe);

                if (!ask_result.isSuccess)
                {
                    TipMessage_Type_Light.Show("Send command failed", ask_result.result, BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light_woBtn.Show("", "Send command succeeded", BCAppConstants.INFO_MSG);
                }
                app.OperationHistoryBLL.
                    addOperationHis(app.LoginUserID,
                                    nameof(StatusTree),
                                    $"Excute charger ID:{CouplerData.ChargerID} coupler ID:{CouplerData.ID} is Enable:{isEnalbe}, action result:{ask_result.result}");
            }

            private void chargeToDisable(object sender, RoutedEventArgs args)
            {
                askChargeEnableOrDisabel(false);
            }
        }

        private void pnl_Item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var id = ((System.Windows.Controls.Label)(((StackPanel)sender)?.Children?[3]))?.Content as String;
                Adapter.Invoke((obj) =>
                {
                    ((MainWindow)App.Current.MainWindow).MainView.MainLayout.Map_Base.TransferObjectMouseLeftButtonDown(id, sender, e);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }



    }
}
