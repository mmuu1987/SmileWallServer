<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetVoiceOffice.aspx.cs" Inherits="SmileWallServer.Webpage.VoiceOffice.GetVoiceOffice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title> <%=Title %></title>
    <script type="text/javascript">

</script>
    <style>
        body, html {
            margin: 0px;
        }
    </style>
    ​
</head>
<body>
    <form id="form1" runat="server">
   <div  style="width:100%;position: relative;background-color: white;float:left;margin-top: 5px;"> 
       
    <div style="<%=isAiApp %>">请用支付宝扫码获取图片 </div> 
    <img src="<%=BgPath %>" alt="" style="width:100%;" />

       
   
    <a href=""><image src="<%= PicturePath %>"  style="<%=Style%>" />
    </a>
 

 
</div>
    </form>
</body>
</html>
