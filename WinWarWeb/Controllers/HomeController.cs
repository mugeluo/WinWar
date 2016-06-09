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

        int userid = 111111;
        public ActionResult Index(string  id)
        {
            ViewBag.ID = id ?? "16";
            return View();
        }

        public ActionResult Detail(long id=0)
        {
            if (id == 0)
            {
               return Redirect("/Home/Index");
            }

            var item = NewsBusiness.BaseBusiness.GetNewsDetail(id, userid);
            ViewBag.Item = item;

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
            var items = NewsBusiness.BaseBusiness.GetNews(keywords, typeID, pageSize, userid, ref lastNewsCode);
            jsonResult.Add("items", items);
            jsonResult.Add("lastNewsCode", lastNewsCode);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetNewsComments(long id)
        {
            long lastCommentID = 0;
            var items = NewsBusiness.BaseBusiness.GetNewsComments(id, pageSize, userid, ref lastCommentID);
            jsonResult.Add("items", items);
            jsonResult.Add("lastCommentID", lastCommentID);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddNewsComment(long id, string content)
        {
            bool flag = NewsBusiness.BaseBusiness.AddNewsComment(content, id, userid, "", 0, 0, "");
            jsonResult.Add("result", flag?1:0);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddNewsCollectCount(long id, int isAdd)
        {
            bool flag = NewsBusiness.BaseBusiness.AddNewsCollectCount(id, isAdd==1?true:false, userid);
            jsonResult.Add("result", flag ? 1 : 0);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddNewsPraiseCount(long id, int isAdd)
        {
            bool flag = NewsBusiness.BaseBusiness.AddNewsPraiseCount(id, isAdd == 1 ? true : false,userid);
            jsonResult.Add("result", flag ? 1 : 0);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
