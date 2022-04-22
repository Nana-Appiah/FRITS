Imports System.Data.Entity.ModelConfiguration

Public Class ObservationStatusDbConfig
    Inherits EntityTypeConfiguration(Of ObservationStatus)

    Sub New()
        Call ObservationStatusDbConfig()
    End Sub
    Public Sub ObservationStatusDbConfig()

        HasKey(Function(o) o.ObservationStatusId)

        [Property](Function(o) o.Description).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired()
        [Property](Function(o) o.Narration).HasColumnType("VARCHAR").HasMaxLength(1000)
        [Property](Function(o) o.StatusCode).HasColumnType("VARCHAR").HasMaxLength(15).IsRequired()




    End Sub
End Class
