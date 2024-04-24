<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BasicInfo.ascx.vb" Inherits="emedicard.BasicInfo" %>
<div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Membership Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                  	<div class="row-fluid">
                        <div class="span4 acct-details">
<%--                            <div class="acct-details">                            
                                <strong><span><label class="labelInfo" for="typeahead">Name:</label></span></strong>&nbsp;<asp:Label ID="lblName" runat="server" Text="Label"></asp:Label><br />
                                <strong><span><label class="labelInfo" for="typeahead">Birthday:</label></span></strong>&nbsp;<asp:Label ID="lblBirthday" runat="server" Text="Label"></asp:Label><br />
                                <strong><span><label class="labelInfo" for="typeahead">Age:</label></span></strong>&nbsp;<asp:Label ID="lblAge" runat="server" Text="Label"></asp:Label><br />
                                <strong><span><label class="labelInfo" for="typeahead">Civil Status:</label></span></strong>&nbsp;<asp:Label ID="lblCivilStatus" runat="server" Text="Label"></asp:Label><br />
                                <strong><span><label class="labelInfo" for="typeahead">Gender:</label></span></strong>&nbsp;<asp:Label ID="lblGender" runat="server" Text="Label"></asp:Label><br />
                                <strong><span><label class="labelInfo" for="typeahead">Company:</label></span></strong>&nbsp;<asp:Label ID="lblCompany" runat="server" Text="Label"></asp:Label><br />                    
                            </div>--%>
                                <table>
                                    <tr>
                                        <td class="td-label"><div class="span-label">Name:</div></td>
                                        <td valign="top"><asp:Label ID="lblName" runat="server" Text="Label"></asp:Label></td>
                                    </tr>      
                                    <tr>
                                        <td class="td-label"><div class="span-label">Birthday:</div></td>
                                        <td valign="top"><asp:Label ID="lblBirthday" runat="server" Text="Label"></asp:Label></td>
                                    </tr>   
                                    <tr>
                                        <td class="td-label"><div class="span-label">Age:</div></td>
                                        <td valign="top"><asp:Label ID="lblAge" runat="server" Text="Label"></asp:Label></td>
                                    </tr>  
                                    <tr>
                                        <td class="td-label"><div class="span-label">Civil Status:</div></td>
                                        <td valign="top"><asp:Label ID="lblCivilStatus" runat="server" Text="Label"></asp:Label></td>
                                    </tr> 
                                    <tr>
                                        <td class="td-label"><div class="span-label">Gender:</div></td>
                                        <td valign="top"><asp:Label ID="lblGender" runat="server" Text="Label"></asp:Label></td>
                                    </tr>     
                                    <tr>
                                        <td class="td-label" valign="top"><div class="span-label">Company:</div></td>
                                        <td valign="top"><asp:Label ID="lblCompany" runat="server" Text="Label"></asp:Label></td>
                                    </tr>                                                            
                                </table>
                        </div>                      
                        <div class="span4 acct-details">
                                    <table >
                                        <tr>
                                            <td class="td-label"><div class="span-label">Account Status:</div></td>
                                            <td valign="top"><asp:Label ID="lblAccountStatus" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Member Code:</div></td>
                                            <td valign="top"><asp:Label ID="lblMemberCode" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Effectivity Date:</div></td>
                                            <td valign="top"><asp:Label ID="lblEffectivityDate" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">DD Limit:</div></td>
                                            <td valign="top"><asp:Label ID="lblDDLimit" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label" valign="top"> <div class="span-label">ID Remarks:</div></td>
                                            <td><div class="remarks-box"><asp:Label ID="lblIDRemarks" runat="server" Text="Label"></asp:Label></div></td>
                                        </tr>

                                    </table>                                    
                                </div>
                                 <div class="span4 acct-details">
                                <table >
                                        <tr>
                                            <td class="td-label"><div class="span-label">Member Type:</div></td>
                                            <td valign="top"><asp:Label ID="lblMemberType" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Plan:</div></td>
                                            <td valign="top"><asp:Label ID="lblPlan" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Validity Date:</div></td>
                                            <td valign="top"><asp:Label ID="lblValidityDate" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">PEC NON-DD:</div></td>
                                            <td valign="top"><asp:Label ID="lblPEC" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label" valign="top"> <div class="span-label">Other Remarks:</div></td>
                                            <td valign="top"><asp:Label ID="lblOtherRemarks" runat="server" Text="Label"></asp:Label></td>
                                        </tr>

                                    </table>                                           
                                </div>
                    </div>    
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->