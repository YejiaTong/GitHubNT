using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using MVCWebApp.UIClasses;
using MVCWebApp.Models.Home;

namespace MVCWebApp.Controllers
{
    /*
     * Controller Layer - Home
     * Index:
     * [GET]
     * Front Page
     * [POST]
     * Get difference in days with absolute value
     * 
     * Api:
     * [GET]
     * Api Page - review applicable APIs
     * 
     * Error:
     * [GET]
     * Error Page - Failure callback
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Message"] = "Your main page.";

            ModelState.Clear();

            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel model)
        {
            ViewData["Message"] = "Your main page.";

            if (ModelState.IsValid)
            {
                try
                {
                    model.Diff = MapperFactory.NETDateViewModel_UINETDate.CreateMapper().Map<UINETDate>(model.DateOne).GetDiff(MapperFactory.NETDateViewModel_UINETDate.CreateMapper().Map<UINETDate>(model.DateTwo).Data);
                    model.Msg = $"<p class=\"bg-success\">The difference between two days is: <strong> {model.Diff} </strong></p>";
                }
                catch(Exception ex)
                {
                    model.Msg = $"<p class=\"bg-danger\">Unexpected failure: <strong> {ex.Message} </strong></p>";
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Api()
        {
            ViewData["Message"] = "Your RESTful APIs page.";

            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            ViewData["Message"] = "Your error page.";

            return View();
        }
    }
}
