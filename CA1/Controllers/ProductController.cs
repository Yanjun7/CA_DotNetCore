using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbGallery db;
        public ProductController(DbGallery db) 
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search(string userInput)
        {
            List<Product> products = db.Products.Where(
                    x => x.Description.Contains(userInput) &&
                    x.ProductName.Contains(userInput)).ToList();
            ViewData["products"] = products;

            Session session = db.Sessions.FirstOrDefault(x =>
                x.UserId == HttpContext.Request.Cookies["sessionId"]);
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View("SearchDetails");

        }

        
    }
}
