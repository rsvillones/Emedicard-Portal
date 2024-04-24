Imports Mailhelper
Imports emedicardBLL

Public Class MembershipCancelConfirmation
    Inherits System.Web.UI.Page
    Dim dtAcctInfo As New DataTable
    Dim objEndorseBLL As New EndorsementBLL
    Dim objEmail As Mailhelper.MailHelper

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Load_Confirmation()
    End Sub

    Private Sub Load_Confirmation()
        Dim sb As New StringBuilder
        'Try
        sb.Append("<html>")
        sb.Append("<head>")
        sb.Append("<title>Membership Notification</title>")
        sb.Append("<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>")
        sb.Append("<style>")
        sb.Append("body, table, tr, td, th {font-family:tahoma; font-size:12px;}")
        sb.Append("</style>")
        sb.Append("</head>")

        sb.Append("<body>")
        sb.Append("<table align=center>")
        sb.Append("<tr><td><img src='https://webportal.medicardphils.com/eMEdicardPublished/Medicard_Images/signature/logo.jpg' width=120 height=86 border=0></td>")
        sb.Append("<td valign=top><br><font style='font-size:14pt;'>MEDICard Philippines, Inc.</font><br>8th Floor The World Centre Building,<br>330 Sen Gil Puyat Ave., Makati City Philippines<br>")
        sb.Append("Trunkline: (02) 884-9999 / Fax. No.: (02) 810-3855 </td></tr></table>")

        sb.Append("<center><h3>M E M B E R S H I P&nbsp;&nbsp;&nbsp;&nbsp;C A N C E L L A T I O N<br>T R A N S A C T I O N&nbsp;&nbsp;&nbsp;&nbsp;N O T I F I C A T I O N</h3></center><br>")
        sb.Append("Please print this page, this serves as your transaction receipt.<br><br>")

        objEndorseBLL.AccountCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("c"), ConfigurationManager.AppSettings("encryptionKey"))
        dtAcctInfo = objEndorseBLL.GetMemCancelationRequest

        If dtAcctInfo.Rows.Count = 0 Then Exit Sub

        Dim strCompanyName As String = dtAcctInfo(0)(0)
        'Dim sCompanyEmail As String = dtAcctInfo(0)(8)
        Dim sUserEmail As String = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), ConfigurationManager.AppSettings("encryptionKey"))
        sb.Append("Company Name: <strong>" + strCompanyName + "</strong><br><br>")
        sb.Append("Transaction Date: <strong>" + FormatDateTime(Now(), 1) + "</strong><br><br>")

        sb.Append("<table border='1' width='100%' cellspacing='0' cellpadding='4'>")
        sb.Append("<tr><th>Effectivity</th><th>Member Code</th><th>Name</th><th>Type</th><th>Birthday</th><th>Remarks</th><th>Requested By</th></tr>")

        For Each dr As DataRow In dtAcctInfo.Rows
            Dim sEffDate As String = IIf(Not IsDBNull(dr("EffectivityDate")), dr("EffectivityDate"), "")
            Dim sMemCode As String = IIf(Not IsDBNull(dr("MemberCode")), dr("MemberCode"), "")
            Dim sName As String = IIf(Not IsDBNull(dr("Name")), dr("Name"), "")
            Dim sType As String = IIf(Not IsDBNull(dr("MemberType")), dr("MemberType"), "")
            Dim sBDay As String = IIf(Not IsDBNull(dr("Birthday")), Format(dr("Birthday"), "MM/dd/yyyy"), "")
            Dim sRem As String = IIf(Not IsDBNull(dr("Remarks")), dr("Remarks"), "")
            Dim sRequestedBy As String = IIf(Not IsDBNull(dr("RequestedBy")), dr("RequestedBy"), "")

            sb.Append("<tr>")
            sb.Append("<td align='center'>" + sEffDate + "</td>")
            sb.Append("<td align='center'>" + sMemCode + "</td>")
            sb.Append("<td>" + UCase(sName) + "</td>")
            sb.Append("<td align='center'>" + sType + "</td>")
            sb.Append("<td align='center'>" + sBDay + "</td>")
            sb.Append("<td align='center'>" + sRem + "&nbsp;</td>")
            sb.Append("<td align='center'>" + sRequestedBy + "</td>")
            sb.Append("</tr>")

            sRequestedBy = ""
            sMemCode = ""
            sName = ""
            sType = ""
            sBDay = ""
            sRem = ""
            sRequestedBy = ""

        Next


        sb.Append("</table>")
        sb.Append("</body>")
        sb.Append("</html>")

        divContent.InnerHtml = sb.ToString

        Mailhelper.MailHelper.SendMailMessage("noreply@medicardphils.com", sUserEmail, "", "", "Membership Cancellation Transaction Notification", sb.ToString)
        'Catch ex As Exception

        'End Try
    End Sub


End Class