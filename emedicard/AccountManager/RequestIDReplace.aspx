<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="RequestIDReplace.aspx.vb" Inherits="emedicard.RequestIDReplace" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <!-- left menu starts -->
			<uc:LeftNav ID="LeftNav1" runat="server" />
			<!-- left menu ends -->
		
			
			
	<div id="content" class="span10">
			<!-- content starts -->
		
                 <uc:AcctInfo ID="NavAccountInfo" runat="server" />
		  
       
			<!-- content ends -->
			   
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Button1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvResult" />
                    <telerik:AjaxUpdatedControl ControlID="Button1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
<div id="Div1" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;Request ID Replacement</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
                    
					<div class="box-content">
                                       
                        <fieldset>
                            <div class="control-group">
                                <label class="control-label" for="typeahead">Last Name</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtLName" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Last Name is required" ControlToValidate="txtLName" 
                                        Display="Dynamic" CssClass="label label-warning"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="typeahead">First Name</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtFName" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator2" runat="server" ErrorMessage="First Name is required" ControlToValidate="txtFName" 
                                        Display="Dynamic" CssClass="label label-warning"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="typeahead">Birth Date</label>
                                <div class="controls">
<%--                                    <telerik:RadDatePicker ID="dpBirthDate" runat="server">
                                    </telerik:RadDatePicker>--%>
                                    <telerik:RadDateInput ID="diBirthDate" runat="server" MinDate="1920-01-01">
                                    </telerik:RadDateInput>&nbsp;(mm/dd/yyyy)
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Birth Date is required" Display="Dynamic" 
                                    ControlToValidate="diBirthDate"  CssClass="label label-warning"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            
                            <div class="control-group">
                                <div class="controls">
<%--                                    <label class="radio">
                                        <asp:RadioButton ID="rbOP" runat="server" Enabled="False" /> PAYplus+ (Online Payment)                                    
                                    </label><br />--%>
                                    <label class="radio">
                                        <asp:RadioButton ID="rbOTC" runat="server" Checked="True" /> Bank (Over the Counter)
                                        <asp:CustomValidator ID="filechecker" runat="server" ErrorMessage="Image with .jpg extension is allowed." 
                                        CssClass="label label-warning" Display="Dynamic"></asp:CustomValidator>
                                    </label>
                                    
                                </div>
                            </div>
                            <div class="control-group">
							    <label class="control-label" for="typeahead">Payment Slip</label>
                               <div class="controls">
                                   <asp:FileUpload ID="FileUpload1" runat="server" />&nbsp;&nbsp;Select file to upload(.jpeg,.jpg,.png,.doc,.docx)
                                   <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Invalid file attachment." 
                                   CssClass="label label-warning" Display="Dynamic" ></asp:CustomValidator>
                               </div>
                                
                            </div>

                            <div class="control-group">
							 
                               <div class="controls">
                                   <asp:Button ID="btnSave" runat="server" Text="Add Request" CssClass="btn" OnClientClick="if(!confirm('Would you like to save request?')) return false; "/>                                  
                               </div>
                                
                            </div>
                            <div class="control-group">					 
                               <div class="controls">
                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label><br />                                   
                               </div>                                
                            </div>
                            <div>
                                <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    EmptyDataText="No data to display">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="ID" ControlStyle-CssClass="apeid" />
                                        <asp:BoundField DataField="RequestedDate" HeaderText="Requested Date" />
                                        <asp:BoundField DataField="name" HeaderText="Name" />
                                        <asp:BoundField DataField="Birthday" DataFormatString="{0:MM/dd/yyyy}" 
                                            HeaderText="Birth Date" />
                                        <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Button ID="ButtonDelete" runat="server" Text="Cancel" CausesValidation="False" CommandName="Delete" autopostback="true"/>                                             
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate> 
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>                            
                            </div>                                                                        
                        </fieldset>
                        <asp:Button ID="Button1" runat="server" Text="Button Cancel Hidden"  
                            style="display: none;" CausesValidation="False"/>
                    </div>
                    
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
