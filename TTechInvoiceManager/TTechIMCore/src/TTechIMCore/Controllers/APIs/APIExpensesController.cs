using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

using TTechIMCore.DBAccess;

/*
 * Public API Layer for TTech IM
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.Controllers.APIs
{
    [Route("apis/expenses")]
    public class APIExpensesController : Controller
    {
        // GET: /<controller>/
        /*public IActionResult Index()
        {
            return View();
        }*/

        // GET: ../ping/
        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "Successful!";
        }

        // GET: ../getallexpenses/{userId}/{dbInstance}
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

    // Exception: Unexpexted error at API Layer
    public class APIExpensesControllerException : Exception
    {
        public APIExpensesControllerException(string msg)
            : base(String.Format(msg)) { }
    }
}
