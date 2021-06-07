using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileWallServer.AppCode;
using SmileWallServer.Model;
using System.Net;
using System.Xml;
using LitJson;

namespace SmileWallServer.Webpage
{
    public partial class WeChat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "加载成功");
        }

       



        public override void ProcessRequest(HttpContext context)
        {



            if (context.Request.RequestType == "GET")
            {

                Log.Info(this.GetType().ToString(), "微信Get请求");

                #region 验证请求来源是否是微信
                string signature = context.Request["signature"];
                string timestamp = context.Request["timestamp"];
                string nonce = context.Request["nonce"];
                string echostr = context.Request["echostr"];


                Log.Info(this.GetType().ToString(), "微信验证请求已经响应");
                string token = "helloWeChat";
                List<string> list = new List<string>() { token, timestamp, nonce };
                list.Sort();
                string data = string.Join("", list);
                byte[] temp1 = Encoding.UTF8.GetBytes(data);
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] temp2 = sha.ComputeHash(temp1);

                var hashCode = BitConverter.ToString(temp2);
                hashCode = hashCode.Replace("-", "").ToLower();

                if (hashCode == signature)
                {
                    context.Response.Write(echostr);
                    context.Response.End();
                }
                #endregion
            }
            else
            {
                Log.Info("\r\n\r\n\r\n========================>>>>>>>>" + this.GetType(), "微信Post请求");

                WXmessage message;
                try
                {
                    GetTokenTime();
                    message = GetWxMessage(context);
                }
                catch (Exception e)
                {

                    Log.Info(this.GetType().ToString(), e.ToString());
                    throw;
                }
                Log.Info(this.GetType().ToString(), message.ToString());

                try
                {
                    // Log.Info(this.GetType().ToString(), "执行次序1");


                    switch (message.MsgType)
                    {
                        case "text":
                            ProcessTextMsg(message, context);
                            break;
                        case "image":
                            if (Common.AccessToken != null)
                            {
                                string filePath = GetMultimedia(Common.AccessToken, message.MediaId);
                                string id = UploadMultimedia(Common.AccessToken, message.MsgType, filePath);
                                Log.Info(this.GetType().ToString(), "图片路径为=>" + filePath);
                                string temp = SendPicTextMessage(message, id);
                                context.Response.Write(temp);
                                context.Response.End();
                            }

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Info(this.GetType().ToString(), e.ToString());
                    throw;
                }

                Log.Info(this.GetType().ToString(), "微信Post请求Response响应完毕");


            }
        }

       
        private void ProcessTextMsg(WXmessage message, HttpContext context)
        {
            string tempxml = "<xml><ToUserName><![CDATA[-tname]]></ToUserName><FromUserName><![CDATA[-fname]]></FromUserName><CreateTime>-time</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[-content]]></Content></xml>";
            tempxml = tempxml.Replace("-tname", message.FromUserName);
            tempxml = tempxml.Replace("-fname", message.ToUserName);
            tempxml = tempxml.Replace("-time", message.CreateTime);
            tempxml = tempxml.Replace("-content", "MsgType:" + message.MsgType + "\ncontent:" + message.Content + "\nMsgId:" + message.MsgId);
            context.Response.Write(tempxml);
            context.Response.End();
        }


        /// <summary>
        /// 下载保存多媒体文件,返回多媒体保存路径
        /// </summary>
        /// <returns></returns>
        public string GetMultimedia(string accessToken, string mediaId)
        {
            string file = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "https://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + accessToken + "&media_id=" + mediaId;

            Common.SetCertificatePolicy();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);


            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                strpath = myResponse.ResponseUri.ToString();
                Log.Info(this.GetType().ToString(), ("接收类别://" + myResponse.ContentType));
                WebClient mywebclient = new WebClient();

                string directory = Server.MapPath("image");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                savepath = directory + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".jpg";
                Log.Info(this.GetType().ToString(), ("路径=> " + savepath));
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    file = savepath;
                }
                catch (Exception ex)
                {
                    savepath = ex.ToString();
                    Log.Info(this.GetType().ToString(), savepath);
                }

            }
            return file;
        }

        private WXmessage GetWxMessage(HttpContext context)
        {
            WXmessage wx = new WXmessage();
            StreamReader str = new StreamReader(context.Request.InputStream, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            wx.ToUserName = xml.SelectSingleNode("xml").SelectSingleNode("ToUserName").InnerText;

            wx.FromUserName = xml.SelectSingleNode("xml").SelectSingleNode("FromUserName").InnerText;

            wx.MsgType = xml.SelectSingleNode("xml").SelectSingleNode("MsgType").InnerText;

            wx.CreateTime = xml.SelectSingleNode("xml").SelectSingleNode("CreateTime").InnerText;

            wx.MsgId = xml.SelectSingleNode("xml").SelectSingleNode("MsgId").InnerText;

            if (xml.SelectSingleNode("xml").SelectSingleNode("MediaId") != null)
                wx.MediaId = xml.SelectSingleNode("xml").SelectSingleNode("MediaId").InnerText;

            if (xml.SelectSingleNode("xml").SelectSingleNode("PicUrl") != null)
                wx.PicUrl = xml.SelectSingleNode("xml").SelectSingleNode("PicUrl").InnerText;

            if (wx.MsgType.Trim() == "text")
            {
                wx.Content = xml.SelectSingleNode("xml").SelectSingleNode("Content").InnerText;
            }
            if (wx.MsgType.Trim() == "event")
            {
                wx.EventName = xml.SelectSingleNode("xml").SelectSingleNode("Event").InnerText;
                wx.EventKey = xml.SelectSingleNode("xml").SelectSingleNode("EventKey").InnerText;
            }
            if (wx.MsgType.Trim() == "voice")
            {
                wx.Recognition = xml.SelectSingleNode("xml").SelectSingleNode("Recognition").InnerText;
            }
            if (wx.MsgType.Trim() == "image")
            {
                wx.MediaId = xml.SelectSingleNode("xml").SelectSingleNode("MediaId").InnerText;
            }

            return wx;
        }

        /// <summary>
        /// 上传多媒体文件,返回 MediaId
        /// </summary>
        /// <param name="accessToken"/>
        /// <param name="type"/>
        /// <returns></returns>
        public string UploadMultimedia(string accessToken, string type, string path)
        {
            string result = "";
            string wxurl = "https://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accessToken + "&type=" + type;
            // string filepath = Server.MapPath("image") + "\\hemeng80.jpg";//(本地服务器的地址)

            Log.Info(this.GetType().ToString(), ("上传路径:" + path));
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(wxurl, "POST", path);
                result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                Log.Info(this.GetType().ToString(), ("上传result:" + result));
                UploadFileId _mode = JsonMapper.ToObject<UploadFileId>(result);
                result = _mode.media_id;
            }
            catch (Exception ex)
            {
                result = "Error:" + ex.Message;

            }
            Log.Info(this.GetType().ToString(), ("上传MediaId: " + result));
            return result;
        }

        protected string SendPicTextMessage(WXmessage mode, string mediaId)
        {
            string str = "<xml>" +
                         "<ToUserName><![CDATA[" + mode.FromUserName + "]]></ToUserName>" +
                         "<FromUserName><![CDATA[" + mode.ToUserName + "]]></FromUserName>" +
                         "<CreateTime>" + DateTime.Now + "</CreateTime>" +
                         "<MsgType><![CDATA[image]]></MsgType>" +
                         "<Image><MediaId><![CDATA[" + mediaId + "]]></MediaId></Image>" +
                         "</xml>";

            return str;
        }

        private string GetToken()
        {

            // 也可以这样写:
            //return  GetPage("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=你的appid&secret=你的secret", "");

            string res = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + Common.AppID + "&secret=" + Common.AppSecret);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();


                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);


                string content = reader.ReadToEnd();

                Log.Info(this.GetType().ToString(), "content is " + content);

                AccessTokenData data = JsonMapper.ToObject<AccessTokenData>(content);
                res = data.access_token;

            }


            return res;
        }
        /// <summary>
        /// 2小时更新一次AccessToken
        /// </summary>
        private void GetTokenTime()
        {
           
                if (string.IsNullOrEmpty(Common.AccessToken))
                {
                    string toke = GetToken();
                    if (toke == null)
                        Log.Info(this.GetType().ToString(), "AccessToken is null");

                    if (!string.IsNullOrEmpty(toke))
                    {
                        Common.AccessToken = toke;
                        Common.AccessTokenTime = DateTime.Now;
                    }
                }
                else
                {
                    int secon = Common.DateDiff(DateTime.Now);
                    if (secon >= 7200)
                    {
                        string toke = GetToken();
                        if (toke == null)
                            Log.Info(this.GetType().ToString(), "AccessToken is null");

                        if (!string.IsNullOrEmpty(toke))
                        {
                            Common.AccessToken = toke;
                            Common.AccessTokenTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        Log.Info(this.GetType().ToString(), "Access Token离上次更新时间为=> " + secon);
                    }
                }
            }
        }
    }



    public class WXmessage
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string MsgType { get; set; }
        public string EventName { get; set; }
        public string Content { get; set; }
        public string Recognition { get; set; }
        public string MediaId { get; set; }
        public string EventKey { get; set; }

        public string CreateTime { get; set; }

        public string PicUrl { get; set; }

        public string MsgId { get; set; }

        public override string ToString()
        {
            string str = "\r\n";

            str += "FormUserName  :" + FromUserName + "\r\n";
            str += "ToUserName    :" + ToUserName + "\r\n";
            str += "MsgType       :" + MsgType + "\r\n";
            str += "EventName     :" + EventName + "\r\n";
            str += "Content       :" + Content + "\r\n";
            str += "Recognition   :" + Recognition + "\r\n";
            str += "MediaId       :" + MediaId + "\r\n";
            str += "EventKey      :" + EventKey + "\r\n";
            str += "CreatTime     :" + CreateTime + "\r\n";
            str += "PicUrl        :" + PicUrl + "\r\n";
            str += "MsgId         :" + MsgId + "\r\n";

            return str;
        }
    }

    public class AccessTokenData
    {
        public string access_token;

        public int expires_in;
    }

    public class UploadFileId
    {
        //{"type":"image","media_id":"RMSimLRUtaW-hSyzBUP6ZTbXr3QlHEeDSefTH3IEVlHbXhfK7MKotodW_eYlw9uT","created_at":1587196703,"item":[]}

        public string type;

        public string media_id;

        public int created_at;


    }
