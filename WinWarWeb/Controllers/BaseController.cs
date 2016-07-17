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
        public Dictionary<string, Object> jsonResult = new Dictionary<string, object>();

        public Passport currentPassport {
            get
            {
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
