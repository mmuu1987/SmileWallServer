using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using SmileWallServer.AppCode;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


/// <summary>
/// Common 的摘要说明
/// </summary>
public static class Common
{
    /// <summary>
    /// 保存图片的路径
    /// </summary>
    public static string SavePicturePath = "";
    /// <summary>
    /// 存储ogv的路径
    /// </summary>
    public static string SaveOgv = "";
    /// <summary>
    /// 存储mp4的路径
    /// </summary>
    public static string SaveMp4 = null;
   
    /// <summary>
    /// 大屏幕段可以拿到的最多图片的数量
    /// </summary>
    public static int MaxSiteCount = 6;
    /// <summary>
    /// 链接到的数据库的名称
    /// </summary>
    public static string Smilewall = "";
    /// <summary>
    /// 链接到数据库的表单
    /// </summary>
    public static string Tabel = "";
    /// <summary>
    /// 链接到数据库的用户名
    /// </summary>
    public static string Username = "";
    /// <summary>
    /// 链接到数据库的密码
    /// </summary>
    public static string Password = "";
    /// <summary>
    /// 链接到数据库的地址和端口
    /// </summary>
    public static string MySqlServer = "";
    /// <summary>
    /// 数据库的端口
    /// </summary>
    public static int MySqlPort = 0;

    public static string ReceiveValue = "";

    private static char[] CharsLetter = { 
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};

   
    public static string AccessToken ;

    public static string AppID = "wxcbbe3446d8874b2c";

    public static string AppSecret = "a8e8f9c6e81ace512e5e1293cce8cd11";

    /// <summary>
    /// 上次刷新AccessToken的时间，微信那边规定两个钟后会更新一次AccessToken
    /// </summary>
    public static DateTime AccessTokenTime;


   
        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback+= RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(  object sender, X509Certificate cert,X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }


        //计算时间的差值
        public static int DateDiff(DateTime DateTime1)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(AccessTokenTime.Ticks);

            TimeSpan ts = ts1.Subtract(ts2);

           

           // dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return ts.Seconds;

            #region note
            //C#中使用TimeSpan计算两个时间的差值
            //可以反加两个日期之间任何一个时间单位。
            //TimeSpan ts = Date1 - Date2;
            //double dDays = ts.TotalDays;//带小数的天数，比如1天12小时结果就是1.5 
            //int nDays = ts.Days;//整数天数，1天12小时或者1天20小时结果都是1  
            #endregion
        }
    


    /// <summary>
    /// 检查UUID是否重复，重复则重新制造一个
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public static string CreatUuid(string uuid, TexturePropertyManager manager)
    {

        char[] chars = uuid.ToCharArray();

        Random random = new Random();

        int n = random.Next(chars.Length);

        char x = chars[n];

        char x1 = CharsLetter[random.Next(CharsLetter.Length)];//从26个字母中获取到一个字母

        while (Equals(x, x1))//如果获取的字母还是相等，则从新获取
        {
            x1 = CharsLetter[random.Next(CharsLetter.Length)];
        }

        chars[n] = x1;//把新字母给回数组

        string temp = new string(chars);

        var texs = manager.GetUserByUsername(temp);//获取的新的UUID再次验证是否有重复

        if (texs.Count == 0)//没有重复则返回新的UUID
            return temp;
        return CreatUuid(temp,manager);//有重复则重新计算
    }

    
    /// <summary>
    /// 读取XML上的配置信息
    /// </summary>
    public static void ReadXml(HttpServerUtility server)
    {

        if (!string.IsNullOrEmpty(SaveMp4)) return;

        if (server == null) throw new ArgumentNullException("path");
        XmlDocument doc = new XmlDocument();
        string xmlPath = server.MapPath("/") + "\\CommonServer.xml";
        if (!File.Exists(xmlPath))
        {
            Log.Error("Common","没有找到XML文件");
           
        }
        doc.Load(xmlPath);    //加载Xml文件  


        var selectSingleNode = doc.SelectSingleNode("CommonTag");
        if (selectSingleNode != null)
        {
            XmlNodeList nodeList = selectSingleNode.ChildNodes;

            foreach (XmlNode item in nodeList)
            {
                if (item.Name == "SaveMp4Path")
                    SaveMp4 = server.MapPath("/") + item.InnerText;//在服务器目录下接收文件
                else if (item.Name == "MaxSiteCount")
                    MaxSiteCount = int.Parse(item.InnerText);
                else if (item.Name == "Smilewall")
                    Smilewall = item.InnerText;
                else if (item.Name == "Username")
                    Username = item.InnerText;
                else if (item.Name == "Password")
                    Password = item.InnerText;
                else if (item.Name == "MySqlServer")
                    MySqlServer = item.InnerText;
                else if (item.Name == "MySqlPort")
                    MySqlPort = int.Parse(item.InnerText);
                else if (item.Name == "SaveOgvPath")
                    SaveOgv = server.MapPath("/") + item.InnerText;//在服务器目录下接收文件
                else if (item.Name == "Table") 
                    Tabel = item.InnerText;
                else if (item.Name == "SavePicturePath")
                    SavePicturePath = server.MapPath("/") + item.InnerText;
            }
        }
    }
   

    /// <summary>
    /// 把获取的流转换为二进制的图片数据
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public static byte[] ConversionPicture(StreamReader inputData)
    {

        try
        {
            Stream stream = inputData.BaseStream;

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            stream.Dispose();

            stream.Flush();

            stream = null;

           Log.Debug("Common  ", "获取贴图类的转换的字节大小为:" + bytes.Length);

            return bytes;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;

    }

    //=======【上报信息配置】===================================
    /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
    */
    public const int REPORT_LEVENL = 2;

    //=======【日志级别】===================================
    /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
    */
    public const int LOG_LEVENL = 3;


}