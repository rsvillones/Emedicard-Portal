<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="EditAgentUser.aspx.vb" Inherits="emedicard.EditAgentUser" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="tl" %>
<%@ Register TagPrefix="uc" TagName="LeftNav" Src="~/eaccount/uctl/left-menu.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav ID="LeftNav1" runat="server"/>
    <tl:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tl:AjaxSetting AjaxControlID="btnSave">
                <UpdatedControls>
                    <tl:AjaxUpdatedControl ControlID="CustomValidator1" />
                    <tl:AjaxUpdatedControl ControlID="CustomValidator2" />
                    <tl:AjaxUpdatedControl ControlID="lblMessage" 
                        LoadingPanelID="RadAjaxLoadingPanel1" />
                    <tl:AjaxUpdatedControl ControlID="btnSave" />
                </UpdatedControls>
            </tl:AjaxSetting>
            <tl:AjaxSetting AjaxControlID="btnAddAccount">
                <UpdatedControls>
                    <tl:AjaxUpdatedControl ControlID="dtgUserAcctList" />
                    <tl:AjaxUpdatedControl ControlID="dtgAcctList" />
                    <tl:AjaxUpdatedControl ControlID="btnAddAccount" 
                        LoadingPanelID="RadAjaxLoadingPanel2" />
                </UpdatedControls>
            </tl:AjaxSetting>
            <tl:AjaxSetting AjaxControlID="Button1">
                <UpdatedControls>
                    <tl:AjaxUpdatedControl ControlID="dtgUserAcctList" />
                    <tl:AjaxUpdatedControl ControlID="dtgAcctList" />
                    <tl:AjaxUpdatedControl ControlID="btnAddAccount" />
                    <tl:AjaxUpdatedControl ControlID="Button1" />
                </UpdatedControls>
            </tl:AjaxSetting>
        </AjaxSettings>
    </tl:RadAjaxManager>
    <tl:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
        <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
    </tl:RadAjaxLoadingPanel>
			<div id="content" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i> Edit User</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>

                    <div class="box-content">
                    <tl:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" 
                        SelectedIndex="0">
                        <Tabs>
                            <tl:RadTab runat="server" Text="User Information" PageViewID="RadPageView1" 
                                Selected="True">
                            </tl:RadTab>
                            <tl:RadTab runat="server" Text="User Accounts" PageViewID="RadPageView2">
                            </tl:RadTab>
                        </Tabs>
                    </tl:RadTabStrip>
					<tl:RadMultiPage ID="RadMultiPage1" runat="server">
                        <tl:RadPageView ID="RadPageView1" runat="server" Selected="true">
					        
					 
                            <div class="box-content">
                                
         
                                
						  
                                <fieldset>
                                    
							  <legend>
                                        <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
                                    </legend>
      
                                    
     
                                    <asp:Button ID="btn" runat="server" CausesValidation="False" CssClass="btn" 
                                        Text="Back" />
                                    					
      
                                    
     
                                    <div class="control-group">
                                        
							  
                                        <label class="control-label" for="typeahead">
                                        Firstname</label>
      
                                        <div class="controls">
                                            
      
                                            <asp:TextBox ID="txtFirstname" runat="server" CssClass="input-xlarge focused" 
                                                style="width: 100%; max-width: 270px;"></asp:TextBox>
                                            
      
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                ControlToValidate="txtFirstname" CssClass="label label-warning" 
                                                ErrorMessage="Firstname required"></asp:RequiredFieldValidator>
                                            
      
                                        </div>
                                        
     
                                    </div>
                                    
     
                                    <div class="control-group">
                                        
							  
                                        <label class="control-label" for="typeahead">
                                        Lastname</label>
      
                                        <div class="controls">
                                            
      
                                            <asp:TextBox ID="txtLastname" runat="server" CssClass="input-xlarge focused" 
                                                style="width: 100%; max-width: 270px;"></asp:TextBox>
                                            
      
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="txtLastname" CssClass="label label-warning" 
                                                ErrorMessage="Lastname required"></asp:RequiredFieldValidator>
                                            
      
                                        </div>
                                        
     
                                    </div>
                                    
     
                                    <div class="control-group">
                                        
							  
                                        <label class="control-label" for="typeahead">
                                        Username</label>
      
                                        <div class="controls">
                                            
      
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="input-xlarge focused" 
                                                Enabled="False" style="width: 100%; max-width: 270px;"></asp:TextBox>
                                            
      
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                ControlToValidate="txtUsername" CssClass="label label-warning" 
                                                ErrorMessage="Username required"></asp:RequiredFieldValidator>
                                            
      
                                        </div>
                                        
     
                                    </div>
                                    
     
                                    <div class="control-group">
                                        
       
                                        <label class="control-label">
                                        Password</label>
       
                                        <div class="controls">
                                            
        
                                            <asp:TextBox ID="txtPassword" runat="server" 
                                                style="width: 100%; max-width: 270px;" TextMode="Password"></asp:TextBox>
                                            
        
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                                CssClass="label label-warning" Display="Dynamic" ErrorMessage=""></asp:CustomValidator>
                                            
       
                                        </div>
     
     
                                    </div>
                                    
     
                                    <div class="control-group">
                                        
       
                                        <label class="control-label">
                                        Confirm Password</label>
       
                                        <div class="controls">
                                            
        
                                            <asp:TextBox ID="txtConfirm" runat="server" 
                                                style="width: 100%; max-width: 270px;" TextMode="Password"></asp:TextBox>
                                            
        
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" 
                                                CssClass="label label-warning" Display="Dynamic" ErrorMessage=""></asp:CustomValidator>
                                            
       
                                        </div>
     
                                        
     
                                    </div>
                                    
     
                                    <div class="control-group">
                                        
							  
                                        <label class="control-label" for="typeahead">
                                        Email Address</label>
      
                                        <div class="controls">
                                            
      
                                            <asp:TextBox ID="txtEmailAddress" runat="server" 
                                                CssClass="input-xlarge focused" Enabled="False" 
                                                style="width: 100%; max-width: 270px;"></asp:TextBox>
                                            
       
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="txtEmailAddress" CssClass="label label-warning" 
                                                Display="Dynamic" ErrorMessage="Invalid email address" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            
      
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                ControlToValidate="txtEmailAddress" CssClass="label label-warning" 
                                                Display="Dynamic" ErrorMessage="Email address required"></asp:RequiredFieldValidator>
                                            
      
                                        </div>
                                        
      
                                        
     
                                    </div>
                                    
     
                                    <div class="control-group">
                                        
       
                                        <label class="control-label">
                                        Status(Active/Inactive)</label>
       
                                        <div class="controls">
                                            
        
                                            <label class="checkbox ">
                                            
        
                                            <asp:CheckBox ID="chkMemStatus" runat="server" class="checkbox" 
                                                style="padding-left: 0 !important; margin-left: 0 !important" />
 Active
                                            </label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <%--<label class="checkbox ">
                                            <asp:CheckBox ID="chkAPE" runat="server" class="checkbox" />
 APE
        
                                            </label>--%>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkUtil" runat="server" class="checkbox" />
 Utilization
                                            </label>
                                            <label class="checkbox ">
                                            
         
                                            <asp:CheckBox ID="chkEndorse" runat="server" class="checkbox" />
 Endorsement       
                                            </label>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkBenefits" runat="server" class="checkbox" />
 Benefits
                                            </label>
                                            <%--<label class="checkbox ">
                                            <asp:CheckBox ID="chkID" runat="server" class="checkbox" />
 ID
                                            </label>--%>
                                            <<%--label class="checkbox ">
                                            <asp:CheckBox ID="chkECU" runat="server" class="checkbox" />
 ECU
                                            </label>--%>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkActiveMem" runat="server" class="checkbox" />
 Active Members       
                                            </label>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkResgnMem" runat="server" class="checkbox" />
 Resigned Members
                                            </label>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkActMem" runat="server" class="checkbox" />
 Action Memo       
                                            </label>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkReimbSatus" runat="server" class="checkbox" />
 Reimbursements       
                                            </label>
                                            <label class="checkbox ">
                                            <asp:CheckBox ID="chkClinicResults" runat="server" class="checkbox" />
 Clinic Results      
                                            </label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info" Text="" 
                                                Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn" Text="Save Changes" />   
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
                        </tl:RadPageView>
                        <tl:RadPageView ID="RadPageView2" runat="server">
                            <h3 style="margin: 10px;">User Assigned Accounts</h3>
                            <div style="padding: 10px;">
                                <asp:GridView ID="dtgUserAcctList" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="ID" />
                                        <asp:BoundField DataField="AccountName" HeaderText="Account Name" />
                                        <asp:BoundField DataField="AccountCode" HeaderText="Account Code" />
                                        <asp:BoundField DataField="AccountCategory" HeaderText="Category" />
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderTemplate>
                                                <asp:Button ID="ButtonDelete" runat="server" autopostback="true" 
                                                    CausesValidation="False" CommandName="Delete" Text="Delete" />                                             
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>                                      
                                        </asp:TemplateField>                                        
                                    </Columns>                                   
                                </asp:GridView>
                            </div>
                            <h3 style="margin: 10px;">Agent Accounts</h3>   
                            <div style="padding: 10px;">     
                                <asp:GridView ID="dtgAcctList" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>                                       
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NAME" HeaderText="Account Name" />
                                        <asp:BoundField DataField="CODE" HeaderText="Account Code" />
                                        <asp:BoundField DataField="ACCT_CATEGORY" HeaderText="Category" />
                                        <asp:BoundField DataField="MOTHER_CODE" HeaderText="Mother Code" />
                                    </Columns>                                    
                                </asp:GridView>   
                                <div class="controls">   
                                    <tl:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Default">
                                    </tl:RadAjaxLoadingPanel>
                                    <asp:Button ID="btnAddAccount" runat="server" CssClass="btn btn-primary" 
                                        OnClientClick="if(!confirm('Do you really like to Add selected accounts to user?')) return false; " 
                                        Text="Add Accounts" />  
                                </div>
                            </div>
                        </tl:RadPageView>
                    </tl:RadMultiPage>
                          <asp:Button ID="Button1" runat="server" Text="Button Cancel Hidden"  
                            style="display: none;" CausesValidation="False"/> 
                 </div>                    
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
