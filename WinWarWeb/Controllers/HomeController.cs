using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WinWarBusiness;
namespace WinWarWeb.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var NewsTypes = NewsBusiness.BaseBusiness.GetNewsTypeByParentCode(16);
            ViewBag.NewsTypes = NewsTypes;

            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }


        #region ajax
        public JsonResult GetNewsTypeByParentCode(int code) {
            var items = NewsBusiness.BaseBusiness.GetNewsTypeByParentCode(code);
            jsonResult.Add("items", items);

            return new JsonResult() { 
                Data=jsonResult,
                JsonRequestBehavior=JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetNews()
        {
            long code=0;
            var items = NewsBusiness.BaseBusiness.GetNews(string.Empty, 0, pageSize, 0, ref code);
            jsonResult.Add("items", items);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
