Imports System.Globalization
Imports System.Threading
Imports PortSight.SecureAccess.ARDataServices
Imports PortSight.SecureAccess.ARObjects

Public Class Global_asax
    Inherits HttpApplication


    Public Shared Property _holdHelper As HoldService

    Sub New()
        _holdHelper = HoldService.Instance()
    End Sub


    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the application is started
        Call DefineAppRoutes()
        Dim Resman As System.Resources.ResourceManager
        Try
            ' Build on version 1.X
            Dim assmbl As System.Reflection.Assembly
            assmbl = System.Reflection.Assembly.Load("SecureAccess")
            Resman = New System.Resources.ResourceManager("SecureAccess.strings", assmbl)
        Catch
            ' Build on version 2.X
            Resman = New System.Resources.ResourceManager("Resources.strings", System.Reflection.Assembly.Load("App_GlobalResources"))
        End Try

        Application("RM") = Resman

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        'Fires when the session is started
        Dim arcn As New ARConnection()
        arcn.ConnectToCatalog()

        Dim readUserData As Boolean = False
        Dim currentUser As New ARUser

        If User.Identity.IsAuthenticated Then
            readUserData = True
            currentUser = arcn.GetUserByLogin(User.Identity.Name)
            If currentUser Is Nothing Then
                'user was not found in the database -> access denied
                arcn.Close()
                Response.Redirect("AccessDenied.aspx")
            Else
                If currentUser.IsLocked Then
                    'account is locked -> access denied
                    arcn.Close()
                    Response.Redirect("AccessDenied.aspx")
                End If
            End If
        End If

        If readUserData Then
            Try
                Dim catalog As ARObject = arcn.GetCatalogObject

                'create user ticket and store it in a session variable
                Session("ARUserTicket") = New ARUserTicket(currentUser)

                'set prefered culture
                Dim userCultureString As String = Nothing
                Dim defaultCultureString As String = ConfigurationManager.AppSettings(ARConstants.cDEFAULT_CULTURE) & ""
                If defaultCultureString = "" Then
                    defaultCultureString = "en-US"
                End If

                userCultureString = currentUser.GetPropertyValue(ARCatalogSettingsConstants.cPREFERED_CULTURE) & ""
                If userCultureString = "" Then
                    'if user doesn't prefere any culture, use the default culture of the catalog
                    userCultureString = ConfigurationManager.AppSettings(ARConstants.cDEFAULT_CULTURE) & ""
                End If
                Try
                    If userCultureString <> "" Then
                        Session("ARCulture") = New CultureInfo(userCultureString)
                    Else
                        Session("ARCulture") = New CultureInfo(defaultCultureString)
                    End If
                Catch
                    Session("ARCulture") = New CultureInfo("en-US")
                End Try

                'write into auditing log
                If Application("StoreLogonAttemptsInAuditingLog") = "1" Then
                    ' globalization
                    Thread.CurrentThread.CurrentCulture = Session("ARCulture")
                    Thread.CurrentThread.CurrentUICulture = Session("ARCulture")
                    Dim rm As System.Resources.ResourceManager = CType(Application("RM"), System.Resources.ResourceManager)
                    arcn.Log(currentUser.ObjectID, currentUser.ObjectID, rm.GetString("Log.LogonSuccessful", Thread.CurrentThread.CurrentUICulture), ARLogConstants.cLOG_LOGON_SUCCESSFUL)
                End If
            Catch
            Finally
                arcn.Close()
            End Try
        End If
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        If (Request.Path.IndexOf(Chr(92)) >= 0 Or System.IO.Path.GetFullPath(Request.PhysicalPath) <> Request.PhysicalPath) Then
            Throw New HttpException(404, "Not Found")
        End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
        'Application("MyContainer") = Nothing
    End Sub

    Private Sub DefineAppRoutes()
        RouteTable.Routes.MapPageRoute("systemsetup", "systemsetup", "~/App/SystemSetup.aspx")
        RouteTable.Routes.MapPageRoute("reviews/record", "reviews/record", "~/App/RecordReview.aspx")
        RouteTable.Routes.MapPageRoute("reviews/manage", "reivews/manage", "~/App/Reviews.aspx")
    End Sub

End Class