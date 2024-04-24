<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="UserManagementHR.aspx.vb" Inherits="emedicard.UserManagementHR" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/eaccount/uctl/left-menu.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<uc:LeftNav ID="LeftNav1" runat="server"/>
<div id="content" class="span10">
          
          <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>List of users</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <div class="row-fluid">
                            <div id="tabs">
                                <ul>
                                    <li><a href="#tabs-1">eCorporate Users</a></li>

                                    <%--<% If oEAcctBLL.AccessLevel = 1 Then  %>
                                        <li><a href="#tabs-2">eAccount Users</a></li>
                                    <% End If  %>--%>

                                </ul>
                                <div id="tabs-1">
                                   <asp:Button ID="btnAddHR" runat="server" Text="Add New User" CssClass="btn btn-primary" /><br /><br />
                                   <asp:GridView ID="grdecorpusers" runat="server" 
                                        CssClass="table table-striped table-bordered bootstrap-datatable tblCons " 
                                        EmptyDataText="No data to display" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="Fullname" HeaderText="Full Name" />
                                            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
                                            <asp:BoundField DataField="AccountName" HeaderText="Company" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                        <a href='<%# Eval("UserID", "EditCorpUser.aspx?uid={0}") %>&t=<%= Request.Querystring("t") %>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>' >Edit</a>
                                                    </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>  
                                </div>

                               <%-- <% If oEAcctBLL.AccessLevel = 1 Then  %>
                            
                                    <div id="tabs-2">
                                        <asp:Button ID="btnAddAgent" runat="server" Text="Add New User" CssClass="btn btn-primary" /><br /><br />
                                        <asp:GridView ID="grdeaccountusers" runat="server" 
                                            CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                            EmptyDataText="No data to display" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="Firstname" HeaderText="First Name" />
                                                <asp:BoundField DataField="Lastname" HeaderText="Last Name" />
                                                <asp:BoundField DataField="Emailaddress" HeaderText="Email Address" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href='<%# Eval("UserID", "EditAgentUser.aspx?j={0}") %>&t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>' >Edit</a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView> 
                                    </div>

                                <% End If  %>--%>

                                
                            </div>                 
                        </div>   


                  	   <%-- <div class="row-fluid">
                              <asp:Button ID="btnAdd" runat="server" Text="Add New User" CssClass="btn btn-primary" /><br /><br />
                              <asp:GridView ID="dtgResult" runat="server" AutoGenerateColumns="False"
                              CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                                  <Columns>
                                      <asp:BoundField DataField="UserID" HeaderText="User ID" Visible="False" />
                                      <asp:BoundField DataField="AccountCode" HeaderText="AccountCode" />
                                      <asp:BoundField DataField="AccountName" HeaderText="AccountName" />
                                      <asp:BoundField DataField="FullName" HeaderText="FullName" />
                                      <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" />
                                      <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href='<%# Eval("UserID", "EditUser.aspx?j={0}") %>&t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>' >Edit</a>
                                        </ItemTemplate>
                                      </asp:TemplateField>
                                  </Columns>
                              </asp:GridView>
                        </div>      --%>                               
                    </div>
				</div><!--/span-->
			</div><!--/row-->
</div>
</asp:Content>
