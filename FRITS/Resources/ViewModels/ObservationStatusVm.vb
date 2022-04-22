Public Class ObservationStatusVm
    Public Property ObservationStatusId As Integer
    Public Property Description As String
    Public Property StatusCode As String
    Public Property IsEnabled As Boolean
    Public Property Narration As String

    Public ReadOnly Property isNew As Boolean
        Get
            Return ObservationStatusId <= 0
        End Get
    End Property



    Public Function IsValid() As Boolean
        If isNew Then
            Return Not String.IsNullOrWhiteSpace(Description) And Not String.IsNullOrWhiteSpace(StatusCode)
        Else
            Return Not String.IsNullOrWhiteSpace(Description) And Not String.IsNullOrWhiteSpace(StatusCode) And Not ObservationStatusId <= 0
        End If
    End Function


End Class
