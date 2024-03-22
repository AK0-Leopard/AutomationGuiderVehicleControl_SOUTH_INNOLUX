using com.mirle.ibg3k0.ohxc.wpf.App;
using NLog;
using STAN.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ViewerObject;

namespace UtilsAPI.BLL
{
    public class UserBLL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;

        public UserBLL(WindownApplication _app)
        {
            app = _app;
        }

        public List<VUASUSR> LoadAllUser()
        {
            var default_result = new List<VUASUSR>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.LoadAllUser();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VUASUSRGRP> LoadAllUserGroup()
        {
            var default_result = new List<VUASUSRGRP>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.LoadAllUserGroup();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VUASUFNC> LoadAllUserGroupFunc()
        {
            var default_result = new List<VUASUFNC>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.LoadAllUserGroupFunc();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public List<VUASFNC> LoadAllFunctionCode()
        {
            var default_result = new List<VUASFNC>();
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.LoadAllFunctionCode();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VUASUSR GetUser(string user_id)
        {
            VUASUSR default_result = null;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.GetUser(user_id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public VUASUFNC GetUserFunc(string user_grp, string function_code)
        {
            VUASUFNC default_result = null;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.GetUserFunc(user_grp, function_code);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public bool CheckUserPassword(string user_id, string password)
        {
            bool default_result = false;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.CheckUserPassword(user_id, password);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }

        public bool CheckUserAuthority(string user_id, string function_code)
        {
            bool default_result = false;
            try
            {
                return app.ObjCacheManager.ObjConverter.BLL.UserBLL.CheckUserAuthority(user_id, function_code);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
                return default_result;
            }
        }
    }
}
