using SmileWallServer.AppCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileWallServer.Webpage.VoiceOffice
{
    public partial class ClientGetMp4 : System.Web.UI.Page
    {
		// Token: 0x0400003E RID: 62
		private TexturePropertyManager _manager = new TexturePropertyManager();

		// Token: 0x0400003F RID: 63
		public string PicturePath = null;

		// Token: 0x04000040 RID: 64
		public string BgPath = null;

		// Token: 0x04000041 RID: 65
		public new string Title;

		// Token: 0x04000042 RID: 66
		public string Style = "width: 78%;position:absolute;top: 18.5%;left: 110px; height: 1500px;";

		// Token: 0x04000043 RID: 67
		public string isAiApp = "display: none; width: 70%;position:absolute;top: 45%;left: 150px;height: 20%;font-size: 80px;text-align: center;";

		public string UUID;

		public string mp4Path;

        public string BtnName = "下载视频";

        public string Description = "请用支付宝扫码获取视频";
        protected void Page_Load(object sender, EventArgs e)
        {
			Common.ReadXml(base.Server);
			 UUID = base.Request["html"];
			string userAgent = base.Request.UserAgent;

            string requestHeads = "\r\n";

            foreach (string requestHeader in base.Request.Headers.Keys)
            {
                requestHeads += requestHeader + ":" + base.Request.Headers[requestHeader] + "\r\n";

            }

            Log.Debug(this.GetType().ToString(), "userAgent is " + userAgent + "\r\n Request.Headers is " + requestHeads);


            bool flag = string.IsNullOrEmpty(userAgent);
			if (flag)
			{
				UUID = null;
			}
			bool flag2 = UUID == null;
			if (flag2)
			{
				base.Response.StatusCode = 404;
				base.Response.StatusDescription = "uuid is error";
				base.Response.ContentType = "text/html; charset=UTF-8";
			}
			else
			{
				try
				{
					IList<TextureInfo> userByUsername = this._manager.GetUserByUsername(UUID);
					bool flag3 = userByUsername.Count >= 1;
					if (flag3)
					{
						string site = userByUsername[0].Site;
                        string room = null;

                        string[] temps = site.Split('_');

                        if (temps.Length == 1)
                        {
                            site = temps[0];

                        }
                        else
                        {
							site = temps[0];
                            room = temps[1];
						}

                        this.BgPath = "http://www.syyj.tglfair.com/Res/000000.png";

						bool flag4 = site == "阿里巴巴拍照留念";
						if (flag4)
						{
							bool flag5 = userAgent.Contains("AlipayClient") || userAgent.Contains("AliApp");
							if (flag5)
							{
								this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;";
								this.PicturePath = "http://www.syyj.tglfair.com/Picture/" + UUID + ".jpg";
								Log.Debug(base.GetType().ToString(), "确认是阿里巴巴app扫码");
							}
							else
							{
								this.isAiApp = "width: 100%;position:absolute;top: 45%;left: 0px;height: 20%;font-size: 80px;text-align: center;";
								this.PicturePath = null;
								Log.Debug(base.GetType().ToString(), "其他的扫码响应");
							}
						}
						else
						{
                            
							bool flag6 = site == "唯品会拍照留念";
							if (flag6)
							{
								this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;";
								this.PicturePath = "http://www.syyj.tglfair.com/Picture/" + UUID + ".jpg";
								Log.Debug(base.GetType().ToString(), "确认是唯品会拍照留念照片");
							}
							else
							{
								bool flag7 = site == "阿里巴巴大文娱试衣镜";
								if (flag7)
								{
                                    Title = "阿里巴巴大文娱试衣镜";
									bool flag8 = userAgent.Contains("AlipayClient") || userAgent.Contains("AliApp");
									if (flag8)
									{
                                       
										this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;";
										this.PicturePath = "http://www.syyj.tglfair.com/Webpage/GetOgv.aspx?html=" + UUID;
										mp4Path = "http://www.syyj.tglfair.com/mp4/" + UUID + ".mp4";
										Log.Debug(base.GetType().ToString(), "确认是阿里巴巴app扫码");

                                        
                                    }
									else
									{
                                       
										this.isAiApp = "width: 100%;position:absolute;top: 45%;left: 0px;height: 20%;font-size: 80px;text-align: center;";
                                        this.PicturePath = null;
                                        this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;display: none;";
										mp4Path = null;
										Log.Debug(base.GetType().ToString(), "其他的扫码响应");
									}
								}

                                bool flag9 = site == "阿里巴巴虚拟直播间";
                               
								if (flag9)
                                {
                                    Title = room;
                                    bool flag10 = userAgent.Contains("AlipayClient") || userAgent.Contains("AliApp");

                                    bool isIos = userAgent != null && (userAgent.Contains("iPad") || userAgent.Contains("iPhone") || userAgent.Contains("iPod") || userAgent.Contains("Mac OS"));


                                    bool isSafari = userAgent.Contains("Safari");

                                    this.Style = "width: 78%;position:absolute;top: 18.5%;left: 110px; height: 1500px;";
                                    this.PicturePath = "http://www.syyj.tglfair.com/Webpage/GetOgv.aspx?html=" + UUID;
                                    mp4Path = "http://www.syyj.tglfair.com/mp4/" + UUID + ".mp4";

                                    if (isIos)
                                    {
                                        this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;display: none;";
                                        this.isAiApp = "width: 70%;position:absolute;top: 35%;left: 150px;height: 20%;font-size: 80px;text-align: center;";
                                        Description = "";


                                        BtnName = "点击播放";
                                        Description = "请点击右上角 更多 或者 ... 用Safari浏览器打开,下载视频";



                                        if (isSafari)
                                        {
											BtnName = "点击下载";
										}

                                        
                                    }
                                    this.BgPath = "http://www.syyj.tglfair.com/Res/bg.png";

                                    //                           if (flag10)
                                    //                           {


                                    //}
                                    //                           else
                                    //                           {

                                    //                               this.Style = "width: 78%;position:absolute;top: 18.5%;left: 110px; height: 1500px;";
                                    //                               this.PicturePath = "http://www.syyj.tglfair.com/Webpage/GetOgv.aspx?html=" + UUID;
                                    //                               mp4Path = "http://www.syyj.tglfair.com/mp4/" + UUID + ".mp4";
                                    //                               //this.isAiApp = "width: 100%;position:absolute;top: 45%;left: 0px;height: 20%;font-size: 80px;text-align: center;";
                                    //                               //this.PicturePath = null;
                                    //                               //this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;display: none;";
                                    //                               //mp4Path = null;
                                    //                               //Log.Debug(base.GetType().ToString(), "其他的扫码响应");
                                    //                           }
                                }

                            }
						}

						
                        base.Response.StatusCode = 200;
						base.Response.StatusDescription = "OK";
						base.Response.ContentType = "text/html; charset=UTF-8";
					}
					else
					{
						base.Response.StatusCode = 404;
						base.Response.StatusDescription = "Not Found";
						base.Response.ContentType = ".txt";
						byte[] bytes = Encoding.Default.GetBytes("没有找到数据");
						base.Response.BinaryWrite(bytes);
						Log.Debug(base.GetType().ToString(), "******************没有找到数据*****************");
					}
				}
				catch (Exception ex)
				{
					Log.Debug(base.GetType().ToString(), ex.ToString());
				}
			}
		}

    }
}