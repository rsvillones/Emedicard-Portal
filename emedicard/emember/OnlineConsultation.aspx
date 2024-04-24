<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/emedicard.Master" CodeBehind="OnlineConsultation.aspx.vb" Inherits="emedicard.OnlineConsultation" %>
<%@  Register TagPrefix="uc" TagName="LeftNav" Src="~/emember/uctl/emember_left_menu.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc:LeftNav ID="LeftNav1" runat="server" /> 
<div id="content" class="span10">
		<h2>Online Consultation</h2>
          <div class="row-fluid sortable">
				<div class="box span12">
					<div class="box-header well" data-original-title>
						<h2><i class="icon-info-sign"></i>Consult to doctor</h2>
						<div class="box-icon">
							
							<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>
							
						</div>
					</div>
					<div class="box-content">
                  	<div class="row-fluid">
                     
                            <div class="acct-details" style="padding-right: 10px;">
                                <strong><span>My Condition:</span></strong><br />
                                <asp:TextBox ID="txtConsultation" runat="server" style="width: 100%; max-width: 250px;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Condition is required." 
                                CssClass="label label-warning" Display="Dynamic" ControlToValidate="txtConsultation"></asp:RequiredFieldValidator><br /><br />
                                <strong><span>Details/Symptoms:</span></strong><br />
                                <asp:TextBox ID="txtSymptoms" runat="server" Rows="4" 
                                                TextMode="MultiLine" style="width: 100%; max-width: 500px;"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Details/Symptoms is required." CssClass="label label-warning" Display="Dynamic" 
                                ControlToValidate="txtSymptoms"></asp:RequiredFieldValidator> <br />                            
                                <strong><span>Doctor:</span></strong><br />
                                <asp:DropDownList ID="ddlDoctors" runat="server" style="width: 100%; max-width: 300px;">
                                            </asp:DropDownList><br /><br />
                                <asp:Button ID="btnSaveConsult" runat="server" Text="Submit" CssClass="btn btn-primary" />                                           
<%--                                <strong><span>My Condition:</span></strong><br />
                                <strong><asp:TextBox ID="txtConsultation" runat="server"></strong><br />
                                <strong><span>Details/Symptoms:</span></strong><br />
                                <asp:TextBox ID="txtSymptoms" runat="server" Rows="4" TextMode="MultiLine" Width="150%"></asp:TextBox><br />
                                <strong><span>Doctor:</span></strong> <br />      
                                <asp:DropDownList ID="ddlDoctors" runat="server">
                                            </asp:DropDownList><br />
                                <asp:Button ID="btnSaveConsult" runat="server" Text="Submit" />--%>
                                       
                            </div>
                      
                    </div>    
                                 
                  </div>
				</div><!--/span-->
			</div><!--/row-->

    <div class="row-fluid sortable">
		<div class="box span12">
            <div class="box-header well" data-original-title>
				<h2><i class="icon-info-sign"></i>Consultations</h2>
				<div class="box-icon">							
					<a href="#" class="btn btn-minimize btn-round"><i class="icon-chevron-up"></i></a>							
				</div>
			</div>
			<div class="box-content">
                <div class="row-fluid">
                    <div id="divcosult" runat="server">
                        
                    </div>
<%--                    <asp:GridView ID="dtgResult" runat="server" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="consultationTitle" HeaderText="Condition" />
                            <asp:BoundField DataField="doctor_name" HeaderText="Doctor" />
                            <asp:BoundField DataField="createdDate" HeaderText="Consultation Date" 
                                DataFormatString="{0:d}" />   
                            <asp:BoundField DataField="new_msg" HeaderText="Replies" />                         
                        </Columns>
                    </asp:GridView>--%>

                </div>
            </div>
        </div>
    </div>
</div> 
                                </strong>
    </div>
</asp:Content>
