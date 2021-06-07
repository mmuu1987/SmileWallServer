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
    /// 获取站点信息
    /// </summary>
    public partial class GetSite : System.Web.UI.Page
    {
        private TexturePropertyManager _manager = new TexturePropertyManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public override void ProcessRequest(HttpContext context)
        {
            Common.ReadXml(Server);

            var site = context.Request["site"];//取第一个斜杠后面的字符,这里得到的站点名称

            if (site == null)
                Log.Debug(this.GetType().ToString(), "!!!!!!!!!!!!!!!获得的站点为空,无法查询站点信息!!!!!!!!!!!!!!!");
            var sites = _manager.GetUseBySite(site);

            var siteList = sites.ToList();

            string str = null;

            if (siteList.Count > 0)//如果查询到站点信息
            {
                for (int i = siteList.Count; i >= 1; i--)//从数据库后面开始拿数据，因为后面的数据才是最新的
                {
                    if (siteList.Count - i >= Common.MaxSiteCount)//限制获取最大站点的数量
                        break;
                    str += siteList[i - 1].Uuid + ":";//客户端取数据的时候以分号间隔
                }

                Log.Debug(this.GetType().ToString(), "SiteServer处理后返回的字符串数据是：" + str);

                context.Response.StatusCode = 200;

                context.Response.StatusDescription = "OK";

                context.Response.ContentType = "text/html; charset=UTF-8";

                byte[] bytes = Encoding.UTF8.GetBytes(str);

                context.Response.BinaryWrite(bytes);
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
    }
}