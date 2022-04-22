Partial Public Class DialogEX
    Inherits System.Web.UI.Page

    '## default content Page
    Public ReadOnly Property DefaultPage() As String
        Get
            Dim url As String = Request("url")
            Return Functions.Decode64(url)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class