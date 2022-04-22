Public Class ReviewFileVm
    Public Property ReviewFileId As Guid
    Public Property ReviewFileType As Short
    Public Property ReferenceTypeId As Guid
    Public Property Description As String
    Public Property FileName As String
    Public Property FileExtension As String


    Public ReadOnly Property GetFileName() As String
        Get
            Return $"{FileName}.{FileExtension}"
        End Get
    End Property

    Public ReadOnly Property GetSimpleFileName() As String
        Get
            Return $"{Description}.{FileExtension}"
        End Get
    End Property

End Class
