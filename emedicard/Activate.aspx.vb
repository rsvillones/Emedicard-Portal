Imports EncryptDecrypt.EncryptDecrypt
Public Class Actiivate
    Inherits System.Web.UI.Page
    Private MemberCode As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("m").Length > 0 OrElse Not Request.QueryString("m") Is Nothing Then
            Try
                MemberCode = Decrypt(Request.QueryString("m"), ConfigurationManager.AppSettings("encryptionKey").ToString())
                Using bll = New ememberBLL
                    If bll.ActivateAccount(MemberCode) Then
                        CustomMessage.InnerHtml = "Your eMember account has been activated. Please try to <a href='Login.aspx'> login </a>."
                    Else
                        CustomMessage.InnerHtml = "Your account is already activated."
                    End If
                End Using
            Catch ex As Exception
                CustomMessage.InnerText = "There's an error on the page. <br />" & ex.Message
            End Try
            
        End If
    End Sub

End Class