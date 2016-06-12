using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinWarWeb.Areas.Manage.Controllers
{
    public class NewsController : Controller
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
    }
}
