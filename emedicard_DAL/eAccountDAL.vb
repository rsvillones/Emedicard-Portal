Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq
Imports System.Transactions
Imports System.Reflection

Public Class eAccountDAL
#Region "Select"

    Public Function CheckLogin(ByVal username As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emed_agent_users
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
                Dim qry = (From u In db.emed_agent_users
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
                Dim qry = (From u In db.emed_agent_users
                            Where u.Username = username And u.Pword = password And u.IsActive = True
                            Select u).FirstOrDefault

                ' User record exists
                If Not qry Is Nothing Then Return True
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function ChangeUserEmail(ByVal sEmail As String, ByVal sNewEmail As String)
        Dim bSuccess As Boolean = False

        Try
            Using db = New emedicardEntities
                Dim objAcctUser As New emed_agent_users
                objAcctUser = (From p In db.emed_agent_users
                             Where p.EmailAddress = sEmail
                             Select p).FirstOrDefault

                If Not objAcctUser Is Nothing Then

                    objAcctUser.Username = Trim(sNewEmail)
                    objAcctUser.EmailAddress = Trim(sNewEmail)
                    db.SaveChanges()
                    bSuccess = True
                End If

            End Using
        Catch ex As Exception
            Throw
        End Try
        Return bSuccess
    End Function
    Public Function GetAgentInfo(ByVal agntUsrName As String)
        Try
            Dim objAgnt As New emed_agent_users
            Using db = New emedicardEntities
                objAgnt = (From p In db.emed_agent_users
                           Where p.Username = agntUsrName
                           Select p).FirstOrDefault

                Return objAgnt
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function GetAgentInfoByID(ByVal agntID As Integer)
        Dim objAgnt As New emed_agent_users
        Using db = New emedicardEntities
            objAgnt = (From p In db.emed_agent_users
                       Where p.UserID = agntID
                       Select p).FirstOrDefault

            Return objAgnt
        End Using
    End Function

    Public Function GetAgentInfoByEmail(ByVal emailad As String)
        Try
            Dim objAgnt As New emed_agent_users
            Using db = New emedicardEntities
                objAgnt = (From p In db.emed_agent_users
                           Where p.EmailAddress = emailad
                           Select p).FirstOrDefault

                Return objAgnt
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function GetAgentInfoByAgentCode(ByVal agntCode As String)
        Dim objAgnt As New emed_agent_users
        Using db = New emedicardEntities
            objAgnt = (From p In db.emed_agent_users
                       Where p.AgentCode = agntCode And p.MainUserID = 0
                       Select p).FirstOrDefault

            Return objAgnt
        End Using
    End Function

    Public Function GetCorpUserInfo(ByVal agntUsrName As String)
        Try
            Dim objAgnt As New emed_corporate_users
            Using db = New emedicardEntities
                objAgnt = (From p In db.emed_corporate_users
                           Where p.Username = agntUsrName
                           Select p).FirstOrDefault

                Return objAgnt
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function GetCorpUserInfoByID(ByVal agntID As Integer)
        Dim objAgnt As New emed_agent_users
        Using db = New emedicardEntities
            objAgnt = (From p In db.emed_agent_users
                       Where p.UserID = agntID
                       Select p).FirstOrDefault

            Return objAgnt
        End Using
    End Function

    Public Function GetAgentUsers(ByVal sAgntCode As String, ByVal uid As Integer) As List(Of emed_agent_users)
        Try

            Using db = New emedicardEntities
                Dim objAgnt = (From p In db.emed_agent_users
                               Where p.AgentCode = sAgntCode And p.UserID <> uid
                               Select p)

                If Not objAgnt Is Nothing Then
                    Return objAgnt.ToList()
                End If
                Return Nothing

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCorporateUsersByUserId(ByVal uid As Integer) As List(Of emed_get_ecorpuser_by_userid_accounts_Result)
        Try

            Using db = New emedicardEntities
                Dim objUsers = (From p In db.emed_ecorpuser_by_userid_accounts(uid)
                                Select p)

                If Not objUsers Is Nothing Then
                    Return objUsers.ToList()
                End If
                Return Nothing

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetCorporateUsersAccountByUserId(ByVal uid As Integer) As List(Of emed_get_ecorpuser_by_userid_accounts_Result)
        Try

            Using db = New emedicardEntities
                Dim objAcc = (From p In db.emed_ecorpuser_by_userid_accounts(uid)
                              Select p)

                If Not objAcc Is Nothing Then
                    Return objAcc.ToList()
                End If
                Return Nothing

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Function GetCorporateAccountByUserId(ByVal uid As Integer) As List(Of emed_get_ecorpaccounts_by_userid_Result)
    '    Try
    '        Using db = New emedicardEntities
    '            Dim objAcc = (From p In db.emed_ecorpaccounts_by_userid(uid)
    '                          Select p)

    '            If Not objAcc Is Nothing Then
    '                Return objAcc.ToList()
    '            End If
    '            Return Nothing

    '        End Using
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Public Function GetCorporateAccountByUserId(ByVal uid As Integer) As List(Of emed_eaccount_users_account)
        Try
            Dim objAgntAcct As New List(Of emed_eaccount_users_account)
            Using db = New emedicardEntities
                objAgntAcct = (From p In db.emed_eaccount_users_account
                               Where p.UserID = uid
                               Select p).ToList

                Return objAgntAcct
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetBookDates(ByVal iYear As Integer, ByVal iMonth As Integer) As DataTable
        Using db = New emedicardEntities
            Dim qryBook = db.emed_getbooking_dates(iYear, iMonth)

            Return ToDataTable(qryBook.ToList)
        End Using
    End Function

    Public Function GetProvinces()
        Using db = New MembershipEntities

            Dim liProvList = From p In db.SYS_PROVINCE_LTBL
                         Select p Order By p.PROVINCE_NAME

            Return ToDataTable(liProvList.ToList)

        End Using
    End Function

    Public Function GetCity(ByVal sProvCode As String)
        Using db = New MembershipEntities

            Dim liCityList = From p In db.SYS_CITY_LTBL
                         Where p.PROVINCE_CODE = sProvCode
                         Select p Order By p.CITY_NAME
            Return ToDataTable(liCityList.ToList)
        End Using
    End Function

    Public Function GetRegion(ByVal sRegCode As String)
        Using db = New MembershipEntities
            Dim lCity As New SYS_CITY_LTBL

            Dim liCityList = From p In db.SYS_CITY_LTBL
                         Where p.CITY_CODE = sRegCode
                         Select p


            lCity = liCityList.FirstOrDefault

            Dim lRegion As New SYS_REGION_LTBL

            Dim lRegionList = From p In db.SYS_REGION_LTBL
                          Where p.REGION_CODE = lCity.REGION_CODE
                          Select p

            lRegion = lRegionList.FirstOrDefault

            Return lRegion.REGION_NAME

        End Using
    End Function

    Public Function GetCompany(ByVal acctcode As String)
        Dim objCompany As New Account
        Using db = New MembershipEntities
            objCompany = (From p In db.Accounts1
                       Where p.ACCOUNT_CODE = acctcode
                       Select p).FirstOrDefault

            Return objCompany
        End Using
    End Function

    Public Function GetAPERequest(ByVal userid As String)
        Using db = New emedicardEntities
            Dim objAPERest As ObjectResult(Of emed_get_ape_request_Result) = db.emed_get_ape_request(userid)

            Return ToDataTable(objAPERest.ToList)
        End Using
    End Function

    Public Function GetPackageCode(ByVal spcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable
            Dim objPCODE As ObjectResult(Of emed_getpackage_code_Result) = db.emed_getpackage_code(spcode)

            dt = ToDataTable(objPCODE.ToList)

            If dt.Columns.Count > 0 Then
                Return dt(0)(0).ToString
            Else
                Return ""
            End If

        End Using

    End Function

    Public Function GetHCS(ByVal stype As String, ByVal sacctcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable

            Dim objHCS As ObjectResult(Of sp_selecthcs_Result) = db.sp_selecthcs(stype, sacctcode)

            Return ToDataTable(objHCS.ToList)
        End Using
    End Function

    Public Function GetPreventiveHCS(ByVal stype As String, ByVal sacctcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable

            Dim objPHCS As ObjectResult(Of SP_SELECTPHCS_Result) = db.SP_SELECTPHCS(stype, sacctcode)

            Return ToDataTable(objPHCS.ToList)
        End Using
    End Function

    Public Function GetECS(ByVal stype As String, ByVal sacctcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable

            Dim objECS As ObjectResult(Of SP_SELECTECS_Result) = db.sp_selectecs(stype, sacctcode)

            Return ToDataTable(objECS.ToList)

        End Using
    End Function

    Public Function GetMFA(ByVal stype As String, ByVal sacctcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable

            Dim objMFA As ObjectResult(Of SP_SELECTMFA_Result) = db.sp_selectmfa(stype, sacctcode)

            Return ToDataTable(objMFA.ToList)
        End Using
    End Function

    Public Function GetDCS(ByVal stype As String, ByVal sacctcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable

            Dim objDCS As ObjectResult(Of SP_SELECTDCS_Result) = db.sp_selectdcs(stype, sacctcode)

            Return ToDataTable(objDCS.ToList)
        End Using
    End Function

    Public Function GetExclusion(ByVal stype As String, ByVal sacctcode As String)
        Using db = New MembershipEntities
            Dim dt As New DataTable

            Dim objEXC As ObjectResult(Of sp_selectexclusion_Result) = db.sp_selectexclusion(sacctcode)

            Return ToDataTable(objEXC.ToList)
        End Using
    End Function

    Public Function GetOhtersHCS(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_select_company_others(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetOhtersPHCS(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectphcs_ssrs_ohter(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetOhtersECS(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectecs_ssrs_others(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetOhtersMFA(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectmfa_ssrs_others(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetOhtersDSC(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectdcs_ssrs_others(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetPEC(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectpec(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetOthersPEC(ByVal sType As String, ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectpec_ssrs_others(sType, sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetPOS(ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectrsp_ssrs(sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetMATOthers(ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectrsp_ssrs_mab_others(sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetECUOthers(ByVal sAcctCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_selectrsp_ssrs_ecu_others(sAcctCode)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetAccountsByAgent(ByVal sAgentCode As String)
        Try
            Using db = New MembershipEntities
                db.CommandTimeout = 0
                'Dim qry As ObjectResult(Of sp_intra_companycode_by_agent_Result) = db.sp_intra_companycode_by_agent("ALL", "", sAgentCode)
                Dim qry As ObjectResult(Of sp_intra_companycode_by_agent_Result) = db.sp_intra_companycode_by_agent("ACTIVE ACCOUNTS", "", sAgentCode)

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Dim s As String = ex.InnerException.ToString
            Throw
        End Try
    End Function
    Public Function GetActiveAccountsByAgent(ByVal sAgentCode As String)
        Try
            Using db = New MembershipEntities
                db.CommandTimeout = 0
                'Dim qry As ObjectResult(Of sp_intra_companycode_by_agent_Result) = db.sp_intra_companycode_by_agent("ALL", "", sAgentCode)
                Dim qry As ObjectResult(Of emed_active_accounts_Result) = db.emed_active_accounts(sAgentCode)

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Dim s As String = ex.InnerException.ToString
            Throw
        End Try
    End Function

    Public Function GetEMEDAccountByAgent(ByVal userid As Integer, ByVal agentCode As String)
        Using db = New emedicardEntities
            db.CommandTimeout = 0
            Dim qry = From p In db.emed_get_user_assigned_accounts(userid, agentCode)
                      Select p

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function GetEMEDAccountByAgent(ByVal userid As Integer, ByVal agentCode As String, ByVal istatus As Short)
        Using db = New emedicardEntities
            Dim qry = From p In db.emed_user_accounts_bystatus(userid, agentCode, istatus)
                      Select p

            Return ToDataTable(qry.ToList)
        End Using
    End Function
    Public Function GetEMEDAgentActiveAccounts(ByVal agentcode As String)
        Using db = New ITMG_INTRAEntities
            Dim qry = From p In db.emed_agent_active_accounts(agentcode)
                      Select p

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function GetEMEDAgentActiveAccounts(ByVal agentcode As String, ByVal istatus As Short)
        Using db = New ITMG_INTRAEntities
            Dim qry = From p In db.emed_agent_active_account_by_status(agentcode, istatus)
                      Select p

            Return ToDataTable(qry.ToList)
        End Using
    End Function
    Public Function GetAccountsByAgent(ByVal sAgentCode As String, ByVal user_id As String, ByVal main_user_id As String)
        Using db = New MembershipEntities
            db.CommandTimeout = 0
            Dim qry = From p In db.sp_emed_company_code_by_agent("ALL", "", sAgentCode, user_id, main_user_id)

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function GetAccountPlan(ByVal accountCode As String, ByVal userid As String)
        Dim objAgntAcct As New emed_eaccount_users_account

        Using db = New emedicardEntities
            objAgntAcct = (From p In db.emed_eaccount_users_account
                           Where p.AccountCode = accountCode And p.UserID = userid
                           Select p).FirstOrDefault

            Return objAgntAcct
        End Using

    End Function

    Public Function GetAccountPlansUnderUserAccounts(ByVal userid As String)
        Dim objAgntAcct As New emed_eaccount_users_account

        Using db = New emedicardEntities
            objAgntAcct = (From p In db.emed_eaccount_users_account
                           Where p.UserID = userid
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

    Public Function GetMotherCode(ByVal accountCode As String)
        Dim objAgntAcct As New Account

        Using db = New MembershipEntities
            objAgntAcct = (From p In db.Accounts1
                          Where p.ACCOUNT_CODE = accountCode
                          Select p).FirstOrDefault

            Return objAgntAcct
        End Using

    End Function

    Public Function GetUtilizationGrpByDisease(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)

        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_account_utilization_ip_grp(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_account_utilization_ip_grp_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function GetUtilization(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_account_utilization_ip(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_account_utilization_ip_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function GetUtilizationGrpByDiseaseOP(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)

        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_account_utilization_op_grp(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_account_utilization_op_grp_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function GetUtilizationOP(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)

        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_account_utilization_op(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_account_utilization_op_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function GetMemberUtilzation(ByVal MemberCode As String, ByVal sDateFr As String, ByVal sDateTo As String)
        Try

            Using db = New ClaimsEntities
                'Dim qry = From a In db.emed_utilization_all_by_date(MemberCode, sDateFr, sDateTo)
                '          Select a

                Dim qry = From a In db.emed_utilization_all_by_date_v2(MemberCode, sDateFr, sDateTo)
                          Select a

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
    End Function

    Public Function Get_ER_ByGrp(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_accounts_utilization_er(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_accounts_utilization_er_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function Get_ER(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)

        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_account_utilization_er_test(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_account_utilization_er_test_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function Get_Reim_Util(ByVal service As String, ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Using db = New ClaimsEntities
            'Dim qry = From p In db.emed_reim_util(service, account_code, date_from, date_to, last_name)
            Dim qry = From p In db.emed_reim_util_v2(service, account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using
    End Function

    Public Function Get_Dental(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Using db = New ClaimsEntities
            'Dim qry = From p In db.SP_Intra_ActiveAccount_Utilization_DT(date_from, date_to, last_name, account_code)
            Dim qry = From p In db.SP_Intra_ActiveAccount_Utilization_DT_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function Get_DCR(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_account_util_dcr(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_account_util_dcr_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function

    Public Function Get_Call_Log(ByVal account_code As String, ByVal date_from As String, ByVal date_to As String, ByVal mem_code As String)
        Using db = New ClaimsEntities
            'Dim qry = From p In db.active_accounts_util_call_log(account_code, date_from, date_to, last_name)
            Dim qry = From p In db.active_accounts_util_call_log_v2(account_code, date_from, date_to, mem_code)

            Return ToDataTable(qry.ToList)
        End Using

    End Function
#End Region

    Public Sub SaveAgent(ByVal oAgentUser As emed_agent_users)
        Using db = New emedicardEntities

            If oAgentUser.UserID > 0 Then
                Dim objAgntUser As New emed_agent_users


                objAgntUser = (From p In db.emed_agent_users
                               Where p.UserID = oAgentUser.UserID
                               Select p).FirstOrDefault

                With objAgntUser
                    .FirstName = oAgentUser.FirstName
                    .LastName = oAgentUser.LastName
                    .Username = oAgentUser.Username
                    .Type = oAgentUser.Type
                    .BrokerName = oAgentUser.BrokerName
                    .EmailAddress = oAgentUser.EmailAddress
                    '.AgentCode = oAgentUser.AgentCode
                    '.MainUserID = 0
                    '.AccessLevel = 1
                    If Trim(oAgentUser.Pword) <> "" Then
                        .Pword = oAgentUser.Pword
                    End If
                    '.Access_APE = oAgentUser.Access_APE
                    '.Access_Utilization = oAgentUser.Access_Utilization
                    '.Access_Endorsement = oAgentUser.Access_Endorsement
                    '.Access_Benefits = oAgentUser.Access_Benefits
                    '.Access_ID = oAgentUser.Access_ID
                    '.Access_ECU = oAgentUser.Access_ECU
                    '.Access_ActiveMembers = oAgentUser.Access_ActiveMembers
                    '.Access_ResignedMembers = oAgentUser.Access_ResignedMembers
                    '.Access_ActionMemos = oAgentUser.Access_ActionMemos
                    '.IsActive = oAgentUser.IsActive
                End With
                db.SaveChanges()

            Else
                db.AddToemed_agent_users(oAgentUser)
                db.SaveChanges()
            End If
        End Using
    End Sub

    Public Function SaveAgentUser(ByVal oAgentUser As emed_agent_users, ByVal iMainID As Integer)
        Dim uID As Integer = 0
        Try


            Using db = New emedicardEntities

                If oAgentUser.UserID > 0 Then
                    Dim objAgntUser As New emed_agent_users


                    objAgntUser = (From p In db.emed_agent_users
                                   Where p.UserID = oAgentUser.UserID
                                   Select p).FirstOrDefault

                    With objAgntUser
                        .FirstName = oAgentUser.FirstName
                        .LastName = oAgentUser.LastName
                        .Username = oAgentUser.Username
                        .Type = oAgentUser.Type
                        .BrokerName = oAgentUser.BrokerName
                        .EmailAddress = oAgentUser.EmailAddress
                        .AccessLevel = 2
                        If Trim(oAgentUser.Pword) <> "" Then
                            .Pword = oAgentUser.Pword
                        End If
                        .Access_APE = oAgentUser.Access_APE
                        .Access_Utilization = oAgentUser.Access_Utilization
                        .Access_Endorsement = oAgentUser.Access_Endorsement
                        .Access_Benefits = oAgentUser.Access_Benefits
                        .Access_ID = oAgentUser.Access_ID
                        .Access_ECU = oAgentUser.Access_ECU
                        .Access_ActiveMembers = oAgentUser.Access_ActiveMembers
                        .Access_ResignedMembers = oAgentUser.Access_ResignedMembers
                        .Access_ActionMemos = oAgentUser.Access_ActionMemos
                        .Access_Reimbursements = oAgentUser.Access_Reimbursements
                        .Access_ClinicResults = oAgentUser.Access_ClinicResults
                        .IsActive = oAgentUser.IsActive
                    End With
                    db.SaveChanges()
                    uID = objAgntUser.UserID
                Else
                    oAgentUser.DateRegistered = Now
                    oAgentUser.DateUserActivated = Now
                    oAgentUser.IsActive = True
                    db.AddToemed_agent_users(oAgentUser)
                    db.SaveChanges()
                    uID = oAgentUser.UserID
                End If
            End Using

        Catch ex As Exception
            Throw
        End Try

        Return uID

    End Function

    Public Sub Save_APE_Request(ByVal APEReq As emed_ape_requests)
        Try
            Using db = New emedicardEntities
                db.AddToemed_ape_requests(APEReq)
                db.SaveChanges()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Sub Delete_APE_Request(ByVal apeid As Integer)
        Dim objAPE As New emed_ape_requests

        Using db = New emedicardEntities

            objAPE = (From p In db.emed_ape_requests
                      Where p.id = apeid
                      Select p).FirstOrDefault
            db.DeleteObject(objAPE)
            db.SaveChanges()

        End Using
    End Sub

    Public Sub Save_Users_Account(ByVal objAcct As emed_eaccount_users_account)
        Dim objUsersAcct As New emed_eaccount_users_account
        Try


            Using db = New emedicardEntities

                db.AddToemed_eaccount_users_account(objAcct)
                db.SaveChanges()

            End Using
        Catch ex As Exception
            Dim msg As String = ex.InnerException.ToString
        End Try
    End Sub

    Public Sub Delete_UserAccounts(ByVal id As String)
        Dim objAPE As New emed_eaccount_users_account

        Using db = New emedicardEntities

            objAPE = (From p In db.emed_eaccount_users_account
                      Where p.id = id
                      Select p).FirstOrDefault
            db.DeleteObject(objAPE)
            db.SaveChanges()

        End Using
    End Sub

    Public Function ResetPassword(ByVal username As String, ByVal newpasssword As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry As emed_agent_users = (From u In db.emed_agent_users
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

    Public Sub Save_Utilization_Request(ByVal objUtil As emed_requested_utilization)

        Using db = New emedicardEntities
            db.AddToemed_requested_utilization(objUtil)
            db.SaveChanges()
        End Using

        'Dim objUtilold As New EMC_REQUESTED_UTILIZATION
        'Try


        '    Using emedOld = New eMEDICardOldEntities
        '        With objUtilold
        '            .UserID = objUtil.UserID
        '            .AccountCode = objUtil.AccountCode
        '            .AccountName = objUtil.AccountName
        '            .Remarks = objUtil.Remarks
        '            .RequestedDate = objUtil.RequestedDate
        '            .RequestedBy = objUtil.RequestedBy
        '            .Status = objUtil.Status
        '        End With

        '        emedOld.AddToEMC_REQUESTED_UTILIZATION(objUtilold)
        '        emedOld.SaveChanges()
        '    End Using
        'Catch ex As Exception
        '    Dim x As String = ex.ToString
        'End Try
    End Sub

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

    '                'For Each p As System.Reflection.PropertyInfo In _itemProperties
    '                '    '_resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                '    'If p.GetValue(item, Nothing) = Nothing Then
    '                '    '    'If p.GetValue(item, Nothing) = False Or p.GetValue(item, Nothing) = True Then
    '                '    '    '    _resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                '    '    'Else
    '                '    '    '    _resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                '    '    'End If
    '                '    '    '_resultDataRow(p.Name).GetType()
    '                '    '    '_resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                '    '    _resultDataRow(p.Name) = DBNull.Value
    '                '    'Else
    '                '    '    _resultDataRow(p.Name) = IIf(Not IsDBNull(p.GetValue(item, Nothing)), p.GetValue(item, Nothing), DBNull.Value)
    '                '    'End If
    '                '    If Not IsDBNull(p.GetValue(item, Nothing)) Then
    '                '        If IsDBNull(p.GetValue(item, Nothing)) Or p.GetValue(item, Nothing) Is Nothing Then
    '                '            _resultDataRow(p.Name) = DBNull.Value
    '                '        Else
    '                '            _resultDataRow(p.Name) = p.GetValue(item, Nothing)
    '                '        End If

    '                '    Else
    '                '        _resultDataRow(p.Name) = DBNull.Value
    '                '    End If


    '                'Next

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
End Class
