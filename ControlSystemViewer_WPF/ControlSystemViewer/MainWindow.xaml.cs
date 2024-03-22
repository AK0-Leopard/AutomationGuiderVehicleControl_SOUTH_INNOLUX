using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
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
using NLog;
using MirleGO_UIFrameWork.UI.uc_Button;
using System.Windows.Forms;
using com.mirle.ibg3k0.Utility.uc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using ViewerObject;

namespace ControlSystemViewer
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 公用參數設定
        public WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定

        public MainWindow()
        {
             InitializeComponent();

            Adapter.Initialize();

            //string sPath = Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location));
            //this.Icon = UtilsAPI.Tool.ImageSetting.ToBitmapImage(new Bitmap(sPath + "\\Resources\\SystemIcon\\system.png"));
            this.Loaded += Window_Loaded;
            this.Closed += Window_Closed;
        }

        public void Loading_Start(string txt = null)
        {
            Adapter.Invoke((obj) =>
            {
                Loading.Start(txt);
            }, null);
        }

        public void Loading_Stop()
        {
            Adapter.Invoke((obj) =>
            {
                Loading.Stop();
            }, null);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isSucceed = false;
                try
                {
                    await Task.Delay(1000);
                    await Task.Run(() => app = WindownApplication.getInstance());
                    isSucceed = app != null;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception");
                }
                finally
                {
                    Loading.Stop();
                }
                if (isSucceed && !app.IsControlSystemConnentable)
                {
                    //調整: 當開啟失敗原因為無法連上C，仍可開啟Viewer使用報表功能
                    string tipMsg = "Control System Unconnentable!\n\nYes: Keep Viewer running for limited functions.\nNo: Restart after Control System connentable.";
                    if (TipMessage_Request_Light.Show(tipMsg) == System.Windows.Forms.DialogResult.No)
                    {
                        isSucceed = false;
                    }
                }
                if (isSucceed)
                {
                    this.Closing += Window_Closing;
                    try
                    {
                        MainView.Start(app);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Exception");
                    }
                }
                else //if (!isSucceed)
                {
                    app = null;
                    TipMessage_Type_Light.Show("System Closing", "Please check LogFiles for further information.\nC:\\LogFiles\\ControlSystemViewer\\*DATE\\", BCAppConstants.ERROR_MSG);
                    //MessageBox.Show("Please check Main Control System is opened and driver is started, \nthen try again.", "System Closing", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //DialogResult dialogResult = TipMessage_Type_Light.Show("System Closing", "Please check Main Control System is opened and driver is started, then try again.", BCAppConstants.ERROR_MSG);
                    App.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                App.Current.Shutdown();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var languageDictionary = App.Current.Resources.MergedDictionaries?[0];
                string sTipMsg = languageDictionary?["TIPMSG_CLOSE_SYSTEM_CONFIRM"]?.ToString() ?? "Are you sure to close the system now?";
                var confirmResult = TipMessage_Request_Light.Show(sTipMsg);
                if (confirmResult != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;    //取消關閉
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                app?.LineBLL.SendMCSCommandAutoAssignChange(true.ToString(), out string result);

                //app?.GetNatsManager()?.close();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Exception");
            }
            finally
            {
                Environment.Exit(Environment.ExitCode);
            }
        }

        private StringBuilder badgeNo = new StringBuilder();
        private DateTime preInputTime = DateTime.Now;
        int BadgeDefaultLength = 10;

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    int timeDiff = (int)DateTime.Now.Subtract(preInputTime).TotalMilliseconds;
                    if (timeDiff > 100)
                        badgeNo.Clear();
                    string sBadgeCode = badgeNo.ToString();
                    if (!string.IsNullOrWhiteSpace(sBadgeCode))
                    {
                        sBadgeCode = sBadgeCode.Trim();
                        if (sBadgeCode.Length < BadgeDefaultLength)
                        {
                            //BCUtility.showMsgBox_Warn(this, BCApplication.getMessageString("NO_AUTHORITY"));
                            logger.Warn("BadgeCode:[{0}] less than BadgeDefaultLength:[{1}]", sBadgeCode, BadgeDefaultLength);
                            return;
                        }
                        //BCUtility.doLoginByBadge(this, bcApp, sBadgeCode.Trim());
                    }
                    badgeNo.Clear();
                    this.Focus();
                }
                else
                {
                    if (e.Key >= Key.D0 && e.Key <= Key.D9)
                    {
                        int timeDiff = (int)DateTime.Now.Subtract(preInputTime).TotalMilliseconds;
                        if (timeDiff > 100)
                            badgeNo.Clear();
                        switch (e.Key)
                        {
                            case Key.D0 :
                                badgeNo.Append(0);
                                break;
                            case Key.D1:
                                badgeNo.Append(1);
                                break;
                            case Key.D2:
                                badgeNo.Append(2);
                                break;
                            case Key.D3:
                                badgeNo.Append(3);
                                break;
                            case Key.D4:
                                badgeNo.Append(4);
                                break;
                            case Key.D5:
                                badgeNo.Append(5);
                                break;
                            case Key.D6:
                                badgeNo.Append(6);
                                break;
                            case Key.D7:
                                badgeNo.Append(7);
                                break;
                            case Key.D8:
                                badgeNo.Append(8);
                                break;
                            case Key.D9:
                                badgeNo.Append(9);
                                break;
                        }
                    }
                    else
                        badgeNo.Clear();
                }
                preInputTime = DateTime.Now;
            }
            catch (Exception ex)
            {
            }
        }
        
    }
}
