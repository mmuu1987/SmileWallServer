using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileWallServer.AppCode;

namespace SmileWallServer.Webpage
{
    /// <summary>
    /// 获取图片
    /// </summary>
    public partial class GetPicture : System.Web.UI.Page
    {
        private TexturePropertyManager _manager = new TexturePropertyManager();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void ProcessRequest(HttpContext context)
        {
            var uuid = context.Request["Uuid"];

            if (uuid == null)
            {
                context.Response.StatusCode = 404;

                context.Response.StatusDescription = "Not Found";

                context.Response.ContentType = ".txt";

                byte[] bytes = Encoding.Default.GetBytes("uuid is error");

                context.Response.BinaryWrite(bytes);

                return;
            }

            try
            {
                //获取数据库里相同UUID的贴图集合
                var texs = _manager.GetUserByUsername(uuid);

                if (texs.Count >= 1)
                {
                    if (File.Exists(texs[0].Url))
                    {
                        context.Response.StatusCode = 200;

                        context.Response.StatusDescription = "ok";

                        string temp = Path.GetExtension(texs[0].Url);

                        if (temp == "ogv")
                        Response.ContentType = "video/mpeg4";
                        else if(temp == "png")
                        context.Response.ContentType = "image/jpeg";

                        byte[] bytes = File.ReadAllBytes(texs[0].Url);

                        context.Response.BinaryWrite(bytes);
                       
                    }
                    else
                    {
                        _manager.DeleteById(texs[0].Id);//删除数据数据

                        context.Response.StatusCode = 404;

                        context.Response.StatusDescription = "Not Found";

                        context.Response.ContentType = ".txt";

                        byte[] bytes = Encoding.Default.GetBytes("没有找到数据");

                        context.Response.BinaryWrite(bytes);

                        Log.Debug(this.GetType().ToString(), "******************没有找到数据*****************");
                    }

                }
                else
                {

                    context.Response.StatusCode = 404;

                    context.Response.StatusDescription = "Not Found";

                    context.Response.ContentType = ".txt";

                    byte[] bytes = Encoding.Default.GetBytes("没有找到数据");

                    context.Response.BinaryWrite(bytes);

                    Log.Debug(this.GetType().ToString(), "******************没有找到数据*****************");
                }
            }
            catch (Exception e1)
            {
                Log.Debug(this.GetType().ToString(), e1.ToString());
            }
        }
    }
}