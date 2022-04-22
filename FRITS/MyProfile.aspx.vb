Public Partial Class MyProfile
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    Private Sub GetData()

        'Open secure access connection
        Me.ARConn.ConnectToCatalog()

        Dim user As ARUser
        user = Me.ARConn.GetUserByID(Me.CurrentUser.ObjectID)

        Me.lblUserId.Text = user.Login
        Me.lblFullName.Text = user.ObjectName
        Me.txtEmailAddress.Text = user.EmailAddress

        For Each member As String In Me.CurrentUser.MembershipObjectAliases.Split(";")
            If Not member = "" Then
                If member.Contains("secureaccess.secureaccess_") = False OrElse member.Contains("system_all_users") = False Then
                    If Not Me.ARConn.GetGroupByAlias(member) Is Nothing Then
                        Me.ltlMembership.Text += Me.ARConn.GetGroupByAlias(member).ObjectName & "; "
                    End If
                End If
            End If
        Next

        Me.ltlMembership.Text = Me.ltlMembership.Text.Remove(Me.ltlMembership.Text.LastIndexOf(";"), 1)

        'Close secure access connection
        Me.ARConn.Close()

    End Sub

    Private Sub SaveData()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Dim user As ARUser
            user = Me.ARConn.GetUserByID(Me.CurrentUser.ObjectID)

            user.EmailAddress = Me.txtEmailAddress.Text

            If Me.chkChangePassword.Checked = True Then

                If Me.txtNewPassword.Text = "" Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You must enter a password for this user.');", True)
                    Exit Sub
                ElseIf Not Me.txtNewPassword.Text = Me.txtConfirmPassword.Text Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Password confirmation failed.lease make sure passwords match.');", True)
                    Exit Sub
                End If

                'If da.GetSettings("AllowComplexPassword") Then
                '    If Not Regex.IsMatch(Me.txtNewPassword.Text, da.GetSettings("ComplexPasswordRegex")) Then
                '        'Display message
                '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & da.GetSettings("ComplexPasswordRegexErrorMsg") & "');", True)
                '        Exit Sub
                '    End If
                'Else
                '    If Not Regex.IsMatch(Me.txtNewPassword.Text, da.GetSettings("RelaxedPasswordRegex")) Then
                '        'Display message
                '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & da.GetSettings("RelaxedPasswordRegexErrorMsg") & "');", True)
                '        Exit Sub
                '    End If
                'End If

                'Set encryption type
                user.PasswordEncryptionType = AREncryptionTypesEnum.MD5
                user.PasswordExpires = Now.Date.AddDays(45)

                'Set new password
                user.Password = Me.txtNewPassword.Text

                'Reset first time login
                user.SetPropertyValue("FirstTimeLogOn", "No")
                user.SetPropertyValue("PasswordReset", "No")

            End If

            user.Update()

            'Write into auditing log
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Changes saved successfully.", "FRITS")

            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Changes saved successfully.');window.location.href = 'Default.aspx';", True)

        Catch ex As Exception
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
            Me.SaveData()
        End If
    End Sub

    Private Sub chkChangePassword_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkChangePassword.CheckedChanged

        Me.txtNewPassword.Enabled = Me.chkChangePassword.Checked
        Me.txtConfirmPassword.Enabled = Me.chkChangePassword.Checked

        If Me.chkChangePassword.Checked Then
            Me.txtNewPassword.Focus()
        End If

    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSubmit.Click
        Me.SaveData()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancel.Click
        Me.Response.Redirect("Default.aspx", True)
    End Sub

#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Me.CheckLicense()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.myprofile"

            'Check authentication.
            If Not Me.CurrentUser Is Nothing Then

                'Display Details
                Me.DisplayPageHeaderDetails()

                'Display Menu Items
                Me.DisplayMenuItems()

                If Not Page.IsPostBack Then

                    'Get data to bind form
                    Me.GetData()

                End If

                If Me.CurrentUser.GetUserObject.GetPropertyValue("FirstTimeLogOn") = "Yes" Then

                    Me.ltlMessage.Text = "<p><b>The system recognizes you as a first time user. You are adviced to change password.</b><p /><br />"
                    Me.chkChangePassword.Checked = True
                    Me.chkChangePassword_CheckedChanged(Me, New EventArgs)
                    Me.txtNewPassword.Focus()

                ElseIf Me.CurrentUser.GetUserObject.GetPropertyValue("PasswordReset") = "Yes" Then

                    Me.ltlMessage.Text = "<p><b>The system recognizes your password has been reset. Kindly change your password to continue.</b><p /><br />"
                    Me.chkChangePassword.Checked = True
                    Me.chkChangePassword_CheckedChanged(Me, New EventArgs)
                    Me.txtNewPassword.Focus()

                End If

            Else
                'Redirect to access denied page.
                Me.Response.Redirect(Me.Request.ApplicationPath & "/AccessDenied.aspx", True)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

End Class