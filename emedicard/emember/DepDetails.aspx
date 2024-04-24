<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="DepDetails.aspx.vb" Inherits="emedicard.DepDetails" %>

<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/emember/uctl/emember_left_menu.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
<%--				<uc:LeftNav runat="server" />--%>
			<!-- left menu ends -->
		
			
			
			<div id="content" class="span10">
			<!-- content starts -->
            <a href="javascript:history.go(-1)"><img src="../img/Arrow_Left.png" style="width: 48px; height: 48px;" alt="back"/></a>
		<h2>Membership Information</h2>
          <div class="row-fluid sortable" style="text-align: center !important;">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Personal Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                  	<div class="row-fluid">
                        <div class="span6" style="width: 100%; text-align: left">
                              
                            <div style="float:left; padding-right:25px">
                            <img src="../img/default.jpg" width="120px" />
                            </div>
                            <div class="acct-details">                            
                                <strong><span>Name:</span></strong>&nbsp;<asp:Label ID="lblName" runat="server" Text="Label"></asp:Label><br />
                                <strong><span>Birthday:</span></strong>&nbsp;<asp:Label ID="lblBirthday" runat="server" Text="Label"></asp:Label><br />
                                <strong><span>Age:</span></strong>&nbsp;<asp:Label ID="lblAge" runat="server" Text="Label"></asp:Label><br />
                                <strong><span>Civil Status:</span></strong>&nbsp;<asp:Label ID="lblCivilStatus" runat="server" Text="Label"></asp:Label><br />
                                <strong><span>Gender:</span></strong>&nbsp;<asp:Label ID="lblGender" runat="server" Text="Label"></asp:Label><br />
                                <strong><span>Company:</span></strong>&nbsp;<asp:Label ID="lblCompany" runat="server" Text="Label" style="width: 100%; max-width: 400px"></asp:Label><br />                    
                            </div>
                        </div>
                      
                    </div>    
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->


              <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Account Information</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <div class="row-fluid">
                            
                                
                               
                                <div class="span6 acct-details">
                                    <table class="tbl-info">
                                        <tr>
                                            <td class="td-label"><div class="span-label" style="width: 120px">Account Status:&nbsp;</div></td>
                                            <td><asp:Label ID="lblAccountStatus" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Member Code:&nbsp;</div></td>
                                            <td><asp:Label ID="lblMemberCode" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Effectivity Date:&nbsp;</div></td>
                                            <td><asp:Label ID="lblEffectivityDate" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">DD Limit:&nbsp;</div></td>
                                            <td><asp:Label ID="lblDDLimit" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label" valign="top"> <div class="span-label">ID Remarks:&nbsp;</div></td>
                                            <td><div class="remarks-box"><asp:Label ID="lblIDRemarks" runat="server" Text="Label"></asp:Label></div></td>
                                        </tr>

                                    </table>                                    
                                </div>
                                <div class="span6 acct-details">
                                <table class="tbl-info">
                                        <tr>
                                            <td class="td-label"><div class="span-label">Member Type:&nbsp;</div></td>
                                            <td><asp:Label ID="lblMemberType" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Plan:&nbsp;</div></td>
                                            <td><asp:Label ID="lblPlan" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">Validity Date:&nbsp;</div></td>
                                            <td><asp:Label ID="lblValidityDate" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label"><div class="span-label">PEC NON-DD:&nbsp;</div></td>
                                            <td><asp:Label ID="lblPEC" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="td-label" valign="top"> <div class="span-label" style="width: 120px">Other Remarks:&nbsp;</div></td>
                                            <td><asp:Label ID="lblOtherRemarks" runat="server" Text="Label"></asp:Label></td>
                                        </tr>

                                    </table>                                           
                                </div>                     
                            
                        </div>  
                    </div>
                </div>
              </div>

            <div class="row-fluid sortable">
				    <div class="box span12">
                        <div class="box-header well" data-original-title>
						    <h2><i class="icon-info-sign"></i>Medical and Dental Availments</h2>
						    <div class="box-icon">							
							    <a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>							
						    </div>
					    </div>
					    <div class="box-content">
                  	        <div class="row-fluid">
                                <asp:GridView ID="grdAvailment" runat="server" 
                                        CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
                                        AutoGenerateColumns="False" EmptyDataText="No record found" 
                                    ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="CONTROL_CODE" HeaderText="Control Code" />
                                        <asp:BoundField DataField="AVAIL_FR" HeaderText="Availment" 
                                            DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="DIAG_DESC" HeaderText="Primary Diagnosis/Remarks" />
                                        <asp:BoundField DataField="DX_REM" HeaderText="Other Diagnosis/Remarks" >
                                        <ItemStyle CssClass="col_size" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HOSPITAL_NAME" HeaderText="Hospital/Doctor" >
                                        <FooterStyle CssClass="txt_right txt_footer" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="APPROVED" DataFormatString="{0:###,###,###.00}" 
                                            HeaderText="Approved" >
                                        <FooterStyle CssClass="txt_right txt_footer" />
                                        <ItemStyle HorizontalAlign="Right" CssClass="txt_right" />
                                        </asp:BoundField>
                                    </Columns>
                                    
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
            </div>
    </div>
</asp:Content>
