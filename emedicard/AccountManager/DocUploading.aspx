<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="DocUploading.aspx.vb" Inherits="emedicard.DocUploading" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    -<uc:LeftNav ID="LeftNav1" runat="server" />
	<!-- left menu ends -->
    <div id="Div1" class="span10">
			<!-- content starts -->		
                 <uc:AcctInfo ID="NavAccountInfo" runat="server" />		         
			<!-- content ends -->			   
    </div>

<div id="Div2" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;File Uploading</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
                    
					<div class="box-content">
                                       
                        <fieldset>
                            <br />
                            <div>
                                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info-orng" Visible="false"></asp:Label>
                            </div>
                            <br />
                            <div class="control-group">
                                <label class="control-label" for="typeahead">Subject</label>
                                <div class="controls">
                                    <asp:DropDownList ID="ddlSubject" runat="server">
                                        <asp:ListItem Selected="True">Membership Endorsement</asp:ListItem>
                                        <asp:ListItem>Membership Cancellation</asp:ListItem>
                                        <asp:ListItem>Other</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="typeahead">Subject (Other)</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtSubject" runat="server" Enabled="False"></asp:TextBox>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="typeahead">Remarks</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtUploadRemarks" runat="server" Rows="4" TextMode="MultiLine" 
                                                      Width="728px" MaxLength="250"></asp:TextBox>
                                </div>
                            </div>  
                            <div class="control-group"><label class="control-label" for="typeahead">File Name</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="File is required." Display="Dynamic" ControlToValidate="FileUpload1" CssClass="label label-warning"></asp:RequiredFieldValidator>
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Allowed files:&nbsp;&nbsp;excel file(.xls, xlsx), compressed file(.zip, .rar), pdf file, images(.jpeg, .png, .gif)<br /> &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;15mb max file size. <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Invalid file attachment." 
                                                    CssClass="label label-warning" Display="Dynamic" ></asp:CustomValidator><br />
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="Dynamic" ControlToValidate="FileUpload1"  
                                ClientValidationFunction = "validate_file"></asp:CustomValidator>

                                </div>  
                            <div class="control-group">
                                <div class="controls">
                                    <div class="errmsg" style="max-width: 350px;">
                                
                                    </div>
                                </div>
                            </div>                                                                     
                            <div class="control-group">
                                <div class="controls">
                                    <asp:Button ID="btnUpload" runat="server" Text="Submit" CssClass="btn btn-primary" CausesValidation="False" />
                                </div>
                            </div>
                        </fieldset>
                        <asp:Button ID="Button1" runat="server" Text="Button Cancel Hidden"  
                            style="display: none;" CausesValidation="False"/>
                    </div>
                    
                </div>
              </div>
       
					<!-- content ends -->

            <!--- REQUEST LISTING -->
             <div class="row-fluid sortable">
				<div class="box span12" id="requestdiv" runat="server">
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;BATCH MEMBERSHIP ENDORSEMENT AND CANCELATION REQUEST</h2>
						<div class="box-icon"> 
                            <a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
                        </div>
                    </div>
                     <div class="box-content">
                         <asp:GridView ID="grdRequest" runat="server" CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                         EmptyDataText="No Data" AutoGenerateColumns="False" >
                             <Columns>
                                 <asp:BoundField DataField="rec_id" HeaderText="Ref. No." >
                                 <ItemStyle Width="80px" />
                                 </asp:BoundField>
                                 <asp:BoundField DataField="subject" HeaderText="Request Type" />
                                 <asp:BoundField DataField="file_path" HeaderText="Filename" />
                                 <asp:BoundField DataField="uploaded_by" HeaderText="Uploaded by" />
                                 <asp:BoundField DataField="uploaded_date" HeaderText="Uploaded Date" />
                                 <asp:BoundField DataField="req_status" HeaderText="Status" />
<%--                                    <asp:TemplateField HeaderText="">
                                        <HeaderTemplate>
                                            <asp:Button ID="ButtonDelete" runat="server" Text="Cancel" CausesValidation="False" CommandName="Delete" autopostback="true"/>                                             
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkCancel" runat="server" />
                                        </ItemTemplate>                                  
                                    </asp:TemplateField>--%>
                             </Columns>
                         </asp:GridView>
                     </div>
                </div>
            </div>
			   
    </div>
</asp:Content>
