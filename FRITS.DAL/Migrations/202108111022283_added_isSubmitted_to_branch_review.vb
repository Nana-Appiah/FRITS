Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_isSubmitted_to_branch_review
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.BranchReviews", "IsSubmitted", Function(c) c.Boolean(nullable := False))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.BranchReviews", "IsSubmitted")
        End Sub
    End Class
End Namespace
