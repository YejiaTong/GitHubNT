Imports System.Data.SqlClient

Imports AutoMapper

Public Module DataReader

    Private Const STR_SQL_CONNECTION As String = ""

    Public Function ReadData(Of T)(ByVal arg_queryString As String) As List(Of T)
        Dim objCfgExp As New Configuration.MapperConfigurationExpression : With objCfgExp
            .CreateMap(Of IDataReader, T)().ConvertUsing(Of DataReaderConverter(Of T))()
        End With
        Dim objMapper As IMapper = New MapperConfiguration(objCfgExp).CreateMapper()
        Dim colRet As List(Of T) = New List(Of T)()

        Using objConn As New SqlConnection(STR_SQL_CONNECTION)
            objConn.Open()

            Using objCmd As New SqlCommand(arg_queryString, objConn)
                Try
                    With objCmd
                        .Connection = objConn
                        .CommandType = CommandType.Text
                    End With

                    Using ObjReader As SqlDataReader = objCmd.ExecuteReader()
                        If ObjReader.HasRows Then
                            While ObjReader.Read()
                                Dim objItem As T = objMapper.Map(Of T)(ObjReader)
                                colRet.Add(objItem)
                            End While
                        Else
                            Return Nothing
                        End If
                    End Using

                Catch ex As Exception
                    ' Exception handling
                End Try
            End Using
        End Using

        Return colRet
    End Function

End Module
