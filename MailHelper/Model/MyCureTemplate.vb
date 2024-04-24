Public Class MyCureTemplate

    Private _data As List(Of MyCureTemplateData1ResultModel)

    Public Property data As List(Of MyCureTemplateData1ResultModel)
        Get
            Return _data
        End Get
        Set(ByVal value As List(Of MyCureTemplateData1ResultModel))
            _data = value
        End Set
    End Property


End Class
