using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CA1.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DbGallery db;

        public ShoppingCartController(DbGallery db)
        {
            this.db = db;
        }

        public IActionResult Index(Guid sessionId)
        {
            Session currentSession = db.Sessions.FirstOrDefault(x => x.Id == sessionId);

            if (currentSession == null)
                return Redirect("/Login/Index");

            List<ShoppingCartDetail> cart = db.ShoppingCart.Where(x => x.UserId == currentSession.UserId).ToList();
            if (cart == null)
                return View("Empty.cshtml");

            int cartCount = cart.Count;

            string[] images = new string[cartCount];
            string[] names = new string[cartCount];
            string[] info = new string[cartCount];
            double[] prices = new double[cartCount];
            int[] quantity = new int[cartCount];

            for(int i = 0; i <cartCount; i++)
            {
                images[i] = cart[i].Product.PhotoLink;
                names[i] = cart[i].Product.ProductName;
                info[i] = cart[i].Product.Description;
                prices[i] = cart[i].Product.Price;
                quantity[i] = cart[i].Quantity;
            }

            ViewData["images"] = images;
            ViewData["names"] = names;
            ViewData["informations"] = info;
            ViewData["prices"] = prices;
            ViewData["quantity"] = quantity;
            ViewData["sessionId"] = HttpContext.Request.Cookies["sessionId"];
            return View();
        }

        public IActionResult Empty()
        {
            return View();
        }

        public IActionResult Add([FromBody] ProductObj productObj)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId == null)
            {
                return Json(new
                {
                    status = "redirect",
                    url = "/Login/Index"
                });
                //return Redirect("/Login/Index");
                //return RedirectToAction("Authenticate", "Login");
            }
            else
            {
                Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
                string userId = session.UserId;
                string productId = productObj.ProductId;

                ShoppingCartDetail cartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);

                if (cartDetail == null)
                {
                    ShoppingCartDetail cart = new ShoppingCartDetail()
                    {
                        UserId = session.UserId,
                        ProductId = productObj.ProductId,
                        Quantity = 1
                    };

                    db.ShoppingCart.Add(cart);
                }
                else
                {
                    cartDetail.Quantity++;
                }

                db.SaveChanges();

                return Json(new
                {
                    status = "success",
                    url = "/ShoppingCart/Index?sessionId=" + sessionId
                });
            }
        }
    }
}
