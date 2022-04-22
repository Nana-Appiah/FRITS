Public Class ObservationStatus
    Public Property ObservationStatusId As Integer
    Public Property Description As String
    Public Property StatusCode As String
    Public Property Narration As String
    Public Property IsEnabled As Boolean

    Public Overridable Property Observations() As ICollection(Of Observation)
        Get
            Return m_Observation
        End Get
        Set(value As ICollection(Of Observation))
            m_Observation = value
        End Set
    End Property
    Private m_Observation As ICollection(Of Observation)


    Public Overridable Property CorrectiveActions() As ICollection(Of CorrectiveAction)
        Get
            Return m_CorrectiveAction
        End Get
        Set(value As ICollection(Of CorrectiveAction))
            m_CorrectiveAction = value
        End Set
    End Property
    Private m_CorrectiveAction As ICollection(Of CorrectiveAction)



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
