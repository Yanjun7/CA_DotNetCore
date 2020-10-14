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
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);

            if (session == null)
                ViewData["IsLogin"] = false;
            else if (session.User == null)
                ViewData["IsLogin"] = false;
            else
                ViewData["IsLogin"] = true;

            ViewData["title"] = "Register";
            ViewData["Is_Register"] = "menu_hilite";
            return View();
        }

        [HttpPost]
        public IActionResult SaveToDatabase(string newUsername, string newPassword1)
        {
            List<User> CurrentUsers = db.Users.ToList();
            bool IsDuplicateUsername = false;

            for (int i = 0; i < CurrentUsers.Count; i++)
            {
                if (newUsername == CurrentUsers[i].Username)
                {
                    IsDuplicateUsername = true;
                    break;
                }
            }

            if (IsDuplicateUsername == true)
            {
                ViewData["IsLogin"] = false;
                ViewData["register"] = "unsuccessful";
                ViewData["username_input"] = newUsername;
                return View("Index");
            }
            else
            {
                int suffix = db.Users.Count() + 1;
                User newUser = new User()
                {
                    Id = "user_" + (1000 + suffix),
                    Username = newUsername,
                    Password = LoginController.GetStringSha256Hash(newPassword1)
                };
                db.Users.Add(newUser);
                //System.Diagnostics.Debug.WriteLine("!!!!!");
                db.SaveChanges();
                ViewData["register"] = "successful";
                ViewData["IsLogin"] = false;
                return RedirectToAction("RegisterSuccessful", "Login");
            }
            
        }

/*        public IActionResult CheckUserName([FromBody] Usernameobj newUsername)
        {
            foreach (User user in db.Users)
            {
                if (user.Username == newUsername.Username)
                {
                    return Json(new
                    {
                        status = "DuplicatedUserName",
                    });
                }
            }
            return Json(new
            {
                status = "success",
            });
        }*/
    }
}