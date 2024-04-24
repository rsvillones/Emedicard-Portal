<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="AddCorpUser.aspx.vb" Inherits="emedicard.AddCorpUser" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<%@ Register TagPrefix="uc" TagName="LeftNav" Src="~/eaccount/uctl/left-menu.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
	<uc:LeftNav ID="LeftNav1" runat="server" />
	<!-- left menu ends -->

    <div id="content" class="span10">
			<!-- content starts -->
                <asp:HiddenField ID="hdnType" runat="server" />
                <asp:HiddenField ID="hdnCorporateCode" runat="server" />
                <asp:HiddenField ID="hdnUsername" runat="server" />
                
                <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <tl:AjaxSetting AjaxControlID="chkALL">
                            <UpdatedControls>
                                <tl:AjaxUpdatedControl ControlID="chkALL" />
                                <tl:AjaxUpdatedControl ControlID="dtgPlans" />
                            </UpdatedControls>
                        </tl:AjaxSetting>
                    </AjaxSettings>
                </tl:RadAjaxManager>
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;Add User</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
                    
					<div class="box-content">
						  <fieldset>
							
                              <tl:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" 
                EnableSkinTransparency="False">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </tl:RadAjaxLoadingPanel>
    
                            <asp:Button ID="btn" runat="server" Text="Back" CssClass="btn" 
                                  CausesValidation="False" />					
                            
                            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label>  

                            <legend>User Information</legend>                   
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Firstname</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtFirstname" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"  
                                       ErrorMessage="Firstname required" CssClass="label label-warning" 
                                       ControlToValidate="txtFirstname"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Lastname</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtLastname" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  
                                       ErrorMessage="Lastname required" CssClass="label label-warning" 
                                       ControlToValidate="txtLastname"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Designation</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtDesignation" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                       ErrorMessage="Designation required" CssClass="label label-warning" 
                                       ControlToValidate="txtDesignation"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Company</label>
                               <div class="controls">                                   
                                <asp:DropDownList ID="cboCompany" runat="server" CssClass="span6 typeahead">
                                </asp:DropDownList>                                
                               </div>
                            </div>
                            
                             <div class="control-group">
							  <label class="control-label" for="typeahead">Company Address</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                       ErrorMessage="Company address required" CssClass="label label-warning" 
                                       ControlToValidate="txtCompanyAddress"></asp:RequiredFieldValidator>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Telephone No.</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtTelephone" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Fax No.</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtFaxNo" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                               </div>
                            </div>
                            <div class="control-group">
							  <label class="control-label" for="typeahead">Mobile No.</label>
                               <div class="controls">
                                   <asp:TextBox ID="txtMobile" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                               </div>
                            </div>
                            
                            <legend>Login and Access Information</legend>
                            <div class="control-group">
							    <label class="control-label" for="typeahead">Username (Email)</label>
                                   <div class="controls">
                                       <asp:TextBox ID="txtUsername" runat="server" CssClass="input-xlarge focused"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
                                           runat="server" ErrorMessage="Username should be a valid email address" 
                                           CssClass="label label-warning" ControlToValidate="txtUsername" 
                                           ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                           Display="Dynamic"  ></asp:RegularExpressionValidator>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator6" 
                                           CssClass="label label-warning"  runat="server" 
                                           ErrorMessage="Username required" ControlToValidate="txtUsername" 
                                           Display="Dynamic" ></asp:RequiredFieldValidator>
                                   </div>
                                                               
                            </div>	
                            <div class="control-group">
							    <label class="control-label" >Accessibility</label>
							    <div class="controls">
								    <%--<label class="checkbox">
                                        <asp:CheckBox ID="chkAPE" runat="server"  />Annual Physical Examination (APE) Scheduling                                         
								    </label>--%>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkUtil" runat="server"  />Utilization Reporting                                         
								    </label>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkEndorsement" runat="server"  />Membership Endorsement                                         
								    </label>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkBenefits" runat="server"  />Benefits and Exclusions                                         
								    </label>
                                    <%--<label class="checkbox">
                                        <asp:CheckBox ID="chkECU" runat="server"  />Executive Check-Up (ECU) Scheduling                                         
								    </label>--%>
                                    <%--<label class="checkbox">
                                        <asp:CheckBox ID="chkRequest" runat="server"  />Request for ID Replacement                                         
								    </label>--%>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkActive" runat="server"  />Active Members                                        
								    </label>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkResigned" runat="server"  />Resigned Members                                         
								    </label>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkActionMemos" runat="server"  />Action Memos                                         
								    </label>
                                    <label class="checkbox">
                                        <asp:CheckBox ID="chkReimbStatus" runat="server"  />Reimbursements                                         
								    </label>
                                    <label class="checkbox ">
                                        <asp:CheckBox ID="chkClinicResults" runat="server" />Clinic Results
                                    </label>  
							    </div>
							</div>	

                            <legend>Account to manage</legend>	
                            <div class="control-group">
                                <asp:GridView ID="dtgAccounts" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                    <Columns>
                                        
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                        <asp:BoundField DataField="ACCOUNTCODE" HeaderText="Account Code" />

                                        <asp:BoundField DataField="ACCOUNTCATEGORY" HeaderText="Account Category" />
                                        <asp:BoundField DataField="MOTHERCODE" HeaderText="Mother Code" />
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                                                       
                            <legend>Plans to utilize</legend>	
                            <div class="control-group">
                                <table>
                                    <tr><td><asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" /></td><td>Select All</td></tr>
                                </table>
                                <asp:GridView ID="dtgPlans" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                    <Columns>
                                        <asp:BoundField DataField="RSPROOMRATE_ID" HeaderText="Room Rate ID" />
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPlanSelect" runat="server" />
                                            </ItemTemplate>                                        
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PLAN_DESC" HeaderText="Plan" />
                                        <asp:BoundField DataField="RNB_FOR" HeaderText="RNB For" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="control-group">
							 
                               <div class="controls">
                                    <input type="hidden" id="savemsg" value="" runat="server" />
                                   <asp:Button ID="btnSubmit" runat="server" Text="Save Changes" CssClass="btn btn-primary" />
                               </div>
                                
                            </div>			
                           			
							
                            <%--<div class="control-group">
							  <label class="control-label" for="typeahead">Profile Picture</label>
                               <div class="controls">
                                   <tl:RadUpload ID="flProfile" runat="server"  AllowedFileExtensions = ".jpg, .png,.jpeg, .gif"
                                    MaxFileInputsCount="1"  InputSize="45"  ControlObjectsVisibility="None">
                                   </tl:RadUpload>

                               
                                <img id="image1" src="" />
                                   
                               </div>
                            </div>--%>
                          </fieldset>                        
                                            
                    </div>
                    
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
