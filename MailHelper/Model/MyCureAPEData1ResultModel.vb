Public Class MyCureAPEData1ResultModel

    Private _report As String
    Private _values As List(Of MyCureAPEData1ValuesResultModel)

    Public Property values As List(Of MyCureAPEData1ValuesResultModel)
        Get
            Return _values
        End Get
        Set(ByVal value As List(Of MyCureAPEData1ValuesResultModel))
            _values = value
        End Set
    End Property

    Public Property report As String
        Get
            Return _report
        End Get
        Set(ByVal value As String)
            _report = value
        End Set
    End Property

End Class
