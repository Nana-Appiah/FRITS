Imports System.DirectoryServices

Partial Public Class Selector
    Inherits PageBase

#Region " Properties"

    '## Filter of the searched objects. The default value is "ARObject.AROperator.", which includes users, groups and organizational units.
    Public Property ObjectTypeNamespace() As String
        Get
            Return ViewState("ObjectTypeNamespace")
        End Get
        Set(ByVal Value As String)
            ViewState("ObjectTypeNamespace") = Value
        End Set
    End Property

    '## If true, user can select more than one item. The default value is False.
    Public Property MultipleSelection() As Boolean
        Get
            Return ViewState("MultipleSelection")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("MultipleSelection") = Value
        End Set
    End Property

    Public Property ShowObjects() As String
        Get
            Return ViewState("ShowObjects")
        End Get
        Set(ByVal Value As String)
            ViewState("ShowObjects") = Value
        End Set
    End Property

#End Region

#Region " Methods"

    Private Sub GetData()

        Me.MultipleSelection = Request.QueryString("MultipleSelection")
        Me.ShowObjects = Request.QueryString("ObjectTypeNamespace")

        Dim objectTypeNamespace As String = ""

        Select Case Me.ShowObjects
            Case "Groups"
                Me.Title = "Groups"
                Me.ObjectTypeNamespace = "ARObject.AROperator.AROpContainer.ARGroup"
            Case "GroupsRolesOrgUnits"
                Me.Title = "Groups,Roles,Org. Units"
                Me.ObjectTypeNamespace = "ARObject.AROperator.AROpContainer"
            Case "OrgUnits"
                Me.Title = "Org. Units"
                Me.ObjectTypeNamespace = "ARObject.AROperator.AROpContainer.AROrgUnit"
            Case "Roles"
                Me.Title = "Roles"
                Me.ObjectTypeNamespace = "ARObject.AROperator.AROpContainer.ARRole"
            Case "Users"
                Me.Title = "Users"
                Me.ObjectTypeNamespace = "ARObject.AROperator.ARUser"
            Case "ActiveDirectory"
                Me.Title = "Active Directory"
                Me.ObjectTypeNamespace = "LDAP"
            Case "CustomTable"
                Me.Title = "Custom Users"
                Me.ObjectTypeNamespace = "CustomTable"
            Case Else
                Me.Title = "Operators"
                Me.ObjectTypeNamespace = "ARObject.AROperator"
        End Select

        If Me.ObjectTypeNamespace = "LDAP" Then

            ' Path to you LDAP directory server.
            ' Contact your network administrator to obtain a valid path.
            Dim adPath As String = "LDAP://" & ConfigurationManager.AppSettings("LDAPDomainName")
            Dim adConnUsername As String = ConfigurationManager.AppSettings("LDAPConnectionUserName")
            Dim adConnPassword As String = ConfigurationManager.AppSettings("LDAPConnectionPassword")

            Dim entry As DirectoryEntry = New DirectoryEntry(adPath, adConnUsername, adConnPassword)

            ' Bind to the native AdsObject to force authentication.
            Dim obj As Object = entry.NativeObject

            Dim search As DirectorySearcher = New DirectorySearcher(entry)
            search.Filter = "(objectCategory=person)"

            Dim result As SearchResult

            For Each result In search.FindAll()
                Me.lstSelection.Items.Add(New ListItem(result.Properties("cn")(0) & " - " & LCase(result.Properties("SAMAccountName")(0)), result.Path))
            Next

        ElseIf Me.ObjectTypeNamespace = "CustomTable" Then

            'Dim Misc As New BLL.Misc

            'Dim ds As DataSet = Misc.GetUsers

            'If ds.Tables(0).Rows.Count Then

            '    Dim objName As String

            '    Me.lstSelection.Items.Clear()

            '    For Each row As DataRow In ds.Tables(0).Rows

            '        objName = Trim(row("szFirst_nm")) & " " & Trim(row("szLast_nm"))

            '        If Not (row("szFirst_nm") Is System.DBNull.Value) Then
            '            objName &= " - " & Trim(row("szLogin_id"))
            '        End If

            '        Me.lstSelection.Items.Add(New ListItem(objName, row("pkID")))

            '    Next

            'End If

        Else

            Me.ARConn.ConnectToCatalog()

            Dim Params(2) As SqlClient.SqlParameter
            Dim ds As DataSet

            Params(0) = Me.ARConn.ARDBProvider.SetSQLParameter("@Substring", "", SqlDbType.NVarChar, 255)
            Params(1) = Me.ARConn.ARDBProvider.SetSQLParameter("@ObjectTypeNamespace", Me.ObjectTypeNamespace, SqlDbType.NVarChar, 255)
            Params(2) = Me.ARConn.ARDBProvider.SetSQLParameter("@TopN", 1000, SqlDbType.Int, 4)

            ds = Me.ARConn.ARDBProvider.ProcessStoredProcedure("Proc_AR_ObjectUser_Search", Params)

            Dim dr As DataRow
            Dim objName As String
            Dim ObjectTypeName As String = ""
            Me.lstSelection.Items.Clear()

            For Each dr In ds.Tables(0).Rows

                objName = dr("ObjectName")

                If Me.ARConn.GetObjectByID(dr("ObjectID")).ObjectAlias.ToString.StartsWith("secureaccess") = False Then

                    If Not (dr("LoginName") Is System.DBNull.Value) Then
                        objName &= " - " & dr("LoginName")
                    End If

                    'If Me.CurrentUser.IsMemberAll("Administrators") OrElse Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "read") Then
                    Me.lstSelection.Items.Add(New ListItem(objName, dr("ObjectID")))
                    'End If

                End If

            Next

            Me.ARConn.Close()

        End If

    End Sub

    Private Sub ClearContrfams()

    End Sub

#End Region

#Region " Events "

    'Private Sub tbrMain_ItemCommand(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.ToolBarItemEventArgs) Handles tbrMain.ItemCommand
    '    Try
    '        If e.Item.Value = "Save" Then
    '            'Check permission to confirm save.
    '            If Not Me.IsAuthorized(Me.CurrentUser, "FRITS.utilities.profilemanager", "Save") Then
    '                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You do not have the permission to save."))
    '                Exit Sub
    '            Else
    '                Me.SaveData()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        'Display message
    '        Me.ShowMessage(ex.Message)
    '    End Try
    'End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager"

            If Not Me.Page.IsPostBack Then
                Me.GetData()
            End If

            'Close secure access connection
            Me.ARConn.Close()

        Catch ex As Exception
            'Display message
            Me.ShowMessage(ex.Message)
        End Try
    End Sub

End Class