Imports System.Data.OleDb
Imports System.IO

Module Common

    Const ProductName = "FRITS"

    Public Connection As OleDbConnection
    Public ConnectionString As String = ""
    Public ReadOnly iv() As Byte = {8, 7, 6, 5, 4, 3, 2, 1}
    Public ReadOnly key() As Byte = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}

    Public Function GetDataFromFile(ByVal filepath As String) As DataSet
        Try
            Dim CommandText As String = ""

            'Check for file extension
            Dim fileinfo As FileInfo
            fileinfo = New FileInfo(filepath)

            If fileinfo.Extension.ToLower = ".csv" OrElse fileinfo.Extension.ToLower = ".txt" Then
                Connection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Path.GetDirectoryName(filepath) & "\;Extended Properties='text;HDR=Yes'")
                CommandText = "select * from " & Path.GetFileName(filepath)
            ElseIf fileinfo.Extension.ToLower = ".xls" Then
                Connection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filepath & ";Extended Properties='Excel 8.0;HDR=Yes'")
                CommandText = "select * from [enquiry$]"
            ElseIf fileinfo.Extension.ToLower = ".xlsx" Then
                Connection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filepath & ";Extended Properties='Excel 8.0;HDR=Yes'")
                CommandText = "select * from [enquiry$]"
            End If

            Connection.Open()

            Dim da As OleDbDataAdapter = New OleDbDataAdapter(CommandText, Connection)
            Dim ds As DataSet = New DataSet

            ds.Clear()
            da.Fill(ds)

            Return ds

        Catch ex As Exception
            Throw ex
        Finally
            Connection.Close()
        End Try
    End Function

End Module
