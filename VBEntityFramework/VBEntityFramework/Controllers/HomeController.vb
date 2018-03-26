Imports System.Net.Http
Imports System.Web.Http

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    <HttpGet()>
    <ActionName("VBMVCVersion")>
    Function VBMVCVersion() As ActionResult
        Response.StatusCode = Net.HttpStatusCode.OK
        Return Content(GetType(Controller).Assembly.GetName.Version.ToString())
    End Function

    Function Index() As ActionResult
        Return View()
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your application description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
End Class
