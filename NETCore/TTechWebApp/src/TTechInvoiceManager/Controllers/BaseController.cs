using System;
using System.Linq;
using System.Collections.Generic;

using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using NTWebApp.Models.InvoiceManager;

using NTWebApp.DBAccess;
using NTWebApp.UIClasses;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NTWebApp.Controllers
{
    public class BaseController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public UIUser GetUserInfo()
        {
            UIUser ret = new UIUser();

            ClaimsPrincipal claims = HttpContext.User;
            var claim = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            int id = Int32.Parse(claim.Value);
            ret.Assign(UsersContext.GetUserById(id));

            return ret;
        }

        public string GetSessionPrefix()
        {
            ClaimsPrincipal claims = HttpContext.User;
            var claim = claims.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            string prefix = claim.Value;

            return prefix;
        }

        public List<ExpenseCategViewModel> LoadUserExpenseCategs(UIUser usr)
        {
            return ExpenseCategsContext.LoadUserExpenseCategs(usr).Select(x => AutoMapperFactory.ExpenseCategViewModel_UIExpenseCateg.CreateMapper().Map<ExpenseCategViewModel>(new UIExpenseCateg(x))).ToList();
        }
    }
}
