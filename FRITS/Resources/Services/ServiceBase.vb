Imports System.Data.Entity
Imports FRITS.DAL
Imports Newtonsoft.Json

Public Class ServiceBase

    Public _context As AppDbContext
    Public Property _currentUserBranchName As String
    Public Property _currentUserBranchCode As String
    Public Property _currentUserId As Integer




    Private Function ReadAppSetting(ByVal key As String)
        Return ConfigurationManager.AppSettings(key)
    End Function


    Public Sub Add(Of T As Class)(ByRef entity As T)
        Dim dbSet As DbSet(Of T)
        dbSet = _context.Set(Of T)()
        dbSet.Add(entity)
        _context.SaveChanges()
    End Sub

    Public Sub AddRang(Of T As Class)(ByRef entity As T)
        Dim dbSet As DbSet(Of T)
        dbSet = _context.Set(Of T)()
        dbSet.AddRange(entity)
        _context.SaveChanges()
    End Sub

    Public Sub Remove(Of T As Class)(ByRef entity As T)
        Dim dbSet As DbSet(Of T)
        dbSet = _context.Set(Of T)()
        If (_context.Entry(entity).State = EntityState.Detached) Then
            dbSet.Attach(entity)
        End If
        dbSet.Remove(entity)
        _context.SaveChanges()
    End Sub

    Public Sub RemoveList(Of T As Class)(ByRef entityList As IEnumerable(Of T))
        Dim dbSet As DbSet(Of T)
        dbSet = _context.Set(Of T)()
        dbSet.RemoveRange(entityList)
        _context.SaveChanges()
    End Sub

    Public Sub Update(Of T As Class)(ByRef entity As T)
        Dim dbSet As DbSet(Of T)
        dbSet = _context.Set(Of T)()
        dbSet.Attach(entity)
        _context.Entry(entity).State = EntityState.Modified
        _context.SaveChanges()
    End Sub


    Private ReadOnly Property _workingDay As DateTime
        Get
            Return DateTime.UtcNow()
        End Get

    End Property


    Public Sub Write_Exception_Log(ByRef exception As Exception, ByVal logName As String)
        Try

            Dim _format As String = "ddMMyyyy_HHmmss"
            Dim _fileName As String = $"{logName}_{DateTime.Now.ToString(_format)}.txt"
            Dim _dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "err_logs")
            If Not Directory.Exists(_dir) Then
                Directory.CreateDirectory(_dir)
            End If
            Dim _errLog As String = Path.Combine(_dir, _fileName)
            Dim err = New With {Key .Msg = exception.Message, .InnerExcept = exception.InnerException.ToString(), .StackTrace = exception.InnerException.StackTrace}
            File.WriteAllText(_errLog, JsonConvert.SerializeObject(err))
        Catch ex As Exception
            File.WriteAllText($"{logName}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.txt", exception.InnerException.ToString)
        End Try
    End Sub


    Private ReadOnly Property SQLDB_Connection(ByVal connStringName) As SqlConnection
        Get
            Dim _connectionString As String = ConfigurationManager.ConnectionStrings(connStringName).ConnectionString
            Return New SqlConnection(_connectionString)
        End Get
    End Property



    'Private _secureAccessConn As SqlConnection
    Private ReadOnly Property SecureAccess_Connection As SqlConnection
        Get
            '_secureAccessConn = DirectCast(Session.Item(NameOf(_secureAccessConn).ToString()), SqlConnection)
            'If IsNothing(_secureAccessConn) Then
            '_secureAccessConn = SQLDB_Connection("SecureAccessConnectionString")
            Return SQLDB_Connection("SecureAccessConnectionString")
            'Session.Add(NameOf(_secureAccessConn).ToString(), _secureAccessConn)
            'End If
            'Return _secureAccessConn
        End Get
    End Property


    Private Function Excecute_Raw_SqlCommand(ByVal strQuery As String) As Boolean
        Dim _sqlConn As SqlConnection = SecureAccess_Connection
        Try
            _sqlConn.Open()
            Dim _sqlCommand As New SqlCommand(strQuery, _sqlConn)
            _sqlCommand.CommandType = CommandType.Text
            _sqlCommand.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            _sqlConn.Close()
            _sqlConn.Dispose()
        End Try
        Return True
    End Function


    Public Function Get_EmployeeList(Optional ByVal userName As String = "") As List(Of EmployeeVm)

        Return Get_All_Users(userName).AsEnumerable().Select(Of EmployeeVm)(Function(s) New EmployeeVm With {
          .EmployeeId = s.Field(Of Integer)("ObjectID"),
          .EmployeeName = s.Field(Of String)("ObjectName"),
          .Username = s.Field(Of String)("Loginname"),
          .EmailAddress = s.Field(Of String)("EmailAddress")
        }).ToList()

    End Function
    Public Function Get_All_Users(Optional ByVal userName As String = "") As DataTable
        Dim _sqlConn As SqlConnection = SecureAccess_Connection
        Dim _results As New DataTable
        Try
            Dim _command = $"SELECT [ObjectID] ,[ObjectName] ,[Loginname] ,[EmailAddress] ,[ObjectDescription] ,[IsLocked] ,[OwnerID] FROM [SecureAccess].[dbo].[View_AR_User_List]"

            If Not String.IsNullOrWhiteSpace(userName) Then
                _command &= $" Where Loginname = '{userName}'"
            End If
            Dim _dtAdapter As New SqlDataAdapter(_command, _sqlConn)
            _dtAdapter.Fill(_results)
            _dtAdapter.Dispose()
        Catch ex As Exception
            ex.Data.Clear()
        Finally
            _sqlConn.Close()
            _sqlConn.Dispose()
        End Try
        Return _results
    End Function

    Public Function Get_Users_In_Group(ByVal groupAliase As String) As DataTable
        Dim _sqlConn As SqlConnection = SecureAccess_Connection
        Dim _results As New DataTable
        Try
            Dim _command = $"SELECT ChildObjectID, ChildObjectName, ChildObjectAlias FROM [SecureAccess].[dbo].[View_AR_Relationship_Joined] WHERE ChildObjectTypeNamespace = 'ARObject.AROperator.ARUser'"

            If Not String.IsNullOrWhiteSpace(groupAliase) Then
                _command &= $" AND ParentObjectAlias = '{groupAliase}'"
            End If
            Dim _dtAdapter As New SqlDataAdapter(_command, _sqlConn)
            _dtAdapter.Fill(_results)
            _dtAdapter.Dispose()
        Catch ex As Exception
            ex.Data.Clear()
        Finally
            _sqlConn.Close()
            _sqlConn.Dispose()
        End Try
        Return _results
    End Function


    Public Function Get_UserGroups() As DataTable
        Dim _sqlConn As SqlConnection = SecureAccess_Connection
        Dim _results As New DataTable
        Try
            Dim _command = $"SELECT ObjectID, ObjectName, ObjectDescription, ObjectAlias FROM [SecureAccess].[dbo].[View_AR_UserGroup_List]"
            Dim _dtAdapter As New SqlDataAdapter(_command, _sqlConn)
            _dtAdapter.Fill(_results)
            _dtAdapter.Dispose()
        Catch ex As Exception
            ex.Data.Clear()
        Finally
            _sqlConn.Close()
            _sqlConn.Dispose()
        End Try
        Return _results
    End Function

    Public Function Get_User_In_Branch(ByVal branchCode As String) As DataTable
        Dim _sqlConn As SqlConnection = SecureAccess_Connection
        Dim _results As New DataTable
        Try
            Dim _command = $"Select u.ObjectID, u.ObjectName, u.EmailAddress, u.ObjectDescription, p.PropertyValue FROM dbo.View_AR_User_List u join dbo.AR_PropertyValue p on u.ObjectID = p.ObjectID where p.PropertyValue = '{branchCode}'"

            Dim _dtAdapter As New SqlDataAdapter(_command, _sqlConn)
            _dtAdapter.Fill(_results)
            _dtAdapter.Dispose()
        Catch ex As Exception
            ex.Data.Clear()
        Finally
            _sqlConn.Close()
            _sqlConn.Dispose()
        End Try
        Return _results
    End Function

End Class
