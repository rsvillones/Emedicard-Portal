Imports emedicardBLL
Imports System.Security.Cryptography

Public Class ChangeEmail
    Inherits System.Web.UI.Page
    Dim md5hash As MD5 = MD5.Create()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnGoTologin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGoTologin.Click
        Response.Redirect("~/Login.aspx")
    End Sub

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSend.Click
        Try

            If rdEmember.Checked Then
                eMember()
            ElseIf rdCorporate.Checked Then
                eCorporate()
            ElseIf rdAgent.Checked Then
                eAccount()
            End If

        Catch ex As Exception
            lblAlert.Text = ex.Message.ToString()
        End Try
    End Sub

    Private Sub eMember()
        Try
            Dim enc As New EncryptDecrypt.EncryptDecrypt
            Using oMember = New emedBLL
                With oMember
                    .UserName = Trim(txtUsername.Text)
                    .NewEmailAddress = Trim(txtNewEmail.Text)
                    .PWord = enc.GetMd5Hash(md5hash, Trim(txtPassword.Text))

                    If Not .CheckEMemberPassword() Then
                        lblInform.Visible = True
                        lblInform.Text = "Wrong User Name or Password."
                        Exit Sub
                    End If

                    If .IsUserNameExists() Then
                        lblInform.Visible = True
                        lblInform.Text = "Your new email is already taken."
                        Exit Sub
                    End If

                    If Not .ChangeUserEmail Then
                        lblInform.Visible = True
                        lblInform.Text = "Change email unsuccessful."
                        Exit Sub
                    Else
                        txtUsername.Text = ""
                        txtNewEmail.Text = ""
                        lblInform.Visible = True
                        lblInform.Text = "Change email successful, you may now use your new email (username) to log in."
                        Exit Sub
                    End If
                End With
            End Using

            lblInform.Visible = False
            lblInform.Text = ""

        Catch ex As Exception

        End Try
    End Sub

    Public Sub eCorporate()
        Try

            Using eCorp = New eCorporateBLL
                With eCorp
                    .Username = Trim(txtUsername.Text)
                    .EmailAddress = Trim(txtNewEmail.Text)
                    .Password = Trim(txtPassword.Text)

                    If Not .Login() Then
                        lblInform.Visible = True
                        lblInform.Text = "Wrong User Name or Password."
                        Exit Sub
                    End If

                    If .CorpUserCheckEmail() Then
                        lblInform.Visible = True
                        lblInform.Text = "Your new email is already taken."
                        Exit Sub
                    End If

                    If Not .ChangeUserEmail Then
                        lblInform.Visible = True
                        lblInform.Text = "Change email unsuccessful."
                        Exit Sub
                    Else
                        txtUsername.Text = ""
                        txtNewEmail.Text = ""
                        lblInform.Visible = True
                        lblInform.Text = "Change email successful, you may now use your new email (username) to log in."
                        Exit Sub
                    End If

                End With
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub eAccount()
        Try

            Using eAcct = New eAccountBLL
                With eAcct
                    .Username = Trim(txtUsername.Text)
                    .EmailAddress = Trim(txtNewEmail.Text)
                    .Password = Trim(txtPassword.Text)

                    If Not .Login() Then
                        lblInform.Visible = True
                        lblInform.Text = "Wrong User Name or Password."
                        Exit Sub
                    End If

                    .Username = Trim(txtNewEmail.Text) 'Use txtNewEmail temporary for checking purpose
                    If .CheckUsername() Then
                        lblInform.Visible = True
                        lblInform.Text = "Your new email is already taken."
                        Exit Sub
                    End If

                    .Username = Trim(txtUsername.Text) 'Assign orignal value to changing email
                    If Not .ChangeUserEmail Then
                        lblInform.Visible = True
                        lblInform.Text = "Change email unsuccessful."
                        Exit Sub
                    Else
                        txtUsername.Text = ""
                        txtNewEmail.Text = ""
                        lblInform.Visible = True
                        lblInform.Text = "Change email successful, you may now use your new email (username) to log in."
                        Exit Sub
                    End If

                End With
            End Using
        Catch ex As Exception

        End Try
    End Sub
End Class