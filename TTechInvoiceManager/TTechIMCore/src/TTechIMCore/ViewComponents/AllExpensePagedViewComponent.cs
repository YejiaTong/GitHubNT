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
    public class AllExpensePagedViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AllExpensePagerViewModel pagerModel)
        {
            AllExpenseViewModel model = null;

            if (ModelState.IsValid)
            {
                try
                {
                    long lastItemId = 0;
                    string lastItemIdStr = HttpContext.Session.Get<string>((GetSessionPrefix() + "IM_LASTITEMID"));
                    if (!String.IsNullOrEmpty(lastItemIdStr))
                    {
                        lastItemId = long.Parse(lastItemIdStr);
                    }
                    pagerModel.LastItemId = lastItemId;

                    model = new AllExpenseViewModel();
                    model.Pager = pagerModel;

                    UIUser usr = GetUserInfo();
                    var expenseCategs = LoadUserExpenseCategs(usr);
                    var list = ExpensesContext.LoadUserExpensesPagedNav(usr, pagerModel.StartTs, pagerModel.EndTs, pagerModel.PageSize, pagerModel.PageIndex).Select(x => AutoMapperFactory.ExpenseViewModel_UIExpense.CreateMapper().Map<ExpenseViewModel>(new UIExpense(x))).ToList();
                    foreach (var item in list)
                    {
                        item.ExpenseCateg = expenseCategs.FirstOrDefault(x => x.ExpenseCategId == item.ExpenseCategId);

                        lastItemId = item.ExpenseId;
                    }
                    model.Expenses = list;

                    HttpContext.Session.Put((GetSessionPrefix() + "IM_LASTITEMID"), lastItemId.ToString());
                }
                catch (Exception ex)
                {
                    ViewData["WarningMessage"] = ex.Message;
                }

                model.Pager.JsonConvertObject = JsonConvert.SerializeObject(model.Pager);

                return View(model);
            }
            else
            {
                model = new AllExpenseViewModel() { Expenses = new List<ExpenseViewModel>(), Pager = pagerModel };
                model.Pager.JsonConvertObject = JsonConvert.SerializeObject(model.Pager);

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
