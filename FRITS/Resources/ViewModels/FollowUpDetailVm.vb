Public Class FollowUpDetailVm
    Public Property FollowUpDetailId As Guid
    Public Property FollowUpId As Guid
    Public Property ObservationId As Guid
    Public Property Remarks As String
    Public Property ObservationStatusId As Integer



    Public Property FollowUpDate As DateTime
    Public Property FollowUpDescription As String
    Public Property ObservationStatusName As String
    Public Property FollowUpByEmployeeName As String



    Sub New()
        FollowUpDate = DateTime.UtcNow
    End Sub



    Public ReadOnly Property isNew As Boolean
        Get
            Return FollowUpDetailId = New Guid
        End Get
    End Property



    Public Function IsValid() As Boolean

        Dim _check = (FollowUpId <> New Guid And
            ObservationId <> New Guid And
            Not String.IsNullOrWhiteSpace(Remarks))

        If Not _check Then Return False

        If isNew Then
            Return FollowUpDetailId = New Guid
        Else
            Return FollowUpDetailId <> New Guid
        End If
    End Function




End Class
