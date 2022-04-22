Public Class CorrectiveActionVm
    Public Property CorrectiveActionId As Guid
    Public Property ObservationId As Guid
    Public Property CorrectionDate As DateTime
    Public Property Remarks As String
    Public Property ObservationStatusId As Integer



    Public Property ObservationStatusName As String

    Public ReadOnly Property isNew As Boolean
        Get
            Return CorrectiveActionId = New Guid
        End Get
    End Property


    Public Function IsValid() As Boolean

        Dim _check = (ObservationId <> New Guid And
            CorrectionDate <> New Date And
            Not String.IsNullOrWhiteSpace(Remarks) And
            ObservationStatusId > 0)

        If Not _check Then Return False

        If isNew Then
            Return CorrectiveActionId = New Guid
        Else
            Return CorrectiveActionId <> New Guid
        End If
    End Function

End Class
