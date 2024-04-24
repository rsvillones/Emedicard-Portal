Imports emedicard_DAL
Imports System.Security.Cryptography

Public Class eAccountBLL
    Inherits AccountInformationBLL
    Dim objDAL As New eAccountDAL
    Private _ErrorMessage As String

    Dim enc As New EncryptDecrypt.EncryptDecrypt
    Private CurrentURL As String = HttpContext.Current.Request.Url.Scheme & "://" & HttpContext.Current.Request.Url.Host
    Dim md5hash As MD5 = MD5.Create()

#Region "Constructors"

    Public Sub New(ByVal username As String, Optional ByVal password As String = Nothing, Optional ByVal accountCode As String = Nothing)
        MyBase.New(accountCode, AccountType.eAccount)
        MyBase.Username = username
        If Not password Is Nothing Then MyBase.Password = password
        If Not accountCode Is Nothing Then MyBase.AccountCode = accountCode
        GetAgentInfo()
        Me.UserType = 2

    End Sub
    Public Sub New()

    End Sub

    Public ReadOnly Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
    End Property

#Region "Functions and Procedure"
    Public Sub SaveAgentInfo()
        objDAL = New eAccountDAL
        Dim objAgentInfo As New emed_agent_users
        With objAgentInfo
            .UserID = UserID
            .FirstName = Firstname
            .LastName = Lastname
            .Username = Username
            If Len(Trim(Password)) > 0 Then .Pword = Password
            .EmailAddress = EmailAddress

        End With
        objDAL.SaveAgent(objAgentInfo)
    End Sub

    Public Function SaveAgentUserInfo()
        Dim iUID As Integer = 0

        objDAL = New eAccountDAL
        Dim objAgentInfo As New emed_agent_users
        With objAgentInfo
            .UserID = UserID
            .FirstName = Firstname
            .LastName = Lastname
            .Username = Username
            If Len(Trim(Password)) > 3 Then
                .Pword = Password
            End If
            .EmailAddress = EmailAddress
            .AccessLevel = 2
            .Access_APE = Access_APE
            .Access_Utilization = Access_Utilization
            .Access_Endorsement = Access_Endorsement
            .Access_Benefits = Access_Benefits
            .Access_ID = Access_ID
            .Access_ECU = Access_ECU
            .Access_ActiveMembers = Access_ActiveMembers
            .Access_ResignedMembers = Access_ResignedMembers
            .Access_ActionMemos = Access_ActionMemos
            .Access_Reimbursements = Access_ReimbStatus
            .Access_ClinicResults = Access_ClinicResults
            .MainUserID = MainAgentID
            .AgentCode = AgentCode
            .IsActive = Active
        End With

        iUID = objDAL.SaveAgentUser(objAgentInfo, MainAgentID)

        Return iUID

    End Function

    Public Sub Save_APE_Request()
        objDAL = New eAccountDAL

        Dim objAgnt As New emed_agent_users
        Dim objCorp As New emed_corporate_users
        Dim id As Integer = 0

        If UserType = 1 Then
            objCorp = objDAL.GetCorpUserInfo(Username)
            id = objCorp.UserID
        Else
            objAgnt = objDAL.GetAgentInfo(Username)
            id = objAgnt.UserID
        End If

        Dim objCompany As New Account

        objCompany = objDAL.GetCompany(AccountCode)

        Dim objAPERqst As New emed_ape_requests

        With objAPERqst
            .UserID = id
            .AccountCode = AccountCode
            .CompanyName = objCompany.ACCOUNT_NAME
            .RequestedDate = Now
            .HeadCount = Head_Count
            .Address = Address
            .CityCode = City_Code
            .ProvinceCode = Province_Code
            .Region = Region
            .Status = "Pending"
            .DateCreated = Now
            .CreatedBy = Username
            If UserType = 1 Then
                .UserType = "CORPORATE"
            Else
                .UserType = "AGENT"
            End If

        End With
        objDAL.Save_APE_Request(objAPERqst)
    End Sub

    Public Sub Delete_APE_Request(ByVal apeid As Integer)

        objDAL = New eAccountDAL

        objDAL.Delete_APE_Request(apeid)

    End Sub

    Public Sub Save_User_Accounts()
        objDAL = New eAccountDAL

        Dim objUserAccounts As New emed_eaccount_users_account

        With objUserAccounts
            .UserID = UserID
            .AccountCode = AccountCode
            .AccountName = CompanyName
            .AccountCategory = Account_Category
            .MotherCode = Mother_Code
            .DateCreated = Now
            objDAL.Save_Users_Account(objUserAccounts)
        End With
    End Sub

    Public Sub Delete_UserAccounts(ByVal id As Integer)
        objDAL = New eAccountDAL

        objDAL.Delete_UserAccounts(id)

    End Sub
#End Region

#End Region

#Region "Select"

    Public Sub GetAgentInfo()
        Dim objAgent As New emed_agent_users
        objDAL = New eAccountDAL

        Try
            objAgent = objDAL.GetAgentInfo(Username)

            With objAgent
                Me.Username = .Username
                Me.Firstname = .FirstName
                Me.Lastname = .LastName
                Me.EmailAddress = .EmailAddress
                Me.UserID = .UserID
                Me.AgentCode = .AgentCode
                Me.AccessLevel = .AccessLevel
                Me.Access_APE = .Access_APE
                Me.Access_Utilization = .Access_Utilization
                Me.Access_Endorsement = .Access_Endorsement
                Me.Access_Benefits = .Access_Benefits
                Me.Access_ECU = .Access_ECU
                Me.Access_ID = .Access_ID
                Me.Access_ActiveMembers = .Access_ActiveMembers
                Me.Access_ResignedMembers = .Access_ResignedMembers
                Me.Access_ActionMemos = .Access_ActionMemos
                Me.Access_ReimbStatus = .Access_Reimbursements
                Me.Access_ClinicResults = .Access_ClinicResults
                Me.MainAgentID = .MainUserID


            End With
        Catch ex As Exception
            Throw New Exception("Invalid Username/Email or password!")
        End Try

    End Sub

    Public Sub GetAgentInfoByID()
        Dim objAgent As New emed_agent_users
        objDAL = New eAccountDAL

        objAgent = objDAL.GetAgentInfoByID(UserID)

        With objAgent
            Me.Username = .Username
            Me.Firstname = .FirstName
            Me.Lastname = .LastName
            Me.EmailAddress = .EmailAddress
            Me.UserID = .UserID
            Me.AgentCode = .AgentCode
            Me.AccessLevel = .AccessLevel
            Me.Access_APE = .Access_APE
            Me.Access_Utilization = .Access_Utilization
            Me.Access_Endorsement = .Access_Endorsement
            Me.Access_Benefits = .Access_Benefits
            Me.Access_ID = .Access_ID
            Me.Access_ECU = .Access_ECU
            Me.Access_ActiveMembers = .Access_ActiveMembers
            Me.Access_ResignedMembers = .Access_ResignedMembers
            Me.Access_ActionMemos = .Access_ActionMemos
            Me.Access_ReimbStatus = .Access_Reimbursements
            Me.Access_ClinicResults = .Access_ClinicResults
            MainAgentID = .MainUserID
            Me.Active = .IsActive
        End With

    End Sub

    Public Sub GetAgentInfoByEmail()
        Dim objAgent As New emed_agent_users
        objDAL = New eAccountDAL
        Try
            objAgent = objDAL.GetAgentInfoByEmail(EmailAddress)

            With objAgent
                Me.Username = .Username
                Me.Firstname = .FirstName
                Me.Lastname = .LastName
                Me.EmailAddress = .EmailAddress
                Me.UserID = .UserID
                Me.AgentCode = .AgentCode
                Me.AccessLevel = .AccessLevel
                Me.Access_APE = .Access_APE
                Me.Access_Utilization = .Access_Utilization
                Me.Access_Endorsement = .Access_Endorsement
                Me.Access_Benefits = .Access_Benefits
                Me.Access_ECU = .Access_ECU
                Me.Access_ID = .Access_ID
                Me.Access_ActiveMembers = .Access_ActiveMembers
                Me.Access_ResignedMembers = .Access_ResignedMembers
                Me.Access_ActionMemos = .Access_ActionMemos
                Me.EmailAddress = .EmailAddress
            End With
        Catch ex As Exception
            _ErrorMessage = "Email does not exist!"
        End Try
    End Sub

    Public Sub GetAgentInfoByCode()
        Dim objAgent As New emed_agent_users
        objDAL = New eAccountDAL
        Try
            objAgent = objDAL.GetAgentInfoByAgentCode(AgentCode)

            With objAgent
                Me.Username = .Username
                Me.Firstname = .FirstName
                Me.Lastname = .LastName
                Me.EmailAddress = .EmailAddress
                Me.UserID = .UserID
                Me.AgentCode = .AgentCode
                Me.AccessLevel = .AccessLevel
                Me.Access_APE = .Access_APE
                Me.Access_Utilization = .Access_Utilization
                Me.Access_Endorsement = .Access_Endorsement
                Me.Access_Benefits = .Access_Benefits
                Me.Access_ECU = .Access_ECU
                Me.Access_ID = .Access_ID
                Me.Access_ActiveMembers = .Access_ActiveMembers
                Me.Access_ResignedMembers = .Access_ResignedMembers
                Me.Access_ActionMemos = .Access_ActionMemos
                Me.EmailAddress = .EmailAddress
            End With
        Catch ex As Exception
            _ErrorMessage = "Email does not exist!"
        End Try
    End Sub

    Public Sub Save_Utilization_Request()
        Dim objUtilization As New emed_requested_utilization

        Dim objCompany As New Account

        Dim eCorpDB As New eCorporateDAL

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

    Public Function GetAgentUsers()
        Try
            objDAL = New eAccountDAL
            Dim objUsersList = objDAL.GetAgentUsers(AgentCode, UserID)

            If Not objUsersList Is Nothing Then
                Return objUsersList.ToList
            End If
            Return Nothing
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCorporateUsersByUserId(ByVal uid As Integer)
        Try
            objDAL = New eAccountDAL
            Dim objUsersList = objDAL.GetCorporateUsersByUserId(uid)

            If Not objUsersList Is Nothing Then
                Return objUsersList.ToList
            End If
            Return Nothing
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBookDates() As DataTable
        Dim dt As New DataTable

        objDAL = New eAccountDAL
        dt = objDAL.GetBookDates(Booking_Year, Booking_Month)

        Return dt
    End Function

    Public Function GetProvinces()
        Dim dt As New DataTable

        objDAL = New eAccountDAL
        dt = objDAL.GetProvinces()

        Return dt
    End Function

    Public Function GetCity()
        Dim dt As New DataTable

        objDAL = New eAccountDAL
        dt = objDAL.GetCity(Me.Province_Code)

        Return dt
    End Function

    Public Function GetRegion()
        Dim sRegion As String

        objDAL = New eAccountDAL
        sRegion = objDAL.GetRegion(Me.Region_Code)
        Return sRegion
    End Function

    Public Function GetAPERequestList(ByVal sType As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        Dim objAgnt As New emed_agent_users
        Dim objCorpUser As New emed_corporate_users

        Select Case sType
            Case "1"
                objCorpUser = objDAL.GetCorpUserInfo(Username)
                dt = objDAL.GetAPERequest(objCorpUser.UserID)

            Case "2"
                objAgnt = objDAL.GetAgentInfo(Username)
                dt = objDAL.GetAPERequest(objAgnt.UserID)

        End Select

        Return dt
    End Function

    Public Function GetPackageCode()
        Dim sPCode As String
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            sPCode = objDAL.GetPackageCode(Mother_Code)
        Else
            sPCode = objDAL.GetPackageCode(AccountCode)
        End If

        Return sPCode
    End Function

    Public Function GetHCS()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetHCS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetHCS(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetPreventiveHCS()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetPreventiveHCS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetPreventiveHCS(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetECS()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetECS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetECS(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetMFA()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetMFA(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetMFA(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetDCS()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetDCS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetDCS(Member_Type, AccountCode)
        End If


        Return dt
    End Function

    Public Function GetExclusion()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetExclusion(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetExclusion(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetOhtersHCS()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetOhtersHCS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetOhtersHCS(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetOhtersPHCS()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetOhtersPHCS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetOhtersPHCS(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetOhtersECS()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetOhtersECS(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetOhtersECS(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetOhtersMFA()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetOhtersMFA(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetOhtersMFA(Member_Type, AccountCode)
        End If

        Return dt

    End Function

    Public Function GetOhtersDCS()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetOhtersDSC(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetOhtersDSC(Member_Type, AccountCode)
        End If

        Return dt

    End Function

    Public Function GetPEC()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetPEC(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetPEC(Member_Type, AccountCode)
        End If

        Return dt

    End Function

    Public Function GetOthersPEC()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetOthersPEC(Member_Type, Mother_Code)
        Else
            dt = objDAL.GetOthersPEC(Member_Type, AccountCode)
        End If

        Return dt
    End Function

    Public Function GetPOS()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetPOS(Mother_Code)
        Else
            dt = objDAL.GetPOS(AccountCode)
        End If

        Return dt
    End Function

    Public Function GetMATOthers()

        Dim dt As New DataTable

        If Trim(Mother_Code) <> "" Then
            dt = objDAL.GetMATOthers(Mother_Code)
        Else
            dt = objDAL.GetMATOthers(AccountCode)
        End If

        Return dt
    End Function

    Public Function GetECUOthers()

        Dim dt As New DataTable

        dt = objDAL.GetECUOthers(AccountCode)

        Return dt
    End Function

    Public Function GetUsersAccounts()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetAccountsByAgent(AgentCode)

        Return dt
    End Function
    Public Function GetAccountsByUserId(ByVal user_id As Integer)
        Try
            objDAL = New eAccountDAL
            Dim objAccList = objDAL.GetCorporateAccountByUserId(user_id)

            If Not objAccList Is Nothing Then
                Return objAccList.ToList
            End If
            Return Nothing
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function GetUsersActiveAccounts()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetActiveAccountsByAgent(AgentCode)

        Return dt
    End Function

    Public Function GetUserAvailableAccounts()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetAccountsByAgent(AgentCode, UserID, MainAgentID)

        Return dt
    End Function

    Public Function GetUsersAccounts(ByVal user_id As Integer)
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetAccountsByAgent(AgentCode)

        Return dt
    End Function

    Public Function GerEMEDUsersAccount()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetEMEDAccountByAgent(UserID, AgentCode)

        Return dt
    End Function

    Public Function GerEMEDUsersAccount(ByVal istatus As Short)
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetEMEDAccountByAgent(UserID, AgentCode, istatus)

        Return dt
    End Function

    Public Function GetEMEDAgentActiveAccounts()
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetEMEDAgentActiveAccounts(AgentCode)

        Return dt
    End Function
    Public Function GetEMEDAgentActiveAccounts(ByVal istatus As Short)
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetEMEDAgentActiveAccounts(AgentCode, istatus)

        Return dt
    End Function
    Public Sub GetAccountPlan()
        Dim objAgentAcct As New emed_eaccount_users_account
        objDAL = New eAccountDAL

        objAgentAcct = objDAL.GetAccountPlan(AccountCode, UserID)

        With objAgentAcct
            Account_Plan = .Plans
        End With

    End Sub
    Public Sub GetAccountPlanUnderUserAccount(ByVal user_id As Integer)
        Dim objAgentAcct As New emed_eaccount_users_account
        objDAL = New eAccountDAL

        objAgentAcct = objDAL.GetAccountPlansUnderUserAccounts(user_id)

        With objAgentAcct
            Account_Plan = .Plans
        End With

    End Sub

    Public Function GetPlanToUtilize(ByVal sAccCode As String)
        Dim dt As New DataTable

        Try
            objDAL = New eAccountDAL
            Return objDAL.GetPlanToUtilize(sAccCode)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetUtilizationGrpByDisease(ByVal date_from As String, ByVal date_to As String, ByVal last_name As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        If Trim(last_name) = "" Then
            last_name = " "
        End If
        dt = objDAL.GetUtilizationGrpByDisease(AccountCode, date_from, date_to, last_name)

        Return dt
    End Function

    Public Function GetUtilizationGrpByDiseaseOP(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.GetUtilizationGrpByDiseaseOP(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function Get_ER_ByGrp(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.Get_ER_ByGrp(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function GetUtilization(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.GetUtilization(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function GetUtilizationOP(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.GetUtilizationOP(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function GetMemberUtilization(ByVal sDateFr As String, ByVal sDateTo As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL

        dt = objDAL.GetMemberUtilzation(Member_Code, sDateFr, sDateTo)

        Return dt
    End Function

    Public Function Get_ER(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.Get_ER(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function Get_Reim_Util(ByVal service As String, ByVal date_from As String, ByVal date_to As String, ByVal last_name As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        If Trim(last_name) = "" Then
            last_name = " "
        End If
        dt = objDAL.Get_Reim_Util(service, AccountCode, date_from, date_to, last_name)

        Return dt
    End Function
    Public Function Get_Dental(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.Get_Dental(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function Get_DCR(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.Get_DCR(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function

    Public Function Get_Call_Log(ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Dim dt As New DataTable
        objDAL = New eAccountDAL
        'If Trim(last_name) = "" Then
        '    last_name = " "
        'End If
        dt = objDAL.Get_Call_Log(AccountCode, date_from, date_to, mem_code)

        Return dt
    End Function
    Public Function GetCompanyName() As String
        Dim sAcctName As String = String.Empty
        Try
            objDAL = New eAccountDAL
            Dim oAccount As New Account

            oAccount = objDAL.GetCompany(AccountCode)

            Me.AgentCode = oAccount.AGENT_CODE

            Return oAccount.ACCOUNT_NAME
        Catch ex As Exception

        End Try

        Return sAcctName
    End Function
    Public Sub GetMotherCode()
        Dim objAgentAcct As New Account
        objDAL = New eAccountDAL

        objAgentAcct = objDAL.GetMotherCode(AccountCode)

        With objAgentAcct
            Mother_Code = .MOTHER_CODE
        End With

    End Sub

#End Region

#Region "LOGIN"
    Public Function CheckUsername() As Boolean
        'BY ALLAN ALBACETE
        '02/04/2013

        'Copied by Christophe Tubig
        '02/12/2013
        Try
            If Not objDAL.CheckLogin(Username) Then
                _ErrorMessage = "Invalid username. Please try again."
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

        'Copied by Christophe Tubig
        '02/12/2013
        Try
            If Not objDAL.CheckLogin(Username, enc.GetMd5Hash(md5hash, Password)) Then
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

        'Copied by Christophe Tubig
        '02/12/2013
        Try
            If Not objDAL.LoginUser(Username, enc.GetMd5Hash(md5hash, Password)) Then
                _ErrorMessage = "Sorry we can't log you in. Maybe your login account is not active or not yet activated."
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function ResetPassword(Optional ByVal pword As String = Nothing) As Boolean
        Try
            If Not GetUserAccount() Then
                _ErrorMessage = "Email account does not exist."
                Return False
            End If

            GetAgentInfoByEmail()

            Dim str As New StringBuilder
            Dim subject As String = "eMedicard [eAccount] Password Retrieval - " & Firstname & " " & Lastname
            Dim recipient As String = String.Empty

            Dim db As New eAccountDAL

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

            recipient = EmailAddress
            str.Append("Hello " & Firstname & " " & Lastname & "<br />")
            str.Append("<p>Here are your account details: </p>")
            str.Append("<p><span><strong>Username: " & Username & "</strong></p>")
            str.Append("<p><span><strong>Password: " & Password & "</strong></p>")
            'If InStr(1, CurrentURL, "webportal") Then
            '    str.Append("<p>Try to login  <a href ='" & CurrentURL & "/eMedicard/Login.aspx'> here</a></p>")
            'Else
            '    str.Append("<p>Try to login  <a href ='" & CurrentURL & "/Login.aspx'> here</a></p>")
            'End If
            str.Append("<p>Try to login  <a href ='" & ConfigurationManager.AppSettings("BaseUrl") & "/Login.aspx'> here</a></p>")

            Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", recipient, "", "", subject, str.ToString())
            'Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", "ctubig@medicardphils.com", "", "", subject, str.ToString())
            Return Mailhelper.MailHelper.Sent

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function ChangeUserEmail()
        Try

            Return objDAL.ChangeUserEmail(Username, EmailAddress)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetUserAccount() As Boolean
        Try
            Using em = New eConsultDAL
                Dim user As New emed_agent_users
                user = objDAL.GetAgentInfoByEmail(EmailAddress)

                If user Is Nothing Then Return False
                Username = user.Username


                Return True
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region
End Class
