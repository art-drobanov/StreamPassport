Imports System.IO

Public Module StreamPassportManager
    Public Const Ext As String = ".sprt"
    Public Const Ext2 As String = ".sprt2"
    Public Const OKMarker As String = ".ok"
    Public Const CorruptedMarker As String = ".corrupted"
    Public Const TextMarker As String = ".txt"
    Public Const ProcessingMarker As String = ".processing"

    Public Delegate Sub MessageOutDelegate(msg As String, title As String, isInfo As Boolean, isExclamation As Boolean, isError As Boolean)

    Public Function CreateFilePassport(fileName As String,
                                       type As StreamPassportType,
                                       useTextOutput As Boolean,
                                       messageOutHandler As MessageOutDelegate) As String
        Dim sprtNewFileName As String = Nothing
        Dim msgTitle = "Create Stream Passport"
        If File.Exists(fileName) Then
            Using fs = File.OpenRead(fileName)
                Dim ISP As IStreamPassport = If(type = StreamPassportType.SHA256, New StreamPassport1(), New StreamPassport2())
                sprtNewFileName = fileName + ISP.Ext
                Dim txtFileName = sprtNewFileName + ISP.TextMarker
                Dim sprtNew = ISP.CreatePassport(Path.GetFileName(fileName), fs)
                WritePassport(sprtNew, sprtNewFileName)
                If useTextOutput Then WritePassportToText(sprtNew, txtFileName)
                If messageOutHandler IsNot Nothing Then
                    Dim msg = String.Format("Stream Passport: '{0}'", Path.GetFileName(sprtNewFileName))
                    messageOutHandler(msg, msgTitle, True, False, False)
                End If
            End Using
        End If
        Return sprtNewFileName
    End Function

    Public Function CheckFilePassport(fileName As String,
                                      type As StreamPassportType,
                                      noTotalHashCheck As Boolean,
                                      sprts As IEnumerable(Of IStreamPassport),
                                      MessageOutHandler As MessageOutDelegate) As Integer
        Dim okCounter = 0
        Dim msgTitle = "Check Stream Passport"
        If File.Exists(fileName) Then
            Using fs = File.OpenRead(fileName)
                Dim ISP As IStreamPassport = If(type = StreamPassportType.SHA256, New StreamPassport1(), New StreamPassport2())
                Dim sp = ISP.CreatePassport(Path.GetFileName(fileName), fs)
                Dim sprtToLoadFileName = fileName + ISP.Ext
                If File.Exists(sprtToLoadFileName) Then
                    Using sprtFs = File.OpenRead(sprtToLoadFileName)
                        Dim sp2 = ISP.LoadPassport(sprtFs)
                        If sp.Compare(sp2, noTotalHashCheck) Then
                            okCounter += 1
                            If MessageOutHandler IsNot Nothing Then
                                Dim msg = String.Format("File '{0}' was successfully verified by stream passport from '{1}'", sp.ID, Path.GetFileName(sprtToLoadFileName))
                                MessageOutHandler(msg, msgTitle, True, False, False)
                            End If
                        End If
                    End Using
                End If
                For Each sprt In sprts
                    If sp.Compare(sprt, noTotalHashCheck) Then
                        If MessageOutHandler IsNot Nothing Then
                            Dim msg = String.Format("File '{0}' was successfully verified by stream passport from list ('..\data')", sp.ID)
                            MessageOutHandler(msg, msgTitle, True, False, False)
                        End If
                        okCounter += 1
                    End If
                Next
                If okCounter = 0 Then
                    If MessageOutHandler IsNot Nothing Then
                        Dim msg = String.Format("No valid stream passport for '{0}'", sp.ID)
                        MessageOutHandler(msg, msgTitle, False, True, False)
                    End If
                End If
            End Using
        Else
            If MessageOutHandler IsNot Nothing Then
                Dim msg = String.Format("File {0} not found", Path.GetFileName(fileName))
                MessageOutHandler(msg, msgTitle, False, True, False)
            End If
        End If
        Return okCounter
    End Function

    Public Sub FileProcessingConsole(fileName As String,
                                     type As StreamPassportType,
                                     useTextOutput As Boolean,
                                     noTotalHashCheck As Boolean)
        If File.Exists(fileName) Then
            Using fs = File.OpenRead(fileName)
                Dim ISP As IStreamPassport = If(type = StreamPassportType.SHA256, New StreamPassport1(), New StreamPassport2())
                Dim sprtNewFileName = fileName + ISP.Ext
                Dim txtFileName = sprtNewFileName + ISP.TextMarker
                Dim okFileMarker = sprtNewFileName + ISP.OKMarker
                Dim corruptedFileMarker = sprtNewFileName + ISP.CorruptedMarker
                Dim processingFileMarker = sprtNewFileName + ISP.ProcessingMarker
                SafeDelete(processingFileMarker) : File.Create(processingFileMarker).Close()
                SafeDelete(okFileMarker)
                SafeDelete(corruptedFileMarker)
                Dim sprtNew = ISP.CreatePassport(Path.GetFileName(fileName), fs)
                If File.Exists(sprtNewFileName) Then
                    Using sprtFs = File.OpenRead(sprtNewFileName)
                        Dim sprtLoaded = LoadPassport(ISP, sprtFs)
                        If sprtLoaded IsNot Nothing AndAlso sprtNew.Compare(sprtLoaded, noTotalHashCheck) Then
                            File.Create(okFileMarker).Close()
                            If useTextOutput Then WritePassportToText(sprtNew, txtFileName)
                        Else
                            File.Create(corruptedFileMarker).Close()
                            SafeDelete(txtFileName)
                        End If
                    End Using
                Else
                    WritePassport(sprtNew, sprtNewFileName)
                    If useTextOutput Then WritePassportToText(sprtNew, txtFileName)
                End If
                SafeDelete(processingFileMarker)
            End Using
        End If
    End Sub

    Public Function GetValidSprtsFromFolder(sprtsPath As String, type As StreamPassportType) As IStreamPassport()
        Dim ISP As IStreamPassport = If(type = StreamPassportType.SHA256, New StreamPassport1(), New StreamPassport2())
        Dim sprts As New List(Of IStreamPassport)
        If Not Directory.Exists(sprtsPath) Then
            Directory.CreateDirectory(sprtsPath)
        End If
        Dim sprtFileNames = Directory.EnumerateFiles(Path.GetDirectoryName(sprtsPath), String.Format("*{0}", ISP.Ext), SearchOption.AllDirectories).ToArray()
        For Each sprtFileName In sprtFileNames
            If File.Exists(sprtFileName) Then
                Using sprtFs = File.OpenRead(sprtFileName)
                    Dim sprtLoaded = LoadPassport(ISP, sprtFs)
                    If sprtLoaded.IsValid() Then
                        sprts.Add(sprtLoaded)
                    End If
                End Using
            End If
        Next
        Return sprts.ToArray()
    End Function

    Private Function LoadPassport(ISP As IStreamPassport, stream As Stream) As IStreamPassport
        Dim sprt As IStreamPassport = Nothing
        Try
            sprt = ISP.LoadPassport(stream)
        Catch
        End Try
        Return sprt
    End Function

    Private Sub WritePassport(sp As IStreamPassport, fileName As String)
        SafeDelete(fileName)
        File.WriteAllText(fileName, sp.Serialize())
    End Sub

    Private Sub WritePassportToText(sp As IStreamPassport, txtFileName As String)
        SafeDelete(txtFileName)
        File.WriteAllText(txtFileName, sp.ToText())
    End Sub

    Public Sub SafeDelete(fileName As String)
        If File.Exists(fileName) Then
            File.SetAttributes(fileName, FileAttributes.Normal)
            File.Delete(fileName)
        End If
    End Sub
End Module
