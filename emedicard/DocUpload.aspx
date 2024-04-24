<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="DocUpload.aspx.vb" Inherits="emedicard.DocUpload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div style="width: 100%;">
<div style="width: 1300px; margin-left: 80px; ">
<div id="Div2" class="span10">
			<!-- content starts -->
                 
              <div class="row-fluid sortable">
				<div class="box span12 dialog">
					
                    <div class="box-header well" data-original-title>
                    
						<h2><i class="icon-info-sign"></i>&nbsp;File Uploading</h2>
						<div class="box-icon"> 
                            <a href="#" class="btn btn-minimize btn-round" id="lnkReply"><i class="icon-chevron-up"></i></a>
                        </div>
					</div>
                    
					<div class="box-content">
                                       
                        <fieldset>
                            <br />
                            <div>
                                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label>
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
                                <label class="control-label" for="typeahead">Other</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtSubject" runat="server" Enabled="False"></asp:TextBox>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label" for="typeahead">Remarks</label>
                                <div class="controls">
                                    <asp:TextBox ID="txtUploadRemarks" runat="server" Rows="4" TextMode="MultiLine" 
                                                      Width="728px" MaxLength="250" style="max-width: 500px;"></asp:TextBox>
                                </div>
                            </div>  
                            <div class="control-group"><label class="control-label" for="typeahead">File Name</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" /><br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Allowed files:&nbsp;&nbsp;excel file(.xls, xlsx), compressed file(.zip, .rar), pdf file, images(.jpeg, .png, .gif)<br /> &nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;15mb max file size. <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Invalid file attachment." 
                                                    CssClass="label label-warning" Display="Dynamic" ></asp:CustomValidator><br />
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="Dynamic" ControlToValidate="FileUpload1"  
                                ClientValidationFunction = "validate_file"></asp:CustomValidator></div>  
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
                            <%--<asp:HiddenField ID="filelimit" runat="server" />--%>

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
                    
						    <h2><i class="icon-info-sign"></i>&nbsp;REQUEST LIST</h2>
						    <div class="box-icon"> 
                                <a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
                            </div>
                        </div>
                         <div class="box-content">
                            <div id="tabs">
                                <ul>
                                    <li><a href="#tabs-1">Active Transactions</a></li>
                                    <li><a href="#tabs-2">Closed Transactions</a></li>
                                </ul>
                                <div id="tabs-1">
                                     <asp:GridView ID="grdRequest" runat="server" CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                     EmptyDataText="No Data" AutoGenerateColumns="False" >
                                         <Columns>
                                             <asp:BoundField DataField="rec_id" HeaderText="Request ID" >
                                             <ItemStyle Width="80px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="subject" HeaderText="Request Type" />
                                             <asp:BoundField DataField="remarks" HeaderText="Message/Remarks" />
                                             <asp:BoundField DataField="file_path" HeaderText="Filename" />
                                             <asp:BoundField DataField="uploaded_by" HeaderText="Created by" />
                                             <asp:BoundField DataField="uploaded_date" HeaderText="Created Date" />
                                             <asp:BoundField HeaderText="New Message" DataField="new_msg" />
                                             <asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDownload" runat="server" Text="Download" class="btn btn-primary" 
                                                          CommandName="DownloadFile" 
                                                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CausesValidation="False" />
                                                </ItemTemplate> 
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnView" runat="server" Text="View" class="btn btn-primary" 
                                                          CommandName="ViewDetail" 
                                                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CausesValidation="False" />
                                                </ItemTemplate> 
                                             </asp:TemplateField>
                                         </Columns>
                                     </asp:GridView>
                                </div>
                                <div id="tabs-2">
                                     <asp:GridView ID="grdRequestDone" runat="server" CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                     EmptyDataText="No Data" AutoGenerateColumns="False" >
                                         <Columns>
                                             <asp:BoundField DataField="rec_id" HeaderText="Request ID" >
                                             <ItemStyle Width="80px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="subject" HeaderText="Request Type" />
                                             <asp:BoundField DataField="remarks" HeaderText="Message/Remarks" />
                                             <asp:BoundField DataField="file_path" HeaderText="Filename" />
                                             <asp:BoundField DataField="uploaded_by" HeaderText="Created by" />
                                             <asp:BoundField DataField="uploaded_date" HeaderText="Created Date" />
                                             <asp:BoundField HeaderText="New Message" DataField="new_msg" />
                                             <asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDownload" runat="server" Text="Download" class="btn btn-primary" 
                                                          CommandName="DownloadFile" 
                                                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CausesValidation="False" />
                                                </ItemTemplate> 
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnView" runat="server" Text="View" class="btn btn-primary" 
                                                          CommandName="ViewDetail" 
                                                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" CausesValidation="False" />
                                                </ItemTemplate> 
                                             </asp:TemplateField>
                                         </Columns>
                                     </asp:GridView>
                                </div>
                            </div> 

                         </div>
                    </div>
                </div>
			   
    </div>
</div>
</div>
</asp:Content>
