<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AccountList.ascx.vb" Inherits="emedicard.AccountList" %>
<asp:GridView ID="grdAccountList" runat="server" 
    CssClass="table table-striped table-bordered bootstrap-datatable tblCons" 
    EmptyDataText="No data to display" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="ACCOUNTNAME" HeaderText="Account Name" />
        <asp:BoundField DataField="ACCOUNTCODE" HeaderText="Account Code" />
        <asp:TemplateField>
            <ItemTemplate>
                <a id="lnk1"  runat="server"
                    href='<%# String.Format("~/AccountManager/Default.aspx?a={0}&c={1}&t={2}&u={3}&agnt={4}",Eval("ACCOUNTCODE"),Request.Querystring("c"), Request.Querystring("t"), httputility.urlencode(Request.Querystring("u")),  Request.Querystring("agnt")) %>' title='' >                             
                    Manage Account
                </a>&nbsp;
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
