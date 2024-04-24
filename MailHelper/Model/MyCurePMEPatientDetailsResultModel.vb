Public Class MyCurePMEPatientDetailsResultModel

    Private _patient As MyCurePMEPatientDetailsPatientResultModel
    Private _attachments As List(Of MyCurePMEAttachmentResultModel)

    Public Property patient As MyCurePMEPatientDetailsPatientResultModel
        Get
            Return _patient
        End Get
        Set(ByVal value As MyCurePMEPatientDetailsPatientResultModel)
            _patient = value
        End Set
    End Property

    Public Property attachments As List(Of MyCurePMEAttachmentResultModel)
        Get
            Return _attachments
        End Get
        Set(ByVal value As List(Of MyCurePMEAttachmentResultModel))
            _attachments = value
        End Set
    End Property

End Class
