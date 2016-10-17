using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json;

using NTWebApp.Models.InvoiceManager;
using NTWebApp.DBAccess;
using NTWebApp.UIClasses;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NTWebApp.Controllers
{
    [Authorize(Roles = "Member")]
    public class InvoiceManagerController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddExpense(AddExpenseViewModel model)
        {
            if(!Request.Method.Equals("GET") && !Request.Form["submit"].Equals("SubmitExpenses"))
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                if (Request.Method.Equals("GET"))
                {
                    if (model.Expenses == null)
                    {
                        model = GetInitialCntlrAddExpenseItem();
                    }

                    return View(model);
                }
                else
                {
                    int index = 0;
                    if (model.Expenses == null)
                    {
                        model.Expenses = new List<ExpenseViewModel>();
                    }

                    if (Request.Form["submit"].Equals("AddNewItem"))
                    {
                        return AddNewItem(model);
                    }
                    else if (Request.Form["submit"].Equals("SubmitExpenses"))
                    {
                        return SubmitExpenses(model);
                    }
                    else if (Int32.TryParse(Request.Form["submit"], out index))
                    {
                        return RemoveItem(model, index);
                    }
                    else
                    {
                        throw new Exception("Oooops!!! Unhandled expection occurs...");
                    }
                }
            }
            else
            {
                if (model.ExpenseCategs == null)
                {
                    model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());
                }

                return View(model);
            }
        }

        public IActionResult AddNewItem(AddExpenseViewModel model)
        {
            if (model.Expenses == null)
            {
                model = GetInitialCntlrAddExpenseItem();
            }
            else
            {
                model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());
                model.Expenses.Add(GetInitialUIExpenseItem());
            }

            ModelState.Clear();

            return View(model);
        }

        public IActionResult SubmitExpenses(AddExpenseViewModel model)
        {
            if (model.Expenses == null || model.Expenses.Count() == 0)
            {
                ViewBag.WarningMessage = "No item found";
            }
            else
            {
                foreach (var item in model.Expenses)
                {
                    if (item.ExpenseCategId == 0)
                    {
                        ViewData["WarningMessage"] = "It is found that the <strong>Expense Category</strong> is missing";
                        model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());

                        return View(model);
                    }

                    if (String.IsNullOrEmpty(item.Name))
                    {
                        ViewData["WarningMessage"] = "It is found that the Expense is <strong>Empty</strong>";
                        model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());

                        return View(model);
                    }

                    item.Name = item.Name.Trim();

                    if (item.ExpenseCategId == 0)
                    {
                        ViewData["WarningMessage"] = "Please select a valid <strong>Expense Category</strong>";
                        model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());

                        return View(model);
                    }

                    if (String.IsNullOrEmpty(item.Name))
                    {
                        ViewData["WarningMessage"] = "It is found that the Expense is <strong>Empty</strong>";
                        model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());

                        return View(model);
                    }
                }

                try
                {
                    ExpensesContext.AddNewExpenses(model.Expenses.Select(x => AutoMapperFactory.ExpenseViewModel_UIExpense.CreateMapper().Map<UIExpense>(x)).Cast<Expense>().ToList(), GetUserInfo());
                }
                catch (Exception ex)
                {
                    ViewData["WarningMessage"] = ex.Message;
                    model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());

                    return View(model);
                }
            }

            model.Expenses = new List<ExpenseViewModel>();
            if (String.IsNullOrEmpty(ViewBag.WarningMessage))
            {
                ViewData["SuccessMessage"] = "New items were added successfully...";
            }

            ModelState.Clear();

            return View(model);
        }

        public IActionResult RemoveItem(AddExpenseViewModel model, int index)
        {
            if (model.Expenses == null)
            {
                model = GetInitialCntlrAddExpenseItem();
            }
            else
            {
                model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());
                model.Expenses.RemoveAt(index);
            }

            ModelState.Clear();

            return View(model);
        }

        public IActionResult ManageExpenseCateg(ManageExpenseCategViewModel model)
        {
            if(!Request.Method.Equals("GET") && !Request.Form["submit"].Equals("Confirm") && !Request.Form["submit"].Equals("Update"))
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                if (Request.Method.Equals("GET"))
                {
                    if (model.ExistingExpenseCategs == null)
                    {
                        model = GetInitialCntlrManageExpenseCateg();
                    }

                    return View(model);
                }
                else
                {
                    int index = 0;
                    if (model.ExistingExpenseCategs == null)
                    {
                        model.ExistingExpenseCategs = LoadUserExpenseCategs(GetUserInfo());
                    }
                    if (model.NewExpenseCategs == null)
                    {
                        model.NewExpenseCategs = new List<ExpenseCategViewModel>();
                    }

                    if (Request.Form["submit"].Equals("Add"))
                    {
                        return AddNewExpenseCategItem(model);
                    }
                    else if (Request.Form["submit"].Equals("Confirm"))
                    {
                        return ConfirmNewExpenseCategItem(model);
                    }
                    else if (Request.Form["submit"].Equals("Update"))
                    {
                        return UpdateExpenseCategs(model);
                    }
                    else
                    {
                        string submitType = Request.Form["submit"];

                        if (submitType.Contains("existing_"))
                        {
                            string indexStr = submitType.Remove(submitType.IndexOf("existing_"), "existing_".Length);
                            if (Int32.TryParse(indexStr, out index))
                            {
                                return RemoveExpenseCategItem(model, "existing_", index);
                            }
                            else
                            {
                                throw new Exception("Oooops!!! Unhandled expection occurs...");
                            }
                        }
                        else if (submitType.Contains("new_"))
                        {
                            string indexStr = submitType.ToString().Remove(submitType.ToString().IndexOf("new_"), "new_".Length);
                            if (Int32.TryParse(indexStr, out index))
                            {
                                return RemoveExpenseCategItem(model, "new_", index);
                            }
                            else
                            {
                                throw new Exception("Oooops!!! Unhandled expection occurs...");
                            }
                        }
                        else
                        {
                            throw new Exception("Oooops!!! Unhandled expection occurs...");
                        }
                    }
                }
            }
            else
            {
                if (model.ExistingExpenseCategs == null)
                {
                    model.ExistingExpenseCategs = LoadUserExpenseCategs(GetUserInfo());
                }

                return View(model);
            }
        }

        public IActionResult AddNewExpenseCategItem(ManageExpenseCategViewModel model)
        {
            model.NewExpenseCategs.Add(new ExpenseCategViewModel());

            ModelState.Clear();

            return View(model);
        }

        public IActionResult ConfirmNewExpenseCategItem(ManageExpenseCategViewModel model)
        {
            if (model.NewExpenseCategs == null || model.NewExpenseCategs.Count() == 0)
            {
                ViewData["NewItemWarningMessage"] = "No item found";
            }
            else
            {
                foreach (var item in model.NewExpenseCategs)
                {
                    if (String.IsNullOrEmpty(item.ExpenseCategName))
                    {
                        ViewData["NewItemWarningMessage"] = "It is found that the Expense Category is <strong>Empty</strong>";

                        return View(model);
                    }

                    item.ExpenseCategName = item.ExpenseCategName.Trim();
                    item.OrderVal = item.OrderVal == 0 ? 1 : item.OrderVal;

                    if (String.IsNullOrEmpty(item.ExpenseCategName))
                    {
                        ViewData["NewItemWarningMessage"] = "It is found that the Expense Category is <strong>Empty</strong>";

                        return View(model);
                    }
                }

                if (model.ExistingExpenseCategs != null && model.ExistingExpenseCategs.Count() != 0)
                {
                    foreach (var item in model.NewExpenseCategs)
                    {
                        var obj = model.ExistingExpenseCategs.FirstOrDefault(x => String.Equals(x.ExpenseCategName, item.ExpenseCategName, StringComparison.OrdinalIgnoreCase));
                        if (obj != null)
                        {
                            ViewData["NewItemWarningMessage"] = "Expense Category: <strong>" + obj.ExpenseCategName + "</strong> aleady exists";

                            return View(model);
                        }
                    }
                }

                try
                {
                    UIUser usr = GetUserInfo();
                    ExpenseCategsContext.AddNewExpenseCategs(model.NewExpenseCategs.Select(x => AutoMapperFactory.ExpenseCategViewModel_UIExpenseCateg.CreateMapper().Map<UIExpenseCateg>(x)).Cast<ExpenseCateg>().ToList(), usr);
                    model.ExistingExpenseCategs = LoadUserExpenseCategs(usr);
                    model.NewExpenseCategs = new List<ExpenseCategViewModel>();
                }
                catch (Exception ex)
                {
                    ViewData["NewItemWarningMessage"] = ex.Message;

                    return View(model);
                }
            }
            if (ViewData["NewItemWarningMessage"] == null)
            {
                ViewData["SuccessMessage"] = "New items were added successfully...";
            }

            ModelState.Clear();

            return View(model);
        }

        public IActionResult UpdateExpenseCategs(ManageExpenseCategViewModel model)
        {
            if (model.ExistingExpenseCategs == null || model.ExistingExpenseCategs.Count() == 0)
            {
                ViewData["ExistingItemWarningMessage"] = "No item found";
            }
            else
            {
                foreach (var item in model.ExistingExpenseCategs)
                {
                    if (String.IsNullOrEmpty(item.ExpenseCategName))
                    {
                        ViewData["ExistingItemWarningMessage"] = "It is found that the Expense Category is <strong>Empty</strong>";

                        return View(model);
                    }

                    item.ExpenseCategName = item.ExpenseCategName.Trim();
                    item.OrderVal = item.OrderVal == 0 ? 1 : item.OrderVal;

                    if (String.IsNullOrEmpty(item.ExpenseCategName))
                    {
                        ViewData["ExistingItemWarningMessage"] = "It is found that the Expense Category is <strong>Empty</strong>";

                        return View(model);
                    }
                }

                foreach (var item in model.ExistingExpenseCategs)
                {
                    int num = model.ExistingExpenseCategs.Where(x => String.Equals(x.ExpenseCategName, item.ExpenseCategName, StringComparison.OrdinalIgnoreCase)).Count();
                    if (num != 1)
                    {
                        ViewData["ExistingItemWarningMessage"] = "Duplicated Expense Category: <strong>" + item.ExpenseCategName + "</strong> aleady exists";

                        return View(model);
                    }
                }

                try
                {
                    UIUser usr = GetUserInfo();
                    ExpenseCategsContext.UpdateExpenseCategs(model.ExistingExpenseCategs.Select(x => AutoMapperFactory.ExpenseCategViewModel_UIExpenseCateg.CreateMapper().Map<UIExpenseCateg>(x)).Cast<ExpenseCateg>().ToList(), usr);
                    model.ExistingExpenseCategs = LoadUserExpenseCategs(usr);
                    model.NewExpenseCategs = new List<ExpenseCategViewModel>();
                }
                catch (Exception ex)
                {
                    ViewData["ExistingItemWarningMessage"] = ex.Message;

                    return View(model);
                }
            }
            if (ViewData["ExistingItemWarningMessage"] == null)
            {
                ViewData["SuccessMessage"] = "Items were updated successfully...";
            }

            ModelState.Clear();

            return View(model);
        }

        public IActionResult RemoveExpenseCategItem(ManageExpenseCategViewModel model, string type, int index)
        {
            try
            {
                switch (type)
                {
                    case "existing_":
                        model.ExistingExpenseCategs.RemoveAt(index);
                        break;
                    case "new_":
                        model.NewExpenseCategs.RemoveAt(index);
                        break;
                    default:
                        throw new Exception("Oooops!!! Unhandled expection occurs...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ModelState.Clear();

            return View(model);
        }

        public IActionResult AllExpense(AllExpensePagerViewModel model, string jsonConvertObject, int pageIndex)
        {
            if (Request.Method.Equals("GET") && String.IsNullOrEmpty(jsonConvertObject) && pageIndex == 0)
            {
                ModelState.Clear();

                HttpContext.Session.Put((GetSessionPrefix() + "IM_LASTITEMID"), model.LastItemId.ToString());

                return View(model);
            }
            else if(!String.IsNullOrEmpty(jsonConvertObject) && pageIndex != 0)
            {
                model = JsonConvert.DeserializeObject<AllExpensePagerViewModel>(jsonConvertObject);
                model.PageIndex = pageIndex;
            }
            
            if(ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult MonthView(MonthViewPagerViewModel model, int Year, int Month)
        {
            ViewData["Message"] = "To be expected...";

            if (Request.Method.Equals("GET") && Year == 0)
            {
                ModelState.Clear();

                model.Year = Year;
                model.Month = Month;

                return View(model);
            }
            else if(Year != 0)
            {
                model.Year = Year;
                model.Month = Month;

                return View(model);
            }

            return View(model);
        }

        public IActionResult WeekView(WeekViewPagerViewModel model, int Year, int Month, int Week)
        {
            ViewData["Message"] = "To be expected...";

            return View(model);
        }

        private ExpenseViewModel GetInitialUIExpenseItem()
        {
            return new ExpenseViewModel();
        }

        private ManageExpenseCategViewModel GetInitialCntlrManageExpenseCateg()
        {
            ManageExpenseCategViewModel model = new ManageExpenseCategViewModel();
            model.ExistingExpenseCategs = LoadUserExpenseCategs(GetUserInfo());
            model.NewExpenseCategs = new List<ExpenseCategViewModel>();

            return model;
        }

        private AddExpenseViewModel GetInitialCntlrAddExpenseItem()
        {
            AddExpenseViewModel model = new AddExpenseViewModel();
            model.Expenses = new List<ExpenseViewModel>();
            model.ExpenseCategs = LoadUserExpenseCategs(GetUserInfo());

            return model;
        }
    }
}
