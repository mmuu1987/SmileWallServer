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
    /// 获取照片网页的后台处理
    /// </summary>
    public partial class GetHtml : System.Web.UI.Page
    {
        private TexturePropertyManager _manager = new TexturePropertyManager();

        public string PicturePath = null;

        public string BgPath = null;

        public string Test = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.ReadXml(Server);

            var uuid = Request["html"];//取第一个斜杠后面的字符

            if (uuid == null)
            {
                Response.StatusCode = 404;

                Response.StatusDescription = "uuid is error";

                Response.ContentType = "text/html; charset=UTF-8";
                return;
            }

            try
            {
                //获取数据库里相同UUID的贴图集合
                var texs = _manager.GetUserByUsername(uuid);

                if (texs.Count >= 1)
                {
                    PicturePath = "http://videotest.gzcloudbeing.com/Picture/" + uuid + ".png";//网络路径
                    //PicturePath = "http://localhost:65294/Picture/" + uuid + ".png";//本地测试

                   // BgPath = "http://localhost:65294/Res/000000.png";
                    BgPath = "http://videotest.gzcloudbeing.com/Res/000000.png";

                    Label1.Text = "当前ID为" + uuid;

                    Response.StatusCode = 200;

                    Response.StatusDescription = "OK";

                    Response.ContentType = "text/html; charset=UTF-8";

                }
                else
                {

                    Response.StatusCode = 404;

                    Response.StatusDescription = "Not Found";

                    Response.ContentType =".txt";

                    byte[] bytes = Encoding.Default.GetBytes("没有找到数据");

                    Response.BinaryWrite( bytes);

                    Log.Debug(this.GetType().ToString(), "******************没有找到数据*****************");
                }
            }
            catch (Exception e1)
            {
                Log.Debug(this.GetType().ToString(),e1.ToString());
            }
        }
    }
}