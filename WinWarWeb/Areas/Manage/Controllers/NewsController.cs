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

        public ActionResult Edit(long id)
        {
            var news= NewsBusiness.BaseBusiness.GetNewsDetail(id, 0);
            ViewBag.News = news;

            return View();
        }
        #region ajax
        public JsonResult GetNews(string keywords, int typeID,int publishStatus, int pageSize, int pageIndex)
        {
            int totalCount = 0;
            int pageCount = 0;
            var items = NewsBusiness.BaseBusiness.GetNews(keywords, typeID,publishStatus,
                pageSize, pageIndex,ref totalCount, ref pageCount);
            jsonResult.Add("items", items);
            jsonResult.Add("totalCount", totalCount);
            jsonResult.Add("pageCount", pageCount);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [ValidateInput(false)]
        public JsonResult SaveNews(string news)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            NewsEntity model = serializer.Deserialize<NewsEntity>(news);
            bool flag = false;
            if (model.News_Uni_Code == 0)
            {
                flag = NewsBusiness.BaseBusiness.AddNews(model);
            }
            else
            {
                flag = NewsBusiness.BaseBusiness.EditNews(model);
            }
            jsonResult.Add("result", flag?1:0);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PublishNews(long id, int isPublish)
        {
           bool flag= NewsBusiness.BaseBusiness.PublishNews(id, isPublish);
           jsonResult.Add("result",flag?1:0);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
