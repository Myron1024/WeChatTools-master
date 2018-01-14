using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatTools.Web.dev
{
    /// <summary>
    /// ZhiHu 的摘要说明
    /// </summary>
    public class ZhiHu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Redirect("http://link.zhihu.com/?target=https%3A//teu7.cn");
 
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