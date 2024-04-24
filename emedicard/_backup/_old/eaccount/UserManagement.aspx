<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="UserManagement.aspx.vb" Inherits="emedicard.UserManagement" %>
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
                          <asp:Button ID="btnAdd" runat="server" Text="Add New User" CssClass="btn btn-primary" /><br /><br />
                          <asp:GridView ID="dtgResult" runat="server" AutoGenerateColumns="False"
                          CssClass="table table-striped table-bordered bootstrap-datatable tblCons">
                              <Columns>
                                  <asp:BoundField DataField="UserID" HeaderText="User ID" Visible="False" />
                                  <asp:BoundField DataField="Username" HeaderText="Username" />
                                  <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                  <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                                  <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                                  <asp:TemplateField>
                                    <ItemTemplate>
                                        <a href='<%# Eval("UserID", "EditUser.aspx?j={0}") %>&t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>' >Edit</a>
                                    </ItemTemplate>
                                  </asp:TemplateField>
                              </Columns>
                          </asp:GridView>
                    </div>                                     
                  </div>
				</div><!--/span-->
			</div><!--/row-->
</div>
</asp:Content>
