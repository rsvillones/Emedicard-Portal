
Public Class emember_left_menu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If eMedicardCollection.IsCorporate Then
            lnkPayment.Visible = False

        Else
            lnkPayment.Visible = True


        End If
    End Sub

End Class