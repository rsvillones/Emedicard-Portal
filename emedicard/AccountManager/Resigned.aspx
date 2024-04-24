<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="Resigned.aspx.vb" Inherits="emedicard.Resigned" %>

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
                        <Telerik:AjaxSetting AjaxControlID="grdMembersPrin">
                            <UpdatedControls>
                                <Telerik:AjaxUpdatedControl ControlID="grdMembersPrin" LoadingPanelID="lp" />
                                
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
						<h2>Resigned Members</h2>
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
                                         <asp:GridView ID="grdMembersPrin" runat="server" 
                                             CssClass="table table-striped table-bordered bootstrap-datatable tblCons grid-setting" 
                                             AutoGenerateColumns="False"   >
                                             <Columns>
                                                 <asp:BoundField DataField="prin_compID" HeaderText="Emp. ID" />
                                                 <asp:BoundField DataField="prin_code" HeaderText="Mem. Code" />
                                                 <asp:BoundField DataField="MEM_NAME" HeaderText="Name" />
                                                 <asp:BoundField DataField="mem_bday" HeaderText="Birthday" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle Width="70px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="mem_age" HeaderText="Age" 
                                                     DataFormatString="{0:G2}" />
                                                 <asp:BoundField DataField="sex_desc" HeaderText="Sex" />
                                                 <asp:BoundField DataField="memcstat_desc" HeaderText="Status" >
                                                 <ItemStyle Width="90px" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="eff_date" HeaderText="Eff. Date" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle Width="100px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="VAL_DATE" DataFormatString="{0:MM/dd/yyyy}" 
                                                     HeaderText="Val. Date">
                                                 <ItemStyle Width="100px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="DATE_RESGN" DataFormatString="{0:MM/dd/yyyy}" 
                                                     HeaderText="Resgn. Date">
                                                 <ItemStyle Width="100px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="plan_desc" HeaderText="Plan" />
                                                 <asp:BoundField DataField="area_desc" HeaderText="Area" />
                                             </Columns>
                                        </asp:GridView>
                                       <%--  <telerik:RadGrid ID="grdActiveMembersPrin" PageSize="500" runat="server" AllowPaging="True"  CssClass="table table-striped table-bordered bootstrap-datatable tblCons" AutoGenerateColumns="False" GridLines="None"  >
                                                   <MasterTableView>

                                                    <RowIndicatorColumn>
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </RowIndicatorColumn>

                                                    <ExpandCollapseColumn>
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="prin_compID" HeaderText="Emp. ID" 
                                                                UniqueName="column12">
                                                            </telerik:GridBoundColumn>                                                                                                                 
                                                            <telerik:GridBoundColumn DataField="prin_code" HeaderText="Mem. Code" 
                                                                UniqueName="column">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_lname" HeaderText="Last Name" 
                                                                UniqueName="column1">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_fname" HeaderText="First Name" 
                                                                UniqueName="column2">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_mi" HeaderText="MI" 
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_bday" HeaderText="Bithday" 
                                                                DataFormatString="{0:d}" UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_age" HeaderText="Age" 
                                                                UniqueName="column5">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="sex_desc" HeaderText="sex_desc" 
                                                                UniqueName="column6">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="memcstat_desc" HeaderText="Status" 
                                                                UniqueName="column7">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="eff_date" HeaderText="Eff. Date" 
                                                                UniqueName="column8" DataType="System.DateTime" DataFormatString="{0:d}">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="val_date" DataFormatString="{0:d}" HeaderText="Val. Date" 
                                                                UniqueName="column9">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="plan_desc" HeaderText="Plan" 
                                                                UniqueName="column10">
                                                            </telerik:GridBoundColumn>                                                                                                                        
                                                            <telerik:GridBoundColumn DataField="area_desc" HeaderText="Area" 
                                                                UniqueName="column11">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                   
                                                </telerik:RadGrid>--%>

                                     
                                     </telerik:RadPageView>
                                             <telerik:RadPageView id="PageView2" runat="server" >
                                             <br />
                                             <asp:Button ID="btnExportDep"  runat="server" Text="Export to Excel" CssClass="btn btn-primary"/><br /><br />
                                                <%--<telerik:RadGrid PageSize="500" ID="grdActiveMembersDep" runat="server" AllowPaging="True"  CssClass="table table-striped table-bordered bootstrap-datatable tblCons"                                                 
                                                    AutoGenerateColumns="False" GridLines="None"  >
                                                   <MasterTableView>

                                                    <RowIndicatorColumn>
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </RowIndicatorColumn>

                                                    <ExpandCollapseColumn>
                                                    <HeaderStyle Width="20px"></HeaderStyle>
                                                    </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="prin_compID" HeaderText="Prin. Emp. ID" 
                                                                UniqueName="column12">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="principal" HeaderText="Principal" 
                                                                UniqueName="column13">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn UniqueName="prin_compid" DataField="prin_compid" 
                                                                HeaderText="Emp ID">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="prin_code" HeaderText="Mem. Code" 
                                                                UniqueName="column">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_lname" HeaderText="Last Name" 
                                                                UniqueName="column1">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_fname" HeaderText="First Name" 
                                                                UniqueName="column2">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_mi" HeaderText="MI" 
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_bday" HeaderText="Bithday" 
                                                                DataFormatString="{0:d}" UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="mem_age" HeaderText="Age" 
                                                                UniqueName="column5">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="sex_desc" HeaderText="sex_desc" 
                                                                UniqueName="column6">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="memcstat_desc" HeaderText="Status" 
                                                                UniqueName="column7">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="eff_date" HeaderText="Eff. Date" 
                                                                UniqueName="column8" DataType="System.DateTime" DataFormatString="{0:d}">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="val_date" DataFormatString="{0:d}" HeaderText="Val. Date" 
                                                                UniqueName="column9">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="plan_desc" HeaderText="Plan" 
                                                                UniqueName="column10">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="dep_description" HeaderText="Relation" 
                                                                UniqueName="column14">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="area_desc" HeaderText="Area" 
                                                                UniqueName="column11">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                   
                                                </telerik:RadGrid>--%>

                                                 <asp:GridView ID="grdMembersDep" runat="server" 
                                             CssClass="table table-striped table-bordered bootstrap-datatable tblCons grid-setting" 
                                             AutoGenerateColumns="False"   >
                                             <Columns>
                                                 <asp:BoundField DataField="prin_compID" HeaderText="Prin. EmpID" />
                                                 <asp:BoundField DataField="principal" HeaderText="principal" />
                                                 <asp:BoundField DataField="DEP_EMPID" HeaderText="Emp ID" />
                                                 <asp:BoundField DataField="dep_code" HeaderText="Mem. Code" />
                                                 <asp:BoundField DataField="MEM_NAME" HeaderText="Name" />
                                                 <asp:BoundField DataField="mem_bday" HeaderText="Birthday" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle Width="70px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="mem_age" HeaderText="Age" 
                                                     DataFormatString="{0:G2}" />
                                                 <asp:BoundField DataField="sex_desc" HeaderText="Sex" />
                                                 <asp:BoundField DataField="memcstat_desc" HeaderText="Status" />
                                                 <asp:BoundField DataField="eff_date" HeaderText="Eff. Date" 
                                                     DataFormatString="{0:MM/dd/yyyy}" >
                                                 <ItemStyle CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="VAL_DATE" DataFormatString="{0:MM/dd/yyyy}" 
                                                     HeaderText="Val. Date">
                                                 <ItemStyle Width="70px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="DATE_RESGN" DataFormatString="{0:MM/dd/yyyy}" 
                                                     HeaderText="Resgn. Date">
                                                 <ItemStyle Width="70px" CssClass="grid-date" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="plan_desc" HeaderText="Plan" />
                                                 <asp:BoundField DataField="area_desc" HeaderText="Area" />
                                             </Columns>
                                        </asp:GridView>
                                            </telerik:RadPageView>
                                    
                            </telerik:RadMultiPage>
                            
                         
                        </div>  
                    </div>
                    
                </div>
              </div>

                
          
		  
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
