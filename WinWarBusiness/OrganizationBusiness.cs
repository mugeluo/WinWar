using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using WinWarEntity;
using WinWarDAL;
namespace WinWarBusiness
{
    public class OrganizationBusiness
    {

        #region Cache

        private static Dictionary<string, List<Users>> _cacheUsers;
        private static Dictionary<string, List<Role>> _cacheRoles;

        /// <summary>
        /// 缓存用户信息
        /// </summary>
        private static Dictionary<string, List<Users>> Users
        {
            get 
            {
                if (_cacheUsers == null)
                {
                    _cacheUsers = new Dictionary<string, List<Users>>();
                }
                return _cacheUsers;
            }
            set
            {
                _cacheUsers = value;
            }
        }


        /// <summary>
        /// 缓存角色信息
        /// </summary>
        private static Dictionary<string, List<Role>> Roles
        {
            get
            {
                if (_cacheRoles == null)
                {
                    _cacheRoles = new Dictionary<string, List<Role>>();
                }
                return _cacheRoles;
            }
            set
            {
                _cacheRoles = value;
            }
        }

        #endregion

        #region 查询

        public static bool IsExistLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName)) return false;

            object count = CommonBusiness.Select("Users", "count(0)", " Status=1 and (LoginName='" + loginName + "'  or BindMobilePhone='" + loginName + "')");
            return Convert.ToInt32(count) > 0;
        }

        public static Users GetUserByUserName(string loginname, string pwd,out int result, string operateip)
        {
            pwd = WinWarTool.Encrypt.GetEncryptPwd(pwd, loginname);
            DataSet ds = new OrganizationDAL().GetUserByUserName(loginname, pwd, out result);
            Users model = null;
            if (ds.Tables.Contains("User") && ds.Tables["User"].Rows.Count > 0)
            {
                model = new Users();
                model.FillData(ds.Tables["User"].Rows[0]);

                model.LogGUID = Guid.NewGuid().ToString();
                model.Role = GetRoleByID(model.RoleID);
               


                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ClientMenus;
                }
                else
                {
                    model.Menus = new List<Menu>();
                    foreach (DataRow dr in ds.Tables["Permission"].Rows)
                    {
                        Menu menu = new Menu();
                        menu.FillData(dr);
                        model.Menus.Add(menu);
                    }
                }
            }
            return model;
        }

        public static bool ConfirmLoginPwd(string loginname, string pwd)
        {
            pwd = WinWarTool.Encrypt.GetEncryptPwd(pwd, loginname);
            int result;
            DataSet ds = new OrganizationDAL().GetUserByUserName(loginname, pwd,out result);

            if (ds.Tables.Contains("User") && ds.Tables["User"].Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public static Users GetUserByMDUserID(string mduserid, string operateip)
        {
            DataSet ds = new OrganizationDAL().GetUserByMDUserID(mduserid);
            Users model = null;
            if (ds.Tables.Contains("User") && ds.Tables["User"].Rows.Count > 0)
            {
                model = new Users();
                model.FillData(ds.Tables["User"].Rows[0]);

                model.LogGUID = Guid.NewGuid().ToString();

                model.Role = GetRoleByID(model.RoleID);
                //处理缓存
                if (!Users.ContainsKey(model.AgentID))
                {
                    GetUsers(model.AgentID);
                }
                if (Users[model.AgentID].Where(u => u.UserID == model.UserID).Count() == 0)
                {
                    Users[model.AgentID].Add(model);
                }
                else
                {
                    var user = Users[model.AgentID].Where(u => u.UserID == model.UserID).FirstOrDefault();
                    user.LogGUID = model.LogGUID;
                }

                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ClientMenus;
                }
                else
                {
                    model.Menus = new List<Menu>();
                    foreach (DataRow dr in ds.Tables["Permission"].Rows)
                    {
                        Menu menu = new Menu();
                        menu.FillData(dr);
                        model.Menus.Add(menu);
                    }
                }
            }
            if (string.IsNullOrEmpty(operateip))
            {
                operateip = "";
            }


            return model;
        }

        public static Users GetUserByAliMemberID(string aliMemberID, string operateip)
        {
            DataSet ds = new OrganizationDAL().GetUserByAliMemberID(aliMemberID);
            Users model = null;
            if (ds.Tables.Contains("User") && ds.Tables["User"].Rows.Count > 0)
            {
                model = new Users();
                model.FillData(ds.Tables["User"].Rows[0]);

                model.LogGUID = Guid.NewGuid().ToString();

                model.Role = GetRoleByID(model.RoleID);

                //处理缓存
                if (!Users.ContainsKey(model.AgentID))
                {
                    GetUsers(model.AgentID);
                }
                if (Users[model.AgentID].Where(u => u.UserID == model.UserID).Count() == 0)
                {
                    Users[model.AgentID].Add(model);
                }
                else
                {
                    var user = Users[model.AgentID].Where(u => u.UserID == model.UserID).FirstOrDefault();
                    user.LogGUID = model.LogGUID;
                }

                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ClientMenus;
                }
                else
                {
                    model.Menus = new List<Menu>();
                    foreach (DataRow dr in ds.Tables["Permission"].Rows)
                    {
                        Menu menu = new Menu();
                        menu.FillData(dr);
                        model.Menus.Add(menu);
                    }
                }
            }
            if (string.IsNullOrEmpty(operateip))
            {
                operateip = "";
            }

            return model;
        }

        public static Users GetUserByUserID(string userid)
        {
            
            if (string.IsNullOrEmpty(userid))
            {
                return null;
            }
            userid = userid.ToLower();

            DataTable dt = new OrganizationDAL().GetUserByUserID(userid);
            Users model = new Users();
            if (dt.Rows.Count > 0)
            {
                model.FillData(dt.Rows[0]);
                model.Role = GetRoleByID(model.RoleID); 
            }
            return model;

        }

        public static List<Users> GetUsers(string keyWords, string departID, string roleID, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            string whereSql ="  Status<>9";

            if (!string.IsNullOrEmpty(keyWords))
                whereSql += " and ( Name like '%" + keyWords + "%' or MobilePhone like '%" + keyWords + "%' or Email like '%" + keyWords + "%')";

            if (!string.IsNullOrEmpty(departID))
                whereSql += " and DepartID='" + departID + "'";

            if (!string.IsNullOrEmpty(roleID))
                whereSql += " and RoleID='" + roleID + "'";

            DataTable dt = CommonBusiness.GetPagerData("Users", "*", whereSql, "AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<Users> list = new List<Users>();
            Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new Users();
                model.FillData(item);

                model.CreateUser = GetUserByUserID(model.CreateUserID);
                model.Role = GetRoleByID(model.RoleID);

                list.Add(model);
            }

            return list;
        }

        public static List<Users> GetUsers(string agentid)
        {
            if (string.IsNullOrEmpty(agentid))
            {
                return new List<Users>();
            }
            if (!Users.ContainsKey(agentid))
            {
                List<Users> list = new List<WinWarEntity.Users>();
                DataTable dt = OrganizationDAL.BaseProvider.GetUsers(agentid);
                foreach (DataRow dr in dt.Rows)
                {
                    Users model = new Users();
                    model.FillData(dr);

                    model.Role = GetRoleByID(model.RoleID);

                    list.Add(model);
                }
                Users.Add(agentid, list);
                return list;
            }
            return Users[agentid].ToList();
        }

        public static List<Users> GetUsersByParentID(string parentid, string agentid)
        {
            var users = GetUsers(agentid).Where(m => m.ParentID == parentid && m.Status == 1).ToList();
            return users;
        }

        public static List<Users> GetStructureByParentID(string parentid, string agentid)
        {
            var users = GetUsersByParentID(parentid, agentid);
            foreach (var user in users)
            {
                user.ChildUsers = GetStructureByParentID(user.UserID, agentid);
            }
            return users;
        }

        public static List<Role> GetRoles()
        {

            DataTable dt = new OrganizationDAL().GetRoles();
            List<Role> list = new List<Role>();
            foreach (DataRow dr in dt.Rows)
            {
                Role model = new Role();
                model.FillData(dr);
                list.Add(model);
            }

            return list;

        }


        public static Role GetRoleByID(string roleid)
        {
            Role model = null;
            DataSet ds = OrganizationDAL.BaseProvider.GetRoleByID(roleid);
            if (ds.Tables.Contains("Role") && ds.Tables["Role"].Rows.Count > 0)
            {
                model = new Role();
                model.FillData(ds.Tables["Role"].Rows[0]);
                model.Menus = new List<Menu>();
                foreach (DataRow dr in ds.Tables["Menus"].Rows)
                {
                    Menu menu = new Menu();
                    menu.FillData(dr);
                    model.Menus.Add(menu);
                }
            }
            return model;
        }

        #endregion

        #region 添加


        public string CreateRole(string name, string parentid, string description, string operateid, string agentid, string clientid)
        {
            string roleid = Guid.NewGuid().ToString();
            bool bl = OrganizationDAL.BaseProvider.CreateRole(roleid, name, parentid, description, operateid, agentid, clientid);
            if (bl)
            {
                return roleid;
            }
            return "";
        }

        public static Users CreateUser(string loginname, string loginpwd, string name, string mobile, string email, string citycode, string address, string jobs,
                               string roleid, string operateid, out int result)
        {
            string userid = Guid.NewGuid().ToString();

            loginpwd = WinWarTool.Encrypt.GetEncryptPwd(loginpwd, loginname);

            Users user = null;

            DataTable dt = OrganizationDAL.BaseProvider.CreateUser(userid, loginname, loginpwd, name, mobile, email, citycode, address, jobs, roleid, operateid, out result);
            if (dt.Rows.Count > 0)
            {
                user = new Users();
                user.FillData(dt.Rows[0]);
            }
            return user;
        }

        #endregion

        #region 编辑/删除

        public static bool UpdateUserAccount(string userid, string loginName, string loginPwd, string agentid)
        {
            loginPwd = WinWarTool.Encrypt.GetEncryptPwd(loginPwd, loginName);
            bool flag = OrganizationDAL.BaseProvider.UpdateUserAccount(userid, loginName, loginPwd);

            if (flag)
            {
                if (Users.ContainsKey(agentid))
                {
                    List<Users> users = Users[agentid];
                    Users u = users.Find(m => m.UserID == userid);
                    u.LoginName = loginName;
                }
            }
            return flag;
        }

        public static bool UpdateUserPass(string userid, string loginPwd, string agentid)
        {
            loginPwd = WinWarTool.Encrypt.GetEncryptPwd(loginPwd, "");
            bool flag = OrganizationDAL.BaseProvider.UpdateUserPass(userid, loginPwd);

            return flag;
        }

        public static bool UpdateUserAccountPwd(string loginName, string loginPwd)
        {
            loginPwd = WinWarTool.Encrypt.GetEncryptPwd(loginPwd, loginName);
            bool flag = OrganizationDAL.BaseProvider.UpdateUserAccountPwd(loginName, loginPwd);

            return flag;
        }

        public static bool UpdateUserInfo(string userid, string name, string jobs, DateTime birthday, int age, string departID, string email, string mobilePhone, string officePhone, string agentid)
        {
            bool flag = OrganizationDAL.BaseProvider.UpdateUserInfo(userid, name, jobs, birthday, age, departID, email, mobilePhone, officePhone);

           //清除缓存
           if (flag)
           {
               if (Users.ContainsKey(agentid))
               {
                   List<Users> users = Users[agentid];
                   Users u = users.Find(m => m.UserID == userid);
                   u.Name = name;
                   u.Jobs = jobs;
                   u.Birthday = birthday;
                   u.Age = age;
                   u.DepartID = departID;
                   u.Email = email;
                   u.MobilePhone = mobilePhone;
                   u.OfficePhone = officePhone;
               }
           }
           return flag;
        }

        public static bool UpdateAccountAvatar(string userid, string avatar, string agentid)
        {
            bool flag = OrganizationDAL.BaseProvider.UpdateAccountAvatar(userid, avatar);

            //清除缓存
            if (flag)
            {
                if (Users.ContainsKey(agentid))
                {
                    List<Users> users = Users[agentid];
                    Users u = users.Find(m => m.UserID == userid);
                    u.Avatar = avatar;
                }
            }
            return flag;
        }

        public static bool UpdateAccountBindMobile(string userid, string bindMobile, string agentid)
        {
            bool flag = OrganizationDAL.BaseProvider.UpdateAccountBindMobile(userid, bindMobile);

            //清除缓存
            if (flag)
            {
                if (Users.ContainsKey(agentid))
                {
                    List<Users> users = Users[agentid];
                    Users u = users.Find(m => m.UserID == userid);
                    u.BindMobilePhone = bindMobile;
                }
            }
            return flag;
        }

        public static bool BindAccountAliMember(string userid, string memberid, string agentid)
        {

            bool flag = OrganizationDAL.BaseProvider.BindAccountAliMember(userid, memberid);

            //清除缓存
            if (flag)
            {
                if (Users.ContainsKey(agentid))
                {
                    List<Users> users = Users[agentid];
                    Users u = users.Find(m => m.UserID == userid);
                    u.AliMemberID = memberid;
                }
            }
            return flag;
        }

        public static bool ClearAccountBindMobile(string userid, string agentid)
        {
            bool flag = OrganizationDAL.BaseProvider.ClearAccountBindMobile(userid);

            //清除缓存
            if (flag)
            {
                if (Users.ContainsKey(agentid))
                {
                    List<Users> users = Users[agentid];
                    Users u = users.Find(m => m.UserID == userid);
                    u.BindMobilePhone = string.Empty;
                }
            }
            return flag;
        }

        public bool UpdateRole(string roleid, string name, string description, string operateid, string ip)
        {
            bool bl = OrganizationDAL.BaseProvider.UpdateRole(roleid, name, description);

            return bl;
        }

        public bool DeleteRole(string roleid, string operateid, string ip, out int result)
        {
            bool bl = OrganizationDAL.BaseProvider.DeleteRole(roleid, out result);

            return bl;
        }

        public bool UpdateRolePermission(string roleid, string permissions, string operateid, string ip)
        {
            return OrganizationDAL.BaseProvider.UpdateRolePermission(roleid, permissions, operateid);
        }

        public bool UpdateUserParentID(string userid, string parentid, string agentid, string operateid, string ip)
        {
            bool bl = OrganizationDAL.BaseProvider.UpdateUserParentID(userid, parentid, agentid);
            if (bl)
            {
                var user = GetUserByUserID(userid);
                user.ParentID = parentid;
            }
            return bl;
        }

        public bool ChangeUsersParentID(string userid, string olduserid, string agentid, string operateid, string ip)
        {
            bool bl = OrganizationDAL.BaseProvider.ChangeUsersParentID(userid, olduserid, agentid);
            if (bl)
            {
                //新员工
                var user = GetUserByUserID(userid);
                //被替换员工
                var oldUser = GetUserByUserID(olduserid);

                user.ParentID = oldUser.ParentID;
                oldUser.ParentID = "";
                var list = GetUsersByParentID(olduserid, agentid);
                foreach (var model in list)
                {
                    model.ParentID = userid;
                }

            }
            return bl;
        }

        public bool DeleteUserByID(string userid, string operateid, string ip, out int result)
        {
            bool bl = OrganizationDAL.BaseProvider.DeleteUserByID(userid, out result);

            return bl;
        }

        public bool UpdateUserRole(string userid, string roleid, string operateid, string ip)
        {
            var user = GetUserByUserID(userid);
            if (user.RoleID.ToLower() != roleid.ToLower())
            {
                bool bl = OrganizationDAL.BaseProvider.UpdateUserRole(userid, roleid, operateid);
                return bl;
            }
            return true;
        }

        #endregion
    }
}
