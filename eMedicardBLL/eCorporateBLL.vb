Imports emedicard_DAL
Imports System.Security.Cryptography
Imports System.Linq
Imports Mailhelper

Public Class eCorporateBLL
    Inherits AccountInformationBLL
    Implements IDisposable

    Private eCorpDB As New eCorporateDAL
    Private eMember As New ememberDAL
    Dim enc As New EncryptDecrypt.EncryptDecrypt
    Private CurrentURL As String = HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Host
    Dim md5hash As MD5 = MD5.Create()

    Private _ErrorMessage As String
    Private _OldPassword As String

    Private _isUpdateusername As Boolean
    Public _AgentEmail As String = String.Empty

    Public Property IsUpdateUsername As Boolean
        Get
            Return _isUpdateusername
        End Get
        Set(value As Boolean)
            _isUpdateusername = value
        End Set
    End Property

    Public Property OldPassword As String
        Get
            Return _OldPassword
        End Get
        Set(value As String)
            _OldPassword = value
        End Set
    End Property
    Public ReadOnly Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
    End Property

    Public ReadOnly Property eCorporateUsers As List(Of emed_corporate_users)
        Get
            Try
                Using db = New eCorporateDAL
                    If Not AccountCode Is Nothing Then Return db.GetCorporateUsers(AccountCode)
                End Using
                Return nothing
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
            
        End Get
    End Property

    Public ReadOnly Property AgentCompany As List(Of Account)
        Get
            ' BY ALLAN ALBACETE
            ' 02/12/2013
            ' GET THE COMPANY OF AGENT
            Dim x As String = Mother_Code
            Using eCorpDB = New eCorporateDAL()
                If Not AccountCode Is Nothing Then Return eCorpDB.GetCompany(AccountCode)
            End Using
            Return Nothing
        End Get
    End Property


    Public ReadOnly Property AdminUser As emed_corporate_users
        Get
            Using db = New eCorporateDAL
                Return db.GetCorporateAdminUser(AccountCode)
            End Using
        End Get
    End Property

    Private _rqtype As Short
    Public Property Request_Type As Short
        Get
            Return _rqtype
        End Get
        Set(ByVal value As Short)
            _rqtype = value
        End Set
    End Property

    Private _uid As Integer
    Public Property UID As Integer
        Get
            Return _uid
        End Get
        Set(ByVal value As Integer)
            _uid = value
        End Set
    End Property

#Region "Constructors"

    Public Sub New(username As String, Optional password As String = Nothing, Optional accountCode As String = Nothing, Optional CorporateUserID As Integer = 0)
        MyBase.New(accountCode, AccountType.eCorporate)
        MyBase.Username = username
        If Not password Is Nothing Then MyBase.Password = password
        If Not accountCode Is Nothing Then MyBase.AccountCode = accountCode
        If CorporateUserID = 0 Then
            GetUserInfo()
        Else
            UserID = CorporateUserID
            GetUserInfoByUserID()
        End If
    End Sub

    Public Sub New()

    End Sub

#End Region

#Region "EMAILING"

    Public Function SendAccountCredentials() As Boolean
        Dim str As New StringBuilder
        Dim subject As String = "Your eMediCard-eCorporate Login Details"
        Dim recipient As String = String.Empty

        recipient = EmailAddress
        str.Append("Hello " & Firstname & " " & Lastname & "<br />")
        str.Append("<p>Here are your eCorporate login details: </p>")
        str.Append("<p><span><strong>Username: " & Username & "</strong></p>")
        str.Append("<p><span><strong>Password: " & PlainPassword & "</strong></p>")
        If ConfigurationManager.AppSettings("DevelopmentMode") = "1" Then ' Sandbox mode
            str.Append("<p>You can now login  <a href ='" & ConfigurationManager.AppSettings("BaseUrl") & "'> here</a></p>")
        Else
            str.Append("<p>You can now login  <a href ='" & ConfigurationManager.AppSettings("BaseUrl") & "'> here</a></p>")
        End If

        str.Append("<p><strong>Note:</strong> Please change your password once you are logged in.</p>")

        Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", recipient, _AgentEmail, "", subject, str.ToString())
        ' Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", "ctubig@medicardphils.com", "", "", subject, str.ToString())
        Return Mailhelper.MailHelper.Sent
    End Function
    Public Function ResetPassword(Optional ByVal pword As String = Nothing) As Boolean
        Try
            If Not GetUserAccount() Then
                _ErrorMessage = "Email account does not exist."
                Return False
            End If

            GetUserInfo()

            Dim str As New StringBuilder
            Dim subject As String = "eMedicard [eCorporate] Account Retrieval - " & Firstname & " " & Lastname
            Dim recipient As String = String.Empty

            Using db = New emedicard_DAL.eCorporateDAL

                If pword Is Nothing Then
                    Password = EncryptDecrypt.EncryptDecrypt.CreateRandomPassword(PasswordLength)
                Else
                    Password = pword
                End If

                If Not db.ResetPassword(Username, enc.GetMd5Hash(md5hash, Password)) Then ' UPDATE FIRST THE ECORPORATE TEMP PASSWORD
                    'if not updated do not send lost account
                    _ErrorMessage = "There is an error in updating your data. Please try again later"
                    Return False ' 
                End If

            End Using
            recipient = EmailAddress
            str.Append("Hello " & Firstname & " " & Lastname & "<br />")
            str.Append("<p>Here are your account details: </p>")
            str.Append("<p><span><strong>Username: " & Username & "</strong></p>")
            str.Append("<p><span><strong>Password: " & Password & "</strong></p>")
            'str.Append("<p>Try to login  <a href ='" & CurrentURL & "/Login.aspx'> here</a></p>")
            str.Append("<p>Try to login  <a href ='" & ConfigurationManager.AppSettings("BaseUrl") & "/Login.aspx'> here</a></p>")

            Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", recipient, "", "", subject, str.ToString())
            'Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", "ctubig@medicardphils.com", "", "", subject, str.ToString())
            Return Mailhelper.MailHelper.Sent

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function SendIDReplacementRequest() As Boolean
        Dim str As New StringBuilder
        Dim subject As String = "Request for ID Replacement"
        Dim recipient As String = String.Empty
        'Dim msgMail As New Mailhelper.MailHelper

        recipient = EmailAddress
        str.Append("<h4>A new request has arrived.</h4>")
        str.Append("<p><span><strong>Member Name: </strong>" & Firstname & " " & Lastname & "</p>")
        'str.Append("<p><span><strong>Member Code: </strong>" & Member_Code & "</p>")
        str.Append("<p><span><strong>Company: </strong>" & CompanyName & "</p>")
        str.Append("<p>This is a request for id replacement for " & Firstname & " " & Lastname & ", please see attached payment slip.</p>")

        Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", ConfigurationManager.AppSettings("urgemail1").ToString, "", "", subject, str.ToString(), EmailAttachment)

        Return Mailhelper.MailHelper.Sent
    End Function
#End Region

#Region "SELECT"

#Region "LOGIN"

    Public Function CheckUsername() As Boolean
        'BY ALLAN ALBACETE
        '02/04/2013
        Try
            If Not eCorpDB.CheckLogin(Username) Then
                _ErrorMessage = "Invalid username or username already exist. Please try again."
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CheckUserPassword() As Boolean
        'BY ALLAN ALBACETE
        '02/04/2013
        Try
            If Not eCorpDB.CheckLogin(Username, enc.GetMd5Hash(md5hash, Password)) Then
                _ErrorMessage = "Invalid password. Please try again."
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function Login() As Boolean
        'BY ALLAN ALBACETE
        '02/04/2013
        Try
            If Not eCorpDB.LoginUser(Username, enc.GetMd5Hash(md5hash, Password)) Then
                _ErrorMessage = "Sorry we can't log you in. Maybe your login account is not active or not yet activated."
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetUserAccount() As Boolean
        Try
            Using em = New eConsultDAL
                Dim user As List(Of emed_corporate_users) = eCorpDB.GetUserAccountByEmail(EmailAddress)

                If user Is Nothing Then Return False
                For Each Val As emed_corporate_users In user
                    Username = Val.Username


                Next
                Return True
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Sub GetUserInfo()
        Try
            Using em = New eConsultDAL
                Dim user As List(Of emed_corporate_users) = eCorpDB.GetUserAccount(Username)

                If user Is Nothing Then Exit Sub
                For Each Val As emed_corporate_users In user

                    Access_ActionMemos = Val.Access_ActionMemos
                    Access_ActiveMembers = Val.Access_ActiveMembers
                    Access_APE = Val.Access_APE
                    Access_Benefits = Val.Access_Benefits
                    Access_ECU = Val.Access_ECU
                    Access_Endorsement = Val.Access_Endorsement
                    Access_ID = Val.Access_ID
                    Access_ResignedMembers = Val.Access_ResignedMembers
                    Access_Utilization = Val.Access_Utilization
                    Access_ReimbStatus = Val.Access_Reimbursements
                    Access_ClinicResults = Val.Access_ClinicResults


                    Firstname = Val.FirstName
                    Lastname = Val.LastName
                    Username = Val.Username
                    CompanyName = Val.CompanyName
                    Mother_Code = Val.RegMotherCode
                    Phone = Val.TelNo
                    Address = Val.CompanyAddress
                    Designation = Val.Designation
                    EmailAddress = Val.EmailAddress
                    Fax = Val.FaxNo
                    Mobile = Val.MobileNo

                    If Val.AccessLevel = 1 Then
                        AccountCode = Val.RegMotherCode
                    Else
                        AccountCode = Val.RegAccountCode
                    End If

                    UserID = Val.UserID
                    AccessLevel = Val.AccessLevel

                Next

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub GetUserInfoByUserID()
        'ALLAN ALBACETE
        '02/12/2013
        'GET USER INFO BY USER ID
        Try
            Using em = New eConsultDAL
                Dim user As List(Of emed_corporate_users) = eCorpDB.GetUserAccount(UserID)

                If user Is Nothing Then Exit Sub
                For Each Val As emed_corporate_users In user

                    Firstname = Val.FirstName
                    Lastname = Val.LastName
                    Username = Val.Username
                    CompanyName = Val.CompanyName
                    Phone = Val.TelNo
                    Address = Val.CompanyAddress
                    Designation = Val.Designation
                    EmailAddress = Val.EmailAddress
                    Fax = Val.FaxNo
                    Mobile = Val.MobileNo

                    Access_ActionMemos = Val.Access_ActionMemos
                    Access_ActiveMembers = Val.Access_ActiveMembers
                    Access_APE = Val.Access_APE
                    Access_Benefits = Val.Access_Benefits
                    Access_ECU = Val.Access_ECU
                    Access_Endorsement = Val.Access_Endorsement
                    Access_ID = Val.Access_ID
                    Access_ResignedMembers = Val.Access_ResignedMembers
                    Access_Utilization = Val.Access_Utilization
                    Access_ReimbStatus = Val.Access_Reimbursements
                    Access_ClinicResults = Val.Access_ClinicResults

                    If Val.AccessLevel = 1 Then
                        AccountCode = Val.RegMotherCode
                    Else
                        AccountCode = Val.RegAccountCode
                    End If
                    AccessLevel = Val.AccessLevel
                Next

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub GetUserID()
        Try
            Using em = New eConsultDAL
                Dim user As emed_corporate_users = eCorpDB.GetCorporateUserInfo(Username)

                If Not user Is Nothing Then UserID = user.UserID


            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Function fGetUserID()
        Dim uid As Integer = -1
        Try
            Using em = New eConsultDAL
                Dim user As emed_corporate_users = eCorpDB.GetCorporateUserInfo(Username)

                If Not user Is Nothing Then
                    uid = user.UserID
                End If

            End Using
            Return uid

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Sub GetCorporateInfo(ByVal AccountCode As String)
        Try
            Dim acct As Account = eCorpDB.GetCorporateInfo(AccountCode)
            CompanyName = acct.ACCOUNT_NAME
            Address = acct.STREET
            Phone = acct.PHONE_NO
            Fax = acct.FAX_NO
            Account_Category = acct.ACCT_CATEGORY
            Mother_Code = acct.MOTHER_CODE

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function GetIDRequest(ByVal recid As Long)

        Dim dt As New DataTable

        dt = eCorpDB.GetIDRequestList(recid)

        Return dt

    End Function

    Public Function GetECUScheduleList()

        Dim dt As New DataTable

        dt = eCorpDB.GetECUScheduleList(AccountCode)

        Return dt

    End Function

    Public Function GetAction_Memos(ByVal sDateFr As String, ByVal sDateTo As String)

        Dim dt As New DataTable

        dt = eCorpDB.GetAction_Memos(AccountCode, sDateFr, sDateTo)

        Return dt

    End Function

    Public Function GetAction_Memos_Dependent(ByVal sDateFr As String, ByVal sDateTo As String)

        Dim dt As New DataTable

        dt = eCorpDB.GetAction_Memos_Dependent(AccountCode, sDateFr, sDateTo)

        Return dt

    End Function

    Function GetCorporateAccountToManage(ByVal accountCode As String)

        Return eCorpDB.GetCorporateAccountToManage(accountCode)

    End Function

#End Region

    Public Sub GetAdminUser()
        Try
            Using db = New eCorporateDAL
                Dim user As emed_corporate_users = db.GetCorporateAdminUser(AccountCode)
                If user IsNot Nothing Then
                    Firstname = user.FirstName
                    Lastname = user.LastName
                    UserID = user.UserID
                    EmailAddress = user.EmailAddress
                    Username = user.Username
                    Designation = user.Designation
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Public Sub GetCompanyContactInfo()
        Try
            Using db = New eCorporateDAL
                Dim user As Account = db.GetCompanyContactInfo(AccountCode)
                If user IsNot Nothing Then
                    Firstname = user.CONTACT_FNAME
                    Lastname = user.CONTACT_LNAME
                    Designation = user.CONTACT_POSITION
                    Mother_Code = user.MOTHER_CODE
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub

    'CHECK USER EXISTENCE BY USERID AND PASSWORD
    Public Function CheckUserExistence() As Boolean
        Try
            Using db = New eCorporateDAL
                If Not db.CheckUsernameExistence(UserID, EmailAddress) Then
                    If db.GetUserAccountByEmail(EmailAddress).Count > 0 Then
                        _ErrorMessage = "Email already exist in database"
                        Return False
                    End If
                End If
                Return True
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCorporateUserAccount()

        Return eCorpDB.GetCorporateUserAccount(userid)

    End Function

    Public Function GetCorporateUserAvailabeAccounts()

        Return eCorpDB.GetCorporateUserAvailabeAccounts(AccountCode, UserID)

    End Function

    Public Function CorpUserCheckEmail()
        Using corp = New eCorporateDAL
            Return corp.CorpUserCheckEmail(EmailAddress)
        End Using
    End Function

    Public Function ChangeUserEmail()
        Try
            Using oDAL = New eCorporateDAL
                Return oDAL.ChangeUserEmail(Username, EmailAddress)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub GetAccountPlan()
        Dim objAgentAcct As New emed_corporate_accounts
        Using objDAL = New eCorporateDAL
            objAgentAcct = objDAL.GetAccountPlanEcorp(AccountCode, UserID)

            With objAgentAcct
                Account_Plan = .Plans
            End With
        End Using

    End Sub
    Public Sub GetAccountPlan(ByVal id As Long)
        Dim objAgentAcct As New emed_corporate_accounts
        Using objDAL = New eCorporateDAL
            objAgentAcct = objDAL.GetAccountPlanEcorp(id)

            With objAgentAcct
                Account_Plan = .Plans
            End With
        End Using

    End Sub
    Public Function GetPlanToUtilize()
        Dim dt As New DataTable

        Try
            Using objDAL = New eCorporateDAL
                Dim sAccCode As String
                If Trim(Mother_Code) <> "" Then
                    sAccCode = Mother_Code
                Else
                    sAccCode = AccountCode
                End If
                Return objDAL.GetPlanToUtilize(sAccCode)
            End Using

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function IsAllowedToUpload() As Boolean

        Using objDal = New eCorporateDAL
            Return objDal.IsAllowedToUpload(AccountCode)
        End Using

    End Function

    Public Function GetCorporateRequestList() As DataTable
        Dim dt As New DataTable
        Using objDal = New eCorporateDAL
            dt = objDal.GetCorporateRequestList(AccountCode, UserType)
            Return dt
        End Using
    End Function

    Public Function GetCorporateRequestListAll() As DataTable
        Dim dt As New DataTable
        Using objDal = New eCorporateDAL
            dt = objDal.GetCorporateRequestListAll(AccountCode, UserType, Request_Type)
            Return dt
        End Using
    End Function

    Public Function GetRequestDetail() As DataTable
        Dim dt As New DataTable
        Using objDal = New eCorporateDAL
            dt = objDal.GetRequestDetails(record_id, AccountCode)
            Return dt
        End Using
    End Function

    Public Sub UpdateRequestViews()
        Using objDal = New eCorporateDAL
            objDal.UpdateRequestViews(record_id, UserType)
        End Using
    End Sub

    Public Function GetEmailFromRequest() As DataTable
        Dim dt As New DataTable
        Using objDal = New eCorporateDAL
            dt = objDal.GetEmailFromRequest(uploaded_by, record_id)
            Return dt
        End Using
    End Function

    Public Function GetTransactionStatus() As Boolean
        Dim bActive As Boolean = True

        Using objDal = New eCorporateDAL
            bActive = objDal.GetTransactionStatus(record_id)
        End Using

        Return bActive
    End Function

    Public Function GetAccountContactInfo()
        Dim oUsers As New emed_corporate_users
        Using objDal = New eCorporateDAL
            oUsers = objDal.GetAccountContactInfo(AccountCode)
        End Using
        If Not IsNothing(oUsers) Then
            EmailAddress = oUsers.EmailAddress

            Return oUsers.EmailAddress
        Else
            EmailAddress = ""
            Return ""
        End If

    End Function
#End Region

#Region "CRUD"

    Public Function UpdateUserProfile() As Boolean
        Try
            Dim objUser As New emed_corporate_users
            With objUser
                .CompanyName = CompanyName
                .Designation = Designation
                .EmailAddress = EmailAddress
                .FaxNo = Fax
                .FirstName = Firstname
                .LastName = Lastname
                .MobileNo = Mobile
                .TelNo = Phone
                .Username = Username
                If Password <> "" Then
                    'CHECK if password already exists
                    .Pword = enc.GetMd5Hash(md5hash, Password)
                End If


            End With

            ' SAVE PROFILE
            Using db = New eCorporateDAL()

                Return db.UpdateProfile(objUser)
            End Using
            'UpdateProfile()
        Catch ex As Exception
            Throw New Exception()
        End Try
    End Function
    Public Function UpdateUserInfo() As Boolean
        Try
            Dim objUser As New emed_corporate_users

            With objUser
                .Access_ActionMemos = Access_ActionMemos
                .Access_ActiveMembers = Access_ActiveMembers
                .Access_APE = Access_APE
                .Access_Benefits = Access_Benefits
                .Access_ECU = Access_ECU
                .Access_Endorsement = Access_Endorsement
                .Access_ID = Access_ID
                .Access_ResignedMembers = Access_ResignedMembers
                .Access_Utilization = Access_Utilization
                .AccessLevel = AccessLevel
                .Access_Reimbursements = Access_ReimbStatus
                .Access_ClinicResults = Access_ClinicResults
                '.CompanyAddress = Address
                .CompanyName = CompanyName
                .Designation = Designation
                .EmailAddress = EmailAddress
                If Password <> "" Then .Pword = enc.GetMd5Hash(md5hash, Password)
                .FaxNo = Fax
                .FirstName = Firstname
                .IsActive = True
                .LastName = Lastname
                .MobileNo = Mobile
                .TelNo = Phone
                If IsUpdateUsername Then .Username = Username
                .UserID = UserID
                .RegMotherCode = RegisteredMotherCode
                .RegAccountCode = RegisteredAccountCode
            End With

            Return eCorpDB.UpdateUserInfo(objUser)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function CheckOldPassword()
        Try
            Using eCorpDal = New eCorporateDAL
                Return eCorpDal.CheckOldPassword(enc.GetMd5Hash(md5hash, OldPassword), Username)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function AddUser()
        Try
            Dim corpUser As New emed_corporate_users

            With corpUser
                .Access_ActionMemos = Access_ActionMemos
                .Access_ActiveMembers = Access_ActiveMembers
                .Access_APE = Access_APE
                .Access_Benefits = Access_Benefits
                .Access_ECU = Access_ECU
                .Access_Endorsement = Access_Endorsement
                .Access_ResignedMembers = Access_ResignedMembers
                .Access_Utilization = Access_Utilization
                .Access_Reimbursements = Access_ReimbStatus
                .Access_ClinicResults = Access_ClinicResults
                .CompanyAddress = Address
                .AccessLevel = AccessLevel
                .MainUserID = MainAgentID
                .CompanyName = CompanyName
                .DateRegistered = Today
                .DateUserActivation = Today
                .Designation = Designation
                .EmailAddress = EmailAddress
                .Access_ID = Access_ID
                .FaxNo = Fax
                .FirstName = Firstname
                .IsActive = True
                .LastName = Lastname
                .MobileNo = Mobile
                .Pword = enc.GetMd5Hash(md5hash, Password)
                .RegAccountCode = RegisteredAccountCode
                .RegMotherCode = RegisteredMotherCode
                .TelNo = Phone
                .Username = Username
            End With

            eCorpDB = New eCorporateDAL
            Dim result As Integer = eCorpDB.AddUser(corpUser)
            Me.UID = result
            If result > 0 Then
                'send login account to new user 
                SendAccountCredentials()
                Return result
            End If

            Return 0

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Sub Save_ID_Request()

        Dim IDRequest As New emed_corporate_id_request

        With IDRequest
            .FirstName = Firstname
            .LastName = Lastname
            .Birthday = IIf(Trim(BirthDate) <> "", CDate(BirthDate), Nothing)
            .AccountCode = AccountCode
            .AccountName = CompanyName
            .PaymentMode = PaymentMode
            .Status = Status
            .RequestedDate = Now
            .RequestedBy = Username
            .UserID = UserID
            .UserType = UserTypeDesc
        End With

        eCorpDB = New eCorporateDAL
        eCorpDB.Save_ID_Request(IDRequest)

    End Sub

    Public Sub Delete_ID_Request(ByVal iRequestID As Integer)

        eCorpDB.Delete_ID_Request(iRequestID)

    End Sub

    Public Sub Save_ECU_Request()
        Dim oECU As New emed_ecu_requests

        Dim objCompany As New Account

        eCorpDB = New eCorporateDAL

        objCompany = eCorpDB.GetCompanyByCode(AccountCode)

        With oECU
            .UserID = UserID
            .AccountCode = AccountCode
            .CompanyName = objCompany.ACCOUNT_NAME
            .MemberCode = Member_Code
            .MemberFName = Firstname
            .MemberLName = Lastname
            .MemberMInitial = MidInitial
            .MemberDesignation = Designation
            .PreferredDate = Prefered_Date
            .HospitalCode = Hospital_Code
            .HospitalName = Hospital_Name
            .Remarks = Remarks
            .Status = "Pending"
            .RequestedDate = Now
            .RequestedBy = Username
            .UserType = "CORPORATE"
        End With


        eCorpDB.Save_ECU_Request(oECU)

    End Sub

    Public Sub GetECUMemberDetails(ByVal sMemberCode As String)
        Dim oMemDtls As New DataTable

        oMemDtls = eMember.GetECUMemberDetails(AccountCode, sMemberCode)

        Member_Code = ""
        Firstname = ""
        MidInitial = ""
        Lastname = ""

        For Each dr As DataRow In oMemDtls.Rows
            Member_Code = dr("PRIN_CODE")
            Firstname = dr("MEM_FNAME")
            MidInitial = dr("MEM_MI")
            Lastname = dr("MEM_LNAME")
        Next

    End Sub

    Public Sub Delete_ECU_Request(ByVal sRqDate As String, ByVal memcode As String)

        eCorpDB = New eCorporateDAL
        eCorpDB.Delete_ECU_Request(sRqDate, memcode)

    End Sub

    Public Sub Save_Utilization_Request()
        Dim objUtilization As New emed_requested_utilization

        Dim objCompany As New Account

        eCorpDB = New eCorporateDAL

        objCompany = eCorpDB.GetCompanyByCode(AccountCode)

        With objUtilization
            .UserID = UserID
            .AccountCode = AccountCode
            .AccountName = objCompany.ACCOUNT_NAME
            .Remarks = Remarks
            .RequestedDate = Now
            .RequestedBy = Username
            .Status = "Pending"
        End With

        eCorpDB.Save_Utilization_Request(objUtilization)
    End Sub

    Public Sub Save_CorpUser_Accounts()
        eCorpDB = New eCorporateDAL

        Dim objUserAccounts As New emed_corporate_accounts

        With objUserAccounts
            .UserID = UserID
            .AccountCode = AccountCode
            .AccountName = CompanyName
            .AccountCategory = Account_Category
            .MotherCode = Mother_Code
            .DateCreated = Now
            .Plans = Account_Plan
            .Status = "Active"
            eCorpDB.Save_CorpUsers_Account(objUserAccounts)
        End With
    End Sub

    Public Function Save_Endorsement_Request()

        Dim lID As Long

        eCorpDB = New eCorporateDAL

        Dim objRequest As New emed_corporate_users_requests

        Dim dt As New DataTable

        User_Fullname = eCorpDB.GetUserFullName(Username, UserType)

        With objRequest
            .remarks = Remarks
            .file_path = file_path
            .uploaded_by = uploaded_by
            .uploaded_date = Now
            .account_code = AccountCode
            .subject = Title
            .is_deleted = False
            .is_mother = Is_Mother
            .mother_id = MotherID
            .is_read_corp = 0
            .is_read_agent = 0
            .is_read_urg = 0
            .grp_type = UserType
            .is_closed = False
            .uploaded_by_name = User_Fullname
            '.status_id = 1
            lID = eCorpDB.Save_Endorsement_Request(objRequest)
        End With

        Return lID

    End Function

    Function GetUserFullName(ByVal UserName As String, ByVal iType As String) As String

        eCorpDB = New eCorporateDAL

        Return eCorpDB.GetUserFullName(UserName, iType)

    End Function

    Public Sub Update_CorporateUser_Plans(ByVal id As Long)
        eCorpDB = New eCorporateDAL

        Try
            eCorpDB.Update_CorporateUser_Plans(id, Account_Plan)
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Sub Delete_EcorpUserAccounts(ByVal id As Integer)
        eCorpDB = New eCorporateDAL

        eCorpDB.Delete_CorpUserAccounts(id)

    End Sub

    Public Sub DeleteEndorsementCancelRequest(ByVal id As Long)

        eCorpDB = New eCorporateDAL

        eCorpDB.DeleteEndorsementCancelRequest(id)

    End Sub
    Public Sub Add_Request_Status_History(ByVal refid As Integer)
        'Dim obj As New emed_request_status_history
        'Using eCorpDB = New eCorporateDAL
        '    obj.reference_id = refid
        '    obj.status_id = 1
        '    obj.status_date = Now
        '    eCorpDB.Add_Request_Status_History(obj)
        'End Using
    End Sub
#End Region
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
