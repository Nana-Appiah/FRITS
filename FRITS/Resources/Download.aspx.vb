Public Partial Class Download
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim FilePath As String = Functions.Decode64(Me.Request.QueryString("path"))
            Dim FileName As String = Path.GetFileName(FilePath)
            Dim Extension As String = Path.GetExtension(FilePath).ToLower
            Dim MIMEType As String = Nothing

            Select Case Extension
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
                Case Else
                    MIMEType = "application/x-binary"
                    'Invalid file type uploaded
                    'MsgBox("Invalid file type to uploaded", MsgBoxStyle.OkOnly, "Invalid file type")
                    Exit Sub
            End Select

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

            Me.Response.ContentType = MIMEType
            Me.Response.AppendHeader("Content-Disposition", "attachment; FileName=" + FilePath)
            Me.Response.BinaryWrite(strFile)
            'Me.Response.Clear()
            Me.Response.End()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class