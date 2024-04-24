<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="PaymentMade.aspx.vb" Inherits="emedicard.PaymentMade" %>
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
						<h2><i class="icon-info-sign"></i>Payment Made</h2>
						<div class="box-icon">							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>							
						</div>
					</div>
					<div class="box-content">
                  	    <div class="row-fluid">
                            <asp:GridView ID="grdPayment" runat="server" 
                                    CssClass="table table-striped table-bordered bootstrap-datatable " 
                                    AutoGenerateColumns="False" EmptyDataText="No record found">
                                <Columns>
                                    <asp:BoundField DataField="SA_NO" HeaderText="SA No." />
                                    <asp:BoundField DataField="Bill_Date" HeaderText="Bill Date" 
                                        DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Covered_from" HeaderText="Covered From" />
                                    <asp:BoundField DataField="covered_to" HeaderText="Covered To" />
                                </Columns>
                                    
                            </asp:GridView>
                        </div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>
