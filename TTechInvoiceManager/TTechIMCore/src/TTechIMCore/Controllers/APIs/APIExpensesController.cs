using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TTechIMCore.Controllers.APIs
{
    public class APIExpensesController : Controller
    {
        [Route("apis/expenses")]
        public class APIExpensesController : Controller
        {
            // GET: /<controller>/
            /*public IActionResult Index()
            {
                return View();
            }*/

            [HttpGet]
            [Route("ping")]
            public string Ping()
            {
                return "Successful!";
            }

            [HttpGet]
            [Route("getallexpenses/{userId}/{dbInstance}")]
            public JsonResult GetAllExpenses(int userId, string dbInstance)
            {
                try
                {
                    if (userId == 0)
                    {
                        throw new APIExpensesControllerException("Invalid input [UserId]");
                    }
                    if (String.IsNullOrEmpty(dbInstance))
                    {
                        throw new APIExpensesControllerException("Invalid input [DBInstance]");
                    }

                    User usr = new User() { UserId = userId, DBInstance = dbInstance };
                    return Json(ExpensesContext.LoadUserExpenses(usr));
                }
                catch (APIExpensesControllerException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new Exception("Data Access error: " + ex.Message);
                }
            }
        }

        public class APIExpensesControllerException : Exception
        {
            public APIExpensesControllerException(string msg)
                : base(String.Format(msg)) { }
        }
    }
}
