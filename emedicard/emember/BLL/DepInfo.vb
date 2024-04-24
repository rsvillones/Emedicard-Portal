Public Class DepInfo
    Public Sub New()

    End Sub

    Private _lname As String
    Public Property last_name As String
        Get
            Return _lname
        End Get
        Set(ByVal value As String)
            _lname = value
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
End Class
