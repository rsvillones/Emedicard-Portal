<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="left-menu.ascx.vb" Inherits="emedicard.left_menu2" %>

<div class="span2 main-menu-span" id="l" runat="server">

	<div class="well nav-collapse sidebar-nav">
		<ul class="nav nav-tabs nav-stacked main-menu">
			<li class="nav-header hidden-tablet">Main</li>
			<li><a href="Default.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-home"></i><span class="hidden-tablet"> Account Manager</span></a></li>
			<li><a href="Profile.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-eye-open"></i><span class="hidden-tablet"> Profile Management</span></a></li>
			<%--<li id="lnk3" runat="server"><a href="UserManager.aspx?t=<%= Request.Querystring("t") %>&c=<%= Request.Querystring("c")%>&u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-edit"></i><span class="hidden-tablet"> User Management</span></a></li>--%>						
            <li><a href="~/Logout.aspx" runat="server"><i class="icon-off"></i><span class="hidden-tablet"> Logout</span></a></li>
		</ul>
		<%--<label id="for-is-ajax" class="hidden-tablet" for="is-ajax"><input id="is-ajax" type="checkbox"> Ajax on menu</label>--%>
	</div><!--/.well -->
</div><!--/span-->