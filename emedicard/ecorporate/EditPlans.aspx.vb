Imports emedicardBLL
Public Class EditPlans
    Inherits System.Web.UI.Page
    Dim ecorp As eCorporateBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim sAccountPlan As String = String.Empty
    Dim pid As Long

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ecorp = New eCorporateBLL

        pid = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("pi"), key)
        ecorp.GetAccountPlan(pid)

        sAccountPlan = ecorp.Account_Plan
        ecorp.AccountCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("c"), key)
        ecorp.GetCompanyContactInfo()

        If Not IsPostBack Then
            Load_Plans()
        End If

        If Request.QueryString("sv") IsNot Nothing Then
            If Request.QueryString("sv").ToString = "1" Then
                lblMessage.Visible = True
                lblMessage.Text = "Plans has been saved."
            End If
        End If

    End Sub

    Private Sub Load_Plans()
        dtgPlans.DataSource = ecorp.GetPlanToUtilize
        dtgPlans.DataBind()
        dtgPlans.Columns(0).Visible = False
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim sPlan As String = String.Empty
        Dim isDataSave As Boolean = False
        Dim bAllPlan As Boolean = True
        For x As Integer = 0 To dtgPlans.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgPlans.Rows(x).Cells(1).FindControl("chkPlanSelect"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    If sPlan.Trim.Length > 0 Then
                        sPlan = sPlan & ", " & dtgPlans.Rows(x).Cells(0).Text.Trim
                    Else
                        sPlan = dtgPlans.Rows(x).Cells(0).Text.Trim
                    End If
                Else
                    bAllPlan = False
                End If
            End If
        Next

        If bAllPlan Then
            sPlan = "ALL"
        End If

        ecorp.Account_Plan = sPlan
        ecorp.Update_CorporateUser_Plans(pid)
        Dim c As String = String.Empty
        c = Request.QueryString("c")
        If InStr(1, HttpContext.Current.Request.Url.ToString, "sv=1") Then
            Response.Redirect("EditPlans.aspx?pi=" & HttpUtility.UrlEncode(Request.QueryString("pi")) & "&c=" & HttpUtility.UrlEncode(Request.QueryString("c")))
        Else
            Response.Redirect("EditPlans.aspx?pi=" & HttpUtility.UrlEncode(Request.QueryString("pi")) & "&c=" & HttpUtility.UrlEncode(Request.QueryString("c")) & "&sv=1")
        End If

    End Sub


    Protected Sub dtgPlans_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dtgPlans.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim cb As System.Web.UI.HtmlControls.HtmlInputCheckBox = DirectCast(e.Row.FindControl("chkPlanSelect"), System.Web.UI.HtmlControls.HtmlInputCheckBox)
            Dim cb As CheckBox = DirectCast(e.Row.FindControl("chkPlanSelect"), CheckBox)
            If sAccountPlan = "ALL" Then
                cb.Checked = True
            Else
                If InStr(1, sAccountPlan, e.Row.Cells(0).Text) Then
                    cb.Checked = True
                End If
            End If
        End If

    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkALL.CheckedChanged
        For x As Integer = 0 To dtgPlans.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(dtgPlans.Rows(x).Cells(1).FindControl("chkPlanSelect"), CheckBox)
            If cb IsNot Nothing Then
                cb.Checked = chkALL.Checked
            End If
        Next
    End Sub
End Class