Imports System.Data.Entity.ModelConfiguration

Public Class CorrectiveActionDbConfig
    Inherits EntityTypeConfiguration(Of CorrectiveAction)

    Sub New()
        Call CorrectiveActionDbConfig()
    End Sub
    Public Sub CorrectiveActionDbConfig()

        HasKey(Function(o) o.CorrectiveActionId)
        HasRequired(Function(o) o.Observation).WithMany(Function(o) o.CorrectiveActions).HasForeignKey(Function(o) o.ObservationId).WillCascadeOnDelete(False)
        HasRequired(Function(o) o.ObservationStatus).WithMany(Function(o) o.CorrectiveActions).HasForeignKey(Function(o) o.ObservationStatusId).WillCascadeOnDelete(False)
        [Property](Function(o) o.Remarks).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()
        [Property](Function(o) o.CorrectionDate).HasColumnType("date").IsRequired()

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
