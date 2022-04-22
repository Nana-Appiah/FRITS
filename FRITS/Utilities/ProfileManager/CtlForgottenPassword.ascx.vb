Partial Public Class CtlForgottenPassword
    Inherits UserControlBase

#Region " Properties"

#End Region

#Region " Methods"

    Private Sub GetData()

    End Sub

    Private Sub SaveData()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Dim user As ARUser = Me.ARConn.GetUserByEmail(Me.txtEmail.Text)

            If Me.txtEmail.Text = "" Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Please enter email address.');", True)
                Exit Sub
            ElseIf System.Text.RegularExpressions.Regex.IsMatch(Me.txtEmail.Text, "\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") = False Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Please enter a valid email address.');", True)
                Exit Sub
            End If

            If Not user Is Nothing Then

                Dim newPassword As String = System.Guid.NewGuid.ToString("D").Substring(0, 8)

                ' Assign values
                user.PasswordEncryptionType = AREncryptionTypesEnum.MD5
                user.Password = newPassword
                user.PasswordExpires = Now.Date.AddDays(1)
                user.SetPropertyValue("PasswordReset", "Yes")

                user.Update()

                'Write into auditing log
                Me.Log(user.ObjectID, Me.GetResourceID(Me.ObjectAlias), "User password reset.", "FRITS")

                'Clear Controls
                Me.ClearControls()

                'Send email to next Registration user
                Try
                    Dim email_message As String = ""

                    email_message &= "Dear User,"
                    email_message &= "<br><br>"
                    email_message &= "Your user name and password are:"
                    email_message &= "<br><br><b>User name:</b>" & " " & user.Login
                    email_message &= "<br><b>Password:</b>" & " " & user.Password
                    email_message &= "<br><br><a href='" & Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "'>" & Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "</a>"
                    email_message &= "<br><br><b>Note:</b> Password expires in 1 day. Kindly reset your password after logging on."

                    Me.SendMail(user.EmailAddress, "User Account Information", email_message, , , , True)

                    'Display message and close window
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("SendPassword.lblPasswordSent.PasswordSentSuccessfully", Me.CurrentCultureInfo) & "');window.location.href='Login.aspx';", True)

                Catch ex As Exception
                    If ex.Message = "Could not access 'CDO.Message' object." Then
                        'Write into auditing log
                        Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "FRITS")
                        'Display message
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Unable to send notification email. Please configure SMTP settings.');", True)
                    Else
                        'Display message
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & ex.Message & "');", True)
                    End If
                End Try

            Else
                'Display message.
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("SendPassword.lblPasswordSent.errorEmailNotFound", Me.CurrentCultureInfo) & "');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & ex.Message & "');", True)
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

    Private Sub ClearControls()
        Me.txtEmail.Text = ""
    End Sub

#End Region

#Region " Events "

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Me.btnSubmit.Enabled = False
        Me.SaveData()
        Me.btnSubmit.Enabled = True
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager.forgottenpassword"

            If Not Me.Page.IsPostBack Then

            End If

        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

End Class