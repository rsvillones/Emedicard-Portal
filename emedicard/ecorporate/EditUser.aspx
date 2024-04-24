<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="EditUser.aspx.vb" Inherits="emedicard.EditUser" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<%@ Register TagPrefix="uc" TagName="LeftNav" Src="~/ecorporate/uctl/left-menu.ascx"%>

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
                    </UpdatedControls>
                </tl:AjaxSetting>
                <tl:AjaxSetting AjaxControlID="chkALL">
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="chkALL" />
                        <tl:AjaxUpdatedControl ControlID="dtgPlans" />
                    </UpdatedControls>
                </tl:AjaxSetting>
                <tl:AjaxSetting AjaxControlID="btnSubmit">
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="dtgUserAcct" />
                        <tl:AjaxUpdatedControl ControlID="dtgAccounts" />
                        <tl:AjaxUpdatedControl ControlID="chkALL" />
                        <tl:AjaxUpdatedControl ControlID="dtgPlans" />
                        <tl:AjaxUpdatedControl ControlID="savemsg" />
                        <tl:AjaxUpdatedControl ControlID="btnSubmit" />
                        <tl:AjaxUpdatedControl ControlID="lblMessage" 
                            LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </tl:AjaxSetting>
                <tl:AjaxSetting AjaxControlID="Button1">
                    <UpdatedControls>
                        <tl:AjaxUpdatedControl ControlID="dtgUserAcct" />
                        <tl:AjaxUpdatedControl ControlID="dtgAccounts" />
                        <tl:AjaxUpdatedControl ControlID="savemsg" />
                        <tl:AjaxUpdatedControl ControlID="btnSubmit" />
                        <tl:AjaxUpdatedControl ControlID="Button1" />
                    </UpdatedControls>
                </tl:AjaxSetting>
            </AjaxSettings>
         </tl:RadAjaxManager>
    <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" 
                EnableSkinTransparency="False">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </tl:RadAjaxLoadingPanel>

                <asp:HiddenField ID="hdnType" runat="server" />
                <asp:HiddenField ID="hdnCorporateCode" runat="server" />
                <asp:HiddenField ID="hdnUsername" runat="server" />
                <asp:HiddenField ID="hdnUserID" runat="server" />
                <asp:HiddenField ID="hdnAccessLevel" runat="server" />

			<div id="content" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;Edit User</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
                    
					<div class="box-content">
                        <tl:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1">
                            <Tabs>
                                <tl:RadTab runat="server" Text="User Information" PageViewID="RadPageView1" Selected="True">
                                </tl:RadTab>
                                <tl:RadTab runat="server" Text="User Accounts" PageViewID="RadPageView2">
                                </tl:RadTab>
                            </Tabs>
                        </tl:RadTabStrip>
                        <tl:RadMultiPage ID="RadMultiPage1" runat="server">
                            <tl:RadPageView ID="RadPageView1" runat="server" Selected="True"><fieldset><legend>
                                    <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></legend>
                                    <asp:Button ID="btn" runat="server" Text="Back" CssClass="btn" />
                                                <div class="control-group"><label class="control-label" for="typeahead">Firstname</label> 
                                                    <div class="controls"><asp:TextBox ID="txtFirstname" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  
                                               ErrorMessage="Firstname required" CssClass="label label-warning" 
                                               ControlToValidate="txtFirstname"></asp:RequiredFieldValidator></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">Lastname</label> 
                                        <div class="controls"><asp:TextBox ID="txtLastname" runat="server" 
                                                CssClass="input-xlarge focused"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="txtLastname" CssClass="label label-warning" 
                                                ErrorMessage="Lastname required"></asp:RequiredFieldValidator></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">Designation</label> 
                                        <div class="controls"><asp:TextBox ID="txtDesignation" runat="server" 
                                                CssClass="input-xlarge focused"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="txtDesignation" CssClass="label label-warning" 
                                                ErrorMessage="Designation required"></asp:RequiredFieldValidator></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">Company</label> 
                                        <div class="controls"><asp:DropDownList ID="cboCompany" runat="server" 
                                                CssClass="span6 typeahead"></asp:DropDownList></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">Telephone No.</label> 
                                        <div class="controls"><asp:TextBox ID="txtTelephone" runat="server" 
                                                CssClass="input-xlarge focused"></asp:TextBox></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">Fax No.</label> 
                                        <div class="controls"><asp:TextBox ID="txtFaxNo" runat="server" 
                                                CssClass="input-xlarge focused"></asp:TextBox></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">Mobile No.</label> 
                                        <div class="controls"><asp:TextBox ID="txtMobile" runat="server" 
                                                CssClass="input-xlarge focused"></asp:TextBox></div></div>
                                    <div class="control-group"><label class="control-label" for="typeahead">User Name/Email Address</label> 
                                        <div class="controls">
                                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="input-xlarge focused" 
                                                Enabled="False"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="txtEmailAddress" CssClass="label label-warning" 
                                                Display="Dynamic" ErrorMessage="Invalid email address" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                ControlToValidate="txtEmailAddress" CssClass="label label-warning" 
                                                Display="Dynamic" ErrorMessage="Email address required"></asp:RequiredFieldValidator></div></div>
                                    <div class="control-group"><label class="control-label">Accessibility</label> <div 
                                            class="controls">
                                            <%--<label class="checkbox">
                                            <asp:CheckBox ID="chkAPE" runat="server" />Annual Physical Examination (APE) Scheduling </label>--%>
                                            <label class="checkbox"><asp:CheckBox ID="chkUtil" runat="server" />Utilization Reporting </label>
                                            <label class="checkbox">
                                            <asp:CheckBox ID="chkEndorsement" runat="server" />Membership Endorsement </label>
                                            <label class="checkbox"><asp:CheckBox ID="chkBenefits" runat="server" />Benefits and Exclusions </label>
                                            <%--<label class="checkbox"><asp:CheckBox ID="chkECU" runat="server" />Executive Check-Up (ECU) Scheduling </label>--%>
                                            <%--<label class="checkbox"><asp:CheckBox ID="chkRequest" runat="server" />Request for ID Replacement </label>--%>
                                            <label class="checkbox"><asp:CheckBox ID="chkActive" runat="server" />Active Members </label>
                                            <label class="checkbox"><asp:CheckBox ID="chkResigned" runat="server" />Resigned Members </label>
                                            <label class="checkbox">
                                            <asp:CheckBox ID="chkActionMemos" runat="server" />Action Memos </label>
                                            <label class="checkbox"><asp:CheckBox ID="chkReimbStatus" runat="server" />Reimbursements </label>
                                            <label class="checkbox"><asp:CheckBox ID="chkClinicResults" runat="server" />Clinic Results </label>
                                            </div></div>
                                    <div class="control-group"><div class="controls"><asp:Button ID="btnSave" 
                                            runat="server" CssClass="btn btn-primary" Text="Save Changes" /></div></div></fieldset></tl:RadPageView>
                            <tl:RadPageView ID="RadPageView2" runat="server"><legend>User Assigned Accounts</legend>
                                <div style="padding: 10px;"><asp:GridView ID="dtgUserAcct" runat="server" 
                                        AutoGenerateColumns="False" 
                                        CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                        EmptyDataText="No Data to display."><Columns>
                                        <asp:BoundField DataField="id" HeaderText="ID" />
                                        <asp:BoundField DataField="AccountName" HeaderText="Account Name" />
                                        <asp:BoundField DataField="AccountCode" HeaderText="Account Code" />
                                        <asp:BoundField DataField="AccountCategory" HeaderText="Category" />
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderTemplate><asp:Button ID="ButtonDelete" runat="server" autopostback="true" 
                                                    CausesValidation="False" CommandName="Delete" Text="Delete" /></HeaderTemplate>
                                            <ItemTemplate><asp:CheckBox ID="chkSelect" runat="server" /></ItemTemplate></asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit Plan">
                                            <ItemTemplate><asp:HyperLink ID="lPlan" runat="server" CssClass="EditPlan">Edit</asp:HyperLink></ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns></asp:GridView></div>

                                    <legend>Available Accounts</legend>
                                    <div style="padding: 10px;">
                                        <asp:GridView ID="dtgAccounts" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                            <Columns>
                                        
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                        
                                                <asp:BoundField DataField="NAME" HeaderText="Account Name" />
                                                <asp:BoundField DataField="CODE" HeaderText="Account Code" />
                                        
                                                <asp:BoundField DataField="ACCT_CATEGORY" HeaderText="Category" />
                                                <asp:BoundField DataField="MOTHER_CODE" HeaderText="Mother Code" />
                                        
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <legend>Plans to utilize</legend>	
                                    <div class="control-group">
                                         <table>
                                            <tr><td><asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" /></td><td>Select All</td></tr>
                                         </table>
                                        <asp:GridView ID="dtgPlans" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                            <Columns>
                                                <asp:BoundField DataField="RSPROOMRATE_ID" HeaderText="Room Rate ID" />
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkPlanSelect" runat="server" />
                                                    </ItemTemplate>                                        
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PLAN_DESC" HeaderText="Plan" />
                                                <asp:BoundField DataField="RNB_FOR" HeaderText="RNB For" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                   <div class="control-group">
							 
                                        <div class="controls">
                                            <input id="savemsg" runat="server" type="hidden" value="" />
                                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" 
                                                Text="Save Changes" />
                                        </div>
                                
                                  </div>
                                </tl:RadPageView>
                        </tl:RadMultiPage>
                          <asp:Button ID="Button1" runat="server" Text="Button Cancel Hidden"  
                            style="display: none;" CausesValidation="False"/> 
                                                 
                        <div class="control-group">
                            <div class="controls">
                                <tl:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
                                </tl:RadAjaxLoadingPanel>
                                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div id="divPlan">
                        
                        </div>
                    </div>
                    
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>

