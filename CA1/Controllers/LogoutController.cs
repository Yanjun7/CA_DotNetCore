using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class LogoutController : Controller
    {
        private readonly DbGallery db;

        public LogoutController(DbGallery db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            db.Sessions.Remove(new Session() { Id = Guid.Parse(sessionId) });
            db.SaveChanges();
            HttpContext.Response.Cookies.Delete("sessionId");
            return RedirectToAction("Index", "Home");
            
        }
    }
}
