Imports emedicardBLL
Imports System.IO

Public Class Utilization
    Inherits System.Web.UI.Page
    Dim objBll As New eAccountBLL
    Dim objBllEcorp As New eCorporateBLL
    Dim dtUtil As New DataTable
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim sAccountPlan As String
    Public Shared oSanitizer As New emedBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Request.QueryString("t").ToString = "1" Then
            objBllEcorp.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            objBllEcorp.GetUserInfo()
            objBllEcorp.AccountCode = Request.QueryString("a")
            objBllEcorp.GetAccountPlan()
            sAccountPlan = objBllEcorp.Account_Plan
            objBll.AccountCode = Request.QueryString("a")
        Else
            objBll.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            objBll.GetAgentInfo()
            objBll.AccountCode = Request.QueryString("a")
            sAccountPlan = "ALL"
        End If

        getMembers()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        'Dim iYr As Double = 0
        'Dim iDays As Double

        'iDays = DateDiff(DateInterval.Day, CDate(dpFrom.SelectedDate), CDate(dpTo.SelectedDate))

        'iYr = iDays / 365.242

        'If iYr > 2 Then
        '    CustomValidator1.IsValid = False
        '    CustomValidator1.ErrorMessage = "Can't load the data, please input a date range with maximum of 2 years."
        'Else
        '    'UserRequest.Visible = True
        'End If

        Select Case ddlService.SelectedValue
            Case "ip", "op", "er"
                If chkGroup.Checked Then
                    Load_Utilization_Report_ByGroup()
                Else
                    Load_Utilization_Report()
                End If
            Case "reip", "reop"
                divlUtil.InnerHtml = Get_Reim_Util()

            Case "dt"
                divlUtil.InnerHtml = Get_Dental()

            Case "dcr"
                divlUtil.InnerHtml = Get_DCR()

            Case "calllog"
                divlUtil.InnerHtml = Get_Call_Log()
            Case "util_all_service"
                divlUtil.InnerHtml = GetMemberUtilization()
        End Select

    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click

        Dim str As New StringBuilder
        Dim filename As String = ddlService.Text
        'Dim iYr As Double = 0
        'Dim iDays As Double

        'iDays = DateDiff(DateInterval.Day, CDate(dpFrom.SelectedDate), CDate(dpTo.SelectedDate))

        'iYr = iDays / 365.242

        'If iYr > 2 Then
        '    CustomValidator1.IsValid = False
        '    CustomValidator1.ErrorMessage = "Can't load the data, please input a date range with maximum of 2 years."
        'Else
        '    UserRequest.Visible = True
        'End If

        Select Case ddlService.SelectedValue
            Case "ip", "op", "er"
                If chkGroup.Checked Then
                    str.Append(GetUtilTableByGrp())
                Else
                    str.Append(GetUtilTable())
                End If
            Case "reip", "reop"
                str.Append(Get_Reim_Util())
            Case "dt"
                str.Append(Get_Dental())
            Case "dcr"
                str.Append(Get_DCR())
            Case "calllog"
                str.Append(Get_Call_Log())
            Case "util_all_service"
                str.Append(GetMemberUtilization())
        End Select

        Response.Clear()

        Response.AddHeader("content-disposition", "attachment;    filename=" & filename & ".xls")

        Response.Charset = ""

        Response.ContentType = "application/vnd.xls"


        Dim stringWrite As TextWriter = New System.IO.StringWriter()
        stringWrite.Write(str.ToString)

        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

        Response.Write(stringWrite.ToString())

        Response.End()
    End Sub


    Private Sub Load_Utilization_Report_ByGroup()

        divlUtil.InnerHtml = "<p><strong>No Data Found.</strong></p>"
        divlUtil.InnerHtml = GetUtilTableByGrp()

    End Sub

    Private Sub Load_Utilization_Report()

        divlUtil.InnerHtml = "<p><strong>No Data Found.</strong></p>"
        divlUtil.InnerHtml = GetUtilTable()

    End Sub

    Function GetUtilTableByGrp()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0

        Select Case ddlService.SelectedValue
            Case "ip"
                'dtUtil = objBll.GetUtilizationGrpByDisease(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.GetUtilizationGrpByDisease(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)
            Case "op"
                'dtUtil = objBll.GetUtilizationGrpByDiseaseOP(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.GetUtilizationGrpByDiseaseOP(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)
            Case "er"
                'dtUtil = objBll.Get_ER_ByGrp(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.Get_ER_ByGrp(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)
        End Select

        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Bill Code</th><th>Principal EmpID</th><th>Name</th><th>Availment</th><th>Diagnosis</th><th>Hospital</th><th>Approved</th></tr>")
            sb.Append("<tr><th>Bill Code</th><th>Principal EmpID</th><th>Name</th><th>Availment</th><th>Hospital</th><th>Approved</th></tr>")
            For Each dr As DataRow In dtUtil.Rows
                If Not IsDBNull(dr("RM_CODE")) Then
                    sRMCode = Trim(dr("RM_CODE"))
                Else
                    sRMCode = "0"
                End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    If Not IsDBNull(dr("ICD10_CODE")) Then
                        strICD10B = Trim(dr("ICD10_CODE"))
                    Else
                        strICD10B = ""
                    End If

                    If strICD10a <> strICD10B And i > 0 Then
                        sb.Append("<tr><td colspan='7' align='right' valign='top'><font color='#006699'><strong>Sub Total: " & Format(SAmount, "#,###.00") & "</strong></font></td></tr>")
                    End If

                    If strICD10a <> strICD10B Then
                        sb.Append("<tr><td colspan='7'><font color='#006699'><strong>ICD10 Code: " & dr("ICD10_CODE") & "/" & dr("ICD10_DESC") & "</strong></font></td></tr>")
                        strICD10a = dr("ICD10_CODE")
                        SAmount = 0
                    End If
                    sb.Append("<tr><td>" & dr("CTLNO") & "</td>")
                    sb.Append("<td></td>")
                    sb.Append("<td>" & dr("LAST_NAME") & ", " & dr("FIRST_NAME") & "</td>")
                    Select Case ddlService.SelectedValue
                        Case "ip"
                            sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "-" & Format(dr("DATE_DISCHARGE"), "MM/dd/yyyy") & "</td>")

                        Case "op", "er"
                            sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "</td>")
                    End Select

                    'sb.Append("<td><span title='" & dr("ICD10_DESC") & "'>" & dr("DX_DESC") & "</span></td>")
                    sb.Append("<td>" & dr("H_NAME") & "</td>")

                    If Not IsDBNull(dr("UTIL_AMT")) Then
                        sb.Append("<td align='right' valign='top'>" & Format(dr("UTIL_AMT"), "#,###.00") & "</td></tr>")
                    Else
                        sb.Append("<td></td></tr>")
                    End If

                    If Not IsDBNull(dr("UTIL_AMT")) Then
                        SAmount = SAmount + dr("UTIL_AMT")
                        TAmount = TAmount + dr("UTIL_AMT")
                    End If
                    'strICD10a = Trim(dr("ICD10_CODE"))
                    i = i + 1
                End If

            Next

            sb.Append("<tr><td colspan='7' align='right' valign='top'><font color='#006699'><strong>Grand Total: " & Format(TAmount, "#,###.00") & "</strong></font></td></tr>")
            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else
            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If

        Return sb.ToString

    End Function

    Function GetUtilTable()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0

        Select Case ddlService.SelectedValue
            Case "ip"
                'dtUtil = objBll.GetUtilization(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.GetUtilization(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)
            Case "op"
                'dtUtil = objBll.GetUtilizationOP(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.GetUtilizationOP(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)
            Case "er"
                'dtUtil = objBll.Get_ER(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.Get_ER(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)
        End Select

        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Bill Code</th><th>Principal EmpID</th><th>Name</th><th>Availment</th><th>Diagnosis</th><th>Hospital</th><th>Approved</th></tr>")
            sb.Append("<tr><th>Bill Code</th><th>Principal EmpID</th><th>Name</th><th>Availment</th><th>Hospital</th><th>Approved</th></tr>")
            For Each dr As DataRow In dtUtil.Rows
                If Not IsDBNull(dr("RM_CODE")) Then
                    sRMCode = Trim(dr("RM_CODE"))
                Else
                    sRMCode = "0"
                End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    sb.Append("<tr><td>" & dr("CTLNO") & "</td>")
                    sb.Append("<td></td>")
                    sb.Append("<td>" & dr("LAST_NAME") & ", " & dr("FIRST_NAME") & "</td>")
                    Select Case ddlService.SelectedValue
                        Case "ip"
                            If Not IsDBNull(dr("DATE_DISCHARGE")) And Not IsDBNull(dr("VISIT_DATE")) Then
                                sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "-" & Format(dr("DATE_DISCHARGE"), "MM/dd/yyyy") & "</td>")
                            Else
                                sb.Append("<td> - </td>")
                            End If


                        Case "op", "er"
                            sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "</td>")
                    End Select
                    'sb.Append("<td><span title='" & dr("ICD10_DESC") & "'>" & dr("DX_DESC") & "</span></td>")
                    sb.Append("<td>" & dr("H_NAME") & "</td>")
                    If Not IsDBNull(dr("UTIL_AMT")) Then
                        sb.Append("<td align='right' valign='top'>" & Format(dr("UTIL_AMT"), "#,###.00") & "</td></tr>")
                    Else
                        sb.Append("<td></td></tr>")
                    End If


                End If
                i = i + 1
                TAmount = TAmount + dr("UTIL_AMT")
            Next
            'sb.Append("<tr><td colspan='6' align='right' valign='top'><font color='#006699'><strong>Grand Total:</strong></font></td><td align='right'><strong>" & Format(TAmount, "#,###.00") & "</strong></tr>")
            sb.Append("<tr><td colspan='5' align='right' valign='top'><font color='#006699'><strong>Grand Total:</strong></font></td><td align='right'><strong>" & Format(TAmount, "#,###.00") & "</strong></tr>")
            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else

            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If


        Return sb.ToString
    End Function

    Function Get_Reim_Util()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0

        Select Case ddlService.SelectedValue
            Case "reip"
                'dtUtil = objBll.Get_Reim_Util("IP", dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.Get_Reim_Util("IP", dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)

            Case "reop"
                'dtUtil = objBll.Get_Reim_Util("OP", dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
                dtUtil = objBll.Get_Reim_Util("OP", dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)

        End Select

        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Principal EmpID</th><th>Name</th><th>File Date</th><th>Due Date</th><th>Processed Date</th><th>Released Date</th><th>Diagnosis</th><th>Hospital</th><th>SA Amount</th><th>Approved</th></tr>")
            sb.Append("<tr><th>Principal EmpID</th><th>Name</th><th>File Date</th><th>Due Date</th><th>Processed Date</th><th>Released Date</th><th>Hospital</th><th>SA Amount</th><th>Approved</th></tr>")
            For Each dr As DataRow In dtUtil.Rows
                If Not IsDBNull(dr("RM_CODE")) Then
                    sRMCode = Trim(dr("RM_CODE"))
                Else
                    sRMCode = "0"
                End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    sb.Append("<tr><td>" & dr("PRIN_COMPID") & "</td>")
                    sb.Append("<td>" & dr("DNAME") & "</td>")
                    sb.Append("<td>" & Format(dr("DATE_RCVD"), "MM/dd/yyyy") & "</td>")
                    sb.Append("<td>" & Format(dr("DATE_DUE"), "MM/dd/yyyy") & "</td>")
                    If Not IsDBNull(dr("PROCESSED_DATE")) Then
                        sb.Append("<td>" & Format(dr("PROCESSED_DATE"), "MM/dd/yyyy hh:mm:ss tt") & "</td>")
                    Else
                        sb.Append("<td></td>")
                    End If
                    sb.Append("<td>" & IIf(Not IsDBNull(dr("DATE_DUE")), Format(dr("DATE_DUE"), "MM/dd/yyyy"), "") & "</td>")
                    'sb.Append("<td>" & dr("DIAG_DESC") & "</td>")
                    sb.Append("<td>" & dr("HOSPITAL_NAME") & "</td>")
                    If Not IsDBNull(dr("AMOUNT")) Then
                        sb.Append("<td align='right' valign='top'>" & Format(dr("AMOUNT"), "#,###.00") & "</td>")
                    Else
                        sb.Append("<td></td>")
                    End If
                    If Not IsDBNull(dr("TOTAL_APPROVED")) Then
                        sb.Append("<td align='right' valign='top'>" & Format(dr("TOTAL_APPROVED"), "#,###.00") & "</td>")
                    Else
                        sb.Append("<td></td>")
                    End If

                End If
                i = i + 1
                If Not IsDBNull(dr("AMOUNT")) Then
                    SAmount = SAmount + dr("AMOUNT")
                End If

                If Not IsDBNull(dr("TOTAL_APPROVED")) Then
                    TAmount = TAmount + dr("TOTAL_APPROVED")
                End If
            Next
            'sb.Append("<tr><td colspan='8' align='right' valign='top'><font color='#006699'><strong>Grand Total:</strong></font></td><td align='right'><strong>" & Format(SAmount, "#,###.00") & "</strong></td><td align='right'><strong>" & Format(TAmount, "#,###.00") & "</strong></td></tr>")
            sb.Append("<tr><td colspan='7' align='right' valign='top'><font color='#006699'><strong>Grand Total:</strong></font></td><td align='right'><strong>" & Format(SAmount, "#,###.00") & "</strong></td><td align='right'><strong>" & Format(TAmount, "#,###.00") & "</strong></td></tr>")
            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else

            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If


        Return sb.ToString
    End Function

    Function Get_Dental()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0


        'dtUtil = objBll.Get_Dental(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
        dtUtil = objBll.Get_Dental(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)

        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Control Code</th><th>Principal EmpID</th><th>Name</th><th>Availment Date</th><th>Diagnosis</th><th>Dentist Name</th><th>Approved</th></tr>")
            sb.Append("<tr><th>Control Code</th><th>Principal EmpID</th><th>Name</th><th>Availment Date</th><th>Dentist Name</th><th>Approved</th></tr>")
            For Each dr As DataRow In dtUtil.Rows
                If Not IsDBNull(dr("RM_CODE")) Then
                    sRMCode = Trim(dr("RM_CODE"))
                Else
                    sRMCode = "0"
                End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    sb.Append("<tr><td>" & dr("CTLNO") & "</td>")
                    sb.Append("<td></td>")
                    sb.Append("<td>" & dr("LAST_NAME") & ", " & dr("FIRST_NAME") & "</td>")
                    sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "</td>")
                    'sb.Append("<td>" & dr("DX_DESC") & "</td>")
                    sb.Append("<td>" & dr("H_NAME") & "</td>")

                    If Not IsDBNull(dr("UTIL_AMT")) Then
                        sb.Append("<td align='right' valign='top'>" & Format(dr("UTIL_AMT"), "#,###.00") & "</td>")
                    Else
                        sb.Append("<td></td>")
                    End If

                End If
                i = i + 1
            Next
            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else

            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If


        Return sb.ToString
    End Function


    Function Get_DCR()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0


        'dtUtil = objBll.Get_DCR(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
        dtUtil = objBll.Get_DCR(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)


        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Principal EmpID</th><th>Name</th><th>Availment Date</th><th>Diagnosis</th><th>Hospital</th></tr>")
            sb.Append("<tr><th>Principal EmpID</th><th>Name</th><th>Availment Date</th><th>Hospital</th></tr>")
            For Each dr As DataRow In dtUtil.Rows
                If Not IsDBNull(dr("RM_CODE")) Then
                    sRMCode = Trim(dr("RM_CODE"))
                Else
                    sRMCode = "0"
                End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    sb.Append("<tr><td></td>")
                    sb.Append("<td>" & dr("LAST_NAME") & ", " & dr("FIRST_NAME") & "</td>")
                    sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "</td>")
                    'sb.Append("<td>" & dr("DX_DESC") & "</td>")
                    sb.Append("<td>" & dr("H_NAME") & "</td></tr>")

                End If
                i = i + 1
            Next
            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else

            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If


        Return sb.ToString
    End Function


    Function Get_Call_Log()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0


        'dtUtil = objBll.Get_Call_Log(dpFrom.SelectedDate, dpTo.SelectedDate, txtName.Text)
        dtUtil = objBll.Get_Call_Log(dpFrom.SelectedDate, dpTo.SelectedDate, txtMemberCode.Text)


        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Principal EmpID</th><th>Name</th><th>Availment Date</th><th>Diagnosis</th><th>Hospital</th></tr>")
            sb.Append("<tr><th>Principal EmpID</th><th>Name</th><th>Availment Date</th><th>Hospital</th></tr>")
            For Each dr As DataRow In dtUtil.Rows
                If Not IsDBNull(dr("RM_CODE")) Then
                    sRMCode = Trim(dr("RM_CODE"))
                Else
                    sRMCode = "0"
                End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    sb.Append("<tr><td></td>")
                    sb.Append("<td>" & dr("LAST_NAME") & ", " & dr("FIRST_NAME") & "</td>")
                    sb.Append("<td>" & Format(dr("VISIT_DATE"), "MM/dd/yyyy") & "</td>")
                    'sb.Append("<td>" & dr("DX_DESC") & "</td>")
                    sb.Append("<td>" & dr("H_NAME") & "</td></tr>")

                End If
                i = i + 1
            Next

            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else
            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If


        Return sb.ToString
    End Function

    Function GetMemberUtilization()
        Dim sb As New StringBuilder
        Dim strICD10a As String = String.Empty
        Dim strICD10B As String = String.Empty
        Dim SAmount As Double = 0
        Dim TAmount As Double = 0
        Dim i As Integer = 0
        Dim dblAmount As Double = 0.0
        Dim objMemBll As New ememberBLL

        objMemBll.GetMembershipInformation(Trim(txtMemberCode.Text))

        If Trim(objMemBll.MemberCode) <> "" Then
            If Trim(objMemBll.AccountCode) <> Trim(Request.QueryString("a")) Then
                Return sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>").ToString
            End If

        End If

        objBll.Member_Code = Trim(txtMemberCode.Text)

        dtUtil = objBll.GetMemberUtilization(dpFrom.SelectedDate, dpTo.SelectedDate)

        If dtUtil.Rows.Count And sAccountPlan.Trim <> "" Then
            Dim sRMCode As String = String.Empty
            sb.Append("<table id='tblUtilization'>")
            'sb.Append("<tr><th>Control Code</th><th>Availment</th><th>Primary Diagnosis/Remarks</th><th>Other Diagnosis/Remarks</th><th>Hospital/Doctor</th><th>Approved</th></tr>")
            'sb.Append("<tr><td colspan='6'><strong>" & objMemBll.MemberName & "</strong></td></tr>")
            sb.Append("<tr><th>Control Code</th><th>Availment</th><th>Hospital/Doctor</th><th>Approved</th></tr>")
            sb.Append("<tr><td colspan='4'><strong>" & objMemBll.MemberName & "</strong></td></tr>")
            For Each dr As DataRow In dtUtil.Rows
                'If Not IsDBNull(dr("CONTROL_CODE")) Then
                '    sRMCode = Trim(dr("CONTROL_CODE"))
                'Else
                '    sRMCode = "0"
                'End If
                If UCase(sAccountPlan) = "ALL" Or InStr(1, Trim(sAccountPlan), sRMCode) Then

                    sb.Append("<tr><td>" & dr("CONTROL_CODE") & "</td>")
                    sb.Append("<td>" & Format(dr("AVAIL_FR"), "MM/dd/yyyy") & "</td>")
                    'sb.Append("<td>" & dr("DIAG_DESC") & "</td>")
                    'sb.Append("<td>" & dr("DX_REM") & "</td>")
                    sb.Append("<td>" & dr("HOSPITAL_NAME") & "</td>")
                    sb.Append("<td align='right' valign='top'>" & Format(dr("APPROVED"), "#,###.00") & "</td></tr>")
                    dblAmount = dblAmount + dr("APPROVED")
                End If
                i = i + 1
            Next
            'sb.Append("<tr><td colspan='4'></td><td align='right'><strong>Total:</strong></td><td align='right'>" & Format(dblAmount, "#,###.00") & "</td></tr>")
            sb.Append("<tr><td colspan='2'></td><td align='right'><strong>Total:</strong></td><td align='right'>" & Format(dblAmount, "#,###.00") & "</td></tr>")
            sb.Append("</table>")
            sb.Append("<br/><div>Disclaimer: This report does not include availments that were not called in for approval and bills that are on process.</div>")
        Else
            sb.Append("<div class='utilDiv'><strong>No Record Found.</strong></div>")
        End If


        Return sb.ToString
    End Function

    'Protected Sub btnRequest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRequest.Click
    '    If txtRemarks.Text.Trim <> "" Then


    '        If Request.QueryString("t").ToString = "1" Then
    '            objBllEcorp.Remarks = txtRemarks.Text
    '            objBllEcorp.Save_Utilization_Request()
    '            UtilSendEmail(objBllEcorp.Firstname & " " & objBllEcorp.Lastname, objBllEcorp.Username, ddlService.Text)
    '        Else
    '            objBll.Remarks = txtRemarks.Text
    '            objBll.Save_Utilization_Request()
    '            UtilSendEmail(objBll.Firstname & " " & objBll.Lastname, objBll.Username, ddlService.Text)
    '        End If

    '        txtRemarks.Text = ""

    '        lblMessage.Visible = True
    '        lblMessage.Text = "Your request has been submitted."
    '    Else
    '        RequiredFieldValidator4.Visible = True
    '        RequiredFieldValidator4.IsValid = False
    '        RequiredFieldValidator4.ErrorMessage = "Remarks is required!"
    '    End If

    'End Sub

    'Private Sub UtilSendEmail(ByVal sender_name As String, ByVal sender_email As String, ByVal util_type As String)

    '    Dim sb As New StringBuilder
    '    Dim message As String = String.Empty
    '    Dim from As String = "noreply@medicardphils.com"
    '    Dim subject As String = util_type & " Utilization Request "

    '    sb.Append("<div style='width: 90%; margin: 5px; border: 1px solid black; padding: 10px; font-family: sans-serif; font-size: 12px;'><div style='background-color: yellow; width: 100%; position: relative;'>")
    '    sb.Append("<p>A new utilization request has been made by " & sender_name & " : " & sender_email & "</p></div>")
    '    Dim write As String = oSanitizer.SanitizeInput(txtRemarks.Text)
    '    write = Replace(write, Environment.NewLine, "<br />")
    '    sb.Append("<p>" & write & "</p>")
    '    sb.Append("</div>")

    '    Try
    '        Mailhelper.MailHelper.SendMailMessage(from, "ctubig@medicardphils.com", "", "", subject, sb.ToString)
    '    Catch ex As Exception

    '    End Try

    'End Sub

    'Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click

    '    If chkGroup.Checked Then
    '        ExportExcel(GetUtilTableByGrp, "Utilization IP by Deseases")
    '    Else
    '        ExportExcel(GetUtilTable, "Utilization IP")
    '    End If

    'End Sub

    'Private Sub ExportExcel(ByVal str As String, Optional ByVal filename As String = "filename")
    '    Response.Clear()

    '    Response.AddHeader("content-disposition", "attachment;    filename=" & filename & ".xls")

    '    Response.Charset = ""

    '    ' If you want the option to open the Excel file without saving than

    '    ' comment out the line below

    '    ' Response.Cache.SetCacheability(HttpCacheability.NoCache);

    '    Response.ContentType = "application/vnd.xls"


    '    Dim stringWrite As TextWriter = New System.IO.StringWriter()
    '    stringWrite.Write(Str.ToString)

    '    Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

    '    Response.Write(stringWrite.ToString())

    '    Response.End()
    'End Sub

    Private Sub getMembers()

        Dim emed As New AccountInformationBLL(Request.QueryString("a"), AccountInformationProperties.AccountType.eCorporate)

        'Principal & Dependent
        ddlMember.DataSource = (From x In emed.ActiveMembersPrincipal
                                 Select New With {
                                    .FullName = x.MEM_LNAME & ", " & x.MEM_FNAME,
                                    .Code = x.PRIN_CODE
                                }).ToList().Union((From x In emed.ActiveMembersDependent
                                                             Select New With {
                                                                .FullName = x.MEM_LNAME & ", " & x.MEM_FNAME,
                                                                .Code = x.DEP_CODE
                                                            }).ToList())
        ddlMember.DataTextField = "FullName"
        ddlMember.DataValueField = "Code"
        ddlMember.DataBind()

        ' Additional
        ddlMember.Items.Insert(0, New ListItem("", ""))

    End Sub

End Class