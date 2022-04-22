Partial Public Class Layout2
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Timer.Enabled = False
    End Sub

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles ScriptManager.AsyncPostBackError
        Me.ScriptManager.AsyncPostBackErrorMessage = "Error on page." 'e.Exception.Message
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer.Tick
        Me.Response.Redirect("Login.aspx?fn=logout", True)
    End Sub

End Class