<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DLPage.aspx.vb" Inherits="emedicard.DLPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8">
	<title>Medicard Philippines, Inc. - eMedicard Download Page</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<meta name="description" content="MediCard Philippines, Inc. Access your membership information. Consult online. View your medical history and availments">
	<META NAME="ROBOTS" CONTENT="INDEX, NOFOLLOW">
    <meta http-equiv="X-UA-Compatible" content="ie=9" /> 

	<!-- The styles -->
	<link id="bs_css" href="css/bootstrap-cerulean.css" rel="stylesheet">
	<style type="text/css">
	  body {
		padding-bottom: 40px;
	  }
	  .sidebar-nav {
		padding: 9px 0;
	  }
	</style>
	<link href="css/bootstrap-responsive.css" rel="stylesheet">
	<link href="css/charisma-app.css" rel="stylesheet">
	<link href="css/jquery-ui-1.8.21.custom.css" rel="stylesheet">
	<link href='css/fullcalendar.css' rel='stylesheet'>
	<link href='css/fullcalendar.print.css' rel='stylesheet'  media='print'>
	<link href='css/chosen.css' rel='stylesheet'>
	<link href='css/uniform.default.css' rel='stylesheet'>
	<link href='css/colorbox.css' rel='stylesheet'>
	<link href='css/jquery.cleditor.css' rel='stylesheet'>
	<link href='css/jquery.noty.css' rel='stylesheet'>
	<link href='css/noty_theme_default.css' rel='stylesheet'>
	<link href='css/elfinder.min.css' rel='stylesheet'>
	<link href='css/elfinder.theme.css' rel='stylesheet'>
	<link href='css/jquery.iphone.toggle.css' rel='stylesheet'>
	<link href='css/opa-icons.css' rel='stylesheet'>
	<link href='css/uploadify.css' rel='stylesheet'>
    <link href='css/eConsult.css' rel='stylesheet'>
</head>
<body>
<form id="Form1" runat="server">
    <div class="navbar">
		<div class="navbar-inner">
			<div class="container-fluid">
				<a class="btn btn-navbar" data-toggle="collapse" data-target=".top-nav.nav-collapse,.sidebar-nav.nav-collapse">
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
				</a>
				<a class="brand"> <img id="Img1" alt="Medicard Logo" src="~/img/loginbannerOld.png" style="width:180px" runat="server" /> </a>
				

				<!-- user dropdown ends -->
				
				<div class="top-nav nav-collapse">
					<ul class="nav">
						<a class="brand" href="#"><span runat="server" id="lblHead"></span> </a>
					</ul>
				</div><!--/.nav-collapse -->
			</div>
		</div>
	</div>
    <div style="padding: 20px;">
        <% If Not IsNothing(Request.QueryString("flname")) Then%>
            <% Response.Write(Request.QueryString("flname").ToString)%>
        <% End If%>
        <br />
        <%--<a href="<% Response.write("file:///\\uranus\ECorporateFileUpload\" & "20130815-112739_test upload 15.9MB.rar") %>">DL<img src="img/File-Downloads-icon.png" style="height:64px; width: 64px;" /></a>--%>
        <asp:Button ID="btnDownload" runat="server" Text="Download" />
    </div>
</form>
</body>
</html>
