Public Class RiskCategoryVm
    Public Property RiskCategoryId As Integer
    Public Property RiskCategoryDesc As String

    Public ReadOnly Property isNew As Boolean
        Get
            Return RiskCategoryId <= 0
        End Get
    End Property



    Public Function IsValid() As Boolean
        If isNew Then
            Return Not String.IsNullOrWhiteSpace(RiskCategoryDesc)
        Else
            Return Not String.IsNullOrWhiteSpace(RiskCategoryDesc) And Not RiskCategoryId <= 0
        End If
    End Function
End Class
