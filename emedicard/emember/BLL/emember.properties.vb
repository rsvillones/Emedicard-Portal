Imports emedicard_DAL
Public Class emember
    Private _userCode As String
    Private _password As String
    Private _LoginMessage As String
    Private _FirstName As String
    Private _LastName As String
    Private _MemberName As String
    Private _Age As Integer
    Private _MemberCode As String
    Private _Company As String
    Private _Birthday As Date
    Private _Gender As String
    Private _CivilStatus As String
    Private _AccountStatus As String
    Private _AccountCode As String
    Private _EffectivityDate As String
    Private _DDLimit As Double
    Private _IDRemarks As String
    Private _MemberType As String
    Private _Plan As String
    Private _ValidityDate As String
    Private _PECNonDD As String
    Private _OtherRemarks As String
    Private _Address As String
    Private _Phone As String
    Private _Cellphone As String
    Private _EmailAddress As String
    Private _UsrID As Long
    Private _dtFr As String = String.Empty
    Private _dtTo As String = String.Empty
    Private _memcode As String = String.Empty
    Private _ctrlcode As String = String.Empty

    Public ReadOnly Property EnrolledDependents As List(Of emedicard_DAL.EnrolledDependents)
        Get
            If MemberCode Is Nothing Or MemberCode <> "" Then
                Using db = New ememberDAL
                    Return db.GetEnrolledDependents(MemberCode)
                End Using
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property ReimbursementsOutPatient As List(Of emedicard_DAL.emed_reimbusement_by_date_Result)
        Get
            If MemberCode Is Nothing Or MemberCode <> "" Then
                Using db = New ememberDAL
                    Return db.GetReimbursements("op", MemberCode, Me.Date_From, Me.Date_To)
                End Using
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property ReimbursementsInpatient As List(Of emedicard_DAL.emed_reimbusement_by_date_Result)
        Get
            If MemberCode Is Nothing Or MemberCode <> "" Then
                Using db = New ememberDAL
                    Return db.GetReimbursements("ip", MemberCode, Me.Date_From, Me.Date_To)
                End Using
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property Availments As List(Of emedicard_DAL.Availments)
        Get
            If Not MemberCode Is Nothing Or MemberCode <> "" Then
                Using db As New ememberDAL
                    Return db.GetAvailments(MemberCode)
                End Using
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property DependentDetails As List(Of emedicard_DAL.DependentDetails)
        Get
            If Not MemberCode Is Nothing Or MemberCode <> "" Then
                Dim db As New ememberDAL()
                Return db.GetDependents(MemberCode)
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property DependentPaymentHistory As List(Of emedicard_DAL.sp_intra_payment_history_dep_emed_Result)
        Get
            If Not MemberCode Is Nothing Or MemberCode <> "" Then
                Using db = New ememberDAL
                    Return db.GetDependentPaymentHistory(MemberCode)
                End Using
            End If
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property PrincipalPaymentHistory As List(Of emedicard_DAL.sp_intra_payment_history_prin_emed_Result)
        Get
            If Not MemberCode Is Nothing And MemberCode <> "" Then
                Using db = New ememberDAL
                    Return db.GetPrincipalPaymentHistory(MemberCode)
                End Using
            End If
            Return Nothing
        End Get
    End Property
    Public Property AccountCode As String
        Get
            Return _AccountCode
        End Get
        Set(value As String)
            _AccountCode = value
        End Set
    End Property
    Public Property MemberName As String
        Get
            Return _MemberName
        End Get
        Set(value As String)
            _MemberName = value
        End Set
    End Property
    Public Property LoginMessage As String
        Get
            Return _LoginMessage
        End Get
        Set(value As String)
            _LoginMessage = value
        End Set
    End Property
    Public Property UserCode As String
        Get
            Return _userCode
        End Get
        Set(value As String)
            _userCode = value
        End Set
    End Property
    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property
    Public Property Firstname As String
        Get
            Return _FirstName
        End Get
        Set(value As String)
            _FirstName = value
        End Set
    End Property
    Public Property Lastname As String
        Get
            Return _LastName
        End Get
        Set(value As String)
            _LastName = value
        End Set
    End Property
    Public Property Age As Integer
        Get
            Return _Age
        End Get
        Set(value As Integer)
            _Age = value
        End Set
    End Property
    Public Property MemberCode As String
        Get
            Return _MemberCode
        End Get
        Set(value As String)
            _MemberCode = value
        End Set
    End Property
    Public Property Company As String
        Get
            Return _Company
        End Get
        Set(value As String)
            _Company = value
        End Set
    End Property
    Public Property Birthday As Date
        Get
            Return _Birthday
        End Get
        Set(value As Date)
            _Birthday = value
        End Set
    End Property
    Public Property Gender As String
        Get
            Return IIf(_Gender = "1", "Male", "Female")
        End Get
        Set(value As String)
            _Gender = value
        End Set
    End Property
    Public Property CivilStatus As String
        Get
            Return _CivilStatus
        End Get
        Set(value As String)
            _CivilStatus = value
        End Set
    End Property
    Public Property AccountStatus As String
        Get
            Return _AccountStatus
        End Get
        Set(value As String)
            _AccountStatus = value
        End Set
    End Property
    Public Property EffectivityDate As String
        Get
            Return _EffectivityDate
        End Get
        Set(value As String)
            _EffectivityDate = value
        End Set
    End Property
    Public Property DDLimit As Double
        Get
            Return _DDLimit
        End Get
        Set(value As Double)
            _DDLimit = value
        End Set
    End Property
    Public Property IDRemarks As String
        Get
            Return _IDRemarks
        End Get
        Set(value As String)
            _IDRemarks = value
        End Set
    End Property
    Public Property MemberType As String
        Get
            Return _MemberType
        End Get
        Set(value As String)
            _MemberType = value
        End Set
    End Property
    Public Property Plan As String
        Get
            Return _Plan
        End Get
        Set(value As String)
            _Plan = value
        End Set
    End Property
    Public Property Validitydate As String
        Get
            Return _ValidityDate
        End Get
        Set(value As String)
            _ValidityDate = value
        End Set
    End Property
    Public Property PECNonDD As String
        Get
            Return _PECNonDD
        End Get
        Set(value As String)
            _PECNonDD = value
        End Set
    End Property
    Public Property OtherRemarks As String
        Get
            Return _OtherRemarks
        End Get
        Set(value As String)
            _OtherRemarks = value
        End Set
    End Property
    Public Property Address As String
        Get
            Return _Address
        End Get
        Set(value As String)
            _Address = value
        End Set
    End Property
    Public Property EmailAddress As String
        Get
            Return _EmailAddress
        End Get
        Set(value As String)
            _EmailAddress = value
        End Set
    End Property
    Public Property Phone As String
        Get
            Return _Phone
        End Get
        Set(value As String)
            _Phone = value
        End Set
    End Property
    Public Property Cellphone As String
        Get
            Return _Cellphone
        End Get
        Set(value As String)
            _Cellphone = value
        End Set
    End Property

    Public Property UserID As Long
        Get
            Return _UsrID
        End Get
        Set(ByVal value As Long)
            _UsrID = value
        End Set
    End Property
    Public Property Date_From As String
        Get
            Return _dtFr
        End Get
        Set(ByVal value As String)
            _dtFr = value
        End Set
    End Property

    Public Property Date_To As String
        Get
            Return _dtTo
        End Get
        Set(ByVal value As String)
            _dtTo = value
        End Set
    End Property

    Public Property Control_Code As String
        Get
            Return _ctrlcode
        End Get
        Set(ByVal value As String)
            _ctrlcode = value
        End Set
    End Property


    Private _reason_dis As String
    Public Property reason_of_disapproval As String
        Get
            Return _reason_dis
        End Get
        Set(ByVal value As String)
            _reason_dis = value
        End Set
    End Property

    Private _lacking As String
    Private Property lacking_documents As String
        Get
            Return _lacking
        End Get
        Set(ByVal value As String)
            _lacking = value
        End Set
    End Property

    Private _HospBillFrom As Boolean
    Public Property hospital_bills_from As Boolean
        Get
            Return _HospBillFrom
        End Get
        Set(ByVal value As Boolean)
            _HospBillFrom = value
        End Set
    End Property

    Private _HospBillFromVal As String
    Public Property hospital_bills_from_value
        Get
            Return _HospBillFromVal
        End Get
        Set(ByVal value)
            _HospBillFromVal = value
        End Set
    End Property


    Private _moreover As Boolean
    Public Property moreover_please_submit As Boolean
        Get
            Return _moreover
        End Get
        Set(ByVal value As Boolean)
            _moreover = value
        End Set
    End Property

    Private _moreoverval As String
    Public Property moreover_value As String
        Get
            Return _moreoverval
        End Get
        Set(ByVal value As String)
            _moreoverval = value
        End Set
    End Property

    Private _loano As Boolean
    Public Property copy_of_transmittal_loa As Boolean
        Get
            Return _loano
        End Get
        Set(ByVal value As Boolean)
            _loano = value
        End Set
    End Property

    Private _loaval As String
    Public Property loa_no As String
        Get
            Return _loaval
        End Get
        Set(ByVal value As String)
            _loaval = value
        End Set
    End Property

    Private _app_amt As String
    Public Property approved_amount As String
        Get
            Return _app_amt
        End Get
        Set(ByVal value As String)
            _app_amt = value
        End Set
    End Property

   
    Private _chkno As String
    Public Property check_no As String
        Get
            Return _chkno
        End Get
        Set(ByVal value As String)
            _chkno = value
        End Set
    End Property

    Private _checkdate As String
    Public Property check_date As String
        Get
            Return _checkdate
        End Get
        Set(ByVal value As String)
            _checkdate = value
        End Set
    End Property

    Private _checkreldate As String
    Public Property check_release_date As String
        Get
            Return _checkreldate
        End Get
        Set(ByVal value As String)
            _checkreldate = value
        End Set
    End Property

    Private _readyforreldate As String
    Public Property ready_for_release_date As String
        Get
            Return _readyforreldate
        End Get
        Set(ByVal value As String)
            _readyforreldate = value
        End Set
    End Property

    Private _rem As String
    Public Property remarks As String
        Get
            Return _rem
        End Get
        Set(ByVal value As String)
            _rem = value
        End Set
    End Property

    Private _final_lack_date As String
    Public Property lacking_date As String
        Get
            Return _final_lack_date
        End Get
        Set(ByVal value As String)
            _final_lack_date = value
        End Set
    End Property

    Private _final_denied_date As String
    Public Property denied_date As String
        Get
            Return _final_denied_date
        End Get
        Set(ByVal value As String)
            _final_denied_date = value
        End Set
    End Property

    Private _memo_rel_date As String
    Public Property memo_released_date As String
        Get
            Return _memo_rel_date
        End Get
        Set(ByVal value As String)
            _memo_rel_date = value
        End Set
    End Property
    Private _otherMemRem As String
    Public Property OtherMemoRemarks As String
        Get
            Return _otherMemRem
        End Get
        Set(value As String)
            _otherMemRem = value
        End Set
    End Property
End Class
