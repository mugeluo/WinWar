using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace YXERP.Common
{
    public class Common
    {
        //云销客户端的ClientID、AgentID
        public static string YXClientID = System.Configuration.ConfigurationManager.AppSettings["YXClientID"] ?? string.Empty;
        public static string YXAgentID = System.Configuration.ConfigurationManager.AppSettings["YXAgentID"] ?? string.Empty;

        //支付宝对接页面
        public static string AlipaySuccessPage = System.Configuration.ConfigurationManager.AppSettings["AlipaySuccessPage"] ?? string.Empty;
        public static string AlipayNotifyPage = System.Configuration.ConfigurationManager.AppSettings["AlipayNotifyPage"] ?? string.Empty;

       /// <summary>
       /// 获取请求方ip
       /// </summary>
       /// <param name="request"></param>
       /// <returns></returns>
        public static string GetRequestIP()
        {
            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers.Get("X-Real-IP")) ? System.Web.HttpContext.Current.Request.UserHostAddress : System.Web.HttpContext.Current.Request.Headers["X-Real-IP"];
        }

        /// <summary>
        /// 写支付宝文本日志
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool WriteAlipayLog(string content) {
            try
            {
                string path = HttpContext.Current.Server.MapPath(@"C:\WebLog\Alipay");
                //string path = HttpContext.Current.Server.MapPath("~/Log/Alipay");
                if (!Directory.Exists(path))//判断是否有该文件  
                    Directory.CreateDirectory(path);
                string logFileName = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";//生成日志文件  
                if (!File.Exists(logFileName))//判断日志文件是否为当天  
                    File.Create(logFileName);//创建文件  
                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流  
                writer.WriteLine(DateTime.Now.ToString() + " " + content);
                writer.Flush();
                writer.Dispose();
                writer.Close();
                
                return true;
            }
            catch
            {
                return false;
            };
        }

        /// <summary>
        /// 存入手机验证码会话
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="code"></param>
        public static void SetCodeSession(string mobilePhone, string code) 
        {
            HttpContext.Current.Session[mobilePhone] = code;
        }

        /// <summary>
        /// 验证手机验证码
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool ValidateMobilePhoneCode(string mobilePhone, string code)
        {
            if (HttpContext.Current.Session[mobilePhone] != null)
            {
              return  HttpContext.Current.Session[mobilePhone].ToString() == code;
            }

            return false;
        }

        /// <summary>
        /// 清除手机验证码会话
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static void ClearMobilePhoneCode(string mobilePhone)
        {
            if (HttpContext.Current.Session[mobilePhone] != null)
            {
                 HttpContext.Current.Session.Remove(mobilePhone);
            }
        }

        /// <summary>
        /// 获取今天是星期几
        /// </summary>
        /// <param name="msg">显示周日还是星期日</param>
        /// <returns></returns>
        public static string Week(string msg, int day)
        {
            if (string.IsNullOrEmpty(msg)) {
                msg = "星期";
            }
            string[] weekdays = { "" + msg + "日", "" + msg + "一", "" + msg + "二", "" + msg + "三", "" + msg + "四", "" + msg + "五", "" + msg + "六" };
            string week = weekdays[day];
            return week;
        }

         public static bool IsMobileDevice()
        {
            string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "netfront", "java", "opera mobi", 
                                        "opera mini", "ucweb", "windows ce", "symbian", "series","webos", "sony", "blackberry", "dopod", "nokia",
                                        "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma","docomo", 
                                        "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith",
                                        "palmsource", "alcatel","amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", 
                                        "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", 
                                        "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220",  "w3c ", "acs-", "alav", 
                                        "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco",
                                        "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", 
                                        "maxo", "midp", "mits","mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", 
                                        "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-",
            "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo","teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda",
            "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "Googlebot-Mobile" };
            bool isMoblie = false;
            if (System.Web.HttpContext.Current.Request.UserAgent.ToString().ToLower() != null)
            {
                for (int i = 0; i < mobileAgents.Length; i++)
                {
                    if (System.Web.HttpContext.Current.Request.UserAgent.ToString().ToLower().IndexOf(mobileAgents[i]) >= 0)
                    {
                        isMoblie = true;
                        break;
                    }
                }
            }
            if (isMoblie)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 缓存

        #region 用户登录密码错误缓存
        private static Dictionary<string, PwdErrorUserEntity> _cachePwdErrorUsers;
        public static Dictionary<string, PwdErrorUserEntity> CachePwdErrorUsers
        {
            set { _cachePwdErrorUsers = value; }

            get { 

                if(_cachePwdErrorUsers==null)
                {
                    _cachePwdErrorUsers= new Dictionary<string, PwdErrorUserEntity>();
                }

                return _cachePwdErrorUsers;
            }
        }

        #endregion


        #endregion
    }

    public class PwdErrorUserEntity
    {
        public int ErrorCount;
        public DateTime ForbidTime;
    }
}