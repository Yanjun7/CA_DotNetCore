using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using CA1.Database;
using Microsoft.VisualBasic;

namespace CA1.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbGallery db;

        public HomeController(DbGallery db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            List<Product> products = db.Products.ToList();
            string[] images = new string[products.Count];
            string[] names = new string[products.Count];
            string[] tags = new string[products.Count];
            string[] info = new string[products.Count];
            double[] prices = new double[products.Count];
            for(int i = 0; i < products.Count; i++)
            {
                images[i] = products[i].PhotoLink;
                names[i] = products[i].ProductName;
                tags[i] = products[i].PhotoTag;
                info[i] = products[i].Description;
                prices[i] = products[i].Price;
            }

            ViewData["images"] = images;
            ViewData["names"] = names;
            ViewData["tags"] = tags;
            ViewData["informations"] = info;
            ViewData["prices"] = prices;
            ViewData[""]
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
