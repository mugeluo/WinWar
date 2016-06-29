using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WinWarBusiness;
using WinWarEntity;
using WinWarWeb.Models;
namespace WinWarWeb.Areas.Manage.Controllers
{
    public class OrganizationController :BaseController
    {
        //
        // GET: /Manage/System/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Department()
        {
            return View();
        }

        public ActionResult Roles()
        {
            return View();
        }

        public ActionResult RolePermission(string id)
        {
            ViewBag.Model = OrganizationBusiness.GetRoleByID(id);
            ViewBag.Menus = CommonBusiness.ClientMenus.Where(m => m.PCode == ExpandClass.CLIENT_TOP_CODE).ToList();
            return View();
        }

        public ActionResult Users()
        {
            ViewBag.Roles = OrganizationBusiness.GetRoles();

            ViewBag.IsSysAdmin = CurrentUser.Role.IsDefault == 1 ? true : false;

            return View();
        }

        public ActionResult CreateUser()
        {
            ViewBag.Roles = OrganizationBusiness.GetRoles();
            return View();
        }

        public ActionResult Structure()
        {
            var list = OrganizationBusiness.GetStructureByParentID("6666666666", CurrentUser.AgentID);
            if (list.Count > 0)
            {
                ViewBag.Model = list[0];
            }
            else
            {
                ViewBag.Model = new Users();
            }
            return View();
        }

        public ActionResult UpdatePwd()
        {
            return View();
        }

        #region Ajax

        public JsonResult GetRoles()
        {
            var list = OrganizationBusiness.GetRoles();
            foreach (var item in list)
            {
                if (item.CreateUser == null && !string.IsNullOrEmpty(item.CreateUserID))
                {
                    var user = OrganizationBusiness.GetUserByUserID(item.CreateUserID);
                    item.CreateUser = new Users() { Name = user.Name };
                }
            }
            JsonDictionary.Add("items", list);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetRoleByID(string id)
        {
            var model = OrganizationBusiness.GetRoleByID(id);
            JsonDictionary.Add("model", model);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveRole(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Role model = serializer.Deserialize<Role>(entity);

            if (string.IsNullOrEmpty(model.RoleID))
            {
                model.RoleID = new OrganizationBusiness().CreateRole(model.Name, model.ParentID, model.Description, CurrentUser.UserID, CurrentUser.AgentID, CurrentUser.ClientID);
            }
            else
            {
                bool bl = new OrganizationBusiness().UpdateRole(model.RoleID, model.Name, model.Description, CurrentUser.UserID, OperateIP);
                if (!bl)
                {
                    model.RoleID = "";
                }
            }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteRole(string roleid)
        {
            int result = 0;
            bool bl = new OrganizationBusiness().DeleteRole(roleid, CurrentUser.UserID, OperateIP, out result);
            JsonDictionary.Add("status", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateUserPwd(string userID, string loginPwd)
        {
            bool bl = OrganizationBusiness.UpdateUserPass(userID, loginPwd, CurrentUser.AgentID);
            JsonDictionary.Add("status", bl);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateMobilePhone(string userID)
        {
            bool flag = false;
            int result = 0;
            if (!string.IsNullOrEmpty(CurrentUser.LoginName))
            {
                Users item = WinWarBusiness.OrganizationBusiness.GetUserByUserID(userID);
                if (!string.IsNullOrEmpty(item.BindMobilePhone))
                {
                    flag = OrganizationBusiness.ClearAccountBindMobile(userID, CurrentUser.AgentID);
                    if (flag)
                    {
                        result = 1;
                        if (!string.IsNullOrEmpty(item.MobilePhone))
                        {
                            result = 2;
                        }
                    }
                }
                else
                {
                    result = 3;
                }
            }

            JsonDictionary.Add("result", result);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateUserBaseInfo(string entity, string userID)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(userID))
            {
                bool flag = false;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                WinWarEntity.Users newItem = serializer.Deserialize<WinWarEntity.Users>(entity);
                WinWarEntity.Users item = OrganizationBusiness.GetUserByUserID(userID);
                flag = OrganizationBusiness.UpdateUserInfo(userID, newItem.Name, item.Jobs, item.Birthday, item.Age.Value, newItem.DepartID,
                                                            newItem.Email, newItem.MobilePhone, item.OfficePhone, CurrentUser.AgentID);
                result = flag ? 1 : 0;
            }
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveRolePermission(string roleid, string permissions)
        {
            if (permissions.Length > 0)
            {
                permissions = permissions.Substring(0, permissions.Length - 1);

            }
            bool bl = new OrganizationBusiness().UpdateRolePermission(roleid, permissions, CurrentUser.UserID, OperateIP);
            JsonDictionary.Add("status", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveUser(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Users model = serializer.Deserialize<Users>(entity);

            int result = 0;

            var user = OrganizationBusiness.CreateUser(model.LoginName, model.LoginName, model.Name, model.MobilePhone, model.Email, model.CityCode, model.Address, model.Jobs, model.RoleID, CurrentUser.UserID, out result);

            JsonDictionary.Add("model", user);
            JsonDictionary.Add("result", result);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult IsExistLoginName(string loginname)
        {
            var bl = OrganizationBusiness.IsExistLoginName(loginname);
            JsonDictionary.Add("status", bl);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUsersByParentID(string parentid = "")
        {
            //var list = OrganizationBusiness.GetUsersByParentID(parentid, CurrentUser.AgentID).OrderBy(m => m.FirstName).ToList();
            JsonDictionary.Add("items", null);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult GetUsers(string filter)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            FilterUser model = serializer.Deserialize<FilterUser>(filter);
            int totalCount = 0;
            int pageCount = 0;

            var list = OrganizationBusiness.GetUsers(model.Keywords, model.DepartID, model.RoleID, PageSize, model.PageIndex, ref totalCount, ref pageCount);

            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("items", list);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserAll()
        {
            var list = OrganizationBusiness.GetUsers(CurrentUser.AgentID).Where(m => m.Status == 1).ToList();
            JsonDictionary.Add("items", list);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        public JsonResult DeleteUserByID(string userid)
        {
            int result = 0;
            bool bl = new OrganizationBusiness().DeleteUserByID(userid, CurrentUser.UserID, OperateIP, out result);

            JsonDictionary.Add("status", bl);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //编辑员工角色
        public JsonResult UpdateUserRole(string userid, string roleid)
        {
            bool bl = new OrganizationBusiness().UpdateUserRole(userid, roleid, CurrentUser.UserID, OperateIP);

            JsonDictionary.Add("status", bl);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ConfirmLoginPwd(string loginName, string loginPwd)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                loginName = CurrentUser.LoginName;
            }
            bool bl = OrganizationBusiness.ConfirmLoginPwd(loginName, loginPwd);
            JsonDictionary.Add("Result", bl);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateUserPass(string loginPwd)
        {
            bool bl = OrganizationBusiness.UpdateUserPass(CurrentUser.UserID, loginPwd, CurrentUser.AgentID);
            JsonDictionary.Add("Result", bl);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

    }
}
