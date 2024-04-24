Friend NotInheritable Class eMedicardCollection
    Private Shared _isCorporate As Boolean = True


    Public Shared Property IsCorporate() As Boolean
        Get
            Return _isCorporate
        End Get
        Set(ByVal value As Boolean)
            _isCorporate = value
        End Set
    End Property
End Class
