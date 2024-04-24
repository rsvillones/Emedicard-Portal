Imports emedicardBLL
Public Class UserManager
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Request.QueryString("c") Is Nothing Then
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Using ecorp = New eCorporateBLL(userCode, Nothing, Request.QueryString("c"))
                    grdUsers.DataSource = ecorp.eCorporateUsers
                    grdUsers.DataBind()
                End Using
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Response.Redirect("~/ecorporate/AddUser.aspx?t=" & Request.QueryString("t") & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")))
    End Sub
End Class