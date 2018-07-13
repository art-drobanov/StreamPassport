<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me._sprtsGroupBox = New System.Windows.Forms.GroupBox()
        Me._openDataFolderButton = New System.Windows.Forms.Button()
        Me._orderBySizeCheckBox = New System.Windows.Forms.CheckBox()
        Me._sprtsListBox = New System.Windows.Forms.ListBox()
        Me._copyToClipboardButton = New System.Windows.Forms.Button()
        Me._checkFileButton = New System.Windows.Forms.Button()
        Me._selectAllButton = New System.Windows.Forms.Button()
        Me._createStreamPassportButton = New System.Windows.Forms.Button()
        Me._addToListAfterCreationCheckBox = New System.Windows.Forms.CheckBox()
        Me._refreshListButton = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me._noTotalHashCheckBox = New System.Windows.Forms.CheckBox()
        Me._textFileOutputCheckBox = New System.Windows.Forms.CheckBox()
        Me._sprtsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        '_sprtsGroupBox
        '
        Me._sprtsGroupBox.BackColor = System.Drawing.Color.YellowGreen
        Me._sprtsGroupBox.Controls.Add(Me._openDataFolderButton)
        Me._sprtsGroupBox.Controls.Add(Me._orderBySizeCheckBox)
        Me._sprtsGroupBox.Controls.Add(Me._sprtsListBox)
        Me._sprtsGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me._sprtsGroupBox.Location = New System.Drawing.Point(12, 12)
        Me._sprtsGroupBox.Name = "_sprtsGroupBox"
        Me._sprtsGroupBox.Size = New System.Drawing.Size(1134, 483)
        Me._sprtsGroupBox.TabIndex = 1
        Me._sprtsGroupBox.TabStop = False
        Me._sprtsGroupBox.Text = "Valid Stream Passports in '..\data'"
        '
        '_openDataFolderButton
        '
        Me._openDataFolderButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me._openDataFolderButton.Location = New System.Drawing.Point(881, 0)
        Me._openDataFolderButton.Name = "_openDataFolderButton"
        Me._openDataFolderButton.Size = New System.Drawing.Size(144, 19)
        Me._openDataFolderButton.TabIndex = 2
        Me._openDataFolderButton.Text = "OPEN DATA FOLDER"
        Me._openDataFolderButton.UseVisualStyleBackColor = True
        '
        '_orderBySizeCheckBox
        '
        Me._orderBySizeCheckBox.AutoSize = True
        Me._orderBySizeCheckBox.Location = New System.Drawing.Point(1031, 0)
        Me._orderBySizeCheckBox.Name = "_orderBySizeCheckBox"
        Me._orderBySizeCheckBox.Size = New System.Drawing.Size(97, 19)
        Me._orderBySizeCheckBox.TabIndex = 3
        Me._orderBySizeCheckBox.Text = "Order by size"
        Me._orderBySizeCheckBox.UseVisualStyleBackColor = True
        '
        '_sprtsListBox
        '
        Me._sprtsListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._sprtsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me._sprtsListBox.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me._sprtsListBox.FormattingEnabled = True
        Me._sprtsListBox.Location = New System.Drawing.Point(6, 19)
        Me._sprtsListBox.Name = "_sprtsListBox"
        Me._sprtsListBox.ScrollAlwaysVisible = True
        Me._sprtsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me._sprtsListBox.Size = New System.Drawing.Size(1121, 457)
        Me._sprtsListBox.TabIndex = 2
        '
        '_copyToClipboardButton
        '
        Me._copyToClipboardButton.Image = CType(resources.GetObject("_copyToClipboardButton.Image"), System.Drawing.Image)
        Me._copyToClipboardButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._copyToClipboardButton.Location = New System.Drawing.Point(771, 504)
        Me._copyToClipboardButton.Name = "_copyToClipboardButton"
        Me._copyToClipboardButton.Size = New System.Drawing.Size(189, 48)
        Me._copyToClipboardButton.TabIndex = 11
        Me._copyToClipboardButton.Text = "Copy Passport(s) To Clipboard"
        Me._copyToClipboardButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._copyToClipboardButton.UseVisualStyleBackColor = True
        '
        '_checkFileButton
        '
        Me._checkFileButton.Image = CType(resources.GetObject("_checkFileButton.Image"), System.Drawing.Image)
        Me._checkFileButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._checkFileButton.Location = New System.Drawing.Point(12, 504)
        Me._checkFileButton.Name = "_checkFileButton"
        Me._checkFileButton.Size = New System.Drawing.Size(144, 48)
        Me._checkFileButton.TabIndex = 4
        Me._checkFileButton.Text = "Check File"
        Me._checkFileButton.UseVisualStyleBackColor = True
        '
        '_selectAllButton
        '
        Me._selectAllButton.Image = CType(resources.GetObject("_selectAllButton.Image"), System.Drawing.Image)
        Me._selectAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._selectAllButton.Location = New System.Drawing.Point(604, 504)
        Me._selectAllButton.Name = "_selectAllButton"
        Me._selectAllButton.Size = New System.Drawing.Size(161, 48)
        Me._selectAllButton.TabIndex = 10
        Me._selectAllButton.Text = "No Selection (Get ALL)"
        Me._selectAllButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._selectAllButton.UseVisualStyleBackColor = True
        '
        '_createStreamPassportButton
        '
        Me._createStreamPassportButton.Image = CType(resources.GetObject("_createStreamPassportButton.Image"), System.Drawing.Image)
        Me._createStreamPassportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._createStreamPassportButton.Location = New System.Drawing.Point(162, 504)
        Me._createStreamPassportButton.Name = "_createStreamPassportButton"
        Me._createStreamPassportButton.Size = New System.Drawing.Size(164, 48)
        Me._createStreamPassportButton.TabIndex = 5
        Me._createStreamPassportButton.Text = "Create Stream Passport"
        Me._createStreamPassportButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._createStreamPassportButton.UseVisualStyleBackColor = True
        '
        '_addToListAfterCreationCheckBox
        '
        Me._addToListAfterCreationCheckBox.AutoSize = True
        Me._addToListAfterCreationCheckBox.Location = New System.Drawing.Point(332, 537)
        Me._addToListAfterCreationCheckBox.Name = "_addToListAfterCreationCheckBox"
        Me._addToListAfterCreationCheckBox.Size = New System.Drawing.Size(137, 17)
        Me._addToListAfterCreationCheckBox.TabIndex = 8
        Me._addToListAfterCreationCheckBox.Text = "Add to list after creation"
        Me._addToListAfterCreationCheckBox.UseVisualStyleBackColor = True
        '
        '_refreshListButton
        '
        Me._refreshListButton.Image = CType(resources.GetObject("_refreshListButton.Image"), System.Drawing.Image)
        Me._refreshListButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._refreshListButton.Location = New System.Drawing.Point(492, 504)
        Me._refreshListButton.Name = "_refreshListButton"
        Me._refreshListButton.Size = New System.Drawing.Size(106, 48)
        Me._refreshListButton.TabIndex = 9
        Me._refreshListButton.Text = "Refresh List"
        Me._refreshListButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._refreshListButton.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(966, 504)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(180, 13)
        Me.LinkLabel1.TabIndex = 9
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "http://www.webdesignerdepot.com/"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(966, 525)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(122, 13)
        Me.LinkLabel2.TabIndex = 10
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "http://graphicrating.com"
        '
        '_noTotalHashCheckBox
        '
        Me._noTotalHashCheckBox.AutoSize = True
        Me._noTotalHashCheckBox.Location = New System.Drawing.Point(332, 504)
        Me._noTotalHashCheckBox.Name = "_noTotalHashCheckBox"
        Me._noTotalHashCheckBox.Size = New System.Drawing.Size(122, 17)
        Me._noTotalHashCheckBox.TabIndex = 6
        Me._noTotalHashCheckBox.Text = "No total hash check"
        Me._noTotalHashCheckBox.UseVisualStyleBackColor = True
        '
        '_textFileOutputCheckBox
        '
        Me._textFileOutputCheckBox.AutoSize = True
        Me._textFileOutputCheckBox.Checked = True
        Me._textFileOutputCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me._textFileOutputCheckBox.Location = New System.Drawing.Point(332, 520)
        Me._textFileOutputCheckBox.Name = "_textFileOutputCheckBox"
        Me._textFileOutputCheckBox.Size = New System.Drawing.Size(144, 17)
        Me._textFileOutputCheckBox.TabIndex = 7
        Me._textFileOutputCheckBox.Text = "Add output to the text file"
        Me._textFileOutputCheckBox.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1157, 561)
        Me.Controls.Add(Me._textFileOutputCheckBox)
        Me.Controls.Add(Me._noTotalHashCheckBox)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me._refreshListButton)
        Me.Controls.Add(Me._addToListAfterCreationCheckBox)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me._createStreamPassportButton)
        Me.Controls.Add(Me._selectAllButton)
        Me.Controls.Add(Me._checkFileButton)
        Me.Controls.Add(Me._copyToClipboardButton)
        Me.Controls.Add(Me._sprtsGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Stream Passport GUI"
        Me._sprtsGroupBox.ResumeLayout(False)
        Me._sprtsGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents _sprtsGroupBox As GroupBox
    Friend WithEvents _sprtsListBox As ListBox
    Friend WithEvents _copyToClipboardButton As Button
    Friend WithEvents _orderBySizeCheckBox As CheckBox
    Friend WithEvents _checkFileButton As Button
    Friend WithEvents _selectAllButton As Button
    Friend WithEvents _createStreamPassportButton As Button
    Friend WithEvents _addToListAfterCreationCheckBox As CheckBox
    Friend WithEvents _refreshListButton As Button
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents LinkLabel2 As LinkLabel
    Friend WithEvents _openDataFolderButton As Button
    Friend WithEvents _noTotalHashCheckBox As CheckBox
    Friend WithEvents _textFileOutputCheckBox As CheckBox
End Class
