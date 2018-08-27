Imports System.IO
Imports System.Windows.Forms
Imports StreamPassport

Module Program
    Private Const _ext = StreamPassportManager.Ext
    Private Const _ext2 = StreamPassportManager.Ext2
    Private Const _corruptedExt = StreamPassportManager.CorruptedMarker
    Private Const _okExt = StreamPassportManager.OKMarker
    Private Const _txt = StreamPassportManager.TextMarker

    Sub Main(args As String())
        Console.WriteLine("------------------------------------------------------")
        Console.WriteLine("Stream Checksum Integrity Verifier (SHA-256 & SHA-512)")
        Console.WriteLine(My.Application.Info.Version.ToString())
        Console.WriteLine("------------------------------------------------------")

        Dim switches = {"-r", "-2", "-t", "-n"}
        Dim argsf = args.Where(Function(item) Not switches.Contains(item.ToLower().Trim())).ToArray()
        Dim isRecursive = args.Contains("-r")
        Dim useSprt2 = args.Contains("-2")
        Dim useTextOutput = args.Contains("-t")
        Dim noTotalHash = args.Contains("-n")

        If argsf.Count = 0 Then
            Console.WriteLine("Use -r to enable recursive directory processing")
            Console.WriteLine("Use -2 to add SHA-512 calculation (*.sprt2)")
            Console.WriteLine("Use -t to add output to the text file")
            Console.WriteLine("Use -n to not to check 'total' hash")
            Console.WriteLine()
        End If

        Dim files As String()
        If argsf.Count <> 0 Then
            files = argsf
        Else
            Dim appExeName = Path.GetFileName(Application.ExecutablePath)
            files = Directory.EnumerateFiles(Path.GetDirectoryName(Application.ExecutablePath), "*.*",
                                             If(isRecursive, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly)).Where(Function(item)
                                                                                                                                    Return Not item.EndsWith(_ext) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2) AndAlso
                                                                                                                                         Not item.EndsWith(_ext + _corruptedExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2 + _corruptedExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext + _okExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2 + _okExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext + _txt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2 + _txt) AndAlso
                                                                                                                                         Not item.EndsWith(appExeName)
                                                                                                                                End Function).ToArray()
        End If
        Dim processedOk As Integer = 0
        Dim processedWithErr As Integer = 0
        For Each fileName In files
            If File.Exists(fileName) Then
                Try
                    StreamPassportManager.FileProcessingConsole(fileName, StreamPassportType.SHA256, useTextOutput, noTotalHash)
                    If useSprt2 Then
                        StreamPassportManager.FileProcessingConsole(fileName, StreamPassportType.SHA512, useTextOutput, noTotalHash)
                    End If
                    processedOk += 1
                    Console.WriteLine(String.Format("Processed: {0} ({1} / {2}, Errors total: {3})", fileName, processedOk, files.Length, processedWithErr))
                Catch ex As Exception
                    processedWithErr += 1
                    Console.WriteLine(String.Format("Error: {0} ({1} / {2}, Errors total: {3})", fileName, processedOk, files.Length, processedWithErr))
                End Try
            Else
                Console.WriteLine(String.Format("Not found: {0} ({1} / {2}, Errors total: {3})", fileName, processedOk, files.Length, processedWithErr))
            End If
        Next
    End Sub
End Module
