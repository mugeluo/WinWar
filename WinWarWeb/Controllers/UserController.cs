using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinWarBusiness;

namespace WinWarWeb.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["WinWarUser"] == null){
                return Redirect("/user/login");
            }

            ViewBag.Passport = currentPassport;
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            var authorizeUrl = WeiXin.Sdk.Token.GetAuthorizeUrl(Server.UrlEncode(WeiXin.Sdk.AppConfig.CallBackUrl), returnUrl, YXERP.Common.Common.IsMobileDevice());
            return Redirect(authorizeUrl);
        }

        public ActionResult WeiXinCallBack(string code, string state)
        {
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
                    if(user.UserID==0)
                    {
                        if(string.IsNullOrEmpty(passport.unionid))
                        {
                            passport = WeiXin.Sdk.Passport.GetUserInfo(token.access_token, openid);
                        }

                       PassportBusiness.BindWeiXinID(passport.nickname,passport.headimgurl,passport.unionid);
                       user = PassportBusiness.GetPassportByWeiXinID(unionid);
                    }

                    Session["WinWarUser"]=user;
                    if (!string.IsNullOrEmpty(state))
                    {
                        return Redirect(state);
                    }
                }
            }

            return Redirect("/Home/Index");
        }

        public ActionResult Logout()
        {
            Session["WinWarUser"] = null;

            return Redirect("/home/index");
        }

        #region ajax
        public JsonResult GetNewsFavorites(int pageSize, long lastFavoriteID)
        {
            if (Session["WinWarUser"] == null)
            {
                jsonResult.Add("result", -1);
            }
            else
            {
                var items = NewsBusiness.BaseBusiness.GetNewsFavorites(currentPassport.UserID, pageSize, ref lastFavoriteID);
                jsonResult.Add("items", items);
                jsonResult.Add("lastFavoriteID", lastFavoriteID);
                jsonResult.Add("result", -1);
            }

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
