using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CA1.Database;
using CA1.Models;
using System.Security.Cryptography.X509Certificates;

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
            ViewData["IsLogin"] = false;
            return View();
        }

        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public IActionResult Authenticate(string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == GetStringSha256Hash(password));
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);

            if (user == null)
            {
                ViewData["Is_Login"] = "menu_hilite"; 
                ViewData["errMsg"] = "No such user or incorrect password.";
                ViewData["IsLogin"] = false;
                return View("Index");
            }
            else if (session != null)
            {
                if (session.User == null)
                {
                    session.UserId = user.Id;
                    session.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                    session.IsLogin = true;
                    db.SaveChanges();
                    return Redirect("/ShoppingCart/MergeCart");
                }
            }
            else
            {
                session = new Session()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    IsLogin = true,
                    IsReadyToCheckOut = false
                };

                db.Sessions.Add(session);
                db.SaveChanges();

                Response.Cookies.Append("sessionId", session.Id.ToString());
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegisterSuccessful()
        {
            ViewData["Is_Login"] = "menu_hilite";
            ViewData["IsLogin"] = false;
            ViewData["register"] = "successful";
            return View("Index");
        }

        public IActionResult CheckOutUnsuccessful()
        {
            ViewData["Is_Login"] = "menu_hilite";
            ViewData["IsLogin"] = false;
            ViewData["checkout"] = "unsuccessful";
            return View("Index");
        }
    }
}
