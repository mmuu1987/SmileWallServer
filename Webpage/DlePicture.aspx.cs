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
    public partial class DlePicture : System.Web.UI.Page
    {
        private TexturePropertyManager _manager = new TexturePropertyManager();
        protected void Page_Load(object sender, EventArgs e)
        {


            var uuid = Request["del"];//取第一个斜杠后面的字符
            
            try
            {
                //获取数据库里相同UUID的贴图集合
                var texs = _manager.GetUserByUsername(uuid);

                if (texs.Count >= 1)
                {
                    if (File.Exists(texs[0].Url))
                    {
                        Response.StatusCode = 200;

                        Response.StatusDescription = "OK";

                        Response.ContentType = "text/html; charset=utf-8";

                        byte[] bytes = Encoding.Default.GetBytes("删除成功");

                        Response.BinaryWrite(bytes);

                        _manager.DeleteById(texs[0].Id);//删除数据数据

                        File.Delete(texs[0].Url);//删除硬盘上的数据,这里服务器跟存储图片的电脑是在一起的

                    }
                    else
                    {
                        _manager.DeleteById(texs[0].Id);//删除数据数据

                        Response.StatusCode = 404;

                        Response.StatusDescription = "Not Found";

                        Response.ContentType = ".txt";

                        byte[] bytes = Encoding.Default.GetBytes("没有找到数据");

                        Response.BinaryWrite(bytes);

                        Log.Debug(this.GetType().ToString(), "******************没有找到数据*****************");
                    }
                }
                else
                {
                    Response.StatusCode = 404;

                    Response.StatusDescription = "Not Found";

                    Response.ContentType = ".txt";

                    byte[] bytes = Encoding.Default.GetBytes("没有找到数据");

                    Response.BinaryWrite(bytes);

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