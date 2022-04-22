Public Class RiskLevelVm
    Public Property RiskLevelId As Integer
    Public Property Description As String
    Public Property RiskScore As Decimal

    Public ReadOnly Property isNew As Boolean
        Get
            Return RiskLevelId <= 0
        End Get
    End Property



    Public Function IsValid() As Boolean
        If isNew Then
            Return Not String.IsNullOrWhiteSpace(Description) And RiskScore > 0
        Else
            Return Not String.IsNullOrWhiteSpace(Description) And RiskScore > 0 And Not RiskLevelId <= 0
        End If
    End Function
End Class
