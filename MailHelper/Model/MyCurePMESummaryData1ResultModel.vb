Public Class MyCurePMESummaryData1ResultModel

    Private _APEReportTemplate As String
    Private _id As String
    Private _patient As String
    Private _createdAt As Long

    Public Property APEReportTemplate As String
        Get
            Return _APEReportTemplate
        End Get
        Set(ByVal value As String)
            _APEReportTemplate = value
        End Set
    End Property

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

    Public Property createdAt As Long
        Get
            Return _createdAt
        End Get
        Set(ByVal value As Long)
            _createdAt = value
        End Set
    End Property

End Class
