using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WinWarBusiness;
using WinWarEntity;
namespace WinWarWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string  id)
        {
            if (currentPassport.UserID == 0)
            {
                var authorizeUrl = WeiXin.Sdk.Token.GetAuthorizeUrl(Server.UrlEncode(WeiXin.Sdk.AppConfig.CallBackUrl), string.Empty, YXERP.Common.Common.IsMobileDevice());
                return Redirect(authorizeUrl);
            }
            else
            {
                ViewBag.ID = id ?? "6";
                ViewBag.Passport = currentPassport;
            }

            return View();
        }

        public ActionResult Detail(long id=0)
        {
            if (id == 0)
            {
               return Redirect("/Home/Index");
            }

            var item = NewsBusiness.BaseBusiness.GetNewsDetail(id, currentPassport.UserID);
            ViewBag.Item = item;
            ViewBag.Passport = currentPassport;

            return View();
        }

        public ActionResult NewsDetail()
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

        public JsonResult GetNews(string keywords, int typeID, long lastNewsCode, int pageSize)
        {
            var items = NewsBusiness.BaseBusiness.GetNews(keywords, typeID, pageSize, currentPassport.UserID, ref lastNewsCode);
            jsonResult.Add("items", items);
            jsonResult.Add("lastNewsCode", lastNewsCode);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetNewsDetail(long id)
        {
            var item = NewsBusiness.BaseBusiness.GetNewsDetail(id, currentPassport.UserID);
            jsonResult.Add("item",item);
            jsonResult.Add("passport",currentPassport);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetNewsComments(long id,long lastCommentID,int pageSize)
        {
            var items = NewsBusiness.BaseBusiness.GetNewsComments(id, pageSize, currentPassport.UserID, ref lastCommentID);
            jsonResult.Add("items", items);
            jsonResult.Add("lastCommentID", lastCommentID);
            jsonResult.Add("result", 1);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddNewsComment(string comment)
        {
            if (Session["WinWarUser"] == null)
            {
                jsonResult.Add("result", -1);
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                NewsCommentEntity model = serializer.Deserialize<NewsCommentEntity>(comment);

                bool flag = NewsBusiness.BaseBusiness.AddNewsComment(model.Content, model.News_Uni_Code, currentPassport.UserID, currentPassport.Name,
                    model.Reply_ID, model.Reply_User_ID, model.Reply_User_Name);
                jsonResult.Add("result", flag ? 1 : 0);

                if (flag)
                {
                    List<NewsCommentEntity> items = new List<NewsCommentEntity>();
                    model.Create_Date = DateTime.Now;
                    model.Reply_Count = 1;
                    model.User_Name = currentPassport.Name;
                    model.CreateUser = currentPassport;
                    items.Add(model);
                    jsonResult.Add("items", items);
                }
            }

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddNewsCollectCount(long id, int isAdd)
        {
            if (Session["WinWarUser"] == null)
            {
                jsonResult.Add("result", -1);
            }
            else
            {
                bool flag = NewsBusiness.BaseBusiness.AddNewsCollectCount(id, isAdd==1?true:false, currentPassport.UserID);
                jsonResult.Add("result", flag ? 1 : 0);  
            }

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddNewsPraiseCount(long id, int isAdd)
        {
            if (Session["WinWarUser"] == null)
            {
                jsonResult.Add("result", -1);
            }
            else
            {
                bool flag = NewsBusiness.BaseBusiness.AddNewsPraiseCount(id, isAdd == 1 ? true : false, currentPassport.UserID);
                jsonResult.Add("result", flag ? 1 : 0);
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
