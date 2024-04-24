<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Endorsement.aspx.vb" Inherits="emedicard.Endorsement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

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

<!-- content starts -->
<div id="content" class="span10">            
    <div class="row-fluid sortable">
	    <div class="box span12">
					
            <div class="box-header well" data-original-title>
                    
			    <h2><i class="icon-info-sign"></i>&nbsp;MEMBERSHIP ENDORSEMENT</h2>
			    <div class="box-icon"> 
                    <a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
                </div>

            </div>
            <!-- Start of box content -->
            <div class="box-content">
                       <telerik:radajaxmanager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="btnSavePrincipal">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="lblMessage" LoadingPanelID="lp" />
                                            <telerik:AjaxUpdatedControl ControlID="cboPlan" />
                                            <telerik:AjaxUpdatedControl ControlID="txtLastname" />
                                            <telerik:AjaxUpdatedControl ControlID="txtFirstname" />
                                            <telerik:AjaxUpdatedControl ControlID="txtMiddleInitial" />
                                            <telerik:AjaxUpdatedControl ControlID="rdBirthdate" />
                                            <telerik:AjaxUpdatedControl ControlID="cboGender" />
                                            <telerik:AjaxUpdatedControl ControlID="cboCivilStatus" />
                                            <telerik:AjaxUpdatedControl ControlID="txtRemarks" />
                                            <telerik:AjaxUpdatedControl ControlID="btnSavePrincipal"  />
                                            <telerik:AjaxUpdatedControl ControlID="grdPrinicipal" LoadingPanelID="lp" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>

                                    <telerik:AjaxSetting AjaxControlID="cboPrincipalStatus">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="lblPrincode" />
                                            
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    
                                    <telerik:AjaxSetting AjaxControlID="btnSaveDependent">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                                            <telerik:AjaxUpdatedControl ControlID="lblPrincode"  />                                                                                        
                                            <telerik:AjaxUpdatedControl ControlID="cboRelation" />
                                            <telerik:AjaxUpdatedControl ControlID="cboPlanDep" />
                                            <telerik:AjaxUpdatedControl ControlID="txtDepLastname" />
                                            <telerik:AjaxUpdatedControl ControlID="txtDepFirstname" />
                                            <telerik:AjaxUpdatedControl ControlID="txtDepMiddleInitial" />
                                            <telerik:AjaxUpdatedControl ControlID="rdDepBirthdate" />
                                            <telerik:AjaxUpdatedControl ControlID="cboDepGender" />
                                            <telerik:AjaxUpdatedControl ControlID="cboDepCivilStatus" />
                                            <telerik:AjaxUpdatedControl ControlID="txtDepRemarks" />
                                            <telerik:AjaxUpdatedControl ControlID="btnSaveDependent" />
                                            <telerik:AjaxUpdatedControl ControlID="grdDependent" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="grdPrinicipal">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="grdPrinicipal" LoadingPanelID="lp" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    <telerik:AjaxSetting AjaxControlID="grdDependent">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="grdDependent" LoadingPanelID="lp" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>

                                    <telerik:AjaxSetting AjaxControlID="Button1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="lblMessage"  />                                                                                        
                                            <telerik:AjaxUpdatedControl ControlID="grdPrinicipal" />
                                            <telerik:AjaxUpdatedControl ControlID="Button1" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    
                                    <telerik:AjaxSetting AjaxControlID="Button2">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="grdDependent" />
                                            <telerik:AjaxUpdatedControl ControlID="Button2" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    
                                    <telerik:AjaxSetting AjaxControlID="Button4">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="Button4" />
                                            <telerik:AjaxUpdatedControl ControlID="grdRequest" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                    
                                </AjaxSettings>
                                
                             </telerik:radajaxmanager>
                        <telerik:radajaxloadingpanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" 
                                    EnableSkinTransparency="False">
                            <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
                        </telerik:radajaxloadingpanel>
                            <br />
                <br />
                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info-orng" Visible="false" style="color: #ffffff !important;"></asp:Label>
                <!--start of inner box content -->
                <div class="box-content">
                    <div id="MemEndorsement">
                        <ul>
                            <li style="display:none;"><a href="#tabs-1">Single Endorsement</a></li>
                            <li><a href="#tabs-2">Batch Uploading</a></li>
                        </ul>
                        <div id="tabs-1">
                            <div style="padding: 0 0 5px 0; width: 100%; text-align: right;">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel Membership" 
                                    CssClass="btn btn-primary" />
                            </div>
                            <div id="subtabs">                                <ul>                                    <li><a href="#subtabs-1">Principal</a></li>
                                    <li><a href="#subtabs-2">Dependent</a></li>                                </ul>                                <div id="subtabs-1">                                    <table class="table table-striped table-bordered bootstrap-datatable tblCons"><tr><td 
                                            colspan="6"><span style="color:Red">NOTES:</span> <ul><li>Online submission of enrollee does not mean an automatic approval of membership. It is subject to the approval of MEDICard&#39;s Underwriting Department.</li><li>Membership Status of additional enrollees filed online is displayed below.</li><li>To submit support documents such as Birth/Marriage/Death Certificate and others, please go to Bacth Uploading Tab.</li></ul></td></tr><tr><td><strong>Plan:</strong></td><td 
                                        colspan="5"><asp:DropDownList ID="cboPlan" runat="server" Width="350px"></asp:DropDownList><asp:ValidationSummary 
                                        ID="ValidationSummary1" runat="server" ShowMessageBox="True" 
                                        ShowSummary="False" ValidationGroup="principal" /></td></tr><tr><td><strong>Lastname:</strong> </td><td><asp:TextBox 
                                            ID="txtLastname" runat="server" style="max-width: 100px;"></asp:TextBox><asp:RequiredFieldValidator 
                                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLastname" 
                                            ErrorMessage="Lastname required." ForeColor="Red" ValidationGroup="principal">*</asp:RequiredFieldValidator></td><td><strong>Firstname:</strong></td><td><asp:TextBox 
                                            ID="txtFirstname" runat="server" style="max-width: 140px;"></asp:TextBox><asp:RequiredFieldValidator 
                                            ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstname" 
                                            ErrorMessage="Firstname required." ForeColor="Red" ValidationGroup="principal">*</asp:RequiredFieldValidator></td><td><strong>Middle Initial</strong></td><td><asp:TextBox 
                                            ID="txtMiddleInitial" runat="server" MaxLength="3" Width="29px"></asp:TextBox><asp:RequiredFieldValidator 
                                            ID="RequiredFieldValidator3" runat="server" 
                                            ControlToValidate="txtMiddleInitial" ErrorMessage="Middle Initial required." 
                                            ForeColor="Red" ValidationGroup="principal">*</asp:RequiredFieldValidator></td></tr><tr><td><strong>Birthdate:</strong></td><td><telerik:RadDatePicker 
                                            ID="rdBirthdate" runat="server" MinDate="1950-01-01"></telerik:RadDatePicker><asp:RequiredFieldValidator 
                                            ID="RequiredFieldValidator8" runat="server" ControlToValidate="rdBirthdate" 
                                            ErrorMessage="Birth Date is required." ForeColor="Red" 
                                            ValidationGroup="principal">*</asp:RequiredFieldValidator></td><td><strong>Gender:</strong></td><td>
                                            <asp:DropDownList 
                                            ID="cboGender" runat="server" Width="100px"><asp:ListItem Value="1">Male</asp:ListItem><asp:ListItem 
                                            Value="0">Female</asp:ListItem></asp:DropDownList>&nbsp;</td><td><strong>Civil Status:</strong></td><td>
                                            <asp:DropDownList 
                                            ID="cboCivilStatus" runat="server" Width="100px"><asp:ListItem Value="0">Single</asp:ListItem><asp:ListItem 
                                            Value="0">Married</asp:ListItem></asp:DropDownList>&nbsp;</td></tr><tr><td><strong>Remarks:</strong></td><td 
                                            colspan="5"><asp:TextBox ID="txtRemarks" runat="server" Rows="4" 
                                            TextMode="MultiLine" Width="728px"></asp:TextBox></td></tr><tr><td 
                                            colspan="6">
                                            <asp:Button ID="btnSavePrincipal" runat="server" CssClass="btn" 
                                            Text="Submit" ValidationGroup="principal" /></td></tr>                                    </table>                                </div>                                <div id="subtabs-2">                                    <table class="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    style="width: 100%"><tr><td colspan="6"><span style="color:Red">NOTES:</span> <ul><li>Online submission of enrollee does not mean an automatic approval of membership. It is subject to the approval of MEDICard&#39;s Underwriting Department.</li><li>Membership Status of additional enrollees filed online is displayed below.</li><li>To submit support documents such as Birth/Marriage/Death Certificate and others, please go to Bacth Uploading Tab.</li></ul></td></tr><tr><td><strong>Principal Status:</strong></td><td 
                                        colspan="5"><asp:DropDownList ID="cboPrincipalStatus" runat="server" 
                                        AutoPostBack="True" Width="258px"><asp:ListItem Value="0">Existing Principal (with member code)</asp:ListItem><asp:ListItem 
                                            Value="1">&nbsp;Newly Added Principal (pending applications) </asp:ListItem></asp:DropDownList></td></tr><tr><td><asp:Label 
                                            ID="lblPrincode" runat="server" Font-Bold="True" Text="Principal Code:"></asp:Label></strong></td><td 
                                            colspan="5"><asp:TextBox ID="txtPrincipalCode" runat="server" CssClass="txtPrinCode"></asp:TextBox>
                                                <asp:DropDownList ID="cboPrincipalCode" runat="server">
                                                </asp:DropDownList>
                                                <span id="prinvalidator" style="display: none;">*</span></td></tr><tr><td><strong>Relation to Principal:</strong></td><td 
                                            colspan="5"><asp:DropDownList ID="cboRelation" runat="server"></asp:DropDownList></td></tr><tr><td><strong>Plan:</strong></td><td 
                                        colspan="5"><asp:DropDownList ID="cboPlanDep" runat="server" Width="350px"></asp:DropDownList><asp:ValidationSummary 
                                        ID="ValidationSummary2" runat="server" ShowMessageBox="True" 
                                        ShowSummary="False" ValidationGroup="dependent" /></td></tr><tr><td 
                                            style="height: 53px"><strong>Lastname:</strong></td><td 
                                            style="height: 53px"><asp:TextBox ID="txtDepLastname" runat="server" style="max-width: 140px;"></asp:TextBox><asp:RequiredFieldValidator 
                                                ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDepLastname" 
                                                ErrorMessage="Lastname required." ForeColor="Red" ValidationGroup="dependent">*</asp:RequiredFieldValidator></td><td 
                                            style="height: 53px"><strong>Firstname:</strong></td><td 
                                            style="height: 53px"><asp:TextBox ID="txtDepFirstname" runat="server" style="max-width: 100px;"></asp:TextBox><asp:RequiredFieldValidator 
                                                ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDepFirstname" 
                                                ErrorMessage="Firstname required." ForeColor="Red" ValidationGroup="dependent">*</asp:RequiredFieldValidator></td><td 
                                            style="height: 53px"><strong>Middle Initial:</strong></td><td 
                                            style="height: 53px"><asp:TextBox ID="txtDepMiddleInitial" runat="server" 
                                                Width="29px"></asp:TextBox><asp:RequiredFieldValidator 
                                                ID="RequiredFieldValidator6" runat="server" 
                                                ControlToValidate="txtDepMiddleInitial" ErrorMessage="Lastname required." 
                                                ForeColor="Red" ValidationGroup="dependent">*</asp:RequiredFieldValidator></td></tr><tr><td><strong>Birthdate:</strong></td><td><telerik:RadDatePicker 
                                            ID="rdDepBirthdate" runat="server" MinDate="1950-01-01"></telerik:RadDatePicker><asp:RequiredFieldValidator 
                                            ID="RequiredFieldValidator7" runat="server" ControlToValidate="rdDepBirthdate" 
                                            ErrorMessage="Dependent birthdate required." ForeColor="Red" 
                                            ValidationGroup="dependent">*</asp:RequiredFieldValidator></td><td><strong>Gender:</strong></td><td>
                                            <asp:DropDownList 
                                            ID="cboDepGender" runat="server" Width="100px"><asp:ListItem Value="1">Male</asp:ListItem><asp:ListItem 
                                            Value="0">Female</asp:ListItem></asp:DropDownList></td><td><strong>Civil Status:</strong></td><td>
                                            <asp:DropDownList 
                                            ID="cboDepCivilStatus" runat="server" Width="100px"><asp:ListItem Value="0">Single</asp:ListItem><asp:ListItem 
                                            Value="1">Married</asp:ListItem></asp:DropDownList></td></tr><tr><td><strong>Remarks:</strong></td><td 
                                            colspan="5">
                                            <asp:TextBox ID="txtDepRemarks" runat="server" Rows="4" 
                                            TextMode="MultiLine" Width="600px"></asp:TextBox></td></tr><tr><td 
                                            colspan="6">
                                            <asp:Button ID="btnSaveDependent" runat="server" CssClass="btn" 
                                            Text="Submit" ValidationGroup="dependent" /></td></tr>                                    </table>                                </div>                            </div>
                            <!-- Start of Single Endorsement -->
                            <div class="row-fluid sortable"><div class="box span12"><div 
                                class="box-header well" data-original-title=""><h2><i class="icon-info-sign"></i>&nbsp;INDIVIDUAL ENDORSEMENT LISTING (PRINCIPAL)</h2><div 
                                class="box-icon"><a class="btn btn-minimize btn-round" href="#"><i 
                                class="icon-chevron-up"></i></a></div></div><div class="box-content"><asp:GridView 
                                    ID="grdPrinicipal" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    DataKeyNames="id" EmptyDataText="No Data"><Columns>
                                        <asp:BoundField 
                                        DataField="id" HeaderText="Reference No." /><asp:BoundField DataField="PlanDetails" 
                                        HeaderText="Plan" /><asp:BoundField DataField="PrinName" 
                                        HeaderText="Name" /><asp:BoundField DataField="Birthday" 
                                        DataFormatString="{0:MMMM d, yyyy}" HeaderText="Birthday" /><asp:BoundField 
                                        DataField="Gender" HeaderText="Gender" /><asp:BoundField 
                                        DataField="CivilStatus" HeaderText="Civil Status" /><asp:BoundField 
                                        DataField="RequestedBy" HeaderText="Requested By" /><asp:BoundField 
                                        DataField="Status" HeaderText="Status" /><asp:TemplateField HeaderText=""><%--                                        <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkButton1"  CommandArgument='<%# Eval("ID") %>' 
                                                                            CommandName="Delete" runat="server">
                                                                        Cancel</asp:LinkButton>
                            </ItemTemplate>--%><ItemTemplate><asp:CheckBox ID="chkCancel" runat="server" /></ItemTemplate><HeaderTemplate><asp:Button 
                                        ID="ButtonDelete" runat="server" autopostback="true" CausesValidation="False" 
                                        CommandName="Delete" Text="Cancel" /></HeaderTemplate></asp:TemplateField></Columns></asp:GridView></div></div>
                            </div>

									    <!--- PRINCIPAL LISTING --><div 
                                    class="row-fluid sortable"><div class="box span12"><div class="box-header well" 
                                            data-original-title=""><h2><i class="icon-info-sign"></i>&nbsp;INDIVIDUAL ENDORSEMENT LISTING (DEPENDENT)</h2><div 
                                            class="box-icon"><a class="btn btn-minimize btn-round" href="#"><i 
                                            class="icon-chevron-up"></i></a></div></div><div class="box-content"><asp:GridView 
                                                ID="grdDependent" runat="server" AutoGenerateColumns="False" 
                                                CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                                DataKeyNames="id" EmptyDataText="No Data"><Columns>
                                            <asp:BoundField 
                                                    DataField="id" HeaderText="Reference No." /><asp:BoundField DataField="PlanDetails" 
                                                    HeaderText="Plan" /><asp:BoundField DataField="DepName" HeaderText="Name" /><asp:BoundField 
                                                    DataField="PrinName" HeaderText="Principal Name" /><asp:BoundField 
                                                    DataField="Birthday" DataFormatString="{0:MMMM d, yyyy}" 
                                                    HeaderText="Birthday" /><asp:BoundField DataField="Gender" 
                                                    HeaderText="Gender" /><asp:BoundField DataField="CivilStatus" 
                                                    HeaderText="Civil Status" /><asp:BoundField DataField="RequestedBy" 
                                                    HeaderText="Requested By" /><asp:BoundField DataField="Status" 
                                                    HeaderText="Status" /><asp:TemplateField HeaderText=""><HeaderTemplate><asp:Button 
                                                        ID="ButtonDelete" runat="server" autopostback="true" CausesValidation="False" 
                                                        CommandName="Delete" Text="Cancel" /></HeaderTemplate><ItemTemplate><asp:CheckBox 
                                                        ID="chkCancel" runat="server" /></ItemTemplate></asp:TemplateField></Columns></asp:GridView></div></div><asp:Button 
                                        ID="Button1" runat="server" CausesValidation="False" style="display: none" 
                                        Text="Button Cancel Hidden" /><asp:Button ID="Button2" runat="server" 
                                        CausesValidation="False" style="display: none" 
                                        Text="Button Cancel Hidden Dep" /><asp:Button ID="Button3" runat="server" 
                                        CausesValidation="False" style="display: none" Text="Delete" /><asp:Button 
                                        ID="Button4" runat="server" CausesValidation="False" style="display: none" 
                                        Text="Button Cancel Hidden Req" /></div>

                        </div>
                        <!-- End of Tab1 -->
                        <div id="tabs-2">
                            <div class="box-content"><asp:ValidationSummary ID="ValidationSummary3" 
                                    runat="server" ShowMessageBox="True" ShowSummary="False" 
                                    ValidationGroup="uploading" />
                                    <fieldset><br /><div><asp:Label ID="Label1" 
                                        runat="server" CssClass="alert alert-info-orng" Text="" Visible="false"></asp:Label></div><br /><div 
                                        class="control-group"><label class="control-label" for="typeahead">Subject</label> <div 
                                            class="controls"><asp:DropDownList ID="ddlSubject" runat="server"><asp:ListItem 
                                                    Selected="True">Membership Enrollment</asp:ListItem><asp:ListItem>Membership Cancellation</asp:ListItem><asp:ListItem>Compliance to action memo</asp:ListItem>
                                                <asp:ListItem>Change of Status</asp:ListItem>
                                                <asp:ListItem>Correction of Details</asp:ListItem>
                                            </asp:DropDownList></div></div><div 
                                        class="control-group"><label class="control-label" for="typeahead">Remarks</label> <div 
                                            class="controls"><asp:TextBox ID="txtUploadRemarks" runat="server" 
                                                MaxLength="250" Rows="4" TextMode="MultiLine" Width="728px"></asp:TextBox></div></div><div 
                                        class="control-group"><label class="control-label" for="typeahead">File Name</label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:FileUpload 
                                            ID="FileUpload1" runat="server" Multiple="Multiple" />&nbsp;<asp:RequiredFieldValidator 
                                            ID="RequiredFieldValidator9" runat="server" ControlToValidate="FileUpload1" 
                                            CssClass="label label-warning" Display="Dynamic" 
                                            ErrorMessage="File is required." ValidationGroup="uploading"></asp:RequiredFieldValidator>
                                                    <%--<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Allowed files:&nbsp;&nbsp;excel file(.xls, xlsx), compressed file(.zip, .rar), pdf file, images(.jpeg, .png, .gif)<br /> &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Note: Single file uploading with maximum file size of 15mb per file.--%> 
                                                    <br />
                                                    <span style="color:Red">NOTES:</span> <ul><li>Allowed files:&nbsp;&nbsp;excel file(.xls, xlsx), compressed file(.zip, .rar), pdf file, images(.jpeg, .png, .gif)</li><li>Multiple file(s) uploading with maximum file size of 15mb per file. (<i>Use Ctrl/Cmd + Select, to choose multiple files</i>)</li></ul>
                                            <asp:CustomValidator 
                                            ID="CustomValidator1" runat="server" CssClass="label label-warning" 
                                            Display="Dynamic" ErrorMessage="Invalid file attachment."></asp:CustomValidator><br /><asp:CustomValidator 
                                            ID="CustomValidator2" runat="server" ClientValidationFunction="validate_file" 
                                            ControlToValidate="FileUpload1" Display="Dynamic" ErrorMessage="" 
                                            ValidationGroup="uploading"></asp:CustomValidator></div><div 
                                        class="control-group"><div class="controls"><div class="errmsg" 
                                                style="max-width: 350px;"></div></div></div><div 
                                        class="control-group"><div class="controls"><asp:Button ID="btnUpload" 
                                                runat="server" CssClass="btn btn-primary" Text="Submit" 
                                                ValidationGroup="uploading" /></div></div></fieldset> <asp:Button 
                                    ID="Button5" runat="server" CausesValidation="False" style="display: none;" 
                                    Text="Button Cancel Hidden" />
                            </div>
                            <!-- Start of multiple Endorsement -->
                            <div class="row-fluid sortable">
                                <div ID="requestdiv" runat="server" class="box span12">
                                    <div class="box-header well" data-original-title=""><h2><i 
                                                    class="icon-info-sign"></i>&nbsp;BATCH UPLOADING REQUESTS</h2><div 
                                                    class="box-icon"><a class="btn btn-minimize btn-round" href="#"><i 
                                                        class="icon-chevron-up"></i></a></div>
                                    </div>
                                    <div class="box-content">
					                    <div id="RequestList">
                                            <ul>
                                                <li><a href="#rtabs-1">Active Transactions</a></li>
                                                <li><a href="#rtabs-2">Closed Transactions</a></li>
                                            </ul>
                                            <div id="rtabs-1">
							                    <div style="overflow: auto; max-height: 550px;">
                                                    <asp:GridView ID="grdRequest" runat="server" 
                                                        AutoGenerateColumns="False" 
                                                        CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                                        EmptyDataText="No Data" PageSize="5" AllowPaging="True">
                                                            <Columns>
                                                                <asp:TemplateField 
                                                                HeaderText="Reference No.">
                                                                    <ItemTemplate>
                                                                        <a id="lnk1"  runat="server" style="color: 	#3399FF !important" 
                                                                            href='<%# String.Format("RequesDetails.aspx?t={0}&c={1}&u={2}&agnt={3}&a={4}&dtl={5}",Request.Querystring("t"), Request.Querystring("c"), httputility.urlencode(Request.Querystring("u")),  Request.Querystring("agnt"), httputility.urlencode(Request.Querystring("a")), httputility.urlencode(EncryptDecrypt.EncryptDecrypt.Encrypt(Eval("rec_id"), key))) %>' title='View Details' >                         
                                                                            <%# Eval("rec_id") %></a>&nbsp;
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField 
                                                                DataField="subject" HeaderText="Request Type" /><asp:BoundField 
                                                                DataField="remarks" HeaderText="Message/Remarks" /><asp:BoundField 
                                                                DataField="file_path" HeaderText="Filename" /><asp:BoundField 
                                                                DataField="uploaded_by" HeaderText="Created by" /><asp:BoundField 
                                                                DataField="uploaded_date" HeaderText="Created Date" /><asp:BoundField 
                                                                DataField="new_msg" HeaderText="New Message" />
                                                               <%-- <asp:BoundField DataField="status_description" HeaderText="Status" />
                                                                <asp:BoundField DataField="status_date" HeaderText="Date" />--%>
                                                                <asp:TemplateField><ItemTemplate>
                                                                    <asp:ImageButton ID="btnDownload2" runat="server" ImageUrl="~/img/Download.png" 
                                                                    CausesValidation="False" class="btn btn-primary" 
                                                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                                        CommandName="DownloadFile" Text="" Height="16" Width="16" ToolTip="Download File" />
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                    <%--  <asp:TemplateField 
                                                                HeaderText="View"><ItemTemplate><asp:Button ID="btnView" runat="server" 
                                                                        CausesValidation="False" class="btn btn-primary" 
                                                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                                        CommandName="ViewDetail" Text="View" /></ItemTemplate></asp:TemplateField>--%>
                                                            </Columns>
                                                    </asp:GridView>
                                                </div>
						                    </div>
                                            <!-- End of Tab1 -->
                                            <div id="rtabs-2">
							                    <div style="overflow: auto; max-height: 550px;">
                                                    <asp:GridView ID="grdClosedReq" runat="server" 
                                                        AutoGenerateColumns="False" 
                                                        CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                                        EmptyDataText="No Data" AllowPaging="True">
                                                            <Columns>
                                                                <asp:TemplateField 
                                                                HeaderText="Reference No.">
                                                                    <ItemTemplate>
                                                                        <a id="lnk2"  runat="server" style="color: 	#3399FF !important" 
                                                                            href='<%# String.Format("RequesDetails.aspx?t={0}&c={1}&u={2}&agnt={3}&a={4}&dtl={5}",Request.Querystring("t"), Request.Querystring("c"), httputility.urlencode(Request.Querystring("u")),  Request.Querystring("agnt"), httputility.urlencode(Request.Querystring("a")), httputility.urlencode(EncryptDecrypt.EncryptDecrypt.Encrypt(Eval("rec_id"), key))) %>' title='View Details' >                         
                                                                            <%# Eval("rec_id") %></a>&nbsp;
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField 
                                                                DataField="subject" HeaderText="Request Type" /><asp:BoundField 
                                                                DataField="remarks" HeaderText="Message/Remarks" /><asp:BoundField 
                                                                DataField="file_path" HeaderText="Filename" /><asp:BoundField 
                                                                DataField="uploaded_by" HeaderText="Created by" /><asp:BoundField 
                                                                DataField="uploaded_date" HeaderText="Created Date" /><asp:BoundField 
                                                                DataField="new_msg" HeaderText="New Message" />
                                                                <%--<asp:BoundField DataField="status_description" HeaderText="Status" />
                                                                <asp:BoundField DataField="status_date" HeaderText="Date" />--%>
                                                                <asp:TemplateField><ItemTemplate>
                                                                    <asp:ImageButton ID="btnDownload3" runat="server" ImageUrl="~/img/Download.png" 
                                                                    CausesValidation="False" class="btn btn-primary" 
                                                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                                        CommandName="DownloadFile" Text="" Height="16" Width="16" ToolTip="Download File" />
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                    <%--  <asp:TemplateField 
                                                                HeaderText="View"><ItemTemplate><asp:Button ID="btnView" runat="server" 
                                                                        CausesValidation="False" class="btn btn-primary" 
                                                                        CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" 
                                                                        CommandName="ViewDetail" Text="View" /></ItemTemplate></asp:TemplateField>--%>
                                                            </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <!-- end of tab2 -->
                                        </div>
                                    </div>
                                </div> 
                            </div>
                        </div>
                        <!-- end of tab2 -->
                    </div>
                    <!-- end of tabs -->
                </div>
                <!--end of inner box content -->
            </div>
            <!-- end of box content -->
    </div>
<!-- content starts -->
</div>
    </div>
</asp:Content>
