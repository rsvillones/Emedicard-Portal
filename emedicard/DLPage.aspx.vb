Imports System.IO

Public Class DLPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDownload.Click

        Try
            Dim requestFile As String = Request.QueryString("flname")

            If String.IsNullOrEmpty(requestFile) Then
                Throw New FileNotFoundException("File to download cannot be null or empty")
            End If

            ' Get file name from URI string in C#
            ' http://stackoverflow.com/a/1105614
            'Dim uri = New Uri("file://uranus/ECorporateFileUpload/" & requestFile)
            'Dim filename As String = Path.GetFullPath("\\uranus\ECorporateFileUpload\" & requestFile)
            Dim fileInfo = New FileInfo(ConfigurationManager.AppSettings("UploadDir") & requestFile)

            If Not fileInfo.Exists Then
                Throw New FileNotFoundException("File to download was not found", "")
            End If

            Response.ClearHeaders()
            Response.ClearContent()
            Response.ContentType = GetContentType(fileInfo.Extension)

            Response.AppendHeader("Content-Disposition", "attachment; filename=""" + fileInfo.Name + """")
            Response.AppendHeader("Content-Length", fileInfo.Length.ToString())
            Response.TransmitFile(fileInfo.FullName)
            Response.Flush()
            ' ignore exception

        Catch ex As FileNotFoundException
            Response.StatusCode = CInt(System.Net.HttpStatusCode.NotFound)
            Response.StatusDescription = ex.Message
        Catch ex As Exception
            Response.StatusCode = CInt(System.Net.HttpStatusCode.InternalServerError)
            Response.StatusDescription = String.Format("Error downloading file: {0}", ex.Message)
        End Try
        ' ignore exception

    End Sub
    Function GetContentType(ByVal sExtesion As String) As String
        Dim sType As String = String.Empty

        Select Case sExtesion
            Case ".xls"
                sType = "application/vnd.ms-excel"
            Case ".xlsx"
                sType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Case ".zip"
                sType = "application/x-zip-compressed"
            Case ".rar"
                sType = "application/octet-stream"
            Case ".pdf"
                sType = "application/pdf"
            Case ".jpeg"
                sType = "image/jpeg"
            Case ".png"
                sType = "image/png"
            Case ".gif"
                sType = "image/gif"
            Case ".csv"
                sType = "application/vnd.ms-excel"
        End Select

        Return sType
    End Function
End Class