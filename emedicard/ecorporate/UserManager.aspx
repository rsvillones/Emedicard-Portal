<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="UserManager.aspx.vb" Inherits="emedicard.UserManager" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/ecorporate/uctl/left-menu.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!-- left menu starts -->
				<uc:LeftNav ID="LeftNav1" runat="server" />
			<!-- left menu ends -->
		
			
			
			<div id="content" class="span10">
			<!-- content starts -->
		
           
              <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Users</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <div class="row-fluid">
                        <asp:Button ID="btnAdd" runat="server" Text="Add User" CssClass="btn btn-primary" /><br /><br />
                          <asp:GridView ID="grdUsers" runat="server" 
                                CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                AutoGenerateColumns="False">
                              <Columns>
                                  <asp:BoundField DataField="Username" HeaderText="Username" />
                                  <asp:BoundField DataField="Firstname" HeaderText="Firstname" />
                                  <asp:BoundField DataField="Lastname" HeaderText="Lastname" />
                                   <asp:TemplateField>
                                        <ItemTemplate>
                                            <a id="A1"  runat="server"
                                                href='<%# String.Format("~/eCorporate/EditUser.aspx?uid={0}&c={1}&t={2}&u={3}&a={4}",httputility.urlencode(EncryptDecrypt.EncryptDecrypt.Encrypt(Eval("UserID"), ConfigurationManager.AppSettings("encryptionKey"))),Request.Querystring("c"),Request.Querystring("t"),httputility.urlencode(Request.Querystring("u")),httputility.urlencode(EncryptDecrypt.EncryptDecrypt.Encrypt(Eval("RegAccountCode"), ConfigurationManager.AppSettings("encryptionKey"))))  %>' title='' >                             
                                                Edit
                                            </a>&nbsp;
                                        </ItemTemplate>
                                    </asp:TemplateField>
                              </Columns>

                            </asp:GridView>
                        </div>  
                    </div>
                    
                </div>
              </div>

                
          
		  
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
