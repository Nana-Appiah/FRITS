Partial Public Class _Dialog
    Inherits System.Web.UI.Page

    '## default content Page
    Public ReadOnly Property DefaultPage() As String
        Get
            Dim url As String = Request("url")
            Return Functions.Decode64(url)
        End Get
    End Property

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If CDbl(Me.Request.Browser.Version) > 7.0 Then
        End If
        Me.Server.Transfer("DialogEX.aspx?url=" & Me.Request("url"), True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class