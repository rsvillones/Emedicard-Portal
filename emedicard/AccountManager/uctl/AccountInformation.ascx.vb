Imports emedicardBLL
Public Class AccountInformation
    Inherits System.Web.UI.UserControl
    Public CompanyName As String
    Dim AccountCode As String
    Dim AgentName As String
    Dim EffectivityDate As String
    Dim ValidityDate As String
    Dim userCode As String
    Dim key As String = ConfigurationManager.AppSettings("encryptionKey")
    Private strError As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try

        If Not Request.QueryString("t") Is Nothing Then
            userCode = EncryptDecrypt.EncryptDecrypt.Decrypt(Request.QueryString("u"), key)
            Select Case Request.QueryString("t")
                Case 1 ' eCORPORATE
                    'Dim acc = New eCorporateBLL(userCode, AccountInformationProperties.AccountType.eCorporate, Request.QueryString("c"))
                    ''Edited by crhis 4/13/2013
                    Dim acc = New eCorporateBLL(userCode, AccountInformationProperties.AccountType.eCorporate, Request.QueryString("a"))

                    If Trim(acc.EffectivityDate) <> "" Then
                        EffectivityDate = Format(CDate(acc.EffectivityDate), "MM/dd/yyyy")
                        ValidityDate = Format(CDate(acc.ValidityDate), "MM/dd/yyyy")
                    End If
                    CompanyName = acc.ManageAccountName
                    AgentName = acc.AgentName
                    'AccountCode = Request.QueryString("c")
                    'edited by chris 4/13/2013

                    AccountCode = Request.QueryString("a")

                Case 2 'AGENT
                    Dim acc = New eAccountBLL(userCode, Nothing, Request.QueryString("a"))
                    If Trim(acc.EffectivityDate) <> "" Then
                        EffectivityDate = Format(CDate(acc.EffectivityDate), "MM/dd/yyyy")
                        ValidityDate = Format(CDate(acc.ValidityDate), "MM/dd/yyyy")
                    End If

                    CompanyName = acc.CompanyName
                    AgentName = acc.Firstname & " " & acc.Lastname
                    AccountCode = Request.QueryString("a")

            End Select

            lblAccountCode.Text = AccountCode
            lblAgent.Text = AgentName
            lblCompany.Text = CompanyName
            lblValidity.Text = ValidityDate
            lblEffectivityDate.Text = EffectivityDate

        End If

        'Catch ex As Exception
        ' Response.Write(ex.Message & "<br />")
        'Throw New Exception(ex.Message)
        'End Try
    End Sub
End Class