using SmileWallServer.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileWallServer.Webpage.VoiceOffice
{
    public partial class GetVoiceOffice : System.Web.UI.Page
    {
		// Token: 0x06000056 RID: 86 RVA: 0x00003D28 File Offset: 0x00001F28
		protected void Page_Load(object sender, EventArgs e)
		{
			Common.ReadXml(base.Server);
			string text = base.Request["html"];
			string userAgent = base.Request.UserAgent;
			bool flag = string.IsNullOrEmpty(userAgent);
			if (flag)
			{
				text = null;
			}
			bool flag2 = text == null;
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
					IList<TextureInfo> userByUsername = this._manager.GetUserByUsername(text);
					bool flag3 = userByUsername.Count >= 1;
					if (flag3)
					{
						string site = userByUsername[0].Site;
						bool flag4 = site == "阿里巴巴拍照留念";
						if (flag4)
						{
							bool flag5 = userAgent.Contains("AlipayClient") || userAgent.Contains("AliApp");
							if (flag5)
							{
								this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;";
								this.PicturePath = "http://www.syyj.tglfair.com/Picture/" + text + ".jpg";
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
                            if (site == "阿里巴巴菜鸟地网" || site == "唯品会拍照留念")
                            {
                                this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;";
                                this.PicturePath = "http://www.syyj.tglfair.com/Picture/" + text + ".jpg";
                                Log.Debug(base.GetType().ToString(), "确认是唯品会拍照留念照片");
							}
							
							else
							{
								bool flag7 = site == "阿里巴巴大文娱试衣镜";
								if (flag7)
								{
									bool flag8 = userAgent.Contains("AlipayClient") || userAgent.Contains("AliApp");
									if (flag8)
									{
										this.Style = "width: 100%;position:absolute;top: 27.5%;left: 0px;";
										this.PicturePath = "http://www.syyj.tglfair.com/Picture/" + text + ".jpg";
                                        this.Title = "阿里巴巴";
										Log.Debug(base.GetType().ToString(), "确认是阿里巴巴app扫码");
									}
									else
									{
										this.isAiApp = "width: 100%;position:absolute;top: 45%;left: 0px;height: 20%;font-size: 80px;text-align: center;";
										this.PicturePath = null;
										Log.Debug(base.GetType().ToString(), "其他的扫码响应");
									}
								}
                                else if(site =="")
                                {
                                    
                                }
							}
						}
						this.BgPath = "http://www.syyj.tglfair.com/Res/000000.png";
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

		// Token: 0x0400003E RID: 62
		private TexturePropertyManager _manager = new TexturePropertyManager();

		// Token: 0x0400003F RID: 63
		public string PicturePath = null;

		// Token: 0x04000040 RID: 64
		public string BgPath = null;

		// Token: 0x04000041 RID: 65
		public new string Title;

		// Token: 0x04000042 RID: 66
		public string Style = "width: 41%;position:absolute;top: 27.5%;left: 26%;";

		// Token: 0x04000043 RID: 67
		public string isAiApp = "display: none; width: 100%;position:absolute;top: 45%;left: 0px;height: 20%;font-size: 80px;text-align: center;";
	}
}