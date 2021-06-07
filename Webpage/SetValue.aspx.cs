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
    /// 菜鸟地网用到的，用pad客户端接收信息
    /// </summary>
    public partial class SetValue : System.Web.UI.Page
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

            Common.ReceiveValue = _uuid + "!*_*!" + _sited;

            Log.Debug(this.GetType().ToString(), "从菜鸟地网pad接收到的消息是：" +Common.ReceiveValue);


            byte[] bytes = Encoding.UTF8.GetBytes("Receive info is " + Common.ReceiveValue);

            context.Response.BinaryWrite(bytes);
        }

     
    }
}