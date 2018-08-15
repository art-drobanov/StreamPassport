Imports System.IO

Public Module StreamPassportManager
    Public Const Ext As String = ".sprt"
    Public Const Ext2 As String = ".sprt2"
    Public Const OKMarker As String = ".ok"
    Public Const CorruptedMarker As String = ".corrupted"
    Public Const TextMarker As String = ".txt"
    Public Const ProcessingMarker As String = ".processing"

    Public Sub FileProcessing(fileName As String, type As StreamPassportType, useTextOutput As Boolean, noTotalHashCheck As Boolean)
        Dim ISP As IStreamPassport = If(type = StreamPassportType.SHA256, New StreamPassport1(), New StreamPassport2())
        Dim txtFileName = fileName + ISP.Ext + ISP.TextMarker
        Dim okFileMarker = fileName + ISP.Ext + ISP.OKMarker
        Dim corruptedFileMarker = fileName + ISP.Ext + ISP.CorruptedMarker
        Dim processingFileMarker = fileName + ISP.Ext + ISP.ProcessingMarker
        Using fs = File.OpenRead(fileName)
            SafeDelete(processingFileMarker) : File.Create(processingFileMarker).Close()
            SafeDelete(okFileMarker)
            SafeDelete(corruptedFileMarker)
            Dim sprtNew = ISP.CreatePassport(Path.GetFileName(fileName), fs)
            Dim sprtFileName = fileName + ISP.Ext
            If File.Exists(sprtFileName) Then
                Using sprtFs = File.OpenRead(sprtFileName)
                    Dim sprtLoaded = LoadStreamPassport(ISP, sprtFs)
                    If sprtLoaded IsNot Nothing AndAlso sprtNew.Compare(sprtLoaded, noTotalHashCheck) Then
                        File.Create(okFileMarker).Close()
                        If useTextOutput Then WritePassportToText(sprtNew, txtFileName)
                    Else
                        File.Create(corruptedFileMarker).Close()
                        SafeDelete(txtFileName)
                    End If
                End Using
            Else
                File.WriteAllText(sprtFileName, sprtNew.Serialize())
                If useTextOutput Then WritePassportToText(sprtNew, txtFileName)
            End If
            SafeDelete(processingFileMarker)
        End Using
    End Sub

    Public Sub SafeDelete(fileName As String)
        If File.Exists(fileName) Then
            File.SetAttributes(fileName, FileAttributes.Normal)
            File.Delete(fileName)
        End If
    End Sub

    Private Function LoadStreamPassport(ISP As IStreamPassport, stream As Stream) As IStreamPassport
        Dim sprt As IStreamPassport = Nothing
        Try
            sprt = ISP.LoadPassport(stream)
        Catch
        End Try
        Return sprt
    End Function

    Private Sub WritePassportToText(sp As IStreamPassport, txtFileName As String)
        SafeDelete(txtFileName)
        File.WriteAllText(txtFileName, sp.ToText())
    End Sub
End Module
