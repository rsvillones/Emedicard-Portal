﻿Imports emedicard_DAL
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
        Me.AccountCode = AccountCode
        If Not AgentCode Is Nothing Then Me.AgentCode = AgentCode
        Me.UserType = UserType
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
