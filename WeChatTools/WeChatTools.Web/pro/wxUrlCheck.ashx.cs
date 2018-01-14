using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WeChatTools.Core;

namespace WeChatTools.Web
{
    /// <summary>
    /// 微信域名检测工具--公共测试
    /// </summary>
    public class WXUrlCheck : IHttpHandler
    {
        private const int DURATION = 24 * 60;
        private static string userIP = "127.0.0.1";
        string userKey = "341e0b5df120394ec99e517b67774399";
        private TimeSpan _strWorkingDayAM = DateTime.Parse("09:00").TimeOfDay;//工作时间上午08:00
        private TimeSpan _strWorkingDayPM = DateTime.Parse("17:00").TimeOfDay;
        public void ProcessRequest(HttpContext context)
        {
            userIP = GetWebClientIp(context);
            context.Response.ContentType = "text/plain";
            TimeSpan dspNow = DateTime.Now.TimeOfDay;
            string result = string.Empty;
            //   if (IsInTimeInterval(dspNow, _strWorkingDayAM, _strWorkingDayPM))
            //   {
            if (!IsValid(context))
            {
                result = "{\"State\":false,\"Data\":\"" + userIP + "\",\"Msg\":\"当天请求上限,请明天再试,需要讨论技术，进群交流 QQ群:316344705!\"}";
            }
            else
            {
                if (!string.IsNullOrEmpty(context.Request["url"]))
                {

                    if (!string.IsNullOrEmpty(context.Request["key"]) && context.Request["key"].Length == 32)
                    {
                        userKey = context.Request["key"]; //key ,md5值
                    }

                    //需要检测的网址
                    string urlCheck = context.Request["url"]; //检测的值
                    urlCheck = urlCheck.Replace("https://", "").Replace("http://", "");
                    string json = "{\"Mode\":\"WXCheckUrl\",\"Param\":\"{\'CheckUrl\':\'" + urlCheck + "\',\'UserKey\':\'" + userKey + "\'}\"}";

                    ServiceApiClient SpVoiceObj = new ServiceApiClient("NetTcpBinding_IServiceApi");
                    SpVoiceObj.Open();
                    result = SpVoiceObj.Api(json);
                    SpVoiceObj.Close();
                    /*
                    Logger.WriteLoggger(urlCheck + ":HTTP_CDN_SRC_IP--" + context.Request.ServerVariables["HTTP_CDN_SRC_IP"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":HTTP_Cdn-Src-Ip--" + context.Request.ServerVariables["HTTP_Cdn-Src-Ip"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":Cdn-Src-Ip--" + context.Request.ServerVariables["Cdn-Src-Ip"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":HTTP_X_FORWARDED_FOR--" + context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":Proxy-Client-IP--" + context.Request.ServerVariables["Proxy-Client-IP"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":WL-Proxy-Client-IP--" + context.Request.ServerVariables["WL-Proxy-Client-IP"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":HTTP_CLIENT_IP--" + context.Request.ServerVariables["HTTP_CLIENT_IP"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":HTTP_VIA--" + context.Request.ServerVariables["HTTP_VIA"] + ":" + result);
                    Logger.WriteLoggger(urlCheck + ":REMOTE_ADDR--" + context.Request.ServerVariables["REMOTE_ADDR"] + ":" + result);
                    Logger.WriteLoggger("==================================================");
                    */
                    Logger.WriteLoggger(userIP + ":" + userKey + ":" + result);


                }
                else
                {
                    result = "{\"State\":false,\"Data\":\"" + userIP + "\",\"Msg\":\"参数错误,进qq群交流:316344705！\"}";

                }
            }
            /*   }
               else
               {
                   context.Response.Write(userIP + ":测试接口,请在每天(09:00-17:00)时间段进行测试,需要讨论技术,进群交流 QQ群:316344705");

               } */
            if (!string.IsNullOrEmpty(context.Request.QueryString["callback"]))
            {
                string callBack = context.Request.QueryString["callback"].ToString(); //回调

                result = callBack + "(" + result + ")";
            }
            context.Response.Write(result);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt()
        {
            System.DateTime time = DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        private bool IsInTimeInterval(TimeSpan time, TimeSpan startTime, TimeSpan endTime)
        {
            //判断时间段开始时间是否小于时间段结束时间，如果不是就交换
            if (startTime > endTime)
            {
                TimeSpan tempTime = startTime;
                startTime = endTime;
                endTime = tempTime;
            }

            if (time > startTime && time < endTime)
            {
                return true;
            }
            return false;
        }

        /// <summary>  
        /// 获取外网ip地址  
        /// </summary>  
        public static string GetExtenalIpAddress()
        {
            String url = "http://city.ip138.com/ip2city.asp";
            string IP = "未获取到外网ip";
            try
            {
                //从网址中获取本机ip数据    
                System.Net.WebClient client = new System.Net.WebClient();
                client.Encoding = System.Text.Encoding.Default;
                string str = client.DownloadString(url);
                client.Dispose();

                //提取外网ip数据 [218.104.71.178]    
                int i1 = str.IndexOf("["), i2 = str.IndexOf("]");

                if (!str.Equals("")) IP = str.Substring(i1 + 1, i2 - 1 - i1);
                else IP = GetExtenalIpAddress_0();
            }
            catch (Exception) { }

            return IP;
        }

        /// <summary>  
        /// 获取外网ip地址  
        /// </summary>  
        public static string GetExtenalIpAddress_0()
        {
            var tempIp = "";
            try
            {
                WebRequest wr = (HttpWebRequest)WebRequest.Create("http://ip.chinaz.com/getip.aspx");
                var stream = wr.GetResponse().GetResponseStream();
                var sr = new StreamReader(stream, Encoding.GetEncoding("gb2312"));
                var all = sr.ReadToEnd();
                //读取网站的数据
                int start = all.IndexOf("[") + 1;
                int end = all.IndexOf("]", start);
                tempIp = all.Substring(start, end - start);
                sr.Close();
                stream.Close();
            }
            catch
            {
                // ignored
            }
            return tempIp;
        }


        public static string GetWebClientIp(HttpContext httpContext)
        {
            string customerIP = "127.0.0.1";

            if (httpContext == null || httpContext.Request == null || httpContext.Request.ServerVariables == null) return customerIP;

            customerIP = httpContext.Request.ServerVariables["HTTP_CDN_SRC_IP"];

            if (String.IsNullOrWhiteSpace(customerIP) || "unknown".Equals(customerIP.ToLower()))
            {
             
                customerIP = httpContext.Request.ServerVariables["Proxy-Client-IP"];
            }
            if (String.IsNullOrWhiteSpace(customerIP) || "unknown".Equals(customerIP.ToLower()))
            {
              
                customerIP = httpContext.Request.ServerVariables["WL-Proxy-Client-IP"];
            }

            if (String.IsNullOrWhiteSpace(customerIP) || "unknown".Equals(customerIP.ToLower()))
            {
             
                customerIP = httpContext.Request.ServerVariables["HTTP_VIA"];
            }

            if (String.IsNullOrWhiteSpace(customerIP))
            {
               
                customerIP = httpContext.Request.ServerVariables["HTTP_CLIENT_IP"];
                if (!String.IsNullOrWhiteSpace(customerIP) && customerIP.Contains(","))
                {
                    customerIP = customerIP.Split(new char[] { ',' })[0];
                }
            }

            if (String.IsNullOrWhiteSpace(customerIP) || "unknown".Equals(customerIP.ToLower()))
            {
                
                customerIP = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrWhiteSpace(customerIP) && customerIP.Contains(","))
                {
                    customerIP = customerIP.Split(new char[] { ',' })[0];
                }
            }
            else
            {
                
                customerIP = httpContext.Request.ServerVariables["REMOTE_ADDR"];

            }
            
            if (!IsIP(customerIP))
            {
                customerIP = "127.0.0.1";
            }
            return customerIP;
        }

        public static string GetRealIP(string CustomerIP)
        {
            string result = String.Empty;

            result = CustomerIP;

            //可能有代理   
            if (!string.IsNullOrWhiteSpace(result))
            {
                //没有"." 肯定是非IP格式  
                if (result.IndexOf(".") == -1)
                {
                    result = null;
                }
                else
                {
                    //有","，估计多个代理。取第一个不是内网的IP。  
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);

                        string[] temparyip = result.Split(",;".ToCharArray());

                        if (temparyip != null && temparyip.Length > 0)
                        {
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                //找到不是内网的地址  
                                if (IsIP(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];
                                }
                            }
                        }
                    }
                    //代理即是IP格式  
                    else if (IsIP(result))
                    {
                        return result;
                    }
                    //代理中的内容非IP  
                    else
                    {
                        result = "";
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        public static bool IsValid(HttpContext context)
        {
            if (context.Request.Browser.Crawler) return false;
            string key = userIP;

            int hit = (Int32)(context.Cache[key] ?? 0);
            if (hit > 14) return false;
            else hit++;

            if (hit == 1)
            {
                context.Cache.Add(key, hit, null, DateTime.Now.AddMinutes(DURATION), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

            }
            else
            {
                context.Cache[key] = hit;
            }
            return true;
        }


    }
}