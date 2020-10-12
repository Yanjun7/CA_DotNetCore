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

            if (cart.Count == 0)
            {
                ViewData["count"] = false;
            }
            else
            {
                ViewData["count"] = true;
            }

            int cartCount = cart.Count;

            string[] productId = new string[cartCount];
            string[] images = new string[cartCount];
            string[] names = new string[cartCount];
            string[] info = new string[cartCount];
            double[] prices = new double[cartCount];
            int[] quantity = new int[cartCount];
            double total = 0;

            for(int i = 0; i <cartCount; i++)
            {
                productId[i] = cart[i].ProductId;
                images[i] = cart[i].Product.PhotoLink;
                names[i] = cart[i].Product.ProductName;
                info[i] = cart[i].Product.Description;
                prices[i] = cart[i].Product.Price;
                quantity[i] = cart[i].Quantity;
                total += (cart[i].Product.Price * cart[i].Quantity);
            }

            ViewData["productId"] = productId;
            ViewData["images"] = images;
            ViewData["names"] = names;
            ViewData["informations"] = info;
            ViewData["prices"] = prices;
            ViewData["quantity"] = quantity;
            ViewData["total"] = total;
            ViewData["sessionId"] = sessionId;
            ViewData["username"] = currentSession.User.Username.ToUpper();

            return View();
        }

        [HttpPost]
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

        [HttpPost]
        public IActionResult Minus([FromBody] ProductObj productId)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            string userId = session.UserId;
            ShoppingCartDetail shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId.ProductId);

            if (shoppingCartDetail.Quantity == 0)
                shoppingCartDetail.Quantity = 0;
            else
                shoppingCartDetail.Quantity -= 1;

            db.SaveChanges();

            return Json(new
            {
                status = "success",
                quantity = shoppingCartDetail.Quantity
            });
        }

        [HttpPost]
        public IActionResult Plus([FromBody] ProductObj productId)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            string userId = session.UserId;
            ShoppingCartDetail shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId.ProductId);

            if (shoppingCartDetail.Quantity == 20)
                shoppingCartDetail.Quantity = 20; //setting a limit of maximum 20 in quantity per product, per transaction
            else
                shoppingCartDetail.Quantity += 1;

            db.SaveChanges();

            return Json(new
            {
                status = "success",
                quantity = shoppingCartDetail.Quantity
            });
        }

        public IActionResult Total()
        {
            string SessionId = HttpContext.Request.Cookies["sessionId"];
            Session currentSession = db.Sessions.FirstOrDefault(x => x.Id.ToString() == SessionId);

            List<ShoppingCartDetail> cart = db.ShoppingCart.Where(x => x.UserId == currentSession.UserId).ToList();

            int cartCount = cart.Count;
            double Total = 0;

            for (int i = 0; i < cartCount; i++)
            {
                Total += cart[i].Product.Price * cart[i].Quantity;
            }

            return Json(new { 
                status = "success",
                total = Total
            });
        }


      
        public IActionResult CartIcon()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];

            if (sessionId != null)
            {
                Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
                List<ShoppingCartDetail> cartItem = db.ShoppingCart.Where(x => x.UserId == session.UserId).ToList();

                int cartItemCount = 0;

                foreach (ShoppingCartDetail i in cartItem)
                {
                    cartItemCount += i.Quantity;
                }

                if (cartItemCount == 0)
                {
                    return Json(new
                    {
                        Count = 0
                    });
                }
                else
                {
                    return Json(new
                    {
                        Count = cartItemCount
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Count = ""
                });
            }
        }
    }
}
