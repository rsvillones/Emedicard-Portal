Imports emedicardBLL

Public Class EditCorpUser
    Inherits System.Web.UI.Page
    Dim ecorp As New eCorporateBLL
    Dim eaccount As New eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim sUserAccountCode As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            lblMessage.Visible = False

            ecorp = New eCorporateBLL(Nothing, Nothing, Nothing, Request.QueryString("uid"))
            eaccount = New eAccountBLL
            eaccount.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            eaccount.GetAgentInfo()

            If Not Page.IsPostBack Then
                cboCompany.DataSource = eaccount.GetAccountsByUserId(eaccount.UserID)
                cboCompany.DataValueField = "Accountcode"
                cboCompany.DataTextField = "Accountname"
                cboCompany.DataBind()
                cboCompany.SelectedValue = ecorp.AccountCode

                txtDesignation.Text = ecorp.Designation
                txtEmailAddress.Text = ecorp.EmailAddress
                txtFaxNo.Text = ecorp.Fax
                txtFirstname.Text = ecorp.Firstname
                txtLastname.Text = ecorp.Lastname
                txtMobile.Text = ecorp.Mobile
                txtTelephone.Text = ecorp.Phone
                lblUsername.Text = ecorp.Firstname & " " & ecorp.Lastname

                hdnCorporateCode.Value = ecorp.AccountCode
                hdnUsername.Value = ecorp.Username
                hdnType.Value = "1"
                hdnUserID.Value = ecorp.UserID
                hdnAccessLevel.Value = ecorp.AccessLevel
                hdnAgentUname.Value = eaccount.Username

                chkActionMemos.Checked = ecorp.Access_ActionMemos
                chkActive.Checked = ecorp.Access_ActiveMembers
                'chkAPE.Checked = ecorp.Access_APE
                chkBenefits.Checked = ecorp.Access_Benefits
                'chkECU.Checked = ecorp.Access_ECU
                chkEndorsement.Checked = ecorp.Access_Endorsement
                'chkRequest.Checked = ecorp.Access_ID
                chkResigned.Checked = ecorp.Access_ResignedMembers
                chkUtil.Checked = ecorp.Access_Utilization
                chkReimbStatus.Checked = ecorp.Access_ReimbStatus
                chkClinicResults.Checked = ecorp.Access_ClinicResults

                'dtgAccounts.DataSource = eaccount.GetAccountsByUserId(eaccount.UserID)
                'dtgAccounts.DataBind()

                'dtgPlans.DataSource = eaccount.GetPlanToUtilize(ecorp.AccountCode)
                'dtgPlans.Columns(0).Visible = True
                'dtgPlans.DataBind()
                'dtgPlans.Columns(0).Visible = False

                Load_UserAccounts()
                Load_Available_Accounts()
                Load_Plans(ecorp.AccountCode)

            End If

            'lblMessage.Visible = False
            'sUserAccountCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("a"), key)
            'If Not Request.QueryString("c") Is Nothing Then
            '    ecorp = New eCorporateBLL(Request.QueryString("u"), Nothing, Request.QueryString("c"), EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("uid"), key))

            '    Dim ecorpMain As New eCorporateBLL
            '    ecorpMain = New eCorporateBLL(Request.QueryString("u"), Nothing, Request.QueryString("c"))
            '    ecorpMain.AccountCode = Request.QueryString("c")
            '    ecorpMain.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            '    ecorpMain.GetUserID()

            '    If Not Page.IsPostBack Then

            '        'cboCompany.DataSource = ecorp.AgentCompany
            '        'cboCompany.DataValueField = "ACCOUNT_CODE"
            '        'cboCompany.DataTextField = "ACCOUNT_NAME"
            '        cboCompany.DataSource = ecorpMain.GetCorporateUserAccount()
            '        cboCompany.DataValueField = "Accountcode"
            '        cboCompany.DataTextField = "Accountname"
            '        cboCompany.DataBind()
            '        cboCompany.SelectedValue = sUserAccountCode

            '        txtDesignation.Text = ecorp.Designation
            '        txtEmailAddress.Text = ecorp.EmailAddress
            '        txtFaxNo.Text = ecorp.Fax
            '        txtFirstname.Text = ecorp.Firstname
            '        txtLastname.Text = ecorp.Lastname
            '        txtMobile.Text = ecorp.Mobile
            '        txtTelephone.Text = ecorp.Phone
            '        lblUsername.Text = ecorp.Firstname & " " & ecorp.Lastname

            '        hdnCorporateCode.Value = Request.QueryString("c")
            '        hdnUsername.Value = Request.QueryString("u")
            '        hdnType.Value = "1"
            '        hdnUserID.Value = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("uid"), key)
            '        hdnAccessLevel.Value = ecorp.AccessLevel


            '        chkActionMemos.Checked = ecorp.Access_ActionMemos
            '        chkActive.Checked = ecorp.Access_ActiveMembers
            '        'chkAPE.Checked = ecorp.Access_APE
            '        chkBenefits.Checked = ecorp.Access_Benefits
            '        'chkECU.Checked = ecorp.Access_ECU
            '        chkEndorsement.Checked = ecorp.Access_Endorsement
            '        'chkRequest.Checked = ecorp.Access_ID
            '        chkResigned.Checked = ecorp.Access_ResignedMembers
            '        chkUtil.Checked = ecorp.Access_Utilization
            '        chkReimbStatus.Checked = ecorp.Access_ReimbStatus
            '        chkClinicResults.Checked = ecorp.Access_ClinicResults

            '        Load_UserAccounts()
            '        Load_Available_Accounts()
            '        Load_Plans()
            '    End If

            'End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub Load_UserAccounts()
        dtgUserAcct.DataSource = ecorp.GetCorporateUserAccount()
        dtgUserAcct.DataBind()
    End Sub

    Private Sub Load_Available_Accounts()
        'Dim ecorpMain As New eCorporateBLL
        'ecorpMain = New eCorporateBLL(Request.QueryString("u"), Nothing, Request.QueryString("c"))
        'ecorpMain.AccountCode = Request.QueryString("c")
        'ecorpMain.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
        'ecorpMain.GetUserID()
        'dtgAccounts.DataSource = ecorpMain.GetCorporateUserAvailabeAccounts() 'ecorp.GetCorporateUserAvailabeAccounts()
        'dtgAccounts.DataBind()

        eaccount = New eAccountBLL
        eaccount.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
        eaccount.GetAgentInfo()
        dtgAccounts.DataSource = eaccount.GetAccountsByUserId(eaccount.UserID)
        dtgAccounts.DataBind()

    End Sub
    Private Sub Load_Plans(ByVal sAcctCode As String)

        dtgPlans.DataSource = eaccount.GetPlanToUtilize(sAcctCode)
        dtgPlans.Columns(0).Visible = True
        dtgPlans.DataBind()
        dtgPlans.Columns(0).Visible = False
    End Sub
    Protected Sub btn_Click(sender As Object, e As EventArgs) Handles btn.Click
        Response.Redirect("UserManagementHR.aspx?t=2" & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(hdnAgentUname.Value, key)))
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try

            Using eCorp = New eCorporateBLL(EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key), Nothing, Nothing, Request.QueryString("uid"))
                With eCorp
                    .EmailAddress = txtEmailAddress.Text
                    If Len(Trim(Trim(txtEmailAddress.Text))) > 0 Then
                        If Trim(eCorp.EmailAddress) <> Trim(txtEmailAddress.Text) Then
                            If eCorp.CorpUserCheckEmail Then
                                lblMessage.Visible = True
                                lblMessage.Text = "Email is already taken."
                                Exit Sub
                            End If
                        End If
                    End If

                    .Username = Trim(txtEmailAddress.Text)

                    .Access_ActionMemos = IIf(chkActionMemos.Checked = True, "1", 0)
                    .Access_ActiveMembers = IIf(chkActive.Checked = True, "1", 0)
                    '.Access_APE = IIf(chkAPE.Checked = True, "1", 0)
                    .Access_Benefits = IIf(chkBenefits.Checked = True, "1", 0)
                    '.Access_ECU = IIf(chkECU.Checked = True, "1", 0)
                    .Access_Endorsement = IIf(chkEndorsement.Checked = True, "1", 0)
                    .Access_ResignedMembers = IIf(chkResigned.Checked = True, "1", 0)
                    .Access_Utilization = IIf(chkUtil.Checked = True, "1", 0)
                    '.Access_ID = IIf(chkRequest.Checked, "1", 0)
                    .Access_ReimbStatus = IIf(chkReimbStatus.Checked, "1", 0)
                    .Access_ClinicResults = IIf(chkClinicResults.Checked, "1", 0)
                    .AccessLevel = hdnAccessLevel.Value
                    .AccountCode = cboCompany.SelectedValue
                    .CompanyName = cboCompany.SelectedItem.Text
                    .Designation = txtDesignation.Text

                    .Fax = txtFaxNo.Text
                    .Firstname = txtFirstname.Text
                    .Lastname = txtLastname.Text
                    .Mobile = txtMobile.Text
                    .Phone = txtTelephone.Text
                    .UserID = hdnUserID.Value

                    .RegisteredMotherCode = hdnCorporateCode.Value
                    .RegisteredAccountCode = cboCompany.SelectedValue
                    If .UpdateUserInfo() Then
                        lblMessage.Visible = True
                        lblMessage.Text = "User information successfully updated."
                    End If

                End With
            End Using
        Catch ex As Exception
            lblMessage.Visible = True
            lblMessage.Text = ex.Message
        End Try
    End Sub

    Protected Sub dtgUserAcct_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dtgUserAcct.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim b As Button = DirectCast(e.Row.FindControl("ButtonDelete"), Button)
            b.Attributes.Add("onclick", "return ConfirmOnDelete();")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim sHost As String
            If InStr(1, HttpContext.Current.Request.Url.ToString, "webportal") Then
                If Request.Url.Port = 80 Then
                    sHost = Request.Url.Scheme & "://" & Request.Url.Host & "/eMedicard/"
                Else
                    sHost = Request.Url.Scheme & "://" & Request.Url.Host & ":" & Request.Url.Port & "/eMedicard/"
                End If
            Else
                sHost = Request.Url.Scheme & "://" & Request.Url.Host & ":" & Request.Url.Port & "/"
            End If
            Dim a As HyperLink = DirectCast(e.Row.FindControl("lPlan"), HyperLink)

            a.NavigateUrl = "ecorporate/EditPlans.aspx?pi=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(0).Text, key)) & "&c=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(2).Text, key))
            a.Attributes.Add("onclick", "return NewWindow('" & ConfigurationManager.AppSettings("BaseUrl") & "/" & a.NavigateUrl & "', 'Plans to utilize',600, 600);")
            'If InStr(1, HttpContext.Current.Request.Url.ToString, "webportal") Then
            '    a.Attributes.Add("onclick", "return NewWindow('" & sHost & a.NavigateUrl & "', 'Plans to utilize',600, 600);")
            'Else
            '    a.Attributes.Add("onclick", "return NewWindow('" & sHost & "ecorporate/" & a.NavigateUrl & "', 'Plans to utilize',600, 600);")
            'End If
        End If
    End Sub

    Private Sub DeleteRecords(ByVal sc As StringCollection)
        For Each item As String In sc
            ecorp.Delete_EcorpUserAccounts(item)
        Next
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim sc As New StringCollection()
        Dim id As String = String.Empty
        For i As Integer = 0 To dtgUserAcct.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgUserAcct.Rows(i).Cells(4).FindControl("chkSelect"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    id = dtgUserAcct.Rows(i).Cells(0).Text
                    sc.Add(id)
                End If
            End If
        Next
        DeleteRecords(sc)
        Load_UserAccounts()
        Load_Available_Accounts()
        Load_Plans("")
    End Sub


    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Try
            Dim sPlan As String = String.Empty
            Dim isDataSave As Boolean = False
            Dim bAllPlan As Boolean = True
            Dim bNoSelection As Boolean = True

            For x As Integer = 0 To dtgPlans.Rows.Count - 1
                Dim cb As CheckBox = DirectCast(dtgPlans.Rows(x).Cells(1).FindControl("chkPlanSelect"), CheckBox)
                If cb IsNot Nothing Then
                    If cb.Checked Then
                        If sPlan.Trim.Length = 0 Then
                            sPlan = dtgPlans.Rows(x).Cells(0).Text.Trim
                        Else
                            sPlan = sPlan & ", " & dtgPlans.Rows(x).Cells(0).Text.Trim
                        End If
                        bNoSelection = False
                    Else
                        bAllPlan = False
                    End If
                End If
            Next

            If bNoSelection Then
                'lblMessage.Visible = True
                'lblMessage.CssClass = "alert alert-warning"
                'lblMessage.Text = "Please select plans to utilize."
                'Exit Sub
                sPlan = "ALL"
            End If

            If bAllPlan Then
                sPlan = "ALL"
            End If

            'ecorp.UserID = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("uid"), key).ToString.Trim
            ecorp.UserID = Request.QueryString("uid")
            For i As Integer = 0 To dtgAccounts.Rows.Count - 1
                Dim cb As CheckBox = DirectCast(dtgAccounts.Rows(i).Cells(0).FindControl("chkSelect"), CheckBox)
                If cb IsNot Nothing Then
                    If cb.Checked Then
                        With ecorp
                            .AccountCode = Trim(dtgAccounts.Rows(i).Cells(2).Text)
                            .CompanyName = Trim(dtgAccounts.Rows(i).Cells(1).Text)
                            .Account_Category = Trim(dtgAccounts.Rows(i).Cells(3).Text)
                            .Mother_Code = IIf(UCase(Trim(dtgAccounts.Rows(i).Cells(3).Text)) = "MOTHER", "", Trim(dtgAccounts.Rows(i).Cells(4).Text))
                            .Account_Plan = sPlan
                            .Save_CorpUser_Accounts()
                            isDataSave = True
                        End With
                    End If
                End If
            Next

            If isDataSave = True Then
                lblMessage.Visible = True
                lblMessage.Text = "Account(s) has been added to user."
            End If
            chkALL.Checked = False
            Load_UserAccounts()
            Load_Available_Accounts()
            Load_Plans(ecorp.AccountCode)
        Catch ex As Exception

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