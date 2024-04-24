Imports emedicard_DAL
Imports emedicardBLL
Imports System.Security.Cryptography
Imports EncryptDecrypt.EncryptDecrypt

Public Class ememberBLL
    Inherits emember
    Implements IDisposable
    Dim bll As New ememberDAL
    Private acctType As Integer
    Private CurrentURL As String = HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Host
    Private EncKey As String = ConfigurationManager.AppSettings("encryptionKey").ToString()
    Dim enc As New EncryptDecrypt.EncryptDecrypt
    Dim md5hash As MD5 = MD5.Create()
    Enum AccountType
        emember = 0
        ecorporate = 1
        eaccount = 2
    End Enum

    Public Sub New()

    End Sub
    Public Sub New(ByVal userCode As String, Optional ByVal AccountType As AccountType = 0)

        Me.UserCode = userCode
        GetMemberInformation(userCode)
        acctType = AccountType

    End Sub
    Public Sub New(ByVal usercode As String, ByVal password As String, Optional ByVal AccountType As AccountType = 0)
        Me.UserCode = usercode
        Me.Password = password
        acctType = AccountType
    End Sub
    Public Function CheckUsername() As Boolean
        Try


            'E-member login
            If Not bll.CheckEMemberUserName(UserCode) Then
                Me.LoginMessage = "Incorrect username"
                Return False
            End If
            Return True
        Catch ex As Exception
            Me.LoginMessage = ex.InnerException.ToString()
            Throw New Exception(ex.InnerException.ToString)
        End Try
    End Function
    Public Function CheckUserAccount() As Boolean
        Try
            
            'E-member login
            If Not bll.CheckEMemberPassword(UserCode, enc.GetMd5Hash(md5hash, Password)) Then
                Me.LoginMessage = "Incorrect password or not yet activated"
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function

    Public Function IsMemberActivated() As Boolean
        Try

            If Not bll.IsMemberActivated(UserCode) Then
                Me.LoginMessage = "Your account is not yet activated."
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function

    Public Function GetMemberInformation(ByVal Usercode As String) As Boolean
        Try
            Dim member As emedicard_DAL.emedicard_emember_users
            member = bll.GetUserInfo(Usercode)

            If Not member Is Nothing Then

                MemberCode = member.MemberCode
                Me.UserID = member.UserID
                GetMembershipInformation(MemberCode)
                EmailAddress = member.EmailAddress
                Password = member.Pword
                Return True
            End If
            Return False
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function
    Public Function IsMemberCodeExists(memberCode As String) As Boolean
        Try
            Using db = New ememberDAL
                Return db.CheckEMemberCode(memberCode)
            End Using
        Catch ex As Exception

        End Try
    End Function
    Public Function UpdateMemberLoginAccount(userCode As String, EmailAdd As String, Pass As String) As Boolean
        Try

            Using db = New ememberDAL
                Return db.UpdateEmemberAccount(userCode, EmailAdd, IIf(Pass <> "", enc.GetMd5Hash(md5hash, Pass), ""))
            End Using
        Catch ex As Exception

        End Try

    End Function

    Public Function ValidateMembershipInformation(ByVal MemberCode As String, Birthdate As Date) As Boolean
        'BY ALLAN ALBACETE
        '02/02/2013
        'FOR emember registration
        Try
            Dim member As emedicard_DAL.MemberDetails

            member = bll.ValidateMember(MemberCode, Birthdate)

            If Not member Is Nothing Then Return True
            LoginMessage = "Invalid account details. Please check your member code/birthdate"
            Return False

        Catch ex As Exception
            LoginMessage = ex.Message
        End Try

    End Function
    Public Sub GetMembershipInformation(ByVal MemberCode As String)
        Try
            Dim member As emedicard_DAL.MemberDetails

            member = bll.GetMemberInformation(MemberCode)
            With member
                MemberName = .MEM_NAME
                Firstname = .MEM_FNAME
                Lastname = .MEM_LNAME
                Birthday = .BDAY
                CivilStatus = .CIVIL_STATUS
                Company = .ACCOUNT_NAME
                AccountCode = .ACCOUNT_CODE
                Age = .AGE
                Gender = .MEM_SEX
                AccountStatus = .Mem_OStat_Code
                MemberType = .MEM_TYPE
                Me.MemberCode = MemberCode
                EffectivityDate = .EFF_DATE
                Validitydate = .VAL_DATE
                DDLimit = .DD_Reg_Limits
                PECNonDD = .ERC_Limits
                Plan = .Plan_Desc

                If Not .ID_REM Is Nothing Then
                    If .ID_REM.Length > 0 Then IDRemarks += .ID_REM & "<br />"
                End If

                If Not .ID_REM2 Is Nothing Then
                    If .ID_REM2.Length > 0 Then IDRemarks += .ID_REM2 & "<br />"
                End If

                If Not .ID_REM3 Is Nothing Then
                    If .ID_REM3.Length > 0 Then IDRemarks += .ID_REM3 & "<br />"
                End If

                If Not .ID_REM4 Is Nothing Then
                    If .ID_REM4.Length > 0 Then IDRemarks += .ID_REM4 & "<br />"
                End If

                If Not .ID_REM5 Is Nothing Then
                    If .ID_REM5.Length > 0 Then IDRemarks += .ID_REM5 & "<br />"
                End If

                If Not .ID_REM6 Is Nothing Then
                    If .ID_REM6.Length > 0 Then IDRemarks += .ID_REM6 & "<br />"
                End If

                If Not .ID_REM7 Is Nothing Then
                    If .ID_REM7.Length > 0 Then IDRemarks += .ID_REM7 & "<br />"
                End If

                'If .ID_REM.Length > 0 Then IDRemarks += .ID_REM & "<br />"
                'If .ID_REM2.Length > 0 Then IDRemarks += .ID_REM2 & "<br />"
                'If .ID_REM3.Length > 0 Then IDRemarks += .ID_REM3 & "<br />"
                'If .ID_REM4.Length > 0 Then IDRemarks += .ID_REM4 & "<br />"
                'If .ID_REM5.Length > 0 Then IDRemarks += .ID_REM5 & "<br />"
                'If .ID_REM6.Length > 0 Then IDRemarks += .ID_REM6 & "<br />"
                'If .ID_REM7.Length > 0 Then IDRemarks += .ID_REM7 & "<br />"

                OtherRemarks = .OTHER_REM
                MemberType = .MEM_TYPE

            End With
        Catch ex As Exception
            LoginMessage = ex.Message
        End Try

    End Sub

    Public Function RegisterMember() As Boolean
        'BY ALLAN ALBACETE 01/29/2013
        Try
            Dim user As New emedicard_DAL.emedicard_emember_users
            Dim db As New emedicard_DAL.ememberDAL
            Dim enc As New EncryptDecrypt.EncryptDecrypt

            With user
                .AccessLevel = 1
                .AccountCode = AccountCode
                .AccountName = Company
                .Birthday = Birthday
                .EmailAddress = EmailAddress
                .Firstname = Firstname
                .HomeAddress = Address
                .IsActive = True
                .Lastname = Lastname
                .MemberCode = MemberCode
                .MobileNo = Cellphone
                .Pword = enc.GetMd5Hash(md5hash, Password)
                .TelNo = IIf(Phone = "", " ", Phone)
                .Username = EmailAddress
                If db.RegisterMember(user) Then Return SendActivationLink()
                Return False
            End With

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function IsEmailExists(ByVal email As String)
        Using ememDAL = New ememberDAL
            Return ememDAL.IsEmailExists(email)
        End Using
    End Function
    Public Function SendActivationLink() As Boolean
        'BY ALLAN ALBACETE 01/29/2013
        Try

            Dim str As New StringBuilder
            Dim subject As String = "eMedicard (eMember) Activation Link - " & Firstname & " " & Lastname
            Dim recipient As String = EmailAddress

            str.Append("Hello " & Firstname & " " & Lastname & "<br />")
            str.Append("<p>Thank you for registering to our eMedicard - eMember application. Please click the activation link to complete your registration.</p>")
            'str.Append("<a href ='" & CurrentURL & "/emedicard/Activate.aspx?m=" & HttpUtility.UrlEncode(Encrypt(MemberCode, EncKey)) & "'>" & CurrentURL & "/emedicard/Activate.aspx?m=" & HttpUtility.UrlEncode(Encrypt(MemberCode, EncKey)) & "</a><br />")
            str.Append("<a href ='" & ConfigurationManager.AppSettings("BaseUrl") & "/Activate.aspx?m=" & HttpUtility.UrlEncode(Encrypt(MemberCode, EncKey)) & "'>" & ConfigurationManager.AppSettings("BaseUrl") & "/Activate.aspx?m=" & HttpUtility.UrlEncode(Encrypt(MemberCode, EncKey)) & "</a><br />")
            str.Append("<p>Username : <strong> " & UserCode & "</strong></p>")
            Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", recipient, "", "", subject, str.ToString())

            Return Mailhelper.MailHelper.Sent
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function SendInforRequest(msg As String, sender As String) As Boolean
        'BY ALLAN ALBACETE 01/31/2013
        ' SENDS EMAIL NOTIFICATION TO URG

        Try
            Dim strTo As String = ConfigurationManager.AppSettings("urgemail1")
            Dim from As String = ConfigurationManager.AppSettings("emailsender")
            Dim subj = "Request of Change In Membership Information - " & UCase(sender)

            Dim str As New StringBuilder

            str.Append("<p> I would like to update my personal information as stated below:</p>")
            str.Append("<p>" & msg & "</p>")
            str.Append("<p> Thank you,</p>")
            str.Append("<p>" & sender & "</p>")
            str.Append("<p>" & MemberCode & "</p>")
            str.Append("<p>" & Company & "</p>")

            'Mailhelper.MailHelper.SendMailMessage(from, strTo, "ctubig@medicardphils.com", "", subj, str.ToString())
            Mailhelper.MailHelper.SendMailMessage(from, strTo, "", "", subj, str.ToString())
            Return Mailhelper.MailHelper.Sent
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function SendLostAccount() As Boolean
        ' BY ALLAN ALBACETE 01/30/2013
        Try
            Dim str As New StringBuilder
            Dim subject As String = "eMedicard (eMember) Account Retrieval - " & Firstname & " " & Lastname
            Dim recipient As String = String.Empty
            Dim enc As New EncryptDecrypt.EncryptDecrypt

            Dim iPasswordLength As Integer = ConfigurationManager.AppSettings("passwordlength")
            Password = EncryptDecrypt.EncryptDecrypt.CreateRandomPassword(iPasswordLength)

            Using db = New emedicard_DAL.ememberDAL
                GetUserAccountDetails(UserCode)
                Password = EncryptDecrypt.EncryptDecrypt.CreateRandomPassword(iPasswordLength)
                If Not db.UpdateEmemberPassword(UserCode, enc.GetMd5Hash(md5hash, Password)) Then ' UPDATE FIRST THE EMEMBER PASSWORD
                    'if not updated do not send lost account
                    Me.LoginMessage = "There is an error in updating your data. Please try again later"
                    Return False ' 
                End If

            End Using
            recipient = EmailAddress
            str.Append("Hello " & Firstname & " " & Lastname & "<br />")
            str.Append("<p>Here are your account details: </p>")
            str.Append("<p><span><strong>Username: " & UserCode & "</strong></p>")
            str.Append("<p><span><strong>Password: " & Password & "</strong></p>")
            'str.Append("<p>Try to login  <a href ='" & CurrentURL & "/emedicard/Login.aspx'> here</a></p>")
            str.Append("<p>Try to login  <a href ='" & ConfigurationManager.AppSettings("BaseUrl") & "'> here</a></p>")

            Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", recipient, "", "", subject, str.ToString())
            Return Mailhelper.MailHelper.Sent
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function ActivateAccount(ByVal memberCode As String) As Boolean
        'BY ALLAN ALBACETE 01/29/2013
        Try
            Using db = New emedicard_DAL.ememberDAL
                Return db.ActivateUser(memberCode)
            End Using
        Catch ex As Exception

        End Try
    End Function

    Public Sub GetUserAccountDetails(ByVal userCode As String)
        ' BY ALLAN ALBACETE 01/30/2013
        Try
            Using db = New emedicard_DAL.ememberDAL
                Dim member As emedicard_emember_users = db.GetUserInfo(userCode)
                With member

                    AccountCode = .AccountCode
                    Company = .AccountName
                    Birthday = .Birthday
                    EmailAddress = .EmailAddress
                    Firstname = .Firstname
                    Address = .HomeAddress
                    Lastname = .Lastname
                    MemberCode = .MemberCode
                    Cellphone = .MobileNo
                    'Generate new password
                    Password = .Firstname.Substring(0, 1) & .Lastname

                End With
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Function GetReimbursementStatus() As DataTable
        Dim dt As New DataTable
        Try
            Using bll = New ememberDAL
                dt = bll.GetReimbursementStatus(Me.MemberCode)
                'dt = bll.GetReimbursementStatus("33625419")

                ' Sample :
                ' Disapproved - 33625419
                ' With lacking documents - 33227964
                ' Waiting for hospital bills - 31099747
                ' With billing concern - 33147900
                ' for check preparation - 33571584
                ' Ready for release- 31145655

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return dt

    End Function

    Function GetReimbursementStatusCorp(ByVal memberCode As String, ByVal acctCode As String, ByVal dDateFr As Date, ByVal dDateTo As Date) As DataTable
        Dim dt As New DataTable
        Try
            Using db = New emedicard_DAL.ememberDAL
                Dim member As emedicard_DAL.MemberDetails = db.GetMemberInformation(memberCode)
                If Not IsDBNull(member) Then
                    If member.ACCOUNT_CODE = acctCode Then
                        Using bll = New ememberDAL
                            dt = bll.GetReimbursementStatusWithDateRange(memberCode, dDateFr, dDateTo)
                        End Using
                    End If
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return dt

    End Function

    Function GetReimbControlStatus()
        Dim sStatus As String = String.Empty

        Using bll = New ememberDAL
            sStatus = bll.GetReimbControlStatus(Control_Code, MemberCode)
        End Using

        Return sStatus
    End Function

    Public Sub GetDisapprovalReason()
        Dim reimb As New emedicard_DAL.ReimbursementDetails
        Try
            Using bll = New ememberDAL
                reimb = bll.GetDisapprovalReason(Control_Code)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        reason_of_disapproval = reimb.reason_of_disapproval
        denied_date = reimb.denied_date
        memo_released_date = reimb.memo_released_date

    End Sub
    Public Function  GetActionMemoSubstatus(controlCode As String) as String
        Dim status As String = String.Empty

        Using bll = New ememberDAL
            status = bll.GetActionMemoStatus(controlCode)
        End Using
        Return status
    End Function

    Public Sub GetActionMemo()

        Dim reimb As New emedicard_DAL.ReimbursementDetails
        Try
            Using bll = New ememberDAL
                reimb = bll.GetActionMemo(Control_Code)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        remarks = reimb.remarks
        lacking_date = reimb.lacking_date
        memo_released_date = reimb.memo_released_date

    End Sub

    Public Sub GetWHBMemo()

        Dim reimb As New emedicard_DAL.ReimbursementDetails
        Try
            Using bll = New ememberDAL
                reimb = bll.GetWHBMemo(Control_Code)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        With reimb
            remarks = .remarks
            hospital_bills_from = .hospital_bills_from
            hospital_bills_from_value = .hospital_bills_from_value
            moreover_please_submit = .moreover_please_submit
            moreover_value = .moreover_value
            copy_of_transmittal_loa = .copy_of_transmittal_loa
            loa_no = .loa_no
            lacking_date = .lacking_date
            memo_released_date = .memo_released_date
        End With

    End Sub

    Public Function GetReimDetails(controlCode As String, memberCode As String) As String
        Dim sHTML As New StringBuilder
        Dim reimb As New emedicard_DAL.ReimbursementDetails
        GetMembershipInformation(memberCode)
        Dim dt As DataTable
        Try
            Using bll = New ememberDAL
                reimb = bll.Get_Reimb_Details(controlCode, memberCode)
                dt = bll.GetReimbDetailsRows(controlCode, memberCode)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Dim row As DataRow
        Try
            sHTML.Append("<strong>Reimbursement Details</strong>")
            sHTML.Append("<p>Dear " & MemberName & " </p>")
            sHTML.Append("<p>Your claim has been approved with the following details: </p>")

            sHTML.Append("<table border='0' class='table table-striped table-bordered bootstrap-datatable'>")
            sHTML.Append("<tr>")
            sHTML.Append("<td align='center'><strong>Control Code</strong></td>")
            sHTML.Append("<td align='center'><strong>Reimbursable Amount</strong></td>")
            sHTML.Append("<td align='center'><strong>Availment/Confinement Date</strong></td>")
            sHTML.Append("</tr>")

            For Each row In dt.Rows
                sHTML.Append("<tr>")
                sHTML.Append("<td align='center'>" & controlCode & "</td>")
                sHTML.Append("<td align='right'>" & Format(row("total_approved"), "##,###.00") & "</td>")
                sHTML.Append("<td align='center'>" & row("availedDate") & "</td>")
                sHTML.Append("</tr>")
            Next
            sHTML.Append("<table>")






            With reimb
                sHTML.Append("<table border='0'>")
                sHTML.Append("<tr><td  width='200px'><strong>Total Check Amount: </strong></td>")
                sHTML.Append("<td>" & .approved_amount & "</td></tr>")
                sHTML.Append("<tr><td><strong>Check Number: </strong></td>")
                sHTML.Append("<td>" & .check_no & "</td></tr>")
                sHTML.Append("<tr><td><strong>Check Date: </strong></td>")
                sHTML.Append("<td>" & .check_date & "</td></tr>")
                sHTML.Append("<tr><td><strong>Check Released Date: </strong></td>")
                sHTML.Append("<td>" & .check_release_date & "</td></tr>")
                sHTML.Append("</table><br />")
                If CDbl(.disapproved_amount) > 0 Then
                    sHTML.Append("<p>However, the following are not covered:</p>")
                    sHTML.Append("<table border='0'>")
                    sHTML.Append("<tr><td width='250px'><strong>Disapproved Remarks: </strong></td>")
                    sHTML.Append("<td>" & .reason_of_disapproval & "</td></tr>")
                    sHTML.Append("<tr><td><strong>Total Disapproved Amount: </strong></td>")
                    sHTML.Append("<td>" & .disapproved_amount & "</td></tr>")
                    sHTML.Append("</table><br />")
                End If

            End With




            sHTML.Append("<p>Should you have question/s, please call us at 884-9924 to 25.</p>")

            sHTML.Append("Thank you.")


        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
        Return sHTML.ToString()

    End Function
    Public Sub GetReimbDetails()
        Dim reimb As New emedicard_DAL.ReimbursementDetails
        Try
            Using bll = New ememberDAL
                reimb = bll.Get_Reimb_Details(Control_Code, MemberCode)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        With reimb
            approved_amount = .approved_amount
            check_no = .check_no
            check_date = .check_date
            remarks = .remarks
            ready_for_release_date = .ready_for_release_date
            check_release_date = .check_release_date
            OtherMemoRemarks = .OtherMemoRemarks

        End With

    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
