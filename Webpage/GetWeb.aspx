<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetWeb.aspx.cs" Inherits="SmileSite.Webpage.Webpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>广州地铁博物馆</title>
    <script type="text/javascript">
		</script>
		<style>
			body,
			html {
				margin: 0px;
			}
		</style>​
</head>
<body>
		<div style="width:100%;position: relative;background-color: blue;float:left;margin-top: 5px;">

			<img src="<%=BgPath %>" alt="" style="width:100%" />

			<a href="">
				<image src="http://imagetest.gzcloudbeing.com/date1/246102.png" style="width:100%;position:absolute;top:26.5%;left:0px;" />
    		</a>
			<div style="z-index: 500; width:100%; height:48.2%; position:absolute; top:26.5%; left:0px; background-color: black;">
				<video controls="controls" src="<%=VideoPath %>"
				style="width: 100%; height:100%; position:absolute; left:0px; border: none; border-radius: 100px;">
				</video>
			</div>
			<div style="position:absolute;top:90%;left:0px;width:100%; text-align: center;font-size: 16px;font-weight: bold; ">
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></div>
		</div>
	</body>
</html>
