/***
 * Filename: HomeController.cs
 * Author : ebendutoit
 * Class   : HomeController
 *          Entrypoint for website if navigated to server
 ***/
using System.Diagnostics;
using Mapper_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mapper_Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                    RequestId =
                            Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}