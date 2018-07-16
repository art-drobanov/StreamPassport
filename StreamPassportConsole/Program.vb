Imports System.IO
Imports System.Windows.Forms
Imports StreamPassport

Module Program
    Private Const _ext = ".sprt"
    Private Const _ext2 = ".sprt2"
    Private Const _corruptedExt = ".corrupted"
    Private Const _okExt = ".ok"
    Private Const _txt = ".txt"

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
            Dim exeName = Path.GetFileName(Application.ExecutablePath)
            files = Directory.EnumerateFiles(Path.GetDirectoryName(Application.ExecutablePath), "*.*",
                                             If(isRecursive, SearchOption.AllDirectories, SearchOption.TopDirectoryOnly)).Where(Function(item)
                                                                                                                                    Return Not item.EndsWith(_ext) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2) AndAlso
                                                                                                                                         Not item.EndsWith(_ext + _corruptedExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2 + _corruptedExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext + _okExt) AndAlso
                                                                                                                                         Not item.EndsWith(_ext2 + _okExt) AndAlso
                                                                                                                                         Not item.EndsWith(exeName)
                                                                                                                                End Function).ToArray()
        End If
        Dim processed As Integer = 0
        For Each fileName In files
            If File.Exists(fileName) Then
                Using fs = File.OpenRead(fileName)

                    'SPRT1
                    Dim sp1Loaded As StreamPassport.StreamPassport1 = Nothing
                    Try
                        sp1Loaded = StreamPassport1Manager.Load(fs)
                    Catch
                    End Try
                    If sp1Loaded IsNot Nothing Then
                        If useTextOutput Then
                            If File.Exists(fileName + _ext + _txt) Then
                                SafeDelete(fileName + _ext + _txt)
                            End If
                            File.WriteAllText(fileName + _ext + _txt, sp1Loaded.ToText())
                        End If
                    Else
                        Dim sprt = StreamPassport1Manager.Create(Path.GetFileName(fileName), fs) 'sprt
                        If File.Exists(fileName + _ext) Then
                            Using sprtFs = File.OpenRead(fileName + _ext)
                                Dim sp2 = StreamPassport1Manager.Load(sprtFs)
                                If sprt.Compare(sp2, noTotalHash) Then
                                    SafeDelete(fileName + _ext + _corruptedExt)
                                    File.Create(fileName + _ext + _okExt)
                                Else
                                    SafeDelete(fileName + _ext + _okExt)
                                    File.Create(fileName + _ext + _corruptedExt)
                                End If
                            End Using
                        Else
                            File.WriteAllText(fileName + _ext, sprt.Serialize())
                            If useTextOutput Then
                                If File.Exists(fileName + _ext + _txt) Then
                                    SafeDelete(fileName + _ext + _txt)
                                End If
                                File.WriteAllText(fileName + _ext + _txt, sprt.ToText())
                            End If
                        End If
                    End If

                    'SPRT2
                    Dim sp2Loaded As StreamPassport.StreamPassport2 = Nothing
                    Try
                        sp2Loaded = StreamPassport2Manager.Load(fs)
                    Catch
                    End Try
                    If sp2Loaded IsNot Nothing Then
                        If useTextOutput Then
                            If File.Exists(fileName + _ext + _txt) Then
                                SafeDelete(fileName + _ext + _txt)
                            End If
                            File.WriteAllText(fileName + _ext + _txt, sp2Loaded.ToText())
                        End If
                    Else
                        If useSprt2 Then 'sprt2
                            Dim sprt2 = StreamPassport2Manager.Create(Path.GetFileName(fileName), fs)
                            If File.Exists(fileName + _ext2) Then
                                Using sprtFs = File.OpenRead(fileName + _ext2)
                                    Dim sp2 = StreamPassport2Manager.Load(sprtFs)
                                    If sprt2.Compare(sp2, noTotalHash) Then
                                        SafeDelete(fileName + _ext2 + _corruptedExt)
                                        File.Create(fileName + _ext2 + _okExt)
                                    Else
                                        SafeDelete(fileName + _ext2 + _okExt)
                                        File.Create(fileName + _ext2 + _corruptedExt)
                                    End If
                                End Using
                            Else
                                File.WriteAllText(fileName + _ext2, sprt2.Serialize())
                                If useTextOutput Then
                                    If File.Exists(fileName + _ext2 + _txt) Then
                                        SafeDelete(fileName + _ext2 + _txt)
                                    End If
                                    File.WriteAllText(fileName + _ext2 + _txt, sprt2.ToText())
                                End If
                            End If
                        End If
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
