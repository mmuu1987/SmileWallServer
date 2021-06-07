<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetHtml.aspx.cs" Inherits="SmileWallServer.Webpage.GetHtml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>广州地铁博物馆</title>
    <script type="text/javascript">
		
    </script>
	<style>
		body,html{
			margin: 0px;
		}
	</style>​
</head>
<body>
    <form id="form1" runat="server">
   <div  style="width:100%;position: relative;background-color: blue;float:left;margin-top: 5px;"> 

    <img src="<%=BgPath %>" alt="" style="width:100%" />
 
    <a href=""><image src="<%= PicturePath %>"  style=" width:100%;position:absolute;top:26.5%;left:0px;width:100%;position:absolute;top:26.5%;left:0px;" />
    </a>
 
<div style="position:absolute;top:90%;left:0px;width:100%;text-align: center;font-size: 16px;font-weight: bold;">
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <%--<img src="<%=Test %>" alt="..." style="width: 100px"/>--%>
</div>
 
</div>
    </form>
</body>
</html>
