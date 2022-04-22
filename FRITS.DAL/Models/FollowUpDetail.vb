Imports System.ComponentModel.DataAnnotations.Schema

Public Class FollowUpDetail
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property FollowUpDetailId As Guid
    Public Property FollowUpId As Guid
    Public Property ObservationId As Guid
    Public Property Remarks As String
    Public Property ObservationStatusId As Integer



    Public Overridable Property FollowUp As FollowUp
        Get
            Return m_FollowUp
        End Get
        Set(value As FollowUp)
            m_FollowUp = value
        End Set
    End Property
    Private m_FollowUp As FollowUp

    Public Overridable Property Observation As Observation
        Get
            Return m_Observation
        End Get
        Set(value As Observation)
            m_Observation = value
        End Set
    End Property
    Private m_Observation As Observation


    Public Overridable Property ObservationStatus As ObservationStatus
        Get
            Return m_ObservationStatus
        End Get
        Set(value As ObservationStatus)
            m_ObservationStatus = value
        End Set
    End Property
    Private m_ObservationStatus As ObservationStatus

End Class
