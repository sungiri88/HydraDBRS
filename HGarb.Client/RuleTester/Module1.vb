Module Module1

    Sub Main()
        'Dim buildRules As New HGarb.Business.Rules.BuildRule()
        'buildRules.ConstructRuleCode("Ford Credit Auto Owner Trust 2016-C")
        Dim connectionString As String = "Server=SHANKI-PC\SQLEXPRESS;Database=HGarbSuiteNew;User Id=sa;Password=123;"
        Dim evelRun As New RuleTester.EValuate.EvalRunTime("Ford Credit Auto Owner Trust", "Ford Credit Auto Owner Trust 2016-C", connectionString, "052017")
        evelRun.runRules()
    End Sub

End Module
