
Imports System
Imports System.Text
Imports System.Security.Cryptography
Public Class Encryption

    'encrypt / decrypt password
    Public Function HashText(ByVal text As String) As String
        Using md5 = New MD5CryptoServiceProvider()
            Dim bytes = Encoding.UTF8.GetBytes(text)
            Dim hash = md5.ComputeHash(bytes)
            Return Convert.ToBase64String(hash)
        End Using
    End Function
End Class

