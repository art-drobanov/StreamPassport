Imports System.Text
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

<DataContract>
Public Class StreamPassport2
    Private _sha512Cng As New SHA512Cng()
    Private _sha512Man As New SHA512Managed()
    Private _sha512Csp As New SHA512CryptoServiceProvider()

    <DataMember>
    Public Property ID As String

    <DataMember>
    Public Property SHA512 As String

    <DataMember>
    Public Property StreamSize As String

    <DataMember>
    Public Property Total As String

    Public Sub New()
    End Sub

    Public Sub New(id As String, sha512 As Byte(), streamSize As Long)
        Me.ID = id
        Me.SHA512 = BytesToHex(sha512).ToUpper()
        Me.StreamSize = streamSize.ToString().ToUpper()
        Me.Total = CalcTotal().ToUpper()
    End Sub

    Public Function Serialize() As String
        Return Serializer.SaveObjectToJsonString(Me).Replace("{", "{" + vbCrLf).Replace("}", vbCrLf + "}").Replace(",", "," + vbCrLf)
    End Function

    Public Sub Deserialize(objJson As String)
        Dim obj = Serializer.LoadObjectFromJsonString(Of StreamPassport2)(objJson)
        Me.ID = obj.ID
        Me.SHA512 = obj.SHA512.ToUpper()
        Me.StreamSize = obj.StreamSize.ToUpper()
        Me.Total = obj.Total.ToUpper()
    End Sub

    Public Function IsValid() As Boolean
        Return Me.Total = CalcTotal()
    End Function

    Public Function Compare(sprt As StreamPassport2) As Boolean
        If Me.IsValid() AndAlso sprt.IsValid Then
            If Me.SHA512 = sprt.SHA512 AndAlso Me.StreamSize = sprt.StreamSize Then
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
        Return String.Format("| ID: {0,-64} | Size: {1, 26} | SHA-512: {2,-128} |", Me.ID.Substring(0, Math.Min(Me.ID.Length, 128)),
                             streamSizeLong.ToString("#,##0"), Me.SHA512)
    End Function

    Private Function CalcTotal() As String
        Dim hashBytes As New List(Of Byte)
        For Each b In Encoding.ASCII.GetBytes(Me.ID)
            hashBytes.Add(b)
        Next
        For Each b In Encoding.ASCII.GetBytes(Me.SHA512)
            hashBytes.Add(b)
        Next
        For Each b In Encoding.ASCII.GetBytes(Me.StreamSize)
            hashBytes.Add(b)
        Next
        Dim totalCng = BytesToHex(_sha512Cng.ComputeHash(hashBytes.ToArray()))
        Dim totalMan = BytesToHex(_sha512Man.ComputeHash(hashBytes.ToArray()))
        Dim totalCsp = BytesToHex(_sha512Csp.ComputeHash(hashBytes.ToArray()))
        If totalCng <> totalMan OrElse totalCng <> totalCsp Then
            Throw New Exception("StreamPassport: SHA-512 provider error!")
        Else
            Return totalCng
        End If
    End Function

    Private Function BytesToHex(data As Byte()) As String
        Return BitConverter.ToString(data).Replace("-", String.Empty).ToUpper()
    End Function
End Class
