Imports System
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Net.Mail
Imports System.Net.Mail.MailMessage
Imports System.Data
Imports System.Web.Security
Imports System.Threading
Imports System.Globalization
Imports SecureAccess
Imports PortSight.SecureAccess.ARObjects
Imports PortSight.SecureAccess.ARDataServices

Public Class UserControlBase
    Inherits System.Web.UI.UserControl

#Region " DECLARATIONS "

    Private _ARConn As ARConnection
    Private _ticket As ARUserTicket = Nothing
    Private _OwnerID As Integer
    Private _ResManager As System.Resources.ResourceManager
    Private _CurrentCultureInfo As System.Globalization.CultureInfo
    Private _FailedLoginCount As String
    Private _BranchName As String
    Private _BranchCode As String
    Private _BrandId As Integer
    Private _DepartmentCode As String
#End Region

#Region " Properties"

    Public Property CurrentUser() As ARUserTicket
        Get
            If _ticket Is Nothing Then
                Try
                    _ticket = CType(HttpContext.Current.Session("ARUserTicket"), PortSight.SecureAccess.ARObjects.ARUserTicket)
                Catch
                    _ticket = Nothing
                End Try
            End If
            Return _ticket
        End Get

        Set(ByVal Value As ARUserTicket)
            If Value Is Nothing Then
                HttpContext.Current.Session.Remove("ARUserTicket")
                _ticket = Nothing
            Else
                HttpContext.Current.Session("ARUserTicket") = Value
                _ticket = Value
            End If
        End Set
    End Property

    Public Property OwnerID() As Integer
        Get
            _OwnerID = CType(HttpContext.Current.Session("AROwnerID"), Integer)
            'If _OwnerID = 0 Then _OwnerID = 0 '-1
            Return _OwnerID
        End Get
        Set(ByVal Value As Integer)
            HttpContext.Current.Session("AROwnerID") = Value
        End Set
    End Property

    Public ReadOnly Property ResManager() As System.Resources.ResourceManager
        Get
            If _ResManager Is Nothing Then
                _ResManager = (CType(HttpContext.Current.Application("RM"), System.Resources.ResourceManager))
            End If
            Return _ResManager
        End Get
    End Property

    Public ReadOnly Property CurrentCultureInfo() As System.Globalization.CultureInfo
        Get
            ' Globalization
            If Not (HttpContext.Current.Session("ARCulture") Is Nothing) Then
                Return CType(HttpContext.Current.Session("ARCulture"), CultureInfo)
            Else
                Return New CultureInfo("en-us")
            End If
        End Get
    End Property

    Public ReadOnly Property ARConn() As ARConnection
        Get
            If IsNothing(_ARConn) Then
                _ARConn = New ARConnection(ConfigurationManager.AppSettings("SecureAccessConnectionString"))
            End If
            Return _ARConn
        End Get
    End Property

    Public ReadOnly Property FailedLoginCount() As String
        Get
            _FailedLoginCount = Me.CurrentUser.GetUserObject.GetPropertyValue("FailedLoginCount")
            Return _FailedLoginCount
        End Get
    End Property

#End Region

#Region " Methods"

    Public ReadOnly Property ResourceID() As Integer
        Get
            Return Me.ARConn.GetResourceByAlias(ObjectAlias).ObjectID
        End Get
    End Property

    Public Property ObjectID() As Integer
        Get
            Return ViewState("ObjectID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("ObjectID") = Value
        End Set
    End Property

    Public Property ObjectAlias() As String
        Get
            Return ViewState("ObjectAlias")
        End Get
        Set(ByVal Value As String)
            ViewState("ObjectAlias") = Value
        End Set
    End Property

    Public Function CheckOwnership(ByVal parentObjectID As Integer, ByVal childOwnerID As Integer) As Boolean
        If Me.ARConn.GetObjectByID(parentObjectID).OwnerID = childOwnerID Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CheckReflexiveMembership(ByVal parentObjectID As Integer, ByVal childObjectID As Integer) As Boolean
        Dim dbrel As New ARDBRelationship()
        dbrel.Connection = Me.ARConn.ARDBProvider.Connection
        Return dbrel.IsReflexiveRelationship(parentObjectID, childObjectID, GetRelationshipTypeID(ARConstants.cRELATIONSHIP_TYPE_NAMESPACE_VALUE, ARConstants.cRELATIONSHIP_TYPE_ALIAS_VALUES_MEMBERSHIP))
    End Function

    Public Function GetRelationshipTypeID(ByVal strNamespace As String, ByVal strAlias As String) As Integer
        '##PARAM RelationshipType Relationship Type in form <namespace>.<alias>
        Dim ds As DataSet

        'Create Array of conditions for selecting proper Relationshiptype
        Dim CondArray(1, 1) As String
        CondArray(0, 0) = "RelationshipTypeNamespace"
        CondArray(0, 1) = strNamespace
        CondArray(1, 0) = "RelationshipTypeAlias"
        CondArray(1, 1) = strAlias
        Dim ARDBRelationshiptype As ARDBRelationshipType = New ARDBRelationshipType()
        ARDBRelationshiptype.Connection = Me.ARConn.ARDBProvider.Connection
        ARDBRelationshiptype.Parameters = CondArray
        ds = ARDBRelationshiptype.SelectRecords()
        GetRelationshipTypeID = CInt(ds.Tables(0).Rows(0)("RelationshipTypeID"))
        ARDBRelationshiptype.Close()
        ARDBRelationshiptype = Nothing
    End Function

    Public Function GetOperatorByID(ByVal objectID As Integer) As AROperator
        Return Me.ARConn.GetOperatorByID(objectID)
    End Function

    Public Function GetResourceID(ByVal ObjectAlias As String) As Integer
        Return Me.ARConn.GetResourceByAlias(ObjectAlias).ObjectID
    End Function

    Public Sub Log(ByVal ActorObjectID As Integer, ByVal AccessedObjectID As Integer, ByVal Message As String, ByVal EventCode As String)
        Me.ARConn.Log(ActorObjectID, AccessedObjectID, Message, EventCode)
    End Sub

    Public Sub SendMail(ByVal [to] As String, ByVal subject As String, ByVal msg As String, Optional ByVal attachment As String = "", Optional ByVal cc As String = "", Optional ByVal bcc As String = "", Optional ByVal isHtml As Boolean = False)

        Dim smtpClient As New SmtpClient

        ' You can specify the host name or ipaddress of your server
        ' Default in IIS will be localhost 
        smtpClient.Host = Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("SMTPServer")

        ' Default port will be 25
        smtpClient.Port = 25

        Dim mm As New MailMessage

        ' From address will be given as a MailAddress ObjectGetApplicationByAlias("FRITS").SetPropertyValue
        mm.From = New MailAddress(Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("NotificationEmail"), Me.ARConn.GetApplicationByAlias("FRITS").GetPropertyValue("NotificationEmailAlias"))

        ' To address collection of MailAddress
        mm.To.Add([to])

        'Body can be Html or text format
        'Specify true if it  is html message
        mm.IsBodyHtml = isHtml

        ' Message subject content
        mm.Subject = subject

        ' Message body content
        mm.Body = msg

        ' CC and BCC optional
        ' MailAddressCollection class is used to send the email to various users
        ' You can specify Address as new MailAddress("admin1@yoursite.com")

        If cc <> "" Then
            Dim delim As Char = ";"
            Dim str As String
            For Each str In cc.Split(delim)
                If Not str = "" Then
                    mm.CC.Add(New MailAddress(str))
                End If
            Next
        End If

        If bcc <> "" Then
            Dim delim As Char = ";"
            Dim str As String
            For Each str In bcc.Split(delim)
                If Not str = "" Then
                    mm.Bcc.Add(New MailAddress(str))
                End If
            Next
        End If

        'Build an IList of mail attachments using the files named in the string.
        If attachment <> "" Then
            Dim delim As Char = ";"
            Dim str As String
            For Each str In attachment.Split(delim)
                If Not str = "" Then
                    mm.Attachments.Add(New Attachment(str))
                End If
            Next
        End If

        ' Send SMTP mail
        smtpClient.Send(mm)

    End Sub

    Public Function IsAuthorized(ByVal ticket As ARUserTicket, ByVal resourcealias As String, ByVal permissionalias As String) As Boolean
        If ARHelper.IsAuthorized(ticket.Login, resourcealias, permissionalias) Then
            Return True
        Else
            'gets all operators authorized for this permission
            Dim authorizedOperator As ARObject
            For Each authorizedOperator In GetAuthorisedOperators(Me.ARConn.GetResourceByAlias(resourcealias).ObjectID)
                For Each membership As String In ticket.MembershipObjectAliases.Split(";")
                    If Not membership = "" Then
                        Dim arOperator As AROperator
                        If Not Me.ARConn.GetGroupByAlias(membership) Is Nothing Then
                            If authorizedOperator.ObjectID = Me.ARConn.GetGroupByAlias(membership).ObjectID Then
                                arOperator = Me.ARConn.GetOperatorByID(authorizedOperator.ObjectID)
                                Return arOperator.IsAuthorized(resourcealias, permissionalias)
                            End If
                        ElseIf Not Me.ARConn.GetRoleByAlias(membership) Is Nothing Then
                            If authorizedOperator.ObjectID = Me.ARConn.GetRoleByAlias(membership).ObjectID Then
                                arOperator = Me.ARConn.GetOperatorByID(authorizedOperator.ObjectID)
                                Return arOperator.IsAuthorized(resourcealias, permissionalias)
                            End If
                        End If
                    End If
                Next
            Next
            Return False
        End If
    End Function

    Public Function GetAuthorisedOperators(ByVal ResourceID As Integer) As ARObjectsCollection

        Dim o As New ARObjectsCollection

        Dim dr As DataRow = Nothing
        Dim ds As DataSet
        Dim ardbobj As New ARDBObject
        Dim arobj As ARObject

        ds = ardbobj.SelectPermissionMatrix(ResourceID)

        Dim lastValue As String = ""
        Dim rowIndex As Integer = 0
        Dim period As Integer = 0

        If ds.Tables(0).Rows.Count > 0 Then

            lastValue = ds.Tables(0).Rows(0)("ChildObjectID")

            'find the row-repeating period
            While lastValue = ds.Tables(0).Rows(rowIndex)("ChildObjectID")
                rowIndex += 1
                If rowIndex = ds.Tables(0).Rows.Count Then
                    Exit While
                End If
            End While

            period = rowIndex

            For rowIndex = 0 To ds.Tables(0).Rows.Count - 1 Step period
                arobj = Me.ARConn.GetObjectByID(ds.Tables(0).Rows(rowIndex)("ChildObjectID"))
                o.Add(arobj)
            Next

        End If

        Return o

    End Function

    Public Sub ShowMessage(ByVal msg As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alertMessage('" & msg.Replace(vbCrLf, " ") & "');", True)
        'Me.ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "<script language='javascript'>alert('" & Utils.JSEscape(msg) & "');</script>")
    End Sub

    Public Sub ShowSuccessMessage(ByVal msg As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "success", "showSuccessMessage('" & msg.Replace(vbCrLf, " ") & "');", True)
    End Sub

    Public Sub ShowErrorMessage(ByVal msg As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "success", "showErrorMessage('" & msg.Replace(vbCrLf, " ") & "');", True)
    End Sub


    Public Function GetPermissions(ByVal ResourceID As Integer) As Object

        Dim o As New ArrayList

        Dim ResourceAlias As String = Nothing
        Dim resource As ARResource = Me.ARConn.GetResourceByID(ResourceID)

        If Not resource Is Nothing Then
            ResourceAlias = resource.ObjectAlias
        End If

        Dim Params(0, 1) As String
        Params(0, 0) = ARConstants.cRELATIONSHIP_TYPE_NAMESPACE
        Params(0, 1) = ResourceAlias

        Me.ARConn.ARDBProvider.Parameters = Params
        Me.ARConn.ARDBProvider.Table = "View_AR_RelationshipType"

        Dim ds As DataSet = Me.ARConn.ARDBProvider.SelectRecords

        If ds.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ds.Tables(0).Rows
                o.Add(dr(2))
            Next
        End If

        Return o

    End Function

    Public ReadOnly Property BranchName() As String
        Get
            _BranchName = Me.CurrentUser.GetUserObject.GetPropertyValue("BranchName").ToString()
            Return _BranchName
        End Get
    End Property
    Public ReadOnly Property BranchCode() As String
        Get
            _BranchCode = Me.CurrentUser.GetUserObject.GetPropertyValue("BranchCode").ToString()
            Return _BranchCode
        End Get
    End Property


    Private itService As ITService
    Public Property _itService As ITService
        Get
            itService = DirectCast(Session.Item("itService"), ITService)
            If itService Is Nothing Then
                itService = New ITService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            End If
            Return itService
        End Get
        Set(value As ITService)
            itService = value
            Session.Add("itService", itService)
        End Set
    End Property


    Private setupService As SetupService
    Public Property _setupService As SetupService
        Get
            setupService = DirectCast(Session.Item("setupService"), SetupService)
            If setupService Is Nothing Then
                setupService = New SetupService(Me.BranchCode, Me.BranchName, Me.CurrentUser.ObjectID)
            End If
            Return setupService
        End Get
        Set(value As SetupService)
            setupService = value
            Session.Add("setupService", setupService)
        End Set
    End Property



#End Region

End Class
