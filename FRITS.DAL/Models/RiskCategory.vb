Public Class RiskCategory
    Public Property RiskCategoryId As Integer
    Public Property RiskCategoryDesc As String


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
