Public Class ReviewVm
    Public Property ReviewId As Guid
    Public Property Description As String
    Public Property ReviewCode As String
    Public Property Findings As List(Of FindingVm)
    Public Property IsAuthorised As Boolean
    Public Property IsClosed As Boolean
    Public Property ClosedById As Integer
    Public Property ClosedDate As Date

    Public Property ClosedByName As String


    Sub New()
        Findings = New List(Of FindingVm)
    End Sub



    Public ReadOnly Property isNew As Boolean
        Get
            Return ReviewId = New Guid
        End Get
    End Property



    Public Function IsValid() As Boolean
        If isNew Then
            Return Not String.IsNullOrWhiteSpace(Description) And Not String.IsNullOrWhiteSpace(ReviewCode)
        Else
            Return Not String.IsNullOrWhiteSpace(Description) And Not String.IsNullOrWhiteSpace(ReviewCode) And Not ReviewId = New Guid
        End If
    End Function


End Class
