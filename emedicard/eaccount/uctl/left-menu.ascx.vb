Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Public Class left_menu1
    Inherits System.Web.UI.UserControl
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Public oEAcctBLL As eAccountBLL
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Load_AgentInfo()
    End Sub
    Private Sub Load_AgentInfo()
        oEAcctBLL = New eAccountBLL
        With oEAcctBLL
            .Username = Decrypt(Request.QueryString("u"), key)
            .GetAgentInfo()
        End With

    End Sub
End Class