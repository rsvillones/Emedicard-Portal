<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="ActionMemos.aspx.vb" Inherits="emedicard.ActionMemos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
<uc:LeftNav ID="LeftNav1" runat="server" />
<!-- left menu ends -->
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSubmit" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="divActMemos" />
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
                    
			<h2><i class="icon-info-sign"></i>&nbsp;ACTION MEMOS MEMBER</h2>
			<div class="box-icon">
							
				<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
			</div>
		</div>

        <div class="box-content">
            <div class="control-group">
				<label class="control-label" for="typeahead">From</label>
                <div class="controls">
                    <telerik:RadDatePicker ID="dpFrom" runat="server">
                    </telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Date from is required!" ControlToValidate="dpFrom" 
                        CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-group">
				<label class="control-label" for="typeahead">To</label>
                <div class="controls">
                    <telerik:RadDatePicker ID="dpTo" runat="server">
                    </telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Date from is required!" ControlToValidate="dpTo" 
                        CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="control-group">
				<label class="control-label" for="typeahead"></label>
                <div class="controls">
                    <asp:DropDownList ID="ddlMemType" runat="server">
                        <asp:ListItem Selected="True">PRINCIPAL</asp:ListItem>
                        <asp:ListItem>DEPENDENT</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div> 
            <div class="control-group">
                <div class="controls">
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
                    </telerik:RadAjaxLoadingPanel>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit"  CssClass="btn"/>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage=""></asp:CustomValidator>
                </div>
            </div> 

            <div style="text-align: right;">
                <asp:Button ID="btnExport" runat="server" Text="Export to excel"  CssClass="btn btn-primary"/>
            </div>
            <div id="divActMemos" runat="server">
                        
            </div>

        </div>
    </div>
    </div>
       
					<!-- content ends -->
			   
</div>

</asp:Content>
