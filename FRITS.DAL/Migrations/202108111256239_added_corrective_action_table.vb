Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_corrective_action_table
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.CorrectiveActions",
                Function(c) New With
                    {
                        .CorrectiveActionId = c.Guid(nullable := False, identity := True),
                        .ObservationId = c.Guid(nullable := False),
                        .CorrectionDate = c.DateTime(nullable := False, storeType := "date"),
                        .Remarks = c.String(nullable := False, maxLength := 1000, unicode := false),
                        .ObservationStatusId = c.Int(nullable := False),
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
                .PrimaryKey(Function(t) t.CorrectiveActionId) _
                .ForeignKey("dbo.Observations", Function(t) t.ObservationId) _
                .ForeignKey("dbo.ObservationStatus", Function(t) t.ObservationStatusId) _
                .Index(Function(t) t.ObservationId) _
                .Index(Function(t) t.ObservationStatusId)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.CorrectiveActions", "ObservationStatusId", "dbo.ObservationStatus")
            DropForeignKey("dbo.CorrectiveActions", "ObservationId", "dbo.Observations")
            DropIndex("dbo.CorrectiveActions", New String() { "ObservationStatusId" })
            DropIndex("dbo.CorrectiveActions", New String() { "ObservationId" })
            DropTable("dbo.CorrectiveActions")
        End Sub
    End Class
End Namespace
