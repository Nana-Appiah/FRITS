Imports System.io
Imports System.Security.Cryptography
Imports System.Threading

Public Class TextFileCrypto

    ' Encrypts and decrypts text file given a key

    Private mstrFile As String ' File to decrypt
    Private mstrKey As String ' Key file
    Private mKey(7) As Byte  ' DES key
    Private mDES As DESCryptoServiceProvider
    Private mIV(7) As Byte ' Initialization Vector

    Public Sub New()
        mDES = New DESCryptoServiceProvider
    End Sub

    Public Property KeyFile() As String
        Get
            Return mstrKey
        End Get
        Set(ByVal Value As String)
            If File.Exists(Value) Then
                mstrKey = Value
                OpenKeyFile() ' Open the key file and read its contents
            Else
                Throw New FileNotFoundException(Value & " file does not exist.")
            End If
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return mstrFile
        End Get
        Set(ByVal Value As String)
            If File.Exists(Value) Then
                mstrFile = Value
            Else
                Throw New FileNotFoundException(Value & " does not exist.")
            End If
        End Set
    End Property

    Private Sub OpenKeyFile()
        Dim fsKey As New FileStream(mstrKey, _
          FileMode.Open, FileAccess.Read)

        ' Open the key file and read the key from it
        fsKey.Read(mKey, 0, 8)
        fsKey.Read(mIV, 0, 8)
        fsKey.Close()

        mDES.Key = mKey
        mDES.IV = mIV
    End Sub

    Public Function SaveKeyFile(ByVal FilePath As String) As Boolean
        Dim fsKey As New FileStream(FilePath, _
          FileMode.OpenOrCreate, FileAccess.Write)

        ' Generate a new random key and IV and save it to the file
        ' Note these will be generated randomly automatically
        ' if you don't do it
        mDES.GenerateKey()
        mDES.GenerateIV()

        mKey = mDES.Key
        mIV = mDES.IV

        fsKey.Write(mKey, 0, mKey.Length)
        fsKey.Write(mIV, 0, mKey.Length)
        fsKey.Close()
        mstrKey = FilePath
        Return True
    End Function

    Public Function EncryptFile(ByVal tempfile As String) As Boolean
        ' Encrypt the given file

        ' Check the key
        If mKey Is Nothing Then
            Throw New Exception("You must have a key in place first.")
            Return False
        End If

        Dim fsInput As New FileStream(mstrFile, _
          FileMode.Open, FileAccess.Read)
        Dim fsOutput As New FileStream(tempfile, _
          FileMode.Create, FileAccess.Write)
        fsOutput.SetLength(0)

        Dim arInput() As Byte

        ' Create DES Encryptor from this instance
        Dim desEncrypt As ICryptoTransform = mDES.CreateEncryptor()
        ' Create Crypto Stream that transforms file stream using DES encryption
        Dim sCrypto As New CryptoStream(fsOutput, desEncrypt, _
          CryptoStreamMode.Write)

        ReDim arInput(Convert.ToInt32(fsInput.Length - 1))
        fsInput.Read(arInput, 0, Convert.ToInt32(fsInput.Length))
        fsInput.Close()

        ' Write out DES encrypted file
        sCrypto.Write(arInput, 0, arInput.Length)
        sCrypto.Close()

        ' Delete and rename
        File.Copy(tempfile, mstrFile, True)
        File.Delete(tempfile)
        Return True
    End Function

    Public Function DecryptFile(ByVal tempfile As String) As Boolean
        ' Decrypt the given file

        ' Check the key
        If mKey Is Nothing Then
            Throw New Exception("You must have a key in place first.")
            Return False
        End If

        ' Create file stream to read encrypted file back
        Dim fsRead As New FileStream(mstrFile, FileMode.Open, _
          FileAccess.Read)
        Dim fsOutput As New FileStream(tempfile, _
          FileMode.Create, FileAccess.Write)
        ' Create DES Decryptor from our des instance
        Dim desDecrypt As ICryptoTransform = mDES.CreateDecryptor()
        ' Create crypto stream set to read and do a des decryption 
        ' transform on incoming bytes
        Dim sCrypto As New CryptoStream(fsRead, desDecrypt, _
          CryptoStreamMode.Read)
        Dim swWriter As New StreamWriter(fsOutput)
        Dim srReader As New StreamReader(sCrypto)

        ' Write out the decrypted file
        swWriter.Write(srReader.ReadToEnd)

        ' Close and clean up
        swWriter.Close()
        fsOutput.Close()
        fsRead.Close()

        ' Delete and rename
        WaitForExclusiveAccess(mstrFile)
        File.Copy(tempfile, mstrFile, True)
        File.Delete(tempfile)
        Return True
    End Function

    Private Sub WaitForExclusiveAccess(ByVal fullPath As String)
        While (True)
            Try
                Dim file As FileStream
                file = New FileStream(fullPath, FileMode.Append, _
                  FileAccess.Write, FileShare.None)
                file.Close()
                Exit Sub
            Catch e As Exception
                Thread.Sleep(100)
            End Try
        End While
    End Sub

End Class
