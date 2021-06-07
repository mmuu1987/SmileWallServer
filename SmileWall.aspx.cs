using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using SmileWallServer.AppCode;


namespace SmileSite
{
    /// <summary>
    /// POST影片的数据处理
    /// </summary>
    public partial class SmileWall : Page
    {
        private readonly TexturePropertyManager _manager = new TexturePropertyManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(this.GetType().ToString(), "page load");
        }

        public override void ProcessRequest(HttpContext context)
        {
            Log.Info(this.GetType().ToString(), "page PostData");

            Stream s = context.Request.InputStream;

            if (s.Length == 0)
            {
                byte[] bytes = Encoding.Default.GetBytes("数据流为空");
                context.Response.BinaryWrite(bytes);
                return;
            }
           
            Common.ReadXml(Server);

            TextureInfo tex = new TextureInfo();

            var uuid = context.Request.Headers["FileName"];

            var sited = context.Request.Headers["Sited"];

            var extension = context.Request.Headers["Extension"];

            if (extension == null) return;

            try
            {
                if (uuid != null && sited != null)
                {
                    //获取数据库里相同UUID的贴图集合
                    IList<TextureInfo> texs = _manager.GetUserByUsername(uuid);

                    if (texs.Count == 0) //如果等于0 表明没有相同的UUID
                        //没有元素，则要新建
                        tex.Uuid = uuid;
                    if (texs.Count > 0)
                    {
                        //遇到重名，则重新创建UUID
                        string temp = Common. CreatUuid(uuid,_manager);
                        tex.Uuid = temp;
                    }

                    tex.Url = Common.SaveOgv + @"\" + tex.Uuid + ".ogv";

                    tex.Site = sited;

                    int lenght = (int)s.Length;

                    byte[] video = new byte[s.Length];

                    if (lenght <= 0) return;

                    s.Read(video, 0, video.Length);

                    //File.WriteAllBytes(@"C:\Users\Administrator\Desktop\test.mp4", video);

                    //存储贴图，如果该路径上没有后该文件夹，则先创建文件夹
                    if (!Directory.Exists(Common.SaveOgv))
                    {
                        Directory.CreateDirectory(Common.SaveOgv);
                    }
                    File.WriteAllBytes(tex.Url, video);

                    //存储数据到数据库，这里路径跟图片内容是分开存贮的
                    _manager.SaveUser(tex);

                    context.Response.StatusCode = 200;

                    context.Response.StatusDescription = "OK";

                    context.Response.ContentType = "text/html; charset=UTF-8";

                    byte[] bytes = Encoding.Default.GetBytes(tex.Uuid);

                    context.Response.BinaryWrite(bytes);

                    CreatMp4File(tex.Url);

                    s.Flush();
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
        /// <summary>
        /// 把传递过来的视频转换成mp4格式，方便网页调用
        /// </summary>
        private void CreatMp4File(string filePath)
        {
            Thread t = new Thread((() =>
            {
                try
                {
                    Process p = new Process();

                    p.StartInfo.FileName = Server.MapPath("/") + "/lib/ffmpeg-20170225-7e4f32f-win64-static/bin/ffmpeg.exe";

                    Log.Info(this.GetType() + "路径是：",
                        Server.MapPath("/") + "/lib/ffmpeg-20170225-7e4f32f-win64-static/bin/ffmpeg.exe");

                    p.StartInfo.UseShellExecute = false;
                    string srcFile = filePath;

                    string destFile = null;

                    if (!Directory.Exists(Common.SaveMp4))
                    {
                        Directory.CreateDirectory(Common.SaveMp4);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    destFile = Common.SaveMp4 + "/"+fileName + ".mp4";

                    //p.StartInfo.Arguments = "-i " + srcFile + " -y  -vcodec libtheora -f ogv -b 500000 " + destFile;    //执行参数

                    p.StartInfo.Arguments = "-i " + srcFile + " -y  -vcodec libxvid -f mp4 -b 500000 -an " + destFile;    //执行参数

                    p.StartInfo.UseShellExecute = false;  ////不使用系统外壳程序启动进程
                    p.StartInfo.CreateNoWindow = true;  //不显示dos程序窗口i

                    p.StartInfo.RedirectStandardInput = true;

                    p.StartInfo.RedirectStandardOutput = true;

                    p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中

                    p.StartInfo.UseShellExecute = false;

                    p.Start();

                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    p.BeginErrorReadLine();//开始异步读取

                    p.WaitForExit();//阻塞等待进程结束

                    p.Close();//关闭进程

                    p.Dispose();//释放资源
                }
                catch(Exception e)
                {
                    Log.Info("线程错误:", e.ToString());
                }
                
            }));

            t.Start();
        }
    }
}