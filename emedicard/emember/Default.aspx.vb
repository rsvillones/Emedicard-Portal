Public Class _Default1
    Inherits System.Web.UI.Page
    Dim memberCode As String
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("u").Length > 0 Then
                Dim s As String = Request.QueryString("u")
                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(s, key)
                Dim bll As New ememberBLL(userCode)
                ' bll.GetMemberInformation(userCode)
                eMedicardCollection.IsCorporate = True

                memberCode = bll.MemberCode
                strError = bll.LoginMessage
                With bll
                    lblAge.Text = bll.Age
                    lblBirthday.Text = bll.Birthday
                    lblCivilStatus.Text = bll.CivilStatus
                    lblCompany.Text = .Company
                    If .Company.Contains("INDIVIDUAL ACCOUNT") OrElse .Company.Contains("FAMILY ACCOUNT") Then eMedicardCollection.IsCorporate = False
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

                    grdDependent.DataSource = bll.DependentDetails
                    grdDependent.DataBind()
                End With

            End If
        Catch ex As Exception
            Response.Write(strError)
        End Try
        
    End Sub


End Class