using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WeChatTools.Core;

namespace WeChatTools.Web.dev
{
    /// <summary>
    /// urlTransfer 的摘要说明
    /// </summary>
    public class urlTransfer : IHttpHandler
    {
        private string wxCheckApi = "https://api.kuweimi.com/pro/wxUrlCheck.ashx";//微信域名检测api
        private string wxCheckApiKey = "341e0b5df120394ec99e517b67774399";//微信域名检测授权key

        private string gotoRedirectUrl = "http://weixin.sogou.com/";//最终用户访问的网址
        public void ProcessRequest(HttpContext context)
        {
            //http://localhost:2180/dev/urlTransfer.ashx?domain=www.bbb.com&url=http%3a%2f%2fwww.bbb.com%2findex.php%3fg%3dWap%26m%3dVote%26a%3dindex%26token%3duDSrEHNs9CFGcTSC%26wecha_id%3docMqvwRjzPH9eseHRc_Z9nlP-DSM%26id%3d25%26iMicms%3dmp.weixin.qq.com
            //http://www.aaa.com/dev/urlTransfer.ashx?url=http://wwww.bbb.com/index.php?g=Wap&m=Vote&a=index&token=uDSrEHNs9CFGcTSC&wecha_id=ocMqvwRjzPH9eseHRc_Z9nlP-DSM&id=25&iMicms=mp.weixin.qq.com 
            context.Response.ContentType = "text/plain";
            string getUrl = QueryString("url");//参数1：用户传当前推广的网址 
            string getDomain = QueryString("domain");//参数2：用户传当前推广的域名(也是getUrl网址里面的域名，主要是分割getUrl用的)

            string[] sArray = Regex.Split(getUrl, getDomain, RegexOptions.IgnoreCase);
            string domainLeft = sArray[0];
            string domainRight = sArray[1];
            string domainCenter = GetRandHostUrl();
            if (!string.IsNullOrEmpty(domainCenter))
            {
                gotoRedirectUrl = domainLeft + domainCenter + domainRight;
            }

            context.Response.Redirect(gotoRedirectUrl);
           //  context.Response.Write(gotoRedirectUrl);
            context.Response.End();
        }



        private string GetRandHostUrl()
        {
            Random ran = new Random();
            int RandKey = ran.Next(4, 9);
            string randUrl = "";

            bool isBlacklist = true;
           // int xx = 0;//没有做剔除操作，暂时限制循环次数。
            while (isBlacklist)
            {
                try
                {
                    string hosturl = ConfigTool.ReadVerifyConfig("Host", "HostUrl");//这些域名都需要指向用户最终要访问的站点
                    string[] sArray = hosturl.Split(',');
                    Random ran1 = new Random();
                    int RandKey1 = ran.Next(0, sArray.Length);//随机选中域名


                    randUrl = GenerateRandomNumber(RandKey);//随机二级域名

                    if (string.IsNullOrEmpty(sArray[RandKey1]))
                    {
                        randUrl = "";
                        isBlacklist = false;
                        return randUrl;
                    }
                    else
                    {
                        randUrl = randUrl + "." + sArray[RandKey1] + "";
                    }

                    wxCheckApi = ConfigTool.ReadVerifyConfig("wxCheckApi", "WeiXin"); ;
                    wxCheckApiKey = ConfigTool.ReadVerifyConfig("wxCheckApiKey", "WeiXin");

                    WebRequest wr = (HttpWebRequest)WebRequest.Create(wxCheckApi + "?key=" + wxCheckApiKey + "&url=http://" + randUrl);
                    var stream = wr.GetResponse().GetResponseStream();
                    var sr = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                    var all = sr.ReadToEnd();
                    sr.Close();
                    stream.Close();
                    //读取网站的数据
                    if (all.Contains("屏蔽"))
                    {
                        //剔除域名
                        if (hosturl.Contains(sArray[RandKey1] + ","))
                        {
                            hosturl = hosturl.Replace(sArray[RandKey1] + ",", "");
                        }
                        if (hosturl.Contains("," + sArray[RandKey1]))
                        {
                            hosturl = hosturl.Replace("," + sArray[RandKey1], "");
                        }
                        ConfigTool.WriteVerifyConfig("Host", hosturl, "HostUrl");//剔除黑名单域名
                    }
                    else
                    {
                        isBlacklist = false;
                        return randUrl;
                    }
                    
                   
                    

                }
                catch (Exception ex)
                {
                    randUrl = "";
                    //randUrl = ex.Message.ToString();
                    return randUrl;
                }
            }
            return randUrl;
        }


        private static char[] constant =   
         {   
            '0','1','2','3','4','5','6','7','8','9',  
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'   
         };

        private static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(36);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(36)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// url请求里的参数
        /// 过滤非法字符
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string QueryString(string param)
        {
            if (HttpContext.Current.Request[param] == null || HttpContext.Current.Request[param].ToString().Trim() == "")
            {
                return "";
            }
            string ret = HttpContext.Current.Request[param].ToString().Trim();
            ret = ret.Replace(",", "");
            ret = ret.Replace("'", "");
            ret = ret.Replace(";", "");

            return ret;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}