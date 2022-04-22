Public Class FindingVm

    Public Property FindingId As Guid
    Public Property FindingNo As String
    Public Property BranchReviewId As Guid
    Public Property Description As String
    Public Property RiskLevelId As Integer
    Public Property ReferenceOfficerIds As String
    Public Property BranchCode As String
    Public Property RiskCategoryId As Integer
    Public Property RiskSubCategory As String
    Public Property ManagementAwareness As String
    Public Property Observations As List(Of ObservationVm)
    Public Property RiskLevelName As String
    Public Property RiskCategoryName As String


    Public Property FindingFiles As List(Of ReviewFileVm)




    Sub New()
        FindingId = New Guid ' Guid.NewGuid()
        Observations = New List(Of ObservationVm)
        ReferenceOfficerIds = 0
        FindingFiles = New List(Of ReviewFileVm)
    End Sub


    Public ReadOnly Property ObservationCount As Integer
        Get
            Return Observations.Count()
        End Get
    End Property


    Public ReadOnly Property isNew As Boolean
        Get
            Return FindingId = New Guid
        End Get
    End Property


    Public Function IsValid() As Boolean
        If isNew Then
            Return Not String.IsNullOrWhiteSpace(Description) And Not BranchReviewId = New Guid And Not RiskLevelId <= 0 And FindingId = New Guid And RiskCategoryId > 0 And Not String.IsNullOrWhiteSpace(RiskSubCategory) And Not String.IsNullOrWhiteSpace(ManagementAwareness) And ObservationCount > 0 And Not String.IsNullOrWhiteSpace(FindingNo)
        Else
            Return Not String.IsNullOrWhiteSpace(Description) And Not BranchReviewId = New Guid And Not RiskLevelId <= 0 And Not FindingId = New Guid And RiskCategoryId > 0 And Not String.IsNullOrWhiteSpace(RiskSubCategory) And Not String.IsNullOrWhiteSpace(ManagementAwareness) And ObservationCount > 0 And Not String.IsNullOrWhiteSpace(FindingNo)
        End If
    End Function




End Class
