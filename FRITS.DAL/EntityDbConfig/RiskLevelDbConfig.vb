Imports System.Data.Entity.ModelConfiguration

Public Class RiskLevelDbConfig
    Inherits EntityTypeConfiguration(Of RiskLevel)

    Sub New()
        Call RiskLevelDbConfig()
    End Sub
    Public Sub RiskLevelDbConfig()

        HasKey(Function(o) o.RiskLevelId)
        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired()

    End Sub
End Class
