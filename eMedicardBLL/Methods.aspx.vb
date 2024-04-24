Imports System.Web.Services

Public Class Methods
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    <WebMethod()> _
    Public Function SaveDepApplication() As DepInfo
        Dim DependentInfo As New DepInfo

        With DependentInfo
            .last_name = Page.Request.Form("d_lastname")
            .first_name = Page.Request.Form("d_firstname")
        End With

        Return DependentInfo
    End Function
End Class