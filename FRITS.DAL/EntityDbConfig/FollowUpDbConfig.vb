Imports System.Data.Entity.ModelConfiguration
Public Class FollowUpDbConfig
    Inherits EntityTypeConfiguration(Of FollowUp)

    Sub New()
        Call FollowUpDbConfig()
    End Sub
    Public Sub FollowUpDbConfig()

        HasKey(Function(o) o.FollowUpId)

        [Property](Function(o) o.FollowUpDate).HasColumnType("date").IsRequired()
        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()


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
