Public Class MyCurePMEPatientDetailsPatientCardResultModel

    Private _number As String
    Private _status As String

    Public Property number As String
        Get
            Return _number
        End Get
        Set(ByVal value As String)
            _number = value
        End Set
    End Property

    Public Property status As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

End Class
