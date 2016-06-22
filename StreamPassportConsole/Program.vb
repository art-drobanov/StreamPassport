Imports System.IO
Imports System.Windows.Forms
Imports StreamPassport

Module Program
    Private Const _ext = ".sprt"
    Private Const _corruptedExt = ".corrupted"
    Private Const _okExt = ".ok"

    Sub Main(args As String())
        Console.WriteLine("----------------------------------------")
        Console.WriteLine("- StreamPassport (version 1.0) SHA-256 -")
        Console.WriteLine("----------------------------------------")
        Dim files As String()
        If args.Count <> 0 Then
            files = args
        Else
            Dim exeName = Path.GetFileName(Application.ExecutablePath)
            files = Directory.EnumerateFiles(Path.GetDirectoryName(Application.ExecutablePath)).Where(Function(item)
                                                                                                          Return Not item.EndsWith(_ext) AndAlso
                                                                                                                 Not item.EndsWith(_corruptedExt) AndAlso
                                                                                                                 Not item.EndsWith(_okExt) AndAlso
                                                                                                                 Not item.EndsWith(exeName)
                                                                                                      End Function).ToArray()
        End If
        Dim processed As Integer = 0
        For Each fileName In files
            If File.Exists(fileName) Then
                Using fs = File.OpenRead(fileName)
                    Dim sp = StreamPassportManager.Create(Path.GetFileName(fileName), fs)
                    If File.Exists(fileName + _ext) Then
                        Using sprtFs = File.OpenRead(fileName + _ext)
                            Dim sp2 = StreamPassportManager.Load(sprtFs)
                            If sp.Compare(sp2) Then
                                SafeDelete(fileName + _ext + _corruptedExt)
                                File.Create(fileName + _ext + _okExt)
                            Else
                                SafeDelete(fileName + _ext + _okExt)
                                File.Create(fileName + _ext + _corruptedExt)
                            End If
                        End Using
                    Else
                        File.WriteAllText(fileName + _ext, sp.Serialize())
                    End If
                    processed += 1
                    Console.WriteLine(String.Format("Processed: {0} ({1} / {2})", fileName, processed, files.Length))
                End Using
            Else
                Console.WriteLine(String.Format("Not found: {0} ({1} / {2})", fileName, processed, files.Length))
            End If
        Next
    End Sub

    Sub SafeDelete(fileName As String)
        If File.Exists(fileName) Then
            File.SetAttributes(fileName, FileAttributes.Normal)
            File.Delete(fileName)
        End If
    End Sub
End Module
