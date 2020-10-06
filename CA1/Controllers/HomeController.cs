using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using CA1.Database;

namespace CA1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbGallery db;
        public HomeController(DbGallery db)
        {
            this.db = db;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string[] imgs = {
                "https%3A%2F%2Fss2.bdstatic.com%2F70cFvnSh_Q1YnxGkpoWK1HF6hhy%2Fit%2Fu%3D3733467419%2C221273421%26fm%3D15%26gp%3D0.jpg?w=350&s=392928858ea8428d43dee478d5ddf4b9",
                "https%3A%2F%2Fss3.bdstatic.com%2F70cFv8Sh_Q1YnxGkpoWK1HF6hhy%2Fit%2Fu%3D2274242543%2C1146141556%26fm%3D15%26gp%3D0.jpg?w=350&s=01b916e68045e6cd48bf31250fc28b05",
                "https%3A%2F%2Fss1.bdstatic.com%2F70cFuXSh_Q1YnxGkpoWK1HF6hhy%2Fit%2Fu%3D1439855652%2C821863955%26fm%3D26%26gp%3D0.jpg?w=350&s=e6c9cf39751387e3870f2c1c938e6604",
                "https%3A%2F%2Fwww.ubergrad.com%2Fblog%2Fwp-content%2Fuploads%2F2020%2F07%2Fdata-analytics_ug.jpg?w=350&s=ef245424f04be2411e8c6ef206d67ee2",
                "https%3A%2F%2Fstatic.gunnarpeipman.com%2Fwp-content%2Fuploads%2F2017%2F03%2Faspnet-core-syslog.png?w=350&s=3f28e58544df454a82b681d34533c7e0",
                "https%3A%2F%2Fcynapse.com%2Fimg%2FNumerics-Home-Banner.png?w=350&s=c8fb7768c86c3fd11ae59420f71589cb",
            };

            string[] name =
            {
                ".NET Charts",
                ".NET Paypal",
                ".NET ML",
                ".NET Analytics",
                ".NET Logger",
                ".NET Numerics",
            };
            string[] information =
            {
                "Brings powerful charting capabilities to your .NET applications.",
                "Integrate your .NET apps with paypal the easy way!",
                "Supercharged .NET machine learning libraries.",
                "Performs data mining and analytics easily in .NET.",
                "Logs and aggregates events easily in your .NET apps.",
                "Powerful numerical methods for your .NET simulations.",
            };
            string[] Price =
           {
                "$99",
                "$69",
                "$299",
                "$299",
                "$49",
                "$199",
            };

            ViewData["images"] = imgs;
            ViewData["names"] = name;
            ViewData["informations"] = information;
            ViewData["prices"] = Price;
            ViewData["url_prefix"] = "https://nus-sa51-team9.imgix.net/";
            ViewData["after_price"] = "Add To Cart";
            ViewData["Is_Home"] = "menu_hilite";

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

            return View("SearchResults");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
