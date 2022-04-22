Imports System.ComponentModel.DataAnnotations.Schema

Public Class ActionPlan
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property ActionPlanId As Guid
    Public Property ObservationId As Guid
    Public Property Description As String
    Public Property ScheduleOfficerResponse As String

    Public Overridable Property Observation As Observation
        Get
            Return m_observation
        End Get
        Set(value As Observation)
            m_observation = value
        End Set
    End Property
    Private m_observation As Observation

End Class
