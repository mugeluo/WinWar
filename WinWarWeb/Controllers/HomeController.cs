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

        public ActionResult Index(string  id)
        {
            ViewBag.ID = id ?? "16";
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }


        #region ajax
        public JsonResult GetNewsTypeByParentID(int id) {
            var items = NewsBusiness.BaseBusiness.GetNewsTypeByParentID(id);
            jsonResult.Add("items", items);

            return new JsonResult() { 
                Data=jsonResult,
                JsonRequestBehavior=JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetNews(string keywords, int typeID, long lastNewsCode)
        {
            var items = NewsBusiness.BaseBusiness.GetNews(keywords, typeID, pageSize, 0, ref lastNewsCode);
            jsonResult.Add("items", items);
            jsonResult.Add("lastNewsCode", lastNewsCode);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        #endregion
    }
}
