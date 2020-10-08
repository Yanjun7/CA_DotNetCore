using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CA1.Database;
using CA1.Models;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace CA1.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbGallery db;

        public LoginController(DbGallery db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            ViewData["Is_Login"] = "menu_hilite";

            return View();
        }

        public IActionResult Authenticate(string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
            ViewData["userName"] = username;
            if (user == null)
            {
                ViewData["Is_Login"] = "menu_hilite";
                ViewData["errMsg"] = "No such user or incorrect password.";
                return View("Index");
            }
            else
            {
                Session session = new Session()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                Response.Cookies.Append("sessionId", session.Id.ToString());
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
