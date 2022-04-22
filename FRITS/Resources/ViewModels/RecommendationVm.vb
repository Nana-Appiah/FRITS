Public Class RecommendationVm
    Public Property RecommendationId As Guid
    Public Property RecommendationType As Short
    Public Property TypeReferenceId As Guid
    Public Property Description As String

    Sub New(ByVal recommenType As RecommendationTypes)
        Me.RecommendationType = recommenType
        RecommendationId = New Guid
    End Sub

    Sub New()

    End Sub

    Public ReadOnly Property isNew As Boolean
        Get
            Return RecommendationId = New Guid
        End Get
    End Property



    Public Function IsValid() As Boolean

        Dim _check = (TypeReferenceId <> New Guid And
            Not String.IsNullOrWhiteSpace(Description) And
            RecommendationType >= 0)

        If Not _check Then Return False

        If isNew Then
            Return RecommendationId = New Guid
        Else
            Return RecommendationId <> New Guid
        End If
    End Function


End Class
