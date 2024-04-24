<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditPlans.aspx.vb" Inherits="emedicard.EditPlans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plans to utilize</title>
    <link id="Link18" runat="server" href="~/css/emed.css" rel='stylesheet'/>
    <script src="js/jquery-1.7.2.min.js"></script>
	<!-- jQuery UI -->
	<script src="js/jquery-ui-1.8.21.custom.min.js"></script>
    <script src="js/emed.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-left: 10px">
        <legend>Plans to utilize</legend>	
    <div class="control-group">
        <label><asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" />Select All</label>
        <div id="tblDivPlan" style="height: 450px; overflow: auto">
            <asp:GridView ID="dtgPlans" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered bootstrap-datatable tblCons popup-datagrid" style="max-width: 600px">
                <Columns>
                    <asp:BoundField DataField="RSPROOMRATE_ID" HeaderText="Room Rate ID" />
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPlanSelect" runat="server" CssClass="case"  />
                            <%--<input id="chkPlanSelect" type="checkbox" runat="server" class="case"/>--%>
                        </ItemTemplate>                                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="PLAN_DESC" HeaderText="Plan" />
                    <asp:BoundField DataField="RNB_FOR" HeaderText="RNB For" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="controls" style="margin-top: 20px;">
        <input id="savemsg" runat="server" type="hidden" value="" />
        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" 
            Text="Save Changes" />
    </div>
    <div class="control-group" style="margin-top: 14px;">
        <div class="controls">
            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="alert alert-info" Visible="false"></asp:Label>
        </div>
    </div>
    </div>
    </form>
</body>
</html>
