Imports emedicard_DAL
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq

Imports System.Transactions
Imports System.Reflection
Imports System.Data.SqlClient


Public Class ememberDAL
    Implements IDisposable


    'Check Username
    Public Function CheckEMemberUserName(ByVal usercode As String) As Boolean
        Try
            Using db = New emedicardEntities()

                Dim qry = (From c In db.emedicard_emember_users
                           Where c.Username = usercode
                           Select c).FirstOrDefault

                If Not qry Is Nothing Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function

    'Check Password if Exists
    Public Function CheckEMemberPassword(ByVal usercode As String, ByVal password As String) As Boolean
        Try
            Using db = New emedicardEntities()
                Dim qry = (From u In db.emedicard_emember_users
                           Where u.Username = usercode And u.Pword = password And Not u.DateActivationConfirmation Is Nothing
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function

    'Check if member's account is activated
    Public Function IsMemberActivated(ByVal usercode As String) As Boolean
        Try
            Using db = New emedicardEntities()
                Dim qry = (From u In db.emedicard_emember_users
                           Where u.Username = usercode And Not u.DateActivationConfirmation Is Nothing
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function

    Public Function CheckEMemberCode(ByVal memberCode As String) As Boolean
        Try
            Using db = New emedicardEntities()
                Dim qry = (From u In db.emedicard_emember_users
                           Where u.MemberCode = memberCode
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.InnerException.Message)
        End Try
    End Function
    'Get User Information

    Public Function GetUserInfo(ByVal userCode As String) As emedicard_emember_users
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emedicard_emember_users
                           Where u.Username = userCode
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    Return qry
                End If
                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function ValidateMember(ByVal MemberCode As String, ByVal Birthdate As Date)
        Try
            Using db = New MembershipEntities()
                Dim qry As MemberDetails = (From m In db.GetMemberDetails(MemberCode)
                                            Where m.BDAY = Birthdate
                                            Select m).FirstOrDefault

                If Not qry Is Nothing Then
                    Return qry
                End If
                Return Nothing

            End Using
        Catch ex As Exception
            Throw New Exception(ex.InnerException.ToString)
        End Try
    End Function

    Public Function GetMemberInformation(ByVal MemberCode As String)
        Try
            Using db = New MembershipEntities()
                Dim qry As MemberDetails = (From m In db.GetMemberDetails(MemberCode)
                                            Select m).FirstOrDefault

                If Not qry Is Nothing Then
                    Return qry
                End If
                Return Nothing

            End Using
        Catch ex As Exception
            Throw New Exception(ex.InnerException.ToString)
        End Try
    End Function

    Public Function GetDependents(ByVal MemberCode As String) As List(Of DependentDetails)
        Try
            Using db = New MembershipEntities
                Dim qry = From d In db.GetDependentDetails(MemberCode)
                          Select d

                Dim result As List(Of DependentDetails) = qry.ToList
                Return result
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetDependentPaymentHistory(ByVal MemberCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_intra_payment_history_dep_emed(MemberCode)

                If Not qry Is Nothing Then Return qry.ToList()

                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try
    End Function

    Public Function GetPrincipalPaymentHistory(ByVal memberCode As String)
        Try
            Using db = New ITMG_INTRAEntities
                Dim qry = From p In db.sp_intra_payment_history_prin_emed(memberCode)

                If Not qry Is Nothing Then Return qry.ToList()

                Return Nothing
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try
    End Function

    Public Function GetAvailments(ByVal MemberCode As String) As List(Of Availments)
        Try

            Using db = New ClaimsEntities
                'Dim qry = From a In db.GetAvailments(MemberCode)
                '          Select a

                Dim qry = From a In db.GetAvailments_v2(MemberCode)
                          Select a

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
    End Function


    Public Function GetReimbursements(ByVal Type As String, ByVal memberCode As String, ByVal sDateFr As String, ByVal sDateTo As String) As List(Of emed_reimbusement_by_date_Result)
        Try
            Using db = New ClaimsEntities
                Dim qry = From a In db.emed_reimbusement_by_date(Type, memberCode, sDateFr, sDateTo)
                          Select a

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
    End Function

    Public Function GetReimbursementStatus(ByVal sMemCode As String) As DataTable
        Dim ds As New DataSet
        Try
            Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
                Dim cmd As New SqlCommand
                Dim da As New SqlDataAdapter

                With cmd
                    .Connection = conn
                    .CommandType = CommandType.StoredProcedure
                    '.CommandText = "REIM_Get_Reimbursement_Status_EMED_V3"
                    .CommandText = "REIM_Get_Reimbursement_Status_EMED_V3"
                    .Parameters.AddWithValue("@member_code", sMemCode)
                End With
                da.SelectCommand = cmd
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try

        Return ds.Tables(ds.Tables.Count - 1)

    End Function

    Public Function GetReimbursementStatusWithDateRange(ByVal sMemCode As String, ByVal dDateFr As Date, ByVal dDateTo As Date) As DataTable
        Dim ds As New DataSet
        Try
            Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
                Dim cmd As New SqlCommand
                Dim da As New SqlDataAdapter

                With cmd
                    .Connection = conn
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "REIM_Get_Reimbursement_Status_EMED_V4"
                    .Parameters.AddWithValue("@member_code", sMemCode)
                    .Parameters.AddWithValue("@start_date", dDateFr)
                    .Parameters.AddWithValue("@end_date", dDateTo)
                End With
                da.SelectCommand = cmd
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try

        Return ds.Tables(ds.Tables.Count - 1)

    End Function

    Public Function GetReimbControlStatus(ByVal sControlCode As String, ByVal sMemCode As String)
        Dim sStatus As String = String.Empty

        Try
            Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
                Dim cmd As New SqlCommand
                Dim prmControlCode As New SqlParameter("@control_code", SqlDbType.VarChar, 25)
                Dim prmStatus As New SqlParameter("@status", SqlDbType.VarChar, 25)
                Dim prmMemCode As New SqlParameter("@mem_code", SqlDbType.VarChar, 25)

                prmControlCode.Value = sControlCode
                prmMemCode.Value = sMemCode
                prmStatus.Direction = ParameterDirection.Output

                With cmd

                    .Connection = conn
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "reim_get_status_emedicard"
                    .Parameters.Add(prmControlCode)
                    .Parameters.Add(prmMemCode)
                    .Parameters.Add(prmStatus)

                End With

                conn.Open()

                cmd.ExecuteNonQuery()

                sStatus = cmd.Parameters(1).Value
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try

        Return sStatus

    End Function
    Function GetActionMemoStatus(controlCode As String) As String
        ' get the action memo status (lacking of documents,with hospital bill and disapproved
        'Dim controlCode As String = controlCode

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            Dim actionMemo As String = String.Empty

            With cmd

                .Connection = conn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "sp_reim_get_status_action_memo"
                .Parameters.Add("@controlCode", SqlDbType.VarChar).Value = controlCode

                conn.Open()
                dr = .ExecuteReader
                If dr.HasRows Then
                    While dr.Read

                        actionMemo = dr(0)
                    End While
                End If
                Return actionMemo

            End With


        End Using


    End Function
    Function GetDisapprovalReason(ByVal ctrl_code As String) As ReimbursementDetails
        Dim reim As New ReimbursementDetails

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            With cmd
                Dim strSQL = "select isnull(replace(replace(d.memo_txt, '#Reason*', ' '), '#Limitation*', ' '), c.memo_txt) as memo_txt, " &
                             "b.denied_date, b.memo_released_date " &
                             "from  reim_op_info a " &
                             "left join reim_processing_status b on a.control_code = b.control_code " &
                             "left join  reim_memo_empty c on a.control_code = c.control_code  " &
                             "left join reim_memo_disapproved d on a.control_code = d.control_code where a.control_code = @control_code"
                'strSQL = strSQL & " UNION select isnull(replace(replace(a.memo_txt, '#Reason*', ' '), '#Limitation*', ' '), b.memo_txt) as memo_txt, " &
                '             "d.denied_date, d.memo_released_date " &
                '             "from reim_memo_disapproved as a full join reim_memo_empty as b on a.control_code = b.control_code " &
                '             "left join reim_ip_info as c on a.disapproved_memo_id = c.disapproved_memo_id " &
                '             "left join reim_processing_status as d on c.processing_status_id = d.processing_status_id " &
                '             "where a.control_code = @control_code or b.control_code = @control_code"
                strSQL = strSQL & " UNION select isnull(replace(replace(d.memo_txt, '#Reason*', ' '), '#Limitation*', ' '), c.memo_txt) as memo_txt, " &
                             "b.denied_date, b.memo_released_date " &
                             "from  reim_ip_info a " &
                             "left join reim_processing_status b on a.control_code = b.control_code " &
                             "left join  reim_memo_empty c on a.control_code = c.control_code  " &
                             "left join reim_memo_disapproved d on a.control_code = d.control_code where a.control_code = @control_code"
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = strSQL
                .Parameters.Add("@control_code", SqlDbType.VarChar).Value = ctrl_code
                conn.Open()
                dr = .ExecuteReader
                If dr.HasRows Then
                    While dr.Read
                        If Not IsDBNull(dr(0)) Then
                            reim.reason_of_disapproval = dr(0)
                            If Not IsDBNull(dr("denied_date")) Then
                                reim.denied_date = Format(dr("denied_date"), "MM/dd/yyyy")
                            End If
                            If Not IsDBNull(dr("memo_released_date")) Then
                                reim.memo_released_date = dr("memo_released_date")
                            End If
                        End If
                    End While
                End If
            End With
        End Using

        Return reim
    End Function

    Function GetActionMemo(ByVal ctrl_code As String) As ReimbursementDetails
        Dim reim As New ReimbursementDetails

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            With cmd
                Dim strSQL = "select a.memo_txt, b.lacking_date, b.memo_released_date from reim_memo_action as a " & _
                             "left join reim_processing_status as b on a.control_code = b.control_code where a.control_code = @control_code"
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = strSQL
                .Parameters.Add("@control_code", SqlDbType.VarChar).Value = ctrl_code
                conn.Open()
                dr = .ExecuteReader
                If dr.HasRows Then
                    While dr.Read
                        If Not IsDBNull(dr(0)) Then
                            reim.remarks = Mid(dr(0), 1, Len((dr(0).ToString)) - 1)
                        End If
                        If Not IsDBNull(dr("lacking_date")) Then
                            reim.lacking_date = dr("lacking_date")
                        End If
                        If Not IsDBNull(dr("memo_released_date")) Then
                            reim.memo_released_date = dr("memo_released_date")
                        End If
                    End While
                End If
            End With
        End Using

        Return reim
    End Function

    Function GetWHBMemo(ByVal ctrl_code As String) As ReimbursementDetails
        Dim reim As New ReimbursementDetails

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            With cmd
                Dim strSQL = "select a.box_1, a.box_2, a.box_3, ISNULL(b.hospital_name, c.hospital_name) as hospital_name, " & _
                             " a.box_2_txt , a.loa_no, d.lacking_date, d.memo_released_date from reim_memo_whb as a left join " & _
                             "Claims.dbo.SYS_HOSPITALS_LTBL as b on a.hospital_code = b.HOSPITAL_CODE left join reim_util_hospital as c  " & _
                             "on a.hospital_code = c.hospital_code left join reim_processing_status as d on a.control_code = d.control_code" & _
                             " where a.control_code = @control_code "
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = strSQL
                .Parameters.Add("@control_code", SqlDbType.VarChar).Value = ctrl_code
                conn.Open()
                dr = .ExecuteReader
                If dr.HasRows Then
                    While dr.Read
                        If Not IsDBNull(dr("box_1")) Then
                            reim.hospital_bills_from = dr("box_1")
                        End If
                        If Not IsDBNull(dr("box_2")) Then
                            reim.moreover_please_submit = dr("box_2")
                        End If
                        If Not IsDBNull(dr("box_3")) Then
                            reim.copy_of_transmittal_loa = dr("box_3")
                        End If
                        If Not IsDBNull(dr("hospital_name")) Then
                            reim.hospital_bills_from_value = dr("hospital_name")
                        End If
                        If Not IsDBNull(dr("box_2_txt")) Then
                            reim.moreover_value = dr("box_2_txt")
                        End If
                        If Not IsDBNull(dr("loa_no")) Then
                            reim.loa_no = dr("loa_no")
                        End If
                        If Not IsDBNull(dr("lacking_date")) Then
                            reim.lacking_date = dr("lacking_date")
                        End If
                        If Not IsDBNull(dr("memo_released_date")) Then
                            reim.memo_released_date = dr("memo_released_date")
                        End If
                    End While
                End If
            End With
        End Using

        Return reim
    End Function
    Public Function GetReimbDetailsRows(ByVal ctrl_code As String, ByVal mem_code As String) As DataTable
        Dim ds As New DataSet
        Try
            Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
                Dim cmd As New SqlCommand
                Dim da As New SqlDataAdapter

                With cmd
                    .Connection = conn
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "Reim_Reimb_Details_EMED"
                    .Parameters.AddWithValue("@member_code", mem_code)
                    .Parameters.AddWithValue("@control_code", ctrl_code)
                End With
                da.SelectCommand = cmd
                da.Fill(ds)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try

        Return ds.Tables(ds.Tables.Count - 1)
    End Function
    Function Get_Reimb_Details(ByVal ctrl_code As String, ByVal mem_code As String) As ReimbursementDetails
        Dim reim As New ReimbursementDetails

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("reimDB").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            With cmd

                .Connection = conn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "Reim_Get_Reimb_Details_EMED"
                .Parameters.Add("@control_code", SqlDbType.VarChar).Value = ctrl_code
                .Parameters.Add("@member_code", SqlDbType.VarChar).Value = mem_code
                conn.Open()
                dr = .ExecuteReader
                If dr.HasRows Then
                    While dr.Read
                        If Not IsDBNull(dr("total_approved")) Then
                            reim.approved_amount = Format(dr("total_approved"), "#,###.00")
                        End If
                        If Not IsDBNull(dr("check_no")) Then
                            reim.check_no = dr("check_no")
                        End If
                        If Not IsDBNull(dr("check_date")) Then
                            reim.check_date = Format(CDate(dr("check_date")), "MM/dd/yyyy")
                        End If
                        If Not IsDBNull(dr("rmd_hold_rem")) Then
                            reim.remarks = dr("rmd_hold_rem")
                        End If
                        If Not IsDBNull(dr("rmd_lapse_rem")) Then
                            reim.OtherMemoRemarks = dr("rmd_lapse_rem")
                        End If
                        If Not IsDBNull(dr("for_release_date")) Then
                            reim.ready_for_release_date = Format(CDate(dr("for_release_date")), "MM/dd/yyyy")
                        End If
                        If Not IsDBNull(dr("check_released_date")) Then
                            reim.check_release_date = Format(CDate(dr("check_released_date")), "MM/dd/yyyy")
                        End If

                        If Not IsDBNull(dr("total_disapproved")) Then
                            reim.disapproved_amount = Format(dr("total_disapproved"), "##,###.00")
                        End If

                        If Not IsDBNull(dr("disapproved_remarks")) Then
                            reim.reason_of_disapproval = dr("disapproved_remarks")
                        End If
                    End While
                End If
            End With
        End Using

        Return reim
    End Function

    'Public Function GetReimbursementsByDate(ByVal Type As String, ByVal memberCode As String, ByVal sDateFr As String, ByVal sDateTo As String) As List(Of emed_reimbusement_by_date_Result)
    '    Try
    '        Using db = New ClaimsEntities
    '            'Dim qry = From a In db.getMemberReimbursement(Type, memberCode)
    '            '          Select a
    '            Dim qry = From a In db.emed_reimbusement_by_date(Type, memberCode, sDateFr, sDateTo)
    '                      Select a

    '            Return qry.ToList
    '        End Using
    '    Catch ex As Exception
    '        Throw New Exception(ex.Message.ToString())
    '    End Try
    'End Function
    Public Function GetEnrolledDependents(ByVal memberCode As String) As List(Of EnrolledDependents)
        Try
            Using db = New emedicardEntities
                Dim qry = From d In db.GetEnrolledDependent(memberCode)
                          Select d

                Return qry.ToList

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
    End Function

    Public Function GetCancelMemberList(ByVal userid As Integer, ByVal useracct As String, ByVal sStatus As String)

        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emc_cancel_membership_list(userid, useracct, Nothing)
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetMemCancelationRequest(ByVal acct_code As String)

        Try
            Using db = New emedicardEntities

                Dim qry = From p In db.emc_membership_cancel_confiramation_list(acct_code)
                          Select p

                Return ToDataTable(qry.ToList)

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub SaveConsultation(ByVal objConsult As emed_consultation)
        Using db = New emedicardEntities
            Dim objCons As New emed_consultation

            objCons = (From p In db.emed_consultation
                      Where p.ConsultationID = objConsult.ConsultationID).FirstOrDefault

            If Not IsNothing(objCons) Then
                With objCons
                    .ConsultationTitle = objConsult.ConsultationTitle
                    .Consultation = objConsult.Consultation
                    .CreatedDate = objConsult.CreatedDate
                    .UserID = objConsult.UserID
                    .DoctorID = objConsult.DoctorID
                End With
                db.SaveChanges()
            Else
                db.AddToemed_consultation(objConsult)
                db.SaveChanges()
            End If

        End Using
    End Sub

    Public Sub SaveConsultationDtls(ByVal objConsult As emed_consultation_details)
        Try
            Using db = New emedicardEntities

                db.AddToemed_consultation_details(objConsult)
                db.SaveChanges()

            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function ChangeUserEmail(ByVal sEmail As String, ByVal sNewEmail As String)
        Dim bSuccess As Boolean = False

        Try
            Using db = New emedicardEntities
                Dim objMember As New emedicard_emember_users
                objMember = (From p In db.emedicard_emember_users
                             Where p.EmailAddress = sEmail
                             Select p).FirstOrDefault

                If Not objMember Is Nothing Then

                    objMember.Username = Trim(sNewEmail)
                    objMember.EmailAddress = Trim(sNewEmail)
                    db.SaveChanges()
                    bSuccess = True
                End If

            End Using
        Catch ex As Exception
            Throw
        End Try
        Return bSuccess
    End Function
    Public Function GetDoctors()
        Using db = New emedicardEntities
            Dim objDocList As ObjectResult(Of emed_list_of_doctor_Result) = db.emed_list_of_doctor

            Return ToDataTable(objDocList.ToList)
        End Using

    End Function

    Public Function GetECUMemberDetails(ByVal sAccountCode As String, ByVal sMemberCode As String)
        Using db = New MembershipEntities

            Dim objMemDtls As ObjectResult(Of emed_check_member_details_Result) = db.emed_check_member_details(sAccountCode, sMemberCode)

            Return ToDataTable(objMemDtls.ToList)

        End Using
    End Function



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

    Public Function RegisterMember(ByVal user As emedicard_emember_users) As Boolean
        'BY ALLAN ALBACETE 01/29/2013
        Try
            Using db = New emedicardEntities
                user.AccessLevel = 1
                user.DateRegistered = Today()
                user.IsActive = True
                db.AddToemedicard_emember_users(user)
                If db.SaveChanges() > 0 Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    Public Function ActivateUser(ByVal MemberCode As String) As Boolean
        'BY ALLAN ALBACETE 01/29/2013
        Try
            Using db = New emedicardEntities

                Dim qry = (From u In db.emedicard_emember_users
                           Where u.DateActivationConfirmation Is Nothing And u.MemberCode = MemberCode
                           Select u).FirstOrDefault

                If Not qry Is Nothing Then
                    'ACTIVATE user
                    qry.DateActivationConfirmation = Today
                    qry.IsActive = True

                End If
                ' db.emedicard_emember_users.AddObject(qry)
                Dim result As Integer = db.SaveChanges()

                If result > 0 Then Return True

                Return False

            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function UpdateEmemberAccount(ByVal UserCode As String, ByVal email As String, ByVal password As String) As Boolean
        Try
            If password.Length > 0 Then UpdateEmemberPassword(UserCode, password)

            Using db = New emedicardEntities

                Dim qry = (From u In db.emedicard_emember_users
                          Where u.Username = UserCode
                          Select u).FirstOrDefault


                If Not qry Is Nothing Then
                    'Update Email
                    qry.EmailAddress = email
                End If

                Dim result As Integer = db.SaveChanges()
                If result > 0 Then Return True
                Return False

            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    Public Function UpdateEmemberPassword(ByVal Usercode As String, ByVal NewPassword As String) As Boolean
        Try
            Using db = New emedicardEntities
                Dim qry = (From u In db.emedicard_emember_users
                           Where u.Username = Usercode
                           Select u).FirstOrDefault


                If Not qry Is Nothing Then
                    'UPDATE
                    qry.Pword = NewPassword
                End If

                Dim result As Integer = db.SaveChanges()
                If result > 0 Then Return True
                Return False


            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function IsEmailExists(ByVal email As String) As Boolean
        Using db = New emedicardEntities
            Dim qry = (From u In db.emedicard_emember_users
                       Where u.EmailAddress = email
                       Select u).FirstOrDefault


            If Not qry Is Nothing Then Return True

            Return False

        End Using
    End Function

    Public Function IsUserNameExists(ByVal sUserName As String) As Boolean
        Using db = New emedicardEntities
            Dim qry = (From u In db.emedicard_emember_users
                       Where u.Username = sUserName
                       Select u).FirstOrDefault


            If Not qry Is Nothing Then Return True

            Return False

        End Using
    End Function

    Public Function GetRecepients(ByVal sMoudleName As String, ByVal sAppType As String)

        Try
            Using db = New emedicardEntities
                Dim qry = From p In db.emed_email_settings
                          Where p.mail_description = sMoudleName And p.app_type = sAppType
                          Select p

                Return ToDataTable(qry.ToList)
            End Using
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetEmailCC(ByVal ApplicationType As String, ByVal AccountCode As String, ByVal AgentCode As String, ByVal CC As String)
        Dim dt As New DataTable

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("tritonDevEmed").ToString)
            Dim cmd As New SqlCommand
            Dim da As New SqlDataAdapter
            With cmd
                .Connection = conn
                .CommandType = CommandType.StoredProcedure
                .CommandText = "email_cc"
                .Parameters.Add("@app_type", SqlDbType.VarChar).Value = ApplicationType
                .Parameters.Add("@account_code", SqlDbType.VarChar).Value = AccountCode
                .Parameters.Add("@agent_code", SqlDbType.VarChar).Value = AgentCode
                .Parameters.Add("@cc", SqlDbType.VarChar).Value = CC
                'conn.Open()
            End With

            da.SelectCommand = cmd
            da.Fill(dt)

        End Using

        Return dt
    End Function

    Function GetEmail(ByVal user_name As String, ByVal user_type As String, Optional ByVal agent_code As String = Nothing, Optional ByVal account_code As String = Nothing) As String
        Dim sEmail As String = String.Empty

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("tritonDevEmed").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                Select Case user_type
                    Case "emember"
                        .CommandText = "SELECT EmailAddress FROM emed_emember_users where Username = @param"
                        .Parameters.Add("@param", SqlDbType.VarChar).Value = user_name
                    Case "ecorp"
                        If Trim(account_code) <> "" Then
                            .CommandText = "SELECT EmailAddress FROM emed_corporate_users  where AccessLevel = 1 and  RegAccountCode = @param"
                            .Parameters.Add("@param", SqlDbType.VarChar).Value = account_code
                        Else
                            .CommandText = "SELECT EmailAddress FROM emed_corporate_users  where Username = @param"
                            .Parameters.Add("@param", SqlDbType.VarChar).Value = user_name
                        End If
                    Case "eaccount"
                        If Trim(agent_code) <> "" Then
                            .CommandText = "SELECT EmailAddress FROM emed_agent_users  where MainUserID = 0 and AgentCode = @param"
                            .Parameters.Add("@param", SqlDbType.VarChar).Value = agent_code
                        Else
                            .CommandText = "SELECT EmailAddress FROM emed_agent_users  where Username = @param"
                            .Parameters.Add("@param", SqlDbType.VarChar).Value = user_name
                        End If
                End Select

            End With
            Try
                conn.Open()
                dr = cmd.ExecuteReader
                If dr.HasRows Then
                    While dr.Read
                        sEmail = dr("EmailAddress")
                    End While
                End If
            Catch ex As Exception

            Finally
                conn.Close()
            End Try

        End Using

        Return sEmail

    End Function

    Function GetEmail_forcc(ByVal user_type As String, Optional ByVal account_code As String = Nothing) As String
        Dim sEmail As String = String.Empty

        Using conn = New SqlConnection(ConfigurationManager.ConnectionStrings("tritonDevEmed").ToString)
            Dim cmd As New SqlCommand
            Dim dr As SqlDataReader
            With cmd
                .Connection = conn
                .CommandType = CommandType.Text

                Select Case user_type
                    Case "ecorp"
                        If Trim(account_code) <> "" Then
                            .CommandText = "SELECT top 1 a.EmailAddress as 'EmailAddress' FROM emed_agent_users a INNER JOIN emed_eaccount_users_account b ON a.UserID = b.UserID WHERE a.AccessLevel = 2 and b.AccountCode = @param"
                            .Parameters.Add("@param", SqlDbType.VarChar).Value = account_code
                        End If
                    Case "eaccount"
                        If Trim(account_code) <> "" Then
                            .CommandText = "SELECT top 1 a.EmailAddress as 'EmailAddress' FROM emed_agent_users a INNER JOIN emed_eaccount_users_account b ON a.UserID = b.UserID WHERE a.AccessLevel = 2 and b.AccountCode = @param"
                            .Parameters.Add("@param", SqlDbType.VarChar).Value = account_code
                        End If
                End Select

            End With
            Try
                conn.Open()
                dr = cmd.ExecuteReader
                If dr.HasRows Then
                    While dr.Read
                        sEmail = dr("EmailAddress")
                    End While
                End If
            Catch ex As Exception

            Finally
                conn.Close()
            End Try

        End Using

        Return sEmail

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
