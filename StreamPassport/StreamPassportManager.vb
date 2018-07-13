Imports System.IO
Imports System.Security.Cryptography

Public Module StreamPassportManager
    Private _sha256 As New SHA256Cng()

    Public Function Create(id As String, stream As Stream) As StreamPassport
        stream.Seek(0, SeekOrigin.Begin)
        Dim sha256 = _sha256.ComputeHash(stream)
        Dim passport = New StreamPassport(id, sha256, stream.Length)
        Return passport
    End Function

    Public Function Load(stream As Stream) As StreamPassport
        Using sr = New StreamReader(stream, Text.Encoding.UTF8, False, 4096, True)
            Dim objJson = sr.ReadToEnd()
            Dim obj = New StreamPassport()
            obj.Deserialize(objJson)
            With obj
                .ID = If(.ID Is Nothing, String.Empty, .ID)
                .SHA256 = If(.SHA256 Is Nothing, String.Empty, .SHA256)
                .StreamSize = If(.StreamSize Is Nothing, String.Empty, .StreamSize)
                .Total = If(.Total Is Nothing, String.Empty, .Total)
            End With
            Return obj
        End Using
    End Function
End Module
