<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Profile.aspx.vb" Inherits="emedicard.Profile" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/emember/uctl/emember_left_menu.ascx"%>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="tl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
	<uc:leftnav ID="LeftNav1" runat="server" />               
<!-- left menu ends -->


    <div id="content" class="span10">
<%--        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>--%>

        <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default">
        </tl:RadAjaxLoadingPanel>
        <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <tl:AjaxSetting AjaxControlID="btnRequest"  >
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="lblMessage" LoadingPanelID="lp" /> 
                        <tl:AjaxUpdatedControl ControlID="btnRequest" />                           
                    </UpdatedControls>
                </tl:AjaxSetting>
                  <tl:AjaxSetting AjaxControlID="btnUpdate"  >
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="lblMessage" LoadingPanelID="lp" />                        
                         <tl:AjaxUpdatedControl ControlID="CustomValidator1" />    
                        <tl:AjaxUpdatedControl ControlID="RequiredFieldValidator3" />
                        <tl:AjaxUpdatedControl ControlID="CompareValidator1" />
                        <tl:AjaxUpdatedControl ControlID="btnUpdate" />
                        <tl:AjaxUpdatedControl ControlID="RequiredFieldValidator2" />
                    </UpdatedControls>
                </tl:AjaxSetting>   
            </AjaxSettings>
        </tl:RadAjaxManager>
         <div class="row-fluid sortable">
				<div class="box span12">                
                    <div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Profile</h2>
						<div class="box-icon">							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>							
						</div>
					</div>
					<div class="box-content">
                  	    <div class="row-fluid">
                           <asp:Label ID="lblMessage" runat="server" Text="" /> <br />
                            <tl:radtabstrip ID="RadTabStrip1" runat="server" SelectedIndex="0" 
                                MultiPageID="pg" CausesValidation="False">
                                <tabs>
                                    <tl:RadTab runat="server" Text="Personal Info" PageViewID="PageView1" 
                                        Selected="True"   >
                                        
                                    </tl:RadTab>
                                    <tl:RadTab runat="server" Text="Login Account" PageViewID="PageView2">
                                    </tl:RadTab>
                                </tabs>
                            </tl:radtabstrip>
                            <tl:radmultipage ID="pg" runat="server" SelectedIndex="0">
                                     <tl:RadPageView id="PageView1" runat="server" Selected="true"  >
                                     <br />
                                         <div class="span6">
                                             <div style="float:left; padding-right:25px">
                                                 <img src="../img/default.jpg" width="120px" />
                                             </div>
                                             <div class="acct-details">
                                                 <div class="acct-details">
                                                     <br />
                                                     <strong>
                                                     <span>Name:</span></strong>
                                                     <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                     <strong>
                                                     <span>Birthday:</span></strong> 
                                                     <asp:Label ID="lblBirthday" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                     <strong>
                                                     <span>Age:</span></strong>
                                                     <asp:Label ID="lblAge" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                     <strong>
                                                     <span>Civil Status:</span></strong> 
                                                     <asp:Label ID="lblCivilStatus" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                     <strong>
                                                     <span>Gender:</span></strong>                                                     
                                                     <asp:Label ID="lblGender" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                     <strong>
                                                     <span>Company:</span></strong>                                                     
                                                     <asp:Label ID="lblCompany" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                 </div>
                                             </div>
                                         </div>
                                         <div style="clear:both"></div>
                                         <br />
                                         <fieldset>
                                             <div class="control-group">
                                                 <div class="controls">
                                                     <span>If you need to change your personal info, please input your details that 
                                                     you want to change.</span><br />
                                                     <asp:TextBox ID="txtDetails" runat="server" Height="57px" TextMode="MultiLine" 
                                                         Width="383px" /><br />
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                         ControlToValidate="txtDetails" 
                                                         ErrorMessage="Please input your request of change of information." 
                                                         ForeColor="Red" Text="*" ValidationGroup="val1"></asp:RequiredFieldValidator>
                                                     <br />
                                                     <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                         ShowMessageBox="True" ShowSummary="False" ValidationGroup="val1" />
                                                     <asp:Button ID="btnRequest" runat="server" CssClass="btn" 
                                                         Text="Request for update" ValidationGroup="val1" />
                                                 </div>
                                             </div>
                                         </fieldset>
                                     </div>      
                                    </tl:RadPageView>
                                    <tl:RadPageView id="PageView2" runat="server" >
                                        <fieldset>
                                            <div class="control-group">
                                                <div class="controls">
                                                    <br />
                                                    <table border="0">
                                                        <tr>
                                                            <td>
                                                                Username</td>
                                                            <td>
                                                                <asp:Label ID="lblUsername" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span>Email (for email notifications):</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span>Old Password:</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtOldPword" runat="server" TextMode="Password"></asp:TextBox>
                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                                                    ControlToValidate="txtOldPword" CssClass="label label-warning" 
                                                                    Display="Dynamic" ErrorMessage="Old password doesn't match!"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span>Password:</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                                    ControlToValidate="txtPassword" CssClass="label label-warning" 
                                                                    Display="Dynamic" ErrorMessage="Password is required!"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span>Confirm Password:</span></td>
                                                            <td>
                                                                <asp:TextBox ID="txtConfirmPword" runat="server" TextMode="Password"></asp:TextBox>
                                                                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                                                    ControlToCompare="txtPassword" ControlToValidate="txtConfirmPword" 
                                                                    CssClass="label label-warning" Display="Dynamic" 
                                                                    ErrorMessage="Confirm password doesn't match!"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn" Text="Update" 
                                                                    ValidationGroup="val2" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                    ControlToValidate="txtEmail" 
                                                                    ErrorMessage="Please input your request of change of information." 
                                                                    ForeColor="Red" Text="*" ValidationGroup="val2"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>                       
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" 
                                                        ShowMessageBox="True" ShowSummary="False" ValidationGroup="val2" />
                                                </div>
                                            </div>
                                        </fieldset>                                   
                                    </tl:RadPageView>
                            </tl:radmultipage>                            
                        </div>
                    </div>
                </div>
        </div>      
    </div>
</asp:Content>
