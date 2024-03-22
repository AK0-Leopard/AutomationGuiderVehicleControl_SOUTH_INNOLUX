using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverterInterface.BLL
{
    public interface IUserBLL
    {
        List<ViewerObject.VUASUSR> LoadAllUser();
        List<ViewerObject.VUASUSRGRP> LoadAllUserGroup();
        List<ViewerObject.VUASUFNC> LoadAllUserGroupFunc();
        List<ViewerObject.VUASFNC> LoadAllFunctionCode();
        ViewerObject.VUASUSR GetUser(string user_id);
        ViewerObject.VUASUFNC GetUserFunc(string user_grp, string function_code);
        bool CheckUserPassword(string user_id, string password);
        bool CheckUserAuthority(string user_id, string function_code);
    }
}
