Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Imports System.Security.Cryptography
Public Class AddAgentUser
    Inherits System.Web.UI.Page
    Dim oBLL As eAccountBLL
    Dim oEAcctBLL As New eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim enc As EncryptDecrypt.EncryptDecrypt
    Dim dtAccounts As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Load_User_Accounts()

            If Not Request.QueryString("m") Is Nothing Then
                lblMessage.Visible = True
                lblMessage.Text = HttpUtility.UrlDecode(Request.QueryString("m")).ToString
            End If

        End If
    End Sub

    Private Sub Load_User_Accounts()
        oEAcctBLL.AgentCode = Request.QueryString("agnt")
        dtAccounts = oEAcctBLL.GetUsersActiveAccounts()
        dtgAcctList.DataSource = dtAccounts
        dtgAcctList.DataBind()
        dtgAcctList.Columns(4).Visible = False
    End Sub

    Private Sub Load_AgentInfo()
        With oEAcctBLL
            .Username = Decrypt(Request.QueryString("u"), key)
            .GetAgentInfo()

        End With

    End Sub

    Function Save_AgentUser() As Boolean
        Dim iUID As Integer = 0
        Dim md5Hash As MD5 = MD5.Create
        enc = New EncryptDecrypt.EncryptDecrypt
        oBLL = New eAccountBLL
        Dim bSuccess As Boolean = True
        Try


            With oBLL
                .Username = Trim(txtEmail.Text)
                .EmailAddress = Trim(txtEmail.Text)

                If oBLL.CheckUsername Then
                    Return False
                    Exit Function
                End If
                .Firstname = txtFname.Text
                .Lastname = txtLname.Text
                .Username = txtEmail.Text

                .Password = enc.GetMd5Hash(md5Hash, txtPassword.Text)
                '.Access_APE = IIf(chkAPE.Checked, 1, 0)
                .Access_Utilization = IIf(chkUtil.Checked, 1, 0)
                .Access_Endorsement = IIf(chkEndorse.Checked, 1, 0)
                .Access_Benefits = IIf(chkBenefits.Checked, 1, 0)
                '.Access_ID = IIf(chkID.Checked, 1, 0)
                '.Access_ECU = IIf(chkECU.Checked, 1, 0)
                .Access_ActiveMembers = IIf(chkActiveMem.Checked, 1, 0)
                .Access_ResignedMembers = IIf(chkResgnMem.Checked, 1, 0)
                .Access_ActionMemos = IIf(chkActMem.Checked, 1, 0)
                .Access_ReimbStatus = IIf(chkReimbStatus.Checked, 1, 0)
                .Access_ClinicResults = IIf(chkClinicResults.Checked, 1, 0)
                .MainAgentID = oEAcctBLL.UserID
                .AgentCode = oEAcctBLL.AgentCode

                iUID = .SaveAgentUserInfo()

                For i As Integer = 0 To dtgAcctList.Rows.Count - 1
                    Dim cb As CheckBox = DirectCast(dtgAcctList.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                    If cb IsNot Nothing Then
                        If cb.Checked Then
                            With oBLL
                                .UserID = iUID
                                .AccountCode = Trim(dtgAcctList.Rows(i).Cells(2).Text)
                                .CompanyName = Trim(dtgAcctList.Rows(i).Cells(1).Text)
                                .Account_Category = Trim(dtgAcctList.Rows(i).Cells(3).Text)
                                .Mother_Code = IIf(UCase(Trim(dtgAcctList.Rows(i).Cells(3).Text)) = "MOTHER", "", Trim(dtgAcctList.Rows(i).Cells(4).Text))
                                .Save_User_Accounts()
                            End With

                        End If
                    End If
                Next
            End With
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Load_AgentInfo()
        If Save_AgentUser() Then
            lblMessage.Visible = True
            lblMessage.Text = "User information successfully updated."
            Dim msg As String = "User information successfully updated."
            ClearFields()
            Response.Redirect("AddAgentUser.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")) & "&agnt=" & Request.QueryString("agnt") & "&m=" & HttpUtility.UrlEncode(msg))
        Else
            lblIform.Visible = True
            lblIform.Text = "Email is already taken."
        End If

    End Sub

    Private Sub ClearFields()
        oEAcctBLL = Nothing
        txtFname.Text = ""
        txtLname.Text = ""
        txtEmail.Text = ""
        txtPassword.Text = ""
        txtConfirm.Text = ""

        'chkAPE.Checked = False
        chkUtil.Checked = False
        chkEndorse.Checked = False
        chkBenefits.Checked = False
        'chkID.Checked = False
        'chkECU.Checked = False
        chkActiveMem.Checked = False
        chkResgnMem.Checked = False
        chkActMem.Checked = False
        chkReimbStatus.Checked = False
        chkClinicResults.Checked = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Response.Redirect("UserManagement.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")) & "&agnt=" & Request.QueryString("agnt"))
    End Sub
End Class