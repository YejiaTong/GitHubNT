Imports VBEF.DAL.Object

Imports AutoMapper

Public Class ObjectDataContext

    Public Sub New()

    End Sub

    Public Function GetObject(ByVal arg_colInputs As IDictionary(Of String, Object)) As DALObject
        Dim objCfgExp As New Configuration.MapperConfigurationExpression : With objCfgExp
            .CreateMap(Of IDictionary(Of String, Object), DALObject)().ConvertUsing(Of DataListConverter(Of DALObject))()
        End With
        Dim objCfg As MapperConfiguration = New MapperConfiguration(objCfgExp)
        Dim objMapper As IMapper = objCfg.CreateMapper()

        Dim objRet As DALObject = objMapper.Map(Of DALObject)(arg_colInputs)

        Return objRet
    End Function

End Class
