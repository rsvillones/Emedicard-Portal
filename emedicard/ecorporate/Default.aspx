<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Default.aspx.vb" Inherits="emedicard._Default2" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/ecorporate/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctList" Src="~/AccountManager/uctl/AccountList.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <!-- left menu starts -->
			<uc:LeftNav ID="LeftNav1" runat="server" />
			<!-- left menu ends -->
		
			
			
			<div id="content" class="span10">
			<!-- content starts -->
		
              <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>&nbsp;User Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <table class="table table-bordered table-striped tblCons">
                            <tbody>
                                <tr>
                                    <th width="20%"><img src="../img/default.jpg" width="100%" /></th>
                                    <td><strong>Username:</strong> <span><asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Name:</strong> <span><asp:Label ID="lblName" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Company Name:</strong> <span><asp:Label ID="lblCompany" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Designation:</strong> <span><asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Address:</strong> <span><asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Telephone No.:</strong> <span><asp:Label ID="lblTelephone" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Mobile No.:</strong> <span><asp:Label ID="lblMobile" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Fax No.:</strong> <span><asp:Label ID="lblFax" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Email:</strong> <span><asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></span></br>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
                </div>
              </div>

              <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Account List</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <div class="row-fluid">
                           <%-- <div class="span6">
                                                                       
                            </div>--%>
                            <!-- Account Information User Control here-->
                            
                            <uc:AcctList ID="AcctList" runat="server" />
                        </div>  
                    </div>
                    
                </div>
              </div>


          
		  
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
