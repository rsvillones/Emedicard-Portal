Public Class MyCurePMEResultModel

    Private _template As String
    Private _values As List(Of MyCureAPEData1ValuesResultModel)
    Private _populated As MyCurePMEPatientDetailsResultModel

    Public Property values As List(Of MyCureAPEData1ValuesResultModel)
        Get
            Return _values
        End Get
        Set(ByVal value As List(Of MyCureAPEData1ValuesResultModel))
            _values = value
        End Set
    End Property

    Public Property template As String
        Get
            Return _template
        End Get
        Set(ByVal value As String)
            _template = value
        End Set
    End Property

    Public Property populated As MyCurePMEPatientDetailsResultModel
        Get
            Return _populated
        End Get
        Set(ByVal value As MyCurePMEPatientDetailsResultModel)
            _populated = value
        End Set
    End Property

End Class
