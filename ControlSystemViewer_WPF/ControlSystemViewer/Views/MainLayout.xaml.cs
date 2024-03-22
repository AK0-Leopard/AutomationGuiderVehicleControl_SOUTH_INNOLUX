using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.Utility.uc;
using MainLayout.ViewModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewerObject;

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// MainLayout.xaml 的互動邏輯
    /// </summary>
    public partial class MainLayout : UserControl
    {
        #region 公用參數設定
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public EventHandler RequestLogInOut;
        public WindownApplication app = null;
        private Settings.MainLayout mainLayoutSettings = null;
        private MainLayoutViewModel vm = null;
        private VLINE line = null;
        private Point RecPoint_MouseDown = new Point(-1, -1);
        private bool RecIsHor = false;
        private int RecWidth_Side;
        private int RecHeight_Data;
        private int iDefaultWidth = 0;
        private int iDefaultHeight = 0;
        #endregion 公用參數設定

        public MainLayout()
        {
            InitializeComponent();

            vm = DataContext as MainLayoutViewModel;
        }

        public void Start(WindownApplication _app)
        {
            app = _app;
            mainLayoutSettings = app?.ObjCacheManager.ViewerSettings?.mainLayout;
            if (!(mainLayoutSettings?.ShowSignalLight ?? false))
                grid_SignalLight.Visibility = Visibility.Collapsed;
            bool showPortStatus = mainLayoutSettings?.sideArea.ShowPortStatus ?? false;
            line = app.ObjCacheManager.GetLine();
            if (!app.IsControlSystemConnentable)
            {
                var oProjectInfo = app.ObjCacheManager.GetSelectedProject();
                vm.Str_ProductLine = oProjectInfo.ProductLine;
            }

            Map_Base.Start(app, this, SideArea.StatusTree?.ListVehicle);
            CurrentData.Start(app);
            int width_side = mainLayoutSettings?.sideArea.DefaultWidth ?? 0;
            int height_data = mainLayoutSettings?.currentData.DefaultHeight ?? 0;
            if (vm != null)
            {
                vm.app = app;
                //vm.Str_ProductLine = _app.ObjCacheManager.MapId;
                vm.Str_ProductLine = line?.LINE_ID ?? "";

                int width_PortStatus = 0;
                if (showPortStatus)
                {
                    // Resize PortStatus
                    var monitoringPorts = app.ObjCacheManager.GetMonitoringPorts();
                    int maxStageCount = monitoringPorts.Any(o => o.COUNT_STAGE == 5) ? 5 :
                                        monitoringPorts.Any(o => o.COUNT_STAGE == 4) ? 4 :
                                        monitoringPorts.Any(o => o.COUNT_STAGE == 3) ? 3 :
                                        monitoringPorts.Any(o => o.COUNT_STAGE == 2) ? 2 :
                                        monitoringPorts.Any(o => o.COUNT_STAGE == 1) ? 1 : 0;
                    //int maxStageCount = 1; // agvc 1 for charging space
                    width_PortStatus = maxStageCount * 90 + 25 + (int)SystemParameters.VerticalScrollBarWidth;
                }
                width_side = width_PortStatus > width_side ? width_PortStatus : width_side;
                vm.Width_Side = width_side;
                vm.Height_Data = height_data;
                //vm.Side(); // Hide

                iDefaultWidth = vm.Width_Side;
                iDefaultHeight = vm.Height_Data;
                vm.ControlSystemConnectionChanged += controlSystemConnectionChanged;
                vm.Start();
            }
            SideArea.StatusTree.Start(width_side);

            if (showPortStatus)
            {
                SideArea.tab_PortStatus.Visibility = Visibility.Visible;
                SideArea.PortStatus.Start(app);
            }

            ObjCacheManager_AlarmChange(null, null);
            _LineInfoChange(null, null);
            registerEvents();
        }

        #region Event
        private void registerEvents()
        {
            //ResizeLine_Hor.MouseDown += ResizeLine_Hor_MouseDown;
            ResizeLine_Ver.MouseDown += ResizeLine_Ver_MouseDown;
            this.MouseMove += ResizeLine_MouseMove;
            this.MouseUp += ResizeLine_MouseUp;
            //ResizeLine_Hor.MouseMove += ResizeLine_Hor_MouseMove;
            //ResizeLine_Hor.MouseUp += ResizeLine_Hor_MouseUp;

            app.ObjCacheManager.LogInUserChanged += ObjCacheManager_LogInUserChanged;
            app.ObjCacheManager.AlarmChange += ObjCacheManager_AlarmChange;
            line.LineInfoChange += _LineInfoChange;
            line.ConnectionInfoChange += _LineInfoChange;
            app.ObjCacheManager.ChargerUpdateComplete += ObjCacheManager_ChargerUpdateComplete;

            Map_Base.MonitoringVhChanged += _MonitoringVhChanged;
            CurrentData.SelectedVhChanged += _SelectedVhChanged;
        }

        private void unregisterEvents()
        {
            //ResizeLine_Hor.MouseDown -= ResizeLine_Hor_MouseDown;
            ResizeLine_Ver.MouseDown -= ResizeLine_Ver_MouseDown;
            this.MouseMove -= ResizeLine_MouseMove;
            this.MouseUp -= ResizeLine_MouseUp;
            //ResizeLine_Hor.MouseMove -= ResizeLine_Hor_MouseMove;
            //ResizeLine_Hor.MouseUp -= ResizeLine_Hor_MouseUp;

            app.ObjCacheManager.LogInUserChanged -= ObjCacheManager_LogInUserChanged;
            app.ObjCacheManager.AlarmChange -= ObjCacheManager_AlarmChange;
            line.LineInfoChange -= _LineInfoChange;
            line.ConnectionInfoChange -= _LineInfoChange;
            app.ObjCacheManager.ChargerUpdateComplete -= ObjCacheManager_ChargerUpdateComplete;

            Map_Base.MonitoringVhChanged -= _MonitoringVhChanged;
            CurrentData.SelectedVhChanged -= _SelectedVhChanged;
        }

        private void controlSystemConnectionChanged(object sender, bool isConnected)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    SideArea.StatusTree.ControlSystemConnectionChange(isConnected);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_LogInUserChanged(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (string.IsNullOrEmpty(app.LoginUserID)) //未登入
                    {
                        lbl_LogIn_Default.Visibility = Visibility.Visible;
                        lbl_LogIn.Visibility = Visibility.Hidden;

                        if (SideArea.tab_QuickVhCmd.IsSelected)
                            SideArea.tab_StatusTree.IsSelected = true;
                        SideArea.tab_QuickVhCmd.Visibility = Visibility.Hidden;

                        Operation_Base.Close();
                        Maintenance_Base.Close();
                    }
                    else
                    {
                        lbl_LogIn_Default.Visibility = Visibility.Hidden;
                        lbl_LogIn.Visibility = Visibility.Visible;

                        if (app.UserBLL.CheckUserAuthority(app.LoginUserID, UAS_Def.Maintenance_Function.FUNC_VEHICLE_MANAGEMENT))
                        {
                            SideArea.tab_QuickVhCmd.Visibility = Visibility.Visible;
                            SideArea.QuickVehicleCommand.Start();
                        }
                    }
                    lbl_LogIn.Content = app.LoginUserID;
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_AlarmChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (vm != null)
                    {
                        vm.SetSignal("Alarm", app.ObjCacheManager.GetAlarms().Count <= 0);
                    }
                    SideArea.StatusTree.ControlSystemInfoUpdate();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _LineInfoChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (vm != null && app != null)
                    {
                        vm.SetSignal("Host", line.IS_LINK_HOST);

                        StatusCST.SetNumOfWaiting(line.COUNT_CST_STATE_WAITING);
                        StatusCST.SetNumOfTransfer(line.COUNT_CST_STATE_TRANSFER);

                        StatusMCSQueue.SetNumOfWaitingAssigned(line.COUNT_HOST_CMD_TRANSFER_STATE_WAITING);
                        StatusMCSQueue.SetNumOfAssigned(line.COUNT_HOST_CMD_TRANSFER_STATE_ASSIGNED);

                        StatusVehicle.SetNumOfAutoRemote(line.COUNT_VEHICLE_MODE_AUTO_REMOTE);
                        StatusVehicle.SetNumOfAutoLocal(line.COUNT_VEHICLE_MODE_AUTO_LOCAL);
                        StatusVehicle.SetNumOfIdle(line.COUNT_VEHICLE_MODE_IDLE);
                        StatusVehicle.SetNumOfError(line.COUNT_VEHICLE_MODE_ERROR);
                    }
                    SideArea.StatusTree.ControlSystemInfoUpdate();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_ChargerUpdateComplete(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    SideArea.StatusTree?.ChargerUpdate();
                    Map_Base?.ResetAdr_Charger(); // update address background color for charger 
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _MonitoringVhChanged(object sender, string vh_id)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    SideArea.StatusTree?.SetSelectedVehicle(vh_id);
                    CurrentData?.SelectVh_VHStatus(vh_id);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _SelectedVhChanged(object sender, string vh_id)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    Map_Base?.SetQuickVehicleCommand_VhID(this, vh_id);
                    Map_Base?.SetMonitorVehicle(this, vh_id);
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ResizeLine_Hor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RecIsHor = true;
            ResizeLine_MouseDown(sender, e);
        }

        private void ResizeLine_Ver_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RecIsHor = false;
            ResizeLine_MouseDown(sender, e);
        }

        private void ResizeLine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && RecPoint_MouseDown == new Point(-1, -1) && vm != null)
            {
                RecPoint_MouseDown = e.GetPosition(this);
                RecWidth_Side = vm.Width_Side;
                RecHeight_Data = vm.Height_Data;
            }
        }

        private void ResizeLine_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && RecPoint_MouseDown != new Point(-1, -1) && vm != null)
            {
                if (RecIsHor)
                {
                    int diff = (int)(e.GetPosition(this).X - RecPoint_MouseDown.X);
                    int newWidth_Side = RecWidth_Side + diff;
                    vm.Width_Side = newWidth_Side < 0 ? 0 : newWidth_Side > this.ActualWidth ? (int)this.ActualWidth : newWidth_Side;
                }
                else //if(!RecIsHor)
                {
                    int diff = (int)(e.GetPosition(this).Y - RecPoint_MouseDown.Y);
                    int newHeight_Data = RecHeight_Data - diff;
                    vm.Height_Data = newHeight_Data < 0 ? 0 : newHeight_Data > this.ActualHeight ? (int)this.ActualHeight : newHeight_Data;
                }
            }
        }

        private void ResizeLine_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && RecPoint_MouseDown != new Point(-1, -1))
            {
                RecPoint_MouseDown = new Point(-1, -1);
            }
        }

        private void SetDefaultWidth_Click(object sender, RoutedEventArgs e)
        {
            if (vm != null)
            {
                vm.Width_Side = iDefaultWidth;
            }
        }

        private void SetDefaultHeight_Click(object sender, RoutedEventArgs e)
        {
            if (vm != null)
            {
                vm.Height_Data = iDefaultHeight;
            }
        }

        private void HideCurrentData_Click(object sender, RoutedEventArgs e)
        {
            if (vm != null)
            {
                vm.Height_Data = 0;
            }
        }
        #endregion Event

        private void btnLogInOut_Click(object sender, RoutedEventArgs e)
        {
            RequestLogInOut?.Invoke(sender, EventArgs.Empty);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Operation_Base.Close();
            Maintenance_Base.Close();
            Log_Base.Close();
            Report_Base.Close();
            Map_Base.InitMonitorVehicle();
            Map_Base.setReservedRail(app?.ObjCacheManager?.GetAllReserveInfo() ?? new List<string>());
        }

        #region Quick Vehicle Command
        public void QuickVehicleCommand_BringToFront()
        {
            if (!SideArea.tab_QuickVhCmd.IsVisible) return;

            if (!SideArea.tab_QuickVhCmd.IsSelected)
                SideArea.tab_QuickVhCmd.IsSelected = true;
        }

        public void QuickVehicleCommand_SelectVh(string vh_id)
        {
            if (!SideArea.tab_QuickVhCmd.IsVisible) return;

            QuickVehicleCommand_BringToFront();

            SideArea.QuickVehicleCommand.SelectVehicle(vh_id);
        }

        private int iSelectType_Switch = 1; // 0:From, 1:To
        public void QuickVehicleCommand_SelectAdr(string sAddr)
        {
            if (!SideArea.tab_QuickVhCmd.IsVisible) return;

            QuickVehicleCommand_BringToFront();

            if (iSelectType_Switch == 0)
            {
                SideArea.QuickVehicleCommand.SelectFromAdr(sAddr);
            }
            else //if (iSelectType_Switch == 1)
            {
                SideArea.QuickVehicleCommand.SelectToAdr(sAddr);
            }
            iSelectType_Switch = iSelectType_Switch == 0 ? 1 : 0;
        }
        #endregion Quick Vehicle Command
    }
}
