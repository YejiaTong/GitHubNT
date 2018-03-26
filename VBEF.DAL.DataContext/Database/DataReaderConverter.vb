Imports System.Reflection

Imports AutoMapper

Public Class DataReaderConverter(Of TDto)
    Implements ITypeConverter(Of IDataReader, TDto)

    Public Function Convert(ByVal arg_source As IDataReader, ByVal arg_destination As TDto, ByVal arg_context As ResolutionContext) As TDto Implements ITypeConverter(Of IDataReader, TDto).Convert
        If arg_destination Is Nothing Then
            Dim objType As Type = GetType(TDto)
            arg_destination = Activator.CreateInstance(objType)
        End If
        For Each objProperty As PropertyInfo In GetType(TDto).GetProperties().ToList()
            If arg_source(objProperty.Name) IsNot Nothing Then
                objProperty.SetValue(arg_destination, arg_source(objProperty.Name), Nothing)
            End If
        Next

        Return arg_destination
    End Function

End Class
