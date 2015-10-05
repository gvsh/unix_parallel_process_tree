Public Class LevelDataClass

    Public SasCodesList As New System.Collections.Generic.List(Of SASFileDataClass)
    Public LevelNumber As Integer

    Public Sub New(ByVal InputTreeNode As TreeNode)

        LevelNumber = InputTreeNode.Index + 1

        For Each SasCode In InputTreeNode.Nodes
            SasCodesList.Add(New SASFileDataClass(sasname:=SasCode.text, ProgramNo:=SasCode.index + 1, ParentLevel:=Me))
        Next
    End Sub
End Class
