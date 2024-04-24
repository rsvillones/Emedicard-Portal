<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="emember_left_menu.ascx.vb" Inherits="emedicard.emember_left_menu" %>
<div class="span2 main-menu-span">
				<div class="well nav-collapse sidebar-nav">
					<ul class="nav nav-tabs nav-stacked main-menu">
						<li class="nav-header hidden-tablet">Main</li>
						<li><a class="ajax-link" href="Default.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-home"></i><span class="hidden-tablet"> Membership Information</span></a></li>
						
                        <li id="lnkPayment" runat="server"><a class="ajax-link" href="PaymentMade.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-eye-open"></i><span class="hidden-tablet"> Latest Payment Made</span></a></li>

						<li><a class="ajax-link" href="Availments.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-edit"></i><span class="hidden-tablet"> Medical and Dental Availments</span></a></li>
						<li><a class="ajax-link" href="Reimbursements.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-list-alt"></i><span class="hidden-tablet"> Reimbursement Status</span></a></li>
                        <!--<li><a class="ajax-link" href="EnrollDependent.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-edit"></i><span class="hidden-tablet"> Enroll Dependents</span></a></li>-->						
                        <%--<li><a class="ajax-link" href="OnlineConsultation.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-list-alt"></i><span class="hidden-tablet"> Consultation</span></a></li>--%>						
						<li class="nav-header hidden-tablet">Profile</li>
						<li><a class="ajax-link" href="Profile.aspx?u=<%= httputility.urlencode(Request.Querystring("u")) %>"><i class="icon-list-alt"></i><span class="hidden-tablet"> Edit Profile</span></a></li>
					</ul>
					<%--<label id="for-is-ajax" class="hidden-tablet" for="is-ajax"><input id="is-ajax" type="checkbox"> Ajax on menu</label>--%>
				</div><!--/.well -->
			</div><!--/span-->