Imports Mailhelper
Imports System.Security.Cryptography
Public Class Profile
    Inherits System.Web.UI.Page
    Dim memberCode As String
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Dim sPword As String = String.Empty
    Dim md5hash As MD5 = MD5.Create()
    Dim enc As New EncryptDecrypt.EncryptDecrypt

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Request.QueryString("u").Length > 0 Then
                    userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                    Dim bll As New ememberBLL(userCode)
                    bll.GetMemberInformation(userCode)

                    memberCode = bll.MemberCode
                    strError = bll.LoginMessage
                    With bll
                        lblAge.Text = bll.Age
                        lblBirthday.Text = bll.Birthday
                        lblCivilStatus.Text = bll.CivilStatus
                        lblCompany.Text = .Company
                        lblGender.Text = .Gender
                        lblName.Text = .MemberName
                        txtEmail.Text = .EmailAddress
                        lblUsername.Text = .UserCode
                        sPword = .Password
                    End With
                End If
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub btnRequest_Click(sender As Object, e As EventArgs) Handles btnRequest.Click
        Try
            Using bll = New ememberBLL

                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                bll.GetMemberInformation(userCode)

                If bll.SendInforRequest(txtDetails.Text, lblName.Text) Then
                    btnRequest.Enabled = False
                    lblMessage.Text = "Your request has been sent to Underwriting Department for approval and updating."
                End If
                btnRequest.Enabled = True
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click

        Try
            If Trim(txtPassword.Text) = "" Then
                RequiredFieldValidator3.ErrorMessage = "Password is required!"
                RequiredFieldValidator3.IsValid = False
                Exit Sub
            End If

            If txtConfirmPword.Text = "" Then
                CompareValidator1.ErrorMessage = "Confirm password is required!"
                CompareValidator1.IsValid = False
                Exit Sub
            Else
                If txtPassword.Text <> txtConfirmPword.Text Then
                    CompareValidator1.ErrorMessage = "Confirm password doesn't match!"
                    CompareValidator1.IsValid = False
                    Exit Sub
                End If
            End If
            Using bll = New ememberBLL(userCode)
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                bll.GetMemberInformation(userCode)
                sPword = bll.Password
                If sPword = enc.GetMd5Hash(md5hash, txtOldPword.Text) Then
                    If bll.UpdateMemberLoginAccount(lblUsername.Text, txtEmail.Text, txtPassword.Text) Then
                        lblMessage.Text = "Your account has been changed."
                    End If

                Else
                    CustomValidator1.ErrorMessage = "Old password doesn't match!"
                    CustomValidator1.IsValid = False
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class