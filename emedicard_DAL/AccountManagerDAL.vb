Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq
Imports System.Transactions
Imports System.Reflection

Public Class AccountManagerDAL
    Implements IDisposable
    Private AccountCode As String
    Public Sub New()

    End Sub

    Public Sub New(ByVal AccountCode As String)
        Me.AccountCode = AccountCode
    End Sub

    Public Function SaveMemberShipEndorsementPrincipal(ByVal EndorsementPrincipal As EndorsementPrincipal)
        Try
            Using db = New emedicardEntities
                Dim qry = (From e In db.EndorsementPrincipal
                           Where e.id = EndorsementPrincipal.id
                           Select e).FirstOrDefault

                If qry Is Nothing Then
                    db.EndorsementPrincipal.AddObject(EndorsementPrincipal)
                Else
                    'UPDATE 
                End If

                If db.SaveChanges() > 0 Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function SaveMembershipEndorsementDependent(ByVal dep As EndorsementDependent)
        Try
            Using db = New emedicardEntities
                Dim qry = (From d In db.EndorsementDependents
                           Where d.id = dep.id
                           Select d).FirstOrDefault

                If qry Is Nothing Then
                    db.EndorsementDependents.AddObject(dep)
                Else

                End If

                If db.SaveChanges() > 0 Then
                    Return True
                End If

                Return False

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function SaveMembershipEndorsementOld(ByVal EndorsementPrincipal As EMC_NEW_APPLICATION_PRINCIPAL)
        Try
            Using db = New eMEDICardOldEntities
                Dim qry = (From e In db.EMC_NEW_APPLICATION_PRINCIPAL
                         Where e.id = EndorsementPrincipal.id
                         Select e).FirstOrDefault

                If qry Is Nothing Then
                    db.EMC_NEW_APPLICATION_PRINCIPAL.AddObject(EndorsementPrincipal)
                End If

                If db.SaveChanges() > 0 Then
                    Return True
                End If

                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function SaveMembershipEndorsementOld(ByVal EndorsementDependent As EMC_NEW_APPLICATION_DEPENDENT) As Boolean
        Try
            Using db = New eMEDICardOldEntities
                Dim qry = (From e In db.EMC_NEW_APPLICATION_DEPENDENT
                           Where e.id = EndorsementDependent.id
                           Select e).FirstOrDefault

                If qry Is Nothing Then db.EMC_NEW_APPLICATION_DEPENDENT.AddObject(EndorsementDependent)
                If db.SaveChanges() > 0 Then Return True
                Return False

            End Using
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub SaveMembershipCancelRequest(ByVal objRequest As emed_members_to_cancel)

        Try
            Using db = New emedicardEntities

                db.AddToemed_members_to_cancel(objRequest)
                db.SaveChanges()

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Delete_Cancelation_Request(ByVal iRequestID As Integer)
        Dim objRequest As New emed_members_to_cancel

        Try

            Using db = New emedicardEntities
                objRequest = (From p In db.emed_members_to_cancel
                              Where p.id = iRequestID
                              Select p).FirstOrDefault

                db.DeleteObject(objRequest)
                db.SaveChanges()

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetAccountsByCode(ByVal AccountCode As String)
        'BY ALLAN ALBACETE
        ' 02/06/2013
        ' GET COMPANIES BY ACCOUNT CODE
        Try
            Using db = New MembershipEntities
                Dim qry = From a In db.Accounts1
                           Where a.ACCOUNT_CODE = AccountCode And a.ACCTSTATUS_CODE = "ASC0000003"
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

    Public Function GetAccountsSOB(ByVal AccountCode As String) As List(Of SYS_SOB_INFORMATION_MTBL)
        'BY ALLAN ALBACETE
        ' 02/06/2013
        ' GET COMPANIES BY ACCOUNT CODE
        Try
            Using db = New MembershipEntities
                Dim qry = From a In db.SYS_SOB_INFORMATION_MTBL
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


    Public Function GetLLAccountsByCode(ByVal AccountCode As String, ByVal userid As Integer)
        'BY ALLAN ALBACETE
        ' 02/06/2013
        ' GET COMPANIES BY ACCOUNT CODE
        Try
            Using db = New emedicardEntities
                Dim qry = From a In db.corp_account_listing(AccountCode, userid)

                If Not qry Is Nothing Then
                    Return qry.ToList
                End If

                Return Nothing

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetAccountsByAgent(ByVal AgentCode As String) As List(Of Account)
        'BY ALLAN ALBACETE
        ' 02/06/2013
        ' GET COMPANIES BY AGENT
        Try
            Using db = New MembershipEntities
                Dim qry = From a In db.Accounts1
                          Where a.AGENT_CODE = AgentCode And a.ACCTSTATUS_CODE = "ASC0000003"
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


    Public Function GetAccountInfo(ByVal AccountCode As String) As AccountList
        ' GET COMPLETE ACCOUNTINFO FROM MEMBERSHIP DB
        Try
            Dim objAccount As New AccountList
            Using db = New MembershipEntities
                objAccount = (From p In db.AccountLists
                           Where p.ACCOUNT_CODE = AccountCode
                           Select p).FirstOrDefault

                Return objAccount
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function



    Public Function GetActiveMembersDependent() As List(Of ActiveMembersDependent)
        Try
            Using db = New MembershipEntities
                Dim qry = From m In db.GetActiveMembersDependent(AccountCode)
                          Select m

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetActiveMembersPrincipal() As List(Of ActiveMembersPrincipal)
        Try
            Using db = New MembershipEntities
                Dim qry = From m In db.GetActiveMembersPrincipal(AccountCode)
                          Select m

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetNewApplicationNum() As String
        Try

            Using dbMembership = New MembershipEntities()
                Dim qry = (From a In dbMembership.GetAPPNUM()
                          Select a).FirstOrDefault
                Return qry.last_id
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return Nothing
    End Function

    Public Function GetAccountPlans() As List(Of AccountPlans)
        Try
            Using db = New MembershipEntities
                Dim qry = From a In db.GetAccountPlans(AccountCode)
                          Select a
                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetMembersRelations() As List(Of MembersRelation)
        Try
            Using db = New MembershipEntities
                Dim qry = From r In db.MembersRelations
                          Where r.DEP_DESCRIPTION <> ""
                          Select r

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetPrincipalEndorsementList(ByVal accountCode As String) As List(Of EndorsementListing)
        Try
            Using db = New emedicardEntities
                Dim qry = From d In db.GetEndorsementListingByStatus("Principal", 0, accountCode, 16)
                          Select d

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetPrincipalEndorsement(ByVal accountCode As String) As DataTable
        Try
            Using db = New emedicardEntities
                Dim qry = From d In db.GetEndorsementListingByStatus("Principal", 0, accountCode, 16)
                          Select d

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetDependentEndorsementListAll(ByVal accountCode As String) As List(Of EndorsementListAll)
        Try
            Using db = New emedicardEntities
                Dim qry = From d In db.GetEndorsementList("Dependent", 0, accountCode)
                          Select d

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetPrincipalEndorsementListAll(ByVal accountCode As String) As List(Of EndorsementListAll)
        Try
            Using db = New emedicardEntities
                Dim qry = From d In db.GetEndorsementList("Principal", 0, accountCode)
                          Select d

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CheckPrincipal(ByVal memberCode As String, ByVal accountCode As String) As ActiveMember
        Try
            Using db = New MembershipEntities
                Dim qry = (From p In db.GetActiveMember(memberCode, accountCode)
                           Select p).FirstOrDefault

                Return qry

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CancelDependentEndorsement(ByVal ID As Integer) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From p In db.EndorsementDependents
                           Where p.id = ID
                           Select p).FirstOrDefault

                If qry IsNot Nothing Then
                    ' DELETE
                    db.DeleteObject(qry)
                    If db.SaveChanges > 0 Then Return True
                End If
                Return False

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CancelPrinicpalEndorsement(ByVal ID As Integer) As Boolean
        Try
            Using db = New emedicardEntities
                Dim objPrin As New EndorsementPrincipal
                objPrin = (From p In db.EndorsementPrincipal
                           Where p.id = ID
                           Select p).FirstOrDefault

                If objPrin IsNot Nothing Then
                    ' DELETE
                    db.DeleteObject(objPrin)

                    If db.SaveChanges > 0 Then Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetResignedMembersPrincipal(ByVal accountCode As String) As List(Of ResignedMembersPrincipal)
        Try
            Using db = New MembershipEntities
                Dim qry = From p In db.GetPrincipalResignedMembers(accountCode)
                          Select p

                Return qry.ToList

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetResignedMembersDependent(ByVal accountCode As String) As List(Of DependentResignedMembers)
        Try
            Using db = New MembershipEntities
                Dim qry = From p In db.GetDependentResignedMembers(accountCode)
                          Select p

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetMemberInfo(ByVal memcode As String, ByVal accountCode As String)
        Try
            Dim db As New MembershipEntities
            'Dim qry = From p In db.emed_get_member_info(memcode)
            '          Select p
            Dim qry As ObjectResult(Of emed_get_member_info_Result) = db.emed_get_member_info(memcode, accountCode)

            If Not qry Is Nothing Then Return qry.FirstOrDefault

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ChekMemberExistCancel(ByVal memcode As String)

        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_members_to_cancel
                          Select p
                          Where p.MemberCode = memcode And p.Status = "Pending"

                If qry.ToList.Count > 0 Then Return True

                Return False
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function ToDataTable(Of T)(ByVal varlist As IEnumerable(Of T)) As DataTable
        Dim dtReturn As New DataTable()


        ' column names
        Dim oProps As PropertyInfo() = Nothing


        If varlist Is Nothing Then
            Return dtReturn
        End If


        For Each rec As T In varlist
            ' Use reflection to get property names, to create table, Only first time, others will follow
            If oProps Is Nothing Then
                oProps = DirectCast(rec.[GetType](), Type).GetProperties()
                For Each pi As PropertyInfo In oProps
                    Dim colType As Type = pi.PropertyType


                    If (colType.IsGenericType) AndAlso (colType.GetGenericTypeDefinition() = GetType(Nullable(Of ))) Then
                        colType = colType.GetGenericArguments()(0)
                    End If


                    dtReturn.Columns.Add(New DataColumn(pi.Name, colType))
                Next
            End If


            Dim dr As DataRow = dtReturn.NewRow()


            For Each pi As PropertyInfo In oProps
                dr(pi.Name) = If(pi.GetValue(rec, Nothing) Is Nothing, DBNull.Value, pi.GetValue(rec, Nothing))
            Next


            dtReturn.Rows.Add(dr)
        Next
        Return dtReturn
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
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
