Imports emedicardBLL
Imports System.IO
Public Class _Default4
    Inherits System.Web.UI.Page
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim dtRequest As DataTable

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    '    If Not IsPostBack Then
    '        GetRequestList()
    '    End If

    'End Sub

    'Private Sub GetRequestList()
    '    Try
    '        Using bll = New eCorporateBLL
    '            If Request.QueryString("t") = "1" Then
    '                bll.UserType = 1
    '            ElseIf Request.QueryString("t") = "2" Then
    '                bll.UserType = 2
    '            Else
    '                Exit Sub
    '            End If

    '            bll.AccountCode = Request.QueryString("a")
    '            dtRequest = bll.GetCorporateRequestListAll

    '            For Each dr As DataRow In dtRequest.Rows
    '                If Len(Trim(dr(1).ToString)) > 100 Then
    '                    dr(1) = Right(dr(1), 100) & "..."
    '                    dtRequest.AcceptChanges()
    '                End If
    '            Next

    '            grdRequest.DataSource = dtRequest
    '            grdRequest.DataBind()

    '        End Using
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Protected Sub grdRequest_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdRequest.RowCommand
    '    If (e.CommandName = "DownloadFile") Then
    '        ' Retrieve the row index stored in the CommandArgument property.
    '        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

    '        Dim grow As GridViewRow = grdRequest.Rows(index)

    '        Dim sFilename As String = grdRequest.Rows(index).Cells(3).Text
    '        sFilename = sFilename.Replace("&nbsp;", "")
    '        If Trim(sFilename) = "" Then
    '            Exit Sub
    '        End If

    '        Response.Clear()

    '        Response.ClearContent()
    '        Response.ClearHeaders()
    '        Response.BufferOutput = True

    '        Response.AddHeader("Content-Disposition", "attachment; filename=" + sFilename)
    '        Response.ContentType = "application/octet-stream"
    '        Response.WriteFile("F:\ECorporateFileUpload\" & sFilename)
    '        Response.End()
    '        HttpContext.Current.ApplicationInstance.CompleteRequest()

    '    ElseIf e.CommandName = "ViewDetail" Then

    '        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    '        Dim sRequestID As String = grdRequest.Rows(index).Cells(0).Text

    '        sRequestID = EncryptDecrypt.EncryptDecrypt.Encrypt(sRequestID, key)

    '        Response.Redirect("RequesDetails.aspx?t=" & Request.QueryString("t") & "&c=" & Request.QueryString("c") & "&u=" & HttpUtility.UrlEncode(Request.QueryString("u")) & "&agnt=" & Request.QueryString("agnt") & "&a=" & Request.QueryString("a") & "&dtl=" & HttpUtility.UrlEncode(sRequestID))

    '    End If
    'End Sub

    'Protected Sub grdRequest_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRequest.RowDataBound


    'End Sub
End Class