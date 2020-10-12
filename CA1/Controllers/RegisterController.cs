using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CA1.Database;
using Microsoft.Extensions.Configuration;
using CA1.Models;

namespace CA1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DbGallery db;

        public RegisterController(DbGallery db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewData["title"] = "Register";
            ViewData["Is_Register"] = "menu_hilite";
            return View();
        }

        [HttpPost]
        public IActionResult SaveToDatabase(string newUsername,string newPassword1)
        {
            foreach(User user in db.Users)
            {
                if (user.Username == newUsername)
                {
                    return Json(new
                    {
                        status = "DuplicatedUserName",
                    });
                }
            }
            int suffix = db.Users.Count()+1;
            User newUser = new User()
            {
                Id = "User" + suffix,
                Username = newUsername,
                Password = LoginController.GetStringSha256Hash(newPassword1)
            };
            db.Users.Add(newUser);
            db.SaveChanges();
            return RedirectToAction("Index", "Login");
        }
    }
}
