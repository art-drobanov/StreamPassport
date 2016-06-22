Imports System.Text
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

<DataContract>
Public Class StreamPassport
    Private _totalHF As New SHA256Managed

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

    Public Function Compare(sprt As StreamPassport) As Boolean
        If Me.IsValid() AndAlso sprt.IsValid Then
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
        Dim sb As New StringBuilder()
        sb.Append(String.Format("ID:         {0}", Me.ID) + vbCrLf)
        sb.Append(String.Format("SHA-256:    {0}", Me.SHA256) + vbCrLf)
        sb.Append(String.Format("StreamSize: {0}", Me.StreamSize) + vbCrLf)
        sb.Append(String.Format("Total:      {0}", Me.Total) + vbCrLf)
        Return sb.ToString()
    End Function

    Private Function CalcTotal() As String
        Dim hashBytes As New List(Of Byte)
        For Each b In Encoding.ASCII.GetBytes(Me.ID)
            hashBytes.Add(b)
        Next
        For Each b In Encoding.ASCII.GetBytes(Me.SHA256)
            hashBytes.Add(b)
        Next
        For Each b In Encoding.ASCII.GetBytes(Me.StreamSize)
            hashBytes.Add(b)
        Next
        Dim total = BytesToHex(_totalHF.ComputeHash(hashBytes.ToArray()))
        Return total
    End Function

    Private Function BytesToHex(data As Byte()) As String
        Return BitConverter.ToString(data).Replace("-", String.Empty).ToUpper()
    End Function
End Class
