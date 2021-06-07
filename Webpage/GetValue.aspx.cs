using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileWallServer.AppCode;

namespace SmileWallServer.Webpage
{
    public partial class GetValue : System.Web.UI.Page
    {

        private TexturePropertyManager _manager = new TexturePropertyManager();
        /// <summary>
        /// 该对象生成的UUID
        /// </summary>
        private string _uuid;

        private string _sited;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void ProcessRequest(HttpContext context)
        {

            Common.ReadXml(Server);

            context.Response.StatusCode = 200;

            context.Response.StatusDescription = "OK";

            context.Response.ContentType = "text/html; charset=UTF-8";



            _uuid = context.Request["PicUuid"];

            _sited = context.Request["Sited"];

           

            Log.Debug(this.GetType().ToString(), "竖屏获取信息，信息为：" + Common.ReceiveValue);

            byte[] bytes;
            if (string.IsNullOrEmpty(Common.ReceiveValue))
            {
                Log.Debug(this.GetType().ToString(), "Common.ReceiveValue 数据为Null：");


                bytes = Encoding.UTF8.GetBytes("data is error");

                context.Response.BinaryWrite(bytes);
                return;
            }


            bytes = Encoding.UTF8.GetBytes(Common.ReceiveValue);

            Common.ReceiveValue = null;//重置，新的数据进来

            context.Response.BinaryWrite(bytes);
        }
    }
}