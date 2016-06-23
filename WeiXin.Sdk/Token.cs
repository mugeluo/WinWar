using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace WeiXin.Sdk
{
    public class Token
    {
        public static string GetAuthorizeUrl(string redirect_uri){
            return string.Format("https://open.weixin.qq.com/connect/qrconnect?appid={0}&redirect_uri={1}&response_type={2}&scope={3}",
                "wx5ed9c76f98d78bc1", redirect_uri, "code", "snsapi_login");
        }


    }
}
