Imports System.ComponentModel.DataAnnotations.Schema

Public Class Observation
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property ObservationId As Guid
    Public Property FindingId As Guid
    Public Property ObservationDate As DateTime
    Public Property ObservationNo As String
    Public Property Description As String
    Public Property Implication As String
    Public Property RootCauseAnalysis As String
    Public Property ResolutionTiming As DateTime
    Public Property ObservationStatusId As Integer
    Public Property RiskLevelId As Integer
    Public Property ReferenceOfficerIds As String
    Public Property ManagementResponse As String
    Public Property Assumptions As String
    Public Property MitigatingControl As String



    Public Overridable Property Finding As Finding
        Get
            Return m_finding
        End Get
        Set(value As Finding)
            m_finding = value
        End Set
    End Property
    Private m_finding As Finding




    Public Overridable Property RiskLevel As RiskLevel
        Get
            Return m_risklevel
        End Get
        Set(value As RiskLevel)
            m_risklevel = value
        End Set
    End Property
    Private m_risklevel As RiskLevel


    Public Overridable Property ObservationStatus As ObservationStatus
        Get
            Return m_ObservationStatus
        End Get
        Set(value As ObservationStatus)
            m_ObservationStatus = value
        End Set
    End Property
    Private m_ObservationStatus As ObservationStatus



    Public Overridable Property ActionPlans() As ICollection(Of ActionPlan)
        Get
            Return m_ActionPlan
        End Get
        Set(value As ICollection(Of ActionPlan))
            m_ActionPlan = value
        End Set
    End Property
    Private m_ActionPlan As ICollection(Of ActionPlan)


    Public Overridable Property FollowUpDetails() As ICollection(Of FollowUpDetail)
        Get
            Return m_FollowUpDetail
        End Get
        Set(value As ICollection(Of FollowUpDetail))
            m_FollowUpDetail = value
        End Set
    End Property
    Private m_FollowUpDetail As ICollection(Of FollowUpDetail)


    Public Overridable Property CorrectiveActions() As ICollection(Of CorrectiveAction)
        Get
            Return m_CorrectiveAction
        End Get
        Set(value As ICollection(Of CorrectiveAction))
            m_CorrectiveAction = value
        End Set
    End Property
    Private m_CorrectiveAction As ICollection(Of CorrectiveAction)





    'Blindly Has Recommendations By RecommendationTypeReference  RecommendationTypes.Observation_Recommendation



End Class
