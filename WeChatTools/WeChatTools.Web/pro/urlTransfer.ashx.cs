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
    /// 域名中转工具,随机切换域名，保证活动正常进行
    /// </summary>
    public class urlTransfer : IHttpHandler
    {
        string pToken = "uDSrEHNs9CFGcTSC";
        string pWecha_id = "ocMqvwRjzPH9eseHRc_Z9nlP-DSM";
        string pId = "25";
        string iMicms = "mp.weixin.qq.com";
        string hmdOpenid = "";
        string stateUrl = "";
        string redirectUrl = "http://weixin.sogou.com/";
        private ServiceApi _service = null;
        public void ProcessRequest(HttpContext context)
        {
            //GET /index.php?g=Wap&m=Vote&a=index&token=uDSrEHNs9CFGcTSC&wecha_id=ocMqvwRjzPH9eseHRc_Z9nlP-DSM&id=25&iMicms=mp.weixin.qq.com HTTP/1.1
            context.Response.ContentType = "text/plain";
            pToken = QueryString("token");
            pWecha_id = QueryString("wecha_id");
            pId = QueryString("id");
            iMicms = QueryString("iMicms");

            stateUrl = ConfigTool.ReadVerifyConfig("state", "HostUrl");

            if (stateUrl.Equals("true"))
            {
                string tokens = ConfigTool.ReadVerifyConfig("tokens", "HostUrl");
                if (tokens.Contains(pToken))
                {
                    string pRedirectUrl = "http://" + GetRandHostUrl() + "/index.php?g=Wap&m=Vote&a=index&id=" + pId + "&token=" + pToken + "&wecha_id=" + pWecha_id + "&iMicms=" + iMicms;

                    string json = "{\"Mode\": \"WXCheckUrl\", \"Param\": \"{\'CheckUrl\':\'" + pRedirectUrl + "\'}\"}";
                    _service = new ServiceApi();
                    string resultCheck = _service.Api(json);

                    if (!resultCheck.Contains("屏蔽"))
                    {
                        //域名未被微信封号
                        hmdOpenid = ConfigTool.ReadVerifyConfig("hmdOpenid", "HostUrl");
                        if (!hmdOpenid.Contains(pWecha_id))
                        {
                            //当前访问的用户不在黑名单
                            redirectUrl = pRedirectUrl;
                            Logger.WriteLoggger("openid:" + pWecha_id);  //后续可以扩展，可以屏蔽一些恶意投诉的微信号
                        }
                    }
                    else
                    {
                       
                        //修改投票状态
                        ConfigTool.WriteVerifyConfig("state", "false", "HostUrl");
                        Logger.WriteLoggger("恶意的openid:" + pWecha_id);  //后续可以扩展，可以屏蔽一些恶意投诉的微信号
                    }
                }

            }

            context.Response.Redirect(redirectUrl);
            // context.Response.Write(redirectUrl);
            context.Response.End();
        }

        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
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

        private string GetRandHostUrl()
        {
            Random ran = new Random();
            int RandKey = ran.Next(4, 9);
            string randUrl = "";

            try
            {
                string hosturl = ConfigTool.ReadVerifyConfig("Host", "HostUrl");
                string[] sArray = hosturl.Split(',');
                Random ran1 = new Random();
                int RandKey1 = ran.Next(0, sArray.Length);//随机选中域名


                randUrl = GenerateRandomNumber(RandKey);

                if (string.IsNullOrEmpty(sArray[RandKey1]))
                {
                    randUrl = randUrl + "." + ConfigTool.ReadVerifyConfig("exp", "HostUrl");
                }
                else
                {
                    randUrl = randUrl + "." + sArray[RandKey1] + "";
                }

            }
            catch (Exception ex)
            {
                randUrl = "abc." + ConfigTool.ReadVerifyConfig("exp", "HostUrl");
                //randUrl = ex.Message.ToString();
                return randUrl;
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
        /// 获取随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<string> GetRandString(int len, int count)
        {
            List<string> list = new List<string>();
            double max_value = Math.Pow(36, len);
            if (max_value > long.MaxValue)
            {
                //(string.Format("Math.Pow(36, {0}) 超出 long最大值！", len));
                return null;

            }

            long all_count = (long)max_value;
            long stepLong = all_count / count;
            if (stepLong > int.MaxValue)
            {
                // ShowError(string.Format("stepLong ({0}) 超出 int最大值！", stepLong));
                return null;
            }
            int step = (int)stepLong;
            if (step < 3)
            {
                // ShowError("step 不能小于 3!");
                return null;
            }
            long begin = 0;

            Random rand = new Random();
            while (true)
            {
                long value = rand.Next(1, step) + begin;
                begin += step;
                list.Add(GetChart(len, value));
                if (list.Count == count)
                {
                    break;
                }
            }

            list = SortByRandom(list);

            return list;
        }
        //数字+字母
        private const string CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 将数字转化成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetChart(int len, long value)
        {
            StringBuilder str = new StringBuilder();
            while (true)
            {
                str.Append(CHAR[(int)(value % 36)]);
                value = value / 36;
                if (str.Length == len)
                {
                    break;
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="charList"></param>
        /// <returns></returns>
        private List<string> SortByRandom(List<string> charList)
        {
            Random rand = new Random();
            for (int i = 0; i < charList.Count; i++)
            {
                int index = rand.Next(0, charList.Count);
                string temp = charList[i];
                charList[i] = charList[index];
                charList[index] = temp;
            }

            return charList;
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