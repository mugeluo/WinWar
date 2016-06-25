using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinWarEntity;
namespace WinWarWeb.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        //public int pageSize = 15;

        public Dictionary<string, Object> jsonResult = new Dictionary<string, object>();

        public Passport currentPassport {
            get {
                if (Session["WinWarUser"] == null)
                {
                    return new Passport();
                }
                else {
                    return (Passport)Session["WinWarUser"];
                }
            }
        }

    }
}
