Public Class MyCurePMEAttachmentResultModel

    Private _testName As String
    Private _attachmentURLs As List(Of String)

    Public Property testName As String
        Get
            Return _testName
        End Get
        Set(ByVal value As String)
            _testName = value
        End Set
    End Property

    Public Property attachmentURLs As List(Of String)
        Get
            Return _attachmentURLs
        End Get
        Set(ByVal value As List(Of String))
            _attachmentURLs = value
        End Set
    End Property

End Class
