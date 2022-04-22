Imports System.Data.Entity.ModelConfiguration

Public Class FindingDbConfig
    Inherits EntityTypeConfiguration(Of Finding)

    Sub New()
        Call FindingDbConfig()
    End Sub
    Public Sub FindingDbConfig()

        HasKey(Function(o) o.FindingId)

        HasRequired(Function(o) o.BranchReview).WithMany(Function(o) o.Findings).HasForeignKey(Function(o) o.BranchReviewId).WillCascadeOnDelete(False)
        HasRequired(Function(o) o.RiskLevel).WithMany(Function(o) o.Findings).HasForeignKey(Function(o) o.RiskLevelId).WillCascadeOnDelete(False)
        HasRequired(Function(o) o.RiskCategory).WithMany(Function(o) o.Findings).HasForeignKey(Function(o) o.RiskCategoryId).WillCascadeOnDelete(False)
        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(1000).IsRequired()
        [Property](Function(o) o.BranchCode).HasColumnType("VARCHAR").HasMaxLength(10).IsRequired()
        [Property](Function(o) o.FindingNo).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired()
        [Property](Function(o) o.RiskSubCategory).HasColumnType("VARCHAR").HasMaxLength(100).IsRequired()
        [Property](Function(o) o.ManagementAwareness).HasColumnType("VARCHAR").HasMaxLength(100).IsRequired()


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
