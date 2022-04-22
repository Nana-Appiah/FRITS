Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class added_review_file_table
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.ReviewFiles",
                Function(c) New With
                    {
                        .ReviewFileId = c.Guid(nullable := False, identity := True),
                        .ReviewFileType = c.Short(nullable := False),
                        .ReferenceTypeId = c.Guid(nullable := False),
                        .Description = c.String(nullable := False, maxLength := 5000, unicode := false),
                        .FileName = c.String(nullable := False, maxLength := 255, unicode := false),
                        .FileExtension = c.String(nullable := False, maxLength := 10, unicode := false),
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
                .PrimaryKey(Function(t) t.ReviewFileId)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.ReviewFiles")
        End Sub
    End Class
End Namespace
