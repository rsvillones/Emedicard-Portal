        <%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="emedicard.Login" %>
        <%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html lang="en">
<head>
	<!--
		Charisma v1.0.0

		Copyright 2012 Muhammad Usman
		Licensed under the Apache License v2.0
		http://www.apache.org/licenses/LICENSE-2.0

		http://usman.it
		http://twitter.com/halalit_usman
	-->
	<meta charset="utf-8">
	<title>Medicard Philippines, Inc. - eMedicard</title>
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

	<!-- The HTML5 shim, for IE6-8 support of HTML5 elements -->
	<!--[if lt IE 9]>
	  <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
	<![endif]-->

	<!-- The fav icon -->
	<link rel="shortcut icon" href="img/favicon.ico">
    <script src="js/md5.js"></script>
	<script language="javascript">
	    function onSubmit() {
	        document.getElementById("txtPassword").value = hex_md5(document.getElementById("txtPassword").value);
	        
	        return true;
	    }
        
        <!--[if !IE]><!-->
        if (/*@cc_on!@*/false) {
            document.documentElement.className+=' ie10';
        }
        <!--<![endif]-->
    </script>	
</head>


<body>
<form runat="server" defaultbutton="btnLogin">
<asp:scriptmanager runat="server"></asp:scriptmanager>
      <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <tl:AjaxSetting AjaxControlID="btnLogin">
                        <UpdatedControls>
                            <tl:AjaxUpdatedControl ControlID="btnLogin" LoadingPanelID="lp" />
                            <tl:AjaxUpdatedControl ControlID="lblAlert" />
                            
                        </UpdatedControls>
                    </tl:AjaxSetting>
                </AjaxSettings>
             </tl:RadAjaxManager>
        <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" 
                    EnableSkinTransparency="True">
            <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
        </tl:RadAjaxLoadingPanel>
		<div class="container-fluid">
		<div class="row-fluid">
		
			<div class="row-fluid">
				<div class="span12 center login-header">
                    <img src="img/eMedicard-01.png" />
					
				</div><!--/span-->
			</div><!--/row-->
			<br /><br /><br /><br /><br /><br /><br />
			<div class="row-fluid">
				<div class="well span5 center login-box">
                
					<div class="alert alert-info">
						<asp:Label ID="lblAlert" runat="server"
                            Text="Please login with your Username and Password."></asp:Label>
					</div>
					<form class="form-horizontal" action="index.html" method="post">
						<fieldset>
							<div class="input-prepend" title="Username" data-rel="tooltip">
								<span class="add-on"><i class="icon-user"></i></span>
                                <asp:TextBox ID="txtUsername" CssClass="input-large span10" runat="server" ></asp:TextBox>
                                
							</div>
							<div class="clearfix"></div>

							<div class="input-prepend" title="Password" data-rel="tooltip">
								<span class="add-on"><i class="icon-lock"></i></span>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="input-large span10" 
                                    TextMode="Password"></asp:TextBox>
                                
							</div>
							<div class="clearfix"></div>

							
                            <div class="control-group" style="text-align:left">
                                <div class="controls">
                                    <table width="100%"  >
                                        <tr>
                                            <td align="center"><img src="img/eMember.png" /></td>
                                            <td align="center"><img src="img/eCorporate.png" /></td>
                                            <td align="center"><img src="img/eAccount.png" /></td>
                                        </tr>
                                        <tr>
                                            <td align="center"> 
                                                <label class="radio">
                                                    <asp:RadioButton ID="rdEmember" runat="server" GroupName="AccountType" Checked="true" />
                                                </label>
                                            </td>
                                            <td align="center">
                                                <label class="radio">
                                                    <asp:RadioButton ID="rdCorporate"  GroupName="AccountType" runat="server" />
                                                </label>
                                            </td>
                                            <td align="center">
                                                <label class="radio">
                                                    <asp:RadioButton ID="rdAgent"  GroupName="AccountType" runat="server" />
                                                </label>
                                            </td>
                                        </tr>
                                    </table>                                                                            
                                </div>
                            </div>
                            <div class="input-prepend" style="text-align:left">
                            <span><a href ='~/ForgotPassword.aspx?t=1' runat="server">&nbsp;&nbsp;Forgot Password</a></span><br /> &nbsp;
                            <span>
                                <a id="usrseting" href ='~/ChangeEmail.aspx?t=1' runat="server" 
                                    visible="True">Change Email</a></span><br /> &nbsp;
                            <span>Not yet registered?<a id="A1" href ='~/MemberRegistration.aspx' runat="server">&nbsp;Sign up</a> here.</span>

							</div>
							<div class="clearfix"></div>
                            
							<p class="center span5">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" />							
                                
							</p>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage=""></asp:CustomValidator>

						</fieldset>
					</form>
                    <div id="dialog" title="Browser Compatibility Issue" style="display:none">
                        <p>We just want to inform you that eMedicard may not work with your web browser (Internet Explorer 10). We recommend Google Chrome, FireFox, and IE 8/9. </p>
                    </div>
                    
                    
				</div><!--/span-->
			</div><!--/row-->
				</div><!--/fluid-row-->
		            <div style="margin-left:auto; margin-right:auto; width:150px">                        
                        <!--- DO NOT EDIT - GlobalSign SSL Site Seal Code - DO NOT EDIT --->
                            <table width=130 border=0 cellspacing=0 cellpadding=0 title="CLICK TO VERIFY: This site uses a GlobalSign SSL Certificate to secure your personal information." ><tr><td><span id="ss_img_wrapper_130-65_image_en"><a href="http://www.globalsign.com/" target=_blank title="SSL"><img alt="SSL" border=0 id="ss_img" src="//seal.globalsign.com/SiteSeal/images/gs_noscript_130-65_en.gif"></a></span><script type="text/javascript" src="//seal.globalsign.com/SiteSeal/gs_image_130-65_en.js"></script><br /><a href="http://www.globalsign.com/" target=_blank  style="color:#000000; text-decoration:none; font:bold 8px arial; margin:0px; padding:0px;">Get GlobalSign SSL Certificates</a></td></tr></table>
                        <!--- DO NOT EDIT - GlobalSign SSL Site Seal Code - DO NOT EDIT --->
                    </div>
	</div><!--/.fluid-container-->

	<!-- external javascript
	================================================== -->
	<!-- Placed at the end of the document so the pages load faster -->

	<!-- jQuery -->
	<script src="js/jquery-1.7.2.min.js"></script>
	<!-- jQuery UI -->
	<script src="js/jquery-ui-1.8.21.custom.min.js"></script>
	<!-- transition / effect library -->
	<script src="js/bootstrap-transition.js"></script>
	<!-- alert enhancer library -->
	<script src="js/bootstrap-alert.js"></script>
	<!-- modal / dialog library -->
	<script src="js/bootstrap-modal.js"></script>
	<!-- custom dropdown library -->
	<script src="js/bootstrap-dropdown.js"></script>
	<!-- scrolspy library -->
	<script src="js/bootstrap-scrollspy.js"></script>
	<!-- library for creating tabs -->
	<script src="js/bootstrap-tab.js"></script>
	<!-- library for advanced tooltip -->
	<script src="js/bootstrap-tooltip.js"></script>
	<!-- popover effect library -->
	<script src="js/bootstrap-popover.js"></script>
	<!-- button enhancer library -->
	<script src="js/bootstrap-button.js"></script>
	<!-- accordion library (optional, not used in demo) -->
	<script src="js/bootstrap-collapse.js"></script>
	<!-- carousel slideshow library (optional, not used in demo) -->
	<script src="js/bootstrap-carousel.js"></script>
	<!-- autocomplete library -->
	<script src="js/bootstrap-typeahead.js"></script>
	<!-- tour library -->
	<script src="js/bootstrap-tour.js"></script>
	<!-- library for cookie management -->
	<script src="js/jquery.cookie.js"></script>
	<!-- calander plugin -->
	<script src='js/fullcalendar.min.js'></script>
	<!-- data table plugin -->
	<script src='js/jquery.dataTables.min.js'></script>

	<!-- chart libraries start -->
	<script src="js/excanvas.js"></script>
	<script src="js/jquery.flot.min.js"></script>
	<script src="js/jquery.flot.pie.min.js"></script>
	<script src="js/jquery.flot.stack.js"></script>
	<script src="js/jquery.flot.resize.min.js"></script>
	<!-- chart libraries end -->

	<!-- select or dropdown enhancer -->
	<script src="js/jquery.chosen.min.js"></script>
	<!-- checkbox, radio, and file input styler -->
	<script src="js/jquery.uniform.min.js"></script>
	<!-- plugin for gallery image view -->
	<script src="js/jquery.colorbox.min.js"></script>
	<!-- rich text editor libfrary -->
	<script src="js/jquery.cleditor.min.js"></script>
	<!-- notification plugin -->
	<script src="js/jquery.noty.js"></script>
	<!-- file manager library -->
	<script src="js/jquery.elfinder.min.js"></script>
	<!-- star rating plugin -->
	<script src="js/jquery.raty.min.js"></script>
	<!-- for iOS style toggle switch -->
	<script src="js/jquery.iphone.toggle.js"></script>
	<!-- autogrowing textarea plugin -->
	<script src="js/jquery.autogrow-textarea.js"></script>
	<!-- multiple file upload plugin -->
	<script src="js/jquery.uploadify-3.1.min.js"></script>
	<!-- history.js for cross-browser state change on ajax -->
	<script src="js/jquery.history.js"></script>
	<!-- application script for Charisma demo -->
	<script src="js/charisma.js"></script>
	<script src="js/init.js"></script>
    <center>
    <footer>
			<p>MediCard Philippines, Inc. <asp:Label ID="lblYear" runat="server" Text="Label"></asp:Label></p>
			
    </footer>	
    </center>
</form>	

<script>
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
  m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

    ga('create', 'UA-40523015-1', 'medicardphils.com');
    ga('send', 'pageview');

</script>
</body>

</html>
