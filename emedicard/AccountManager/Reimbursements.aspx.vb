Imports emedicardBLL
Imports System.IO

Public Class Reimbursements
    Inherits System.Web.UI.Page
    Dim objBll As New eAccountBLL
    Dim objBllEcorp As New eCorporateBLL
    Dim dtReimb As New DataTable
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim sAccountPlan As String
    Public Shared oSanitizer As New emedBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            If Request.QueryString("t").ToString = "1" Then
                objBllEcorp.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
                objBllEcorp.GetUserInfo()
                objBllEcorp.AccountCode = Request.QueryString("a")
                objBllEcorp.GetAccountPlan()
                sAccountPlan = objBllEcorp.Account_Plan
                objBll.AccountCode = Request.QueryString("a")
            Else
                objBll.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
                objBll.GetAgentInfo()
                objBll.AccountCode = Request.QueryString("a")
                sAccountPlan = "ALL"
            End If

            getMembers()
        Else
            gvResult.Visible = True
        End If

    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnView.Click
        Load_Record()

    End Sub

    Private Sub Load_Record()
        Try
            If txtMemberCode.Text.Length > 0 Then
                Using bll As New ememberBLL()
                    gvResult.DataSource = bll.GetReimbursementStatusCorp(txtMemberCode.Text, Request.QueryString("a"), dpFrom.SelectedDate, dpTo.SelectedDate)
                    gvResult.DataBind()
                End Using

                If gvResult.Rows.Count <= 0 Then
                    Dim dt As DataTable = New DataTable
                    dt.Columns.Add("member_code")
                    dt.Columns.Add("control_code")
                    dt.Columns.Add("received_date")
                    dt.Columns.Add("due_date")
                    dt.Columns.Add("visit_date")
                    dt.Columns.Add("reim_status")
                    dt.Columns.Add("")
                    dt.Rows.Add(dt.NewRow())
                    gvResult.DataSource = dt
                    gvResult.DataBind()
                End If
            Else
                gvResult.DataSource = New DataTable
                gvResult.DataBind()
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try
    End Sub

    Private Sub getMembers()

        Dim emed As New AccountInformationBLL(Request.QueryString("a"), AccountInformationProperties.AccountType.eCorporate)

        'Principal & Dependent
        ddlMember.DataSource = (From x In emed.ActiveMembersPrincipal
                                 Select New With {
                                    .FullName = x.MEM_LNAME & ", " & x.MEM_FNAME,
                                    .Code = x.PRIN_CODE
                                }).ToList().Union((From x In emed.ActiveMembersDependent
                                                             Select New With {
                                                                .FullName = x.MEM_LNAME & ", " & x.MEM_FNAME,
                                                                .Code = x.DEP_CODE
                                                            }).ToList())
        ddlMember.DataTextField = "FullName"
        ddlMember.DataValueField = "Code"
        ddlMember.DataBind()

        ' Additional
        ddlMember.Items.Insert(0, New ListItem("", ""))

    End Sub

    Private Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        'Load_Record()

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
                Or e.Row.Cells(5).Text = "For Approval" Or e.Row.Cells(5).Text = "For Revision" Or Trim(e.Row.Cells(5).Text) = "" Or Trim(e.Row.Cells(5).Text) = "&nbsp;" Then ' Updated by alan 2016-23-05" Then
                HyperLink1.Visible = False
            Else
                'HyperLink1.AccessKey = "ReimbDetails.aspx?ctr=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(1).Text, key)) & "&mc=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(0).Text, key)) & "&rsd=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(5).Text, key))
                HyperLink1.NavigateUrl = "ReimbDetails.aspx?ctr=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(1).Text, key)) & "&mc=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(0).Text, key)) & "&rsd=" & HttpUtility.UrlEncode(EncryptDecrypt.EncryptDecrypt.Encrypt(e.Row.Cells(5).Text, key))
            End If

        ElseIf e.Row.RowType = DataControlRowType.EmptyDataRow Then
            e.Row.CssClass = "no-class"
        End If

    End Sub

End Class