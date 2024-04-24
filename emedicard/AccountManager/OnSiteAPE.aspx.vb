Imports Telerik.Web.UI
Imports emedicardBLL
Imports EncryptDecrypt

Public Class OnSiteAPE
    Inherits System.Web.UI.Page
    Dim dtBookDate As DataTable
    Dim sBookDate1 As String
    Dim sBookdate2 As String
    Dim eAcctBLL As New eAccountBLL
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Load_Province()
            Load_City()
            GetRegion()
            Load_APE_Request()
        End If
    End Sub


    Protected Sub Calendar1_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles Calendar1.DayRender

        Dim sDayName As String = e.Day.Date.DayOfWeek.ToString()

        sBookdate2 = e.Day.Date.ToString

        If sBookDate1 = "" Then
            dtBookDate = New DataTable
            eAcctBLL.Booking_Year = Year(e.Day.Date)
            eAcctBLL.Booking_Month = Year(e.Day.Date)
            dtBookDate = eAcctBLL.GetBookDates
            sBookDate1 = e.Day.Date.ToString
        Else
            If Month(CDate(sBookDate1)) <> Month(CDate(sBookdate2)) Then
                dtBookDate = New DataTable

                eAcctBLL.Booking_Year = Year(e.Day.Date)
                eAcctBLL.Booking_Month = Month(e.Day.Date)
                dtBookDate = eAcctBLL.GetBookDates

                sBookDate1 = e.Day.Date.ToString
            End If
        End If
        If dtBookDate.Rows.Count > 0 Then
            Dim bFound As Boolean = False

            For Each dr As DataRow In dtBookDate.Rows
                If CDate(dr("BookedDates")) = e.Day.Date Then
                    bFound = True
                End If
            Next

            If bFound = False Then

                If sDayName <> "Saturday" And sDayName <> "Sunday" Then
                    'e.Cell.BackColor = System.Drawing.Color.Blue
                    e.Cell.ForeColor = Drawing.Color.Blue
                Else
                    e.Day.IsSelectable = False
                End If
            Else
                e.Day.IsSelectable = False
            End If
        Else
            If sDayName <> "Saturday" And sDayName <> "Sunday" Then
                'e.Cell.BackColor = System.Drawing.Color.Blue
                e.Cell.ForeColor = Drawing.Color.Blue
            Else
                e.Day.IsSelectable = False
            End If
        End If

    End Sub


    Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Calendar1.SelectionChanged
        txtDate.Text = Calendar1.SelectedDate
    End Sub
    Private Sub Load_Province()
        With ddlProvince
            .DataSource = eAcctBLL.GetProvinces
            .DataValueField = "PROVINCE_CODE"
            .DataTextField = "PROVINCE_NAME"
            .DataBind()
        End With

    End Sub

    Private Sub Load_City()
        With ddlCity
            eAcctBLL.Province_Code = ddlProvince.SelectedValue
            ddlCity.DataSource = eAcctBLL.GetCity
            .DataValueField = "CITY_CODE"
            .DataTextField = "CITY_NAME"
            .DataBind()
        End With

    End Sub
    Private Sub GetRegion()
        eAcctBLL.Region_Code = ddlCity.SelectedValue
        txtRegion.Text = eAcctBLL.GetRegion
    End Sub
    Protected Sub ddlProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlProvince.SelectedIndexChanged
        Load_City()
        GetRegion()
    End Sub

    Protected Sub ddlCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCity.SelectedIndexChanged
        GetRegion()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If Convert.ToInt16(txtCount.Text) < 50 Then
            HeadCountValidator.IsValid = False
            Exit Sub
        End If
        With eAcctBLL
            .Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
            .AgentCode = Request.QueryString("agnt")
            .AccountCode = Request.QueryString("a")
            .UserType = Request.QueryString("t")
            .Request_Date = txtDate.Text
            .Head_Count = txtCount.Text
            .Address = txtAddress.Text
            .Province_Code = ddlProvince.SelectedValue
            .City_Code = ddlCity.SelectedValue
            .Region = txtRegion.Text
            .Save_APE_Request()
        End With
        lblMessage.Visible = True
        lblMessage.Text = "Request has been saved."

        txtDate.Text = ""
        txtCount.Text = ""
        txtAddress.Text = ""
        ddlProvince.SelectedIndex = 0
        ddlCity.SelectedIndex = 0
        txtRegion.Text = ""

        Load_APE_Request()
    End Sub

    Private Sub Load_APE_Request()

        eAcctBLL.Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u").ToString, key)
        gvResult.DataSource = eAcctBLL.GetAPERequestList(Request.QueryString("t").ToString)
        gvResult.DataBind()

    End Sub

    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand

    End Sub

    Protected Sub gvResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvResult.RowDeleting

    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim b As Button = DirectCast(e.Row.FindControl("ButtonDelete"), Button)
            b.Attributes.Add("onclick", "return ConfirmOnDelete();")
        End If
    End Sub

    Private Sub DeleteRecords(ByVal sc As StringCollection)

        For Each item As String In sc
            eAcctBLL.Delete_APE_Request(item)
        Next

    End Sub
    'Protected Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As EventArgs)

    'End Sub
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim sc As New StringCollection()
        Dim id As String = String.Empty
        For i As Integer = 0 To gvResult.Rows.Count - 1
            Dim cb As CheckBox = DirectCast(gvResult.Rows(i).Cells(8).FindControl("CheckBox1"), CheckBox)
            If cb IsNot Nothing Then
                If cb.Checked Then
                    id = gvResult.Rows(i).Cells(0).Text
                    sc.Add(id)
                End If
            End If
        Next
        DeleteRecords(sc)
        Load_APE_Request()
    End Sub
End Class