<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="left-menu.ascx.vb" Inherits="emedicard.left_menu1" %>
<div class="span2 main-menu-span">
				<div class="well nav-collapse sidebar-nav">
					<ul class="nav nav-tabs nav-stacked main-menu">
						<li class="nav-header hidden-tablet">Main</li>
						<li><a class="ajax-link" href="Default.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>"><i class="icon-home"></i><span class="hidden-tablet"> Account Manager</span></a></li>
						<li><a class="ajax-link" href="EditProfile.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>"><i class="icon-eye-open"></i><span class="hidden-tablet"> Profile Management</span></a></li>
                        <% If oEAcctBLL.AccessLevel = 1 Then%>
                            <li><a class="ajax-link" href="UserManagement.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>"><i class="icon-user"></i><span class="hidden-tablet"> User Management</span></a></li>		
                        <% Else If oEAcctBLL.AccessLevel = 2 Then%>		
                            <li><a class="ajax-link" href="UserManagementHR.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= HttpUtility.UrlEncode(Request.Querystring("u")) %>&agnt=<%= Request.Querystring("agnt") %>"><i class="icon-user"></i><span class="hidden-tablet"> eCorp Management</span></a></li>	
                        <% End If%>
                        <li><a id="A1" href="~/Logout.aspx" runat="server"><i class="icon-off"></i><span class="hidden-tablet"> Logout</span></a></li>
					</ul>
					<%--<label id="for-is-ajax" class="hidden-tablet" for="is-ajax"><input id="is-ajax" type="checkbox"> Ajax on menu</label>--%>
				</div><!--/.well -->
			</div><!--/span-->