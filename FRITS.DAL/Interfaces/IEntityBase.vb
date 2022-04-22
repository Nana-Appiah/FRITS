Public Interface IEntityBase
    Property CreatedDate As DateTime
    Property CreatedByID As Integer
    Property LastUpdatedDate As DateTime?
    Property LastUpdatedByID As Integer?
    Property IsAuthorised As Boolean
    Property AuthorisedByID As Integer?
    Property AuthorisedDate As DateTime?
    Property DeletedDate As DateTime?
    Property DeletedByID As Integer?

    Property IsSelfAuthorise As Boolean

End Interface
