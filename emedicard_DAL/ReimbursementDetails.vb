Public Class ReimbursementDetails

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

    Private _disapp_amt As String
    Public Property disapproved_amount As String
        Get
            Return _disapp_amt
        End Get
        Set(ByVal value As String)
            _disapp_amt = value
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
    Private _otherRem As String
    Public Property OtherMemoRemarks As String
        Get
            Return _otherRem
        End Get
        Set(value As String)
            _otherRem = value
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
End Class
