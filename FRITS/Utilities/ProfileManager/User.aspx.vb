Partial Public Class User
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    Private Sub GetData()
        Try
            Me.ARConn.ConnectToCatalog()

            Dim user As ARUser
            user = Me.ARConn.GetUserByID(Me.ObjectID)

            If Not (user Is Nothing) Then

                Try
                    If Not user.Login Is DBNull.Value Then
                        Me.txtUserName.Text = user.Login
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not user.ObjectName Is DBNull.Value Then
                        Me.txtName.Text = user.ObjectName
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not user.ObjectDescription Is DBNull.Value Then
                        Me.txtDesc.Text = user.ObjectDescription
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not user.EmailAddress Is DBNull.Value Then
                        Me.txtEmailAddress.Text = user.EmailAddress
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not (user.GetPropertyValue("BranchCode") Is String.Empty) Then
                        Me.ddlBranch.SelectedValue = IIf(user.GetPropertyValue("BranchCode") Is Nothing, 0, user.GetPropertyValue("BranchCode"))
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not (user.GetPropertyValue("DepartmentCode") Is String.Empty) Then
                        Me.ddlDepartment.SelectedValue = IIf(user.GetPropertyValue("DepartmentCode") Is Nothing, 0, user.GetPropertyValue("DepartmentCode"))
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not IsDBNull(user.ValidFrom) Then
                        Me.PickerFrom.SelectedDate = user.ValidFrom
                    End If
                Catch ex As Exception
                End Try

                Try
                    If Not IsDBNull(user.ValidTo) Then
                        Me.PickerTo.SelectedDate = user.ValidTo
                    End If
                Catch ex As Exception
                End Try

            End If

            Me.LoadMemeberOf()
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

            Dim user As ARUser

            If Not Me.ObjectID = Nothing Then

                user = Me.ARConn.GetUserByID(Me.ObjectID)

                ' Assign values
                user.Login = Me.txtUserName.Text
                user.ObjectName = Me.txtName.Text
                user.ObjectAlias = Me.txtUserName.Text.Replace(" ", "_").ToLower
                user.ObjectDescription = Me.txtDesc.Text
                user.EmailAddress = Me.txtEmailAddress.Text
                user.ValidFrom = Me.PickerFrom.SelectedDate
                user.ValidTo = Me.PickerTo.SelectedDate

                If Me.chkChangePassword.Checked = True Then

                    If Me.txtPassword.Text = "" Then
                        'Display message
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You must enter a password for this user.');", True)
                        Exit Sub
                    ElseIf Not Me.txtPassword.Text = Me.txtConfirm.Text Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Password confirmation failed. Please make sure passwords match.');", True)
                        Exit Sub
                    End If

                    'Set new password
                    user.Password = Me.txtPassword.Text
                    user.PasswordEncryptionType = AREncryptionTypesEnum.MD5
                    user.PasswordExpires = Now.Date.AddDays(45)

                End If

                'Set custom properties
                user.SetPropertyValue("BranchCode", Me.ddlBranch.SelectedValue)
                user.SetPropertyValue("BranchName", Me.ddlBranch.SelectedItem.Text)
                user.SetPropertyValue("DepartmentName", Me.ddlDepartment.SelectedItem.Text)
                user.SetPropertyValue("DepartmentCode", Me.ddlDepartment.SelectedItem.Value)

                'Update user object
                user.Update()

                If Me.chkChangePassword.Checked = True Then

                    'Send email notification 
                    If IIf(Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("AllowNotification") = "Yes", True, False) Then

                        'Send email notification 
                        Dim email_message As String = ""

                        email_message &= "Your user name and password are:"
                        email_message &= "<br><br><b>User name:</b>" & " " & user.Login
                        email_message &= "<br><b>Password:</b>" & " " & user.Password
                        email_message &= "<br><br>Please log on by visiting <a href='" & Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "'>" & Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "</a>"

                        Try
                            Me.SendMail(user.EmailAddress, "User Account Information", email_message, , , , True)

                        Catch ex As Exception
                            If ex.Message = "Could not access 'CDO.Message' object." Then
                                'Write into auditing log
                                Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "frits")
                                Throw ex
                                Exit Sub
                            Else
                                'Write into auditing log
                                Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "frits")
                                Throw ex
                                Exit Sub
                            End If
                        End Try

                    End If

                End If

                'Log into auditing log
                Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("UserDetails.Log.Change", Thread.CurrentThread.CurrentUICulture) & " " & user.Login, "frits")

                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Changes saved successfully.');parent.returnValue='reload';", True)

            Else

                user = New ARUser
                user.ARConnection = Me.ARConn

                Dim CheckLoginResult As ARCheckLoginAliasResultsEnum

                'Check user name:        
                CheckLoginResult = user.CheckLogin(txtUserName.Text)

                If Not CheckLoginResult = ARCheckLoginAliasResultsEnum.OK Then

                    Select Case CheckLoginResult
                        Case ARCheckLoginAliasResultsEnum.Empty
                            'Display message
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("UserNew.reqUserName.Empty", Thread.CurrentThread.CurrentUICulture) & "');", True)
                            Exit Sub
                        Case ARCheckLoginAliasResultsEnum.ForbiddenCharacters
                            'Display message
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("UserNew.reqUserName.ForbiddenCharacters", Thread.CurrentThread.CurrentUICulture) & " " & ARConstants.cUSERNAME_FORBIDDEN_CHARACTERS.Replace(".", "") & "');", True)
                            Exit Sub
                        Case ARCheckLoginAliasResultsEnum.AlreadyExists
                            Dim tmpNo As Integer = 1
                            Do While tmpNo < 10000
                                'Generate new username by increasing added integer.
                                CheckLoginResult = user.CheckLogin(Me.txtUserName.Text.Trim & tmpNo)
                                If CheckLoginResult = ARCheckLoginAliasResultsEnum.AlreadyExists Then
                                    tmpNo += 1
                                Else
                                    Me.txtUserName.Text = Me.txtUserName.Text & tmpNo
                                    Exit Do
                                End If
                            Loop
                            Me.txtName.Focus()

                            'Display message
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("UserNew.reqUserName.Exists", Thread.CurrentThread.CurrentUICulture) & " Try " & Me.txtUserName.Text & "." & "');", True)
                            Exit Sub
                    End Select

                ElseIf Me.txtPassword.Text = "" Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You must enter a password for this user.');", True)
                    Exit Sub
                ElseIf Not Me.txtPassword.Text = Me.txtConfirm.Text Then
                    'Display message
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Password confirmation failed. Please make sure passwords match.');", True)
                    Exit Sub
                End If

                'Create new user
                Dim cn As New SqlConnection(ConfigurationManager.ConnectionStrings("SecureAccessConnectionString").ConnectionString)
                Dim cmd As New SqlCommand

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Proc_AR_User_InsertShort"
                cmd.Connection = cn
                cn.Open()

                With cmd.Parameters

                    .Add(New SqlParameter("@ObjectName", SqlDbType.NVarChar)).Value = Me.txtName.Text
                    .Add(New SqlParameter("@ObjectAlias", SqlDbType.NVarChar)).Value = Me.txtUserName.Text.Replace(" ", "_").ToLower
                    .Add(New SqlParameter("@ObjectTypeID", SqlDbType.Int)).Value = 24
                    .Add(New SqlParameter("@ObjectValidFrom", SqlDbType.DateTime)).Value = Me.PickerFrom.SelectedDate
                    .Add(New SqlParameter("@ObjectValidTo", SqlDbType.DateTime)).Value = Me.PickerTo.SelectedDate
                    .Add(New SqlParameter("@ObjectDescription", SqlDbType.NVarChar)).Value = Me.txtDesc.Text
                    .Add(New SqlParameter("@Loginname", SqlDbType.NVarChar)).Value = Me.txtUserName.Text
                    .Add(New SqlParameter("@Password", SqlDbType.NVarChar)).Value = Me.txtPassword.Text
                    .Add(New SqlParameter("@PasswordEncryption", SqlDbType.NVarChar)).Value = "MD5"
                    .Add(New SqlParameter("@PasswordExpires", SqlDbType.DateTime)).Value = Me.PickerTo.SelectedDate
                    .Add(New SqlParameter("@EmailAddress", SqlDbType.NVarChar)).Value = Me.txtEmailAddress.Text
                    .Add(New SqlParameter("@ObjectCustomField1", SqlDbType.NVarChar)).Value = 0
                    .Add(New SqlParameter("@ObjectCustomField2", SqlDbType.UniqueIdentifier)).Value = DBNull.Value

                End With

                cmd.ExecuteNonQuery()

                'Get user details and update
                user = Me.ARConn.GetUserByLogin(Me.txtUserName.Text)

                'Set custom properties
                user.PasswordEncryptionType = AREncryptionTypesEnum.MD5
                user.Password = Me.txtPassword.Text
                user.PasswordExpires = Now.Date.AddDays(45)

                'Set custom properties
                user.SetPropertyValue("FirstTimeLogOn", "Yes")
                user.SetPropertyValue("BranchCode", Me.ddlBranch.SelectedValue)
                user.SetPropertyValue("BranchName", Me.ddlBranch.SelectedItem.Text)
                user.SetPropertyValue("DepartmentName", Me.ddlDepartment.SelectedItem.Text)
                user.SetPropertyValue("DepartmentCode", Me.ddlDepartment.SelectedItem.Value)

                'Update user object
                user.Update()

                Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("UserNew.Log.InsertNew", Thread.CurrentThread.CurrentUICulture) & " " & user.Login, "frits")

                Dim Msg As New System.Text.StringBuilder

                'Send email notification 
                If IIf(Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("AllowNotification") = "Yes", True, False) Then

                    Dim email_message As String = ""

                    email_message &= "Your user name and password are:"
                    email_message &= "<br><br><b>User name:</b>" & " " & user.Login
                    email_message &= "<br><b>Password:</b>" & " " & user.Password
                    email_message &= "<br><br>Please log on by visiting <a href=""" & Me.ARConn.GetApplicationByAlias("fams").GetPropertyValue("HTTPServer") & Me.Request.ApplicationPath & "'>" & Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("HTTPServer") & "</a>."

                    Try
                        Me.SendMail(user.EmailAddress, "User Account Information", email_message, , , , True)
                    Catch ex As Exception
                        If ex.Message = "Could not access 'CDO.Message' object." Then
                            'Write into auditing log
                            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "frits")
                            'Display message
                            Msg.Append("Unable to send notification email. Please configure SMTP settings." & vbCrLf)
                        Else
                            'Display message
                            Msg.Append(ex.Message & vbCrLf)
                        End If
                    End Try

                End If

                'Display message
                Msg.Append("New user created successfully.")

                'Display message
                If Msg.Length > 0 Then
                    'Display message and close window
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Msg.ToString & "');parent.returnValue='reload';", True)
                End If

            End If
        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

    Private Sub ClearContrfams()

        Me.txtUserName.Text = ""
        Me.txtName.Text = ""
        Me.txtDesc.Text = ""
        Me.txtEmailAddress.Text = ""
        Me.ddlBranch.ClearSelection()
        Me.ddlDepartment.ClearSelection()
        Me.txtPassword.Text = ""
        Me.txtPassword.Enabled = True
        Me.txtConfirm.Text = ""
        Me.txtConfirm.Enabled = True
        Me.PickerFrom.SelectedDate = Now.Date
        Me.PickerTo.SelectedDate = Nothing
        Me.CalendarFrom.SelectedDate = Now.Date
        Me.CalendarTo.SelectedDate = Now.Date.AddYears(1)

    End Sub

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
        rootnode.ImageUrl = "~/Images/Icons/20x20/User.gif"
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
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Replace(Me.ResManager.GetString("Membership.ReflexiveMembership.ShowMembersMode", Thread.CurrentThread.CurrentUICulture) & " " & Me.ARConn.GetObjectByID(AddedObjectID).ObjectName, "'", "\'") & "');", True)
                Else
                    arObj.AddChild(AddedObjectID, ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE & "." & ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP, False, Now)
                    'Log into auditing log
                    Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorAdded", Thread.CurrentThread.CurrentUICulture) & " " & Me.ARConn.GetObjectByID(AddedObjectID).ObjectAlias, "frits")
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
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorRemoved", Thread.CurrentThread.CurrentUICulture) & " " & member.ObjectAlias, "frits")

            Me.ARConn.Close()

            Me.LoadMemebers()

        End If

    End Sub

#End Region

#Region " Member Of"

    Private Sub LoadMemeberOf()

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
        rootnode.ImageUrl = "~/Images/Icons/20x20/UserGroup.gif"
        rootnode.Text = "Member Of"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstMemberOf.Nodes.Add(rootnode)

        If ds.Tables(0).Rows.Count > 0 Then

            For Each dr As DataRow In ds.Tables(0).Rows

                If dr("ParentObjectTypeNamespace").ToLower = ARConstants.cUSERGROUP_TYPE_NAMESPACE.ToLower OrElse dr("ParentObjectTypeNamespace").ToLower = ARConstants.cORG_UNIT_TYPE_NAMESPACE.ToLower Then

                    Dim node As New ComponentArt.Web.UI.TreeViewNode

                    node.ImageUrl = "~/Images/Icons/16x16/UserGroup.gif"
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
                    'Close secure access connection
                    Me.ARConn.Close()
                    Exit Sub
                Else
                    arObj.AddParent(AddedObjectID, ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE & "." & ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP, False, Now)
                    'Log into auditing log
                    Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorAdded", Thread.CurrentThread.CurrentUICulture) & " " & arObj.ObjectAlias, "frits")
                End If

            End If

        Next

        Me.ARConn.Close()

        Me.LoadMemeberOf()

    End Sub

    Private Sub RemoveMemberOf(ByVal param As Object)

        Me.ARConn.ConnectToCatalog()

        Dim arObj As ARObject = Me.ARConn.GetObjectByID(Me.ObjectID)

        'check permissions
        If Not (Me.IsAuthorized(Me.CurrentUser, ARUIUtilities.GetResourceForObjectType(arObj.ObjectType), ARPermissionConstants.cPERMISSION_MANAGE)) Then
            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("Permissions.AccessDenied.Change", Thread.CurrentThread.CurrentUICulture) & "');", True)
            'Close secure access connection
            Me.ARConn.Close()
        Else
            Dim member As AROperator
            member = Me.ARConn.GetOperatorByID(Me.ObjectID)
            member.RemoveFromGroup(param)
            'Log into auditing log
            Me.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("Membership.Log.OperatorRemoved", Thread.CurrentThread.CurrentUICulture) & " " & member.ObjectAlias, "frits")
        End If

        'Close secure access connection
        Me.ARConn.Close()

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
            Case "load:user" : Me.LoadMemeberOf()
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

    Private Sub chkChangePassword_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkChangePassword.CheckedChanged
        Me.txtPassword.Enabled = Me.chkChangePassword.Checked
        Me.txtConfirm.Enabled = Me.chkChangePassword.Checked
        If Me.chkChangePassword.Checked Then
            Me.txtPassword.Focus()
        End If
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager.users"

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

                Me.BranchSqlDataSource.SelectCommandType = SqlDataSourceCommandType.Text
                Me.BranchSqlDataSource.SelectCommand = "SELECT Code, Name FROM [dbo].[Branches]"

                Me.ddlBranch.Items.Clear()
                Me.ddlBranch.DataBind()
                Me.ddlBranch.Items.Insert(0, New ListItem("", 0))

                Me.DepartmentSqlDataSource.SelectCommandType = SqlDataSourceCommandType.Text
                Me.DepartmentSqlDataSource.SelectCommand = "SELECT DepartmentID, Name FROM [dbo].[Department]"

                Me.ddlDepartment.Items.Clear()
                Me.ddlDepartment.DataBind()
                Me.ddlDepartment.Items.Insert(0, New ListItem("", 0))


                If Not Request("id") = Nothing Then
                    Me.ObjectID = Me.Request("id")
                    Me.ltlChange.Visible = True
                    Me.GetData()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "parent.window.document.title='Properties';", True)
                Else
                    Me.ClearContrfams()
                    Me.TabStrip.Tabs(1).Visible = False
                    Me.TabStrip.Tabs(2).Visible = False
                    Me.ltlChange.Visible = False
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "parent.window.document.title='New User';", True)
                End If

            End If
        Catch ex As Exception
            'Display message
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

End Class