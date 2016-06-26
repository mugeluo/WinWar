using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace WeiXin.Sdk
{
    public class Token
    {
        public static string GetAuthorizeUrl(string redirect_uri,string returnUrl){
            string url= string.Format("https://open.weixin.qq.com/connect/qrconnect?appid={0}&redirect_uri={1}&response_type={2}&scope={3}",
                AppConfig.AppKey, redirect_uri, "code", "snsapi_login");

            if (!string.IsNullOrEmpty(returnUrl)) {
                url += "&state="+returnUrl;
            }
            return url;
        }

        public static TokenEntity GetAccessToken(string code)
        {

            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras.Add("appid",AppConfig.AppKey);
            paras.Add("secret", AppConfig.AppSecret);
            paras.Add("code", code);
            paras.Add("grant_type", "authorization_code");

            var result = HttpRequest.RequestServer(ApiOption.access_token, paras, RequestType.Post);
            return JsonConvert.DeserializeObject<TokenEntity>(result);
        }

    }
}
