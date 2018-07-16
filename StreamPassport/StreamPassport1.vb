Imports System.Runtime.Serialization
Imports System.Security.Cryptography

<DataContract>
Public Class StreamPassport1
    Inherits StreamPassport

    <DataMember>
    Public Property ID As String

    <DataMember>
    Public Property SHA256 As String

    <DataMember>
    Public Property StreamSize As String

    <DataMember>
    Public Property Total As String

    Public Shared ReadOnly Property Ext As String = ".sprt"
    Public Shared ReadOnly Property CorruptedMarker As String = ".corrupted"
    Public Shared ReadOnly Property OKMarker As String = ".ok"
    Public Shared ReadOnly Property TextMarker As String = ".txt"

    Public Sub New()
        MyBase.New(New SHA256Cng(), New SHA256Managed(), New SHA256CryptoServiceProvider())
    End Sub

    Public Sub New(id As String, sha256 As Byte(), streamSize As Long)
        MyBase.New(New SHA256Cng(), New SHA256Managed(), New SHA256CryptoServiceProvider())
        Me.ID = id
        Me.SHA256 = BytesToHex(sha256).ToUpper()
        Me.StreamSize = streamSize.ToString().ToUpper()
        Me.Total = CalcTotal(Me.ID, Me.SHA256, Me.StreamSize).ToUpper()
    End Sub

    Public Function Serialize() As String
        Return Serializer.SaveObjectToJsonString(Me).Replace("{", "{" + vbCrLf).Replace("}", vbCrLf + "}").Replace(",", "," + vbCrLf)
    End Function

    Public Sub Deserialize(objJson As String)
        Dim obj = Serializer.LoadObjectFromJsonString(Of StreamPassport1)(objJson)
        Me.ID = obj.ID
        Me.SHA256 = obj.SHA256.ToUpper()
        Me.StreamSize = obj.StreamSize.ToUpper()
        Me.Total = obj.Total.ToUpper()
    End Sub

    Public Function IsValid() As Boolean
        Return Me.Total = CalcTotal(Me.ID, Me.SHA256, Me.StreamSize)
    End Function

    Public Function Compare(sprt As StreamPassport1, noTotalHash As Boolean) As Boolean
        If noTotalHash OrElse (Me.IsValid() AndAlso sprt.IsValid()) Then
            Return Me.SHA256 = sprt.SHA256 AndAlso Me.StreamSize = sprt.StreamSize
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String
        Return MyBase.ToStringBase(Me.ID, Me.SHA256, Me.StreamSize)
    End Function

    Public Shadows Function ToText() As String
        Return MyBase.ToTextBase(Me.ID, Me.SHA256, Me.StreamSize)
    End Function
End Class
