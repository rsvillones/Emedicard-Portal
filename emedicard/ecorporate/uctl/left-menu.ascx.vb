Imports emedicardBLL
Imports EncryptDecrypt
Public Class left_menu2
    Inherits System.Web.UI.UserControl
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Using ecorp = New eCorporateBLL(EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key))
                'If ecorp.AccessLevel = 2 Then lnk3.Visible = False
            End Using
        Catch ex As Exception

        End Try
    End Sub

End Class