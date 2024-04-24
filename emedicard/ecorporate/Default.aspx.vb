Imports emedicardBLL
Public Class _Default2
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Request.QueryString("u").Length > 0 Then
                Dim s As String = Request.QueryString("u")
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(s, key)
                Dim ecorp As New eCorporateBLL(userCode)
                With ecorp
                    lblAddress.Text = .Address
                    lblCompany.Text = .CompanyName
                    lblDesignation.Text = .Designation
                    lblEmail.Text = .EmailAddress
                    lblFax.Text = .Fax
                    lblMobile.Text = .Mobile
                    lblName.Text = .Firstname & " " & .Lastname
                    lblTelephone.Text = .Phone
                    lblUsername.Text = .Username
                End With


            End If
        Catch ex As Exception

        End Try
    End Sub

    
End Class