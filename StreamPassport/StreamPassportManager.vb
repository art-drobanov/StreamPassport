Imports System.IO

Public Module StreamPassportManager
    Public Const Ext As String = ".sprt"
    Public Const Ext2 As String = ".sprt2"
    Public Const CorruptedMarker As String = ".corrupted"
    Public Const OKMarker As String = ".ok"
    Public Const TextMarker As String = ".txt"

    Public Sub FileProcessing(fileName As String, type As StreamPassportType, useTextOutput As Boolean, noTotalHash As Boolean)
        Dim ISP As IStreamPassport = If(type = StreamPassportType.SHA256, New StreamPassport1(), New StreamPassport2())
        Dim txtFileName = fileName + ISP.Ext + ISP.TextMarker
        Dim corruptedFileName = fileName + ISP.Ext + ISP.CorruptedMarker
        Dim okFileName = fileName + ISP.Ext + ISP.OKMarker
        Using fs = File.OpenRead(fileName)
            Dim sp As IStreamPassport = Nothing
            Try
                sp = ISP.LoadPassport(fs)
            Catch
            End Try
            If sp IsNot Nothing Then
                If useTextOutput Then
                    If File.Exists(txtFileName) Then
                        SafeDelete(txtFileName)
                    End If
                    File.WriteAllText(txtFileName, sp.ToText())
                End If
            Else
                Dim sprt = ISP.CreatePassport(Path.GetFileName(fileName), fs)
                Dim sprtFileName = fileName + ISP.Ext
                If File.Exists(sprtFileName) Then
                    Using sprtFs = File.OpenRead(sprtFileName)
                        sp = ISP.LoadPassport(sprtFs)
                        If sprt.Compare(sp, noTotalHash) Then
                            SafeDelete(corruptedFileName)
                            File.Create(okFileName)
                        Else
                            SafeDelete(okFileName)
                            File.Create(corruptedFileName)
                        End If
                    End Using
                Else
                    File.WriteAllText(sprtFileName, sprt.Serialize())
                    If useTextOutput Then
                        If File.Exists(txtFileName) Then
                            SafeDelete(txtFileName)
                        End If
                        File.WriteAllText(txtFileName, sprt.ToText())
                    End If
                End If
            End If
        End Using
    End Sub

    Public Sub SafeDelete(fileName As String)
        If File.Exists(fileName) Then
            File.SetAttributes(fileName, FileAttributes.Normal)
            File.Delete(fileName)
        End If
    End Sub
End Module
