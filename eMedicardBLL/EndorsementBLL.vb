Imports emedicard_DAL
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq

Public Class EndorsementBLL
    Inherits EndorsementProperties
    Implements IDisposable
    Dim IsCopyToOldIntra As String = ConfigurationManager.AppSettings("CopyToOldIntra").ToString

    Public Function SaveDependent(isCorporate As Boolean) As Boolean
        Try
            Dim dep As New EndorsementDependent
            Dim depOld As New EMC_NEW_APPLICATION_DEPENDENT

            If IsExistingPrincipal Then
                '======================================================================
                ' If Existing principal was chosen
                ' check first if member exists in membership database 
                '======================================================================

                Dim db As New AccountManagerDAL()
                Dim mem As ActiveMember = db.CheckPrincipal(PrincipalMemberCode, AccountCode)
                If mem Is Nothing Then
                    ErrorMessage = "Principal does not exist."
                    Return False
                Else
                    PrincipalAppNum = mem.PRIN_APPNUM
                End If
            End If


            With dep
                .AccountCode = AccountCode

                ' GET THE PRINCIPAL APPNUM
                '=========================
                .Prin_AppNum = PrincipalAppNum
                '=========================

                ' GET NEW APPNUM 
                '================
                .AppNum = AppNum
                '================
                .Birthday = Birthdate
                .CivilStatus = CivilStatus
                .CompanyName = CompanyName

                .RequestedDate = Today
                .RequestedBy = RequestedBy
                .FirstName = Firstname
                .Gender = Sex
                .LastName = Lastname
                .UserType = IIf(isCorporate, "CORPORATE", "AGENT/BROKER")
                .MemberType = MemberType
                .Remarks = Remarks
                .MiddleInitial = MiddleInitial
                .PlanCode = PlanCode
                .RelationshipCode = RelationshipCode
                .RelationShipDetail = MemberRelationshipDetail
                .Prin_MemCode = MemberCode
                .PlanDetails = PlanDetails
                .Prin_MemCode = PrincipalMemberCode
                .PlanRNBFor = PlanRNBFor
                .UserID = UserID
                .Status = 16 ' PENDING
            End With


            With depOld
                .AccountCode = AccountCode
                .Prin_AppNum = PrincipalAppNum
                .AppNum = AppNum
                .Birthday = Birthdate
                .CivilStatus = CivilStatus
                .CompanyName = CompanyName
                .RequestedDate = Today
                .RequestedBy = RequestedBy
                .FirstName = Firstname
                .Gender = Sex
                .LastName = Lastname
                .UserType = IIf(isCorporate, "CORPORATE", "AGENT/BROKER")
                .MemberType = MemberType
                .Remarks = Remarks
                .MiddleInitial = MiddleInitial
                .PlanCode = PlanCode
                .RelationshipCode = RelationshipCode
                .RelationShipDetail = MemberRelationshipDetail
                .Prin_MemCode = PrincipalMemberCode
                .PlanDetails = PlanDetails
                .PlanRNBFor = PlanRNBFor
                .UserID = UserID
                .Status = 16 ' PENDING
            End With

            Using db = New AccountManagerDAL(AccountCode)
                If db.SaveMembershipEndorsementDependent(dep) Then
                    If IsCopyToOldIntra = 1 Then Return db.SaveMembershipEndorsementOld(depOld)
                    Return True
                End If
            End Using
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    '============================
    ' SAVE Endorsement
    ' Param: isCorporate : if true if eCorporate, false if eAccount)
    '=====================================
    Public Function Save(isCorporate As Boolean) As Boolean
        Try
            Dim prin As New EndorsementPrincipal
            Dim prinOld As New EMC_NEW_APPLICATION_PRINCIPAL ' for old intranet

            With prin

                .AccountCode = AccountCode
                .AppNum = AppNum
                .Birthday = Birthdate
                .CivilStatus = CInt(CivilStatus)
                .CompanyName = CompanyName

                .RequestedDate = Today
                .RequestedBy = RequestedBy
                .FirstName = Firstname
                .Gender = Sex
                .LastName = Lastname
                .UserType = IIf(isCorporate, "CORPORATE", "AGENT/BROKER")
                .MemberType = MemberType
                .Remarks = Remarks
                .MiddleInitial = MiddleInitial
                .PlanCode = PlanCode
                .PlanDetails = PlanDetails
                .PlanRNBFor = PlanRNBFor
                .UserID = UserID
                .Status = 16 ' PENDING

            End With


            With prinOld
                .AccountCode = AccountCode
                .AppNum = AppNum
                .Birthday = Birthdate
                .CivilStatus = CivilStatus
                .CompanyName = CompanyName
                .RequestedDate = Today
                .RequestedBy = RequestedBy
                .FirstName = Firstname
                .Gender = Sex
                .LastName = Lastname
                .UserType = IIf(isCorporate, "CORPORATE", "AGENT/BROKER")
                .MemberType = MemberType
                .Remarks = Remarks
                .MiddleInitial = MiddleInitial
                .PlanCode = PlanCode
                .PlanDetails = PlanDetails
                .PlanRNBFor = PlanRNBFor
                .UserID = UserID
                .Status = 16 ' PENDING
            End With

            Using db = New AccountManagerDAL
                If db.SaveMemberShipEndorsementPrincipal(prin) Then
                    If IsCopyToOldIntra = 1 Then Return db.SaveMembershipEndorsementOld(prinOld)
                    Return True
                End If
            End Using
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CancelPrincipalEndorsement(ID As Integer) As Boolean

        Try
            Using db = New AccountManagerDAL
                If db.CancelPrinicpalEndorsement(ID) Then
                    Me.Message = "Endorsement record for principal deleted."
                    Return True
                End If
            End Using
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Sub CheckMemberValidity(ByVal sMemCode As String, ByVal sAccountCode As String)
        Dim objPrin As New SYS_UWPRINCIPAL_ACTIVE_MTBL

        Using db = New eCorporateDAL
            objPrin = db.CheckMemberValidity(sMemCode, sAccountCode)
        End Using

        If Not IsNothing(objPrin) Then
            MemberCode = objPrin.PRIN_CODE
            Firstname = objPrin.MEM_FNAME
            MiddleInitial = objPrin.MEM_MI
            Lastname = objPrin.MEM_LNAME
        Else
            MemberCode = ""
            Firstname = ""
            MiddleInitial = ""
            Lastname = ""
        End If

    End Sub

    Public Function CancelDependentEndorsement(ID As Integer) As Boolean

        Try
            Using db = New AccountManagerDAL
                If db.CancelDependentEndorsement(ID) Then
                    Me.Message = "Endorsement record for dependent deleted."
                    Return True
                End If
            End Using
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Sub SaveMembershipCancelRequest()
        Dim oMemToCancel As New emed_members_to_cancel

        Try
            Using db = New AccountManagerDAL

                With oMemToCancel
                    .UserID = UserID
                    .AccountCode = AccountCode
                    .CompanyName = AccountName
                    .MemberCode = MemberCode
                    .AppNum = PrincipalAppNum
                    .MemberType = MemberType
                    .LastName = Lastname
                    .FirstName = Firstname
                    .Birthday = Birthdate
                    .Remarks = Remarks
                    .EffectivityDate = EffectivityDate
                    .RequestedBy = UserName
                    .RequestedDate = Now
                    .Status = "Pending"
                    .UserType = UserType
                    .MotherCode = MotherCode
                End With

                db.SaveMembershipCancelRequest(oMemToCancel)
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Delete_Cancelation_Request(ByVal iRequestID As Integer)

        Using db = New AccountManagerDAL
            db.Delete_Cancelation_Request(iRequestID)
        End Using

    End Sub

    Public Function GetPrincipalEndorsement() As DataTable
        Dim dt As New DataTable
        Using db = New AccountManagerDAL
            dt = db.GetPrincipalEndorsement(AccountCode)
        End Using

        Return dt
    End Function

#Region "SELECT"
    Public Sub GetMemberInfo()
        Dim objMemInfo As New emed_get_member_info_Result

        Try
            Using db = New AccountManagerDAL
                objMemInfo = db.GetMemberInfo(MemberCode, AccountCode)
                If Not objMemInfo Is Nothing Then
                    With objMemInfo
                        MemberCode = .member_code
                        PrincipalAppNum = .PRIN_APPNUM
                        AccountCode = .account_code
                        AccountName = .account_name
                        Firstname = .MEM_FNAME
                        Lastname = .MEM_LNAME
                        MiddleInitial = .MEM_MI
                        Birthdate = .mem_bday
                        MemberType = .mem_type
                        MotherCode = .MOTHER_CODE
                    End With
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function ChekMemberExistCancel()
        Try
            Using db = New AccountManagerDAL
                Return db.ChekMemberExistCancel(MemberCode)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCancelMemberList()
        Try
            Dim dt As New DataTable

            Using db = New ememberDAL

                dt = db.GetCancelMemberList(UserID, AccountCode, "")
                Return dt

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetMemCancelationRequest()
        Try
            Dim dt As New DataTable

            Using db = New ememberDAL

                dt = db.GetMemCancelationRequest(AccountCode)
                Return dt

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

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
