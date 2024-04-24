Public Class SGMailToken

    Private _access_token As String

    Public Property access_token As String
        Get
            Return _access_token
        End Get
        Set(ByVal value As String)
            _access_token = value
        End Set
    End Property

End Class
