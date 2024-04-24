Imports emedicardBLL
Imports EncryptDecrypt

Public Class OnlineConsultation
    Inherits System.Web.UI.Page
    Dim emedBLL As emedicardBLL.emedBLL
    Dim ememBLL As ememberBLL
    Dim uid As Long
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim userCode As String
    Dim bll As ememberBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
        bll = New ememberBLL(userCode)
        bll.GetMemberInformation(userCode)
        uid = bll.UserID

        If IsNumeric(uid) Then
            If Not IsPostBack Then
                Load_Doctors()
                Load_ConsultationList(uid)
            End If
        End If

    End Sub
    Private Sub Load_Doctors()
        Dim lDoctors As List(Of Doctors) = GetDoctors()
        For Each f As Doctors In lDoctors

            Dim text As String = String.Format("{0}|{1}", f.doctor_name.PadRight(10, " "c) & " ", " " & f.specialization.PadRight(10, " "c) & " ")

            ddlDoctors.Items.Add(New ListItem(text, f.doctor_id))
        Next
        ddlDoctors.DataBind()
    End Sub

    Function GetDoctors() As List(Of Doctors)
        Dim objDocList As New List(Of Doctors)
        Dim dt As New DataTable
        emedBLL = New emedicardBLL.emedBLL

        dt = emedBLL.GetDoctors

        For i = 0 To dt.Rows.Count - 1
            objDocList.Add(New Doctors(dt(i)(0), dt(i)(1), dt(i)(2)))
        Next

        Return objDocList
    End Function

    Protected Sub btnSaveConsult_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveConsult.Click

        emedBLL = New emedicardBLL.emedBLL
        emedBLL.SaveConsultation(txtConsultation.Text, txtSymptoms.Text, ddlDoctors.SelectedValue, uid)
        SendEmail()
        Response.Redirect("OnlineConsultation.aspx?u=" & Server.UrlEncode(Request.QueryString("u")))

    End Sub

    Private Sub Load_ConsultationList(ByVal iuserid As Integer)
        emedBLL = New emedicardBLL.emedBLL
        Dim dt As New DataTable
        Dim row As Integer = 1

        dt = emedBLL.GetConsultation(iuserid, False)
        Dim sbConsult As New StringBuilder
        sbConsult.Append("<table id=""tblConsult"" class='tblCons'><tr><th>Conditions</th><th>Doctor</th><th>Consultation Date</th><th>Unread Msg</th></tr>")
        For i = 0 To dt.Rows.Count - 1
            If row Mod 2 = 0 Then
                sbConsult.Append("<tr class='even' onmouseover='ChangeColor(this, true);' onmouseout='ChangeColor(this, false);' onclick=""OpenConsultationDtls('" & Server.UrlEncode(Request.QueryString("u")) & "', '" & EncryptDecrypt.EncryptDecrypt.Encrypt(Server.UrlEncode(dt.Rows(i)(0)), key) & "')""><td>" & dt.Rows(i)(1) & "</td><td>" & dt.Rows(i)(4) & "</td><td>" & Format(dt.Rows(i)(2), "MM/dd/yyyy") & "</td><td>" & dt.Rows(i)(3) & "</td></tr>")
            Else
                sbConsult.Append("<tr class='odd' onmouseover='ChangeColor(this, true);' onmouseout='ChangeColor(this, false);' onclick=""OpenConsultationDtls('" & Server.UrlEncode(Request.QueryString("u")) & "', '" & EncryptDecrypt.EncryptDecrypt.Encrypt(Server.UrlEncode(dt.Rows(i)(0)), key) & "')""><td>" & dt.Rows(i)(1) & "</td><td>" & dt.Rows(i)(4) & "</td><td>" & Format(dt.Rows(i)(2), "MM/dd/yyyy") & "</td><td>" & dt.Rows(i)(3) & "</td></tr>")
            End If
            row += 1
        Next
        sbConsult.Append("</table>")
        divcosult.InnerHtml = sbConsult.ToString

        'dtgResult.DataSource = emedBLL.GetConsultation(iuserid, False)
        'dtgResult.DataBind()
    End Sub
    Private Sub SendEmail()
        Dim objDoc As New emedProperties
        Dim sb As New StringBuilder

        emedBLL = New emedicardBLL.emedBLL
        objDoc = emedBLL.GetDoctorInfo(ddlDoctors.SelectedValue)

        Dim message As String = String.Empty
        Dim from As String = "noreply@medicardphils.com"
        Dim subject As String = "Patient Online Consultation - " & txtConsultation.Text
        'Dim sURL As String = "http://localhost:65402/eConsultForm.aspx?mode=1&id=" & Server.UrlEncode(Encrypt(txtEmail.Text, ConfigurationManager.AppSettings("encryptionKey")))

        sb.Append("<div style='width: 90%; margin: 5px; border: 1px solid black; padding: 10px; font-family: sans-serif; font-size: 12px;'><div style='background-color: yellow; width: 100%; position: relative;'><p>A new consultation has arrived!</p></div>")
        sb.Append("<table style='font-size: 12px;'><tr><td style='width: 100px;'><strong>Consultant</strong></td>" & "<td>" & StrConv(objDoc.doctor_firstname, vbProperCase) & " " & StrConv(objDoc.doctor_lastname, vbProperCase) & "</td></tr>")
        sb.Append("<tr><td style='width: 100px;'><strong>Submitted by</strong></td>" & "<td>" & StrConv(bll.Firstname, vbProperCase) & " " & StrConv(bll.Lastname, vbProperCase) & "</td></tr>")
        sb.Append("<tr><td style='width: 100px;'><strong>Submitted date</strong></td>" & "<td>" & Format(Now, "MMM. dd yyyy hh:mm tt") & "</td></tr>")
        sb.Append("<tr><td style='width: 100px;'><strong>Ticket name</strong></td>" & "<td>" & txtConsultation.Text & "</td></tr>")
        sb.Append("</table>")
        Dim write As String = emedBLL.SanitizeInput(txtSymptoms.Text)
        write = Replace(write, Environment.NewLine, "<br />")
        'Dim writeArea As String = write.Text.Replace(Environment.NewLine, "<br />")
        sb.Append("<p>" & write & "</p><p>Click the link to log in. <a href='https://webportal.medicardphils.com/econsult/'>https://webportal.medicardphils.com/econsult/</a></p>")
        sb.Append("</div>")

        'message = "<p> Hello Dr. " & objDoc.doctor_firstname & " " & objDoc.doctor_lastname & ",</p><br>"
        'message += "<p> There's a consultation sent via Online Consultation. The detail is <br><br>" & txtSymptoms.Text & ".</p>"
        Try
            Mailhelper.MailHelper.SendMailMessage(from, objDoc.doctor_email, "webteam@medicardphils.com", "", subject, sb.ToString)
        Catch ex As Exception

        End Try

    End Sub
End Class