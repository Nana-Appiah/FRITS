Partial Public Class CtlLogin
    Inherits UserControlBase

#Region " Properties"

#End Region

#Region " Methods"

    '## Create user ticket after successful authentication.
    Public Sub CreateTicket()

        'Open secure access connection
        Me.ARConn.ConnectToCatalog()

        'Dim lic As New LicenseManager

        Dim user As ARUser = Nothing
        user = Me.ARConn.GetUserByLogin(Me.txtLoginID.Text.ToString.Trim)

        user.SetPropertyValue("LastTimeLogOn", IIf(user.GetPropertyValue("CurrentTimeLogOn") = "", Now.ToString, user.GetPropertyValue("CurrentTimeLogOn")))
        user.SetPropertyValue("CurrentTimeLogOn", Now.ToString)
        user.SetPropertyValue("FailedLoginCount", "0")

        'create user ticket
        Me.Session("ARUserTicket") = New ARUserTicket(user)

        'write into auditing log
        Me.Log(user.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Log.LogonSuccessful", Me.CurrentCultureInfo), "FRITS")

        Me.ARConn.Close()

        'lic.LastUse()

    End Sub

    '## Redirects user after successful authentication.
    Public Sub Redirect()

        ' redirection to original page
        Dim strRedirect As String = Request("ReturnUrl")

        Dim strRedirectTo As String = Trim(ConfigurationManager.AppSettings("SecureAccessLogonFormRedirectsTo"))

        If (Not strRedirectTo Is Nothing) AndAlso (strRedirectTo <> "") Then
            FormsAuthentication.SetAuthCookie(txtLoginID.Text, False)
            strRedirectTo = strRedirectTo & "?ReturnUrl=" & strRedirect
            Response.Redirect(strRedirectTo, True)
        ElseIf (Not strRedirect Is Nothing) AndAlso (strRedirect <> "") Then
            FormsAuthentication.SetAuthCookie(txtLoginID.Text, False)
            Response.Redirect(strRedirect, True)
        Else
            FormsAuthentication.RedirectFromLoginPage(txtLoginID.Text, False)
        End If

    End Sub

#End Region

#Region " Events "

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Dim logonFailedText As String = Nothing
            Dim authResult As ARAuthenticationResultsEnum = ARHelper.AuthenticateUser(Me.txtLoginID.Text.ToString.Trim, Me.txtPassword.Text.ToString.Trim, Nothing, Functions.GetAES256EncryptionKey(), Functions.GetAES256InitializationVector(), Functions.GetAES256CipherStringFormat())

            If authResult = ARAuthenticationResultsEnum.OK Then

                Dim maxPWDs As Boolean = False

                maxPWDs = Functions.MaxPasswordAttempts(Me.txtLoginID.Text.ToString().Trim(), Me.ARConn.ConnectionString)

                If (maxPWDs) Then

                    logonFailedText = Me.ResManager.GetString("LogonForm.lblLogonError.MaxWrongPasswordsAttempt", Me.CurrentCultureInfo)

                    RaiseEvent LogonFailed(authResult)

                    'Show Message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & logonFailedText & "');", True)

                Else

                    Me.CreateTicket()

                    Functions.ClearMaxPasswordAttempts(Me.txtLoginID.Text.ToString().Trim(), Me.ARConn.ConnectionString)

                    RaiseEvent LogonSuccessful()

                    Me.Redirect()

                End If

            Else

                RaiseEvent LogonFailed(authResult)

                Select Case authResult
                    Case ARAuthenticationResultsEnum.WrongPassword, ARAuthenticationResultsEnum.AccountDoesNotExist
                        If (Functions.MaxPasswordAttempts(Me.txtLoginID.Text.ToString().Trim(), Me.ARConn.ConnectionString)) Then
                            logonFailedText = Me.ResManager.GetString("LogonForm.lblLogonError.MaxWrongPasswordsAttempt", Me.CurrentCultureInfo)
                        Else
                            logonFailedText = Me.ResManager.GetString("LogonForm.lblLogonError.WrongPasswordOrAccount", Me.CurrentCultureInfo)
                        End If
                    Case ARAuthenticationResultsEnum.ExpiredPassword
                        logonFailedText = Me.ResManager.GetString("LogonForm.lblLogonError.PasswordExpired", Me.CurrentCultureInfo)
                    Case ARAuthenticationResultsEnum.LockedAccount
                        logonFailedText = Me.ResManager.GetString("LogonForm.lblLogonError.LockedAccount", Me.CurrentCultureInfo)
                End Select

                Dim user As ARUser = Me.ARConn.GetUserByLogin(Me.txtLoginID.Text.ToString.Trim)

                If Not user Is Nothing Then

                    Dim failedLoginCount As String = user.GetPropertyValue("FailedLoginCount")

                    failedLoginCount = CStr(CInt(IIf(failedLoginCount = "", "0", failedLoginCount)) + 1)

                    user.SetPropertyValue("FailedLoginCount", failedLoginCount)

                    If CInt(failedLoginCount) > 3 Then

                        logonFailedText = Me.ResManager.GetString("LogonForm.lblLogonError.LockedAccount", Me.CurrentCultureInfo)

                        user.SetPropertyValue("FailedLoginCount", "0")
                        user.ValidTo = Now
                        user.Update()

                    End If

                    Dim catalog As ARObject = Me.ARConn.GetCatalogObject

                    'log into auditing log
                    If catalog.GetPropertyValue("ar_StoreLogonAttemptsInAuditingLog") = "1" Then
                        Me.Log(user.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Log.LogonFailed", Me.CurrentCultureInfo) & " " & logonFailedText & "; Used user name: " & txtLoginID.Text, "FRITS")
                    End If

                End If

                'Show Message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & logonFailedText & "');", True)

            End If

        Catch ex As Exception
            Throw ex
        Finally
            'Me.ARConn.Close()
        End Try
    End Sub

    '## This event is raised when user successfully logs on.
    Public Event LogonSuccessful()

    '## This event is raised when logon fails.
    Public Event LogonFailed(ByVal result As ARAuthenticationResultsEnum)

    '## Occurs before user signs out.
    Public Event BeforeSignOut()

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.login"

            'Logout request 
            If Me.Request.QueryString("fn") = "logout" Then

                RaiseEvent BeforeSignOut()

                'Signout
                FormsAuthentication.SignOut()

                'Clear session
                Me.Session.Abandon()

                If Not Me.CurrentUser Is Nothing Then

                    'write into auditing log
                    Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID("FRITS.logout"), "Logged off successfully.", "FRITS")

                End If

            End If

            If Not Page.IsPostBack Then
                Me.txtLoginID.Focus()
            End If

        Catch ex As Exception
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub

End Class