Public Class Availments
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Dim dTotAmount As Double = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Load_Record()
        Try
            If Request.QueryString("u").Length > 0 Then
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Using bll As New ememberBLL(userCode)
                    grdAvailment.DataSource = bll.Availments
                    grdAvailment.DataBind()
                End Using
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
    End Sub
    Protected Sub grdAvailment_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAvailment.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            dTotAmount = dTotAmount + CDbl(e.Row.Cells(5).Text())

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(4).Text = "Total: "
            e.Row.Cells(5).Text = Format(dTotAmount, "#,###.00")

        End If
    End Sub

    Protected Sub grdAvailment_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles grdAvailment.PreRender
        Load_Record()

        If grdAvailment.Rows.Count > 0 Then

            grdAvailment.UseAccessibleHeader = True

            ' grdAvailment.HeaderRow.TableSection = TableRowSection.TableHeader

            'grdAvailment.FooterRow.TableSection = TableRowSection.TableFooter

        End If
    End Sub
End Class