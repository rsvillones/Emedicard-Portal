Public Class emedicard1
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Request.QueryString("t") Is Nothing Then
        '    If Request.QueryString("t") = 1 Then
        '        lblHead.InnerText = "eCorporate Services"
        '    ElseIf Request.QueryString("t") = 2 Then
        '        lblHead.InnerText = "eAccount Services"
        '    Else
        '        lblHead.InnerText = "eMember Services"
        '    End If
        'End If
        'lblYear.Text = Year(Today)
    End Sub

End Class