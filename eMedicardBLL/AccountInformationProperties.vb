Imports emedicard_DAL
Imports System.Net.Mail

'BY ALLAN ALBACETE
'02/04/2013
'INFO/FIELDS OF CORPORATE
Public Class AccountInformationProperties
    Private _username As String
    Private _password As String
    Private _Firstname As String 'Contact person's firstname
    Private _Lastname As String
    Private _Midname As String
    Private _CompanyName As String 'Contact person's lastname
    Private _ManageAccountName As String
    Private _Designation As String 'Contact person's designation
    Private _Address As String 'Contact person's designation
    Private _Phone As String 'Contact person's phone
    Private _Mobile As String 'Contact person's mobile
    Private _Fax As String 'Contact person's fax
    Private _EmailAddress As String 'Contact person's email address
    Private _AccountCode As String
    Private _ManageAccountCode As String
    Private _AgentCode As String
    Private _AgentName As String
    Private _AccountListing As String
    Private _UserType As String
    Private _UserTypeDesc As String
    Private _USerID As Integer
    Private _Access_ActionMemos As Integer
    Private _Access_ActiveMembers As Integer
    Private _Access_APE As Integer
    Private _Access_Benefits As Integer
    Private _Access_ECU As Integer
    Private _Access_Endorsement As Integer
    Private _Access_ResignedMembers As Integer
    Private _Access_Utilization As Integer
    Private _AccessLevel As Integer
    Private _RegisteredMotherCode As String
    Private _RegisteredAccontCode As String
    Private _PlainPassword As String
    Private _AccessID As Integer ' Request for ID replacement
    Private _EffectivityDate As String
    Private _ValidityDate As String
    Private _mainUserID As Integer
    Private _broker As String
    Private _iBookYr As Integer
    Private _iBookMth As Integer
    Private _sProvinceCode As String
    Private _sRegCode As String
    Private _rqstdate As String
    Private _headcount As Integer
    Private _ctycode As String
    Private _provcode As String
    Private _region As String
    Private _memtype As String
    Private _BirthDate As String
    Private _PaymentMode As String
    Private _Status As String
    Private _acctCategory As String
    Private _acctMotherCode As String = String.Empty
    Private _memCode As String
    Private _prefDate As String
    Private _hospCode As String
    Private _hospName As String
    Private _remarks As String
    Private _plan As String
    Private _sfilepath As String
    Private _uploadedby As String
    Private _uploadeddate As String
    Private _processedby As String
    Private _processeddate As String
    Private _bisdeleted As String
    Private _lrecid As Long
    Private _AttchMent As List(Of Attachment)
    Private _stitle As String
    Private _is_mother As Boolean
    Private _imohter_id As Integer
    Private _fullname As String
    Private _Access_ReimbStatus As Integer
    Private _Access_ClinicResults As Integer
    Private _ID_REM As String
    Private _ID_REM2 As String
    Private _ID_REM3 As String
    Private _ID_REM4 As String
    Private _ID_REM5 As String
    Private _ID_REM6 As String
    Private _ID_REM7 As String

    Public ReadOnly Property ActiveMembersPrincipal() As List(Of ActiveMembersPrincipal)
        Get
            Using db = New AccountManagerDAL(AccountCode)
                Return db.GetActiveMembersPrincipal
            End Using
        End Get
    End Property

    Public ReadOnly Property ActiveMembersDependent As List(Of ActiveMembersDependent)
        Get
            Using db = New AccountManagerDAL(AccountCode)
                Return db.GetActiveMembersDependent
            End Using
        End Get
    End Property
    Public Property EffectivityDate As String
        Get
            Return _EffectivityDate
        End Get
        Set(ByVal value As String)
            _EffectivityDate = value
        End Set
    End Property

    Public Property ValidityDate As String
        Get
            Return _ValidityDate
        End Get
        Set(ByVal value As String)
            _ValidityDate = value
        End Set
    End Property


    Public Property AgentName As String
        Get
            Return _AgentName
        End Get
        Set(ByVal value As String)
            _AgentName = value
        End Set
    End Property

    Public Property Access_ID As Integer
        Get
            Return _AccessID
        End Get
        Set(ByVal value As Integer)
            _AccessID = value
        End Set
    End Property

    Public Property PlainPassword As String
        Get
            Return _PlainPassword
        End Get
        Set(ByVal value As String)
            _PlainPassword = value
        End Set
    End Property

    Public ReadOnly Property PasswordLength As Integer
        Get
            Return ConfigurationManager.AppSettings("passwordlength")
        End Get
    End Property

    Public Property RegisteredMotherCode As String
        Get
            Return _RegisteredMotherCode
        End Get
        Set(ByVal value As String)
            _RegisteredMotherCode = value
        End Set
    End Property

    Public Property RegisteredAccountCode As String
        Get
            Return _RegisteredAccontCode
        End Get
        Set(ByVal value As String)
            _RegisteredAccontCode = value
        End Set
    End Property

    Public Property AccessLevel As Integer
        Get
            Return _AccessLevel
        End Get
        Set(ByVal value As Integer)
            _AccessLevel = value
        End Set
    End Property

    Public Property Access_ActiveMembers As Integer
        Get
            Return _Access_ActiveMembers
        End Get
        Set(ByVal value As Integer)
            _Access_ActiveMembers = value
        End Set
    End Property

    Public Property Access_ActionMemos As Integer
        Get
            Return _Access_ActionMemos
        End Get
        Set(ByVal value As Integer)
            _Access_ActionMemos = value
        End Set
    End Property

    Public Property Access_APE As Integer
        Get
            Return _Access_APE
        End Get
        Set(ByVal value As Integer)
            _Access_APE = value
        End Set
    End Property

    Public Property Access_Benefits As Integer
        Get
            Return _Access_Benefits
        End Get
        Set(ByVal value As Integer)
            _Access_Benefits = value
        End Set
    End Property

    Public Property Access_ECU As Integer
        Get
            Return _Access_ECU
        End Get
        Set(ByVal value As Integer)
            _Access_ECU = value
        End Set
    End Property

    Public Property Access_Endorsement As Integer
        Get
            Return _Access_Endorsement
        End Get
        Set(ByVal value As Integer)
            _Access_Endorsement = value
        End Set
    End Property

    Public Property Access_ResignedMembers As Integer
        Get
            Return _Access_ResignedMembers
        End Get
        Set(ByVal value As Integer)
            _Access_ResignedMembers = value
        End Set
    End Property

    Public Property Access_Utilization As Integer
        Get
            Return _Access_Utilization
        End Get
        Set(ByVal value As Integer)
            _Access_Utilization = value
        End Set
    End Property

    Public Property Access_ReimbStatus As Integer
        Get
            Return _Access_ReimbStatus
        End Get
        Set(ByVal value As Integer)
            _Access_ReimbStatus = value
        End Set
    End Property

    Public Property Access_ClinicResults As Integer
        Get
            Return _Access_ClinicResults
        End Get
        Set(ByVal value As Integer)
            _Access_ClinicResults = value
        End Set
    End Property

    Public Property ID_REM As String
        Get
            Return _ID_REM
        End Get
        Set(ByVal value As String)
            _ID_REM = value
        End Set
    End Property

    Public Property ID_REM2 As String
        Get
            Return _ID_REM2
        End Get
        Set(ByVal value As String)
            _ID_REM2 = value
        End Set
    End Property

    Public Property ID_REM3 As String
        Get
            Return _ID_REM3
        End Get
        Set(ByVal value As String)
            _ID_REM3 = value
        End Set
    End Property

    Public Property ID_REM4 As String
        Get
            Return _ID_REM4
        End Get
        Set(ByVal value As String)
            _ID_REM4 = value
        End Set
    End Property

    Public Property ID_REM5 As String
        Get
            Return _ID_REM5
        End Get
        Set(ByVal value As String)
            _ID_REM5 = value
        End Set
    End Property

    Public Property ID_REM6 As String
        Get
            Return _ID_REM6
        End Get
        Set(ByVal value As String)
            _ID_REM6 = value
        End Set
    End Property

    Public Property ID_REM7 As String
        Get
            Return _ID_REM7
        End Get
        Set(ByVal value As String)
            _ID_REM7 = value
        End Set
    End Property

    Public Enum AccountType
        eMember = 0
        eCorporate = 1
        eAccount = 2
    End Enum

    'Public ReadOnly Property AccountListing As List(Of Account)
    Public ReadOnly Property AccountListing
        Get
            Using db = New AccountManagerDAL

                Select Case _UserType
                    Case 1 ' Corporate
                        Return db.GetLLAccountsByCode(AccountCode, UserID)
                    Case 2 ' eAgent
                        Return db.GetAccountsByAgent(AgentCode)
                End Select

            End Using
            Return Nothing
        End Get
    End Property


    Public Property UserID As Integer
        Get
            Return _USerID
        End Get
        Set(ByVal value As Integer)
            _USerID = value
        End Set
    End Property

    Public Property UserType As Integer
        Get
            Return _UserType
        End Get
        Set(ByVal value As Integer)
            _UserType = value
        End Set
    End Property

    Public Property UserTypeDesc As String
        Get
            Return _UserTypeDesc
        End Get
        Set(ByVal value As String)
            _UserTypeDesc = value
        End Set
    End Property

    Public Property AgentCode() As String
        Get
            Return _AgentCode
        End Get
        Set(ByVal value As String)
            _AgentCode = value
        End Set
    End Property

    Public Property AccountCode As String
        Get
            Return _AccountCode
        End Get
        Set(ByVal value As String)
            _AccountCode = value
        End Set
    End Property

    Public Property Firstname As String
        Get
            Return _Firstname
        End Get
        Set(ByVal value As String)
            _Firstname = value
        End Set
    End Property

    Public Property Lastname As String
        Get
            Return _Lastname
        End Get
        Set(ByVal value As String)
            _Lastname = value
        End Set
    End Property

    Public Property MidInitial As String
        Get
            Return _Midname
        End Get
        Set(ByVal value As String)
            _Midname = value
        End Set
    End Property

    Public Property CompanyName As String
        Get
            Return _CompanyName
        End Get
        Set(ByVal value As String)
            _CompanyName = value
        End Set
    End Property

    Public Property Designation As String
        Get
            Return _Designation
        End Get
        Set(ByVal value As String)
            _Designation = value
        End Set
    End Property

    Public Property Address As String
        Get
            Return _Address
        End Get
        Set(ByVal value As String)
            _Address = value
        End Set
    End Property

    Public Property Phone As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = value
        End Set
    End Property

    Public Property Mobile As String
        Get
            Return _Mobile
        End Get
        Set(ByVal value As String)
            _Mobile = value
        End Set
    End Property

    Public Property Fax As String
        Get
            Return _Fax
        End Get
        Set(ByVal value As String)
            _Fax = value
        End Set
    End Property

    Public Property EmailAddress As String
        Get
            Return _EmailAddress
        End Get
        Set(ByVal value As String)
            _EmailAddress = value
        End Set
    End Property

    Public Property Username As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property MainAgentID As Integer
        Get
            Return _mainUserID
        End Get
        Set(ByVal value As Integer)
            _mainUserID = value
        End Set
    End Property

    Public Property broker_name As String
        Get
            Return _broker
        End Get
        Set(ByVal value As String)
            _broker = value
        End Set
    End Property

    Public Property Booking_Year As Integer
        Get
            Return _iBookYr
        End Get
        Set(ByVal value As Integer)
            _iBookYr = value
        End Set
    End Property

    Public Property Booking_Month As Integer
        Get
            Return _iBookMth
        End Get
        Set(ByVal value As Integer)
            _iBookMth = value
        End Set
    End Property

    Public Property Region_Code As String
        Get
            Return _sRegCode
        End Get
        Set(ByVal value As String)
            _sRegCode = value
        End Set
    End Property

    Public Property Request_Date As String
        Get
            Return _rqstdate
        End Get
        Set(ByVal value As String)
            _rqstdate = value
        End Set
    End Property

    Public Property Head_Count As Integer
        Get
            Return _headcount
        End Get
        Set(ByVal value As Integer)
            _headcount = value
        End Set
    End Property

    Public Property City_Code As String
        Get
            Return _ctycode
        End Get
        Set(ByVal value As String)
            _ctycode = value
        End Set
    End Property

    Public Property Province_Code As String
        Get
            Return _provcode
        End Get
        Set(ByVal value As String)
            _provcode = value
        End Set
    End Property

    Public Property Region As String
        Get
            Return _region
        End Get
        Set(ByVal value As String)
            _region = value
        End Set
    End Property

    Public Property Member_Type As String
        Get
            Return _memtype
        End Get
        Set(ByVal value As String)
            _memtype = value
        End Set
    End Property

    Public Property BirthDate As String
        Get
            Return _BirthDate
        End Get
        Set(ByVal value As String)
            _BirthDate = value
        End Set
    End Property

    Public Property PaymentMode As String
        Get
            Return _PaymentMode
        End Get
        Set(ByVal value As String)
            _PaymentMode = value
        End Set
    End Property

    Public Property Status As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property

    Public Property Account_Category As String
        Get
            Return _acctCategory
        End Get
        Set(ByVal value As String)
            _acctCategory = value
        End Set
    End Property

    Public Property Mother_Code As String
        Get
            Return _acctMotherCode
        End Get
        Set(ByVal value As String)
            _acctMotherCode = value
        End Set
    End Property

    Public Property Member_Code As String
        Get
            Return _memCode
        End Get
        Set(ByVal value As String)
            _memCode = value
        End Set
    End Property

    Public Property Prefered_Date As String
        Get
            Return _prefDate
        End Get
        Set(ByVal value As String)
            _prefDate = value
        End Set
    End Property

    Public Property Hospital_Code As String
        Get
            Return _hospCode
        End Get
        Set(ByVal value As String)
            _hospCode = value
        End Set
    End Property

    Public Property Hospital_Name As String
        Get
            Return _hospName
        End Get
        Set(ByVal value As String)
            _hospName = value
        End Set
    End Property

    Public Property Remarks As String
        Get
            Return _remarks
        End Get
        Set(ByVal value As String)
            _remarks = value
        End Set
    End Property

    Public Property Account_Plan As String
        Get
            Return _plan
        End Get
        Set(ByVal value As String)
            _plan = value
        End Set
    End Property

    Public Property ManageAccountCode As String
        Get
            Return _ManageAccountCode
        End Get
        Set(ByVal value As String)
            _ManageAccountCode = value
        End Set
    End Property

    Public Property ManageAccountName As String
        Get
            Return _ManageAccountName
        End Get
        Set(ByVal value As String)
            _ManageAccountName = value
        End Set
    End Property

    Private _isactive As Boolean
    Public Property Active As Boolean
        Get
            Return _isactive
        End Get
        Set(ByVal value As Boolean)
            _isactive = value
        End Set
    End Property

    Public Property EmailAttachment As List(Of Attachment)
        Get
            Return _AttchMent
        End Get
        Set(ByVal value As List(Of Attachment))
            _AttchMent = value
        End Set
    End Property

    Public Property record_id As Long
        Get
            Return _lrecid
        End Get
        Set(ByVal value As Long)
            _lrecid = value
        End Set
    End Property

    Public Property file_path As String
        Get
            Return _sfilepath
        End Get
        Set(ByVal value As String)
            _sfilepath = value
        End Set
    End Property

    Public Property uploaded_by As String
        Get
            Return _uploadedby
        End Get
        Set(ByVal value As String)
            _uploadedby = value
        End Set
    End Property

    Public Property uploaded_date As String
        Get
            Return _uploadeddate
        End Get
        Set(ByVal value As String)
            _uploadeddate = value
        End Set
    End Property

    Public Property processed_by As String
        Get
            Return _processedby
        End Get
        Set(ByVal value As String)
            _processedby = value
        End Set
    End Property

    Public Property processed_date As String
        Get
            Return _processeddate
        End Get
        Set(ByVal value As String)
            _processeddate = value
        End Set
    End Property

    Public Property is_deleted As Boolean
        Get
            Return _bisdeleted
        End Get
        Set(ByVal value As Boolean)
            _bisdeleted = value
        End Set
    End Property

    Public Property Title As String
        Get
            Return _stitle
        End Get
        Set(ByVal value As String)
            _stitle = value
        End Set
    End Property

    Public Property Is_Mother As Boolean
        Get
            Return _is_mother
        End Get
        Set(ByVal value As Boolean)
            _is_mother = value
        End Set
    End Property

    Public Property MotherID As Integer
        Get
            Return _imohter_id
        End Get
        Set(ByVal value As Integer)
            _imohter_id = value
        End Set
    End Property

    Public Property User_Fullname As String
        Get
            Return _fullname
        End Get
        Set(ByVal value As String)
            _fullname = value
        End Set
    End Property
End Class
