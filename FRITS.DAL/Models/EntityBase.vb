Imports System.ComponentModel.DataAnnotations.Schema

Public Class EntityBase
    Implements IEntityBase
    Public Property CreatedDate As DateTime Implements IEntityBase.CreatedDate
    Public Property CreatedByID As Integer Implements IEntityBase.CreatedByID
    Public Property LastUpdatedDate As DateTime? Implements IEntityBase.LastUpdatedDate
    Public Property LastUpdatedByID As Integer? Implements IEntityBase.LastUpdatedByID
    Public Property IsAuthorised As Boolean Implements IEntityBase.IsAuthorised
    Public Property AuthorisedByID As Integer? Implements IEntityBase.AuthorisedByID
    Public Property AuthorisedDate As DateTime? Implements IEntityBase.AuthorisedDate
    Public Property DeletedDate As DateTime? Implements IEntityBase.DeletedDate
    Public Property DeletedByID As Integer? Implements IEntityBase.DeletedByID

    <NotMapped>
    Public Property IsSelfAuthorise As Boolean Implements IEntityBase.IsSelfAuthorise

End Class
