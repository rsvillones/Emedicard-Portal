Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Imports System.Security.Cryptography

Public Class EditProfile
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Public oEAcctBLL As eAccountBLL
    Public Shared objAcctBll As eAccountBLL
    Public Shared sfname As String
    Public Shared slname As String
    Public Shared susername As String
    Public Shared spassword As String
    Public Shared semail As String
    Public Shared objEnc As New EncryptDecrypt.EncryptDecrypt
    Public Shared iuserid As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Load_AgentInfo()
        End If
    End Sub
    Private Sub Load_AgentInfo()
        oEAcctBLL = New eAccountBLL
        With oEAcctBLL
            .Username = Decrypt(Request.QueryString("u"), key)
            .GetAgentInfo()
            txtFname.Text = .Firstname
            txtLname.Text = .Lastname
            txtUsername.Text = .Username
            txtEmail.Text = .EmailAddress
            iuserid = .UserID
        End With

    End Sub

    Private Sub AssingValues()
        sfname = txtFname.Text
        slname = txtLname.Text
        susername = txtUsername.Text
        spassword = txtPassword.Text
        semail = txtEmail.Text
    End Sub
    Public Shared Sub SaveInfo()
        objAcctBll = New eAccountBLL
        With objAcctBll
            Dim md5Hash As MD5 = MD5.Create
            .UserID = iuserid
            .Firstname = sfname
            .Lastname = slname
            .Username = susername
            If Trim(spassword) <> "" Then
                .Password = objEnc.GetMd5Hash(md5Hash, spassword)
            End If
            .EmailAddress = semail
        End With
        objAcctBll.SaveAgentInfo()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Response.Redirect("Default.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")) & "&agnt=" & Request.QueryString("agnt"))
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click


        If txtPassword.Text.Trim.Length < 6 And txtPassword.Text.Trim <> "" Then
            CustomValidator1.Visible = True
            CustomValidator1.IsValid = False
            CustomValidator1.ErrorMessage = "Password must be atleast 6 alphanumeric characters."
            Exit Sub
        End If

        If txtConfirm.Text.Trim = "" And txtPassword.Text.Trim.Length > 0 Then
            CustomValidator2.Visible = True
            CustomValidator2.IsValid = False
            CustomValidator2.ErrorMessage = "Confirm password is required."
            Exit Sub
        End If

        AssingValues()
        SaveInfo()
        'savemsg.InnerHtml = "<div class='alert alert-block ' <p>Changes has been save.</p></div>"
        lblMessage.Visible = True
        lblMessage.Text = "User information successfully updated."

        CustomValidator1.Visible = False
        CustomValidator1.IsValid = True
        CustomValidator1.ErrorMessage = ""

        CustomValidator2.Visible = False
        CustomValidator2.IsValid = True
        CustomValidator2.ErrorMessage = ""
    End Sub
End Class