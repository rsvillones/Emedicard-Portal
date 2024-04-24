Public Class MyCurePMEPatientDetailsPatientNameResultModel

    Private _firstName As String
    Private _middleName As String
    Private _lastName As String

    Public Property firstName As String
        Get
            Return _firstName
        End Get
        Set(ByVal value As String)
            _firstName = value
        End Set
    End Property

    Public Property middleName As String
        Get
            Return _middleName
        End Get
        Set(ByVal value As String)
            _middleName = value
        End Set
    End Property

    Public Property lastName As String
        Get
            Return _lastName
        End Get
        Set(ByVal value As String)
            _lastName = value
        End Set
    End Property

End Class
