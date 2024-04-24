<%@ Page Title="" Language="vb" EnableEventValidation = "false"  AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="ActiveMembers.aspx.vb" Inherits="emedicard.ActiveMembers" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@ Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- left menu starts -->
	<uc:LeftNav ID="LeftNav1" runat="server" />
	<!-- left menu ends -->
    <div id="Div1" class="span10">
			
		
                 <uc:AcctInfo ID="NavAccountInfo" runat="server" />
		  
       
			
			   
    </div>

        <!-- content starts -->
		<div id="content" class="span10">
           

                   <Telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <Telerik:AjaxSetting AjaxControlID="grdActiveMembersPrin">
                            <UpdatedControls>
                                <Telerik:AjaxUpdatedControl ControlID="grdActiveMembersPrin" LoadingPanelID="lp" />                                
                            </UpdatedControls>
                        </Telerik:AjaxSetting>
                    </AjaxSettings>
                 </Telerik:RadAjaxManager>
                <Telerik:RadAjaxLoadingPanel ID="lp" runat="server" Skin="Default" BackgroundPosition="None" 
                            EnableSkinTransparency="False">
                    <asp:Image runat="server" ID="loader" ImageAlign="Middle" ImageUrl="~/img/ajax-loaders/ajax-loader-7.gif" />
                </Telerik:RadAjaxLoadingPanel>

              <div class="row-fluid sortable">
				<div class="box span12" style="width:100%">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Active Members</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                        <div class="row-fluid">
                           
                            
                           <telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="members"  
                              SelectedIndex="0">
                                <tabs>
                                    <telerik:RadTab runat="server" Text="Principal" PageViewID="PageView1" 
                                        Selected="True"   >
                                       
                                    </telerik:RadTab>
                                    <telerik:RadTab runat="server" Text="Dependent" PageViewID="PageView2">
                                    </telerik:RadTab>
                                </tabs>
                            </telerik:RadTabStrip>
                              <telerik:RadMultiPage ID="members" runat="server" SelectedIndex="0">
                                    
                                     <telerik:RadPageView id="PageView1" runat="server"  Selected="true"><br />
                                        <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CssClass="btn btn-primary"/><br /><br />
                                        
                                         <div id="gridscroll" style="max-height:400px;overflow-y:scroll;">
                                         
                                            <asp:GridView ID="grdActiveMembersPrin" runat="server" 
                                             CssClass="table table-striped table-bordered bootstrap-datatable tblCons grid-setting" 
                                             AutoGenerateColumns="False" EmptyDataText="No Data" style="overflow:scroll; max-height:250px;max-width:1050px;"  >
                                             <Columns>
                                                 <asp:BoundField DataField="prin_compID" HeaderText="Emp. ID" />
                                                 <asp:BoundField DataField="prin_code" HeaderText="Mem. Code" />
                                                 <asp:BoundField DataField="mem_lname" HeaderText="Last Name" />
                                                 <asp:BoundField DataField="mem_fname" HeaderText="First Name" />
                                                 <asp:BoundField DataField="mem_mi" HeaderText="MI" />
                                                 <asp:BoundField DataField="mem_bday" HeaderText="Birthday" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="mem_age" HeaderText="Age" 
                                                     DataFormatString="{0:G2}" />
                                                 <asp:BoundField DataField="sex_desc" HeaderText="Sex" />
                                                 <asp:BoundField DataField="memcstat_desc" HeaderText="Status" />
                                                 <asp:BoundField DataField="eff_date" HeaderText="Eff. Date" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="val_date" HeaderText="Val. Date" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="plan_desc" HeaderText="Plan" />
                                                 <asp:BoundField DataField="area_desc" HeaderText="Area" />
                                                 <asp:BoundField DataField="ID_REM" HeaderText="ID_Rem1" ItemStyle-Width="100px" />
                                                 <asp:BoundField DataField="ID_REM2" HeaderText="ID_Rem2" ItemStyle-Width="100px" />
                                                 <asp:BoundField DataField="ID_REM3" HeaderText="ID_Rem3" ItemStyle-Width="100px" />
                                                 <asp:BoundField DataField="ID_REM4" HeaderText="ID_Rem4" ItemStyle-Width="100px" />
                                                 <asp:BoundField DataField="ID_REM5" HeaderText="ID_Rem5" ItemStyle-Width="100px" />
                                                 <asp:BoundField DataField="ID_REM6" HeaderText="ID_Rem6" ItemStyle-Width="100px" />
                                                 <asp:BoundField DataField="ID_REM7" HeaderText="ID_Rem7" ItemStyle-Width="100px" />
                                             </Columns>
                                        </asp:GridView>

                                        </div>
                                         
                                     
                                     </telerik:RadPageView>
                                             <telerik:RadPageView id="PageView2" runat="server" ><br />
                                             <asp:Button ID="btnExportDep"  runat="server" Text="Export to Excel" CssClass="btn btn-primary"/><br /><br />
                                            
                                                <div id="gridscroll2" style="max-height:400px;overflow-y:scroll;">

                                                    <asp:GridView ID="grdActiveMembersDep" runat="server" 
                                                         CssClass="table table-striped table-bordered bootstrap-datatable tblCons grid-setting" 
                                                         AutoGenerateColumns="False" EmptyDataText="No Data"   >
                                                         <Columns>
                                                             <asp:BoundField DataField="prin_compID" HeaderText="Prin. EmpID" />
                                                             <asp:BoundField DataField="principal" HeaderText="principal" />
                                                             <asp:BoundField DataField="DEP_EMPID" HeaderText="Emp ID" />
                                                             <asp:BoundField DataField="dep_code" HeaderText="Mem. Code" />
                                                             <asp:BoundField DataField="mem_lname" HeaderText="Last Name" />
                                                             <asp:BoundField DataField="mem_fname" HeaderText="First Name" />
                                                             <asp:BoundField DataField="mem_mi" HeaderText="MI" />
                                                             <asp:BoundField DataField="mem_bday" HeaderText="Birthday" 
                                                                 DataFormatString="{0:MM/dd/yyyy}" >
                                                             <ItemStyle CssClass="grid-date" />
                                                             </asp:BoundField>
                                                             <asp:BoundField DataField="mem_age" HeaderText="Age" 
                                                                 DataFormatString="{0:G2}" />
                                                             <asp:BoundField DataField="sex_desc" HeaderText="Sex" />
                                                             <asp:BoundField DataField="memcstat_desc" HeaderText="Status" />
                                                             <asp:BoundField DataField="eff_date" HeaderText="Eff. Date" 
                                                                 DataFormatString="{0:MM/dd/yyyy}" >
                                                             <ItemStyle CssClass="grid-date" />
                                                             </asp:BoundField>
                                                             <asp:BoundField DataField="val_date" HeaderText="Val. Date" 
                                                                 DataFormatString="{0:MM/dd/yyyy}" >
                                                             <ItemStyle CssClass="grid-date" />
                                                             </asp:BoundField>
                                                             <asp:BoundField DataField="plan_desc" HeaderText="Plan" />
                                                             <asp:BoundField DataField="area_desc" HeaderText="Area" />
                                                             <asp:BoundField DataField="ID_REM" HeaderText="ID_Rem1" ItemStyle-Width="100px" />
                                                             <asp:BoundField DataField="ID_REM2" HeaderText="ID_Rem2" ItemStyle-Width="100px" />
                                                             <asp:BoundField DataField="ID_REM3" HeaderText="ID_Rem3" ItemStyle-Width="100px" />
                                                             <asp:BoundField DataField="ID_REM4" HeaderText="ID_Rem4" ItemStyle-Width="100px" />
                                                             <asp:BoundField DataField="ID_REM5" HeaderText="ID_Rem5" ItemStyle-Width="100px" />
                                                             <asp:BoundField DataField="ID_REM6" HeaderText="ID_Rem6" ItemStyle-Width="100px" />
                                                             <asp:BoundField DataField="ID_REM7" HeaderText="ID_Rem7" ItemStyle-Width="100px" />
                                                         </Columns>
                                                    </asp:GridView>

                                                </div>

                                                 
                                            </telerik:RadPageView>
                                    
                            </telerik:RadMultiPage>
                            
                         
                        </div>  
                    </div>
                    
                </div>
              </div>

                
          
		  
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
