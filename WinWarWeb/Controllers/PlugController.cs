using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinWarWeb.Controllers
{
    public class PlugController : BaseController
    {
        public JsonResult UploadFile()
        {
            string folder = "/Content/UploadFiles/";
            if (Request.Form.AllKeys.Contains("folder") && !string.IsNullOrEmpty(Request.Form["folder"]))
            {
                folder = Request.Form["folder"];
            }
            string uploadPath = HttpContext.Server.MapPath(folder);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            List<string> list = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (i == 10)
                {
                    break;
                }
                HttpPostedFileBase file = Request.Files[i];
                //判断图片类型
                string ContentType = file.ContentType;
                Dictionary<string, string> types = new Dictionary<string, string>();
                types.Add("image/x-png", "1");
                types.Add("image/png", "1");
                types.Add("image/gif", "1");
                types.Add("image/jpeg", "1");
                types.Add("image/tiff", "1");
                types.Add("application/x-MS-bmp", "1");
                types.Add("image/pjpeg", "1");
                if (!types.ContainsKey(ContentType))
                {
                    continue;
                }
                if (file.ContentLength > 1024 * 1024 * 5)
                {
                    continue;
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssms") + new Random().Next(1000, 9999).ToString() + "." + Path.GetExtension(file.FileName);
                string filePath = uploadPath + fileName;
                file.SaveAs(filePath);
                list.Add(folder + fileName);
            }
            jsonResult.Add("Items", list);

            return new JsonResult()
            {
                Data = jsonResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
