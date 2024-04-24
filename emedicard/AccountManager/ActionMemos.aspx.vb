Imports emedicardBLL
Imports System.IO

Public Class ActionMemos
    Inherits System.Web.UI.Page
    Dim objBllEcorp As New eCorporateBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request.QueryString("t")) Then
            'remove; 07-23-2021
            'If Request.QueryString("t").ToString.Trim = "1" Then
            '    objBllEcorp.AccountCode = Request.QueryString("c")
            'Else
            '    objBllEcorp.AccountCode = Request.QueryString("a")
            'End If

            'apply; 07-23-2021
            objBllEcorp.AccountCode = Request.QueryString("a")

        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click

        If ddlMemType.SelectedValue = "PRINCIPAL" Then
            divActMemos.InnerHtml = GetActionMemosPrincipal()
        Else
            divActMemos.InnerHtml = GetActionMemosDependent()
        End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click

        If ddlMemType.SelectedValue = "PRINCIPAL" Then
            ExportExcel(GetActionMemosPrincipal, "Action Memos Principal")
        Else
            ExportExcel(GetActionMemosDependent, "Action Memos Dependent")
        End If

    End Sub

    Function GetActionMemosPrincipal()
        Dim sb As New StringBuilder

        Dim dt As New DataTable

        dt = objBllEcorp.GetAction_Memos(dpFrom.SelectedDate, dpTo.SelectedDate)

        If dt.Rows.Count > 0 Then
            sb.Append("<table id='tblActMemo'><tr><th>EMP. ID</th><th>MEM. CODE</th><th>LAST NAME</th><th>FIRST NAME</th><th>MI</th><th>BIRTHDAY</th><th>AGE</th><th>SEX</th><th>STATUS</th><th>EFF. DATE</th><th>VAL. DATE</th><th>MEMO DATE</th><th>STATUS</th><th>PLAN</th><th>AREA</th><th>REMARKS</th></tr>")
            For Each dr As DataRow In dt.Rows
                sb.Append("<tr><td></td>")
                sb.Append("<td>" & dr("PRIN_CODE") & "</td>")
                sb.Append("<td>" & dr("MEM_LNAME") & "</td>")
                sb.Append("<td>" & dr("MEM_FNAME") & "</td>")
                sb.Append("<td>" & dr("MEM_MI") & "</td>")

                If Not IsDBNull(dr("MEM_BDAY")) Then
                    sb.Append("<td>" & Format(dr("MEM_BDAY"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                sb.Append("<td>" & dr("MEM_AGE") & "</td>")
                sb.Append("<td>" & dr("SEX_DESC") & "</td>")
                sb.Append("<td>" & dr("MEMCSTAT_DESC") & "</td>")

                If Not IsDBNull(dr("EFF_DATE")) Then
                    sb.Append("<td>" & Format(dr("EFF_DATE"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                If Not IsDBNull(dr("VAL_DATE")) Then
                    sb.Append("<td>" & Format(dr("VAL_DATE"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                If Not IsDBNull(dr("ACTMEM_DATE")) Then
                    sb.Append("<td>" & Format(dr("ACTMEM_DATE"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                sb.Append("<td>" & dr("REMARKS") & "</td>")
                sb.Append("<td>" & dr("PLAN_DESC") & "</td>")

                If Not IsDBNull(dr("AREA_DESC")) Then
                    sb.Append("<td>" & dr("AREA_DESC") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                If Not IsDBNull(dr("ACTMEM_REM")) Then
                    sb.Append("<td>" & dr("ACTMEM_REM") & "</td></tr>")
                Else
                    sb.Append("<td></td></tr>")
                End If

            Next
            sb.Append("</table>")
        Else
            sb.Append("<div style='width: 100%; text-align: center; color: red;'><p>No Record Found!</p></div>")
        End If

        Return sb.ToString
    End Function

    Function GetActionMemosDependent()
        Dim sb As New StringBuilder

        Dim dt As New DataTable

        dt = objBllEcorp.GetAction_Memos_Dependent(dpFrom.SelectedDate, dpTo.SelectedDate)

        If dt.Rows.Count > 0 Then
            sb.Append("<table id='tblActMemo'><tr><th>PRINCIPAL EMP ID 	</th><th>PRINCIPAL NAME</th><th>MEM. CODE</th><th>LAST NAME</th><th>FIRST NAME</th><th>MI</th><th>BIRTHDAY</th><th>AGE</th><th>SEX</th><th>STATUS</th><th>EFF. DATE</th><th>VAL. DATE</th><th>MEMO DATE</th><th>STATUS</th><th>PLAN</th><th>RELATION</th><th>AREA</th><th>REMARKS</th></tr>")
            For Each dr As DataRow In dt.Rows
                sb.Append("<tr><td></td>")
                sb.Append("<td>" & dr("PRINCIPAL") & "</td>")
                sb.Append("<td>" & dr("DEP_CODE") & "</td>")
                sb.Append("<td>" & dr("MEM_LNAME") & "</td>")
                sb.Append("<td>" & dr("MEM_FNAME") & "</td>")
                sb.Append("<td>" & dr("MEM_MI") & "</td>")

                If Not IsDBNull(dr("MEM_BDAY")) Then
                    sb.Append("<td>" & Format(dr("MEM_BDAY"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                sb.Append("<td>" & dr("MEM_AGE") & "</td>")
                sb.Append("<td>" & dr("SEX_DESC") & "</td>")
                sb.Append("<td>" & dr("MEMCSTAT_DESC") & "</td>")

                If Not IsDBNull(dr("EFF_DATE")) Then
                    sb.Append("<td>" & Format(dr("EFF_DATE"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                If Not IsDBNull(dr("VAL_DATE")) Then
                    sb.Append("<td>" & Format(dr("VAL_DATE"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                If Not IsDBNull(dr("ACTMEM_DATE")) Then
                    sb.Append("<td>" & Format(dr("ACTMEM_DATE"), "MM/dd/yyyy") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                sb.Append("<td>" & dr("REMARKS") & "</td>")
                sb.Append("<td>" & dr("PLAN_DESC") & "</td>")
                sb.Append("<td>" & dr("DEP_DESCRIPTION") & "</td>")

                If Not IsDBNull(dr("AREA_DESC")) Then
                    sb.Append("<td>" & dr("AREA_DESC") & "</td>")
                Else
                    sb.Append("<td></td>")
                End If

                If Not IsDBNull(dr("ACTMEM_REM")) Then
                    sb.Append("<td>" & dr("ACTMEM_REM") & "</td></tr>")
                Else
                    sb.Append("<td></td></tr>")
                End If

            Next
            sb.Append("</table>")
        Else
            sb.Append("<div style='width: 100%; text-align: center; color: red;'><p>No Record Found!</p></div>")
        End If

        Return sb.ToString
    End Function


    Private Sub ExportExcel(ByVal str As String, Optional ByVal filename As String = "filename")
        Response.Clear()

        Response.AddHeader("content-disposition", "attachment;    filename=" & filename & ".xls")

        Response.Charset = ""

        ' If you want the option to open the Excel file without saving than

        ' comment out the line below

        ' Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.ContentType = "application/vnd.xls"


        Dim stringWrite As TextWriter = New System.IO.StringWriter()
        stringWrite.Write(str.ToString)

        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

        Response.Write(stringWrite.ToString())

        Response.End()
    End Sub

End Class