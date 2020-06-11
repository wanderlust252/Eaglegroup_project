using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Eaglegroup_project.Models;
using Eaglegroup_project.Extensions;

namespace Eaglegroup_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var email = User.GetSpecificClaim("FullName");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
