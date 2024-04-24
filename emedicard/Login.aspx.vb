Imports EncryptDecrypt
Imports emedicardBLL
Public Class Login
    Inherits System.Web.UI.Page
    Dim bll As New ememberBLL
    Dim eCorp As New eCorporateBLL
    Dim eAccnt As New eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim acctCode As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not IsNothing(Request.QueryString("ReturnUrl")) Then
                Response.Redirect("Login.aspx?")
            End If
        End If
        lblYear.Text = Today.Year.ToString

    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try
            lblAlert.Text = ""
            CustomValidator1.Validate()
        Catch ex As Exception
            'CustomValidator1.ErrorMessage = ex.Message
        End Try
    End Sub

    Protected Sub CustomValidator1_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustomValidator1.ServerValidate
        Try
            Dim u As String = LCase(txtUsername.Text)
            Dim p As String = txtPassword.Text
            Dim strRedirectPath As String = "~/emember/"

            'try only
            'Mailhelper.MailHelper.SendMailMessage("", "rsvillones@medicardphils.com", "", "", "sample 04142021", "sample only via portal 04142021")
            'end try

            If rdEmember.Checked Then
                'e-Member Login
                bll = New ememberBLL(u, p, ememberBLL.AccountType.emember)
                If (Not bll.CheckUsername()) OrElse (Not bll.CheckUserAccount()) OrElse (Not bll.GetMemberInformation(u)) Then
                    lblAlert.Text = bll.LoginMessage
                    args.IsValid = False
                    Exit Sub
                End If

                If bll.AccountStatus.Contains("RESIGNED") Then
                    lblAlert.Text = "Membershisp status is resigned already."
                    args.IsValid = False
                    Exit Sub
                End If

                If Not Request.QueryString("ReturnUrl") Is Nothing Then
                    If Request.QueryString("ReturnUrl").Contains("Default.aspx") Then
                        strRedirectPath = "~" & Request.QueryString("ReturnUrl")
                    Else

                        strRedirectPath = "~/emember/?u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))
                    End If

                Else

                    strRedirectPath = "~/emember/?u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))
                End If


            ElseIf rdCorporate.Checked Then 'ECORPORATE LOGIN

                Using ecorp = New eCorporateBLL(u, p)
                    acctCode = ecorp.AccountCode
                    If Not ecorp.CheckUsername() OrElse Not ecorp.CheckUserPassword() OrElse Not ecorp.Login() Then
                        lblAlert.Text = ecorp.ErrorMessage
                        args.IsValid = False
                        Exit Sub
                    End If
                End Using

                If Not Request.QueryString("ReturnUrl") Is Nothing Then
                    If acctCode = "11082010-030043" Then
                        strRedirectPath = "~/DocUpload.aspx?t=1&c=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(acctCode, key)) & "&u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))
                    Else

                    End If
                    If Request.QueryString("ReturnUrl").Contains("Default.aspx") Then

                        strRedirectPath = "~" & Request.QueryString("ReturnUrl") & "t=1&c=" & acctCode & "&u=" & EncryptDecrypt.EncryptDecrypt.Encrypt(u, key)
                    Else

                        strRedirectPath = "~/Default.aspx?t=1&c=" & acctCode & "&u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))
                    End If

                Else
                    If acctCode = "11082010-030043" Then
                        strRedirectPath = "~/DocUpload.aspx?t=1&c=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(acctCode, key)) & "&u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))
                    Else
                        strRedirectPath = "~/ecorporate/?t=1&c=" & acctCode & "&u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))
                    End If
                End If
            ElseIf rdAgent.Checked Then
                Dim sAgentCode As String = String.Empty
                Using eAccnt = New eAccountBLL(u, p)
                    acctCode = eAccnt.AccountCode
                    sAgentCode = eAccnt.AgentCode
                    If Not eAccnt.CheckUsername() OrElse Not eAccnt.CheckUserPassword() OrElse Not eAccnt.Login() Then
                        lblAlert.Text = eAccnt.ErrorMessage
                        args.IsValid = False
                        Exit Sub
                    End If
                End Using

                Dim uemail As String = HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(u, key))

                If Not Request.QueryString("ReturnUrl") Is Nothing Then

                    If Request.QueryString("ReturnUrl").Contains("Default.aspx") Then
                        strRedirectPath = "~" & Request.QueryString("ReturnUrl") & "t=2&agnt=" & sAgentCode & "&c=" & acctCode & "&u=" & uemail
                    Else
                        strRedirectPath = "~/Default.aspx?t=2&agnt=" & sAgentCode & "&c=" & acctCode & "&u=" & uemail
                    End If

                Else
                    strRedirectPath = "~/eaccount/Default.aspx?t=2&agnt=" & sAgentCode & "&c=" & acctCode & "&u=" & uemail
                End If

            End If

            FormsAuthentication.RedirectFromLoginPage(u, False)
            Response.Redirect(strRedirectPath)

        Catch ex As Exception

            lblAlert.Text = ex.Message.ToString
            'Throw New Exception(ex.InnerException.ToString)
        End Try


    End Sub

    Protected Sub rdCorporate_CheckedChanged(sender As Object, e As EventArgs) Handles rdCorporate.CheckedChanged
        ' If rdCorporate.Checked Then Response.Redirect("https://secure.medicardphils.com/content/e-medicard/corporate/")


    End Sub

    Protected Sub rdAgent_CheckedChanged(sender As Object, e As EventArgs) Handles rdAgent.CheckedChanged
        ' If rdAgent.Checked Then Response.Redirect("https://secure.medicardphils.com/content/e-medicard/account/")
    End Sub

 
End Class