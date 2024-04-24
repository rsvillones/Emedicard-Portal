<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Default.aspx.vb" Inherits="emedicard._Default4" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

            <!-- left menu starts -->
			<uc:LeftNav ID="LeftNav1" runat="server" />
			<!-- left menu ends -->
		
			
			
			<div id="content" class="span10">
			<!-- content starts -->
		
                 <uc:AcctList ID="NavAccountInfo" runat="server" />
		  
       
			<!-- content ends -->
			   
            </div>
<%--            <div id="Div2" class="span10">
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
                            <div style="font: 0.85em;">
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
                                     <asp:BoundField DataField="status_description" HeaderText="Status">
                                     </asp:BoundField>
                                 </Columns>
                             </asp:GridView>
                            </div>
                         </div>
                    </div>
                </div>
            </div>--%>
</asp:Content>
