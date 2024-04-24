Imports emedicardBLL
Imports EncryptDecrypt.EncryptDecrypt
Public Class _Default3
    Inherits System.Web.UI.Page
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Public oEAcctBLL As eAccountBLL

    Private Sub _Default3_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Load_AgentInfo()
        Load_Accounts()
        'Response.Write(oEAcctBLL.Username & " " & oEAcctBLL.Firstname & oEAcctBLL.Lastname & " " & oEAcctBLL.EmailAddress)
    End Sub

    Private Sub Load_AgentInfo()
        oEAcctBLL = New eAccountBLL
        With oEAcctBLL
            .Username = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
            .GetAgentInfo()
        End With

    End Sub

    Private Sub Load_Accounts()
        Dim AgentCode As String = String.Empty
        Dim CorporateCode As String = String.Empty

        If Not Request.QueryString("t") Is Nothing Then
            If Not Request.QueryString("agnt") Is Nothing Then AgentCode = Request.QueryString("agnt")

            Using bll = New eAccountBLL
                bll.UserType = Request.QueryString("t")
                bll.AgentCode = AgentCode

                bll.Username = Decrypt(Request.QueryString("u"), key)
                bll.GetAgentInfo()

                If bll.AccessLevel = 2 Then
                    grdActiveAccounts.DataSource = bll.GerEMEDUsersAccount(1)
                    grdActiveAccounts.DataBind()

                    grdResignedAccounts.DataSource = bll.GerEMEDUsersAccount(2)
                    grdResignedAccounts.DataBind()
                Else
                    grdActiveAccounts.DataSource = bll.GetEMEDAgentActiveAccounts(1)
                    grdActiveAccounts.DataBind()

                    grdResignedAccounts.DataSource = bll.GetEMEDAgentActiveAccounts(2)
                    grdResignedAccounts.DataBind()
                End If

                'If bll.MainAgentID = 0 Then
                '    grdActiveAccounts.DataSource = bll.GetEMEDAgentActiveAccounts(1)
                '    grdActiveAccounts.DataBind()

                '    grdResignedAccounts.DataSource = bll.GetEMEDAgentActiveAccounts(2)
                '    grdResignedAccounts.DataBind()
                'Else

                '    grdActiveAccounts.DataSource = bll.GerEMEDUsersAccount(1)
                '    grdActiveAccounts.DataBind()

                '    grdResignedAccounts.DataSource = bll.GerEMEDUsersAccount(2)
                '    grdResignedAccounts.DataBind()
                'End If
            End Using

        End If
    End Sub
End Class