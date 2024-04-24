<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChangeEmail.aspx.vb" Inherits="emedicard.ChangeEmail" %>
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
	<meta name="description" content="MediCard Philippines, Inc. Access your membership information. Consult online. View your medical availments">
	

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
    </script>	
</head>


<body>
<form id="Form1" runat="server">
<asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
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
					<h2>Welcome to e-MediCard</h2>
				</div><!--/span-->
			</div><!--/row-->
			
			<div class="row-fluid">
				<div class="well span5 center login-box">
					<div class="alert alert-info">
						<asp:Label ID="lblAlert" runat="server"
                            Text="Please provide your current email, password, and new email in order to change your email. Pleas use your new email to log in if the update is successful."></asp:Label>
					</div>
					<form class="form-horizontal" action="index.html" method="post">
						<fieldset>
							<div class="input-prepend" title="Current Email (User Name)" data-rel="tooltip">
								<span class="add-on"><i class="icon-user"></i></span>
                                <asp:TextBox ID="txtUsername" CssClass="input-large span10" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ErrorMessage="Email is required!" CssClass="label label-warning" Display="Dynamic" 
                                    ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ErrorMessage="Invalid Email!" ControlToValidate="txtUsername" Display="Dynamic" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="label label-warning"></asp:RegularExpressionValidator>
							</div>
							<div class="clearfix"></div>

							<div class="input-prepend" title="New Email" data-rel="tooltip">
								<span class="add-on"><i class="icon-user"></i></span>
                                <asp:TextBox ID="txtNewEmail" CssClass="input-large span10" runat="server" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ErrorMessage="Email is required!" CssClass="label label-warning" Display="Dynamic" 
                                    ControlToValidate="txtNewEmail"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                        ErrorMessage="Invalid Email!" ControlToValidate="txtNewEmail" Display="Dynamic" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="label label-warning"></asp:RegularExpressionValidator>
                                
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
                                    <label class="radio">
                                        <asp:RadioButton ID="rdEmember" runat="server" 
                                        GroupName="AccountType" Checked="true" />eMember
                                    </label>
                              
                                    <br />
                              
                                    <label class="radio">
                                        <asp:RadioButton ID="rdCorporate"  GroupName="AccountType" runat="server" />eCorporate
                                    </label>
                                    <br />
                                        <label class="radio">
                                        <asp:RadioButton ID="rdAgent"  GroupName="AccountType" runat="server" />eAccount
                                    </label>
                                </div>
                            </div>

							<div class="clearfix"></div>
                            <% If Trim(lblInform.Text) <> "" Then%>
					        <div id="divInform" runat="server" class="alert alert-info">
						        <asp:Label ID="lblInform" runat="server"
                                    Text="" Visible="False"></asp:Label>
					        </div>
                            <% End If%>
							<div class="clearfix"></div>
                            
							<p class="center span5">
                                <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="btn btn-primary" />&nbsp;
                                <asp:Button ID="btnGoTologin" runat="server" Text="Login Now" CssClass="btn btn-primary" CausesValidation="False" />
							
							</p>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage=""></asp:CustomValidator>

						</fieldset>
					</form>
				</div><!--/span-->
			</div><!--/row-->
				</div><!--/fluid-row-->
		
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
