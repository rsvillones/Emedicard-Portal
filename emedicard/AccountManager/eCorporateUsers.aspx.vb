Imports emedicardBLL

Public Class eCorporateUsers
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim ecorp2 As eCorporateBLL
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'txtPassword.Visible = False
            'lblPassword.Visible = False
            If Not Page.IsPostBack Then
                'hdnUserID.Value = 0
                'Dim ecorp As New emedicardBLL.eCorporateBLL()
                'ecorp.AccountCode = Request.QueryString("a")
                'ecorp.GetAdminUser()
                'If ecorp.UserID = 0 Then
                '    ecorp.GetCompanyContactInfo()
                'End If
                'hdnUserID.Value = ecorp.UserID
                'txtFirstname.Text = ecorp.Firstname
                'txtLastname.Text = ecorp.Lastname
                'txtEmailAddress.Text = ecorp.EmailAddress
                'txtDesignation.Text = ecorp.Designation
                'If ecorp.UserID <> 0 Then
                '    lblUsername.Text = " - " & ecorp.Username
                '    txtPassword.Visible = True
                '    lblPassword.Visible = True
                'End If

                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Using ecorp = New eCorporateBLL()
                    ecorp.AccountCode = Request.QueryString("a")
                    dtgResult.DataSource = ecorp.eCorporateUsers
                    dtgResult.DataBind()
                End Using
            End If
        Catch ex As Exception
            'lblMessage.ForeColor = Drawing.Color.Red
            'lblMessage.Text = ex.Message
        End Try
    End Sub

    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
    '    Try
    '        Dim ecorp As New emedicardBLL.eCorporateBLL(EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key), Request.QueryString("a"))

    '        ecorp.GetCorporateInfo(Request.QueryString("a"))
    '        With ecorp

    '            .Designation = txtDesignation.Text
    '            .UserID = hdnUserID.Value
    '            .Firstname = txtFirstname.Text
    '            .Lastname = txtLastname.Text
    '            .EmailAddress = txtEmailAddress.Text
    '            .AccessLevel = 1 ' SET TO ADMIN LEVEL
    '            .Access_APE = 1
    '            .Access_Benefits = 1
    '            .Access_ECU = 1
    '            .Access_Endorsement = 1
    '            .Access_ActionMemos = 1
    '            .Access_ActiveMembers = 1
    '            .Access_ID = 1
    '            .Access_ResignedMembers = 1
    '            .Access_Utilization = 1
    '            .Access_ReimbStatus = 1
    '            .Access_ClinicResults = 1
    '            .RegisteredAccountCode = Request.QueryString("a")
    '            .RegisteredMotherCode = Request.QueryString("a")
    '            If hdnUserID.Value = 0 Then
    '                'ADD USER
    '                .Username = IIf(lblUsername.Text = "", txtEmailAddress.Text, lblUsername.Text)
    '                .Password = EncryptDecrypt.EncryptDecrypt.CreateRandomPassword(6) ' Need to encrypt in BLL                    
    '                .PlainPassword = .Password
    '                If .CheckUsername Then
    '                    lblMessage.Visible = True
    '                    lblMessage.ForeColor = Drawing.Color.Red
    '                    lblMessage.Text = "Username or email address already taken"
    '                    Exit Sub
    '                End If
    '                ._AgentEmail = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
    '                If .AddUser() Then
    '                    lblMessage.Visible = True
    '                    lblMessage.ForeColor = Drawing.Color.Blue
    '                    lblMessage.Text = "New user added. An email was sent to the user."
    '                End If

    '                ecorp2 = New eCorporateBLL()
    '                ecorp2.GetCorporateInfo(Trim(Request.QueryString("a")))
    '                With ecorp2
    '                    .UserID = ecorp.UID
    '                    .AccountCode = Trim(Request.QueryString("a"))
    '                    .Account_Plan = "ALL"
    '                    .Save_CorpUser_Accounts()
    '                End With

    '            Else
    '                ' UPDATE USER
    '                If chkChangeUser.Checked Then
    '                    'Check username existence
    '                    If Not .CheckUserExistence() Then
    '                        lblMessage.Visible = True
    '                        lblMessage.ForeColor = Drawing.Color.Red
    '                        lblMessage.Text = .ErrorMessage
    '                        Exit Sub
    '                    Else
    '                        'Username exists
    '                        .IsUpdateUsername = True
    '                        .Username = txtEmailAddress.Text
    '                    End If
    '                End If

    '                .Password = IIf(txtPassword.Text = "", "", txtPassword.Text)

    '                If .UpdateUserInfo() Then
    '                    lblMessage.Visible = True
    '                    lblMessage.ForeColor = Drawing.Color.Blue
    '                    lblMessage.Text = "User updated."
    '                End If

    '            End If
    '        End With
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)
    '    End Try
    'End Sub
End Class