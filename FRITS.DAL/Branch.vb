Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

Partial Public Class Branch
    Public Property BranchId As Integer

    <StringLength(50)>
    Public Property Name As String

    <StringLength(5)>
    Public Property Code As String

    <StringLength(5)>
    Public Property Mnemonic As String

    Public Property ManagerId As Integer?

    <StringLength(10)>
    Public Property OldCode As String
End Class
