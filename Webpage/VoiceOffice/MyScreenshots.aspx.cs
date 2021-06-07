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
    public partial class MyScreenshots : System.Web.UI.Page
	{// Token: 0x06000058 RID: 88 RVA: 0x000038C8 File Offset: 0x00001AC8
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004094 File Offset: 0x00002294
		public override void ProcessRequest(HttpContext context)
		{
			Uri url = context.Request.Url;
			string str = "有数据进来 ;";
			Log.Debug(base.GetType().ToString(), str + ((url != null) ? url.ToString() : null));
			

			Common.ReadXml(base.Server);
			context.Response.StatusCode = 200;
			context.Response.StatusDescription = "OK";
			context.Response.ContentType = "text/html; charset=UTF-8";
			StreamReader streamReader = new StreamReader(context.Request.Files["PicUuidData"].InputStream);
			bool flag = streamReader.BaseStream.Length <= 0L;
			if (flag)
			{
				context.Response.StatusCode = 404;
				context.Response.StatusDescription = "not strema";
				context.Response.ContentType = ".txt";
			}
			else
			{
				string s = this.SaveUuid(context, streamReader);
				byte[] bytes = Encoding.Default.GetBytes(s);
				context.Response.BinaryWrite(bytes);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004170 File Offset: 0x00002370
		private string SaveUuid(HttpContext p, StreamReader inputData)
		{
			string className = base.GetType().ToString();
			string str = "SaveUuid:";
			Uri url = p.Request.Url;
			Log.Debug(className, str + ((url != null) ? url.ToString() : null));
			try
			{
				this._uuid = p.Request["PicUuid"];
				this._sited = p.Request["Sited"];
				byte[] array = Common.ConversionPicture(inputData);
				IList<TextureInfo> userByUsername = this._manager.GetUserByUsername(this._uuid);
				TextureInfo textureInfo = new TextureInfo();
				textureInfo.Site = this._sited;
				textureInfo.Uuid = this._uuid;
				bool flag = userByUsername.Count > 0;
				if (flag)
				{
					TextureInfo textureInfo2 = textureInfo;
					textureInfo2.Uuid += DateTime.Now.ToFileTime().ToString();
				}
				textureInfo.Url = Common.SavePicturePath + "\\" + textureInfo.Uuid + ".jpg";
				string className2 = base.GetType().ToString();
				string str2 = "POST request:";
				Uri url2 = p.Request.Url;
				Log.Debug(className2, str2 + ((url2 != null) ? url2.ToString() : null) + "\r\n    tex.Url is " + textureInfo.Url);
				bool flag2 = !Directory.Exists(Common.SavePicturePath);
				if (flag2)
				{
					Directory.CreateDirectory(Common.SavePicturePath);
				}
				Log.Debug(base.GetType().ToString(), "bytes length is " + array.Length.ToString());
				File.WriteAllBytes(textureInfo.Url, array);
				this._manager.SaveUser(textureInfo);
				return "http://www.syyj.tglfair.com/Webpage/VoiceOffice/GetVoiceOffice.aspx?html=" + textureInfo.Uuid;
			}
			catch (Exception ex)
			{
				Log.Debug(base.GetType().ToString(), ex.ToString());
			}
			return null;
		}

		// Token: 0x04000045 RID: 69
		private TexturePropertyManager _manager = new TexturePropertyManager();

		// Token: 0x04000046 RID: 70
		private string _uuid;

		// Token: 0x04000047 RID: 71
		private string _sited;

	}
}