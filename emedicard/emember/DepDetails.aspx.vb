Public Class DepDetails
    Inherits System.Web.UI.Page
    Dim memberCode As String
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Dim dTotAmount As Double = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("di").Length > 0 Then

                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("di"), key)
                Dim bll As New ememberBLL
                ' bll.GetMemberInformation(userCode)
                bll.GetMembershipInformation(userCode)
                memberCode = bll.MemberCode
                strError = bll.LoginMessage
                With bll
                    lblAge.Text = bll.Age
                    lblBirthday.Text = bll.Birthday
                    lblCivilStatus.Text = bll.CivilStatus
                    lblCompany.Text = .Company
                    lblGender.Text = .Gender
                    lblName.Text = .MemberName
                    lblAccountStatus.Text = .AccountStatus
                    lblMemberCode.Text = memberCode
                    lblEffectivityDate.Text = .EffectivityDate
                    lblDDLimit.Text = String.Format("{0:##,###,###.00}", .DDLimit)
                    lblIDRemarks.Text = .IDRemarks
                    lblMemberType.Text = .MemberType
                    lblPlan.Text = .Plan
                    lblValidityDate.Text = .Validitydate
                    If Left(.PECNonDD, 2) = "0." Then
                        lblPEC.Text = "0.00"
                    Else
                        lblPEC.Text = Format(.PECNonDD, "###.00")
                    End If

                    lblOtherRemarks.Text = .OtherRemarks

                    grdAvailment.DataSource = bll.Availments
                    grdAvailment.DataBind()

                End With

            End If
        Catch ex As Exception
            Response.Write(strError)
        End Try
    End Sub

    'Private Sub Load_Availment()
    '    Try
    '        If Request.QueryString("di").Length > 0 Then
    '            userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
    '            Using bll As New ememberBLL(userCode)
    '                grdAvailment.DataSource = bll.Availments
    '                grdAvailment.DataBind()
    '            End Using
    '        End If
    '    Catch ex As Exception
    '        Response.Write(ex.Message.ToString())
    '    End Try
    'End Sub
    Protected Sub grdAvailment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAvailment.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            dTotAmount = dTotAmount + CDbl(e.Row.Cells(5).Text())

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(4).Text = "Total: "
            e.Row.Cells(5).Text = Format(dTotAmount, "#,###.00")

        End If
    End Sub
End Class