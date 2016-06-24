﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinWarBusiness;

namespace WinWarWeb.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/
        long userid = 111111;
        public ActionResult Index()
        {
            //return Redirect("/User/Login");

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.AuthorizeUrl = WeiXin.Sdk.Token.GetAuthorizeUrl(Server.UrlEncode(WeiXin.Sdk.AppConfig.CallBackUrl));
            
            return View();
        }

        //微信登录
        public ActionResult WeiXinCallBack(string code)
        {
            //string token="raYxos81Hp3Qp7zfyiqA97vLawpOULs757jUQ646y3BKqvANNEmODU2XyaI224uxxeaQsF5Y-OqFrxX4FtebBvGtees4tNVbrXW19B-H2MM";
            //string id="oqjcAxOaH4nsl9iaJomIG2f9r7Qk";
            if (!string.IsNullOrEmpty(code))
            {
                var token = WeiXin.Sdk.Token.GetAccessToken(code);
                if (!string.IsNullOrEmpty(token.access_token))
                {
                    string unionid = token.unionid??string.Empty;
                    string openid = token.openid;
                    var passport=new WeiXin.Sdk.PassportEntity();

                    if (string.IsNullOrEmpty(unionid))
                    {
                        passport = WeiXin.Sdk.Passport.GetUserInfo(token.access_token, openid);
                        unionid=passport.unionid;
                    }

                    WinWarEntity.Passport user=PassportBusiness.GetPassportByWeiXinID(unionid);
                    if(string.IsNullOrEmpty(user.UserID))
                    {
                        if(string.IsNullOrEmpty(passport.unionid))
                        {
                            passport = WeiXin.Sdk.Passport.GetUserInfo(token.access_token, openid);
                        }

                       PassportBusiness.BindWeiXinID(passport.nickname,passport.headimgurl,passport.unionid);
                       user = PassportBusiness.GetPassportByWeiXinID(unionid);
                    }

                    Session["WinWarUser"]=user;
                    return Redirect("/User/Index");
                }
            }

            return Redirect("/Home/Index");
        }
        #region ajax
        public JsonResult GetNewsFavorites(int pageSize, long lastFavoriteID)
        {
            var items = NewsBusiness.BaseBusiness.GetNewsFavorites(userid, pageSize, ref lastFavoriteID);
            jsonResult.Add("items", items);
            jsonResult.Add("lastFavoriteID", lastFavoriteID);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
