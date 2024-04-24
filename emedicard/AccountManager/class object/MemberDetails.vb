Public Class MemberDetails
    Private _memcode As String
    Public Property MemberCode As String
        Get
            Return _memcode
        End Get
        Set(ByVal value As String)
            _memcode = value
        End Set
    End Property

    Private _fname As String
    Public Property first_name As String
        Get
            Return _fname
        End Get
        Set(ByVal value As String)
            _fname = value
        End Set
    End Property

    Private _mi As String
    Public Property midinit As String
        Get
            Return _mi
        End Get
        Set(ByVal value As String)
            _mi = value
        End Set
    End Property

    Private _lname As String
    Public Property last_name As String
        Get
            Return _lname
        End Get
        Set(ByVal value As String)
            _lname = value
        End Set
    End Property

    Private _accountCode As String
    Public Property account_code As String
        Get
            Return _accountCode
        End Get
        Set(ByVal value As String)
            _accountCode = value
        End Set
    End Property
End Class
