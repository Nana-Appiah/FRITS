Imports System.ComponentModel.DataAnnotations.Schema

Public Class BranchReview
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property BranchReviewId As Guid
    Public Property ReviewId As Guid
    Public Property EmployeeId As Integer
    Public Property DateAssigned As Date
    Public Property BranchCode As String
    Public Property IsSubmitted As Boolean
    Public Property ReviewInstruction As String
    Public Property StartDate As Date
    Public Property EndDate As Date

    Public Property IsClosed As Boolean

    Public Overridable Property Review As Review
        Get
            Return m_Review
        End Get
        Set(value As Review)
            m_Review = value
        End Set
    End Property
    Private m_Review As Review



    Public Overridable Property Findings() As ICollection(Of Finding)
        Get
            Return m_Finding
        End Get
        Set(value As ICollection(Of Finding))
            m_Finding = value
        End Set
    End Property
    Private m_Finding As ICollection(Of Finding)








End Class
