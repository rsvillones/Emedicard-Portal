Imports System.IO
Public Class Resigned
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("a") Is Nothing Then Exit Sub
            Using bll = New emedicardBLL.emedBLL(Request.QueryString("a"))
                grdMembersDep.DataSource = bll.ResignedMembersDependent
                grdMembersDep.DataBind()

                grdMembersPrin.DataSource = bll.ResignedMembersPrincipal
                grdMembersPrin.DataBind()
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub GridToExcel(grd As GridView, Optional filename As String = "filename")
        Dim str As New StringBuilder


        str.AppendLine("<table id='tblPrin' border ='1' >")

        '********* HEADER
        str.AppendLine("<tr>")

        For i As Integer = 0 To grd.Columns.Count - 1
            str.AppendLine("<td>" & grd.Columns(i).HeaderText & "</td>")
        Next
        str.AppendLine("</tr>")
        '********************

        '******** DATA


        For intRow As Integer = 0 To grd.Rows.Count - 2
            str.AppendLine("<tr>")
            For intCol As Integer = 0 To grd.Columns.Count - 1

                str.AppendLine("<td>" & grd.Rows(intRow).Cells(intCol).Text & "</td>")

            Next
            str.AppendLine("</tr>")
        Next
        '**********************

        str.AppendLine("</table>")


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

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        If grdMembersPrin.Columns.Count = 0 Then Exit Sub
        GridToExcel(grdMembersPrin, "Resigned Principals")
        
    End Sub

    Protected Sub btnExportDep_Click(sender As Object, e As EventArgs) Handles btnExportDep.Click

        If grdMembersDep.Columns.Count = 0 Then Exit Sub
        GridToExcel(grdMembersDep, "Resigned Dependents")
        
    End Sub
End Class