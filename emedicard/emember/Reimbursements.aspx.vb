Public Class Reimbursemts
    Inherits System.Web.UI.Page
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Private Sub Load_Record()
        Dim userCode As String
        Try
            If Request.QueryString("u").Length > 0 Then
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Using bll As New ememberBLL(userCode)
                    'bll.Date_From = dpFr.SelectedDate
                    'bll.Date_To = dpTo.SelectedDate
                    'bll.GetMemberInformation(userCode)
                    'bll.MemberCode = Request.QueryString("mc")
                    gvResult.DataSource = bll.GetReimbursementStatus
                    'grdReimOP.DataSource = bll.ReimbursementsOutPatient

                    gvResult.DataBind()
                    'grdReimOP.DataBind()
                End Using
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try
    End Sub

    'Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoad.Click
    '    Dim iYr As Double = 0
    '    Dim iDays As Double

    '    iDays = DateDiff(DateInterval.Day, CDate(dpFr.SelectedDate), CDate(dpTo.SelectedDate))

    '    iYr = iDays / 365.242
    '    If iYr > 2 Then
    '        CustomValidator1.IsValid = False
    '        CustomValidator1.ErrorMessage = "Can't load the data, please input a date range not more than 2 years."
    '    Else
    '        Load_Record()
    '    End If

    'End Sub

    Private Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender

        Load_Record()

        If gvResult.Rows.Count > 0 Then

            gvResult.UseAccessibleHeader = True

            gvResult.HeaderRow.TableSection = TableRowSection.TableHeader

            gvResult.FooterRow.TableSection = TableRowSection.TableFooter
        End If

    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim HyperLink1 As HyperLink = e.Row.FindControl("HyperLink1")
            ' If e.Row.Cells(5).Text = "Unprocessed" Or e.Row.Cells(5).Text = "Processed/Under evaluation" Then
            If e.Row.Cells(5).Text = "For Processing" Or e.Row.Cells(5).Text = "In Process" Or e.Row.Cells(5).Text = "For check preparation" _
                Or e.Row.Cells(5).Text = "For Approval" Or e.Row.Cells(5).Text = "For Revision" Or Trim(e.Row.Cells(5).Text) = "&nbsp;" Then ' Updated by alan 2016-23-05" Then
                HyperLink1.Visible = False
            Else
                HyperLink1.NavigateUrl = "ReimbDetails.aspx?ctr=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(1).Text, key)) & "&mc=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(0).Text, key)) & "&rsd=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(5).Text, key))
            End If
        End If

    End Sub
End Class