Imports System
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity
Imports System.Linq

Partial Public Class BranchContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=BranchContext")
    End Sub

    Public Overridable Property Branches As DbSet(Of Branch)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        modelBuilder.Entity(Of Branch)() _
            .Property(Function(e) e.OldCode) _
            .IsUnicode(False)
    End Sub
End Class
