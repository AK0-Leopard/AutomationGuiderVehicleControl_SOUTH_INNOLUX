//*********************************************************************************
//      LogIn.cs
//*********************************************************************************
// File Name: LogIn.cs
// Description: 
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
// 2022/06/02    Steven Hong    N/A            A0.01   修正用Badge No登入功能錯誤
//**********************************************************************************

using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
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
using System.Windows.Threading;

namespace ControlSystemViewer.Views.Menu_System
{
    /// <summary>
    /// LogIn.xaml 的互動邏輯
    /// </summary>
    public partial class LogIn : UserControl
    {
        #region 公用參數設定
        public event EventHandler CloseEvent;
        WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 公用參數設定
        public LogIn()
        {
            InitializeComponent();
        }

        private void _Loaded(object sender, RoutedEventArgs e)
        {
            start();
        }

        private void start()
        {
            app = app ?? WindownApplication.getInstance();

            txt_UserID.Text = "";
            password_box.Password = "";

            //將游標指定在UserID位置
            txt_UserID.Focus();
            InputMethod.SetIsInputMethodEnabled(txt_UserID, false); //設置IME和輸入是否可以是中文

            //將游標指定在UserID位置
            //txt_UserID.Focus(); 
            //this.Dispatcher.BeginInvoke((Action)delegate
            //{
            //    Keyboard.Focus(txt_UserID);
            //}, DispatcherPriority.Render);
        }

        private async void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userID = txt_UserID.Text;
                string password = password_box.Password;
                bool isSuccess = false;
                string result = string.Empty; 
                await Task.Run(() => isSuccess = app.LineBLL.SendLogInRequest(userID, password, out result));
                if (isSuccess)
                {
                    CloseEvent?.Invoke(this, e);
                    TipMessage_Type_Light_woBtn.Show("", "Login Successful.", BCAppConstants.INFO_MSG);
                    app.login(userID);
                }
                else
                {
                    TipMessage_Type_Light.Show("", "Login Fail.", BCAppConstants.WARN_MSG);
                    password_box.Focus();
                }
            }
            catch (Exception ex) { logger.Error(ex, "Exception"); }
        }

        private StringBuilder badgeNo = new StringBuilder();
        private DateTime preInputTime = DateTime.Now;
        int BadgeDefaultLength = 10;

        private async void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                bool isSuccess = false;
                string result = string.Empty;
                if (e.Key == Key.Return)
                {
                    int timeDiff = (int)DateTime.Now.Subtract(preInputTime).TotalMilliseconds;
                    if (timeDiff > 100)
                        badgeNo.Clear();
                    //badgeNo = new StringBuilder("123123123123", 12);
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

                        await Task.Run(() => isSuccess = app.LineBLL.SendLoginByBadge(sBadgeCode, out result));
                        if (isSuccess)
                        {
                            CloseEvent?.Invoke(this, e);
                            TipMessage_Type_Light_woBtn.Show("", "Login Successful.", BCAppConstants.INFO_MSG);

                            //A0.01 Start
                            string user_id = string.Empty;
                            await Task.Run(() => isSuccess = app.LineBLL.SendBadgeGetUserID(sBadgeCode, out user_id));
                            if(isSuccess)
                            {
                                app.login(user_id);
                            }
                            //app.login(sBadgeCode);
                            //A0.01 End
                        }
                        else
                        {
                            TipMessage_Type_Light.Show("", "Login Fail.", BCAppConstants.WARN_MSG);
                            password_box.Focus();
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
                            case Key.D0:
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
