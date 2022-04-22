Imports System.Data.Entity.ModelConfiguration

Public Class BranchReviewDbConfig

    Inherits EntityTypeConfiguration(Of BranchReview)

    Sub New()
        Call BranchReviewDbConfig()
    End Sub
    Public Sub BranchReviewDbConfig()

        HasKey(Function(o) o.BranchReviewId)

        HasRequired(Function(o) o.Review).WithMany(Function(o) o.BranchReviews).HasForeignKey(Function(o) o.ReviewId).WillCascadeOnDelete(False)
        [Property](Function(o) o.DateAssigned).HasColumnType("datetime2").IsRequired()
        [Property](Function(o) o.BranchCode).HasColumnType("VARCHAR").HasMaxLength(10).IsRequired()
        [Property](Function(o) o.StartDate).HasColumnType("datetime2").IsRequired()
        [Property](Function(o) o.EndDate).HasColumnType("datetime2").IsRequired()


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
