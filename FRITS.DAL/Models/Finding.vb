Imports System.ComponentModel.DataAnnotations.Schema

Public Class Finding
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property FindingId As Guid
    Public Property BranchReviewId As Guid
    Public Property Description As String
    Public Property FindingNo As String
    Public Property RiskLevelId As Integer
    Public Property BranchCode As String
    Public Property RiskCategoryId As Integer
    Public Property RiskSubCategory As String
    Public Property ManagementAwareness As String


    Public Overridable Property BranchReview As BranchReview
        Get
            Return m_BranchReview
        End Get
        Set(value As BranchReview)
            m_BranchReview = value
        End Set
    End Property
    Private m_BranchReview As BranchReview


    Public Overridable Property RiskLevel As RiskLevel
        Get
            Return m_RiskLevel
        End Get
        Set(value As RiskLevel)
            m_RiskLevel = value
        End Set
    End Property
    Private m_RiskLevel As RiskLevel


    Public Overridable Property RiskCategory As RiskCategory
        Get
            Return m_RiskCategory
        End Get
        Set(value As RiskCategory)
            m_RiskCategory = value
        End Set
    End Property
    Private m_RiskCategory As RiskCategory





    Public Overridable Property Observations() As ICollection(Of Observation)
        Get
            Return m_Observation
        End Get
        Set(value As ICollection(Of Observation))
            m_Observation = value
        End Set
    End Property
    Private m_Observation As ICollection(Of Observation)



End Class
