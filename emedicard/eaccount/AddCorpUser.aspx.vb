Imports emedicardBLL

Public Class AddCorpUser
    Inherits System.Web.UI.Page
    'Dim ecorp As eCorporateBLL
    Dim eaccount As eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then

                eaccount = New eAccountBLL
                With eaccount
                    .Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
                    .GetAgentInfo()

                    hdnCorporateCode.Value = Request.QueryString("c")
                    hdnType.Value = Request.QueryString("t")
                    hdnUsername.Value = Request.QueryString("u")
                    cboCompany.DataSource = eaccount.GetAccountsByUserId(eaccount.UserID)
                    cboCompany.DataValueField = "Accountcode" '"ACCOUNT_CODE"
                    cboCompany.DataTextField = "Accountname" '"ACCOUNT_NAME"
                    cboCompany.DataBind()

                    If Not IsPostBack Then
                        dtgAccounts.DataSource = eaccount.GetAccountsByUserId(eaccount.UserID)
                        dtgAccounts.DataBind()
                        dtgAccounts.Columns(4).Visible = False

                        dtgPlans.DataSource = eaccount.GetPlanToUtilize(dtgAccounts.SelectedValue(0).ToString)
                        dtgPlans.DataBind()
                        dtgPlans.Columns(0).Visible = False

                    End If

                End With

            End If

            If Not Request.QueryString("m") Is Nothing Then
                lblMessage.Visible = True
                lblMessage.Text = HttpUtility.UrlDecode(Request.QueryString("m")).ToString
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn.Click
        Response.Redirect("UserManagementHR.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")))
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Try
            Dim encTool As New emedBLL

            Dim sPlan As String = String.Empty
            Dim bAllPlan As Boolean = True
            Dim bNoSelection = True

            'For x As Integer = 0 To dtgPlans.Rows.Count - 1
            '    Dim cb As CheckBox = DirectCast(dtgPlans.Rows(x).Cells(1).FindControl("chkPlanSelect"), CheckBox)
            '    If cb IsNot Nothing Then
            '        If cb.Checked Then
            '            If sPlan.Trim.Length = 0 Then
            '                sPlan = dtgPlans.Rows(x).Cells(0).Text.Trim
            '            Else
            '                sPlan = sPlan & ", " & dtgPlans.Rows(x).Cells(0).Text.Trim
            '            End If
            '            bNoSelection = False
            '        Else
            '            bAllPlan = False
            '        End If
            '    End If
            'Next

            'bAllPlan = True
            'If bNoSelection Then
            '    lblMessage.Visible = True
            '    lblMessage.CssClass = "alert alert-warning"
            '    lblMessage.Text = "Please select plans to utilize."
            '    Exit Sub
            'End If

            Using ecorp = New eCorporateBLL()

                '1. Check if the username exists
                ecorp.Username = txtUsername.Text
                If ecorp.CheckUsername() Then
                    lblMessage.Visible = True
                    lblMessage.CssClass = "alert alert-warning"
                    lblMessage.Text = "Username/email already exist. "
                    Exit Sub
                End If
                '2. Fill the properties
                ecorp.Access_ActionMemos = IIf(chkActionMemos.Checked = True, "1", 0)
                ecorp.Access_ActiveMembers = IIf(chkActive.Checked = True, "1", 0)
                'ecorp.Access_APE = IIf(chkAPE.Checked = True, "1", 0)
                ecorp.Access_Benefits = IIf(chkBenefits.Checked = True, "1", 0)
                'ecorp.Access_ECU = IIf(chkECU.Checked = True, "1", 0)
                ecorp.Access_Endorsement = IIf(chkEndorsement.Checked = True, "1", 0)
                ecorp.Access_ResignedMembers = IIf(chkResigned.Checked = True, "1", 0)
                ecorp.Access_Utilization = IIf(chkUtil.Checked = True, "1", 0)
                'ecorp.Access_ID = IIf(chkRequest.Checked, "1", 0)
                ecorp.Access_ReimbStatus = IIf(chkReimbStatus.Checked, "1", 0)
                ecorp.Access_ClinicResults = IIf(chkClinicResults.Checked, "1", 0)
                ecorp.AccessLevel = 2
                ecorp.AccountCode = cboCompany.SelectedValue
                ecorp.Address = txtCompanyAddress.Text
                ecorp.CompanyName = cboCompany.SelectedItem.Text
                ecorp.Designation = txtDesignation.Text
                ecorp.EmailAddress = txtUsername.Text
                ecorp.Fax = txtFaxNo.Text
                ecorp.Firstname = txtFirstname.Text
                ecorp.Lastname = txtLastname.Text
                ecorp.Mobile = txtMobile.Text
                ecorp.PlainPassword = EncryptDecrypt.EncryptDecrypt.CreateRandomPassword(ecorp.PasswordLength)
                ecorp.Password = ecorp.PlainPassword
                ecorp.Phone = txtTelephone.Text

                eaccount = New eAccountBLL
                eaccount.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
                eaccount.GetAgentInfo()
                ecorp.MainAgentID = eaccount.UserID

                ecorp.RegisteredAccountCode = cboCompany.SelectedValue
                ecorp.RegisteredMotherCode = hdnCorporateCode.Value
                Dim result As Integer = ecorp.AddUser()

                If result > 0 Then

                    sPlan = "ALL"

                    For i As Integer = 0 To dtgAccounts.Rows.Count - 1
                        Dim cb As CheckBox = DirectCast(dtgAccounts.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                        If cb IsNot Nothing Then
                            If cb.Checked Then
                                With ecorp
                                    .UserID = result
                                    .AccountCode = Trim(dtgAccounts.Rows(i).Cells(2).Text)
                                    .CompanyName = Trim(dtgAccounts.Rows(i).Cells(1).Text)
                                    .Account_Category = Trim(dtgAccounts.Rows(i).Cells(3).Text)
                                    .Mother_Code = IIf(UCase(Trim(dtgAccounts.Rows(i).Cells(3).Text)) = "MOTHER", "", Trim(dtgAccounts.Rows(i).Cells(4).Text))
                                    .Account_Plan = sPlan
                                    .Save_CorpUser_Accounts()
                                End With

                            End If
                        End If
                    Next

                    Dim SaveMsg As String = String.Empty
                    SaveMsg = "New user added. An email was sent to user for login credentials."
                    Response.Redirect("AddCorpUser.aspx?t=" & Request.QueryString("t") & "&c=" & Request.QueryString("c") & "&u=" & Request.QueryString("u") & "&m=" & HttpUtility.UrlEncode(SaveMsg))

                End If
            End Using

        Catch ex As Exception
            lblMessage.Visible = True
            lblMessage.CssClass = "alert alert-warning"
            lblMessage.Text = "Unexpected Error. " & ex.Message
        End Try
    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkALL.CheckedChanged
        For x As Integer = 0 To dtgPlans.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgPlans.Rows(x).Cells(1).FindControl("chkPlanSelect"), CheckBox)
            If cb IsNot Nothing Then
                cb.Checked = chkALL.Checked
            End If
        Next
    End Sub
End Class