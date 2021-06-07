using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileWallServer.AppCode;
using System.Text.RegularExpressions;

namespace SmileWallServer.Webpage
{
    /// <summary>
    /// 获取OGV文件
    /// </summary>
    public partial class GetOgv : System.Web.UI.Page
    {
        private TexturePropertyManager _manager = new TexturePropertyManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.ReadXml(Server);
            string requestHeads = "\r\n";

            foreach (string requestHeader in base.Request.Headers.Keys)
            {
                requestHeads += requestHeader + ":" + base.Request.Headers[requestHeader] + "\r\n";
            }

            Log.Debug(this.GetType().ToString(),  "Request.Headers is " + requestHeads);

            string userAgent = base.Request.UserAgent;

            bool isIos = userAgent != null && (userAgent.Contains("iPad") || userAgent.Contains("iPhone") || userAgent.Contains("iPod") ||userAgent.Contains("Mac OS"));


            string uuid = Request["html"];
            try
            {



                if (uuid == null)
                {
                    byte[] bytes = Encoding.Default.GetBytes("the value is error on GetOgv");
                    Response.BinaryWrite(bytes);
                }
                else
                {

                   
                    string filePath = Common.SaveMp4 + "/"+uuid + ".mp4";
                    if (!File.Exists(filePath))
                    {
                        byte[] bytesTemp = Encoding.Default.GetBytes("not find the path "+ filePath);
                        Response.BinaryWrite(bytesTemp);
                        return;
                    }
                    byte[] bytes = File.ReadAllBytes(filePath);


                    if (!isIos)
                    {
                        

                        base.Response.StatusCode = 200;
                        Response.BinaryWrite(bytes);

                        Response.StatusDescription = "ok";
                        Response.ContentType = "video/mp4";

                        string infos = "\r\n";
                       
                        Log.Info(this.GetType().ToString(), "Getmp4 安卓端 传输成功");
                    }
                    else
                    {
                        var range = Request.Headers.Get("Range");
                        var indexs = range != null ? range.Split('=')[1].Split('-') : null;
                        var startIndex = indexs != null ? int.Parse(indexs[0]) : 0;
                        var endIndex = indexs != null ? int.Parse(indexs[1]) : 0;

                        //var fileMemoryStream = new MemoryStream(bytes); //
                        //int length = endIndex - startIndex + 1;



                        byte[] fileByte = new byte[endIndex - startIndex + 1];//这里的fileByteLength是返回数据的长度，fileByteLength=endIndex-startIndex+1

                       // Array.Copy(bytes,startIndex,fileByte,0,fileByte.Length);



                        var fileMemoryStream = new MemoryStream(bytes); //
                       
                       // byte[] fileByte = new byte[fileByteLength] //这里的fileByteLength是返回数据的长度，fileByteLength=endIndex-startIndex+1
                        fileMemoryStream.Position = startIndex;
                        fileMemoryStream.Read(fileByte, 0, fileByte.Length);
                        fileMemoryStream.Flush();
                        fileMemoryStream.Close();
                          


                        Response.Headers.Set("Accept-Ranges", "bytes");
                        Response.Headers.Add("Content-Range", "bytes " + startIndex + "-" + endIndex + "/" + bytes.Length);
                        //streamLength 为整个数据的长度
                        Response.Headers.Set("Content-Length", fileByte.Length.ToString());
                        Response.Headers.Set("Content-Type", "video/mp4");
                        Response.Headers.Set("Proxy-Connection", "keep-alive");



                        if (fileByte.Length == 1)
                        {
                            Response.StatusCode = 200;
                            Response.Headers.Set("Content-Length", bytes.Length.ToString());
                        }
                        else Response.StatusCode = 206;

                        
                        Response.BinaryWrite(fileByte);

                        //Response.StatusDescription = "ok";
                        Response.ContentType = "video/mp4";
                        
                        string infos = "\r\n";
                        foreach (string header in base.Response.Headers.Keys)
                        {
                            infos += header + ":" + base.Response.Headers[header] + "\r\n";
                        }

                        Log.Debug(base.GetType().ToString(), "响应头部是： " + infos+ "\r\nGetmp4 IOS 传输成功 \r\n\r\n\r\n"+ Response.IsClientConnected+"\r\n" +
                                                             Response.StatusCode);

                       

                    }
                  
                }
            }
            catch (Exception e2)
            {
                Log.Info(this.GetType().ToString(), e2.ToString());
            }
        }








        //public override void ProcessRequest(HttpContext context)
        //{
          


        //    string requestHeads = "\r\n";

        //    foreach (string requestHeader in context.Request.Headers.Keys)
        //    {
        //        requestHeads += requestHeader + ":" + base.Request.Headers[requestHeader] + "\r\n";
        //    }

        //    Log.Debug(this.GetType().ToString(), "PostData contextRequest.Headers is " + requestHeads);
        //    Log.Info(this.GetType().ToString(), "page PostData");

        //}
    }
}