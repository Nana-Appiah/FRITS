Imports System.DirectoryServices
Imports PortSight.SecureAccess

Partial Public Class ProfileManager_Default
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    '## This method hide permission checkboxes.
    Private Sub HidePermissions()

        Me.divActivate.Visible = False
        Me.divCreate.Visible = False
        Me.divCheck.Visible = False
        Me.divChange.Visible = False
        Me.divDelete.Visible = False
        Me.divRead.Visible = False
        Me.divUpload.Visible = False
        Me.divSubmit.Visible = False
        Me.divCancel.Visible = False
        Me.divAuthorize.Visible = False
        Me.divDecline.Visible = False
        Me.divRelease.Visible = False
        Me.divManage.Visible = False
        Me.divCommit.Visible = False
        Me.divGenerate.Visible = False
        Me.divPrint.Visible = False
        Me.divExport.Visible = False
        Me.divList.Visible = False
        Me.divSend.Visible = False
        Me.divScan.Visible = False
        Me.divReset.Visible = False
        Me.divReverse.Visible = False
        Me.divOrder.Visible = False
        Me.divReceive.Visible = False
        Me.divSearch.Visible = False

    End Sub

    '## This method clear permission checkboxes.
    Private Sub ClearPermissions()

        Me.chkActivate.Checked = False
        Me.chkCreate.Checked = False
        Me.chkCheck.Checked = False
        Me.chkChange.Checked = False
        Me.chkDelete.Checked = False
        Me.chkRead.Checked = False
        Me.chkUpload.Checked = False
        Me.chkSubmit.Checked = False
        Me.chkCancel.Checked = False
        Me.chkAuthorize.Checked = False
        Me.chkDecline.Checked = False
        Me.chkRelease.Checked = False
        Me.chkManage.Checked = False
        Me.chkCommit.Checked = False
        Me.chkGenerate.Checked = False
        Me.chkPrint.Checked = False
        Me.chkExport.Checked = False
        Me.chkList.Checked = False
        Me.chkSend.Checked = False
        Me.chkScan.Checked = False
        Me.chkReset.Checked = False
        Me.chkReverse.Checked = False
        Me.chkOrder.Checked = False
        Me.chkReceive.Checked = False
        Me.chkSearch.Checked = False

    End Sub

    '## Converts Relationship Type to its ID value.
    Protected Overloads Function GetRelationshipTypeID(ByVal provider As ARDBConnection, ByVal strNamespace As String, ByVal strAlias As String) As Integer
        '##PARAM RelationshipType Relationship Type in form <namespace>.<alias>
        Dim ds As DataSet
        'Create Array of conditions for selecting proper Relationshiptype
        Dim CondArray(1, 1) As String
        CondArray(0, 0) = "RelationshipTypeNamespace"
        CondArray(0, 1) = strNamespace
        CondArray(1, 0) = "RelationshipTypeAlias"
        CondArray(1, 1) = strAlias
        Dim ARDBRelationshiptype As ARDBRelationshipType = New ARDBRelationshipType
        ARDBRelationshiptype.Connection = provider.Connection
        ARDBRelationshiptype.Parameters = CondArray
        ds = ARDBRelationshiptype.SelectRecords()
        GetRelationshipTypeID = CInt(ds.Tables(0).Rows(0)("RelationshipTypeID"))
        ARDBRelationshiptype.Close()
        ARDBRelationshiptype = Nothing
    End Function

    '## This method imports users from a custom table.
    Private Sub ImportCustomTable()
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            Dim arcn As New ARConnection()
            arcn.ConnectToCatalog("Data Source=10.150.0.241;Initial Catalog=frits;User ID=sa;Password=$Passw0rd")

            arcn.ARDBProvider.Table = "View_AR_User_Joined"

            Dim ds As DataSet = arcn.ARDBProvider.SelectRecords

            If ds.Tables(0).Rows.Count > 0 Then

                'Create new user
                Dim cnn As New SqlConnection("Data Source=10.150.0.241;Initial Catalog=SecureAccess;User ID=sa;Password=$Passw0rd")
                Dim cmd As New SqlCommand

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Proc_AR_User_InsertShort"
                cmd.Connection = cnn
                cmd.CommandTimeout = 36000
                cnn.Open()

                Dim user As ARUser

                For Each dr As DataRow In ds.Tables(0).Rows

                    If Not dr("ObjectID") = "337" Then

                        user = Me.ARConn.GetUserByEmail(dr("EmailAddress"))

                        If user Is Nothing Then

                            With cmd.Parameters

                                .Clear()

                                .Add(New SqlParameter("@ObjectName", SqlDbType.NVarChar)).Value = dr("ObjectName")
                                .Add(New SqlParameter("@ObjectAlias", SqlDbType.NVarChar)).Value = dr("ObjectAlias")
                                .Add(New SqlParameter("@ObjectTypeID", SqlDbType.Int)).Value = dr("ObjectTypeID")
                                .Add(New SqlParameter("@ObjectValidFrom", SqlDbType.DateTime)).Value = dr("ObjectValidFrom")
                                .Add(New SqlParameter("@ObjectValidTo", SqlDbType.DateTime)).Value = dr("ObjectValidTo")
                                .Add(New SqlParameter("@ObjectDescription", SqlDbType.NVarChar)).Value = dr("ObjectDescription")
                                .Add(New SqlParameter("@ObjectCustomField1", SqlDbType.NVarChar)).Value = dr("ObjectCustomField1")
                                .Add(New SqlParameter("@Loginname", SqlDbType.NVarChar)).Value = dr("Loginname")
                                .Add(New SqlParameter("@Password", SqlDbType.NVarChar)).Value = dr("Password")
                                .Add(New SqlParameter("@PasswordEncryption", SqlDbType.NVarChar)).Value = dr("PasswordEncryption")
                                .Add(New SqlParameter("@PasswordExpires", SqlDbType.DateTime)).Value = DBNull.Value
                                .Add(New SqlParameter("@EmailAddress", SqlDbType.NVarChar)).Value = dr("EmailAddress")

                            End With

                            Try
                                cmd.ExecuteNonQuery()
                            Catch ex As Exception
                            End Try

                        Else

                            If IsNumeric(user.ObjectID) Then
                                Dim arObj As ARObject = arcn.GetObjectByID(11690)
                                arObj.AddChild(user.ObjectID, ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE & "." & ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP, False, Now)
                            End If

                        End If

                    End If

                Next

                Me.LoadUsers()

            End If
        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

    '## This method imports users from a file.
    Private Sub ImportFile(Optional ByVal filename As String = "")
        Try
            Dim fi As New FileInfo("D:\users.csv")

            'Load data into datatable
            Dim dt As DataTable = GetDataFromFileCsv(fi.DirectoryName, fi.Name)

            If dt.Rows.Count > 0 Then

                Dim arcn As New ARConnection()
                arcn.ConnectToCatalog(ConfigurationManager.ConnectionStrings("SecureAccessConnectionString").ConnectionString)

                'Create new user
                Dim cn As New SqlConnection(ConfigurationManager.ConnectionStrings("SecureAccessConnectionString").ConnectionString)
                Dim cmd As New SqlCommand

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Proc_AR_User_InsertShort"
                cmd.Connection = cn
                cmd.CommandTimeout = 36000
                cn.Open()

                Dim user As ARUser

                For Each dr As DataRow In dt.Rows

                    user = arcn.GetUserByEmail(dr(2).ToString.ToLower)

                    If user Is Nothing Then
                        Try
                            Dim username As String = dr(1).ToString.ToLower

                            user = New ARUser
                            user.ARConnection = arcn

                            With cmd.Parameters

                                .Clear()

                                .Add(New SqlParameter("@ObjectName", SqlDbType.NVarChar)).Value = dr(0).ToString
                                .Add(New SqlParameter("@ObjectAlias", SqlDbType.NVarChar)).Value = username
                                .Add(New SqlParameter("@ObjectTypeID", SqlDbType.Int)).Value = 24
                                .Add(New SqlParameter("@ObjectValidFrom", SqlDbType.DateTime)).Value = Now.Date
                                .Add(New SqlParameter("@ObjectValidTo", SqlDbType.DateTime)).Value = Now.Date.AddDays(45)
                                .Add(New SqlParameter("@ObjectDescription", SqlDbType.NVarChar)).Value = ""
                                .Add(New SqlParameter("@Loginname", SqlDbType.NVarChar)).Value = username
                                .Add(New SqlParameter("@Password", SqlDbType.NVarChar)).Value = "Admin123"
                                .Add(New SqlParameter("@PasswordEncryption", SqlDbType.NVarChar)).Value = "MD5"
                                .Add(New SqlParameter("@PasswordExpires", SqlDbType.DateTime)).Value = Now.Date.AddDays(45)
                                .Add(New SqlParameter("@EmailAddress", SqlDbType.NVarChar)).Value = dr(2).ToString.ToLower
                                .Add(New SqlParameter("@ObjectCustomField1", SqlDbType.NVarChar)).Value = 0
                                .Add(New SqlParameter("@ObjectCustomField2", SqlDbType.UniqueIdentifier)).Value = DBNull.Value

                            End With

                            cmd.ExecuteNonQuery()

                            user = arcn.GetUserByEmail(dr(2).ToString.ToLower)

                            If IsNumeric(user.ObjectID) Then
                                Dim arObj As ARObject = arcn.GetObjectByID(999)
                                arObj.AddChild(user.ObjectID, ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE & "." & ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP, False, Now)
                            End If
                        Catch ex As Exception
                        End Try
                    End If

                Next

                'Popup message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Upload successful');", True)

                Me.LoadUsers()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetDataFromFileCsv(ByVal strFolderPath As String, ByVal strFileName As String) As DataTable
        Try
            'CharacterSet=65001 will needed for UTF-8 settings
            Dim strConnString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strFolderPath & ";Extended Properties='text;HDR=Yes;FMT=Delimited;CharacterSet=65001;'"
            Dim conn As New OleDbConnection(strConnString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT * FROM [" & strFileName & "]", conn)
                Dim da As New OleDbDataAdapter()

                da.SelectCommand = cmd
                Dim ds As New DataSet()

                da.Fill(ds)
                da.Dispose()
                Return ds.Tables(0)
            Catch
                Return Nothing
            Finally
                conn.Close()
            End Try
        Catch ex As Exception
            Return New DataTable()
        End Try
    End Function

#Region " Add New"

    Private Sub AddOperator(Optional ByVal resourceid As Object = 0, Optional ByVal operatorid As Object = 0)
        Dim userArr() As String
        Dim i As Integer
        userArr = Split(operatorid, ";")
        Dim arcn As New ARObjects.ARConnection
        arcn.ConnectToCatalog()
        Dim resource As ARObjects.ARResource = arcn.GetResourceByID(resourceid)
        For i = userArr.GetLowerBound(0) To userArr.GetUpperBound(0)
            If IsNumeric(userArr(i)) Then
                resource.AddChild(CInt(userArr(i)), ARConstants.cRELATIONSHIP_NULLRELATIONSHIP, False, Now)
                Dim arrResource As String() = resource.ObjectAlias.Split("."c)
                For j = 0 To arrResource.Length - 2
                    If j > 0 Then
                        Dim objAlias As String = "frits." & arrResource(j)
                        Dim obj As ARResource = arcn.GetResourceByAlias(objAlias)
                        obj.GrantAccess(CInt(userArr(i)), GetRelationshipTypeID(arcn.ARDBProvider, objAlias, "List"))
                    End If
                Next
                Dim arOperator As String = arcn.GetObjectByID(userArr(i)).ObjectName
                'Log into auditing log
                arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("ARUIMembership.Log.OperatorAdded", Thread.CurrentThread.CurrentUICulture) & " " & arOperator, "secureaccess_" & ARLogConstants.cLOG_MEMBER_ADDED)
            End If
        Next
        arcn.Close()
        Me.LoadApplicationParts()
    End Sub

    Private Sub AddCustomTableUser(ByVal Params As Object)

        Dim bsuccess As Boolean = False

        Dim msg As New System.Text.StringBuilder

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim user As New ARUser
        user.ARConnection = arcn

        Dim userArr() As String
        Dim i As Integer
        userArr = Split(Params, ";")

        For i = userArr.GetLowerBound(0) To userArr.GetUpperBound(0)

            If IsNumeric(userArr(i)) Then

                Dim ds As DataSet = Nothing

                'ds = (New Misc).GetUserDetails(CInt(userArr(i)))

                If ds.Tables(0).Rows.Count > 0 Then

                    Dim cn As SqlConnection
                    Dim cmd As New SqlCommand
                    cn = New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
                    cn.Open()

                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "Proc_AR_User_InsertShort"
                    cmd.Connection = cn

                    With cmd.Parameters
                        .Clear()
                        .Add(New SqlParameter("@ObjectName", SqlDbType.NVarChar)).Value = Trim(ds.Tables(0).Rows(0)("szFirst_nm")) & " " & Trim(ds.Tables(0).Rows(0)("szLast_nm"))
                        .Add(New SqlParameter("@ObjectAlias", SqlDbType.NVarChar)).Value = Trim(ds.Tables(0).Rows(0)("szLogin_id"))
                        .Add(New SqlParameter("@ObjectTypeID", SqlDbType.Int)).Value = 24
                        .Add(New SqlParameter("@ObjectValidFrom", SqlDbType.DateTime)).Value = Now
                        .Add(New SqlParameter("@ObjectValidTo", SqlDbType.DateTime)).Value = DBNull.Value
                        .Add(New SqlParameter("@ObjectDescription", SqlDbType.NVarChar)).Value = "User"
                        .Add(New SqlParameter("@Loginname", SqlDbType.NVarChar)).Value = Trim(ds.Tables(0).Rows(0)("szLogin_id"))
                        .Add(New SqlParameter("@Password", SqlDbType.NVarChar)).Value = Trim(ds.Tables(0).Rows(0)("szPassword_tx"))
                        .Add(New SqlParameter("@PasswordEncryption", SqlDbType.NVarChar)).Value = "MD5"
                        .Add(New SqlParameter("@PasswordExpires", SqlDbType.DateTime)).Value = DBNull.Value
                        .Add(New SqlParameter("@EmailAddress", SqlDbType.NVarChar)).Value = ConfigurationManager.AppSettings("SystemEmail")
                        .Add(New SqlParameter("@ObjectCustomField1", SqlDbType.NVarChar)).Value = DBNull.Value
                        .Add(New SqlParameter("@ObjectCustomField2", SqlDbType.UniqueIdentifier)).Value = DBNull.Value
                    End With

                    cmd.ExecuteScalar()

                    cn.Close()

                    'Get user details and update
                    user = arcn.GetUserByLogin(ds.Tables(0).Rows(0)("szLogin_id"))
                    'Set encryption type
                    user.PasswordEncryptionType = AREncryptionTypesEnum.MD5
                    'Set default password
                    user.Password = ds.Tables(0).Rows(0)("szPassword_tx")
                    'Set first time logon
                    user.SetPropertyValue("FirstTimeLogOn", "Yes")
                    'Set branch for records retrieval
                    user.CustomField4 = ""

                    'Get group and ensure that the user is not a member of that group
                    Dim group As ARGroup
                    group = arcn.GetGroupByAlias("system_all_users")

                    If Not group Is Nothing Then
                        If Not user.IsMember(group.ObjectID) Then
                            'Add user to group
                            user.AddToGroup(group.ObjectID)
                        End If
                    End If

                    'Update user details
                    user.Update()

                    'Log into auditing log
                    arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("ARUIUserNew.Log.InsertNew", Thread.CurrentThread.CurrentUICulture) & " " & user.Login, "secureaccess_" & ARLogConstants.cLOG_USER_CREATED)

                    'Send email notification 
                    If ConfigurationManager.AppSettings("AllowEmailNotification") Then

                        Dim email_message As String
                        email_message = "Your User Account Information"
                        email_message = "Your user name and password are:"
                        email_message &= "<br><br><b>User name:</b>" & " " & user.Login
                        email_message &= "<br><b>Password:</b>" & " " & user.Password
                        email_message &= "<br><br>Please log on "
                        email_message &= "<a href='http://" & HttpContext.Current.Server.MachineName & HttpContext.Current.Request.ApplicationPath & "'>" & ConfigurationManager.AppSettings("ApplicationDescription") & "</a>."

                        Try
                            'SendMail(user.EmailAddress, ConfigurationManager.AppSettings("SystemEmail"), ConfigurationManager.AppSettings("ApplicationDescription") & " User Account Information", email_message, True)
                        Catch ex As Exception
                            If ex.Message = "Could not access 'CDO.Message' object." Then
                                'Write into auditing log
                                arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "frits")
                                'Display message
                                msg.Append("Unable to send notification email. Please configure SMTP settings.")
                            Else
                                'Display message
                                msg.Append(ex.Message)
                            End If
                        End Try

                    End If

                    bsuccess = True

                End If

            End If
        Next

        If bsuccess = True Then
            msg.Append("Employee accounts successfully imported.")
        End If

        'Display message
        If msg.Length > 0 Then
            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & msg.ToString & "');", True)
        End If

        arcn.Close()

        Me.LoadUsers()

    End Sub

    Private Sub AddActiveDirectoryUser(ByVal Params As Object)

        Dim bsuccess As Boolean = False
        Dim msg As New System.Text.StringBuilder

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim user As New ARUser
        user.ARConnection = arcn

        Dim userArr() As String
        Dim i As Integer
        userArr = Split(Params, ";")

        For i = userArr.GetLowerBound(0) To userArr.GetUpperBound(0)

            If userArr(i).ToString <> "" Then

                Dim FullName As String = ""
                Dim UserName As String = ""
                Dim EmailAddress As String = ""
                Dim Password As String = ARPasswordUtils.GeneratePassword(5)

                ' Path to you LDAP directory server.
                ' Contact your network administrator to obtain a valid path.
                Dim adPath As String = "LDAP://" & ConfigurationManager.AppSettings("LDAPDomainName")
                Dim adConnUsername As String = ConfigurationManager.AppSettings("LDAPConnectionUserName")
                Dim adConnPassword As String = ConfigurationManager.AppSettings("LDAPConnectionPassword")

                Dim entry As DirectoryEntry = New DirectoryEntry(adPath, adConnUsername, adConnPassword)
                Dim search As DirectorySearcher = New DirectorySearcher(entry)

                ' Bind to the native AdsObject to force authentication.
                Dim obj As Object = entry.NativeObject

                search.Filter = "(objectCategory=person)"
                search.PropertiesToLoad.Add("cn")
                search.PropertiesToLoad.Add("givenName")
                search.PropertiesToLoad.Add("sn")
                search.PropertiesToLoad.Add("displayName")
                search.PropertiesToLoad.Add("mail")
                search.PropertiesToLoad.Add("title")
                search.PropertiesToLoad.Add("physicalDeliveryOffice")
                search.PropertiesToLoad.Add("pager")
                search.PropertiesToLoad.Add("homephone")
                search.PropertiesToLoad.Add("facsimileTelephoneNumber")
                search.PropertiesToLoad.Add("wwwHomepage")
                search.PropertiesToLoad.Add("streetAddress")
                search.PropertiesToLoad.Add("l")
                search.PropertiesToLoad.Add("PostalCode")
                search.PropertiesToLoad.Add("department")
                search.PropertiesToLoad.Add("company")
                search.PropertiesToLoad.Add("info")
                search.PropertiesToLoad.Add("SAMAccountName")

                Dim result As SearchResult

                For Each result In search.FindAll()

                    If result.Path = CType(userArr(i), String) Then

                        If Not result.Properties("SAMAccountName") Is Nothing Then
                            UserName = LCase(result.Properties("SAMAccountName")(0))
                        Else
                            Exit For
                        End If
                        If Not result.Properties("givenName") Is Nothing Then
                            FullName = result.Properties("givenName")(0) & " " & result.Properties("sn")(0)
                        End If
                        If Not result.Properties("mail") Is Nothing Then
                            EmailAddress = result.Properties("mail")(0)
                        End If

                        Exit For

                    End If

                Next

                'Create new user
                Dim cn As SqlConnection
                Dim cmd As New SqlCommand

                cn = New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
                cn.Open()

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "Proc_AR_User_InsertShort"
                cmd.Connection = cn

                With cmd.Parameters
                    .Add(New SqlParameter("@ObjectName", SqlDbType.NVarChar)).Value = FullName
                    .Add(New SqlParameter("@ObjectAlias", SqlDbType.NVarChar)).Value = UserName
                    .Add(New SqlParameter("@ObjectTypeID", SqlDbType.Int)).Value = 24
                    .Add(New SqlParameter("@ObjectValidFrom", SqlDbType.DateTime)).Value = Now
                    .Add(New SqlParameter("@ObjectValidTo", SqlDbType.DateTime)).Value = DBNull.Value
                    .Add(New SqlParameter("@ObjectDescription", SqlDbType.NVarChar)).Value = "User"
                    .Add(New SqlParameter("@Loginname", SqlDbType.NVarChar)).Value = UserName
                    .Add(New SqlParameter("@Password", SqlDbType.NVarChar)).Value = Password
                    .Add(New SqlParameter("@PasswordEncryption", SqlDbType.NVarChar)).Value = "MD5"
                    .Add(New SqlParameter("@PasswordExpires", SqlDbType.DateTime)).Value = DBNull.Value
                    .Add(New SqlParameter("@EmailAddress", SqlDbType.NVarChar)).Value = EmailAddress
                    .Add(New SqlParameter("@ObjectCustomField1", SqlDbType.NVarChar)).Value = DBNull.Value
                    .Add(New SqlParameter("@ObjectCustomField2", SqlDbType.UniqueIdentifier)).Value = DBNull.Value
                End With

                cmd.ExecuteScalar()

                cn.Close()

                'Get user details and update
                user = arcn.GetUserByLogin(UserName)
                'Set default password
                user.Password = Password
                'Set first time logon
                user.SetPropertyValue("FirstTimeLogOn", "Yes")
                'Set ID for records retrieval
                user.SetPropertyValue("SSID", "")

                'Get group and ensure that the user is not a member of that group
                Dim group As ARGroup
                group = arcn.GetGroupByAlias("system_all_users")

                If Not group Is Nothing Then
                    If Not user.IsMember(group.ObjectID) Then
                        'Add user to group
                        user.AddToGroup(group.ObjectID)
                    End If
                End If

                'Update user details
                user.Update()

                'Log into auditing log
                arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("ARUIUserNew.Log.InsertNew", Thread.CurrentThread.CurrentUICulture) & " " & user.Login, "secureaccess_" & ARLogConstants.cLOG_USER_CREATED)

                'Send email notification 
                If ConfigurationManager.AppSettings("AllowEmailNotification") Then

                    Dim email_message As String
                    email_message = "Your User Account Information"
                    email_message = "Your user name and password are:"
                    email_message &= "<br><br><b>User name:</b>" & " " & user.Login
                    email_message &= "<br><b>Password:</b>" & " " & user.Password
                    email_message &= "<br><br>Please log on "
                    email_message &= "<a href='http://" & HttpContext.Current.Server.MachineName & HttpContext.Current.Request.ApplicationPath & "'>" & ConfigurationManager.AppSettings("ApplicationDescription") & "</a>."

                    Try
                        'SendMail(user.EmailAddress, ConfigurationManager.AppSettings("SystemEmail"), ConfigurationManager.AppSettings("ApplicationDescription") & " User Account Information", email_message, True)
                    Catch ex As Exception
                        If ex.Message = "Could not access 'CDO.Message' object." Then
                            'Write into auditing log
                            arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), "Unable to send notification email.", "frits")
                            'Display message
                            msg.Append("Unable to send notification email. Please configure SMTP settings.")
                        Else
                            'Display message
                            msg.Append(ex.Message)
                        End If
                    End Try

                End If

                bsuccess = True

            End If

        Next

        If bsuccess = True Then
            msg.Append("Active directory accounts successfully imported.")
        End If

        'Display message
        If msg.Length > 0 Then
            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & msg.ToString & "');", True)
        End If

        arcn.Close()

        Me.LoadUsers()

    End Sub

#End Region

#Region " Delete"

    Private Sub DeleteOperator(Optional ByVal resourceid As Object = 0, Optional ByVal operatorid As Object = 0)
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim arObj As ARObject
        Dim arApp As ARApplicationPart = arcn.GetApplicationPartByID(resourceid)
        For Each arObj In arApp.GetChildrenAll
            If arObj.ObjectID = operatorid Then
                arObj.RemoveAllParentRelationships(arApp.ObjectID)
                'Log into auditing log
                arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("ARUIDelegatedUsers.Log.OperatorRemoved", Thread.CurrentThread.CurrentUICulture) & " " & arObj.ObjectName, "secureaccess_" & ARLogConstants.cLOG_DELEGATED_OPERATOR_REMOVED)
                Exit Sub
            End If
        Next
        arcn.Close()
        Me.LoadApplicationParts()
    End Sub

    Private Sub DeleteUser(ByVal id As Object)
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim user As ARUser = arcn.GetUserByID(id)
        If Not user Is Nothing Then
            Dim objectneme As String = user.ObjectName
            'Dim obj As New BLL.User
            'If obj.HasRecord(id) = 0 Then
            user.Delete()
            'Log into auditing log
            arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("UserList.Log.Delete", Thread.CurrentThread.CurrentUICulture) & " " & objectneme, "secureaccess_" & ARLogConstants.cLOG_USER_DELETED)
            'End If
        End If
        arcn.Close()
        Me.LoadUsers()
    End Sub

    Private Sub DeleteGroup(ByVal id As Object)
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim group As ARGroup = arcn.GetGroupByID(id)
        If Not group Is Nothing Then
            Dim objectneme As String = group.ObjectName
            group.Delete()
            'Log into auditing log
            arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("UserGroupList.Log.Delete", Thread.CurrentThread.CurrentUICulture) & " " & objectneme, "secureaccess_" & ARLogConstants.cLOG_USERGROUP_DELETED)
        End If
        arcn.Close()
        Me.LoadGroups()
    End Sub

    Private Sub DeleteRole(ByVal id As Object)
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim role As ARRole = arcn.GetRoleByID(id)
        If Not role Is Nothing Then
            Dim objectneme As String = role.ObjectName
            role.Delete()
            'Log into auditing log
            arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("RoleList.Log.Delete", Thread.CurrentThread.CurrentUICulture) & " " & objectneme, "secureaccess_" & ARLogConstants.cLOG_ROLE_DELETED)
        End If
        arcn.Close()
        Me.LoadGroups()
    End Sub

    Private Sub DeleteOrganizationalUnits(ByVal id As Object)
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim orgunit As AROrgUnit = arcn.GetOrgUnitByID(id)
        If Not orgunit Is Nothing Then
            Dim objectneme As String = orgunit.ObjectName
            orgunit.Delete()
            'Log into auditing log
            arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), Me.ResManager.GetString("UserGroupList.Log.Delete", Thread.CurrentThread.CurrentUICulture) & " " & objectneme, "secureaccess_" & ARLogConstants.cLOG_ORGUNIT_DELETED)
        End If
        arcn.Close()
        Me.LoadGroups()
    End Sub

    Private Sub DeleteLog()
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim ardblog As New ARDBLog()
        ardblog.Connection = arcn.ARDBProvider.Connection
        ardblog.DeleteAll()
        arcn.Close()
    End Sub

    Private Sub ClearSession(ByVal id As Object)
        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()
        Dim user As ARUser = arcn.GetUserByID(id)
        If Not user Is Nothing Then
            user.SetPropertyValue("SessionId", "")
            'Log into auditing log
            arcn.Log(Me.CurrentUser.ObjectID, Me.GetResourceID(Me.ObjectAlias), user.ObjectName & " session cleared successfully.", "secureaccess_Information")
        End If
        arcn.Close()
        Me.LoadUsers()
    End Sub

#End Region

#Region " Load"

    '## This method load application parts
    Private Sub LoadApplicationParts()

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim Params(0, 1) As String
        Params(0, 0) = "ApplicationID"
        Params(0, 1) = arcn.GetApplicationByAlias("frits").ObjectID

        arcn.ARDBProvider.Table = "View_AR_AllApplicationParts"
        arcn.ARDBProvider.Parameters = Params

        Dim ds As DataSet
        ds = arcn.ARDBProvider.SelectRecords

        Me.lstApplicationParts.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode
        rootnode.ImageUrl = "~/Images/Icons/20x20/ApplicationPart.gif"
        rootnode.Text = "Application Parts"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstApplicationParts.Nodes.Add(rootnode)

        Dim ht As New Hashtable

        ht.Add("Login", "Login")
        ht.Add("Logout", "Logout")
        ht.Add("Welcome", "Welcome")
        ht.Add("My Profile", "My Profile")
        ht.Add("Change Password", "Change Password")
        ht.Add("Forgotten Password", "Forgotten Password")
        ht.Add("Access Denied", "Access Denied")
        ht.Add("Applications", "Applications")
        ht.Add("Auditing Log", "Auditing Log")
        ht.Add("Catalog Settings", "Catalog Settings")
        ht.Add("Custom Properties Definition", "Custom Properties Definition")
        'ht.Add("Users", "Users")
        'ht.Add("Groups", "Groups")
        'ht.Add("Roles", "Roles")
        'ht.Add("Organizational Units", "Organizational Units")

        If ds.Tables(0).Rows.Count > 0 Then

            Dim dr, drChild As DataRow

            For Each dr In ds.Tables(0).Rows

                If Not ht.ContainsValue(dr("ObjectName")) Then

                    Dim node As New ComponentArt.Web.UI.TreeViewNode

                    node.ImageUrl = "~/Images/Icons/16x16/ApplicationPart.gif"
                    node.Text = dr("ObjectName")
                    node.Value = dr("ObjectID")

                    Params(0, 0) = "ParentObjectID"
                    Params(0, 1) = dr("ObjectID")

                    arcn.ARDBProvider.Table = "View_AR_AllApplicationParts"
                    arcn.ARDBProvider.Parameters = Params

                    Dim dsChild As DataSet
                    dsChild = arcn.ARDBProvider.SelectRecords

                    If dsChild.Tables(0).Rows.Count > 0 Then

                        For Each drChild In dsChild.Tables(0).Rows

                            If Not ht.ContainsValue(drChild("ObjectName")) Then

                                Dim nodeChild As New ComponentArt.Web.UI.TreeViewNode

                                nodeChild.ImageUrl = "~/Images/Icons/16x16/ApplicationPart.gif"
                                nodeChild.Text = drChild("ObjectName")
                                nodeChild.Value = drChild("ObjectID")

                                Params(0, 0) = "ParentObjectID"
                                Params(0, 1) = drChild("ObjectID")

                                arcn.ARDBProvider.Table = "View_AR_AllApplicationParts"
                                arcn.ARDBProvider.Parameters = Params

                                Dim dsChildChild As DataSet
                                dsChildChild = arcn.ARDBProvider.SelectRecords

                                If dsChildChild.Tables(0).Rows.Count > 0 Then

                                    For Each drChildChild In dsChildChild.Tables(0).Rows

                                        If Not ht.ContainsValue(drChildChild("ObjectName")) Then

                                            Dim nodeChildChild As New ComponentArt.Web.UI.TreeViewNode

                                            nodeChildChild.ImageUrl = "~/Images/Icons/16x16/ApplicationPart.gif"
                                            nodeChildChild.Text = drChildChild("ObjectName")
                                            nodeChildChild.Value = drChildChild("ObjectID")

                                            nodeChild.Nodes.Add(nodeChildChild)

                                        End If

                                    Next

                                End If

                                node.Nodes.Add(nodeChild)

                            End If

                        Next

                    End If

                    rootnode.Nodes.Add(node)

                End If

            Next

        End If

        Me.lstApplicationParts.Nodes.Sort()

        arcn.Close()

    End Sub

    '## This method load users.
    Private Sub LoadUsers()

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Me.lstUsers.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode

        rootnode.ImageUrl = "~/Images/Icons/20x20/User.gif"
        rootnode.Text = "Users"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstUsers.Nodes.Add(rootnode)

        'arcn.ARDBProvider.Table = "View_AR_User_List"

        Dim ds As DataSet
        ds = arcn.ARDBProvider.ProcessQuery("SELECT * from View_AR_User_List ORDER BY ObjectName ASC")

        If ds.Tables(0).Rows.Count > 0 Then

            Dim memberof As String = ""

            'Me.ltlUserScript.Text = ""
            'Me.ltlUserScript.Text &= "<script language=""javascript"">" & vbCrLf
            'Me.ltlUserScript.Text &= "      var user_has_record = new Array();" & vbCrLf

            Dim i As Integer = 0
            Dim dr As DataRow

            For Each dr In ds.Tables(0).Rows

                If Not dr("ObjectID") = "337" Then

                    Dim node As New ComponentArt.Web.UI.TreeViewNode

                    node.ImageUrl = "~/Images/Icons/16x16/User.gif"
                    node.Text = dr("ObjectName")
                    node.Value = dr("ObjectID")

                    rootnode.Nodes.Add(node)

                    'Dim obj As New BLL.User

                    'If obj.HasRecord(dr("ObjectID")) = 0 Then
                    '    Me.ltlUserScript.Text &= "      user_has_record[" & i & "]= """ & dr("ObjectID") & """" & vbCrLf
                    '    i += 1
                    'End If

                End If

            Next

            'Me.ltlUserScript.Text &= "</script>"

        End If

        Me.lstUsers.Nodes.Sort("ObjectName", False)

        arcn.Close()

    End Sub

    '## This method load groups.
    Private Sub LoadGroups()

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Me.lstGroups.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode

        rootnode.ImageUrl = "~/Images/Icons/20x20/UserGroup.gif"
        rootnode.Text = "Groups"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstGroups.Nodes.Add(rootnode)

        'arcn.ARDBProvider.Table = "View_AR_UserGroup_List"

        Dim ds As DataSet
        ds = arcn.ARDBProvider.ProcessQuery("SELECT * from View_AR_UserGroup_List ORDER BY ObjectName ASC")

        If ds.Tables(0).Rows.Count > 0 Then

            Dim dr As DataRow

            For Each dr In ds.Tables(0).Rows

                Dim node As New ComponentArt.Web.UI.TreeViewNode

                node.ImageUrl = "~/Images/Icons/16x16/UserGroup.gif"
                node.Text = dr("ObjectName")
                node.Value = dr("ObjectID")

                rootnode.Nodes.Add(node)

            Next

        End If

        Me.lstGroups.Nodes.Sort()

        arcn.Close()

    End Sub

    '## This method load groups.
    Private Sub LoadRoles()

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Me.lstRoles.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode

        rootnode.ImageUrl = "~/Images/Icons/20x20/ApplicationRole.gif"
        rootnode.Text = "Roles"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstRoles.Nodes.Add(rootnode)

        Dim Params(0, 1) As String
        Params(0, 0) = "ObjectAlias"
        Params(0, 1) = "<##XLIKE##> N'frits.'"

        arcn.ARDBProvider.Table = "View_AR_Role_List"
        arcn.ARDBProvider.Parameters = Params

        Dim ds As DataSet
        ds = arcn.ARDBProvider.SelectRecords

        If ds.Tables(0).Rows.Count > 0 Then

            Dim dr As DataRow

            For Each dr In ds.Tables(0).Rows

                Dim node As New ComponentArt.Web.UI.TreeViewNode

                node.ImageUrl = "~/Images/Icons/16x16/ApplicationRole.gif"
                node.Text = dr("ObjectName")
                node.Value = dr("ObjectID")

                rootnode.Nodes.Add(node)

            Next

        End If

        Me.lstRoles.Nodes.Sort()

        arcn.Close()

    End Sub

    '## This method load organizational units.
    Private Sub LoadOrganizationalUnits()

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Me.lstOrganizationalUnits.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode

        rootnode.ImageUrl = "~/Images/Icons/20x20/OU.gif"
        rootnode.Text = "Organizational Units"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstOrganizationalUnits.Nodes.Add(rootnode)

        Dim ardbobj As New ARDBObject()
        Dim ds As DataSet
        Dim dr As DataRow
        Dim ParentOUID As Integer
        ParentOUID = CInt(arcn.GetOrgUnitByAlias(ARConstants.cORG_UNIT_ROOT_ALIAS).ObjectID)

        ds = ardbobj.SelectChildOrgUnits(ParentOUID, 100)

        If ds.Tables.Count > 0 Then

            If ds.Tables(0).Rows.Count > 0 Then

                For Each dr In ds.Tables(0).Rows

                    Dim node As New ComponentArt.Web.UI.TreeViewNode

                    node.ImageUrl = "~/Images/Icons/16x16/OU.gif"
                    node.Text = dr("ObjectName")
                    node.Value = dr("ObjectID")

                    CreateOrganizationalUnitsNode(node, dr("ObjectID"))

                    rootnode.Nodes.Add(node)

                Next

            End If

        End If

        Me.lstOrganizationalUnits.Nodes.Sort()

        arcn.Close()

    End Sub

    '## This method create organizational unit node.
    Private Sub CreateOrganizationalUnitsNode(ByVal node As ComponentArt.Web.UI.TreeViewNode, ByVal ObjectID As Integer)

        Dim ardbobj As New ARDBObject()
        Dim ds As DataSet
        Dim dr As DataRow

        ds = ardbobj.SelectChildOrgUnits(ObjectID, 100)

        If ds.Tables.Count > 0 Then

            For Each dr In ds.Tables(0).Rows

                Dim newnode As New ComponentArt.Web.UI.TreeViewNode

                newnode.ImageUrl = "~/Images/Icons/16x16/OU.gif"
                newnode.Text = dr("ObjectName")
                newnode.Value = dr("ObjectID")

                node.Nodes.Add(newnode)

                CreateOrganizationalUnitsNode(newnode, dr("ObjectID"))

            Next

        End If

    End Sub

    '## This method load authorised operators.
    Private Sub LoadAuthorisedOperators(ByVal ResourceID As Integer)

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim dr As DataRow = Nothing
        Dim ds As DataSet
        Dim ardbobj As New ARDBObject
        Dim arobj As ARObject

        ds = ardbobj.SelectPermissionMatrix(ResourceID)

        Dim lastValue As String = ""
        Dim rowIndex As Integer = 0
        Dim period As Integer = 0

        If ds.Tables(0).Rows.Count = 0 Then
            'Display message
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("ARUIPermissionMatrix.NoOperators", Thread.CurrentThread.CurrentUICulture) & "');", True)
            Exit Sub
        Else
            lastValue = ds.Tables(0).Rows(0)("ChildObjectID")
            'find the row-repeating period
            While lastValue = ds.Tables(0).Rows(rowIndex)("ChildObjectID")
                rowIndex += 1
                If rowIndex = ds.Tables(0).Rows.Count Then
                    Exit While
                End If
            End While

            period = rowIndex

            Me.lstAuthorisedOperators.Nodes.Sort()
            Me.lstAuthorisedOperators.Nodes.Clear()

            Dim rootnode As New ComponentArt.Web.UI.TreeViewNode
            rootnode.ImageUrl = "~/Images/Icons/20x20/UserGroup.gif"
            rootnode.Text = "Operators"
            rootnode.Value = "root"
            rootnode.Expanded = True

            Me.lstAuthorisedOperators.Nodes.Add(rootnode)

            For rowIndex = 0 To ds.Tables(0).Rows.Count - 1 Step period
                Dim n As New ComponentArt.Web.UI.TreeViewNode
                n.Text = ds.Tables(0).Rows(rowIndex)("ChildObjectName")
                n.Value = ds.Tables(0).Rows(rowIndex)("ChildObjectID")
                arobj = arcn.GetObjectByID(n.Value)
                If arobj.ObjectTypeRevised = ARObjectTypesEnum.ARUser Then
                    n.ImageUrl = "~/Images/Icons/16x16/User.gif"
                ElseIf arobj.ObjectTypeRevised = ARObjectTypesEnum.ARGroup Then
                    n.ImageUrl = "~/Images/Icons/16x16/UserGroup.gif"
                ElseIf arobj.ObjectTypeRevised = ARObjectTypesEnum.ARRole Then
                    n.ImageUrl = "~/Images/Icons/16x16/ApplicationRole.gif"
                ElseIf arobj.ObjectTypeRevised = ARObjectTypesEnum.AROrgUnit Then
                    n.ImageUrl = "~/Images/Icons/16x16/OU.gif"
                End If
                rootnode.Nodes.Add(n)
            Next

        End If

        Me.lstAuthorisedOperators.Nodes.Sort()

        arcn.Close()

    End Sub

    '## This method load permissions.
    Private Sub LoadPermissions(ByVal ResourceID As Integer)

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim ResourceAlias As String = Nothing
        Dim resource As ARResource = arcn.GetResourceByID(ResourceID)

        If Not resource Is Nothing Then
            ResourceAlias = resource.ObjectAlias
        End If

        Dim Params(0, 1) As String
        Params(0, 0) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE
        Params(0, 1) = ResourceAlias

        arcn.ARDBProvider.Parameters = Params
        arcn.ARDBProvider.Table = "View_AR_RelationshipType"

        Dim ds As DataSet = arcn.ARDBProvider.SelectRecords

        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                CType(Me.FindControl("div" & dr(1)), HtmlGenericControl).Visible = True
            Next
        End If

        arcn.Close()

    End Sub

    '## This method load audit logs.
    Private Sub LoadAuditLog()

        Try

            Dim arcn As New ARConnection
            arcn.ConnectToCatalog()

            arcn.ARDBProvider.Table = "View_AR_Log_Joined"

            Dim ds As DataSet
            ds = arcn.ARDBProvider.SelectRecords

            If ds.Tables(0).Rows.Count > 0 Then
                Dim dr As DataRow
                For Each dr In ds.Tables(0).Rows
                    If Not dr("EventCode") = "frits" Then
                        dr.Delete()
                    End If
                Next
            End If

            arcn.Close()

            Me.Grid.DataSourceID = "LogSqlDataSource"
            Me.Grid.DataBind()


        Catch ex As Exception

        End Try


    End Sub

#End Region

#End Region

#Region " Events "

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles ScriptManager.AsyncPostBackError
        Me.ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message
    End Sub

    Private Sub cmdCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles cmdCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "new:operator" : Me.AddOperator(e.Parameters(1), e.Parameters(2))
            Case "del:operator" : Me.DeleteOperator(e.Parameters(1), e.Parameters(2))
        End Select
    End Sub

    Private Sub lstApplicationPartsCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstApplicationPartsCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "load:operator" : Me.LoadApplicationParts()
        End Select
        Me.lstApplicationParts.RenderControl(e.Output)
    End Sub

    Private Sub lstAuthorisedOperatorsCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstAuthorisedOperatorsCallBack.Callback
        Me.HidePermissions()
        Me.LoadAuthorisedOperators(e.Parameter)
        Me.lstAuthorisedOperators.RenderControl(e.Output)
    End Sub

    Private Sub lstUsersCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstUsersCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "new:ctuser" : Me.AddCustomTableUser(e.Parameters(1))
            Case "new:aduser" : Me.AddActiveDirectoryUser(e.Parameters(1))
            Case "del:user" : Me.DeleteUser(e.Parameters(1))
            Case "load:user" : Me.LoadUsers()
            Case "clr:user" : Me.ClearSession(e.Parameters(1))
            Case "import:custom" : Me.ImportCustomTable()
            Case "import:file" : Me.ImportFile()
        End Select
        Me.tbl.RenderControl(e.Output)
        Me.lstUsers.RenderControl(e.Output)
    End Sub

    Private Sub lstGroupsCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstGroupsCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "del:group" : Me.DeleteGroup(e.Parameters(1))
            Case "load:group" : Me.LoadGroups()
        End Select
        Me.lstGroups.RenderControl(e.Output)
    End Sub

    Private Sub lstRolesCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstRolesCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "del:role" : Me.DeleteRole(e.Parameters(1))
            Case "load:role" : Me.LoadRoles()
        End Select
        Me.lstRoles.RenderControl(e.Output)
    End Sub

    Private Sub lstOrganizationalUnitsCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles lstOrganizationalUnitsCallBack.Callback
        Dim command As String = e.Parameters(0)
        Select Case command
            Case "del:ou" : Me.DeleteOrganizationalUnits(e.Parameters(1))
            Case "load:ou" : Me.LoadOrganizationalUnits()
        End Select
        Me.lstOrganizationalUnits.RenderControl(e.Output)
    End Sub

    Private Sub PermissionsCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles PermissionsCallBack.Callback

        Me.ClearPermissions()
        Me.HidePermissions()

        If e.Parameters(2) = "Administrators" Then
            Me.divPermissions.Style.Item("Display") = "none"
        Else
            Me.divPermissions.Style.Item("Display") = "block"

            Dim arcn As New ARConnection
            arcn.ConnectToCatalog()

            Dim resource As ARResource = arcn.GetResourceByID(CInt(e.Parameters(0)))

            Dim Params(0, 1) As String
            Params(0, 0) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE
            Params(0, 1) = resource.ObjectAlias

            arcn.ARDBProvider.Parameters = Params
            arcn.ARDBProvider.Table = "View_AR_RelationshipType"

            Dim ds As DataSet = arcn.ARDBProvider.SelectRecords

            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    If e.Parameters.Length > 1 Then
                        CType(Me.FindControl("div" & dr(1)), HtmlGenericControl).Visible = True
                        Dim arOperator As AROperator = arcn.GetOperatorByID(CInt(e.Parameters(1)))
                        If Not arOperator Is Nothing Then
                            CType(Me.FindControl("chk" & dr(1)), CheckBox).Checked = arOperator.IsAuthorized(resource.ObjectAlias, dr(1))
                        End If
                    End If
                Next
            End If

            arcn.Close()

        End If

        Me.PermissionsPlaceHolder.RenderControl(e.Output)

    End Sub

    Private Sub PermissionCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles PermissionCallBack.Callback

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim ResourceID As Integer = CInt(e.Parameters(0))
        Dim OperatorID As Integer = CInt(e.Parameters(1))
        Dim PermissionType As String = e.Parameters(2)
        Dim DenyAccess As Boolean = CBool(IIf(e.Parameters(3) = "true", False, True))

        Dim resource As ARResource = arcn.GetResourceByID(ResourceID)
        Dim arOperator As AROperator = arcn.GetOperatorByID(OperatorID)

        arOperator.AddParent(resource.ObjectID, GetRelationshipTypeID(arcn.ARDBProvider, resource.ObjectAlias, PermissionType), DenyAccess, Now)

        arcn.Close()

    End Sub

    Private Sub txtSearchUser_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchUser.TextChanged

        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Me.lstUsers.Nodes.Clear()

        Dim rootnode As New ComponentArt.Web.UI.TreeViewNode

        rootnode.ImageUrl = "~/Images/Icons/20x20/User.gif"
        rootnode.Text = "Users"
        rootnode.Value = "root"
        rootnode.Expanded = True

        Me.lstUsers.Nodes.Add(rootnode)

        Dim Params(2) As SqlParameter

        Params(0) = arcn.ARDBProvider.SetSQLParameter("@Substring", txtSearchUser.Text, SqlDbType.NVarChar, 255)
        Params(1) = arcn.ARDBProvider.SetSQLParameter("@ObjectTypeNamespace", "ARObject.AROperator.ARUser", SqlDbType.NVarChar, 255)
        Params(2) = arcn.ARDBProvider.SetSQLParameter("@TopN", 15, SqlDbType.Int, 4)

        Dim ds As DataSet
        ds = arcn.ARDBProvider.ProcessStoredProcedure("Proc_AR_ObjectUser_Search", Params)

        If ds.Tables(0).Rows.Count > 0 Then

            Dim dr As DataRow

            For Each dr In ds.Tables(0).Rows

                If Not dr("ObjectID") = "337" Then

                    Dim node As New ComponentArt.Web.UI.TreeViewNode

                    node.ImageUrl = "~/Images/Icons/16x16/User.gif"
                    node.Text = dr("ObjectName")
                    node.Value = dr("ObjectID")

                    rootnode.Nodes.Add(node)

                End If

            Next

        End If

        Me.lstUsers.Nodes.Sort("ObjectName", False)

        arcn.Close()

    End Sub

    Private Sub gridCallBack_Callback(ByVal sender As Object, ByVal e As ComponentArt.Web.UI.CallBackEventArgs) Handles gridCallBack.Callback
        If e.Parameter = "purge" Then
            Me.DeleteLog()
        End If
        Me.LoadAuditLog()
        Me.Grid.RenderControl(e.Output)
    End Sub

    Private Sub NeedRebind(ByVal sender As Object, ByVal oArgs As EventArgs) Handles Grid.NeedRebind
        Me.Grid.DataBind()
    End Sub

    Private Sub NeedDataSource(ByVal sender As Object, ByVal oArgs As EventArgs) Handles Grid.NeedDataSource
        Me.LoadAuditLog()
    End Sub

    Private Sub PageChanged(ByVal sender As Object, ByVal oArgs As ComponentArt.Web.UI.GridPageIndexChangedEventArgs) Handles Grid.PageIndexChanged
        Me.Grid.CurrentPageIndex = oArgs.NewIndex
    End Sub

    Private Sub Sort(ByVal sender As Object, ByVal oArgs As ComponentArt.Web.UI.GridSortCommandEventArgs) Handles Grid.SortCommand
        Me.Grid.Sort = oArgs.SortExpression
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager"

            'Check authentication.
            If Not Me.CurrentUser Is Nothing Then

                'Check access open page.
                If Me.CurrentUser.IsMemberAll("Administrators") OrElse Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "read") Then

                    If Not Page.IsPostBack _
                         And Not Me.lstApplicationPartsCallBack.IsCallback _
                         And Not Me.lstAuthorisedOperatorsCallBack.IsCallback _
                         And Not Me.lstUsersCallBack.IsCallback _
                         And Not Me.lstGroupsCallBack.IsCallback _
                         And Not Me.lstRolesCallBack.IsCallback _
                         And Not Me.lstOrganizationalUnitsCallBack.IsCallback _
                         And Not Me.gridCallBack.IsCallback _
                         And Not Me.PermissionsCallBack.IsCallback _
                         And Not Me.PermissionCallBack.IsCallback Then

                        'Hide permission.
                        Me.HidePermissions()

                        'Load tree data.
                        Me.LoadApplicationParts()
                        Me.LoadUsers()
                        Me.LoadGroups()
                        Me.LoadRoles()
                        Me.LoadOrganizationalUnits()
                        Me.LoadAuditLog()

                        'Bind all databound contrfams on this page.
                        Me.DataBind()

                    End If

                Else
                    'Hide page content
                    Me.pnlContentPane.Visible = False

                    'Display message and close window
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You do not have the permission to access this page.');parent.returnValue='reload';parent.close();", True)
                End If

            Else
                'Hide page content
                Me.pnlContentPane.Visible = False

                'Display message and close window
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You do not have the permission to access this page.');parent.returnValue='reload';parent.close();", True)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

End Class