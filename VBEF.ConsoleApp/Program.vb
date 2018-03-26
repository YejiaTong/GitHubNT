Imports VBEF.DAL.Object
Imports VBEF.DAL.DataContext

Module Program

    Sub Main()
        Console.WriteLine("*** VB Entity Framework PoC ***")

        Try
            Console.WriteLine("- Attempt to convert")

            Dim colInputs As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
            colInputs("ID") = 1
            colInputs("Name") = "Object"

            Dim objDataContext As ObjectDataContext = New ObjectDataContext()
            Dim objItem As DALObject = objDataContext.GetObject(colInputs)

            Console.WriteLine(String.Format("- Result: {0}, {1}", objItem.ID, objItem.Name))
        Catch ex As Exception
            Console.WriteLine(String.Format("- Unexpected Error: {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace))
        Finally

        End Try

        Console.WriteLine("*** End... Press any key to exit. ***")
        Console.ReadKey()
    End Sub

End Module
