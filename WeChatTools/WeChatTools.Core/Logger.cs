
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
namespace WeChatTools.Core
{
    
    /// <summary>
    /// 验证项目Token
    /// </summary>
    public class Logger
    {
        #region=============日志处理==============
        //在网站根目录下创建日志目录
        public static string path = System.Web.HttpRuntime.AppDomainAppPath.ToString() + "/Logs";

        /**
      * 实际的写日志操作
      * @param type 日志记录类型
      * @param className 类名
      * @param content 写入内容
      */
        public static void WriteLoggger(string content)
        {
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/Logtxt_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);

            //向日志文件写入内容
            string write_content = time + ": " + content;
            mySw.WriteLine(write_content);

            //关闭日志文件
            mySw.Close();
        }
        #endregion

    }

}