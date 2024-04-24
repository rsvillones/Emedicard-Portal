<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Availments.aspx.vb" Inherits="emedicard.Availments" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/emember/uctl/emember_left_menu.ascx"%>
<%@  Register TagPrefix="uc2" TagName="BasicInfo" Src="~/emember/uctl/BasicInfo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <!-- left menu starts -->
	<uc:LeftNav ID="LeftNav1" runat="server" />               
<!-- left menu ends -->

<div id="content" class="span10">
         <uc2:BasicInfo ID="BasicInfo1" runat="server" />
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
                                    CssClass="table table-striped table-bordered bootstrap-datatable tblCons datatable" 
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
                                    <asp:BoundField DataField="APPROVED" DataFormatString="{0:N}" 
                                        HeaderText="Approved" >
                                    <FooterStyle CssClass="txt_right txt_footer" />
                                    <ItemStyle HorizontalAlign="Right" CssClass="txt_right" />
                                    </asp:BoundField>
                                </Columns>
                                    
                            </asp:GridView>
                        </div>
                        <div>Disclaimer: This utilization report does not include availments that did not call for approval and bills on process.</div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
