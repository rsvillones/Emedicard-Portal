Public Class MyCurePersonalDetailsData1ResultModel

    Private _id As String
    Private _patient As String

    Public Property id As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Property patient As String
        Get
            Return _patient
        End Get
        Set(ByVal value As String)
            _patient = value
        End Set
    End Property

End Class
