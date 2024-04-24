Public Class EnrollDependent
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("u").Length > 0 Then
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Using bll As New ememberBLL(userCode)
                    grdDependents.DataSource = bll.EnrolledDependents
                    grdDependents.DataBind()

                End Using
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
        


    End Sub

    Protected Sub grdDependents_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdDependents.SelectedIndexChanged

    End Sub
End Class