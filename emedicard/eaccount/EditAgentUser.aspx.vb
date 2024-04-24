Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Imports System.Security.Cryptography

Public Class EditAgentUser
    Inherits System.Web.UI.Page
    Dim oBLL As eAccountBLL
    Dim oEAcctBLL As eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim enc As EncryptDecrypt.EncryptDecrypt
    Dim dtAccounts As DataTable
    Dim dtUserAccounts As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Load_AgentUser()
            'Load_AgentInfo()
            Load_UserAccounts()
            Load_Available_Accounts()
        End If
    End Sub

    Private Sub Load_AgentUser()
        oBLL = New eAccountBLL
        With oBLL
            .UserID = Request.QueryString("j")
            .GetAgentInfoByID()

            txtFirstname.Text = .Firstname
            txtLastname.Text = .Lastname
            txtUsername.Text = .Username
            txtEmailAddress.Text = .EmailAddress
            chkMemStatus.Checked = .Active
            'chkAPE.Checked = .Access_APE
            chkUtil.Checked = .Access_Utilization
            chkEndorse.Checked = .Access_Endorsement
            chkBenefits.Checked = .Access_Benefits
            'chkID.Checked = .Access_ID
            'chkECU.Checked = .Access_ECU
            chkActiveMem.Checked = .Access_ActiveMembers
            chkResgnMem.Checked = .Access_ResignedMembers
            chkActMem.Checked = .Access_ActionMemos
            chkReimbSatus.Checked = IIf(IsDBNull(.Access_ReimbStatus), False, .Access_ReimbStatus)
            chkClinicResults.Checked = IIf(IsDBNull(.Access_ClinicResults), False, .Access_ClinicResults)

        End With

    End Sub
    'Private Sub Load_AgentInfo()
    '    oEAcctBLL = New eAccountBLL
    '    With oEAcctBLL
    '        .Username = Decrypt(Request.QueryString("u"), key)
    '        .GetAgentInfo()
    '    End With

    'End Sub

    Private Sub Save_AgentUser()
        With oBLL
            .Firstname = txtFirstname.Text
            .Lastname = txtLastname.Text
            .Username = txtUsername.Text
            If Len(Trim(txtPassword.Text)) > 3 Then
                Dim md5Hash As MD5 = MD5.Create
                enc = New EncryptDecrypt.EncryptDecrypt
                .Password = enc.GetMd5Hash(md5Hash, txtPassword.Text)
            End If
            .EmailAddress = txtEmailAddress.Text
            .Active = IIf(chkMemStatus.Checked, 1, 0)
            '.Access_APE = IIf(chkAPE.Checked, 1, 0)
            .Access_Utilization = IIf(chkUtil.Checked, 1, 0)
            .Access_Endorsement = IIf(chkEndorse.Checked, 1, 0)
            .Access_Benefits = IIf(chkBenefits.Checked, 1, 0)
            '.Access_ID = IIf(chkID.Checked, 1, 0)
            '.Access_ECU = IIf(chkECU.Checked, 1, 0)
            .Access_ActiveMembers = IIf(chkActiveMem.Checked, 1, 0)
            .Access_ResignedMembers = IIf(chkResgnMem.Checked, 1, 0)
            .Access_ActionMemos = IIf(chkActMem.Checked, 1, 0)
            .Access_ReimbStatus = IIf(chkReimbSatus.Checked, 1, 0)
            .Access_ClinicResults = IIf(chkClinicResults.Checked, 1, 0)
            .SaveAgentUserInfo()
        End With
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        If txtPassword.Text.Trim <> "" And txtPassword.Text.Trim.Length < 6 Then
            CustomValidator1.Visible = True
            CustomValidator1.IsValid = False
            CustomValidator1.ErrorMessage = "Password must be atleast 6 alphanumeric characters."
            Exit Sub
        End If

        If txtConfirm.Text.Trim <> txtPassword.Text.Trim And txtPassword.Text.Trim <> "" Then
            CustomValidator2.Visible = True
            CustomValidator2.IsValid = False
            CustomValidator2.ErrorMessage = "Password doesn't match."
            Exit Sub
        End If

        If txtConfirm.Text.Trim <> "" And txtPassword.Text.Trim = "" Then
            CustomValidator2.Visible = True
            CustomValidator2.IsValid = False
            CustomValidator2.ErrorMessage = "Password doesn't match."
            Exit Sub
        End If

        oBLL = New eAccountBLL
        With oBLL
            .UserID = Request.QueryString("j")
            .GetAgentInfoByID()
        End With
        Save_AgentUser()
        lblMessage.Visible = True
        lblMessage.Text = "User information successfully updated."
    End Sub

    Protected Sub btn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn.Click
        Response.Redirect("UserManagement.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")) & "&agnt=" & Request.QueryString("agnt"))
    End Sub

#Region "User Assigned Accounts"

    Private Sub Load_UserAccounts()
        dtUserAccounts = New DataTable
        dtUserAccounts = oBLL.GerEMEDUsersAccount

        dtgUserAcctList.DataSource = dtUserAccounts
        dtgUserAcctList.DataBind()
        'dtgUserAcctList.Columns(0).Visible = False
    End Sub

    Protected Sub dtgUserAcctList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dtgUserAcctList.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim b As Button = DirectCast(e.Row.FindControl("ButtonDelete"), Button)
            b.Attributes.Add("onclick", "return ConfirmOnDelete();")
        End If
    End Sub

    Private Sub DeleteRecords(ByVal sc As StringCollection)
        For Each item As String In sc
            oBLL.Delete_UserAccounts(item)
        Next
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Load_AgentUser()
        Dim sc As New StringCollection()
        Dim id As String = String.Empty
        For i As Integer = 0 To dtgUserAcctList.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgUserAcctList.Rows(i).Cells(4).FindControl("chkSelect"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    id = dtgUserAcctList.Rows(i).Cells(0).Text
                    sc.Add(id)
                End If
            End If
        Next
        DeleteRecords(sc)
        Load_UserAccounts()
        Load_Available_Accounts()
    End Sub
#End Region

#Region "Available Accounts"
    Public Sub Load_Available_Accounts()

        oBLL.AgentCode = Request.QueryString("agnt")
        oBLL.UserID = Request.QueryString("j")
        dtAccounts = New DataTable
        dtAccounts = oBLL.GetUserAvailableAccounts()
        dtgAcctList.DataSource = dtAccounts
        dtgAcctList.DataBind()
        'dtgAcctList.Columns(4).Visible = False

    End Sub
#End Region

    Protected Sub btnAddAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAccount.Click
        oBLL = New eAccountBLL
        For i As Integer = 0 To dtgAcctList.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgAcctList.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    With oBLL
                        .UserID = Request.QueryString("j")
                        .AccountCode = Trim(dtgAcctList.Rows(i).Cells(2).Text)
                        .CompanyName = Trim(dtgAcctList.Rows(i).Cells(1).Text)
                        .Account_Category = Trim(dtgAcctList.Rows(i).Cells(3).Text)
                        .Mother_Code = IIf(UCase(Trim(dtgAcctList.Rows(i).Cells(3).Text)) = "MOTHER", "", Trim(dtgAcctList.Rows(i).Cells(4).Text))
                        .Save_User_Accounts()
                    End With

                End If
            End If
        Next
        Load_AgentUser()
        Load_UserAccounts()

    End Sub
End Class