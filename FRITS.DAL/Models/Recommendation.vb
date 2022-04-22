Imports System.ComponentModel.DataAnnotations.Schema

Public Class Recommendation
    Inherits EntityBase
    Implements IEntityBase

    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property RecommendationId As Guid
    Public Property RecommendationType As Short
    Public Property TypeReferenceId As Guid
    Public Property Description As String
End Class
