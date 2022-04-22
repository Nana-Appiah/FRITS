Imports System.ComponentModel.DataAnnotations.Schema

Public Class Review
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property ReviewId As Guid
    Public Property Description As String
    Public Property ReviewCode As String

    Public Property IsClosed As Boolean
    Public Property ClosedById As Integer
    Public Property ClosedDate As Date

    Public Overridable Property BranchReviews() As ICollection(Of BranchReview)
        Get
            Return m_EmployeeReview
        End Get
        Set(value As ICollection(Of BranchReview))
            m_EmployeeReview = value
        End Set
    End Property
    Private m_EmployeeReview As ICollection(Of BranchReview)


End Class
