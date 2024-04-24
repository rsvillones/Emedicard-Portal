Public Class MyCurePMEPatientDetailsPatientCompanyResultModel

    Private _externalId As String
    Private _name As String

    Public Property externalId As String
        Get
            Return _externalId
        End Get
        Set(ByVal value As String)
            _externalId = value
        End Set
    End Property

    Public Property name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

End Class
