<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AccountInformation.ascx.vb" Inherits="emedicard.AccountInformation" %>
<div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>&nbsp;Account Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <table class="table table-bordered table-striped tblCons">
                            <tbody>
                                <tr>
                                    <td><strong>Account Code:</strong> <span><asp:Label ID="lblAccountCode" runat="server" Text=""></asp:Label></span></br>                                       
                                       <strong>Company Name:</strong> <span><asp:Label ID="lblCompany" runat="server" Text=""></asp:Label></span></br>                                       
                                       <strong>Account Officer/Broker:</strong> <span><asp:Label ID="lblAgent" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Effectivity Date:</strong> <span><asp:Label ID="lblEffectivityDate" runat="server" Text=""></asp:Label></span></br>
                                       <strong>Validity Date:</strong> <span><asp:Label ID="lblValidity" runat="server" Text=""></asp:Label></span></br>                                       
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    
                </div>
              </div>