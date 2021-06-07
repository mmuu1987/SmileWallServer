using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileWallServer.AppCode;

namespace SmileSite.Webpage
{
    /// <summary>
    /// 获取视频网页的后台处理
    /// </summary>
    public partial class Webpage : System.Web.UI.Page
    {
        /// <summary>
        /// 视频的服务器路径
        /// </summary>
        public string VideoPath = null;
        /// <summary>
        /// 背景图片的路径
        /// </summary>
        public string BgPath = null;



        private TexturePropertyManager _manager = new TexturePropertyManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "加载成功");

            string s = Request.QueryString["FileName"];

            string url = null;

            Common.ReadXml(Server);

            if (s == null)
            {
                byte[] bytes = Encoding.Default.GetBytes("null");
                Response.BinaryWrite(bytes);
            }
            else
            {
                var texs = _manager.GetUserByUsername(s);

                if (texs.Count <= 0)
                {
                    Response.StatusCode = 404;
                    Response.StatusDescription = "Not Found";

                }
                else if (texs.Count >= 1)
                {
                    TextureInfo tex = texs[0];
                    url = "http://videotest.gzcloudbeing.com/mp4/" + s+".mp4";
                    //Headself = "731867.mp4";
                    VideoPath = url;

                    BgPath = "http://videotest.gzcloudbeing.com/Res/000000.png";
                    // Label1.Text = "<source src= '"+url+"' type=\"video/mp4\"></source>";
                    this.Label1.Text = "当前ID为：" + tex.Uuid;
                    Response.StatusCode = 200;
                    Response.StatusDescription = "ok";
                    Response.ContentType = "text/html";

                }
            }
        }
    }
}