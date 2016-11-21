using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;

using NTWebApp.DBAccess;
using NTWebApp.UIClasses;
using NTWebApp.Models.Home;
using NTWebApp.Models.Account;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NTWebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "To be expected...";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "To be expected...";

            return View();
        }

        public IActionResult Login()
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }

        public async Task<JsonResult> ValidateLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UIUser usr = new UIUser();
                usr.UserName = model.LoginId;
                usr.Email = model.LoginId;
                usr.Password = model.Password;

                try
                {
                    try
                    {
                        usr.Assign(UsersContext.ValidateUser(usr));
                    }
                    catch (Exception ex)
                    {
                        ViewData["LoginErrorMessage"] = ex.Message;

                        return this.Json("Login info does not pass validation");
                        //return View("Login");
                    }

                    // User  Authentication handling
                    const string Issuer = "Noah Tong";
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, usr.FirstName + " " + usr.LastName, ClaimValueTypes.String, Issuer));
                    claims.Add(new Claim(ClaimTypes.Role, "Member", ClaimValueTypes.String, Issuer));
                    claims.Add(new Claim(ClaimTypes.UserData, usr.UserId.ToString(), ClaimValueTypes.Integer32, Issuer));
                    var userIdentity = new ClaimsIdentity("SecureLogin");
                    userIdentity.AddClaims(claims);
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.Authentication.SignInAsync("CookieMiddlewareInstance", userPrincipal,
                        new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            AllowRefresh = false
                        });

                    var obj = AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<AccountViewModel>(usr);

                    return this.Json("Pass---" + obj.DefaultController + "/" + obj.DefaultView);
                    //return RedirectToAction("Account", "Home", obj);
                }
                catch (Exception ex)
                {
                    ViewData["LoginErrorMessage"] = ex.Message;

                    return this.Json("Login info does not pass validation");
                    //return View("Login");
                }
            }
            else
            {
                return this.Json("Login info does not pass validation");
            }
        }

        /*public async Task<IActionResult> ValidateLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UIUser usr = new UIUser();
                usr.UserName = model.LoginId;
                usr.Email = model.LoginId;
                usr.Password = model.Password;
                try
                {
                    try
                    {
                        usr.Assign(UsersContext.ValidateUser(usr));
                    }
                    catch (Exception ex)
                    {
                        ViewData["LoginErrorMessage"] = ex.Message;

                        return View("Login");
                    }

                    // User  Authentication handling
                    const string Issuer = "Noah Tong";
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, usr.FirstName + " " + usr.LastName, ClaimValueTypes.String, Issuer));
                    claims.Add(new Claim(ClaimTypes.Role, "Member", ClaimValueTypes.String, Issuer));
                    claims.Add(new Claim(ClaimTypes.UserData, usr.UserId.ToString(), ClaimValueTypes.Integer32, Issuer));
                    var userIdentity = new ClaimsIdentity("SecureLogin");
                    userIdentity.AddClaims(claims);
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.Authentication.SignInAsync("CookieMiddlewareInstance", userPrincipal,
                        new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            AllowRefresh = false
                        });

                    var obj = AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<AccountViewModel>(usr);

                    return RedirectToAction("Account", "Home", obj);
                }
                catch (Exception ex)
                {
                    ViewData["LoginErrorMessage"] = ex.Message;

                    return View("Login");
                }
            }
            else
            {
                return View("Login");
            }
        }*/

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CookieMiddlewareInstance");
            return View("LogoutRed");
        }

        public IActionResult LogoutRed()
        {
            return View();
        }

        public IActionResult Register()
        {
            if(ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult RegisterUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(model.UserName))
                    {
                        ViewData["WarningMessage"] = "<strong>Username</strong> is empty";
                    }
                    else if (String.IsNullOrEmpty(model.Email))
                    {
                        ViewData["WarningMessage"] = "<strong>Email</strong> is empty";
                    }
                    else
                    {
                        UIUser usr = new UIUser();
                        usr.UserName = model.UserName;
                        usr.Email = model.Email;

                        UsersContext.RegisterUser(usr);
                        ViewData["SuccessMessage"] = "An email will reach <strong>" + model.Email + "</strong> shortly with your temporary passcode";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["WarningMessage"] = ex.Message;
                }

                return View("Register");
            }
            else
            {
                return View("Register");
            }
        }

        public IActionResult ForgotPassword()
        {
            if(ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult ExeForgotPassword(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(model.UserName))
                    {
                        ViewData["WarningMessage"] = "<strong>Username</strong> is empty";
                    }
                    else if (String.IsNullOrEmpty(model.Email))
                    {
                        ViewData["WarningMessage"] = "<strong>Email</strong> is empty";
                    }
                    else
                    {
                        UIUser usr = new UIUser();
                        usr.UserName = model.UserName;
                        usr.Email = model.Email;

                        UsersContext.ForgotPassword(usr);

                        ViewData["SuccessMessage"] = "An email will reach <strong>" + usr.Email + "</strong> shortly with your reset temporary passcode";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["WarningMessage"] = ex.Message;
                }

                return View("ForgotPassword");
            }
            else
            {
                return View("ForgotPassword");
            }
        }

        public IActionResult MessageBoard(MessageBoardMsgViewModel model)
        {
            if (Request.Method.Equals("GET"))
            {
                ModelState.Clear();

                if (!User.Identities.Any(u => u.IsAuthenticated))
                {
                    model = new MessageBoardMsgViewModel();

                    return View(model);
                }
                else
                {
                    model = new MessageBoardMsgViewModel();

                    UIUser usr = GetUserInfo();
                    model.Name = usr.UserName;
                    model.Email = usr.Email;

                    return View(model);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    UIUser usr = null;

                    if (!User.Identities.Any(u => u.IsAuthenticated))
                    {

                    }
                    else
                    {
                        usr = GetUserInfo();
                    }

                    try
                    {
                        MessageBoardMsgsContext.AddNewMessageBoardMessage(AutoMapperFactory.MessageBoardMsgViewModel_UIMessageBoardMsg.CreateMapper().Map<UIMessageBoardMsg>(model), usr);

                        ViewData["MessageBoardSuccessMessage"] = "Thank you for your comments!";
                    }
                    catch(Exception ex)
                    {
                        ViewData["MessageBoardErrorMessage"] = ex.Message;
                    }

                    model = new MessageBoardMsgViewModel();

                    if (usr != null)
                    {
                        model.Name = usr.UserName;
                        model.Email = usr.Email;
                    }

                    model.Message = String.Empty;

                    ModelState.Clear();

                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
        }

        public IActionResult Forbidden(string errorMessage)
        {
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "Unexpected Error:" + Environment.NewLine + errorMessage;

                return View();
            }
            else
            {
                ViewData["Message"] = "!!! Something goes wrong. We're sorry but please go back to the Home/Login page...";

                return View();
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Account", "Home");
            }
        }
    }
}
