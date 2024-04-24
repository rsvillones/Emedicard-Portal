<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="AddAgentUser.aspx.vb" Inherits="emedicard.AddAgentUser" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<%@ Register TagPrefix="uc" TagName="LeftNav" Src="~/eaccount/uctl/left-menu.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav ID="LeftNav1" runat="server"/>
    <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tl:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <tl:AjaxUpdatedControl ControlID="lblMessage" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                    <tl:AjaxUpdatedControl ControlID="btnSave" />
                    <tl:AjaxUpdatedControl ControlID="lblIform" />
                </UpdatedControls>
            </tl:AjaxSetting>
        </AjaxSettings>
    </tl:RadAjaxManager>
    <tl:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </tl:RadAjaxLoadingPanel>
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
                            <label class="control-label">User name (Email)</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtEmail" runat="server" style="width: 100%; max-width: 200px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                        ErrorMessage="Email is required!" class="ValErrMsg" Display="Dynamic" 
                                        ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ErrorMessage="Invalid Email!" ControlToValidate="txtEmail" Display="Dynamic" 
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="label label-warning"></asp:RegularExpressionValidator>
                                </div>  
                            </div>
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
                            <label class="control-label">Password</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtPassword" runat="server" 
                                        style="width: 100%; max-width: 200px;" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password is required!" ControlToValidate="txtPassword" Display="Dynamic"  CssClass="label label-warning"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="6 to 18 alpha numeric" ValidationExpression="^[a-zA-Z0-9\s]{6,18}$" 
                                    CssClass="label label-warning" Display="Dynamic"></asp:RegularExpressionValidator>
                                </div>                    
                        </div>
                        <div class="control-group">
                            <label class="control-label">Confirm Password</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtConfirm" runat="server" 
                                        style="width: 100%; max-width: 200px;" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Confirm Password is required!" ControlToValidate="txtConfirm"
                                    CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password doesn't matched!" ControlToValidate="txtConfirm" 
                                    ControlToCompare="txtPassword" CssClass="label label-warning" Display="Dynamic"></asp:CompareValidator>
                                </div>                     
                        </div>
                        <div class="control-group">
                            <label class="control-label">Accounts</label>
                            <div class="controls">
                                <asp:GridView ID="dtgAcctList" runat="server" AutoGenerateColumns="False" 
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
                                        <asp:BoundField DataField="MOTHER_CODE" HeaderText="Mohter Code" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="control-group">
                        <label class="control-label">User Access</label>
                                <div class="controls">
                                        
                                        <%--<label class="checkbox ">
                                            <asp:CheckBox ID="chkAPE" runat="server" class="checkbox"/> APE
                                        </label>--%>
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkUtil" runat="server" class="checkbox"/> Utilization
                                        </label>
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkEndorse" runat="server" class="checkbox"/> Endorsement
                                        </label>
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkBenefits" runat="server" class="checkbox"/> Benefits
                                        </label>
                                        <%--<label class="checkbox ">
                                            <asp:CheckBox ID="chkID" runat="server" class="checkbox"/> ID
                                        </label>  --%>                                 

                                        <%--<label class="checkbox ">
                                            <asp:CheckBox ID="chkECU" runat="server" class="checkbox"/> ECU
                                        </label>--%>
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkActiveMem" runat="server" class="checkbox"/> Active Members
                                        </label>
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkResgnMem" runat="server" class="checkbox"/> Resigned Members
                                        </label>
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkActMem" runat="server" class="checkbox"/> Action Memo
                                        </label>                                      
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkReimbStatus" runat="server" class="checkbox"/> Reimbursements
                                        </label>   
                                        <label class="checkbox ">
                                            <asp:CheckBox ID="chkClinicResults" runat="server" class="checkbox"/> Clinic Results
                                        </label> 
                                </div>                     
                        </div> 
                        <div class="control-group">
							 
                            <div class="controls">
                                <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary" />
                                   
                            </div>
                                
                        </div>
                        <div class="width: 100%; text-align: center;">
                            <asp:Label ID="lblIform" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label> 
                        </div> 
                    </fieldset>  
                    </div>   
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->

</div>
</asp:Content>
