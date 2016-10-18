using System;
using System.Linq;
using System.Collections.Generic;

using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using NTWebApp.Models.InvoiceManager;

using NTWebApp.DBAccess;
using NTWebApp.UIClasses;

namespace NTWebApp.ViewComponents
{
    public class MonthViewDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MonthViewPagerViewModel pagerModel)
        {
            MonthViewViewModel model = null;

            if (ModelState.IsValid)
            {
                try
                {
                    model = new MonthViewViewModel();
                    model.Pager = pagerModel;

                    UIUser usr = GetUserInfo();
                    var list = MonthExpensesContext.LoadUserMonthExpenses(usr, pagerModel.SelectedMonth.MonthStartTs, pagerModel.SelectedMonth.MonthEndTs).Select(x => AutoMapperFactory.MonthViewItemViewModel_UIMonthExpense.CreateMapper().Map<MonthViewItemViewModel>(new UIMonthExpense(x))).ToList();
                    model.MonthViewItems = list;
                    if(list.Count() != 0)
                    {
                        model.MonthTotalCost = list.Select(x => x.TotalCost).Sum();
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
                model = new MonthViewViewModel();
                model.MonthViewItems = new List<MonthViewItemViewModel>();
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
