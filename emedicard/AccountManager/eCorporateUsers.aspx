<%@ Page Title="" Language="vb" AutoEventWireup="false"  validateRequest="false" enableEventValidation="false" MasterPageFile="~/emedicard.Master" CodeBehind="eCorporateUsers.aspx.vb" Inherits="emedicard.eCorporateUsers" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- left menu starts -->
	<uc:LeftNav ID="LeftNav1" runat="server" />
	<!-- left menu ends -->
    <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tl:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <tl:AjaxUpdatedControl ControlID="btnSave"  />
                    <tl:AjaxUpdatedControl ControlID="lblMessage" LoadingPanelID="lp" />
                    <tl:AjaxUpdatedControl ControlID="lblPasswordRequired" />                    
                </UpdatedControls>
            </tl:AjaxSetting>
        </AjaxSettings>
   </tl:RadAjaxManager>
    <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" EnableSkinTransparency="False">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </tl:RadAjaxLoadingPanel>		
			
			
	<div id="content" class="span10">
	<!-- content starts -->
		
            <uc:AcctList ID="NavAccountInfo" runat="server" />		         
	

    <div class="row-fluid sortable">
		<div class="box span12" style="width:100%">
			<div class="box-header well" data-original-title>
				<h2><i class="icon-info-sign"></i>eCorporate User</h2>
				<div class="box-icon">							
					<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>							
				</div>
			</div>
            
			<div class="box-content">                

                <%--<fieldset>
                    <asp:HiddenField ID="hdnUserID" runat="server" />
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Blue" Text="" CssClass="alert alert-info" Visible="false"></asp:Label> <br />
                    <legend>User Information <asp:Label runat="server" ID ="lblUsername" Text="" ></asp:Label></legend>                   
                    
						<asp:CheckBox ID="chkChangeUser" runat="server" Visible="False"  /> Change username (email will be your username).
					
					
                    <div class="control-group">
                        <label class="control-label" for="typeahead">Email Address:</label>
                        <div class="controls">
                            
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="input-xlarge focused" 
                                ></asp:TextBox>
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"  
                                ErrorMessage="Email" CssClass="label label-warning" 
                                ControlToValidate="txtEmailAddress"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"  
                            ErrorMessage="Invalid Email Address"  CssClass="label label-warning"  ControlToValidate="txtEmailAddress"
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="typeahead">Firstname:</label>
                        <div class="controls">
                            <asp:TextBox ID="txtFirstname" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"  
                                ErrorMessage="Firstname required" CssClass="label label-warning" 
                                ControlToValidate="txtFirstname"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="typeahead">Lastname:</label>
                        <div class="controls">
                            <asp:TextBox ID="txtLastname" runat="server" CssClass="input-xlarge focused" 
                                ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  
                                ErrorMessage="Lastname required" CssClass="label label-warning" 
                                ControlToValidate="txtLastname"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="typeahead">Designation:</label>
                        <div class="controls">
                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="input-xlarge focused" 
                                ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  
                                ErrorMessage="Designation required" CssClass="label label-warning" 
                                ControlToValidate="txtDesignation"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    <div class="control-group">
                    <asp:Label ID="lblPassword" runat="server" CssClass="control-label" Text="Password (Leave blank if no changes):" ></asp:Label>
                        
                        <div class="controls">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="input-xlarge focused" 
                                ></asp:TextBox>
                        </div>
                    </div>

                    <div class="control-group">
							  
                               <div class="controls">
                                   <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn" />                                   
                               </div>
                                
                            </div>			

                </fieldset>--%>
                
                <div class="row-fluid">
                    <asp:Button ID="btnAdd" runat="server" Text="Add New User" CssClass="btn btn-primary" /><br /><br />
                    <asp:GridView ID="dtgResult" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                        <Columns>
                            <asp:BoundField DataField="UserID" HeaderText="User ID" Visible="False" />
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                            <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                            <asp:TemplateField>
                            <ItemTemplate>
                                <a href='<%# Eval("UserID", "EditUser.aspx?j={0}") %>&t=<%= Request.QueryString("t") %>&c=<%= Request.QueryString("c")%>&u=<%= HttpUtility.UrlEncode(Request.QueryString("u")) %>&agnt=<%= Request.QueryString("agnt") %>' >Edit</a>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div> 
                                             
            </div>
        </div>
    </div>
    <!-- content ends -->			   
    </div>
</asp:Content>