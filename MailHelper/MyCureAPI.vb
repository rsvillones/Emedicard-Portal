Imports System.Net.Mail

Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Configuration
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization

Public Class MyCureAPI

    Public Shared _MyCureAuth As String = ConfigurationManager.AppSettings("MyCure_Auth").ToString
    Public Shared _MyCureUser As String = ConfigurationManager.AppSettings("MyCure_User").ToString
    Public Shared _MyCurePswd As String = ConfigurationManager.AppSettings("MyCure_Pswd").ToString
    Public Shared _MyCureHost As String = ConfigurationManager.AppSettings("MyCure_Host").ToString

    Public Shared _MyCurePersonalDetails As String = ConfigurationManager.AppSettings("MyCure_PersonalDetails").ToString
    Public Shared _MyCureMedicalRecords As String = ConfigurationManager.AppSettings("MyCure_MedicalRecords").ToString

    Public Shared _MyCureMemberEncounters As String = ConfigurationManager.AppSettings("MyCure_MemberEncounters").ToString
    Public Shared _MyCureEncounters As String = ConfigurationManager.AppSettings("MyCure_Encounters").ToString
    Public Shared _MyCureTemplate As String = ConfigurationManager.AppSettings("MyCure_Template").ToString
    Public Shared _MyCurePMESummary As String = ConfigurationManager.AppSettings("MyCure_PMESummary").ToString

    Public Shared Function GetToken() As String

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        Dim httpWebRequest = CType(WebRequest.Create(_MyCureAuth), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "POST"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Host = _MyCureHost

        Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
            Dim input = "{ ""email"": """ & _MyCureUser & ""","
            input = input & """password"" : """ & _MyCurePswd & """ "
            input = input & " }"

            streamWriter.Write(input)
            streamWriter.Flush()
            streamWriter.Close()
        End Using

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim entries = response.TrimStart("{"c).TrimEnd("}"c).Replace("""", String.Empty).Split(","c)

        For Each entry In entries

            If entry.Split(":"c)(0) = "accessToken" Then
                Return entry.Split(":"c)(1)
            End If
        Next

        Return Nothing
    End Function

    Public Shared Function GetMyCureId(ByVal memberCode As String) As String

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        Dim token As String = GetToken()
        Dim reqURI = _MyCurePersonalDetails & "?px_archived[$exists]=false&insuranceCards.number=" & memberCode
        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Host = _MyCureHost
        httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim blogObject As MyCurePersonalDetailsResultModel
        blogObject = js.Deserialize(Of MyCurePersonalDetailsResultModel)(response)

        If Not blogObject Is Nothing Then
            Return blogObject.data(0).id
        End If

        Return Nothing
    End Function

    Public Shared Function GetMyCureEncounterId(ByVal memberCode As String, ByVal rptType As String, Optional ByVal dtFrom As DateTime = Nothing, Optional ByVal dtTo As DateTime = Nothing) As String

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        'Dim token As String = GetToken()
        Dim reqURI = _MyCureMemberEncounters & "?memberCode=" & memberCode & "&reportType=" & rptType
        If dtFrom <> Nothing And dtTo <> Nothing Then
            reqURI = _MyCureMemberEncounters & "?memberCode=" & memberCode & "&reportType=" & rptType & "&dateFrom=" & dtFrom.ToString("yyyy-MM-dd") & "&dateTo=" & dtTo.ToString("yyyy-MM-dd")
        End If

        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim blogObject As MyCurePersonalDetailsResultModel
        blogObject = js.Deserialize(Of MyCurePersonalDetailsResultModel)(response)

        If Not blogObject Is Nothing Then
            If blogObject.total <> "0" Then
                Return blogObject.data(0).id
            End If
        End If

        Return Nothing
    End Function

    Public Shared Function GetMyCurePatientId(ByVal memberCode As String, ByVal rptType As String) As String

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        'Dim token As String = GetToken()
        Dim reqURI = _MyCureMemberEncounters & "?memberCode=" & memberCode & "&reportType=" & rptType
        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim blogObject As MyCurePersonalDetailsResultModel
        blogObject = js.Deserialize(Of MyCurePersonalDetailsResultModel)(response)

        If Not blogObject Is Nothing Then
            If blogObject.total <> "0" Then
                Return blogObject.data(0).patient
            End If
        End If

        Return Nothing
    End Function

    Public Shared Function GetMyCureEncounterTemplate(ByVal templateId As String) As String

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        'Dim token As String = GetToken()
        Dim reqURI = _MyCureTemplate & "/" & templateId
        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim blogObject As MyCureTemplate
        blogObject = js.Deserialize(Of MyCureTemplate)(response)

        If Not blogObject Is Nothing Then
            Return blogObject.data(0).template
        End If

        Return Nothing
    End Function

    Public Shared Function Get_PME(ByVal encounterId As String) As MyCurePMEResultModel

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        'Dim token As String = GetToken()
        Dim reqURI = _MyCureEncounters & "/" & encounterId
        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim ListBlogObject As MyCurePMEResultModel
        ListBlogObject = js.Deserialize(Of MyCurePMEResultModel)(response)

        Return ListBlogObject
    End Function

    Public Shared Function Get_PME_Summary(ByVal accountCode As String, ByVal rptType As String, Optional ByVal dtFrom As DateTime = Nothing, Optional ByVal dtTo As DateTime = Nothing) As MyCurePMESummaryResultModel

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        rptType = rptType.Replace("SUM", "")

        'Dim token As String = GetToken()
        Dim reqURI = _MyCurePMESummary & "?companyCode=" & accountCode & "&reportType=" & rptType
        If dtFrom <> Nothing And dtTo <> Nothing Then
            reqURI = _MyCurePMESummary & "?companyCode=" & accountCode & "&reportType=" & rptType & "&dateFrom=" & dtFrom.ToString("yyyy-MM-dd") & "&dateTo=" & dtTo.ToString("yyyy-MM-dd")
        End If

        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim ListBlogObject As MyCurePMESummaryResultModel
        ListBlogObject = js.Deserialize(Of MyCurePMESummaryResultModel)(response)

        Return ListBlogObject
    End Function

    Public Shared Function Get_PME_Summary_continuation(ByVal accountCode As String, ByVal rptType As String, ByVal skip As Integer, Optional ByVal dtFrom As DateTime = Nothing, Optional ByVal dtTo As DateTime = Nothing) As MyCurePMESummaryResultModel

        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

        rptType = rptType.Replace("SUM", "")

        'Dim token As String = GetToken()
        Dim reqURI = _MyCurePMESummary & "?companyCode=" & accountCode & "&reportType=" & rptType & "&skip=" & skip.ToString()
        If dtFrom <> Nothing And dtTo <> Nothing Then
            reqURI = _MyCurePMESummary & "?companyCode=" & accountCode & "&reportType=" & rptType & "&dateFrom=" & dtFrom.ToString("yyyy-MM-dd") & "&dateTo=" & dtTo.ToString("yyyy-MM-dd") & "&skip=" & skip.ToString()
        End If

        Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
        httpWebRequest.ContentType = "application/json"
        httpWebRequest.Method = "GET"
        httpWebRequest.Accept = "application/json"
        'httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        Dim js As New JavaScriptSerializer
        Dim ListBlogObject As MyCurePMESummaryResultModel
        ListBlogObject = js.Deserialize(Of MyCurePMESummaryResultModel)(response)

        Return ListBlogObject
    End Function

    Public Shared Function GET_APE(ByVal memberCode As String, Optional ByVal dtFrom As DateTime = Nothing, Optional ByVal dtTo As DateTime = Nothing) As List(Of MyCureAPEResultModel)
        Try
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

            Dim mycureId As String = GetMyCureId(memberCode)
            Dim token As String = GetToken()

            Dim reqURI = ""
            If dtFrom = Nothing And dtTo = Nothing Then
                reqURI = _MyCureMedicalRecords & "?patient=" & mycureId & "&type=ape-report"
            Else
                reqURI = _MyCureMedicalRecords & "?type=ape-report&patient=" & mycureId & "&createdAt[$gte]=" & dtFrom.ToString("yyyy-MM-dd") & "&createdAt[$lte]=" & dtTo.ToString("yyyy-MM-dd")
            End If

            Dim httpWebRequest = CType(WebRequest.Create(reqURI), HttpWebRequest)
            httpWebRequest.ContentType = "application/json"
            httpWebRequest.Method = "GET"
            httpWebRequest.Accept = "application/json"
            'httpWebRequest.Host = _MyCureHost
            httpWebRequest.Headers.Add("Authorization", "Bearer " & token)

            Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
            Dim response As String
            Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                response = streamReader.ReadToEnd()
            End Using

            Dim js As New JavaScriptSerializer
            Dim ListBlogObject As List(Of MyCureAPEResultModel)
            ListBlogObject = js.Deserialize(Of List(Of MyCureAPEResultModel))(response)

            Return ListBlogObject

        Catch ex As Exception

        End Try

        Return Nothing
    End Function

    Public Shared Sub GETAPE(ByVal memberCode As String)

    End Sub

End Class
