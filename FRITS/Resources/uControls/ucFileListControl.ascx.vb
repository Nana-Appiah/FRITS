Imports System.IO
Imports System.Net.Mime
Imports System.Threading.Tasks

Public Class ucFileListControl
    Inherits UserControlBase

    Private Property _fileList As List(Of ReviewFileVm)
    Public Property FileList() As List(Of ReviewFileVm)
        Get
            _fileList = DirectCast(Session.Item("_fileList"), List(Of ReviewFileVm))
            Return _fileList
        End Get
        Set(ByVal value As List(Of ReviewFileVm))
            _fileList = value
            Session.Add("_fileList", _fileList)
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(dtlFileList)
    End Sub
    Private _referenceFileTypeId As Guid
    Public Property ReferenceFileTypeId() As Guid
        Get
            _referenceFileTypeId = DirectCast(Session.Item("_referenceFileTypeId"), Guid)
            Return _referenceFileTypeId
        End Get
        Set(ByVal value As Guid)
            _referenceFileTypeId = value
            Session.Add("_referenceFileTypeId", _referenceFileTypeId)
        End Set
    End Property

    Private _reviewFileType As ReviewFileTypes
    Public Property ReviewFileType() As ReviewFileTypes
        Get
            _reviewFileType = DirectCast(Session.Item("_reviewFileType"), ReviewFileTypes)
            Return _reviewFileType
        End Get
        Set(ByVal value As ReviewFileTypes)
            _reviewFileType = value
            Session.Add("_reviewFileType", _reviewFileType)
        End Set
    End Property

    Private _uploadPath As String
    Private Property UploadPath() As String
        Get
            _uploadPath = DirectCast(Session.Item("_uploadPath"), String)
            If String.IsNullOrWhiteSpace(_uploadPath) Then
                _uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings.Item("ReviewFileUploadPath").ToString()
                Session.Add("_uploadPath", _uploadPath)
            End If
            Return _uploadPath
        End Get
        Set(ByVal value As String)
            _uploadPath = value
            Session.Add("_uploadPath", _uploadPath)
        End Set
    End Property


    Public Sub Show_Files()

        If Not Directory.Exists(Path.Combine(UploadPath)) Then
            Directory.CreateDirectory(UploadPath)
        End If
        dtlFileList.DataSource = FileList
        dtlFileList.DataBind()
    End Sub



    Protected Sub DeleteFile(ByVal reviewFileId As Guid)

        Dim _file = _itService.Get_Uploaded_File_By_ReviewFileId(reviewFileId)

        If IsNothing(_file) Then
            ShowErrorMessage("File Not Found!")
            Return
        End If

        If Not Directory.Exists(Path.Combine(UploadPath)) Then
            Directory.CreateDirectory(UploadPath)
            Return
        End If

        File.Delete(Path.Combine(UploadPath, _file.GetFileName()))
        Call _itService.Delete_ReviewFile_Async(reviewFileId)

        Call Show_Files()

    End Sub

    Protected Sub DownloadFile(ByVal reviewFileId As Guid)

        Dim _file = _itService.Get_Uploaded_File_By_ReviewFileId(reviewFileId)

        If IsNothing(_file) Then
            ShowErrorMessage("File Not Found!")
            Return
        End If

        Dim filePath As String = Path.Combine(UploadPath, _file.GetFileName())

        If File.Exists(filePath) Then

            Dim oStream As System.IO.Stream = Nothing

            Try
                oStream = New System.IO.FileStream(path:=filePath, mode:=System.IO.FileMode.Open, share:=System.IO.FileShare.Read, access:=System.IO.FileAccess.Read)
                Response.Buffer = False
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", "attachment; filename=" & _file.GetSimpleFileName())
                Dim lngFileLength As Long = oStream.Length
                Response.AddHeader("Content-Length", lngFileLength.ToString())
                Dim lngDataToRead As Long = lngFileLength

                While lngDataToRead > 0

                    If Response.IsClientConnected Then
                        Dim intBufferSize As Integer = 8 * 1024
                        Dim bytBuffers As Byte() = New System.Byte(intBufferSize - 1) {}
                        Dim intTheBytesThatReallyHasBeenReadFromTheStream As Integer = oStream.Read(buffer:=bytBuffers, offset:=0, count:=intBufferSize)
                        Response.OutputStream.Write(buffer:=bytBuffers, offset:=0, count:=intTheBytesThatReallyHasBeenReadFromTheStream)
                        Response.Flush()
                        lngDataToRead = lngDataToRead - intTheBytesThatReallyHasBeenReadFromTheStream
                    Else
                        lngDataToRead = -1
                    End If
                End While

            Catch
            Finally

                If oStream IsNot Nothing Then
                    oStream.Close()
                    oStream.Dispose()
                    oStream = Nothing
                End If

                Response.Close()
            End Try

        Else
            ShowMessage("File Not Found!   Please Ask User To Upload File Again!!")
        End If

    End Sub


    Protected Sub dtlFileList_ItemCommand(source As Object, e As DataListCommandEventArgs)

        Dim _recordId = Guid.Parse(dtlFileList.DataKeys(e.Item.ItemIndex).ToString())

        If e.CommandName.Equals("OpenFile") Then

        ElseIf e.CommandName.Equals("DownloadFile") Then
            Call DownloadFile(_recordId)
        ElseIf e.CommandName.Equals("DeleteFile") Then
            Call DeleteFile(_recordId)
        End If
    End Sub
End Class