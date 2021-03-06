﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinWarWeb.Areas.Manage.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Manage/Home/
        public ActionResult Index()
        {
            if (Session["ClientManager"] == null) {
                return Redirect("/manage/home/Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["ClientManager"] = null;
            return Redirect("/manage/home/Login");
        }

        public JsonResult UserLogin(string userName, string pwd)
        {
            int result = 0;
            Dictionary<string, object> resultObj = new Dictionary<string, object>();
            string operateip = string.Empty;
            int outResult;
            WinWarEntity.Users model = WinWarBusiness.OrganizationBusiness.GetUserByUserName(userName, pwd, out outResult, operateip);
            if (model != null)
            {
                Session["ClientManager"] = model;
            }
            result = outResult;
            resultObj.Add("result", result);

            return new JsonResult
            {
                Data = resultObj,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
