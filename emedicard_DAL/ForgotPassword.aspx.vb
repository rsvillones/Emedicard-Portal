Imports emedicardBLL
'BY ALLAN ALBACETE
' 1/30/2013
Public Class ForgotPassword
    Inherits System.Web.UI.Page
    Private username As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Select Case ddlAccType.Text
                Case "eMember"
                    btnSend.Text = "Retrieve eMember login"
                Case "eCorporate"
                    btnSend.Text = "Retrieve eCorporate login"
                Case "eAccount"
                    btnSend.Text = "Retrieve eAccount login"
            End Select

        End If
    End Sub


    Protected Sub btnGoTologin_Click(sender As Object, e As EventArgs) Handles btnGoTologin.Click
        Response.Redirect("~/Login.aspx")
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Select Case ddlAccType.Text
                Case "eMember"
                    RetrieveEmemberAccount()
                Case "eCorporate"
                    SendECorporateAccount()
                Case "eAccount"
                    SendEAccount()
            End Select


        Catch ex As Exception
            lblAlert.Text = ex.Message.ToString()
        End Try
    End Sub

    Private Sub SendECorporateAccount()
        Try
            Dim bll As New eCorporateBLL()
            bll.EmailAddress = txtEmailAddress.Text
            bll.GetUserAccount()
            bll.GetUserInfo()
            If bll.ResetPassword() Then
                If Not bll.ErrorMessage Is Nothing Then
                    lblAlert.Text = "Email account does not exist."
                    lblAlert.ForeColor = Drawing.Color.Red
                Else
                    lblAlert.Text = "Your login account was sent to your email. Thank you."
                End If
            Else
                lblAlert.ForeColor = Drawing.Color.Black
                lblAlert.Text = "An error occured. Please try again."
            End If
        Catch ex As Exception
            lblAlert.Text = "An error occured. Please try again. " & ex.Message
        End Try
        

    End Sub


    Private Sub SendEAccount()
        Dim bll As New eAccountBLL()
        Try

            bll.EmailAddress = txtEmailAddress.Text
            bll.GetAgentInfoByEmail()
            If bll.ResetPassword() Then
                If Not bll.ErrorMessage Is Nothing Then
                    lblAlert.Text = "Email account does not exist."
                    lblAlert.ForeColor = Drawing.Color.Red
                Else
                    lblAlert.Text = "Your login account was sent to your email. Thank you."
                End If
            Else
                lblAlert.ForeColor = Drawing.Color.Black
                lblAlert.Text = "An error occured. Please try again."
            End If
        Catch ex As Exception
            If Trim(bll.ErrorMessage) <> "" Then
                lblAlert.Text = bll.ErrorMessage
            Else
                lblAlert.Text = "An error occured. Please try again. " & ex.Message
            End If
        End Try


    End Sub

    Private Sub RetrieveEmemberAccount()
        Dim bll As New ememberBLL(txtEmailAddress.Text)

        bll.CheckUsername()
        'Check username if exist
        'bll.EmailAddress = txtEmailAddress.Text
        If Not bll.LoginMessage Is Nothing Then
            lblAlert.Text = "Email account does not exist."
            lblAlert.ForeColor = Drawing.Color.Red

        Else
            ' send to email

            If bll.SendLostAccount() Then
                lblAlert.ForeColor = Drawing.Color.Black
                lblAlert.Text = "Your login account was sent to your email. Thank you."
            Else
                lblAlert.Text = bll.LoginMessage
                lblAlert.ForeColor = Drawing.Color.Red
            End If

        End If
    End Sub
End Class