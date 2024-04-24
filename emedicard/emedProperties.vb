Public Class emedProperties
    Public Sub New()

    End Sub

    Private _DocEmail As String
    Public Property doctor_email As String
        Get
            Return _DocEmail
        End Get
        Set(ByVal value As String)
            _DocEmail = value
        End Set
    End Property

    Private _docfname As String
    Public Property doctor_firstname As String
        Get
            Return _docfname
        End Get
        Set(ByVal value As String)
            _docfname = value
        End Set
    End Property

    Private _doclname As String
    Public Property doctor_lastname As String
        Get
            Return _doclname
        End Get
        Set(ByVal value As String)
            _doclname = value
        End Set
    End Property

  
End Class
