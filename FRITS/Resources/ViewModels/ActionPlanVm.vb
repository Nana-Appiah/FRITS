Public Class ActionPlanVm
    Public Property ActionPlanId As Guid
    Public Property ObservationId As Guid
    Public Property Description As String
    Public Property ScheduleOfficerResponse As String


    Public Property ResolutionTiming As DateTime
    Public Property ObservationStatusId As Integer
    Public Property ReferenceOfficerIds As String
    Public Property AssignedEmployeeNames As String

    Sub New()
        ResolutionTiming = DateTime.UtcNow
    End Sub
    Private Function IsNew() As Boolean
        Return ActionPlanId = New Guid
    End Function

    Public Function IsValid() As Boolean
        If IsNew() Then
            Return (ActionPlanId = New Guid And ObservationId <> New Guid And Not String.IsNullOrWhiteSpace(Description))
        Else
            Return (ActionPlanId <> New Guid And ObservationId <> New Guid And Not String.IsNullOrWhiteSpace(Description))
        End If

    End Function


End Class
