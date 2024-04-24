Public Class MyCurePersonalDetailsResultModel

    Private _total As String
    Private _limit As String
    Private _Data As List(Of MyCurePersonalDetailsData1ResultModel)

    Public Property total As String
        Get
            Return _total
        End Get
        Set(ByVal value As String)
            _total = value
        End Set
    End Property

    Public Property limit As String
        Get
            Return _limit
        End Get
        Set(ByVal value As String)
            _limit = value
        End Set
    End Property

    Public Property data As List(Of MyCurePersonalDetailsData1ResultModel)
        Get
            Return _Data
        End Get
        Set(ByVal value As List(Of MyCurePersonalDetailsData1ResultModel))
            _Data = value
        End Set
    End Property

End Class
