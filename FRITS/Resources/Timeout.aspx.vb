Partial Public Class Timeout
    Inherits PageBase

    '## Occurs before user signs out.
    Public Event BeforeSignOut()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Signout
        FormsAuthentication.SignOut()
        'Clear session
        Me.Session.Abandon()
        'Redirect to login page
        Me.Response.Redirect("../Login.aspx", True)
    End Sub

End Class