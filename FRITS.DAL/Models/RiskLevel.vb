Public Class RiskLevel
    Public Property RiskLevelId As Integer
    Public Property Description As String
    Public Property RiskScore As Decimal





    Public Overridable Property Findings() As ICollection(Of Finding)
        Get
            Return m_Finding
        End Get
        Set(value As ICollection(Of Finding))
            m_Finding = value
        End Set
    End Property
    Private m_Finding As ICollection(Of Finding)


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
