<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master"
    CodeBehind="MyCureAPE.aspx.vb" Inherits="emedicard.MyCureAPE" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" %>

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

    <style type="text/css">
      @media print {
          body * {
            visibility: hidden;
          }
          #divRptMCR, #divRptMCR * {
            visibility: visible;
          }
          #divRptMCR {
            position: absolute;
            left: 0;
            top: 0;
          }
        }
    </style>

    <script language="javascript">
	    function exportExcel() {
	        $find('ctl00_MainContent_ClinicResultsViewer').exportReport('EXCELOPENXML');

	        return false;
	    }

	    function exportPdf() {
	        $find('ctl00_MainContent_ClinicResultsViewer').exportReport('PDF');

	        return false;
	    }

	    function printWindow() {
	        window.print()
	    }
    </script>

    <div id="Div1" class="span10">
        <!-- content starts -->
        <div class="row-fluid sortable">
            <div class="box span12">
                <div class="box-header well" data-original-title>
                    <h2>
                        <i class="icon-info-sign"></i>&nbsp;Medicard Clinics Results</h2>
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
                            <asp:DropDownList ID="ddlMember" class="js-select2-mycurepme" style="width: 200px;" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="control-group">
                        <label class="control-label" for="typeahead">
                            Report type </label>
                        <div class="controls">
                            <asp:DropDownList ID="ddlPMEtype" style="width: 300px;" runat="server">
                                <%--<asp:ListItem Value="ape">ANNUAL PHYSICAL EXAM</asp:ListItem>
                                <asp:ListItem Value="pe">PRE-EMPLOYMENT</asp:ListItem>
                                <asp:ListItem Value="ecu">EXECUTIVE CHECK-UP</asp:ListItem>--%>
                                <asp:ListItem Value="apesum">SUMMARY - ANNUAL PHYSICAL EXAM</asp:ListItem>
                                <asp:ListItem Value="pesum">SUMMARY - PRE-EMPLOYMENT</asp:ListItem>
                                <asp:ListItem Value="ecusum">SUMMARY - EXECUTIVE CHECK-UP</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Report type is required!" ControlToValidate="ddlPMEtype" 
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
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" AutoPostback = false />
                            &nbsp;<asp:Button ID="btnView" runat="server" Text="View detail" CssClass="btn btn-primary" AutoPostback = false />
                            &nbsp;<asp:Button ID="btnPrint" Visible="false" OnClientClick="printWindow()" runat="server" Text="Print" CssClass="btn btn-primary" AutoPostback = false />
                            &nbsp;<asp:Button ID="btnExportExcel" Visible="false" OnClientClick="exportExcel()" runat="server" Text="Export to Excel" CssClass="btn btn-primary" AutoPostback = false />
                            &nbsp;<asp:Button ID="btnExportPDF" Visible="false" OnClientClick="exportPdf()" runat="server" Text="Export to PDF" CssClass="btn btn-primary" AutoPostback = false />
                        </div>
                    </div>

                    <div class="box-content">
                        <div class="row-fluid">
                            <div class="control">
                                <asp:Button ID="btnPrev" runat="server" Text="Prev" CssClass="btn btn-primary" AutoPostback = false />
                                &nbsp;<asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary" AutoPostback = false />
                                &nbsp;<label class="control-label" id="lblShow"></label>
                            </div>
                            
                            <%-- start  --%>

                            <div id="divRptMCR"><asp:Literal ID="rpt" runat="server"></asp:Literal></div>
                            <rsweb:ReportViewer ID="ClinicResultsViewer" runat="server" hidden="hidden" Width="100%" Height="100%"></rsweb:ReportViewer>

                            <%-- end --%>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <!-- content ends -->
    </div>

</asp:Content>
