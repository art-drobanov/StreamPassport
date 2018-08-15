﻿Imports System.IO
Imports System.Text
Imports StreamPassport

Public Class MainForm
    Private _sprts As StreamPassport1()
    Private _sprtsPath As String

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text += " " + My.Application.Info.Version.ToString()
        _sprtsPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "..", "data")
        RefreshSprtsListBox()
    End Sub

    Private Function GetValidSprtsFromFolder(sprtsPath As String) As StreamPassport.StreamPassport1()
        Dim sprts As New List(Of StreamPassport.StreamPassport1)
        If Not Directory.Exists(sprtsPath) Then
            Directory.CreateDirectory(sprtsPath)
        End If
        Dim sprtsPaths = Directory.EnumerateFiles(Path.GetDirectoryName(sprtsPath), "*.sprt", SearchOption.AllDirectories).ToArray()
        For Each sprtPath In sprtsPaths
            If File.Exists(sprtPath) Then
                Dim sprtText = File.ReadAllText(sprtPath)
                Dim sprt As New StreamPassport.StreamPassport1()
                Try
                    sprt.Deserialize(sprtText)
                Catch
                End Try
                If sprt.IsValid() Then
                    sprts.Add(sprt)
                End If
            End If
        Next
        Return sprts.ToArray()
    End Function

    Private Sub _orderBySizeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _orderBySizeCheckBox.CheckedChanged
        RefreshSprtsListBox()
    End Sub

    Private Sub _copyToClipboardButton_Click(sender As Object, e As EventArgs) Handles _copyToClipboardButton.Click
        Dim result As New StringBuilder
        If _sprtsListBox.SelectedIndices.Count() <> 0 Then
            For Each idx In _sprtsListBox.SelectedIndices
                Dim sprt = CType(_sprtsListBox.Items(idx), StreamPassport.StreamPassport1)
                Dim sprtString = sprt.Serialize()
                result.Append(sprtString)
                result.Append(vbCrLf)
                result.Append(vbCrLf)
            Next
        Else
            For Each sprt In _sprtsListBox.Items
                Dim sprtString = sprt.Serialize()
                result.Append(sprtString)
                result.Append(vbCrLf)
                result.Append(vbCrLf)
            Next
        End If
        Clipboard.Clear()
        Clipboard.SetText(result.ToString())
    End Sub

    Private Sub _selectAllButton_Click(sender As Object, e As EventArgs) Handles _selectAllButton.Click
        _sprtsListBox.SelectedItems.Clear()
    End Sub

    Private Sub _checkFileButton_Click(sender As Object, e As EventArgs) Handles _checkFileButton.Click
        Dim ofd = New OpenFileDialog
        With ofd
            .RestoreDirectory = True
            .AddExtension = True
            .DefaultExt = ".*"
            .Filter = "All files (*.*)|*.*"
        End With
        If ofd.ShowDialog() = DialogResult.OK Then
            If File.Exists(ofd.FileName) Then
                Using fs = File.OpenRead(ofd.FileName)
                    Dim sp = StreamPassport1.Create(Path.GetFileName(ofd.FileName), fs)
                    Dim sp2Path = ofd.FileName + ".sprt"
                    Dim okCounter As Integer = 0
                    If File.Exists(sp2Path) Then
                        Using sprtFs = File.OpenRead(sp2Path)
                            Dim sp2 = StreamPassport1.Load(sprtFs)
                            If sp.Compare(sp2, _noTotalHashCheckBox.Checked) Then
                                okCounter += 1
                                MessageBox.Show(String.Format("File '{0}' was successfully verified by stream passport from '{1}'", sp.ID, Path.GetFileName(sp2Path)), "Stream Passport Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End Using
                    End If
                    For Each sprt In _sprts
                        If sp.Compare(sprt, _noTotalHashCheckBox.Checked) Then
                            MessageBox.Show(String.Format("File '{0}' was successfully verified by stream passport from list ('..\data')", sp.ID), "Stream Passport Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            okCounter += 1
                        End If
                    Next
                    If okCounter = 0 Then
                        MessageBox.Show(String.Format("No valid stream passport for '{0}'", sp.ID), "Stream Passport Check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub _createStreamPassportButton_Click(sender As Object, e As EventArgs) Handles _createStreamPassportButton.Click
        Dim ofd = New OpenFileDialog
        With ofd
            .RestoreDirectory = True
            .AddExtension = True
            .DefaultExt = ".*"
            .Filter = "All files (*.*)|*.*"
        End With
        If ofd.ShowDialog() = DialogResult.OK Then
            If File.Exists(ofd.FileName) Then
                Using fs = File.OpenRead(ofd.FileName)
                    Dim sprt = StreamPassport1.Create(Path.GetFileName(ofd.FileName), fs)
                    Dim spPath = Path.GetDirectoryName(ofd.FileName)
                    Dim sprtPath = Path.Combine(spPath, ofd.FileName + ".sprt")
                    StreamPassportManager.SafeDelete(sprtPath)
                    File.WriteAllText(sprtPath, sprt.Serialize())
                    If _textFileOutputCheckBox.Checked Then
                        If File.Exists(sprtPath + ".txt") Then
                            StreamPassportManager.SafeDelete(sprtPath + ".txt")
                        End If
                        File.WriteAllText(sprtPath + ".txt", sprt.ToText())
                    End If
                    MessageBox.Show(String.Format("Stream Passport: '{0}'", Path.GetFileName(sprtPath)), "Stream Passport Create", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If _addToListAfterCreationCheckBox.Checked Then
                        Dim sprtTargetPath = Path.Combine(_sprtsPath, Path.GetFileName(sprtPath))
                        StreamPassportManager.SafeDelete(sprtTargetPath)
                        File.Copy(sprtPath, sprtTargetPath)
                        RefreshSprtsListBox()
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub _refreshListButton_Click(sender As Object, e As EventArgs) Handles _refreshListButton.Click
        RefreshSprtsListBox()
    End Sub

    Private Sub RefreshSprtsListBox()
        _sprts = GetValidSprtsFromFolder(_sprtsPath)
        Dim orderBySize As Boolean = False
        Me.Invoke(Sub()
                      orderBySize = _orderBySizeCheckBox.Checked
                  End Sub)
        _sprts = _sprts.OrderBy(Function(item)
                                    Return If(orderBySize, Val(item.StreamSize), item.ID)
                                End Function).ToArray()
        Me.Invoke(Sub()
                      _sprtsListBox.Items.Clear()
                      For Each sprt In _sprts
                          _sprtsListBox.Items.Add(sprt)
                      Next
                  End Sub)
    End Sub

    Private Sub _openDataFolderButton_Click(sender As Object, e As EventArgs) Handles _openDataFolderButton.Click
        If MessageBox.Show("Open stream passport's folder?", "..\DATA", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
            System.Diagnostics.Process.Start("explorer", _sprtsPath)
        End If
    End Sub
End Class
