Imports System.Drawing
Imports AjaxControlToolkit

Public Class ucAsyncFileUploadControl
    Inherits UserControlBase

    Private saveFileAs As String
    Public Property _saveFileAs() As String
        Get
            saveFileAs = DirectCast(Session.Item("_saveFileAs"), String)
            Return saveFileAs
        End Get
        Set(ByVal value As String)
            saveFileAs = value
            Session.Add("_saveFileAs", saveFileAs)
        End Set
    End Property


    Private fileBytes As Byte()
    Public Property _fileBytes As Byte()
        Get
            fileBytes = DirectCast(Session.Item("_fileBytes"), Byte())
            Return fileBytes
        End Get
        Set(ByVal value As Byte())
            fileBytes = value
            Session.Add("_fileBytes", fileBytes)
        End Set
    End Property


    Private Property _fileContent As Stream



    Private fileContentType As String
    Public Property _fileContentType As String
        Get
            fileContentType = DirectCast(Session.Item("_fileContentType"), String)
            Return fileContentType
        End Get
        Set(ByVal value As String)
            fileContentType = value
            Session.Add("_fileContentType", fileContentType)
        End Set
    End Property




    Private fileExtension As String
    Public Property _fileExtension As String
        Get
            fileExtension = DirectCast(Session.Item("_fileExtension"), String)
            Return fileExtension
        End Get
        Set(ByVal value As String)
            fileExtension = value
            Session.Add("_fileExtension", fileExtension)
        End Set
    End Property




    Private _uploadFileName As String
    Public Property UploadFileName() As String
        Get
            Return txtFileDescription.Text
        End Get
        Set(ByVal value As String)
            txtFileDescription.Text = value
            'Session.Add("_uploadFileName", _uploadFileName)
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

    Public _reviewFileType As ReviewFileTypes
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






    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMesg.Text = String.Empty
    End Sub




    Protected Sub FileUploadComplete(ByVal sender As Object, ByVal e As AsyncFileUploadEventArgs)

        If Integer.Parse(e.FileSize) > 0 Then
            _saveFileAs = $"{UploadFileName}-{Guid.NewGuid()}"
            _fileBytes = fuUploadAsync.FileBytes()
            _fileContent = fuUploadAsync.FileContent
            _fileContentType = fuUploadAsync.ContentType
            _fileExtension = fuUploadAsync.FileName.Split(".")(1)
            btnUploadFile.Enabled = True
        End If
    End Sub

    Protected Sub btnUploadFile_Click(sender As Object, e As EventArgs)

        If Not Directory.Exists(Path.Combine(UploadPath)) Then
            Directory.CreateDirectory(UploadPath)
        End If

        Dim _fileName = Path.Combine(UploadPath, _saveFileAs)

        File.WriteAllBytes($"{_fileName}.{_fileExtension}", _fileBytes)

        Dim _uploadFile = New ReviewFileVm() With {
            .Description = UploadFileName,
            .FileExtension = _fileExtension,
            .FileName = _saveFileAs,
            .ReferenceTypeId = ReferenceFileTypeId,
            .ReviewFileType = ReviewFileType
        }

        Dim _result = _itService.Save_File_Upload_Info(_uploadFile)

        If _result.Status Then
            lblMesg.Text = "File Successfully Uploaded To Server!"
            lblMesg.BackColor = Color.Green
            lblMesg.ForeColor = Color.White
            txtFileDescription.Text = String.Empty
            ShowSuccessMessage(_result.Message)
        Else
            lblMesg.Text = "File Upload To Server Filed!"
            lblMesg.BackColor = Color.Red
            lblMesg.ForeColor = Color.White
            ShowErrorMessage(_result.Message)
        End If

    End Sub


End Class