Imports System.Data.Entity.ModelConfiguration

Public Class ReviewDbConfig
    Inherits EntityTypeConfiguration(Of Review)

    Sub New()
        Call ReviewDbConfig()
    End Sub
    Public Sub ReviewDbConfig()

        HasKey(Function(o) o.ReviewId)

        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(500).IsRequired()
        [Property](Function(o) o.ReviewCode).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired()
        [Property](Function(o) o.ClosedDate).HasColumnType("date")

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
