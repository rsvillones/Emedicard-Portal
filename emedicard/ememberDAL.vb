Imports emedicard_DAL
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.EntityClient
Imports System.Data.Objects
Imports System.Linq

Imports System.Transactions
Public Class ememberDAL
    Implements IDisposable


    'Check Username
    Public Function CheckEMemberUserName(usercode As String) As Boolean
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
    Public Function CheckEMemberPassword(usercode As String, password As String) As Boolean
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

    Public Function CheckEMemberCode(memberCode As String) As Boolean
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

    Public Function ValidateMember(ByVal MemberCode As String, Birthdate As Date)
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

    Public Function GetDependentPaymentHistory(ByVal MemberCode As String) As List(Of DependentPaymentHistory)
        Try
            Using db = New MembershipEntities
                Dim qry = From p In db.GetDependentPaymentHistory(MemberCode)
                          Select p

                Dim result As List(Of DependentPaymentHistory) = qry.ToList
                Return result
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try
    End Function

    Public Function GetPrincipalPaymentHistory(ByVal memberCode As String) As List(Of PrinicipalPaymentHistory)
        Try
            Using db = New MembershipEntities
                Dim qry = From p In db.GetPrincipalPaymentHistory(memberCode)
                          Select p

                Dim result As List(Of PrinicipalPaymentHistory) = qry.ToList

                Return result
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try
    End Function

    Public Function GetAvailments(ByVal MemberCode As String) As List(Of Availments)
        Try
            'DateFrom As Date, DateTo As Date
            Using db = New ClaimsEntities
                Dim qry = From a In db.GetAvailments(MemberCode)
                          Select a

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
    End Function

    Public Function GetReimbursements(ByVal Type As String, ByVal memberCode As String) As List(Of MemberReimbursement)
        Try
            Using db = New ClaimsEntities
                Dim qry = From a In db.getMemberReimbursement(Type, memberCode)
                          Select a

                Return qry.ToList
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString())
        End Try
    End Function

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
    Public Function GetDoctors()
        Using db = New emedicardEntities
            Dim objDocList As ObjectResult(Of emed_list_of_doctor_Result) = db.emed_list_of_doctor

            Return ToDataTable(objDocList.ToList)
        End Using

    End Function

    Public Function ToDataTable(Of T)(ByVal list As List(Of T)) As DataTable
        Dim _resultDataTable As New DataTable("results")
        Dim _resultDataRow As DataRow = Nothing
        Dim dt As New DataTable
        Dim _itemProperties() As System.Reflection.PropertyInfo

        ' Meta Data.
        ' Each item property becomes a column in the table 
        ' Build an array of Property Getters, one for each Property 
        ' in the item class. Can pass anything as [item] it is just a 
        ' place holder parameter, later we will invoke it with the
        ' correct item. This code assumes the runtime does not change
        ' the ORDER in which the proprties are returned.
        If list Is Nothing Then
        Else
            If list.Count > 0 Then
                _itemProperties = list.Item(0).GetType().GetProperties()
                'MsgBox(_itemProperties.Length)
                For Each p As System.Reflection.PropertyInfo In _itemProperties

                    'dt.Columns.Add(p.Name, _
                    '          p.GetGetMethod.ReturnType())
                    dt.Columns.Add(p.Name, _
                               If(Nullable.GetUnderlyingType(p.PropertyType), p.PropertyType))
                Next

                For Each item As T In list

                    ' Get the data from this item into a DataRow
                    ' then add the DataRow to the DataTable.
                    ' Eeach items property becomes a colunm.
                    '
                    _itemProperties = item.GetType().GetProperties()
                    _resultDataRow = dt.NewRow()
                    For Each p As System.Reflection.PropertyInfo In _itemProperties
                        '_resultDataRow(p.Name) = p.GetValue(item, Nothing)
                        If p.GetValue(item, Nothing) = Nothing Then
                            _resultDataRow(p.Name) = DBNull.Value
                        Else
                            _resultDataRow(p.Name) = IIf(Not IsDBNull(p.GetValue(item, Nothing)), p.GetValue(item, Nothing), DBNull.Value)
                        End If

                    Next
                    dt.Rows.Add(_resultDataRow)
                Next
            End If
        End If
        ''MsgBox(dt.Rows.Count)
        Return dt
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

    Public Function ActivateUser(MemberCode As String) As Boolean
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

    Public Function UpdateEmemberAccount(UserCode As String, email As String, password As String) As Boolean
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
    Public Function UpdateEmemberPassword(Usercode As String, NewPassword As String) As Boolean
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
