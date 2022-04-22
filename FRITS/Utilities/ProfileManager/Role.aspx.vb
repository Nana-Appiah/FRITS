Partial Public Class Role
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    Private Sub GetData()
        Try
            Me.ARConn.ConnectToCatalog()

            Dim role As ARRole
            role = Me.ARConn.GetRoleByID(Me.ObjectID)

            If Not (role Is Nothing) Then

                Try
                    If Not role.ObjectName Is DBNull.Value Then
                        Me.txtName.Text = role.ObjectName
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not role.ObjectDescription Is DBNull.Value Then
                        Me.txtDesc.Text = role.ObjectDescription
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not IsDBNull(role.ValidFrom) Then
                        Me.PickerFrom.SelectedDate = role.ValidFrom
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not IsDBNull(role.ValidTo) Then
                        Me.PickerTo.SelectedDate = role.ValidTo
                    End If
                Catch ex As Exception
                End Try

            End If

            Me.LoadMemeberOfs()
            Me.LoadMemebers()

            Me.CalendarFrom.SelectedDate = Now.Date
            Me.CalendarTo.SelectedDate = Now.Date.AddYears(1)

        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

    Private Sub SaveData()
        Try
            Me.ARConn.ConnectToCatalog()

            Dim role As ARRole
            Dim FullAlias As String = "FRITS." & Me.txtName.Text.Replace(" ", "").ToLower()

            If Not Me.ObjectID = Nothing Then

                role = Me.ARConn.GetRoleByID(Me.ObjectID)

                role.ObjectName = Me.txtName.Text
                role.ObjectDescription = Me.txtDesc.Text
                role.ValidFrom = Me.PickerFrom.SelectedDate
                role.ValidTo = Me.PickerTo.SelectedDate
                role.Update()

                'Log into auditing log
                Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("RoleDetails.Log.Change", Thread.CurrentThread.CurrentUICulture) & " " & role.ObjectName, "FRITS")

                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Changes saved successfully.');parent.returnValue='reload';", True)

            Else
                role = New ARRole
                role.ARConnection = Me.ARConn

                Me.txtName.Text = Me.txtName.Text.Trim
                Me.txtDesc.Text = Me.txtDesc.Text.Trim

                If Me.txtName.Text = "" Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("RoleNew.lblErrorApplicationRoleName.Empty", Thread.CurrentThread.CurrentUICulture) & "');", True)
                    Exit Sub
                End If

                'check if there's an existing role for specified application with the same alias
                If Not (Me.ARConn.GetRoleByAlias(FullAlias) Is Nothing) Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("RoleNew.lblErrorObjectAlias.ExistingObjectAlias", Thread.CurrentThread.CurrentUICulture) & "');", True)
                    Exit Sub
                End If

                Try
                    'Create new role
                    role.ObjectName = Me.txtName.Text
                    role.ObjectDescription = Me.txtDesc.Text
                    role.ValidFrom = Me.PickerFrom.SelectedDate
                    role.ValidTo = Me.PickerTo.SelectedDate
                    role.ObjectAlias = FullAlias
                    role.InsertNew()

                    'Log into auditing log
                    Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("RoleNew.Log.InsertNew", Thread.CurrentThread.CurrentUICulture) & " " & role.ObjectAlias, "FRITS")

                    'Display message and close window
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("RoleNew.lblMessage.Created", Thread.CurrentThread.CurrentUICulture) & "');parent.returnValue='reload';", True)

                    'Clear Contrfams
                    Me.ClearContrfams()

                Catch exc As Exception
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("RoleNew.lblMessage.Error", Thread.CurrentThread.CurrentUICulture) & "');", True)
                    Exit Sub
                End Try

            End If
        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

    Private Sub ClearContrfams()

        Me.txtName.Text = ""
        Me.txtDesc.Text = ""
        Me.PickerFrom.SelectedDate = Now.Date
        Me.PickerTo.SelectedDate = Now.Date.AddYears(1)
        Me.CalendarFrom.SelectedDate = Now.Date
        Me.CalendarTo.SelectedDate = Now.Date.AddYears(1)

    End Sub

#Region " MemberOf"

    Private Sub LoadMemeberOfs()

        Me.ARConn.ConnectToCatalog()

        Dim Params(2, 1) As String
        Params(0, 0) = "ChildObjectID"
        Params(0, 1) = ObjectID
        Params(1, 0) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE
        Params(1, 1) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE
        Params(2, 0) = ARConstants.cRELATIONSHIP_TYPE_ALIAS
        Params(2, 1) = ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP

        Me.ARConn.ARDBProvider.Parameters = Params
        Me.ARConn.ARDBProvider.Table = "View_AR_Relationship_Joined"

        Dim ds As DataSet = Me.ARConn.ARDBProvider.SelectRecords

        Me.lstMemberOf.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode
        rootnode.ImageUrl = "~/Images/Icons/20x20/ApplicationRole.gif"
        rootnode.Text = "Member Of"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstMemberOf.Nodes.Add(rootnode)

        If ds.Tables(0).Rows.Count > 0 Then

            For Each dr As DataRow In ds.Tables(0).Rows

                If dr("ChildObjectTypeNamespace").ToLower = ARConstants.cUSERGROUP_TYPE_NAMESPACE.ToLower Then

                    Dim node As New ComponentArt.Web.UI.TreeViewNode

                    node.ImageUrl = "~/Images/Icons/16x16/ApplicationRole.gif"
                    node.Text = dr("ParentObjectName")
                    node.Value = dr("ParentObjectID")

                    rootnode.Nodes.Add(node)

                End If

            Next

        End If

        Me.lstMemberOf.Nodes.Sort()

        Me.ARConn.Close()

    End Sub

    Private Sub AddMemberOf(ByVal Params As Object)

        Me.ARConn.ConnectToCatalog()

        Dim AROperator As AROperator = Me.ARConn.GetOperatorByID(Me.ObjectID)

        Dim resourceAlias As String
        Dim permissionTypeAlias As String

        Select Case AROperator.ObjectType.ToLower
            Case LCase(ARConstants.cUSER_TYPE_NAMESPACE & "." & ARConstants.cUSER_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_USERS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_CHANGE
            Case LCase(ARConstants.cUSERGROUP_TYPE_NAMESPACE & "." & ARConstants.cUSERGROUP_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_GROUPS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_MANAGE
            Case LCase(ARConstants.cORG_UNIT_TYPE_NAMESPACE & "." & ARConstants.cORG_UNIT_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_ORGUNITS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_MANAGE
            Case LCase(ARConstants.cROLE_TYPE_NAMESPACE & "." & ARConstants.cROLE_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_APPLICATIONS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_MANAGE
        End Select

        Dim newRecords() As String = Split(Params, ";")
        Dim i As Integer
        Dim AddedObjectID As Integer
        Dim arObj As ARObject = Me.ARConn.GetObjectByID(Me.ObjectID)
        Dim arMsg As New System.Text.StringBuilder
        Dim iBelong As Boolean = False

        For i = 0 To newRecords.GetUpperBound(0)

            If IsNumeric(newRecords(i)) Then

                AddedObjectID = CInt(newRecords(i))

                If CheckReflexiveMembership(AddedObjectID, arObj.ObjectID) Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Replace(Me.ResManager.GetString("Membership.ReflexiveMembership.ShowMembersOfMode", Thread.CurrentThread.CurrentUICulture) & " " & Me.ARConn.GetObjectByID(AddedObjectID).ObjectName, "'", "\'") & "');", True)
                    Exit Sub
                Else
                    arObj.AddParent(AddedObjectID, ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE & "." & ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP, False, Now)
                    'Log into auditing log
                    Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorAdded", Thread.CurrentThread.CurrentUICulture) & " " & arObj.ObjectAlias, "FRITS")
                End If

            End If

        Next

        Me.ARConn.Close()

        Me.LoadMemeberOfs()

    End Sub

    Private Sub RemoveMemberOf(ByVal param As Object)

        Me.ARConn.ConnectToCatalog()

        Dim arObj As ARObject = Me.ARConn.GetObjectByID(Me.ObjectID)

        'check permissions
        If Not (Me.IsAuthorized(Me.CurrentUser, ARUIUtilities.GetResourceForObjectType(arObj.ObjectType), ARPermissionConstants.cPERMISSION_MANAGE)) Then

            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("Permissions.AccessDenied.Change", Thread.CurrentThread.CurrentUICulture) & "');", True)

            'Log into auditing log
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Permissions.AccessDenied.Change", Thread.CurrentThread.CurrentUICulture), "FRITS")

        Else

            Dim member As AROperator

            member = Me.ARConn.GetOperatorByID(Me.ObjectID)
            member.RemoveFromGroup(param)

            'Log into auditing log
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorRemoved", Thread.CurrentThread.CurrentUICulture) & " " & member.ObjectAlias, "FRITS")

            Me.ARConn.Close()

            Me.LoadMemeberOfs()

        End If

    End Sub
#End Region

#Region " Member"

    Private Sub LoadMemebers()

        Me.ARConn.ConnectToCatalog()

        Dim Params(2, 1) As String
        Params(0, 0) = "ParentObjectID"
        Params(0, 1) = ObjectID
        Params(1, 0) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE
        Params(1, 1) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE
        Params(2, 0) = ARConstants.cRELATIONSHIP_TYPE_ALIAS
        Params(2, 1) = ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP

        Me.ARConn.ARDBProvider.Parameters = Params
        Me.ARConn.ARDBProvider.Table = "View_AR_Relationship_Joined"

        Dim ds As DataSet = Me.ARConn.ARDBProvider.SelectRecords

        Me.lstMember.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode
        rootnode.ImageUrl = "~/Images/Icons/20x20/ApplicationRole.gif"
        rootnode.Text = "Member"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstMember.Nodes.Add(rootnode)

        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                If dr("ChildObjectTypeNamespace").ToLower = ARConstants.cUSER_TYPE_NAMESPACE.ToLower Then
                    Dim node As New ComponentArt.Web.UI.TreeViewNode
                    node.ImageUrl = "~/Images/Icons/16x16/User.gif"
                    node.Text = dr("ChildObjectName")
                    node.Value = dr("ChildObjectID")
                    rootnode.Nodes.Add(node)
                End If
            Next
        End If

        Me.lstMember.Nodes.Sort()

        Me.ARConn.Close()

    End Sub

    Private Sub AddMember(ByVal Params As Object)

        Me.ARConn.ConnectToCatalog()

        Dim resourceAlias As String
        Dim permissionTypeAlias As String

        Dim AROperator As AROperator = Me.ARConn.GetOperatorByID(Me.ObjectID)

        Select Case AROperator.ObjectType.ToLower
            Case LCase(ARConstants.cUSER_TYPE_NAMESPACE & "." & ARConstants.cUSER_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_USERS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_CHANGE
            Case LCase(ARConstants.cUSERGROUP_TYPE_NAMESPACE & "." & ARConstants.cUSERGROUP_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_GROUPS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_MANAGE
            Case LCase(ARConstants.cORG_UNIT_TYPE_NAMESPACE & "." & ARConstants.cORG_UNIT_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_ORGUNITS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_MANAGE
            Case LCase(ARConstants.cROLE_TYPE_NAMESPACE & "." & ARConstants.cROLE_TYPE_ALIAS)
                resourceAlias = ARPermissionConstants.cRESOURCE_APPLICATIONS
                permissionTypeAlias = ARPermissionConstants.cPERMISSION_MANAGE
        End Select

        Dim i As Integer
        Dim addedObjectID As Integer
        Dim newRecords() As String = Split(Params, ";")
        Dim arObj As ARObject = Me.ARConn.GetObjectByID(Me.ObjectID)
        Dim arMsg As New System.Text.StringBuilder
        Dim iBelong As Boolean = False

        For i = 0 To newRecords.GetUpperBound(0)

            If IsNumeric(newRecords(i)) Then

                addedObjectID = CInt(newRecords(i))

                If CheckReflexiveMembership(addedObjectID, arObj.ObjectID) Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Replace(Me.ResManager.GetString("Membership.ReflexiveMembership.ShowMembersMode", Thread.CurrentThread.CurrentUICulture) & " " & Me.ARConn.GetObjectByID(addedObjectID).ObjectName, "'", "\'") & "');", True)
                Else
                    arObj.AddChild(addedObjectID, ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE & "." & ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP, False, Now)
                    'Log into auditing log
                    Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorAdded", Thread.CurrentThread.CurrentUICulture) & " " & Me.ARConn.GetObjectByID(addedObjectID).ObjectAlias, "FRITS")
                End If

            End If

        Next

        Me.ARConn.Close()

        Me.LoadMemebers()

    End Sub

    Private Sub RemoveMember(ByVal param As Object)

        Me.ARConn.ConnectToCatalog()

        Dim arObj As ARObject = Me.ARConn.GetObjectByID(Me.ObjectID)

        'check permissions
        If Not (Me.IsAuthorized(Me.CurrentUser, ARUIUtilities.GetResourceForObjectType(arObj.ObjectType), ARPermissionConstants.cPERMISSION_MANAGE)) Then
            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("Permissions.AccessDenied.Change", Thread.CurrentThread.CurrentUICulture) & "');", True)
        Else
            Dim member As AROperator
            member = Me.ARConn.GetOperatorByID(param)
            member.RemoveFromGroup(Me.ObjectID)

            'Log into auditing log
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorRemoved", Thread.CurrentThread.CurrentUICulture) & " " & member.ObjectAlias, "FRITS")

            Me.ARConn.Close()

            Me.LoadMemebers()

        End If

    End Sub

#End Region

#End Region

#Region " Events "

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles ScriptManager.AsyncPostBackError
        Me.ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message
    End Sub

    Private Sub tbrMain_ItemCommand(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.ToolBarItemEventArgs) Handles tbrMain.ItemCommand
        If e.Item.Value = "Save" Then
            'Check permission to confirm save.
            If Not Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "change") Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You do not have the permission to save.');", True)
            Else
                Me.SaveData()
            End If
        End If
    End Sub

    Private Sub lstMemberOfCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstMemberOfCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "new:memberof" : Me.AddMemberOf(Right(e.Parameters(1), Len(e.Parameters(1)) - InStr(e.Parameters(1), "_")))
            Case "delete:memberof" : Me.RemoveMemberOf(e.Parameters(1))
            Case "load:user" : Me.LoadMemeberOfs()
        End Select
        Me.lstMemberOf.RenderControl(e.Output)
    End Sub

    Private Sub lstMemberCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstMemberCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "new:member" : Me.AddMember(Right(e.Parameters(1), Len(e.Parameters(1)) - InStr(e.Parameters(1), "_")))
            Case "delete:member" : Me.RemoveMember(e.Parameters(1))
            Case "load:member" : Me.LoadMemebers()
        End Select
        Me.lstMember.RenderControl(e.Output)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager.roles"

            If Not Request("id") = Nothing Then
                'Check access and permission.
                If Not Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "create") Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("Permissions.AccessDenied.Create", Thread.CurrentThread.CurrentUICulture) & "');", True)
                    Me.pnlContentPane.Visible = False
                    Exit Sub
                End If
            Else
                'Check access and permission.
                If Not Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "change") Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("Permissions.AccessDenied.Change", Thread.CurrentThread.CurrentUICulture) & "');", True)
                    Me.pnlContentPane.Visible = False
                    Exit Sub
                End If
            End If

            If Not Me.Page.IsPostBack Then

                If Not Request("id") = Nothing Then
                    Me.ObjectID = Me.Request("id")
                    Me.GetData()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "parent.window.document.title='Properties';", True)
                Else
                    Me.ClearContrfams()
                    Me.TabStrip.Tabs(1).Visible = False
                    Me.TabStrip.Tabs(2).Visible = False
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "parent.window.document.title='New Role';", True)
                End If

            End If

        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

End Class