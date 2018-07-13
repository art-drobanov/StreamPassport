Imports System.IO
Imports System.Security.Cryptography

Public Module StreamPassport2Manager
    Private _sha512 As New SHA512Cng()

    Public Function Create(id As String, stream As Stream) As StreamPassport2
        stream.Seek(0, SeekOrigin.Begin)
        Dim sha512 = _sha512.ComputeHash(stream)
        Dim passport = New StreamPassport2(id, sha512, stream.Length)
        Return passport
    End Function

    Public Function Load(stream As Stream) As StreamPassport2
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
End Module
