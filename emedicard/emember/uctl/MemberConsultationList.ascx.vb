
Public Class MemberConsultationList
    Inherits System.Web.UI.UserControl
    Dim emedBLL As emedicardBLL.emedBLL
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Load_Consultation(ByVal iuserid As Long, ByVal isDoctor As Boolean)
        dtgResult.DataSource = emedBLL.GetConsultation(iuserid, isDoctor)
        dtgResult.DataBind()
    End Sub

End Class