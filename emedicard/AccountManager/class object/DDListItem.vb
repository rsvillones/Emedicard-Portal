Public Class DDListItem

    Private _svalue As String
    Public Property value As String
        Get
            Return _svalue
        End Get
        Set(ByVal value As String)
            _svalue = value
        End Set
    End Property

    Private _sdesc As String
    Public Property description As String
        Get
            Return _sdesc
        End Get
        Set(ByVal value As String)
            _sdesc = value
        End Set
    End Property

End Class
