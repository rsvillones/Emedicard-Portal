
Public Class MemberRegistration
    Inherits System.Web.UI.Page

    Private Sub MemberRegistration_Init(sender As Object, e As System.EventArgs) Handles Me.Init



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCheckMembership_Click(sender As Object, e As EventArgs) Handles btnCheckMembership.Click
        'Check Member If exists

        Dim member As New ememberBLL

        'Check first on emember user table
        If member.IsMemberCodeExists(txtMemberCode.Text) Then
            CustomError.InnerText = "The member is already registered"
            Exit Sub
        End If

        'Validate membership with birthdate
        If Not member.ValidateMembershipInformation(txtMemberCode.Text, rdBirthdate.SelectedDate) Then
            CustomError.InnerText = member.LoginMessage
            Exit Sub
        End If
        'Veify on membership db
        member.GetMembershipInformation(txtMemberCode.Text)

        If Not member.AccountStatus Is Nothing Then
            If Not member.AccountStatus Is Nothing Or member.AccountStatus.Contains("ACTIVE") Then

                DisableEnableFields(False)
                txtFirstname.Text = member.Firstname
                txtLastname.Text = member.Lastname
                txtCompany.Text = member.Company
                ' rdBirthdate.SelectedDate = member.Birthday
                btnCheckMembership.Enabled = False
                ValidationSummary1.Enabled = True
                hdnAccountCode.Value = member.AccountCode
                CustomError.InnerText = ""

            Else

                DisableEnableFields(True)
                btnCheckMembership.Enabled = True
                ValidationSummary1.Enabled = False
                CustomError.InnerText = "Member code does not exist"



            End If
        Else 'NO RECORD FOUND

            DisableEnableFields(True)
            btnCheckMembership.Enabled = True
            ValidationSummary1.Enabled = False
            CustomError.InnerText = "Member code does not exist"

        End If



    End Sub

    Private Sub DisableEnableFields(state As Boolean)

        txtAddress.ReadOnly = state
        txtEmailAddress.ReadOnly = state
        txtFirstname.ReadOnly = state
        txtLastname.ReadOnly = state
        txtMobile.ReadOnly = state
        txtPassword.ReadOnly = state
        txtPassword2.ReadOnly = state
        txtPhone.ReadOnly = state
        ' rdBirthdate.Enabled = state

    End Sub

    Protected Sub cusCustom_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cusCustom.ServerValidate
        If (args.Value.Length >= 6) Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub

    Protected Sub btnSignUp_Click(sender As Object, e As EventArgs) Handles btnSignUp.Click
        Dim bll As New ememberBLL


        With bll
            .AccountCode = hdnAccountCode.Value
            .Company = txtCompany.Text
            .UserCode = txtEmailAddress.Text
            .Password = txtPassword.Text
            .Firstname = txtFirstname.Text
            .Lastname = txtLastname.Text
            .Birthday = rdBirthdate.SelectedDate
            .Address = txtAddress.Text
            .Cellphone = txtMobile.Text
            .EmailAddress = txtEmailAddress.Text
            .MemberCode = txtMemberCode.Text
            If .RegisterMember() Then Response.Redirect("~/ActivationSent.aspx?s=1&t=0")

            CustomError.InnerText = "There is an error processing your registration. Please try again."

        End With

    End Sub
End Class