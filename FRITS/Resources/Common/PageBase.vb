Imports System.Net.Mail
Imports System.Globalization
Imports PortSight.SecureAccess.ARObjects
Imports PortSight.SecureAccess.ARDataServices
Imports System.Web.Script.Serialization
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Reflection
Imports PATechnologies.Web.Common
Imports SecureAccess
Imports FRITS.DAL

Public Class PageBase
    Inherits System.Web.UI.Page

#Region " DECLARATIONS "
    Private _ARConn As ARConnection
    Private _ticket As ARUserTicket = Nothing
    Private _OwnerID As Integer
    Private _ResManager As System.Resources.ResourceManager
    Private _CurrentCultureInfo As System.Globalization.CultureInfo
    Private _Glb As GlobalLib
    Private _LastLogin As String
    Private _LicenseOK As Boolean
    Private _LicenseType As Integer
    Private _BranchName As String
    Private _BranchCode As String
    Private _BrandId As Integer
    Private _DepartmentCode As String
#End Region


#Region " Properties"

    Public Property Mode() As String
        Get
            Return ViewState("_Mode")
        End Get
        Set(ByVal Value As String)
            ViewState("_Mode") = Value
        End Set
    End Property

    Public ReadOnly Property LastLogin() As String
        Get
            _LastLogin = Me.CurrentUser.GetUserObject.GetPropertyValue("LastTimeLogOn")
            Return _LastLogin
        End Get
    End Property

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
    'Public ReadOnly Property BranchID() As String
    '    Get
    '        Dim branch = New BranchRepository(_context).GetList().Where(Function(x) x.BranchCode.Equals(Me.BranchCode)).FirstOrDefault()
    '        If (Not branch Is Nothing) Then
    '            _BrandId = branch.BranchId
    '        End If
    '        Return _BrandId
    '    End Get
    'End Property
    Public ReadOnly Property DepartmentCode() As String
        Get
            _DepartmentCode = IIf(Me.CurrentUser.GetUserObject.GetPropertyValue("DepartmentCode") Is Nothing, "0", Me.CurrentUser.GetUserObject.GetPropertyValue("DepartmentCode"))
            Return _DepartmentCode
        End Get
    End Property

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

    Public ReadOnly Property Glb() As GlobalLib
        Get
            If IsNothing(_Glb) Then
                _Glb = New GlobalLib
            End If
            Return _Glb
        End Get
    End Property

    Public Property LicenseOK() As Boolean
        Get
            _LicenseOK = CType(HttpContext.Current.Session("LicenseOK"), Boolean)
            Return _LicenseOK
        End Get
        Set(ByVal Value As Boolean)
            HttpContext.Current.Session("LicenseOK") = Value
        End Set
    End Property

    Public Property LicenseType() As Integer
        Get
            _LicenseType = CType(HttpContext.Current.Session("LicenseType"), Integer)
            Return _LicenseType
        End Get
        Set(ByVal Value As Integer)
            HttpContext.Current.Session("LicenseType") = Value
        End Set
    End Property



    Private _viewAllBranches As Boolean
    Public Property ViewAllBranches() As Boolean
        Get
            Return IIf(BranchCode = "01", True, False) ' _viewAllBranches 'True 
        End Get
        Set(ByVal value As Boolean)
            _viewAllBranches = value
        End Set
    End Property

    'Private _userDepartment As Integer
    Public Property UserDepartmentId As Integer
        Get
            Return _userDept ' _userDepartment
        End Get
        Set(value As Integer)
            _userDept = value
        End Set
    End Property


    Private _viewAllDepartments As Boolean
    Public Property ViewAllDepartments() As Boolean
        Get
            Return IIf(BranchCode = "01", True, False) ' _viewAllDepartments 'True 
        End Get
        Set(ByVal value As Boolean)
            _viewAllDepartments = value
        End Set
    End Property

    Private _editFinancialDetails As Boolean
    Public Property EditFinancialDetails() As Boolean
        Get
            Return IIf(BranchCode = "01", True, False) '_editFinancialDetails
        End Get
        Set(ByVal value As Boolean)
            _editFinancialDetails = value
        End Set
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

    Public Sub RunScript(ByVal jsscript As String)
        Me.ClientScript.RegisterStartupScript(Me.GetType(), "Script", "<script language='javascript'>" & jsscript & "</script>")
    End Sub

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

    Public Sub ConfirmDelete(ByVal msg As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "success", "confirmDelete('" & msg.Replace(vbCrLf, " ") & "');", True)
    End Sub

    Public Sub ClosePage(ByVal results As String)
        Me.ClientScript.RegisterClientScriptBlock(Me.GetType(), "CloseScript", "<script language=""Javascript"">parent.returnValue='" & results & "';parent.close();</script>")
    End Sub

    Public Sub ClosePage()
        Me.ClientScript.RegisterStartupScript(Me.GetType(), "CloseScript", "<script language=""Javascript"">parent.close();</script>")
    End Sub

    Public Sub PageHeader(ByVal title As String)
        Me.ClientScript.RegisterStartupScript(Me.GetType(), "PageHeaderScript", "<script language='javascript'>top.document.getElementById('lblPageHeader').innerText ='" & title & "';</script>")
    End Sub

    Public Sub Log(ByVal ActorObjectID As Integer, ByVal AccessedObjectID As Integer, ByVal Message As String, ByVal EventCode As String)
        Me.ARConn.Log(ActorObjectID, AccessedObjectID, Message, EventCode)
    End Sub

    Public Sub SendMail(ByVal [to] As String, ByVal subject As String, ByVal msg As String, Optional ByVal attachment As String = "", Optional ByVal cc As String = "", Optional ByVal bcc As String = "", Optional ByVal isHtml As Boolean = False)

        Dim smtpClient As New SmtpClient

        ' You can specify the host name or ipaddress of your server
        ' Default in IIS will be localhost 
        smtpClient.Host = Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("SMTPServer")

        ' Default port will be 25
        smtpClient.Port = 25

        Dim mm As New MailMessage

        ' From address will be given as a MailAddress ObjectGetApplicationByAlias("cms").SetPropertyValue
        mm.From = New MailAddress(Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("NotificationEmail"), Me.ARConn.GetApplicationByAlias("frits").GetPropertyValue("NotificationEmailAlias"))

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

    Public Sub DisplayPageHeaderDetails()

        If Not Me.CurrentUser Is Nothing Then

            'Display current user
            CType(Me.Master.FindControl("lblCurrentUser"), Label).Text = ""
            CType(Me.Master.FindControl("lblCurrentUser"), Label).Text &= "Welcome, " & Me.CurrentUser.ObjectName

            'Display logout link
            CType(Me.Master.FindControl("lblLogout"), Label).Text = ""
            CType(Me.Master.FindControl("lblLogout"), Label).Text = " | <a href=""javascript:top.LogOut();"" title=""Log Out"" onmouseover=""window.status='Log Out'; return true;"" onmouseout=""window.status=''; return true;"">Log Out</a>"

            'Display top banner links
            CType(Me.Master.FindControl("lblLink"), Label).Text = ""
            CType(Me.Master.FindControl("lblLink"), Label).Text = "<a href=""javascript:top.LaunchHomepage();"" title=""Home"" onmouseover=""window.status='Home'; return true;"" onmouseout=""window.status=''; return true;"">Home</a> | <a href=""javascript:top.LaunchMyProfile();"" title=""My Profile"" onmouseover=""window.status='My Profile'; return true;"" onmouseout=""window.status=''; return true;"">My Profile</a> | <a href=""javascript:LaunchHelp();"" title=""Help"" onmouseover=""window.status='Help'; return true;"" onmouseout=""window.status=''; return true;"">Help</a>"

            CType(Me.Master.FindControl("lblLastLogin"), Label).Text = "<b>Last Login :</b> " & Me.LastLogin

        End If

    End Sub

    Public Sub DisplayMenuItems()

        Dim mnu As ComponentArt.Web.UI.Menu

        mnu = CType(Me.Master.FindControl("Menu"), ComponentArt.Web.UI.Menu)

        For Each mnuItem As ComponentArt.Web.UI.MenuItem In mnu.Items

            If Not mnuItem.Text = "Dashboard" Then

                If Not Me.CurrentUser Is Nothing Then

                    mnuItem.Visible = ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItem.Value, "list") OrElse ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItem.Value, "read")

                    For Each mnuItemChild As ComponentArt.Web.UI.MenuItem In mnuItem.Items

                        mnuItemChild.Width = Unit.Pixel(200)
                        mnuItemChild.Visible = ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItemChild.Value, "list") OrElse ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItemChild.Value, "read")

                        For Each mnuItemChildChild As ComponentArt.Web.UI.MenuItem In mnuItemChild.Items

                            mnuItemChildChild.Width = Unit.Pixel(200)
                            mnuItemChildChild.Visible = ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItemChildChild.Value, "list") OrElse ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItemChildChild.Value, "read")

                            For Each mnuItemChildChildChild As ComponentArt.Web.UI.MenuItem In mnuItemChildChild.Items

                                mnuItemChildChildChild.Width = Unit.Pixel(200)
                                mnuItemChildChildChild.Visible = ARHelper.IsAuthorized(Me.CurrentUser.Login, mnuItemChildChildChild.Value, "read")

                            Next

                        Next

                    Next

                Else
                    mnuItem.Visible = False
                End If

            End If
        Next

    End Sub

    Public Function IsAuthorized(ByVal ticket As ARUserTicket, ByVal resourcealias As String, ByVal permissionalias As String) As Boolean

        If ARHelper.IsAuthorized(ticket.Login, resourcealias, permissionalias) Then
            Return True
        Else
            'gets all operators authorized for this permission
            Try
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
            Catch ex As Exception
                ex.Data.Clear()
            End Try

        End If
        Return False
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

    Public Sub CheckLicense()

        If Me.LicenseOK = False Then

            Dim lic As New LicenseManager

            If lic.CheckLicense Then

                Me.LicenseOK = True
                Me.LicenseType = lic.LicenseType

            Else

                ' Do not show menu
                'CType(Me.Master.FindControl("Menu"), ComponentArt.Web.UI.Menu).Visible = False

                ' Do not show content place holder
                'CType(Me.Master.FindControl("ContentPlaceHolder"), ContentPlaceHolder).Visible = False

                ' Show message box
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & lic.Error.Replace("<br>", "\n") & ".');", True)

                ' Show error on dialog
                'CType(Me.Master.FindControl("txtLicenseError"), Label).Text = lic.Error

                ' Show dialog box
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "license_toggle();", True)

                'Redirect to sign in page.
                Me.Response.Redirect(Me.Request.ApplicationPath & "/Login.aspx?fn=logout&msg=" & Functions.Encode64(lic.Error), True)

            End If

        Else

            If Me.LicenseType = LicenseTypeEnum.Evaluation Then

                '' Show message box
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('You are currently running in demo mode');", True)

                Dim tbl As New StringBuilder()

                tbl.Append("<div id=""nag"">")
                tbl.Append("<table cellspacing=""0"" cellpadding=""0"" border=""1"" align=""center"">")
                tbl.Append("    <tr>")
                tbl.Append("        <td style=""background-color: #CFE05A; height: 25px; padding-left:5px; padding-right:5px;"" nowrap=""nowrap"">")
                tbl.Append("            You are currently running in demo mode")
                tbl.Append("        </td>")
                tbl.Append("    </tr>")
                tbl.Append("</table>")
                tbl.Append("</div>")

                ' Show message on page
                CType(Me.Master.FindControl("ltlText"), Literal).Text = tbl.ToString()

            End If

        End If

    End Sub

    Public Sub PasswordExpiryAlert()
        Dim noOfDays As Integer = DateDiff(DateInterval.Day, Now.Date, Me.CurrentUser.GetUserObject.PasswordExpires)
        Dim msg As String = "Your password expires in " & noOfDays & " day(s). Change your password now."
        If noOfDays < 8 Then
            Me.ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "<script language='javascript'>alert('" & Utils.JSEscape(msg) & "');window.location.href='MyProfile.aspx';</script>")
        End If
    End Sub

    Private Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Me.ClientScript.RegisterClientScriptBlock(Me.GetType(), "", "<script type=""text/javascript"">var applicationPath = '" & Me.Request.ApplicationPath & "';</script>")
    End Sub

#End Region


#Region "ADDED_BY_CYO"

    Public _ApplicationAliase = "frits"
    Private __context As AppDbContext
    Public Property _context() As AppDbContext
        Get

            If (__context Is Nothing) Then
                If HttpContext.Current.Items("_context") = Nothing Then
                    __context = New AppDbContext()
                    HttpContext.Current.Items("_context") = __context
                Else
                    __context = DirectCast(HttpContext.Current.Items("_context"), AppDbContext)
                End If
            End If

            Return __context
        End Get
        Set(ByVal value As AppDbContext)
            __context = value
        End Set
    End Property


    'Public _context As AppDbContext()
    Public _userBranch As Integer
    Public _userDept As Integer

    Public Function CreateAppContext(Optional ByVal trans As System.Data.Common.DbTransaction = Nothing) As AppDbContext

        If Not IsNothing(trans) Then
            _context = New AppDbContext(CurrentUser().ObjectID, trans)
        Else
            _context = New AppDbContext(CurrentUser().ObjectID)
        End If

        loggedOnUserId = CurrentUser().ObjectID

        'If (Me.BranchCode.ToString() <> Nothing) Then
        '    _userBranch = GetUserBranchId(Me.BranchCode)
        'Else
        '    _userBranch = 0
        'End If

        _userDept = IIf(Me.DepartmentCode.ToString() Is Nothing, 0, CInt(Me.DepartmentCode))
        Return _context

    End Function

    Public Function GetResponse(msg As String) As Boolean
        Dim rslt = MsgBox(msg & "?", 32 + 4)
        If rslt = vbOK Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub Validate_Access(ByVal pageAlias As String, ByVal permValues As String)

        Dim permList = permValues.Split(",")
        Dim permCount As Integer = 0

        For Each perm As String In permList
            If HasPermision(pageAlias, perm) Then
                permCount += 1
                Exit For
            End If
        Next

        If permCount = 0 Then
            'Redirect to access denied page.
            Me.Response.Redirect(Me.Request.ApplicationPath & "/AccessDenied.aspx", True)
        End If
    End Sub

    Protected Sub Show_StartUpInfo(ByVal pageAlias As String)
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = _ApplicationAliase & "." & pageAlias

            'Check authentication.
            If Not Me.CurrentUser Is Nothing Then

                If Me.CurrentUser.IsMemberAll("Administrators") OrElse (HasPermision(pageAlias, "read") Or HasPermision(pageAlias, "list")) Then

                    'Display Details
                    Me.DisplayPageHeaderDetails()

                    'Display Menu Items
                    Me.DisplayMenuItems()
                    'Me.CreateAppContext()

                Else
                    'Redirect to access denied page.
                    Me.Response.Redirect(Me.Request.ApplicationPath & "/AccessDenied.aspx", True)

                End If

            Else
                'Redirect to sign in page.
                Me.Response.Redirect(Me.Request.ApplicationPath & "/Login.aspx", True)
            End If

            'Session.Add("ViewAllBranches", HasPermision("useraccesslevel.viewallbranches", "read", False))
            'Me.ViewAllBranches = Session("ViewAllBranches")
            'Session.Add("ViewAllDept", HasPermision("useraccesslevel.viewalldepartments", "read", False))

            'Me.ViewAllDepartments = Session("ViewAllDept")

        Catch ex As Exception
            Throw ex
        Finally
            'Close secure access connection
            Me.ARConn.Close()
        End Try
    End Sub



    Public Function ValidateObject(ByVal obj As Object, ByRef displayObj As Label) As Boolean
        Dim rslt As Boolean = True
        Dim str As String = Nothing
        Dim str1 As String = Nothing

        'Dim ctrl = TryCast(Me.Page.FindControl(displayObj.ID.ToString), Label)

        If Not obj.Item("IsValid") Then
            rslt = False
            Dim err = obj.Item("Errors")
            str = "<b>Please Check The Following To Continue!</b>" & "<br/><br/><br/>"
            For i = 0 To err.Count - 1
                str &= err(i)(1) & "<br />"   '(err(i)(0) & " : " &
                str1 &= err(i)(1) & vbCr
            Next
            displayObj.Text = str
            displayObj.Visible = True
            ShowToast(Me.Page, ToastType.Info, str1, "Check Following", ToastPosition.TopStretch, False)
            ShowMessage(str1)
        Else
            displayObj.Visible = False
            displayObj.Text = ""
        End If

        Return rslt
    End Function

    Public Function GridHasRows(ByVal Grid As GridView) As Boolean
        Dim rslt As Boolean = True
        If Grid.Rows.Count <= 0 Then
            ShowMessage("No Record(s) Found!")
            ShowToast(Me.Page, ToastType.Info, "No Record(s) Found", "Info.", ToastPosition.TopStretch, True)
            rslt = False
        End If
        Return rslt
    End Function

    Public Enum ToastType
        Success
        Info
        Warning
        [Error] 'Reserved word so we use []
    End Enum

    Public Enum ToastPosition
        TopRight
        TopLeft
        TopCenter
        TopStretch
        BottomRight
        BottomLeft
        BottomCenter
        BottomStretch
    End Enum

    Public Shared Sub ShowToast(Page As Page, Type As ToastType, Msg As String,
Optional Title As String = "", Optional Position As ToastPosition = ToastPosition.TopCenter,
Optional ShowCloseButton As Boolean = True)

        Dim strType = "", strPosition = ""

        Select Case Type
            Case ToastType.Success
                strType = "success"
            Case ToastType.Info
                strType = "info"
            Case ToastType.Warning
                strType = "warning"
            Case ToastType.Error
                strType = "error"
        End Select

        'Set the position based on selected and change value to match toastr plug in
        Select Case Position
            Case ToastPosition.TopRight
                strPosition = "toast-top-right"
            Case ToastPosition.TopLeft
                strPosition = "toast-top-left"
            Case ToastPosition.TopCenter
                strPosition = "toast-top-center"
            Case ToastPosition.TopStretch
                strPosition = "toast-top-full-width"
            Case ToastPosition.BottomRight
                strPosition = "toast-bottom-right"
            Case ToastPosition.BottomLeft
                strPosition = "toast-bottom-left"
            Case ToastPosition.BottomCenter
                strPosition = "toast-bottom-center"
            Case ToastPosition.BottomStretch
                strPosition = "toast-bottom-full-width"
        End Select

        Dim script = "toastify('" & strType & "', '" & CleanStr(Msg) & "', '" & CleanStr(Title) & "', '" & strPosition & "', '" & ShowCloseButton & "');"
        Page.ClientScript.RegisterStartupScript(GetType(Page), "toastedMsg", script, True)

    End Sub

    Private Shared Function CleanStr(Text As String) As String
        'This function replaces ' with its html code equivalent
        'in order not to terminate the js statement string
        Return Text.Replace("'", "&#39;")
    End Function
    'Public Shared Sub ShowToastr(page As Page, message As String, title As String, Optional type As String = "info")
    '    page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", [String].Format("toastr.{0}('{1}', '{2}');", type.ToLower(), message, title), addScriptTags:=True)
    'End Sub

    Public Sub ColorGrid(ByVal obj As GridView)
        Dim i As Integer = 0
        For Each dgRw As GridViewRow In obj.Rows
            If i = 2 Then
                dgRw.BackColor = Drawing.Color.AliceBlue
            ElseIf i = 5 Then
                dgRw.BackColor = Drawing.Color.WhiteSmoke
                i = -1
            Else
                dgRw.BackColor = Drawing.ColorTranslator.FromHtml("#FFFFFF")
            End If
            i += 1
        Next
    End Sub

    Public Function GetGridViewRowRecordId(ByVal sender As Object) As Integer

        Dim gr As GridViewRow = DirectCast(sender.NamingContainer, GridViewRow)
        Dim recId As Integer = Convert.ToInt32(sender.DataKeys(gr.RowIndex).Value.ToString())
        Return recId

    End Function
    Public Sub RedirectToCallPage()
        Response.Redirect(GetRequestParameter("GoBackTo").ToString, True)
    End Sub
    Public Sub GoToHomePage()
        Response.Redirect("~/Default.aspx", True)
    End Sub


    Public Sub Redirect_To_Path_With_QueryString(ByVal goToPage As String, ByVal goBackToPage As String, queryParamHash As Dictionary(Of String, String), Optional ByVal doEncrypt As Boolean = True)
        queryParamHash.Add("GoBackTo", $"~/{goBackToPage}.aspx")
        Response.Redirect($"~/{goToPage}.aspx" & CreateQueryString(queryParamHash), doEncrypt)
    End Sub


    Public Sub GoTo_GoBackTo_Page()
        Dim _page = GetRequestParameter("GoBackTo")
        Response.Redirect($"{_page}")
    End Sub

    Public Function CreateQueryString(paraHash As Dictionary(Of String, String), Optional ByVal doEncrypt As Boolean = True) As String
        Dim rtnStr As String = Nothing
        For i = 0 To paraHash.Count - 1
            If IsNothing(rtnStr) Then
                rtnStr = "?" & paraHash.ElementAt(i).Key & "=" & IIf(doEncrypt, Encrypt(paraHash.ElementAt(i).Value), paraHash.ElementAt(i).Value)
            Else
                rtnStr = rtnStr & "&" & paraHash.ElementAt(i).Key & "=" & IIf(doEncrypt, Encrypt(paraHash.ElementAt(i).Value), paraHash.ElementAt(i).Value)
            End If
        Next

        Return rtnStr
    End Function

    Public Function GetRequestParameter(ByVal pkey As String, Optional ByVal doDecrypt As Boolean = True) As Object
        If Not Request.QueryString.AllKeys.Contains(pkey) Then Return Nothing
        If doDecrypt Then
            Return Decrypt(Request.QueryString(pkey.ToString))
        Else
            Return Request.QueryString(pkey.ToString)
        End If
    End Function

    Public Function Get_All_Request_Parameters(Optional ByVal doDecrypt As Boolean = True) As NameValueCollection
        Return Request.QueryString
    End Function


    Protected Sub gridView_OnPageIndexChanging(sender As Object, e As GridViewPageEventArgs)

        Dim myGrid As GridView = TryCast(sender, GridView)
        myGrid.PageIndex = e.NewPageIndex

    End Sub


    Public Sub Init_AfterGridViewBind(pg As LinkPager, gv As GridView, totRecs As Integer)
        pg.TotalRecords = totRecs
        pg.TotalPages = IIf((totRecs Mod gv.PageSize) = 0, Convert.ToInt32(totRecs / gv.PageSize), (IIf(Convert.ToInt32(totRecs / gv.PageSize) = 0, 1, Convert.ToInt32(totRecs / gv.PageSize))))
        pg.RefreshPager()
    End Sub

#End Region

    Public Sub CloseDialogWindow(Optional ByVal IsSubmitted As Boolean = False)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "ClosePage", "$(document).ready(function(){refreshParent();return true;});", True)
    End Sub

    Public Sub ShowDialogWindow(ByVal path As String, ByVal height As Integer, ByVal width As Integer, Optional ByVal Title As String = Nothing, Optional ByVal refreshParent As Boolean = True)

        Dim buildScript As New StringBuilder

        buildScript.Append("var url = base64encode(applicationPath + '" & path & "');")
        buildScript.AppendLine("window.showModalDialog(applicationPath + '/Resources/Dialog.aspx?url=' + url, '', 'dialogHeight:" & height & "px;dialogWidth:" & width & "px;center:yes;resizable:no;scroll:no;status:no;help:no');")

        Dim sr As String = "$(document).ready(function(){" & buildScript.ToString() & ";return true;});"

        ScriptManager.RegisterStartupScript(Me, GetType(Page), Title.ToString, sr, True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "parent.window.document.title='" & Title & "';", True)
        If refreshParent Then ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "refreshParent();", True)

    End Sub

    Public Sub ShowPrintPreview(Optional ByVal Title As String = "Print Preview")
        'Dim s As String = "window.open('" & url + "', 'popup_window', 'width=880,height=1300,left=100,top=100,resizable=yes');"
        'ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)
        'ScriptManager.RegisterStartupScript(Me, GetType(Page), "MessagePopUp", "window.location.href = applicationPath + '/Reports/PrintPreview.aspx';", True)
        'Call OpenWindow()
        Call ShowDialogWindow("/Reports/PrintPreview.aspx", 1300, 880, Title, False)
    End Sub


    Public Function ConvertToJsonFormat(ByVal Obj) As String
        Dim cnvt As New JavaScriptSerializer()
        Return cnvt.Serialize(Obj)
    End Function

    Public Sub CallJavaFunction(ByVal funcNameWithParam As String, cNme As String)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), cNme, funcNameWithParam, True)
        'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "showDisplay();", True)
    End Sub


    Public Function HasPermision(ByVal permVal As String, ByVal perm As String, Optional ByVal showPrompt As Boolean = True) As Boolean

        'Return True

        Dim AppPart = _ApplicationAliase & "." & permVal

        Dim rslt = IsAuthorized(Me.CurrentUser, AppPart, perm)
        If rslt = False Then
            If showPrompt Then
                ShowMessage("Access Denied!")
            End If
        End If
        Return rslt
    End Function


    Public Function WriteLog(ByVal logMsg As String) As Boolean  'Me.ResManager.GetString("Log.LogonSuccessful", Me.CurrentCultureInfo)
        'write into auditing log  Me.User.ObjectID
        Me.Log(CurrentUser().ObjectID, Me.GetResourceID(Me.ObjectAlias), logMsg, "frits")
        Return True
    End Function





    '/------------------------REPORT PRINTING BEGINS ---------------------------/




    Public Function TranslateStringToCR(ByVal strVal As String) As String
        'Dim rtnStr As String = "'"
        'Dim i As Integer = 0

        'If strVal Is Nothing Then
        '    Return Nothing
        'End If

        'Try
        '    For Each subStr As String In strVal.Split(Chr(10))

        '        subStr = subStr.Trim(vbCrLf.ToCharArray)

        '        If i > 0 Then
        '            rtnStr &= "' & Chr(10) & Chr(13) & '"
        '        End If
        '        i += 1

        '        'subStr = subStr.Replace("'", "' & Chr(39) & '")
        '        rtnStr &= subStr

        '    Next

        '    rtnStr = rtnStr & "'"

        '    Return rtnStr

        'Catch ex As Exception
        '    Return Nothing
        'End Try





        Dim Returnstring As String = "'"

        'Split the string at every LF
        For Each SubString As String In strVal.Split(Chr(10))

            'Trim all the CR / LF characters
            SubString = SubString.Trim(vbCr.ToCharArray)
            ' SubString = SubString.Trim(vbCrLf.ToCharArray)
            SubString = SubString.Replace("'", "' & Chr(39) & '")
            SubString = SubString.Replace(";", "' & Chr(59) & '")
            SubString = SubString.Replace(":", "' & Chr(58) & '")
            SubString = SubString.Replace("@", "' & Chr(64) & '")
            SubString = SubString.Replace("*", "' & Chr(42) & '")
            SubString = SubString.Replace("-", "' & Chr(45) & '")

            Returnstring = Returnstring & SubString

        Next

        Returnstring = Returnstring & "'"

        Return Returnstring
    End Function

    Protected Sub OpenWindow()
        Dim url As String = "/Reports/PrintPreview.aspx"
        Dim s As String = "window.open(applicationPath + '" & url + "', 'FRITS', 'width=900,height=1300,center=yes,resizable=yes,scroll=yes,status=no,help=no');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", s, True)
    End Sub


    Protected Shared reportQueue As New Queue()
    Protected Shared Function CreateReport(reportClass As Type) As ReportDocument
        Dim report As Object = Activator.CreateInstance(reportClass)
        reportQueue.Enqueue(report)
        Return DirectCast(report, ReportDocument)
    End Function

    Public Shared Function GetReport(reportClass As Type) As ReportDocument
        '75 is my print job limit.
        If reportQueue.Count >= 75 Then
            DirectCast(reportQueue.Dequeue(), ReportDocument).Dispose()
        End If
        Return CreateReport(reportClass)
    End Function


    Dim rptDoc As ReportDocument
    Public Function LoadReportFile(ByVal rptFileName As String, ByVal rptData As DataTable, Optional ByVal AutoPrint As Boolean = False, Optional ByVal printCopies As Integer = 1) As Boolean
        rptDoc = GetReport(GetType(ReportDocument))
        rptDoc.Load(Server.MapPath("~/Reports/" & rptFileName))
        Session.Add("reportDoc", rptDoc)
        Session.Add("reportData", rptData)
        Session.Add("AutoPrint", AutoPrint)
        Session.Add("printCopies", printCopies)
        Return True
    End Function

    '/------------------------REPORT PRINTING ENDS ----------------------------/

    'Public Function GetBranchList(Optional ByVal includeAllBranhes = True) As IEnumerable(Of Branch)
    '    Dim branchRepo As New BranchRepository(_context)
    '    Dim rslt = branchRepo.GetRecords.ToList()
    '    If Not Me.ViewAllBranches Then
    '        rslt = rslt.Where(Function(o) o.BranchId = _userBranch).ToList()
    '    End If

    '    If includeAllBranhes Then
    '        rslt.Insert(0, New Branch() With {.BranchName = "ALL BRANCHES", .BranchID = "0", .BranchMnemonic = "ALB"})
    '    End If
    '    Return rslt
    'End Function


    Public Shared Function ConvertToDataTable(Of T)(ByVal list As IList(Of T), Optional ByVal doProps As Boolean = False) As DataTable
        Dim table As New DataTable()
        Dim fields() As FieldInfo = GetType(T).GetFields()
        Dim props() As PropertyInfo = GetType(T).GetProperties()

        For Each field As FieldInfo In fields
            table.Columns.Add(field.Name, field.FieldType)
        Next
        If (doProps) Then
            For Each p As PropertyInfo In props
                table.Columns.Add(p.Name, p.PropertyType)
            Next
        End If

        For Each item As T In list
            Dim row As DataRow = table.NewRow()
            For Each field As FieldInfo In fields
                row(field.Name) = field.GetValue(item)
            Next
            table.Rows.Add(row)
        Next

        If (doProps) Then
            Dim iCount As Integer = 0
            For Each item As T In list
                Dim row As DataRow = table.Rows(iCount)
                For Each p As PropertyInfo In props
                    row.Item(p.Name) = p.GetValue(item, Nothing)
                Next
                'table.Rows.Add(row)
                iCount += 1
            Next
        End If


        Return table
    End Function

    Public Enum ShowListType
        AuthorisedList = 0
        UnAuthorisedList = 1
    End Enum

    Enum MultiViewOption
        EntityListView = 0
        AddEditEntityView = 1
    End Enum

    Public Overridable Sub NavigateViews(ByRef mlvControl As MultiView, ByVal viewIndex As MultiViewOption, Optional ByVal sessionVariable As String = "")
        mlvControl.ActiveViewIndex = viewIndex
        If Not String.IsNullOrWhiteSpace(sessionVariable) Then Session.Add(sessionVariable, viewIndex)
    End Sub


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









End Class
