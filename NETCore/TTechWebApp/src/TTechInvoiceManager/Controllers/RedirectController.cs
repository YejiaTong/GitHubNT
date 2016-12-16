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

namespace NTWebApp.Controllers
{
    [AllowAnonymous]
    public class RedirectController : BaseController
    {
        public async Task<IActionResult> LoginRouter()
        {
            if(IsGuest())
            {
                return RedirectToAction("Login", "Account");
            }

            UIUser usr = new UIUser();
            if (IsExternalUser(ref usr))
            {
                var refUsr = UsersContext.ValidateExternalUser(usr, "External");
                if(refUsr.UserId != 0)
                {
                    usr.Assign(refUsr);

                    // User Authentication handling
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
                            IsPersistent = true,
                            AllowRefresh = false
                        });

                    return RedirectToAction("LoginRed", "Redirect");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else if(IsMember(ref usr))
            {
                return RedirectToAction(usr.DefaultView, usr.DefaultController);
            }
            else
            {
                return RedirectToAction("Forbidden", "Account");
            }

            //return View();
        }

        [Authorize(Roles = "External")]
        public async Task<IActionResult> RegisterThroughExternal()
        {
            try
            {
                UIUser usr = new UIUser();
                if (IsExternalUser(ref usr))
                {
                    if (usr.UserId == 0)
                    {
                        UsersContext.RegisterExternalUser(usr, "External");

                        usr.Assign(UsersContext.ValidateExternalUser(usr, "External"));

                        if(usr.UserId != 0)
                        {
                            // User Authentication handling
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
                                    IsPersistent = true,
                                    AllowRefresh = false
                                });

                            return RedirectToAction("LoginRed", "Redirect");
                        }
                    }
                }

                throw new Exception("Account Synchronization failed. Please retry or contact us.");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult LoginRed()
        {
            return View();
        }

        public IActionResult LogoutRed()
        {
            return View();
        }
    }
}
