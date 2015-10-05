
Public Class GenerateCodesMainProgram
    Implements CodeStructureInterface

    Private BeginningCodeString As String
    Private EndCodeString As String
    Private MiddleCodeString As String

    Private VariableDeclarationsString As String

    Private TabStringPrefix As String
    Private FullCode As String

    Public LevelList As New System.Collections.Generic.List(Of LevelDataClass)

    Const quote As String = """"

    Public Function WriteFullCode()

        TabStringPrefix = vbTab

        For Each LevelNodes As TreeNode In Form1.LevelStructure.Nodes(0).Nodes
            LevelList.Add(New LevelDataClass(LevelNodes))
        Next

        BeginningCodeString = My.Resources.BeginCode.ToString
        MiddleCodeString = WriteCodeForVariableDeclarations()
        EndCodeString = My.Resources.EndCode.ToString()

        Dim TabStringForFork As String = vbTab
        For Each CurrentLevel As LevelDataClass In LevelList
            For Each CurrentSasCode As SASFileDataClass In CurrentLevel.SasCodesList
                ForkAChild(BeginCode:=BeginningCodeString, MiddleCode:=MiddleCodeString, EndCode:=EndCodeString, LandP:=CurrentSasCode.LandPNumber, ParentTabPosition:=TabStringForFork)
            Next
            TabStringForFork = TabStringForFork & vbTab
            MiddleCodeString = MiddleCodeString & WaitForChildren(WaitLevel:=CurrentLevel, EndCode:=EndCodeString, WaitTabPosition:=TabStringForFork) & Environment.NewLine
            MiddleCodeString = MiddleCodeString & CheckStatusOfChildren(WaitLevel:=CurrentLevel, EndCode:=EndCodeString, WaitTabPosition:=TabStringForFork) & Environment.NewLine
        Next

        Return BeginningCodeString & Environment.NewLine & MiddleCodeString & Environment.NewLine & EndCodeString
    End Function

    Public Sub ForkAChild(ByRef BeginCode As String, ByRef MiddleCode As String, ByRef EndCode As String, ByVal LandP As String, ByVal ParentTabPosition As String)

        Dim ChildComment As String = "/* In child */" & Environment.NewLine
        Dim ParentComment As String = "/* In parent */" & Environment.NewLine

        MiddleCode = MiddleCode & Environment.NewLine & _
        ParentTabPosition & "if ((pid" & LandP & " = fork()) < 0) then " & Environment.NewLine & _
        ParentTabPosition & vbTab & "err_sys(" & quote & "fork error\n" & quote & ");" & Environment.NewLine & _
        ParentTabPosition & "else if (pid" & LandP & " == 0) {" & Environment.NewLine & _
        ParentTabPosition & vbTab & ChildComment & _
        ParentTabPosition & vbTab & "if (execvp(" & quote & "sas" & quote & ", cmd" & LandP & ") < 0) then" & Environment.NewLine & _
        ParentTabPosition & vbTab & vbTab & "err_sys(" & quote & "execvp error\n" & quote & ");" & Environment.NewLine & _
        ParentTabPosition & "}" & Environment.NewLine & _
        ParentTabPosition & "else if (pid" & LandP & " > 0) {" & Environment.NewLine & _
        ParentTabPosition & vbTab & ParentComment & Environment.NewLine

        EndCode = ParentTabPosition & "}" & Environment.NewLine & EndCode

    End Sub

    Public Function WaitForChildren(ByRef WaitLevel As LevelDataClass, ByRef EndCode As String, ByVal WaitTabPosition As String)

        Dim WaitString As String
        WaitString = Nothing

        For Each CurrentWaitSasCode As SASFileDataClass In WaitLevel.SasCodesList

            If WaitLevel.SasCodesList.Count = 1 Then
                WaitString = WaitString & WaitTabPosition & "if (waitpid(pid" & CurrentWaitSasCode.LandPNumber & ", &sts" & CurrentWaitSasCode.LandPNumber & ", 0) != pid" & CurrentWaitSasCode.LandPNumber & ") {" & Environment.NewLine & _
                                WaitTabPosition & vbTab & "err_sys(" & quote & "waitpid error" & quote & ");" & Environment.NewLine & _
                                WaitTabPosition & "}" & Environment.NewLine
                WaitString = WaitString & WaitTabPosition & "else {" & Environment.NewLine
                Dim CompletionComment As String = "/* All Level " & WaitLevel.LevelNumber & " jobs have completed*/"
                WaitString = WaitString & Environment.NewLine & WaitTabPosition & CompletionComment & Environment.NewLine
                EndCode = WaitTabPosition & "}" & Environment.NewLine & EndCode
            ElseIf CurrentWaitSasCode.ProgramNumber = 1 Then
                WaitString = WaitString & WaitTabPosition & "if (waitpid(pid" & CurrentWaitSasCode.LandPNumber & ", &sts" & CurrentWaitSasCode.LandPNumber & ", 0) != pid" & CurrentWaitSasCode.LandPNumber & ") {" & Environment.NewLine & _
                WaitTabPosition & vbTab & "err_sys(" & quote & "waitpid error" & quote & ");" & Environment.NewLine & _
                WaitTabPosition & "}" & Environment.NewLine
            ElseIf CurrentWaitSasCode.ProgramNumber < WaitLevel.SasCodesList.Count Then
                WaitString = WaitString & WaitTabPosition & "else if (waitpid(pid" & CurrentWaitSasCode.LandPNumber & ", &sts" & CurrentWaitSasCode.LandPNumber & ", 0) != pid" & CurrentWaitSasCode.LandPNumber & ") {" & Environment.NewLine & _
                WaitTabPosition & vbTab & "err_sys(" & quote & "waitpid error" & quote & ");" & Environment.NewLine & _
                WaitTabPosition & "}" & Environment.NewLine
            ElseIf CurrentWaitSasCode.ProgramNumber = WaitLevel.SasCodesList.Count Then
                WaitString = WaitString & WaitTabPosition & "else if (waitpid(pid" & CurrentWaitSasCode.LandPNumber & ", &sts" & CurrentWaitSasCode.LandPNumber & ", 0) != pid" & CurrentWaitSasCode.LandPNumber & ") {" & Environment.NewLine & _
                                WaitTabPosition & vbTab & "err_sys(" & quote & "waitpid error" & quote & ");" & Environment.NewLine & _
                                WaitTabPosition & "}" & Environment.NewLine
                WaitString = WaitString & WaitTabPosition & "else {" & Environment.NewLine
                Dim CompletionComment As String = "/* All Level " & WaitLevel.LevelNumber & " jobs have completed*/"
                WaitString = WaitString & Environment.NewLine & WaitTabPosition & CompletionComment & Environment.NewLine
                EndCode = WaitTabPosition & "}" & Environment.NewLine & EndCode
            End If
        Next
        Return WaitString

    End Function

    Public Function CheckStatusOfChildren(ByRef WaitLevel As LevelDataClass, ByRef EndCode As String, ByVal WaitTabPosition As String)

        Dim WaitString As String
        WaitString = Nothing

        For Each CurrentWaitSasCode As SASFileDataClass In WaitLevel.SasCodesList

        Next
        Return WaitString

        'if (sts0101 > 4) {
        '	pr_exit(sts0101);
        '	printf("Error in Job %s\n", lev0101);
        '	Level1Err = 1;
        '} else {
        '	printf("%s completed successfully \n", lev0101);
        '}
    End Function

    Public Function WriteCodeForVariableDeclarations()

        Dim comment1 As String = "/* Create aliases for each program name in each level*/"
        VariableDeclarationsString = TabStringPrefix & comment1 & Environment.NewLine

        For Each CurrentLevel As LevelDataClass In LevelList
            For Each CurrentSasCode As SASFileDataClass In CurrentLevel.SasCodesList
                VariableDeclarationsString = VariableDeclarationsString & TabStringPrefix & CurrentSasCode.WriteStringVariableDeclarations()
            Next
        Next

        Dim comment2 As String = "/* Create commands which will be used in execvp statements*/"
        VariableDeclarationsString = VariableDeclarationsString & Environment.NewLine & TabStringPrefix & comment2 & Environment.NewLine

        For Each CurrentLevel As LevelDataClass In LevelList
            For Each CurrentSasCode As SASFileDataClass In CurrentLevel.SasCodesList
                VariableDeclarationsString = VariableDeclarationsString & TabStringPrefix & CurrentSasCode.WriteUnixOptionCode()
            Next
        Next

        Dim comment3 As String = "/* Create pid variables for each sas code in each level*/"
        VariableDeclarationsString = VariableDeclarationsString & Environment.NewLine & TabStringPrefix & comment3 & Environment.NewLine

        Dim tempi As Integer = 0
        VariableDeclarationsString = VariableDeclarationsString & TabStringPrefix & "pid_t" & vbTab
        For Each CurrentLevel As LevelDataClass In LevelList
            For Each CurrentSasCode As SASFileDataClass In CurrentLevel.SasCodesList
                If tempi = 0 Then
                    VariableDeclarationsString = VariableDeclarationsString & "pid" & CurrentSasCode.LandPNumber
                    tempi = 1
                Else
                    VariableDeclarationsString = VariableDeclarationsString & ", pid" & CurrentSasCode.LandPNumber
                End If

            Next
        Next
        VariableDeclarationsString = VariableDeclarationsString & ";" & Environment.NewLine

        Dim comment4 As String = "/* Create status variables for each sas code in each level and Level Error Indicators for each Level*/"
        VariableDeclarationsString = VariableDeclarationsString & Environment.NewLine & TabStringPrefix & comment4 & Environment.NewLine
        Dim tempi2 As Integer = 0
        VariableDeclarationsString = VariableDeclarationsString & TabStringPrefix & "int" & vbTab
        For Each CurrentLevel As LevelDataClass In LevelList
            If tempi2 = 0 Then
                VariableDeclarationsString = VariableDeclarationsString & "Level" & CurrentLevel.LevelNumber & "Err"
                tempi2 = 1
            Else
                VariableDeclarationsString = VariableDeclarationsString & ", Level" & CurrentLevel.LevelNumber & "Err"
            End If
            For Each CurrentSasCode As SASFileDataClass In CurrentLevel.SasCodesList
                VariableDeclarationsString = VariableDeclarationsString & ", sts" & CurrentSasCode.LandPNumber
            Next
        Next
        VariableDeclarationsString = VariableDeclarationsString & ";" & Environment.NewLine

        Dim comment5 As String = "/* Initialize Level Error Indicators for each Level*/"
        VariableDeclarationsString = VariableDeclarationsString & Environment.NewLine & TabStringPrefix & comment5 & Environment.NewLine
        For Each CurrentLevel As LevelDataClass In LevelList
            VariableDeclarationsString = VariableDeclarationsString & TabStringPrefix & "Level" & CurrentLevel.LevelNumber & "Err" & " = 0;" & Environment.NewLine
        Next

        Return VariableDeclarationsString
    End Function
End Class
