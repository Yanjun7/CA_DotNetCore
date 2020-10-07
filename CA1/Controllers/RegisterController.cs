using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            ViewData["title"] = "Register";
            ViewData["Is_Register"] = "menu_hilite";
            return View();
        }
    }
}
