Imports System.Runtime.Serialization
Imports System.Security.Cryptography
Imports System.Text

<DataContract>
Public MustInherit Class StreamPassport
    Protected _hashCng As HashAlgorithm
    Protected _hashMan As HashAlgorithm
    Protected _hashCsp As HashAlgorithm

    Public Sub New(hashCng As HashAlgorithm, hashMan As HashAlgorithm, hashCsp As HashAlgorithm)
        _hashCng = hashCng
        _hashMan = hashMan
        _hashCsp = hashCsp
    End Sub

    Protected Function CalcTotal(id As String, hash As String, streamSize As String) As String
        Dim encoding = Text.Encoding.ASCII
        Dim hashBytes As New List(Of Byte)
        For Each b In encoding.GetBytes(id) 'TODO: homoglyph removing from ID (filename)
            hashBytes.Add(b)
        Next
        For Each b In encoding.GetBytes(hash)
            hashBytes.Add(b)
        Next
        For Each b In encoding.GetBytes(streamSize)
            hashBytes.Add(b)
        Next
        Dim totalCng = BytesToHex(_hashCng.ComputeHash(hashBytes.ToArray()))
        Dim totalMan = BytesToHex(_hashMan.ComputeHash(hashBytes.ToArray()))
        Dim totalCsp = BytesToHex(_hashCsp.ComputeHash(hashBytes.ToArray()))
        If totalCng <> totalMan OrElse totalCng <> totalCsp Then
            Throw New Exception("StreamPassport: hash provider error!")
        Else
            Return totalCng
        End If
    End Function

    Protected Function BytesToHex(data As Byte()) As String
        Return BitConverter.ToString(data).Replace("-", String.Empty).ToUpper()
    End Function

    Protected Function ToStringBase(id As String, hash As String, streamSize As String) As String
        Dim streamSizeLong = Long.Parse(streamSize)
        Return String.Format("| ID: {0,-64} | Size: {1, 26} | SHA-{2}: {3,-64} |",
                             id.Substring(0, Math.Min(id.Length, 64)),
                             streamSizeLong.ToString("#,##0"),
                             If(hash.Length = 64, "256", If(hash.Length = 128, "512", "...")),
                             hash.Substring(0, Math.Min(hash.Length, 64)))
    End Function

    Protected Function ToTextBase(id As String, hash As String, streamSize As String) As String
        Dim sb = New StringBuilder()

        'Header
        sb.AppendLine("===============")
        sb.AppendLine(" StreamPassport")
        sb.AppendLine("---------------")

        'ID
        sb.AppendLine(String.Format("ID: {0};", id))
        sb.AppendLine("---------------")

        'StreamSize
        Dim sizeStack = New Stack()
        For Each d In streamSize
            sizeStack.Push(d)
        Next
        Dim sizeString = New StringBuilder()
        Dim sizeCounter = 1
        For Each d In sizeStack
            sizeString.Append(d)
            If sizeCounter = 3 Then
                sizeCounter = 0
                sizeString.Append(" ")
            End If
            sizeCounter += 1
        Next
        sb.AppendLine(String.Format("Size: {0};", New String(sizeString.ToString().Trim().Reverse().ToArray())))
        sb.AppendLine("---------------")

        'Hash
        sb.AppendLine(String.Format("SHA-{0}:", If(hash.Length = 64, "256", If(hash.Length = 128, "512", "..."))))
        sb.Append("  01: ") : Dim row = 1
        For i = 1 To hash.Length
            sb.Append(hash(i - 1))
            If i Mod 4 = 0 AndAlso i Mod 8 <> 0 Then
                sb.Append("-")
            End If
            If i Mod 8 = 0 AndAlso i < hash.Length Then
                row += 1
                sb.Append(vbCrLf)
                sb.Append(String.Format("  {0, 2}: ", row.ToString("00")))
            End If
        Next
        sb.Append(vbCrLf)
        sb.AppendLine("===============")

        Return sb.ToString()
    End Function
End Class
