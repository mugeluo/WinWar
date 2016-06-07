using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinWarWeb.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public int pageSize = 10;

        public Dictionary<string, Object> jsonResult = new Dictionary<string, object>();

    }
}
