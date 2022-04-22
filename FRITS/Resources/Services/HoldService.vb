Public Class HoldService

    Private ReadOnly _holdBag As Dictionary(Of String, Object)
    Private Shared __instance As HoldService
    Sub New()
        _holdBag = New Dictionary(Of String, Object)
    End Sub

    Public Sub Clear()
        _holdBag.Clear()
    End Sub

    Public Sub AddRenewItem(ByVal key As String, ByVal item As Object)
        If _holdBag.Keys.Contains(key) Then _holdBag.Remove(key)
        _holdBag.Add(key, item)
    End Sub

    Public Function HasKey(ByVal key As String)
        Return _holdBag.Keys.Contains(key)
    End Function

    Public Sub Remove(ByVal key As String)
        _holdBag.Remove(key)
    End Sub

    Public Function GetItem(Of T)(ByVal key As String, Optional ByVal remove As Boolean = True) As T
        If _holdBag.Keys.Contains(key) Then
            Dim obj As Object = _holdBag(key)
            If remove Then _holdBag.Remove(key)
            If Not obj Is Nothing Then
                Return CType(obj, T)
            Else
                Return Nothing
            End If
        End If
        Return Nothing
    End Function


    Public Function ReadItem(Of T)(ByVal key As String) As T
        Return GetItem(Of T)(key, False)
    End Function

    Public Function IsDirty() As Boolean
        Return _holdBag.Keys.Count > 0
    End Function

    Public Sub RemoveAllWithKeyContaining(ByVal key As String)
        Dim _keyList = _holdBag.Where(Function(x) x.Key.ToUpper().StartsWith($"{key.ToUpper()}_")).ToList()
        For Each itm In _keyList
            Remove(itm.Key)
        Next
    End Sub

    Public Function Branch_Hold_Records(ByVal branchCode As String) As List(Of KeyValuePair(Of String, Object))
        Dim _keyList = _holdBag.Where(Function(x) x.Key.ToUpper().StartsWith($"{branchCode.ToUpper()}_")).ToList()
        Return _keyList
    End Function

    Public Function Module_Hold_Records(ByVal branchCode As String, ByVal viewModelName As String) As List(Of KeyValuePair(Of String, Object))
        Dim _keyList = _holdBag.Where(Function(x) x.Key.ToUpper().StartsWith($"{branchCode.ToUpper()}_") And x.Key.Contains(viewModelName)).ToList()
        Return _keyList
    End Function

    Public Shared Function Instance() As HoldService

        If __instance Is Nothing Then __instance = New HoldService

        Return __instance

    End Function


End Class
