Public Class RecordFilterOption
    Public Property fromDate As Date
    Public Property toDate As Date
    Public Property branchCode As String
    Public Property reviewName As String
    Public Property employeeId As Integer
    Public Property reviewCode As String
    Public Property reviewId As Guid
    Public Property isSubmitted As Boolean

    Sub New()
        fromDate = New Date
        toDate = New Date
        branchCode = String.Empty
        reviewName = String.Empty
        employeeId = 0
        reviewCode = String.Empty
        reviewId = New Guid
    End Sub
End Class
