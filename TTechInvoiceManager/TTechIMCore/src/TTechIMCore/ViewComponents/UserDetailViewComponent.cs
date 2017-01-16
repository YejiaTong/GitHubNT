using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TTechIMCore.Models.Home;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TTechIMCore.ViewComponents
{
    public class UserDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
    }
}
