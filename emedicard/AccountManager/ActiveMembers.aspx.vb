Imports emedicardBLL
Imports System.IO
Imports Telerik.Web.UI
Public Class ActiveMembers
    Inherits System.Web.UI.Page
    Dim TotalPrincipal As Integer
    Dim TotalDependents As Integer
    Dim dtPrin As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim accountCode As String = String.Empty

            If Not Request.QueryString("t") Is Nothing Then
                'remove; 07-23-2021
                'accountCode = Request.QueryString("c")
                'If Request.QueryString("c") = "" Then accountCode = Request.QueryString("a")

                'apply; 07-23-2021
                accountCode = Request.QueryString("a")

                Dim emed As New AccountInformationBLL(accountCode, AccountInformationProperties.AccountType.eCorporate)

                grdActiveMembersPrin.DataSource = emed.ActiveMembersPrincipal
                'dtPrin = grdActiveMembersPrin.DataSource
                grdActiveMembersPrin.DataBind()

                grdActiveMembersDep.DataSource = emed.ActiveMembersDependent
                grdActiveMembersDep.DataBind()

            End If

        End If
    End Sub

    Private Sub LoadPrincipalMembers()
        Try
            Dim accountCode As String = String.Empty

            If Not Request.QueryString("t") Is Nothing Then
                'remove; 07-23-2021
                'accountCode = Request.QueryString("c")
                'If Request.QueryString("c") = "" Then accountCode = Request.QueryString("a")

                'apply; 07-23-2021
                accountCode = Request.QueryString("a")

                Dim emed As New AccountInformationBLL(accountCode, AccountInformationProperties.AccountType.eCorporate)

                grdActiveMembersPrin.DataSource = emed.ActiveMembersPrincipal
                grdActiveMembersPrin.DataBind()

            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub LoadDependentMembers()
        Try
            Dim accountCode As String = String.Empty
            If Not Request.QueryString("t") Is Nothing Then
                'remove; 07-23-2021
                'accountCode = Request.QueryString("c")
                'If Request.QueryString("c") = "" Then accountCode = Request.QueryString("a")

                'apply; 07-23-2021
                accountCode = Request.QueryString("a")

                Dim emed As New AccountInformationBLL(accountCode, AccountInformationProperties.AccountType.eCorporate)

                grdActiveMembersDep.DataSource = emed.ActiveMembersDependent
                grdActiveMembersDep.DataBind()

            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        'Export principal   
        If grdActiveMembersPrin.Columns.Count = 0 Then Exit Sub
        GridToExcel(grdActiveMembersPrin, "Active Members")
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


        'For intRow As Integer = 0 To grd.Rows.Count - 2
        For intRow As Integer = 0 To grd.Rows.Count - 1
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

        Response.ContentType = "application/vnd.xls"


        Dim stringWrite As TextWriter = New System.IO.StringWriter()
        stringWrite.Write(str.ToString)

        Dim htmlWrite As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(stringWrite)

        Response.Write(stringWrite.ToString())

        Response.End()
    End Sub
    Private Sub RadGridToExcel(grd As Telerik.Web.UI.RadGrid)
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

        For Each row As GridDataItem In grd.Items
            str.AppendLine("<tr>")
            For intCol As Integer = 0 To grd.Columns.Count - 1

                str.AppendLine("<td>" & row.Cells.Item(intCol).Text & "</td>")

            Next
            str.AppendLine("</tr>")
        Next
            'For intRow As Integer = 0 To grd.Items.Count - 2
            '    str.AppendLine("<tr>")
            '    For intCol As Integer = 0 To grd.Columns.Count - 1

            '        str.AppendLine("<td>" & grd.Items(intRow).Cells(intCol).Text & "</td>")

            '    Next
            '    str.AppendLine("</tr>")
            'Next
            '**********************

            str.AppendLine("</table>")


            Response.Clear()

            Response.AddHeader("content-disposition", "attachment;    filename=FileName.xls")

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


    Public Overrides Sub VerifyRenderingInServerForm(control As Control)

        ' Confirms that an HtmlForm control is rendered for the        specified ASP.NET server control at run time.

    End Sub


    Protected Sub btnExportDep_Click(sender As Object, e As EventArgs) Handles btnExportDep.Click
        If grdActiveMembersDep.Columns.Count = 0 Then Exit Sub
        GridToExcel(grdActiveMembersDep, "Active Dependents")
    End Sub

  
   
End Class