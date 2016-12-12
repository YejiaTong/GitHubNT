using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using MVCWebApp.UIClasses;

namespace MVCWebApp.Controllers
{
    /*
     * RESTful API Layer
     * api/ping
     * Ping web service
     * 
     * api/getDiffInDays/{dateOne}/{dateTwo}
     * Get difference in days with absolute value
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class APIController : Controller
    {
        // GET: /<controller>/
        /*public IActionResult Index()
        {
            return View();
        }*/

        /*
        * Ping RESTful APIs
        * Route: api/ping
        * */
        [HttpGet]
        [Route("api/ping")]
        public string Ping()
        {
            return "Successful!";
        }

        /*
         * Date input format: YYYYMMDD
         * Route: api/getDiffInDays/{dateOne}/{dateTwo}
         * */
        [HttpGet]
        [Route("api/getDiffInDays/{dateOne}/{dateTwo}")]
        public string GetDiffInDays(string dateOne, string dateTwo)
        {
            try
            {
                int diff = UINETDate.Parse(dateOne) - UINETDate.Parse(dateTwo);
                return diff.ToString();
            }
            catch (Exception ex)
            {
                return "Unexpected failure: " + ex.Message;
            }
        }
    }
}
