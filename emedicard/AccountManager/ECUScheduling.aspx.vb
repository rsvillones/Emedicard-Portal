Imports emedicardBLL
Public Class ECUScheduling
    Inherits System.Web.UI.Page
    Dim eCorp As New eCorporateBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        eCorp.AccountCode = Request.QueryString("c")
        eCorp.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
        eCorp.GetUserID()
        If Not IsPostBack Then
            Load_ECU_Requests()
        End If
    End Sub

    Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar1.DayRender
        If e.Day.Date < Today Then
            e.Day.IsSelectable = False
            e.Cell.ForeColor = Drawing.Color.Red
        End If
    End Sub
    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Calendar1.SelectionChanged
        txtDate.Text = Calendar1.SelectedDate
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        eCorp.GetECUMemberDetails(Trim(txtMemCode.Text))
        If eCorp.Member_Code <> "" Then
            With eCorp
                .Prefered_Date = txtDate.Text
                .Member_Code = txtMemCode.Text
                .Designation = txtDesignation.Text
                .Hospital_Code = ddlHospital.SelectedValue
                .Hospital_Name = ddlHospital.SelectedItem.Text
                .Remarks = txtRemarks.Text
            End With

            eCorp.Save_ECU_Request()

            lblMessage.Visible = True
            lblMessage.Text = "Request has been saved."

            txtDate.Text = ""
            txtMemCode.Text = ""
            txtDesignation.Text = ""
            ddlHospital.SelectedIndex = -1
            txtRemarks.Text = ""

            lblVerifyMsg.Visible = False
            lblVerifyMsg.Text = ""

            Load_ECU_Requests()
        Else
            CustomValidator1.IsValid = False
            CustomValidator1.ErrorMessage = "Invalid Member!"
        End If


    End Sub

    Protected Sub btnVerify_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVerify.Click
        eCorp.GetECUMemberDetails(Trim(txtMemCode.Text))
        If eCorp.Member_Code <> "" Then
            txtDesignation.Enabled = True
            ddlHospital.Enabled = True
            btnSave.Enabled = True
            txtRemarks.Enabled = True
            CustomValidator1.IsValid = True
            CustomValidator1.ErrorMessage = ""
            lblVerifyMsg.Visible = True
            lblVerifyMsg.Text = "Member is valid."
        Else
            txtDesignation.Enabled = False
            ddlHospital.Enabled = False
            btnSave.Enabled = False
            CustomValidator1.IsValid = False
            CustomValidator1.ErrorMessage = "Member not found!"
        End If
    End Sub

    Private Sub Load_ECU_Requests()
        Me.gvResult.Columns(0).Visible = True
        Me.gvResult.Columns(1).Visible = True
        Me.gvResult.DataSource = eCorp.GetECUScheduleList
        Me.gvResult.DataBind()
        Me.gvResult.Columns(0).Visible = False
        Me.gvResult.Columns(1).Visible = False

    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim b As Button = DirectCast(e.Row.FindControl("ButtonDelete"), Button)
            b.Attributes.Add("onclick", "return ConfirmOnDelete();")
        End If
    End Sub

    Private Sub DeleteRecords(ByVal sc As StringCollection)
        Dim sParam() As String

        For Each item As String In sc

            sParam = item.Split("|")
            eCorp.Delete_ECU_Request(sParam(0), sParam(1))

        Next

    End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim sc As New StringCollection()
        Dim id As String = String.Empty
        For i As Integer = 0 To gvResult.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(gvResult.Rows(i).Cells(9).FindControl("CheckBox1"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    id = gvResult.Rows(i).Cells(1).Text & "|" & gvResult.Rows(i).Cells(3).Text
                    sc.Add(id)
                End If
            End If
        Next
        DeleteRecords(sc)
        Load_ECU_Requests()
    End Sub
End Class