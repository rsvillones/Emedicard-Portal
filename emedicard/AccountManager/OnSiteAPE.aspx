<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="OnSiteAPE.aspx.vb" Inherits="emedicard.OnSiteAPE" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav ID="LeftNav1" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Calendar1">
                <UpdatedControls>
                    
                    <telerik:AjaxUpdatedControl ControlID="Calendar1" />
                    
                    <telerik:AjaxUpdatedControl ControlID="txtDate" />
                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProvince">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlProvince" />
                    <telerik:AjaxUpdatedControl ControlID="ddlCity" />
                    <telerik:AjaxUpdatedControl ControlID="txtRegion" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlCity">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlCity" />
                    <telerik:AjaxUpdatedControl ControlID="txtRegion" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                    <telerik:AjaxUpdatedControl ControlID="txtDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtCount" />
                    <telerik:AjaxUpdatedControl ControlID="HeadCountValidator" />
                    <telerik:AjaxUpdatedControl ControlID="txtAddress" />
                    <telerik:AjaxUpdatedControl ControlID="ddlProvince" />
                    <telerik:AjaxUpdatedControl ControlID="ddlCity" />
                    <telerik:AjaxUpdatedControl ControlID="txtRegion" />
                    <telerik:AjaxUpdatedControl ControlID="btnSave" />
                    <telerik:AjaxUpdatedControl ControlID="gvResult" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Button1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvResult" />
                    <telerik:AjaxUpdatedControl ControlID="Button1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
<div id="content" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;ON-SITE APE SCHEDULING</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
                    
					<div class="box-content">
                    <div class="eAccdtls">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label><br />
                    <font color="#FF0000"><strong>NOTES:</strong></font>                    
                        <ul type="square"><li>Annual Physical Exam can be conducted within your company premises for as long as there is a minimum of 50 enrollees who would undergo APE. </li>
		                        <li>Onsite APE is limited to the list of provinces provided below.</li> 
		                        <li>Dates in blue are the available dates for Onsite APE Scheduling.</li>   
		                        <li>Request is subject to approval of MEDICard's Medical Services Department.</li> 
		                        <li>APE Schedule Status is displayed below.</li> 
		                        <li>Pending request can be cancelled anytime.</li>
		                        <li>Approved request can no longer be cancelled online if its 4 days or less prior to the approved APE Schedule.  Cancellation should be done by calling MEDICard's Medical Services Department at 884-9933/864-0980.</li> 
		
                        </ul> 
                    </div><!-- eAccdtls --><br />
                        <fieldset>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Check Available Date</label>
                               <div class="controls">
                                   <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar><br />
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Schedule Date</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtDate" runat="server" 
                                       style="width: 100%; max-width: 100px;" ReadOnly="True"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  
                                       ErrorMessage="Schedule date is required!" CssClass="label label-warning" 
                                       ControlToValidate="txtDate" Display="Dynamic"></asp:RequiredFieldValidator>
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Head Count</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtCount" runat="server" style="width: 100%; max-width: 30px;">50</asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  
                                       ErrorMessage="Head count is required!" CssClass="label label-warning" 
                                       ControlToValidate="txtCount" Display="Dynamic"></asp:RequiredFieldValidator>
                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Head count must be integer!" CssClass="label label-warning" ControlToValidate="txtCount" ValidationExpression="^\d+$" Display="Dynamic"> </asp:RegularExpressionValidator>  
                                   <asp:CustomValidator ID="HeadCountValidator" runat="server" ErrorMessage="Head count must be atleast 50 head!" ControlToValidate="txtCount" CssClass="label label-warning" Display="Dynamic"></asp:CustomValidator> 

                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Exact Address</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtAddress" runat="server" 
                                       style="width: 100%; max-width: 350px;" MaxLength="250"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  
                                       ErrorMessage="Adress is required!" CssClass="label label-warning" 
                                       ControlToValidate="txtAddress" Display="Dynamic"></asp:RequiredFieldValidator>
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Province</label>
                               <div class="controls">
                                   <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="True">
                                   </asp:DropDownList>
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">City</label>
                               <div class="controls">
                                   <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True">
                                   </asp:DropDownList>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Region</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtRegion" runat="server" ReadOnly="True" style="width: 100%; max-width: 100px;"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"  
                                       ErrorMessage="Schedule date is required!" CssClass="label label-warning" 
                                       ControlToValidate="txtRegion" Display="Dynamic"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							 
                               <div class="controls">
                                   <asp:Button ID="btnSave" runat="server" Text="Add Request" CssClass="btn" OnClientClick="if(!confirm('Would you like to save request?')) return false; "/>
                                   
                               </div>
                                
                            </div>
                            <div>
                                <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    EmptyDataText="No data to display">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="Request No." ControlStyle-CssClass="apeid" />
                                        <asp:BoundField DataField="RequestedDate" HeaderText="Requested Date" />
                                        <asp:BoundField HeaderText="Head Count" DataField="HeadCount" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" />
                                        <asp:BoundField DataField="CITY_NAME" HeaderText="City" />
                                        <asp:BoundField DataField="PROVINCE_NAME" HeaderText="Province" />
                                        <asp:BoundField DataField="Region" HeaderText="Region" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Requested By" />
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
                    
                    </div><!-- box-content -->
                    
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
