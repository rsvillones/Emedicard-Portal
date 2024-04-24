Imports emedicardBLL
Public Class left_menu
    Inherits System.Web.UI.UserControl
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("t") Is Nothing Then
            userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
            Select Case Request.QueryString("t")
                Case 1 ' eCORPORATE
                    Dim eCorp = New eCorporateBLL(userCode, AccountInformationProperties.AccountType.eCorporate, Request.QueryString("c"))
                    With eCorp
                        lnkAccountInfo.Visible = True
                        lnkActionMemos.Visible = .Access_ActionMemos
                        lnkActiveMembers.Visible = .Access_ActiveMembers
                        lnkBenefitsInclusions.Visible = .Access_Benefits
                        lnkEndorsement.Visible = .Access_Endorsement
                        'lnkIDReplacement.Visible = .Access_ID
                        lnkOnsiteAPE.Visible = .Access_APE
                        lnkResignedMembers.Visible = .Access_ResignedMembers
                        lnkUtilization.Visible = .Access_Utilization
                        'lnkECU.Visible = .Access_ECU
                        lnkReimbStatus.Visible = IIf(IsDBNull(.Access_ReimbStatus), False, .Access_ReimbStatus)

                        'lnkMyCureAPE.Visible = True
                        lnkMyCureAPE.Visible = IIf(IsDBNull(.Access_ClinicResults), False, .Access_ClinicResults)

                    End With
                Case 2 'AGENT
                    Dim agent = New eAccountBLL(userCode)
                    With agent
                        lnkAccountInfo.Visible = True
                        lnkActionMemos.Visible = .Access_ActionMemos
                        lnkActiveMembers.Visible = .Access_ActiveMembers
                        lnkBenefitsInclusions.Visible = .Access_Benefits
                        lnkEndorsement.Visible = .Access_Endorsement
                        'lnkIDReplacement.Visible = .Access_ID
                        lnkOnsiteAPE.Visible = .Access_APE
                        lnkResignedMembers.Visible = .Access_ResignedMembers
                        lnkUtilization.Visible = .Access_Utilization
                        'lnkECU.Visible = .Access_ECU
                        lnkReimbStatus.Visible = IIf(IsDBNull(.Access_ReimbStatus), False, .Access_ReimbStatus)

                        'lnkMyCureAPE.Visible = True
                        lnkMyCureAPE.Visible = IIf(IsDBNull(.Access_ClinicResults), False, .Access_ClinicResults)

                    End With
            End Select

            'lnkSendDocuments.Visible = True

        End If
    End Sub

End Class