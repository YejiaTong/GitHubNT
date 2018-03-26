Imports System.Reflection

Imports AutoMapper

Public Class DataListConverter(Of T)
    Implements ITypeConverter(Of IDictionary(Of String, Object), T)

    Public Function Convert(ByVal arg_source As IDictionary(Of String, Object), ByVal arg_destination As T, ByVal arg_context As ResolutionContext) As T Implements ITypeConverter(Of IDictionary(Of String, Object), T).Convert
        If arg_destination Is Nothing Then
            Dim objType As Type = GetType(T)
            arg_destination = Activator.CreateInstance(objType)
        End If
        For Each objProperty As PropertyInfo In GetType(T).GetProperties().ToList()
            If arg_source.ContainsKey(objProperty.Name) AndAlso objProperty.CanWrite Then
                objProperty.SetValue(arg_destination, arg_source(objProperty.Name), Nothing)
            End If
        Next

        Return arg_destination
    End Function

End Class
