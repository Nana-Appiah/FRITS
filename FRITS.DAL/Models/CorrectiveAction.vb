Imports System.ComponentModel.DataAnnotations.Schema

Public Class CorrectiveAction
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property CorrectiveActionId As Guid
    Public Property ObservationId As Guid
    Public Property CorrectionDate As DateTime
    Public Property Remarks As String
    Public Property ObservationStatusId As Integer


    Public Overridable Property ObservationStatus As ObservationStatus
        Get
            Return m_ObservationStatus
        End Get
        Set(value As ObservationStatus)
            m_ObservationStatus = value
        End Set
    End Property
    Private m_ObservationStatus As ObservationStatus

    Public Overridable Property Observation As Observation
        Get
            Return m_Observation
        End Get
        Set(value As Observation)
            m_Observation = value
        End Set
    End Property
    Private m_Observation As Observation



End Class
