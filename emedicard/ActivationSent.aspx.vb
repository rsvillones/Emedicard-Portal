Public Class ActivationSent
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("s") = 1 Then

                If Request.QueryString("t") = 1 Then  'TYPE 1 - eMember Account
                    CustomMessage.InnerText = "Your eMember account is now activated. Please try to <a href ='Login.aspx'>login </a> now."
                ElseIf Request.QueryString("t") = 0 Then ' TYPE 0 - Activation sent
                    CustomMessage.InnerText = "You are one step closer to to complete the registration. An email has been sent to you to activate your account. Thank you."
                End If

            End If
        End If
    End Sub

End Class