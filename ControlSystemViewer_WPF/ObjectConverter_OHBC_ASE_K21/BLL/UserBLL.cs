using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.Data;
using ObjectConverterInterface.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHBC_ASE_K21.BLL
{
    public class UserBLL : IUserBLL
    {
        private UasConverter uasConverter = new UasConverter();

        private string connectionString = "";
        public UserBLL(string _connectionString)
        {
            connectionString = _connectionString ?? "";
        }

        public List<ViewerObject.VUASUSR> LoadAllUser() => uasConverter.GetVUASUSRs(loadAllUser());
        private List<UASUSR> loadAllUser()
        {
            List<UASUSR> rtnObj = new List<UASUSR>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from user in con.UASUSR.AsNoTracking()
                            select user;
                rtnObj.AddRange(query?.ToList() ?? new List<UASUSR>());
            }
            return rtnObj;
        }

        public List<ViewerObject.VUASUSRGRP> LoadAllUserGroup() => uasConverter.GetVUASUSRGRPs(loadAllUserGroup());
        private List<UASUSRGRP> loadAllUserGroup()
        {
            List<UASUSRGRP> rtnObj = new List<UASUSRGRP>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from userGroup in con.UASUSRGRP.AsNoTracking()
                            select userGroup;
                rtnObj.AddRange(query?.ToList() ?? new List<UASUSRGRP>());
            }
            return rtnObj;
        }

        public List<ViewerObject.VUASUFNC> LoadAllUserGroupFunc() => uasConverter.GetVUASUFNCs(loadAllUserGroupFunc());
        private List<UASUFNC> loadAllUserGroupFunc()
        {
            List<UASUFNC> rtnObj = new List<UASUFNC>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from userFun in con.UASUFNC.AsNoTracking()
                            select userFun;
                rtnObj.AddRange(query?.ToList() ?? new List<UASUFNC>());
            }
            return rtnObj;
        }

        public List<ViewerObject.VUASFNC> LoadAllFunctionCode() => uasConverter.GetVUASFNCs(loadAllFunctionCode());
        private List<UASFNC> loadAllFunctionCode()
        {
            List<UASFNC> rtnObj = new List<UASFNC>();
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                var query = from funcCode in con.UASFNC.AsNoTracking()
                            select funcCode;
                rtnObj.AddRange(query?.ToList() ?? new List<UASFNC>());
            }
            return rtnObj;
        }

        public ViewerObject.VUASUSR GetUser(string user_id) => uasConverter.GetVUASUSR(getUser(user_id));
        private UASUSR getUser(string user_id)
        {
            UASUSR rtnObj = null;
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                rtnObj = getUser(con, user_id);
            }
            return rtnObj;
        }
        private UASUSR getUser(DBConnection_EF con, string user_id)
        {
            var query = from user in con.UASUSR.AsNoTracking()
                        where user.USER_ID == user_id.Trim()
                        select user;
            return query?.SingleOrDefault();
        }

        public ViewerObject.VUASUFNC GetUserFunc(string user_grp, string function_code) => uasConverter.GetVUASUFNC(getUserFunc(user_grp, function_code));
        private UASUFNC getUserFunc(string user_grp, string function_code)
        {
            UASUFNC rtnObj = null;
            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                rtnObj = getUserFunc(con, user_grp, function_code);
            }
            return rtnObj;
        }
        private UASUFNC getUserFunc(DBConnection_EF con, string user_grp, string function_code)
        {
            UASUFNC rtnObj = null;
            var query = from userFun in con.UASUFNC.AsNoTracking()
                        where userFun.USER_GRP == user_grp.Trim()
                        && userFun.FUNC_CODE == function_code.Trim()
                        select userFun;
            rtnObj = query.FirstOrDefault();
            return rtnObj;
        }

        public bool CheckUserPassword(string user_id, string password)
        {
            bool result = false;

            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                con.BeginTransaction();
                UASUSR loginUser = getUser(con, user_id);
                if (loginUser != null)
                {
                    result = loginUser.PASSWD == password;
                }
                con.Commit();
            }
            return result;
        }

        public bool CheckUserAuthority(string user_id, string function_code)
        {
            bool result = false;

            using (DBConnection_EF con = DBConnection_EF.GetUContext())
            {
                if (!string.IsNullOrWhiteSpace(connectionString))
                    con.Database.Connection.ConnectionString = connectionString;

                // MenuBar第一層 => 若DB有，子功能全開(.Enabled = true) / DB沒有，子功能獨立判斷，若有任一子功能有，第一層也要開
                bool isOperationFirstLayer = function_code == ViewerObject.UAS_Def.Operation_Function.FUNC_OPERATION_FUN;
                bool isMaintenanceFirstLayer = function_code == ViewerObject.UAS_Def.Maintenance_Function.FUNC_MAINTENANCE_FUN;
                FieldInfo[] OperationFields = typeof(ViewerObject.UAS_Def.Operation_Function).GetFields();
                FieldInfo[] MaintenanceFields = typeof(ViewerObject.UAS_Def.Maintenance_Function).GetFields();
                List<string> SubFuncList = new List<string>();
                string function_code_FirstLayer = "";
                if (isOperationFirstLayer)
                    SubFuncList.AddRange(OperationFields.Where(x => x.Name.Trim() != function_code).Select(func => func.Name).ToList());
                else if (isMaintenanceFirstLayer)
                    SubFuncList.AddRange(MaintenanceFields.Where(x => x.Name.Trim() != function_code).Select(func => func.Name).ToList());
                else
                {
                    if (OperationFields.Where(x => x.Name == function_code?.Trim()).Any())
                        function_code_FirstLayer = ViewerObject.UAS_Def.Operation_Function.FUNC_OPERATION_FUN;
                    else if (MaintenanceFields.Where(x => x.Name == function_code?.Trim()).Any())
                        function_code_FirstLayer = ViewerObject.UAS_Def.Maintenance_Function.FUNC_MAINTENANCE_FUN;
                }

                con.BeginTransaction();
                UASUSR loginUser = getUser(con, user_id);
                if (loginUser != null)
                {
                    //A0.01 UserFunc userFunc = userFuncDao.getUserFunc(conn, user_id, function_code);
                    // MenuBar第一層 => 若DB有，子功能全開(.Enabled = true) / DB沒有，子功能獨立判斷，若有任一子功能有，第一層也要開
                    UASUFNC userFunc = getUserFunc(con, loginUser.USER_GRP, function_code);
                    if (userFunc != null) result = true;
                    else
                    {
                        if (isOperationFirstLayer || isMaintenanceFirstLayer) // MenuBar第一層DB沒有，若有任一子功能有，第一層也要開
                        {
                            result = false;
                            int idx = 0;
                            while (!result)
                            {
                                if (idx >= SubFuncList.Count())
                                    break;

                                result = getUserFunc(con, loginUser.USER_GRP, SubFuncList[idx]) != null;
                                idx++;
                            }
                        }
                        else if (!string.IsNullOrEmpty(function_code_FirstLayer)) // MenuBar第一層 => 若DB有，子功能全開 (子功能DB沒有，但若第一層有，還是要開)
                        {
                            result = getUserFunc(con, loginUser.USER_GRP, function_code_FirstLayer) != null;
                        }
                    }
                }
                con.Commit();
            }
            return result;
        }
    }

    public class UasConverter
    {
        public ViewerObject.VUASUSR GetVUASUSR(UASUSR input)
        {
            ViewerObject.VUASUSR output = null;
            if (input != null)
            {
                output = new ViewerObject.VUASUSR()
                {
                    USER_ID = input.USER_ID?.Trim() ?? "",
                    PASSWD = input.PASSWD?.Trim() ?? "",
                    BADGE_NUMBER = input.BADGE_NUMBER?.Trim() ?? "",
                    USER_NAME = input.USER_NAME?.Trim() ?? "",
                    DISABLE_FLG = input.DISABLE_FLG?.Trim() ?? "",
                    POWER_USER_FLG = input.POWER_USER_FLG?.Trim() ?? "",
                    ADMIN_FLG = input.ADMIN_FLG?.Trim() ?? "",
                    USER_GRP = input.USER_GRP?.Trim() ?? "",
                    DEPARTMENT = input.DEPARTMENT?.Trim() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.VUASUSR> GetVUASUSRs(List<UASUSR> input)
        {
            List<ViewerObject.VUASUSR> output = new List<ViewerObject.VUASUSR>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVUASUSR(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
        public ViewerObject.VUASUSRGRP GetVUASUSRGRP(UASUSRGRP input)
        {
            ViewerObject.VUASUSRGRP output = null;
            if (input != null)
            {
                output = new ViewerObject.VUASUSRGRP()
                {
                    USER_GRP = input.USER_GRP?.Trim() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.VUASUSRGRP> GetVUASUSRGRPs(List<UASUSRGRP> input)
        {
            List<ViewerObject.VUASUSRGRP> output = new List<ViewerObject.VUASUSRGRP>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVUASUSRGRP(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
        public ViewerObject.VUASUFNC GetVUASUFNC(UASUFNC input)
        {
            ViewerObject.VUASUFNC output = null;
            if (input != null)
            {
                output = new ViewerObject.VUASUFNC()
                {
                    USER_GRP = input.USER_GRP?.Trim() ?? "",
                    FUNC_CODE = input.FUNC_CODE?.Trim() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.VUASUFNC> GetVUASUFNCs(List<UASUFNC> input)
        {
            List<ViewerObject.VUASUFNC> output = new List<ViewerObject.VUASUFNC>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVUASUFNC(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
        public ViewerObject.VUASFNC GetVUASFNC(UASFNC input)
        {
            ViewerObject.VUASFNC output = null;
            if (input != null)
            {
                output = new ViewerObject.VUASFNC()
                {
                    FUNC_CODE = input.FUNC_CODE?.Trim() ?? "",
                    FUNC_NAME = input.FUNC_NAME?.Trim() ?? ""
                };
            }
            return output;
        }
        public List<ViewerObject.VUASFNC> GetVUASFNCs(List<UASFNC> input)
        {
            List<ViewerObject.VUASFNC> output = new List<ViewerObject.VUASFNC>();
            if (input?.Count > 0)
            {
                foreach (var i in input)
                {
                    var o = GetVUASFNC(i);
                    if (o != null) output.Add(o);
                }
            }
            return output;
        }
    }
}
