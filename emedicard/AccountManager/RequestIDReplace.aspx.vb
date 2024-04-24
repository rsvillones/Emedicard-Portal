Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Imports System.Security.Cryptography
Imports System.Net.Mail

Public Class RequestIDReplace
    Inherits System.Web.UI.Page
    Public eCorporate As New eCorporateBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim iUserID As Integer
    Dim CompanyName As String
    Dim AccountCode As String
    Dim AgentName As String
    Dim EffectivityDate As String
    Dim ValidityDate As String
    Dim userCode As String
    Dim userName As String
    Private strError As String = String.Empty
    Dim dtIDRqst As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Load_AgentInfo()
        Load_AccountInfo()
        If Not IsPostBack Then
            Load_ID_Request()
        End If
    End Sub

    Private Sub Load_AgentInfo()

        With eCorporate
            .Username = Decrypt(Request.QueryString("u"), key)
            .GetUserInfo()
            iUserID = .UserID
            userName = .Username
        End With

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        If Save_RequestID() Then
            ClearData()
            lblMessage.Visible = True
            lblMessage.Text = "Request has been saved."
        End If
        Load_ID_Request()

    End Sub

    Function Save_RequestID() As Boolean
        Dim flAttach As New List(Of Attachment)
        Dim fAttach As Attachment

        If FileUpload1.HasFile Then
            Dim arrValidTypes() As String = {"jpeg", "jpg", "png", "doc", "docx", "xls", "xlsx"}
            Dim sExtension As String = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName)
            Dim isValid As Boolean = False

            For i = 0 To arrValidTypes.Length - 1
                If sExtension = "." & arrValidTypes(i).ToString Then
                    isValid = True
                End If
            Next

            If isValid = False Then
                CustomValidator1.ErrorMessage = "Invalid file."
                CustomValidator1.IsValid = False

                Return False
                Exit Function
            End If

            Dim lSize As Long = FileUpload1.PostedFile.ContentLength

            If lSize > 1048576 Then
                lblMessage.Visible = True
                lblMessage.Text = "Attachment must no be greater than 1 mb."

                Return False
                Exit Function
            End If

            fAttach = New Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName)
            flAttach.Add(fAttach)
        End If


        With eCorporate
            .UserID = iUserID
            .Firstname = txtFName.Text
            .Lastname = txtLName.Text
            .BirthDate = diBirthDate.SelectedDate
            .AccountCode = AccountCode
            .CompanyName = CompanyName
            .PaymentMode = IIf(rbOTC.Checked, "BANK", "CREDIT CARD")
            .Status = "Pending"
            .Username = userName
            .UserID = iUserID
            Select Case Request.QueryString("t")
                Case 1 ' eCORPORATE
                    .UserTypeDesc = "CORPORATE"
                Case 2 'eAccount
                    .UserTypeDesc = "AGENT"
            End Select

            .Save_ID_Request()
            .EmailAttachment = flAttach
            If .SendIDReplacementRequest() Then
                Return True
                Exit Function
            End If
        End With

        Return True
    End Function

    Private Sub ClearData()
        txtFName.Text = ""
        txtLName.Text = ""
        diBirthDate.Text = ""
    End Sub

    Private Sub Load_AccountInfo()
        Try

            If Not Request.QueryString("t") Is Nothing Then
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Select Case Request.QueryString("t")
                    Case 1 ' eCORPORATE
                        Dim acc = New eCorporateBLL(userCode, AccountInformationProperties.AccountType.eCorporate, Request.QueryString("c"))
                        EffectivityDate = acc.EffectivityDate
                        ValidityDate = acc.ValidityDate
                        CompanyName = acc.CompanyName
                        AgentName = acc.AgentName
                        AccountCode = Request.QueryString("c")

                    Case 2 'AGENT
                        Dim acc = New eAccountBLL(userCode, Nothing, Request.QueryString("a"))
                        EffectivityDate = acc.EffectivityDate
                        ValidityDate = acc.ValidityDate
                        CompanyName = acc.CompanyName
                        AgentName = acc.AgentName
                        AccountCode = Request.QueryString("a")

                End Select

            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub

    Private Sub Load_ID_Request()
        dtIDRqst = eCorporate.GetIDRequest(iUserID)

        gvResult.DataSource = dtIDRqst
        gvResult.DataBind()
    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim b As Button = DirectCast(e.Row.FindControl("ButtonDelete"), Button)
            b.Attributes.Add("onclick", "return ConfirmOnDelete();")
        End If
    End Sub

    Private Sub DeleteRecords(ByVal sc As StringCollection)
        For Each item As String In sc
            eCorporate.Delete_ID_Request(item)
        Next
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim sc As New StringCollection()
        Dim id As String = String.Empty
        For i As Integer = 0 To gvResult.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(gvResult.Rows(i).Cells(5).FindControl("CheckBox1"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    id = gvResult.Rows(i).Cells(0).Text
                    sc.Add(id)
                End If
            End If
        Next
        DeleteRecords(sc)
        Load_ID_Request()
    End Sub


End Class