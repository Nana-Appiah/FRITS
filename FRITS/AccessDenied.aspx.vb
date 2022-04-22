Public Partial Class AccessDenied
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

#End Region

#Region " Events "

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.accessdenied"

            'Check authentication.
            If Not Me.CurrentUser Is Nothing Then

                If Not Page.IsPostBack Then

                    'Display Details
                    Me.DisplayPageHeaderDetails()

                    'Display Menu Items
                    Me.DisplayMenuItems()

                End If

            Else
                'Redirect to sign in page.
                Me.Response.Redirect(Me.Request.ApplicationPath & "/Login.aspx", True)
            End If
        Catch ex As Exception
            'Display message
            Me.ShowMessage(ex.Message)
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

End Class