using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WeiXin.Sdk
{
    public class AppConfig
    {
        public static string WeiXinApiUrl = ConfigurationManager.AppSettings["WeiXinApiUrl"] ?? "https://api.weixin.qq.com";
        public static string AppKey = ConfigurationManager.AppSettings["AppKey"] ?? "wx5ed9c76f98d78bc1";
        public static string AppSecret = ConfigurationManager.AppSettings["AppSecret"] ?? "681039cb005034260656ed75672bb8f5";
        public static string CallBackUrl = ConfigurationManager.AppSettings["CallBackUrl"] ?? "localhost:9999/User/WeiXinCallBack";
    }
}
