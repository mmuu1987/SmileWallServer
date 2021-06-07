<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientGetMp4.aspx.cs" Inherits="SmileWallServer.Webpage.VoiceOffice.ClientGetMp4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title> <%=Title %></title>
    <meta name="description" property="og:description" content="不忘初心  牢记使命"/>
   
    <script type="text/javascript">

</script>
    
    <style>
        body, html {
            margin: 0px;
        }
         .aaa{
        color: white;
    font-size: 30px;
    display: block;
    width: 20%;
    height: 80px;
    background: gray;
    text-align: center;
    line-height: 80px;
    letter-spacing: 5px;
    text-decoration: none;
    position: absolute;
    top: 13%;
    left: 40%;
    }
    </style>
    ​
</head>
<body>
    <form id="form1" runat="server">
   <img src= "http://www.syyj.tglfair.com/Res/alibabalogo.jpg" style="display: none" />
   <div  style="width:100%;position: relative;background-color: white;float:left;margin-top: -200px;"> 
       
    <div style="<%=isAiApp %>"> <%=Description %> </div> 
    <img src="<%=BgPath %>" alt="" style="width:100%;" />

    <a class="aaa" href= "<%= mp4Path %>" download= "<%= UUID %>"> <%=BtnName %> </a>
   <video src="<%= PicturePath %>" controls="controls" webkit-playsinline="true" playsinline   style="<%=Style%>" autoplay="autoplay" loop="loop"  type="video/mp4"></video>
    <%--<a href="<%= PicturePath %>"><video src="<%= PicturePath %>" controls="controls" style="<%=Style%>"></video>
    </a>--%>
   

 
</div>
    </form>
</body>
</html>
