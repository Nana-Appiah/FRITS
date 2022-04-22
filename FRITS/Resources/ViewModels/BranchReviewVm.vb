Public Class BranchReviewVm
    Public Property BranchReviewId As Guid
    Public Property EmployeeId As Integer
    Public Property BranchName As String
    Public Property BranchCode As String
    Public Property EmployeeName As String
    Public Property ReviewId As Guid
    Public Property ReviewFrom As DateTime
    Public Property ReviewTo As DateTime
    Public Property ReviewInstruction As String
    Public Property IsSubmitted As Boolean
    Public Property IsClosed As Boolean
    Public Property IsAuthorised As Boolean
    Public Property ReviewName As String
    Public Property ReviewCode As String

    Sub New()
        BranchReviewId = New Guid
    End Sub


    Private Function Valid_DateRange() As Boolean
        If Not IsDate(ReviewFrom) Or Not IsDate(ReviewTo) Then Return False
        If CDate(ReviewTo) < CDate(ReviewFrom) Then Return False
        Return True
    End Function


    Public Function IsValid() As Boolean
        Return (EmployeeId > 0 And Not String.IsNullOrWhiteSpace(BranchCode) And Not String.IsNullOrWhiteSpace(ReviewId.ToString) And Valid_DateRange())
    End Function

End Class
