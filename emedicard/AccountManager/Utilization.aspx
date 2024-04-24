<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Utilization.aspx.vb" Inherits="emedicard.Utilization" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

            <!-- left menu starts -->
			<uc:LeftNav ID="LeftNav1" runat="server" />
			<!-- left menu ends -->
		
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <%--<telerik:AjaxUpdatedControl ControlID="btnSave" 
                        LoadingPanelID="RadAjaxLoadingPanel2" />--%>
                    <telerik:AjaxUpdatedControl ControlID="CustomValidator1" />
                    <telerik:AjaxUpdatedControl ControlID="divlUtil" />
                    <telerik:AjaxUpdatedControl ControlID="UserRequest" />
                    <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                    <telerik:AjaxUpdatedControl ControlID="btnRequest" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="btnRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="UserRequest" />
                    <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                    <telerik:AjaxUpdatedControl ControlID="btnRequest" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>		
			
	<div id="content" class="span10">
			<!-- content starts -->
		
                 <uc:AcctInfo ID="NavAccountInfo" runat="server" />
		  
       
			<!-- content ends -->
			   
    </div>

<div id="Div1" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;UTILIZATION REPORTING</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>

                    <div class="box-content">
                        <asp:TextBox ID="txtMemberCode" type="hidden" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtMemberName" type="hidden" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtName" type="hidden" runat="server"></asp:TextBox>

                         <div class="control-group">
							<label class="control-label" for="typeahead">Select Sevice</label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlService" runat="server">
                                    <asp:ListItem Value="util_all_service">MEMBER UTILIZATION ALL SERVICE (PER MEMBER)</asp:ListItem>
                                    <asp:ListItem Value="ip">IN-PATIENT</asp:ListItem>
                                    <asp:ListItem Value="op">OUT-PATIENT</asp:ListItem>
                                    <asp:ListItem Value="er">MEDICAL SERVICE</asp:ListItem>
                                    <asp:ListItem Value="reip">REIMBURSEMENT IN-PATIENT</asp:ListItem>
                                    <asp:ListItem Value="reop">REIMBURSEMENT OUT-PATIENT</asp:ListItem>
                                    <asp:ListItem Value="dt">DENTAL</asp:ListItem>
                                    <asp:ListItem Value="dcr">REPORTED BUT NOT YET BILLED (IN-PATIENT)</asp:ListItem>
                                    <asp:ListItem Value="calllog">REPORTED BUT NOT YET BILLED (OUT-PATIENT/MEDICAL SERVICE)</asp:ListItem>
                                </asp:DropDownList>
                                <label style="display:inline-block"><asp:CheckBox ID="chkGroup" runat="server" /> Group by Diseases </label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Service is required!" ControlToValidate="ddlService" 
                                CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div> 
                        <%--<div id="memcode" class="control-group">
							<label class="control-label" for="typeahead">Member Code</label>
                            <div class="controls">
                                <asp:TextBox ID="txtMemberCode" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
							<label class="control-label" for="typeahead">Last name</label>
                            <div class="controls">
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </div>
                        </div> --%>
                        <div class="control-group">
                            <label class="control-label" for="typeahead">
                                Member </label>
                            <div class="controls">
                                <asp:DropDownList ID="ddlMember" class="js-select2-reimbmem" style="width: 200px;" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Member is required!" ControlToValidate="ddlMember" 
                                CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="control-group">
							<label class="control-label" for="typeahead">Date From</label>
                            <div class="controls">
                                <telerik:RadDatePicker ID="dpFrom" runat="server">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Date from is required!" ControlToValidate="dpFrom" 
                                 CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="control-group">
							<label class="control-label" for="typeahead">Date To</label>
                            <div class="controls">
                                <telerik:RadDatePicker ID="dpTo" runat="server">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Date to is required!" ControlToValidate="dpTo" 
                                CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="control-group">
							 
                            <div class="controls">
                                <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default">
                                </telerik:RadAjaxLoadingPanel>--%>
                                <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="btn" />  
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" 
                                    CssClass="label label-warning" Display="Dynamic" ControlToValidate="dpFrom"></asp:CustomValidator>                               
                            </div>
                                
                        </div>

                        <div class="control-group">			 
                            <div class="controls">
                                <asp:Button ID="btnExport"  runat="server" Text="Export to Excel" CssClass="btn btn-primary"/>                             
                            </div>
                        </div>

                        
<%--                        <div style="text-align: right;">
                            <asp:Button ID="btnExport" runat="server" Text="Export to excel" />
                        </div>--%>

                        <div id="divlUtil" runat="server" style="width: 100%; position: relative; background-color: #f9f9f9;" >
                        </div>

                        <%--<div id="UserRequest" runat="server" visible="False">
                            <div class="control-group">
							    <label class="control-label" for="typeahead"></label>
                                <div class="controls" style="width: 100%; margin-top: 20px;">
                                    REQUEST FOR UTILIZATION BEYOND 2 YEARS
                                </div>
                            </div>
                            <div class="control-group">
							    <label class="control-label" for="typeahead">Remarks</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="80px" 
                                        style="width: 100%; max-width: 400px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Remarks is required!" ControlToValidate="txtRemarks" 
                                    CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div> 
                            <div class="control-group">
							 
                                <div class="controls">
                                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
                                    </telerik:RadAjaxLoadingPanel>
                                    <asp:Button ID="btnRequest" runat="server" Text="Submit" CssClass="btn" CausesValidation="False" />                                 
                                </div>
                                <div style="width: 100%; text-align: center;">
                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label>
                                </div>
                            </div>                        
                        </div>--%>

                    </div>
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
