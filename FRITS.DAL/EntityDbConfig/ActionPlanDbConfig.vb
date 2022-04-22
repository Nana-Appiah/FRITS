Imports System.Data.Entity.ModelConfiguration

Public Class ActionPlanDbConfig
    Inherits EntityTypeConfiguration(Of ActionPlan)

    Sub New()
        Call ActionPlanDbConfig()
    End Sub
    Public Sub ActionPlanDbConfig()

        HasKey(Function(o) o.ActionPlanId)
        HasRequired(Function(o) o.Observation).WithMany(Function(o) o.ActionPlans).HasForeignKey(Function(o) o.ObservationId).WillCascadeOnDelete(False)

        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()
        [Property](Function(o) o.ScheduleOfficerResponse).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()

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
