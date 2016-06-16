using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WinWarBusiness;
using WinWarEntity;

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
        public JsonResult GetNews(string keywords, int typeID, int pageSize, int pageIndex)
        {
            int totalCount = 0;
            int pageCount = 0;
            var items = NewsBusiness.BaseBusiness.GetNews(keywords, typeID, pageSize, pageIndex,ref totalCount, ref pageCount);
            jsonResult.Add("items", items);
            jsonResult.Add("totalCount", totalCount);
            jsonResult.Add("pageCount", pageCount);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveNews(string news)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            NewsEntity model = serializer.Deserialize<NewsEntity>(news);
            bool flag = NewsBusiness.BaseBusiness.AddNews(model);
            jsonResult.Add("result", flag?1:0);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
