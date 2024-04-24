Public Class MyCureTemplateData1ResultModel

    Private _id As String
    Private _template As String

    Public Property id As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
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
End Class
