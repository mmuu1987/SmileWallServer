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
    /// 照片上传到服务器在这里的处理
    /// </summary>
    public partial class MyScreenshots : System.Web.UI.Page
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

            StreamReader sr = new StreamReader(context.Request.InputStream);

            if (sr.BaseStream.Length <= 0)
            {
                context.Response.StatusCode = 404;

                context.Response.StatusDescription = "not strema";

                context.Response.ContentType = ".txt";

                return;

            }

            string path = SaveUuid(context, sr);

            byte[] bytes = Encoding.Default.GetBytes(path);

            context.Response.BinaryWrite(bytes); 
        }

        /// <summary>
        /// 辨别UUID在数据库中是否有，没则保存，有则替换UUID,并返回处理后的新的UUID路径
        /// </summary>
        private string SaveUuid(HttpContext p, StreamReader inputData)
        {
            Log.Debug(this.GetType().ToString(), "POST request:"+p.Request.Url);

            _uuid = p.Request.Headers["PicUuid"];

            _sited = p.Request.Headers["Site"];

          

            byte[] bytes =Common. ConversionPicture(inputData);

            try
            {
                //获取数据库里相同UUID的贴图集合
                IList<TextureInfo> texs = _manager.GetUserByUsername(_uuid);

                TextureInfo tex = new TextureInfo();

                if (texs.Count == 0)//如果等于0 表明没有相同的UUID
                    //没有元素，则要新建
                    tex.Uuid = _uuid;
                if (texs.Count > 0)
                {
                    //遇到重名，则重新创建UUID
                    string temp =Common. CreatUuid(_uuid,_manager);
                    tex.Uuid = temp;
                }

                tex.Url = Common.SavePicturePath + @"\" + tex.Uuid + ".png";


                tex.Site = _sited;

                //存储贴图，如果该路径上没有后该文件夹，则先创建文件夹
                if (!Directory.Exists(Common.SavePicturePath))
                {
                    Directory.CreateDirectory(Common.SavePicturePath);
                }

                File.WriteAllBytes(tex.Url, bytes);
                //存储数据到数据库，这里路径跟图片内容是分开存贮的
                _manager.SaveUser(tex);
                return tex.Url;
            }
            catch (Exception e)
            {
                Log.Debug(this.GetType().ToString(), e.ToString());
            }
            return null;
        }
    }
}