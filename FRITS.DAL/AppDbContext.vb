Imports System.Data.Entity

Public Class AppDbContext
    Inherits DbContext

    Property loggedOnUser As Integer
    Public Sub New(Optional ByVal userIndx As Integer = 1)
        MyBase.New("name=FRITSConnection")
        loggedOnUser = userIndx
    End Sub


    Public Sub New(ByVal userIndx As Integer, ByVal useTrans As System.Data.Common.DbTransaction)
        MyBase.New("name=FRITSConnection")
        loggedOnUser = userIndx
        Me.Database.UseTransaction(useTrans)
    End Sub

    Public Sub New()
        MyBase.New("name=FRITSConnection")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)

        modelBuilder.Configurations.Add(New ActionPlanDbConfig())
        modelBuilder.Configurations.Add(New ObservationStatusDbConfig())
        modelBuilder.Configurations.Add(New RiskLevelDbConfig())
        modelBuilder.Configurations.Add(New RiskCategoryConfig())
        modelBuilder.Configurations.Add(New ReviewDbConfig())
        modelBuilder.Configurations.Add(New BranchReviewDbConfig())
        modelBuilder.Configurations.Add(New FindingDbConfig())
        modelBuilder.Configurations.Add(New FollowUpDbConfig())
        modelBuilder.Configurations.Add(New FollowUpDetailDbConfig())
        modelBuilder.Configurations.Add(New ObservationDbConfig())
        modelBuilder.Configurations.Add(New RecommendationDbConfig())
        modelBuilder.Configurations.Add(New CorrectiveActionDbConfig())
        modelBuilder.Configurations.Add(New ReviewFileDbConfig())

        MyBase.OnModelCreating(modelBuilder)

    End Sub

    Public Overrides Function SaveChanges() As Integer

        Dim _entryDateTime As DateTime = Convert.ToDateTime(System.DateTime.Now())
        For Each objEntity In ChangeTracker.Entries.Where(Function(e) TypeOf (e.Entity) Is IEntityBase)

            If objEntity.State = EntityState.Modified Then
                Dim entity = DirectCast(objEntity.Entity, IEntityBase)
                If Not IsNothing(entity) Then
                    entity.LastUpdatedDate = _entryDateTime
                    entity.LastUpdatedByID = loggedOnUser

                    If entity.IsSelfAuthorise Then
                        entity.IsAuthorised = True
                        entity.AuthorisedByID = loggedOnUser
                        entity.AuthorisedDate = _entryDateTime
                    End If
                End If
            End If

            If objEntity.State = EntityState.Deleted Then
                Dim entity = DirectCast(objEntity.Entity, IEntityBase)
                If (entity.DeletedByID = 0) AndAlso (entity.DeletedDate = Convert.ToDateTime("0001-01-01")) Then
                    entity.DeletedDate = _entryDateTime
                    entity.DeletedByID = loggedOnUser
                    objEntity.State = EntityState.Modified
                ElseIf Not (IsNothing(entity)) Then
                    objEntity.State = EntityState.Deleted
                End If
            End If

            If objEntity.State = EntityState.Added Then
                Dim entity = DirectCast(objEntity.Entity, IEntityBase)
                If Not IsNothing(entity) Then
                    entity.CreatedDate = _entryDateTime
                    entity.CreatedByID = loggedOnUser
                    entity.DeletedDate = Nothing
                    entity.LastUpdatedDate = Nothing
                    entity.LastUpdatedByID = Nothing
                    entity.DeletedByID = Nothing

                    If entity.IsSelfAuthorise Then
                        entity.IsAuthorised = True
                        entity.AuthorisedByID = loggedOnUser
                        entity.AuthorisedDate = _entryDateTime
                    End If

                End If
            End If

        Next

        Return MyBase.SaveChanges()

    End Function


    Public Property Reviews() As DbSet(Of Review)
        Get
            Return m_Review
        End Get
        Set(value As DbSet(Of Review))
            m_Review = value
        End Set
    End Property
    Private m_Review As DbSet(Of Review)

    Public Property Findings() As DbSet(Of Finding)
        Get
            Return m_Finding
        End Get
        Set(value As DbSet(Of Finding))
            m_Finding = value
        End Set
    End Property
    Private m_Finding As DbSet(Of Finding)

    Public Property Observations() As DbSet(Of Observation)
        Get
            Return m_Observation
        End Get
        Set(value As DbSet(Of Observation))
            m_Observation = value
        End Set
    End Property
    Private m_Observation As DbSet(Of Observation)

    Public Property ActionPlans() As DbSet(Of ActionPlan)
        Get
            Return m_ActionPlan
        End Get
        Set(value As DbSet(Of ActionPlan))
            m_ActionPlan = value
        End Set
    End Property
    Private m_ActionPlan As DbSet(Of ActionPlan)

    Public Property Recommendations() As DbSet(Of Recommendation)
        Get
            Return m_Recommendation
        End Get
        Set(value As DbSet(Of Recommendation))
            m_Recommendation = value
        End Set
    End Property
    Private m_Recommendation As DbSet(Of Recommendation)

    Public Property FollowUps() As DbSet(Of FollowUp)
        Get
            Return m_FollowUp
        End Get
        Set(value As DbSet(Of FollowUp))
            m_FollowUp = value
        End Set
    End Property
    Private m_FollowUp As DbSet(Of FollowUp)

    Public Property FollowUpDetails() As DbSet(Of FollowUpDetail)
        Get
            Return m_FollowUpDetail
        End Get
        Set(value As DbSet(Of FollowUpDetail))
            m_FollowUpDetail = value
        End Set
    End Property
    Private m_FollowUpDetail As DbSet(Of FollowUpDetail)

    Public Property RiskLevels() As DbSet(Of RiskLevel)
        Get
            Return m_RiskLevel
        End Get
        Set(value As DbSet(Of RiskLevel))
            m_RiskLevel = value
        End Set
    End Property
    Private m_RiskLevel As DbSet(Of RiskLevel)

    Public Property RiskCategories() As DbSet(Of RiskCategory)
        Get
            Return m_RiskCategory
        End Get
        Set(value As DbSet(Of RiskCategory))
            m_RiskCategory = value
        End Set
    End Property
    Private m_RiskCategory As DbSet(Of RiskCategory)

    Public Property ObservationStatus() As DbSet(Of ObservationStatus)
        Get
            Return m_ObservationStatus
        End Get
        Set(value As DbSet(Of ObservationStatus))
            m_ObservationStatus = value
        End Set
    End Property
    Private m_ObservationStatus As DbSet(Of ObservationStatus)

    Public Property BranchReviews() As DbSet(Of BranchReview)
        Get
            Return m_BranchReview
        End Get
        Set(value As DbSet(Of BranchReview))
            m_BranchReview = value
        End Set
    End Property
    Private m_BranchReview As DbSet(Of BranchReview)

    Public Property CorrectiveActions() As DbSet(Of CorrectiveAction)
        Get
            Return m_CorrectiveAction
        End Get
        Set(value As DbSet(Of CorrectiveAction))
            m_CorrectiveAction = value
        End Set
    End Property
    Private m_CorrectiveAction As DbSet(Of CorrectiveAction)


    Public Property ReviewFiles() As DbSet(Of ReviewFile)
        Get
            Return m_ReviewFile
        End Get
        Set(value As DbSet(Of ReviewFile))
            m_ReviewFile = value
        End Set
    End Property
    Private m_ReviewFile As DbSet(Of ReviewFile)

End Class
