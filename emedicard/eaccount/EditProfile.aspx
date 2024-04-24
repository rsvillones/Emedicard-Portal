<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="EditProfile.aspx.vb" Inherits="emedicard.EditProfile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/eaccount/uctl/left-menu.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<uc:LeftNav runat="server"/>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Button1">

                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="CustomValidator1" />
                    <telerik:AjaxUpdatedControl ControlID="CustomValidator2" />
                    <telerik:AjaxUpdatedControl ControlID="Button1" />
                </UpdatedControls>

            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </telerik:RadAjaxLoadingPanel>
<div id="content" class="span10">
          <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Account Officer Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label> 
                  	<div class="row-fluid" style="margin-top: 20px;">
                    <asp:Button ID="btnCancel" runat="server" Text="Back" class="btn" 
                            CausesValidation="False"/>
                    <fieldset>
                    <div class="control-group">
                        <label class="control-label">First name</label>
                            <div class="controls">
                                <asp:TextBox ID="txtFname" runat="server" style="width: 100%; max-width: 200px;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="First name is required!" ControlToValidate="txtFname" CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label">Last name</label>
                            <div class="controls">
                                <asp:TextBox ID="txtLname" runat="server" style="width: 100%; max-width: 200px;"></asp:TextBox>
                            </div>  
                    </div>
                   
                    <div class="control-group">
                        <label class="control-label">Username</label>
                            <div class="controls">
                                <asp:TextBox ID="txtUsername" runat="server" 
                                    style="width: 100%; max-width: 200px;" Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ErrorMessage="User name is required!" CssClass="label label-warning" Display="Dynamic" 
                                    ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
                            </div>                     
                    </div>
                    <div class="control-group">
                        <label class="control-label">Password</label>
                            <div class="controls">
                                <asp:TextBox ID="txtPassword" runat="server" 
                                    style="width: 100%; max-width: 200px;" TextMode="Password"></asp:TextBox> 
                                <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                    ErrorMessage="Password is required." CssClass="label label-warning" 
                                    Display="Dynamic" ControlToValidate="txtPassword"></asp:CustomValidator>                           
                            </div>                    
                    </div>
                    <div class="control-group">
                        <label class="control-label">Confirm Password</label>
                            <div class="controls">
                                <asp:TextBox ID="txtConfirm" runat="server" 
                                    style="width: 100%; max-width: 200px;" TextMode="Password"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password doesn't matched!" ControlToValidate="txtConfirm" 
                                ControlToCompare="txtPassword" CssClass="label label-warning" Display="Dynamic"></asp:CompareValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Confirm password is required." ControlToValidate="txtConfirm" 
                                 CssClass="label label-warning" Display="Dynamic"></asp:CustomValidator>
                            </div>                     
                    </div>
 
                    <div class="control-group">
                        <label class="control-label">Email</label>
                            <div class="controls">
                                <asp:TextBox ID="txtEmail" runat="server" 
                                    style="width: 100%; max-width: 200px;" Enabled="False"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ErrorMessage="Email is required!" class="ValErrMsg" Display="Dynamic" 
                                    ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                        ErrorMessage="Invalid Email!" ControlToValidate="txtEmail" Display="Dynamic" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="label label-warning"></asp:RegularExpressionValidator>
                            </div>  
                        </div>

                        <div class="control-group">
                                <div class="form-actions">
                                    <table>
                                        <tr>
                                            <td><asp:Button ID="Button1" runat="server" Text="Save" class="btn btn-primary"/></td>
                                            <td></td>
                                        </tr>
                                    </table>                               

                                </div>
                         </div>
                        <div class="control-group">
                            <div id="savemsg" runat="server">
                            
                            </div>
                        </div> 
                    </fieldset>  
                    </div>   
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->

</div>
<%--    </div>--%>
</asp:Content>
