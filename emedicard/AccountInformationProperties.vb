Imports emedicard_DAL
'BY ALLAN ALBACETE
'02/04/2013
'INFO/FIELDS OF CORPORATE
Public Class AccountInformationProperties
    Private _username As String
    Private _password As String
    Private _Firstname As String 'Contact person's firstname
    Private _Lastname As String
    Private _CompanyName As String 'Contact person's lastname
    Private _Designation As String 'Contact person's designation
    Private _Address As String 'Contact person's designation
    Private _Phone As String 'Contact person's phone
    Private _Mobile As String 'Contact person's mobile
    Private _Fax As String 'Contact person's fax
    Private _EmailAddress As String 'Contact person's email address
    Private _AccountCode As String
    Private _AgentCode As String
    Private _AccountListing As String
    Private _UserType As String
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


    Public Property RegisteredMotherCode As String
        Get
            Return _RegisteredMotherCode
        End Get
        Set(value As String)
            _RegisteredMotherCode = value
        End Set
    End Property

    Public Property RegisteredAccountCode As String
        Get
            Return _RegisteredAccontCode
        End Get
        Set(value As String)
            _RegisteredAccontCode = value
        End Set
    End Property

    Public Property AccessLevel As Integer
        Get
            Return _AccessLevel
        End Get
        Set(value As Integer)
            _AccessLevel = value
        End Set
    End Property

    Public Property Access_ActiveMembers As Integer
        Get
            Return _Access_ActiveMembers
        End Get
        Set(value As Integer)
            _Access_ActiveMembers = value
        End Set
    End Property

    Public Property Access_ActionMemos As Integer
        Get
            Return _Access_ActionMemos
        End Get
        Set(value As Integer)
            _Access_ActionMemos = value
        End Set
    End Property

    Public Property Access_APE As Integer
        Get
            Return _Access_APE
        End Get
        Set(value As Integer)
            _Access_ActionMemos = value
        End Set
    End Property

    Public Property Access_Benefits As Integer
        Get
            Return _Access_Benefits
        End Get
        Set(value As Integer)

        End Set
    End Property

    Public Property Access_ECU As Integer
        Get
            Return _Access_ECU
        End Get
        Set(value As Integer)
            _Access_ECU = value
        End Set
    End Property

    Public Property Access_Endorsement As Integer
        Get
            Return _Access_Endorsement
        End Get
        Set(value As Integer)
            _Access_Endorsement = value
        End Set
    End Property

    Public Property Access_ResignedMembers As Integer
        Get
            Return _Access_ResignedMembers
        End Get
        Set(value As Integer)
            _Access_ResignedMembers = value
        End Set
    End Property

    Public Property Access_Utilization As Integer
        Get
            Return _Access_Utilization
        End Get
        Set(value As Integer)
            _Access_Utilization = value
        End Set
    End Property

    Public Enum AccountType
        eMember = 0
        eCorporate = 1
        eAccount = 2
    End Enum

    Public ReadOnly Property AccountListing As List(Of Account)
        Get
            Using db = New AccountManagerDAL

                Select Case _UserType
                    Case 1 ' Corporate
                        Return db.GetAccountsByCode(AccountCode)
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
        Set(value As Integer)
            _USerID = value
        End Set
    End Property

    Public Property UserType As Integer
        Get
            Return _UserType
        End Get
        Set(value As Integer)
            _UserType = value
        End Set
    End Property

    Public Property AgentCode() As String
        Get
            Return _AgentCode
        End Get
        Set(value As String)
            _AgentCode = value
        End Set
    End Property

    Public Property AccountCode As String
        Get
            Return _AccountCode
        End Get
        Set(value As String)
            _AccountCode = value
        End Set
    End Property

    Public Property Firstname As String
        Get
            Return _Firstname
        End Get
        Set(value As String)
            _Firstname = value
        End Set
    End Property

    Public Property Lastname As String
        Get
            Return _Lastname
        End Get
        Set(value As String)
            _Lastname = value
        End Set
    End Property

    Public Property CompanyName As String
        Get
            Return _CompanyName
        End Get
        Set(value As String)
            _CompanyName = value
        End Set
    End Property

    Public Property Designation As String
        Get
            Return _Designation
        End Get
        Set(value As String)
            _Designation = value
        End Set
    End Property

    Public Property Address As String
        Get
            Return _Address
        End Get
        Set(value As String)
            _Address = value
        End Set
    End Property

    Public Property Phone As String
        Get
            Return _Phone
        End Get
        Set(value As String)
            _Phone = value
        End Set
    End Property

    Public Property Mobile As String
        Get
            Return _Mobile
        End Get
        Set(value As String)
            _Mobile = value
        End Set
    End Property

    Public Property Fax As String
        Get
            Return _Fax
        End Get
        Set(value As String)
            _Fax = value
        End Set
    End Property

    Public Property EmailAddress As String
        Get
            Return _EmailAddress
        End Get
        Set(value As String)
            _EmailAddress = value
        End Set
    End Property

    Public Property Username As String
        Get
            Return _username
        End Get
        Set(value As String)
            _username = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property
End Class
