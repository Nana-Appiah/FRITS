Imports System.Management

Public Class LicenseManager

    Private _MACAddress As String
    Private _ProcessorId As String
    Private _VolumeSerial As String
    Private _MotherBoardId As String
    Private _ProductVersion As String
    Private _BeginDate As String
    Private _EndDate As String
    Private _CompanyName As String
    Private _CompanyAlias As String
    Private _Activated As Integer
    Private _Error As String
    Private _LicenseType As LicenseType

#Region "Property"

    Public Property MACAddress() As String
        Get
            Return _MACAddress
        End Get
        Set(ByVal Value As String)
            _MACAddress = Value
        End Set
    End Property

    Public Property ProcessorId() As String
        Get
            Return _ProcessorId
        End Get
        Set(ByVal Value As String)
            _ProcessorId = Value
        End Set
    End Property

    Public Property VolumeSerial() As String
        Get
            Return _VolumeSerial
        End Get
        Set(ByVal Value As String)
            _VolumeSerial = Value
        End Set
    End Property

    Public Property MotherBoardId() As String
        Get
            Return _MotherBoardId
        End Get
        Set(ByVal Value As String)
            _MotherBoardId = Value
        End Set
    End Property

    Public Property ProductVersion() As String
        Get
            Return _ProductVersion
        End Get
        Set(ByVal Value As String)
            _ProductVersion = Value
        End Set
    End Property

    Public Property BeginDate() As String
        Get
            Return _BeginDate
        End Get
        Set(ByVal Value As String)
            _BeginDate = Value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal Value As String)
            _EndDate = Value
        End Set
    End Property

    Public Property CompanyName() As String
        Get
            Return _CompanyName
        End Get
        Set(ByVal Value As String)
            _CompanyName = Value
        End Set
    End Property

    Public Property CompanyAlias() As String
        Get
            Return _CompanyAlias
        End Get
        Set(ByVal Value As String)
            _CompanyAlias = Value
        End Set
    End Property

    Public Property Activated() As Integer
        Get
            Return _Activated
        End Get
        Set(ByVal Value As Integer)
            _Activated = Value
        End Set
    End Property

    Public Property [Error]() As String
        Get
            Return _Error
        End Get
        Set(ByVal Value As String)
            _Error = Value
        End Set
    End Property

    Public Property LicenseType() As LicenseType
        Get
            Return _LicenseType
        End Get
        Set(ByVal Value As LicenseType)
            _LicenseType = Value
        End Set
    End Property

#End Region

#Region "Function"

    Public Function GetMACAddress() As String
        Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        Dim MACAddress As String = String.Empty
        For Each mo As ManagementObject In moc
            If (MACAddress.Equals(String.Empty)) Then
                If CBool(mo("IPEnabled")) Then MACAddress = mo("MacAddress").ToString()
                mo.Dispose()
            End If
            MACAddress = MACAddress.Replace(":", String.Empty)
        Next
        Return MACAddress
    End Function

    Public Function GetProcessorId() As String
        Dim strProcessorId As String = String.Empty
        Dim query As New SelectQuery("Win32_processor")
        Dim search As New ManagementObjectSearcher(query)
        Dim info As ManagementObject
        For Each info In search.Get()
            strProcessorId = info("processorId").ToString()
        Next
        Return strProcessorId
    End Function

    Public Function GetVolumeSerial(Optional ByVal strDriveLetter As String = "C") As String
        Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", strDriveLetter))
        disk.Get()
        Return disk("VolumeSerialNumber").ToString()
    End Function

    Public Function GetMotherBoardId() As String
        Dim strMotherBoardID As String = String.Empty
        Dim query As New SelectQuery("Win32_BaseBoard")
        Dim search As New ManagementObjectSearcher(query)
        Dim info As ManagementObject
        For Each info In search.Get()
            strMotherBoardID = info("SerialNumber").ToString()
        Next
        Return strMotherBoardID
    End Function

    Public Function GetProductVersion() As String
        Dim attribute As Object
        'attribute = MySettings. Info.Version.ToString
        If attribute.Length > 0 Then
            Return attribute.ToString
        Else
            Return String.Empty
        End If
    End Function

#End Region

    Public Sub New()

    End Sub

    Public Function Load() As License
        Try
            Dim des As New TripleDES(key, iv)
            Dim sr As StreamReader = My.Computer.FileSystem.OpenTextFileReader(AppDomain.CurrentDomain.BaseDirectory & "\license.lic")
            Dim data As String = sr.ReadToEnd

            If Not data = String.Empty Then

                Dim strFileText As String = data

                strFileText = des.Decrypt(strFileText)

                Dim arrFileText As String() = strFileText.Split("%"c)

                If arrFileText.Length = 11 Then

                    Me.MACAddress = CType(arrFileText(0), String)
                    Me.ProcessorId = CType(arrFileText(1), String)
                    Me.VolumeSerial = CType(arrFileText(2), String)
                    Me.MotherBoardId = CType(arrFileText(3), String)
                    Me.ProductVersion = CType(arrFileText(4), String)
                    Me.BeginDate = CType(arrFileText(5), String)
                    Me.EndDate = CType(arrFileText(6), String)
                    Me.CompanyName = CType(arrFileText(7), String)
                    Me.CompanyAlias = CType(arrFileText(8), String)
                    Me.Activated = CType(arrFileText(9), Integer)
                    Me.LicenseType = CType(arrFileText(10), Integer)

                End If

                Return License.OK
            Else
                Return License.Failed
            End If
        Catch ex As Exception
            Return License.Failed
        End Try
    End Function

    Public Function Load(ByVal data As String) As License
        Try
            Dim des As New TripleDES(key, iv)

            If Not data = String.Empty Then

                Dim strFileText As String = data

                strFileText = des.Decrypt(strFileText)

                Dim arrFileText As String() = strFileText.Split("%"c)

                If arrFileText.Length = 11 Then

                    Me.MACAddress = CType(arrFileText(0), String)
                    Me.ProcessorId = CType(arrFileText(1), String)
                    Me.VolumeSerial = CType(arrFileText(2), String)
                    Me.MotherBoardId = CType(arrFileText(3), String)
                    Me.ProductVersion = CType(arrFileText(4), String)
                    Me.BeginDate = CType(arrFileText(5), String)
                    Me.EndDate = CType(arrFileText(6), String)
                    Me.CompanyName = CType(arrFileText(7), String)
                    Me.CompanyAlias = CType(arrFileText(8), String)
                    Me.Activated = CType(arrFileText(9), Integer)
                    Me.LicenseType = CType(arrFileText(10), Integer)

                End If

                Return License.OK
            Else
                Return License.Failed
            End If
        Catch ex As Exception
            Return License.Failed
        End Try
    End Function

    Public Function CheckLicense() As Boolean
        Try
            Dim _MACAddress As String = GetMACAddress()
            Dim _ProcessorId As String = GetProcessorId()
            Dim _VolumeSerial As String = GetVolumeSerial()
            Dim _MotherBoardId As String = GetMotherBoardId()
            Dim _ProductVersion As String = GetProductVersion()
            Dim _BeginDate As String = Me.BeginDate.Substring(0, 2) & "/" & Me.BeginDate.Substring(2, 2) & "/" & Me.BeginDate.Substring(4, 4)
            Dim _EndDate As String = Me.EndDate.Substring(0, 2) & "/" & Me.EndDate.Substring(2, 2) & "/" & Me.EndDate.Substring(4, 4)

            If Not Me.Activated = 1 Then
                Me.Error = "Activation code is invalid."
                Return False
            ElseIf Not _MACAddress = Me.MACAddress Then
                Me.Error = "Activation code is invalid."
                Return False
            ElseIf Not _ProcessorId = Me.ProcessorId Then
                Me.Error = "Activation code is invalid."
                Return False
            ElseIf Not _VolumeSerial = Me.VolumeSerial Then
                Me.Error = "Activation code is invalid."
                Return False
            ElseIf Not _MotherBoardId = Me.MotherBoardId Then
                Me.Error = "Activation code is invalid."
                Return False
            ElseIf Not _ProductVersion = Me.ProductVersion Then
                Me.Error = "Activation code is invalid."
                Return False
            ElseIf Not Format(Now.Date, "MM/dd/yyyy") < _EndDate Then
                Me.Error = "Activation period has expired."
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Me.Error = "Activation code is invalid."
            Return False
        End Try
    End Function

End Class

Public Enum LicenseType
    Evaluation = 0
    Purchase = 1
End Enum

Public Enum License
    OK = 0
    Failed = 1
End Enum
