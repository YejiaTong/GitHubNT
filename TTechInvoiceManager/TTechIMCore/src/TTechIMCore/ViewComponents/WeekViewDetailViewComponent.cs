using System;
using System.Linq;
using System.Collections.Generic;

using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using TTechIMCore.Models.InvoiceManager;
using TTechIMCore.DBAccess;
using TTechIMCore.UIClasses;

namespace TTechIMCore.ViewComponents
{
    public class WeekViewDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(WeekViewPagerViewModel pagerModel)
        {
            WeekViewViewModel model = null;

            if (ModelState.IsValid)
            {
                try
                {
                    model = new WeekViewViewModel();
                    model.Pager = pagerModel;

                    UIUser usr = GetUserInfo();
                    var list = WeekExpensesContext.LoadUserWeekExpenses(usr, pagerModel.SelectedWeek.WeekStartTs, pagerModel.SelectedWeek.WeekEndTs).Select(x => AutoMapperFactory.WeekViewItemViewModel_UIWeekExpense.CreateMapper().Map<WeekViewItemViewModel>(new UIWeekExpense(x))).ToList();
                    model.WeekViewItems = list;
                    if (list.Count() != 0)
                    {
                        model.WeekTotalCost = list.Select(x => x.TotalCost).Sum();
                    }
                }
                catch (Exception ex)
                {
                    ViewData["WarningMessage"] = ex.Message;
                }

                return View(model);
            }
            else
            {
                model = new WeekViewViewModel();
                model.WeekViewItems = new List<WeekViewItemViewModel>();
                model.Pager = pagerModel;

                return View(model);
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
