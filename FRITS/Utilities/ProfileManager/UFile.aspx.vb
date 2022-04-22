Partial Public Class UFile
    Inherits PageBase

#Region " Properties"

#End Region

#Region " Methods"

    '## This method imports users from a file.
    Private Sub ImportFile()
        Try
            Dim pPath As String = Me.Server.MapPath("~\Temp\")

            Dim tempfile As String = pPath & "\" & Me.txtFile.FileName

            Try
                Me.txtFile.SaveAs(tempfile)
            Catch ex As Exception
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('ERROR: " & ex.Message & "');", True)
                Exit Sub
            End Try

            Dim fi As New IO.FileInfo(tempfile)

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

                My.Computer.FileSystem.DeleteFile(tempfile)

                'Popup message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('Upload successful');parent.returnValue='reload';", True)

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
            Return Nothing
        End Try
    End Function

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
                'Me.SaveData()
            End If
        End If
    End Sub

    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Me.ImportFile()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Open secure access connection
            Me.ARConn.ConnectToCatalog()

            'Set resource id
            Me.ObjectAlias = "frits.utilities.profilemanager.users"

            'Check access and permission.
            If Not Me.IsAuthorized(Me.CurrentUser, Me.ObjectAlias, "change") Then
                'Display message
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "alert('" & Me.ResManager.GetString("Permissions.AccessDenied.Change", Thread.CurrentThread.CurrentUICulture) & "');", True)
                Me.pnlContentPane.Visible = False
                Exit Sub
            End If

            If Not Me.Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "jsCall", "parent.window.document.title='Import File';", True)
                Me.btnImport.Attributes.Add("onclick", "javascript:" + Me.btnImport.ClientID + ".disabled=true;ShowProcessing();" + ClientScript.GetPostBackEventReference(Me.btnImport, ""))
            End If

        Catch ex As Exception
            Throw ex
        Finally
            Me.ARConn.Close()
        End Try
    End Sub

End Class