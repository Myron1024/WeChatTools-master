using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatTools.Web
{
    /// <summary>
    /// HandlerWeb2 的摘要说明
    /// </summary>
    public class HandlerWeb2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("http://" + GetRandHostUrl() + "/weixin/Buddhism/BuddhismEventList.aspx?wid=55&bid=44");
            //  context.Response.Redirect("http://" + GetRandHostUrl() + "/weixin/Buddhism/BuddhismEventList.aspx?wid=55&bid=44");
        }

        private string GetRandHostUrl()
        {
            Random ran = new Random();
            int RandKey = ran.Next(4, 9);
            string randUrl = "";

            try
            {

                randUrl = GenerateRandomNumber(RandKey);
                randUrl = randUrl + ".flzt.canjr.com";
            }
            catch (Exception ex)
            {
                randUrl = "sohu.cn.zlfbam.cn";
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}