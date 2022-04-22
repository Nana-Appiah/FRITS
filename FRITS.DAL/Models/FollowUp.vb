Imports System.ComponentModel.DataAnnotations.Schema

Public Class FollowUp
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property FollowUpId As Guid
    Public Property FollowUpDate As DateTime
    Public Property Description As String




    Public Overridable Property FollowUpDetails() As ICollection(Of FollowUpDetail)
        Get
            Return m_FollowUpDetail
        End Get
        Set(value As ICollection(Of FollowUpDetail))
            m_FollowUpDetail = value
        End Set
    End Property
    Private m_FollowUpDetail As ICollection(Of FollowUpDetail)


End Class
