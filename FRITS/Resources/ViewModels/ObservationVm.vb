Public Class ObservationVm
    Public Property ObservationId As Guid
    Public Property FindingId As Guid
    Public Property ObservationDate As DateTime
    Public Property ObservationNo As String
    Public Property RiskLevelId As Integer
    Public Property Description As String
    Public Property ResolutionTiming As DateTime
    Public Property ObservationStatusId As Integer
    Public Property Implication As String
    Public Property RootCauseAnalysis As String
    Public Property ActionPlans As List(Of ActionPlanVm)
    Public Property FollowUpDetails As List(Of FollowUpDetailVm)
    Public Property Recommendations As List(Of RecommendationVm)
    Public Property CorrectiveActions As List(Of CorrectiveActionVm)
    Public Property ManagementResponse As String
    Public Property Assumptions As String
    Public Property MitigatingControl As String


    Public Property ObservationFiles As List(Of ReviewFileVm)


    Sub New()
        ObservationId = New Guid
        ActionPlans = New List(Of ActionPlanVm)
        FollowUpDetails = New List(Of FollowUpDetailVm)
        CorrectiveActions = New List(Of CorrectiveActionVm)
        ObservationDate = DateTime.UtcNow
        ObservationFiles = New List(Of ReviewFileVm)
    End Sub



    Public ReadOnly Property isNew As Boolean
        Get
            Return ObservationId = New Guid
        End Get
    End Property



    Public Function IsValid() As Boolean

        Dim _check = (FindingId <> New Guid And
            ObservationDate <> New Date And
            Not String.IsNullOrWhiteSpace(Description) And
            ObservationStatusId > 0 And
            ResolutionTiming <> New Date And
            Recommendations.Count > 0 And
            ActionPlans.Count > 0)

        If Not _check Then Return False

        If isNew Then
            Return ObservationId = New Guid
        Else
            Return ObservationId <> New Guid
        End If
    End Function


End Class
