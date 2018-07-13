Imports System.Text
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

<DataContract>
Public Class StreamPassport
    Private _sha256Cng As New SHA256Cng()
    Private _sha256Man As New SHA256Managed()
    Private _sha256Csp As New SHA256CryptoServiceProvider()

    <DataMember>
    Public Property ID As String

    <DataMember>
    Public Property SHA256 As String

    <DataMember>
    Public Property StreamSize As String

    <DataMember>
    Public Property Total As String

    Public Sub New()
    End Sub

    Public Sub New(id As String, sha256 As Byte(), streamSize As Long)
        Me.ID = id
        Me.SHA256 = BytesToHex(sha256).ToUpper()
        Me.StreamSize = streamSize.ToString().ToUpper()
        Me.Total = CalcTotal().ToUpper()
    End Sub

    Public Function Serialize() As String
        Return Serializer.SaveObjectToJsonString(Me).Replace("{", "{" + vbCrLf).Replace("}", vbCrLf + "}").Replace(",", "," + vbCrLf)
    End Function

    Public Sub Deserialize(objJson As String)
        Dim obj = Serializer.LoadObjectFromJsonString(Of StreamPassport)(objJson)
        Me.ID = obj.ID
        Me.SHA256 = obj.SHA256.ToUpper()
        Me.StreamSize = obj.StreamSize.ToUpper()
        Me.Total = obj.Total.ToUpper()
    End Sub

    Public Function IsValid() As Boolean
        Return Me.Total = CalcTotal()
    End Function

    Public Function Compare(sprt As StreamPassport, noTotalHash As Boolean) As Boolean
        If noTotalHash OrElse (Me.IsValid() AndAlso sprt.IsValid()) Then
            If Me.SHA256 = sprt.SHA256 AndAlso Me.StreamSize = sprt.StreamSize Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String
        Dim streamSizeLong = Long.Parse(Me.StreamSize)
        Return String.Format("| ID: {0,-64} | Size: {1, 26} | SHA-256: {2,-64} |", Me.ID.Substring(0, Math.Min(Me.ID.Length, 64)),
                             streamSizeLong.ToString("#,##0"), Me.SHA256)
    End Function

    Public Function ToText() As String
        Dim sb = New StringBuilder()

        'Header
        sb.AppendLine("==============")
        sb.AppendLine("StreamPassport")
        sb.AppendLine("==============")

        'ID
        sb.AppendLine(String.Format("ID: {0};", Me.ID))

        'Size
        Dim sizeStack = New Stack()
        For Each d In Me.StreamSize
            sizeStack.Push(d)
        Next
        Dim sizeString = New StringBuilder()
        Dim sizeCounter = 1
        For Each d In sizeStack
            sizeString.Append(d)
            If sizeCounter = 3 Then
                sizeCounter = 1
                sizeString.Append(" ")
            End If
            sizeCounter += 1
        Next
        sb.AppendLine(String.Format("Size: {0};", New String(sizeString.ToString().Reverse().ToArray())))

        'SHA-256
        sb.AppendLine("SHA-256:")
        sb.AppendLine("=============")
        sb.Append("01: ") : Dim row = 1
        For i = 1 To Me.SHA256.Length
            sb.Append(Me.SHA256(i - 1))
            If i Mod 4 = 0 AndAlso i Mod 8 <> 0 Then
                sb.Append("-")
            End If
            If i Mod 8 = 0 AndAlso i < Me.SHA256.Length Then
                row += 1
                sb.Append(vbCrLf)
                sb.Append(String.Format("{0, 2}: ", row.ToString("00")))
            End If
        Next
        sb.Append(vbCrLf)
        sb.AppendLine("=============")

        Return sb.ToString()
    End Function

    Private Function CalcTotal() As String
        Dim encoding = Text.Encoding.ASCII
        Dim hashBytes As New List(Of Byte)
        For Each b In encoding.GetBytes(Me.ID) 'TODO: homoglyph removing from ID (filename)
            hashBytes.Add(b)
        Next
        For Each b In encoding.GetBytes(Me.SHA256)
            hashBytes.Add(b)
        Next
        For Each b In encoding.GetBytes(Me.StreamSize)
            hashBytes.Add(b)
        Next
        Dim totalCng = BytesToHex(_sha256Cng.ComputeHash(hashBytes.ToArray()))
        Dim totalMan = BytesToHex(_sha256Man.ComputeHash(hashBytes.ToArray()))
        Dim totalCsp = BytesToHex(_sha256Csp.ComputeHash(hashBytes.ToArray()))
        If totalCng <> totalMan OrElse totalCng <> totalCsp Then
            Throw New Exception("StreamPassport: SHA-256 provider error!")
        Else
            Return totalCng
        End If
    End Function

    Private Function BytesToHex(data As Byte()) As String
        Return BitConverter.ToString(data).Replace("-", String.Empty).ToUpper()
    End Function
End Class
