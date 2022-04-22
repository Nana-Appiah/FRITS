Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class Initial_Database_Schema
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.ActionPlans",
                Function(c) New With
                    {
                        .ActionPlanId = c.Guid(nullable := False, identity := True),
                        .ObservationId = c.Guid(nullable := False),
                        .Description = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.ActionPlanId) _
                .ForeignKey("dbo.Observations", Function(t) t.ObservationId) _
                .Index(Function(t) t.ObservationId)
            
            CreateTable(
                "dbo.Observations",
                Function(c) New With
                    {
                        .ObservationId = c.Guid(nullable := False, identity := True),
                        .FindingId = c.Guid(nullable := False),
                        .ObservationDate = c.DateTime(nullable := False, storeType := "date"),
                        .ObservationNo = c.String(nullable := False, maxLength := 30, unicode := false),
                        .Description = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .Implication = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .RootCauseAnalysis = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .ResolutionTiming = c.DateTime(nullable := False, storeType := "date"),
                        .ObservationStatusId = c.Int(nullable := False),
                        .RiskLevelId = c.Int(nullable := False),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.ObservationId) _
                .ForeignKey("dbo.Findings", Function(t) t.FindingId) _
                .ForeignKey("dbo.ObservationStatus", Function(t) t.ObservationStatusId) _
                .ForeignKey("dbo.RiskLevels", Function(t) t.RiskLevelId) _
                .Index(Function(t) t.FindingId) _
                .Index(Function(t) t.ObservationStatusId) _
                .Index(Function(t) t.RiskLevelId)
            
            CreateTable(
                "dbo.Findings",
                Function(c) New With
                    {
                        .FindingId = c.Guid(nullable := False, identity := True),
                        .BranchReviewId = c.Guid(nullable := False),
                        .Description = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .FindingNo = c.String(nullable := False, maxLength := 10, unicode := false),
                        .RiskLevelId = c.Int(nullable := False),
                        .ReferenceOfficerIds = c.String(maxLength := 255, unicode := false),
                        .BranchCode = c.String(nullable := False, maxLength := 10, unicode := false),
                        .RiskCategoryId = c.Int(nullable := False),
                        .RiskSubCategory = c.String(nullable := False, maxLength := 100, unicode := false),
                        .ManagementAwareness = c.String(nullable := False, maxLength := 100, unicode := false),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.FindingId) _
                .ForeignKey("dbo.BranchReviews", Function(t) t.BranchReviewId) _
                .ForeignKey("dbo.RiskCategories", Function(t) t.RiskCategoryId) _
                .ForeignKey("dbo.RiskLevels", Function(t) t.RiskLevelId) _
                .Index(Function(t) t.BranchReviewId) _
                .Index(Function(t) t.RiskLevelId) _
                .Index(Function(t) t.RiskCategoryId)
            
            CreateTable(
                "dbo.BranchReviews",
                Function(c) New With
                    {
                        .BranchReviewId = c.Guid(nullable := False, identity := True),
                        .ReviewId = c.Guid(nullable := False),
                        .EmployeeId = c.Int(nullable := False),
                        .DateAssigned = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .BranchCode = c.String(nullable := False, maxLength := 10, unicode := false),
                        .ReviewInstruction = c.String(),
                        .StartDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .EndDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.BranchReviewId) _
                .ForeignKey("dbo.Reviews", Function(t) t.ReviewId) _
                .Index(Function(t) t.ReviewId)
            
            CreateTable(
                "dbo.Reviews",
                Function(c) New With
                    {
                        .ReviewId = c.Guid(nullable := False, identity := True),
                        .Description = c.String(nullable := False, maxLength := 500, unicode := false),
                        .ReviewCode = c.String(nullable := False, maxLength := 20, unicode := false),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.ReviewId)
            
            CreateTable(
                "dbo.RiskCategories",
                Function(c) New With
                    {
                        .RiskCategoryId = c.Int(nullable := False, identity := True),
                        .RiskCategoryDesc = c.String(nullable := False, maxLength := 255, unicode := false)
                    }) _
                .PrimaryKey(Function(t) t.RiskCategoryId)
            
            CreateTable(
                "dbo.RiskLevels",
                Function(c) New With
                    {
                        .RiskLevelId = c.Int(nullable := False, identity := True),
                        .Description = c.String(nullable := False, maxLength := 255, unicode := false),
                        .RiskScore = c.Decimal(nullable := False, precision := 18, scale := 2)
                    }) _
                .PrimaryKey(Function(t) t.RiskLevelId)
            
            CreateTable(
                "dbo.FollowUpDetails",
                Function(c) New With
                    {
                        .FollowUpDetailId = c.Guid(nullable := False, identity := True),
                        .FollowUpId = c.Guid(nullable := False),
                        .ObservationId = c.Guid(nullable := False),
                        .Remarks = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.FollowUpDetailId) _
                .ForeignKey("dbo.FollowUps", Function(t) t.FollowUpId) _
                .ForeignKey("dbo.Observations", Function(t) t.ObservationId) _
                .Index(Function(t) t.FollowUpId) _
                .Index(Function(t) t.ObservationId)
            
            CreateTable(
                "dbo.FollowUps",
                Function(c) New With
                    {
                        .FollowUpId = c.Guid(nullable := False, identity := True),
                        .FollowUpDate = c.DateTime(nullable := False, storeType := "date"),
                        .Description = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.FollowUpId)
            
            CreateTable(
                "dbo.ObservationStatus",
                Function(c) New With
                    {
                        .ObservationStatusId = c.Int(nullable := False, identity := True),
                        .Description = c.String(nullable := False, maxLength := 255, unicode := false),
                        .StatusCode = c.String(nullable := False, maxLength := 15, unicode := false),
                        .Narration = c.String(maxLength := 1000, unicode := false),
                        .IsEnabled = c.Boolean(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ObservationStatusId)
            
            CreateTable(
                "dbo.Recommendations",
                Function(c) New With
                    {
                        .RecommendationId = c.Guid(nullable := False, identity := True),
                        .RecommendationType = c.Short(nullable := False),
                        .TypeReferenceId = c.Guid(nullable := False),
                        .Description = c.String(nullable := False, maxLength := 5000, unicode := false),
                        .CreatedDate = c.DateTime(nullable := False, precision := 7, storeType := "datetime2"),
                        .CreatedByID = c.Int(nullable := False),
                        .LastUpdatedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .LastUpdatedByID = c.Int(),
                        .IsAuthorised = c.Boolean(nullable := False),
                        .AuthorisedByID = c.Int(),
                        .AuthorisedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedDate = c.DateTime(precision := 7, storeType := "datetime2"),
                        .DeletedByID = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.RecommendationId)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.ActionPlans", "ObservationId", "dbo.Observations")
            DropForeignKey("dbo.Observations", "RiskLevelId", "dbo.RiskLevels")
            DropForeignKey("dbo.Observations", "ObservationStatusId", "dbo.ObservationStatus")
            DropForeignKey("dbo.FollowUpDetails", "ObservationId", "dbo.Observations")
            DropForeignKey("dbo.FollowUpDetails", "FollowUpId", "dbo.FollowUps")
            DropForeignKey("dbo.Observations", "FindingId", "dbo.Findings")
            DropForeignKey("dbo.Findings", "RiskLevelId", "dbo.RiskLevels")
            DropForeignKey("dbo.Findings", "RiskCategoryId", "dbo.RiskCategories")
            DropForeignKey("dbo.Findings", "BranchReviewId", "dbo.BranchReviews")
            DropForeignKey("dbo.BranchReviews", "ReviewId", "dbo.Reviews")
            DropIndex("dbo.FollowUpDetails", New String() { "ObservationId" })
            DropIndex("dbo.FollowUpDetails", New String() { "FollowUpId" })
            DropIndex("dbo.BranchReviews", New String() { "ReviewId" })
            DropIndex("dbo.Findings", New String() { "RiskCategoryId" })
            DropIndex("dbo.Findings", New String() { "RiskLevelId" })
            DropIndex("dbo.Findings", New String() { "BranchReviewId" })
            DropIndex("dbo.Observations", New String() { "RiskLevelId" })
            DropIndex("dbo.Observations", New String() { "ObservationStatusId" })
            DropIndex("dbo.Observations", New String() { "FindingId" })
            DropIndex("dbo.ActionPlans", New String() { "ObservationId" })
            DropTable("dbo.Recommendations")
            DropTable("dbo.ObservationStatus")
            DropTable("dbo.FollowUps")
            DropTable("dbo.FollowUpDetails")
            DropTable("dbo.RiskLevels")
            DropTable("dbo.RiskCategories")
            DropTable("dbo.Reviews")
            DropTable("dbo.BranchReviews")
            DropTable("dbo.Findings")
            DropTable("dbo.Observations")
            DropTable("dbo.ActionPlans")
        End Sub
    End Class
End Namespace
