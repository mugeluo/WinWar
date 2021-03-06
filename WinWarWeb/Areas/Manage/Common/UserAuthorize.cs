﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WinWarEntity;
using WinWarBusiness;

namespace WinWarWeb.Manage.Common
{
    public class UserAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["ClientManager"] == null)
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }
            else
            {
                //if (user.LogGUID != OrganizationBusiness.GetUserByUserID(user.UserID, user.AgentID).LogGUID)
                //{
                //    httpContext.Response.StatusCode = 402;
                //    return false;
                //}
            }
            return true;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Response.StatusCode == 401)
            {
                filterContext.Result = new RedirectResult("/Manage/Home/Login?ReturnUrl=" + HttpContext.Current.Request.Url);
                return;
            }


            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            var action = filterContext.ActionDescriptor.ActionName.ToLower();

            var currentUser = (WinWarEntity.Users)filterContext.HttpContext.Session["ClientManager"];

            var menu = CommonBusiness.ClientMenus.Where(m => m.Controller.ToLower() == controller && m.View.ToLower() == action).FirstOrDefault();

            //需要判断权限
            if (menu != null && menu.IsLimit == 1)
            {
                WinWarEntity.Users user = (WinWarEntity.Users)filterContext.HttpContext.Session["ClientManager"];
                if (user.Menus.Where(m => m.MenuCode == menu.MenuCode).Count() <= 0)
                {

                    if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                    {
                        Dictionary<string, string> result = new Dictionary<string, string>();
                        result.Add("result", "10001");
                        filterContext.Result = new JsonResult()
                        {
                            Data = result,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        var urlRequest = filterContext.RequestContext.HttpContext.Request;
                        throw new HttpException(403, urlRequest.UrlReferrer != null ? urlRequest.UrlReferrer.AbsoluteUri : urlRequest.Url.AbsoluteUri);
                        //filterContext.RequestContext.HttpContext.Response.Write("<script>alert('您没有权限访问此页面');history.back();</script>");
                        //filterContext.RequestContext.HttpContext.Response.End();
                    }
                }
            }

        }
    }
}