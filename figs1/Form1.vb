Public Class Form1
    Dim optSet As Integer = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If ListBox1.SelectedItems.Count > 0 Then
            ' MUST loop backwards thru collections when removing
            ' or you will remove the wrong things, miss stuff and
            ' run out early
            For n As Integer = ListBox1.SelectedItems.Count - 1 To 0 Step -1
                ' remove the current selected item from items
                ListBox1.Items.Remove(ListBox1.SelectedItems(n))
            Next n
        End If

        UpdateLists()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        ListBox1.Items.Clear()
        UpdateLists()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        PreviewFile(1)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            'Dim sr As New System.IO.StreamReader(OpenFileDialog1.FileName)
            'MessageBox.Show(sr.ReadToEnd)
            'SR.Close()
            For Each file In OpenFileDialog1.FileNames
                ListBox1.Items.Add(file)
            Next file
        End If

        UpdateLists()

        'WebBrowser1.DocumentText = OpenFileDialog1.FileNames.Length.ToString
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim selIndL1 As Integer
        Dim selItems As Integer

        selItems = ListBox1.SelectedItems.Count
        ListBox2.ClearSelected()
        ListBox3.ClearSelected()
        ListBox4.ClearSelected()
        ListBox5.ClearSelected()

        If selItems > 0 Then
            For n As Integer = 0 To selItems - 1
                selIndL1 = ListBox1.SelectedIndices(n)
                ListBox2.SetSelected(selIndL1, True)
                ListBox3.SetSelected(selIndL1, True)
                ListBox4.SetSelected(selIndL1, True)
                ListBox5.SetSelected(selIndL1, True)
                'WebBrowser1.DocumentText = ListBox1.SelectedIndices(1).ToString
            Next n
        End If

        PreviewFileCheckBox()

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        PreviewFileCheckBox()

    End Sub

    Public Sub PreviewFile(ByVal fileType As Integer)

        Dim myTikzFile As New TikzFile
        Dim textSelected As String
        Dim itemsSelected As String
        Dim fileToPreview As String = " "
        Dim fileInfo As System.IO.FileInfo

        itemsSelected = ListBox1.SelectedItems.Count

        If itemsSelected = 1 Then
            textSelected = ListBox1.SelectedItem.ToString
            myTikzFile.FullName = textSelected
            If fileType = 1 Then
                fileToPreview = myTikzFile.TikzFileName
            ElseIf fileType = 2 Then
                fileToPreview = myTikzFile.PdfFileName
            ElseIf fileType = 3 Then
                fileToPreview = myTikzFile.TexFileName
            ElseIf fileType = 4 Then
                fileToPreview = myTikzFile.OptsFileName
            End If

            fileInfo = My.Computer.FileSystem.GetFileInfo(fileToPreview)
            Label2.Text = "Preview file:" & fileToPreview

            If fileInfo.Length < TextBox1.Text * 1024 Then
                WebBrowser1.Navigate(fileToPreview)
            Else
                WebBrowser1.DocumentText = "File size too big"
            End If
        ElseIf itemsSelected <> 1 Then
            WebBrowser1.DocumentText = "Only 1 file has to be selected"
        End If

    End Sub

    Public Sub PreviewFileCheckBox()

        Dim fileInfo As System.IO.FileInfo

        If ListBox1.SelectedItems.Count = 1 And CheckBox1.Checked = True Then
            fileInfo = My.Computer.FileSystem.GetFileInfo(ListBox1.SelectedItem.ToString)
            Label2.Text = "Preview file: " & ListBox1.SelectedItem.ToString
            If fileInfo.Length < TextBox1.Text * 1024 Then
                WebBrowser1.Navigate(ListBox1.SelectedItem.ToString)
            Else
                WebBrowser1.DocumentText = "File size too big"
            End If
        ElseIf CheckBox1.Checked = True And ListBox1.SelectedItems.Count <> 1 Then
            WebBrowser1.DocumentText = "Only 1 file has to be selected"
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

        If TextBox1.Text > 1000 Then
            TextBox1.Text = 1000
        End If

        PreviewFileCheckBox()

    End Sub

    Private Sub UpdateLists()

        Dim nFiles As Integer = ListBox1.Items.Count
        Dim pdfExist(nFiles - 1) As Integer
        Dim texExist(nFiles - 1) As Integer
        Dim i As Integer
        Dim myTikzFile As New TikzFile

        ListBox2.Items.Clear()
        ListBox3.Items.Clear()
        ListBox4.Items.Clear()
        If optSet = 0 Then
            ListBox5.Items.Clear()
        End If

        For i = 0 To nFiles - 1

            ListBox2.Items.Add(i + 1)
            myTikzFile.FullName = ListBox1.Items(i)

            If System.IO.File.Exists(myTikzFile.PdfFileName) Then
                ListBox3.Items.Add("PDF")
            Else
                ListBox3.Items.Add("None")
            End If

            If System.IO.File.Exists(myTikzFile.TexFileName) Then
                ListBox4.Items.Add("TEX")
            Else
                ListBox4.Items.Add("None")
            End If
            If optSet = 0 Then
                ListBox5.Items.Add("None")
            End If
        Next

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        RunPdfLatex()
    End Sub

    Public Class TikzFile

        Public FullName As String

        ReadOnly Property FileName() As String
            Get
                Return System.IO.Path.GetFileNameWithoutExtension(FullName)
            End Get
        End Property

        ReadOnly Property Path() As String
            Get
                Return System.IO.Path.GetDirectoryName(FullName)
            End Get
        End Property

        ReadOnly Property Seperator() As String
            Get
                Return System.IO.Path.DirectorySeparatorChar
            End Get
        End Property

        ReadOnly Property TikzFileName() As String
            Get
                Return Path & Seperator & FileName & ".tikz"
            End Get
        End Property

        ReadOnly Property PdfFileName() As String
            Get
                Return Path & Seperator & FileName & ".pdf"
            End Get
        End Property

        ReadOnly Property TexFileName() As String
            Get
                Return Path & Seperator & FileName & ".tex"
            End Get
        End Property

        ReadOnly Property OptsFileName() As String
            Get
                Return Path & Seperator & FileName & ".opts"
            End Get
        End Property


    End Class

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        UpdateLists()

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        Dim nFiles As Integer

        If OpenFileDialog2.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            For Each file In OpenFileDialog2.FileNames
                ListBox6.Items.Add(file)
            Next file
        End If

        nFiles = ListBox6.Items.Count
        ListBox7.Items.Clear()

        For i = 0 To nFiles - 1
            ListBox7.Items.Add(i + 1)
        Next

        'WebBrowser1.DocumentText = OpenFileDialog1.FileNames.Length.ToString
    End Sub

    Private Sub ListBox6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox6.SelectedIndexChanged

        ListBox7.SelectedIndex = ListBox6.SelectedIndex

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim fileList As Integer
        Dim optList As UInteger
        Dim itemsSelected As Integer
        Dim optSelected As Integer
        Dim indSelected As Integer

        fileList = ListBox5.Items.Count
        optList = ListBox7.Items.Count

        If fileList > 0 And optList > 0 Then

            itemsSelected = ListBox1.SelectedItems.Count
            optSelected = ListBox7.SelectedItems.Count

            If itemsSelected > 0 And optSelected = 1 Then
                For n As Integer = 0 To itemsSelected - 1
                    indSelected = ListBox1.SelectedIndices(n)
                    ListBox5.Items(indSelected) = ListBox7.SelectedItem
                Next n
            Else
                WebBrowser1.DocumentText = "Select files and or options"
            End If
        Else
            WebBrowser1.DocumentText = "Add files and/or options"
        End If
        optSet = 1
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        GenTeX()

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim itemCount As Integer

        itemCount = ListBox1.Items.Count

        For i = 0 To itemCount - 1
            ListBox1.SetSelected(i, True)
        Next

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        PreviewFile(2)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        PreviewFile(3)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Dim optIndex As Integer
        Dim optFile As String
        Dim selIndex As Integer
        Dim itemsSelected As Integer
        Dim optsCount As Integer
        Dim msg As String

        optsCount = ListBox6.Items.Count
        itemsSelected = ListBox5.SelectedItems.Count
        msg = ""

        If optsCount = 0 Then
            WebBrowser1.DocumentText = "Add options file(s)"
        Else
            If itemsSelected = 1 Then
                selIndex = ListBox5.SelectedIndex
                Try
                    optIndex = ListBox5.SelectedItem
                Catch ex As Exception
                    msg = "Option set to 1 for the selected file"
                    ListBox5.Items(selIndex) = 1
                Finally
                    Label10.Text = Label10.Text & " " & msg
                    optIndex = ListBox5.SelectedItem
                    optFile = ListBox6.Items(optIndex - 1)
                    WebBrowser1.Navigate(optFile)
                    Label2.Text = "Preview file: " & optFile
                End Try
            ElseIf itemsSelected <> 1 Then
                WebBrowser1.DocumentText = "Only 1 file has to be selected"
            End If
        End If

    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        PreviewOptFile()
    End Sub

    Public Sub PreviewOptFile()

        Dim myTikzFile As New TikzFile
        Dim fileToPreview As String
        Dim fileInfo As System.IO.FileInfo

        fileToPreview = ListBox6.SelectedItem.ToString

        fileInfo = My.Computer.FileSystem.GetFileInfo(fileToPreview)
        Label2.Text = "Preview file:" & fileToPreview

        If fileInfo.Length < TextBox1.Text * 1024 Then
            WebBrowser1.Navigate(fileToPreview)
        Else
            WebBrowser1.DocumentText = "File size too big"
        End If

    End Sub

    Public Sub GenTeX()
        Dim m As New TikzFile
        Dim nFiles As Integer = ListBox1.Items.Count
        Dim optIndex As Integer
        Dim optFile As String
        Dim fileList As Integer
        Dim optList As UInteger
        Dim itemsSelected As Integer
        Dim optSelected As Integer

        fileList = ListBox1.Items.Count
        optList = ListBox7.Items.Count

        If fileList > 0 And optList > 0 Then

            itemsSelected = ListBox1.SelectedItems.Count
            optSelected = ListBox7.SelectedItems.Count

            If itemsSelected > 0 And optSelected = 1 Then
                For i = 0 To nFiles - 1

                    m.FullName = ListBox1.Items(i)
                    optIndex = Convert.ToInt32(ListBox5.Items(i))
                    optFile = ListBox6.Items(optIndex - 1)
                    System.IO.File.Delete(m.TexFileName)
                    My.Computer.FileSystem.CopyFile(optFile, m.TexFileName, True)

                    Using sw As System.IO.StreamWriter = System.IO.File.AppendText(m.TexFileName)
                        sw.WriteLine()
                        sw.WriteLine("\begin{document}")
                        sw.WriteLine("\input{" & m.FileName & ".tikz}")
                        sw.WriteLine("\end{document}")
                    End Using

                Next
                Label11.Text = "Completed"
            Else
                WebBrowser1.DocumentText = "Select files and or options"
            End If
        Else
            WebBrowser1.DocumentText = "Add files and/or options"
        End If
        optSet = 1


    End Sub


    Sub RunPdfLatex()

        Dim myTikzFile As New TikzFile
        Dim pi = New ProcessStartInfo()

        If ListBox1.SelectedItems.Count > 0 Then
            For n As Integer = ListBox1.SelectedItems.Count - 1 To 0 Step -1
                myTikzFile.FullName = ListBox1.Items(n)
                pi.WorkingDirectory = myTikzFile.Path
                pi.FileName = "pdflatex"
                pi.Arguments = myTikzFile.TexFileName
                Process.Start(pi)
            Next n
        End If

        UpdateLists()
        PreviewFileCheckBox()
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        GenTeX()
        RunPdfLatex()
    End Sub
End Class