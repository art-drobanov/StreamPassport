Imports System.IO
Imports System.Text
Imports StreamPassport

Public Class MainForm
    Private _sprts As IStreamPassport()
    Private _sprtsPath As String

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text += " " + My.Application.Info.Version.ToString()
        _sprtsPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "..", "data")
        RefreshSprtsListBox()
    End Sub

    Private Sub _orderBySizeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _orderBySizeCheckBox.CheckedChanged
        RefreshSprtsListBox()
    End Sub

    Private Sub _copyToClipboardButton_Click(sender As Object, e As EventArgs) Handles _copyToClipboardButton.Click
        Dim result As New StringBuilder
        If _sprtsListBox.SelectedIndices.Count() <> 0 Then
            For Each idx In _sprtsListBox.SelectedIndices
                Dim sprt = CType(_sprtsListBox.Items(idx), IStreamPassport)
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
            StreamPassportManager.CheckFilePassport(ofd.FileName, StreamPassportType.SHA256, _noTotalHashCheckBox.Checked, _sprts, AddressOf MessageOutHandler)
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
            Dim sprtPath = StreamPassportManager.CreateFilePassport(ofd.FileName, StreamPassportType.SHA256, _textFileOutputCheckBox.Checked, AddressOf MessageOutHandler)
            If _addToListAfterCreationCheckBox.Checked Then
                Dim sprtTargetPath = Path.Combine(_sprtsPath, Path.GetFileName(sprtPath))
                StreamPassportManager.SafeDelete(sprtTargetPath)
                File.Copy(sprtPath, sprtTargetPath)
                RefreshSprtsListBox()
            End If
        End If
    End Sub

    Private Sub _refreshListButton_Click(sender As Object, e As EventArgs) Handles _refreshListButton.Click
        RefreshSprtsListBox()
    End Sub

    Private Sub RefreshSprtsListBox()
        _sprts = StreamPassportManager.GetValidSprtsFromFolder(_sprtsPath, StreamPassportType.SHA256)
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

    Private Sub _openStreamPassportsStorageFolderButton_Click(sender As Object, e As EventArgs) Handles _openStreamPassportsStorageFolderButton.Click
        If MessageBox.Show("Open stream passport's folder?", "..\data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.OK Then
            System.Diagnostics.Process.Start("explorer", _sprtsPath)
        End If
    End Sub

    Private Sub MessageOutHandler(msg As String, title As String, isInfo As Boolean, isExclamation As Boolean, isError As Boolean)
        MessageBox.Show(msg, title, MessageBoxButtons.OK, If(isInfo, MessageBoxIcon.Information, If(isExclamation, MessageBoxIcon.Exclamation, MessageBoxIcon.Error)))
    End Sub
End Class
