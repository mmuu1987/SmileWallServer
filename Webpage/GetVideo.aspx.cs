using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileWallServer.AppCode;

namespace SmileSite.Webpage
{
    /// <summary>
    /// 获取视频
    /// </summary>
    public partial class GetVideo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Log.Info(this.GetType().ToString(), "加载成功");
            string s = Request.QueryString["Url"];

            try
            {
                if (s == null)
                {
                    byte[] bytes = Encoding.Default.GetBytes("传值错误");
                    Response.BinaryWrite(bytes);
                }
                else
                {

                    byte[] bytes = File.ReadAllBytes(s);
                    Response.BinaryWrite(bytes);
                    Response.StatusCode = 200;
                    Response.StatusDescription = "ok";
                    Response.ContentType = "video/mpeg4";
                    
                    Log.Info(this.GetType().ToString(), "传输成功");
                }
            }
            catch (Exception e2)
            {
                Log.Info(this.GetType().ToString(), e2.ToString());
            }
           
        }
    }
}