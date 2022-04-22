Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class removed_fields_from_findings_and_added_it_to_observation
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Observations", "ReferenceOfficerIds", Function(c) c.String(maxLength := 255, unicode := false))
            DropColumn("dbo.Findings", "ReferenceOfficerIds")
        End Sub
        
        Public Overrides Sub Down()
            AddColumn("dbo.Findings", "ReferenceOfficerIds", Function(c) c.String(maxLength := 255, unicode := false))
            DropColumn("dbo.Observations", "ReferenceOfficerIds")
        End Sub
    End Class
End Namespace
