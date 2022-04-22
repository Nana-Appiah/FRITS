Partial Public Class ChangePassword
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    Private Sub GetData()

    End Sub

    Private Sub SaveData()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Dim user As ARUser = Me.ARConn.GetUserByLogin(Me.CurrentUser.Login)

            If Not user.Password = Me.txtOldPassword.Text Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Invalid old password provided.');", True)
                Exit Sub
            End If

            If Me.txtNewPassword.Text = "" Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You must enter a password for this user.');", True)
                Exit Sub
            ElseIf Not Me.txtNewPassword.Text = Me.txtConfirmPassword.Text Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Password confirmation failed. Please make sure passwords match.');", True)
                Exit Sub
            End If

            ' Assign values
            user.PasswordEncryptionType = AREncryptionTypesEnum.MD5
            user.Password = Me.txtNewPassword.Text
            user.PasswordExpires = Now.Date.AddDays(45)

            user.Update()

            'Send email notification 
            If Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("AllowEmailNotification") Then

                'Send email to next Registration user
                Try
                    Dim email_message As String = ""

                    email_message &= "Dear User,"
                    email_message &= "<br><br>"
                    email_message &= "Your Account Information"
                    email_message &= "<br><br>Your password has been changed. Your user name and new password are:"
                    email_message &= "<br><br><b>User name:</b>" & " " & user.Login
                    email_message &= "<br><b>Password:</b>" & " " & user.Password
                    email_message &= "<br><br>Please log on "
                    email_message &= "<br><br><a href='" & Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "'>" & Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "</a>"

                    Me.SendMail(user.EmailAddress, "User Account Information", email_message, , , , True)

                Catch ex As Exception
                    If ex.Message = "Could not access 'CDO.Message' object." Then
                        'Write into auditing log
                        Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "FRITS")
                        'Display message
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Unable to send notification email. Please configure SMTP settings.');", True)
                        Exit Sub
                    Else
                        'Display message
                        Throw ex
                        Exit Sub
                    End If
                End Try

            End If

            'Clear Contrfams
            Me.ClearContrfams()

            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Password changed successfully.');", True)

            'Close page
            Me.ClosePage("reload")

        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

    Private Sub ClearContrfams()

    End Sub

#End Region

#Region " Events "

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles ScriptManager.AsyncPostBackError
        Me.ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Me.SaveData()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager.changepassword"

            If Not Me.Page.IsPostBack Then

            End If

        Catch ex As Exception
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

End Class