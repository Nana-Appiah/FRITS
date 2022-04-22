Imports System.Linq



Partial Public Class ConfigureApp
    Inherits PageBase


    Private Const CREATE As String = "Create"
    Private Const READ As String = "Read"
    Private Const CHANGE As String = "Change"
    Private Const DELETE As String = "Delete"
    Private Const GENERATE As String = "Generate"


    Private Const Permission_List As String = "List"
    Private Const Permission_List_Read As String = "List,Read"
    Private Const Permission_Create_Read_Change_Delete As String = "List,Create,Read,Change,Delete"
    Private Const Permission_Create_Read_Change As String = "List,Create,Read,Change"
    Private Const Permission_All As String = "List,Create,Read,Change,Delete,Generate"




    Private _dbInitialized As Boolean
    Public Property IsInitialize() As Boolean
        Get
            Return _dbInitialized
        End Get
        Set(ByVal value As Boolean)
            _dbInitialized = value
        End Set
    End Property


    Private _arConn As ARConnection
    Public Property arcn() As ARConnection
        Get
            If (Not IsInitialize()) Then
                _arConn = New ARConnection
                _arConn.ConnectToCatalog()
            End If

            Return _arConn
        End Get
        Set(ByVal value As ARConnection)
            _arConn = value
        End Set

    End Property


    Private _app As ARApplication
    Public Property app() As ARApplication
        Get
            If (Not IsInitialize()) Then
                _app = arcn.GetApplicationByAlias("frits")
            End If
            Return _app
        End Get
        Set(ByVal value As ARApplication)
            _app = value
        End Set
    End Property


    'Private _appPart As ARApplicationPart
    'Public Property appPart() As ARApplicationPart
    '    Get
    '        If (Not IsInitialize()) Then
    '            _appPart = New ARApplicationPart()
    '        End If
    '        Return _appPart
    '    End Get
    '    Set(ByVal value As ARApplicationPart)
    '        _appPart = value
    '    End Set
    'End Property


    Private _relType As ARDBRelationshipType
    Public Property relType() As ARDBRelationshipType
        Get
            If (Not IsInitialize()) Then
                _relType = New ARDBRelationshipType
                _relType.Connection = arcn.ARDBProvider.Connection
            End If
            Return _relType
        End Get
        Set(ByVal value As ARDBRelationshipType)
            _relType = value
        End Set
    End Property


    Private AdminGroupId As Integer
    Private ITGroupId As Integer
    Private AuditGroupId As Integer



    Private Sub InitializeDB()

        app = arcn.GetApplicationByAlias("frits")

        ' get ObjectID of the "Administrators" group
        AdminGroupId = arcn.GetGroupByAlias("Administrators").ObjectID
        ITGroupId = 12303
        AuditGroupId = 12518
        IsInitialize = True

    End Sub


    Private Function ApplicationPartList() As IList(Of Tuple(Of String, String, List(Of String)))

        Dim permList As List(Of Tuple(Of String, String, List(Of String))) = New List(Of Tuple(Of String, String, List(Of String)))

        permList.Add(CreateAppPart("Login", "login"))
        permList.Add(CreateAppPart("Logout", "logout"))
        permList.Add(CreateAppPart("Access Denied", "accessdenied"))
        permList.Add(CreateAppPart("Welcome", "welcome", Permission_List_Read))
        permList.Add(CreateAppPart("My Profile", "myprofile", Permission_Create_Read_Change_Delete))

        'permList.Add(CreateAppPart("Setup", "setup", Permission_Create_Read_Change_Delete))

        'permList.Add(CreateAppPart("Management", "management", Permission_List))
        'permList.Add(CreateAppPart("Reviews", "management.reviews.maintain", Permission_Create_Read_Change_Delete))
        'permList.Add(CreateAppPart("Record Review", "management.reviews.recordreview", Permission_Create_Read_Change_Delete))
        'permList.Add(CreateAppPart("Update Review - Branch", "management.reviews.branchupdate", Permission_Create_Read_Change_Delete))
        'permList.Add(CreateAppPart("View / Update Review", "management.reviews.viewandupdate", Permission_Create_Read_Change_Delete))

        'permList.Add(CreateAppPart("Authorise", "authorise", Permission_List))
        'permList.Add(CreateAppPart("Authorise Review", "authorise.reviews", Permission_Create_Read_Change_Delete))
        'permList.Add(CreateAppPart("Authorise Assignment", "authorise.assignment", Permission_Create_Read_Change_Delete))


        'permList.Add(CreateAppPart("Reports", "reports", Permission_List))

        permList.Add(CreateAppPart("Utilities", "utilities", Permission_List))
        permList.Add(CreateAppPart("Profile Manager", "utilities.profilemanager", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Users", "utilities.profilemanager.users", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Groups", "utilities.profilemanager.groups", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Org. Units", "utilities.profilemanager.orgunits", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Roles", "utilities.profilemanager.roles", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Change Password", "utilities.profilemanager.changepassword", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Forgotten Password", "utilities.profilemanager.forgottenpassword", Permission_Create_Read_Change_Delete))
        permList.Add(CreateAppPart("Settings", "utilities.settings", Permission_Create_Read_Change_Delete))


        Return permList

    End Function

    Private Function CreateAppPart(ByVal partName As String, ByVal partAlias As String, Optional ByVal commaSepPermission As String = Nothing) As Tuple(Of String, String, List(Of String))
        Dim perms As List(Of String)

        If (commaSepPermission Is Nothing) Then
            perms = New List(Of String)
        Else
            perms = commaSepPermission.Split(",").ToList()
        End If

        Return New Tuple(Of String, String, List(Of String))(partName, partAlias, perms)
    End Function




    Protected Sub CreatePartWithPermission(ByVal objectName As String, ByVal partAliase As String, ByVal permissions As IList(Of String))

        Dim objectAlias = arcn.GetApplicationByID(app.ObjectID).ObjectAlias & "." & partAliase ' ".login"

        Dim partNotExist As Boolean = arcn.GetApplicationPartByAlias(objectAlias) Is Nothing

        Dim obj As New ARApplicationPart()

        If partNotExist Then

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()
        Else
            obj = arcn.GetApplicationPartByAlias(objectAlias)
        End If


        If permissions.Count > 0 Then

            For Each perm As String In permissions

                Dim relPerms = relType.SelectRecords()

                Dim relPermsList = relPerms.Tables(0).AsEnumerable()

                Dim recExist = relPermsList.Where(Function(o) o.Field(Of String)("RelationshipTypeAlias").Equals(perm.ToLower()) And o.Field(Of String)("RelationshipTypeName").Equals(perm) And o.Field(Of String)("RelationshipTypeNamespace").Equals(objectAlias))

                If (recExist.Count > 0) Then


                    If (perm.ToLower() = "list") Then

                        obj.GrantAccess(AdminGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(ITGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(AuditGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                    Else

                        obj = arcn.GetApplicationPartByAlias(objectAlias)

                        obj.GrantAccess(AdminGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(ITGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(AuditGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))

                    End If

                    Continue For
                End If

                If (recExist.Count = 0) Then

                    relType.Insert(perm, perm.ToLower(), obj.ObjectAlias, perm, False)

                    If (perm.ToLower() = "list") Then

                        obj.GrantAccess(AdminGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(ITGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(AuditGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))

                    Else

                        obj = arcn.GetApplicationPartByAlias(objectAlias)

                        obj.GrantAccess(AdminGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(ITGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))
                        obj.GrantAccess(AuditGroupId, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, perm))

                    End If

                End If

            Next

        End If


    End Sub



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Call InitializeDB()
        End If


    End Sub

    Private Sub OldWay()


        Dim arcn As New ARConnection
        arcn.ConnectToCatalog()

        Dim arapp As ARApplication
        arapp = arcn.GetApplicationByAlias("frits")

        Dim arRelType As New ARDBRelationshipType
        arRelType.Connection = arcn.ARDBProvider.Connection

        ' get ObjectID of the "Administrators" group
        Dim groupID As Integer = arcn.GetGroupByAlias("Administrators").ObjectID
        Dim ITGroup As Integer = 12303

        Dim objectName As String
        Dim objectAlias As String

        objectName = "Login"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".login"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

        End If

        objectName = "Logout"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".logout"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

        End If

        objectName = "Access Denied"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".accessdenied"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

        End If

        objectName = "Welcome"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".welcome"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

        End If

        objectName = "My Profile"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".myprofile"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

        End If

        objectName = "Setup"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".setup"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

            arRelType.Insert("List", "list", obj.ObjectAlias, "List", False)

            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "List"))

            objectName = "Branches"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".setup.branches"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If

        End If


        objectName = "Management"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

            arRelType.Insert("List", "list", obj.ObjectAlias, "List", False)

            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "List"))



            '---------------------REQUEST RECORDS ENDS--------------------




            '--------------------------REGISTER BEGINS-------------------------

            objectName = "Register"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.register"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))




                objectName = "Fixed Assets"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.register.fixedassets"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If


                objectName = "Authorise New Assets"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.register.authorisenewasset"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



                objectName = "Asset Store"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.register.assetstore"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



                objectName = "Classify Fixed Asset"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.register.classifyasset"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



            End If
            '--------------------------------REGISTER ENDS ----------------------------






            '------------------------------- MAINTENANCE BEGINS -------------------------



            objectName = "Maintenance"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



                objectName = "Schedule Maintenance"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance.schedulemaintenance"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))






                    objectName = "Create Schedule"
                    objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance.schedulemaintenance.createschedule"

                    If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                        obj = New ARApplicationPart()

                        obj.ARConnection = arcn
                        obj.ObjectName = objectName
                        obj.ObjectAlias = objectAlias
                        obj.ObjectDescription = ""

                        obj.InsertNew()

                        arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                        arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                        arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                        arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                        obj = arcn.GetApplicationPartByAlias(objectAlias)

                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                    End If


                    objectName = "Schedule Asset(s)"
                    objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance.schedulemaintenance.scheduleassets"

                    If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                        obj = New ARApplicationPart()

                        obj.ARConnection = arcn
                        obj.ObjectName = objectName
                        obj.ObjectAlias = objectAlias
                        obj.ObjectDescription = ""

                        obj.InsertNew()

                        arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                        arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                        arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                        arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                        obj = arcn.GetApplicationPartByAlias(objectAlias)

                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                        obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                    End If


                End If


                objectName = "Record Maintenance Details"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance.recordmaintenancedetail"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If


                objectName = "Maintenance Reports"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance.maintenancereports"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



                objectName = "Configure Maintenance"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.maintenance.configuremaintenance"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If '




            End If '------------------------------- MAINTENANCE ENDS -------------------------





            '
            objectName = "Procurement"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.procurement"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


            End If





            '-------------------MOVEMENT BEGINS -----------------
            objectName = "Movement"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



                objectName = "Assign Asset To Branch"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement.assigntobranch"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



                objectName = "Authorise Asset Assignment"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement.authoriseassignment"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If


                objectName = "Move Asset"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movements.moveasset"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



                objectName = "Authorise Asset Movement"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement.authorisemovement"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If


                objectName = "Receive Asset(s)"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement.receiveasset"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If





                objectName = "Move To Store"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement.movetostore"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If


                objectName = "Movement Logs"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.movement.movementlog"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



            End If  '---------------- MOVEMENT ENDS --------------------------









            '-------------------DEPRECIATION BEGINS -----------------
            objectName = "Depreciation"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



                objectName = "Asset Summary"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation.assetsummary"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If


                objectName = "Run Depreciation"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation.rundepreciation"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If




                objectName = "Depreciation Records"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation.depreciationrecords"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



                objectName = "Post Values To T24"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation.postvalues"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



                objectName = "Summary Report"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation.summaryreport"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If




                objectName = "Financial Entries"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.depreciation.financialentries"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If


            End If   '--------------------- DEPRECIATION ENDS -------------------------------




            '-------------------DISPOSAL BEGINS -----------------
            objectName = "Disposal"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.disposal"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



                objectName = "Raise Disposal Notice"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".management.disposal.raisenotice"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



            End If



        End If  '------------------------ MANAGEMENT ENDS --------------------------------





        '----------------------------- ENQUIRIES BEGINS --------------------------------



        objectName = "Enquiries"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

            arRelType.Insert("List", "list", obj.ObjectAlias, "List", False)

            obj = arcn.GetApplicationPartByAlias(objectAlias)

            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "List"))





            objectName = "Asset Store"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.store"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



                objectName = "Un-Authorised Assets"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.store.unauthorisedasset"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If




                objectName = "Authorised Assets"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.store.authorisedasset"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



                objectName = "Asset Status"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.store.assetstatus"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If


                objectName = "Asset Condition"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.store.assetcondition"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



                objectName = "Asset Supplier"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.store.assetsupplier"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If

            End If '---------------- Asset Store Ends --------------------------





            objectName = "Fixed Asset Classification"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.fixedassetclassification"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If '---------------- Fixed Asset Classification Ends --------------------------



            objectName = "Asset Item Classification"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assetitemclassification"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If '---------------- Asset Item Classification Ends --------------------------




            objectName = "Asset Type"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assettype"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If '---------------- Asset Type Ends --------------------------



            objectName = "Asset Category"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.category"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If '---------------- Asset Category Ends --------------------------




            objectName = "Asset Item Owner"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.owner"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If '---------------- Asset Owner Ends --------------------------





            objectName = "Asset Item User"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.itemuser"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If '---------------- Asset Item User Ends --------------------------






            objectName = "Assignments"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assignment"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))




                objectName = "Un-Authorised Assignments"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assignment.unauthorised"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If




                objectName = "Authorised Assignments"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assignment.authorised"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



                objectName = "Un-Received Assignments"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assignment.unreceived"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If


                objectName = "Received Assignments"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.assignment.received"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If


            End If '---------------- Assignments Ends --------------------------





            objectName = "Movements"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.movements"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))





                objectName = "Un-Authorised Movements"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.movements.unauthorised"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If




                objectName = "Authorised Movements"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.movements.authorised"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If



                objectName = "Un-Received Movements"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.movements.unreceived"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If


                objectName = "Received Movements"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.movements.received"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If


            End If '---------------- Movements Ends --------------------------






            objectName = "Authorisations"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.authorisation"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))





                objectName = "Asset Entry Authorisation"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.authorisation.newentry"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If




                objectName = "Assignment Authorisation"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.authorisation.assignment"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If



                objectName = "Movement Authorisation"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.authorisation.movement"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


                End If

            End If '---------------- Authorisations Ends --------------------------



            objectName = "Branch Assets"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.branchassets"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



            End If




            objectName = "Suppliers"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".enquiries.suppliers"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))



            End If



        End If
        '----------------------------- ENQUIRIES ENDS -------------------------------------


        objectName = "User Access Level"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".useraccesslevel"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

            arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
            arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
            arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
            arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

            obj = arcn.GetApplicationPartByAlias(objectAlias)

            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


            objectName = "View All Branches"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".useraccesslevel.viewallbranches"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If


            objectName = "View All Departments"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".useraccesslevel.viewalldepartments"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))


            End If

        End If






        objectName = "Reports"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".reports"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

            arRelType.Insert("List", "list", obj.ObjectAlias, "List", False)

            obj = arcn.GetApplicationPartByAlias(objectAlias)

            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "List"))


            objectName = "Summary"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".reports.summary"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If

        End If







        objectName = "Utilities"
        objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities"

        If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

            Dim obj As New ARApplicationPart()

            obj.ARConnection = arcn
            obj.ObjectName = objectName
            obj.ObjectAlias = objectAlias
            obj.ObjectDescription = ""

            obj.InsertNew()

            arRelType.Insert("List", "list", obj.ObjectAlias, "List", False)

            obj = arcn.GetApplicationPartByAlias(objectAlias)

            obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "List"))

            objectName = "Profile Manager"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                objectName = "Users"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager.users"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If

                objectName = "Groups"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager.groups"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If

                objectName = "Org. Units"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager.orgunits"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If

                objectName = "Roles"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager.roles"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If

                objectName = "Change Password"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager.changepassword"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If






                objectName = "Forgotten Password"
                objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.profilemanager.forgottenpassword"

                If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                    obj = New ARApplicationPart()

                    obj.ARConnection = arcn
                    obj.ObjectName = objectName
                    obj.ObjectAlias = objectAlias
                    obj.ObjectDescription = ""

                    obj.InsertNew()

                    arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                    arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                    arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                    arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                    obj = arcn.GetApplicationPartByAlias(objectAlias)

                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                    obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

                End If

            End If

            objectName = "Settings"
            objectAlias = arcn.GetApplicationByID(arapp.ObjectID).ObjectAlias & ".utilities.settings"

            If arcn.GetApplicationPartByAlias(objectAlias) Is Nothing Then

                obj = New ARApplicationPart()

                obj.ARConnection = arcn
                obj.ObjectName = objectName
                obj.ObjectAlias = objectAlias
                obj.ObjectDescription = ""

                obj.InsertNew()

                arRelType.Insert("Read", "read", obj.ObjectAlias, "Read", False)
                arRelType.Insert("Create", "create", obj.ObjectAlias, "Create", False)
                arRelType.Insert("Change", "change", obj.ObjectAlias, "Change", False)
                arRelType.Insert("Delete", "delete", obj.ObjectAlias, "Delete", False)

                obj = arcn.GetApplicationPartByAlias(objectAlias)

                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Read"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Create"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Change"))
                obj.GrantAccess(groupID, GetRelationshipTypeID(arcn.ARDBProvider, obj.ObjectAlias, "Delete"))

            End If

        End If


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


    Protected Sub RunAppConfiguration()

        For Each part In ApplicationPartList()

            Dim _partName = part.Item1
            Dim _partAlias = part.Item2
            Dim _partPerms = part.Item3

            Call CreatePartWithPermission(_partName, _partAlias, _partPerms)

        Next

    End Sub

    Protected Sub btnCreateConfiguration_Click(sender As Object, e As EventArgs)

        AdminGroupId = arcn.GetGroupByAlias("Administrators").ObjectID
        ITGroupId = 12303
        AuditGroupId = 12518
        Call RunAppConfiguration()
        ShowMessage("Configuration Successfully Completed")
    End Sub
End Class