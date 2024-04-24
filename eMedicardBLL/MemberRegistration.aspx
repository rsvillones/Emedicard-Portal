<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MemberRegistration.aspx.vb" Inherits="emedicard.MemberRegistration" %>

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
	  .field-label
	  {
	      float: left;width: 120px !important; text-align:left !important;
	  }
	  .field-note 
	  {
	      font-size:0.8em;
	      font-style:italic;
	  }
	</style>
     <script type="text/javascript">
         function AlphanumericValidation(evt) {
             var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
             //User type Enter key
             if (charCode == 13) {

                 //Do something, set controls focus or do anything
                 return false;
             }
             //User can not type non alphanumeric chacacters
             if ((charCode < 48) || (charCode > 122) || ((charCode > 57) && (charCode < 65)) || ((charCode > 90) && (charCode < 97))) {
                 //Show message or do something
                 return false;
             }
         }

         function isNumberKey(evt) {

             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                 return false;

             return true;

         }

         function CheckPasswordLength(source, args) {
             if (args.Value.length < 6)
                 args.IsValid = false;
             else
                 args.IsValid = true;
         }

    </script>

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
<asp:HiddenField ID="hdnAccountCode" runat="server" />
    
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default">
            </tl:RadAjaxLoadingPanel>
             <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <tl:AjaxSetting AjaxControlID="btnCheckMembership"  >
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="rdBirthdate"  />
<tl:AjaxUpdatedControl ControlID="btnCheckMembership" LoadingPanelID="lp"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtEmailAddress"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtPassword"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtPassword2"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtFirstname"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtLastname"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtAddress"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtPhone"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtMobile"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="txtCompany"></tl:AjaxUpdatedControl>
<tl:AjaxUpdatedControl ControlID="CustomError"></tl:AjaxUpdatedControl>
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtEmailAddress"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtPassword"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtPassword2"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtFirstname"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtLastname"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="rdBirthdate"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtAddress"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtPhone"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtCompany"  />
                        </UpdatedControls>
                        <UpdatedControls >
                            <tl:AjaxUpdatedControl ControlID="txtMobile"  />
                        </UpdatedControls>
                     
                       
                        <UpdatedControls>
                            <tl:AjaxUpdatedControl ControlID="CustomError" />
                        </UpdatedControls>
                       
                    </tl:AjaxSetting>
                    
                    <tl:AjaxSetting AjaxControlID="btnSignUp">
                        <UpdatedControls>
                            <tl:AjaxUpdatedControl ControlID="CustomError" />
                        </UpdatedControls>
                    </tl:AjaxSetting>
                    
                </AjaxSettings>
            </tl:RadAjaxManager>

		<div class="container-fluid">
		<div class="row-fluid">
		
			<div class="row-fluid">
				<div class="span12 center login-header">
					<h2>Welcome to e-Medicard</h2>
				</div><!--/span-->
			</div><!--/row-->
			
			<div class="row-fluid">
				<div class="well span5 center login-box">
					<div class="alert alert-info" style="text-align:left">
						<p>
                            With <strong>eMember Services</strong>, MEDICard members can access the following: 
                            <ol type="a">
                                <li>Membership Information.</li>
                                <li>Latest Payment Made.</li>
                                <li>Medical and Dental Availments. (Principal and Dependents)</li>
                                <!--<li></li>-->
                                <li>Reimbursement Status/Details.</li>

                            </ol>
                            <span  runat="server" id="CustomError" style="color:Red"></span>
                        </p>

					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                            ShowMessageBox="True" ShowSummary="False" />

					</div>
					<form class="form-horizontal" action="index.html" method="post">
						<fieldset style="text-align:left">
                            <h3>Account Information</h3>
							<div class="input-prepend" title="Member Code" data-rel="tooltip">
								<span class="field-label add-on" >Member Code</span>
                                <asp:TextBox ID="txtMemberCode" CssClass="input-small span10" runat="server" Width="150px" ></asp:TextBox>                                
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Member code is already registered!"></asp:CustomValidator> 
							</div>
                            <div class="clearfix"></div>

                            <div class="input-prepend" title="Birthdate" data-rel="tooltip">
                                <table>
                                    <td valign="top">
                                        <span class="add-on field-label" style="display: inline">Birthdate</span>
                                    </td>
                                    <td>
                                        <tl:RadDatePicker ID="rdBirthdate" runat="server" MinDate="1900-12-31" >
                                        </tl:RadDatePicker>(mm/dd/yyyy)
                                    </td>
                                </table>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" ErrorMessage="Birthdate required" Text="*"  ControlToValidate="rdBirthDate"></asp:RequiredFieldValidator>
                                <asp:Button ID="btnCheckMembership" CssClass="btn btn-primary" runat="server" 
                                    Text="Check Membership" CausesValidation="false" />
                             </div>
                             
                             
							<div class="clearfix"></div>
                            <br />
							<div class="input-prepend" title="Email Address" data-rel="tooltip">
								<span class="add-on field-label">Email Address</span>
                                <asp:TextBox  ID="txtEmailAddress" CssClass="input-small span10"  ReadOnly="true" runat="server" Width="200" ></asp:TextBox>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator1" runat="server" 
                                    ErrorMessage="Email address required" ForeColor="Red" Text="*" ControlToValidate="txtEmailAddress" 
                                   ></asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ErrorMessage="Invalid email address" Display="Dynamic" 
                                    ControlToValidate="txtEmailAddress"  Text="*"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ForeColor="Red"></asp:RegularExpressionValidator>
                                
                                <!--input-small span10-->
							</div>

                            <div class="clearfix"></div>

                            <div class="input-prepend" title="Password" data-rel="tooltip">
                            
								<span class="add-on field-label">Password</span>
                                <asp:TextBox ID="txtPassword" TextMode="Password"  ReadOnly="true" CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>  <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2" runat="server" 
                                    ErrorMessage="Password required " ControlToValidate="txtPassword" 
                                    Text ="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator runat="server" id="cusCustom" 
                                    controltovalidate="txtPassword" ClientValidationFunction="CheckPasswordLength" OnServerValidate="cusCustom_ServerValidate"
                                    errormessage="The password should have minimum 6 characters long" 
                                    ForeColor="Red" SetFocusOnError="True">*</asp:CustomValidator>
                                <br /> <span class="field-note"> Minimum of 6 alpha-numeric characters </span>                               
							</div>
                            <div class="clearfix"></div>
                            <div class="input-prepend" title="Confirm Password" data-rel="tooltip">
								<span class="add-on field-label">Confirm Password</span>
                                <asp:TextBox ID="txtPassword2" TextMode="Password" ReadOnly="true" CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>                                
                                 <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                    ErrorMessage="Password does not matched. Confirm your password corrrectly" 
                                    ControlToCompare="txtPassword" ControlToValidate="txtPassword2" 
                                    SetFocusOnError="True" ForeColor="Red">*</asp:CompareValidator>
							</div>
                            <div class="clearfix"></div>

                            <h3>Personal Information</h3>
                            <div class="input-prepend" title="Firstname" data-rel="tooltip">								
                                <span class="add-on field-label">Firstname</span>
                                <asp:TextBox ID="txtFirstname"  ReadOnly="true" CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Text="*"  
                                    runat="server" ErrorMessage="Firstname required" 
                                    ControlToValidate="txtFirstname" SetFocusOnError="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ErrorMessage="Invalid data in firstname" Text="*" 
                                    ControlToValidate="txtFirstname" ValidationExpression="[^'><=]*" 
                                    ForeColor="Red"></asp:RegularExpressionValidator>
							</div>
                            <div class="clearfix"></div>

                            <div class="input-prepend" title="Lastname" data-rel="tooltip">								
                                <span class="add-on field-label">Lastname</span>
                                <asp:TextBox ID="txtLastname" ReadOnly="true"  CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>                                
                                <asp:RequiredFieldValidator ControlToValidate="txtLastname" ForeColor="Red" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Lastname required" Text="*"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                    ErrorMessage="Invalid data in Lastname" Text="*" 
                                    ControlToValidate="txtLastname" ValidationExpression="[^'><=]*" ForeColor="Red"></asp:RegularExpressionValidator>
							</div>
                            
                            
                            
                            <div class="clearfix"></div>


                            <div class="input-prepend" title="Home Address" data-rel="tooltip">								                              
                                    <span class="add-on field-label">Home Address</span>
                                <asp:TextBox ID="txtAddress" ReadOnly="true"  CssClass="input-small span10" runat="server" Width="300" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                    runat="server" ForeColor="Red" ErrorMessage="Address required"  Text="*" ControlToValidate="txtAddress"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                        Text="*" ErrorMessage="Invalid data in address" ControlToValidate="txtAddress" 
                                        ValidationExpression="[^'><=]*" ForeColor="Red"></asp:RegularExpressionValidator>
							</div>
                            <div class="clearfix"></div>

                            <div class="input-prepend" title="Phone No." data-rel="tooltip">
								<span class="add-on field-label">Phone No.</span>
                                <asp:TextBox ID="txtPhone" ReadOnly="true"  CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>  
                                &nbsp;
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" 
                                    runat="server"  Text="*" ErrorMessage="Invalid data in phone number" 
                                    ControlToValidate="txtPhone" ValidationExpression="[^'><=]*" ForeColor="Red"></asp:RegularExpressionValidator> 
							</div>
                            <div class="clearfix"></div>
							
                            <div class="input-prepend" title="Mobile No." data-rel="tooltip">
								<span class="add-on field-label">Mobile No. </span>
                                <asp:TextBox ID="txtMobile" ReadOnly="true" CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>                                
                                &nbsp;
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                                    Text="*" ErrorMessage="Invalid data in cellphone number" 
                                    ControlToValidate="txtMobile" ValidationExpression="[^'><=]*" ForeColor="Red"></asp:RegularExpressionValidator>
							</div>
                            <div class="clearfix"></div>

                            <div class="input-prepend" title="Company" data-rel="tooltip">
								<span class="add-on field-label">Company </span>
                                <asp:TextBox ID="txtCompany"  ReadOnly="true" CssClass="input-small span10" runat="server" Width="200" ></asp:TextBox>                                
							</div>
                            <div class="clearfix"></div>
                            
                            <p class="center span5">
                                <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" 
                                    CssClass="btn btn-primary" />							
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
	<!-- rich text editor library -->
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
</body>

</html>
