Imports emedicard_DAL
Imports System.Security.Cryptography

Public Class emedBLL
    Inherits emedProperties
    Implements IDisposable

    Dim objEmedDAL As ememberDAL
    Dim objEconsultDAL As eConsultDAL
    Dim md5hash As MD5 = MD5.Create()
    Dim enc As New EncryptDecrypt.EncryptDecrypt
    Dim _AccountCode As String = String.Empty
    Public Sub New()

    End Sub

    Public Sub New(AccountCode As String)
        Try
            _AccountCode = AccountCode
        Catch ex As Exception

        End Try
    End Sub
#Region "eConsultation Procedures"
    Public Sub SaveConsultation(ByVal sTitle As String, ByVal sDetails As String, ByVal iDoctor As Integer, ByVal uid As Long)
        Dim isSuccess As Boolean = False
        objEmedDAL = New emedicard_DAL.ememberDAL
        Dim objConsult As New emedicard_DAL.emed_consultation

        Try
            With objConsult
                .ConsultationTitle = sTitle
                .Consultation = sDetails
                .DoctorID = iDoctor
                .CreatedDate = Now
                .IP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
                .UserID = uid
            End With

            objEmedDAL.SaveConsultation(objConsult)

            isSuccess = True
        Catch ex As Exception
            isSuccess = False
        End Try
    End Sub

    Public Function GetDoctors() As DataTable
        objEmedDAL = New emedicard_DAL.ememberDAL

        Dim dt As New DataTable
        dt = objEmedDAL.GetDoctors

        Return dt
    End Function

    Public Function GetConsultation(ByVal iuserid As Long, ByVal isdoctor As Boolean)
        objEconsultDAL = New emedicard_DAL.eConsultDAL
        Dim dt As New DataTable
        dt = objEconsultDAL.GetCosultationList(iuserid, isdoctor)

        Return dt
    End Function

    Public Sub SaveUserMessage(ByVal iConstID As Integer, ByVal sMsg As String, ByVal isDoctor As Boolean)
        objEmedDAL = New emedicard_DAL.ememberDAL
        Dim objConsultDtl As New emedicard_DAL.emed_consultation_details

        Try
            With objConsultDtl
                .ConsultationID = iConstID
                .ConsultationText = SanitizeInput(sMsg)
                .CreatedDate = Now
                .isDoctor = isDoctor
            End With
            objEmedDAL.SaveConsultationDtls(objConsultDtl)

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Function GetConsultationMsg(ByVal iconsilttid As Integer)
        objEconsultDAL = New emedicard_DAL.eConsultDAL
        Dim ds As New DataSet

        ds = objEconsultDAL.GetConsultationMsg(iconsilttid)

        Return ds
    End Function

    Public Sub MarkConversation(ByVal iConstID As Long, ByVal isDoctor As Boolean)
        Dim objConsult As New emedicard_DAL.eConsultDAL
        objConsult.MarkConversation(iConstID, isDoctor)
    End Sub
    Public Function GetDoctorInfo(ByVal iDocID As Integer)

        Dim objDocInfo As New emed_doctors_user
        Dim objBLLProp As New emedicardBLL.emedProperties

        Using objEconsultDAL = New emedicard_DAL.eConsultDAL
            objDocInfo = objEconsultDAL.GetDoctorInfo(iDocID)
            With objBLLProp
                .doctor_firstname = objDocInfo.Firstname
                .doctor_lastname = objDocInfo.Lastname
                .doctor_email = objDocInfo.Username
            End With
        End Using
        Return objBLLProp

    End Function

    Public Function GetDoctorEmail(ByVal iConsID As Long)
        Dim objDocInfo As New emed_doctors_user
        Dim objBLLProp As New emedicardBLL.emedProperties

        Using objEconsultDAL = New emedicard_DAL.eConsultDAL
            objDocInfo = objEconsultDAL.GetDoctorEmail(iConsID)
            With objBLLProp
                .doctor_firstname = objDocInfo.Firstname
                .doctor_lastname = objDocInfo.Lastname
                .doctor_email = objDocInfo.Username
            End With
        End Using
        Return objBLLProp
    End Function

    Public Function SanitizeInput(ByVal sInput As String)

        ' Encode the string input
        Dim sb As New StringBuilder(HttpUtility.HtmlEncode(sInput))
        '' Selectively allow  <b> and <i>
        'sb.Replace("&lt;b&gt;", "<b>")
        'sb.Replace("&lt;/b&gt;", "")
        'sb.Replace("&lt;i&gt;", "<i>")
        'sb.Replace("&lt;/i&gt;", "")

        Return sb.ToString

    End Function

    Public Sub GetRecepients()
        Dim dt As New DataTable

        Me.send_to_tag = ""
        Me.send_to_email = ""
        Me.cc = ""
        Me.bcc = ""
        Me.self = False

        Using dal = New ememberDAL
            dt = dal.GetRecepients(mail_description, application_type)
            For Each dr As DataRow In dt.Rows
                If Not IsNothing(dr("send_to_tag")) Then
                    Me.send_to_tag = dr("send_to_tag")
                End If
                If Not IsNothing(dr("send_to_email")) Then
                    Me.send_to_email = dr("send_to_email")
                End If
                If Not IsNothing(dr("cc")) Then
                    Me.cc = dr("cc")
                End If
                If Not IsNothing(dr("bcc")) Then
                    Me.bcc = dr("bcc")
                End If
                If Not IsNothing(dr("include_self")) Then
                    Me.self = dr("include_self")
                End If
            Next
        End Using
    End Sub

    Public Function GetCC()
        Dim dt As New DataTable
        Using dal = New ememberDAL
            dt = dal.GetEmailCC(application_type, prm_account_code, prm_agent_code, cc)
        End Using

        Return dt
    End Function
    Function GetEmail(ByVal user_name As String, ByVal user_type As String, Optional ByVal agent_code As String = Nothing, Optional ByVal acct_code As String = Nothing) As String
        Dim sEmail As String = String.Empty
        Using dal = New ememberDAL
            sEmail = dal.GetEmail(user_name, user_type, agent_code, acct_code)
        End Using

        Return sEmail
    End Function

    Function GetEmailCC(ByVal user_type As String, Optional ByVal acct_code As String = Nothing) As String
        Dim sEmail As String = String.Empty
        Using dal = New ememberDAL
            sEmail = dal.GetEmail_forcc(user_type, acct_code)
        End Using

        Return sEmail
    End Function

#End Region

#Region "Membership"
    Private _AccountPlans As List(Of AccountPlans)
    Public ReadOnly Property AccountPlans As List(Of AccountPlans)
        Get
            Dim memDAL As New AccountManagerDAL(_AccountCode)

            Return memDAL.GetAccountPlans
        End Get
    End Property

    Private _Relations As List(Of MembersRelation)
    Public ReadOnly Property MembersRelationList As List(Of MembersRelation)
        Get
            Dim memDal As New AccountManagerDAL(_AccountCode)
            Return memDal.GetMembersRelations()
        End Get
    End Property

    Private _AppNum As String
    Public ReadOnly Property AppNum As String
        Get
            Using dal = New AccountManagerDAL
                Return dal.GetNewApplicationNum
            End Using
        End Get
    End Property


    Public ReadOnly Property ResignedMembersDependent As List(Of DependentResignedMembers)
        Get
            Using dal = New AccountManagerDAL()
                Return dal.GetResignedMembersDependent(_AccountCode)
            End Using
        End Get
    End Property


    Public ReadOnly Property ResignedMembersPrincipal As List(Of ResignedMembersPrincipal)
        Get
            Using dal = New AccountManagerDAL
                Return dal.GetResignedMembersPrincipal(_AccountCode)
            End Using
        End Get
    End Property

#End Region


    Public Function EncryptPassword(password As String) As String
        Return enc.GetMd5Hash(md5hash, password)
    End Function

    Public Function IsUserNameExists() As Boolean
        Try
            Using oDAL = New ememberDAL
                Return oDAL.IsUserNameExists(NewEmailAddress)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CheckEMemberPassword() As Boolean
        Try
            Using oDAL = New ememberDAL
                Return oDAL.CheckEMemberPassword(UserName, PWord)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function ChangeUserEmail()
        Try
            Using oDAL = New ememberDAL
                Return oDAL.ChangeUserEmail(UserName, NewEmailAddress)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
