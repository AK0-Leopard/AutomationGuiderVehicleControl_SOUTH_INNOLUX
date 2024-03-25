using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using com.mirle.ibg3k0.Utility.uc;
using ControlSystemViewer.PopupWindows;
using MainView.ViewModel;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using System;
using System.Collections.Generic;
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
using ViewerObject;

namespace ControlSystemViewer.Views
{
    /// <summary>
    /// MainView.xaml 的互動邏輯
    /// </summary>
    public partial class MainView : UserControl
    {
        #region 公用參數設定
        public WindownApplication app = null;
        private Settings.MainView mainViewSettings = null;
        private TipMessagePopupWindow tipMessagePopupWindow = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region MenuItem
        private Settings.MenuItem_System menuItem_System = null;
        private Settings.MenuItem_Operation menuItem_Operation = null;
        private Settings.MenuItem_Maintenance menuItem_Maintenance = null;
        private Settings.MenuItem_Log menuItem_Log = null;
        #endregion
        #endregion 公用參數設定

        public MainView()
        {
            InitializeComponent();
        }

        public void Start(WindownApplication _app)
        {
            app = _app;
            mainViewSettings = app?.ObjCacheManager.ViewerSettings?.mainView;

            menuItem_System = app?.ObjCacheManager.ViewerSettings?.menuItem_System;
            menuItem_Operation = app?.ObjCacheManager.ViewerSettings?.menuItem_Operation;
            menuItem_Maintenance = app?.ObjCacheManager.ViewerSettings?.menuItem_Maintenance;
            menuItem_Log = app?.ObjCacheManager.ViewerSettings?.menuItem_Log;
            DynamicMenu_Item_Set();

            MainLayout.Start(app);
            MainViewModel vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.app = app;
                vm.Start();
                vm.SetLogoCustomer(mainViewSettings?.CustomerLogo);
            }
       
            ObjCacheManager_TipMessagesChange(null, app?.ObjCacheManager?.TipMessages);

            checkAuthority();
            registerEvents();

            new LogInPopupWindow().ShowDialog();
        }

        private void checkAuthority()
        {
            if (string.IsNullOrEmpty(app.LoginUserID)) //未登入
            {
                MenuItem_LogIn.Visibility = Visibility.Visible;
                MenuItem_LogOut.Visibility = Visibility.Collapsed;
                MenuItem_ChangePassword.Visibility = Visibility.Collapsed;
                MenuItem_AccountManagement.Visibility = Visibility.Collapsed;
                MenuItem_Operation.IsEnabled = false;
                MenuItem_Maintenance.IsEnabled = false;
            }
            else
            {
                MenuItem_LogIn.Visibility = Visibility.Collapsed;
                MenuItem_LogOut.Visibility = Visibility.Visible;
                MenuItem_ChangePassword.Visibility = Visibility.Visible;
                MenuItem_AccountManagement.Visibility = app.UserBLL.CheckUserAuthority(app.LoginUserID, UAS_Def.System_Function.FUNC_USER_MANAGEMENT) ?
                                                        Visibility.Visible : Visibility.Collapsed;
                MenuItem_Operation.IsEnabled = app.UserBLL.CheckUserAuthority(app.LoginUserID, UAS_Def.Operation_Function.FUNC_OPERATION_FUN);
                MenuItem_Maintenance.IsEnabled = app.UserBLL.CheckUserAuthority(app.LoginUserID, UAS_Def.Maintenance_Function.FUNC_MAINTENANCE_FUN);
            }
        }

        #region Event
        private void registerEvents()
        {
            MainLayout.RequestLogInOut += MainLayout_RequestLogInOut;
            app.ObjCacheManager.LogInUserChanged += ObjCacheManager_LogInUserChanged;
            app.ObjCacheManager.TipMessagesChange += ObjCacheManager_TipMessagesChange;
        }

        private void unregisterEvents()
        {
            MainLayout.RequestLogInOut -= MainLayout_RequestLogInOut;
            app.ObjCacheManager.LogInUserChanged -= ObjCacheManager_LogInUserChanged;
        }

        private void MainLayout_RequestLogInOut(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(app.LoginUserID)) //未登入
                LOGIN_Click(sender, null);
            else
                LOGOUT_Click(sender, null);
        }

        private void ObjCacheManager_LogInUserChanged(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    checkAuthority();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_TipMessagesChange(object sender, List<VTIPMESSAGE> tip_message)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    if (tipMessagePopupWindow == null)
                    {
                        tipMessagePopupWindow = new TipMessagePopupWindow();
                        tipMessagePopupWindow.CloseEvent += TipMessagePopupWindow_CloseEvent;
                    }
                    tipMessagePopupWindow.SetTipMessage(tip_message); //this function will deside popup or not
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        #endregion Event

        #region Click Language
        private void ChangeLanguage_enUS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                app.ObjCacheManager.LoadLanguageFile("en-US.xaml");
                app.LanguageCode = "en-US";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ChangeLanguage_zhCN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                app.ObjCacheManager.LoadLanguageFile("zh-CN.xaml");
                app.LanguageCode = "zh-CN";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ChangeLanguage_zhTW_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                app.ObjCacheManager.LoadLanguageFile("zh-TW.xaml");
                app.LanguageCode = "zh-TW";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }


        #endregion Click Language

        #region Click System
        private void LOGIN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new LogInPopupWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void LOGOUT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
                string sTipMsg = languageDictionary?["TIPMSG_LOGOUT_CONFIRM"]?.ToString() ?? "Are you sure to log out now?";
                var confirmResult = TipMessage_Request_Light.Show(sTipMsg + $" ({app?.LoginUserID})");
                if (confirmResult == System.Windows.Forms.DialogResult.Yes)
                {
                    //UASUtility.doLogout(app);
                    //closeAllOpenForm();
                    //closeUasMainForm();
                    //Refresh();
                    string userid = app.LoginUserID;
                    app.logout();
                    sTipMsg = languageDictionary?["TIPMSG_LOGOUT_SUCCEED"]?.ToString() ?? "Logout Succeed.";
                    TipMessage_Type_Light_woBtn.Show("", sTipMsg, BCAppConstants.INFO_MSG);

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void PASSWORD_CHANGE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new PasswordChangePopupWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ACCOUNT_MANAGEMENT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AccountManagementPopupWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void SYSTEM_MODE_CONTROL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SystemModeControlPopupWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void AOVMAPCONTROL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new AOVPopupWindow().ShowDialog();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unregisterEvents();
                App.Current.MainWindow.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        #endregion Click System

        #region Click Operation
        private void OPERATION_FUNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainLayout.Maintenance_Base.Close();
                MainLayout.Log_Base.Close();
                if(app.ObjCacheManager.ViewerSettings.menuItem_Report.Display)MainLayout.Report_Base.Close();
                MainLayout.Operation_Base.Show(app, ((MenuItem)sender).Header?.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        #endregion Click Operation

        #region Click Maintenance
        private void MAINTENANCE_FUNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainLayout.Operation_Base.Close();
                MainLayout.Log_Base.Close();
                if (app.ObjCacheManager.ViewerSettings.menuItem_Report.Display) MainLayout.Report_Base.Close();
                MainLayout.Maintenance_Base.Show(app, ((MenuItem)sender).Header?.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }   
        #endregion Click Maintenance

        #region Click Log
        private void LOG_HISTORY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainLayout.Operation_Base.Close();
                MainLayout.Maintenance_Base.Close();
                if (app.ObjCacheManager.ViewerSettings.menuItem_Report.Display) MainLayout.Report_Base.Close();
                MainLayout.Log_Base.Show(app, ((MenuItem)sender).Header?.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        #endregion Click Log

        #region Click Report
        private void REPORT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainLayout.Operation_Base.Close();
                MainLayout.Maintenance_Base.Close();
                MainLayout.Log_Base.Close();
                MainLayout.Report_Base.Show(app, ((MenuItem)sender).Header?.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
        #endregion Click Report

        #region Click Help
        private void ABOUT_Click(object sender, RoutedEventArgs e)
        {
            new AboutPopupWindow().ShowDialog();
        }
        private void VEHICLE_COLOR_INFO_Click(object sender, RoutedEventArgs e)
        {
            new VehicleColorInfoWindow().ShowDialog();
        }

        private void SPEC_LINK_Click(object sender, RoutedEventArgs e)
        {
            string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)) + "\\Doc\\";
            string sKeyword = "*Manual*";

            UtilsAPI.Tool.OpenFile.OpenFileByPathKeyword(sPath, sKeyword);
        }
        private void TROUBLESHOOTING_Click(object sender, RoutedEventArgs e)
        {
            new ControlSystemViewer.PopupWindows.TroubleshootingPopupWindow().Show();
            //new Troubleshooting().ShowDialog();
        }
        #endregion Click Help

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sType = ((MenuItem)((MenuItem)sender)?.Parent)?.Header?.ToString();
                string sBtn = ((MenuItem)sender)?.Header?.ToString();
                if (sType == null || sBtn == null) return;

                var dictionaries = App.Current.Resources.MergedDictionaries[0];
                if (sType == dictionaries["SYSTEM"]?.ToString())
                {
                    if (sBtn == dictionaries["LOG_IN"]?.ToString())
                    {

                    }
                    else if (sBtn == dictionaries["CLOSE"]?.ToString())
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void TIP_MESSAGE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tipMessagePopupWindow == null)
                {
                    tipMessagePopupWindow = new TipMessagePopupWindow();
                    tipMessagePopupWindow.CloseEvent += TipMessagePopupWindow_CloseEvent;
                    tipMessagePopupWindow.Show();
                }
                else
                {
                    tipMessagePopupWindow.BringToFront();
                }
                ObjCacheManager_TipMessagesChange(null, app?.ObjCacheManager?.TipMessages);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void TipMessagePopupWindow_CloseEvent(object sender, EventArgs e)
        {
            try
            {
                tipMessagePopupWindow.CloseEvent -= TipMessagePopupWindow_CloseEvent;
                tipMessagePopupWindow = null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void DynamicMenu_Item_Set()
        {
            bool visible;

            //System
            visible = menuItem_System?.Visible_SystemModeControl ?? true;
            MenuItem_SystemModeControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            //Operation
            visible = menuItem_Operation?.Visible_SystemModeControl ?? false;
            MenuItem_SystemModeControl_OPERATION.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            visible = menuItem_Operation?.Visible_PortManagement ?? false;
            MenuItem_PortManagement.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            //Maintenance
            visible = menuItem_Maintenance?.Visible_EquipmentConstant ?? false;
            MenuItem_MAINTENANCE_EQUIPMENT_CONSTANT.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            visible = menuItem_Maintenance?.Visible_ParkZoneManagement ?? false;
            MenuItem_MAINTENANCE_PARKINGZONE_MANAGEMENT.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            //Log
            visible = menuItem_Log?.Visible_STATISTICS ?? false;
            MenuItem_STATISTICS.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            //Repott
            visible = app.ObjCacheManager.ViewerSettings.menuItem_Report.Display;
            MenuReport.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            //visible = app.ObjCacheManager.ViewerSettings.InfluxDBsetting.Token_VehicleInfo != "";
            //MenuItem_LongCharging.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            MenuItem_LongCharging.Visibility = Visibility.Visible;
            MenuItem_UtilizationRateByStatus.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            MenuItem_StabilityByStatus.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            visible = WindownApplication.getInstance().ObjCacheManager.GetSelectedProject().Customer.ToString().Contains("M4");
            MenuItem_RealExecuteTime.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            MenuItem_HIDinfohistory.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
