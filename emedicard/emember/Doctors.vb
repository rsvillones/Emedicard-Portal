Public Class Doctors
    Private _docid As Integer
    Private _docname As String
    Private _spec As String
    Public Sub New(ByVal doctor_id As Integer, ByVal doctor_name As String, ByVal specialization As String)
        _docid = doctor_id
        _docname = doctor_name
        _spec = specialization
    End Sub

    Public Property doctor_id As String
        Get
            Return _docid
        End Get
        Set(ByVal value As String)
            _docid = value
        End Set
    End Property

    Public Property doctor_name As String
        Get
            Return _docname
        End Get
        Set(ByVal value As String)
            _docname = value
        End Set
    End Property

    Public Property specialization As String
        Get
            Return _spec
        End Get
        Set(ByVal value As String)
            _spec = value
        End Set
    End Property
End Class
