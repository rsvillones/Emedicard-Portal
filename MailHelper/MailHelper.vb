Imports System.Net.Mail

Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Configuration
Imports System.Web.Script.Serialization

Public Class MailHelper
    Private Shared lSuccess As Boolean

    Public Shared _SGAuth As String = ConfigurationManager.AppSettings("SG_Auth").ToString
    Public Shared _SGSend As String = ConfigurationManager.AppSettings("SG_Send").ToString
    Public Shared _SGUser As String = ConfigurationManager.AppSettings("SG_User").ToString
    Public Shared _SGPswd As String = ConfigurationManager.AppSettings("SG_Pswd").ToString
    Public Shared _SGHost As String = ConfigurationManager.AppSettings("SG_Host").ToString
    Public Shared _SGGUser As String = ConfigurationManager.AppSettings("SG_GUser").ToString
    Public Shared _SGGPswd As String = ConfigurationManager.AppSettings("SG_GPswd").ToString
    Public Shared _Sender As String = ConfigurationManager.AppSettings("SG_Mail").ToString

    Public Shared _cbURL As String = ConfigurationManager.AppSettings("WH_cbURL").ToString
    Public Shared _WHMail As String = ConfigurationManager.AppSettings("WH_MailLog").ToString
    Public Shared _WHHost As String = ConfigurationManager.AppSettings("WH_Host").ToString

    Public Shared ReadOnly Property Sent As Boolean
        Get
            Return lSuccess
        End Get
    End Property
    ''' <summary>
    ''' Sends an mail message
    ''' </summary>
    ''' <param name="from">Sender address</param>
    ''' <param name="recepient">Recepient address</param>
    ''' <param name="bcc">Bcc recepient</param>
    ''' <param name="cc">Cc recepient</param>
    ''' <param name="subject">Subject of mail message</param>
    ''' <param name="body">Body of mail message</param>
    Public Shared Sub SendMailMessage(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String, Optional ByVal lAttachment As List(Of Attachment) = Nothing)
        Try
            ' =====================================================
            ' SMTP
            ' =====================================================
            ' Instantiate a new instance of MailMessage
            'Dim mMailMessage As New MailMessage()
            'Dim i, iCnt As Integer

            '' Set the sender address of the mail message
            ''mMailMessage.From = New MailAddress(from)
            'mMailMessage.From = New MailAddress("no_reply@medicardphils.com")
            '' Set the recepient address of the mail message
            'mMailMessage.To.Add(New MailAddress(recepient))

            '' Check if the bcc value is nothing or an empty string
            'If Not bcc Is Nothing And bcc <> String.Empty Then
            '    ' Set the Bcc address of the mail message
            '    Dim sBCC As String() = bcc.Split(",")

            '    For i = 0 To sBCC.Length - 1
            '        mMailMessage.Bcc.Add(New MailAddress(sBCC(i).ToString))
            '    Next

            'End If

            '' Check if the cc value is nothing or an empty value
            'If Not cc Is Nothing And cc <> String.Empty Then
            '    Dim sCC As String() = cc.Split(",")

            '    For i = 0 To sCC.Length - 1
            '        ' Set the CC address of the mail message
            '        mMailMessage.CC.Add(New MailAddress(sCC(i).ToString))
            '    Next

            'End If

            'If Not lAttachment Is Nothing Then
            '    For Each lItem As Attachment In lAttachment
            '        mMailMessage.Attachments.Add(lItem)
            '    Next
            '    'iCnt = aryAttachment.Count - 1
            '    'For i = 0 To iCnt
            '    '    If FileExists(aryAttachment(i)) Then _
            '    '      mMailMessage.Attachments.Add(aryAttachment(i))
            '    'Next

            'End If

            '' Set the subject of the mail message
            'mMailMessage.Subject = subject
            '' Set the body of the mail message
            'mMailMessage.Body = body

            '' Set the format of the mail message body as HTML
            'mMailMessage.IsBodyHtml = True
            '' Set the priority of the mail message to normal
            'mMailMessage.Priority = MailPriority.Normal

            '' Instantiate a new instance of SmtpClient
            'Dim mSmtpClient As New SmtpClient()
            '' Send the mail message
            'mSmtpClient.Timeout = 500000
            'mSmtpClient.Send(mMailMessage)
            ' =====================================================


            ' =====================================================
            ' SendGrid
            ' =====================================================
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)

            SendMail(recepient, bcc, cc, subject, body, lAttachment)
            ' =====================================================

            lSuccess = True
        Catch ex As Exception
            lSuccess = False
        End Try

    End Sub

    Private Shared Function FileExists(ByVal FileFullPath As String) As Boolean
        If Trim(FileFullPath) = "" Then Return False

        Dim f As New IO.FileInfo(FileFullPath)
        Return f.Exists

    End Function

    Public Shared Function GetToken() As String

        Dim base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(_SGUser & ":" + _SGPswd))

        Dim httpWebRequest = CType(WebRequest.Create(_SGAuth), HttpWebRequest)
        httpWebRequest.ContentType = "application/x-www-form-urlencoded"
        httpWebRequest.Method = "POST"
        httpWebRequest.Accept = "application/json;charset=UTF-8"
        httpWebRequest.Host = _SGHost
        httpWebRequest.Headers.Add("Authorization", "Basic " + base64authorization)

        Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
            Dim input = "grant_type=password&username=" & _SGGUser & "&password=" & _SGGPswd

            streamWriter.Write(input)
            streamWriter.Flush()
            streamWriter.Close()
        End Using

        Dim httpResponse = CType(httpWebRequest.GetResponse(), HttpWebResponse)
        Dim response As String

        Using streamReader = New StreamReader(httpResponse.GetResponseStream())
            response = streamReader.ReadToEnd()
        End Using

        'Dim entries = response.TrimStart("{"c).TrimEnd("}"c).Replace("""", String.Empty).Split(","c)

        'For Each entry In entries

        '    If entry.Split(":"c)(0) = "access_token" Then
        '        Return entry.Split(":"c)(1)
        '    End If
        'Next

        Dim js As New JavaScriptSerializer
        Dim blogObject As SGMailToken
        blogObject = js.Deserialize(Of SGMailToken)(response)

        If Not blogObject Is Nothing Then
            Return blogObject.access_token
        End If

        Return Nothing
    End Function

    Public Shared Sub SendMail(ByVal recipient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String, Optional ByVal lAttachment As List(Of Attachment) = Nothing)
        Try

            Dim token As String = GetToken()

            Dim origRequest As HttpWebRequest = DirectCast(HttpWebRequest.Create(_SGSend), HttpWebRequest)
            origRequest.ContentType = "application/json;charset=UTF-8"
            origRequest.Host = _SGHost
            origRequest.Accept = "application/json"
            origRequest.Method = "POST"
            origRequest.Headers.Add("Authorization", "Bearer " & token)

            Dim strAtt As String = ""
            Dim strBCC As String = ""
            Dim strCC As String = ""
            If Not lAttachment Is Nothing Then
                strAtt = ""
                For Each lItem As Attachment In lAttachment
                    'mMailMessage.Attachments.Add(lItem)

                    Dim br = New BinaryReader(lItem.ContentStream)
                    Dim bytes() = br.ReadBytes(Convert.ToInt32(lItem.ContentStream.Length))
                    Dim file As String = Convert.ToBase64String(bytes)

                    If Not strAtt = "" Then
                        strAtt = strAtt & ", "
                    End If

                    strAtt = """fileName"": """ & lItem.Name & ""","
                    strAtt = strAtt & """fileBase64"": """ & file & """ "
                Next
            End If

            'CC
            If Not String.IsNullOrEmpty(cc) Then
                If cc.Contains(",") Then
                    strCC = ""
                    Dim sp As String() = cc.Split(","c)
                    Dim s As String
                    For Each s In sp
                        If Not strCC = "" Then
                            strCC = strCC & ","
                        End If
                        strCC = strCC & """" & s.Trim() & """"
                    Next
                Else
                    strCC = """" & cc & """"
                End If
            End If

            'BCC
            If Not String.IsNullOrEmpty(bcc) Then
                If bcc.Contains(",") Then
                    strBCC = ""
                    Dim sp As String() = bcc.Split(","c)
                    Dim s As String
                    For Each s In sp
                        If Not s = "ctubig@medicardphils.com" Then
                            If Not strBCC = "" Then
                                strBCC = strBCC & ","
                            End If
                            strBCC = strBCC & """" & s.Trim() & """"
                        End If
                    Next
                Else
                    strBCC = """" & bcc & """"
                End If
            End If

            'DEV BCC
            If Not strBCC = "" Then
                strBCC = strBCC & "," & """rsvilliones@medicardphils.com"""
            Else
                strBCC = """rsvilliones@medicardphils.com"""
            End If

            ' DATA
            Using streamWriter = New StreamWriter(origRequest.GetRequestStream())
                Dim input = "{ ""sender"": """ & _Sender & ""","
                input = input & """recipients"" : [ """ & recipient & """ ], "
                input = input & If(String.IsNullOrEmpty(strCC), "", """cc"" : [ " & strCC & " ], ")
                input = input & If(String.IsNullOrEmpty(strBCC), "", """bcc"" : [ " & strBCC & " ], ")
                input = input & """callbackUrl"" : """ & _cbURL & ""","
                input = input & """applicationId"" : """ & "eMedicard" & ""","
                input = input & """subject"" : """ & subject & """ , "
                input = input & """message"" : """ & body & """"
                input = input & If(lAttachment Is Nothing, "", """, ""attachments"" : [ " & strAtt & " ] ")
                input = input & " }"

                streamWriter.Write(input)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim httpResponse = CType(origRequest.GetResponse(), HttpWebResponse)
            Dim response As String

            Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                response = streamReader.ReadToEnd()
            End Using

            'Dim entries = response.TrimStart("{"c).TrimEnd("}"c).Replace("""", String.Empty).Split(","c)

            ''For Each entry In entries
            ''    If entry.Split(":"c)(0) = "message" Then
            ''        'Return If(entry.Split(":"c)(1).ToString() = "Email has been queued for sending.", True, False)
            ''        If (entry.Split(":"c)(1).ToString() = "Email has been forwarded to Sendgrid") Then
            ''            lSuccess = True
            ''        Else
            ''            lSuccess = False
            ''        End If
            ''    End If
            ''Next

            ' SAVE MAIL LOG STARTS HERE
            Dim msgID As String = ""
            'For Each entry2 In entries
            '    If entry2.Split(":"c)(0) = "messageId" Then
            '        msgID = entry2.Split(":"c)(1).ToString()
            '    End If
            'Next

            Dim js As New JavaScriptSerializer
            Dim blogObject As SGMailResponse
            blogObject = js.Deserialize(Of SGMailResponse)(response)
            If Not blogObject Is Nothing Then
                msgID = blogObject.messageId
            End If

            Dim mailLogRequest As HttpWebRequest = DirectCast(HttpWebRequest.Create(_WHMail), HttpWebRequest)
            mailLogRequest.ContentType = "application/json;charset=UTF-8"
            mailLogRequest.Host = _WHHost
            mailLogRequest.Accept = "application/json"
            mailLogRequest.Method = "POST"

            ' MAIL LOG - DATA
            Using streamWriter = New StreamWriter(mailLogRequest.GetRequestStream())
                Dim input = "{ ""messageId"": """ & msgID & ""","
                input = input & """applicationId"" : """ & "eMedicard" & ""","
                input = input & """recipients"" : [ """ & recipient & """ ], "
                input = input & If(String.IsNullOrEmpty(strCC), "", """cc"" : [ " & strCC & " ], ")
                input = input & If(String.IsNullOrEmpty(strBCC), "", """bcc"" : [ " & strBCC & " ], ")
                input = input & """subject"" : """ & subject & """ , "
                input = input & """message"" : """ & body & """"
                input = input & If(lAttachment Is Nothing, "", """, ""attachments"" : [ " & strAtt & " ] ")
                input = input & " }"

                streamWriter.Write(input)
                streamWriter.Flush()
                streamWriter.Close()
            End Using

            Dim httpResponse2 = CType(mailLogRequest.GetResponse(), HttpWebResponse)
            Dim response2 As String

            Using streamReader = New StreamReader(httpResponse2.GetResponseStream())
                response2 = streamReader.ReadToEnd()
            End Using

            'Dim entries2 = response2.TrimStart("{"c).TrimEnd("}"c).Replace("""", String.Empty).Split(","c)

            ''For Each entry3 In entries2
            ''    If entry3.Split(":"c)(0) = "MAIL LOGGED." Then
            ''        lSuccess = True
            ''    Else
            ''        lSuccess = False
            ''    End If
            ''Next

            lSuccess = True

            'lSuccess = False
        Catch ex As Exception
            lSuccess = False
        End Try
    End Sub

End Class