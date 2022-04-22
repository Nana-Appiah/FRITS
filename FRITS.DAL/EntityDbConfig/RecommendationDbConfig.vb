Imports System.Data.Entity.ModelConfiguration

Public Class RecommendationDbConfig
    Inherits EntityTypeConfiguration(Of Recommendation)

    Sub New()
        Call RecommendationDbConfig()
    End Sub
    Public Sub RecommendationDbConfig()

        HasKey(Function(o) o.RecommendationId)
        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(5000).IsRequired()

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
