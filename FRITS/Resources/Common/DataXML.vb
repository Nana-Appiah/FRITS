Imports System.Xml
Imports System.IO

Public Class DataXML

    Private _body As String = ""
    Private _arr, _arrData As ArrayList

    Public Sub New()
        _arr = New ArrayList
        _arrData = New ArrayList
    End Sub

    Public Sub Add(ByVal value As String)
        _arr.Add(value)
    End Sub

    Public Sub BuildXML()

        _body = "<?xml version=""1.0"" encoding=""utf-8""?>" & vbCrLf
        _body &= "   <row>" & vbCrLf

        For Each item In _arr
            _body &= "     <col>" & item & "</col>" & vbCrLf
        Next

        _body &= "   </row>"

    End Sub

    Public Function GetXML() As String
        Return _body
    End Function

    Public Sub BuildData(ByVal data As String)

        Dim ds As New DataSet()

        Dim reader As New XmlTextReader(New StringReader(data))
        ds.ReadXml(reader)

        If ds.Tables.Count > 0 Then
            If ds.Tables(0).Rows.Count > 0 Then
                For Each rw In ds.Tables(0).Rows
                    _arrData.Add(rw(0).ToString)
                Next
            End If
        End If

    End Sub

    Public Function GetData(ByVal index As Integer) As String
        Return _arrData(index).ToString
    End Function

End Class
