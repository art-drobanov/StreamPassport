Imports System.IO
Imports System.Runtime.Serialization
Imports System.Security.Cryptography
Imports StreamPassport

<DataContract>
Public Class StreamPassport2
    Inherits StreamPassport
    Implements IStreamPassport

    Public Shared Function Create(id As String, stream As Stream) As StreamPassport2
        Static sha512Cng As New SHA512Cng()
        stream.Seek(0, SeekOrigin.Begin)
        Dim sha512 = sha512Cng.ComputeHash(stream)
        Dim passport = New StreamPassport2(id, sha512, stream.Length)
        Return passport
    End Function

    Public Shared Function Load(stream As Stream) As StreamPassport2
        Using sr = New StreamReader(stream, Text.Encoding.UTF8, False, 4096, True)
            Dim objJson = sr.ReadToEnd()
            Dim obj = New StreamPassport2()
            obj.Deserialize(objJson)
            With obj
                .ID = If(.ID Is Nothing, String.Empty, .ID)
                .SHA512 = If(.SHA512 Is Nothing, String.Empty, .SHA512)
                .StreamSize = If(.StreamSize Is Nothing, String.Empty, .StreamSize)
                .Total = If(.Total Is Nothing, String.Empty, .Total)
            End With
            Return obj
        End Using
    End Function

    Public ReadOnly Property Ext As String = StreamPassportManager.Ext2 Implements IStreamPassport.Ext
    Public ReadOnly Property OKMarker As String = StreamPassportManager.OKMarker Implements IStreamPassport.OKMarker
    Public ReadOnly Property CorruptedMarker As String = StreamPassportManager.CorruptedMarker Implements IStreamPassport.CorruptedMarker
    Public ReadOnly Property TextMarker As String = StreamPassportManager.TextMarker Implements IStreamPassport.TextMarker
    Public ReadOnly Property ProcessingMarker As String = StreamPassportManager.ProcessingMarker Implements IStreamPassport.ProcessingMarker

    <DataMember>
    Public Property ID As String Implements IStreamPassport.ID
    <DataMember>
    Public Property SHA512 As String Implements IStreamPassport.HASH
    <DataMember>
    Public Property StreamSize As String Implements IStreamPassport.StreamSize
    <DataMember>
    Public Property Total As String Implements IStreamPassport.Total

    Public Sub New()
        MyBase.New(New SHA512Cng(), New SHA512Managed(), New SHA512CryptoServiceProvider())
    End Sub

    Public Sub New(id As String, sha512 As Byte(), streamSize As Long)
        MyBase.New(New SHA512Cng(), New SHA512Managed(), New SHA512CryptoServiceProvider())
        Me.ID = id
        Me.SHA512 = BytesToHex(sha512).ToUpper()
        Me.StreamSize = streamSize.ToString().ToUpper()
        Me.Total = CalcTotal(Me.ID, Me.SHA512, Me.StreamSize).ToUpper()
    End Sub

    Public Function CreatePassport(id As String, stream As Stream) As IStreamPassport Implements IStreamPassport.CreatePassport
        Return Create(id, stream)
    End Function

    Public Function LoadPassport(stream As Stream) As IStreamPassport Implements IStreamPassport.LoadPassport
        Return Load(stream)
    End Function

    Public Function Serialize() As String Implements IStreamPassport.Serialize
        Return Serializer.SaveObjectToJsonString(Me).Replace("{", "{" + vbCrLf).Replace("}", vbCrLf + "}").Replace(",", "," + vbCrLf)
    End Function

    Public Sub Deserialize(objJson As String) Implements IStreamPassport.Deserialize
        Dim obj = Serializer.LoadObjectFromJsonString(Of StreamPassport2)(objJson)
        Me.ID = obj.ID
        Me.SHA512 = obj.SHA512.ToUpper()
        Me.StreamSize = obj.StreamSize.ToUpper()
        Me.Total = obj.Total.ToUpper()
    End Sub

    Public Function IsValid() As Boolean Implements IStreamPassport.IsValid
        Return Me.Total = CalcTotal(Me.ID, Me.SHA512, Me.StreamSize)
    End Function

    Public Function Compare(sprt As IStreamPassport, Optional noTotalHashCheck As Boolean = False) As Boolean Implements IStreamPassport.Compare
        If noTotalHashCheck OrElse (sprt.IsValid() AndAlso Me.IsValid()) Then
            Return Me.SHA512 = sprt.HASH AndAlso Me.StreamSize = sprt.StreamSize
        Else
            Return False
        End If
    End Function

    Public Overrides Function ToString() As String Implements IStreamPassport.ToString
        Return MyBase.ToStringBase(Me.ID, Me.SHA512, Me.StreamSize)
    End Function

    Public Shadows Function ToText() As String Implements IStreamPassport.ToText
        Return MyBase.ToTextBase(Me.ID, Me.SHA512, Me.StreamSize)
    End Function
End Class
