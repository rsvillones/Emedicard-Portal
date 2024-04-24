<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="MemCancelation.aspx.vb" Inherits="emedicard.MemCancelation" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav ID="LeftNav1" runat="server" />
	<!-- left menu ends -->
    <div id="Div1" class="span10">
			<!-- content starts -->		
                 <uc:AcctInfo ID="NavAccountInfo" runat="server" />		         
			<!-- content ends -->			   
    </div>
             <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                 <AjaxSettings>
                     <telerik:AjaxSetting AjaxControlID="btnSubmit">
                         <UpdatedControls>
                             <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                             <telerik:AjaxUpdatedControl ControlID="RequiredFieldValidator1" />
                             <telerik:AjaxUpdatedControl ControlID="RequiredFieldValidator2" />
                             <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                             <telerik:AjaxUpdatedControl ControlID="btnSubmit" />
                             <telerik:AjaxUpdatedControl ControlID="lblRecMsg" />
                             <telerik:AjaxUpdatedControl ControlID="dtgMemCancel" />
                         </UpdatedControls>
                     </telerik:AjaxSetting>
                     <telerik:AjaxSetting AjaxControlID="Button1">
                         <UpdatedControls>
                             <telerik:AjaxUpdatedControl ControlID="dtgMemCancel" />
                             <telerik:AjaxUpdatedControl ControlID="Button1" />
                         </UpdatedControls>
                     </telerik:AjaxSetting>
                 </AjaxSettings>
            </telerik:RadAjaxManager>
		<div id="content" class="span10">
              <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>&nbsp; MEMBERSHIP ENDORSEMENT (CANCEL MEMBERSHIP) </h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        
                        <div class="row-fluid">
                            <div class="eAccdtls">
                                <font color="#FF0000"><strong>NOTES:</strong></font>                    
                                <ul type="square"><li>Please surrender ID to MEDICard's Underwriting Department.</li> 
		                                <li>Appropriate refund will be computed based on the date when the ID was surrendered.</li> 
		                                <li>List of members with membership cancellation request filed online is displayed below. </li> 
                                        <li>Membership Cancellation is applicable to principal members only. </li>
                                </ul>
                            </div>
                        </div><br />
                        <div class="control-group">
							<label class="control-label" for="typeahead">Member Code</label>
                            <div class="controls">
                                <asp:TextBox ID="txtMemCode" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Member Code is required!" ControlToValidate="txtMemCode" CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div> 
<%--                        <div class="control-group">
							<label class="control-label" for="typeahead">Member Type</label>
                            <div class="controls">
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem Selected="True">PRINCIPAL</asp:ListItem>
                                    <asp:ListItem>DEPENDENT</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div> --%>
                        <div class="control-group">
							<label class="control-label" for="typeahead">Effectivity Date</label>
                            <div class="controls">
                                    <telerik:raddatepicker ID="dpEffDate" runat="server">
                                    </telerik:raddatepicker>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Effectivity Date from is required!" ControlToValidate="dpEffDate" 
                                        CssClass="label label-warning" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div> 
                        <div class="control-group">
                        <div class="control-group">
							<label class="control-label" for="typeahead">Remarks</label>
                            <div class="controls">
                                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="350" Rows="5" 
                                    TextMode="MultiLine" style="width: 100%; max-width: 300px;"></asp:TextBox>
                            </div>
                        </div> 
                        <div class="control-group">							 
                            <div class="controls">
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default">
                                </telerik:RadAjaxLoadingPanel>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" />  <br />
                                <asp:Label ID="lblRecMsg" runat="server" CssClass="label label-warning" 
                                    Visible="False"></asp:Label>
                                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="label label-warning" Visible="false"></asp:Label>
                            </div>
                                
                        </div> 
                        <div>
                            <asp:GridView ID="dtgMemCancel" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    EmptyDataText="No data to display">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="ID">
                                    <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EffectivityDate" DataFormatString="{0:d}" 
                                        HeaderText="Effectivity Date" />
                                    <asp:BoundField DataField="MemberCode" HeaderText="Member Code" />
                                    <asp:BoundField DataField="MemberType" HeaderText="Member Type" />
                                    <asp:BoundField DataField="mem_name" HeaderText="Name" />
                                    <asp:BoundField DataField="Birthday" HeaderText="Birth Date" 
                                        DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested by" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Action">
                                        <HeaderTemplate>
                                            <asp:Button ID="ButtonDelete" runat="server" Text="Cancel" CausesValidation="False" CommandName="Delete" autopostback="true"/>                                             
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkCancel" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>
                </div>
              </div>
                  <asp:Button ID="Button1" runat="server" Text="Button Cancel Hidden"  
                            style="display: none;" CausesValidation="False" />


<%--                  <asp:Button ID="btnPrev" runat="server" Text="Preview Notification" class="btn btn-primary" 
                      CausesValidation="False" />--%>
                      <%--<div id="divBtn" runat="server">
                          <asp:HyperLink ID="HyperLink1" runat="server">HyperLink</asp:HyperLink>
                      </div>--%>

            
					<!-- content ends -->
			   
    </div>
    </div>
</asp:Content>
