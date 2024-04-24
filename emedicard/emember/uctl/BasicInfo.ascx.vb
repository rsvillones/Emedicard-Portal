Public Class BasicInfo
    Inherits System.Web.UI.UserControl
    Dim memberCode As String
    Dim userCode As String
    Dim memberType As String
    Private strError As String = String.Empty
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("u").Length > 0 Then

                userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
                Dim bll As New ememberBLL()
                bll.GetMemberInformation(userCode)

                memberCode = bll.MemberCode
                strError = bll.LoginMessage
                With bll
                    lblAge.Text = bll.Age
                    lblBirthday.Text = bll.Birthday
                    lblCivilStatus.Text = bll.CivilStatus
                    lblCompany.Text = .Company
                    If .Company.Contains("Individual") OrElse .Company.Contains("Family Account") Then eMedicardCollection.IsCorporate = False

                    lblGender.Text = .Gender
                    lblName.Text = .MemberName
                    lblAccountStatus.Text = .AccountStatus
                    lblMemberCode.Text = memberCode
                    lblEffectivityDate.Text = .EffectivityDate
                    lblDDLimit.Text = String.Format("{0:##,###,###.00}", .DDLimit)
                    lblIDRemarks.Text = .IDRemarks
                    lblMemberType.Text = .MemberType
                    memberType = .MemberType
                    lblPlan.Text = .Plan
                    lblValidityDate.Text = .Validitydate
                    If Left(.PECNonDD, 2) = "0." Then
                        lblPEC.Text = "0.00"
                    Else
                        lblPEC.Text = Format(.PECNonDD, "###.00")
                    End If
                    lblOtherRemarks.Text = .OtherRemarks
                End With

            End If
        Catch ex As Exception
            Response.Write(strError)
        End Try
    End Sub

End Class