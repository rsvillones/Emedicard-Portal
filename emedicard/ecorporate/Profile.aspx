<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Profile.aspx.vb" Inherits="emedicard.Profile1" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/ecorporate/uctl/left-menu.ascx"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
	<uc:LeftNav ID="LeftNav1" runat="server" />
	<!-- left menu ends -->

    <div id="content" class="span10">
     <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <tl:AjaxSetting AjaxControlID="txtEmailAddress">
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="txtUsername"  />
                        <tl:AjaxUpdatedControl ControlID="txtEmailAddress" />
                    </UpdatedControls>
                </tl:AjaxSetting>
                <tl:AjaxSetting AjaxControlID="btnSave">
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="CustomValidator3" />
                        <tl:AjaxUpdatedControl ControlID="CustomValidator1" />
                        <tl:AjaxUpdatedControl ControlID="CustomValidator2" />
                        <tl:AjaxUpdatedControl ControlID="lblMessage" LoadingPanelID="lp" />
                        <tl:AjaxUpdatedControl ControlID="btnSave" />
                    </UpdatedControls>
                </tl:AjaxSetting>
            </AjaxSettings>
         </tl:RadAjaxManager>
    <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" 
                EnableSkinTransparency="False">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </tl:RadAjaxLoadingPanel>

    
    <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>&nbsp;User Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                    
                        <fieldset>
                            
							<legend><asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></legend>
                            
<%--                            <asp:Button ID="btn" runat="server" Text="Back" CssClass="btn" 
                                CausesValidation="False" />		--%>			
                            
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Username</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtUsername" runat="server" CssClass="input-xlarge focused" 
                                       Enabled="False"></asp:TextBox>
                               </div>
                            </div>
                            
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Firstname</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtFirstname" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  
                                       ErrorMessage="Firstname required" CssClass="label label-warning" 
                                       ControlToValidate="txtFirstname"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Lastname</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtLastname" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  
                                       ErrorMessage="Lastname required" CssClass="label label-warning" 
                                       ControlToValidate="txtLastname"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Designation</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtDesignation" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                       ErrorMessage="Designation required" CssClass="label label-warning" 
                                       ControlToValidate="txtDesignation"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                          
                            
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Telephone No.</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtTelephone" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Fax No.</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtFaxNo" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Mobile No.</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtMobile" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                               </div>
                            </div>
                            <div class="control-group">                                  
							      <label class="control-label" for="typeahead">Email</label>
                                   <div class="controls">
                                       <asp:TextBox ID="txtEmailAddress" runat="server" 
                                           CssClass="input-xlarge focused" Enabled="False"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
                                           runat="server" ErrorMessage="Invalid email address" 
                                           CssClass="label label-warning" ControlToValidate="txtEmailAddress" 
                                           ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                           Display="Dynamic"></asp:RegularExpressionValidator>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" 
                                           CssClass="label label-warning"  runat="server" 
                                           ErrorMessage="Email address required" ControlToValidate="txtEmailAddress" 
                                           Display="Dynamic"></asp:RequiredFieldValidator>
                                       
                                   </div>
                               </div>
                              <legend>Login and Access Information</legend>
                              <p>Leave blank for no changes</p>
                              <div class="control-group">                                  
							      <div class="controls">

                                   </div>
                               </div>
<div class="control-group">
							  <label class="control-label" for="typeahead">Old Password</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="CustomValidator" 
                                   CssClass="label label-warning" Display="Dynamic" ControlToValidate="txtOldPassword"></asp:CustomValidator>

                               </div>
                            </div> 							

                            <div class="control-group">
							  <label class="control-label" for="typeahead">New Password</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" 
                                   CssClass="label label-warning" Display="Dynamic" ControlToValidate="txtNewPassword"></asp:CustomValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Confirm Password</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtConfirm" TextMode="Password" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
<%--                                   <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password does not match." ControlToValidate="txtConfirm" ControlToCompare="txtNewPassword" 
                                    CssClass="label label-warning" Display="Dynamic"></asp:CompareValidator>--%>
                                   <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Confirm password is required." 
                                   CssClass="label label-warning" Display="Dynamic"></asp:CustomValidator>
                               </div>
                            </div>
                            <div class="control-group">
							 
                               <div class="controls">
                               <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label><br />
                                   <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary" />
                                   
                               </div>
                                
                            </div>			
                           			
							
                            <%--<div class="control-group">
							  <label class="control-label" for="typeahead">Profile Picture</label>
                               <div class="controls">
                                   <tl:RadUpload ID="flProfile" runat="server"  AllowedFileExtensions = ".jpg, .png,.jpeg, .gif"
                                    MaxFileInputsCount="1"  InputSize="45"  ControlObjectsVisibility="None">
                                   </tl:RadUpload>

                               
                                <img id="image1" src="" />
                                   
                               </div>
                            </div>--%>
                          </fieldset>                        
                          
                      </div>
                            
                    </div>
                    
                </div>
              </div>
    </div>
</asp:Content>
