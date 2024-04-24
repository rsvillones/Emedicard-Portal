Public Class MyCurePMEPatientDetailsPatientResultModel

    Private _name As MyCurePMEPatientDetailsPatientNameResultModel
    Private _dateOfBirth As Long
    Private _sex As String
    Private _insuranceCards As List(Of MyCurePMEPatientDetailsPatientCardResultModel)
    Private _companies As List(Of MyCurePMEPatientDetailsPatientCompanyResultModel)
    Private _maritalStatus As String

    Public Property name As MyCurePMEPatientDetailsPatientNameResultModel
        Get
            Return _name
        End Get
        Set(ByVal value As MyCurePMEPatientDetailsPatientNameResultModel)
            _name = value
        End Set
    End Property

    Public Property dateOfBirth As Long
        Get
            Return _dateOfBirth
        End Get
        Set(ByVal value As Long)
            _dateOfBirth = value
        End Set
    End Property

    Public Property sex As String
        Get
            Return _sex
        End Get
        Set(ByVal value As String)
            _sex = value
        End Set
    End Property

    Public Property insuranceCards As List(Of MyCurePMEPatientDetailsPatientCardResultModel)
        Get
            Return _insuranceCards
        End Get
        Set(ByVal value As List(Of MyCurePMEPatientDetailsPatientCardResultModel))
            _insuranceCards = value
        End Set
    End Property

    Public Property companies As List(Of MyCurePMEPatientDetailsPatientCompanyResultModel)
        Get
            Return _companies
        End Get
        Set(ByVal value As List(Of MyCurePMEPatientDetailsPatientCompanyResultModel))
            _companies = value
        End Set
    End Property

    Public Property maritalStatus As String
        Get
            Return _maritalStatus
        End Get
        Set(ByVal value As String)
            _maritalStatus = value
        End Set
    End Property

End Class
