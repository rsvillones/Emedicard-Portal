Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq
Imports System.Transactions

Public Class AccountManagerDAL
    Implements IDisposable

  
    Public Function GetAccountsByCode(AccountCode As String) As List(Of Account)
        'BY ALLAN ALBACETE
        ' 02/06/2013
        ' GET COMPANIES BY ACCOUNT CODE
        Try
            Using db = New MembershipEntities
                Dim qry = From a In db.Accounts1
                           Where a.ACCOUNT_CODE = AccountCode
                           Select a

                If Not qry Is Nothing Then
                    Return qry.ToList
                End If

                Return Nothing

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetAccountsByAgent(AgentCode As String) As List(Of Account)
        'BY ALLAN ALBACETE
        ' 02/06/2013
        ' GET COMPANIES BY AGENT
        Try
            Using db = New MembershipEntities
                Dim qry = From a In db.Accounts1
                          Where a.AGENT_CODE = AgentCode
                          Select a

                If Not qry Is Nothing Then
                    Return qry.ToList
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function



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
