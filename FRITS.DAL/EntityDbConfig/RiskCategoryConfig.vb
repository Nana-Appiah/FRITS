Imports System.Data.Entity.ModelConfiguration

Public Class RiskCategoryConfig
    Inherits EntityTypeConfiguration(Of RiskCategory)

    Sub New()
        Call RiskCategoryConfig()
    End Sub
    Public Sub RiskCategoryConfig()

        HasKey(Function(o) o.RiskCategoryId)
        [Property](Function(o) o.RiskCategoryDesc).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired()

    End Sub
End Class
