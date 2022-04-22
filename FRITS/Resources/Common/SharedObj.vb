Imports Microsoft.VisualBasic

Public Class SharedObj
    Dim branchname As String
    Public Property getname() As String
        Get
            Return branchname
        End Get
        Set(ByVal value As String)
            branchname = value
        End Set
    End Property
End Class
