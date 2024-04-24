<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Default.aspx.vb" Inherits="emedicard._Default3" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/eaccount/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountList.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav runat="server" />
<div id="content" class="span10">
          <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>User Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                  	<div class="row-fluid">
                        <table>
                            <tr>
                                <td rowspan="">
                                  <asp:Image ID="Image1" runat="server" ImageUrl="~/img/default.jpg" style="width: 100%; height: 100%; max-width: 150px; max-height: 150px;"/><br />                              
                                </td>
                                <td style="vertical-align: top; padding: 10px;">
                                    <strong>User ID:</strong> <span><%= oEAcctBLL.Username%></span></br>
                                    <strong>Name:</strong> <span><%= oEAcctBLL.Firstname & " " & oEAcctBLL.Lastname%></span></br>
                                    <strong>Email:</strong> <span><%= oEAcctBLL.EmailAddress%></span></br>
                                </td>
                            </tr>
                        </table>
                    </div>    
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->

          <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Account Manager</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                  	<div class="row-fluid">
                        <%--<uc:AcctList runat="server" />--%> 
                        <div id="tabs">
                            <ul>
                                <li><a href="#tabs-1">Active Accounts</a></li>
                                <li><a href="#tabs-2">Resigned Accounts</a></li>
                            </ul>
                            <div id="tabs-1">
                               <asp:GridView ID="grdActiveAccounts" runat="server" 
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons " 
                                    EmptyDataText="No data to display" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                        <asp:BoundField DataField="ACCOUNTCODE" HeaderText="Account Code" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <a id="lnk1"  runat="server" style="color: 	#3399FF !important" 
                                                    href='<%# String.Format("~/AccountManager/Default.aspx?a={0}&c={1}&t={2}&u={3}&agnt={4}",Eval("ACCOUNTCODE"),Request.Querystring("c"), Request.Querystring("t"), httputility.urlencode(Request.Querystring("u")),  Request.Querystring("agnt")) %>' title='' >                             
                                                    Manage Account
                                                </a>&nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>  
                            </div>
                            <div id="tabs-2">
                                <asp:GridView ID="grdResignedAccounts" runat="server" 
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                    EmptyDataText="No data to display" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
                                        <asp:BoundField DataField="ACCOUNTCODE" HeaderText="Account Code" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <a id="lnk1"  runat="server" style="color: 	#3399FF !important" 
                                                    href='<%# String.Format("~/AccountManager/Default.aspx?a={0}&c={1}&t={2}&u={3}&agnt={4}",Eval("ACCOUNTCODE"),Request.Querystring("c"), Request.Querystring("t"), httputility.urlencode(Request.Querystring("u")),  Request.Querystring("agnt")) %>' title='' >                             
                                                    Manage Account
                                                </a>&nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView> 
                            </div>
                        </div>                 
                    </div>    
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->
</div>
</asp:Content>
