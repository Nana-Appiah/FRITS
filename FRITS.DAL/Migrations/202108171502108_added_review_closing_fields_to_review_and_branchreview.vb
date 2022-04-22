Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_review_closing_fields_to_review_and_branchreview
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.BranchReviews", "IsClosed", Function(c) c.Boolean(nullable := False))
            AddColumn("dbo.Reviews", "IsClosed", Function(c) c.Boolean(nullable := False))
            AddColumn("dbo.Reviews", "ClosedById", Function(c) c.Int(nullable := False))
            AddColumn("dbo.Reviews", "ClosedDate", Function(c) c.DateTime(nullable := False, storeType := "date"))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Reviews", "ClosedDate")
            DropColumn("dbo.Reviews", "ClosedById")
            DropColumn("dbo.Reviews", "IsClosed")
            DropColumn("dbo.BranchReviews", "IsClosed")
        End Sub
    End Class
End Namespace
