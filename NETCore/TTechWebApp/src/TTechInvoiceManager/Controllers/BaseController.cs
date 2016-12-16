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

        public bool IsGuest()
        {
            ClaimsPrincipal claims = HttpContext.User;
            var claim = claims.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role));
            if (claim != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsExternalUser(ref UIUser usr)
        {
            ClaimsPrincipal claims = HttpContext.User;
            var claimExternal = claims.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role) && x.Value.Equals("External"));
            var claimMember = claims.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role) && x.Value.Equals("Member"));
            var claimProfilePic = claims.Claims.FirstOrDefault(x => x.Type.Equals("profile-picture"));
            if (claimExternal != null && claimMember == null)
            {
                var claimExternalId = claims.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier));
                var claimEmail = claims.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email));

                if(claimProfilePic != null)
                {
                    usr.ProfilePhotoUrl = claimProfilePic.Value;
                }

                usr.UserName = claimExternalId.Value;
                usr.Email = claimEmail.Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsMember(ref UIUser usr)
        {
            ClaimsPrincipal claims = HttpContext.User;
            var claimMember = claims.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role) && x.Value.Equals("Member"));
            if (claimMember != null)
            {
                usr = GetUserInfo();
                return true;
            }
            else
            {
                return false;
            }
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
