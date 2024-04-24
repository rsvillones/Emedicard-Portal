Imports emedicardBLL
Public Class ConsultationDetails
    Inherits System.Web.UI.Page
    Dim emedBLL As emedicardBLL.emedBLL
    Dim ds As DataSet
    Dim uid As Long
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim userCode As String
    Dim iConsultatioID As Long
    Dim sTitle As String = String.Empty
    Dim bll As ememberBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
        bll = New ememberBLL(userCode)
        bll.GetMemberInformation(userCode)
        uid = bll.UserID
        iConsultatioID = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("cd"), key)


        MarkConversation()
        Load_Messages()

    End Sub

    Protected Sub btnSaveConsult_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveConsult.Click
        emedBLL = New emedicardBLL.emedBLL
        emedBLL.SaveUserMessage(iConsultatioID, txtMessage.Text, False)
        SendEmail()
        Response.Redirect("ConsultationDetails.aspx?u=" & Server.UrlEncode(Request.QueryString("u")) & "&cd=" & Server.UrlEncode(Request.QueryString("cd")))
    End Sub
    Private Sub Load_Messages()
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim sbConstDtls As New StringBuilder
        Dim sClsName As String = String.Empty
        Dim sName As String = String.Empty
        Dim sMSG As String
        Dim sConsultMsg As String = String.Empty

        emedBLL = New emedicardBLL.emedBLL
        ds = New DataSet
        ds = emedBLL.GetConsultationMsg(iConsultatioID)

        dt1 = ds.Tables(0)
        dt2 = ds.Tables(1)
        With sbConstDtls
            For Each dr As DataRow In dt1.Rows
                sTitle = dr("consultationTitle")
                .Append("<div><h3>" & dr("consultationTitle") & "</h3>")
                .Append("<p>Consultation date : " & Format(dr("createdDate"), "MMM dd, yyyy") & " Patient : " & StrConv(dr("member_name"), vbProperCase) & "</p>")
                sConsultMsg = dr("Consultation")
                sConsultMsg = Replace(sConsultMsg, Environment.NewLine, "<br />")
                .Append("<p>" & sConsultMsg & "</p></div>")
            Next

            For Each dr As DataRow In dt2.Rows
                If dr("isDoctor") = True Then
                    sName = dr("doctor_name")
                    sClsName = "docMsg"
                Else
                    sName = dr("member_name")
                    sClsName = "pntMsg"
                End If
                sMSG = dr("ConsultationText")
                sMSG = Replace(sMSG, Environment.NewLine, "<br />")
                '.Append("<div class=""" & sClsName & """><p>Date : " & dr("CreatedDate") & " " & sName & "<br>" & dr("ConsultationText") & "</P></div><br>")
                .Append("<div><div class='msgHd'><font class='uname'><strong>" & StrConv(sName, vbProperCase) & "</strong> </font><font class='pdate'>" & Format(dr("CreatedDate"), "MMM dd, yyyy") & "</font></div><div class='usrMsg'>" & sMSG & "</div></div><br>")
            Next
            consdtls.InnerHtml = sbConstDtls.ToString
        End With
    End Sub

    Private Sub MarkConversation()
        emedBLL = New emedicardBLL.emedBLL
        emedBLL.MarkConversation(iConsultatioID, False)
    End Sub

    Private Sub SendEmail()
        emedBLL = New emedicardBLL.emedBLL
        Dim objDoc As New emedProperties
        Dim sb As New StringBuilder

        objDoc = emedBLL.GetDoctorEmail(iConsultatioID)

        Dim message As String = String.Empty
        Dim from As String = "noreply@medicardphils.com"
        Dim subject As String = "Patient Online Consultation"
        Dim sURL As String = "https://webportal.medicardphils.com/econsult/"

        sb.Append("<div style='width: 90%; margin: 5px; border: 1px solid black; padding: 10px; font-family: sans-serif; font-size: 12px;'><div style='background-color: yellow; width: 100%; position: relative;'><p>A new consultation has arrived!</p></div>")
        sb.Append("<table style='font-size: 12px;'><tr><td style='width: 100px;'><strong>Consultant</strong></td>" & "<td>" & StrConv(objDoc.doctor_firstname, vbProperCase) & " " & StrConv(objDoc.doctor_lastname, vbProperCase) & "</td></tr>")
        sb.Append("<tr><td style='width: 100px;'><strong>Submitted by</strong></td>" & "<td>" & StrConv(bll.Firstname, vbProperCase) & " " & StrConv(bll.Lastname, vbProperCase) & "</td></tr>")
        sb.Append("<tr><td style='width: 100px;'><strong>Submitted date</strong></td>" & "<td>" & Format(Now, "MMM. dd yyyy hh:mm tt") & "</td></tr>")
        sb.Append("<tr><td style='width: 100px;'><strong>Ticket name</strong></td>" & "<td>" & sTitle & "</td></tr>")
        sb.Append("</table>")
        Dim write As String = emedBLL.SanitizeInput(txtMessage.Text)
        write = Replace(write, Environment.NewLine, "<br />")
        'Dim writeArea As String = write.Text.Replace(Environment.NewLine, "<br />")
        sb.Append("<p>" & write & "</p><p>Click the link to log in. <a href='https://webportal.medicardphils.com/econsult/'>https://webportal.medicardphils.com/econsult/</a></p>")
        sb.Append("</div>")

        'message = "<p> Hello Dr. " & objDoc.doctor_firstname & ",</p><br>"
        'message += "<p> You have a new message sent via online consultation. Please click the login link to see the details. <br><br>" & "<a href='" & sURL & "'>" & sURL & ".</p>"
        Try
            Mailhelper.MailHelper.SendMailMessage(from, objDoc.doctor_email, "webteam@medicardphils.com", "", subject, sb.ToString)
        Catch ex As Exception

        End Try

    End Sub
End Class