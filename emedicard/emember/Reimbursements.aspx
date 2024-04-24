<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Reimbursements.aspx.vb" Inherits="emedicard.Reimbursemts" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/emember/uctl/emember_left_menu.ascx"%>
<%@  Register TagPrefix="uc2" TagName="BasicInfo" Src="~/emember/uctl/BasicInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
	<uc:leftnav ID="LeftNav1" runat="server" />               
<!-- left menu ends -->
    <div id="content" class="span10">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnLoad">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RequiredFieldValidator2" />
                        <telerik:AjaxUpdatedControl ControlID="RequiredFieldValidator3" />
                        <telerik:AjaxUpdatedControl ControlID="btnLoad" 
                            LoadingPanelID="RadAjaxLoadingPanel2" />
                        <telerik:AjaxUpdatedControl ControlID="CustomValidator1" />
                        <telerik:AjaxUpdatedControl ControlID="grdReimIP" />
                        <telerik:AjaxUpdatedControl ControlID="grdReimOP" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
         <uc2:basicinfo ID="BasicInfo1" runat="server" />
         <div class="row-fluid sortable">
				<div class="box span12">
                    <div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Reimbursements</h2>
						<div class="box-icon">							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>							
						</div>
					</div>
                    
					<div class="box-content">
                  	    <div class="row-fluid">
<%--                            <div class="control-group">
							    <label class="control-label" for="typeahead">Date From</label>
                                <div class="controls">
                                    <telerik:RadDatePicker ID="dpFr" runat="server">
                                    </telerik:RadDatePicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Date from is required!" ControlToValidate="dpFr" 
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
                                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default">
                                    </telerik:RadAjaxLoadingPanel>
                                    <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn" />  
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="The date range must not be more than 2 years!" 
                                    CssClass="label label-warning" Display="Dynamic" ></asp:CustomValidator>
                                </div>
                                
                            </div>--%>
                            <div>
                                <asp:GridView ID="gvResult" runat="server" CssClass="table table-striped table-bordered bootstrap-datatable tblCons datatable" 
                                    EmptyDataText="No data to display" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="member_code" HeaderText="Member Code" />
                                        <asp:BoundField DataField="control_code" HeaderText="Control Code" />
                                        <asp:BoundField DataField="received_date" HeaderText="Received Date" 
                                            DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="due_date" HeaderText="Due Date" 
                                            DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="visit_date" HeaderText="Visit Date" />
                                        <asp:BoundField DataField="reim_status" HeaderText="Status" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="vdetails">Details</asp:HyperLink>
                                            </ItemTemplate>                                          
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>                  
                        </div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
