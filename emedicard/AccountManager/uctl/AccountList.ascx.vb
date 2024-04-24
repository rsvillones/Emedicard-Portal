Imports emedicardBLL
Imports emedicardBLL.AccountInformationProperties.AccountType
Imports EncryptDecrypt.EncryptDecrypt
Public Class AccountList
    Inherits System.Web.UI.UserControl
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Dim objEcopBLL As New eCorporateBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AgentCode As String = String.Empty
        Dim CorporateCode As String = String.Empty

        If Not Request.QueryString("t") Is Nothing Then
            If Not Request.QueryString("agnt") Is Nothing Then AgentCode = Request.QueryString("agnt")
            If Not Request.QueryString("c") Is Nothing Then CorporateCode = Request.QueryString("c")
            Select Case Request.QueryString("t")
                Case 1 ' eCorporate
                    'Using bll = New AccountInformationBLL(CorporateCode, eCorporate)
                    '    Dim uname As String = Decrypt(Request.QueryString("u"), key)
                    '    objEcopBLL.Username = uname
                    '    bll.UserID = objEcopBLL.fGetUserID()
                    '    grdAccountList.DataSource = bll.AccountListing
                    '    grdAccountList.DataBind()
                    'End Using

                    Using bll = New eCorporateBLL

                        bll.Username = Decrypt(Request.QueryString("u"), key)
                        bll.GetUserInfo()

                        grdAccountList.DataSource = bll.GetCorporateUserAccount()
                        grdAccountList.DataBind()

                    End Using

                Case 2 ' eAccount

                    'Using bll = New AccountInformationBLL()
                    '    bll.UserType = Request.QueryString("t")
                    '    bll.AgentCode = AgentCode

                    '    grdAccountList.DataSource = bll.AccountListing
                    '    grdAccountList.DataBind()
                    'End Using

                    Using bll = New eAccountBLL
                        bll.UserType = Request.QueryString("t")
                        bll.AgentCode = AgentCode

                        bll.Username = Decrypt(Request.QueryString("u"), key)
                        bll.GetAgentInfo()

                        If bll.AccessLevel = 2 Then
                            grdAccountList.DataSource = bll.GerEMEDUsersAccount
                            grdAccountList.DataBind()
                        Else
                            grdAccountList.DataSource = bll.GetEMEDAgentActiveAccounts
                            grdAccountList.DataBind()
                        End If

                        'If bll.MainAgentID = 0 Then
                        '    grdAccountList.DataSource = bll.GetEMEDAgentActiveAccounts
                        '    grdAccountList.DataBind()
                        'Else
                        '    grdAccountList.DataSource = bll.GerEMEDUsersAccount
                        '    grdAccountList.DataBind()
                        'End If
                    End Using
            End Select
        End If
    End Sub



End Class