using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.Utility.uc;
using MirleGO_UIFrameWork.UI.uc_Button;
using NLog;
using System;
using System.Collections.Generic;
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
using UtilsAPI.Tool;
using ViewerObject;

namespace ControlSystemViewer.Views.Menu_System
{
    /// <summary>
    /// AccountManagement.xaml 的互動邏輯
    /// </summary>
    public partial class AccountManagement : UserControl
    {
        #region 公用參數設定
        private WindownApplication app = null;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public event EventHandler CloseEvent;

        List<FieldInfo> System_Function = typeof(UAS_Def.System_Function).GetFields().ToList();
        List<FieldInfo> Operation_Function = typeof(UAS_Def.Operation_Function).GetFields().ToList();
        List<FieldInfo> Maintenance_Function = typeof(UAS_Def.Maintenance_Function).GetFields().ToList();

        //Operation Log
        Dictionary<string, string> New_Del_Data = new Dictionary<string, string>();
        Dictionary<string, string> OldData = new Dictionary<string, string>();

        //List<FieldInfo> Debug_Function = typeof(UAS_Def.Debug_Function).GetFields().ToList();
        #endregion 公用參數設定

        public AccountManagement()
        {
            InitializeComponent();

            start();
        }

        private void start()
        {
            try
            {
                app = WindownApplication.getInstance();
                registerEvent();
                //this.users = users;
                grid_UserAcc.ItemsSource = app.ObjCacheManager.GetUsers();
                var user_grps = app.ObjCacheManager.GetUserGroups();
                grid_UserGroup.ItemsSource = user_grps;
                //UA_Group.ItemsSource = app.ObjCacheManager.GetUserGroups();
                UA_Group.Items.Clear();
                //GA_Group.Items.Clear();

                foreach (var ugrp in user_grps)
                {
                    UA_Group.Items.Add(ugrp.USER_GRP);
                    //GA_Group.Items.Add(ugrp.USER_GRP);
                }

                refresh_grid_UserAcc();

                refresh_UserGrp();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void registerEvent()
        {
            try
            {
                app.ObjCacheManager.UasChange += ObjCacheManager_UasChange;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void unregisterEvent()
        {
            try
            {
                app.ObjCacheManager.UasChange -= ObjCacheManager_UasChange;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void ObjCacheManager_UasChange(object sender, EventArgs e)
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    refresh_grid_UserAcc();
                    refresh_UserGrp();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void refresh_grid_UserAcc()
        {
            try
            {
                grid_UserAcc.ItemsSource = app.ObjCacheManager.GetUsers();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void refresh_UserGrp()
        {
            try
            {
                var user_grps = app.ObjCacheManager.GetUserGroups();
                grid_UserGroup.ItemsSource = user_grps;
                UA_Group.SelectedIndex = -1;
                UA_Group.Items.Clear();
                foreach (var ugrp in user_grps)
                {
                    UA_Group.Items.Add(ugrp.USER_GRP);
                    //GA_Group.Items.Add(ugrp.USER_GRP);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void _Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UA_UserID.Focus(); //將游標指定在userID位置

                var FunctionCodeList = app.ObjCacheManager.GetFunctionCodes();

                //updateUserGroupTreeView(userFuncList);

                tV_Permission.ItemsSource = TreeViewModel.SetTree("Select All", FunctionCodeList);
                tV_Permission.Height = 445;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void TabItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TabItemPreviewMouseLeftButtonUp(sender, e);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void TabItemPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender.Equals(TabItem_UA))
                {
                    UserAccountInfo.Visibility = Visibility;
                    GroupAccountInfo.Visibility = Visibility.Collapsed;
                }
                else if (sender.Equals(TabItem_GA))
                {
                    UserAccountInfo.Visibility = Visibility.Collapsed;
                    GroupAccountInfo.Visibility = Visibility;
                    //registerFunction();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        public void Refresh(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender.Equals(TabItem_UA))
                {
                    clearTextBox();
                    refresh_grid_UserAcc();
                }
                else if (sender.Equals(TabItem_GA))
                {
                    clearTextBox();
                    refresh_UserGrp();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void clearTextBox()
        {
            try
            {
                UA_UserID.Clear();
                UA_Password.Clear();
                UA_ConfrimPassword.Clear();
                UA_UserName.Clear();
                UA_Department.Clear();
                UA_BadgeNumber.Clear();
                UA_Group.SelectedIndex = -1;
                UA_AccountActivation_radbtn_Yes.IsChecked = true;
                UA_AccountActivation_radbtn_No.IsChecked = false;
                GA_Group.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void clearUserGroupTreeview()
        {
            try
            {
                TreeViewModel root1 = (TreeViewModel)tV_Permission.Items[0];

                for (int i = 0; i < root1.Children.Count; i++)
                {
                    root1.Children[i].IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private VUASUSRGRP getSelectedRowToTextBox_UserGroup()
        {
            //A0.01 Start
            var selectUserGroup = new VUASUSRGRP();

            try
            {
                if (grid_UserGroup.SelectedItems == null)
                    return null;
                if (grid_UserGroup.SelectedItems.Count < 1)
                    return null;

                selectUserGroup.USER_GRP = grid_UserGroup.SelectedItems[0].ToString();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return selectUserGroup;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null) btn.IsEnabled = false;
            try
            {
                if (sender.Equals(btn_Clear))
                {
                    clearTextBox();

                    clearUserGroupTreeview();
                }
                else if (sender.Equals(btn_Add))
                {
                    if (TabItem_UA.IsSelected)
                    {
                        userAccountManagement(AccountManagementAction.Add);
                    }
                    else if (TabItem_GA.IsSelected)
                    {
                        groupAccountManagement(AccountManagementAction.Add);
                    }
                }
                else if (sender.Equals(btn_Modify))
                {
                    if (TabItem_UA.IsSelected)
                    {
                        userAccountManagement(AccountManagementAction.Mod);
                    }
                    else if (TabItem_GA.IsSelected)
                    {
                        groupAccountManagement(AccountManagementAction.Mod);
                    }
                }
                else if (sender.Equals(btn_Delete))
                {
                    if (TabItem_UA.IsSelected)
                    {
                        userAccountManagement(AccountManagementAction.Delete);
                    }
                    else if (TabItem_GA.IsSelected)
                    {
                        groupAccountManagement(AccountManagementAction.Delete);
                    }
                }
                else if (sender.Equals(btn_Close))
                {
                    CloseEvent?.Invoke(this, e);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            finally
            {
                if (btn != null) btn.IsEnabled = true;
            }
        }

        private string getTreeviewItemsSetting(TreeViewModel root, int index1, int index2)
        {
            string result = string.Empty;
            try
            {
                if ((bool)root.Children[index1].Children[index2].IsChecked)
                {
                    result = true.ToString();
                    return result;
                }
                else
                {
                    result = false.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return result;
        }

        private async void userAccountManagement(AccountManagementAction action)
        {
            try
            {
                string userID = UA_UserID.Text?.Trim();
                string password = UA_Password.Password;
                string password_v = UA_ConfrimPassword.Password;
                //string group_id =UA_Group
                string group_id = UA_Group.SelectedValue?.ToString()?.Trim() ?? "";
                string disable_flag = (bool)UA_AccountActivation_radbtn_No.IsChecked ? "Y" : "N";

                string user_name = UA_UserName.Text;
                string department = UA_Department.Text;
                string badge_num = UA_BadgeNumber.Text;
                //string group_id  =.Text;

                if (string.IsNullOrEmpty(userID))
                {
                    TipMessage_Type_Light.Show("Failure", "UserID must not be null or empty.", BCAppConstants.INFO_MSG);
                    UA_UserID.Focus();
                    return;
                }

                if (action == AccountManagementAction.Add || action == AccountManagementAction.Mod)
                {
                    if (password != password_v)
                    {
                        TipMessage_Type_Light.Show("Failure", "Please make sure two password are same.", BCAppConstants.INFO_MSG);
                        UA_Password.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(password_v))
                    {
                        TipMessage_Type_Light.Show("Failure", "Please fill in the password.", BCAppConstants.INFO_MSG);
                        if (string.IsNullOrWhiteSpace(password)) UA_Password.Focus();
                        else if (string.IsNullOrWhiteSpace(password_v)) UA_ConfrimPassword.Focus();
                        return;
                    }
                }

                bool isSuccess = false;
                string result = string.Empty;
                string msg = string.Empty;
                switch (action)
                {
                    case AccountManagementAction.Add:
                        msg = "Added ";
                        if (grid_UserAcc.ItemsSource != null)
                        {
                            var userAccs = grid_UserAcc.ItemsSource as List<VUASUSR>;
                            if (userAccs != null && userAccs.Count > 0)
                            {
                                bool isAlreadyExist = userAccs.Where(u => u.USER_ID?.Trim() == userID).Any();
                                if (isAlreadyExist)
                                {
                                    TipMessage_Type_Light.Show("Failure", $"User {userID} already exists.", BCAppConstants.INFO_MSG);
                                    UA_Group.Focus();
                                    return;
                                }
                            }
                        }
                        await Task.Run(() => isSuccess =
                            app.LineBLL.SendUserAccountAddRequest(
                                userID,
                                password,
                                user_name,
                                disable_flag,
                                group_id,
                                badge_num,
                                department,
                                out result));
                        break;
                      
                    case AccountManagementAction.Mod:
                        if (userID == "ADMIN" && group_id != "ADMIN")
                        {
                            TipMessage_Type_Light.Show("Failure", "Can not change ADMIN's group.", BCAppConstants.INFO_MSG);
                            UA_Group.Focus();
                            return;
                        }
                        msg = "Updated ";
                        await Task.Run(() => isSuccess =
                            app.LineBLL.SendUserAccountUpdateRequest(
                               userID,
                                password,
                                user_name,
                                disable_flag,
                                group_id,
                                badge_num,
                                department,
                                out result));
                        break;
                    case AccountManagementAction.Delete:
                        if (userID == "ADMIN")
                        {
                            TipMessage_Type_Light.Show("Failure", "Can not delete ADMIN.", BCAppConstants.INFO_MSG);
                            return;
                        }
                        else if (userID == app.LoginUserID)
                        {
                            TipMessage_Type_Light.Show("Failure", "Can not delete current Login account.", BCAppConstants.INFO_MSG);
                            return;
                        }
                        msg = "Deleted ";
                        await Task.Run(() => isSuccess =
                            app.LineBLL.SendUserAccountDeleteRequest(userID, out result));
                        break;
                }

                if (isSuccess)
                {
                    TipMessage_Type_Light.Show("Succeed", $"{msg} User {userID}.", BCAppConstants.INFO_MSG);
                    Refresh(TabItem_UA, null);
                }
                else
                {
                    TipMessage_Type_Light.Show("Failure", $"Result: {result}.", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private async void groupAccountManagement(AccountManagementAction action)
        {
            try
            {
                string group_id = GA_Group.Text?.Trim();

                if (string.IsNullOrWhiteSpace(group_id))
                {
                    TipMessage_Type_Light.Show("Failure", "Group could not be empty.", BCAppConstants.INFO_MSG);
                    GA_Group.Focus();
                    return;
                }
                Dictionary<string, bool> fun_enable = new Dictionary<string, bool>();
                TreeViewModel root = (TreeViewModel)tV_Permission.Items[0];

                fun_enable.Add(group_id, false);
                System_Function.ForEach(x =>
                {
                    bool check = root.Children.Find(y => y.Name == nameof(System_Function))
                        .Children.Find(y => y.Name == (string)x.GetValue(null))?.IsChecked ?? false;
                    fun_enable.Add(x.Name, check);
                });
                Operation_Function.ForEach(x =>
                {
                    bool check = root.Children.Find(y => y.Name == nameof(Operation_Function))
                        .Children.Find(y => y.Name == (string)x.GetValue(null))?.IsChecked ?? false;
                    fun_enable.Add(x.Name, check);
                });
                Maintenance_Function.ForEach(x =>
                {
                    bool check = root.Children.Find(y => y.Name == nameof(Maintenance_Function))
                        .Children.Find(y => y.Name == (string)x.GetValue(null))?.IsChecked ?? false;
                    fun_enable.Add(x.Name, check);
                });

                if (action == AccountManagementAction.Add || action == AccountManagementAction.Mod)
                {
                    if (fun_enable == null || fun_enable.Count == 0)
                    {
                        TipMessage_Type_Light.Show("Failure", "GroupSettings must not be null or empty.", BCAppConstants.INFO_MSG);
                        return;
                    }
                }

                bool isSuccess = false;
                string result = string.Empty;
                string msg = string.Empty;
                switch (action)
                {
                    case AccountManagementAction.Add:
                        if (grid_UserGroup.ItemsSource != null)
                        {
                            var userGrps = grid_UserGroup.ItemsSource as List<VUASUSRGRP>;
                            if (userGrps != null && userGrps.Count > 0)
                            {
                                bool isAlreadyExist = userGrps.Where(u => u.USER_GRP?.Trim() == group_id).Any();
                                if (isAlreadyExist)
                                {
                                    TipMessage_Type_Light.Show("Failure", $"Group {group_id} already exists.", BCAppConstants.INFO_MSG);
                                    UA_Group.Focus();
                                    return;
                                }
                            }
                        }
                        msg = "Added ";
                        await Task.Run(() => isSuccess = app.LineBLL.SendUserGroupAddRequest(fun_enable, out result));
                        break;
                    case AccountManagementAction.Mod:
                        bool isEnable_FUNC_USER_MANAGEMENT = false;
                        if (group_id == "ADMIN" &&
                            fun_enable.TryGetValue(UAS_Def.System_Function.FUNC_USER_MANAGEMENT, out isEnable_FUNC_USER_MANAGEMENT) && !isEnable_FUNC_USER_MANAGEMENT)
                        {
                            TipMessage_Type_Light.Show("Failure", "Can not remove User Manage Func from ADMIN.", BCAppConstants.INFO_MSG);
                            return;
                        }
                        msg = "Updated ";
                        await Task.Run(() => isSuccess = app.LineBLL.SendUserGroupUpdateRequest(fun_enable, out result));
                        break;
                    case AccountManagementAction.Delete:
                        msg = "Deleted ";
                        if (group_id == "ADMIN")
                        {
                            TipMessage_Type_Light.Show("Failure", "Can not delete ADMIN.", BCAppConstants.INFO_MSG);
                            return;
                        }
                        await Task.Run(() => isSuccess = app.LineBLL.SendUserGroupDeleteRequest(group_id, out result));
                        if (isSuccess) clearUserGroupTreeview();
                        break;
                }

                if (isSuccess)
                {
                    TipMessage_Type_Light.Show("Succeed", $"{msg} Group {group_id}.", BCAppConstants.INFO_MSG);
                    Refresh(TabItem_UA, null);
                }
                else
                {
                    TipMessage_Type_Light.Show("Failure", $"Result: {result}.", BCAppConstants.INFO_MSG);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void grid_UserAcc_cell_click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var userAccData = (VUASUSR)grid_UserAcc.SelectedItem;
                if (userAccData == null) return;

                UA_UserID.Text = userAccData.USER_ID ?? string.Empty;
                UA_Password.Password = userAccData.PASSWD ?? string.Empty;
                UA_ConfrimPassword.Password = userAccData.PASSWD ?? string.Empty;
                UA_UserName.Text = userAccData.USER_NAME ?? string.Empty;
                UA_Group.Text = userAccData.USER_GRP ?? string.Empty;
                UA_BadgeNumber.Text = userAccData.BADGE_NUMBER ?? string.Empty;

                if (!userAccData.IS_ACTIVE)
                {
                    UA_AccountActivation_radbtn_Yes.IsChecked = false;
                    UA_AccountActivation_radbtn_No.IsChecked = true;
                }
                else
                {
                    UA_AccountActivation_radbtn_Yes.IsChecked = true;
                    UA_AccountActivation_radbtn_No.IsChecked = false;
                }

                UA_Department.Text = userAccData.DEPARTMENT ?? string.Empty;

                #region OperationLog
                OldData.Clear();
                OldData.Add("UserID", UA_UserID.Text);
                OldData.Add("UserName", UA_UserName.Text);
                OldData.Add("GroupID", UA_Group.Text);
                OldData.Add("BadgeNum", UA_BadgeNumber.Text);
                OldData.Add("Department", UA_Department.Text);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void grid_UserGroup_cell_click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var usergroupData = (VUASUSRGRP)grid_UserGroup.SelectedItem;
                if (usergroupData == null) return;

                GA_Group.Text = usergroupData.USER_GRP ?? string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void UserGroup_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                VUASUSRGRP selectUserGroup = getGroupIDFromGroupList();

                if (selectUserGroup == null) return;

                var userFuncList = app.ObjCacheManager.GetUserFuncs(selectUserGroup.USER_GRP);

                clearUserGroupTreeview();

                foreach (var uasufnc in userFuncList)
                {
                    string System_value = (string)System_Function.Where(x => x.Name == uasufnc.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;
                    string Operation_value = (string)Operation_Function.Where(x => x.Name == uasufnc.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;
                    string Maintenance_value = (string)Maintenance_Function.Where(x => x.Name == uasufnc.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;
                    //string Debug_value = (string)Debug_Function.Where(x => x.Name == uasufnc.FUNC_CODE.Trim()).FirstOrDefault()?.GetValue(null) ?? null;

                    if (!string.IsNullOrEmpty(System_value))
                    {
                        TreeViewModel root = (TreeViewModel)tV_Permission.Items[0];
                        root = (TreeViewModel)tV_Permission.Items[0];
                        root.Children.Find(x => x.Name == "System_Function")
                            .Children.Find(x => x.Name == System_value).IsChecked = true;
                    }
                    else if (!string.IsNullOrEmpty(Operation_value))
                    {
                        TreeViewModel root = (TreeViewModel)tV_Permission.Items[0];
                        root = (TreeViewModel)tV_Permission.Items[0];
                        root.Children.Find(x => x.Name == "Operation_Function")
                            .Children.Find(x => x.Name == Operation_value).IsChecked = true;
                    }
                    else if (!string.IsNullOrEmpty(Maintenance_value))
                    {
                        TreeViewModel root = (TreeViewModel)tV_Permission.Items[0];
                        root = (TreeViewModel)tV_Permission.Items[0];
                        root.Children.Find(x => x.Name == "Maintenance_Function")
                            .Children.Find(x => x.Name == Maintenance_value).IsChecked = true;
                    }
                    //else if (!string.IsNullOrEmpty(Debug_value))
                    //{
                    //    TreeViewModel root = (TreeViewModel)tV_Permission.Items[0];
                    //    root = (TreeViewModel)tV_Permission.Items[0];
                    //    root.Children.Find(x => x.Name == "Debug_Function")
                    //        .Children.Find(x => x.Name == Debug_value).IsChecked = true;
                    //}


                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.System_Function.FUNC_USER_MANAGEMENT))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(0, 0);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.System_Function.FUNC_CLOSE_SYSTEM))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(0, 1);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.System_Function.FUNC_LOGIN))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(0, 2);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Operation_Function.FUNC_SYSTEM_CONCROL_MODE))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(1, 0);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Operation_Function.FUNC_TRANSFER_MANAGEMENT))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(1, 1);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Maintenance_Function.FUNC_ADVANCED_SETTINGS))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(2, 0);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Maintenance_Function.FUNC_MTS_MTL_MAINTENANCE))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(2, 1);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Maintenance_Function.FUNC_PORT_MAINTENANCE))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(2, 2);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Maintenance_Function.FUNC_VEHICLE_MANAGEMENT))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(2, 3);
                    //}
                    //if (SCUtility.isMatche(uasufnc.FUNC_CODE.Trim(), BCAppConstants.Debug_Function.FUNC_DEBUG))
                    //{
                    //    getEachTreeViewItemCheckboxSetting(3, 0);
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void getEachTreeViewItemCheckboxSetting(int index1, int index2)
        {
            try
            {
                TreeViewModel root = (TreeViewModel)tV_Permission.Items[0];
                root = (TreeViewModel)tV_Permission.Items[0];
                root.Children[index1].Children[index2].IsChecked = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private VUASUSRGRP getGroupIDFromGroupList()
        {
            //A0.01 Start
            VUASUSRGRP selectUserGroup = new VUASUSRGRP();

            try
            {
                if (grid_UserGroup.SelectedItem == null)
                    return null;
                if (grid_UserGroup.Items.Count < 1)
                    return null;

                selectUserGroup.USER_GRP = (grid_UserGroup.Columns[0].GetCellContent(grid_UserGroup.Items[grid_UserGroup.SelectedIndex]) as TextBlock).Text.ToString();

                //int selectedRowCnt = UserGroupGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
                //if (selectedRowCnt <= 0) 
                //{
                //    return null;
                //}
                //int selectedIndex = UserGroupGridView.SelectedRows[0].Index;
                //if (userGroupList.Count <= selectedIndex) 
                //{
                //    return null;
                //}
                //UserGroup selectUserGroup = userGroupList[selectedIndex];
                //A0.01 End
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
            return selectUserGroup;
        }
    }

    public enum AccountManagementAction
    {
        Add = 0,
        Mod,
        Delete
    }
}
