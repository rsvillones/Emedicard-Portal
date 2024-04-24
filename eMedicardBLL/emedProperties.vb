Public Class emedProperties


    Public Sub New()

    End Sub

    Private _DocEmail As String
    Public Property doctor_email As String
        Get
            Return _DocEmail
        End Get
        Set(ByVal value As String)
            _DocEmail = value
        End Set
    End Property

    Private _docfname As String
    Public Property doctor_firstname As String
        Get
            Return _docfname
        End Get
        Set(ByVal value As String)
            _docfname = value
        End Set
    End Property

    Private _doclname As String
    Public Property doctor_lastname As String
        Get
            Return _doclname
        End Get
        Set(ByVal value As String)
            _doclname = value
        End Set
    End Property

    Private _uname As String
    Public Property UserName As String
        Get
            Return _uname
        End Get
        Set(ByVal value As String)
            _uname = value
        End Set
    End Property

    Private _upass As String
    Public Property PWord As String
        Get
            Return _upass
        End Get
        Set(ByVal value As String)
            _upass = value
        End Set
    End Property

    Private _curremail As String
    Public Property EmailAddress As String
        Get
            Return _curremail
        End Get
        Set(ByVal value As String)
            _curremail = value
        End Set
    End Property

    Private _newemail As String
    Public Property NewEmailAddress As String
        Get
            Return _newemail
        End Get
        Set(ByVal value As String)
            _newemail = value
        End Set
    End Property

    Private _apptype As String
    Public Property application_type
        Get
            Return _apptype
        End Get
        Set(ByVal value)
            _apptype = value
        End Set
    End Property

    Private _modname As String
    Public Property mail_description
        Get
            Return _modname
        End Get
        Set(ByVal value)
            _modname = value
        End Set
    End Property

    Private _sendtotag As String
    Public Property send_to_tag As String
        Get
            Return _sendtotag
        End Get
        Set(ByVal value As String)
            _sendtotag = value
        End Set
    End Property

    Private _sendtomail As String
    Public Property send_to_email As String
        Get
            Return _sendtomail
        End Get
        Set(ByVal value As String)
            _sendtomail = value
        End Set
    End Property

    Private _cc As String
    Public Property cc As String
        Get
            Return _cc
        End Get
        Set(ByVal value As String)
            _cc = value
        End Set
    End Property

    Private _bcc As String
    Public Property bcc As String
        Get
            Return _bcc
        End Get
        Set(ByVal value As String)
            _bcc = value
        End Set
    End Property

    Private _self As Boolean
    Public Property self As Boolean
        Get
            Return _self
        End Get
        Set(ByVal value As Boolean)
            _self = value
        End Set
    End Property

    Private _acccode As String
    Public Property prm_account_code As String
        Get
            Return _acccode
        End Get
        Set(ByVal value As String)
            _acccode = value
        End Set
    End Property

    Private _agntcode As String
    Public Property prm_agent_code As String
        Get
            Return _agntcode
        End Get
        Set(ByVal value As String)
            _agntcode = value
        End Set
    End Property

End Class
