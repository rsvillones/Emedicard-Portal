<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="ConsultationDetails.aspx.vb" Inherits="emedicard.ConsultationDetails" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/emember/uctl/emember_left_menu.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav ID="LeftNav1" runat="server" /> 
<div id="content" class="span10">
		<h2>Online Consultation</h2>
          <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Consultation Details</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                  	<div class="row-fluid">
                        <div class="span6" style="padding-right: 10px;">   
                            <div class="acct-details">
                                <div id="consdtls" runat="server">
                                    
                                </div>
                            </div>                    
                            <div class="acct-details">
<%--                                <table>
                                    <tr>
                                        <td><strong><span>Message:</span></strong></td>
                                        <td>&nbsp;<asp:TextBox ID="txtMessage" runat="server" Rows="4" 
                                                TextMode="MultiLine" Width="150%"></asp:TextBox><br /></td>                                    
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSaveConsult" runat="server" Text="Submit" /></td>                         
                                    </tr>
                                </table> --%>                        
                                <strong><span>Message:</span></strong><br />
                                <asp:TextBox ID="txtMessage" runat="server" Rows="4" 
                                     TextMode="MultiLine" Width="100%"></asp:TextBox><br /><br />
                                 <asp:Button ID="btnSaveConsult" runat="server" Text="Submit" cssclass="btn btn-primary"/>                              
                                <br />              
                            </div>
                        </div>
                      
                    </div>    
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->
</div> 

</asp:Content>
