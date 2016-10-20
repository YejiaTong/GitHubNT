using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NTWebApp.ViewComponents
{
    public class LoginWidgetViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
}
