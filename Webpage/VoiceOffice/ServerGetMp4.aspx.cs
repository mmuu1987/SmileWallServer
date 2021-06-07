using SmileWallServer.AppCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileWallServer.Webpage.VoiceOffice
{
    public partial class ServerGetMp4 : System.Web.UI.Page
    {
        private readonly TexturePropertyManager _manager = new TexturePropertyManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "page load");
        }

        public override void ProcessRequest(HttpContext context)
        {
            Log.Info(this.GetType().ToString(), "page PostData");

            StreamReader s = new StreamReader(context.Request.Files["PicUuidData"].InputStream);
           

            if (s.BaseStream.Length <= 0L)
            {
                byte[] bytes = Encoding.Default.GetBytes("数据流为空");
                context.Response.BinaryWrite(bytes);
                return;
            }

            Common.ReadXml(Server);

            TextureInfo tex = new TextureInfo();

            var uuid = context.Request["PicUuid"];

            var sited = context.Request["Sited"];

            var extension = context.Request["Extension"];
            Log.Info(this.GetType().ToString(), "page PostData  1    "+ uuid + "    sited " + sited+ "    extension " + extension);
            if (extension == null) return;
            Log.Info(this.GetType().ToString(), "page PostData  2");
            try
            {
                if (uuid != null && sited != null)
                {
                    //获取数据库里相同UUID的贴图集合   
                    IList<TextureInfo> texs = _manager.GetUserByUsername(uuid);
                    Log.Info(this.GetType().ToString(), "page PostData  2.1");
                    if (texs.Count == 0) //如果等于0 表明没有相同的UUID
                        //没有元素，则要新建
                        tex.Uuid = uuid;
                    Log.Info(this.GetType().ToString(), "page PostData  3");
                    if (texs.Count > 0)
                    {
                        //遇到重名，则重新创建UUID
                        string temp = Common.CreatUuid(uuid, _manager);
                        tex.Uuid = temp;
                    }
                    Log.Info(this.GetType().ToString(), "page PostData  1");
                    tex.Url = Common.SaveMp4 + @"\" + tex.Uuid + ".mp4";

                    tex.Site = sited;

                   

                    //File.WriteAllBytes(@"C:\Users\Administrator\Desktop\test.mp4", video);

                    //存储贴图，如果该路径上没有后该文件夹，则先创建文件夹
                    if (!Directory.Exists(Common.SaveMp4))
                    {
                        Directory.CreateDirectory(Common.SaveMp4);
                    }
                    File.WriteAllBytes(tex.Url, Common.ConversionPicture(s));

                    //存储数据到数据库，这里路径跟图片内容是分开存贮的
                    _manager.SaveUser(tex);

                    context.Response.StatusCode = 200;

                    context.Response.StatusDescription = "OK";

                    context.Response.ContentType = "text/html; charset=UTF-8";

                    byte[] bytes = Encoding.Default.GetBytes(tex.Uuid);

                    context.Response.BinaryWrite(bytes);



                    
                    s.Close();
                    s.Dispose();
                }
                else//出现异常
                {
                    context.Response.StatusCode = 404;

                    context.Response.StatusDescription = "uuid is null || sited is null";

                    context.Response.ContentType = ".txt";

                    byte[] bytes = Encoding.Default.GetBytes("UUID获取空值，或者站点为空值");

                    context.Response.BinaryWrite(bytes);

                    Log.Debug(this.GetType().ToString(), "******************没有找到数据*****************");
                }
            }
            catch (Exception e)
            {
                Log.Info(this.GetType().ToString(), e.ToString());
            }
        }
    }
}