<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="ECUScheduling.aspx.vb" Inherits="emedicard.ECUScheduling" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
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
            <telerik:AjaxSetting AjaxControlID="btnVerify">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblVerifyMsg" />
                    <telerik:AjaxUpdatedControl ControlID="btnVerify" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="CustomValidator1" />
                    <telerik:AjaxUpdatedControl ControlID="txtDesignation" />
                    <telerik:AjaxUpdatedControl ControlID="ddlHospital" />
                    <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                    <telerik:AjaxUpdatedControl ControlID="btnSave" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                    <telerik:AjaxUpdatedControl ControlID="Calendar1" />
                    <telerik:AjaxUpdatedControl ControlID="txtDate" />
                    <telerik:AjaxUpdatedControl ControlID="txtMemCode" />
                    <telerik:AjaxUpdatedControl ControlID="lblVerifyMsg" />
                    <telerik:AjaxUpdatedControl ControlID="CustomValidator1" />
                    <telerik:AjaxUpdatedControl ControlID="txtDesignation" />
                    <telerik:AjaxUpdatedControl ControlID="ddlHospital" />
                    <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                    <telerik:AjaxUpdatedControl ControlID="btnSave" 
                        LoadingPanelID="RadAjaxLoadingPanel2" />
                    <telerik:AjaxUpdatedControl ControlID="gvResult" />
                    <telerik:AjaxUpdatedControl ControlID="Button1" />
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
<div id="Div1" class="span10">
		<!-- content starts -->
		
                <uc:acctinfo ID="NavAccountInfo" runat="server" />
		  
       
		<!-- content ends -->
			   
</div>
<div id="content" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;ECU SCHEDULING</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
                    
					<div class="box-content">
                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label><br />
                    <font color="#FF0000"><strong>NOTES:</strong></font>                    
                        <ul type="square" class="resp_text"><li>ECU is limited to the list of hospitals provided below. </li>
		                        <li>Request is subject to approval of MEDICard's Medical Operations Department. </li>
		                        <li>ECU Schedule Status is displayed below. </li>
		                        <li>Pending request can be cancelled anytime. </li>
		                        <li>Approved request can no longer be cancelled online if its 4 days or less prior to the approved ECU Schedule. Cancellation should be done by calling MEDICard's Medical Operations Department at 884-9936. </li>		
                        </ul> 
                        <fieldset>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Check Available Date</label>
                               <div class="controls">
                                   <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar><br />
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Prefered Date</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtDate" runat="server" 
                                       style="width: 100%; max-width: 100px;" ReadOnly="True"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  
                                       ErrorMessage="Schedule date is required!" CssClass="label label-warning" 
                                       ControlToValidate="txtDate" Display="Dynamic"></asp:RequiredFieldValidator>
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Member Code</label>
                               <div class="controls">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMemCode" runat="server" style="width: 100%; max-width: 100px; margin: 0 0 10px 0;"></asp:TextBox>
                                                <asp:Label ID="lblVerifyMsg" runat="server" Visible="False" CssClass="alert alert-info" Text="Label" ></asp:Label>
                                            </td>
                                            <td valign="top">
                                               <asp:Button ID="btnVerify" runat="server" Text="Verify" 
                                                   class="btn btn-small" CausesValidation="False"/>
                                               <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
                                               </telerik:RadAjaxLoadingPanel>
                                               
                                            </td>
                                            <td>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  
                                                   ErrorMessage="Head count is required!" CssClass="label label-warning" 
                                                   ControlToValidate="txtMemCode" Display="Dynamic"></asp:RequiredFieldValidator>
                                               <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Invalid Member!" CssClass="label label-warning" 
                                                   ControlToValidate="txtDate" Display="Dynamic"></asp:CustomValidator>                                        
                                            </td>
                                        </tr>
<%--                                        <tr>
                                            <td colspan="3" style="padding: 10px 0 0 0;"><asp:Label ID="lblVerifyMsg" runat="server" Visible="False" CssClass="alert alert-info" Text="Label" ></asp:Label></td>
                                        </tr>--%>
                                    </table>
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Designation</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtDesignation" runat="server" 
                                       style="width: 100%; max-width: 250px;" Enabled="False"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  
                                       ErrorMessage="Schedule date is required!" CssClass="label label-warning" 
                                       ControlToValidate="txtDesignation" Display="Dynamic"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Hospital</label>
                               <div class="controls">
                                   <asp:DropDownList ID="ddlHospital" runat="server" AutoPostBack="True" 
                                       Enabled="False">
                                       <asp:ListItem Value=" "></asp:ListItem>
                                       <asp:ListItem Value="001177">ASIAN HOSPITAL AND MEDICAL CENTER</asp:ListItem>
                                       <asp:ListItem Value="000150">CEBU DOCTORS HOSPITAL</asp:ListItem>
                                       <asp:ListItem Value="000827">DIAGNOSTIC NETWORKS INC. (HUMANA)</asp:ListItem>
                                       <asp:ListItem Value="000018">MAKATI MEDICAL CENTER</asp:ListItem>
                                       <asp:ListItem Value="000218">ST. LUKE&#39;S MEDICAL CENTER</asp:ListItem>
                                       <asp:ListItem Value="000168">THE MEDICAL CITY</asp:ListItem>
                                   </asp:DropDownList>
                               </div>
                            </div> 
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Remarks</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" 
                                       style="width: 100%; max-width: 450px;" Height="60" EnableTheming="False" 
                                       Enabled="False"></asp:TextBox>
                               </div>
                            </div>
                            <div class="control-group">
							 
                               <div class="controls">
                                   <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default">
                                   </telerik:RadAjaxLoadingPanel>
                                   <asp:Button ID="btnSave" runat="server" Text="Add Request" CssClass="btn" 
                                       OnClientClick="if(!confirm('Would you like to save request?')) return false; " 
                                       Enabled="False"/>
                                   
                               </div>
                                
                            </div>
                            <div>
                                <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    EmptyDataText="No data to display">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="Request No." 
                                            ControlStyle-CssClass="apeid" >
                                        <ControlStyle CssClass="apeid"></ControlStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RequestedDate" DataFormatString="{0:G}" 
                                            HeaderText="dateid" />
                                        <asp:BoundField DataField="RequestedDate" HeaderText="Requested Date" 
                                            DataFormatString="{0:d}" />
                                        <asp:BoundField HeaderText="Member Code" DataField="MemberCode" />
                                        <asp:BoundField DataField="name" HeaderText="Name" />
                                        <asp:BoundField DataField="HospitalName" HeaderText="Hospital Name" />
                                        <asp:BoundField DataField="PreferredDate" HeaderText="Prefered Date" 
                                            DataFormatString="{0:d}" />
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