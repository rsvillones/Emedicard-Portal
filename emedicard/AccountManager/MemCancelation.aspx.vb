Imports emedicardBLL
Public Class MemCancelation
    Inherits System.Web.UI.Page
    Dim objBLL As New EndorsementBLL
    Public key As String = ConfigurationManager.AppSettings("encryptionKey")
    Public sParam As String
    Public sURL As String
    Public sAcctCode As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sParamURL As String = ""
        If Request.QueryString("t").Trim = "1" Then
            sParam = HttpUtility.UrlEncode((EncryptDecrypt.EncryptDecrypt.Encrypt(Request.QueryString("c"), ConfigurationManager.AppSettings("encryptionKey")))) & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u"))
        Else
            sParam = HttpUtility.UrlEncode((EncryptDecrypt.EncryptDecrypt.Encrypt(Request.QueryString("a"), ConfigurationManager.AppSettings("encryptionKey")))) & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u"))
        End If

        'sURL = "https://webportal.medicardphils.com/emedicarddev/AccountManager/MembershipCancelConfirmation.aspx?c=" & sParam
        'sURL = "https://webportal.medicardphils.com/emedicard/AccountManager/MembershipCancelConfirmation.aspx?c=" & sParam
        'sURL = "http://localhost:49922/AccountManager/MembershipCancelConfirmation.aspx?c=" & sParam

        'PROD
        'sURL = "http://webportal.medicardphils.com/emedicard/AccountManager/MembershipCancelConfirmation.aspx?c=" & sParam
        sURL = ConfigurationManager.AppSettings("BaseUrl") & "/AccountManager/MembershipCancelConfirmation.aspx?c=" & sParam

        'DEV
        'sURL = "http://staging.medicardphils.com/stgemedicard/AccountManager/MembershipCancelConfirmation.aspx?c=" & sParam

        InitializedBLL()
        If Not IsPostBack Then
            Load_CancelMembershipRequest()
        End If
        sParamURL = "<input type='button' value='Preview Notification' class='btn btn-primary' style='font-size:12px;width:150px'  onClick=""" & "NewWindow('" & sURL & "','cancel_confirm','800','500')"""">"
        'divBtn.InnerHtml = sParamURL

        'btnPrev.OnClientClick = "NewWindow('" & sURL & "','cancel_confirm','800','500')"
        'btnPrev.OnClientClick = "NewWindow('www.google.com','cancel_confirm','800','500')"
    End Sub

    Private Sub Cancel_Member()

        lblMessage.Visible = False
        lblMessage.Text = ""
        lblRecMsg.Visible = False
        lblRecMsg.Text = ""

        objBLL.MemberCode = txtMemCode.Text

        objBLL.GetMemberInfo()
        If objBLL.Lastname <> "" Then
            If objBLL.MotherCode <> Nothing Then
                If objBLL.MotherCode <> sAcctCode Then
                    lblMessage.Visible = True
                    lblMessage.Text = "Member doesn't belong to the company."
                    Exit Sub
                End If
            Else
                If objBLL.AccountCode <> sAcctCode Then
                    lblMessage.Visible = True
                    lblMessage.Text = "Member doesn't belong to the company."
                    Exit Sub
                End If
            End If

            If objBLL.MemberType.Trim <> "PRINCIPAL" Then
                lblMessage.Visible = True
                lblMessage.Text = "Membership Cancellation is applicable to principal only."
                Exit Sub
            End If

            lblMessage.Visible = False
            lblMessage.Text = ""
            lblRecMsg.Visible = False
            lblRecMsg.Text = ""

            If objBLL.ChekMemberExistCancel Then
                lblRecMsg.Visible = True
                lblRecMsg.Text = "Member has already a pending cancelation request!"
                Exit Sub
            End If

            objBLL.EffectivityDate = dpEffDate.SelectedDate
            objBLL.Remarks = Trim(txtRemarks.Text)
            objBLL.SaveMembershipCancelRequest()

            Clear_Fields()

            lblMessage.Visible = True
            lblMessage.Text = "Your request has been submitted!"

            'Notification
            Notification_CancelMember(objBLL.AccountCode, objBLL.AccountName, objBLL.MemberCode, objBLL.MemberType,
                                      objBLL.Lastname, objBLL.Firstname, objBLL.Remarks, objBLL.EffectivityDate)

            Load_CancelMembershipRequest()
        Else
            lblRecMsg.Visible = True
            lblRecMsg.Text = "Member not found! Please check member code."
        End If
    End Sub

    Private Sub InitializedBLL()
        Dim objAcc As New eAccountBLL
        Dim objCorp As New eCorporateBLL
        If Request.QueryString("t") = 1 Then

            objCorp.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            objBLL.UserID = objCorp.fGetUserID
            objBLL.UserName = objCorp.Username
            objBLL.AccountCode = Request.QueryString("c")
            objBLL.UserType = "CORPORATE"
            sAcctCode = Request.QueryString("c").Trim

        ElseIf Request.QueryString("t") = 2 Then

            objAcc.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            objAcc.GetAgentInfo()
            objBLL.UserID = objAcc.UserID
            objBLL.UserName = objAcc.Username
            objBLL.AccountCode = Request.QueryString("a")
            objBLL.UserType = "ACCOUNT OFFICER"


            sAcctCode = Request.QueryString("a").Trim
        Else
            Exit Sub
        End If
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Cancel_Member()
    End Sub

    Private Sub Load_CancelMembershipRequest()

        dtgMemCancel.DataSource = objBLL.GetCancelMemberList()
        dtgMemCancel.DataBind()

    End Sub

    Protected Sub dtgMemCancel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dtgMemCancel.RowDataBound
        Dim sDisabled As String = String.Empty

        If e.Row.RowType = DataControlRowType.Header Then
            Dim b As Button = DirectCast(e.Row.FindControl("ButtonDelete"), Button)
            b.Attributes.Add("onclick", "return ConfirmOnDelete();")
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(7).Text <> "Pending" Then
                Dim cb As CheckBox = DirectCast(e.Row.FindControl("chkCancel"), CheckBox)
                cb.Enabled = False
            End If
        End If
    End Sub

    Private Sub DeleteRecords(ByVal sc As StringCollection)
        For Each item As String In sc
            objBLL.Delete_Cancelation_Request(item)
        Next
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim sc As New StringCollection()
        Dim id As String = String.Empty
        For i As Integer = 0 To dtgMemCancel.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgMemCancel.Rows(i).Cells(7).FindControl("chkCancel"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    id = dtgMemCancel.Rows(i).Cells(0).Text
                    sc.Add(id)
                End If
            End If
        Next
        DeleteRecords(sc)
        Load_CancelMembershipRequest()
    End Sub

    Private Sub Clear_Fields()
        txtMemCode.Text = ""
        txtRemarks.Text = ""
        dpEffDate.SelectedDate = Nothing
    End Sub

    Private Sub Notification_CancelMember(ByVal sAccCode As String, ByVal sAccName As String, ByVal sMemCode As String,
                                          ByVal sMemType As String, ByVal sLName As String, ByVal sFName As String,
                                          ByVal sRemarks As String, ByVal dEffDate As Date)
        Dim sAccountName As String = String.Empty
        Dim sAgnCode As String = String.Empty
        Dim sSender As String = String.Empty

        Using bll = New eCorporateBLL
            With bll
                .Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                .uploaded_by = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                .AccountCode = Trim(Request.QueryString("a"))
                .UserType = Trim(Request.QueryString("t"))
                Using acctBLLInfo = New AccountInformationBLL(Request.QueryString("a"), 1)
                    sAccountName = acctBLLInfo.CompanyName
                    sAgnCode = acctBLLInfo.AgentCode
                End Using

                Dim sMsg As String
                Dim sTitle As String = "Membership Cancellation - " & sAccName
                Dim sDLink As String = String.Empty


                Using embll = New emedBLL

                    If bll.UserType = 1 Then
                        'Get eAccount
                        embll.application_type = "ecorp"
                    ElseIf bll.UserType = 2 Then
                        embll.application_type = "eaccount"
                    End If
                    embll.mail_description = "Membership Cancellation"

                    embll.GetRecepients()

                    Dim dtCC As New DataTable
                    Dim sCC As String = String.Empty

                    Using oCorpBLL = New eCorporateBLL
                        oCorpBLL.AccountCode = Trim(Request.QueryString("a"))
                        oCorpBLL.GetAccountContactInfo()

                        embll.prm_account_code = Trim(Request.QueryString("a"))
                        embll.prm_agent_code = sAgnCode
                        dtCC = embll.GetCC()

                        For Each dr As DataRow In dtCC.Rows
                            If Trim(sCC) = "" Then
                                sCC = dr(0)
                            Else
                                sCC = sCC & "," & dr(0)
                            End If
                        Next

                        If bll.UserType = 1 Then
                            sSender = embll.GetEmail(bll.Username, "ecorp")
                        ElseIf bll.UserType = 2 Then
                            sSender = embll.GetEmail(bll.Username, "eaccount")
                        End If

                        If Trim(embll.send_to_tag) <> "" Then
                            Select Case Trim(embll.send_to_tag)
                                Case "ecorp_main"

                                    If Trim(sCC) <> "" Then
                                        sCC = sCC & " , " & embll.GetEmail("", "ecorp", "", bll.AccountCode)
                                    Else
                                        sCC = embll.GetEmail("", "ecorp", "", bll.AccountCode)
                                    End If

                                Case "eaccount_main"

                                    If Trim(sCC) <> "" Then
                                        sCC = sCC & " , " & embll.GetEmail("", "eaccount", sAgnCode, bll.AccountCode)
                                    Else
                                        sCC = embll.GetEmail("", "eaccount", sAgnCode, bll.AccountCode)
                                    End If

                            End Select
                        End If

                        'If embll.self Then
                        '    embll.send_to_email = embll.send_to_email & ", " & sSender
                        'End If

                        If Trim(embll.send_to_email) <> "" Then
                            Dim bsent As Boolean = False
                            Dim sUserFullName As String = oCorpBLL.GetUserFullName(.uploaded_by, bll.UserType)

                            sMsg = "<p> A new Membership Cancellation(Single) was filed by " & sUserFullName & " with the following details:</p>"
                            sMsg = sMsg & "<p>Company : " & sAccName & "</p>"
                            sMsg = sMsg & "<p>Membercode : " & sMemCode & "</p>"
                            sMsg = sMsg & "<p>Name : " & sFName & " " & sLName & "</p>"
                            sMsg = sMsg & "<p>Membersip Type : " & sMemType & "</p>"
                            sMsg = sMsg & "<p>Effectivity Date : " & dEffDate.ToString("MMMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) & "</p>"
                            sMsg = sMsg & "<p>Date time received : " & Now & "</p>"
                            sMsg = sMsg & "<p>Remarks : " & Trim(sRemarks) & "</p>"

                            Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", embll.send_to_email, embll.bcc, sCC, sTitle, sMsg)
                            bsent = Mailhelper.MailHelper.Sent

                            Dim sNotifyMsg As String = String.Empty
                            If bll.UserType = 1 Then
                                sNotifyMsg = "<p>Thank you for using eCorporate!</p>"
                            ElseIf bll.UserType = 2 Then
                                sNotifyMsg = "<p>Thank you for using eAccount!</p>"
                            End If
                            sNotifyMsg = sNotifyMsg & "<p>We have received your new request for Membership Cancellation.</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Company : " & sAccName & "</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Membercode : " & sMemCode & "</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Name : " & sFName & " " & sLName & "</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Membersip Type : " & sMemType & "</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Effectivity Date : " & dEffDate.ToString("MMMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) & "</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Date time received : " & Now & "</p>"
                            sNotifyMsg = sNotifyMsg & "<p>Remarks : " & Trim(sRemarks) & "</p>"

                            Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", sSender, "", "", sTitle, sNotifyMsg)

                        End If

                    End Using
                    'Mailhelper.MailHelper.SendMailMessage(.uploaded_by, "mgabat@medicardphils.com", "ctubig@medicardphils.com", "ctubig@medicardphils.com", sTitle, sMsg)
                End Using
            End With
        End Using
    End Sub

End Class