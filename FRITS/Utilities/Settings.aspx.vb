Public Partial Class Settings
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    Private Sub GetData()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Me.txtHTTPServer.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("HTTPServer")
            Me.txtSMTPServer.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("SMTPServer")
            Me.chkAllowNotification.Checked = IIf(Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("AllowNotification") = "Yes", True, False)
            Me.txtNotificationEmail.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("NotificationEmail")
            Me.txtNotificationEmailAlias.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("NotificationEmailAlias")
            Me.txtOrderFolder.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("OrderFolder")
            Me.txtBatchNo.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("BatchNo")
            Me.txtBatchPrefix.Text = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("BatchPrefix")

        Catch ex As Exception
            'Display message
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

    Private Sub SaveData()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("HTTPServer", Me.txtHTTPServer.Text)
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("SMTPServer", Me.txtSMTPServer.Text)
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("AllowNotification", IIf(Me.chkAllowNotification.Checked, "Yes", "No"))
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("NotificationEmail", Me.txtNotificationEmail.Text)
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("NotificationEmailAlias", Me.txtNotificationEmailAlias.Text)
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("OrderFolder", Me.txtOrderFolder.Text)
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("BatchNo", Me.txtBatchNo.Text)
            Me.ARConn.GetApplicationByAlias("fams").SetPropertyValue("BatchPrefix", Me.txtBatchPrefix.Text)

            'Write into auditing log
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Changes saved successfully.", "fams")

            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Changes saved successfully.');", True)

        Catch ex As Exception
            'Display message
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

#End Region

#Region " Events "

    Private Sub tbrMain_ItemCommand(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.ToolBarItemEventArgs) Handles tbrMain.ItemCommand
        If e.Item.Value = "Save" Then
            'Check permission to save.
            If Not ARHelper.IsAuthorized(Me.CurrentUser.Login, Me.ObjectAlias, "change") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You do not have the permission to change');", True)
                Exit Sub
            Else
                Me.SaveData()
            End If
        End If
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Me.SaveData()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Me.Response.Redirect("Default.aspx", True)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.settings"

            'Check authentication.
            If Not Me.CurrentUser Is Nothing Then

                'Check permission to open page.
                If Me.CurrentUser.IsMemberAll("Administrators") OrElse Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "read") Then

                    If Not Page.IsPostBack Then

                        'Display Details
                        Me.DisplayPageHeaderDetails()

                        'Display Menu Items
                        Me.DisplayMenuItems()

                        Me.GetData()

                    End If

                Else
                    'Redirect to access denied page.
                    Me.Response.Redirect(Me.Request.ApplicationPath & "/AccessDenied.aspx", True)
                End If
            Else
                'Redirect to sign in page.
                Me.Response.Redirect(Me.Request.ApplicationPath & "/Login.aspx", True)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

End Class