Public Class _Default
    Inherits System.Web.UI.Page
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim userCode As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strRedirectPath As String = String.Empty
        Dim u As String = Request.QueryString("u")

        If Not Request.QueryString("u") Is Nothing Then
            Select Case Request.QueryString("t").ToString
                Case "0"
                    strRedirectPath = "~/emember/?u=" & HttpUtility.UrlEncode(u)
                Case "1"
                    strRedirectPath = String.Format("~/ecorporate/?t=1&u={0}&c={1}", HttpUtility.UrlEncode(u), Request.QueryString("c"))
                Case "2"
                    'strRedirectPath = "~/eaccount/?u=" & EncryptDecrypt.EncryptDecrypt.Encrypt(u, key)
                    strRedirectPath = String.Format("~/eaccount/?t=2&agnt={0}&c={1}&u={2}", Request.QueryString("agnt"), Request.QueryString("c"), HttpUtility.UrlEncode(u))
            End Select
            Response.Redirect(strRedirectPath)
        End If
    End Sub

End Class