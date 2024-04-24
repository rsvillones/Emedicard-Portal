Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Public Class UserManagement
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Public oEAcctBLL As eAccountBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Load_AgentInfo()
        If oEAcctBLL.AccessLevel = 1 Then
            GetAgentUsers()
        End If
    End Sub

    Private Sub GetAgentUsers()
        dtgResult.DataSource = oEAcctBLL.GetAgentUsers()
        dtgResult.DataBind()
    End Sub

    Private Sub Load_AgentInfo()
        oEAcctBLL = New eAccountBLL
        With oEAcctBLL
            .Username = Decrypt(Request.QueryString("u"), key)
            .GetAgentInfo()
        End With

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("AddAgentUser.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")) & "&agnt=" & Request.QueryString("agnt"))
    End Sub
End Class