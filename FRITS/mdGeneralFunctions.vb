Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports FRITS.DAL
Imports PortSight.SecureAccess.ARObjects

Module mdGeneralFunctions

    Public loggedOnUserId As Integer
    Public loggedOnUserBranchId As Integer
    Dim _context As New AppDbContext
    Public Function ChangeWordCase(ByVal str As String, Optional ByVal ToLower_1_ToUpper_2_ToTitle_3 As Short = 1) As String
        Dim myTI As TextInfo = New CultureInfo("en-US", False).TextInfo

        If ToLower_1_ToUpper_2_ToTitle_3 = 1 Then
            Return myTI.ToLower(str)
        ElseIf ToLower_1_ToUpper_2_ToTitle_3 = 2 Then
            Return myTI.ToUpper(str)
        Else : ToLower_1_ToUpper_2_ToTitle_3 = 3
            Return myTI.ToTitleCase(str)
        End If
    End Function

    Public Function Calc_Depreciation(ByVal Amt As Decimal, ByVal pcnt As Decimal, Optional ByVal period As Integer = 12)

        Return (Amt / period) '(Amt * ((pcnt * 0.01) / period))

    End Function





    Public Function FormatAmount(amt As Decimal, Optional decPls As Integer = 4) As Decimal
        Return FormatNumber(amt, decPls)
    End Function



    Public Function FormatDate4Display(ByVal strDate As String) As String
        Return IIf(IsDate(CDate(strDate)), Format(CDate(strDate), "ddd, dd-MMM-yyyy"), Nothing)
    End Function

    Public Function FormatNumber4Display(ByVal val As String, Optional decimalPlace As Integer = 2) As Decimal
        Return IIf(IsNumeric(CDec(val)), FormatNumber(CDec(val), decimalPlace, TriState.True, TriState.True), FormatNumber(0, decimalPlace, TriState.True, TriState.True))
    End Function
    Public Function DisplayAsCurrency(ByVal val As Decimal) As String
        Dim rslt As String = Nothing
        If val < 0 Then
            rslt = "(GH¢ " & val * -1 & "p)"
        Else
            rslt = "GH¢ " & val & "p"
        End If
        Return rslt
    End Function


    Public Function Encrypt(clearText As String) As String
        Dim EncryptionKey As String = "EAF6B7EC25DC0FFBDB7B1D49DB32EE921B3DDA8265DF78361ECE503AD4C579FA"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function


    Public Function Decrypt(cipherText As String) As String
        Dim EncryptionKey As String = "EAF6B7EC25DC0FFBDB7B1D49DB32EE921B3DDA8265DF78361ECE503AD4C579FA"
        cipherText = cipherText.Replace(" ", "+")
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
             &H65, &H64, &H76, &H65, &H64, &H65,
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function



    Public Function ShowUserName(ByVal userId As String) As String
        If userId = Nothing Or userId = "0" Then Return ""
        Dim arCon As New ARConnection
        arCon.ConnectToCatalog()
        Dim tt As New ARObject
        tt = arCon.GetUserByID(userId)

        Dim rslt = tt.ObjectName
        arCon.Close()

        Return rslt.ToString
    End Function




    'Public Function GetUserBranchId(ByVal brnchCode As String) As Integer
    '    Dim bRepo As New BranchRepository(_context)
    '    Return bRepo.GetBranchId(brnchCode)
    'End Function




    Private Function ExpectedDepreciationRuns(ByVal depStartDate As Date, ByVal depEndDate As Date) As Integer
        Return DateDiff(DateInterval.Month, CDate(depStartDate), CDate(depEndDate))
    End Function


    Public Function RandomString(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next

        Return sResult
    End Function


    Public Function RequestStatus() As DataTable
        Dim dt As New DataTable
        dt.TableName = "requestStatus"
        dt.Columns.Add("Id", GetType(System.Int32))
        dt.Columns.Add("StatusDesc", GetType(System.String))

        dt.Rows.Add(0, "Pending Confirmation")
        dt.Rows.Add(1, "Confirmed, Pending Approval")
        dt.Rows.Add(2, "Approved")
        Return dt
    End Function


End Module
