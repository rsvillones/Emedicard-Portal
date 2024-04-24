<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master"
    CodeBehind="Reimbursements.aspx.vb" Inherits="emedicard.Reimbursements" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx" %>
<%@ Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
    <uc:LeftNav ID="LeftNav1" runat="server" />
    <!-- left menu ends -->
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSave" LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="CustomValidator1" />
                    <telerik:AjaxUpdatedControl ControlID="divlUtil" />
                    <telerik:AjaxUpdatedControl ControlID="UserRequest" />
                    <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                    <telerik:AjaxUpdatedControl ControlID="btnRequest" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRequest">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="UserRequest" />
                    <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                    <telerik:AjaxUpdatedControl ControlID="btnRequest" />
                </UpdatedControls>
            </telerik:AjaxSetting>
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
                    <h2>
                        <i class="icon-info-sign"></i>&nbsp;Reimbursements</h2>
                    <div class="box-icon">
                        <a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
                    </div>
                </div>

                <div class="box-content">
                    <asp:TextBox ID="txtMemberCode" type="hidden" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtMemberName" type="hidden" runat="server"></asp:TextBox>
                    <asp:HyperLink ID="gHyperLink" type="hidden" runat="server" CssClass="vdetails"></asp:HyperLink>
                    <div class="control-group">
                        <label class="control-label" for="typeahead">
                            Member </label>
                        <div class="controls">
                            <asp:DropDownList ID="ddlMember" class="js-select2-reimbmem" style="width: 200px;" runat="server">
                            </asp:DropDownList>
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
                            <asp:Button ID="btnView" runat="server" Text="View detail" CssClass="btn btn-primary" AutoPostback = false />
                        </div>
                    </div>

                    <div class="box-content">
                        <div class="row-fluid">
                            <asp:GridView ID="gvResult" runat="server" CssClass="table table-striped table-bordered bootstrap-datatable tblCons datatable"
                                EmptyDataText="No data to display" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:BoundField DataField="member_code" HeaderText="Member Code" />
                                    <asp:BoundField DataField="control_code" HeaderText="Control Code" />
                                    <asp:BoundField DataField="received_date" HeaderText="Received Date" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="due_date" HeaderText="Due Date" DataFormatString="{0:d}" />
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
        <!-- content ends -->
    </div>
</asp:Content>
