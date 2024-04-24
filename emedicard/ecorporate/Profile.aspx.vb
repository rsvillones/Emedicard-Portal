Imports emedicardBLL
Public Class Profile1
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblMessage.Visible = False

            If Not Page.IsPostBack Then

                If Not Request.QueryString("c") Is Nothing Then
                    Dim ecorp As New eCorporateBLL(EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key), Nothing, Request.QueryString("c"))

                    txtDesignation.Text = ecorp.Designation
                    txtEmailAddress.Text = ecorp.EmailAddress
                    txtFaxNo.Text = ecorp.Fax
                    txtFirstname.Text = ecorp.Firstname
                    txtLastname.Text = ecorp.Lastname
                    txtMobile.Text = ecorp.Mobile
                    txtTelephone.Text = ecorp.Phone
                    txtUsername.Text = ecorp.Username
                    lblUsername.Text = ecorp.Firstname & " " & ecorp.Lastname

                   
                End If
            End If
        Catch ex As Exception
            lblMessage.Text = "Error encountered. " & ex.Message()
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Using ecorp = New eCorporateBLL(EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key), Nothing, Request.QueryString("c"))
                With ecorp

                    .Designation = txtDesignation.Text
                    .EmailAddress = txtEmailAddress.Text
                    .Fax = txtFaxNo.Text
                    .Firstname = txtFirstname.Text
                    .Lastname = txtLastname.Text
                    .Mobile = txtMobile.Text
                    .Phone = txtTelephone.Text
                    .Username = txtUsername.Text

                    If Len(Trim(Trim(txtOldPassword.Text))) > 0 Then
                        .OldPassword = Trim(txtOldPassword.Text)
                        If .CheckOldPassword = False Then
                            lblMessage.Visible = True
                            lblMessage.Text = "Old Password is wrong."
                            Exit Sub
                        End If


                    End If

                    If txtOldPassword.Text.Trim <> "" And txtNewPassword.Text.Trim = "" Then
                        CustomValidator1.Visible = True
                        CustomValidator1.IsValid = False
                        CustomValidator1.ErrorMessage = "New password is required."
                        Exit Sub
                    End If
                    

                    If txtNewPassword.Text.Trim.Length < 6 And txtNewPassword.Text.Trim <> "" Then
                        CustomValidator1.Visible = True
                        CustomValidator1.IsValid = False
                        CustomValidator1.ErrorMessage = "Password must be atleast 6 alphanumeric characters."
                        Exit Sub
                    End If

                    If txtNewPassword.Text.Trim.Length > 0 And txtOldPassword.Text.Trim = "" Then
                        CustomValidator3.Visible = True
                        CustomValidator3.IsValid = False
                        CustomValidator3.ErrorMessage = "Old password is required."
                        Exit Sub
                    End If

                    If txtConfirm.Text.Trim = "" And txtNewPassword.Text.Trim.Length > 0 And txtOldPassword.Text.Trim <> "" Then
                        CustomValidator2.Visible = True
                        CustomValidator2.IsValid = False
                        CustomValidator2.ErrorMessage = "Confirm password is required."
                        Exit Sub
                    End If

                    If txtConfirm.Text.Trim <> txtNewPassword.Text.Trim And txtOldPassword.Text.Trim <> "" Then
                        CustomValidator2.Visible = True
                        CustomValidator2.IsValid = False
                        CustomValidator2.ErrorMessage = "Password doesn't match."
                        Exit Sub
                    End If

                    If txtConfirm.Text.Trim <> "" And txtNewPassword.Text.Trim = "" Then
                        CustomValidator2.Visible = True
                        CustomValidator2.IsValid = False
                        CustomValidator2.ErrorMessage = "Password doesn't match."
                        Exit Sub
                    End If

                    'Check if use the email address as username

                    .Username = txtUsername.Text

                    .Password = txtNewPassword.Text
                    If .UpdateUserProfile() Then
                        lblMessage.Visible = True
                        lblMessage.Text = "Your profile has been successfully updated."
                    Else
                        lblMessage.Visible = True
                        lblMessage.Text = "Updating of user profile failed. Try again."
                    End If

                    CustomValidator1.Visible = False
                    CustomValidator1.IsValid = True
                    CustomValidator1.ErrorMessage = ""

                    CustomValidator2.Visible = False
                    CustomValidator2.IsValid = True
                    CustomValidator2.ErrorMessage = ""
                End With
            End Using
        Catch ex As Exception
            lblMessage.Visible = True
            lblMessage.Text = "An error encountered. " & ex.Message()
        End Try
    End Sub

    Protected Sub txtEmailAddress_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEmailAddress.TextChanged
        txtUsername.Text = txtEmailAddress.Text
    End Sub
End Class