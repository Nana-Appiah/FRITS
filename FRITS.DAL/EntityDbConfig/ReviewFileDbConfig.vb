Imports System.Data.Entity.ModelConfiguration

Public Class ReviewFileDbConfig
    Inherits EntityTypeConfiguration(Of ReviewFile)

    Sub New()
        Call ReviewFileDbConfig()
    End Sub
    Public Sub ReviewFileDbConfig()

        HasKey(Function(o) o.ReviewFileId)
        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(5000).IsRequired()
        [Property](Function(o) o.FileName).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired()
        [Property](Function(o) o.FileExtension).HasColumnType("VARCHAR").HasMaxLength(10).IsRequired()
        [Property](Function(o) o.ReferenceTypeId).IsRequired()

        [Property](Function(o) o.CreatedDate).HasColumnType("datetime2").IsRequired()
        [Property](Function(o) o.CreatedByID).IsRequired()

        [Property](Function(o) o.LastUpdatedDate).HasColumnType("datetime2").IsOptional()
        [Property](Function(o) o.LastUpdatedByID).IsOptional()
        [Property](Function(o) o.DeletedDate).HasColumnType("datetime2").IsOptional()
        [Property](Function(o) o.DeletedByID).IsOptional()
        [Property](Function(o) o.AuthorisedDate).HasColumnType("datetime2").IsOptional()
        [Property](Function(o) o.AuthorisedByID).IsOptional()

    End Sub
End Class
