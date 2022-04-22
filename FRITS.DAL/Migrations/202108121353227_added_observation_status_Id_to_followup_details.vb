Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_observation_status_Id_to_followup_details
        Inherits DbMigration
    
        Public Overrides Sub Up()
            AddColumn("dbo.FollowUpDetails", "ObservationStatusId", Function(c) c.Int(nullable := False))
            CreateIndex("dbo.FollowUpDetails", "ObservationStatusId")
            AddForeignKey("dbo.FollowUpDetails", "ObservationStatusId", "dbo.ObservationStatus", "ObservationStatusId")
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.FollowUpDetails", "ObservationStatusId", "dbo.ObservationStatus")
            DropIndex("dbo.FollowUpDetails", New String() { "ObservationStatusId" })
            DropColumn("dbo.FollowUpDetails", "ObservationStatusId")
        End Sub
    End Class
End Namespace
