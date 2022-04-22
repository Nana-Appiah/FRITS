Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class modified_findingno_reviewNo_observationNo
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AlterColumn("dbo.Observations", "ObservationNo", Function(c) c.String(nullable := False, maxLength := 50, unicode := false))
            AlterColumn("dbo.Findings", "FindingNo", Function(c) c.String(nullable := False, maxLength := 50, unicode := false))
            AlterColumn("dbo.Reviews", "ReviewCode", Function(c) c.String(nullable := False, maxLength := 50, unicode := false))
        End Sub
        
        Public Overrides Sub Down()
            AlterColumn("dbo.Reviews", "ReviewCode", Function(c) c.String(nullable := False, maxLength := 20, unicode := false))
            AlterColumn("dbo.Findings", "FindingNo", Function(c) c.String(nullable := False, maxLength := 10, unicode := false))
            AlterColumn("dbo.Observations", "ObservationNo", Function(c) c.String(nullable := False, maxLength := 30, unicode := false))
        End Sub
    End Class
End Namespace
