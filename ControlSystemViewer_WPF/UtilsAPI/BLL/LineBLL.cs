//*********************************************************************************
//      LineBLL.cs
//*********************************************************************************
// File Name: LineBLL.cs
// Description: 
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
// 2022/06/02    Steven Hong    N/A            A0.01   增加根據Badge No取得User ID功能
//**********************************************************************************

using com.mirle.ibg3k0.ohxc.wpf.App;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Text;
using com.mirle.ibg3k0.ohxc.wpf.Common;
using Newtonsoft.Json;
using ViewerObject;
using NLog;
using System.Reflection;
using System.Linq;

namespace UtilsAPI.BLL
{
    public class LineBLL
    {
        WindownApplication app = null;
        public LineBLL(WindownApplication _app)
        {
            app = _app;
        }

        #region Nats
        public void SubscriberLineInfo(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {
            app.GetNatsManager().Subscriber(subject, handler, is_last: true);
            // app.GetNatsManager().Subscriber(subject, handler);
        }

        public void UnsubscriberLineInfo(string subject)
        {
            app.GetNatsManager().Unsubscribe(subject);
        }

        public void ProcLineInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putLine(handler.Message.Data);
        }

        public void ProcTcpInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putVhCommuLOG(handler.Message.Data);
        }

        public void ProcSecsInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putMcsCommuLOG(handler.Message.Data);
        }

        public void ProcSystemInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putSystemProcLOGs(handler.Message.Data);
        }

        public void OnlineCheckInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putOnlineCheckInfo(handler.Message.Data);
        }

        public void TransferInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putTransferInfo(handler.Message.Data);
        }

        public void PingCheckInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putPingCheckInfo(handler.Message.Data);
        }

        //public void MTLMTSInfo(object sender, StanMsgHandlerArgs handler)
        //{
        //    var bytes = handler.Message.Data;
        //    sc.ProtocolFormat.OHTMessage.MTL_MTS_INFO mtlmts_info = sc.BLL.LineBLL.Convert2Object_MTLMTSInfo(bytes);

        //    app.ObjCacheManager.putMTL_MTSCheckInfo(mtlmts_info);
        //}
        
        public void SubscriberTipMessageInfo(string subject, EventHandler<StanMsgHandlerArgs> handler)
        {
            app.GetNatsManager().Subscriber(subject, handler, is_last: true);
            // app.GetNatsManager().Subscriber(subject, handler);
        }

        public void ProcTipMessageInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putTipMessageInfos(handler.Message.Data);
        }

        public void ConnectioneInfo(object sender, StanMsgHandlerArgs handler)
        {
            app.ObjCacheManager.putConnectionInfo(handler.Message.Data);
        }
        #endregion Nats

        #region WebAPI
        public bool SendLinkStatusChange(bool isLinkOK, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "ConnectionInfo",
                "LinkStatusChange",
            };
            string linkstatus = isLinkOK ? "LinkOK" : "LinkFail";
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(linkstatus)}={linkstatus}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(linkstatus), linkstatus);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendHostModeChange(string hostmode, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "ConnectionInfo",
                "HostModeChange",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(hostmode)}={hostmode}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(hostmode), hostmode);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendHostModeCheckAuthorityByBadge(string badgeNo, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "ConCrolByBadge",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(badgeNo)}={badgeNo}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(badgeNo), badgeNo);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendTSCStateChange(string tscstate, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "ConnectionInfo",
                "TSCStateChange",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(tscstate)}={tscstate}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(tscstate), tscstate);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendControlIsExist(string exist, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "ConnectionInfo",
                "ThisExist",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(exist)}={exist}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST);
            return result == "OK";
        }

        public bool SendLogInRequest(string userID, string password, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "LogIn",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userID)}={userID}").Append("&");
            sb.Append($"{nameof(password)}={password}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(userID), userID);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendLoginByBadge(string badgeNo, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "LogInByBadge",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(badgeNo)}={badgeNo}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(badgeNo), badgeNo);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        //A0.01 Start
        public bool SendBadgeGetUserID(string badgeNo, out string user_id)
        {
            user_id = string.Empty;
            bool rtnCode = true;

            string[] action_targets = new string[]
            {
                "UserControl",
                "GetUserIDByBadge",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(badgeNo)}={badgeNo}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            user_id = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(badgeNo), badgeNo);
            app.SystemOperationLogBLL.addSystemOperationHis(user_id);

            if(user_id == string.Empty)
            {
                rtnCode = false;
            }

            return rtnCode;
        }
        //A0.01 ENd

        public bool SendUserAccountAddRequest(string userID, string password, string userName, string isDisable, string userGrp, string badgeNo, string department, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserAccountAdd",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userID)}={userID}").Append("&");
            sb.Append($"{nameof(password)}={password}").Append("&");
            sb.Append($"{nameof(userName)}={userName}").Append("&");
            sb.Append($"{nameof(isDisable)}={isDisable}").Append("&");
            sb.Append($"{nameof(userGrp)}={userGrp}").Append("&");
            sb.Append($"{nameof(badgeNo)}={badgeNo}").Append("&");
            sb.Append($"{nameof(department)}={department}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(userID),  userID);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(userName),  userName);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(isDisable),  isDisable);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(userGrp),  userGrp);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(badgeNo),  badgeNo);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(department),  department);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendUserAccountUpdateRequest(string userID, string password, string userName, string isDisable, string userGrp, string badgeNo, string department, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserAccountUpdate",
            };
            VUASUSR oldUser = app.UserBLL.GetUser(userID);
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userID)}={userID}").Append("&");
            sb.Append($"{nameof(password)}={password}").Append("&");
            sb.Append($"{nameof(userName)}={userName}").Append("&");
            sb.Append($"{nameof(isDisable)}={isDisable}").Append("&");
            sb.Append($"{nameof(userGrp)}={userGrp}").Append("&");
            sb.Append($"{nameof(badgeNo)}={badgeNo}").Append("&");
            sb.Append($"{nameof(department)}={department}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            if(oldUser.USER_ID != null)
            {
                app.SystemOperationLogBLL.addData_KeyValue("UserID", oldUser.USER_ID, userID);
                app.SystemOperationLogBLL.addData_KeyValue("UserName", oldUser.USER_NAME, userName);
                app.SystemOperationLogBLL.addData_KeyValue("isDisable", oldUser.DISABLE_FLG, isDisable);
                app.SystemOperationLogBLL.addData_KeyValue("UserGroup", oldUser.USER_GRP, userGrp);
                app.SystemOperationLogBLL.addData_KeyValue("BadgeNo", oldUser.BADGE_NUMBER, badgeNo);
                app.SystemOperationLogBLL.addData_KeyValue("Department", oldUser.DEPARTMENT, department);
                app.SystemOperationLogBLL.addSystemOperationHis(result);
            }        
            return result == "OK";

        }

        public bool SendUserAccountDeleteRequest(string userID, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserAccountDelete",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userID)}={userID}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue("UserID",userID);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendUserGroupAddRequest(string userGrp, string funcCloseSystem, string funcSystemControlMode, string funcLogin, string funcAccountManagement
            , string funcVehicleManagement, string funcTransferManagement, string funcMTLMTSMaintenance, string funcPortMaintenance, string funcDebug, string funcAdvancedSettings, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserGroupAdd",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userGrp)}={userGrp}").Append("&");
            sb.Append($"{nameof(funcCloseSystem)}={funcCloseSystem}").Append("&");
            sb.Append($"{nameof(funcSystemControlMode)}={funcSystemControlMode}").Append("&");
            sb.Append($"{nameof(funcLogin)}={funcLogin}").Append("&");
            sb.Append($"{nameof(funcAccountManagement)}={funcAccountManagement}").Append("&");
            sb.Append($"{nameof(funcVehicleManagement)}={funcVehicleManagement}").Append("&");
            sb.Append($"{nameof(funcTransferManagement)}={funcTransferManagement}").Append("&");
            sb.Append($"{nameof(funcMTLMTSMaintenance)}={funcMTLMTSMaintenance}").Append("&");
            sb.Append($"{nameof(funcPortMaintenance)}={funcPortMaintenance}").Append("&");
            sb.Append($"{nameof(funcDebug)}={funcDebug}").Append("&");
            sb.Append($"{nameof(funcAdvancedSettings)}={funcAdvancedSettings}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue("UserGroup", userGrp);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendUserGroupAddRequest(Dictionary<string, bool> fun_enable, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserGroupAdd",
            };
            StringBuilder sb = new StringBuilder();
            var a = JsonConvert.SerializeObject(fun_enable);
            byte[] byteArray = Encoding.UTF8.GetBytes(a.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            foreach(var item in fun_enable)
            {
                app.SystemOperationLogBLL.addData_KeyValue(item.Key.ToString(), item.Value.ToString());
            }
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendUserGroupUpdateRequest(string userGrp, string funcCloseSystem, string funcSystemControlMode, string funcLogin, string funcAccountManagement,
                                               string funcVehicleManagement, string funcTransferManagement, string funcMTLMTSMaintenance, string funcPortMaintenance,
                                               string funcDebug, string funcAdvancedSettings, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserGroupUpdate",
            };
            StringBuilder sb = new StringBuilder();
            HashSet<string> hsOldData = new HashSet<string>();
            var OlduserFuncList = app.ObjCacheManager.GetUserFuncs(userGrp);
            if(OlduserFuncList.Count > 0)
            {
                string oldGroup = OlduserFuncList[0].USER_GRP;
                foreach (var uasufnc in OlduserFuncList)
                {
                    hsOldData.Add(uasufnc.FUNC_CODE.Trim());
                }
            }        

            sb.Append($"{nameof(userGrp)}={userGrp}").Append("&");
            sb.Append($"{nameof(funcCloseSystem)}={funcCloseSystem}").Append("&");
            sb.Append($"{nameof(funcSystemControlMode)}={funcSystemControlMode}").Append("&");
            sb.Append($"{nameof(funcLogin)}={funcLogin}").Append("&");
            sb.Append($"{nameof(funcAccountManagement)}={funcAccountManagement}").Append("&");
            sb.Append($"{nameof(funcVehicleManagement)}={funcVehicleManagement}").Append("&");
            sb.Append($"{nameof(funcTransferManagement)}={funcTransferManagement}").Append("&");
            sb.Append($"{nameof(funcMTLMTSMaintenance)}={funcMTLMTSMaintenance}").Append("&");
            sb.Append($"{nameof(funcPortMaintenance)}={funcPortMaintenance}").Append("&");
            sb.Append($"{nameof(funcDebug)}={funcDebug}").Append("&");
            sb.Append($"{nameof(funcAdvancedSettings)}={funcAdvancedSettings}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue("UserGroup", userGrp, userGrp);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcCloseSystem), hsOldData.Contains(nameof(funcCloseSystem)).ToString(), funcCloseSystem);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcSystemControlMode), hsOldData.Contains(nameof(funcSystemControlMode)).ToString(), funcSystemControlMode);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcLogin), hsOldData.Contains(nameof(funcLogin)).ToString(), funcLogin);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcAccountManagement), hsOldData.Contains(nameof(funcAccountManagement)).ToString(), funcAccountManagement);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcVehicleManagement), hsOldData.Contains(nameof(funcVehicleManagement)).ToString(), funcVehicleManagement);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcTransferManagement), hsOldData.Contains(nameof(funcTransferManagement)).ToString(), funcTransferManagement);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcMTLMTSMaintenance), hsOldData.Contains(nameof(funcMTLMTSMaintenance)).ToString(), funcMTLMTSMaintenance);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcPortMaintenance), hsOldData.Contains(nameof(funcPortMaintenance)).ToString(), funcPortMaintenance);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcDebug), hsOldData.Contains(nameof(funcDebug)).ToString(), funcDebug);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(funcAdvancedSettings), hsOldData.Contains(nameof(funcAdvancedSettings)).ToString(), funcAdvancedSettings);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendUserGroupUpdateRequest(Dictionary<string, bool> fun_enable,out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserGroupUpdate",
            };
            string oldusergroup = fun_enable.Keys.ToList()[0];
            HashSet<string> hsOldData = new HashSet<string>();
            var OlduserFuncList = app.ObjCacheManager.GetUserFuncs(oldusergroup);
            if (OlduserFuncList.Count > 0)
            {
                foreach (var uasufnc in OlduserFuncList)
                {
                    hsOldData.Add(uasufnc.FUNC_CODE.Trim());
                }
            }

            StringBuilder sb = new StringBuilder();
            var a = JsonConvert.SerializeObject(fun_enable);

            byte[] byteArray = Encoding.UTF8.GetBytes(a.ToString());

            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            foreach (var item in fun_enable)
            {
                    app.SystemOperationLogBLL.addData_KeyValue(item.Key.ToString(), hsOldData.Contains(item.Key.ToString()).ToString(), item.Value.ToString());   
            }
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendUserGroupDeleteRequest(string userGrp, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UserGroupDelete",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userGrp)}={userGrp}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue("UserGroup", userGrp);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendExitRequest(string userID, string password, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "Exit",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userID)}={userID}").Append("&");
            sb.Append($"{nameof(password)}={password}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue("UserID", userID);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendPasswordChange(string userID, string password_o, string password_n, string password_v, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "UserControl",
                "UpdatePassword",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(userID)}={userID}").Append("&");
            sb.Append($"{nameof(password_o)}={password_o}").Append("&");
            sb.Append($"{nameof(password_n)}={password_n}").Append("&");
            sb.Append($"{nameof(password_v)}={password_v}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue("UserID", userID);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMCSCommandAutoAssignChange(string AutoAssign, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "MCSQueueSwitch",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(AutoAssign)}={AutoAssign}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);
            return result == "OK";
        }

        public bool SendMCSCommandCancelAbort(string mcs_cmd, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "CancelAbort",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(mcs_cmd)}={mcs_cmd}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(mcs_cmd), mcs_cmd);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMCSCommandForceFinish(string mcs_cmd, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "ForceFinish",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(mcs_cmd)}={mcs_cmd}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(mcs_cmd), mcs_cmd);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMCSCommandAssignVehicle(string mcs_cmd, string vh_id, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "AssignVehicle",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(mcs_cmd)}={mcs_cmd}").Append("&");
            sb.Append($"{nameof(vh_id)}={vh_id}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(mcs_cmd), mcs_cmd);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMCSCommandShift(string mcs_cmd, string vh_id, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "ShiftCommand",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(mcs_cmd)}={mcs_cmd}").Append("&");
            sb.Append($"{nameof(vh_id)}={vh_id}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(mcs_cmd), mcs_cmd);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(vh_id), vh_id);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMCSCommandChangeStatus(string mcs_cmd, string status, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "ChangeStatus",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(mcs_cmd)}={mcs_cmd}").Append("&");
            sb.Append($"{nameof(status)}={status}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(mcs_cmd), mcs_cmd);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(status), status);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMCSCommandChangePriority(string mcs_cmd, string priority, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "TransferManagement",
                "Priority",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(mcs_cmd)}={mcs_cmd}").Append("&");
            sb.Append($"{nameof(priority)}={priority}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(mcs_cmd), mcs_cmd);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(priority), priority);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendMTSMTLInterlock(string station_id, string isSet, out string result)
        {
            result = string.Empty;
            string[] action_targets = new string[]
            {
                "MTSMTLInfo",
                "InterlockRequest",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(station_id)}={station_id}").Append("&");
            sb.Append($"{nameof(isSet)}={isSet}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(station_id), station_id);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(isSet), isSet);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        #region LogLevel
        public bool SendLogLevelAdd( out string result)
        {
            result = string.Empty;
            string level = LogStatus.nowLevel;
            string[] action_targets = new string[]
            {
                "UserControl",
                "LogLevelAdd",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(level)}={level}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(level), level);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendLogLevelChange( out string result)
        {

            result = string.Empty;
            string oldlevel = LogStatus.oldLevel;
            string newlevel = LogStatus.nowLevel;

            string[] action_targets = new string[]
            {
                "UserControl",
                "LogLevelChange",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(oldlevel)}={oldlevel}").Append("&");
            sb.Append($"{nameof(newlevel)}={newlevel}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(oldlevel), oldlevel);
            app.SystemOperationLogBLL.addData_KeyValue(nameof(newlevel), newlevel);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }

        public bool SendLogLevelRemove( out string result)
        {
            result = string.Empty;
            string level = LogStatus.nowLevel;
            string[] action_targets = new string[]
            {
                "UserControl",
                "LogLevelRemove",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(level)}={level}");
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());
            result = app.GetWebClientManager().PostInfoToServer(WebClientManager.OHxC_CONTROL_URI, action_targets, WebClientManager.HTTP_METHOD.POST, byteArray);

            app.SystemOperationLogBLL.addData_KeyValue(nameof(level), level);
            app.SystemOperationLogBLL.addSystemOperationHis(result);

            return result == "OK";
        }
        #endregion

        #endregion WebAPI
    }
}

