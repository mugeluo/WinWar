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
            return View();
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
