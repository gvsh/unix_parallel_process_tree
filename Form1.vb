
Public Class Form1
    Private fullcode As String

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Button1.Enabled = False

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'MsgBox(LevelStructure.Nodes(0).Nodes(0).Text)
        Dim GenerateCodesMainProgramInstance As New GenerateCodesMainProgram
        'System.Diagnostics.Debug.WriteLine(GenerateCodesMainProgramInstance.WriteFullCode)
        Try
            My.Computer.FileSystem.WriteAllText( _
                                            file:="C:\Users\Gaurav\Desktop\OrderingCode.c", _
                                            text:=GenerateCodesMainProgramInstance.WriteFullCode, _
                                            append:=False)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        PropertyGrid1.Visible = True
        LevelStructure.Visible = False
        Button1.Enabled = False
        Button3.Enabled = True
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        PropertyGrid1.Visible = False
        Button1.Enabled = True
        LevelStructure.Visible = True
        Button3.Enabled = False
    End Sub
End Class
