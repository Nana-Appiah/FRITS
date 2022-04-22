Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_schedule_officer_response_and_assumptions_mitigating_control_assumptions
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.ActionPlans", "ScheduleOfficerResponse", Function(c) c.String(nullable := False, maxLength := 1000, unicode := false))
            AddColumn("dbo.Observations", "Assumptions", Function(c) c.String(maxLength := 1000, unicode := false))
            AddColumn("dbo.Observations", "MitigatingControl", Function(c) c.String(maxLength := 1000, unicode := false))
        End Sub
        
        Public Overrides Sub Down()
            DropColumn("dbo.Observations", "MitigatingControl")
            DropColumn("dbo.Observations", "Assumptions")
            DropColumn("dbo.ActionPlans", "ScheduleOfficerResponse")
        End Sub
    End Class
End Namespace
