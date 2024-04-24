Imports emedicard_DAL
Public Class EndorsementProperties

    Private _Message As String
    Public Property Message As String
        Get
            Return _Message
        End Get
        Set(value As String)
            _Message = value
        End Set
    End Property
    Private _isExistingPrincipal As Boolean
    Public Property IsExistingPrincipal As Boolean
        Get
            Return _isExistingPrincipal
        End Get
        Set(value As Boolean)
            _isExistingPrincipal = value
        End Set
    End Property

    Private _ErroMessage As String
    Public Property ErrorMessage As String
        Get
            Return _ErroMessage
        End Get
        Set(value As String)
            _ErroMessage = value
        End Set
    End Property
    Private _PrincipalAppNum As String
    Public Property PrincipalAppNum As String
        Get
            Return _PrincipalAppNum
        End Get
        Set(value As String)
            _PrincipalAppNum = value
        End Set
    End Property

    Private _MemberRelationshipDetail As String
    Public Property MemberRelationshipDetail As String
        Get
            Return _MemberRelationshipDetail
        End Get
        Set(value As String)
            _MemberRelationshipDetail = value
        End Set
    End Property

    Private _RelationshipCode As String
    Public Property RelationshipCode As String
        Get
            Return _RelationshipCode
        End Get
        Set(value As String)
            _RelationshipCode = value
        End Set
    End Property


    '==============================================================
    ' PRINCIPAL ENDORSEMENT LISTING - ALL STATUS
    '==============================================================    
    Public ReadOnly Property PrincipalEndorsementList As List(Of EndorsementListAll)
        Get
            Using db = New AccountManagerDAL
                Return db.GetPrincipalEndorsementListAll(AccountCode)
            End Using
            Return Nothing
        End Get
    End Property

    '==============================================
    ' DEPENDENT ENDORSEMENT LISTING - ALL STATUS
    '==============================================
    Public ReadOnly Property DependentEndorsementList As List(Of EndorsementListAll)
        Get
            Using db = New AccountManagerDAL
                Return db.GetDependentEndorsementListAll(AccountCode)
            End Using
        End Get
    End Property
        
    Private _PrincipalEndorsementListing As List(Of EndorsementListing)
    Public ReadOnly Property PrincipalEndorsementListing As List(Of EndorsementListing)
        Get
            Using db = New AccountManagerDAL
                Return db.GetPrincipalEndorsementList(AccountCode)
            End Using
        End Get
    End Property

    Private _RequestedBy As String
    Public Property RequestedBy As String
        Get
            Return _RequestedBy
        End Get
        Set(value As String)
            _RequestedBy = value
        End Set
    End Property

    Private _email As String
    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    Private _UserID As Integer
    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Private _Remarks As String
    Public Property Remarks As String
        Get
            Return _Remarks
        End Get
        Set(value As String)
            _Remarks = value
        End Set
    End Property

    Private _memberType As String
    Public Property MemberType As String
        Get
            Return _memberType
        End Get
        Set(value As String)
            _memberType = value
        End Set
    End Property


    Private _CompanyName As String
    Public ReadOnly Property CompanyName As String
        Get
            Dim acctDAL As New emedicard_DAL.eAccountDAL
            Dim acct As Account
            acct = acctDAL.GetCompany(AccountCode)
            If Not acct Is Nothing Then
                Return acct.ACCOUNT_NAME
            End If
            Return String.Empty
        End Get

    End Property


    Private _AppNum As Double
    Public ReadOnly Property AppNum As Double
        Get
            Using db = New emedBLL
                Return CDbl(db.AppNum)
            End Using
        End Get
    End Property


    Private _PrincipalEmpID As String
    Public Property PrincipalEmpID As String
        Get
            Return _PrincipalEmpID
        End Get
        Set(value As String)
            _PrincipalEmpID = value
        End Set
    End Property

    Private _PrincipalMemberCode As String
    Public Property PrincipalMemberCode As String
        Get
            Return _PrincipalMemberCode
        End Get
        Set(value As String)
            _PrincipalMemberCode = value
        End Set
    End Property

    Private _MemberCode As String
    Public Property MemberCode As String
        Get
            Return _MemberCode
        End Get
        Set(value As String)
            _MemberCode = value
        End Set
    End Property

    Private _Lastname As String
    Public Property Lastname As String
        Get
            Return _Lastname
        End Get
        Set(value As String)
            _Lastname = value
        End Set
    End Property

    Private _Firstname As String
    Public Property Firstname As String
        Get
            Return _Firstname
        End Get
        Set(value As String)
            _Firstname = value
        End Set
    End Property


    Private _MiddleInitial As String
    Public Property MiddleInitial As String
        Get
            Return _MiddleInitial
        End Get
        Set(value As String)
            _MiddleInitial = value
        End Set
    End Property


    Private _BirthDate As Date
    Public Property Birthdate As Date
        Get
            Return _BirthDate
        End Get
        Set(value As Date)
            _BirthDate = value
        End Set
    End Property

    Private _Age As Integer
    Public Property Age As Integer
        Get
            Return _Age
        End Get
        Set(value As Integer)
            _Age = value
        End Set
    End Property


    Private _Sex As String
    Public Property Sex As String
        Get
            Return _Sex
        End Get
        Set(value As String)
            _Sex = value
        End Set
    End Property

    Private _CivilStatus As String
    Public Property CivilStatus As String
        Get
            Return _CivilStatus
        End Get
        Set(value As String)
            _CivilStatus = value
        End Set
    End Property


    Private _EffectivityDate As Date
    Public Property EffectivityDate As Date
        Get
            Return _EffectivityDate
        End Get
        Set(value As Date)
            _EffectivityDate = value
        End Set
    End Property

    Private _ValidyDate As Date
    Public Property ValidityDate As Date
        Get
            Return _ValidyDate
        End Get
        Set(value As Date)
            _ValidyDate = value
        End Set
    End Property

    Private _PlanDetails As String
    Public Property PlanDetails As String
        Get
            Return _PlanDetails
        End Get
        Set(value As String)
            _PlanDetails = value
        End Set
    End Property

    Private _PlanCode As String
    Public Property PlanCode As String
        Get
            Return _PlanCode
        End Get
        Set(value As String)
            _PlanCode = value
        End Set
    End Property

    Private _PlanRNBFor As String
    Public Property PlanRNBFor As String
        Get
            Return _PlanRNBFor
        End Get
        Set(value As String)
            _PlanRNBFor = value
        End Set
    End Property

    Private _Area As String
    Public Property Area As String
        Get
            Return _Area
        End Get
        Set(value As String)
            _Area = value
        End Set
    End Property

    Private _MemberRelation As String
    Public Property MemberRelation As String
        Get
            Return _MemberRelation
        End Get
        Set(value As String)
            _MemberRelation = value
        End Set
    End Property


    Private _AccountCode As String
    Public Property AccountCode As String
        Get
            Return _AccountCode
        End Get
        Set(value As String)
            _AccountCode = value
        End Set
    End Property

    Private _AccountName As String
    Public Property AccountName As String
        Get
            Return _AccountName
        End Get
        Set(ByVal value As String)
            _AccountName = value
        End Set
    End Property

    Private _uname As String
    Public Property UserName As String
        Get
            Return _uname
        End Get
        Set(ByVal value As String)
            _uname = value
        End Set
    End Property

    Private _utype As String
    Public Property UserType As String
        Get
            Return _utype
        End Get
        Set(ByVal value As String)
            _utype = value
        End Set
    End Property

    Private _mcode As String
    Public Property MotherCode As String
        Get
            Return _mcode
        End Get
        Set(ByVal value As String)
            _mcode = value
        End Set
    End Property
End Class
