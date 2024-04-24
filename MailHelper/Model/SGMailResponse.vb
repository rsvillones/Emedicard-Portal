Public Class SGMailResponse

    Private _messageId As String

    Public Property messageId As String
        Get
            Return _messageId
        End Get
        Set(ByVal value As String)
            _messageId = value
        End Set
    End Property

End Class
