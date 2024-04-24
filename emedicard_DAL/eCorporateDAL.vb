Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq
Imports System.Transactions
Imports System.Reflection

Public Class eCorporateDAL
    Implements IDisposable

#Region "Constructor"
    Public Sub New()

    End Sub
#End Region

#Region "SELECT"

    Public Function GetUserAccount(ByVal username As String) As List(Of emed_corporate_users)
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.Username = username)

                If Not qry Is Nothing Then
                    Return qry.ToList()
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetUserAccount(ByVal userID As Integer) As List(Of emed_corporate_users)
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.UserID = userID
                           Select u)

                If Not qry Is Nothing Then
                    Return qry.ToList()
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CheckUsernameExistence(ByVal userID As Integer, ByVal email As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.UserID = userID And u.EmailAddress = email
                           Select u).FirstOrDefault

                If qry IsNot Nothing Then Return True 'Username exist
                Return False



            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetUserAccountByEmail(ByVal Email As String) As List(Of emed_corporate_users)
        Try
            Using db = New emedicardEntities
                Dim qry = From u In db.emed_corporate_users
                           Where u.EmailAddress = Email
                           Select u

                If Not qry Is Nothing Then
                    Return qry.ToList()
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CheckLogin(ByVal username As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                            Where u.Username = username
                            Select u).FirstOrDefault

                ' User record exists
                If Not qry Is Nothing Then Return True

                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CheckLogin(ByVal username As String, ByVal password As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                            Where u.Username = username And u.Pword = password
                            Select u).FirstOrDefault

                ' User record exists
                If Not qry Is Nothing Then Return True

                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function LoginUser(ByVal username As String, ByVal password As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                            Where u.Username = username And u.Pword = password And Not u.DateUserActivation Is Nothing And u.IsActive = True
                            Select u).FirstOrDefault

                ' User record exists
                If Not qry Is Nothing Then Return True
                Return False
            End Using
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetCorporateAdminUser(ByVal accountCode As String) As emed_corporate_users
        Try
            'BY ALLAN ALBACETE 04/08/2013
            'Get admin user in mother company
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                          Where u.RegAccountCode = accountCode And u.AccessLevel = 1
                          Select u).FirstOrDefault

                Return qry
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function GetCompanyContactInfo(ByVal accountCode As String) As Account
        Try
            'BY ALLAN ALBACETE 04/08/2013
            'Get admin user in mother company
            Using db = New MembershipEntities
                Dim qry = (From u In db.Accounts1
                          Where u.ACCOUNT_CODE = accountCode
                          Select u).FirstOrDefault

                Return qry
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function GetCorporateUsers(ByVal accountCode As String) As List(Of emed_corporate_users)
        Try
            'BY ALLAN ALBACETE 02/11/2013
            'Get All users in mother company
            Using db = New emedicardEntities
                Dim qry = From u In db.emed_corporate_users
                          Where u.RegMotherCode = accountCode
                          Select u

                If Not qry Is Nothing Then
                    Return qry.ToList
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCompany(ByVal accountCode As String) As List(Of Account)
        Dim objAcc As New Account
        Try
            Using db = New MembershipEntities
                objAcc = (From p In db.Accounts1
                          Where p.ACCOUNT_CODE = accountCode
                          Select p).FirstOrDefault
                If objAcc.ACCT_CATEGORY.Trim = "MOTHER" Then
                    Dim qry = From c In db.Accounts1
                              Where c.ACCOUNT_CODE = accountCode Or c.MOTHER_CODE = accountCode
                              Select c

                    If Not qry Is Nothing Then Return qry.ToList
                Else
                    If objAcc.MOTHER_CODE.Trim <> "" Then
                        Dim qry = From c In db.Accounts1
                                  Where c.MOTHER_CODE = objAcc.MOTHER_CODE.Trim
                                  Select c

                        If Not qry Is Nothing Then Return qry.ToList
                    Else
                        Dim qry = From c In db.Accounts1
                                  Where c.ACCOUNT_CODE = accountCode
                                  Select c

                        If Not qry Is Nothing Then Return qry.ToList
                    End If

                End If


                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCorporateInfo(ByVal accountCode As String) As Account
        Try
            Using db = New MembershipEntities
                Dim qry = (From c In db.Accounts1
                          Where c.ACCOUNT_CODE = accountCode
                          Select c).FirstOrDefault

                If Not qry Is Nothing Then Return qry

                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCorporateUserInfo(ByVal username As String) As emed_corporate_users
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.Username = username
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then Return qry
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCorporateUserInfo(ByVal userID As Integer) As emed_corporate_users
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.UserID = userID
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then Return qry
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCorporateParentUser(ByVal accountCode As String) As emed_corporate_users
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                            Where u.RegMotherCode = accountCode And u.AccessLevel = 1
                            Select u).FirstOrDefault

                If Not qry Is Nothing Then Return qry
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetIDRequestList(ByVal recid As Long)
        Try
            Using db = New emedicardEntities
                Dim objRequest As ObjectResult(Of emed_get_id_request_Result) = db.emed_get_id_request(recid)

                Return ToDataTable(objRequest.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetECUScheduleList(ByVal accountCode As String)
        Try
            Using db = New emedicardEntities
                Dim objRequest As ObjectResult(Of emed_ecu_schedule_list_Result) = db.emed_ecu_schedule_list(accountCode)

                Return ToDataTable(objRequest.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetAction_Memos(ByVal accountCode As String, ByVal sDateFr As String, ByVal sDateTo As String)
        Using db = New MembershipEntities
            Dim qry = db.get_action_memos(accountCode, sDateFr, sDateTo)

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function GetAction_Memos_Dependent(ByVal accountCode As String, ByVal sDateFr As String, ByVal sDateTo As String)
        Using db = New MembershipEntities
            Dim qry = db.action_memos_dependent(accountCode, sDateFr, sDateTo)

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function GetCorporateAccountToManage(ByVal accountCode As String)
        Dim mother_code As String = String.Empty
        Try


            Using db = New MembershipEntities

                Dim qry = db.emed_corporate_account_list(accountCode)

                If Not qry Is Nothing Then Return qry.ToList()

                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCorporateUserAccount(ByVal userid As Integer)
        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_corporate_accounts
                          Where p.UserID = userid
                          Select p.AccountName, p.AccountCode, p.AccountCategory, p.id

                If Not qry Is Nothing Then Return qry.ToList()

                Return Nothing

            End Using
        Catch ex As Exception
            Throw New Exception(ex.ToString)
        End Try
    End Function

    Public Function GetCorporateUserAvailabeAccounts(ByVal account_code As String, ByVal userid As Integer)

        Try
            Using db = New MembershipEntities
                Dim qry = db.emed_corp_user_available_acc(account_code, userid)

                If Not qry Is Nothing Then Return qry.ToList()
                'Return qry.ToList()
                Return Nothing
            End Using
        Catch ex As Exception
            Throw (New Exception(ex.ToString))
        End Try
    End Function

    Public Function GetAccountPlanEcorp(ByVal accountCode As String, ByVal userid As String)
        Dim objAgntAcct As New emed_corporate_accounts

        Using db = New emedicardEntities
            objAgntAcct = (From p In db.emed_corporate_accounts
                          Where p.AccountCode = accountCode And p.UserID = userid
                          Select p).FirstOrDefault

            Return objAgntAcct
        End Using

    End Function

    Public Function GetAccountPlanEcorp(ByVal id As Long)
        Dim objAgntAcct As New emed_corporate_accounts

        Using db = New emedicardEntities
            objAgntAcct = (From p In db.emed_corporate_accounts
                          Where p.id = id
                          Select p).FirstOrDefault

            Return objAgntAcct
        End Using

    End Function

    Public Function GetPlanToUtilize(ByVal accountCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.emed_GetAccountPlan(accountCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsAllowedToUpload(ByVal accountcode As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim objAccount As New emed_corporate_accounts
                objAccount = (From p In db.emed_corporate_accounts
                             Where p.AccountCode = accountcode
                             Select p).FirstOrDefault

                Return objAccount.allow_upload
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCorporateRequestList(ByVal sAccountCode As String, ByVal igroup As Short) As DataTable
        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_corporate_users_request_list(sAccountCode, igroup)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCorporateRequestListAll(ByVal sAccountCode As String, ByVal igroup As Short, ByVal itype As Short) As DataTable
        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_corporate_request_list(sAccountCode, igroup, itype)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetRequestDetails(ByVal mother_id As String, ByVal sAcctCode As String) As DataTable
        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_corporate_users_requests
                          Where (p.rec_id = mother_id Or p.mother_id = mother_id) And p.account_code = sAcctCode
                          Select p
                          Order By p.rec_id Descending

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetTransactionStatus(ByVal iID As Integer) As Boolean
        Dim iStatus As Boolean = 1

        Try
            Using db = New emedicardEntities
                Dim request As New emed_corporate_users_requests

                request = (From p In db.emed_corporate_users_requests
                          Where p.rec_id = iID
                          Select p).FirstOrDefault

                iStatus = request.is_closed
            End Using
        Catch ex As Exception
            Throw
        End Try

        Return iStatus
    End Function

    Public Function GetAccountContactInfo(ByVal acct_code As String)
        Dim users As New emed_corporate_users
        Try
            Using db = New emedicardEntities


                users = (From p In db.emed_corporate_users
                          Where p.RegAccountCode = acct_code And p.AccessLevel = 1
                          Select p).FirstOrDefault

            End Using
        Catch ex As Exception
            Throw
        End Try
        Return users
    End Function

    Public Sub UpdateRequestViews(ByVal mother_id As String, ByVal iType As Short)
        Try
            Using db = New emedicardEntities
                db.emed_update_request_views(mother_id, iType)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetUserFullName(ByVal sUsername As String, ByVal iType As Short)
        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_get_user_fullname(sUsername, iType)
                          Select p

                Return qry(0).ToString
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetEmailFromRequest(ByVal sUploadedBy As String, ByVal irecid As Long) As DataTable

        Try
            Using db = New emedicardEntities

                Dim qry = From p In db.emed_corporate_users_requests Where p.uploaded_by <> sUploadedBy And (p.rec_id = irecid Or p.mother_id = irecid) Group By p.uploaded_by Into Group
                'Dim queryResults = Query.ToList()
                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "CRUD"

    Public Function ResetPassword(ByVal username As String, ByVal newpasssword As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry As emed_corporate_users = (From u In db.emed_corporate_users
                           Where u.Username = username).FirstOrDefault

                If Not qry Is Nothing Then
                    'UPDATE
                    qry.Pword = newpasssword
                End If

                Dim result As Integer = db.SaveChanges()
                Return result
            End Using
            Return Nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function UpdateProfile(ByVal user As emed_corporate_users) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.Username = user.Username
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    qry.Designation = user.Designation
                    qry.EmailAddress = user.EmailAddress
                    qry.FaxNo = user.FaxNo
                    qry.FirstName = user.FirstName
                    qry.LastName = user.LastName
                    qry.MobileNo = user.MobileNo
                    qry.TelNo = user.TelNo
                    qry.Username = user.Username
                    If user.Pword <> "" Then qry.Pword = user.Pword
                    If db.SaveChanges() > 0 Then Return True
                    Return False
                End If
                Return False
            End Using
        Catch ex As Exception

        End Try
    End Function
    Public Function UpdateUserInfo(ByVal user As emed_corporate_users) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_corporate_users
                           Where u.UserID = user.UserID
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    'Update
                    qry.Access_ActionMemos = user.Access_ActionMemos
                    qry.Access_ActiveMembers = user.Access_ActiveMembers
                    qry.Access_APE = user.Access_APE
                    qry.Access_Benefits = user.Access_Benefits
                    qry.Access_ECU = user.Access_ECU
                    qry.Access_Endorsement = user.Access_Endorsement
                    qry.Access_ID = user.Access_ID
                    qry.Access_ResignedMembers = user.Access_ResignedMembers
                    qry.Access_Utilization = user.Access_Utilization
                    qry.Access_Reimbursements = user.Access_Reimbursements
                    qry.Access_ClinicResults = user.Access_ClinicResults
                    qry.AccessLevel = user.AccessLevel
                    ' qry.CompanyAddress = user.CompanyAddress
                    qry.CompanyName = user.CompanyName
                    qry.Designation = user.Designation
                    qry.EmailAddress = user.EmailAddress
                    qry.FaxNo = user.FaxNo
                    qry.FirstName = user.FirstName
                    qry.IsActive = True
                    qry.LastName = user.LastName
                    qry.MobileNo = user.MobileNo
                    qry.TelNo = user.TelNo
                    qry.RegMotherCode = user.RegMotherCode
                    qry.RegAccountCode = user.RegAccountCode
                    qry.UserID = user.UserID

                    If user.Username IsNot Nothing Then qry.Username = user.Username

                    If db.SaveChanges() > 0 Then Return True
                    Return False
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function AddUser(ByVal user As emed_corporate_users)
        Dim objUser As New emed_corporate_users

        objUser = user
        Try
            Using db = New emedicardEntities
                db.emed_corporate_users.AddObject(objUser)
                If db.SaveChanges() > 0 Then Return objUser.UserID
                Return 0
            End Using
            Return False
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function CheckOldPassword(ByVal sOldPass As String, ByVal sUserName As String)
        Try
            Using db = New emedicardEntities
                Dim oUser As New emed_corporate_users
                oUser = (From p In db.emed_corporate_users
                     Where p.Username = sUserName
                     Select p).FirstOrDefault

                If oUser.Pword = sOldPass Then Return True

                Return False

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function CorpUserCheckEmail(ByVal sEmail As String)
        Try
            Using db = New emedicardEntities
                Dim objCorpUsers As New emed_corporate_users
                objCorpUsers = (From p In db.emed_corporate_users
                           Where p.EmailAddress = Trim(sEmail)
                           Select p).FirstOrDefault

                If Not objCorpUsers Is Nothing Then Return True

                Return False

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub Save_ID_Request(ByVal objIDReq As emed_corporate_id_request)
        Try
            Using db = New emedicardEntities
                db.AddToemed_corporate_id_request(objIDReq)
                db.SaveChanges()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Delete_ID_Request(ByVal iRequestID As Integer)
        Dim objRequest As New emed_corporate_id_request

        Try

            Using db = New emedicardEntities
                objRequest = (From p In db.emed_corporate_id_request
                              Where p.id = iRequestID
                              Select p).FirstOrDefault

                db.DeleteObject(objRequest)
                db.SaveChanges()

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Save_ECU_Request(ByVal objECU As emed_ecu_requests)
        Try
            Using db = New emedicardEntities
                db.AddToemed_ecu_requests(objECU)
                db.SaveChanges()
            End Using

            Dim objECUold As New EMC_ECU_REQUESTS
            Using emedOld = New eMEDICardOldEntities
                With objECUold

                    .UserID = objECU.UserID
                    .AccountCode = objECU.AccountCode
                    .CompanyName = objECU.CompanyName
                    .MemberCode = objECU.MemberCode
                    .MemberLName = objECU.MemberLName
                    .MemberFName = objECU.MemberFName
                    .MemberMInitial = objECU.MemberMInitial
                    .MemberDesignation = objECU.MemberDesignation
                    .PreferredDate = objECU.PreferredDate
                    .HospitalCode = objECU.HospitalCode
                    .HospitalName = objECU.HospitalName
                    .Remarks = objECU.Remarks
                    .Status = objECU.Status
                    .RequestedDate = objECU.RequestedDate
                    .RequestedBy = objECU.RequestedBy
                    .UserType = objECU.UserType

                End With

                emedOld.AddToEMC_ECU_REQUESTS(objECUold)
                emedOld.SaveChanges()
            End Using


        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Sub Delete_ECU_Request(ByVal requestdate As String, ByVal memcode As String)
        Dim objRequest As New List(Of emed_ecu_requests)
        Dim dDateRequest As DateTime = Convert.ToDateTime(requestdate)
        Dim obj As New emed_ecu_requests

        Try

            Using db = New emedicardEntities

                Dim qry = From p In db.emed_ecu_requests
                          Where p.MemberCode = memcode
                          Select p

                objRequest = qry.ToList

                For Each item As emed_ecu_requests In objRequest
                    If item.RequestedDate.ToString = requestdate.ToString Then
                        obj = item
                    End If
                Next

                db.DeleteObject(obj)
                db.SaveChanges()

                Dim objEMCRequest As New List(Of EMC_ECU_REQUESTS)
                Dim obj2 As New EMC_ECU_REQUESTS

                Using emedOld = New eMEDICardOldEntities
                    Dim qry2 = From p In emedOld.EMC_ECU_REQUESTS
                              Where p.MemberCode = memcode
                              Select p

                    objEMCRequest = qry2.ToList

                    For Each item As EMC_ECU_REQUESTS In objEMCRequest
                        If item.RequestedDate.ToString = requestdate.ToString Then
                            obj2 = item
                        End If
                    Next

                    emedOld.DeleteObject(obj2)
                    emedOld.SaveChanges()

                End Using
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Save_Utilization_Request(ByVal objUtil As emed_requested_utilization)

        Using db = New emedicardEntities
            db.AddToemed_requested_utilization(objUtil)
            db.SaveChanges()
        End Using

        Dim objUtilold As New EMC_REQUESTED_UTILIZATION
        Try


            Using emedOld = New eMEDICardOldEntities
                With objUtilold
                    .UserID = objUtil.UserID
                    .AccountCode = objUtil.AccountCode
                    .AccountName = objUtil.AccountName
                    .Remarks = objUtil.Remarks
                    .RequestedDate = objUtil.RequestedDate
                    .RequestedBy = objUtil.RequestedBy
                    .Status = objUtil.Status
                End With

                emedOld.AddToEMC_REQUESTED_UTILIZATION(objUtilold)
                emedOld.SaveChanges()
            End Using
        Catch ex As Exception
            Dim x As String = ex.ToString
        End Try
    End Sub

    Public Sub Save_CorpUsers_Account(ByVal objAcct As emed_corporate_accounts)
        Dim objUsersAcct As New emed_corporate_accounts
        Try

            Using db = New emedicardEntities

                db.AddToemed_corporate_accounts(objAcct)
                db.SaveChanges()

            End Using
        Catch ex As Exception
            Dim msg As String = ex.InnerException.ToString
        End Try
    End Sub

    Public Function Save_Endorsement_Request(ByVal objRequest As emed_corporate_users_requests)

        Dim lID As Long = 0

        Try

            Using db = New emedicardEntities

                db.AddToemed_corporate_users_requests(objRequest)
                db.SaveChanges()
                lID = objRequest.rec_id

            End Using
        Catch ex As Exception
            Dim msg As String = ex.InnerException.ToString
        End Try

        Return lID
    End Function

    Public Sub Update_CorporateUser_Plans(ByVal id As Long, ByVal plan As String)
        Dim objUsersAcct As New emed_corporate_accounts

        Try

            Using db = New emedicardEntities
                objUsersAcct = (From p In db.emed_corporate_accounts
                               Where p.id = id
                               Select p).FirstOrDefault

                If Not objUsersAcct Is Nothing Then
                    objUsersAcct.Plans = plan
                    db.SaveChanges()
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Sub Delete_CorpUserAccounts(ByVal id As String)
        Dim objAPE As New emed_corporate_accounts

        Using db = New emedicardEntities

            objAPE = (From p In db.emed_corporate_accounts
                      Where p.id = id
                      Select p).FirstOrDefault
            db.DeleteObject(objAPE)
            db.SaveChanges()

        End Using
    End Sub

    Public Function GetCompanyByCode(ByVal acctcode As String)
        Dim objCompany As New Account
        Using db = New MembershipEntities
            objCompany = (From p In db.Accounts1
                       Where p.ACCOUNT_CODE = acctcode
                       Select p).FirstOrDefault

            Return objCompany
        End Using
    End Function

    Public Function ChangeUserEmail(ByVal sEmail As String, ByVal sNewEmail As String)
        Dim bSuccess As Boolean = False

        Try
            Using db = New emedicardEntities
                Dim objCorpUser As New emed_corporate_users
                objCorpUser = (From p In db.emed_corporate_users
                             Where p.EmailAddress = sEmail
                             Select p).FirstOrDefault

                If Not objCorpUser Is Nothing Then

                    objCorpUser.Username = Trim(sNewEmail)
                    objCorpUser.EmailAddress = Trim(sNewEmail)
                    db.SaveChanges()
                    bSuccess = True
                End If

            End Using
        Catch ex As Exception
            Throw
        End Try
        Return bSuccess

    End Function
    Public Sub DeleteEndorsementCancelRequest(ByVal id As Long)

        Dim objReq As New emed_corporate_users_requests

        Using db = New emedicardEntities

            objReq = (From p In db.emed_corporate_users_requests
                      Where p.rec_id = id
                      Select p).FirstOrDefault

            objReq.is_deleted = True
            db.SaveChanges()

        End Using
    End Sub

    'Public Sub Add_Request_Status_History(ByVal emed_req_hist As emed_request_status_history)
    '    'Using db = New emedicardEntities
    '    '    db.AddToemed_request_status_history(emed_req_hist)
    '    '    db.SaveChanges()
    '    'End Using
    'End Sub

    Function CheckMemberValidity(ByVal sMemCode As String, ByVal sAccountCode As String) As SYS_UWPRINCIPAL_ACTIVE_MTBL
        Dim obj As New SYS_UWPRINCIPAL_ACTIVE_MTBL

        Using db = New MembershipEntities
            obj = (From p In db.SYS_UWPRINCIPAL_ACTIVE_MTBL
                   Where p.PRIN_CODE = sMemCode And p.ACCOUNT_CODE = sAccountCode).FirstOrDefault

        End Using

        Return obj

    End Function
#End Region

#Region "Functions"
    'Public Function ToDataTable(Of T)(ByVal list As List(Of T)) As DataTable
    '    Dim _resultDataTable As New DataTable("results")
    '    Dim _resultDataRow As DataRow = Nothing
    '    Dim dt As New DataTable
    '    Dim _itemProperties() As System.Reflection.PropertyInfo

    '    ' Meta Data.
    '    ' Each item property becomes a column in the table 
    '    ' Build an array of Property Getters, one for each Property 
    '    ' in the item class. Can pass anything as [item] it is just a 
    '    ' place holder parameter, later we will invoke it with the
    '    ' correct item. This code assumes the runtime does not change
    '    ' the ORDER in which the proprties are returned.
    '    If list Is Nothing Then
    '    Else
    '        If list.Count > 0 Then
    '            _itemProperties = list.Item(0).GetType().GetProperties()
    '            'MsgBox(_itemProperties.Length)
    '            For Each p As System.Reflection.PropertyInfo In _itemProperties

    '                'dt.Columns.Add(p.Name, _
    '                '          p.GetGetMethod.ReturnType())
    '                dt.Columns.Add(p.Name, _
    '                           If(Nullable.GetUnderlyingType(p.PropertyType), p.PropertyType))
    '            Next

    '            For Each item As T In list

    '                ' Get the data from this item into a DataRow
    '                ' then add the DataRow to the DataTable.
    '                ' Eeach items property becomes a colunm.
    '                '
    '                _itemProperties = item.GetType().GetProperties()
    '                _resultDataRow = dt.NewRow()
    '                For Each p As System.Reflection.PropertyInfo In _itemProperties
    '                    '_resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                    If p.GetValue(item, Nothing) = Nothing Then
    '                        'If p.GetValue(item, Nothing) = False Or p.GetValue(item, Nothing) = True Then
    '                        '    _resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                        'Else
    '                        '    _resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                        'End If
    '                        '_resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                        _resultDataRow(p.Name) = DBNull.Value
    '                    Else
    '                        _resultDataRow(p.Name) = IIf(Not IsDBNull(p.GetValue(item, Nothing)), p.GetValue(item, Nothing), DBNull.Value)
    '                    End If

    '                Next
    '                dt.Rows.Add(_resultDataRow)
    '            Next
    '        End If
    '    End If
    '    ''MsgBox(dt.Rows.Count)
    '    Return dt
    'End Function
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
