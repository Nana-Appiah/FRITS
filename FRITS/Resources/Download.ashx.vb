Imports System.Web
Imports System.Web.Services

Public Class Download
    Implements System.Web.IHttpHandler

    Sub Process_Request(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim MIMEType As String = Nothing
        Dim FilePath As String = Functions.Decode64(context.Request.QueryString("path"))
        Dim FileName As String = Path.GetFileName(FilePath)
        Dim Extension As String = Path.GetExtension(FilePath).ToLower

        Select Case Extension.ToLower
            Case ".gif"
                MIMEType = "image/gif"
            Case ".jpg", ".jpeg", ".jpe"
                MIMEType = "image/jpeg"
            Case ".png"
                MIMEType = "image/png"
            Case ".rtf", ".rtx"
                MIMEType = "text/richtext"
            Case ".txt"
                MIMEType = "text/plain"
            Case ".zip"
                MIMEType = "application/zip"
            Case ".pdf"
                MIMEType = "application/pdf"
            Case ".doc", ".docx"
                MIMEType = "application/msword"
            Case ".xls", ".xlsx"
                MIMEType = "application/excel"
            Case ".seq"
                MIMEType = "text/plain"
            Case Else
                MIMEType = "application/x-binary"
                Exit Sub
        End Select

        context.Response.ContentType = MIMEType
        context.Response.AppendHeader("Content-Disposition", "attachment; FileName=" + FileName)

        'Get file info
        Dim fi As FileInfo = New IO.FileInfo(FilePath)

        Dim fs As FileStream = Nothing
        Dim br As BinaryReader = Nothing
        Dim strFile(CInt(fi.Length)) As Byte

        Try
            fs = New FileStream(FilePath, FileMode.Open, FileAccess.Read)
            br = New BinaryReader(fs)
            strFile = br.ReadBytes(CInt(fi.Length))
        Finally
            ' make sure objects are closed in case the thread
            ' was aborted in the middle of this method
            If Not (br Is Nothing) Then br.Close()
            If Not (fs Is Nothing) Then fs.Close()
        End Try

        context.Response.BinaryWrite(strFile)

    End Sub

    ReadOnly Property Is_Reusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class