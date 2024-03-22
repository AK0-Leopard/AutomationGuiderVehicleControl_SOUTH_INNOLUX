using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc.Common;
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

namespace ControlSystemViewer.Views.Menu_System
{
    /// <summary>
    /// PasswordChange.xaml 的互動邏輯
    /// </summary>
    public partial class PasswordChange : UserControl
    {
        #region 公用參數設定
        public WindownApplication app { get; private set; } = null;
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        public event EventHandler CloseEvent;
        #endregion 公用參數設定

        public PasswordChange()
        {
            InitializeComponent();
        }

        private void _Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                app = app ?? WindownApplication.getInstance();
                //系統無USER登入時，不允許開啟密碼變更介面
                if (string.IsNullOrEmpty(app.LoginUserID))
                {
                    CloseEvent?.Invoke(this, null);
                    return;
                }
                txt_UserID.Text = app.LoginUserID; //顯示當前登入者ID
                old_password_box.Focus(); //將游標指定在密碼位置
                System.Windows.Input.InputMethod.SetIsInputMethodEnabled(txt_UserID, false); //設置IME和輸入是否可以是中文

                registerEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void registerEvent()
        {
            try
            {
                app.ObjCacheManager.LogInUserChanged += ObjCacheManager_LogInUserChanged;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void unregisterEvent()
        {
            try
            {
                app.ObjCacheManager.LogInUserChanged -= ObjCacheManager_LogInUserChanged;
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
                app = app ?? WindownApplication.getInstance();
                //USER登出後，關閉密碼變更介面
                if (string.IsNullOrEmpty(app.LoginUserID))
                {
                    CloseEvent?.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender.Equals(btn_Change))
                {
                    string userID = txt_UserID.Text;
                    string old_password = old_password_box.Password;
                    string new_password = new_password_box.Password;
                    string verify_new_password = verify_password_box.Password;
                    if (string.IsNullOrWhiteSpace(old_password) || string.IsNullOrWhiteSpace(new_password) || string.IsNullOrWhiteSpace(verify_new_password))
                    {
                        TipMessage_Type_Light.Show("Failure", "Please fill in the password.", BCAppConstants.INFO_MSG);
                        if (string.IsNullOrWhiteSpace(old_password)) old_password_box.Focus();
                        else if(string.IsNullOrWhiteSpace(old_password)) new_password_box.Focus();
                        else if(string.IsNullOrWhiteSpace(old_password)) verify_password_box.Focus();
                        return;
                    }
                    else if (new_password != verify_new_password)
                    {
                        TipMessage_Type_Light.Show("Failure", "Please make sure two new password are same.", BCAppConstants.INFO_MSG);
                        new_password_box.Focus();
                        return;
                    }
                    else
                    {
                        bool isSuccess = false;
                        string result = string.Empty;
                        await Task.Run(() => isSuccess = app.LineBLL.SendPasswordChange(userID, old_password, new_password, verify_new_password, out result));
                        if (isSuccess)
                        {
                            //變更成功
                            TipMessage_Type_Light_woBtn.Show("", "Update Successful.", BCAppConstants.INFO_MSG);
                            CloseEvent?.Invoke(this, null);
                        }
                        else
                        {
                            //變更失敗
                            TipMessage_Type_Light.Show("", result, BCAppConstants.WARN_MSG);
                            old_password_box.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }
    }
}
