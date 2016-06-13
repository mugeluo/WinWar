using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinWarBusiness;

namespace WinWarWeb.Areas.Manage.Controllers
{
    public class NewsController : BaseController
    {
        //
        // GET: /Manage/News/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        #region ajax
        public JsonResult GetNews(string keywords, int typeID, long lastNewsCode, int pageSize)
        {
            var items = NewsBusiness.BaseBusiness.GetNews(keywords, typeID, pageSize, ref lastNewsCode);
            jsonResult.Add("items", items);
            jsonResult.Add("lastNewsCode", lastNewsCode);
            jsonResult.Add("totalCount",23);
            jsonResult.Add("pageCount", 3);
            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
