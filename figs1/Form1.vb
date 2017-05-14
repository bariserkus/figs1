Public Class Form1
    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

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
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListBox1.SelectedItems.Count = 1 Then
            WebBrowser1.Navigate(ListBox1.SelectedItem.ToString)
        Else
            MessageBox.Show("Select only one file", "Error")
        End If
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As Object, e As EventArgs)

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
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
