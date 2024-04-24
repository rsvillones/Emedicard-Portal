Imports emedicard_DAL
Public Class emedBLL
    Dim objEmedDAL As ememberDAL
    Dim objEconsultDAL As eConsultDAL
    Public Sub New()

    End Sub

    Public Sub SaveConsultation(ByVal sTitle As String, ByVal sDetails As String, ByVal iDoctor As Integer, ByVal uid As Long)
        Dim isSuccess As Boolean = False
        objEmedDAL = New emedicard_DAL.ememberDAL
        Dim objConsult As New emedicard_DAL.emed_consultation

        Try
            With objConsult
                .ConsultationTitle = sTitle
                .Consultation = sDetails
                .DoctorID = iDoctor
                .CreatedDate = Now
                .IP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                .UserID = uid
            End With

            objEmedDAL.SaveConsultation(objConsult)

            isSuccess = True
        Catch ex As Exception
            isSuccess = False
        End Try
    End Sub

    Public Function GetDoctors() As DataTable
        objEmedDAL = New emedicard_DAL.ememberDAL

        Dim dt As New DataTable
        dt = objEmedDAL.GetDoctors

        Return dt
    End Function

    Public Function GetConsultation(ByVal iuserid As Long, ByVal isdoctor As Boolean)
        objEconsultDAL = New emedicard_DAL.eConsultDAL
        Dim dt As New DataTable
        dt = objEconsultDAL.GetCosultationList(iuserid, isdoctor)

        Return dt
    End Function

    Public Sub SaveUserMessage(ByVal iConstID As Integer, ByVal sMsg As String, ByVal isDoctor As Boolean)
        objEmedDAL = New emedicard_DAL.ememberDAL
        Dim objConsultDtl As New emedicard_DAL.emed_consultation_details

        Try
            With objConsultDtl
                .ConsultationID = iConstID
                .ConsultationText = SanitizeInput(sMsg)
                .CreatedDate = Now
                .isDoctor = isDoctor
            End With
            objEmedDAL.SaveConsultationDtls(objConsultDtl)

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Function GetConsultationMsg(ByVal iconsilttid As Integer)
        objEconsultDAL = New emedicard_DAL.eConsultDAL
        Dim ds As New DataSet

        ds = objEconsultDAL.GetConsultationMsg(iconsilttid)

        Return ds
    End Function

    Public Sub MarkConversation(ByVal iConstID As Long, ByVal isDoctor As Boolean)
        Dim objConsult As New emedicard_DAL.eConsultDAL
        objConsult.MarkConversation(iConstID, isDoctor)
    End Sub
    Public Function GetDoctorInfo(ByVal iDocID As Integer)

        Dim objDocInfo As New emed_doctors_user
        Dim objBLLProp As New emedicardBLL.emedProperties

        Using objEconsultDAL = New emedicard_DAL.eConsultDAL
            objDocInfo = objEconsultDAL.GetDoctorInfo(iDocID)
            With objBLLProp
                .doctor_firstname = objDocInfo.Firstname
                .doctor_lastname = objDocInfo.Lastname
                .doctor_email = objDocInfo.Username
            End With
        End Using
        Return objBLLProp

    End Function

    Public Function GetDoctorEmail(ByVal iConsID As Long)
        Dim objDocInfo As New emed_doctors_user
        Dim objBLLProp As New emedicardBLL.emedProperties

        Using objEconsultDAL = New emedicard_DAL.eConsultDAL
            objDocInfo = objEconsultDAL.GetDoctorEmail(iConsID)
            With objBLLProp
                .doctor_firstname = objDocInfo.Firstname
                .doctor_lastname = objDocInfo.Lastname
                .doctor_email = objDocInfo.Username
            End With
        End Using
        Return objBLLProp
    End Function

    Public Function SanitizeInput(ByVal sInput As String)

        ' Encode the string input
        Dim sb As New StringBuilder(HttpUtility.HtmlEncode(sInput))
        '' Selectively allow  <b> and <i>
        'sb.Replace("&lt;b&gt;", "<b>")
        'sb.Replace("&lt;/b&gt;", "")
        'sb.Replace("&lt;i&gt;", "<i>")
        'sb.Replace("&lt;/i&gt;", "")

        Return sb.ToString
    End Function
End Class
