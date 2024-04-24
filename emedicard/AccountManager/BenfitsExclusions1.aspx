<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard1.Master" CodeBehind="BenfitsExclusions1.aspx.vb" Inherits="emedicard.BenfitsExclusions1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/AccountManager/uctl/left-menu.ascx"%>
<%@  Register TagPrefix="uc" TagName="AcctInfo" Src="~/AccountManager/uctl/AccountInformation.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <!-- left menu starts -->
			<!-- left menu ends -->
		
			


<div id="Div1" class="span10">
			<!-- content starts -->
              
            
              <div class="row-fluid sortable">
				<div class="box span12">
					
					<div class="box-content">
                        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="1" 
                            MultiPageID="RadMultiPage1" Visible=false>
                            <Tabs>
                                <telerik:RadTab runat="server" Text="Hospitalization and Out-Patient Services" 
                                    PageViewID="RadPageView1">
                                </telerik:RadTab>
                                <%--<telerik:RadTab runat="server" Text="Preventive Healthcare Services" 
                                    PageViewID="RadPageView2" Selected="True">
                                </telerik:RadTab>--%>
                                <telerik:RadTab runat="server" Text="Emergency Care Services" 
                                    PageViewID="RadPageView3">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" Text="Member Finacial Assistance" 
                                    PageViewID="RadPageView4">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" Text="Dental Care Services" 
                                    PageViewID="RadPageView5">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" PageViewID="RadPageView7" 
                                    Text="Pre-Existing Condition">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" PageViewID="RadPageView8" 
                                    Text="Point of Service">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" PageViewID="RadPageView9" Text="Maternity">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" PageViewID="RadPageView10" 
                                    Text="Executive Check Up">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" Text="Exclusions" PageViewID="RadPageView6" >
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" PageViewID="RadPageView11" 
                                    Text="Dreaded Disease">
                                </telerik:RadTab>
                                <telerik:RadTab runat="server" PageViewID="RadPageView12" 
                                    Text="Membership Eligibility">
                                </telerik:RadTab>
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="1">
                            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true">                               
                                <div style="margin: 10px;"> 
                                    <% If sPackageCode = "PKG0000005" Then%>
                                        <div id="tabs">
                                         <ul>
                                            <li><a href="#tabs-1">Principal Member</a></li>
                                            <li><a href="#tabs-2">Qualified Dependent</a></li>
                                            <li><a href="#tabs-3">Extended Dependent</a></li>
                                        </ul>
                                        <%--<h3>Principal Member</h3>--%>
                                        <div id="tabs-1">
                                        <div class="eAccdtls">  
                                            <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                            <% Else%>
                                                <% If dtHCSPrn.Rows.Count > 0 Then
                                                        If dtHCSPrn.Rows(0)("HCS_NO_BENEFIT") Then%>  
                                                    <p style="color: Red; font: bold 14px;">NO BENEFITS</p>  
                                                <% Else%>                       
                                                    <label class="eAccLabels">Laparoscopic Surgery and Lithotripsy :&nbsp;</label><asp:Label ID="lblLapSurgP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>                                       
                                                    <label class="eAccLabels">Chemo/Radiotheraphy/Dialysis :&nbsp;</label><asp:Label ID="lblChemoP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>                                       
                                                    <label class="eAccLabels">MRI/CT Scan/UTS :&nbsp;</label><asp:Label ID="lblMRIP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Human blood products :&nbsp;</label><asp:Label ID="lblHumBloodP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Complex Diagnostic exams :&nbsp;</label><asp:Label ID="lblComplexDiagP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Work Related Illness/Accidents :&nbsp;</label><asp:Label ID="lblWorkIllP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Organ Transplant :&nbsp;</label><asp:Label ID="lblOrgTrans" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Congenital Illness :&nbsp;</label><asp:Label ID="lblCongIllP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Open Heart Surgery :&nbsp;</label><asp:Label ID="lblOpenSurgP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Cataract Extraction :&nbsp;</label><asp:Label ID="lblCataractP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">PT/Speech Theraphy :&nbsp;</label><asp:Label ID="lblPTP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">1st Dose of Anti-rabies :&nbsp;</label><asp:Label ID="lblAntiRabP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Laser Treatment for Glaucoma :&nbsp;</label><asp:Label ID="lblLTGlaucuomaP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/><br/>
                                                <% End If
                                                End If%>

                                               <legend><h4>OTHERS</h4></legend>
                                               <div id="hcsOtherPRN" runat="server">
                                                   <asp:GridView ID="dtgHCSOtherPRN" runat="server" 
                                                       class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                       <Columns>
                                                           <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                           <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                       </Columns>
                                                   </asp:GridView>
                                               </div>
                                            <% End If%>
                                        </div><!-- eAccdtls -->
                                        </div><!-- tab 1 -->
    <%--                                    <h3>Qualified Dependent</h3> --%>
                                        <div id="tabs-2">
                                        <div class="eAccdtls"> 
                                            <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                            <% Else%>  
                                                <%  
                                                    If dtHCSDep.Rows.Count > 0 Then
                                                        If dtHCSDep.Rows(0)("HCS_NO_BENEFIT") Then%>   
                                                    <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                <% Else%>                     
                                                    <label class="eAccLabels">Laparoscopic Surgery and Lithotripsy :&nbsp;</label><asp:Label ID="lblLapSurgQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>                                       
                                                    <label class="eAccLabels">Chemo/Radiotheraphy/Dialysis :&nbsp;</label><asp:Label ID="lblChemoQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>                                       
                                                    <label class="eAccLabels">MRI/CT Scan/UTS :&nbsp;</label><asp:Label ID="lblMRIQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Human blood products :&nbsp;</label><asp:Label ID="lblHumBloodQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Complex Diagnostic exams :&nbsp;</label><asp:Label ID="lblComplexDiagQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Work Related Illness/Accidents :&nbsp;</label><asp:Label ID="lblWorkIllQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Congenital Illness :&nbsp;</label><asp:Label ID="lblCongIllQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Open Heart Surgery :&nbsp;</label><asp:Label ID="lblOpenSurgQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Cataract Extraction :&nbsp;</label><asp:Label ID="lblCataractQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">PT/Speech Theraphy :&nbsp;</label><asp:Label ID="lblPTQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">1st Dose of Anti-rabies :&nbsp;</label><asp:Label ID="lblAntiRabQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Laser Treatment for Glaucoma :&nbsp;</label><asp:Label ID="lblLTGlaucuomaQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/><br />
                                                <% End If
                                                                                          End If%>
                                               <legend><h4>OTHERS</h4></legend>
                                               <div id="hcsOtherDEP" runat="server">
                                                   <asp:GridView ID="dtgHCSOtherDEP" runat="server" 
                                                       class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                       <Columns>
                                                           <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                           <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                       </Columns>
                                                   </asp:GridView>
                                               </div>
                                            <% End If%>
                                        </div><!-- eAccdtls -->
                                        </div><!-- tab 2 -->
                                        <%--<h3>Extended Dependent</h3>  --%>
                                        <div id="tabs-3">
                                        <div class="eAccdtls"> 
                                            <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                            <% Else%>  
                                                <% If dtHCSExt.Rows.Count > 0 Then

                                                        If dtHCSExt.Rows(0)("HCS_NO_BENEFIT") Then%> 
                                                    <p style="color: Red; font: bold 14px;">NO BENEFITS</p>    
                                                <% Else%>                       
                                                    <label class="eAccLabels">Laparoscopic Surgery and Lithotripsy :&nbsp;</label><asp:Label ID="lblLapSurg" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>                                       
                                                    <label class="eAccLabels">Chemo/Radiotheraphy/Dialysis :&nbsp;</label><asp:Label ID="lblChemo" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>                                       
                                                    <label class="eAccLabels">MRI/CT Scan/UTS :&nbsp;</label><asp:Label ID="lblMRI" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Human blood products :&nbsp;</label><asp:Label ID="lblHumBlood" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Complex Diagnostic exams :&nbsp;</label><asp:Label ID="lblComplexDiag" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Work Related Illness/Accidents :&nbsp;</label><asp:Label ID="lblWorkIll" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">Congenital Illness :&nbsp;</label> <span><asp:Label ID="lblCongIll" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Open Heart Surgery :&nbsp;</label><asp:Label ID="lblOpenSurg" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Cataract Extraction :&nbsp;</label><asp:Label ID="lblCataract" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/>
                                                    <label class="eAccLabels">PT/Speech Theraphy :&nbsp;</label><asp:Label ID="lblPT" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">1st Dose of Anti-rabies :&nbsp;</label><asp:Label ID="lblAntiRab" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Laser Treatment for Glaucoma :&nbsp;</label><asp:Label ID="lblLTGlaucuoma" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/><br />
                                                <% End If
                                                End If%>
                                               <legend><h4>OTHERS</h4></legend>
                                               <div id="hcsOtherEXT" runat="server">
                                                   <asp:GridView ID="dtgHCSOtherEXT" runat="server" 
                                                       class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                       <Columns>
                                                           <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                           <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                       </Columns>
                                                   </asp:GridView>
                                               </div>
                                            <% End If%>
                                        </div><!-- eAccdtls -->
                                        </div><!-- tab 3 -->
                                        </div><!--tabs -->

                                    <% Else%>
                                        <% LoadStandard("HCS")%>
                                        <div class="eAccdtls">
                                            <div id="standardHCS" runat="server"></div>
                                        </div>
                                    <% End If%>
                                </div>                          
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView2" runat="server">
                                <div style="margin: 10px;">
                                    <% If sPackageCode = "PKG0000005" Then%>
                                        <div id="tabPHS">
                                         <ul>
                                            <li><a href="#tabPHS-1">Principal Member</a></li>
                                            <li><a href="#tabPHS-2">Qualified Dependent</a></li>
                                            <li><a href="#tabPHS-3">Extended Dependent</a></li>
                                        </ul>
                                        <%--<h3>Principal Member</h3> --%>
                                        <div id="tabPHS-1">
                                        <div class="eAccdtls"> 
                                            <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                            <% Else%>  
                                                <% 
                                                    If dtPHCSPrn.Rows.Count > 0 Then
                                                        If dtPHCSPrn.Rows(0)("PHCS_NO_BENEFIT") Then%>   
                                                    <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                <% Else%>      
                                                    <label class="eAccLabels">MEDICard HO :&nbsp;</label><asp:Image ID="imgMedHOP" runat="server" ImageUrl="~/img/tick.ico" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">At company premises :&nbsp;</label><asp:Image ID="imgCompPremP" runat="server" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">Nearest Hospital and Clinic :&nbsp;</label><asp:Image ID="imgNearHospP" runat="server" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">ECG for :&nbsp;</label><asp:Label ID="lblECGP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Pap smear for :&nbsp;</label><asp:Label ID="lblPapsmearP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Eye refraction :&nbsp;</label><asp:Image ID="imgEyeRefP" runat="server" CssClass="toggleChk"/>
                                                    <br/><br/>                                   
                                                <% End If
                                                End If%>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="phcsOtherPRN" runat="server">
                                                       <asp:GridView ID="dtgPHCSOtherPRN" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                            <% End If%>
                                        </div><!-- eAccdtls -->
                                        </div><!-- tabPHS-1 -->
                                        <%--<h3>Qualified Dependent</h3> --%>
                                        <div id="tabPHS-2">
                                        <div class="eAccdtls"> 
                                            <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                            <% Else%>  
                                                <% If dtPHCSDep.Rows.Count > 0 Then
                                                        If dtPHCSDep.Rows(0)("PHCS_NO_BENEFIT") Then%>   
                                                    <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                <% Else%>      
                                                    <label class="eAccLabels">MEDICard HO :&nbsp;</label><asp:Image ID="imgMedHOQ" runat="server" ImageUrl="~/img/tick.ico" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">At company premises :&nbsp;</label><asp:Image ID="imgCompPremQ" runat="server" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">Nearest Hospital and Clinic :&nbsp;</label><asp:Image ID="imgNearHospQ" runat="server" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">ECG for :&nbsp;</label><asp:Label ID="lblECGQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Pap smear for :&nbsp;</label><asp:Label ID="lblPapsmearQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Eye refraction :&nbsp;</label><asp:Image ID="imgEyeRefQ" runat="server" CssClass="toggleChk"/>                                     
                                                    <br/><br/>
                                                <% End If
                                                End If%>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="phcsOtherDEP" runat="server">
                                                       <asp:GridView ID="dtgPHCSOtherDEP" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                            <% End If%>
                                        </div><!-- eAccdtls -->
                                        </div><!-- tabPHS-2 -->
                                        <%--<h3>Extended Dependent</h3> --%>
                                        <div id="tabPHS-3">
                                        <div class="eAccdtls"> 
                                            <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                            <% Else%>  
                                                <%  If dtPHCSExt.Rows.Count > 0 Then
                                                        If dtPHCSExt.Rows(0)("PHCS_NO_BENEFIT") Then%>   
                                                    <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                <% Else%>      
                                                    <label class="eAccLabels">MEDICard HO :&nbsp;</label><asp:Image ID="imgMedHOE" runat="server" ImageUrl="~/img/tick.ico" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">At company premises :&nbsp;</label><asp:Image ID="imgCompPremE" runat="server" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">Nearest Hospital and Clinic :&nbsp;</label><asp:Image ID="imgNearHospE" runat="server" CssClass="toggleChk"/>
                                                    <br/> 
                                                    <label class="eAccLabels">ECG for :&nbsp;</label><asp:Label ID="lblECGE" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Pap smear for :&nbsp;</label><asp:Label ID="lblPapsmearE" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                    <br/> 
                                                    <label class="eAccLabels">Eye refraction :&nbsp;</label><asp:Image ID="imgEyeRefE" runat="server" CssClass="toggleChk"/>                                     
                                                    <br/><br/>
                                                <% End If
                                                End If%>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="phcsOtherEXT" runat="server">
                                                       <asp:GridView ID="dtgPHCSOtherEXT" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                            <% End If%>
                                        </div><!-- eAccdtls -->
                                        </div><!-- tabPHS-3 -->
                                        </div><!-- tabPHS -->
                                    <% Else%>
                                        <% LoadStandard("PHCS")%>
                                        <div class="eAccdtls">
                                            <div id="standardPHCS" runat="server"></div>
                                        </div>
                                    <% End If%>                              
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView3" runat="server">
                            <%-- ECS--%>
                            <div style="margin: 10px;">
                                <% If sPackageCode = "PKG0000005" Then%>
                                    <% If Request.QueryString("a") = "04102003-000081" Then%>
                                        <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                    <% Else%>
                                         <div id="tabECS">
                                             <ul>
                                                <li><a href="#tabECS-1">Principal Member</a></li>
                                                <li><a href="#tabECS-2">Qualified Dependent</a></li>
                                                <li><a href="#tabECS-3">Extended Dependent</a></li>
                                            </ul> 
                                            <div id="tabECS-1">
                                                <div class="eAccdtls">
                                                    <div id="tblContentP" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="ecsOtherPRN" runat="server">
                                                       <asp:GridView ID="dtgECSOtherPRN" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>
                                            <div id="tabECS-2">
                                                <div class="eAccdtls">
                                                    <div id="tblContentQ" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="ecsOtherDEP" runat="server">
                                                       <asp:GridView ID="dtgECSOtherDEP" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>
                                            <div id="tabECS-3">
                                                <div class="eAccdtls">
                                                    <div id="tblContentE" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="ecsOtherEXT" runat="server">
                                                       <asp:GridView ID="dtgECSOtherEXT" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>                                           
                                        </div>
  
                                    <% End If%>

                                <% Else%>
                                    <% LoadStandard("ECS")%>
                                    <div class="eAccdtls">
                                        <div id="standardECS" runat="server"></div>
                                    </div>
                                <% End If%>
                            </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView4" runat="server">
                            <%-- MFA--%>
                            <div style="margin: 10px;">
                                <% If sPackageCode = "PKG0000005" Then%>
                                    <% If Request.QueryString("a") = "04102003-000081" Then%>
                                        <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                    <% Else%>
                                        <div id="tabMFA">
                                             <ul>
                                                <li><a href="#tabMFA-1">Principal Member</a></li>
                                                <li><a href="#tabMFA-2">Qualified Dependent</a></li>
                                                <li><a href="#tabMFA-3">Extended Dependent</a></li>
                                            </ul>
                                            <div id="tabMFA-1">
                                                <div class="eAccdtls">
                                                   <div id="tblMFAP" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="mfaOtherPRN" runat="server">
                                                       <asp:GridView ID="dtgMFAOtherPRN" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>
                                            <div id="tabMFA-2">
                                                <div class="eAccdtls">
                                                   <div id="tblMFAQ" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="mfaOtherDEP" runat="server">
                                                       <asp:GridView ID="dtgMFAOtherDEP" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>
                                            <div id="tabMFA-3">
                                                <div class="eAccdtls">
                                                    <div id="tblMFAE" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="mfaOtherEXT" runat="server">
                                                       <asp:GridView ID="dtgMFAOtherEXT" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div> 
                                        </div>       
                                        
                                    <% End If%>

                                <% Else%>
                                    <% LoadStandard("MFA")%>
                                    <div class="eAccdtls">
                                        <div id="standardMFA" runat="server"></div>
                                    </div>
                                <% End If%>
                            </div>                                
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView5" runat="server">
                                <div style="margin: 10px;">
                                    <% If sPackageCode = "PKG0000005" Then%>
                                        <%--<h3>Principal Member</h3> --%>
                                        <div id="tabDCS">
                                             <ul>
                                                <li><a href="#tabDCS-1">Principal Member</a></li>
                                                <li><a href="#tabDCS-2">Qualified Dependent</a></li>
                                                <li><a href="#tabDCS-3">Extended Dependent</a></li>
                                            </ul>
                                            <div id="tabDCS-1">
                                                <div class="eAccdtls"> 
                                                    <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                        <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                                    <% Else%>  
                                                        <% If dtDCSP.Rows.Count > 0 Then
                                                                If dtDCSP.Rows(0)("DCS_NO_BENEFIT") Then%>   
                                                            <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                        <% Else%>       
                                                            <label class="eAccLabels">Permanent Amalgam Filling :&nbsp;</label><asp:Label ID="lblFillingP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/> 
                                                            <label class="eAccLabels">Prophylaxis :&nbsp;</label><asp:Label ID="lblProphylaxisP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/>   
                                                            <label class="eAccLabels">Light Cure :&nbsp;</label><asp:Label ID="lblLigtCureP" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/> <br />                                     
                                                        <% End If
                                                        End If%>
                                                       <legend><h4>OTHERS</h4></legend>
                                                       <div id="dcsOtherPRN" runat="server">
                                                           <asp:GridView ID="dtgDCSOtherPRN" runat="server" 
                                                               class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                               <Columns>
                                                                   <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                                   <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                               </Columns>
                                                           </asp:GridView>
                                                       </div>
                                                    <% End If%>
                                                </div> 
                                            </div>
                                            <div id="tabDCS-2">
                                                <div class="eAccdtls"> 
                                                    <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                        <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                                    <% Else%>  
                                                        <% If dtDCSQ.Rows.Count > 0 Then
                                                                If dtDCSQ.Rows(0)("DCS_NO_BENEFIT") Then%>   
                                                            <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                        <% Else%>      
                                                            <label class="eAccLabels">Permanent Amalgam Filling :&nbsp;</label><asp:Label ID="lblFillingQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/> 
                                                            <label class="eAccLabels">Prophylaxis :&nbsp;</label><asp:Label ID="lblProphylaxisQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/>   
                                                            <label class="eAccLabels">Light Cure :&nbsp;</label><asp:Label ID="lblLigtCureQ" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/> <br />
                                                        <% End If
                                                        End If%>
                                                       <legend><h4>OTHERS</h4></legend>
                                                       <div id="dcsOtherDEP" runat="server">
                                                           <asp:GridView ID="dtgDCSOtherDEP" runat="server" 
                                                               class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                               <Columns>
                                                                   <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                                   <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                               </Columns>
                                                           </asp:GridView>
                                                       </div>
                                                    <% End If%>
                                                </div> 
                                            </div>
                                            <div id="tabDCS-3">
                                                <div class="eAccdtls"> 
                                                    <% If Request.QueryString("a") = "04102003-000081" Then%>
                                                        <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                                    <% Else%>  
                                                        <% If dtDCSE.Rows.Count > 0 Then
                                                                If dtDCSE.Rows(0)("DCS_NO_BENEFIT") Then%>   
                                                            <p style="color: Red; font: bold 14px;">NO BENEFITS</p>   
                                                        <% Else%>      
                                                            <label class="eAccLabels">Permanent Amalgam Filling :&nbsp;</label><asp:Label ID="lblFillingE" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/> 
                                                            <label class="eAccLabels">Prophylaxis :&nbsp;</label><asp:Label ID="lblProphylaxisE" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/>   
                                                            <label class="eAccLabels">Light Cure :&nbsp;</label><asp:Label ID="lblLigtCureE" runat="server" Text="" CssClass="eAccLabelValue"></asp:Label>
                                                            <br/> <br />
                                                        <% End If
                                                        End If%>
                                                       <legend><h4>OTHERS</h4></legend>
                                                       <div id="dcsOtherEXT" runat="server">
                                                           <asp:GridView ID="dtgDCSOtherEXT" runat="server" 
                                                               class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                               <Columns>
                                                                   <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                                   <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                               </Columns>
                                                           </asp:GridView>
                                                       </div>
                                                    <% End If%>
                                                </div>
                                            </div>
                                        </div><!-- tabDCS -->
                                    <% Else%>
                                        <% LoadStandard("DCS")%>
                                        <div class="eAccdtls">
                                            <div id="standardDCS" runat="server"></div>
                                        </div>
                                    <% End If%>    
                                </div>                             
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView6" runat="server">
                                <% If sPackageCode = "PKG0000005" Then%>
                                    <div class="eAccdtls" style="padding: 10px 15px 10px 15px;">                                   
                                        <% If Request.QueryString("a") = "04102003-000081" Then%>
                                            <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                        <% Else%>
                                            <h3>Exclusions for this Company</h3><br />
                                            <div id="DivExclusion" runat="server"></div>
                                        <% End If%>
                                    </div>
                                <% End If%>                                
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView7" runat="server">
                            <%-- PEC --%>
                            <div style="margin: 10px;">
                                <% If sPackageCode = "PKG0000005" Then%>
                                    <% If Request.QueryString("a") = "04102003-000081" Then%>
                                        <p style="color: Red; font: bold 14px;">See Doc Dennis for details.</p>
                                    <% Else%>
                                         <div id="tabPEC">
                                             <ul>
                                                <li><a href="#tabPEC-1">Principal Member</a></li>
                                                <li><a href="#tabPEC-2">Qualified Dependent</a></li>
                                                <li><a href="#tabPEC-3">Extended Dependent</a></li>
                                            </ul> 
                                            <div id="tabPEC-1">
                                                <div class="eAccdtls">
                                                    <div id="tblPECP" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="pecOtherPRN" runat="server">
                                                       <asp:GridView ID="dtgOthersPECP" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>
                                            <div id="tabPEC-2">
                                                <div class="eAccdtls">
                                                    <div id="tblPECD" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="pecOtherDEP" runat="server">
                                                       <asp:GridView ID="dtgOthersPECD" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>
                                            <div id="tabPEC-3">
                                                <div class="eAccdtls">
                                                    <div id="tblPECE" runat="server"></div>
                                                   <legend><h4>OTHERS</h4></legend>
                                                   <div id="pecOtherEXT" runat="server">
                                                       <asp:GridView ID="dtgOthersPECE" runat="server" 
                                                           class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                                           <Columns>
                                                               <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                               <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                                           </Columns>
                                                       </asp:GridView>
                                                   </div>
                                                </div><!-- eAccdtls -->
                                            </div>                                           
                                        </div>
  
                                    <% End If%>

                                <% Else%>
                                    <% LoadStandard("PEC")%>
                                    <div class="eAccdtls">
                                        <div id="standardPEC" runat="server"></div>
                                    </div>
                                <% End If%>
                            </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView8" runat="server">
                                <div class="eAccdtls">
                                    <div id="posContent" runat="server"></div>
                                </div> 
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView9" runat="server">
                                <div class="eAccdtls">
                                    <div id="matContent" runat="server"></div>
                                    <div id="matOther" runat="server">
                                        <asp:GridView ID="dtgMatOthers" runat="server" 
                                            class="table table-bordered table-striped tblCons" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="BENEFIT_DESC" HeaderText="Description" />
                                                <asp:BoundField DataField="RSOOTHERDTL_COL1" HeaderText="Value" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div> 
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView10" runat="server">
                                <div class="eAccdtls">
                                    <br />
                                    <div id="execContent" runat="server"></div>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView11" runat="server">
                                <% LoadStandard("DRD")%>
                                <div class="eAccdtls">
                                    <div id="drdContent" runat="server"></div>
                                </div>
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="RadPageView12" runat="server">
                                <% LoadStandard("ME")%>
                                <div class="eAccdtls">
                                    <div id="meContent" runat="server"></div>
                                </div>
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </div>
                    
                </div>
              </div>
       
					<!-- content ends -->
			   
    </div>
</asp:Content>
