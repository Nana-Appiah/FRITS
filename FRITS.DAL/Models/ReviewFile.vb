Imports System.ComponentModel.DataAnnotations.Schema

Public Class ReviewFile
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property ReviewFileId As Guid
    Public Property ReviewFileType As Short
    Public Property ReferenceTypeId As Guid
    Public Property Description As String
    Public Property FileName As String
    Public Property FileExtension As String


End Class
