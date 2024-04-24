Imports emedicard_DAL
'BY ALLAN ALBACETE
'02/04/2013
'BUSINESS LAYER OF CORPORATE ACCOUNT
Public Class AccountInformationBLL
    Inherits AccountInformationProperties
    Implements IDisposable

    Dim acctDB As New AccountManagerDAL
    Public Sub New()

    End Sub
    Public Sub New(AccountCode As String, UserType As AccountType, Optional AgentCode As String = Nothing)
        If Not AccountCode Is Nothing And AccountCode <> "" Then
            Me.AccountCode = AccountCode
            If Not AgentCode Is Nothing Then Me.AgentCode = AgentCode
            Me.UserType = UserType
            GetAccountInfo()
        End If
        
    End Sub

    Private Sub GetAccountInfo()
        Try
            Using db = New AccountManagerDAL
                Dim acc As List(Of Account)
                Dim acc2 As AccountList
                Dim sob As List(Of SYS_SOB_INFORMATION_MTBL)

                acc2 = db.GetAccountInfo(AccountCode)
                acc = db.GetAccountsByCode(AccountCode)
                If Trim(acc2.MOTHER_CODE) <> "" Then
                    Mother_Code = acc2.MOTHER_CODE
                    sob = db.GetAccountsSOB(Mother_Code)
                Else
                    sob = db.GetAccountsSOB(AccountCode)
                End If

                For Each Val As Account In acc
                    CompanyName = Val.ACCOUNT_NAME
                    ManageAccountName = Val.ACCOUNT_NAME
                    ManageAccountCode = Val.ACCOUNT_CODE
                    ValidityDate = ""
                    EffectivityDate = ""
                Next

                For Each Val As SYS_SOB_INFORMATION_MTBL In sob
                    ValidityDate = IIf(Val.VALIDITY_DATE Is Nothing, "", Val.VALIDITY_DATE.ToString)
                    EffectivityDate = IIf(Val.EFFECTIVITY_DATE Is Nothing, "", Val.EFFECTIVITY_DATE.ToString)
                Next

                AgentName = acc2.AGENT_FNAME & " " & acc2.AGENT_LNAME
                AgentCode = acc2.AGENT_CODE
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
