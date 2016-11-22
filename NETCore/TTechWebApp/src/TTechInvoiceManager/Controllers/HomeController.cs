using System;
using System.Linq;
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
    [Authorize(Roles = "Member")]
    public class HomeController : BaseController
    {
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

        public IActionResult Account(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Placeholder
            }
            else
            {
                // Placeholder
            }

            if (model.UserId == 0)
            {
                model = AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<AccountViewModel>(GetUserInfo());
            }

            HttpContext.Session.Put(GetSessionPrefix() + "Home_Account", model);
            return View(model);
        }

        public IActionResult ChangeProfilePic(string ProfilePhotoUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usr = AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<UIUser>(HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                    usr.ProfilePhotoUrl = ProfilePhotoUrl;
                    usr.Assign(UsersContext.UpdateUserProfilePhoto(usr));

                    var model = AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<AccountViewModel>(usr);
                    HttpContext.Session.Put(GetSessionPrefix() + "Home_Account", model);
                    return View("Account", model);
                }
                catch (Exception ex)
                {
                    return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                }
            }
            else
            {
                return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
            }
        }

        public IActionResult ChangePassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(model.Password) && !String.IsNullOrEmpty(model.RetryPassword))
                {
                    if (model.Password != model.RetryPassword)
                    {
                        ViewData["PasswordErrorMessage"] = "Password not matched";

                        return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                    }
                    else if (!Regex.IsMatch(model.Password, UIUser.PasswordRegexString))
                    {
                        ViewData["PasswordErrorMessage"] = "Password does not meet the requirements";

                        return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                    }
                    else
                    {
                        try
                        {
                            UIUser usr = new UIUser();
                            usr.UserId = model.UserId;
                            usr.UserName = model.UserName;
                            usr.Email = model.Email;
                            usr.Password = model.Password;

                            UsersContext.UpdateUserPassword(usr);

                            ViewData["PasswordSuccessMessage"] = "Password was updated successfully";

                            return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                        }
                        catch (Exception ex)
                        {
                            ViewData["PasswordErrorMessage"] = ex.Message;

                            return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                        }
                    }
                }
                else
                {
                    ViewData["PasswordErrorMessage"] = "Empty password";

                    return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                }
            }
            else
            {
                return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
            }
        }

        public IActionResult EditProfile(UserDetailViewModel model)
        {
            if(ModelState.IsValid)
            {
                UIUser newUsr = AutoMapperFactory.UserDetailViewModel_UIUserMapping.CreateMapper().Map<UIUser>(model);
                UIUser existedUsr = GetUserInfo();

                if (existedUsr.Compare(newUsr))
                {
                    ViewData["ProfileErrorMessage"] = "No change was made to User Profile";

                    return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
                }
                else
                {
                    newUsr.UserId = existedUsr.UserId;
                    newUsr.UserName = existedUsr.UserName;
                    newUsr.Email = existedUsr.Email;
                    newUsr.IsActive = existedUsr.IsActive;
                    newUsr.ProfilePhotoUrl = existedUsr.ProfilePhotoUrl;
                    newUsr.SecurityToken = existedUsr.SecurityToken;
                    newUsr.DBInstance = existedUsr.DBInstance;

                    try
                    {
                        newUsr.Assign(UsersContext.UpdateUserProfile(newUsr));

                        ViewData["ProfileSuccessMessage"] = "Account info was updated successfully";

                        var accountModel = AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<AccountViewModel>(newUsr);
                        HttpContext.Session.Put(GetSessionPrefix() + "Home_Account", accountModel);
                        return View("Account", accountModel);
                    }
                    catch (Exception ex)
                    {
                        ViewData["ProfileErrorMessage"] = ex.Message;

                        return View("Account", AutoMapperFactory.AccountViewModel_UIUserMapping.CreateMapper().Map<AccountViewModel>(existedUsr));
                    }
                }
            }
            else
            {
                return View("Account", HttpContext.Session.Get<AccountViewModel>(GetSessionPrefix() + "Home_Account"));
            }
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Setting(SettingViewModel model)
        {
            if (Request.Method.Equals("GET"))
            {
                model = new SettingViewModel();

                UIUser usr = GetUserInfo();
                model = new SettingViewModel();
                model.SiteMaps = SiteMapsContext.LoadAllSiteMaps().Select(x => AutoMapperFactory.SiteMapViewModel_UISiteMap.CreateMapper().Map<SiteMapViewModel>(new UISiteMap(x))).ToList();
                var item = model.SiteMaps.FirstOrDefault(x => x.SiteMapController.Equals(usr.DefaultController) && x.SiteMapView.Equals(usr.DefaultView));
                item.IsSelected = true;

                return View(model);
            }
            else
            {
                UIUser usr = GetUserInfo();
                SiteMapViewModel item = model.SiteMaps.FirstOrDefault(x => x.IsSelected);
                if(item == null)
                {
                    ViewData["ErrorMessage"] = "Unhandled exception: item not found";
                }
                else
                {
                    bool update = false;
                    try
                    {
                        update = SiteMapsContext.UpdateUserDefaultView(usr, AutoMapperFactory.SiteMapViewModel_UISiteMap.CreateMapper().Map<UISiteMap>(item));
                    }
                    catch (Exception ex)
                    {
                        ViewData["ErrorMessage"] = ex.Message;
                    }
                    
                    if(update)
                    {
                        ViewData["SuccessMessage"] = "Successfully updated the User Front Page";
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Unhandled exception: failed to update the record";
                    }
                }

                return View(model);
            }
        }

        public IActionResult LoginRouter()
        {
            UIUser usr = GetUserInfo();

            return RedirectToAction(usr.DefaultView, usr.DefaultController);

            //return View();
        }

        public IActionResult Invoice()
        {
            ViewData["Message"] = "To be expected...!!!";

            return View();
        }
    }
}
