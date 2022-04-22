Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_management_response_to_observation
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.Observations", "ManagementResponse", Function(c) c.String(maxLength := 8000, unicode := false))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Observations", "ManagementResponse")
        End Sub
    End Class
End Namespace
