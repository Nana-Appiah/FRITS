Imports System.Data.Entity.ModelConfiguration

Public Class ObservationDbConfig
    Inherits EntityTypeConfiguration(Of Observation)

    Sub New()
        Call ObservationDbConfig()
    End Sub
    Public Sub ObservationDbConfig()

        HasKey(Function(o) o.ObservationId)

        HasRequired(Function(o) o.Finding).WithMany(Function(o) o.Observations).HasForeignKey(Function(o) o.FindingId).WillCascadeOnDelete(False)
        HasRequired(Function(o) o.RiskLevel).WithMany(Function(o) o.Observations).HasForeignKey(Function(o) o.RiskLevelId).WillCascadeOnDelete(False)
        [Property](Function(o) o.ObservationDate).HasColumnType("date").IsRequired()
        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()
        [Property](Function(o) o.Implication).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()
        [Property](Function(o) o.RootCauseAnalysis).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()
        [Property](Function(o) o.MitigatingControl).HasColumnType("VARCHAR").HasMaxLength(1000)
        [Property](Function(o) o.Assumptions).HasColumnType("VARCHAR").HasMaxLength(1000)
        [Property](Function(o) o.ObservationNo).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired()
        [Property](Function(o) o.ResolutionTiming).HasColumnType("date")
        [Property](Function(o) o.ReferenceOfficerIds).HasColumnType("VARCHAR").HasMaxLength(255)
        [Property](Function(o) o.ManagementResponse).HasColumnType("VARCHAR")


        HasRequired(Function(o) o.ObservationStatus).WithMany(Function(o) o.Observations).HasForeignKey(Function(o) o.ObservationStatusId).WillCascadeOnDelete(False)

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
