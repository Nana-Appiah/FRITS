Public Class FollowUpVm
    Public Property FollowUpId As Guid
    Public Property FollowUpDate As DateTime
    Public Property Description As String
    Public Property FollowUpDetails As List(Of FollowUpDetailVm)

    Sub New()
        FollowUpDate = DateTime.UtcNow
        FollowUpDetails = New List(Of FollowUpDetailVm)
    End Sub





    Public ReadOnly Property isNew As Boolean
        Get
            Return FollowUpId = New Guid
        End Get
    End Property



    Public Function IsValid() As Boolean

        Dim _check = (FollowUpDate <> New Date And
            Not String.IsNullOrWhiteSpace(Description) And
            FollowUpDetails.Count > 0)

        If Not _check Then Return False

        If isNew Then
            Return FollowUpId = New Guid
        Else
            Return FollowUpId <> New Guid
        End If
    End Function

End Class
