Imports System.Data.Entity.ModelConfiguration

Public Class FollowUpDetailDbConfig
    Inherits EntityTypeConfiguration(Of FollowUpDetail)

    Sub New()
        Call FollowUpDetailDbConfig()
    End Sub
    Public Sub FollowUpDetailDbConfig()

        HasKey(Function(o) o.FollowUpDetailId)

        HasRequired(Function(o) o.FollowUp).WithMany(Function(o) o.FollowUpDetails).HasForeignKey(Function(o) o.FollowUpId).WillCascadeOnDelete(False)
        HasRequired(Function(o) o.Observation).WithMany(Function(o) o.FollowUpDetails).HasForeignKey(Function(o) o.ObservationId).WillCascadeOnDelete(False)
        HasRequired(Function(o) o.ObservationStatus).WithMany(Function(o) o.FollowUpDetails).HasForeignKey(Function(o) o.ObservationStatusId).WillCascadeOnDelete(False)
        [Property](Function(o) o.Remarks).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()

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
