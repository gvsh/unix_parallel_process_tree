Public Class SASFileDataClass

    Public SasFileName As String

    Public LevelNumber As Integer
    Public ProgramNumber As Integer

    Public ParentLevelNode As LevelDataClass

    Public RealMemSize As Integer
    Public MemSize As Integer
    Public SortSize As Integer

    Public RealMemSizeOption As String
    Public MemSizeOption As String
    Public SortSizeOption As String

    Public Mprint As String
    Public FullStimer As String

    Public SASCodeForThisFile As String

    Public UnixOptionsList As String
    Const quote As String = """"
    Public LandPNumber As String


    Public Sub New(ByVal sasname As String, ByVal ProgramNo As Integer, ByRef ParentLevel As LevelDataClass)

        SasFileName = sasname
        ProgramNumber = ProgramNo
        ParentLevelNode = ParentLevel
        LevelNumber = ParentLevelNode.LevelNumber

        LandPNumber = "l" & LevelNumber & "p" & ProgramNumber

        RealMemSize = 4
        MemSize = 4
        SortSize = 4

        RealMemSizeOption = RealMemSize & "G"
        MemSizeOption = MemSize & "G"
        SortSizeOption = SortSize & "G"

        Mprint = "mprint"
        FullStimer = "fullstimer"
    End Sub

    Public Function WriteStringVariableDeclarations() As String
        Return "char *lev" & LandPNumber & " = " & quote & SasFileName & quote & ";" & Environment.NewLine
    End Function

    Public Function WriteUnixOptionCode() As String

        Return "char *cmd" & LandPNumber & "[]" & " = " & _
                "{" & _
                quote & "sas" & quote & "," & _
                " lev" & LandPNumber & _
                ", " & quote & "-realmemsize" & quote & ", " & quote & RealMemSizeOption & quote & _
                ", " & quote & "-memsize" & quote & ", " & quote & MemSizeOption & quote & _
                ", " & quote & "-sortsize" & quote & ", " & quote & SortSizeOption & quote & _
                ", " & quote & "-" & Mprint & quote & ", " & quote & "-" & FullStimer & quote & ", " & _
                quote & "-errorabend" & quote & ", " & quote & "-" & "threads" & quote & _
                ", (char *)0};" & Environment.NewLine
    End Function

End Class
